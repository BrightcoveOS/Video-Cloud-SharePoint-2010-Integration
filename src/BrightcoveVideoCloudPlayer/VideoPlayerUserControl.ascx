<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VideoPlayerUserControl.ascx.cs" Inherits="BrightcoveVideoCloudIntegration.VideoPlayer.VideoPlayerUserControl" %>

<div id="message" class="videoPlayer" runat="server"></div>

<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6.1/jquery.min.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript" src="http://admin.brightcove.com/js/BrightcoveExperiences.js"></script>
<script language="JavaScript" type="text/javascript">
    var vcCurrentWeb = "<%= Microsoft.SharePoint.SPContext.Current.Web.Url %>";

    document.cookie = "vcCurrentWeb=" + escape(vcCurrentWeb) + "; path=/";
    $(document).ready(InitVideoPlayer0);

    function InitVideoPlayer0()
    {
        InitVideoPlayer(['<%= this.message.ClientID %>', '<%= this.PlayerId %>', '<%= this.VideoId %>', 
            '<%= this.PlaylistId %>', '<%= this.PlaylistId %>', '<%= this.PlaylistId %>', '<%= this.BackgroundColor %>', 
            '<%= this.PlayerWidth %>', '<%= this.PlayerHeight %>', '<%= this.AutoStart.ToString().ToLower() %>']);
    }

    function InitVideoPlayer(args) {
        var playerContainer = document.getElementById(args[0]);
        var player = brightcove.createElement("object");
        var params = {};

        params.playerID = args[1];

        if (args[2] && (args[2] != '')) {
            params.videoId = args[2];
        }
        else {
            params["@playlistTabs"] = args[3];
            params["@playlistTabs.featured"] = args[3];
            params["@playlistCombo"] = args[4];
            params["@playlistCombo.featured"] = args[4];
            params["@videoList"] = args[5];
            params["@videoList.featured"] = args[5];
            params.dynamicStreaming = "true";
            params.cacheAMFURL = "http://share.brightcove.com/services/messagebroker/amf";
            params.secureConnections = "true";
        }


        params.bgcolor = args[6];
        params.width = args[7];
        params.height = args[8];
        params.autoStart = args[9];
        params.isVid = "true";
        params.isUI = "true";

        player.id = "player_" + params.playerID + "_" + params.videoId;

        if (player.id.indexOf("__") < 0) {
            for (var i in params) {
                var parameter = brightcove.createElement("param");

                parameter.name = i;
                parameter.value = params[i];
                player.appendChild(parameter);
            }

            brightcove.createExperience(player, playerContainer, true);
        }
        else {
            //playerContainer.innerHTML = "<p>Set the <b>Player ID</b> for this web part's <i>Configuration</i>.</p>";
        }
    }
</script>
