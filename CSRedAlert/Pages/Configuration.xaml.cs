using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CSRedAlert.Core.Classes;
using CSRedAlert.Core;
using System.Globalization;
using Windows.UI.Popups;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace CSRedAlert.Pages
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class Configuration : Page
    {
        /// <summary>
        /// Updates items in listboxes.
        /// </summary>
        /// <param name="Mode">Select 0 to update both Room and Device listbox. 1 for Room and 2 for Device only</param>
        private void UpdateListBox(byte Mode = 0)
        {
            if (Mode == 0 || Mode == 1)
            {
                lb_Rooms.Items.Clear();
            }

            if (MainPage.MyHome.Rooms.Count > 0)
            {
                if (Mode == 0 || Mode == 1)
                {
                    foreach (var Room in MainPage.MyHome.Rooms)
                    {
                        lb_Rooms.Items.Add(Room);
                    }

                    //lb_Rooms.SelectedIndex = 0;
                }

                if (Mode == 0 || Mode == 2)
                {
                    lb_Devices.Items.Clear();
                    if (MainPage.MyHome.Rooms[lb_Rooms.SelectedIndex].Devices.Count > 0)
                    {
                        try {
                            foreach (var Device in MainPage.MyHome.Rooms[lb_Rooms.SelectedIndex].Devices)
                            {
                                lb_Devices.Items.Add(Device);
                            }
                        }
                        catch(Exception ex)
                        {
                            MainPage.Logs.Add(new Core.Classes.Log(ex.Message));
                        }

                        try {
                            foreach (var Sensor in MainPage.MyHome.Rooms[lb_Rooms.SelectedIndex].Sensors)
                            {
                                lb_Devices.Items.Add(Sensor);
                            }
                        }
                        catch(Exception ex)
                        {
                            MainPage.Logs.Add(new Core.Classes.Log(ex.Message));
                        }
                        try {
                            foreach (var WindowShutter in MainPage.MyHome.Rooms[lb_Rooms.SelectedIndex].WindowShutters)
                            {
                                lb_Devices.Items.Add(WindowShutter);
                            }
                        }
                        catch(Exception ex)
                        {
                            MainPage.Logs.Add(new Core.Classes.Log(ex.Message));
                        }
                        //lb_Devices.SelectedIndex = 0;
                    }
                }
            }
        }

        public Configuration()
        { 
            this.InitializeComponent();
            UpdateListBox(1);
        }

        private void btn_AddRoom_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Core.Classes.Room NewRoom = new Core.Classes.Room();

            NewRoom.I2C_SlaveAdress = 0x40;
            NewRoom.Color = "";
            NewRoom.Name = tbx_RoomName.Text.ToString();

            MainPage.MyHome.Rooms.Add(NewRoom);
            UpdateListBox(1);
        }

        private void btn_RemoveRoom_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (lb_Rooms.Items.Count > 0 && lb_Rooms.SelectedIndex != -1)
            {
                MainPage.MyHome.Rooms.Remove((Core.Classes.Room)lb_Rooms.SelectedItem);
                lb_Rooms.Items.Remove(lb_Rooms.SelectedItem);
            }
            UpdateListBox();
        }

        private void lb_Rooms_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (lb_Rooms.Items.Count > 0)
            {
                UpdateRoomDetailPane((Core.Classes.Room)lb_Rooms.SelectedItem);
                UpdateDeviceDetailPane((Core.Classes.Room)lb_Rooms.SelectedItem, true);
            }
        }

        private void btn_Save_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (lb_Rooms.Items.Count > 0 && lb_Rooms.SelectedIndex != -1)
            {
                // Update Room Detail
                Core.Classes.Room SelectedRoom = (Core.Classes.Room)lb_Rooms.SelectedItem;

                SelectedRoom.Name = tbx_RoomName.Text.ToString();

                int NewAddress = 0;
                IFormatProvider _Culture = new CultureInfo("en-US");
                int.TryParse(tbx_I2CslaveAddress.Text.Replace("0x", ""), System.Globalization.NumberStyles.HexNumber, _Culture, out NewAddress);
                SelectedRoom.I2C_SlaveAdress = NewAddress;
                
                // Update Device Detail
                if (lb_Devices.Items.Count > 0)
                {
                    if (lb_Devices.SelectedIndex != -1)
                    {
                        System.Type type = lb_Devices.SelectedItem.GetType();
                        if (type == typeof(Core.Classes.Device))
                        {
                            Core.Classes.Device SelectedDevice = (Core.Classes.Device)lb_Devices.SelectedItem;

                            SelectedDevice.Name = tbx_DeviceName.Text.ToString();
                            try
                            {
                                SelectedDevice.Pin = (Core.Classes.Device.PinsEnum)Enum.Parse(typeof(Core.Classes.Device.PinsEnum), cbx_DevicePin.SelectedItem.ToString());
                            }
                            catch (Exception ex)
                            {
                                SelectedDevice.Pin = (Core.Classes.Device.PinsEnum)Enum.Parse(typeof(Core.Classes.Device.PinsEnum), "D0");
                                MainPage.Logs.Add(new Core.Classes.Log(ex.Message));
                            }

                        } else if (type == typeof(Core.Classes.WindowShutter))
                        {
                            Core.Classes.WindowShutter SelectedDevice = (Core.Classes.WindowShutter)lb_Devices.SelectedItem;

                            SelectedDevice.Name = tbx_DeviceName.Text.ToString();
                            try
                            {
                                SelectedDevice.Pin = (Core.Classes.Device.PinsEnum)Enum.Parse(typeof(Core.Classes.Device.PinsEnum), cbx_DevicePin.SelectedItem.ToString());
                                SelectedDevice.SecondaryPin = (Core.Classes.Device.PinsEnum)Enum.Parse(typeof(Core.Classes.Device.PinsEnum), cbx_DevicePinSec.SelectedItem.ToString());
                            }
                            catch(Exception ex)
                            {
                                SelectedDevice.Pin = (Core.Classes.Device.PinsEnum)Enum.Parse(typeof(Core.Classes.Device.PinsEnum), "D0");
                                SelectedDevice.SecondaryPin = (Core.Classes.Device.PinsEnum)Enum.Parse(typeof(Core.Classes.Device.PinsEnum), "D1");
                                MainPage.Logs.Add(new Core.Classes.Log(ex.Message));
                            }
                        } else if (type == typeof(Core.Classes.Sensor))
                        {
                            Core.Classes.Sensor SelectedDevice = (Core.Classes.Sensor)lb_Devices.SelectedItem;

                            SelectedDevice.Name = tbx_DeviceName.Text.ToString();
                            try
                            {
                                SelectedDevice.Pin = (Core.Classes.Device.PinsEnum)Enum.Parse(typeof(Core.Classes.Device.PinsEnum), cbx_DevicePin.SelectedItem.ToString());
                            }
                            catch(Exception ex)
                            {
                                SelectedDevice.Pin = (Core.Classes.Device.PinsEnum)Enum.Parse(typeof(Core.Classes.Device.PinsEnum), "A1");
                                MainPage.Logs.Add(new Core.Classes.Log(ex.Message));
                            }
                        }
                    }
                }
            }

            // Save
            Core.Classes.Home.SaveHome(MainPage.MyHome);
        }

        private void UpdateRoomDetailPane(Core.Classes.Room SelectedRoom)
        {
            tbx_RoomName.Text = SelectedRoom.Name;
            tbx_I2CslaveAddress.Text = "0x" + SelectedRoom.I2C_SlaveAdress.ToString("X");
        }

        private void UpdateDeviceDetailPane(Core.Classes.Room SelectedRoom, bool SelectFirst = false, bool SkipClear = false)
        {
            if (SkipClear == false)
            {
                ClearDeviceDetailPane();
            }
            if (SelectedRoom.Devices.Count > 0 || SelectedRoom.Sensors.Count > 0 || SelectedRoom.WindowShutters.Count > 0)
            {
                if (SkipClear == false)
                {
                    try {
                        foreach (var _Device in SelectedRoom.Devices)
                        {
                            lb_Devices.Items.Add(_Device);
                        }
                    }
                    catch(Exception ex)
                    {
                        MainPage.Logs.Add(new Core.Classes.Log(ex.Message));
                    }
                    try {
                        foreach (var _Sensor in SelectedRoom.Sensors)
                        {
                            lb_Devices.Items.Add(_Sensor);
                        }
                    }
                    catch(Exception ex)
                    {
                        MainPage.Logs.Add(new Core.Classes.Log(ex.Message));
                    }
                    try { 
                        foreach (var _WindowShutter in SelectedRoom.WindowShutters)
                        {
                            lb_Devices.Items.Add(_WindowShutter);
                        }
                    }
                    catch(Exception ex)
                    {
                        MainPage.Logs.Add(new Core.Classes.Log(ex.Message));
                    }
                }

                if (SelectFirst)
                {
                    lb_Devices.SelectedIndex = 0;
                }

                var type = lb_Devices.SelectedItem.GetType();
                
                if(type == typeof(Core.Classes.Device))
                {
                    Core.Classes.Device SelectedDevice = (Core.Classes.Device)lb_Devices.SelectedItem;
                    try
                    {
                        tbx_DeviceName.Text = SelectedDevice.Name;
                        cbx_DevicePin.SelectedItem = SelectedDevice.Pin.ToString();
                    }
                    catch(Exception ex)
                    {
                        tbx_DeviceName.Text = "";
                        cbx_DevicePin.SelectedItem = "D0";
                        MainPage.Logs.Add(new Core.Classes.Log(ex.Message));
                    }
                }
                else if (type == typeof(Core.Classes.WindowShutter))
                {
                    Core.Classes.WindowShutter SelectedDevice = (Core.Classes.WindowShutter)lb_Devices.SelectedItem;
                    try
                    {
                        tbx_DeviceName.Text = SelectedDevice.Name;
                        cbx_DevicePin.SelectedItem = SelectedDevice.Pin.ToString();
                        cbx_DevicePinSec.SelectedItem = SelectedDevice.SecondaryPin.ToString();
                    }
                    catch(Exception ex)
                    {
                        tbx_DeviceName.Text = "";
                        cbx_DevicePin.SelectedItem = "D0";
                        cbx_DevicePin.SelectedItem = "D1";
                        MainPage.Logs.Add(new Core.Classes.Log(ex.Message));
                    }
                }
                else if(type == typeof(Core.Classes.Sensor))
                {
                    Core.Classes.Sensor SelectedDevice = (Core.Classes.Sensor)lb_Devices.SelectedItem;
                    try
                    {
                        tbx_DeviceName.Text = SelectedDevice.Name;
                        cbx_DevicePin.SelectedItem = SelectedDevice.Pin.ToString();
                    }
                    catch(Exception ex)
                    {
                        tbx_DeviceName.Text = "";
                        cbx_DevicePin.SelectedItem = "A2";
                        MainPage.Logs.Add(new Core.Classes.Log(ex.Message));
                    }
                }
            }
        }

        private void ClearDeviceDetailPane()
        {
            if (lb_Devices.Items != null && lb_Devices.Items.Count > 0)
            {
                lb_Devices.Items.Clear();
            }
            tbx_DeviceName.Text = "";
        }

        private void btn_AddDevice_Tapped(object sender, TappedRoutedEventArgs e)
        {

            switch(cbx_DeviceType.SelectedIndex){
                case 0:
                    Core.Classes.Device NewDevice = new Core.Classes.Device();
                    NewDevice.Name = "My Device";
                    NewDevice.Pin = Core.Classes.Device.PinsEnum.D0;
                    MainPage.MyHome.Rooms[lb_Rooms.SelectedIndex].AddDevice(NewDevice);
                    break;
                case 1:
                    Core.Classes.WindowShutter NewWindowShutter = new Core.Classes.WindowShutter();
                    NewWindowShutter.Name = "My Window Shutter";
                    NewWindowShutter.Pin = Core.Classes.Device.PinsEnum.D0;
                    NewWindowShutter.SecondaryPin = Core.Classes.Device.PinsEnum.D1;
                    MainPage.MyHome.Rooms[lb_Rooms.SelectedIndex].AddWindowShutter(NewWindowShutter);
                    break;
                case 2:
                    Core.Classes.Sensor NewSensor = new Core.Classes.Sensor();
                    NewSensor.Name = "My Sensor";
                    NewSensor.Pin = Core.Classes.Sensor.PinsEnum.D0;
                    MainPage.MyHome.Rooms[lb_Rooms.SelectedIndex].AddSensor(NewSensor);
                    break;
            }

            // Only update device listbox
            UpdateListBox(2);
        }

        private void btn_RemoveDevice_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (lb_Devices.Items.Count > 0 && lb_Devices.SelectedIndex != -1)
            {
                if (lb_Devices.SelectedItem.GetType() == typeof(Core.Classes.Device))
                {
                    MainPage.MyHome.Rooms[lb_Rooms.SelectedIndex].Devices.Remove((Core.Classes.Device)lb_Devices.SelectedItem);
                    lb_Devices.Items.Remove(lb_Devices.SelectedItem);
                } else if (lb_Devices.SelectedItem.GetType() == typeof(Core.Classes.WindowShutter))
                {
                    MainPage.MyHome.Rooms[lb_Rooms.SelectedIndex].WindowShutters.Remove((Core.Classes.WindowShutter)lb_Devices.SelectedItem);
                    lb_Devices.Items.Remove(lb_Devices.SelectedItem);
                } else if (lb_Devices.SelectedItem.GetType() == typeof(Core.Classes.Sensor))
                {
                    MainPage.MyHome.Rooms[lb_Rooms.SelectedIndex].Sensors.Remove((Core.Classes.Sensor)lb_Devices.SelectedItem);
                    lb_Devices.Items.Remove(lb_Devices.SelectedItem);
                }
            }
            UpdateListBox(2);
            UpdateDeviceDetailPane((Core.Classes.Room)lb_Rooms.SelectedItem, true, false);
        }

        private void cbx_DeviceType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                switch (cbx_DeviceType.SelectedIndex)
                {
                    case 0: //Device Type "Device"
                        cbx_DevicePinSec.Visibility = Visibility.Collapsed;
                        //cbx_DevicePin.Items.Clear();
                        addPins();
                        break;
                    case 1: //Device type Window Shutter
                        cbx_DevicePin.Items.Clear();

                        cbx_DevicePinSec.Visibility = Visibility.Visible;
                        //cbx_DevicePinSec.Items.Clear();
                        addPins();
                        break;
                    case 2: //Device Type Sensor
                        cbx_DevicePinSec.Visibility = Visibility.Collapsed;
                        //cbx_DevicePin.Items.Clear();
                        addPins();
                        break;

                }
            }
            catch (Exception ex)
            {
                //var dialog = new MessageDialog("Exception: " + ex);
                //dialog.Commands.Add(new UICommand("OK"));
                //await dialog.ShowAsync();
                MainPage.Logs.Add(new Core.Classes.Log(ex.Message));
            }

        }

        private void lb_Devices_Tapped(object sender, TappedRoutedEventArgs e)
        {
            System.Type type = lb_Devices.SelectedItem.GetType();
            if(type == typeof(Core.Classes.Device))
            {
                cbx_DeviceType.SelectedIndex = 0;
            } else if (type == typeof(Core.Classes.WindowShutter))
            {
                cbx_DeviceType.SelectedIndex = 1;
            } else if(type == typeof(Core.Classes.Sensor))
            {
                cbx_DeviceType.SelectedIndex = 2;
            }else
            {
                cbx_DeviceType.SelectedIndex = 0;
            }

            addPins();
            UpdateDeviceDetailPane((Core.Classes.Room)lb_Rooms.SelectedItem, false, true);
        }

        private void addPins()
        {
            cbx_DevicePin.Items.Clear();
            switch (cbx_DeviceType.SelectedIndex)
            {
                case 0:
                case 1:
                    cbx_DevicePin.Items.Add("D0");
                    cbx_DevicePin.Items.Add("D1");
                    cbx_DevicePin.Items.Add("D3");
                    cbx_DevicePin.Items.Add("D4");
                    cbx_DevicePin.Items.Add("D5");
                    cbx_DevicePin.Items.Add("D6");
                    cbx_DevicePin.Items.Add("D7");
                    cbx_DevicePin.Items.Add("D8");
                    cbx_DevicePin.Items.Add("D9");
                    cbx_DevicePin.Items.Add("D10");
                    cbx_DevicePin.Items.Add("D11");
                    cbx_DevicePin.Items.Add("D12");

                    if (cbx_DeviceType.SelectedIndex == 1)
                    {
                        cbx_DevicePinSec.Items.Clear();
                        cbx_DevicePinSec.Items.Add("D0");
                        cbx_DevicePinSec.Items.Add("D1");
                        cbx_DevicePinSec.Items.Add("D3");
                        cbx_DevicePinSec.Items.Add("D4");
                        cbx_DevicePinSec.Items.Add("D5");
                        cbx_DevicePinSec.Items.Add("D6");
                        cbx_DevicePinSec.Items.Add("D7");
                        cbx_DevicePinSec.Items.Add("D8");
                        cbx_DevicePinSec.Items.Add("D9");
                        cbx_DevicePinSec.Items.Add("D10");
                        cbx_DevicePinSec.Items.Add("D11");
                        cbx_DevicePinSec.Items.Add("D12");
                    }
                    break;
                case 2:
                    cbx_DevicePin.Items.Add("A2");
                    cbx_DevicePin.Items.Add("A3");
                    break;

            }
        }
    }
}
