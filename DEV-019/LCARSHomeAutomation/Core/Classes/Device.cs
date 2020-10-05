using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LCARSHomeAutomation.Core.Classes
{
    [DataContract]
    public class Device
    {
        [DataMember]
        public int I2C_Slave_Address;

        public enum PinsEnum : byte
        {
            /// <summary>
            /// Also used for Serial RX 
            /// </summary>
            D0 = 0,
            /// <summary>
            /// Also used for Serial TX
            /// </summary>
            D1 = 1,
            /// <summary>
            /// Also provides PWM
            /// </summary>
            D3 = 3,
            D4 = 4,
            /// <summary>
            /// Also provides PWM
            /// </summary>
            D5 = 5,
            /// <summary>
            /// Also provides PWM
            /// </summary>
            D6 = 6,
            D7 = 7,
            D8 = 8,
            /// <summary>
            /// Also provides PWM
            /// </summary>
            D9 = 9,
            /// <summary>
            /// Also provides PWM
            /// </summary>
            D10 = 10,
            /// <summary>
            /// Also provides PWM
            /// </summary>
            D11 = 11,
            D12 = 12,
            A2 = 16,
            A3 = 17,
            NotDefined = 255
        }

        public enum StatusEnum
        {
            Off,
            On,
            NotAvailable
        }

        [DataMember]
        public ushort Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ImagePath { get; set; }

        [DataMember]
        public PinsEnum Pin { get; set; }

        public StatusEnum Status { get; set; }

        public async Task<byte> GetState()
        {
            // Return 0 if device does not have valid I2C_Slave_Address
            if (I2C_Slave_Address == 0)
            {
                return 0;
            }

            // Create mode 1 request and return appropriate byte from response
            var Response = Communication.I2C_Helper.WriteRead(I2C_Slave_Address, Communication.I2C_Helper.Mode.Mode1).Result;
            switch (Pin)
            {
                case PinsEnum.D0:
                    return Response[0];
                case PinsEnum.D1:
                    return Response[1];
                case PinsEnum.D3:
                    return Response[2];
                case PinsEnum.D4:
                    return Response[3];
                case PinsEnum.D5:
                    return Response[4];
                case PinsEnum.D6:
                    return Response[5];
                case PinsEnum.D7:
                    return Response[6];
                case PinsEnum.D8:
                    return Response[7];
                case PinsEnum.D9:
                    return Response[8];
                case PinsEnum.D10:
                    return Response[9];
                case PinsEnum.D11:
                    return Response[10];
                case PinsEnum.D12:
                    return Response[11];
                case PinsEnum.A2:
                    return Response[12];
                case PinsEnum.A3:
                    return Response[13];
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Turns on device
        /// </summary>
        /// <returns>Returns the status of device after operation</returns>
        public async Task<StatusEnum> TurnOn()
        {
            var Response = await Communication.I2C_Helper.WriteRead(I2C_Slave_Address, Communication.I2C_Helper.Mode.Mode2, (byte)Pin, 1);
            Status = StatusEnum.On;
            return Status;
        }

        /// <summary>
        /// Turns off device
        /// </summary>
        /// <returns>Returns the status of device after operation</returns>
        public async Task<StatusEnum> TurnOff()
        {
            var Response = await Communication.I2C_Helper.WriteRead(I2C_Slave_Address, Communication.I2C_Helper.Mode.Mode2, (byte)Pin, 0);
            Status = StatusEnum.Off;
            return Status;
        }

        /// <summary>
        /// Turns on device
        /// </summary>
        /// <returns>Returns the status of device after operation</returns>
        public async Task<StatusEnum> TurnOn(int SlaveAddress)
        {
            var Response = await Communication.I2C_Helper.WriteRead(SlaveAddress, Communication.I2C_Helper.Mode.Mode2, (byte)Pin, 1);
            Status = StatusEnum.On;
            return Status;
        }

        /// <summary>
        /// Turns off device
        /// </summary>
        /// <returns>Returns the status of device after operation</returns>
        public async Task<StatusEnum> TurnOff(int SlaveAddress)
        {
            var Response = await Communication.I2C_Helper.WriteRead(SlaveAddress, Communication.I2C_Helper.Mode.Mode2, (byte)Pin, 0);
            Status = StatusEnum.Off;
            return Status;
        }


    }
}
