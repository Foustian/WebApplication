function GetIQNotification(isNextPage) {
    var jsonPostData = {};
    if (typeof (isNextPage) !== 'undefined') {
        jsonPostData = { p_IsNext: isNextPage };
    }

    $.ajax({
        url: _urlIQNotification_DisplayClientNotification,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {
                
                $("#divSetupContent").html("<div id=\"divIQNotification_Details\"></div><div id=\"divIQNotification_Content\"></div>");
                $("#divIQNotification_Content").html(result.HTML);

                $("#divIQNotification_ScrollContent").css("height", documentHeight - 200);


                $("#divIQNotification_ScrollContent").enscroll({
                    verticalTrackClass: 'track4',
                    verticalHandleClass: 'handle4',
                    margin: '0 0 0 10px'
                });

                showIQNotificationNoOfRecords(result);
            }
            else {
                if (typeof (isNextPage) === 'undefined') {
                    ClearResultsOnError('divIQNotification_Content', 'divIQNotificationPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetIQNotification(" + isNextPage + ")"));
                }
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            if (typeof (isNextPage) === 'undefined') {
                ClearResultsOnError('divIQNotification_Content', 'divIQNotificationPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetIQNotification(" + isNextPage + ")"));
            }
            ShowNotification(_msgErrorOccured);
        }
    });
}

function EditIQNotification(id) {
    var jsonPostData = {
        p_ID: id
    }

    $.ajax({
        url: _urlIQNotification_GetIQNotificationByID,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {
                $("#divIQNotification_Content").hide();
                $("#divIQNotification_Details").html(result.HTML);
                $("#divIQNotification_Details").show();

                $('#IQNotifationSettings_SearchRequestList').multiSelect({
                     selectableHeader: "<a href='javascript:;' onclick='$(\"#IQNotifationSettings_SearchRequestList\").multiSelect(\"select_all\");'>Select All</a>",
                     selectionHeader: "<a href='javascript:;' onclick='$(\"#IQNotifationSettings_SearchRequestList\").multiSelect(\"deselect_all\");'>Deselect All</a>",
                });

                $(".chosen-select").chosen({
                    display_disabled_options: true,
                    default_item: CONST_ZERO,
                    width: "93%"
                });
            }
            else {
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured);
        }
    });
}

function checkFrequency(item) {
    if (item.value == "OnceWeek") {
        $("#IQNotifationSettings_Notification_Day").removeAttr("disabled");
        $("#IQNotifationSettings_Notification_Time").removeAttr("disabled");
    }
    else if (item.value == "OnceDay") {

        $("#IQNotifationSettings_Notification_Day").val("-1");
        
        $("#IQNotifationSettings_Notification_Day").prop("disabled", "disabled");
        $("#IQNotifationSettings_Notification_Time").removeAttr("disabled");
    }
    else {
        $("#IQNotifationSettings_Notification_Day").val("-1");
        $("#IQNotifationSettings_Notification_Time").val("0");

        $("#IQNotifationSettings_Notification_Day").prop("disabled", "disabled");
        $("#IQNotifationSettings_Notification_Time").prop("disabled", "disabled");
    }
}


function showIQNotificationNoOfRecords(result) {
    if (result.hasMoreRecords) {
        $('#btnIQNotificationNextPage').show();
    }
    else {
        $('#btnIQNotificationNextPage').hide();
    }

    if (result.hasPreviousRecords) {
        $('#btnIQNotificationPreviousPage').show();
    }
    else {
        $('#btnIQNotificationPreviousPage').hide();
    }
    if (result.recordLabel != '') {
        $('#lblIQNotificationRecords').html(result.recordLabel);
    }
}

function DeleteIQNotification(id) {
    getConfirm("Delete IQ Notification", _msgConfirmIQNotificationDelete, "Confirm Delete", "Cancel", function (res) {
        if (res) {
            var jsonPostData = { p_ID: id}

            $.ajax({
                url: _urlIQNotification_DeleteIQNotification,
                contentType: "application/json; charset=utf-8",
                type: "post",
                dataType: 'json',
                data: JSON.stringify(jsonPostData),
                success: function (result) {
                    CheckForSessionExpired(result);

                    if (result != null && result.isSuccess) {
                        GetIQNotification();
                        ShowNotification(_msgIQNotificationDeleted);
                    }
                    else {
                        ShowNotification(result.error);
                    }
                },
                error: function (a, b, c) {
                    ShowNotification(_msgErrorOccured, a);
                }
            });
        }
    });
}

function SaveIQNotifationSettings() {
    if (ValidateIQNotifationSettings()) {
        $("#frmIQNotification").ajaxSubmit({
            target: "",
            global: true,
            success: function (res) {
                if (res.isSuccess) {

                    if ($("#IQNotifationSettings_IQNotificationKey").val() == "0") {
                        ShowNotification(_msgIQNotificationAdded);
                    }
                    else {
                        ShowNotification(_msgIQNotificationUpdated);
                    }
                    //ClearIQNotification();
                    GetIQNotification();
                }
                else {
                    ShowNotification(res.errorMsg);
                }
            }
        });
    }
}

function ValidateIQNotifationSettings() {

    var isValid = true;

    $("#spanIQNotifationSettings_SearchRequestList").html("").hide();
    $("#spanIQNotifationSettings_Notification_Address").html("").hide();
    $("#spanIQNotifationSettings_Notification_Time").html("").hide();
    $("#spanIQNotifationSettings_Frequency").html("").hide();
    $("#spanIQNotifationSettings_Notification_Day").html("").hide();
    

    if ($("#IQNotifationSettings_Notification_Address").val().trim() == "") {
        $("#spanIQNotifationSettings_Notification_Address").show().html(_msgCustomerRegistrationEmailRequiredField);
        isValid = false;
    }

    if ($("#IQNotifationSettings_Notification_Address").val() != "") {
        var Toemail = $("#IQNotifationSettings_Notification_Address").val();
        if (Toemail.substr(Toemail.length - 1) == ";") {
            Toemail = Toemail.slice(0, -1);
        }
        $(Toemail.split(';')).each(function (index, value) {
            if (value.indexOf(',') > 0) {
                $("#spanIQNotifationSettings_Notification_Address").show().html(_msgEmailNotCommaSeprated);
                isValid = false;
                return;

            }
            else {
                if (!CheckEmailAddress($.trim(value))) {
                    $("#spanIQNotifationSettings_Notification_Address").show().html(_msgOneEmailAddressInCorrect);
                    isValid = false;
                    return;
                }
            }

        });

        if(Toemail.split(';').length > _MaxEmailAdressAllowed)
        {
            $("#spanIQNotifationSettings_Notification_Address").show().html(_msgMaxEmailAdressLimitExceeds.replace(/@@MaxLimit@@/g, _MaxEmailAdressAllowed));
            isValid = false;
        }
    }

    if ($("#IQNotifationSettings_SearchRequestList").val() == null || $("#IQNotifationSettings_SearchRequestList").val().length == 0) {
        $("#spanIQNotifationSettings_SearchRequestList").show().html(_msgIQAgentRequired);
        isValid = false;
    }

    if ($("#IQNotifationSettings_Frequency").val() == "0") {
            $("#spanIQNotifationSettings_Frequency").show().html(_msgFrequencyRequired);
            isValid = false;
        }

    if (($("#IQNotifationSettings_Frequency").val() == "OnceDay" || $("#IQNotifationSettings_Frequency").val() == "OnceWeek") && $.trim($("#IQNotifationSettings_Notification_Time").val()) == "0") {
       $("#spanIQNotifationSettings_Notification_Time").show().html(_msgTimeRequired);
        isValid = false;
    }
        
   if ($("#IQNotifationSettings_Frequency").val() == "OnceWeek" && $("#IQNotifationSettings_Notification_Day").val() == "-1") {
        $("#spanIQNotifationSettings_Notification_Day").show().html(_msgDayRequired);
        isValid = false;
   }

    return isValid;
}

function ClearIQNotification() {
    $("#IQNotifationSettings_IQNotificationKey").val("0");
    $('#IQNotifationSettings_SearchRequestList').multiSelect('deselect_all');
    $("#IQNotifationSettings_ReportImageID").val('');
    $("#IQNotifationSettings_Frequency").val('');
    $("#IQNotifationSettings_Notification_Day").val('');
    $("#IQNotifationSettings_Notification_Time").val('');
    $("#IQNotifationSettings_MediaTypeList").val("0");
    $("#IQNotifationSettings_Notification_Address").val("");
}

function CancelIQNotifationSettings()
{
    $("#divIQNotification_Content").show();
    $("#divIQNotification_Details").hide();
}