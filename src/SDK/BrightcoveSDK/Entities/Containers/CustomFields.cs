using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Web;
using System.Diagnostics;

namespace BrightcoveSDK.Entities.Containers
{

	[Serializable]
	public class CustomFields : ISerializable
	{
		public Dictionary<string, string> Values;
		public CustomFields() {
			Values = new Dictionary<string, string>();
		}
		protected CustomFields(SerializationInfo info, StreamingContext context) {
			Values = new Dictionary<string, string>();
			foreach (var entry in info) {
				Values.Add(entry.Name, entry.Value.ToString());
			}
		}
		public void GetObjectData(SerializationInfo info, StreamingContext context) {
			foreach (string key in Values.Keys) {
				info.AddValue(key, Values[key]);
			}
		}
	}  
}
