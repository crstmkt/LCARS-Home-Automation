using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CSRedAlert.Core.Classes
{
    [DataContract]
    public class Log
    {
        [DataMember]
        public DateTime Timestamp { get; set; }

        [DataMember]
        public String Entry { get; set; }

        public Log()
        {
            string entry = "No logentry was given";
            new Log(entry);
        }

        public Log(String LogEntry)
        {
            Timestamp = DateTime.Now;
            Entry = LogEntry;
        }

        public static async void SaveLog(List<Log> NewLog)
        {
            MemoryStream _MemoryStream = new MemoryStream();
            DataContractSerializer Serializer = new DataContractSerializer(typeof(Log));
            Serializer.WriteObject(_MemoryStream, NewLog);

            StorageFile _File = await ApplicationData.Current.LocalFolder.CreateFileAsync("Logs.bin", CreationCollisionOption.ReplaceExisting);

            using (Stream fileStream = await _File.OpenStreamForWriteAsync()) 
            {
                _MemoryStream.Seek(0, SeekOrigin.Begin);
                await _MemoryStream.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                fileStream.Dispose();
            }
        }

        public static async Task<List<Log>> LoadLog()
        {
            StorageFolder _Folder = ApplicationData.Current.LocalFolder;
            StorageFile _File;

            try
            {
                _File = await _Folder.GetFileAsync("Logs.bin");

                Stream stream = await _File.OpenStreamForReadAsync();

                DataContractSerializer Serializer = new DataContractSerializer(typeof(Log));

                return (List<Log>)Serializer.ReadObject(stream);
            }
            catch
            {
                return new List<Log>();
            }
        }

    }
}
