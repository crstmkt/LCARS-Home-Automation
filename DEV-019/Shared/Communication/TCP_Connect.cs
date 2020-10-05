using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.Diagnostics;
using System.IO;

namespace Shared.Communication
{
    public class TCP_Connect
    {
        StreamSocket socket;
        HostName host;
        DataWriter Dwriter;
        StreamWriter Swriter;

        public TCP_Connect(HostName host)
        {
            this.socket = new StreamSocket();
            this.Swriter = new StreamWriter(socket.OutputStream.AsStreamForWrite());
            this.Dwriter = new DataWriter(socket.OutputStream);
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

        public async Task Send(Shared.Classes.Message request)
        {
            try
            {
                #region "Stream Reader / Writer"
                //Write data to the echo server.
                //Stream streamOut = socket.OutputStream.AsStreamForWrite();
                //this.Swriter = new StreamWriter(streamOut);
                //await Swriter.WriteAsync();
                //await Swriter.WriteAsync(request.Data, 0, request.Data.Length); //WriteLineAsync(request.Data);
                //await Swriter.WriteLineAsync();
                //await Swriter.FlushAsync();

                //Read data from the echo server.
                //Stream streamIn = socket.InputStream.AsStreamForRead();
                //StreamReader reader = new StreamReader(streamIn);
                //string response = await reader.ReadLineAsync();
                #endregion

                #region "Data Reader / Writer"
                //this.Dwriter.WriteBytes(request.Data);
                //await this.Dwriter.StoreAsync();
                //this.Dwriter.DetachStream();

                //DataReader reader = new DataReader(socket.InputStream);
                //uint stringLength = reader.ReadUInt32();
                //uint actualStringLength = await reader.LoadAsync(stringLength);
                //Debug.WriteLine(reader.ReadString(actualStringLength));
                #endregion

            }
            catch(Exception ex)
            {
                //Log Exception
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task Send(string request)
        {
            Stream streamOut = socket.OutputStream.AsStreamForWrite();
            this.Swriter = new StreamWriter(streamOut);
            await Swriter.WriteLineAsync(request);
            await Swriter.FlushAsync();
        }

        public async Task<String> Read()
        {
            //Read data from the echo server.
            Stream streamIn = socket.InputStream.AsStreamForRead();
            StreamReader reader = new StreamReader(streamIn);
            string response = await reader.ReadLineAsync();
            return response;
        }

        public void Disconnect()
        {
            //this.writer.Dispose();
            this.socket.Dispose();
        }
    }
}
