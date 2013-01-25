<%@ Assembly Name="BrightcoveVideoCloudIntegration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6a792aa6dfad51a4" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Chooser.aspx.cs" Inherits="BrightcoveVideoCloudIntegration.Layouts.BrightcoveVideoCloudIntegration.Chooser" DynamicMasterPageFile="~masterurl/default.master" %>
<%@ Register TagPrefix="WpNs0" Namespace="BrightcoveVideoCloudIntegration.VideoPicklist" Assembly="BrightcoveVideoCloudIntegration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6a792aa6dfad51a4"%>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
<style type="text/css">
body {
 font-family: Verdana,Arial,Helvetica,sans-serif;
 color: #3B3B3B;
 font-size: 8pt;
 background-color: transparent;
}

.result
{
    border-top: 1px solid #ADADAD;
    clear: both;
    height: auto;
    overflow: hidden;
    padding-bottom: 8px;
    padding-top: 10px;
    width: 600px;
}

.result A.videoLink
{
    display: block;
    font-weight: bold;
    width: 82%;
}

.result .description
{
	margin: 0 50px 0 93px;
	padding: 5px 0 0 0;
	width: 400px;
}

.result .tags
{
	clear: both;
	padding: 15px 0 0 93px;
	font-weight:bold;
	width: 400px;
	word-wrap:break-word;
}

.result .thumbnail
{
    float: left;
    height: 100%;
    padding: 0 20px 0 10px;
    width: 64px;
}

.result .thumbnail > IMG
{
	BORDER-BOTTOM: medium none; BORDER-LEFT: medium none; MAX-WIDTH: 64px; MAX-HEIGHT: 64px; BORDER-TOP: medium none; BORDER-RIGHT: medium none;
}

.blueBG 
{
    background: #F9F9F9;
}

/* Picklist styles */
.picklist, .picklistControls
{
    float:left;
    vertical-align:middle;
}

.picklist .content
{
    width:208px;
    height:270px;
}

.picklist .content
{
    /*width:208px;*/
    width:250px;
    height:250px;
    overflow-x:hidden;
    overflow-y:scroll;
	border:1px solid silver;
}

.picklist .videosAvailable
{
	/*height:250px;*/
    overflow-x:hidden;
}
    
.picklist .content .item
{
    /*width:195px;*/
    width:237px;
    height:35px;
    vertical-align:middle;
    clear:both;
    border-bottom:1px solid silver;
    padding-top:5px;
    padding-bottom:5px;
    cursor:pointer;
}
    
.videoName
{
    font-weight:bold;
    color:#000000;
}
    
.videoId
{
    color:Gray;
}

.videoCheckbox
{
    width:25px;
    float:left;
    display:none;
}
    
.videoThumb
{
    width:40px;
    float:left;
    margin-right:5px;
    margin-left:5px;
}

.videoMetadata
{
    width:167px;
    float:left;
    text-wrap:none;
    overflow-x:hidden;
}

.videoMetadata .videoName
{
    display:inline-block;
    width:167px;
    height:15px;
    text-wrap:none;
    overflow-x:hidden;
}
    
.videoThumb img
{
    max-width:40px;
    max-height:30px;
}

.picklistControls
{
	margin-top:80px;
	width:100px;
	text-align:center;
}


.picklistControls button
{
    margin-bottom:10px;
    width:80px;
    padding-left:2px;
    padding-right:2px;
}

.picklistControls .controlSection
{
	margin-bottom:8px;
}

.searchBox
{
    margin-bottom:5px;
}

.searchBox input
{
	/*width:181px;*/
	width:223px;
}

.searchBox button
{
	width:25px;
	background-image:url(/_layouts/images/gosearch15.png);
}

.searchBox button, .searchBox input
{
	margin:0 0 0 0;
	border:none;
	border:1px solid silver;
}

.searchBox_default_text
{
	font-style:italic;
	color:silver;
}

.videoSelected
{
	background-color:#8bd1ee;
}

#playlistName, #playlistType, #tags, #videos
{
    width:200px;
    font-size:12px;
    font-family: Verdana,Arial,Helvetica,sans-serif;
}

#playlistName, #tags, #videos
{
    height:16px;
}

#playlistType
{
    height:20px;
}

#divPicklist
{
    margin-top:5px;
    height:310px;
}

.picklist .moveButtons input
{
    vertical-align:middle;
}

.picklist .moveButtons
{
    width:208px;
    height:16px;
    clear:both;
    color:#000000;
    vertical-align:middle;
}

.picklist .moveButtons > input
{
    margin-left:1px;
    float:left;
}

.picklist .moveButtons > span
{
    float:right;
}

.picklist .moveButtons label
{
    margin-top:2px;
    float:left;
    font-weight:normal;
    color:gray;
}

.picklist .moveButtons button
{
    width:50px;
    height:20px;
    font-size:11px;
    padding-top:0;
}

.buttonBlock
{
    /*display:none;*/
}

.divPicklist, #divPicklist
{
	overflow:hidden;
	width:300px;
}

.chooserContainer
{
    margin-left:25px;
}

.videosAvailable
{
	width:250px;
}

#btnSave {
    background-image: url("/_layouts/BrightcoveVideoCloudIntegration/images/btn-homepage-blue.png");
    background-repeat: repeat-x;
    border: 0 none;
    border-radius: 4px 4px 4px 4px;
    color: #FFFFFF;
    padding: 2px 14px;
    text-shadow: 1px 1px 0 #3C666B !important;	
}    

#btnSave:hover {
    background-image: url("/_layouts/BrightcoveVideoCloudIntegration/images/tertiary-nav-hover.gif");
    background-repeat: repeat-x;
}

#btnCancel {
    background-image: url("/_layouts/BrightcoveVideoCloudIntegration/images/btn-homepage-blue.png");
    background-repeat: repeat-x;
    border: 0 none;
    border-radius: 4px 4px 4px 4px;
    color: #FFFFFF;
    padding: 2px 14px;
    text-shadow: 1px 1px 0 #3C666B !important;	
}    

#btnCancel:hover {
    background-image: url("/_layouts/BrightcoveVideoCloudIntegration/images/tertiary-nav-hover.gif");
    background-repeat: repeat-x;
}

.paging
{
    text-align:center;
    margin:5px 5px 5px 5px;
    width:150px;
    clear:both;
    display:block;
}

.paging .itemRange
{
    margin-left:20px;
    margin-right:20px;
}

A:link
{
    color: #96a44f;
    font-weight: bold;
    text-decoration: none;
}

A:visited
{
    color: #96a44f;
}
</style>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div class="chooserContainer">
	    <WpNs0:VideoPicklist ID="VideoPicklist1" runat="server" PartOrder="1"></WpNs0:VideoPicklist>

        <script type="text/javascript">
            $(document).ready(function () {
                $(".videosSelected").html("");

                if (picklistChooserType == "PlaylistId") {
                    $(".searchBox *").hide();
                }
            });
        </script>

        <center class="buttonBlock">
            <p>
                <button id="btnSave" onclick="AddSelectedItems();return ChooserDone(true);">OK</button>&nbsp;&nbsp;<button id="btnCancel" onclick="return ChooserCancel(true);">Cancel</button>
            </p>
        </center>
    </div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Chooser
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
Chooser
</asp:Content>
