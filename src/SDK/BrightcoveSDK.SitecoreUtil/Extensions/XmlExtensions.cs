using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace BrightcoveSDK.SitecoreUtil.Extensions
{
	public static class XmlNodeExtensions
	{
		public static string GetAttribute(this XmlNode x, string attribute) {
			return (x.Attributes[attribute] != null) ? x.Attributes[attribute].Value : "";
		}
	}
}
