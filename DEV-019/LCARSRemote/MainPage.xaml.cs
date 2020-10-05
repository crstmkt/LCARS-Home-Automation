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
using Windows.ApplicationModel;

// Die Vorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 dokumentiert.

namespace LCARSRemote
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public static Shared.Classes.Home MyHome = new Shared.Classes.Home();

        public MainPage()
        {
            this.InitializeComponent();

            //On Startup get Home Object from Server
            GetHomeObject();

            //On Startup go to Dashboard
            this.frm_ContentFrame.Navigate(typeof(Pages.Dashboard), null);

            this.txb_Header.Text = String.Concat(string.Format("{0}", Package.Current.DisplayName), string.Format(" Version: {0}.{1}.{2}.{3}",
                                                        Package.Current.Id.Version.Major,
                                                        Package.Current.Id.Version.Minor,
                                                        Package.Current.Id.Version.Build,
                                                        Package.Current.Id.Version.Revision));

        }

        public async void GetHomeObject()
        {
            Shared.Classes.Home localHome = new Shared.Classes.Home();
            Shared.Classes.Home remoteHome = new Shared.Classes.Home();

            //Get local Home object
            try
            {
                MyHome = Shared.Classes.Home.LoadHome().Result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            //Get Remote Home Object
            //Hostname has to be outsourced in Config data or somewhat
            //Shared.Communication.TCP_Connect tcpcon = new Shared.Communication.TCP_Connect(new HostName("minwinpc"));
            
            //await tcpcon.Connect();
            //await tcpcon.Send(Shared.Classes.Serializer.Serialize("getHome"));
            //await 
            //string resp = tcpcon.Read().Result;
            //tcpcon.socket.InputStream;

            //if(remoteHome.LastUpdate > localHome.LastUpdate)
            //{
            //    MyHome = remoteHome;
            //}
            //tcpcon.Disconnect();
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
        private void btn_Logs_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

        }

        private async void btn_Temp_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Shared.Communication.TCP_Connect tcpcon = new Shared.Communication.TCP_Connect(new HostName("minwinpc"));
            await tcpcon.Connect();
            await tcpcon.Send("3$Elem1$Elem2$Elem3");
            tcpcon.Disconnect();
        }

        private void btn_Settings_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            this.frm_ContentFrame.Navigate(typeof(Pages.Settings), null);
        }

        private void btn_Info_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            this.frm_ContentFrame.Navigate(typeof(Pages.Info), null);
        }

        #endregion


    }
}
