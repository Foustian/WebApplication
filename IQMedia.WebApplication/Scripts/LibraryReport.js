var _IsReportLoadFailed = true;
var _IsReportLoadOnProgress = false;
var _CurrentReportID = 0;
var _OrigPrimaryGroup = null;
var _OrigSecondaryGroup = null;
var _OrigSort = null;
var _HasPendingSortChanges = false; // Used to check for unsaved changes when closing the Sort Report popup
var _HasCustomSort = false;

$(function () {
    DisplaySavedIQReport();
    $("#divIQReport").on("shown", function () {
        $("#txtReportTitle").focus();
    });

    $('#divSavedIQReportsScrollContent').enscroll({
        horizontalScrolling: true,
        verticalScrolling: true,
        verticalTrackClass: 'track4',
        verticalHandleClass: 'handle4',
        horizontalTrackClass: 'horizontal-track2',
        horizontalHandleClass: 'horizontal-handle2',
        margin: '0 0 0 10px',
        pollChanges: true,
        addPaddingToPane: true
    });

    $('#divMergeReportScroll').mCustomScrollbar({
        advanced: {
            updateOnContentResize: true,
            autoScrollOnFocus: false
        }
    });
});

function OpenInsertLibraryReport() {

    var output = $.map($("#ulIQArchieveResults input[type=checkbox]:checked"), function (n, i) {
        return n.value;
    }).join(',');

    if (output != "") {

        $("#divIQReport button[type=button]").removeAttr("disabled");
        $("#spanReportTitle").html("").hide();
        $("#txtReportTitle").val("");

        var selectedItemCount = output.toString().split(',').length;

        var maxItemReportLimit = parseInt($("#hdnLibraryMaxLibraryReportItems").val());
        if (selectedItemCount > maxItemReportLimit) {
            getConfirm("Max Limit Exceeded", _LibraryMaxLibraryReportItemsMessage.replace(/@@MaxLibraryReportItems@@/g, maxItemReportLimit), "Confirm", "Cancel", function (res) {
                if (res) {
                    $("#divIQReport").modal({
                        backdrop: 'static',
                        keyboard: true,
                        dynamic: true
                    });
                }
            });
        }
        else {
            $("#divIQReport").modal({
                backdrop: 'static',
                keyboard: true,
                dynamic: true
            });
        }

        if ($("#ddlReportFolder option").length <= 0) {
            GetReportFolders();
        }
    }
    else {
        ShowNotification(_msgAtleastOneItemForReport);
    }
}

function SaveIQReport() {

    if (ValidateIQReport()) {


        var output = $.map($("#ulIQArchieveResults input[type=checkbox]:checked"), function (n, i) {
            return n.value;
        }).join(',');

        var jsonPostData = { p_ReportIDs: output, p_ReportTitle: $("#txtReportTitle").val(), p_ReportImage: $("#ddlReportImage").val(), p_FolderID: $("#ddlReportFolder").val() }

        $.ajaxSetup({ cache: false });

        $("#divIQReport button[type=button]").prop("disabled", "disabled");

        $.ajax({
            url: _urlLibraryInsertLibraryReport,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: 'json',
            data: JSON.stringify(jsonPostData),
            success: function (result) {

                CheckForSessionExpired(result);

                if (result.isSuccess) {
                    if (result.isDuplicate) {
                        $("#spanReportTitle").html(_msgReportTitleExists).show();
                    }
                    else {
                        CancelIQReportPopup();
                        ShowNotification(_msgReportSaved);
                        DisplaySavedIQReport();
                        GetClientReportFolder();
                    }
                }
                else {
                    ShowNotification(_msgErrorWhileSavingReport);
                }

                $(".media input[type=checkbox]").each(function () {
                    $(this).prop("checked", false);
                    $(this).closest(".media").css("background", "");
                });

                $("#chkInputAll").prop("checked", false);
                $("#divIQReport button[type=button]").removeAttr("disabled");
            },
            error: function (a, b, c) {
                ShowNotification(_msgSomeErrorProcessing);
                $("#divIQReport button[type=button]").removeAttr("disabled");
            }
        });
    }
}

function ValidateIQReport() {
    var flag = true;
    if ($.trim($("#txtReportTitle").val()) == "") {
        $("#spanReportTitle").html(_msgTitleRequired).show();
        flag = false;
    }

    if ($("#ddlReportFolder").val() == "0") {
        $("#spanReportFolder").html(_msgSelectFolder).show();
        flag = false;
    }

    
    return flag;
}

function CancelIQReportPopup() {

    $("#divIQReport").css({ "display": "none" });
    $("#divIQReport").unbind().modal();
    $("#divIQReport").modal("hide");

    $("#spanReportTitle").html("").hide();
    
    $("#txtReportTitle").val("");
    $("#spanReportFolder").html("").hide();
    $('#ddlReportFolder option').filter(function () { return $(this).html() == rootFolderName; }).prop("selected", "selected");
    $('#ddlReportImage').val('');
}

function DisplaySavedIQReport() {
    $("#imgSavedReportLoading").show();
    $("#divAddtoReport").addClass("blurOnlyControls");
    $("#divAddtoReportMsg").html("Please Wait...");
    _IsReportLoadOnProgress = true;
    $.ajax({
        url: _urlLibraryGetSavedIQReport,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: 'json',
        data: {},
        global: false,
        success: function (result) {
            if ($("#tree").html() != "") {
                $("#imgSavedReportLoading").hide();
                $(".displayReportGrid").show();
            }
            CheckForSessionExpired(result);
            if (result.isSuccess) {
                _IsReportLoadOnProgress = false;
                _IsReportLoadFailed = false;
                $("#divAddtoReport").removeClass("blurOnlyControls");
                $("#divAddtoReportMsg").html("");
                $("#divSavedIQReports").html(result.HTML);

                if (result.reportItems != null) {

                    var reportOptions = '<option value="">&lt; Blank &gt;</option>';
                    $.each(result.reportItems, function (index, item) {
                        reportOptions = reportOptions + '<option value="' + item.Item1 + '">' + EscapeHTML(item.Item2) + '</option>';
                    });
                    $("#ddlReportNames").html(reportOptions);
                }

                setTimeout(function () {
                    $("#divSavedIQReports").width($("#divSavedIQReports").next().width());
                }, 500);
            }
            else {
                _IsReportLoadOnProgress = false;
            }
        },
        error: function (a, b, c) {
            $("#imgSavedReportLoading").hide();
            _IsReportLoadOnProgress = false;
        }
    });
}

function GenerateLibraryReport(reportid) {
    var deferred = $.Deferred();
    CancelReportEditOptions();
    if (reportid != null && reportid > 0) {

        var jsonPostData = { p_ReportID: reportid }

        $.ajax({
            url: _urlLibraryGenerateLibraryReportByID,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: function (result) {

                CheckForSessionExpired(result);

                if (result.isSuccess) {
                    $("#divReportResults").html(result.HTML);
                    if (result.reportSettings != null) {
                        if (result.reportSettings.Sort != null && result.reportSettings.Sort != "") {
                            $('input:radio[name=rdSort]').filter('[value="' + result.reportSettings.Sort + '"]').prop('checked', true);
                        }

                        $('input:checkbox[name=chkShowHide]').filter('[value=TotalAudience]').prop('checked', result.reportSettings.ShowTotalAudience);
                        $('input:checkbox[name=chkShowHide]').filter('[value=TotalMediaValue]').prop('checked', result.reportSettings.ShowTotalMediaValue);
                        $('input:checkbox[name=chkShowHide]').filter('[value=Audience]').prop('checked', result.reportSettings.ShowAudience);
                        $('input:checkbox[name=chkShowHide]').filter('[value=MediaValue]').prop('checked', result.reportSettings.ShowMediaValue);
                        $('input:checkbox[name=chkShowHide]').filter('[value=Sentiment]').prop('checked', result.reportSettings.ShowSentiment);
                        $('input:checkbox[name=chkShowHide]').filter('[value=NationalValue]').prop('checked', result.reportSettings.ShowNationalValues);
                        $('input:checkbox[name=chkShowHide]').filter('[value=TotalNationalAudience]').prop('checked', result.reportSettings.ShowTotalNationalAudience);
                        $('input:checkbox[name=chkShowHide]').filter('[value=TotalNationalMediaValue]').prop('checked', result.reportSettings.ShowTotalNationalMediaValue);
                        $('input:checkbox[name=chkShowHide]').filter('[value=CoverageSources]').prop('checked', result.reportSettings.ShowCoverageSources);

                        $('input:checkbox[name=chkShowHideDashboard]').prop('checked', false);
                        if (result.reportSettings.ChartMediaTypes != null) {
                            $.each(result.reportSettings.ChartMediaTypes, function (index, value) {
                                $('input:checkbox[name=chkShowHideDashboard]').filter('[value=' + value + ']').prop('checked', 'true');
                            });
                        }

                        _OrigSort = $("#divReportEditOptions input[type=radio]:checked").val();
                        _HasCustomSort = result.reportHasCustomSort;
                        if (result.reportHasCustomSort) {
                            $("#rdCustomSort").show();
                            $('input:radio[value="Custom"]').prop('checked', true);
                        }
                        else {
                            $("#rdCustomSort").hide();
                        }

                        if (result.reportSettings.PrimaryGroup) {
                            $("#ddlPrimaryGroup").val(result.reportSettings.PrimaryGroup);
                        }
                        else {
                            $("#ddlPrimaryGroup").val("SubMediaType");
                        }
                        if (result.reportSettings.SecondaryGroup) {
                            $("#ddlSecondaryGroup").val(result.reportSettings.SecondaryGroup);
                        }
                        else {
                            $("#ddlSecondaryGroup").val('CategoryName');
                        }

                        if ($("input[name=chkShowHide]").length == $("input[name=chkShowHide]:checked").length) {
                            $("#chkShowHideAll").prop("checked", true);
                        } else {
                            $("#chkShowHideAll").prop("checked", false);
                        }

                        if ($("input[name=chkShowHideDashboard]").length == $("input[name=chkShowHideDashboard]:checked").length) {
                            $("#chkDashboardItemsAll").prop("checked", true);
                        } else {
                            $("#chkDashboardItemsAll").prop("checked", false);
                        }
                    }
                    $("#txtEditReportTitle").val(result.title);
                    $("#ddlSavedReportImage").val(result.reportImageId);

                    $('input:checkbox[name=chkShowHideDashboard]').each(function () {
                        var medium = $(this).val();

                        if ($(this).prop("checked")) {

                            jsonPostData = { p_ReportID: reportid, p_Medium: medium };

                            $.ajax({
                                url: _urlLibraryGetAdhocMediumData,
                                contentType: "application/json; charset=utf-8",
                                type: "post",
                                dataType: "json",
                                data: JSON.stringify(jsonPostData),
                                success: function (result) {
                                    if (result.isSuccess) {
                                        var divChartId = "div" + medium + "Chart";

                                        $("#" + divChartId).html('');
                                        $("#" + divChartId).html(result.HTML);

                                        if (result.hasData) {
                                            if (result.CategoryDescription == "Overview") {
                                                SetDashboardOverviewHTML(result, "divOverviewChart", function () { return false; }, "divOverviewChart");

                                                $("#divLineChartOptions").hide();

                                                $(".liSubMediaCharts").prop("onclick", null);
                                                $(".liSubMediaCharts").removeClass("cursorPointer");
                                            }
                                            else {
                                                SetDashboardMediumHTML(result, medium, divChartId);

                                                $("#" + divChartId + " #divDuration").hide();
                                                $("#" + divChartId + " #imgDmaList").hide();
                                                $("#" + divChartId + " #imgDmaMap").hide();
                                                $("#" + divChartId + " #imgCountryList").hide();
                                                $(".mediumChartLi img").hide();

                                                if (medium == "NM") {
                                                    $("#" + divChartId + " #divTVDmaMain").hide();
                                                }
                                            }
                                        }
                                    }
                                    else {
                                        ShowNotification(_msgErrorWhileGeneratingReport);
                                        deferred.reject("fail");
                                    }
                                },
                                error: function (a, b, c) {
                                    ShowNotification(_msgSomeErrorProcessing);
                                    deferred.reject("fail");
                                }
                            });

                            $("#div" + medium).show();
                        }
                    });

                    if (screen.height >= 768 && $("#hdnReportID").val() !== _CurrentReportID) {
                        setTimeout(function () { $("#divReportScrollContent").mCustomScrollbar("scrollTo", "top"); }, 200);
                    }
                    deferred.resolve("success"); // this will allow to execute .done method. 

                    _CurrentReportID = $("#hdnReportID").val();
                    SetImageSrc();
                }
                else {
                    ShowNotification(_msgErrorWhileGeneratingReport);
                    deferred.reject("fail");
                }
            },
            error: function (a, b, c) {
                ShowNotification(_msgSomeErrorProcessing);
                deferred.reject("fail");
            }
        });
    }
    return deferred.promise();
}

function GenerateReportPDF(ignoreDuplicates, ignoreSizeLimit) {

    if ($("#hdnReportID").val() == undefined || $("#hdnReportID").val() == "") {
        ShowNotification("No report found to export");
        return;
    }

    var chartHTML = [];
    $('input:checkbox[name=chkShowHideDashboard]:checked').each(function () {
        var jsonChart = { medium: $(this).val(), html: $("#div" + $(this).val()).html() }
        chartHTML.push(jsonChart);
    });

    var jsonPostData = { p_ReportID: $("#hdnReportID").val(), p_IgnoreDuplicates: ignoreDuplicates, p_IgnoreSizeLimit: ignoreSizeLimit, p_ChartHTML: chartHTML }

    $.ajax({
        url: _urlLibraryGenerateReportPDF,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess) {
                $('#divJobStatusExportedPopup').modal({
                    backdrop: 'static',
                    keyboard: true,
                    dynamic: true
                });
            }
            else {
                if (result.hasDuplicate != undefined) {
                    // If the report has already been exported but not yet processed, check if the user wants to do a second export
                    getConfirm("PDF Request Exists", "A PDF export of this report is currently processing. Do you still want to create another export?", "Confirm", "Cancel", function (res) {
                        if (res) {
                            // Ignore size limitations, since if it's gotten to this point that limit has already been checked
                            GenerateReportPDF(true, true);
                        }
                    });
                }
                else if (result.isSizeExceeded != undefined) {
                    if (result.isExportAllowed) {
                        // If an admin user is attempting to export a large report, warn them to notify IT so that we know to keep an eye on it
                        getConfirm("Notify IT", "A report of this size may have trouble processing. Please notify IT so they can monitor its progress. Click OK to continue.", "", "", function (res) {
                            if (res) {
                                GenerateReportPDF(false, true);
                            }
                        });
                    }
                    else {
                        // If the report is too large, require the user to go through support for help, since it can have trouble processing. No confirmation is required, it's just a useful prebuilt popup.
                        getConfirm("Contact Support", "To export reports containing more than 1,000 items, please contact iQ media support for assistance at support@iq.media.", "", "");
                    }
                }
                else {
                    ShowNotification(result.errorMessage);
                }
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgSomeErrorProcessing);
        }
    });
}

function GenerateReportCSV() {

    if ($("#hdnReportID").val() == undefined || $("#hdnReportID").val() == "") {
        ShowNotification(_msgNoReportToExport);
        return;
    }

    var jsonPostData = { p_ReportID: $("#hdnReportID").val() }

    $.ajax({
        url: _urlLibraryGenerateReportCSV,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess) {
                window.location = "/Library/DownloadCSVFile/";
            }
            else {
                ShowNotification(_msgErroWhileDownloadingFile);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgSomeErrorProcessing);
        }
    });
}

function OpenReportModifyPopup() {
    var output = $.map($("#ulIQArchieveResults input[type=checkbox]:checked"), function (n, i) {
        return n.value;
    }).join(',');

    if (output != "") {
        
        $("#divIQReportAppend button[type=button]").removeAttr("disabled");
        $("#spanReportDropDownItem").html("").hide();
        $("#ddlReportNames").val("");

        $("#divIQReportAppend").modal({
            backdrop: 'static',
            keyboard: true,
            dynamic: true
        });

        if (_IsReportLoadFailed && !_IsReportLoadOnProgress) {
            DisplaySavedIQReport();
        }
    }
    else {
        ShowNotification(_msgAtleastOneItemForReport);
    }
}

function CancelIQReportAppendPopup() {
    $("#divIQReportAppend").css({ "display": "none" });
    $("#divIQReportAppend").modal("hide");

    $("#spanReportDropDownItem").html("").hide();
    $("#ddlReportNames").val("");
}

function ModifyIQReport() {

    var output = $.map($("#ulIQArchieveResults input[type=checkbox]:checked"), function (n, i) {
        return n.value;
    }).join(',');

    if (output == "") {
        ShowNotification(_msgAtleastOneItemForReport);
        return;
    }
    if ($("#ddlReportNames").val() == "") {
        $("#spanReportDropDownItem").html(_msgSelectReportToModify).show();
        return;
    }

    var jsonPostData = { p_ReportID: $("#ddlReportNames").val(), p_ArchiveResultIDs: output }

    $.ajax({
        url: _urlLibraryModifyReport,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess) {
                CancelIQReportAppendPopup();
                ShowNotification(result.updateCount + " item(s) added to report successfully");
                $(".media input[type=checkbox]").each(function () {
                    $(this).prop("checked", false);
                    $(this).closest(".media").css("background", "");
                });

                $("#chkInputAll").prop("checked", false);
                if (result.updateCount > 0) {
                    $("#divReportResults").html("");
                }
            }
            else {
                ShowNotification(_msgErroWhileModifyReport);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgSomeErrorProcessing);
        }
    });
}

function EnableReportEditOptions() {
    if ($("#hdnReportID").val() == undefined || $("#hdnReportID").val() == "") {
        ShowNotification(_msgNoReportToEdit);
        return;
    }
    $("#divReportEditOptions").show();
    $("#divReportScrollContent input[type=checkbox]").show();

    _OrigPrimaryGroup = $("#ddlPrimaryGroup").val();
    _OrigSecondaryGroup = $("#ddlSecondaryGroup").val();
}
function CancelReportEditOptions() {
    $("#divReportEditOptions").hide();
    $("#divReportScrollContent input[type=checkbox]").hide();
    $("#divReportScrollContent input[type=checkbox]").each(function () {
        $(this).prop("checked", false);
    });
}

function RemoveReportItems() {
    var output = $.map($("#divReportScrollContent input[type=checkbox]:checked"), function (n, i) {
        return n.value;
    }).join(',');

    if (output == "") {
        ShowNotification(_msgSelectRecordToDelete);
        return;
    }
    if ($("#hdnReportID") == undefined || $("#hdnReportID").val() == "") {
        ShowNotification(_msgNoReportToRemove);
        return;
    }

    getConfirm("Edit Report", "Remove Selected Items", "Confirm Deletion", "Cancel", function (res) {
        if (res) {
            var jsonPostData = { p_ReportID: $("#hdnReportID").val(), p_ArchiveResultIDs: output }

            $.ajax({
                url: _urlLibraryRemoveReportItems,
                contentType: "application/json; charset=utf-8",
                type: "post",
                dataType: "json",
                data: JSON.stringify(jsonPostData),
                success: function (result) {
                    if (result.isSuccess) {

                        ShowNotification(result.deleteCount + " item(s) removed successfully");

                        if (result.deleteCount > 0) {
                            // Render modified report after removing of items
                            CancelReportEditOptions();
                            GenerateLibraryReport($("#hdnReportID").val());
                            
                        }
                    }
                    else {
                        ShowNotification(_msgErroWhileModifyReport);
                    }
                },
                error: function (a, b, c) {
                    ShowNotification(_msgSomeErrorProcessing);
                }
            });
        }
    });
}

function DeleteReportByReportID() {

    if ($("#hdnReportID").val() == undefined || $("#hdnReportID").val() == "") {
        ShowNotification(_msgNoReportToDelete);
        return;
    }

    getConfirm("Delete Report", $("#h4DisplayReportTitle").html(), "Confirm Report Deletion", "Cancel", function (res) {
        if (res) {

            var jsonPostData = { p_ReportID: $("#hdnReportID").val() }

            $.ajax({
                url: _urlLibraryRemoveReportByID,
                contentType: "application/json; charset=utf-8",
                type: "post",
                dataType: "json",
                data: JSON.stringify(jsonPostData),
                success: function (result) {
                    if (result.isSuccess) {
                        DisplaySavedIQReport();
                        GetClientReportFolder();
                        $("#divReportResults").html("");
                        CancelReportEditOptions();
                        ShowNotification(_msgReportDeleted);
                    }
                    else {
                        ShowNotification(_msgErrorWhileDeletingReport);
                    }
                },
                error: function (a, b, c) {
                    ShowNotification(_msgSomeErrorProcessing);
                }
            });
        }
    });
}

function SaveReport(isSaveAs) {
    var txtTitle = null;
    if (isSaveAs) {
        txtTitle = $("#txtSaveReportTitle");
    }
    else {
        txtTitle = $("#txtEditReportTitle");
    }

    if ($.trim(txtTitle.val()) == "") {
        ShowNotification(_msgTitleRequired);
        return false;
    }

    var primaryGroup = $("#ddlPrimaryGroup").val();
    var secondaryGroup = $("#ddlSecondaryGroup").val();
    var selectedSort = $("#divReportEditOptions input[type=radio]:checked").val();

    if (_HasCustomSort && (primaryGroup != _OrigPrimaryGroup || secondaryGroup != _OrigSecondaryGroup || selectedSort != "Custom")) {
        getConfirm("Update Report", _msgReportConfirmGroupChange, "Confirm", "Cancel", function (res) {
            if (res) {
                SaveReport_Helper(isSaveAs, true);
            }
        });
    }
    else {
        SaveReport_Helper(isSaveAs, false);
    }
}

function SaveReport_Helper(isSaveAs, resetSort) {
    var txtTitle = null;
    if (isSaveAs) {
        txtTitle = $("#txtSaveReportTitle");
    }
    else {
        txtTitle = $("#txtEditReportTitle");
    }

    var showHideSettings = [];
    var chartMediaTypes = [];
    $("#divReportEditOptions input[type=checkbox]:checked").each(function () {
        if ($(this).val() != "All") {
            if ($(this).attr("name") == "chkShowHideDashboard") {
                chartMediaTypes.push($(this).val());
            }
            else {
                showHideSettings.push($(this).val());
            }
        }
    });

    // Maintain the original sort selection to revert back to if the custom sort is undone
    var sortSettings = $("#divReportEditOptions input[type=radio]:checked").val();
    if (sortSettings == "Custom") {
        sortSettings = _OrigSort;
    }

    var jsonPostData = {
        p_ReportID: $("#hdnReportID").val(),
        p_ShowHideSettings: showHideSettings,
        p_SortSettings: sortSettings,
        p_ReportImageID: $("#ddlSavedReportImage").val(),
        p_IsSaveAs: isSaveAs,
        p_ReportTile: txtTitle.val().trim(),
        p_PrimaryGroup: $("#ddlPrimaryGroup").val(),
        p_SecondaryGroup: $("#ddlSecondaryGroup").val(),
        p_ResetSort: resetSort,
        p_ChartMediaTypes: chartMediaTypes
    }

    $.ajax({
        url: _urlLibrarySaveReportWithSettings,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess) {
                ShowNotification("Updating Report...");
                $("#divReportContent").addClass("blur");
                DisplaySavedIQReport();
                GetClientReportFolder();
                CancelReportEditOptions();

                // Attach a done, fail, and progress handler for the asyncEvent
                $.when(GenerateLibraryReport(result.reportId)).then(
                  function (status) {
                      $('#divNotification').remove();
                      ShowNotification(_msgReportSaved);
                      $("#divReportContent").removeClass("blur");
                  },
                  function (status) {
                      $('#divNotification').remove();
                      ShowNotification(_msgErrorOccured);
                      $("#divReportContent").removeClass("blur");
                  },
                  function (status) {
                      ShowLoading("Updating Report...");
                  }
                );
            }
            else {
                if (result.isDuplicate) {
                    ShowNotification(_msgReportTitleExists);
                }
                else {
                    ShowNotification(_msgErrorOccured);
                }
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgSomeErrorProcessing);
        }
    });
}

function SaveReportAs() {
    $('#divReportPopover').remove();
    $('#btnSaveReportAs').popover({
        trigger: 'manual',
        html: true,
        title: '',
        placement: 'right',
        template: '<div id="divReportPopover" class="popover"><div class="arrow"></div><div class="popover-inner"><div class="popover-content" style="padding:9px 14px;" ><p></p></div></div></div>',
        content: '<input type="text" placeholder="Report Title" id="txtSaveReportTitle" /><div><input type="button"  class="cancelButton marginbottom0"  style="margin-top:0px !important;" value="Cancel"  onclick="$(\'#divReportPopover\').remove();" /><input type="button" id="btnSaveLibReportAs" class="button marginbottom0" style="margin-left:10px !important;margin-top:0px !important;" value="Save" onclick="SaveReport(true);" /><img src="../../Images/Loading_1.gif" class="displayNone" id="imgSaveSearchLoading" /></div>'
    });

    $('#btnSaveReportAs').popover('show');
}

function OpenMergeReportsPopup() {
    $.ajax({
        url: _urlLibraryGetSavedIQReport,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: 'json',
        data: {},
        global: false,
        success: function (result) {
            if (result.isSuccess && result.reportItems != null) {
                var maxReportItems = parseInt($("#hdnLibraryMaxLibraryReportItems").val());
                var currentItems = 0;
                var reportID = $("#hdnReportID").val();
                var html = '';
                $.each(result.reportItems, function (index, item) {
                    var liHTML = '<li><input type="checkbox" style="margin-top:-3px;margin-right:3px;" value="' + item.Item1 + '" recordCount="' + item.Item3 + '"' + (item.Item1 == reportID ? ' checked="checked"' : '') + '/>' + EscapeHTML(item.Item2) + '&nbsp;&nbsp;(' + item.Item3 + ')</li>'

                    // Put the current report at the top of the list
                    if (item.Item1 == reportID) {
                        html = liHTML + html;
                        currentItems = item.Item3;
                    }
                    else {
                        html += liHTML;
                    }
                });
                html = '<ul id="ulMergeReports">' + html + '</ul>';

                $("#spanMergeReportCurrReports").html("1");
                $("#spanMergeReportTotalReports").html(result.reportItems.length);

                $("#divMergeReportScroll").css("height", result.reportItems.length * 22);

                $("#spanMergeReportCurrItems").html(currentItems);
                if (currentItems > maxReportItems) {
                    $("#spanMergeReportCount").css("color", "#FF0000");
                }
                else {
                    $("#spanMergeReportCount").css("color", "");
                }

                $("#divMergeReportOptions").html(html);

                // Give the user a count of the items in the merged report
                $("#ulMergeReports input[type=checkbox]").change(function () {
                    GetMergedReportItemCount(maxReportItems);
                });

                $("#divMergeReportsPopup").modal({
                    backdrop: 'static',
                    keyboard: true,
                    dynamic: true
                });
            }
            else {
                ShowNotification(result.errorMessage);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured);
        }
    });
}

function GetMergedReportItemCount(maxReportItems) {
    var reportIDs = $.map($("#ulMergeReports input[type=checkbox]:checked"), function (n, i) {
        return n.value;
    });

    $("#spanMergeReportCurrReports").html(reportIDs.length);

    if (reportIDs.length > 0) {
        var jsonPostData = {
            reportIDs: reportIDs
        }

        $.ajax({
            url: "/Library/GetMergedReportItemCount/",
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: 'json',
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                if (result.isSuccess) {
                    $("#spanMergeReportCurrItems").html(result.itemCount);
                    if (result.itemCount > maxReportItems) {
                        $("#spanMergeReportCount").css("color", "#FF0000");
                    }
                    else {
                        $("#spanMergeReportCount").css("color", "");
                    }                    
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
    else {
        $("#spanMergeReportCurrItems").html("0");
        $("#spanMergeReportCount").css("color", "");
    }
}

function SaveMergedReport() {
    var reportIDs = $.map($("#ulMergeReports input[type=checkbox]:checked"), function (n, i) {
        return n.value;
    });

    $("#spanMergedReportTitle").html("").hide();
    $("#spanMergedReportFolder").html("").hide();
    $("#spanMergeReportOptions").html("").hide();

    if (ValidateMergeReportInputs(reportIDs)) {
        var jsonPostData = {
            reportTitle: $.trim($("#txtMergedReportTitle").val()),
            reportImage: $("#ddlMergedReportImage").val(), 
            folderID: $("#ddlMergedReportFolder").val(),
            reportIDs: reportIDs
        }

        $.ajax({
            url: _urlLibraryMergeReports,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: 'json',
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                if (result.isSuccess) {
                    if (result.isDuplicate) {
                        $("#spanMergedReportTitle").html(_msgReportTitleExists).show();
                    }
                    else if (result.exceedsMaxLimit) {
                        $("#spanMergeReportOptions").html(_msgMergeReportExceedsLimit.replace("{0}", $("#hdnLibraryMaxLibraryReportItems").val()));
                    }
                    else {
                        CancelMergeReportsPopup();
                        ShowNotification(_msgReportSaved);
                        DisplaySavedIQReport();
                        GetClientReportFolder();
                    }
                }
                else {
                    ShowNotification(_msgErrorWhileSavingReport);
                }
            },
            error: function (a, b, c) {
                ShowNotification(_msgErrorOccured);
            }
        });
    }
}

function ValidateMergeReportInputs(reportIDs) {
    var flag = true;

    if ($.trim($("#txtMergedReportTitle").val()) == "") {
        $("#spanMergedReportTitle").html(_msgTitleRequired).show();
        flag = false;
    }
    if ($("#ddlMergedReportFolder").val() == "0") {
        $("#spanMergedReportFolder").html(_msgSelectFolder).show();
        flag = false;
    }
    if (reportIDs.length < 2) {
        $("#spanMergeReportOptions").html(_msgMergeReportSelectTwo).show();
        flag = false;
    }
    if (parseInt($("#spanMergeReportCurrItems").html()) > parseInt($("#hdnLibraryMaxLibraryReportItems").val())) {
        $("#spanMergeReportOptions").html(_msgMergeReportExceedsLimit.replace("{0}", $("#hdnLibraryMaxLibraryReportItems").val())).show();
        flag = false;    
    }

    return flag;
}

function CancelMergeReportsPopup() {
    $("#divMergeReportsPopup").css({ "display": "none" });
    $("#divMergeReportsPopup").modal("hide");

    $("#txtMergedReportTitle").val("");
    $("#spanMergedReportTitle").html("").hide();
    $('#ddlMergedReportFolder option').filter(function () { return $(this).html() == rootFolderName; }).prop("selected", "selected");
    $("#spanMergedReportFolder").html("").hide();
    $('#ddlMergedReportImage').val("");
    $("#spanMergeReportOptions").html("").hide();
}

function SetupReportForSort() {
    $('#divSortReportPopup').modal({
        backdrop: 'static',
        keyboard: true,
        dynamic: true
    });

    $("#divSortReportPopup").draggable({
        iframeFix: true,
        start: function () {
            ifr = $('#iFrameSortReport');
            var d = $('<div></div>');

            $('#divSortReportPopup').append(d[0]);
            d[0].id = 'temp_div';
            d.css({ position: 'absolute' });
            d.css({ top: ifr.position().top, left: 0 });
            d.height(ifr.height());
            d.width('100%');
        },
        stop: function () {
            $('#temp_div').remove();
        }
    });

    $('#iFrameSortReport').attr("src", "//" + window.location.hostname + "/Library/ReportSort?reportID=" + $("#hdnReportID").val());

    $('#divSortReportPopup').css("position", "");
    $('#divSortReportPopup').css("height", documentHeight - 200);
    $('#iFrameSortReport').css("height", documentHeight - 200);
}

function CloseSortReportPopup() {
    if (_HasPendingSortChanges) {
        getConfirm("Cancel Changes", _msgReportSortUnsavedChanges, "Confirm", "Cancel", function (res) {
            if (res) {
                CloseSortReportPopup_Helper();
            }
        });
    }
    else {
        CloseSortReportPopup_Helper();
    }
}

function CloseSortReportPopup_Helper() {
    _HasPendingSortChanges = false;

    $("#divSortReportPopup").css({ "display": "none" });
    $("#divSortReportPopup").modal("hide");

    GenerateLibraryReport($("#hdnReportID").val());
}

function CloseJobStatusPopUp() {
    $("#divJobStatusExportedPopup").css({ "display": "none" });
    $("#divJobStatusExportedPopup").modal("hide");
}
