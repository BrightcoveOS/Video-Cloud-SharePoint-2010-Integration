using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BrightcoveSDK.Media
{
	[DataContract]
	public class BCObject
	{
		public static DateTime DateFromUnix(string value) {
			double millisecs = double.Parse(value.ToString());
			double secs = millisecs / 1000;
			return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(secs);
		}

        public static string DateToUnix(DateTime value) {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());

            //return the total seconds (which is a UNIX timestamp)
            return Convert.ToString(span.TotalSeconds * 1000);
        }
	}

    public static class BCObjectExtensions {
        
        public static string ToUnixTime(this DateTime value) {
            return BCObject.DateToUnix(value);
        }
    }
}
