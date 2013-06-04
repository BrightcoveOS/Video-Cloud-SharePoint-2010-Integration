<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VideoPicklistUserControl.ascx.cs" Inherits="BrightcoveVideoCloudIntegration.VideoPicklist.VideoPicklistUserControl" %>

<div id="message" class="videoPicklist" runat="server"></div>

<script src="//ajax.googleapis.com/ajax/libs/jquery/1.6.1/jquery.min.js" type="text/javascript"></script>
<script type="text/javascript">
    var queryStringKeyAsyncQueryText = "<%= BrightcoveVideoCloudIntegration.VideoCloudWebPart.QueryStringKeyAsyncQueryText %>";
    var queryStringKeyAsyncChooserText = "<%= BrightcoveVideoCloudIntegration.VideoCloudWebPart.QueryStringKeyAsyncChooserText %>";
    var handleSelectedVideos = null;
    var picklistResult = null;
    var picklistChooserType = null;

    $(window).unload(ChooserDone);
    $(document).ready(function () {
        picklistChooserType = '<%= Request.QueryString[BrightcoveVideoCloudIntegration.VideoCloudWebPart.QueryStringKeyAsyncChooserText] %>';

        if (picklistChooserType != '') {
            $('#divPicklist').show();
            try { picklistResult = window.dialogArguments.value; } catch (e) { picklistResult = ''; }
            if ((picklistResult == undefined) || (picklistResult == "undefined")) {
                picklistResult = '';
            }
            handleSelectedVideos = ChooserDone;

            if (picklistChooserType == "PlaylistId") {
                $(".available .sectionHeader").html("Available Playlists");
            }
        }

        $.ajax({
            url: AddQueryStringParam(window.location.href, queryStringKeyAsyncQueryText + "="),
            success: FillAvailablePicklist
        });
    });

    function ChooserCancel(doCloseWindow) {
        if ((picklistResult == undefined) || (picklistResult == "undefined")) {
            picklistResult = '';
        }

        window.returnValue = picklistResult;

        if (doCloseWindow == true) {
            window.close();

            return false;
        }
    }

    function ChooserDone(doCloseWindow)
    {
        var result = $($('.videosSelected .item')[0]).attr('videoId');

        if (result)
        {
            picklistResult = result;
        }

        if ((picklistResult == undefined) || (picklistResult == "undefined"))
        {
            picklistResult = '';
        }

        try
        {
            if (picklistResult) {
                window.returnValue = picklistResult;
            }
            else {
                window.returnValue = '';
            }
        } catch (e) { }

        if (doCloseWindow == true) {
            window.close();

            return false;
        }
    }

    function AddQueryStringParam(url, param) {
        if (url.indexOf("?") > 0) {
            return url + "&" + param;
        }
        else {
            return url + "?" + param;
        }
    }

    function FillAvailablePicklist(data) {
        // Add the returned SCRIPT tag from the async call
        // It contains: vcAsyncVideoCount, vcAsyncVideoResults
        if (data && (data.indexOf("vcAsyncVideoResults") > 0))
        {
            $('.videoPicklist').html(data);
        }

        if (!vcAsyncVideoResults) {
            return;
        }

        var buffer = new Array();

        for (var i = 0; i < vcAsyncVideoResults.length; i++) {
            var aVideo = vcAsyncVideoResults[i];

            if (aVideo) {
                if (aVideo.thumbnailURL == "") {
                    aVideo.thumbnailURL = "/_layouts/BrightcoveVideoCloudIntegration/images/playlist.png";
                }

                buffer.push(CreatePicklistItem(aVideo));
            }
        }

        $('.videosAvailable').html(buffer.join(''));
        $('#picklistPaging').html(vcAsyncVideoPaging);
        $('#picklistPaging a').each(function () {
            $(this).attr('href', 'javascript:void $.ajax({url:\'' + $(this).attr('href') + '\', success: FillAvailablePicklist});');
        });
    }

    function FillSelectedPicklist(data, callback)
    {
        if (!data) {
            return;
        }

        var buffer = new Array();

        for (var i = 0; i < data.length; i++) {
            buffer.push(CreatePicklistItem(data[i]));
        }

        $('.videosSelected').html(buffer.join(''));
        handleSelectedVideos = callback;
    }

    function CreatePicklistItem(video) {
        var result =
            '<div class="item" videoId="' + video.id + '" onclick="SelectToggleItem(this);">' + 
            '    <div class="videoCheckbox"><input type="checkbox" /></div>' + 
            '    <div class="videoThumb"><img src="' + video.thumbnailURL + '" /></div>' + 
            '    <div class="videoMetadata">' +
            '        <span class="videoName">' + video.name + '</span><br />' +
            '        <span class="videoId">id: ' + video.id + '</span>' +
            '    </div>' + 
            '</div>';

        return result;
    }

    function SelectToggleItem(parentElem) {
        var itemCheckbox = $(parentElem).find('> .videoCheckbox > input')[0];

        if ($(parentElem).hasClass('videoSelected')) {
            $(parentElem).removeClass('videoSelected');
            itemCheckbox.checked = false;
        }
        else {
            $(parentElem).addClass('videoSelected');
            itemCheckbox.checked = true;
        }

        // Get list of selected videos
        if ($(parentElem).parent().hasClass('videosSelected')) {
            var selected = MoveList();

            if (selected.length > 0) {
                $('#inpMoveSelected')[0].checked = true;
            }
            else {
                $('#inpMoveSelected')[0].checked = false;
            }
        }
    }

    function AddSelectedItems() {
        try
        {
            var buffer = new Array();

            $(".videosAvailable .videoSelected").each(function () {
                //buffer.push('<div class="item" videoId="' + $(this).attr("videoId") + '">' + $(this).html() + '</div>');
                buffer.push($(this).clone().wrap('<div></div>').parent().html());
            });

            // Nothing to do id nothing was selected
            if (buffer.length > 0) {
                $(".videosSelected").append(buffer.join(''));
            }

            OnVideosSelected();
        }
        catch (e) { }

        return false;
    }

    function RemoveSelectedItems() {
        try
        { 
            $(".videosSelected .videoSelected").each(function () {
                $(this).remove();
            });

            OnVideosSelected();
        }
        catch (e) { }

        return false;
    }

    function OnVideosSelected() {
        var result = new Array();

        // Unselect available and selected videos
        $(".picklist .videoSelected").removeClass("videoSelected");
        $(".picklist .videoCheckbox input").each(function () { $(this)[0].checked = false; });

        // Get the video IDs in the select list
        $('.videosSelected .item').each(function () {
            result.push(parseInt($(this).attr('videoId')));
        });

        if (handleSelectedVideos) {
            handleSelectedVideos(result.join(","));
        }
    }

    function disableEnterKey(e, elem) {
        var key = 0;

        if (window.event) {
            // IE
            key = window.event.keyCode;
        }
        else {
            // FF
            key = e.which;
        }

        if (key == 13) {
            if (elem) {
                DoSearch(elem);
            }

            return false;
        }
        else {
            return true;
        }
    }

    function DoSearch(elemInput) {
        try {
            if (!elemInput) {
                elemInput = $('.searchBox_default_text')[0];
            }

            if (elemInput.value == '') {
                elemInput.value = 'search video';
                $(elemInput).addClass('searchBox_default_text');
                $.ajax({
                    url: AddQueryStringParam(window.location.toString(), queryStringKeyAsyncQueryText + '='),
                    success: FillAvailablePicklist
                });
            }
            else {
                if (elemInput.value == 'search video') {
                    elemInput.value = '';
                }

                $(elemInput).removeClass('searchBox_default_text');
                $.ajax({
                    url: AddQueryStringParam(window.location.toString(), queryStringKeyAsyncQueryText + '=' + elemInput.value),
                    success: FillAvailablePicklist
                });
            }
        }
        catch (e) {
            // Do nothing
        }

        return false;
    }

    function MoveList() {
        var selected = new Array();

        $('.videosSelected > .item > .videoCheckbox > input').each(function (index) {
            if ($(this)[0].checked) {
                selected.push(index);
            }
        });

        return selected;
    }

    function MoveUp() {
        try {
            var selected = MoveList();

            for (var i = 0; i < selected.length; i++) {
                var currElem = $('.videosSelected .item')[selected[i]];
                var prevElem = $('.videosSelected .item')[selected[i] - 1];

                if (currElem && prevElem) {
                    $(prevElem).before($(currElem));
                }
                else {
                    break;
                }
            }
        }
        catch (e) { }

        return false;
    }

    function MoveDown() {
        try {
            var selected = MoveList();

            for (var i = selected.length - 1; i >= 0; i--) {
                var currElem = $('.videosSelected .item')[selected[i]];
                var nextElem = $('.videosSelected .item')[selected[i] + 1];

                if (currElem && nextElem) {
                    $(currElem).before($(nextElem));
                }
                else {
                    break;
                }
            }
        }
        catch (e) { }

        return false;
    }

    function MoveNone(checked) {
        $('.videosSelected > .item > .videoCheckbox > input').each(function (index) {
            if ($(this)[0].checked != checked) {
                $(this).parent().parent().click();
            }
        });

        return false;
    }
</script>

<div id="divPicklist" style="display:none;">
    <div class="picklist available">
        <div class="sectionHeader">Available Videos</div>
        <div class="searchBox">
            <input class="searchBox_default_text" type="text" value="search video" onkeypress="return disableEnterKey(event, this);" onchange="return DoSearch(this);" onfocus="if(this.value=='search video'){this.value='';$(this).removeClass('searchBox_default_text');}" /><button onclick="return DoSearch();">&nbsp;</button>
        </div>
        <div class="content videosAvailable"></div>
        <div id="picklistPaging" class="paging"></div>
    </div>

    <div class="picklistControls">
        <div class="controlSection">
            <button onclick="return AddSelectedItems();">&gt;&gt;</button>
            <button onclick="return RemoveSelectedItems();">&lt;&lt;</button>
        </div>
        <div class="controlSection" style="display:none;">
            <button onclick="$('.videosAvailable .item').each(function(){$(this).addClass('.videoSelected');});return AddSelectedItems();">&gt;&gt;</button>
            <button onclick="$('.videosSelected .item').each(function(){$(this).addClass('.videoSelected');});return RemoveSelectedItems();">&lt;&lt;</button>
        </div>
    </div>

    <div class="picklist selected">
        <div class="sectionHeader">Videos in this Playlist</div>
        <div class="moveButtons">
            <input type="checkbox" onclick="MoveNone(this.checked);" id="inpMoveSelected" name="moveSelected"/> <label for="moveSelected">move video</label>
            <span>
                <button onclick="return MoveUp();">Up</button><button onclick="return MoveDown();">Down</button>
            </span>
        </div>
        <div class="content videosSelected"></div>
    </div>
</div>
