using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace LCARSRemote.Pages
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class Rooms : Page
    {
        private class DeviceInfo : Shared.Classes.Device
        {
            public string Tooltip { get; set; }
            //public SolidColorBrush StatusColor { get; set; }
            public int SlaveAddress; // <-- need to use to avoid unknown error in-place of I2C_Slave_Address.
        }

        Shared.Classes.Room SelectedRoom;
        Shared.Classes.Device SelectedDevice;
        Shared.Classes.WindowShutter SelectedWindowShutter;
        Shared.Classes.Sensor SelectedSensor;

        public Rooms()
        {
            this.InitializeComponent();

            UpdateRoomListView();
        }

        private void UpdateRoomListView()
        {
            if (MainPage.MyHome.Rooms.Count > 0)
            {
                foreach (var Room in MainPage.MyHome.Rooms)
                {
                    lv_Rooms.Items.Add(Room);
                }
            }
        }

        private void UpdateDeviceListView()
        {
            
            lv_Devices.Items.Clear();

            foreach (var Device in SelectedRoom.Devices)
            {
                DeviceInfo _DevInfo = new DeviceInfo();
                _DevInfo.Id = Device.Id;
                _DevInfo.ImagePath = Device.ImagePath;
                _DevInfo.Name = Device.Name;
                _DevInfo.Pin = Device.Pin;
                _DevInfo.SlaveAddress = Device.I2C_Slave_Address;
                _DevInfo.I2C_Slave_Address = Device.I2C_Slave_Address;
                //._DevInfo.IsOn = (Device.Status==Library.Core.Device.StatusEnum.On)?true:false;

                // Invert Logic due to delay in task completion (TurnOn and TurnOff functions)
                //if (Device.Status == Library.Core.Device.StatusEnum.On)
                //{

                //    _DevInfo.StatusColor = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 50, 255, 50));
                //}
                //else
                //{
                //    _DevInfo.StatusColor = new SolidColorBrush(Windows.UI.Color.FromArgb(150, 128, 128, 128));
                //}
                lv_Devices.Items.Add(_DevInfo);
            }
            grd_Devices.Visibility = lv_Devices.Items.Count() > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateWindowShutterListView()
        {
            
            lv_WindowShutters.Items.Clear();
            
            foreach (var WindowShutter in SelectedRoom.WindowShutters)
            {
                Shared.Classes.WindowShutter _WSInfo = new Shared.Classes.WindowShutter();
                _WSInfo.Id = WindowShutter.Id;
                _WSInfo.ImagePath = WindowShutter.ImagePath;
                _WSInfo.Name = WindowShutter.Name;
                _WSInfo.Pin = WindowShutter.Pin;
                _WSInfo.SecondaryPin = WindowShutter.SecondaryPin;
                _WSInfo.Status = WindowShutter.Status;
                _WSInfo.I2C_Slave_Address = WindowShutter.I2C_Slave_Address;

                lv_WindowShutters.Items.Add(_WSInfo);
            }
            grd_WindowShutters.Visibility = lv_WindowShutters.Items.Count() > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateSensorListView()
        {
         
            lv_Sensors.Items.Clear();

            foreach (var Sensor in SelectedRoom.Sensors)
            {
                Shared.Classes.WindowShutter _SenInfo = new Shared.Classes.WindowShutter();
                _SenInfo.Id = Sensor.Id;
                _SenInfo.ImagePath = Sensor.ImagePath;
                _SenInfo.Name = Sensor.Name;
                _SenInfo.Pin = Sensor.Pin;
                _SenInfo.Status = Sensor.Status;
                _SenInfo.I2C_Slave_Address = Sensor.I2C_Slave_Address;

                lv_Sensors.Items.Add(_SenInfo);
            }
            grd_Sensors.Visibility = lv_Sensors.Items.Count() > 0 ? Visibility.Visible : Visibility.Collapsed;

        }

        private void lv_Rooms_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SelectedRoom = (Shared.Classes.Room)lv_Rooms.SelectedItem;
            UpdateDeviceListView();
            UpdateWindowShutterListView();
            UpdateSensorListView();
        }

        private void btn_up_Click(object sender, RoutedEventArgs e)
        {
            Button clicked = (Button)sender;
            SelectedWindowShutter = (Shared.Classes.WindowShutter)clicked.DataContext;

            Task.Factory.StartNew(() =>
            {
                SelectedWindowShutter.moveUp();
            }).Wait(1000);

        }

        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {
            Button clicked = (Button)sender;
            SelectedWindowShutter = (Shared.Classes.WindowShutter)clicked.DataContext;

            Task.Factory.StartNew(() =>
            {
                SelectedWindowShutter.stop();
            }).Wait(1000);
        }

        private void btn_down_Click(object sender, RoutedEventArgs e)
        {
            Button clicked = (Button)sender;
            SelectedWindowShutter = (Shared.Classes.WindowShutter)clicked.DataContext;

            Task.Factory.StartNew(() =>
            {
                SelectedWindowShutter.moveDown();
            }).Wait(1000);
        }
    }
}
