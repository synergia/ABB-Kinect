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

using ABB.Robotics.Controllers.Discovery;
using ABB.Robotics.Controllers;

namespace ABB_Kinect
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private NetworkScanner scanner = null;
		private Controller controller = null;

		public MainWindow()
		{
			InitializeComponent();
			ListOfDevices.Items.Add("coś");
			ScanNetwork();
		}

		private void ListOfDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			
		}

		private void ScanNetwork()
		{
			scanner = new NetworkScanner();
			scanner.Scan();
			ControllerInfoCollection controllers = scanner.Controllers;
			foreach (ControllerInfo controllerInfo in controllers)
			{
				ListOfDevices.Items.Add(controllerInfo.IPAddress.ToString());
			}
		}
	}
}
