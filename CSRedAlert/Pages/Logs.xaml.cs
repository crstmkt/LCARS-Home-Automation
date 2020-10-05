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
    public sealed partial class Logs : Page
    {
        public Logs()
        {
            this.InitializeComponent();

            //Start Task that updates Logs
            Task.Factory.StartNew(async () =>
            {
            while (true)
            {
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low,
                        () =>
                        {
                            tb_Logs.Text = String.Empty;
                            foreach (Core.Classes.Log LogEntry in MainPage.Logs)
                            {
                                tb_Logs.Text += String.Concat(LogEntry.Timestamp.ToString(), ": ");
                                tb_Logs.Text += String.Concat(LogEntry.Entry.ToString(), "\n");
                            }
                        });
                    await Task.Delay(500);
                }
            });
            
        }
    }
}
