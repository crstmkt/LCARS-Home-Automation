using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using System.Collections.Generic;
using Windows.ApplicationModel.Core;
using Windows.Storage.Streams;


// Die Vorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 dokumentiert.

namespace LCARSHomeAutomation
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static Shared.Classes.Home MyHome;
        public static List<Shared.Classes.Log> Logs = new List<Shared.Classes.Log>();

        public MainPage()
        {
            this.InitializeComponent();

            //Load Home Object
            try
            {
                MyHome = Shared.Classes.Home.LoadHome().Result;
                Logs.Add(new Shared.Classes.Log("Home Loaded."));
            }
            catch (Exception ex)
            {
                Logs.Add(new Shared.Classes.Log(ex.Message));
            }
            //Establish Networking
            try
            {
                EstablishNetworking();
                Logs.Add(new Shared.Classes.Log("Server Running"));
            }
            catch (Exception ex)
            {
                Logs.Add(new Shared.Classes.Log(ex.Message));
            }
            //Start Task that updates Logs (OPTIMIZE)
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low,
                        () =>
                        {
                            //Has to be optimized
                            txb_Events.Text = String.Empty;
                            foreach (Shared.Classes.Log LogEntry in MainPage.Logs)
                            {
                                if (LogEntry.Entry != null)
                                {
                                    txb_Events.Text += String.Concat(LogEntry.Timestamp.ToString(), ": ");
                                    txb_Events.Text += String.Concat(LogEntry.Entry.ToString(), "\n");
                                }
                            }
                        });
                    await Task.Delay(500);
                }
            });

        }

        private async void EstablishNetworking()
        {
            await StartListener();
        }

        public async Task StartListener()
        {
            StreamSocketListener listener = new StreamSocketListener();
            listener.ConnectionReceived += OnConnection;
            listener.Control.KeepAlive = false;

            try
            {
                await listener.BindServiceNameAsync("5463");
                //Well, I don't have any Idea why but THIS seems to fix the problem...
                CoreApplication.Properties.Add("listener", listener);
            }
            catch (Exception ex)
            {
                if (SocketError.GetStatus(ex.HResult) == SocketErrorStatus.Unknown)
                {
                    throw;
                }
                //Logs.Add(ex.Message);
                Debug.WriteLine(ex.Message);
            }

        }

        private async void OnConnection(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            #region "Stream Reader / Writer"
            //Read line from the remote client.
            Stream inStream = args.Socket.InputStream.AsStreamForRead();
            StreamReader reader = new StreamReader(inStream);
            string request = await reader.ReadLineAsync();

            Core.Communication.TCP_Helper helper = new Core.Communication.TCP_Helper(args.Socket.OutputStream.AsStreamForWrite());
            helper.Interpret(request);

            ////reader.ReadAsync(request);

            ////Send the line back to the remote client.
            //Stream outStream = args.Socket.OutputStream.AsStreamForWrite();
            //StreamWriter writer = new StreamWriter(outStream);
            //await writer.WriteLineAsync(request);
            //await writer.FlushAsync();
            #endregion

            #region "Data Reader / Writer"
            //DataReader reader = new DataReader(args.Socket.InputStream);
            //uint sizeFieldCount = await reader.LoadAsync(sizeof(uint));
            //if (sizeFieldCount != sizeof(uint))
            //{
            //    return;
            //}

            //uint stringLength = reader.ReadUInt32();
            //uint actualStringLength = await reader.LoadAsync(stringLength);
            #endregion

            #region "UI Update Code"
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                // Your UI update code goes here!
                //Logs.Add(new Shared.Classes.Log(request)); //StreamReader
                //Logs.Add(new Shared.Classes.Log(reader.ReadBytes())); //DataReader
                //Logs.Add(new Shared.Classes.Log(reader.ReadString(actualStringLength)));
            });
            #endregion

        }

        private void btn_Geofencing_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            this.frm_ContentFrame.Navigate(typeof(Pages.Geofencing), null);
        }

        private async void btn_Send_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Shared.Communication.TCP_Connect tcpcon = new Shared.Communication.TCP_Connect(new HostName("localhost"));
            await tcpcon.Connect();
            //await tcpcon.Send(message);
            await tcpcon.Send("4$64$2$7$1");
        }
    }
}

//https://msdn.microsoft.com/de-de/windows/uwp/networking/sockets
//http://www.codeproject.com/Articles/1079341/Simple-Connection-between-Raspberry-Pi-with-Wind
//https://msdn.microsoft.com/en-us/windows/uwp/networking/sockets
//http://stackoverflow.com/questions/15012549/send-typed-objects-through-tcp-or-sockets