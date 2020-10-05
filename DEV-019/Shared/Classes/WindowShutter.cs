using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace Shared.Classes
{
    public class WindowShutter : Device
    {
        [DataMember]
        public PinsEnum SecondaryPin { get; set; }
        
        public async void moveUp()
        {
            //Hold all current operations to prevent damage on shutter engine
            Shared.Communication.TCP_Connect tcpcon = new Communication.TCP_Connect(new Windows.Networking.HostName("minwinpc"));
            await tcpcon.Connect();
            //stop(tcpcon);

            string PinRequest = String.Format("4${0}${1}${2}${3}",
                                                this.I2C_Slave_Address,
                                                Shared.Communication.I2C_Helper.Mode.Mode2,
                                                (byte)this.Pin,
                                                1);
            await tcpcon.Send(PinRequest);

            tcpcon.Disconnect();
        }

        public async void moveDown()
        {
            //Hold all current operations to prevent damage on shutter engine
            Shared.Communication.TCP_Connect tcpcon = new Communication.TCP_Connect(new Windows.Networking.HostName("minwinpc"));
            await tcpcon.Connect();
            stop(tcpcon);

            //Start the engine
            string PinRequest = String.Format("4${0}${1}${2}${3}",
                                                this.I2C_Slave_Address,
                                                Shared.Communication.I2C_Helper.Mode.Mode2,
                                                (byte)this.Pin,
                                                1);
            await tcpcon.Send(PinRequest);

            string SecPinRequest = String.Format("4${0}${1}${2}${3}",
                                                this.I2C_Slave_Address,
                                                Shared.Communication.I2C_Helper.Mode.Mode2,
                                                (byte)this.SecondaryPin,
                                                1);
            await tcpcon.Send(SecPinRequest);

            tcpcon.Disconnect();

        }

        public async void stop()
        {
            //Hold all current operations to prevent damage on shutter engine
            Shared.Communication.TCP_Connect tcpcon = new Communication.TCP_Connect(new Windows.Networking.HostName("minwinpc"));
            await tcpcon.Connect();
            stop(tcpcon);
            tcpcon.Disconnect();
        }

        public async void stop(Shared.Communication.TCP_Connect tcpcon)
        {
            string PinRequest = String.Format("4${0}${1}${2}${3}",
                                                this.I2C_Slave_Address,
                                                Shared.Communication.I2C_Helper.Mode.Mode2,
                                                (byte)this.Pin,
                                                0);
            await tcpcon.Send(PinRequest);

            string SecPinRequest = String.Format("4${0}${1}${2}${3}",
                                                this.I2C_Slave_Address,
                                                Shared.Communication.I2C_Helper.Mode.Mode2,
                                                (byte)this.SecondaryPin,
                                                0);
            await tcpcon.Send(SecPinRequest);
        }

    }
}
