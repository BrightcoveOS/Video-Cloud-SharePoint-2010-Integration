using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Collections;
using System.Web;
using System.Collections.Specialized;
using Sitecore;
using Sitecore.Pipelines.ConvertToRuntimeHtml;
using HtmlAgilityPack;
using Sitecore.Pipelines.PreprocessRequest;

namespace BrightcoveSDK.SitecoreUtil.Pipelines
{
	public class GetRTEFieldValue
	{
		public void Process(Sitecore.Pipelines.RenderField.RenderFieldArgs args) {
		
			Sitecore.Diagnostics.Assert.ArgumentNotNull(args, "args");
			Sitecore.Diagnostics.Assert.ArgumentNotNull(args.FieldTypeKey, "args.FieldTypeKey");
			
			if (args.FieldTypeKey != "rich text"
			|| String.IsNullOrEmpty(args.FieldValue)
			|| Sitecore.Context.PageMode.IsPageEditorEditing
			|| !Sitecore.Configuration.Settings.HtmlEditor.SupportWebControls) {
				return;
			}
			this.TransformWebControls(args);
		}

		protected void TransformWebControls(Sitecore.Pipelines.RenderField.RenderFieldArgs args) {
			System.Web.UI.Page page = new System.Web.UI.Page();
			page.AppRelativeVirtualPath = "/";
			System.Web.UI.Control control = page.ParseControl(args.Result.FirstPart);
			args.Result.FirstPart = Sitecore.Web.HtmlUtil.RenderControl(control);
		}
	}
}