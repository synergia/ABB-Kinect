﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;

using ABB.Robotics.Controllers.Discovery;
using ABB.Robotics.Controllers;

namespace ABB_Kinect
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		// variable which contains info about ABB, it is used in different funtions as a temporary variable
		private ListNetworkControllerABB NetABB = null;

		private NetworkScanner Scanner = null;
		private NetworkWatcher Watcher = null;
		private Controller ActiveABBController = null; // when connection is established it contains data about active ABB
		private List<ControllerInfo> ListOfABBControllers = new List<ControllerInfo>(); // ABB controller is added to this list when is found
		private ABBConnectionWindow ABBWindow = null; // this window will be opened when connection is established

		private class ListNetworkControllerABB
		{
			public string IPAddress{ get; set; }
			public string Id{ get; set; }
			public string Availability{ get; set; }
			public string IsVirtual{ get; set; }
			public string SystemName{ get; set; }
			public string Version{ get; set; }
			public string ControllerName{ get; set; }
		}

		public MainWindow()
		{
			InitializeComponent();
			InitializeControlsAndIndicators();
			InitializeNetworkWatcher();
			ScanNetwork();
		}

		private void InitializeNetworkWatcher()
		{
			NetABB = new ListNetworkControllerABB();
			Scanner = new NetworkScanner();
			Watcher = new NetworkWatcher(Scanner.Controllers);
			Watcher.Found += new EventHandler<NetworkWatcherEventArgs>(HandleFoundABBEvent);
			Watcher.Lost += new EventHandler<NetworkWatcherEventArgs>(HandleLostABBEvent);
			Watcher.EnableRaisingEvents = true;
		}

		private void InitializeControlsAndIndicators()
		{
			ConnectButton.IsEnabled = false;
			DisconnectButton.IsEnabled = false;
			StatusTextBlock.Text = "Disconnected";
		}

		private ListNetworkControllerABB GetInfoABB(ControllerInfo controllerInfo, ListNetworkControllerABB NetABB)
		{
			NetABB.IPAddress = controllerInfo.IPAddress.ToString();
			NetABB.Id = controllerInfo.Id;
			NetABB.Availability = controllerInfo.Availability.ToString();
			NetABB.IsVirtual = controllerInfo.IsVirtual.ToString();
			NetABB.SystemName = controllerInfo.SystemName;
			NetABB.Version = controllerInfo.Version.ToString();
			NetABB.ControllerName = controllerInfo.ControllerName;
			return NetABB;
		}

		private void GetInfoABB(ControllerInfo controllerInfo)
		{
			NetABB.IPAddress = controllerInfo.IPAddress.ToString();
			NetABB.Id = controllerInfo.Id;
			NetABB.Availability = controllerInfo.Availability.ToString();
			NetABB.IsVirtual = controllerInfo.IsVirtual.ToString();
			NetABB.SystemName = controllerInfo.SystemName;
			NetABB.Version = controllerInfo.Version.ToString();
			NetABB.ControllerName = controllerInfo.ControllerName;
		}

		private void ScanNetwork()
		{
			ConnectButton.IsEnabled = false;

			ListOfABBControllers.Clear();
			NetworkScanner Scanner = new NetworkScanner();
			Scanner.Scan();
			ControllerInfoCollection controllers = Scanner.Controllers;

			ListOfDevices.Dispatcher.Invoke
				(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
				{
					ListOfDevices.Items.Clear();
				}));

			foreach (ControllerInfo controllerInfo in controllers)
			{
				ListNetworkControllerABB ABBList = new ListNetworkControllerABB();
				ABBList = GetInfoABB(controllerInfo,ABBList);
				ListOfDevices.Dispatcher.Invoke
					(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
					{
						ListOfDevices.Items.Add(ABBList);
					}));
				ListOfABBControllers.Add(controllerInfo);
			}
		}
		
		private void DisconnectABB()
		{
			if (ActiveABBController != null)
			{
				ActiveABBController.Logoff();
				ActiveABBController.Dispose();
				ActiveABBController = null;
			}
			DisconnectButton.IsEnabled = false;
			ConnectButton.IsEnabled = false;
			StatusTextBlock.Text = "Disconnected";
			ListOfDevices.UnselectAll();
			if (ABBWindow != null)
			{
				ABBWindow.Close();
			}
		}

		private void HandleFoundABBEvent(object source, NetworkWatcherEventArgs e)
		{
			ControllerInfo Info = e.Controller;
			GetInfoABB(Info);
			ListOfDevices.Dispatcher.Invoke
			(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
			{
				ListOfDevices.Items.Add(NetABB);
			}));
			ListOfABBControllers.Add(Info);
		}

		private void HandleLostABBEvent(object source, NetworkWatcherEventArgs e)
		{
			ScanNetwork();
			DisconnectABB();
		}

		private void ListOfDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (ActiveABBController == null && ListOfDevices.HasItems && ListOfDevices.SelectedItems.Count > 0) // Connection wasn't established
				ConnectButton.IsEnabled = true;
		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			DisconnectABB();
			this.Close();
		}

		private void ConnectButton_Click(object sender, RoutedEventArgs e)
		{
			if (ListOfDevices.SelectedItems.Count == 1)
			{
				ControllerInfo Info = ListOfABBControllers.ElementAt(ListOfDevices.SelectedIndex);

				if (Info.Availability == Availability.Available)
				{
					if (ActiveABBController != null) // to connect can't be other connection
						DisconnectABB();
					
					// Login into controller
					ActiveABBController = ControllerFactory.CreateFrom(Info);
					ActiveABBController.Logon(UserInfo.DefaultUser);
					DisconnectButton.IsEnabled = true;
					ConnectButton.IsEnabled = false;
					StatusTextBlock.Text = "Connected " + ActiveABBController.IPAddress.ToString();

					ABBWindow = new ABBConnectionWindow(ActiveABBController);
					ABBWindow.Closed += ABBWindow_Closed;
				}
				else
				{
					MessageBox.Show("Selected controller not available.");
				}
			}
		}

		private void DisconnectButton_Click(object sender, RoutedEventArgs e)
		{
			DisconnectABB();
		}
		
		private void ABBWindow_Closed(object sender, System.EventArgs e)
		{
			ABBWindow = null;
			DisconnectABB();
		}
	}
}