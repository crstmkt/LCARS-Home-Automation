using System;
using Windows.Devices.Gpio;
using Windows.ApplicationModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.UI.Popups;

// Die Vorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 dokumentiert.

namespace CSRedAlert
{

    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet werden kann oder auf die innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public enum AlertStatusEnum
        {
            Active,
            Inactive,
            NotDefined
        }

        public AlertStatusEnum AlertStatus = AlertStatusEnum.Inactive;

        public static Core.Classes.Home MyHome;
        public static List<Core.Classes.Log> Logs = new List<Core.Classes.Log>();

        public MainPage()
        {
            this.InitializeComponent();

            // Load last saved Home object
            MyHome = Core.Classes.Home.LoadHome().Result;
            Logs.Add(new Core.Classes.Log("Home Loaded."));
            

            // Turn Off all devices
            foreach (var Room in MyHome.Rooms)
            {
                foreach (var Device in Room.Devices)
                {
                    Task.Factory.StartNew(async() => { await Device.TurnOff(); });
                }

                //foreach (var WindowShutter in Room.WindowShutters)
                //{

                //}
            }



            this.tb_VersionInfo.Text = string.Format("Version: {0}.{1}.{2}.{3}",
                                                        Package.Current.Id.Version.Major,
                                                        Package.Current.Id.Version.Minor,
                                                        Package.Current.Id.Version.Build,
                                                        Package.Current.Id.Version.Revision);
            //this.tb_MachineName.Text = string.Format("Machine Name: {0}");
            this.tb_AppName.Text = string.Format("{0}", Package.Current.DisplayName);

            this.frm_ContentFrame.Navigate(typeof(Pages.Dashboard), null);

        }

        #region "Alert Stuff"
        private void alert_Start(object sender, object e)
        {
            Alerter.Visibility = Visibility.Visible;
            AlertPlayer.Play();
            AlertStatus = AlertStatusEnum.Active;
            Logs.Add(new Core.Classes.Log("Alert started!"));
        }

        private void alert_Stop(object sender, object e)
        {
            Alerter.Visibility = Visibility.Collapsed;
            AlertPlayer.Stop();
            AlertStatus = AlertStatusEnum.Inactive;
            Logs.Add(new Core.Classes.Log("Alert stopped!"));
        }

        private void Alerter_Ignore(object sender, RoutedEventArgs e)
        {
            alert_Stop(sender, e);
        }

        private void btn_RedAlert_Click(object sender, RoutedEventArgs e)
        {
            Alerter.Visibility = Visibility.Visible;
            AlertPlayer.IsLooping = true;
            AlertPlayer.Play();
        }
        #endregion

        #region "Navigation"
        private void btn_Manual_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.frm_ContentFrame.Navigate(typeof(Pages.Manual), null);
        }

        private void btn_Dashboard_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.frm_ContentFrame.Navigate(typeof(Pages.Dashboard), null);
        }

        private void btn_Rooms_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.frm_ContentFrame.Navigate(typeof(Pages.Rooms), null);
        }

        private void btn_Logs_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.frm_ContentFrame.Navigate(typeof(Pages.Logs), null);
        }

        private void btn_Alert_Tapped(object sender, TappedRoutedEventArgs e)
        {
            alert_Start(sender, e);
        }

        private void btn_Settings_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.frm_ContentFrame.Navigate(typeof(Pages.Settings), null);
        }

        private void btn_Config_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.frm_ContentFrame.Navigate(typeof(Pages.Configuration), null);
        }

        private void btn_Standby_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //Put Monitor to Standby
        }
        #endregion
    }
}
