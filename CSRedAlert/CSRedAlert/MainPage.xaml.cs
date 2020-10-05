using System;
using Windows.Devices.Gpio;
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

// Die Vorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 dokumentiert.

namespace CSRedAlert
{
    ///// <summary>
    ///// Eine leere Seite, die eigenständig verwendet werden kann oder auf die innerhalb eines Rahmens navigiert werden kann.
    ///// </summary>
    //public sealed partial class MainPage : Page
    //{

    //    private GpioPin sectionOnePin;
    //    private GpioPinValue pinValue;
    //    private SolidColorBrush redBrush = new SolidColorBrush(Windows.UI.Colors.Red);
    //    private SolidColorBrush grayBrush = new SolidColorBrush(Windows.UI.Colors.LightGray);
    //    private DispatcherTimer timer;

    //    public MainPage()
    //    {
    //        this.InitializeComponent();
            
    //        if (InitGPIO())
    //        {
    //            timer = new DispatcherTimer();
    //            timer.Interval = TimeSpan.FromMilliseconds(500);
    //            timer.Tick += Timer_Tick;
    //            timer.Start();
    //        }

    //    }

    //    private void Timer_Tick(object sender, object e)
    //    {
    //        if (pinValue != sectionOnePin.Read())
    //        {
    //            setAlert();
    //            pinValue = sectionOnePin.Read();
    //        }
    //    }
        

    //    private bool InitGPIO()
    //    {
    //        try {
    //            var gpio = GpioController.GetDefault();

    //            // Show an error if there is no GPIO controller
    //            if (gpio == null)
    //            {
    //                sectionOnePin = null;
    //                GpioStatus.Text = "There is no GPIO controller on this device.";
    //                return false;
    //            }

    //            sectionOnePin = gpio.OpenPin(5);
    //            sectionOnePin.SetDriveMode(GpioPinDriveMode.InputPullDown);

    //            pinValue = sectionOnePin.Read();
                
    //            GpioStatus.Text = "GPIO initialized correctly";
    //            setAlert();
    //            return true;
    //        } catch
    //        {
    //            GpioStatus.Text = "An Error occured during GPIO initilazation";
    //            return false;
    //        }
    //    }

    //    private void setAlert()
    //    {
    //        if (sectionOnePin.Read() == GpioPinValue.High)
    //        {
               
    //            AlertPlayer.Stop();
    //            AlertVideo.Stop();
    //            AlertVideo.IsFullWindow = false;
    //            AlertVideo.Visibility = Visibility.Collapsed;
    //            LED.Fill = grayBrush;
    //            GpioStatus.Text = "Everything is fine";
    //        }
    //        else
    //        {
    //            AlertPlayer.IsLooping = true;
    //            AlertPlayer.Play();
    //            AlertVideo.IsLooping = true;
    //            AlertVideo.Visibility = Visibility.Visible;
    //            AlertVideo.IsFullWindow = true;
    //            AlertVideo.Play();
    //            LED.Fill = redBrush;
    //            GpioStatus.Text = "Red Alert!";
    //        }
    //    }

    //    private void btn_Test_Click(object sender, RoutedEventArgs e)
    //    {
    //        AlertVideo.IsLooping = true;
    //        AlertVideo.Visibility = Visibility.Visible;
    //        AlertVideo.IsFullWindow = true;
    //        AlertVideo.Play();
    //    }
    //}
}
