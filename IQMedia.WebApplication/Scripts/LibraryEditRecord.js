var _Source = "";
var _EditRecordMode = false;

function OpenEditRecord(itemid, source) {
    _Source = source;

    $("#spanTitle").html("").hide();
    $("#spanCategory").html("").hide();
    $("#spanKeywords").html("").hide();

    $("#divEditHTML button[type=button]").removeAttr("disabled");

    var jsonPostData = { ID: itemid }
    
    $.ajaxSetup({ cache: false });

    $.ajax({
        url: _urlLibraryGetEditRecord,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: 'json',
        data: JSON.stringify(jsonPostData),
        success: function (result) {

            CheckForSessionExpired(result);

            if (result.isSuccess) {

                $("#divEditHTML").html(result.HTML);

                if (source == "report") {
                    $("#divEditCategory").hide();
                    $("#divEditSubcategory1").hide();
                    $("#divEditSubcategory2").hide();
                    $("#divEditSubcategory3").hide();
                    $("#divEditKeywords").hide();
                    $("#divEditPosSentiment").hide();
                    $("#divEditNegSentiment").hide();
                }
                else {
                    $("#divEditCategory").show();
                    $("#divEditSubcategory1").show();
                    $("#divEditSubcategory2").show();
                    $("#divEditSubcategory3").show();
                    $("#divEditKeywords").show();
                    $("#divEditPosSentiment").show();
                    $("#divEditNegSentiment").show();
                }

                $('#divEditRecordPopup').modal({
                    backdrop: 'static',
                    keyboard: true,
                    dynamic: true
                });

                CheckCategoryValuesOnLoad();
            }
            else {
                ShowNotification(_msgSomeErrorProcessing);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgSomeErrorProcessing);
        }
    });
}

function CancelEditPopup() {
    $("#divEditRecordPopup").css({ "display": "none" });
    $("#divEditRecordPopup").modal("hide");
}

function UpdateRecord() {
    if (ValidateEditRecord()) {
    
        var jsonPostData = {
            p_ID: $("#hdIQArchiveMediaKey").val(),
            p_Title: $("#txtTitle").val(),
            p_CategoryGuid: $("#ddlCategory").val(),
            p_SubCategory1Guid: $("#ddlSubCategory1").val(),
            p_SubCategory2Guid: $("#ddlSubCategory2").val(),
            p_SubCategory3Guid: $("#ddlSubCategory3").val(),
            p_Keywords: $("#txtKeywords").val(),
            p_Description: $("#txtDescription").val(),
            p_DisplayDescription: $("#chkDisplayDescription").prop("checked"),
            p_PositiveSentiment: $("#txtPosSentiment").val(),
            p_NegativeSentiment: $("#txtNegSentiment").val(),
        }

        $("#divEditHTML button[type=button]").prop("disabled", "disabled");

        $.ajaxSetup({ cache: false });

        $.ajax({
            url: _urlLibraryUpdateMediaRecord,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: 'json',
            data: JSON.stringify(jsonPostData),
            success: function (result) {

                CheckForSessionExpired(result);

                if (result.isSuccess) {
                    CancelEditPopup();
                    ShowNotification(_msgRecordUpdated);

                    if (_Source == "report") {
                        ShowNotification("Updating Report...");
                        $("#divReportContent").addClass("blur");

                        // Attach a done, fail, and progress handler for the asyncEvent
                        $.when(GenerateLibraryReport($("#hdnReportID").val())).then(
                          function (status) {
                              $('#divNotification').remove();
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
                        if ($("#spnTitle_" + $("#hdIQArchiveMediaKey").val()) != undefined) {
                            $("#spnTitle_" + $("#hdIQArchiveMediaKey").val()).html($("#txtTitle").val());
                        }

                        _EditRecordMode = true;
                        RefreshResults();
                    }


                    
                }
                else {
                    ShowNotification(_msgSomeErrorProcessing);
                }

                $("#divEditHTML button[type=button]").removeAttr("disabled");
            },
            error: function (a, b, c) {
                ShowNotification(_msgSomeErrorProcessing);
                $("#divEditHTML button[type=button]").removeAttr("disabled");
            }
        });
    }
}

function ValidateEditRecord() {

    var flag = true;

    $("#spanTitle").html("");
    $("#spanCategory").html("");
    $("#spanKeywords").html("");
    $("#spanPosSentiment").html("");
    $("#spanNegSentiment").html("");

    if ($.trim($("#txtTitle").val()) == "") {
        $("#spanTitle").html(_msgTitleRequired).show();
        flag = false;
    }

    if (_Source != "report") {
        if ($("#ddlCategory").val() == "") {
            $("#spanCategory").html(_msgCategoryRequired).show();
            flag = false;
        }

        var posSentiment = $("#txtPosSentiment").val();
        if (posSentiment == "") {
            $("#spanPosSentiment").html(_msgSentimentRequired).show();
            flag = false;
        }
        else if (parseInt(posSentiment) < 0 || parseInt(posSentiment) > 255) {
            $("#spanPosSentiment").html(_msgSentimentOutOfRange).show();
            flag = false;
        }

        var negSentiment = $("#txtNegSentiment").val();
        if (negSentiment == "") {
            $("#spanNegSentiment").html(_msgSentimentRequired).show();
            flag = false;
        }
        else if (parseInt(negSentiment) < 0 || parseInt(negSentiment) > 255) {
            $("#spanNegSentiment").html(_msgSentimentOutOfRange).show();
            flag = false;
        }
    }

    return flag;
}

function CheckCategoryValuesOnLoad() {
    if ($("#ddlCategory").val() != "") {
        OnSelectOfCategory("ddlCategory");
    }
    if ($("#ddlSubCategory1").val() != "") {
        OnSelectOfCategory("ddlSubCategory1");
    }
    if ($("#ddlSubCategory2").val() != "") {
        OnSelectOfCategory("ddlSubCategory2");
    }
}

function OnSelectOfCategory(ddl_id) {

    var PCatId = "ddlCategory";
    var SubCat1Id = "ddlSubCategory1";
    var SubCat2Id = "ddlSubCategory2";
    var SubCat3Id = "ddlSubCategory3";

    $("#divCommonErrorMsg").html("").hide();

    var PCatSelectedValue = $("#" + PCatId).val();
    var Cat1SelectedValue = $("#" + SubCat1Id).val();
    var Cat2SelectedValue = $("#" + SubCat2Id).val();
    var Cat3SelectedValue = $("#" + SubCat3Id).val();


    if (ddl_id == PCatId) {

        $("#" + SubCat1Id + " option").removeAttr("disabled");
        $("#" + SubCat2Id + " option").removeAttr("disabled");
        $("#" + SubCat3Id + " option").removeAttr("disabled");

        if (PCatSelectedValue != "") {

            $("#" + SubCat1Id + " option[value='" + PCatSelectedValue + "']").attr("disabled", "disabled");
            $("#" + SubCat2Id + " option[value='" + PCatSelectedValue + "']").attr("disabled", "disabled");
            $("#" + SubCat3Id + " option[value='" + PCatSelectedValue + "']").attr("disabled", "disabled");


            if (PCatSelectedValue == Cat1SelectedValue) {
                $("#" + SubCat1Id + "").val("");
                $("#" + SubCat2Id + "").val("");
                $("#" + SubCat3Id + "").val("");
            }
            else if (PCatSelectedValue == Cat2SelectedValue) {
                $("#" + SubCat2Id + "").val("");
                $("#" + SubCat3Id + "").val("");
            }
            else if (PCatSelectedValue == Cat3SelectedValue) {
                $("#" + SubCat3Id + "").val("");
            }


            if ($("#" + SubCat1Id + "").val() != "") {
                $("#" + SubCat2Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
                $("#" + SubCat3Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
            }

            if ($("#" + SubCat2Id + "").val() != "") {
                $("#" + SubCat3Id + " option[value='" + $("#" + SubCat2Id + "").val() + "']").attr("disabled", "disabled");
            }

        }
        else {
            $("#" + SubCat1Id + "").val("");
            $("#" + SubCat2Id + "").val("");
            $("#" + SubCat3Id + "").val("");
        }
    }
    else if (ddl_id == SubCat1Id) {
        if (PCatSelectedValue != "") {

            $("#" + SubCat2Id + " option").removeAttr("disabled");
            $("#" + SubCat3Id + " option").removeAttr("disabled");

            $("#" + SubCat2Id + " option[value='" + $("#" + PCatId + "").val() + "']").attr("disabled", "disabled");
            $("#" + SubCat3Id + " option[value='" + $("#" + PCatId + "").val() + "']").attr("disabled", "disabled");

            if (Cat1SelectedValue != "") {

                $("#" + SubCat2Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");

                $("#" + SubCat3Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");

                if (Cat1SelectedValue == Cat2SelectedValue) {
                    $("#" + SubCat2Id + "").val("");
                    $("#" + SubCat3Id + "").val("");
                }
                else if (Cat1SelectedValue == Cat3SelectedValue) {
                    $("#" + SubCat3Id + "").val("");
                }

                if ($("#" + SubCat1Id + "").val() != "") {
                    $("#" + SubCat2Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
                    $("#" + SubCat3Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
                }

                if ($("#" + SubCat2Id + "").val() != "") {
                    $("#" + SubCat3Id + " option[value='" + $("#" + SubCat2Id + "").val() + "']").attr("disabled", "disabled");
                }
            }
            else {
                $("#" + SubCat2Id + "").val("");
                $("#" + SubCat3Id + "").val("");
            }

        }
        else {
            $("#" + SubCat1Id + "").val("");
            $("#" + SubCat2Id + "").val("");
            $("#" + SubCat3Id + "").val("");

            $("#divCommonErrorMsg").html(_msgFirstSelectPrecedingCat).show();
        }
    }
    else if (ddl_id == SubCat2Id) {
        if (PCatSelectedValue != "" && Cat1SelectedValue != "") {
            $("#" + SubCat3Id + " option").removeAttr("disabled");

            $("#" + SubCat3Id + " option[value='" + $("#" + PCatId + "").val() + "']").attr("disabled", "disabled");
            $("#" + SubCat3Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");

            if (Cat2SelectedValue != "") {

                $("#" + SubCat3Id + " option[value='" + $("#" + SubCat2Id + "").val() + "']").attr("disabled", "disabled");

                if (Cat2SelectedValue == Cat3SelectedValue) {
                    $("#" + SubCat3Id + "").val("");
                }

                if ($("#" + SubCat1Id + "").val() != "") {
                    $("#" + SubCat2Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
                    $("#" + SubCat3Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
                }

                if ($("#" + SubCat2Id + "").val() != "") {
                    $("#" + SubCat3Id + " option[value='" + $("#" + SubCat2Id + "").val() + "']").attr("disabled", "disabled");
                }
            }
            else {
                $("#" + SubCat3Id + "").val("");
            }
        }
        else {
            $("#" + SubCat2Id + "").val("");
            $("#" + SubCat3Id + "").val("");

            $("#divCommonErrorMsg").html(_msgFirstSelectPrecedingCat).show();

        }
    }
    else if (ddl_id == SubCat3Id) {
        if (PCatSelectedValue == "" || Cat1SelectedValue == "" || Cat2SelectedValue == "") {
            $("#" + SubCat3Id + "").val("");

            $("#divCommonErrorMsg").html(_msgFirstSelectPrecedingCat).show();
        }
    }
}
