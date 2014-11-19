using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

using Microsoft.Kinect;

using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.RapidDomain;

namespace ABB_Kinect
{
	/// <summary>
	/// Interaction logic for ABBConnectionWindow.xaml
	/// </summary>
	public partial class ABBConnectionWindow : Window
	{
		public class MyKinect
		{
			private const float RenderWidth = 316.0f;
			private const float RenderHeight = 237.0f;
			private const double JointThickness = 3;
			private const double BodyCenterThickness = 10;
			private const double ClipBoundsThickness = 10;

			// Brush used to draw skeleton center point
			private readonly Brush CenterPointBrush = Brushes.Blue;
			// Brush used for drawing joints that are currently tracked
			private readonly Brush TrackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));
			// Brush used for drawing joints that are currently inferred
			private readonly Brush InferredJointBrush = Brushes.Yellow;
			// Pen used for drawing bones that are currently tracked
			private readonly Pen TrackedBonePen = new Pen(Brushes.Green, 6);
			// Pen used for drawing bones that are currently inferred
			private readonly Pen InferredBonePen = new Pen(Brushes.Gray, 1);
			// Active Kinect sensor
			private KinectSensor Sensor;
			// Drawing group for skeleton rendering output
			private DrawingGroup DrawingGroup;
			// Drawing image that we will display
			private DrawingImage ImageSource;
			// Window to display image
			private Image ImageDisplay;

			private Button TryAgainButton;
			private TextBlock KinectNotFoundTextBlock;

			public MyKinect(Image ImageDisplay, Button TryAgainButton, TextBlock KinectNotFoundTextBlock)
			{
				// Complete member initialization
				this.ImageDisplay = ImageDisplay;
				this.TryAgainButton = TryAgainButton;
				this.KinectNotFoundTextBlock = KinectNotFoundTextBlock;

				if(!FindKinect())
				{
					SetKinectFoundControls(false);
				}
				else
				{
					SetKinectFoundControls(true);

					this.DrawingGroup = new DrawingGroup();
					this.ImageSource = new DrawingImage(this.DrawingGroup);
					ImageDisplay.Source = this.ImageSource;

					if (this.Sensor != null) // Unnecessary, but better do
					{
						this.Sensor.SkeletonStream.Enable();
						this.Sensor.SkeletonFrameReady += this.SensorSkeletonFrameReady;

						// Start the sensor!
						try
						{
							this.Sensor.Start();
						}
						catch (IOException)
						{
							this.Sensor = null;
							SetKinectFoundControls(false);
						}
					}
				}
			}
			~MyKinect()
			{
				if (this.Sensor != null)
					this.Sensor.Stop();
			}

			private bool FindKinect()
			{
				/*
				 * Look trough all sensors and start the first connected one.
				 */
				foreach (var potentialSensor in KinectSensor.KinectSensors)
				{
					if (potentialSensor.Status == KinectStatus.Connected)
					{
						this.Sensor = potentialSensor;
						return true;
					}
				}
				return false;
			}
			private void SetKinectFoundControls(bool b)
			{
				if (b) // Kinect was found
				{
					TryAgainButton.Visibility = Visibility.Hidden;
					TryAgainButton.IsEnabled = false;

					KinectNotFoundTextBlock.Visibility = Visibility.Hidden;
				}
				else
				{
					TryAgainButton.Visibility = Visibility.Visible;
					TryAgainButton.IsEnabled = true;

					KinectNotFoundTextBlock.Visibility = Visibility.Visible;
				}
			}

			// DRAWING Functions
			private static void RenderClippedEdges(Skeleton skeleton, DrawingContext drawingContext)
			{
				if (skeleton.ClippedEdges.HasFlag(FrameEdges.Bottom))
				{
					drawingContext.DrawRectangle(
						Brushes.Red,
						null,
						new Rect(0, RenderHeight - ClipBoundsThickness, RenderWidth, ClipBoundsThickness));
				}

				if (skeleton.ClippedEdges.HasFlag(FrameEdges.Top))
				{
					drawingContext.DrawRectangle(
						Brushes.Red,
						null,
						new Rect(0, 0, RenderWidth, ClipBoundsThickness));
				}

				if (skeleton.ClippedEdges.HasFlag(FrameEdges.Left))
				{
					drawingContext.DrawRectangle(
						Brushes.Red,
						null,
						new Rect(0, 0, ClipBoundsThickness, RenderHeight));
				}

				if (skeleton.ClippedEdges.HasFlag(FrameEdges.Right))
				{
					drawingContext.DrawRectangle(
						Brushes.Red,
						null,
						new Rect(RenderWidth - ClipBoundsThickness, 0, ClipBoundsThickness, RenderHeight));
				}
			}
			private void DrawBonesAndJoints(Skeleton skeleton, DrawingContext drawingContext)
			{
				// Render Torso
				this.DrawBone(skeleton, drawingContext, JointType.Head, JointType.ShoulderCenter);
				this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderLeft);
				this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderRight);
				this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.Spine);
				this.DrawBone(skeleton, drawingContext, JointType.Spine, JointType.HipCenter);
				this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipLeft);
				this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipRight);

				// Left Arm
				this.DrawBone(skeleton, drawingContext, JointType.ShoulderLeft, JointType.ElbowLeft);
				this.DrawBone(skeleton, drawingContext, JointType.ElbowLeft, JointType.WristLeft);
				this.DrawBone(skeleton, drawingContext, JointType.WristLeft, JointType.HandLeft);

				// Right Arm
				this.DrawBone(skeleton, drawingContext, JointType.ShoulderRight, JointType.ElbowRight);
				this.DrawBone(skeleton, drawingContext, JointType.ElbowRight, JointType.WristRight);
				this.DrawBone(skeleton, drawingContext, JointType.WristRight, JointType.HandRight);

				// Left Leg
				this.DrawBone(skeleton, drawingContext, JointType.HipLeft, JointType.KneeLeft);
				this.DrawBone(skeleton, drawingContext, JointType.KneeLeft, JointType.AnkleLeft);
				this.DrawBone(skeleton, drawingContext, JointType.AnkleLeft, JointType.FootLeft);

				// Right Leg
				this.DrawBone(skeleton, drawingContext, JointType.HipRight, JointType.KneeRight);
				this.DrawBone(skeleton, drawingContext, JointType.KneeRight, JointType.AnkleRight);
				this.DrawBone(skeleton, drawingContext, JointType.AnkleRight, JointType.FootRight);

				// Render Joints
				foreach (Joint joint in skeleton.Joints)
				{
					Brush drawBrush = null;

					if (joint.TrackingState == JointTrackingState.Tracked)
					{
						drawBrush = this.TrackedJointBrush;
					}
					else if (joint.TrackingState == JointTrackingState.Inferred)
					{
						drawBrush = this.InferredJointBrush;
					}

					if (drawBrush != null)
					{
						drawingContext.DrawEllipse(drawBrush, null, this.SkeletonPointToScreen(joint.Position), JointThickness, JointThickness);
					}
				}
			}
			private void DrawBone(Skeleton skeleton, DrawingContext drawingContext, JointType jointType0, JointType jointType1)
			{
				Joint joint0 = skeleton.Joints[jointType0];
				Joint joint1 = skeleton.Joints[jointType1];

				// If we can't find either of these joints, exit
				if (joint0.TrackingState == JointTrackingState.NotTracked ||
					joint1.TrackingState == JointTrackingState.NotTracked)
				{
					return;
				}

				// Don't draw if both points are inferred
				if (joint0.TrackingState == JointTrackingState.Inferred &&
					joint1.TrackingState == JointTrackingState.Inferred)
				{
					return;
				}

				// We assume all drawn bones are inferred unless BOTH joints are tracked
				Pen drawPen = this.InferredBonePen;
				if (joint0.TrackingState == JointTrackingState.Tracked && joint1.TrackingState == JointTrackingState.Tracked)
				{
					drawPen = this.TrackedBonePen;
				}

				drawingContext.DrawLine(drawPen, this.SkeletonPointToScreen(joint0.Position), this.SkeletonPointToScreen(joint1.Position));
			}
			private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
			{
				// Convert point to depth space.  
				// We are not using depth directly, but we do want the points in our 640x480 output resolution.
				DepthImagePoint depthPoint = this.Sensor.MapSkeletonPointToDepth(
																				 skelpoint,
																				 DepthImageFormat.Resolution640x480Fps30);
				return new Point(depthPoint.X, depthPoint.Y);
			}
			
			// EVENTS
			private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
			{
				Skeleton[] skeletons = new Skeleton[0];

				using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
				{
					if (skeletonFrame != null)
					{
						skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
						skeletonFrame.CopySkeletonDataTo(skeletons);
					}
				}

				using (DrawingContext dc = this.DrawingGroup.Open())
				{
					// Draw a transparent background to set the render size
					dc.DrawRectangle(Brushes.Black, null, new Rect(0.0, 0.0, RenderWidth, RenderHeight));

					if (skeletons.Length != 0)
					{
						foreach (Skeleton skel in skeletons)
						{
							RenderClippedEdges(skel, dc);

							if (skel.TrackingState == SkeletonTrackingState.Tracked)
							{
								this.DrawBonesAndJoints(skel, dc);
							}
							else if (skel.TrackingState == SkeletonTrackingState.PositionOnly)
							{
								dc.DrawEllipse(
									this.CenterPointBrush,
									null,
									this.SkeletonPointToScreen(skel.Position),
									BodyCenterThickness,
									BodyCenterThickness);
							}
						}
					}

					this.DrawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, RenderWidth, RenderHeight));
				}
			}
		}
		// Experimental constants to avoid crashes
		private const int JOINT1MAX = 20;
		private const int JOINT1MIN = -20;
		private const int JOINT2MAX = 10;
		private const int JOINT2MIN = -10;
		private const int JOINT3MAX = 20;
		private const int JOINT3MIN = -20;
		private const int JOINT4MAX = 180;
		private const int JOINT4MIN = -180;
		private const int JOINT5MAX = 90;
		private const int JOINT5MIN = -90;
		private const int JOINT6MAX = 80;
		private const int JOINT6MIN = -80;

		private Controller ABBController = null;
		private RapidData FlagExec = null;
		private Task[] tasks = null;
		private Timer SupervisorTimer = null;

		private MyKinect KinectDevice = null;

		public ABBConnectionWindow(Controller controller)
		{
			InitializeComponent();
			ABBController = controller;
			this.Title = "ABB " + ABBController.IPAddress.ToString();
			ABBWindowTitleLabel.Text = "Connected to " + ABBController.IPAddress.ToString();
			ABBWindowNameLabel.Text = ABBController.Name.ToString();

			InitializeManualMode();
			InitializeTimer();
			this.Show();
		}

		private void InitializeManualMode()
		{
			Joint1Slider.Minimum = JOINT1MIN;
			Joint1Slider.Maximum = JOINT1MAX;
			Joint1Slider.TickFrequency = 1;
			Joint1Slider.IsSnapToTickEnabled = true;
			Joint1Slider.TickFrequency = 1;
			Joint1Slider.LargeChange = 10;

			Joint2Slider.Minimum = JOINT2MIN;
			Joint2Slider.Maximum = JOINT2MAX;
			Joint2Slider.TickFrequency = 1;
			Joint2Slider.IsSnapToTickEnabled = true;
			Joint2Slider.TickFrequency = 1;
			Joint2Slider.LargeChange = 10;

			Joint3Slider.Minimum = JOINT3MIN;
			Joint3Slider.Maximum = JOINT3MAX;
			Joint3Slider.TickFrequency = 1;
			Joint3Slider.IsSnapToTickEnabled = true;
			Joint3Slider.TickFrequency = 1;
			Joint3Slider.LargeChange = 10;

			Joint4Slider.Minimum = JOINT4MIN;
			Joint4Slider.Maximum = JOINT4MAX;
			Joint4Slider.TickFrequency = 1;
			Joint4Slider.IsSnapToTickEnabled = true;
			Joint4Slider.TickFrequency = 1;
			Joint4Slider.LargeChange = 10;

			Joint5Slider.Minimum = JOINT5MIN;
			Joint5Slider.Maximum = JOINT5MAX;
			Joint5Slider.TickFrequency = 1;
			Joint5Slider.IsSnapToTickEnabled = true;
			Joint5Slider.TickFrequency = 1;
			Joint5Slider.LargeChange = 10;

			Joint6Slider.Minimum = JOINT6MIN;
			Joint6Slider.Maximum = JOINT6MAX;
			Joint6Slider.TickFrequency = 1;
			Joint6Slider.IsSnapToTickEnabled = true;
			Joint6Slider.TickFrequency = 1;
			Joint6Slider.LargeChange = 10;

			Joint1TextBlock.Text = ((int)Joint1Slider.Value).ToString();
			Joint2TextBlock.Text = ((int)Joint2Slider.Value).ToString();
			Joint3TextBlock.Text = ((int)Joint3Slider.Value).ToString();
			Joint4TextBlock.Text = ((int)Joint4Slider.Value).ToString();
			Joint5TextBlock.Text = ((int)Joint5Slider.Value).ToString();
			Joint6TextBlock.Text = ((int)Joint6Slider.Value).ToString();

			this.Dispatcher.Invoke
			(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
			{
				GetCurrentJointsAngles();
				UpdateTextBlockValues();
				TurnONJointsEvents();
			}));

			SpeedTxtControl.TextChanged += SpeedTxtControl_TextChanged;
			ABBController.MotionSystem.SpeedRatio = int.Parse(SpeedTxtControl.Text);
		}

		private void InitializeTimer()
		{
			SupervisorTimer = new Timer(100); // check flags every 100 ms
			SupervisorTimer.AutoReset = true;
			SupervisorTimer.Elapsed += SupervisorTimerElapsed;
			SupervisorTimer.Enabled = true;
		}

		private void GetCurrentJointsAngles()
		{
			try
			{
				if (ABBController.OperatingMode == ControllerOperatingMode.Auto)
				{
					tasks = ABBController.Rapid.GetTasks();
					RapidData rd = tasks[0].GetRapidData("MainModule", "current_joints_angles");
					JointTarget angles = new JointTarget();
					if (rd.Value is JointTarget)
					{
						angles = (JointTarget)rd.Value;
						SetSliders(angles);
					}
					else
						throw new InvalidProgramException();
				}
				else
					MessageBox.Show("Automatic mode is required to start execution from a remote client.");	
			}
			catch (System.InvalidOperationException ex)
			{
				MessageBox.Show("Mastership is held by another client." + ex.Message);
			}
			catch (System.Exception ex)
			{
				MessageBox.Show("Unexpected error occured: " + ex.Message);
			}
		}

		private void SetNewJointsAngles()
		{
			try
			{
				if (ABBController.OperatingMode == ControllerOperatingMode.Auto)
				{
					tasks = ABBController.Rapid.GetTasks();
					RapidData rd = tasks[0].GetRapidData("MainModule", "destination_joints_angles");
					JointTarget angles = new JointTarget();
					if (rd.Value is JointTarget)
					{
						angles = (JointTarget)rd.Value;
						angles = GetAnglesFromSliders(angles);

						FlagExec = tasks[0].GetRapidData("MainModule", "flag_exec");
						ABB.Robotics.Controllers.RapidDomain.Bool rapidBool = new ABB.Robotics.Controllers.RapidDomain.Bool();
						rapidBool.Value = true;
						
						using (Mastership m = Mastership.Request(ABBController.Rapid))
						{
							rd.Value = angles;
							FlagExec.Value = rapidBool;
						}
					}
					else
						throw new InvalidProgramException();
				}
				else
					MessageBox.Show("Automatic mode is required to start execution from a remote client.");
			}
			catch (System.InvalidProgramException ex)
			{
				MessageBox.Show("Wrong data type" + ex.Message);
			}
			catch (System.InvalidOperationException ex)
			{
				MessageBox.Show("Mastership is held by another client." + ex.Message);
			}
			catch (System.Exception ex)
			{
				MessageBox.Show("Unexpected error occured: " + ex.Message);
			}
		}

		private void TurnOFFJointsEvents()
		{
			this.Dispatcher.Invoke
			(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
			{
				Joint1Slider.ValueChanged -= Joint1Slider_ValueChanged;
				Joint2Slider.ValueChanged -= Joint2Slider_ValueChanged;
				Joint3Slider.ValueChanged -= Joint3Slider_ValueChanged;
				Joint4Slider.ValueChanged -= Joint4Slider_ValueChanged;
				Joint5Slider.ValueChanged -= Joint5Slider_ValueChanged;
				Joint6Slider.ValueChanged -= Joint6Slider_ValueChanged;
			}));
		}

		private void TurnONJointsEvents()
		{
			this.Dispatcher.Invoke
			(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
			{
				Joint1Slider.ValueChanged += Joint1Slider_ValueChanged;
				Joint2Slider.ValueChanged += Joint2Slider_ValueChanged;
				Joint3Slider.ValueChanged += Joint3Slider_ValueChanged;
				Joint4Slider.ValueChanged += Joint4Slider_ValueChanged;
				Joint5Slider.ValueChanged += Joint5Slider_ValueChanged;
				Joint6Slider.ValueChanged += Joint6Slider_ValueChanged;
			}));
		}

		private JointTarget GetAnglesFromSliders(JointTarget a)
		{
			JointTarget angles = a;
			angles.RobAx.Rax_1 = (float)Joint1Slider.Value;
			angles.RobAx.Rax_2 = (float)Joint2Slider.Value;
			angles.RobAx.Rax_3 = (float)Joint3Slider.Value;
			angles.RobAx.Rax_4 = (float)Joint4Slider.Value;
			angles.RobAx.Rax_5 = (float)Joint5Slider.Value;
			angles.RobAx.Rax_6 = (float)Joint6Slider.Value;
			return angles;
		}
		
		private JointTarget GetAnglesFromArgument(JointTarget a, int A1, int A2, int A3, int A4, int A5, int A6)
		{
			JointTarget angles = a;
			angles.RobAx.Rax_1 = A1;
			angles.RobAx.Rax_2 = A2;
			angles.RobAx.Rax_3 = A3;
			angles.RobAx.Rax_4 = A4;
			angles.RobAx.Rax_5 = A5;
			angles.RobAx.Rax_6 = A6;
			return angles;
		}

		private void SetSliders(ABB.Robotics.Controllers.RapidDomain.JointTarget angles)
		{
			Joint1Slider.Value = angles.RobAx.Rax_1;
			Joint2Slider.Value = angles.RobAx.Rax_2;
			Joint3Slider.Value = angles.RobAx.Rax_3;
			Joint4Slider.Value = angles.RobAx.Rax_4;
			Joint5Slider.Value = angles.RobAx.Rax_5;
			Joint6Slider.Value = angles.RobAx.Rax_6;
		}

		private void AnySliderValueChanged(Slider slider, TextBlock textBlock)
		{
			this.Dispatcher.Invoke
			(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
			{
				textBlock.Text = ((int)slider.Value).ToString();
				SetNewJointsAngles();
			}));
		}

		private void UpdateTextBlockValues()
		{
			AnySliderValueChanged(Joint1Slider, Joint1TextBlock);
			AnySliderValueChanged(Joint2Slider, Joint2TextBlock);
			AnySliderValueChanged(Joint3Slider, Joint3TextBlock);
			AnySliderValueChanged(Joint4Slider, Joint4TextBlock);
			AnySliderValueChanged(Joint5Slider, Joint5TextBlock);
			AnySliderValueChanged(Joint6Slider, Joint6TextBlock);
		}

		private void ReadyState() // Green light, enabled controls...
		{
			ReadyLED.Background = Brushes.LimeGreen;
			TabControl.IsEnabled = true;
			ResetPositionButton.IsEnabled = true;
		}

		private void BusyState() // Yellow light, disabled controls...
		{
			ReadyLED.Background = Brushes.Yellow;
			if (DynamicModeCheckBox.IsChecked == false)
				TabControl.IsEnabled = false;
			else
				TabControl.IsEnabled = true;
			ResetPositionButton.IsEnabled = false;
		}

		private void ErrorState() // Red light...
		{
			ReadyLED.Background = Brushes.Red;
		}

		// EVENTS
		private void SupervisorTimerElapsed(Object sender, ElapsedEventArgs e)
		{
			try
			{
				if (ABBController.OperatingMode == ControllerOperatingMode.Auto)
				{
					this.Dispatcher.Invoke
					(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
					{
						try
						{
							tasks = ABBController.Rapid.GetTasks();
							if (tasks[0].ExecutionStatus.ToString() == "Running")
							{
								FlagExec = tasks[0].GetRapidData("MainModule", "flag_exec");

								ReadyLED.Dispatcher.Invoke
								(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
								{
									if ((ABB.Robotics.Controllers.RapidDomain.Bool)FlagExec.Value == true)
									{
										BusyState();
									}
									else // false
									{
										ReadyState();
									}
								}));
							}
							else
							{
								SupervisorTimer.Enabled = false;
								RapidData FlagOutOfRange = tasks[0].GetRapidData("MainModule", "flag_out_of_range");
								if ((ABB.Robotics.Controllers.RapidDomain.Bool)FlagOutOfRange.Value == true)
									MessageBox.Show("Program stopped because of dangerous joint angles\nPlease back manually to safe position",
										"ABB Out of range", MessageBoxButton.OK, MessageBoxImage.Warning);
								else
									MessageBox.Show("Run program on ABB first!", "ABB Error", MessageBoxButton.OK, MessageBoxImage.Error);
								this.Close();
							}
						}
						catch
						{
							SupervisorTimer = null;
						}
					}));
				}
				else
				{
					MessageBox.Show("Automatic mode is required to start execution from a remote client.");
					this.Close();
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show("Unexpected error occured: " + ex.Message);
			}
		}

		private void ResetPositionButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (ABBController.OperatingMode == ControllerOperatingMode.Auto)
				{
					tasks = ABBController.Rapid.GetTasks();
					using (Mastership m = Mastership.Request(ABBController.Rapid))
					{
						RapidData rd = tasks[0].GetRapidData("MainModule", "destination_joints_angles");
						JointTarget angles = new JointTarget();
						angles = (JointTarget)rd.Value;
						angles = GetAnglesFromArgument(angles, 0, 0, 0, -90, 0, 0);
						rd.Value = angles;

						FlagExec = tasks[0].GetRapidData("MainModule", "flag_exec");
						ABB.Robotics.Controllers.RapidDomain.Bool rapidBool = new ABB.Robotics.Controllers.RapidDomain.Bool();
						rapidBool.Value = true;
						FlagExec.Value = rapidBool;
					}
					this.Dispatcher.Invoke
					(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
					{
						TurnOFFJointsEvents();
						Joint1Slider.Value = 0;
						Joint2Slider.Value = 0;
						Joint3Slider.Value = 0;
						Joint4Slider.Value = -90;
						Joint5Slider.Value = 0;
						Joint6Slider.Value = 0;
						TurnONJointsEvents();
						UpdateTextBlockValues();
					}));
				}
				else
					MessageBox.Show("Automatic mode is required to start execution from a remote client.");
			}
			catch (System.InvalidOperationException ex)
			{
				MessageBox.Show("Mastership is held by another client." + ex.Message);
			}
			catch (System.Exception ex)
			{
				MessageBox.Show("Unexpected error occured: " + ex.Message);
			}
		}

		private void Joint1Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			AnySliderValueChanged(sender as Slider, Joint1TextBlock);
		}

		private void Joint2Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			AnySliderValueChanged(sender as Slider, Joint2TextBlock);
		}

		private void Joint3Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			AnySliderValueChanged(sender as Slider, Joint3TextBlock);
		}

		private void Joint4Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			AnySliderValueChanged(sender as Slider, Joint4TextBlock);
		}

		private void Joint5Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			AnySliderValueChanged(sender as Slider, Joint5TextBlock);
		}

		private void Joint6Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			AnySliderValueChanged(sender as Slider, Joint6TextBlock);
		}

		private void SpeedTxtControl_TextChanged(object sender, TextChangedEventArgs e)
		{
			int NumValue;
			SpeedTxtControl.Dispatcher.Invoke
			(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
			{
				if (int.TryParse(SpeedTxtControl.Text, out NumValue))
				{
					if (NumValue > 100)
					{
						SpeedTxtControl.Text = 100.ToString();
						NumValue = 100;
					}
					else if (NumValue < 0)
					{
						SpeedTxtControl.Text = 0.ToString();
						NumValue = 0;
					}
					else
						SpeedTxtControl.Text = NumValue.ToString();
				}
				ABBController.MotionSystem.SpeedRatio = NumValue;
			}));
		}

		private void ABBConnectionWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (SupervisorTimer != null)
			{
				SupervisorTimer.Elapsed -= SupervisorTimerElapsed;
				SupervisorTimer.Enabled = false;
				SupervisorTimer = null;
			}
		}

		private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (TabControl.SelectedContent != null)
			{
				if(((sender as TabControl).SelectedItem as TabItem).Header.ToString() == "Manual Mode")
				{
					this.Dispatcher.Invoke
					(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
					{
						GetCurrentJointsAngles();
						UpdateTextBlockValues();

						KinectDevice = null;
					}));
				}
				else
				{
					KinectDevice = new MyKinect(KinectImageControl, TryAgainButton, KinectNotFoundTextBlock);
				}
			}
		}
	}
}
