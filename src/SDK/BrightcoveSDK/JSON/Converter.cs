using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BrightcoveSDK.JSON
{
	public class Converter
	{
		public static string Serialize<T>(T obj) {
			System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
			MemoryStream ms = new MemoryStream();
			serializer.WriteObject(ms, obj);
			string retVal = Encoding.Default.GetString(ms.ToArray());
			return retVal;
		}

		public static T Deserialize<T>(string json) {
			T obj = Activator.CreateInstance<T>();
			MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
			System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
			obj = (T)serializer.ReadObject(ms);
			ms.Close();
			return obj;
		}
	}
}
