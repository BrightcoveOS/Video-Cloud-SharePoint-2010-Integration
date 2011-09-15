using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BrightcoveSDK.Containers
{
	[CollectionDataContract]
	public class BCCollection<T> : List<T>
	{}

	public static class BCCollectionExtensions {
		
		public static string ToDelimitedString(this BCCollection<string> list, string Delimiter) {

			string r = "";
			foreach (string s in list) {
				if (r.Length > 0) {
					r += Delimiter;
				}
				r += s;
			}
			return r;
		}
	}
}
