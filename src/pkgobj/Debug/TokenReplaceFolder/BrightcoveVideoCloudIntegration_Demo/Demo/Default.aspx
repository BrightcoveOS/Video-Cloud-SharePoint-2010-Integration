<%@ Page language="C#" MasterPageFile="~masterurl/default.master"    Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage,Microsoft.SharePoint,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" meta:progid="SharePoint.WebPartPage.Document" meta:webpartpageexpansion="full"  %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Import Namespace="Microsoft.SharePoint" %> <%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	Home - Brightcove Integration Demo
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
	<WebPartPages:WebPartZone runat="server" title="loc:TitleBar" id="TitleBar" AllowLayoutChange="false" AllowPersonalization="false"><ZoneTemplate>
	<WebPartPages:TitleBarWebPart runat="server" AllowEdit="True" AllowConnect="True" ConnectionID="00000000-0000-0000-0000-000000000000" Title="Web Part Page Title Bar" IsIncluded="True" Dir="Default" IsVisible="True" AllowMinimize="False" ExportControlledProperties="True" ZoneID="TitleBar" ID="g_d3472c5f_0030_4da3_87ca_5fe8d708a39e" HeaderTitle="default" AllowClose="False" FrameState="Normal" ExportMode="All" AllowRemove="False" AllowHide="True" SuppressWebPartChrome="False" DetailLink="" ChromeType="None" HelpLink="" MissingAssembly="Cannot import this Web Part." PartImageSmall="" HelpMode="Modeless" FrameType="None" AllowZoneChange="True" PartOrder="2" Description="" PartImageLarge="" IsIncludedFilter="" __MarkupType="vsattributemarkup" __WebPartId="{D3472C5F-0030-4DA3-87CA-5FE8D708A39E}" WebPart="true" Height="" Width=""></WebPartPages:TitleBarWebPart>

</ZoneTemplate></WebPartPages:WebPartZone>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderTitleAreaClass" runat="server">
	<style type="text/css">
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
	<meta name="GENERATOR" content="Microsoft SharePoint" />
	<meta name="ProgId" content="SharePoint.WebPartPage.Document" />
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="CollaborationServer" content="SharePoint Team Web Site" />
	<link type="text/css" href="style1.css" rel="stylesheet" />
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
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderSearchArea" runat="server">
	<SharePoint:DelegateControl runat="server"
		ControlId="SmallSearchInputBox"/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderLeftActions" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageDescription" runat="server">
	<SharePoint:ProjectProperty Property="Description" runat="server"/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderBodyRightMargin" runat="server">
	<div height="100%" class="ms-pagemargin"><img src="/_layouts/images/blank.gif" width="10" height="1" alt="" /></div>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageImage" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderNavSpacer" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderLeftNavBar" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderMain" runat="server">


	<div id="contentContainerHome">
		<div id="contentDiv">
		<div id="top"><div id="logo"><a href="default.aspx"><img src="images/brightcove-0.jpg" border="0"></a></div><div class="title">
			Integration Demo</div></div>
			<div class="SIDescription">This site demonstrates the use of the 
				Brightcove Video Cloud API from SharePoint Server 2010. The Brightcove 
				API contains dozens of methods that allow you to upload and tag 
				videos, search and preview videos, publish videos to websites, 
				create and manage playlists, and perform other video-related 
				tasks. This site uses many of these API methods to demonstrate 
				four main features of the Brightcove Video Cloud service. Learn 
				more about these features by clicking on the links below.</div>
			<div id="upload">
				<div class="menuIcon">
					<a href="javascript:void OpenPopUpPage('VideoUpload.aspx',null,750,700);"><img src="images/upload.png" border="0" /></a>
				</div>
				<div class="menuText">
					<div class="menuTitle"><a href="javascript:void OpenPopUpPage('VideoUpload.aspx',null,750,700);">
						Upload</a></div>
					<div class="menuDesc">Upload a video and let Brightcove take 
						care of the encoding. Apply metadata tags and other 
						information.</div>
				</div>
			</div>
			<div id="search">
				<div class="menuIcon">
					<a href="SearchResults.aspx"><img src="images/search.png" border="0" /></a>
				</div>
				<div class="menuText">
					<div class="menuTitle"><a href="SearchResults.aspx">Search</a></div>
					<div class="menuDesc">Use the Brightcove Video Cloud&#39;s 
						advanced search to find videos by name, description and 
						custom tags.</div>
				</div>
			</div>
			<div id="articles">
				<div class="menuIcon">
					<a href="Articles.aspx"><img src="images/articles.png" border="0" /></a>		
				</div>
				<div class="menuText">
					<div class="menuTitle"><a href="Articles.aspx">Publish</a></div>
					<div class="menuDesc">The Brightcove SharePoint web parts 
						make it easy to add your videos to SharePoint blogs, 
						articles and site pages.</div>
				</div>
			</div>
			<div id="playlists">
				<div class="menuIcon">
					<a href="VideoPlaylist.aspx"><img src="images/playlist.png" border="0" /></a>
				</div>
				<div class="menuText">
					<div class="menuTitle"><a href="VideoPlaylist.aspx">
						Playlists</a></div>
					<div class="menuDesc">Create and manage video playlists. The 
						Brightcove API supports both manual and &quot;smart&quot; 
						playlists.</div>
				</div>
		</div>
		</div>
	</div>


</asp:Content>
