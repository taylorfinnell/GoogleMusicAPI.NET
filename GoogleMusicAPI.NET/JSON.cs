using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace GoogleMusicAPI
{
    public class JSON
    {
        public static T DeserializeObject<T>(String data)
        {
            return Deserialize<T>(data);
        }

        public static T Deserialize<T>(String data)
        {
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(data));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            return (T)serializer.ReadObject(ms);
        }
    }
}
