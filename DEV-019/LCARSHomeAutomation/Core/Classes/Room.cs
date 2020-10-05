using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LCARSHomeAutomation.Core.Classes
{
    [DataContract]
    public class Room
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Color { get; set; }

        [DataMember]
        public int I2C_SlaveAdress { get; set; }

        [DataMember]
        public List<Device> Devices;

        [DataMember]
        public List<WindowShutter> WindowShutters;

        [DataMember]
        public List<Sensor> Sensors;

        public Room()
        {
            Devices = new List<Device>();
            WindowShutters = new List<WindowShutter>();
            Sensors = new List<Sensor>();
        }

        public void AddDevice(Device NewDevice)
        {
            NewDevice.Id = (ushort)Devices.Count;
            NewDevice.I2C_Slave_Address = I2C_SlaveAdress;
            Devices.Add(NewDevice);
        }

        public void AddSensor(Sensor NewSensor)
        {
            NewSensor.Id = (ushort)Sensors.Count;
            NewSensor.I2C_Slave_Address = I2C_SlaveAdress;
            Sensors.Add(NewSensor);
        }

        public void AddWindowShutter(WindowShutter NewWindowShutter)
        {
            NewWindowShutter.Id = (ushort)WindowShutters.Count;
            NewWindowShutter.I2C_Slave_Address = I2C_SlaveAdress;
            WindowShutters.Add(NewWindowShutter);
        }

        public void UpdateDevices()
        {
            foreach (var Device in Devices)
            {
                Device.I2C_Slave_Address = I2C_SlaveAdress;
            }
        }
    }
}
