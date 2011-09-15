using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web;

namespace BrightcoveSDK.Extensions
{
	public static class NameValueCollectionExtensions {
		public static bool HasKey(this NameValueCollection QString, string Key) {

			foreach (string key in QString.Keys) {
				if (key.Equals(Key)) {
					return true;
				}
			}

			return false;
		}

		public static string ToQueryString(this NameValueCollection QString) {
			return ToQueryString(QString, new NameValueCollection());
		}

		public static string ToQueryString(this NameValueCollection QString, NameValueCollection OverrideKeys) {
			StringBuilder sb = new StringBuilder();

			string append = "?";
			foreach (string key in OverrideKeys.Keys) {
				if (!OverrideKeys[key].ToString().Equals("")) {
					sb.Append(append + key + "=" + HttpUtility.UrlEncode(OverrideKeys[key].ToString()));
					append = "&";
				}
			}

			foreach (string key in QString.Keys) {
				if (!OverrideKeys.HasKey(key) && !QString[key].ToString().Equals("")) {
					sb.Append(append + key + "=" + HttpUtility.UrlEncode(QString[key].ToString()));
				}
				append = "&";
			}

			return sb.ToString();
		}

	}
}
