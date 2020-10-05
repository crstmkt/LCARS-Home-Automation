using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.Foundation;
using Windows.UI.Core;

// Die Vorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 dokumentiert.

namespace LCARSRemote
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage_Deprecated : Page
    {

        public static Shared.Classes.Home MyHome = new Shared.Classes.Home();

        public MainPage_Deprecated()
        {
            this.InitializeComponent();
            
            //On Startup get Home Object from Server
            //GetHomeObject();
            //On Startup go to Dashboard
            this.frm_ContentFrame.Navigate(typeof(Pages.Configuration), null);
            
        }

        public async void GetHomeObject()
        {
            //Hostname has to be outsourced in Config data or somewhat
            Shared.Communication.TCP_Connect tcpcon = new Shared.Communication.TCP_Connect(new HostName("minwinpc"));

            await tcpcon.Connect();
            //await tcpcon.Send("getHome");
            tcpcon.Disconnect();
        }
        
        #region "Navigation"
        private void btn_Dashboard_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            this.frm_ContentFrame.Navigate(typeof(Pages.Dashboard), null);
        }
        private void btn_Rooms_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            this.frm_ContentFrame.Navigate(typeof(Pages.Rooms), null);
        }
        private void btn_Configuration_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            this.frm_ContentFrame.Navigate(typeof(Pages.Configuration), null);
        }
        #endregion


    }
}
