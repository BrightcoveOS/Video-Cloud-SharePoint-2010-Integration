using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrightcoveSDK.Media;
using Sitecore.Data.Items;
using System.Collections;
using System.Web;
using System.Text.RegularExpressions;

namespace BrightcoveSDK.SitecoreUtil.Extensions
{
	public static class StringExtensions
	{

		public static string StripInvalidChars(this string val) {
			val = val.Replace(" ", "_");
			val = val.Replace("+", "");
			val = val.Replace("!", "");
			val = val.Replace("@", "");
			val = val.Replace("#", "");
			val = val.Replace("$", "");
			val = val.Replace("%", "");
			val = val.Replace("^", "");
			val = val.Replace("*", "");
			val = val.Replace("=", "");
			val = val.Replace("<", "");
			val = val.Replace(">", "");
			val = val.Replace("&", "_");
			val = val.Replace(",", "_");
			val = val.Replace("/", "_");
			val = val.Replace(@"\", "");
			val = val.Replace("|", "");
			val = val.Replace(";", "");
			val = val.Replace(":", "_");
			val = val.Replace("\"", "");
			val = val.Replace("’", "");
			val = val.Replace("é", "e");
			val = val.Replace("(", "");
			val = val.Replace(")", "");
			val = val.Replace("]", "");
			val = val.Replace("[", "");
			val = val.Replace("}", "");
			val = val.Replace("{", "");
			val = val.Replace("?", "");
			val = val.Replace("'", string.Empty);
			val = val.Replace(".", string.Empty);
			val = val.Replace("–", "_");


			//Cleanup double underscores
			val = val.Replace("__", "_");

			//Remove all underscores
			val = val.Replace("_", "");

			return val.Trim();
		}
	}

	public static class DateTimeExtensions
	{
		/// <summary>
		/// Gets a DateField from a DateTime
		/// </summary>
		/// <param name="Date">The current date field</param>
		/// <returns></returns>
		public static string ToDateFieldValue(this DateTime Date) {

			return Date.ToString("yyyyMMddTHHmmss");
		}
	}
}
