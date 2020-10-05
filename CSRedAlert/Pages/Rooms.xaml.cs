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

namespace CSRedAlert.Pages
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class Rooms : Page
    {
        private class DeviceInfo : Core.Classes.Device
        {
            public string Tooltip { get; set; }
            //public SolidColorBrush StatusColor { get; set; }
            public int SlaveAddress; // <-- need to use to avoid unknown error in-place of I2C_Slave_Address.
        }

        //private class WindowShutterInfo : Core.Classes.WindowShutter
        //{

        //}

        Core.Classes.Room SelectedRoom;
        Core.Classes.Device SelectedDevice;
        Core.Classes.WindowShutter SelectedWindowShutter;

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
            if (lv_Devices.Items != null && lv_Devices.Items.Count > 0)
            {
                lv_Devices.Items.Clear();
            }
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
        }

        private void UpdateWindowShutterListView()
        {
            if(lv_WindowShutter.Items != null && lv_WindowShutter.Items.Count > 0)
            {
                lv_WindowShutter.Items.Clear();
            }
            foreach(var WindowShutter in SelectedRoom.WindowShutters)
            {
                Core.Classes.WindowShutter _WSInfo = new Core.Classes.WindowShutter();
                _WSInfo.Id = WindowShutter.Id;
                _WSInfo.ImagePath = WindowShutter.ImagePath;
                _WSInfo.Name = WindowShutter.Name;
                _WSInfo.Pin = WindowShutter.Pin;
                _WSInfo.SecondaryPin = WindowShutter.SecondaryPin;
                _WSInfo.Status = WindowShutter.Status;
                _WSInfo.I2C_Slave_Address = WindowShutter.I2C_Slave_Address;

                lv_WindowShutter.Items.Add(_WSInfo);
            }
        }

        private void lv_Rooms_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SelectedRoom = (Core.Classes.Room)lv_Rooms.SelectedItem;
            UpdateDeviceListView();
            UpdateWindowShutterListView();
        }

        private void lv_Devices_Tapped(object sender, TappedRoutedEventArgs e)
        {   
            SelectedDevice = (Core.Classes.Device)lv_Devices.SelectedItem;

            if(SelectedDevice.Status == Core.Classes.Device.StatusEnum.Off)
            {
                Task.Factory.StartNew(() =>
                {

                    SelectedDevice.TurnOn();
                }).Wait(1000);
                SelectedDevice.Status = Core.Classes.Device.StatusEnum.On;
            }
            else
            {
                Task.Factory.StartNew(() =>
                {
                    SelectedDevice.TurnOff();
                }).Wait(1000);
                SelectedDevice.Status = Core.Classes.Device.StatusEnum.Off;
            }
        }

        private void btn_up_Click(object sender, RoutedEventArgs e)
        {
            SelectedWindowShutter = (Core.Classes.WindowShutter)lv_WindowShutter.SelectedItem;

            Task.Factory.StartNew(() =>
            {
                SelectedWindowShutter.moveUp();
            }).Wait(1000);
                
        }

        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {
            SelectedWindowShutter = (Core.Classes.WindowShutter)lv_WindowShutter.SelectedItem;
            Task.Factory.StartNew(() =>
            {
                SelectedWindowShutter.stop();
            }).Wait(1000);
        }

        private void btn_down_Click(object sender, RoutedEventArgs e)
        {
            SelectedWindowShutter = (Core.Classes.WindowShutter)lv_WindowShutter.SelectedItem;
            Task.Factory.StartNew(() =>
            {
                SelectedWindowShutter.moveDown();
            }).Wait(1000);
        }
    }
}
