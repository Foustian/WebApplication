var IsUGCFileInit = false;
var _UGC_Map_ClientName = '';
var _UGC_Map_SearchTerm = '';
var _LogoWidth = 0;
var _LogoHeight = 0;
$(document).ready(function () {
    
});


function GetUGCSetupSettings(isNextPage) {
    var jsonPostData = {};

    if (typeof (isNextPage) === 'undefined') {
        jsonPostData = {
            p_ClientName: _UGC_Map_ClientName,
            p_SearchTerm: _UGC_Map_SearchTerm
        };
    }
    else {
        jsonPostData = {
            p_IsNext: isNextPage,
            p_ClientName: _UGC_Map_ClientName,
            p_SearchTerm: _UGC_Map_SearchTerm
        };
    }
    
    $.ajax({
        url: _urlGlobalAdmin_DisplayUGCSetupSettings,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        cache: false,
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {


                $("#liUGCMap").ActiveNav();
                $("#divClient_UGCMapPreviousNext").show();
                $(".span9-custom > div").hide();

                $("#divClient_UGCMap").show();

                $("#divClient_UGCMap_Content").html(result.HTML);

                $("#divClient_UGCMap_ScrollContent").css("height", documentHeight - 420);

                $("#divClient_UGCMap_ScrollContent").enscroll({
                    verticalScrolling: true,
                    verticalTrackClass: 'track4',
                    verticalHandleClass: 'handle4',
                    pollChanges: true,
                    addPaddingToPane: true,
                    margin : '0 0 0 10px'
                });

                showClient_UGCMapNoOfRecords(result);


                $("#ddlUGCMap_Client").val(_UGC_Map_ClientName);
                $("#txtUGCMap_SearchTerm").val(_UGC_Map_SearchTerm);
                $('#ddlUGCMap_Client').trigger("chosen:updated");
            }
            else {
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured, a);
        }
    });
}

function SearchClient_UGCMap() {
    if (_UGC_Map_ClientName != $("#ddlUGCMap_Client").val() || _UGC_Map_SearchTerm != $("#txtUGCMap_SearchTerm").val().trim()) {
        _UGC_Map_ClientName = $("#ddlUGCMap_Client").val();
        _UGC_Map_SearchTerm = $("#txtUGCMap_SearchTerm").val().trim();
        GetUGCSetupSettings();
    }
}

function ClearClient_UGCMap() {
    if (_UGC_Map_ClientName != '' || _UGC_Map_SearchTerm != '') {
        _UGC_Map_ClientName = ''
        _UGC_Map_SearchTerm = ''
        GetUGCSetupSettings();
    }
}

function showClient_UGCMapNoOfRecords(result) {
    if (result.hasMoreRecords) {
        $('#btnClient_UGCMapNextPage').show();
    }
    else {
        $('#btnClient_UGCMapNextPage').hide();
    }

    if (result.hasPreviousRecords) {
        $('#btnClient_UGCMapPreviousPage').show();
    }
    else {
        $('#btnClient_UGCMapPreviousPage').hide();
    }
    if (result.recordLabel != '') {
        $('#lblClient_UGCMapRecords').html(result.recordLabel);
    }
}

function EditClientUGCSettings(id) {
    var jsonPostData = {
        p_IQClient_UGCMapKey: id
    };
    
    $.ajax({
        url: _urlGlobalAdmin_EditUGCSetupSettings,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        cache: false,
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            IsUGCFileInit = false;
            if (result != null && result.isSuccess) {
                $(".span9-custom > div").hide();
                $("#divRegistration").html(result.HTML);

                $("#divRegistration").show();

                if ($("#aUGCLogo").length > 0) {
                    $("#aUGCLogo").rlightbox();
                }

                $('#frmUGCSettings input').keydown(function (e) {
                    if (e.keyCode == 13) {
                        SaveUGCSettings();
                    }
                });

                SetUGCFileSelection();
            }
            else {
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured, a);
        }
    });
}

function readURL(submission) {
    _LogoHeight = 0;
    _LogoWidth = 0;
    if (submission.files && submission.files[0]) {
        var reader = new FileReader();
        reader.onload = function (event) {
            var image = new Image();
            image.src = event.target.result;
            image.onload = function () {
                _LogoHeight = this.height;
                _LogoWidth = this.width;
                $("#logoPreviewDiv").show();
                $("#uploadedLogoPreview")
            .attr('src', event.target.result)
            .width(_LogoWidth)
            .height(_LogoHeight);
            }
        };
        reader.readAsDataURL(submission.files[0])
     }
 }

function SetUGCFileSelection() {

    if (!IsBrowserIE()) {
        $("input[type=file]").val("");

        $("#btnUGCMapLogoBrowseFile").show();
        $("#txtUGCMapLogorSelectedFileDisplay").show();
        $("#flUGCMapLogo").hide();
    }
    else {
        $("#flUGCMapLogo").replaceWith($("#flUGCMapLogo").clone(true));
        $("#txtUGCMapLogoSelectedFileDisplay").hide();
        $("#flUGCMapLogo").show();
        $("#btnUGCMapLogoBrowseFile").hide();
    }

    $("#txtUGCMapLogoSelectedFileDisplay").val("");

    if (!IsUGCFileInit) {
        IsUGCFileInit = true;
        $("#btnUGCMapLogoBrowseFile").click(function () { $("#flUGCMapLogo").trigger('click'); });
        $("#flUGCMapLogo").change(function () {

            var selectedfileType = $("#flUGCMapLogo").val().substring($("#flUGCMapLogo").val().lastIndexOf('.') + 1).toLowerCase();
            if ($.inArray(selectedfileType, imageFileTypes) != -1) {
                $("#txtUGCMapLogoSelectedFileDisplay").val($(this).val());
                $("#spanIQClient_UGCMapModel_Logo").html("").hide();
            }
            else {
                $("#txtUGCMapLogoSelectedFileDisplay").val("");
                $("#spanIQClient_UGCMapModel_Logo").html(_msgSelectValidUGCFileType).show();
            }
        });
    }
}

var xhrUGC;
var xhrUGCIE;
var IsUGCUploadActive = false;
function SaveUGCSettings() {
    if (ValidateUGCMapSettings()) {
        if (!IsBrowserIE()) {
            //IsFileInit = false;
            IsUGCUploadActive = true;
            xhrUGC = new XMLHttpRequest();

            ShowLoading();

            var formdata = new FormData($('#frmUGCSettings')[0]);

            xhrUGC.addEventListener("progress", function (e) {
                var done = e.position || e.loaded, total = e.totalSize || e.total;
            }, false);

            if (xhrUGC.upload) {
                xhrUGC.upload.onprogress = function (e) {
                    var done = e.position || e.loaded, total = e.totalSize || e.total;
                    //$("#divProgressbar").progressbar("value", (Math.floor(done / total * 1000) / 10));
                };
            }

            xhrUGC.addEventListener("error", function (evt) {
                IsUGCUploadActive = false;
                IsFileInit = false;
                CancelFileUpload();
                ShowNotification(_msgErrorWhileSavingUGCContent);
            }, false);

            xhrUGC.addEventListener("load", function (e) {
            }, false);

            xhrUGC.onreadystatechange = function (e) {
                if (4 == this.readyState) {
                    CheckForSessionExpired(this.response);
                    IsUGCUploadActive = false;
                    var res = { success: false, error: "", clientId: "0" };
                    // chrome not allow directly to access json object, so we need to parse JSON and use afterwards. 
                    // Firefox allows json object so no need to do same.

                    if (this.response.isSuccess == undefined) {
                        var jsonObj = $.parseJSON(this.response);
                        res.success = jsonObj.isSuccess;
                        res.error = jsonObj.error;
                    }
                    else {
                        res.success = this.response.isSuccess;
                        res.error = this.response.error;
                    }

                    //$("#divProgressbar").hide();
                    CancelUGCFileUpload();
                    if (res.success == true) {
                        IsUGCFileInit = false;
                        ShowNotification(_msgUGCSettingsSaved);
                        GetUGCSetupSettings();
                    }
                    else {
                        ShowNotification(res.error);
                        HideLoading();
                    }
                }
            };

            xhrUGC.open("post", _urlGlobalAdmin_SubmitUGCSettings, true);
            xhrUGC.responseType = "json";
            xhrUGC.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
            xhrUGC.send(formdata);
        }
        else {
            IsUGCUploadActive = true;
            if ($("#flUGCMapLogo").val() == "") {
                $("#hfUGCMapLogoImage").val($("#txtUGCMapLogoSelectedFileDisplay").val());
            }
            else {
                $("#hfUGCMapLogoImage").val("");
            }



            $("#frmUGCSettings").ajaxSubmit({
                target: "",
                beforeSend: function (jqxhrUGC, settings) {
                    xhrUGCIE = jqxhrUGC;
                },
                success: function (res) {
                    IsUGCUploadActive = false;
                    CheckForSessionExpired(res);
                    //$("#divIEProgressbar").hide();
                    CancelUGCFileUpload();
                    var obj = $.parseJSON(res);
                    if (obj.isSuccess) {
                        IsUGCFileInit = false;
                        ShowNotification(_msgUGCSettingsSaved);
                        GetUGCSetupSettings();
                    }
                    else {
                        ShowNotification(obj.errorMsg);
                        HideLoading();
                    }
                }
            })
        }
    }
}

function CancelUGCFileUpload() {
    if (IsUGCUploadActive) {
        if (IsBrowserIE()) {
            xhrUGCIE.abort();
        }
        else {
            xhrUGC.abort();
        }
    }
}

function ValidateUGCMapSettings() {
    var isValid = true;

    $("#spanIQClient_UGCMapModel_SourceID").hide();
    $("#spanIQClient_UGCMapModel_BroadcastLocation").hide();
    $("#spanIQClient_UGCMapModel_BroadcastType").hide();
    $("#spanIQClient_UGCMapModel_RetentionDays").hide();
    $("#spanIQClient_UGCMapModel_Title").hide();
    $("#spanIQClient_UGCMapModel_URL").hide();
    $("#spanIQClient_UGCMapModel_Logo").hide();
    $("#spanIQClient_UGCMapModel_ClientGuid").hide();
    $('#spanIQClient_UGCMapModel_TimeZone').hide();

    if ($('#IQClient_UGCMapModel_TimeZone').val().trim() == '0') {
        $('#spanIQClient_UGCMapModel_TimeZone').html(_msgCustomerRegistrationTimeZoneRequiredField).show();
        isValid = false;
    }
    if ($("#IQClient_UGCMapModel_ClientGuid").val().trim() == '0') {
        $("#spanIQClient_UGCMapModel_ClientGuid").html(_msgCustomerRegistrationClientRequiredField).show();
        isValid = false;
    }

    if ($("#IQClient_UGCMapModel_SourceID").val().trim() == '') {
        $("#spanIQClient_UGCMapModel_SourceID").html(_msgSourceIDRequired).show();
        isValid = false;
    }
    else if ($("#IQClient_UGCMapModel_SourceID").val().trim().length > 8) {
        $("#spanIQClient_UGCMapModel_SourceID").html(_msgSourceIDMax6ChartLong).show();
        isValid = false;
    }

    if ($("#IQClient_UGCMapModel_BroadcastType").val().trim() == '0') {
        $("#spanIQClient_UGCMapModel_BroadcastType").html(_msgBroadcastTypeRequired).show();
        isValid = false;
    }
    if ($("#IQClient_UGCMapModel_Title").val().trim() == '') {
        $("#spanIQClient_UGCMapModel_Title").html(_msgTitleRequired).show();
        isValid = false;
    }

    if ($("#IQClient_UGCMapModel_URL").val().trim() == '') {
        $("#spanIQClient_UGCMapModel_URL").html(_msgUrlRequired).show();
        isValid = false;
    }
    else if (!ValidateUrl($("#IQClient_UGCMapModel_URL").val().trim())) {
        $("#spanIQClient_UGCMapModel_URL").html(_msgInvalidUrl).show();
        isValid = false;
    }

    if ($("#flUGCMapLogo").val().trim() == "") {
        if ($("#aUGCLogo").length == 0) {
            $("#spanIQClient_UGCMapModel_Logo").html(_msgNoFileToUpload).show();
            isValid = false;
        }
    }
    else if ($("#flUGCMapLogo").val().trim() != "") {
        var selectedfileType = $("#flUGCMapLogo").val().substring($("#flUGCMapLogo").val().lastIndexOf('.') + 1).toLowerCase();
        imageFileTypes = ["jpeg", "jpg"];
        if ($.inArray(selectedfileType, imageFileTypes) == -1 || (_LogoHeight>50 && _LogoWidth>50)) {
            $("#txtUGCMapLogoSelectedFileDisplay").val("");
            $("#spanIQClient_UGCMapModel_Logo").html(_msgSelectValidUGCFileType).show();
            isValid = false;
        }
    }

    return isValid;
}

function CancelUGCSettings() {
    $(".span9-custom > div").hide();
    $("#divClient_UGCMap").show();
}