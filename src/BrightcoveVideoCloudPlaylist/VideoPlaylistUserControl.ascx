<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VideoPlaylistUserControl.ascx.cs" Inherits="BrightcoveVideoCloudIntegration.VideoPlaylist.VideoPlaylistUserControl" %>

<script src="//ajax.googleapis.com/ajax/libs/jquery/1.6.1/jquery.min.js" type="text/javascript"></script>
<script type="text/javascript">
var vcPlaylistId = "<%= this.PlaylistId %>";
var vcPlayVideoLink = "<%= this.PlayVideoLink %>";
var vcChangedPlaylist = <%= this.ChangedPlaylist.ToString().ToLower() %>;
var vcIsAdmin = <%= this.IsAdmin.ToString().ToLower() %>;
var vcDefaultPlayerId = <%= this.DefaultPlaylistPlayerId %>;

$(document).ready(InitVideoPlaylist);

function InitVideoPlaylist() {
    if (!vcIsAdmin)
    {
        $('.playlistEdit').css('visibility', 'hidden');
        $('.newPlaylist *').attr('disabled', 'disabled');
        $('#cancel').attr('disabled', '');
        $('#newPlaylist').hide();
        $('#save').hide();
        $('#deletePlaylist').hide();
    }

    if (vcChangedPlaylist || (getQuerystringParam("close") == "1")) {
        try { top.SP.UI.ModalDialog.get_childDialog().close(1); } catch(e) {}

        return;
    }

    $('.blogLink').hide();

    $('.playlistRow').each(function (index) {
        if ((index % 2) == 0) {
            $(this).addClass("blueBG");
        }

        var pThumb = $(this).find('.thumbnail img');
        var vCount = $(this).find('.playlistVideoCount');
        var type = $(this).find('.playlistType');
        var pListId = $(this).find('.playlistId');
        var tags = $(this).find('.playlistTags');

        if (pThumb.attr('src') == '')
        {
            pThumb.attr('src', 'images/playlist.png');
        }

        pThumb.css({ 'max-width':'64px', 'max-height':'48px', 'float':'left', 'vertical-align':'text-top', 'margin-right':'5px' });

        vCount.text(vCount.attr('val') + ' Videos');

        // Change the playlist type display
        var newType = ' | ' + type.text();

        if (type.attr('val').indexOf('OLDEST') >= 0) {
            newType += ' (Published Date)';
        }

        type.text(newType);
        pListId.text(' | Playlist ID: ' + pListId.attr('val'));

        if (tags.attr('val') != '') {
            tags.text(' | Tags: ' + tags.attr('val'));
        }
    });

    // Handle selecting the playlist
    $('.playlistRow input[type=checkbox]').each(function () {
        $(this).click(function () {
            // Uncheck the other rows
            if ($(this).attr('checked') != 'checked') {
                $('.playlistRow input[type=checkbox]').attr('checked', false);
                $('#editPlaylist').hide();
                EditPlaylist('', '', '', '', '');
            }
            else {
                $('.playlistRow input[type=checkbox]').attr('checked', false);
                $(this).attr('checked', true);
                $('#editPlaylist').show();
                $('#deletePlaylist').show();

                var container = $(this).parent();

                EditPlaylist($(this).val(), container.find('.playlist').attr('val'),
                    container.find('.playlistType').attr('val'), container.find('playlistTags').attr('val'), '');
            }
        });
    });

    // Handle playlist type selection
    $('#playlistType').bind("change", function () {
        var playlistTypeVal = $(this).val();

        $('#playlistTypeDesc').hide();

        if (playlistTypeVal == "EXPLICIT") {
            //$('.ctrlTags').css('visibility', 'hidden');
            $('.ctrlTags').hide();
            $('.ctrlVideos').show();
            ShowPicklist();
        }
        else {
            //$('.ctrlTags').css('visibility', 'visible');
            $('.ctrlTags').show();
            $('.ctrlVideos').hide();

            if (playlistTypeVal.indexOf("OLDEST") >= 0) {
                $('#playlistTypeDesc').show();
            }

            HidePicklist();
        }
    });

    // Handle new playlist button
    //$('#newPlaylist').click(NewPlaylist);

    // Handle edit playlist button
    $('#editPlaylist').click(function () {
        $('.newPlaylist').show();
        $('.videoPlaylist').hide();
        $('#save').show();
        $('#cancel').show();
        $('#newPlaylist').hide();
        $('#editPlaylist').hide();
        $('#deletePlaylist').show();

        var qsPid = getQuerystringParam("playlistid");

        if (qsPid == "") {
            window.location = window.location.href + "?playlistid=" + $('#playlistId').val();
        }
        else {
            window.location = window.location.href.replace("playlistid=" + qsPid, "playlistid=" + $('#playlistId').val());
        }

        return false;
    });

    if ((vcPlaylistId != '') && (vcPlaylistId != '0') && !isNaN(vcPlaylistId)) {
        $('.newPlaylist').show();
        $('.videoPlaylist').hide();
        $('#save').show();
        $('#cancel').show();
        $('#newPlaylist').hide();
        $('#editPlaylist').hide();
        $('#deletePlaylist').show();

        // Show/hide tags and video picklist
        $('#playlistTypeDesc').hide();

        if ($('#playlistType').val() == "EXPLICIT") {
            //$('.ctrlTags').css('visibility', 'hidden');
            $('.ctrlTags').hide();
            $('.ctrlVideos').show();
            ShowPicklist();
        }
        else {
            //$('.ctrlTags').css('visibility', 'visible');
            $('.ctrlTags').show();
            $('.ctrlVideos').hide();

            if ($('#playlistType').val().indexOf("OLDEST") >= 0) {
                $('#playlistTypeDesc').show();
            }
        }
    }
    else if (window.location.href.indexOf('playlistid=') > 0)
    {
        NewPlaylist();
    }
}

function NewPlaylist()
{
    $('.newPlaylist').show();
    $('.videoPlaylist').hide();
    $('#save').show();
    $('#cancel').show();
    $('#newPlaylist').hide();
    $('#editPlaylist').hide();
    $('#deletePlaylist').hide();

    // Clear the form
    EditPlaylist('', '', '', '', '');
    ShowPicklist();

    return false;
}

function getQuerystringParam(key) {
    key = key.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regex = new RegExp("[\\?&]" + key + "=([^&#]*)");
    var qs = regex.exec(window.location.href);
    if (qs == null) {
        return '';
    }
    else {
        return qs[1];
    }
}

function EditPlaylist(id, name, type, tags, videos) {
    $('#playlistId').val(id);
    $('#playlistName').val(name);

    if (type && (type != '')) {
        $('#playlistType').val(type);
    }
    else {
        $('#playlistType').val('EXPLICIT');
    }

    $('#playlistTypeDesc').hide();

    if ($('#playlistType').val() == "EXPLICIT") {
        //$('.ctrlTags').css('visibility', 'hidden');
        $('.ctrlTags').hide();
        $('.ctrlVideos').show();
        //ShowPicklist();
    }
    else {
        //$('.ctrlTags').css('visibility', 'visible');
        $('.ctrlTags').show();
        $('.ctrlVideos').hide();
        //HidePicklist();

        if ($('#playlistType').val().indexOf("OLDEST") >= 0) {
            $('#playlistTypeDesc').show();
        }
        else {
            $('#playlistTypeDesc').hide();
        }
    }

    $('#tags').val(tags);
    $('#videos').val(videos);
}

function ShowPicklist() {
    if ($('#divPicklist')[0]) {
        $('#videos').hide();
        $('.ctrlVideos span').hide();

        if ($('.ctrlVideos').html().indexOf("Select video(s)") < 0) {
            $('.ctrlVideos').append('Select video(s)');
        }

        try { FillSelectedPicklist(vcPlaylistVideos, Callback_SetVideos); } catch (e) { }
        $('#divPicklist').appendTo($('.ctrlPicklist'));
        $('#divPicklist').show();
    }
    else {
        // Make sure this is shown if the picklist is not available
        $('#videos').show();
    }
}

function HidePicklist() {
    $('#divPicklist').hide();
}

function Callback_SetVideos(list) {
    if (list) {
        $('#videos').val(list);
    }
    else {
        $('#videos').val('');
    }
}

function CopyBlogCode(pid) {
    var playerId = vcDefaultPlayerId;
    var opid = 'player_' + vcDefaultPlayerId + '_' + Math.floor(100000000 * Math.random());
    var width = '640';
    var height = '390';
    var bgColor = 'FFFFFF';
    var blog =
        '<object id=' + opid + ' class=BrightcoveExperience codeBase="https://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,28,0" classid=clsid:D27CDB6E-AE6D-11cf-96B8-444553540000 width=' + 
            width + ' height=' + height + ' type=application/x-shockwave-flash>' + 
		'    <param name="FlashVars" value="">' +
		'    <param name="Movie" value="https://secure.brightcove.com/services/viewer/federated_f9?&amp;width=' +
            width + '&amp;height=' + height + '&amp;flashID=' + opid + '&amp;playerID=' + playerId + '&amp;%40playlistTabs=' +
            pid + '&amp;%40playlistCombo=' + pid + '&amp;%40videoList=' + pid + '&amp;dynamicStreaming=true&amp;cacheAMFURL=http%3A%2F%2Fshare.brightcove.com%2Fservices%2Fmessagebroker%2Famf&amp;secureConnections=true&amp;bgcolor=%23' + 
            bgColor + '&amp;autoStart=true&amp;isVid=true&amp;isUI=true&amp;debuggerID=">' +
		'    <param name="Src" value="https://secure.brightcove.com/services/viewer/federated_f9?&amp;width=' + 
            width + '&amp;height=' + height + '&amp;flashID=' + opid + '&amp;playerID=' + playerId + '&amp;%40playlistTabs=' +
            pid + '&amp;%40playlistCombo=' + pid + '&amp;%40videoList=' + pid + '&amp;dynamicStreaming=true&amp;cacheAMFURL=http%3A%2F%2Fshare.brightcove.com%2Fservices%2Fmessagebroker%2Famf&amp;secureConnections=true&amp;bgcolor=%23' + 
            bgColor + '&amp;autoStart=true&amp;isVid=true&amp;isUI=true&amp;debuggerID=">' + 
		'    <param name="Base" value="https://sadmin.brightcove.com">' + 
		'    <param name="AllowScriptAccess" value="always">' + 
		'    <param name="BGColor" value="' + bgColor + '">' + 
		'    <param name="SWRemote" value="">' + 
		'    <param name="SeamlessTabbing" value="0">' + 
		'    <param name="AllowNetworking" value="all">' + 
		'    <param name="AllowFullScreen" value="true">' + 
		'    <embed src="http://c.brightcove.com/services/viewer/federated_f9?isVid=1&isUI=1" bgcolor="#' + bgColor + '" flashVars="@40playlistTabs=' +
            pid + '&@playlistCombo=' + pid + '&playerID=' + playerId + '&@videoList=' + pid + '&playerKey=AQ~~,AAAA5HYg_Wk~,dVrb4Dhl3iu-bD2b6KPfevAEl_qyND1e&domain=embed&dynamicStreaming=true" base="https://sadmin.brightcove.com" name="flashObj" width="' + 
            width + '" height="' + height + '" seamlesstabbing="false" type="application/x-shockwave-flash" allowFullScreen="true" allowScriptAccess="always" swLiveConnect="true" pluginspage="http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash">' +
		'    </embed>' + 
	    '</object>';

    prompt('Copy this HTML code to add the video to your blog', blog);
    //window.clipboardData.setData('Text', blog);
}
</script>

<div id="message" class="videoPlaylist" runat="server"><div class="warning">&nbsp;</div></div>

<div class="playlistControls">
    <div class="newPlaylist" style="display:none;">
        <div class="playlistId" style="display:none;">
            <label for="playlistId">ID</label>
            <input id="playlistId" name="playlistId" type="text" value="<%= this.PlaylistId %>" />
        </div>
        <div>
            <label for="playlistName">Name</label>
            <input id="playlistName" name="playlistName" type="text" value="<%= this.PlaylistName %>" />
        </div>
        <div>
            <label for="playlistType">Type</label>
            <select id="playlistType" name="playlistType">
                <%= this.GetPlaylistOptions() %>
            </select>
            <span id="playlistTypeDesc" class="fieldDescriptor" style="display:none;">(published date)</span>
        </div>
        <div class="ctrlTags">
            <label for="tags">Tags</label>
            <input id="tags" name="tags" type="text" value="<%= this.PlaylistTags %>"/>
            <span class="fieldDescriptor">(comma separated)</span>
        </div>
        <div class="ctrlVideos">
            <label for="videos">Videos</label>
            <input id="videos" name="videos" type="text" value="<%= this.VideoIdListString %>" />
            <span class="fieldDescriptor">(comma separated)</span>
        </div>
        <div class="ctrlPicklist"></div>
    </div>

    <div class="buttonSection">
        <input type="button" id="newPlaylist" value="New Playlist" onclick="<%= string.Format(this.EditPlaylistLink, "").Replace("javascript:void", "") %>; return false;" />
        <button id="editPlaylist" style="display:none;">Edit Playlist</button>
        <input type="submit" id="save" name="save" value="Save" style="display:none;" />
        <input type="submit" id="cancel" name="cancel" value="Cancel" style="display:none;" onclick="try { top.SP.UI.ModalDialog.get_childDialog().close(1); } catch(e) {}" />
        <input type="submit" id="deletePlaylist" name="deletePlaylist" style="display:none;" value="Delete Playlist" onclick="if(!confirm('Are you sure you want to delete this playlist?')){ return false; }" />
    </div>
</div>
