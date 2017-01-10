var CONST_ZERO = "0";
var CONST_DeleteNoteIQAgent = "Note:  By deleting this agent, you are also deleting all content that has been populated into your Feeds tab and resulting metrics displayed in your Dashboard tab.  Depending on the amount of content that you are deleting, the timing on content being removed from feeds and updated in your dashboard will vary.  You will receive an email notification once the content has been removed from Feeds and updated in your Dashboard.";
var CONST_SuspendNoteIQAgent = "Note:  By suspending this agent, new content will no longer populate for the agent. Existing content will be kept and displayed.";
var TotalTabs = 0;
var _PreviousZipCodes = [];
var _PreviousExcludeZipCodes = [];
var _PreviousZipCodes_IQRadio = [];
var _PreviousExcludeZipCodes_IQRadio = [];
var searchTermCapitalized = false;
var _HasUnsavedTwitterRule = false;
var _HasUnsavedTVEyesRule = false;

$(function () {

    //    $("#divIQAgentSetupTabsHeader").delegate("div", "click", function () {
    //        ShowIQAgentSetupTabs($(this).index());
    //    });

    $(".show-hide input").click(function (event) {
        event.stopPropagation();
    });

    $(".chosen-select").chosen({
        display_disabled_options: true,
        default_item: CONST_ZERO,
        width: "93%"
    });

    $("#ddlIQAgentSetupDMA_TV").chosen().change(function (event) {
        SetZipCodeEnabled();
    });

    $("#txtIQAgentSetupZipCodes").blur(function () {
        LookupDMAs(true, false);
    });

    $("#txtIQAgentSetupExcludeZipCodes").blur(function () {
        LookupDMAs(true, true);
    });

    $("#txtIQAgentSetupFBPageID").blur(function () {
        LookupFBPageUrls(false);
    });

    $("#txtIQAgentSetupFBPage").blur(function () {
        LookupFBPageIDs(false);
    });

    $("#txtIQAgentSetupExcludeFBPageID").blur(function () {
        LookupFBPageUrls(true);
    });

    $("#txtIQAgentSetupExcludeFBPage").blur(function () {
        LookupFBPageIDs(true);
    });

    $("#ddlIQAgentSetupDMA_IQRadio").chosen().change(function (event) {
        SetIQRadioZipCodeEnabled();
    });

    $("#txtIQAgentSetupZipCodes_IQRadio").blur(function () {
        LookupIQRadioDMAs(true, false);
    });

    $("#txtIQAgentSetupExcludeZipCodes_IQRadio").blur(function () {
        LookupIQRadioDMAs(true, true);
    });

    $("#txtIQAgentSetupBrandId_LR").blur(function () {
        var txtBrandId = $('#txtIQAgentSetupBrandId_LR').val();
        if (txtBrandId != null && txtBrandId.length > 0) {
            AddSearchImageIds(txtBrandId);
        }
        else {
            $("#spantxtIQAgentSetupBrandId_LR").html("").show();
        }
    });
    $("#txtIQAgentSetupBrandId_LR").keyup(function () {
        var txtBrandId = $('#txtIQAgentSetupBrandId_LR').val();
        if (txtBrandId != null && txtBrandId.length > 0 && txtBrandId.slice(-1) == ';') {
            txtBrandId = txtBrandId.substring(0, txtBrandId.length - 1);
            AddSearchImageIds(txtBrandId);
        }
    });
    $("#txtIQAgentSetupSearchImageId_LR").blur(function () {
        var txtSearchId = $('#txtIQAgentSetupSearchImageId_LR').val();
        if (txtSearchId != null && txtSearchId.length > 0) {
            UpdateDisplayedLogos();
        }
        else {
            $("#spantxtIQAgentSetupSearchImageId_LR").html("").show();
            $("#divLRBrandLogos").html("");
        }
    });

    $("#imgTagUserHelp").popover();
});

function AddSearchImageIds(brandId) {
    //get the search image ids
    $("#btnSubmitIQAgentSetupAddEditForm").attr("disabled", "disabled");
    $("#spantxtIQAgentSetupBrandId_LR").html("").show();

    if (brandId != null) {
        var jsonPostData = { BrandID: brandId };

        $.ajax({
            url: "/Setup/GetSearchImageIDs/",
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                if (result != null && result.isSuccess) {
                    if (result.lstSearchImageIds.length > 0) {
                        $.each(result.lstSearchImageIds, function (index, entry) {
                            if ($("#txtIQAgentSetupSearchImageId_LR").val() == null || $("#txtIQAgentSetupSearchImageId_LR").val().indexOf(entry) == -1) {
                                if ($("#txtIQAgentSetupSearchImageId_LR").val() != null && $("#txtIQAgentSetupSearchImageId_LR").val().trim() != '') {
                                    $("#txtIQAgentSetupSearchImageId_LR").val($("#txtIQAgentSetupSearchImageId_LR").val() + ";");
                                }

                                $("#txtIQAgentSetupSearchImageId_LR").val($("#txtIQAgentSetupSearchImageId_LR").val() + entry);
                            }
                            $("#txtIQAgentSetupBrandId_LR").val('');
                        });

                        UpdateDisplayedLogos();
                    }
                    else {
                        $("#spantxtIQAgentSetupBrandId_LR").html(_msgInvalidBrandID).show();
                    }
                }
                else {
                    ShowNotification(_msgErrorOccured);
                }
                $("#btnSubmitIQAgentSetupAddEditForm").removeAttr("disabled");
            },
            error: function (a, b, c) {
                ShowNotification(_msgErrorOccured);
                $("#btnSubmitIQAgentSetupAddEditForm").removeAttr("disabled");
            }
        });
    }
    $("#btnSubmitIQAgentSetupAddEditForm").removeAttr("disabled");
}

function UpdateDisplayedLogos() {
    var invalidIDs = '';
    $("#spantxtIQAgentSetupSearchImageId_LR").html("").show();

    var txtSearchIds = $("#txtIQAgentSetupSearchImageId_LR").val();
    if (txtSearchIds != null) txtSearchIds = txtSearchIds.trim();
    if (txtSearchIds != null && txtSearchIds.length > 0 && txtSearchIds.slice(-1) == ';') $("#txtIQAgentSetupSearchImageId_LR").val(txtSearchIds.substring(0, txtSearchIds.length - 1));

    var LRSearchIDs = $("#txtIQAgentSetupSearchImageId_LR").val();
    if (LRSearchIDs != null) {
        $("#btnSubmitIQAgentSetupAddEditForm").attr("disabled", "disabled");

        LRSearchIDs = LRSearchIDs.split(";");
        var jsonPostData = { LRSearchIDs: LRSearchIDs };

        $.ajax({
            url: "/Setup/GetSearchImagesById/",
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                if (result != null && result.isSuccess) {
                    if (result.lstInvalidIDs != '') {
                        $("#spantxtIQAgentSetupSearchImageId_LR").html(_msgInvalidSearchImageID + result.lstInvalidIDs).show();
                        $("#divLRBrandLogos").html("");
                    }
                    else {
                        //foreach brand display all logos
                        $("#divLRBrandLogos").html("");
                        var divContent = '';
                        $.each(result.lstSearchImages, function (index, entry) {
                            divContent += '<div style="width:50px; height:50px; padding-left:10px;" class="float-left"><img src="' + entry + '" title="' + result.lstSearchImageIDs[index] + '" style="display: block; margin: auto; vertical-align: middle; height: auto; width:auto;"/></div>';
                        });
                        $("#divLRBrandLogos").html(divContent);
                    }
                }
                else {
                    ShowNotification(_msgErrorOccured);
                }
                $("#btnSubmitIQAgentSetupAddEditForm").removeAttr("disabled");
            },
            error: function (a, b, c) {
                ShowNotification(_msgErrorOccured);
                $("#btnSubmitIQAgentSetupAddEditForm").removeAttr("disabled");
            }
        });
    }
}

function ShowIQAgentSetupAddEditPopup(id) {

    ClearIQAgentSearchRequestInfo();

    if (id <= 0) {

        // These sections should be display only, not editable
        $("#divTMSetup").hide();
        $("#divTMTabContent").hide();
        $("#divPMSetup").hide();
        $("#divPMTabContent").hide();
        $("#divTWSetup").hide();
        $("#divTWTabContent").hide();
        $("#divIGSetup").hide();
        $("#divIGTabContent").hide();

        // Can only edit Twitter, TVEyes rules on existing agents
        $("#imgEditTwitterRule").addClass("displayNone");
        $("#imgEditTVEyesRule").addClass("displayNone");
        
        $("#divIQAgentSetupPopupTitle").html("Create IQAgent");
        $("#divIQAgentSetupAddEditPopup").modal({
            backdrop: "static",
            keyboard: true,
            dynamic: true
        });
    }
    else {
        $("#divTMSetup").show();
        $("#divPMSetup").show();
        $("#divTWSetup").show();

        var jsonPostData = { p_ID: id }
        $("#divIQAgentSetupPopupTitle").html("Update IQAgent");
        $.ajax({
            url: "/Setup/GetIQAgentSetupAddEditForm/",
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: function (result) {

                if (result != null && result.isSuccess) {

                    $("#hdnIQAgentSetupAddEditKey").val(result.iqAgentKey);

                    FillIQAgentMediaInformation(result.queryName, result.searchRequestObject);

                    $("#divIQAgentSetupAddEditPopup").modal({
                        backdrop: "static",
                        keyboard: true,
                        dynamic: true
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
}

function CloseIQAgentSetupPopup() {
    if (_HasUnsavedTVEyesRule || _HasUnsavedTwitterRule) {
        var pendingSections = "";
        var trackGuid = null;
        var tvEyesSettingsKey = null;
        if (_HasUnsavedTVEyesRule) {
            pendingSections = "Radio";
            tvEyesSettingsKey = $("#hdnIQAgentSetupTVEyesSettingsKey").val();
        }
        if (_HasUnsavedTwitterRule) {
            pendingSections += (pendingSections.length == 0 ? "" : ", ") + "Twitter";
            trackGuid = $("#txtIQAgentSetupGnipTag_TW").val();
        }

        getConfirm("Confirm Cancel", "There are newly created search rules for these sections: " + pendingSections + ". If you close the agent, they will be lost. Do you want to continue?", "Continue", "Cancel", function (res) {
            if (res == true) {
                var jsonPostData = {
                    searchRequestID: $("#hdnIQAgentSetupAddEditKey").val(),
                    trackGuid: trackGuid,
                    tvEyesSettingsKey: tvEyesSettingsKey
                }

                $.ajax({
                    url: _urlSetupAgentDeleteExternalRules,
                    contentType: "application/json; charset=utf-8",
                    type: "post",
                    dataType: "json",
                    data: JSON.stringify(jsonPostData),
                    success: function (result) {
                        if (result.isSuccess) {
                            CancelIQAgentPopup('divIQAgentSetupAddEditPopup');
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
        });
    }
    else {
        CancelIQAgentPopup('divIQAgentSetupAddEditPopup');
    }
}

function CancelIQAgentPopup(divModalPopupID) {
    $("#" + divModalPopupID).css({ "display": "none" });
    $("#" + divModalPopupID).modal("hide");
}

function ClearIQAgentSearchRequestInfo() {    

    $("#btnSubmitIQAgentSetupAddEditForm").show();
    $("#frmIQAgentSetupAddEdit span").html("").hide();

    $("#hdnIQAgentSetupAddEditKey").val(CONST_ZERO);
    ShowHideTabdiv(0,true);

    _HasUnsavedTVEyesRule = false;
    _HasUnsavedTwitterRule = false;

    $("#chkIQAgentSetup_TV").prop("checked", true);
    $("#chkIQAgentSetup_NM").prop("checked", true);
    $("#chkIQAgentSetup_SM").prop("checked", true);
    $("#chkIQAgentSetup_FB").prop("checked", false);
    $("#chkIQAgentSetup_IG").prop("checked", false);
    $("#chkIQAgentSetup_TW").prop("checked", false);
    $("#chkIQAgentSetup_TM").prop("checked", false);
    $("#chkIQAgentSetup_PM").prop("checked", false);
    $("#chkIQAgentSetup_PQ").prop("checked", true);
    $("#chkIQAgentSetup_FO").prop("checked", true);
    $("#chkIQAgentSetup_BL").prop("checked", true);
    $("#chkIQAgentSetup_LN").prop("checked", true);
    $("#chkIQAgentSetup_IQRadio").prop("checked", true);

    $("#txtIQAgentSetupSearchTerm").val("");
    $("#txtIQAgentSetupTitle").val("");

    // TV fields
    $("#txtIQAgentSetupProgramTitle").val("");
    $("#txtIQAgentSetupAppearing").val("");
    $("#txtIQAgentSetupSearchTerm_TV").val("");
    $("#txtIQAgentSetupZipCodes").val("");
    $("#txtIQAgentSetupExcludeZipCodes").val("");
    $("#chkIQAgentSetupUserMasterSearchTerm_TV").prop("checked", true);

    $("#ddlIQAgentSetupCategory_TV").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupDMA_TV").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupStation_TV").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupAffiliate_TV").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupRegion_TV").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupCountry_TV").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupExcludeDMA_TV").val(CONST_ZERO).trigger("chosen:updated");

    SetZipCodeEnabled();

    // NM fields
    $("#txtIQAgentSetupPublication_NM").val("");
    $("#txtIQAgentSetupSearchTerm_NM").val("");
    $("#chkIQAgentSetupUserMasterSearchTerm_NM").prop("checked", true);

    $("#ddlIQAgentSetupCategory_NM").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupPublicationCategory_NM").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupGenere_NM").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupRegion_NM").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupLanguage_NM").val("English").trigger("chosen:updated");
    $("#ddlIQAgentSetupCountry_NM").val(CONST_ZERO).trigger("chosen:updated");

    // SM fields
    $("#txtIQAgentSetupSource_SM").val("");
    $("#txtIQAgentSetupAuthor_SM").val("");
    $("#txtIQAgentSetupTitle_SM").val("");
    $("#txtIQAgentSetupSearchTerm_SM").val("");
    $("#chkIQAgentSetupUserMasterSearchTerm_SM").prop("checked", true);

    $("#ddlIQAgentSetupSourceType_SM").val(CONST_ZERO).trigger("chosen:updated");

    // BL fields
    $("#txtIQAgentSetupSource_BL").val("");
    $("#txtIQAgentSetupAuthor_BL").val("");
    $("#txtIQAgentSetupTitle_BL").val("");
    $("#txtIQAgentSetupSearchTerm_BL").val("");
    $("#chkIQAgentSetupUserMasterSearchTerm_BL").prop("checked", true);

    // FO fields
    $("#txtIQAgentSetupSource_FO").val("");
    $("#txtIQAgentSetupAuthor_FO").val("");
    $("#txtIQAgentSetupTitle_FO").val("");
    $("#txtIQAgentSetupSearchTerm_FO").val("");
    $("#chkIQAgentSetupUserMasterSearchTerm_FO").prop("checked", true);

    $("#ddlIQAgentSetupSourceType_FO").val(CONST_ZERO).trigger("chosen:updated");

    // FB fields
    $("#txtIQAgentSetupSearchTerm_FB").val("");
    $("#chkIQAgentSetupUserMasterSearchTerm_FB").prop("checked", true);

    $("#chkIQAgentSetupIncludeDefault").prop("checked", true);
    $("#txtIQAgentSetupFBPageID").val("");
    $("#txtIQAgentSetupFBPage").val("");
    $("#txtIQAgentSetupExcludeFBPageID").val("");
    $("#txtIQAgentSetupExcludeFBPage").val("");

    // IG fields
    $("#txtIQAgentSetupSearchTerm_IG").val("");
    $("#chkIQAgentSetupUserMasterSearchTerm_IG").prop("checked", false);
    $("#txtIQAgentSetupIGTag").val("");

    // TW fields
    $("#txtIQAgentSetupGnipTag_TW").val("");

    // TM fields
    $("#hdnIQAgentSetupTVEyesSettingsKey").val("0");
    $("#txtIQAgentSetupTVEyesSearchGUID_TM").val("");

    // PM fields
    $("#txtIQAgentSetupBLPMIXXml_PM").val("");

    // PQ fields
    $("#txtIQAgentSetupPublication_PQ").val("");
    $("#txtIQAgentSetupAuthor_PQ").val("");
    $("#ddlIQAgentSetupLanguage_PQ").val("English").trigger("chosen:updated");
    $("#txtIQAgentSetupSearchTerm_PQ").val("");
    $("#chkIQAgentSetupUserMasterSearchTerm_PQ").prop("checked", true);

    // LN fields
    $("#txtIQAgentSetupPublication_LN").val("");
    $("#txtIQAgentSetupSearchTerm_LN").val("");
    $("#chkIQAgentSetupUserMasterSearchTerm_LN").prop("checked", true);

    $("#ddlIQAgentSetupCategory_LN").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupPublicationCategory_LN").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupGenere_LN").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupRegion_LN").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupLanguage_LN").val("English").trigger("chosen:updated");
    $("#ddlIQAgentSetupCountry_LN").val(CONST_ZERO).trigger("chosen:updated");

    // IQRadio fields
    $("#txtIQAgentSetupSearchTerm_IQRadio").val("");
    $("#txtIQAgentSetupZipCodes_IQRadio").val("");
    $("#txtIQAgentSetupExcludeZipCodes_IQRadio").val("");
    $("#chkIQAgentSetupUserMasterSearchTerm_IQRadio").prop("checked", true);

    $("#ddlIQAgentSetupDMA_IQRadio").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupStation_IQRadio").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupRegion_IQRadio").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupCountry_IQRadio").val(CONST_ZERO).trigger("chosen:updated");
    $("#ddlIQAgentSetupExcludeDMA_IQRadio").val(CONST_ZERO).trigger("chosen:updated");

    SetIQRadioZipCodeEnabled();

    // Exclude Domain fields
    $("#txtIQAgentSetupExcludeDomains_NM").val("");
    $("#txtIQAgentSetupExcludeDomains_SM").val("");
    $("#txtIQAgentSetupExcludeDomains_LN").val("");
    $("#txtIQAgentSetupExcludeDomains_BL").val("");
    $("#txtIQAgentSetupExcludeDomains_FO").val("");
}

function FillIQAgentMediaInformation(queryName, searchRequestObject) {

    if (searchRequestObject != null) {

        $("#chkIQAgentSetup_TV").prop("checked", searchRequestObject.TVSpecified);
        $("#chkIQAgentSetup_NM").prop("checked", searchRequestObject.NewsSpecified);
        $("#chkIQAgentSetup_SM").prop("checked", searchRequestObject.SocialMediaSpecified);
        $("#chkIQAgentSetup_BL").prop("checked", searchRequestObject.BlogSpecified);
        $("#chkIQAgentSetup_FO").prop("checked", searchRequestObject.ForumSpecified);
        $("#chkIQAgentSetup_FB").prop("checked", searchRequestObject.FacebookSpecified);
        $("#chkIQAgentSetup_IG").prop("checked", searchRequestObject.InstagramSpecified);
        $("#chkIQAgentSetup_TW").prop("checked", searchRequestObject.TwitterSpecified);
        $("#chkIQAgentSetup_TM").prop("checked", searchRequestObject.TMSpecified);
        $("#chkIQAgentSetup_PM").prop("checked", searchRequestObject.PMSpecified);
        $("#chkIQAgentSetup_PQ").prop("checked", searchRequestObject.PQSpecified);
        $("#chkIQAgentSetup_LN").prop("checked", searchRequestObject.LexisNexisSpecified);
        $("#chkIQAgentSetup_LR").prop("checked", searchRequestObject.LRSpecified);
        $("#chkIQAgentSetup_IQRadio").prop("checked", searchRequestObject.IQRadioSpecified);

        // Fill "Title(Query Name)"

        if (queryName != null) {
            $("#txtIQAgentSetupTitle").val(queryName);
        }

        // Fill "Search Term"
        if (searchRequestObject.SearchTerm != null) {
            $("#txtIQAgentSetupSearchTerm").val(searchRequestObject.SearchTerm);
        }

        if ($("#divTMSetup").length) {
            // Unnecessary if the section doesn't exist
            ShowHideEditTVEyesRule();
        }
        if (searchRequestObject.TMSpecified) {
            // radio
            if (searchRequestObject.TM != null && searchRequestObject.TMSpecified == true) {
                if (searchRequestObject.TM.TVEyesSettingsKey != null) {
                    $("#hdnIQAgentSetupTVEyesSettingsKey").val(searchRequestObject.TM.TVEyesSettingsKey);
                }

                if (searchRequestObject.TM.TVEyesSearchGUID != null && searchRequestObject.TM.TVEyesSearchGUID != "") {
                    $("#txtIQAgentSetupTVEyesSearchGUID_TM").val(searchRequestObject.TM.TVEyesSearchGUID);
                }
                else if (searchRequestObject.TM.TVEyesSettingsKey > 0) {
                    $("#txtIQAgentSetupTVEyesSearchGUID_TM").val("Generating...");
                }
            }
        }
        if (searchRequestObject.PMSpecified) {
            // pm
            if (searchRequestObject.PM != null) {

                if (searchRequestObject.PM.SearchTerm != null) {
                    $("#txtIQAgentSetupSearchTerm_PM").val(searchRequestObject.PM.SearchTerm.SearchTerm);
                    $("#chkIQAgentSetupUserMasterSearchTerm_PM").prop("checked", searchRequestObject.PM.SearchTerm.IsUserMaster);
                }

                $("#txtIQAgentSetupBLPMIXXml_PM").val(searchRequestObject.PM.BLPMXml);
            }
        }
        if (searchRequestObject.PQSpecified) {
            if (searchRequestObject.PQ != null) {
                if (searchRequestObject.PQ.SearchTerm != null) {
                    $("#txtIQAgentSetupSearchTerm_PQ").val(searchRequestObject.PQ.SearchTerm.SearchTerm);
                    $("#chkIQAgentSetupUserMasterSearchTerm_PQ").prop("checked", searchRequestObject.PQ.SearchTerm.IsUserMaster);
                }

                if (searchRequestObject.PQ.Publications != null) {
                    var pubOutput = $.map(searchRequestObject.PQ.Publications, function (obj, index) { return obj; }).join('; ');
                    $("#txtIQAgentSetupPublication_PQ").val(pubOutput);
                }
                if (searchRequestObject.PQ.Authors != null) {
                    var authorOutput = $.map(searchRequestObject.PQ.Authors, function (obj, index) { return obj; }).join('; ');
                    $("#txtIQAgentSetupAuthor_PQ").val(authorOutput);
                }
                if (searchRequestObject.PQ.Language_Set != null) {
                    if (searchRequestObject.PQ.Language_Set.IsAllowAll == false) {
                        var arr_PQ_Language = [];
                        $.each(searchRequestObject.PQ.Language_Set.Language, function (index, obj) {
                            arr_PQ_Language.push(obj);
                        });
                        $("#ddlIQAgentSetupLanguage_PQ").val(arr_PQ_Language).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupLanguage_PQ").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }
            }
        }

        $("#txtIQAgentSetupBrandId_LR").val("");
        $("#txtIQAgentSetupSearchImageId_LR").val("");
        $("#divLRBrandLogos").html("");
        if (searchRequestObject.LRSpecified) {
            if (searchRequestObject.LR.SearchIDs != null) {
                var lrOutput = $.map(searchRequestObject.LR.SearchIDs, function (obj, index) { return obj; }).join(';');
                $("#txtIQAgentSetupSearchImageId_LR").val(lrOutput);
                UpdateDisplayedLogos();
            }
        }
        
            if (searchRequestObject.TV != null && searchRequestObject.TVSpecified == true) {
                $("#txtIQAgentSetupProgramTitle").val(searchRequestObject.TV.ProgramTitle);
                $("#txtIQAgentSetupAppearing").val(searchRequestObject.TV.Appearing);

                if (searchRequestObject.TV.SearchTerm != null) {
                    $("#txtIQAgentSetupSearchTerm_TV").val(searchRequestObject.TV.SearchTerm.SearchTerm);
                    $("#chkIQAgentSetupUserMasterSearchTerm_TV").prop("checked", searchRequestObject.TV.SearchTerm.IsUserMaster);
                }

                // IQ_Dma_Set
                if (searchRequestObject.TV.IQ_Dma_Set != null) {

                    if (searchRequestObject.TV.IQ_Dma_Set.IsAllowAll == false) {
                        var arr_TV_DMA = [];
                        $.each(searchRequestObject.TV.IQ_Dma_Set.IQ_Dma, function (index, obj) {
                            arr_TV_DMA.push(obj.name);
                        });
                        $("#ddlIQAgentSetupDMA_TV").val(arr_TV_DMA).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupDMA_TV").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }
                SetZipCodeEnabled();    

                // Station_Affiliate_Set
                if (searchRequestObject.TV.Station_Affiliate_Set != null) {

                    if (searchRequestObject.TV.Station_Affiliate_Set.IsAllowAll == false) {
                        var arr_TV_Station_Affil = [];
                        $.each(searchRequestObject.TV.Station_Affiliate_Set.Station_Affil, function (index, obj) {
                            arr_TV_Station_Affil.push(obj.name);
                        });
                        $("#ddlIQAgentSetupAffiliate_TV").val(arr_TV_Station_Affil).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupAffiliate_TV").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // IQ_Station_ID
                if (searchRequestObject.TV.IQ_Station_Set != null) {

                    if (searchRequestObject.TV.IQ_Station_Set.IsAllowAll == false) {
                        var arr_TV_Station_ID = [];
                        $.each(searchRequestObject.TV.IQ_Station_Set.IQ_Station_ID, function (index, obj) {
                            arr_TV_Station_ID.push(obj);
                        });
                        $("#ddlIQAgentSetupStation_TV").val(arr_TV_Station_ID).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupStation_TV").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // IQ_Class_Set
                if (searchRequestObject.TV.IQ_Class_Set != null) {

                    if (searchRequestObject.TV.IQ_Class_Set.IsAllowAll == false) {
                        var arr_TV_IQ_Class = [];
                        $.each(searchRequestObject.TV.IQ_Class_Set.IQ_Class, function (index, obj) {
                            arr_TV_IQ_Class.push(obj.num);
                        });
                        $("#ddlIQAgentSetupCategory_TV").val(arr_TV_IQ_Class).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupCategory_TV").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // IQ_Region_Set
                if (searchRequestObject.TV.IQ_Region_Set != null) {

                    if (searchRequestObject.TV.IQ_Region_Set.IsAllowAll == false) {
                        var arr_TV_IQ_Region = [];
                        $.each(searchRequestObject.TV.IQ_Region_Set.IQ_Region, function (index, obj) {
                            arr_TV_IQ_Region.push(obj.num);
                        });
                        $("#ddlIQAgentSetupRegion_TV").val(arr_TV_IQ_Region).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupRegion_TV").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // IQ_Country_Set
                if (searchRequestObject.TV.IQ_Country_Set != null) {

                    if (searchRequestObject.TV.IQ_Country_Set.IsAllowAll == false) {
                        var arr_TV_IQ_Country = [];
                        $.each(searchRequestObject.TV.IQ_Country_Set.IQ_Country, function (index, obj) {
                            arr_TV_IQ_Country.push(obj.num);
                        });
                        $("#ddlIQAgentSetupCountry_TV").val(arr_TV_IQ_Country).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupCountry_TV").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // Exclude_IQ_Dma_Set
                if (searchRequestObject.TV.Exclude_IQ_Dma_Set != null && searchRequestObject.TV.Exclude_IQ_Dma_Set.Exclude_IQ_Dma != null && searchRequestObject.TV.Exclude_IQ_Dma_Set.Exclude_IQ_Dma.length > 0) {
                    var arr_TV_ExcludeDMA = [];
                    $.each(searchRequestObject.TV.Exclude_IQ_Dma_Set.Exclude_IQ_Dma, function (index, obj) {
                        arr_TV_ExcludeDMA.push(obj.name);
                    });
                    $("#ddlIQAgentSetupExcludeDMA_TV").val(arr_TV_ExcludeDMA).trigger("chosen:updated");
                }
                else {
                    $("#ddlIQAgentSetupExcludeDMA_TV").val(CONST_ZERO).trigger("chosen:updated");
                }

                // Zip Codes
                if (searchRequestObject.TV.ZipCodes != null) {
                    var output = $.map(searchRequestObject.TV.ZipCodes, function (obj, index) { return obj; }).join('; ');
                    $("#txtIQAgentSetupZipCodes").val(output);

                    // Populate _PreviousZipCodes so that removing zip codes will correctly remove the corresponding DMA
                    LookupDMAs(false, false);
                }
                else {
                    _PreviousZipCodes = [];
                }

                // Exclude Zip Codes
                if (searchRequestObject.TV.ExcludeZipCodes != null) {
                    var output = $.map(searchRequestObject.TV.ExcludeZipCodes, function (obj, index) { return obj; }).join('; ');
                    $("#txtIQAgentSetupExcludeZipCodes").val(output);

                    // Populate _PreviousExcludeZipCodes so that removing zip codes will correctly remove the corresponding DMA
                    LookupDMAs(false, true);
                }
                else {
                    _PreviousExcludeZipCodes = [];
                }
            }

            // Online News

            if (searchRequestObject.News != null && searchRequestObject.NewsSpecified == true) {

                if (searchRequestObject.News.Publications != null) {
                    var output = $.map(searchRequestObject.News.Publications, function (obj, index) { return obj; }).join('; ');
                    $("#txtIQAgentSetupPublication_NM").val(output);
                }

                if (searchRequestObject.News.SearchTerm != null) {
                    $("#txtIQAgentSetupSearchTerm_NM").val(searchRequestObject.News.SearchTerm.SearchTerm);
                    $("#chkIQAgentSetupUserMasterSearchTerm_NM").prop("checked", searchRequestObject.News.SearchTerm.IsUserMaster);
                }

                // NewsCategory_Set
                if (searchRequestObject.News.NewsCategory_Set != null) {

                    if (searchRequestObject.News.NewsCategory_Set.IsAllowAll == false) {
                        var arr_NM_Category = [];
                        $.each(searchRequestObject.News.NewsCategory_Set.NewsCategory, function (index, obj) {
                            arr_NM_Category.push(obj);
                        });
                        $("#ddlIQAgentSetupCategory_NM").val(arr_NM_Category).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupCategory_NM").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // PublicationCategory_Set
                if (searchRequestObject.News.PublicationCategory_Set != null) {

                    if (searchRequestObject.News.PublicationCategory_Set.IsAllowAll == false) {
                        var arr_NM_PubCategory = [];
                        $.each(searchRequestObject.News.PublicationCategory_Set.PublicationCategory, function (index, obj) {
                            arr_NM_PubCategory.push(obj);
                        });
                        $("#ddlIQAgentSetupPublicationCategory_NM").val(arr_NM_PubCategory).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupPublicationCategory_NM").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // Genre_Set
                if (searchRequestObject.News.Genre_Set != null) {

                    if (searchRequestObject.News.Genre_Set.IsAllowAll == false) {
                        var arr_NM_Genere = [];
                        $.each(searchRequestObject.News.Genre_Set.Genre, function (index, obj) {
                            arr_NM_Genere.push(obj);
                        });
                        $("#ddlIQAgentSetupGenere_NM").val(arr_NM_Genere).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupGenere_NM").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // Region_Set
                if (searchRequestObject.News.Region_Set != null) {

                    if (searchRequestObject.News.Region_Set.IsAllowAll == false) {
                        var arr_NM_Region = [];
                        $.each(searchRequestObject.News.Region_Set.Region, function (index, obj) {
                            arr_NM_Region.push(obj);
                        });
                        $("#ddlIQAgentSetupRegion_NM").val(arr_NM_Region).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupRegion_NM").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // Language_Set
                if (searchRequestObject.News.Language_Set != null) {

                    if (searchRequestObject.News.Language_Set.IsAllowAll == false) {
                        var arr_NM_Language = [];
                        $.each(searchRequestObject.News.Language_Set.Language, function (index, obj) {
                            arr_NM_Language.push(obj);
                        });
                        $("#ddlIQAgentSetupLanguage_NM").val(arr_NM_Language).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupLanguage_NM").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // Country_Set
                if (searchRequestObject.News.Country_Set != null) {

                    if (searchRequestObject.News.Country_Set.IsAllowAll == false) {
                        var arr_NM_Country = [];
                        $.each(searchRequestObject.News.Country_Set.Country, function (index, obj) {
                            arr_NM_Country.push(obj);
                        });
                        $("#ddlIQAgentSetupCountry_NM").val(arr_NM_Country).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupCountry_NM").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                if (searchRequestObject.News.ExlcudeDomains != null) {
                    var output = $.map(searchRequestObject.News.ExlcudeDomains, function (obj, index) { return obj; }).join('; ');
                    $("#txtIQAgentSetupExcludeDomains_NM").val(output);
                }
            }

            // Social Media

            if (searchRequestObject.SocialMedia != null && searchRequestObject.SocialMediaSpecified == true) {
                $("#txtIQAgentSetupAuthor_SM").val(searchRequestObject.SocialMedia.Author);
                $("#txtIQAgentSetupTitle_SM").val(searchRequestObject.SocialMedia.Title);

                if (searchRequestObject.SocialMedia.Sources != null) {
                    var output = $.map(searchRequestObject.SocialMedia.Sources, function (obj, index) { return obj; }).join('; ');
                    $("#txtIQAgentSetupSource_SM").val(output);
                }

                if (searchRequestObject.SocialMedia.SearchTerm != null) {
                    $("#txtIQAgentSetupSearchTerm_SM").val(searchRequestObject.SocialMedia.SearchTerm.SearchTerm);
                    $("#chkIQAgentSetupUserMasterSearchTerm_SM").prop("checked", searchRequestObject.SocialMedia.SearchTerm.IsUserMaster);
                }

                // SourceType_Set
                if (searchRequestObject.SocialMedia.SourceType_Set != null) {

                    if (searchRequestObject.SocialMedia.SourceType_Set.IsAllowAll == false) {
                        var arr_SM_SourceType = [];
                        $.each(searchRequestObject.SocialMedia.SourceType_Set.SourceType, function (index, obj) {
                            arr_SM_SourceType.push(obj);
                        });
                        $("#ddlIQAgentSetupSourceType_SM").val(arr_SM_SourceType).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupSourceType_SM").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                if (searchRequestObject.SocialMedia.ExlcudeDomains != null) {
                    var output = $.map(searchRequestObject.SocialMedia.ExlcudeDomains, function (obj, index) { return obj; }).join('; ');
                    $("#txtIQAgentSetupExcludeDomains_SM").val(output);
                }
            }

            // Blog

            if (searchRequestObject.Blog != null && searchRequestObject.BlogSpecified == true) {
                $("#txtIQAgentSetupAuthor_BL").val(searchRequestObject.Blog.Author);
                $("#txtIQAgentSetupTitle_BL").val(searchRequestObject.Blog.Title);

                if (searchRequestObject.Blog.Sources != null) {
                    var output = $.map(searchRequestObject.Blog.Sources, function (obj, index) { return obj; }).join('; ');
                    $("#txtIQAgentSetupSource_BL").val(output);
                }

                if (searchRequestObject.Blog.SearchTerm != null) {
                    $("#txtIQAgentSetupSearchTerm_BL").val(searchRequestObject.Blog.SearchTerm.SearchTerm);
                    $("#chkIQAgentSetupUserMasterSearchTerm_BL").prop("checked", searchRequestObject.Blog.SearchTerm.IsUserMaster);
                }

                // SourceType_Set
                if (searchRequestObject.Blog.SourceType_Set != null) {
                    if (searchRequestObject.Blog.SourceType_Set.IsAllowAll == false) {
                        var arr_BL_SourceType = [];
                        $.each(searchRequestObject.Blog.SourceType_Set.SourceType, function (index, obj) {
                            arr_BL_SourceType.push(obj);
                        });
                        $("#ddlIQAgentSetupSourceType_BL").val(arr_BL_SourceType).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupSourceType_BL").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                if (searchRequestObject.Blog.ExlcudeDomains != null) {
                    var output = $.map(searchRequestObject.Blog.ExlcudeDomains, function (obj, index) { return obj; }).join('; ');
                    $("#txtIQAgentSetupExcludeDomains_BL").val(output);
                }
            }

            // Forum

            if (searchRequestObject.Forum != null && searchRequestObject.ForumSpecified == true) {
                $("#txtIQAgentSetupAuthor_FO").val(searchRequestObject.Forum.Author);
                $("#txtIQAgentSetupTitle_FO").val(searchRequestObject.Forum.Title);

                if (searchRequestObject.Forum.Sources != null) {
                    var output = $.map(searchRequestObject.Forum.Sources, function (obj, index) { return obj; }).join('; ');
                    $("#txtIQAgentSetupSource_FO").val(output);
                }

                if (searchRequestObject.Forum.SearchTerm != null) {
                    $("#txtIQAgentSetupSearchTerm_FO").val(searchRequestObject.Forum.SearchTerm.SearchTerm);
                    $("#chkIQAgentSetupUserMasterSearchTerm_FO").prop("checked", searchRequestObject.Forum.SearchTerm.IsUserMaster);
                }

                // SourceType_Set
                if (searchRequestObject.Forum.SourceType_Set != null) {
                    if (searchRequestObject.Forum.SourceType_Set.IsAllowAll == false) {
                        var arr_FO_SourceType = [];
                        $.each(searchRequestObject.Forum.SourceType_Set.SourceType, function (index, obj) {
                            arr_FO_SourceType.push(obj);
                        });
                        $("#ddlIQAgentSetupSourceType_FO").val(arr_FO_SourceType).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupSourceType_FO").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                if (searchRequestObject.Forum.ExlcudeDomains != null) {
                    var output = $.map(searchRequestObject.Forum.ExlcudeDomains, function (obj, index) { return obj; }).join('; ');
                    $("#txtIQAgentSetupExcludeDomains_FO").val(output);
                }
            }

            // Facebook

            if (searchRequestObject.Facebook != null && searchRequestObject.FacebookSpecified == true) {

                if (searchRequestObject.Facebook.SearchTerm != null) {
                    $("#txtIQAgentSetupSearchTerm_FB").val(searchRequestObject.Facebook.SearchTerm.SearchTerm);
                    $("#chkIQAgentSetupUserMasterSearchTerm_FB").prop("checked", searchRequestObject.Facebook.SearchTerm.IsUserMaster);
                }

                $("#chkIQAgentSetupIncludeDefault").prop("checked", searchRequestObject.Facebook.IncludeDefaultPages);

                if (searchRequestObject.Facebook.FBPages != null) {
                    var FBPageIDs = $.map(searchRequestObject.Facebook.FBPages, function (obj, index) { return obj.ID; }).join('; ');
                    var FBPages = $.map(searchRequestObject.Facebook.FBPages, function (obj, index) { return obj.Page; }).join('; ');
                    $("#txtIQAgentSetupFBPageID").val(FBPageIDs);
                    $("#txtIQAgentSetupFBPage").val(FBPages);
                }

                if (searchRequestObject.Facebook.ExcludeFBPages != null) {
                    var ExcludeFBPageIDs = $.map(searchRequestObject.Facebook.ExcludeFBPages, function (obj, index) { return obj.ID; }).join('; ');
                    var ExcludeFBPages = $.map(searchRequestObject.Facebook.ExcludeFBPages, function (obj, index) { return obj.Page; }).join('; ');
                    $("#txtIQAgentSetupExcludeFBPageID").val(ExcludeFBPageIDs);
                    $("#txtIQAgentSetupExcludeFBPage").val(ExcludeFBPages);
                }
            }

            // Instagram

            if (searchRequestObject.Instagram != null && searchRequestObject.InstagramSpecified == true) {
                if (searchRequestObject.Instagram.SearchTerm != null) {
                    $("#txtIQAgentSetupSearchTerm_IG").val(searchRequestObject.Instagram.SearchTerm.SearchTerm);
                    $("#chkIQAgentSetupUserMasterSearchTerm_IG").prop("checked", searchRequestObject.Instagram.SearchTerm.IsUserMaster);
                }

                if (searchRequestObject.Instagram.UserTagString != null) {
                    $("#txtIQAgentSetupIGTag").val(searchRequestObject.Instagram.UserTagString);
                }
            }

            // Twitter

            if ($("#divTWSetup").length) {
                // Unnecessary if the section doesn't exist
                ShowHideEditTwitterRule();
            }
            if (searchRequestObject.Twitter != null && searchRequestObject.TwitterSpecified == true) {
                var output = $.map(searchRequestObject.Twitter.GnipTagList, function (obj, index) { return obj; }).join('; ');
                $("#txtIQAgentSetupGnipTag_TW").val(output);
            }

            // LexisNexis

            if (searchRequestObject.LexisNexis != null && searchRequestObject.LexisNexisSpecified == true) {

                if (searchRequestObject.LexisNexis.Publications != null) {
                    var output = $.map(searchRequestObject.LexisNexis.Publications, function (obj, index) { return obj; }).join('; ');
                    $("#txtIQAgentSetupPublication_LN").val(output);
                }

                if (searchRequestObject.LexisNexis.SearchTerm != null) {
                    $("#txtIQAgentSetupSearchTerm_LN").val(searchRequestObject.LexisNexis.SearchTerm.SearchTerm);
                    $("#chkIQAgentSetupUserMasterSearchTerm_LN").prop("checked", searchRequestObject.LexisNexis.SearchTerm.IsUserMaster);
                }

                // NewsCategory_Set
                if (searchRequestObject.LexisNexis.NewsCategory_Set != null) {

                    if (searchRequestObject.LexisNexis.NewsCategory_Set.IsAllowAll == false) {
                        var arr_LN_Category = [];
                        $.each(searchRequestObject.LexisNexis.NewsCategory_Set.NewsCategory, function (index, obj) {
                            arr_LN_Category.push(obj);
                        });
                        $("#ddlIQAgentSetupCategory_LN").val(arr_LN_Category).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupCategory_LN").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // PublicationCategory_Set
                if (searchRequestObject.LexisNexis.PublicationCategory_Set != null) {

                    if (searchRequestObject.LexisNexis.PublicationCategory_Set.IsAllowAll == false) {
                        var arr_LN_PubCategory = [];
                        $.each(searchRequestObject.LexisNexis.PublicationCategory_Set.PublicationCategory, function (index, obj) {
                            arr_LN_PubCategory.push(obj);
                        });
                        $("#ddlIQAgentSetupPublicationCategory_LN").val(arr_LN_PubCategory).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupPublicationCategory_LN").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // Genre_Set
                if (searchRequestObject.LexisNexis.Genre_Set != null) {

                    if (searchRequestObject.LexisNexis.Genre_Set.IsAllowAll == false) {
                        var arr_LN_Genre = [];
                        $.each(searchRequestObject.LexisNexis.Genre_Set.Genre, function (index, obj) {
                            arr_LN_Genre.push(obj);
                        });
                        $("#ddlIQAgentSetupGenere_LN").val(arr_LN_Genre).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupGenere_LN").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // Region_Set
                if (searchRequestObject.LexisNexis.Region_Set != null) {

                    if (searchRequestObject.LexisNexis.Region_Set.IsAllowAll == false) {
                        var arr_LN_Region = [];
                        $.each(searchRequestObject.LexisNexis.Region_Set.Region, function (index, obj) {
                            arr_LN_Region.push(obj);
                        });
                        $("#ddlIQAgentSetupRegion_LN").val(arr_LN_Region).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupRegion_LN").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // Language_Set
                if (searchRequestObject.LexisNexis.Language_Set != null) {

                    if (searchRequestObject.LexisNexis.Language_Set.IsAllowAll == false) {
                        var arr_LN_Language = [];
                        $.each(searchRequestObject.LexisNexis.Language_Set.Language, function (index, obj) {
                            arr_LN_Language.push(obj);
                        });
                        $("#ddlIQAgentSetupLanguage_LN").val(arr_LN_Language).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupLanguage_LN").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // Country_Set
                if (searchRequestObject.LexisNexis.Country_Set != null) {

                    if (searchRequestObject.LexisNexis.Country_Set.IsAllowAll == false) {
                        var arr_LN_Country = [];
                        $.each(searchRequestObject.LexisNexis.Country_Set.Country, function (index, obj) {
                            arr_LN_Country.push(obj);
                        });
                        $("#ddlIQAgentSetupCountry_LN").val(arr_LN_Country).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupCountry_LN").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                if (searchRequestObject.LexisNexis.ExlcudeDomains != null) {
                    var output = $.map(searchRequestObject.LexisNexis.ExlcudeDomains, function (obj, index) { return obj; }).join('; ');
                    $("#txtIQAgentSetupExcludeDomains_LN").val(output);
                }
            }

            if (searchRequestObject.IQRadio != null && searchRequestObject.IQRadioSpecified == true) {
                if (searchRequestObject.IQRadio.SearchTerm != null) {
                    $("#txtIQAgentSetupSearchTerm_IQRadio").val(searchRequestObject.IQRadio.SearchTerm.SearchTerm);
                    $("#chkIQAgentSetupUserMasterSearchTerm_IQRadio").prop("checked", searchRequestObject.IQRadio.SearchTerm.IsUserMaster);
                }

                // IQ_Dma_Set
                if (searchRequestObject.IQRadio.IQ_Dma_Set != null) {

                    if (searchRequestObject.IQRadio.IQ_Dma_Set.IsAllowAll == false) {
                        var arr_IQRadio_DMA = [];
                        $.each(searchRequestObject.IQRadio.IQ_Dma_Set.IQ_Dma, function (index, obj) {
                            arr_IQRadio_DMA.push(obj.name);
                        });
                        $("#ddlIQAgentSetupDMA_IQRadio").val(arr_IQRadio_DMA).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupDMA_IQRadio").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }
                SetIQRadioZipCodeEnabled();

                // IQ_Station_Set
                if (searchRequestObject.IQRadio.IQ_Station_Set != null) {

                    if (searchRequestObject.IQRadio.IQ_Station_Set.IsAllowAll == false) {
                        var arr_IQRadio_Station_ID = [];
                        $.each(searchRequestObject.IQRadio.IQ_Station_Set.IQ_Station_ID, function (index, obj) {
                            arr_IQRadio_Station_ID.push(obj);
                        });
                        $("#ddlIQAgentSetupStation_IQRadio").val(arr_IQRadio_Station_ID).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupStation_IQRadio").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // IQ_Region_Set
                if (searchRequestObject.IQRadio.IQ_Region_Set != null) {

                    if (searchRequestObject.IQRadio.IQ_Region_Set.IsAllowAll == false) {
                        var arr_IQRadio_IQ_Region = [];
                        $.each(searchRequestObject.IQRadio.IQ_Region_Set.IQ_Region, function (index, obj) {
                            arr_IQRadio_IQ_Region.push(obj.num);
                        });
                        $("#ddlIQAgentSetupRegion_IQRadio").val(arr_IQRadio_IQ_Region).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupRegion_IQRadio").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // IQ_Country_Set
                if (searchRequestObject.IQRadio.IQ_Country_Set != null) {

                    if (searchRequestObject.IQRadio.IQ_Country_Set.IsAllowAll == false) {
                        var arr_IQRadio_IQ_Country = [];
                        $.each(searchRequestObject.IQRadio.IQ_Country_Set.IQ_Country, function (index, obj) {
                            arr_IQRadio_IQ_Country.push(obj.num);
                        });
                        $("#ddlIQAgentSetupCountry_IQRadio").val(arr_IQRadio_IQ_Country).trigger("chosen:updated");
                    }
                    else {
                        $("#ddlIQAgentSetupCountry_IQRadio").val(CONST_ZERO).trigger("chosen:updated");
                    }
                }

                // Exclude_IQ_Dma_Set
                if (searchRequestObject.IQRadio.Exclude_IQ_Dma_Set != null && searchRequestObject.IQRadio.Exclude_IQ_Dma_Set.Exclude_IQ_Dma != null && searchRequestObject.IQRadio.Exclude_IQ_Dma_Set.Exclude_IQ_Dma.length > 0) {
                    var arr_IQRadio_ExcludeDMA = [];
                    $.each(searchRequestObject.IQRadio.Exclude_IQ_Dma_Set.Exclude_IQ_Dma, function (index, obj) {
                        arr_IQRadio_ExcludeDMA.push(obj.name);
                    });
                    $("#ddlIQAgentSetupExcludeDMA_IQRadio").val(arr_IQRadio_ExcludeDMA).trigger("chosen:updated");
                }
                else {
                    $("#ddlIQAgentSetupExcludeDMA_IQRadio").val(CONST_ZERO).trigger("chosen:updated");
                }

                // Zip Codes
                if (searchRequestObject.IQRadio.ZipCodes != null) {
                    var output = $.map(searchRequestObject.IQRadio.ZipCodes, function (obj, index) { return obj; }).join('; ');
                    $("#txtIQAgentSetupZipCodes_IQRadio").val(output);

                    // Populate _PreviousZipCodes_IQRadio so that removing zip codes will correctly remove the corresponding DMA
                    LookupIQRadioDMAs(false, false);
                }
                else {
                    _PreviousZipCodes_IQRadio = [];
                }

                // Exclude Zip Codes
                if (searchRequestObject.IQRadio.ExcludeZipCodes != null) {
                    var output = $.map(searchRequestObject.IQRadio.ExcludeZipCodes, function (obj, index) { return obj; }).join('; ');
                    $("#txtIQAgentSetupExcludeZipCodes_IQRadio").val(output);

                    // Populate _PreviousExcludeZipCodes_IQRadio so that removing zip codes will correctly remove the corresponding DMA
                    LookupIQRadioDMAs(false, true);
                }
                else {
                    _PreviousExcludeZipCodes_IQRadio = [];
                }
            }
    }
}

function SubmitIQAgentSetup() {
    searchTermCapitalized = false;

    //need to know if valid before message gets erased
    var SearchImageErrorMsg = "";
    if ($("#spantxtIQAgentSetupSearchImageId_LR").text().trim() != '') {
        SearchImageErrorMsg = $("#spantxtIQAgentSetupSearchImageId_LR").text();
    }

    $("#frmIQAgentSetupAddEdit span").html("").hide();

    var flag = true;
    var stringToTest;

    if ($.trim($("#txtIQAgentSetupTitle").val()) == "") {
        $("#spanIQAgentSetupTitle").html(_msgIQAgentSetupTitleRequiredField).show();
        flag = false;
    }

    flag = flag && ValidateSearchTerm('', '', 'txtIQAgentSetupSearchTerm', 'spanIQAgentSetupSearchTerm');
    flag = flag && ValidateSearchTerm('chkIQAgentSetup_TV', 'chkIQAgentSetupUserMasterSearchTerm_TV', 'txtIQAgentSetupSearchTerm_TV', 'spantxtIQAgentSetupSearchTerm_TV');
    flag = flag && ValidateSearchTerm('chkIQAgentSetup_NM', 'chkIQAgentSetupUserMasterSearchTerm_NM', 'txtIQAgentSetupSearchTerm_NM', 'spantxtIQAgentSetupSearchTerm_NM');
    flag = flag && ValidateSearchTerm('chkIQAgentSetup_SM', 'chkIQAgentSetupUserMasterSearchTerm_SM', 'txtIQAgentSetupSearchTerm_SM', 'spantxtIQAgentSetupSearchTerm_SM');
    flag = flag && ValidateSearchTerm('chkIQAgentSetup_PQ', 'chkIQAgentSetupUserMasterSearchTerm_PQ', 'txtIQAgentSetupSearchTerm_PQ', 'spantxtIQAgentSetupSearchTerm_PQ');
    flag = flag && ValidateSearchTerm('chkIQAgentSetup_FB', 'chkIQAgentSetupUserMasterSearchTerm_FB', 'txtIQAgentSetupSearchTerm_FB', 'spantxtIQAgentSetupSearchTerm_FB');
    flag = flag && ValidateSearchTerm('chkIQAgentSetup_IG', '', 'txtIQAgentSetupSearchTerm_IG', '');
    flag = flag && ValidateSearchTerm('chkIQAgentSetup_LN', 'chkIQAgentSetupUserMasterSearchTerm_LN', 'txtIQAgentSetupSearchTerm_LN', 'spantxtIQAgentSetupSearchTerm_LN');
    flag = flag && ValidateSearchTerm('chkIQAgentSetup_BL', 'chkIQAgentSetupUserMasterSearchTerm_BL', 'txtIQAgentSetupSearchTerm_BL', 'spantxtIQAgentSetupSearchTerm_BL');
    flag = flag && ValidateSearchTerm('chkIQAgentSetup_FO', 'chkIQAgentSetupUserMasterSearchTerm_FO', 'txtIQAgentSetupSearchTerm_FO', 'spantxtIQAgentSetupSearchTerm_FO');
    flag = flag && ValidateSearchTerm('chkIQAgentSetup_IQRadio', 'chkIQAgentSetupUserMasterSearchTerm_IQRadio', 'txtIQAgentSetupSearchTerm_IQRadio', 'spantxtIQAgentSetupSearchTerm_IQRadio');

    if (SearchImageErrorMsg != null && SearchImageErrorMsg.length > 0) {
        $("#spantxtIQAgentSetupSearchImageId_LR").html(SearchImageErrorMsg).show();
        flag = false;
    }

    if ($('#chkIQAgentSetup_TV').length > 0 && $('#chkIQAgentSetup_TV').is(":checked")) {
        var zipCodes;
        if ($.trim($("#txtIQAgentSetupZipCodes").val()) != "") {
            zipCodes = $.trim($("#txtIQAgentSetupZipCodes").val()).split(';');
            $.each(zipCodes, function (index, obj) {
                if (!ValidateZipCode($.trim(obj))) {
                    $("#spantxtIQAgentSetupZipCodes").html(_msgInvalidZipCode + obj).show();
                    flag = false;
                }
            });
        }

        if ($.trim($("#txtIQAgentSetupExcludeZipCodes").val()) != "") {
            zipCodes = $.trim($("#txtIQAgentSetupExcludeZipCodes").val()).split(';');
            $.each(zipCodes, function (index, obj) {
                if (!ValidateZipCode($.trim(obj))) {
                    $("#spantxtIQAgentSetupExcludeZipCodes").html(_msgInvalidZipCode + obj).show();
                    flag = false;
                }
            });
        }
    }

    if ($('#chkIQAgentSetup_NM').length > 0 && $('#chkIQAgentSetup_NM').is(":checked") && $.trim($("#txtIQAgentSetupExcludeDomains_NM").val()) != "") {
        var domains = $.trim($("#txtIQAgentSetupExcludeDomains_NM").val()).split(';');
        $.each(domains, function (index, obj) {
            stringToTest = $.trim(obj);

            if ((/^"/).test(stringToTest) && (/"$/).test(stringToTest)) {
                stringToTest = stringToTest.substring(1, stringToTest.length - 1);
            }

            if (!TestWildInput(stringToTest)) {
                $("#spanIQAgentSetupExcludeDomains_NM").html(_msgInvalidDomain + obj).show();
                flag = false;
            }
        });        
    }

    if ($('#chkIQAgentSetup_SM').length > 0 && $('#chkIQAgentSetup_SM').is(":checked") && $.trim($("#txtIQAgentSetupExcludeDomains_SM").val()) != "") {
        var domains = $.trim($("#txtIQAgentSetupExcludeDomains_SM").val()).split(';');
        $.each(domains, function (index, obj) {
            stringToTest = $.trim(obj);
            if ((/^"/).test(stringToTest) && (/"$/).test(stringToTest)) {
                stringToTest = stringToTest.substring(1, stringToTest.length - 1);
            }

            if (!TestWildInput(stringToTest)) {
                $("#spanIQAgentSetupExcludeDomains_SM").html(_msgInvalidDomain+ obj).show();
                flag = false;
            }    
        });
    }

    if ($('#chkIQAgentSetup_BL').length > 0 && $('#chkIQAgentSetup_BL').is(":checked") && $.trim($("#txtIQAgentSetupExcludeDomains_BL").val()) != "") {
        var domains = $.trim($("#txtIQAgentSetupExcludeDomains_BL").val()).split(';');
        $.each(domains, function (index, obj) {
            stringToTest = $.trim(obj);
            if ((/^"/).test(stringToTest) && (/"$/).test(stringToTest)) {
                stringToTest = stringToTest.substring(1, stringToTest.length - 1);
            }

            if (!TestWildInput(stringToTest)) {
                $("#spanIQAgentSetupExcludeDomains_BL").html(_msgInvalidDomain + obj).show();
                flag = false;
            }
        });
    }

    if ($('#chkIQAgentSetup_FO').length > 0 && $('#chkIQAgentSetup_FO').is(":checked") && $.trim($("#txtIQAgentSetupExcludeDomains_FO").val()) != "") {
        var domains = $.trim($("#txtIQAgentSetupExcludeDomains_FO").val()).split(';');
        $.each(domains, function (index, obj) {
            stringToTest = $.trim(obj);
            if ((/^"/).test(stringToTest) && (/"$/).test(stringToTest)) {
                stringToTest = stringToTest.substring(1, stringToTest.length - 1);
            }

            if (!TestWildInput(stringToTest)) {
                $("#spanIQAgentSetupExcludeDomains_FO").html(_msgInvalidDomain + obj).show();
                flag = false;
            }
        });
    }

    if ($('#chkIQAgentSetup_LN').length > 0 && $('#chkIQAgentSetup_LN').is(":checked") && $.trim($("#txtIQAgentSetupExcludeDomains_LN").val()) != "") {
        var domains = $.trim($("#txtIQAgentSetupExcludeDomains_LN").val()).split(';');
        $.each(domains, function (index, obj) {
            stringToTest = $.trim(obj);

            if ((/^"/).test(stringToTest) && (/"$/).test(stringToTest)) {
                stringToTest = stringToTest.substring(1, stringToTest.length - 1);
            }

            if (!TestWildInput(stringToTest)) {
                $("#spanIQAgentSetupExcludeDomains_LN").html(_msgInvalidDomain + obj).show();
                flag = false;
            }
        });
    }

    if ($("#chkIQAgentSetup_FB").length > 0 && $("#chkIQAgentSetup_FB").is(":checked")) {
        if ($.trim($("#txtIQAgentSetupFBPageID").val()) == "") {
            // Only allow no pages if the Include Default checkbox is checked
            if (!$("#chkIQAgentSetupIncludeDefault").is(":checked")) {
                $("#spanIQAgentSetupFBPageID").html(_msgNoFBPages).show();
                flag = false;
            }
        }
        else {
            var FBPageIDs = $.trim($("#txtIQAgentSetupFBPageID").val()).split(';');
            $.each(FBPageIDs, function (index, obj) {
                if (obj != parseInt(obj)) {
                    $("#spanIQAgentSetupFBPageID").html(_msgInvalidFBPageID + obj).show();
                    flag = false;
                }
            });
        }

        if ($.trim($("#txtIQAgentSetupExcludeFBPageID").val()) != "") {
            var ExcludeFBPageIDs = $.trim($("#txtIQAgentSetupExcludeFBPageID").val()).split(';');
            $.each(ExcludeFBPageIDs, function (index, obj) {
                if (obj != parseInt(obj)) {
                    $("#spanIQAgentSetupExcludeFBPageID").html(_msgInvalidFBPageID + obj).show();
                    flag = false;
                }
            });
        }
    }

    if ($("#chkIQAgentSetup_TW").length > 0 && $("#chkIQAgentSetup_TW").is(":checked")) {
        if ($.trim($("#txtIQAgentSetupGnipTag_TW").val()) == "") {
            $("#spanIQAgentSetupGnipTag").html("Must create a search rule").show();
            flag = false;
        }
    }

    if ($("#chkIQAgentSetup_TM").length > 0 && $("#chkIQAgentSetup_TM").is(":checked")) {
        if ($.trim($("#txtIQAgentSetupTVEyesSearchGUID_TM").val()) == "") {
            $("#spanIQAgentSetupTVEyesSearchGUID").html("Must create a search rule").show();
            flag = false;
        }
    }

    if ($('#chkIQAgentSetup_IQRadio').length > 0 && $('#chkIQAgentSetup_IQRadio').is(":checked")) {
        var zipCodes;
        if ($.trim($("#txtIQAgentSetupZipCodes_IQRadio").val()) != "") {
            zipCodes = $.trim($("#txtIQAgentSetupZipCodes_IQRadio").val()).split(';');
            $.each(zipCodes, function (index, obj) {
                if (!ValidateZipCode($.trim(obj))) {
                    $("#spantxtIQAgentSetupZipCodes_IQRadio").html(_msgInvalidZipCode + obj).show();
                    flag = false;
                }
            });
        }

        if ($.trim($("#txtIQAgentSetupExcludeZipCodes_IQRadio").val()) != "") {
            zipCodes = $.trim($("#txtIQAgentSetupExcludeZipCodes_IQRadio").val()).split(';');
            $.each(zipCodes, function (index, obj) {
                if (!ValidateZipCode($.trim(obj))) {
                    $("#spantxtIQAgentSetupExcludeZipCodes_IQRadio").html(_msgInvalidZipCode + obj).show();
                    flag = false;
                }
            });
        }
    }

    if (CheckForDuplicateFBPages()) {
        $("#spanIQAgentSetupExcludeFBPageID").html("1 or more pages have been specified in both the include and exclude lists").show();
        flag = false;
    }

    if ($("#chkIQAgentSetup_IG").length > 0 && $("#chkIQAgentSetup_IG").is(":checked") && !ValidateIGTagsUsers()) {
        flag = false;
    }

    $('.chosen-select').trigger("chosen:updated");

    if (flag == true) {
        $("#frmIQAgentSetupAddEdit").ajaxSubmit({
            target: "",
            success: function (res) {
                if (res.isSuccess == true) {
                    if (res.iqAgentKey <= 0) {
                        ShowPopUpNotification(res.msg);
                        $("#btnSubmitIQAgentSetupAddEditForm").removeAttr("disabled");
                    }
                    else {
                        CancelIQAgentPopup('divIQAgentSetupAddEditPopup');
                        GetIQAgentSetupContent(res.iqAgentKey);
                        ShowNotification(res.msg);
                    }
                }
                else {
                    ShowNotification(res.msg);
                    $("#btnSubmitIQAgentSetupAddEditForm").removeAttr("disabled");
                }
            },
            error: function () {
                CancelIQAgentPopup('divIQAgentSetupAddEditPopup');
                ShowNotification(_msgIQAgentSetupSaveError);
            }
        });
    }

    if (searchTermCapitalized) {
        alert("In order to ensure a valid Search Term, all occurrences of lowercase AND, OR, and NOT have been changed to uppercase.");
    }
    
}

function ShowHideTabdiv(elementIndex,isClearOther) {

    var EleID = '';
    var HeaderID = '';
    switch (elementIndex) {
        case 0:
            EleID = 'divTVTabContent'
            HeaderID = 'divTVSetup';
            break;
        case 1:
            EleID = 'divOnlineNewsTabContent';
            HeaderID = 'divNMSetup';
            break;
        case 2:
            EleID = 'divSocialMediaTabContent';
            HeaderID = 'divSMSetup';
            break;
        case 3:
            EleID = 'divTwitterTabContent';
            HeaderID = 'divTWSetup';
            break;
        case 4:
            EleID = 'divTMTabContent';
            HeaderID = 'divTMSetup';
            break;
        case 5:
            EleID = 'divPMTabContent';
            HeaderID = 'divPMSetup';
            break;
        case 6:
            EleID = 'divPQTabContent';
            HeaderID = 'divPQSetup';
            break;
        case 7:
            EleID = 'divFBTabContent';
            HeaderID = 'divFBSetup';
            break;
        case 8:
            EleID = 'divIGTabContent';
            HeaderID = 'divIGSetup';
            break;
        case 9:
            EleID = 'divLRTabContent';
            HeaderID = 'divLRSetup';
            break;
        case 10:
            EleID = 'divLNTabContent';
            HeaderID = 'divLNSetup';
            break;
        case 11:
            EleID = 'divBLTabContent';
            HeaderID = 'divBLSetup';
            break;
        case 12:
            EleID = 'divFOTabContent';
            HeaderID = 'divFOSetup';
            break;
        case 13:
            EleID = 'divIQRadioTabContent';
            HeaderID = 'divIQRadioSetup';
            break;
    }

    if ($("#" + EleID).is(':visible')) {
        $("#" + EleID).hide('slow');
        $("#" + HeaderID).find('img').attr('src', '../images/show.png')
    }
    else {
        $("#" + EleID).show('slow');
        $("#" + HeaderID).find('img').attr('src', '../images/hiden.png')
    }

    if (isClearOther == true) {
        for (idx = 0; idx <= 100; idx++) {
            if (idx != elementIndex) {
                $("#divIQAgentSetupTabs").children().eq(idx * 2 + 1).hide();
                $("#divIQAgentSetupTabs").children().eq(idx * 2).find('img').attr('src', '../images/show.png')
            }
        }
    }
}


function GetIQAgentSetupContent(iqagentkey) {

    $("#divSetupContent").html("");

    $.ajax({
        url: "/Setup/DisplayIQAgentSetupContent/",
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: {},
        success: function (result) {
            if (result != null && result.isSuccess) {
                $("#divSetupContent").html(result.HTML);

                // Display some animation for record that is added/updated
                if (iqagentkey > 0) {
                    //alert(iqagentkey);
                    setTimeout(function () { $("#divSetupIQAgentSearchRequestList_ScrollContent").mCustomScrollbar("scrollTo", "#divIQAgentSearchRequest_" + iqagentkey); }, 500);

                    $("#divIQAgentSearchRequest_" + iqagentkey).animate({ backgroundColor: "#EDB5CC" }, 1000, function () {
                        $("#divIQAgentSearchRequest_" + iqagentkey).animate
                                        ({
                                            backgroundColor: '#fff'
                                        }, 1500);

                    });
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

function DisplayIQAGentSearchXML(itemid) {
    var jsonPostData = { p_ID: itemid }
    $("#textareaIQAgentSetupDisplayXML").val("");
    $.ajax({
        url: _urlSetupIQAgentSearchDisplayXML,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {

            if (result != null && result.isSuccess) {

                $("#textareaIQAgentSetupDisplayXML").val(result.HTML);

                $("#divIQAgentSetupdisplayXMLPopup").modal({
                    backdrop: "static",
                    keyboard: true,
                    dynamic: true
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


function DeleteIQAgent(itemid, queryname) {
    var jsonPostData = { p_ID: itemid };

    getConfirm("Delete IQAgent - " + queryname + "", CONST_DeleteNoteIQAgent, "Confirm Delete", "Cancel", function (res) {
        if (res) {
            $.ajax({
                url: _urlSetupIQAgentDeleteIQAgent,
                contentType: "application/json; charset=utf-8",
                type: "post",
                dataType: "json",
                data: JSON.stringify(jsonPostData),
                success: function (result) {

                    if (result != null && result.isSuccess) {
                        ShowNotification(result.msg);
                        GetIQAgentSetupContent(0);
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
    });
}

var SuspendAgent = function (itemid, queryname, obj) {
    var jsonPostData = { p_ID: itemid };

    getConfirm("Suspend IQAgent - " + queryname + "", CONST_SuspendNoteIQAgent, "Confirm Suspend", "Cancel", function (res) {
        if (res) {
            $.ajax({
                url: _urlSetupAgentSuspendAgent,
                contentType: "application/json; charset=utf-8",
                type: "post",
                dataType: "json",
                data: JSON.stringify(jsonPostData),
                success: function (result) {

                    if (result != null && result.isSuccess) {
                        ShowNotification(result.msg);

                        $("#divName_" + itemid).toggleClass("suspended");
                        $("#divInfo_" + itemid).toggleClass("suspended");
                        $("#aNotif_" + itemid).toggleClass("displayNone");
                        $("#aEdit_" + itemid).toggleClass("displayNone");

                        var parent = $(obj).parent();
                        $(obj).remove();
                        $(parent).append('<a href="#" onclick="ResumeSuspendedAgent(' + itemid + ',\'' + queryname.replace(/'/g, '\\\'') + '\',this)" title="Resume">'
                                    + '<img src="/Images/unsuspend.png" /></a>');
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
    });
}

var ResumeSuspendedAgent = function (itemid, queryname, obj) {
    var jsonPostData = { p_ID: itemid };

    getConfirm("Suspend IQAgent - " + queryname + "", "Are you sure to resume the agent?", "Confirm", "Cancel", function (res) {
        if (res) {
            $.ajax({
                url: _urlSetupAgentResumeSuspendedAgent,
                contentType: "application/json; charset=utf-8",
                type: "post",
                dataType: "json",
                data: JSON.stringify(jsonPostData),
                success: function (result) {

                    if (result != null && result.isSuccess) {
                        ShowNotification("Agent resumed successfully.");

                        $("#divName_" + itemid).toggleClass("suspended");
                        $("#divInfo_" + itemid).toggleClass("suspended");
                        $("#aNotif_" + itemid).toggleClass("displayNone");
                        $("#aEdit_" + itemid).toggleClass("displayNone");

                        var parent = $(obj).parent();
                        $(obj).remove();
                        $(parent).append('<a href="#" onclick="SuspendAgent(' + itemid + ',\'' + queryname.replace(/'/g, '\\\'') + '\',this)" title="Suspend">'
                                    + '<img src="/Images/suspend.png" /></a>');
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
    });
}

function DisplayIQNotificationSettings(searchrequestid) {

    $("#hdnIQAgentSearchRequestID").val("");
    $("#divIQNotificationMessage").html("")

    var jsonPostData = { p_SearchRequestID: searchrequestid }

    $.ajax({
        url: _urlSetupIQAgentNoficationSettingsDisplay,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {

            if (result != null && result.isSuccess == true) {

                $("#divIQAgentSetupIQNotificationHTML").html(result.HTML);
                $("#hdnIQAgentSearchRequestID").val(searchrequestid);

                $("#divIQAgentSetupIQNotificationPopup").modal({
                    backdrop: "static",
                    keyboard: true,
                    dynamic: true
                });


                $("#divIQAgentSetupIQNotificationHTML .chosen-select").chosen({
                    display_disabled_options: true,
                    default_item: CONST_ZERO,
                    width: null,
                    inherit_select_classes : true
                });

                //$("#divIQAgentSetupIQNotificationHTML .chosen-container .search-field input").css("width", "100%")

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

function SetZipCodeEnabled() {
    var selectedDMAs = $("#ddlIQAgentSetupDMA_TV").chosen().val();
    if (selectedDMAs != null && selectedDMAs.length == 1 && selectedDMAs[0] == CONST_ZERO) {
        $("#txtIQAgentSetupZipCodes").attr("disabled", "disabled");
    }
    else {
        $("#txtIQAgentSetupZipCodes").removeAttr("disabled");
    }
}

function LookupDMAs(setSelectedDMAs, isExclude) {
    var spanHelp;
    var txtZipCodes;
    var ddlDMAs;
    var prevZipCodes;

    if (isExclude) {
        spanHelp = $("#spantxtIQAgentSetupExcludeZipCodes");
        txtZipCodes = $("#txtIQAgentSetupExcludeZipCodes");
        ddlDMAs = $("#ddlIQAgentSetupExcludeDMA_TV");
        prevZipCodes = _PreviousExcludeZipCodes;
    }
    else {
        spanHelp = $("#spantxtIQAgentSetupZipCodes");
        txtZipCodes = $("#txtIQAgentSetupZipCodes");
        ddlDMAs = $("#ddlIQAgentSetupDMA_TV");
        prevZipCodes = _PreviousZipCodes;
    }

    spanHelp.hide();
    var currentZipCodes = [];
    var dmaPair;
    var selectedDMAs = ddlDMAs.chosen().val();
    if (selectedDMAs == null || (selectedDMAs.length == 1 && selectedDMAs[0] == "0")) {
        selectedDMAs = [];
    }
    
    if ($.trim(txtZipCodes.val()) != "") {
        var flag = true;
        var zipCodes = $.trim(txtZipCodes.val()).split(';');
        $.each(zipCodes, function (index, obj) {
            var zipCode = $.trim(obj);
            if (!ValidateZipCode(zipCode)) {
                spanHelp.html(_msgInvalidZipCode + zipCode).show();
                flag = false;
            }
            else if ($.inArray(zipCode, currentZipCodes) == -1) {
                currentZipCodes.push(zipCode);
            }
        });

        if (flag == true) {
            var jsonPostData = { zipCodes: currentZipCodes };

            $.ajax({
                url: _urlCommonGetDMAsByZipCode,
                contentType: "application/json; charset=utf-8",
                type: "post",
                dataType: "json",
                data: JSON.stringify(jsonPostData),
                success: function (result) {

                    if (result != null && result.isSuccess == true) {
                        currentZipCodes = result.dmas;

                        if (setSelectedDMAs) {
                            // For any zip codes that were deleted, unselect the corresponding DMAs
                            $.each(prevZipCodes, function (index, obj) {
                                if ($.inArray(obj, currentZipCodes) == -1) {
                                    dmaPair = obj.split(':');
                                    selectedDMAs.splice($.inArray(dmaPair[1], selectedDMAs), 1);
                                }
                            });

                            // For any zip codes that were added, select the corresponding DMAs
                            if (currentZipCodes.length > 0) {

                                $.each(result.dmas, function (index, obj) {
                                    dmaPair = obj.split(':'); // ZipCode:DMA
                                    if ($.inArray(dmaPair[1], selectedDMAs) == -1) {
                                        selectedDMAs.push(dmaPair[1]);
                                    }
                                });

                                ddlDMAs.val(selectedDMAs).trigger("chosen:updated");
                            }

                            if (result.invalidZipCodeMsg != "") {
                                spanHelp.html(_msgInvalidZipCode + result.invalidZipCodeMsg).show();
                            }
                        }

                        if (isExclude) {
                            _PreviousExcludeZipCodes = currentZipCodes;
                        }
                        else {
                            _PreviousZipCodes = currentZipCodes;
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
    }
    else if (prevZipCodes.length > 0) {
        $.each(prevZipCodes, function (index, obj) {
            if ($.inArray(obj, currentZipCodes) == -1) {
                dmaPair = obj.split(':');
                selectedDMAs.splice($.inArray(dmaPair[1], selectedDMAs), 1);
            }
        });

        ddlDMAs.val(selectedDMAs).trigger("chosen:updated");
        if (isExclude) {
            _PreviousExcludeZipCodes = [];
        }
        else {
            _PreviousZipCodes = [];
        }
    }
}

function SetIQRadioZipCodeEnabled() {
    var selectedDMAs = $("#ddlIQAgentSetupDMA_IQRadio").chosen().val();
    if (selectedDMAs != null && selectedDMAs.length == 1 && selectedDMAs[0] == CONST_ZERO) {
        $("#txtIQAgentSetupZipCodes_IQRadio").attr("disabled", "disabled");
    }
    else {
        $("#txtIQAgentSetupZipCodes_IQRadio").removeAttr("disabled");
    }
}

function LookupIQRadioDMAs(setSelectedDMAs, isExclude) {
    var spanHelp;
    var txtZipCodes;
    var ddlDMAs;
    var prevZipCodes;

    if (isExclude) {
        spanHelp = $("#spantxtIQAgentSetupExcludeZipCodes_IQRadio");
        txtZipCodes = $("#txtIQAgentSetupExcludeZipCodes_IQRadio");
        ddlDMAs = $("#ddlIQAgentSetupExcludeDMA_IQRadio");
        prevZipCodes = _PreviousExcludeZipCodes_IQRadio;
    }
    else {
        spanHelp = $("#spantxtIQAgentSetupZipCodes_IQRadio");
        txtZipCodes = $("#txtIQAgentSetupZipCodes_IQRadio");
        ddlDMAs = $("#ddlIQAgentSetupDMA_IQRadio");
        prevZipCodes = _PreviousZipCodes_IQRadio;
    }

    spanHelp.hide();
    var currentZipCodes = [];
    var dmaPair;
    var selectedDMAs = ddlDMAs.chosen().val();
    if (selectedDMAs == null || (selectedDMAs.length == 1 && selectedDMAs[0] == "0")) {
        selectedDMAs = [];
    }

    if ($.trim(txtZipCodes.val()) != "") {
        var flag = true;
        var zipCodes = $.trim(txtZipCodes.val()).split(';');
        $.each(zipCodes, function (index, obj) {
            var zipCode = $.trim(obj);
            if (!ValidateZipCode(zipCode)) {
                spanHelp.html(_msgInvalidZipCode + zipCode).show();
                flag = false;
            }
            else if ($.inArray(zipCode, currentZipCodes) == -1) {
                currentZipCodes.push(zipCode);
            }
        });

        if (flag == true) {
            var jsonPostData = { zipCodes: currentZipCodes };

            $.ajax({
                url: _urlCommonGetDMAsByZipCode,
                contentType: "application/json; charset=utf-8",
                type: "post",
                dataType: "json",
                data: JSON.stringify(jsonPostData),
                success: function (result) {

                    if (result != null && result.isSuccess == true) {
                        currentZipCodes = result.dmas;

                        if (setSelectedDMAs) {
                            // For any zip codes that were deleted, unselect the corresponding DMAs
                            $.each(prevZipCodes, function (index, obj) {
                                if ($.inArray(obj, currentZipCodes) == -1) {
                                    dmaPair = obj.split(':');
                                    selectedDMAs.splice($.inArray(dmaPair[1], selectedDMAs), 1);
                                }
                            });

                            // For any zip codes that were added, select the corresponding DMAs
                            if (currentZipCodes.length > 0) {

                                $.each(result.dmas, function (index, obj) {
                                    dmaPair = obj.split(':'); // ZipCode:DMA
                                    if ($.inArray(dmaPair[1], selectedDMAs) == -1) {
                                        selectedDMAs.push(dmaPair[1]);
                                    }
                                });

                                ddlDMAs.val(selectedDMAs).trigger("chosen:updated");
                            }

                            if (result.invalidZipCodeMsg != "") {
                                spanHelp.html(_msgInvalidZipCode + result.invalidZipCodeMsg).show();
                            }
                        }

                        if (isExclude) {
                            _PreviousExcludeZipCodes_IQRadio = currentZipCodes;
                        }
                        else {
                            _PreviousZipCodes_IQRadio = currentZipCodes;
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
    }
    else if (prevZipCodes.length > 0) {
        $.each(prevZipCodes, function (index, obj) {
            if ($.inArray(obj, currentZipCodes) == -1) {
                dmaPair = obj.split(':');
                selectedDMAs.splice($.inArray(dmaPair[1], selectedDMAs), 1);
            }
        });

        ddlDMAs.val(selectedDMAs).trigger("chosen:updated");
        if (isExclude) {
            _PreviousExcludeZipCodes_IQRadio = [];
        }
        else {
            _PreviousZipCodes_IQRadio = [];
        }
    }
}

function LookupFBPageUrls(isExclude) {
    var txtFBPageIDs;
    var txtFBPages;
    var spanFBPageIDs;
    var spanFBPages;

    if (isExclude) {
        txtFBPageIDs = $("#txtIQAgentSetupExcludeFBPageID");
        txtFBPages = $("#txtIQAgentSetupExcludeFBPage");
        spanFBPageIDs = $("#spanIQAgentSetupExcludeFBPageID");
        spanFBPages = $("#spanIQAgentSetupExcludeFBPage");
    }
    else {
        txtFBPageIDs = $("#txtIQAgentSetupFBPageID");
        txtFBPages = $("#txtIQAgentSetupFBPage");
        spanFBPageIDs = $("#spanIQAgentSetupFBPageID");
        spanFBPages = $("#spanIQAgentSetupFBPage");
    }

    spanFBPageIDs.hide();
    spanFBPages.hide();

    if ($.trim(txtFBPageIDs.val()) != "") {
        var isValid = true;
        var FBPageIDs = $.map($.trim(txtFBPageIDs.val()).split(';'), function (obj, index) {
            return $.trim(obj);
        });
        $.each(FBPageIDs, function (index, obj) {
            if (obj != parseInt(obj)) {
                spanFBPageIDs.html("Invalid Facebook Page ID " + obj).show();
                isValid = false;
            }
        });

        if (isValid) {
            $("#btnSubmitIQAgentSetupAddEditForm").attr("disabled", "disabled");

            var jsonPostData = { FBPageIDs: FBPageIDs };

            $.ajax({
                url: _urlSetupGetFBPageUrlsByID,
                contentType: "application/json; charset=utf-8",
                type: "post",
                dataType: "json",
                data: JSON.stringify(jsonPostData),
                success: function (result) {
                    if (result != null && result.isSuccess) {
                        txtFBPages.val(result.FBPages);

                        if (result.invalidIDs != "") {
                            spanFBPageIDs.html(_msgInvalidFBPageID + result.invalidIDs).show();
                        }
                    }
                    else {
                        ShowNotification(_msgErrorOccured);
                    }
                    $("#btnSubmitIQAgentSetupAddEditForm").removeAttr("disabled");
                },
                error: function (a, b, c) {
                    ShowNotification(_msgErrorOccured);
                    $("#btnSubmitIQAgentSetupAddEditForm").removeAttr("disabled");
                }
            });     
        }
    }
    else {
        txtFBPages.val("");
    }
}

function LookupFBPageIDs(isExclude) {
    var txtFBPageIDs;
    var txtFBPages;
    var spanFBPageIDs;
    var spanFBPages;

    if (isExclude) {
        txtFBPageIDs = $("#txtIQAgentSetupExcludeFBPageID");
        txtFBPages = $("#txtIQAgentSetupExcludeFBPage");
        spanFBPageIDs = $("#spanIQAgentSetupExcludeFBPageID");
        spanFBPages = $("#spanIQAgentSetupExcludeFBPage");
    }
    else {
        txtFBPageIDs = $("#txtIQAgentSetupFBPageID");
        txtFBPages = $("#txtIQAgentSetupFBPage");
        spanFBPageIDs = $("#spanIQAgentSetupFBPageID");
        spanFBPages = $("#spanIQAgentSetupFBPage");
    }

    spanFBPageIDs.hide();
    spanFBPages.hide();

    if ($.trim(txtFBPages.val()) != "") {
        $("#btnSubmitIQAgentSetupAddEditForm").attr("disabled", "disabled");

        var FBPages = $.map($.trim(txtFBPages.val()).split(';'), function (obj, index) {
            return $.trim(obj);
        });
        var jsonPostData = { FBPages: FBPages };

        $.ajax({
            url: _urlSetupGetFBPageIDsByUrl,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                if (result != null && result.isSuccess) {
                    txtFBPageIDs.val(result.FBPageIDs);

                    if (result.invalidPages != "") {
                        spanFBPages.html(_msgInvalidFBPage + result.invalidPages).show();
                    }
                }
                else {
                    ShowNotification(_msgErrorOccured);
                }
                $("#btnSubmitIQAgentSetupAddEditForm").removeAttr("disabled");
            },
            error: function (a, b, c) {
                ShowNotification(_msgErrorOccured);
                $("#btnSubmitIQAgentSetupAddEditForm").removeAttr("disabled");
            }
        });
    }
    else {
        txtFBPageIDs.val("");
    }
}

// Don't allow a page to be both included and excluded
function CheckForDuplicateFBPages() {
    var txtFBPageIDs = $("#txtIQAgentSetupFBPageID");
    var txtExcludeFBPageIDs = $("#txtIQAgentSetupExcludeFBPageID");
    var isDuplicate = false;

    if ($.trim(txtFBPageIDs.val()) != "" && $.trim(txtExcludeFBPageIDs.val()) != "") {
        var FBPageIDs = $.map($.trim(txtFBPageIDs.val()).split(';'), function (obj, index) {
            return $.trim(obj);
        });
        var excludeFBPageIDs = $.map($.trim(txtExcludeFBPageIDs.val()).split(';'), function (obj, index) {
            return $.trim(obj);
        });

        $.each(excludeFBPageIDs, function (index, obj) {
            if ($.inArray(obj, FBPageIDs) > -1) {
                isDuplicate = true;
            }
        });
    }

    return isDuplicate;
}

function ValidateIGTagsUsers() {
    var text = $("#txtIQAgentSetupIGTag").val();

    // textbox cannot be empty and must contain a user or a tag
    if (text.length == 0 || (text.indexOf("@") < 0 && text.indexOf("#") < 0)) {
        $("#spanIQAgentSetupIGTag").html("Must have a valid user or tag").show();
        return false;
    }

    //user cannot be @ or @) and tag cannot be # or #)
    var indicesUser = [];
    var indicesTag = [];
    var valid = true;
    for(var i=0; i<text.length;i++) {
        if (text[i] === "@") indicesUser.push(i);
        if (text[i] === "#") indicesTag.push(i);
    }
    $.each(indicesUser, function (index, value) {
        if ((text.length - 1) == value || (text[value + 1] == " ") || (text[value + 1] == ")")) {
            $("#spanIQAgentSetupIGTag").html("Invalid user").show();
            valid = false;
            return false;
        }
    });
    $.each(indicesTag, function (index, value) {
        if ((text.length - 1) == value || (text[value + 1] == " ") || (text[value + 1] == ")")) {
            $("#spanIQAgentSetupIGTag").html("Invalid tag").show();
            valid = false;
            return false;
        }
    });
    if (valid == false) {
        return false;
    }

    // Validate matching parentheses
    var openParens = 0;

    for (var i = 0; i < text.length; i++) {
        if (text[i] == "(") {
            openParens++;
        }
        else if (text[i] == ")") {
            openParens--;
        }
    }

    if (openParens != 0) {
        $("#spanIQAgentSetupIGTag").html("Unequal number of parentheses.").show();
        return false;
    }

    // Perform regex validation
    var nestingResult = /[@#]\S*[@#]/.exec(text); // Any instance of @@, @#, #@, or ##, with 0 or more non-whitespace characters between them
    if (nestingResult != null) {
        $("#spanIQAgentSetupIGTag").html("Invalid value: " + nestingResult).show();
        return false;
    }

    var invalidUser = /@\.\w+/.exec(text); // Users starting with .
    if (invalidUser != null) {
        $("#spanIQAgentSetupIGTag").html("Invalid user: " + invalidUser).show();
        return false;
    }

    invalidUser = /@\w+\.(?:\s|$|\))/.exec(text);  // Users ending with .
    if (invalidUser != null) {
        $("#spanIQAgentSetupIGTag").html("Invalid user: " + invalidUser).show();
        return false;
    }

    var invalidCharacters = /[^\w.#@\s\(\)]+/.exec(text);  // Any character that is not alphanumeric, not whitespace, and not in [()._]
    if (invalidCharacters != null) {
        $("#spanIQAgentSetupIGTag").html("Invalid value: " + invalidCharacters).show();
        return false;
    }

    var missingQualifier = /(?:^|\s|\()(?!OR\s|NOT\s|AND\s)[\w._]+/.exec(text); // Text that is not preceded by @ or #, and is not a solr keyword (OR, AND, NOT)
    if (missingQualifier != null) {
        $("#spanIQAgentSetupIGTag").html("Must specify either tag (#) or user (@): " + missingQualifier).show();
        return false;
    }

    return true;
}

function ValidateSearchTerm(chkMediumID, chkSearchTermID, txtSearchTermID, spanErrorMsgID) {
    if ((chkMediumID == '' && chkSearchTermID == '') || ($('#' + chkMediumID).is(":checked") && (chkSearchTermID == '' || !$('#' + chkSearchTermID).is(":checked")))) {
        //update the smart quotes to regular quotes
        var searchTerm = $("#" + txtSearchTermID).val().trim().replace(/[\u201C\u201D]/g, '"')/*.replace(/[\u2018\u2019]/g, "'")*/;
        $("#" + txtSearchTermID).val(searchTerm);

        if (chkSearchTermID != '' && $.trim($("#" + txtSearchTermID).val()) == "") {
            $("#" + spanErrorMsgID).html(_msgIQAgentSetupSearchTermRequiredField).show();
            return false;
        }
        else {
            // Basic verification for matching numbers of " and (), to help prevent errors in solr
            var openParens = 0;
            var doubleQuotes = 0;

            for (var i = 0; i < searchTerm.length; i++) {
                if (searchTerm[i] == "(") {
                    openParens++;
                }
                else if (searchTerm[i] == ")") {
                    openParens--;
                }
                else if (searchTerm[i] == "\"") {
                    doubleQuotes++;
                }
            }

            if (doubleQuotes % 2 != 0) {
                $("#" + spanErrorMsgID).html("Unequal number of double quotes.").show();
                return false;
            }
            if (openParens != 0) {
                $("#" + spanErrorMsgID).html("Unequal number of parentheses.").show();
                return false;
            }
        }

        var iAnd = searchTerm.match(/(?:^|\b)(and)(?=\b|$)/gi);
        var iOr = searchTerm.match(/(?:^|\b)(or)(?=\b|$)/gi);
        var iNot = searchTerm.match(/(?:^|\b)(not)(?=\b|$)/gi);
        var And = searchTerm.match(/(?:^|\b)(AND)(?=\b|$)/g);
        var Or = searchTerm.match(/(?:^|\b)(OR)(?=\b|$)/g);
        var Not = searchTerm.match(/(?:^|\b)(NOT)(?=\b|$)/g);

        if ((iAnd != null && iAnd.length > 0 && (And == null || And.length < iAnd.length))
        || (iOr != null && iOr.length > 0 && (Or == null || Or.length < iOr.length))
        || (iNot != null && iNot.length > 0 && (Not == null || Not.length < iNot.length))) {
            searchTermCapitalized = true;

            $("#" + txtSearchTermID).val(searchTerm.replace(/(?:^|\b)(and)(?=\b|$)/gi, 'AND'));
            searchTerm = $("#" + txtSearchTermID).val().trim().replace(/[\u201C\u201D]/g, '"');

            $("#" + txtSearchTermID).val(searchTerm.replace(/(?:^|\b)(or)(?=\b|$)/gi, 'OR'));
            searchTerm = $("#" + txtSearchTermID).val().trim().replace(/[\u201C\u201D]/g, '"');

            $("#" + txtSearchTermID).val(searchTerm.replace(/(?:^|\b)(not)(?=\b|$)/gi, 'NOT'));
            searchTerm = $("#" + txtSearchTermID).val().trim().replace(/[\u201C\u201D]/g, '"');
        }
    }

    return true;
}

///////// Twitter Rule Edit /////////

function ShowHideEditTwitterRule() {
    if ($("#chkIQAgentSetup_TW").is(":checked")) {
        $("#imgEditTwitterRule").removeClass("displayNone");
    }
    else {
        if ($("#txtIQAgentSetupGnipTag_TW").val() != "") {
            getConfirm("Confirm Disable", "Disabling this section will delete the search rule. If the section is reenabled in the future, it will have to be recreated. Do you want to continue?", "Continue", "Cancel", function (res) {
                if (res == true) {
                    $("#imgEditTwitterRule").addClass("displayNone");
                }
                else {
                    $("#chkIQAgentSetup_TW").prop("checked", true);
                }
            });
        }
        else {
            $("#imgEditTwitterRule").addClass("displayNone");
        }
    }
}

function ShowEditTwitterRulePopup(isEditable) {
    var ruleTag = $("#txtIQAgentSetupGnipTag_TW").val();
    var container = $("#divTwitterRules");
    var textAreaHTML = '<textarea rows="2" cols="40" class="twitterRuleTxt" ' + (isEditable ? '' : 'readonly="readonly"') + '/>';

    container.empty();
    $("#divTwitterRuleMsg").html('');
    $('input[name="chkTwitterRuleOption"]').prop("checked", false);

    if (ruleTag != "") {
        $("#txtTwitterRuleTag").val(ruleTag);

        var jsonPostData = {
            trackGuid: ruleTag
        };

        $.ajax({
            url: _urlSetupAgentGetTwitterRule,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                if (result.isSuccess) {
                    if (result.rules.length > 0) {
                        $.each(result.rules, function (index, rule) {
                            var textArea = $(textAreaHTML);

                            // Don't include the options in the user-editable text
                            var ruleText = rule.replace(_twitterEnglishRule + " ", "");
                            ruleText = ruleText.replace(_twitterFollowingRule + " ", "");
                            ruleText = ruleText.replace(_twitterFollowersRule + " ", "");
                            ruleText = ruleText.replace(_twitterTimeZonesRule + " ", "");

                            // Remove the outer parentheses
                            ruleText = ruleText.substring(1, ruleText.length - 1);

                            textArea.text(ruleText);
                            container.append(textArea);
                        });

                        if (result.rules[0].indexOf(_twitterEnglishRule) != -1) {
                            $("#chkEnglish").prop("checked", true);
                        }
                        if (result.rules[0].indexOf(_twitterFollowingRule) != -1) {
                            $("#chkFollowing").prop("checked", true);
                        }
                        if (result.rules[0].indexOf(_twitterFollowersRule) != -1) {
                            $("#chkFollowers").prop("checked", true);
                        }
                        if (result.rules[0].indexOf(_twitterTimeZonesRule) != -1) {
                            $("#chkTimeZones").prop("checked", true);
                        }
                    }
                    else {
                        var textArea = $(textAreaHTML);
                        container.append(textArea);
                    }

                    $("#divEditTwitterRulePopup").modal({
                        backdrop: "static",
                        keyboard: true,
                        dynamic: true
                    });
                }
                else {
                    ShowNotification(_msgErrorOccured);
                }
            },
            error: function (a, b, c) {
                console.error("GetTwitterRule error - " + c);
                ShowNotification(_msgErrorOccured);
            }
        });
    }
    else {
        $("#txtTwitterRuleTag").val("N/A");

        var textArea = $(textAreaHTML);
        container.append(textArea);

        $("#divEditTwitterRulePopup").modal({
            backdrop: "static",
            keyboard: true,
            dynamic: true
        });
    }
}

function SaveTwitterRules() {
    var rules = [];
    var isTooLong = false;
    var optionText = "";
    var hasValue = false;
    var hasAsterisk = false;

    if ($("#chkEnglish").is(":checked")) {
        optionText += _twitterEnglishRule + " ";
    }
    if ($("#chkFollowers").is(":checked")) {
        optionText += _twitterFollowersRule + " ";
    }
    if ($("#chkFollowing").is(":checked")) {
        optionText += _twitterFollowingRule + " ";
    }
    if ($("#chkTimeZones").is(":checked")) {
        optionText += _twitterTimeZonesRule + " ";
    }

    $.each($("#divTwitterRules textArea"), function (index, obj) {
        var text = $(obj).val();
        if (text != "") {
            hasValue = true;

            if (text.indexOf("*") != -1) {
                hasAsterisk = true;
                $(obj).addClass("twitterRuleError");
            }
            else if (text.length > 700) {
                isTooLong = true;
                $(obj).addClass("twitterRuleError");
            }
            else {
                $(obj).removeClass("twitterRuleError");
                rules.push(optionText + "(" + text + ")");
            }
        }
    });

    if (!hasValue) {
        $("#divTwitterRuleMsg").html(_msgIQAgentSetupTwitterRuleRequiredField);
    }
    else if (hasAsterisk) {
        $("#divTwitterRuleMsg").html(_msgIQAgentSetupTwitterRuleNoAsterisks);
    }
    else if (isTooLong) {
        $("#divTwitterRuleMsg").html(_msgIQAgentSetupTwitterRuleTooLong);
    }
    else {
        var jsonPostData = {
            trackGuid: $("#txtIQAgentSetupGnipTag_TW").val(),
            twitterRules: rules,
            agentName: $("#txtIQAgentSetupTitle").val(),
            searchRequestID: $("#hdnIQAgentSetupAddEditKey").val()
        }

        $.ajax({
            url: _urlSetupAgentSaveTwitterRule,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                if (result.isSuccess) {
                    if (result.isInsert) {
                        _HasUnsavedTwitterRule = true;
                        $("#txtIQAgentSetupGnipTag_TW").val(result.trackGuid);
                    }

                    CancelIQAgentPopup("divEditTwitterRulePopup");
                    ShowNotification("Rule saved successfully.");
                }
                else {
                    ShowNotification(_msgErrorOccured);
                }
            },
            error: function (a, b, c) {
                console.error("SaveTwitterRule error - " + c);
                ShowNotification(_msgErrorOccured);
            }
        });
    }
}

function AddTwitterRuleSection() {
    var textArea = $('<textarea rows="2" cols="40" class="twitterRuleTxt" />');
    $("#divTwitterRules").append(textArea);
}

///////// TVEyes Rule Edit /////////

function ShowHideEditTVEyesRule() {
    if ($("#chkIQAgentSetup_TM").is(":checked")) {
        $("#imgEditTVEyesRule").removeClass("displayNone");
    }
    else {
        if ($("#hdnIQAgentSetupTVEyesSettingsKey").val() != "0") {
            getConfirm("Confirm Disable", "Disabling this section will delete the search term. If the section is reenabled in the future, it will have to be recreated. Do you want to continue?", "Continue", "Cancel", function (res) {
                if (res == true) {
                    $("#imgEditTVEyesRule").addClass("displayNone");
                }
                else {
                    $("#chkIQAgentSetup_TM").prop("checked", true);
                }
            });
        }
        else {
            $("#imgEditTVEyesRule").addClass("displayNone");
        }
    }
}

function ShowEditTVEyesRulePopup() {
    var searchGuid = $("#txtIQAgentSetupTVEyesSearchGUID_TM").val();
    var tvEyesSettingsKey = $("#hdnIQAgentSetupTVEyesSettingsKey").val();

    $("#divTVEyesRuleMsg").html('');
    $("#txtTVEyesSearchTerm").val('');

    if (searchGuid != "") {
        $("#txtTVEyesSearchGuid").val(searchGuid);
    }
    else {
        $("#txtTVEyesSearchGuid").val("N/A");
    }

    if (tvEyesSettingsKey == "0") {
        $("#divEditTVEyesRulePopup").modal({
            backdrop: "static",
            keyboard: true,
            dynamic: true
        });
    }
    else {
        var jsonPostData = {
            tvEyesSettingsKey: tvEyesSettingsKey
        };

        $.ajax({
            url: _urlSetupAgentGetTVEyesRule,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                if (result.isSuccess) {
                    // Remove global rule section, since it will be readded on save
                    $("#txtTVEyesSearchTerm").val(result.ruleText.replace(_tvEyesGlobalRule, ""));

                    $("#divEditTVEyesRulePopup").modal({
                        backdrop: "static",
                        keyboard: true,
                        dynamic: true
                    });
                }
                else {
                    ShowNotification(_msgErrorOccured);
                }
            },
            error: function (a, b, c) {
                console.error("GetTVEyesRule error - " + c);
                ShowNotification(_msgErrorOccured);
            }
        });
    }
}

function SaveTVEyesRule() {
    var searchTerm = $("#txtTVEyesSearchTerm").val();

    if (searchTerm == "") {
        $("#divTVEyesRuleMsg").html(_msgIQAgentSetupTVEyesRuleRequiredField);
    }
    else if (searchTerm.indexOf("*") != -1) {
        $("#divTVEyesRuleMsg").html(_msgIQAgentSetupTVEyesRuleNoAsterisks);
    }
    else if (searchTerm.length > 30000) {
        $("#divTVEyesRuleMsg").html(_msgIQAgentSetupTVEyesRuleTooLong.replace("{0}", searchTerm.length));
    }
    else {
        var jsonPostData = {
            ruleID: $("#hdnIQAgentSetupTVEyesSettingsKey").val(),
            searchTerm: searchTerm + " " + _tvEyesGlobalRule,
            agentName: $("#txtIQAgentSetupTitle").val(),
            searchRequestID: $("#hdnIQAgentSetupAddEditKey").val()
        }

        $.ajax({
            url: _urlSetupAgentSaveTVEyesRule,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                if (result.isSuccess) {
                    if (result.isInsert) {
                        _HasUnsavedTVEyesRule = true;
                        $("#hdnIQAgentSetupTVEyesSettingsKey").val(result.ruleID)
                        $("#txtIQAgentSetupTVEyesSearchGUID_TM").val("Generating...");
                    }

                    CancelIQAgentPopup("divEditTVEyesRulePopup");
                    ShowNotification("Rule saved successfully.");
                }
                else {
                    ShowNotification(_msgErrorOccured);
                }
            },
            error: function (a, b, c) {
                console.error("SaveTVEyesRule error - " + c);
                ShowNotification(_msgErrorOccured);
            }
        });
    }
}