<%@ Register TagPrefix="WpNs0" Namespace="BrightcoveVideoCloudIntegration.VideoSearch" Assembly="BrightcoveVideoCloudIntegration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6a792aa6dfad51a4"%>
<%@ Page language="C#" MasterPageFile="~masterurl/default.master"    Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage,Microsoft.SharePoint,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" meta:progid="SharePoint.WebPartPage.Document" meta:webpartpageexpansion="full"  %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ID="Content1" ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	Search - Brightcove Integration Demo
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
										<h2>
                                        </h2>
										<WebPartPages:SPProxyWebPartManager runat="server" ID="__ProxyWebPartManagerForConnections__"></WebPartPages:SPProxyWebPartManager>
										<WebPartPages:WebPartZone runat="server" title="loc:TitleBar" id="TitleBar" AllowLayoutChange="false" AllowPersonalization="false"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderId="PlaceHolderTitleAreaClass" runat="server">
	<style type="text/css">
  </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
	<meta name="GENERATOR" content="Microsoft SharePoint" />
	<meta name="ProgId" content="SharePoint.WebPartPage.Document" />
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="CollaborationServer" content="SharePoint Team Web Site" />
	<script type="text/javascript">
// <![CDATA[
	    var navBarHelpOverrideKey = "WSSEndUser";
// ]]>
	</script>
	<SharePoint:UIVersionedContent ID="WebPartPageHideQLStyles" UIVersion="4" runat="server">
		<ContentTemplate>
<style type="text/css">

</style>
		</ContentTemplate>
	</SharePoint:UIVersionedContent>
    <link rel="stylesheet" type="text/css" href="style1.css" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderId="PlaceHolderSearchArea" runat="server">
	<SharePoint:DelegateControl ID="DelegateControl1" runat="server"
		ControlId="SmallSearchInputBox"/>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderId="PlaceHolderLeftActions" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderId="PlaceHolderPageDescription" runat="server">
	<SharePoint:ProjectProperty ID="ProjectProperty1" Property="Description" runat="server"/>
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderId="PlaceHolderBodyRightMargin" runat="server">
	<div height="100%" class="ms-pagemargin"><img src="/_layouts/images/blank.gif" width="10" height="1" alt="" /></div>
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderId="PlaceHolderPageImage" runat="server"></asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderId="PlaceHolderNavSpacer" runat="server"></asp:Content>
<asp:Content ID="Content11" ContentPlaceHolderId="PlaceHolderLeftNavBar" runat="server"></asp:Content>
<asp:Content ID="Content12" ContentPlaceHolderId="PlaceHolderMain" runat="server">
<div id="contentContainer">
	<div id="contentDiv">
		<div id="top"><div id="logo"><a href="Default.aspx"><img src="images/brightcove-0.jpg" border="0" /></a></div><div class="title">
			Integration Demo:<span class="pgName">Search</span></div></div>

		<table cellpadding="4" cellspacing="0" border="0" width="100%">
				<tr>
					<td id="_invisibleIfEmpty" name="_invisibleIfEmpty" colspan="3" valign="top" width="100%"> 
					<WebPartPages:WebPartZone runat="server" Title="loc:Header" ID="Header" FrameType="TitleBarOnly"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone> </td>
				</tr>
				<tr>
					<td id="_invisibleIfEmpty" name="_invisibleIfEmpty" valign="top" height="100%"> 
					<WebPartPages:WebPartZone runat="server" Title="loc:LeftColumn" ID="LeftColumn" FrameType="TitleBarOnly"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone> </td>
					<td id="_invisibleIfEmpty" name="_invisibleIfEmpty" valign="top" height="100%"> 
					<WebPartPages:WebPartZone runat="server" Title="loc:MiddleColumn" ID="MiddleColumn" FrameType="TitleBarOnly"><ZoneTemplate>
					<WpNs0:VideoSearch runat="server" AllowEdit="True" AllowConnect="True" ConnectionID="00000000-0000-0000-0000-000000000000" Title="Brightcove Video Cloud Search" Dir="Default" IsVisible="True" AllowMinimize="True" ExportControlledProperties="True" EditVideoLink="javascript:void OpenPopUpPage('./VideoEditor.aspx?videoid={0}',null,730,780)" FrameState="Normal" CustomFields="" ExportMode="All" AllowHide="True" SuppressWebPartChrome="False" PlayVideoLink="javascript:void OpenPopUpPage('./VideoPlayer.aspx?videoid={0}&amp;autostart=true',null,675,420)" DetailLink="" ChromeType="None" HelpLink="" ID="g_54b48728_c8dd_44a3_aaf1_a330d39f95a9" MissingAssembly="Cannot import this Web Part." PartImageSmall="" AllowRemove="True" HelpMode="Modeless" FrameType="None" AllowZoneChange="True" Description="" PartImageLarge="" IsIncludedFilter="" __MarkupType="vsattributemarkup" __WebPartId="{54B48728-C8DD-44A3-AAF1-A330D39F95A9}" WebPart="true" Height="" Width="" partorder="2"></WpNs0:VideoSearch>

</ZoneTemplate></WebPartPages:WebPartZone> </td>
					<td id="_invisibleIfEmpty" name="_invisibleIfEmpty" valign="top" height="100%"> 
					<WebPartPages:WebPartZone runat="server" Title="loc:RightColumn" ID="RightColumn" FrameType="TitleBarOnly"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone> </td>
				</tr>
				<tr>
					<td id="_invisibleIfEmpty" name="_invisibleIfEmpty" colspan="3" valign="top" width="100%"> 
					<WebPartPages:WebPartZone runat="server" Title="loc:Footer" ID="Footer" FrameType="TitleBarOnly"><ZoneTemplate></ZoneTemplate></WebPartPages:WebPartZone> </td>
				</tr>
				<script type="text/javascript" language="javascript">				    if (typeof (MSOLayout_MakeInvisibleIfEmpty) == "function") { MSOLayout_MakeInvisibleIfEmpty(); }</script>
		</table>
	</div>
</div>
</asp:Content>
