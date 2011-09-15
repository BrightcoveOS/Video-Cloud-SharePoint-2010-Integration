using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrightcoveSDK.Media;
using Sitecore.Data.Items;

namespace BrightcoveSDK.SitecoreUtil.Entity.Container
{
	public class UpdateInsertPair<T>
	{
		public List<T> NewItems = new List<T>();
		public List<T> UpdatedItems = new List<T>();
	}
}
