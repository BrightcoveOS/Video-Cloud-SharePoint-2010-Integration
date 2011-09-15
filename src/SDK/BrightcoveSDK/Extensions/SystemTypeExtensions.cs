using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrightcoveSDK.Extensions
{
	public static class ListExtensions
	{
		public static string ToDelimString(this List<long> Values, string Delimiter) {

			StringBuilder sb = new StringBuilder();

			foreach (long s in Values) {
				if (sb.Length > 0) {
					sb.Append(Delimiter);
				}
				sb.Append(s.ToString());
			}

			return sb.ToString();
		}

        public static string ToFieldString(this List<VideoFields> fields) {
            StringBuilder sb = new StringBuilder();
            foreach (VideoFields v in fields) {
                if (sb.Length > 0) {
                    sb.Append(",");
                }
                sb.Append(v.ToString());
            }
            return sb.ToString();
        }

        public static string ToFieldString(this List<PlaylistFields> fields) {
            StringBuilder sb = new StringBuilder();
            foreach (VideoFields v in fields) {
                if (sb.Length > 0) {
                    sb.Append(",");
                }
                sb.Append(v.ToString());
            }
            return sb.ToString();
        }
    }
    
    public static class DictionaryExtensions
    {
        public static string DicToString(this Dictionary<VideoFields, string> dic){

            StringBuilder sb = new StringBuilder();
            foreach(KeyValuePair<VideoFields, string> row in dic){
                if (sb.Length > 0) {
                    sb.Append(",");
                }
                sb.Append(row.Key.ToString() + ":" + row.Value);
            }
            return sb.ToString();
        }
    }
}
