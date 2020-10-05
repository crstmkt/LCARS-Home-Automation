using System;
using System.Collections.Generic;
using System.Globalization;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LCARSRemote.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Configuration : Page
    {
        uint deviceType = 0;
        /// <summary>
        /// Updates items in listboxes.
        /// </summary>
        /// <param name="Mode">Select 0 to update both Room and Device listbox. 1 for Room and 2 for Device only</param>
        private void UpdateListBox(byte Mode = 0)
        {
            if (Mode == 0 || Mode == 1)
            {
                lv_Rooms.Items.Clear();
            }

            if (MainPage.MyHome.Rooms.Count > 0)
            {
                if (Mode == 0 || Mode == 1)
                {
                    foreach (var Room in MainPage.MyHome.Rooms)
                    {
                        lv_Rooms.Items.Add(Room);
                    }

                    //lb_Rooms.SelectedIndex = 0;
                }

                if (Mode == 0 || Mode == 2)
                {
                    lv_Devices.Items.Clear();
                    if (MainPage.MyHome.Rooms[lv_Rooms.SelectedIndex].Devices.Count > 0 || 
                        MainPage.MyHome.Rooms[lv_Rooms.SelectedIndex].WindowShutters.Count > 0 ||
                        MainPage.MyHome.Rooms[lv_Rooms.SelectedIndex].Sensors.Count > 0)
                    {
                        try
                        {
                            foreach (var Device in MainPage.MyHome.Rooms[lv_Rooms.SelectedIndex].Devices)
                            {
                                lv_Devices.Items.Add(Device);
                            }
                        }
                        catch { }

                        try
                        {
                            foreach (var Sensor in MainPage.MyHome.Rooms[lv_Rooms.SelectedIndex].Sensors)
                            {
                                lv_Devices.Items.Add(Sensor);
                            }
                        }
                        catch { }
                        try
                        {
                            foreach (var WindowShutter in MainPage.MyHome.Rooms[lv_Rooms.SelectedIndex].WindowShutters)
                            {
                                lv_Devices.Items.Add(WindowShutter);
                            }
                        }
                        catch { }
                        //lb_Devices.SelectedIndex = 0;
                    }
                }
            }
        }

        public Configuration()
        {
            this.InitializeComponent();
            DeviceType_SelectionChanged();
            //On Startup Update Room List Pane
            UpdateListBox(1);
        }

        private void btn_RoomAdd_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Shared.Classes.Room NewRoom = new Shared.Classes.Room();

            NewRoom.I2C_SlaveAdress = 0x40;
            NewRoom.Color = "";
            NewRoom.Name = "New Room";

            MainPage.MyHome.Rooms.Add(NewRoom);
            UpdateListBox(1);
        }

        private void btn_RoomRem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (lv_Rooms.Items.Count > 0 && lv_Rooms.SelectedIndex != -1)
            {
                MainPage.MyHome.Rooms.Remove((Shared.Classes.Room)lv_Rooms.SelectedItem);
                lv_Rooms.Items.Remove(lv_Rooms.SelectedItem);
            }
            UpdateListBox();
        }

        private void lv_Rooms_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (lv_Rooms.Items.Count > 0)
            {
                UpdateRoomDetailPane((Shared.Classes.Room)lv_Rooms.SelectedItem);
                UpdateDeviceDetailPane((Shared.Classes.Room)lv_Rooms.SelectedItem, true);
            }
        }

        private void btn_Save_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (lv_Rooms.Items.Count > 0 && lv_Rooms.SelectedIndex != -1)
            {
                // Update Room Detail
                Shared.Classes.Room SelectedRoom = (Shared.Classes.Room)lv_Rooms.SelectedItem;

                SelectedRoom.Name = tbx_RoomName.Text.ToString();

                int NewAddress = 0;
                IFormatProvider _Culture = new CultureInfo("en-US");
                int.TryParse(tbx_I2CAdress.Text.Replace("0x", ""), System.Globalization.NumberStyles.HexNumber, _Culture, out NewAddress);
                SelectedRoom.I2C_SlaveAdress = NewAddress;

                // Update Device Detail
                if (lv_Devices.Items.Count > 0)
                {
                    if (lv_Devices.SelectedIndex != -1)
                    {
                        System.Type type = lv_Devices.SelectedItem.GetType();
                        if (type == typeof(Shared.Classes.Device))
                        {
                            Shared.Classes.Device SelectedDevice = (Shared.Classes.Device)lv_Devices.SelectedItem;

                            SelectedDevice.Name = tbx_DeviceName.Text.ToString();
                            try
                            {
                                SelectedDevice.Pin = (Shared.Classes.Device.PinsEnum)Enum.Parse(typeof(Shared.Classes.Device.PinsEnum), fv_DevicePin.SelectedItem.ToString());
                            }
                            catch
                            {
                                SelectedDevice.Pin = (Shared.Classes.Device.PinsEnum)Enum.Parse(typeof(Shared.Classes.Device.PinsEnum), "D0");
                            }

                        }
                        else if (type == typeof(Shared.Classes.WindowShutter))
                        {
                            Shared.Classes.WindowShutter SelectedDevice = (Shared.Classes.WindowShutter)lv_Devices.SelectedItem;

                            SelectedDevice.Name = tbx_DeviceName.Text.ToString();
                            try
                            {
                                SelectedDevice.Pin = (Shared.Classes.Device.PinsEnum)Enum.Parse(typeof(Shared.Classes.Device.PinsEnum), fv_DevicePin.SelectedItem.ToString());
                                SelectedDevice.SecondaryPin = (Shared.Classes.Device.PinsEnum)Enum.Parse(typeof(Shared.Classes.Device.PinsEnum), fv_DevicePinSec.SelectedItem.ToString());
                            }
                            catch
                            {
                                SelectedDevice.Pin = (Shared.Classes.Device.PinsEnum)Enum.Parse(typeof(Shared.Classes.Device.PinsEnum), "D0");
                                SelectedDevice.SecondaryPin = (Shared.Classes.Device.PinsEnum)Enum.Parse(typeof(Shared.Classes.Device.PinsEnum), "D1");
                            }
                        }
                        else if (type == typeof(Shared.Classes.Sensor))
                        {
                            Shared.Classes.Sensor SelectedDevice = (Shared.Classes.Sensor)lv_Devices.SelectedItem;

                            SelectedDevice.Name = tbx_DeviceName.Text.ToString();
                            try
                            {
                                SelectedDevice.Pin = (Shared.Classes.Device.PinsEnum)Enum.Parse(typeof(Shared.Classes.Device.PinsEnum), fv_DevicePin.SelectedItem.ToString());
                            }
                            catch
                            {
                                SelectedDevice.Pin = (Shared.Classes.Device.PinsEnum)Enum.Parse(typeof(Shared.Classes.Device.PinsEnum), "A1");
                            }
                        }
                    }
                }
            }

            // Save
            Shared.Classes.Home.SaveHome(MainPage.MyHome);
        }

        private void UpdateRoomDetailPane(Shared.Classes.Room SelectedRoom)
        {
            tbx_RoomName.Text = SelectedRoom.Name;
            tbx_I2CAdress.Text = "0x" + SelectedRoom.I2C_SlaveAdress.ToString("X");
        }

        private void UpdateDeviceDetailPane(Shared.Classes.Room SelectedRoom, bool SelectFirst = false, bool SkipClear = false)
        {
            if (SkipClear == false)
            {
                ClearDeviceDetailPane();
            }
            if (SelectedRoom.Devices.Count > 0 || SelectedRoom.Sensors.Count > 0 || SelectedRoom.WindowShutters.Count > 0)
            {
                if (SkipClear == false)
                {
                    try
                    {
                        foreach (var _Device in SelectedRoom.Devices)
                        {
                            lv_Devices.Items.Add(_Device);
                        }
                    }
                    catch { }
                    try
                    {
                        foreach (var _Sensor in SelectedRoom.Sensors)
                        {
                            lv_Devices.Items.Add(_Sensor);
                        }
                    }
                    catch { }
                    try
                    {
                        foreach (var _WindowShutter in SelectedRoom.WindowShutters)
                        {
                            lv_Devices.Items.Add(_WindowShutter);
                        }
                    }
                    catch { }
                }

                if (SelectFirst)
                {
                    lv_Devices.SelectedIndex = 0;
                }

                var type = lv_Devices.SelectedItem.GetType();

                if (type == typeof(Shared.Classes.Device))
                {
                    Shared.Classes.Device SelectedDevice = (Shared.Classes.Device)lv_Devices.SelectedItem;
                    try
                    {
                        deviceType_Device();
                        tbx_DeviceName.Text = SelectedDevice.Name;
                        fv_DevicePin.SelectedItem = SelectedDevice.Pin.ToString();
                    }
                    catch
                    {
                        tbx_DeviceName.Text = "";
                        fv_DevicePin.SelectedItem = "D0";
                    }
                }
                else if (type == typeof(Shared.Classes.WindowShutter))
                {
                    Shared.Classes.WindowShutter SelectedDevice = (Shared.Classes.WindowShutter)lv_Devices.SelectedItem;
                    try
                    {
                        deviceType_WindowShutter();
                        tbx_DeviceName.Text = SelectedDevice.Name;
                        fv_DevicePin.SelectedItem = SelectedDevice.Pin.ToString();
                        fv_DevicePinSec.SelectedItem = SelectedDevice.SecondaryPin.ToString();
                    }
                    catch
                    {
                        tbx_DeviceName.Text = "";
                        fv_DevicePin.SelectedItem = "D0";
                        fv_DevicePinSec.SelectedItem = "D1";
                    }
                }
                else if (type == typeof(Shared.Classes.Sensor))
                {
                    Shared.Classes.Sensor SelectedDevice = (Shared.Classes.Sensor)lv_Devices.SelectedItem;
                    try
                    {
                        deviceType_Sensor();
                        tbx_DeviceName.Text = SelectedDevice.Name;
                        fv_DevicePin.SelectedItem = SelectedDevice.Pin.ToString();
                    }
                    catch
                    {
                        tbx_DeviceName.Text = "";
                        fv_DevicePin.SelectedItem = "A2";
                    }
                }
            }
        }

        private void ClearDeviceDetailPane()
        {
            if (lv_Devices.Items != null && lv_Devices.Items.Count > 0)
            {
                lv_Devices.Items.Clear();
            }
            tbx_DeviceName.Text = "";
        }

        private void btn_AddDevice_Tapped(object sender, TappedRoutedEventArgs e)
        {

            switch (deviceType)
            {
                case 0:
                    Shared.Classes.Device NewDevice = new Shared.Classes.Device();
                    NewDevice.Name = tbx_DeviceName.Text;
                    NewDevice.Pin = Shared.Classes.Device.PinsEnum.D0;
                    MainPage.MyHome.Rooms[lv_Rooms.SelectedIndex].AddDevice(NewDevice);
                    break;
                case 1:
                    Shared.Classes.WindowShutter NewWindowShutter = new Shared.Classes.WindowShutter();
                    NewWindowShutter.Name = tbx_DeviceName.Text;
                    NewWindowShutter.Pin = Shared.Classes.Device.PinsEnum.D0;
                    NewWindowShutter.SecondaryPin = Shared.Classes.Device.PinsEnum.D1;
                    MainPage.MyHome.Rooms[lv_Rooms.SelectedIndex].AddWindowShutter(NewWindowShutter);
                    break;
                case 2:
                    Shared.Classes.Sensor NewSensor = new Shared.Classes.Sensor();
                    NewSensor.Name = tbx_DeviceName.Text;
                    NewSensor.Pin = Shared.Classes.Sensor.PinsEnum.D0;
                    MainPage.MyHome.Rooms[lv_Rooms.SelectedIndex].AddSensor(NewSensor);
                    break;
            }

            // Only update device listbox
            UpdateListBox(2);
        }

        private void btn_RemoveDevice_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (lv_Devices.Items.Count > 0 && lv_Devices.SelectedIndex != -1)
            {
                if (lv_Devices.SelectedItem.GetType() == typeof(Shared.Classes.Device))
                {
                    MainPage.MyHome.Rooms[lv_Rooms.SelectedIndex].Devices.Remove((Shared.Classes.Device)lv_Devices.SelectedItem);
                    lv_Devices.Items.Remove(lv_Devices.SelectedItem);
                }
                else if (lv_Devices.SelectedItem.GetType() == typeof(Shared.Classes.WindowShutter))
                {
                    MainPage.MyHome.Rooms[lv_Rooms.SelectedIndex].WindowShutters.Remove((Shared.Classes.WindowShutter)lv_Devices.SelectedItem);
                    lv_Devices.Items.Remove(lv_Devices.SelectedItem);
                }
                else if (lv_Devices.SelectedItem.GetType() == typeof(Shared.Classes.Sensor))
                {
                    MainPage.MyHome.Rooms[lv_Rooms.SelectedIndex].Sensors.Remove((Shared.Classes.Sensor)lv_Devices.SelectedItem);
                    lv_Devices.Items.Remove(lv_Devices.SelectedItem);
                }
            }
            UpdateListBox(2);
            UpdateDeviceDetailPane((Shared.Classes.Room)lv_Rooms.SelectedItem, true, false);
        }

        private void DeviceType_SelectionChanged()
        {
            try
            {
                switch (deviceType)
                {
                    case 0: //Device Type "Device"
                        fv_DevicePinSec.Visibility = Visibility.Collapsed;
                        grd_DeviceDefinition.RowDefinitions[6].Height = new GridLength(0);
                        //cbx_DevicePin.Items.Clear();
                        addPins();
                        break;
                    case 1: //Device type Window Shutter
                        fv_DevicePin.Items.Clear();

                        fv_DevicePinSec.Visibility = Visibility.Visible;
                        grd_DeviceDefinition.RowDefinitions[6].Height = new GridLength(25);
                        //cbx_DevicePinSec.Items.Clear();
                        addPins();
                        break;
                    case 2: //Device Type Sensor
                        fv_DevicePinSec.Visibility = Visibility.Collapsed;
                        grd_DeviceDefinition.RowDefinitions[6].Height = new GridLength(0);
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
            }

        }

        private void lv_Devices_Tapped(object sender, TappedRoutedEventArgs e)
        {
            System.Type type = lv_Devices.SelectedItem.GetType();
            if (type == typeof(Shared.Classes.Device))
            {
                btn_DeviceType_Device_Tapped(sender, e);
            }
            else if (type == typeof(Shared.Classes.WindowShutter))
            {
                btn_DeviceType_WindowShutter_Tapped(sender, e);
            }
            else if (type == typeof(Shared.Classes.Sensor))
            {
                btn_DeviceType_Sensor_Tapped(sender, e);
            }

            addPins();
            UpdateDeviceDetailPane((Shared.Classes.Room)lv_Rooms.SelectedItem, false, true);
        }

        private void addPins()
        {
            fv_DevicePin.Items.Clear();
            switch (deviceType)
            {
                case 0:
                case 1:
                    fv_DevicePin.Items.Add("D0");
                    fv_DevicePin.Items.Add("D1");
                    fv_DevicePin.Items.Add("D3");
                    fv_DevicePin.Items.Add("D4");
                    fv_DevicePin.Items.Add("D5");
                    fv_DevicePin.Items.Add("D6");
                    fv_DevicePin.Items.Add("D7");
                    fv_DevicePin.Items.Add("D8");
                    fv_DevicePin.Items.Add("D9");
                    fv_DevicePin.Items.Add("D10");
                    fv_DevicePin.Items.Add("D11");
                    fv_DevicePin.Items.Add("D12");

                    if (deviceType== 1)
                    {
                        fv_DevicePinSec.Items.Clear();
                        fv_DevicePinSec.Items.Add("D0");
                        fv_DevicePinSec.Items.Add("D1");
                        fv_DevicePinSec.Items.Add("D3");
                        fv_DevicePinSec.Items.Add("D4");
                        fv_DevicePinSec.Items.Add("D5");
                        fv_DevicePinSec.Items.Add("D6");
                        fv_DevicePinSec.Items.Add("D7");
                        fv_DevicePinSec.Items.Add("D8");
                        fv_DevicePinSec.Items.Add("D9");
                        fv_DevicePinSec.Items.Add("D10");
                        fv_DevicePinSec.Items.Add("D11");
                        fv_DevicePinSec.Items.Add("D12");
                    }
                    break;
                case 2:
                    fv_DevicePin.Items.Add("A2");
                    fv_DevicePin.Items.Add("A3");
                    break;

            }
        }

        private void btn_DeviceType_Device_Tapped(object sender, TappedRoutedEventArgs e)
        {
            deviceType_Device();
        }

        private void deviceType_Device()
        {
            btn_DeviceType_Device.Background = (Brush)Application.Current.Resources["lcars-blue"];
            btn_DeviceType_WindowShutter.Background = (Brush)Application.Current.Resources["lcars-tan"];
            btn_DeviceType_Sensor.Background = (Brush)Application.Current.Resources["lcars-tan"];
            deviceType = 0;
            DeviceType_SelectionChanged();
        }

        private void btn_DeviceType_WindowShutter_Tapped(object sender, TappedRoutedEventArgs e)
        {
            deviceType_WindowShutter();
        }

        private void deviceType_WindowShutter()
        {
            btn_DeviceType_WindowShutter.Background = (Brush)Application.Current.Resources["lcars-blue"];
            btn_DeviceType_Device.Background = (Brush)Application.Current.Resources["lcars-tan"];
            btn_DeviceType_Sensor.Background = (Brush)Application.Current.Resources["lcars-tan"];
            deviceType = 1;
            DeviceType_SelectionChanged();
        }

        private void btn_DeviceType_Sensor_Tapped(object sender,  TappedRoutedEventArgs e)
        {
            deviceType_Sensor();
        }

        private void deviceType_Sensor()
        {
            btn_DeviceType_Sensor.Background = (Brush)Application.Current.Resources["lcars-blue"];
            btn_DeviceType_WindowShutter.Background = (Brush)Application.Current.Resources["lcars-tan"];
            btn_DeviceType_Device.Background = (Brush)Application.Current.Resources["lcars-tan"];
            deviceType = 2;
            DeviceType_SelectionChanged();
        }
    }
}
