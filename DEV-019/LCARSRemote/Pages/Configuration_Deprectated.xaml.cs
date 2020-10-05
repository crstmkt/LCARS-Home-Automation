using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
    public sealed partial class ConfigurationDeprecated : Page
    {
        public ConfigurationDeprecated()
        {
            this.InitializeComponent();

            Window.Current.SizeChanged += Current_SizeChanged;
            
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            double widthTop = sp_ContentArea.ActualWidth-55;
            double widthBot = widthTop - 110;

            if(widthTop <0)
            {
                widthTop = 0;
            }
            if (widthBot < 0)
            {
                widthBot = 0;
            }

            bor_Blue_Top.Width = widthTop;
            bor_Blue_Bot.Width = widthBot;
            bor_Blue_Bot.Margin = new Thickness { Left = 0, Top = 0, Right = 0, Bottom = 0 };
            btn_Save.BorderThickness = new Thickness { Left = 0, Top = 0, Right = 0, Bottom = 0 };

            Debug.WriteLine(Window.Current.Bounds.Width);

        }
    }
}
