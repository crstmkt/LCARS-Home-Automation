using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CSRedAlert.Core.Communication;

namespace CSRedAlert.Core.Classes
{
    public class WindowShutter : Device
    {
        [DataMember]
        public PinsEnum SecondaryPin { get; set; }
        
        public void moveUp()
        {
            stop();
            I2C_Helper.WriteRead(I2C_Slave_Address, I2C_Helper.Mode.Mode2, (byte)Pin, 1);
        }

        public void moveDown()
        {
            stop();
            I2C_Helper.WriteRead(I2C_Slave_Address, I2C_Helper.Mode.Mode2, (byte)SecondaryPin, 1);
            I2C_Helper.WriteRead(I2C_Slave_Address, I2C_Helper.Mode.Mode2, (byte)Pin, 1);
        }

        public void stop()
        {
            I2C_Helper.WriteRead(I2C_Slave_Address, I2C_Helper.Mode.Mode2, (byte)Pin, 0);
            I2C_Helper.WriteRead(I2C_Slave_Address, I2C_Helper.Mode.Mode2, (byte)SecondaryPin, 0);
        }

    }
}
