using System;
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
		private bool ABBConnectionEstablished = false;
		private ListNetworkControllerABB NetABB = null;
		private NetworkScanner Scanner = null;
		private NetworkWatcher Watcher = null;

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
			ConnectDisconnectButton.IsEnabled = false;
			StatusTextBlock.Text = "Disconnected";
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
				GetInfoABB(controllerInfo);
				ListOfDevices.Dispatcher.Invoke
					(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
					{
						ListOfDevices.Items.Add(NetABB);
					}));
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
		}

		private void HandleLostABBEvent(object source, NetworkWatcherEventArgs e)
		{
			ScanNetwork();
		}

		private void ListOfDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!ABBConnectionEstablished)
				ConnectDisconnectButton.IsEnabled = true;
		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}