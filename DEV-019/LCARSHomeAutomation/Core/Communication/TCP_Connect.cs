using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.Diagnostics;

/*
 * This File is not needed anymore! Moved to Shared.Communication
 */

namespace LCARSHomeAutomation.Core.Communication
{
    public class TCP_Connect
    {
        public StreamSocket socket;
        HostName host;
        DataWriter writer;

        public TCP_Connect(HostName host)
        {
            this.socket = new StreamSocket();
            this.writer = new DataWriter(socket.OutputStream);
            this.host = host;
        }

        public async Task Connect()
        {
            try
            {
                await this.socket.ConnectAsync(this.host, "5463");
            }catch (Exception ex)
            {
                //Log Exception
                Debug.WriteLine(ex.Message);
            }
            
        }

        public async Task Send(string request)
        {
            try
            {
                this.writer.WriteUInt32(writer.MeasureString(request));
                this.writer.WriteString(request);

                await this.writer.StoreAsync();

                this.writer.DetachStream();
                
            }catch(Exception ex)
            {
                //Log Exception
                Debug.WriteLine(ex.Message);
            }
        }

        public void Disconnect()
        {
            this.writer.Dispose();
            this.socket.Dispose();
        }
    }
}
