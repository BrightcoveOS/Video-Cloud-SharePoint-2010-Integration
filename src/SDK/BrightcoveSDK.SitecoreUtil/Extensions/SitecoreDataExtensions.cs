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
	public enum UpdateType { NEW, UPDATE, BOTH };
	
	public static class SitecoreItemExtensions
	{
		/// <summary>
		/// This returns the first child it finds that has the required template or a null
		/// </summary>
		/// <param name="Parent">
		/// Parent
		/// </param>
		/// <param name="Templatename">
		/// this is the template name of the items you want
		/// </param>
		/// <returns>
		/// Returns the first item that matches the templatename or null
		/// </returns>
		public static Item ChildByTemplate(this Item Parent, string Templatename) {

			try {
				return (from child in Parent.GetChildren().ToArray() where child.TemplateName.Equals(Templatename) select child).First();
			} catch (Exception ex) {
				return null;
			}
		}

		/// <summary>
		/// This gets a child item that matches the template name and item name provided
		/// </summary>
		/// <param name="Parent">
		/// The parent to search under for a result
		/// </param>
		/// <param name="Templatename">
		/// The template name of the child to find
		/// </param>
		/// <param name="ItemName">
		/// The item name of the child to find
		/// </param>
		/// <returns>
		/// Returns and item that matches the criteria or null
		/// </returns>
		public static Item ChildByTemplateAndName(this Item Parent, string Templatename, string ItemName) {

			try {
				return (from child in Parent.GetChildren().ToArray() where (child.TemplateName.Equals(Templatename) && child.DisplayName.Equals(ItemName)) select child).First();
			} catch (Exception ex) {
				return null;
			}
		}

		/// <summary>
		/// This gets a child item that matches the template name or item name provided
		/// </summary>
		/// <param name="Parent">
		/// The parent to search under for a result
		/// </param>
		/// <param name="Templatename">
		/// The template name of the child to find
		/// </param>
		/// <param name="ItemName">
		/// The item name of the child to find
		/// </param>
		/// <returns>
		/// Returns and item that matches one of the criteria or null
		/// </returns>
		public static Item ChildByTemplateOrName(this Item Parent, string Templatename, string ItemName) {

			try {
				return (from child in Parent.GetChildren().ToArray() where (child.TemplateName.Equals(Templatename) || child.DisplayName.Equals(ItemName)) select child).First();
			} catch (Exception ex) {
				return null;
			}
		}

		/// <summary>
		/// this returns all the children who have a required template
		/// </summary>
		/// <param name="Parent">
		/// Parent Item
		/// </param>
		/// <param name="Templatename">
		/// this is the template name of the items that you want
		/// </param>
		/// <returns>
		/// Returns a list of items that match the template name
		/// </returns>
		public static List<Item> ChildrenByTemplate(this Item Parent, string Templatename) {
			List<string> types = new List<string>();
			types.Add(Templatename);
			return ChildrenByTemplates(Parent, types);
		}

		/// <summary>
		/// This returns a list of child items based on a list of templates names provided
		/// </summary>
		/// <param name="Parent">
		/// Parent Item to search for children
		/// </param>
		/// <param name="Templatenames">
		/// The list of template names to look for
		/// </param>
		/// <returns>
		/// Returns a list of items that match the templatenames provided
		/// </returns>
		public static List<Item> ChildrenByTemplates(this Item Parent, List<string> Templatenames) {

			return (from child in Parent.GetChildren().ToArray() where Templatenames.Contains(child.TemplateName) select child).ToList();
		}

		/// <summary>
		/// This will look for children of a specified templatename recursively. It will only recursively query under items that match the templatename.
		/// </summary>
		/// <param name="Parent">
		/// Parent item to search under
		/// </param>
		/// <param name="Templatename">
		/// Templatename of the items you want to return
		/// </param>
		/// <returns>
		/// Returns a list of Items that match the template name
		/// </returns>
		public static List<Item> ChildrenByTemplateRecursive(this Item Parent, string Templatename) {

			List<string> types = new List<string>();
			types.Add(Templatename);
			return ChildrenByTemplatesRecursive(Parent, types);
		}

		/// <summary>
		/// This will look for children of a specified templatenames recursively. It will only recursively query under items that match the templatenames.
		/// </summary>
		/// <param name="Parent">
		/// Parent item to search under
		/// </param>
		/// <param name="Templatenames">
		/// Templatenames of the items you want to return
		/// </param>
		/// <returns>
		/// Returns a list of items that match the templatenames provided
		/// </returns>
        public static List<Item> ChildrenByTemplatesRecursive(this Item Parent, List<string> Templatenames) {

			List<Item> list = new List<Item>();
			//get the first level of items
            Item[] children = Parent.GetChildren().ToArray();
            list.AddRange((from child in children where Templatenames.Contains(child.TemplateName) select child).ToList());
            
            //foreach item found look for children of it's type
			foreach (Item i in children) {
				//either way continue to search below it for values
				list.AddRange(i.ChildrenByTemplatesRecursive(Templatenames));
			}

			return list;
		}
	}
}
