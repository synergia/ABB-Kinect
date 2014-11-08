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
			this.Show();
		}

		private void ResetPositionButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (ABBController.OperatingMode == ControllerOperatingMode.Auto)
				{
					tasks = ABBController.Rapid.GetTasks();

					//
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
	}
}
