<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VideoEditorUserControl.ascx.cs" Inherits="BrightcoveVideoCloudIntegration.VideoEditor.VideoEditorUserControl" %>

<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6.1/jquery.min.js" type="text/javascript"></script>
<script language="JavaScript" type="text/javascript" src="http://admin.brightcove.com/js/BrightcoveExperiences.js"></script>
<script type="text/javascript">
    $(document).ready(InitVideoEditor);

    function InitVideoEditor() {
        if ($('#name').val() == '') {
            $('#btnDelete').hide();
            $('.videoPlayer').hide();
            $('.videoPlayer').hide();
        }
        else
        {
            $('.oneVideo').css('visibility', 'hidden');
            $('.addVideo').hide();
            $('#btnDelete').show();
            $('.videoPlayer').show();

            // Check for result actions
            var results = $(".videoEditor").html();

            if (results.indexOf("SUCCESS ") >= 0) {
                if (results.indexOf("<br />") > 0) {
                    $('.videoEditor').prepend('<p class="success">Your video was successfully uploaded. Please allow up to five minutes for changes to propagate through the Brightcove Video Cloud.</p>');
                }
                else {
                    $('.videoEditor').prepend('<p class="success">Your changes were successfully saved. Please allow up to five minutes for changes to propagate through the Brightcove Video Cloud.</p>');
                }
            }
            else if (results.indexOf("ERROR: ") >= 0) {
                $('.videoEditor').prepend('<p class="error">Your changes could not be saved. Please make sure you have completed all required fields.</p>');
            }
        }

        if (!vcIsAdmin) {
            $('.videoBlock *').attr('disabled', 'disabled');
            $('#btnCancel').attr('disabled', '');
            $('#btnSave').hide();
            $('#btnDelete').hide();
        }
    }

    function AddMoreVideos() {
        $('.moreVideos').show();
        $('.addVideo').hide();
    }
</script>

<div id="message" class="videoEditor" runat="server"><div id="instructions" class="warning">Use this form to edit this video's properties. 
Required fields are marked with an asterisk. Allow up to five minutes for changes to propagate through the Brightcove Video Cloud.</div></div>

<div class="videoBlock">
    <div>
        <label for="name">Name<span class="required">*</span></label>
        <input type="text" name="name" id="name" />
    </div>

    <div>
        <label for="shortDesc">Short Description</label>
        <input type="text" name="shortDesc" id="shortDesc" />
    </div>

    <div>
        <label for="longDesc">Long Description</label>
        <textarea name="longDesc" id="longDesc" rows="3" cols="30"></textarea>
    </div>

    <div>
        <label for="isActive">Is Active</label>
        <select name="isActive" id="isActive"><option value="Yes">Yes&nbsp;</option><option>No</option></select>
    </div>

    <div>
        <label for="linkUrl">Related Link URL</label>
        <input type="text" name="linkUrl" id="linkUrl" />
    </div>

    <div>
        <label for="linkText">Related Link Text</label>
        <input type="text" name="linkText" id="linkText" />
    </div>

    <div>
        <label for="videoStillUrl">Video Still</label>
        <input type="text" name="videoStillUrl" id="videoStillUrl" />
        <input class="imageUpload" type="file" name="file_videoStillUrl" id="file_videoStillUrl" />
    </div>

    <div>
        <label for="thumbnailUrl">Thumbnail</label>
        <input type="text" name="thumbnailUrl" id="thumbnailUrl" />
        <input class="imageUpload" type="file" name="file_thumbnailUrl" id="file_thumbnailUrl" />
    </div>

    <div>
        <label for="referenceId">Reference ID</label>
        <input type="text" name="referenceId" id="referenceId" />
    </div>

    <div>
        <label for="startDate">Start Availability Date</label>
        <input type="text" name="startDate" id="startDate" class="datepicker" />
        <span class="fieldDescriptor">mm/dd/yyyy</span>
    </div>

    <div>
        <label for="endDate">End Availability Date</label>
        <input type="text" name="endDate" id="endDate" class="datepicker" />
        <span class="fieldDescriptor">mm/dd/yyyy</span>
    </div>

    <div>
        <label for="economics">Economics</label>
        <select name="economics" id="economics">
            <option></option>
            <option value="FREE">Free&nbsp;</option>
            <option value="AD_SUPPORTED">Ad Supported&nbsp;</option>
        </select>
    </div>

    <div>
        <label for="customFields">Custom Fields</label>
        <input type="text" name="customFields" id="customFields" />
        <span class="fieldDescriptor">e.g., author:Smith,year:2011</span>
    </div>

    <div>
        <label for="tags">Tags</label>
        <input type="text" name="tags" id="tags" />
        <span class="fieldDescriptor">e.g., Apple,iPad</span>
    </div>

    <div class="oneVideo">
        <label for="file1">Video File(s)<span class="required">*</span></label>
        <input type="file" name="file1" id="file1" style="display:inline;" />&nbsp;&nbsp;&nbsp;<a class="addVideo" onclick="AddMoreVideos()">Add More Videos</a>
    </div>

    <div class="moreVideos" style="display:none;">
        <label>&nbsp;</label><input type="file" name="file2" id="file2" />
        <label>&nbsp;</label><input type="file" name="file3" id="file3" />
        <label>&nbsp;</label><input type="file" name="file4" id="file4" />
        <label>&nbsp;</label><input type="file" name="file5" id="file5" />
    </div>

    <p class="buttonBlock">
        <input type="submit" id="btnSave" name="btnSave" value="Save" />
        <input type="submit" id="btnCancel" name="btnCancel" value="Cancel" onclick="try { top.SP.UI.ModalDialog.get_childDialog().close(1); } catch(e) {}" />
        <input type="submit" id="btnDelete" name="btnDelete" value="Delete" style="display:none;" onclick="if(!confirm('Are you sure you want to delete this video?')){ return false; }" />
    </p>
</div>
