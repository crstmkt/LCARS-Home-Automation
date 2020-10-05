using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LCARSHomeAutomation.Core.Communication
{
    public class TCP_Helper
    {
        public Object obj = new Object();
        private Stream outStream;

        public TCP_Helper(Stream outStream)
        {
            this.outStream = outStream;
        }

        public async void Interpret(string request)
        {
            String[] elm = request.Split('$');

            int anzElm = int.Parse(elm[0]);

            for(int i = 1; i<=anzElm; i++)
            {
                MainPage.Logs.Add(new Shared.Classes.Log(elm[i]));
            }

            switch (int.Parse(elm[0]))
            {
                case 4:
                    //Define Param I2C Slave Adress
                    int I2C_SlaveAdress = int.Parse(elm[1]);
                    //Define Param Mode
                    Shared.Communication.I2C_Helper.Mode mode = new Shared.Communication.I2C_Helper.Mode();
                    switch (elm[2])
                    {
                        case "Mode0":
                            mode = Shared.Communication.I2C_Helper.Mode.Mode0;
                            break;
                        case "Mode1":
                            mode = Shared.Communication.I2C_Helper.Mode.Mode1;
                            break;
                        case "Mode2":
                            mode = Shared.Communication.I2C_Helper.Mode.Mode2;
                            break;
                    }
                    //Define Param Pin
                    byte Pin = byte.Parse(elm[3]);

                    //Define Param Value
                    byte PinValue = byte.Parse(elm[4]);

                    await Shared.Communication.I2C_Helper.WriteRead(I2C_SlaveAdress, mode, Pin, PinValue);
                    break;
            }
        }

        private async void getHome()
        {
            //this.obj = MainPage.MyHome;

            //Shared.Classes.Message message = Shared.Classes.Serializer.Serialize(this.obj);

            //StreamWriter writer = new StreamWriter(outStream);
            ////await writer.
            //await writer.WriteLineAsync(message.Data.ToString());
            //Debug.WriteLine(message.Data.ToString());
            //await writer.FlushAsync();

            //return new Shared.Classes.Home();
        }
    }
}
//http://stackoverflow.com/questions/15012549/send-typed-objects-through-tcp-or-sockets