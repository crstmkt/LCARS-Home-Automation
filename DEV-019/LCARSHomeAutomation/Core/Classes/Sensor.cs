using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCARSHomeAutomation.Core.Classes
{
    public class Sensor : Device
    {
        public enum AlertState
        {
            Active,
            Inactive,
            NotDefined
        }

        new public async Task<AlertState> TurnOn()
        {
            //Task_CollectSensorData.Start();

            return 0;
        }

        //Task Task_CollectSensorData = new Task(async () =>
        //{
        //    while (true)
        //    {
        //        var Response = await Core.Communication.I2C_Helper.WriteRead(I2C_Slave_Address, Core.Communication.I2C_Helper.Mode.Mode0);

        //        if (Response == true)
        //        {
        //            //Aufruf 
        //            //MainPage.alert_Start(null, null);
        //        }

        //        await Task.Delay(500);
        //    }
        //});


    }
}
