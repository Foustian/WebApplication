// Globals
var _campResult = null;

var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

var _urlDisplaySetup = "/Setup/DisplayCampaignSetupContent/";
var _urlGetSetupEdit = "/Setup/GetCampaignSetupEdit/";
var _urlSaveCampaign = "/Setup/SaveCampaign/";
var _urlDeleteCampaign = "/Setup/DeleteCampaign/";
var _urlStopCampaign = "/Setup/StopCampaign/";

$(document).ready(function () {

});

function ClearCampaignAddEditPopup() {
    $("#hdnCampaignID").val("null");
    $("#txtCampaignName").val(null);
    $("#ddAgentForCampaign").val(null);
    ResetAddEditDates();
}

function ShowCreatePopup() {
    $("#campaignSetupAddEditTitle").html("Create Campaign");
    ShowCampaignSetupPopup();
}

function ResetAddEditDates() {
    // Initialize start date to now
    $("#dpDateStart").datepicker("setDate", -30);
    // Initialize end date to -30 days previous
    $("#dpDateEnd").datepicker("setDate", new Date(Date.now()));
}

function ShowCampaignSetupPopup() {
    $("#divCampaignSetupAddEditPopup").modal({
        backdrop: "static",
        keyboard: true,
        dynamic: true
    });

    // Send modal slightly further behind on page so as to show date picker pop-up
    $(".modal-backdrop").css("z-index", 1030);

    $("#dpDateStart").datepicker({
        showOn: "focus",
        changeMonth: true,
        changeYear: true
    });

    $("#dpDateEnd").datepicker({
        showOn: "focus",
        changeMonth: true,
        changeYear: true
    });

    ResetAddEditDates();
}

function CancelCampaignPopup() {
    $("#divCampaignSetupAddEditPopup").css("display", "none");
    $("#divCampaignSetupAddEditPopup").modal("hide");

    ClearCampaignAddEditPopup();
}

// ------------------------- Ajax Requests ------------------------- //

function GetCampaignSetupContent() {
    $("#divSetupContent").html("");

    $.ajax({
        url: "/Setup/DisplayCampaignSetupContent/",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        dataType: "json",
        data: {},
        success: function (result) {
            if (result != null && result.isSuccess)
            {
                $("#divSetupContent").html(result.HTML);
            }
            else if (result != null && !result.isSuccess)
            {
                ShowNotification(result.error);
            }
        },
        error: function (result) {
            ShowNotification(result);
        }
    });
}

function ShowEditPopup(campaignID) {
    $("#campaignSetupAddEditTitle").html("Edit Campaign");
    $("#hdnCampaignID").val(campaignID);
    ShowCampaignSetupPopup();

    var jsonPostData = {
        campaignID: campaignID
    }

    $.ajax({
        url: _urlGetSetupEdit,
        contentType: "application/json; charset=utf-8",
        type: "POST",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess)
            {
                $("#txtCampaignName").val(result.campaignName);
                $("#ddAgentForCampaign").val(result.searchRequestID);
                $("#dpDateStart").datepicker("setDate", new Date(result.startDate));
                $("#dpDateEnd").datepicker("setDate", new Date(result.endDate));
            }
            else
            {
                ShowNotification(result.error);
            }
        },
        error: function (result) {
            ShowNotification(result);
        }
    });
}

function StopCampaign(ID) {
    var jsonPostData = {
        campaignID: ID,
        endDate: new Date(Date.now())
    }

    $.ajax({
        url: _urlStopCampaign,
        contentType: "application/json; charset=utf-8",
        type: "POST",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess)
            {
                $("#spanModifiedDate_" + ID).text(result.modifiedDate);
            }
            else
            {
                ShowNotification(result.error);
            }
        },
        error: function (result) {
            ShowNotification(result);
        }
    });
}

function SaveCampaign() {
    var jsonPostData = {
        campaignID: $("#hdnCampaignID").val(),
        campaignName: $("#txtCampaignName").val(),
        agentSRID: $("#ddAgentForCampaign").val(),
        startDate: $("#dpDateStart").datepicker("getDate"),
        endDate: $("#dpDateEnd").datepicker("getDate")
    }

    $.ajax({
        url: _urlSaveCampaign,
        contentType: "application/json; charset=utf-8",
        type: "POST",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess)
            {
                CancelCampaignPopup();
                GetCampaignSetupContent();
                setTimeout(function () {
                    $("#divSetupCampaign_ScrollContent").mCustomScrollbar("scrollTo", "#divCampaignRequest_" + result.CampaignID);
                }, 500);
            }
            else
            {
                ShowNotification(result.error);
            }
        },
        error: function (result) {
            ShowNotification(result);
        }
    });
}

function DeleteCampaign(ID) {
    var jsonPostData = {
        campaignID: ID
    }

    $.ajax({
        url: _urlDeleteCampaign,
        contentType: "application/json; charset=utf-8;",
        type: "POST",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess)
            {
                GetCampaignSetupContent();
            }
            else
            {
                ShowNotification(result.error);
            }
        },
        error: function (result) {
            ShowNotification(result);
        }
    });
}