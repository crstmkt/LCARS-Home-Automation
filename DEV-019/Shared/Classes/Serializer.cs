using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Classes
{
    public static class Serializer
    {

        public static Message Serialize(object anySerializableObject)
        {
            DataContractSerializer DCSerializer = new DataContractSerializer(typeof(Home));

            if (anySerializableObject == typeof(Home))
            {
                DCSerializer = new DataContractSerializer(typeof(Home));
            }
            else if (anySerializableObject == typeof(string))
            {
                DCSerializer = new DataContractSerializer(typeof(string));
            }
            using (var memoryStream = new MemoryStream())
            {
                DCSerializer.WriteObject(memoryStream, anySerializableObject);
                return new Message { Data = memoryStream.ToArray() };
                //return new Message { MemStream = memoryStream };
            }
        }

        //public static object Deserialize(Message message)
        //{
        //    using (var memoryStream = new MemoryStream(message.Data))
        //        return (new BinaryFormatter()).Deserialize(memoryStream);
        //}

    }
}
