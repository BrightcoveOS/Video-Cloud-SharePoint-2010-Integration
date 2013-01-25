<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VideoSearchUserControl.ascx.cs" Inherits="BrightcoveVideoCloudIntegration.VideoSearch.VideoSearchUserControl" %>

<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6.1/jquery.min.js" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(InitVideoSearch);

    function InitVideoSearch() {
        $('.searchResults .result').each(function (index) {
            var item = vcQueryResult.videos[index];
            var desc = item.shortDescription;
            var maxDescLen = 200;

            // Alternate row colors
            if ((index % 2) == 0) {
                $(this).addClass("blueBG");
            }

            if (desc.length > maxDescLen) {
                desc = item.shortDescription.substr(0, maxDescLen) + '...';
            }

            $(this).append('<div class="description">' + desc + '</div>');

            if (item.tags != '') {
                $(this).append('<div class="tags">Tags: ' + item.tags + '</div>');
            }

            var editLink = $(this).find('.editLink');

            if (vcIsAdmin) {
                $(editLink).html("Edit");
            }
            else {
                $(editLink).hide();
            }

            // Add this video to your blog
            $(this).append('<a class="blogLink editLink" onclick="CopyBlogCode(' + $(this).attr("videoId") + ');">Add to blog</a>');
            $('.blogLink').hide();
        });
    }

    function CopyBlogCode(vid) {
        var blog = '<object id="flashObj" width="640" height="390" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,47,0"><param name="movie" value="http://c.brightcove.com/services/viewer/federated_f9?isVid=1&isUI=1" /><param name="bgcolor" value="#FFFFFF" /><param name="flashVars" value="@videoPlayer=' +
            vid + '&playerID=' + 
            vcDefaultPlayerId + '&playerKey=AQ~~,AAAA5HYg_Wk~,dVrb4Dhl3iu-bD2b6KPfevAEl_qyND1e&domain=embed&dynamicStreaming=true" /><param name="base" value="http://admin.brightcove.com" /><param name="seamlesstabbing" value="false" /><param name="allowFullScreen" value="true" /><param name="swLiveConnect" value="true" /><param name="allowScriptAccess" value="always" /><embed src="http://c.brightcove.com/services/viewer/federated_f9?isVid=1&isUI=1" bgcolor="#FFFFFF" flashVars="@videoPlayer=' +
            vid + '&playerID=' + 
            vcDefaultPlayerId + '&playerKey=AQ~~,AAAA5HYg_Wk~,dVrb4Dhl3iu-bD2b6KPfevAEl_qyND1e&domain=embed&dynamicStreaming=true" base="http://admin.brightcove.com" name="flashObj" width="640" height="390" seamlesstabbing="false" type="application/x-shockwave-flash" allowFullScreen="true" allowScriptAccess="always" swLiveConnect="true" pluginspage="http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash"></embed></object>';
        prompt('Copy this HTML code to add the video to your blog', blog);
        //window.clipboardData.setData('Text', blog);
    }
</script>

<div class="searchBlock">
    <label for="keyword">Search</label>
    <input class="searchForm" name="keyword" onkeypress="" />
    <input type="submit" id="btnGo" name="btnGo" value="Go" />
</div>

<div id="message" class="searchResults" runat="server"></div>
