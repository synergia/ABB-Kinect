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
		public ListNetworkControllerABB NetABB = null;
		private NetworkScanner scanner = null;
		private Timer RefreshTimer = null;

		public class ListNetworkControllerABB
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
			InitializeRefreshTimer();
		}

		private void InitializeRefreshTimer()
		{
			RefreshTimer = new Timer(3000);
			RefreshTimer.Elapsed += new ElapsedEventHandler(RefreshNetworkEvent);
			RefreshTimer.AutoReset = true;
			RefreshTimer.Enabled = true;
			NetABB = new ListNetworkControllerABB();
		}

		private void RefreshNetworkEvent(object source, ElapsedEventArgs e)
		{
			ScanNetwork();
		}

		private void ScanNetwork()
		{
			scanner = new NetworkScanner();
			scanner.Scan();
			ControllerInfoCollection controllers = scanner.Controllers;

			ListOfDevices.Dispatcher.Invoke
				(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
			{
				ListOfDevices.Items.Clear();
			}));

			foreach (ControllerInfo controllerInfo in controllers)
			{
				NetABB.IPAddress = controllerInfo.IPAddress.ToString();
				NetABB.Id = controllerInfo.Id;
				NetABB.Availability = controllerInfo.Availability.ToString();
				NetABB.IsVirtual = controllerInfo.IsVirtual.ToString();
				NetABB.SystemName = controllerInfo.SystemName;
				NetABB.Version = controllerInfo.Version.ToString();
				NetABB.ControllerName = controllerInfo.ControllerName;
				ListOfDevices.Dispatcher.Invoke
					(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate()
				{
					ListOfDevices.Items.Add(NetABB); 
				}));
			}
		}
	}
}