using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.RapidDomain;

namespace ABB_Kinect
{
	/// <summary>
	/// Interaction logic for ABBConnectionWindow.xaml
	/// </summary>
	public partial class ABBConnectionWindow : Window
	{
		private Controller ABBController = null;
		private Task[] tasks = null;

		public ABBConnectionWindow(Controller controller)
		{
			InitializeComponent();
			ABBController = controller;
			this.Title = "ABB " + ABBController.IPAddress.ToString();
			ABBWindowTitleLabel.Text = "Connected to " + ABBController.IPAddress.ToString();
			ABBWindowNameLabel.Text = ABBController.Name.ToString();
			InitializeManualMode();
			this.Show();
		}

		private void InitializeManualMode()
		{
			Joint1Slider.Minimum = -180;
			Joint1Slider.Maximum = 180;
			Joint1Slider.TickFrequency = 1;
			Joint1Slider.IsSnapToTickEnabled = true;
			Joint1Slider.TickFrequency = 1;

			Joint2Slider.Minimum = -180;
			Joint2Slider.Maximum = 180;
			Joint2Slider.TickFrequency = 1;
			Joint2Slider.IsSnapToTickEnabled = true;
			Joint2Slider.TickFrequency = 1;

			Joint3Slider.Minimum = -180;
			Joint3Slider.Maximum = 180;
			Joint3Slider.TickFrequency = 1;
			Joint3Slider.IsSnapToTickEnabled = true;
			Joint3Slider.TickFrequency = 1;

			Joint4Slider.Minimum = -180;
			Joint4Slider.Maximum = 180;
			Joint4Slider.TickFrequency = 1;
			Joint4Slider.IsSnapToTickEnabled = true;
			Joint4Slider.TickFrequency = 1;

			Joint5Slider.Minimum = -180;
			Joint5Slider.Maximum = 180;
			Joint5Slider.TickFrequency = 1;
			Joint5Slider.IsSnapToTickEnabled = true;
			Joint5Slider.TickFrequency = 1;

			Joint6Slider.Minimum = -180;
			Joint6Slider.Maximum = 180;
			Joint6Slider.TickFrequency = 1;
			Joint6Slider.IsSnapToTickEnabled = true;
			Joint6Slider.TickFrequency = 1;

			Joint1TextBlock.Text = ((int)Joint1Slider.Value).ToString();
			Joint2TextBlock.Text = ((int)Joint2Slider.Value).ToString();
			Joint3TextBlock.Text = ((int)Joint3Slider.Value).ToString();
			Joint4TextBlock.Text = ((int)Joint4Slider.Value).ToString();
			Joint5TextBlock.Text = ((int)Joint5Slider.Value).ToString();
			Joint6TextBlock.Text = ((int)Joint6Slider.Value).ToString();

			GetCurrentJointsAngles();
		}

		private void GetCurrentJointsAngles()
		{
			try
			{
				if (ABBController.OperatingMode == ControllerOperatingMode.Auto)
				{
					tasks = ABBController.Rapid.GetTasks();
					RapidData rd = tasks[0].GetRapidData("MainModule", "current_joints_angles");
					ABB.Robotics.Controllers.RapidDomain.JointTarget angles = new ABB.Robotics.Controllers.RapidDomain.JointTarget();
					if (rd.Value is ABB.Robotics.Controllers.RapidDomain.JointTarget)
					{
						angles = (ABB.Robotics.Controllers.RapidDomain.JointTarget)rd.Value;
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

		private void SetSliders(ABB.Robotics.Controllers.RapidDomain.JointTarget angles)
		{
			Joint1Slider.Value = angles.RobAx.Rax_1;
			Joint2Slider.Value = angles.RobAx.Rax_2;
			Joint3Slider.Value = angles.RobAx.Rax_3;
			Joint4Slider.Value = angles.RobAx.Rax_4;
			Joint5Slider.Value = angles.RobAx.Rax_5;
			Joint6Slider.Value = angles.RobAx.Rax_6;
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
						RapidData rd = tasks[0].GetRapidData("MainModule", "flag_exec");
						ABB.Robotics.Controllers.RapidDomain.Bool rapidBool = new ABB.Robotics.Controllers.RapidDomain.Bool();
						rapidBool.Value = true;
						rd.Value = rapidBool;
					}
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
			Joint1TextBlock.Text = ((int)Joint1Slider.Value).ToString();
		}

		private void Joint2Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			Joint2TextBlock.Text = ((int)Joint2Slider.Value).ToString();
		}

		private void Joint3Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			Joint3TextBlock.Text = ((int)Joint3Slider.Value).ToString();
		}

		private void Joint4Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			Joint4TextBlock.Text = ((int)Joint4Slider.Value).ToString();
		}

		private void Joint5Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			Joint5TextBlock.Text = ((int)Joint5Slider.Value).ToString();
		}

		private void Joint6Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			Joint6TextBlock.Text = ((int)Joint6Slider.Value).ToString();
		}
	
	}
}
