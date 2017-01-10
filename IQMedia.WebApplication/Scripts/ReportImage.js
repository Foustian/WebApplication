var IsFileInit = false;
var CONST_DeleteNote = "Note:  By deleting this image, if image is associated with any reports, then those reports will display default image";

$(document).ready(function () {
    $('#frmClient input').keydown(function (e) {
        if (e.keyCode == 13) {
            SaveReportImage();
        }
    });
});

function SetFileSelection() {

    if (!IsBrowserIE()) {
        $("input[type=file]").val("");

        $("#btnCustomHeaderBrowseFile").show();
        $("#txtCustomHeaderSelectedFileDisplay").show();
        $("#flCustomHeader").hide();
    }
    else {
        $("#flCustomHeader").replaceWith($("#flCustomHeader").clone(true));
        $("#txtCustomHeaderSelectedFileDisplay").hide();
        $("#flCustomHeader").show();
        $("#btnCustomHeaderBrowseFile").hide();
    }

    $("#txtCustomHeaderSelectedFileDisplay").val("");
    $("#txtPlayerLogoSelectedFileDisplay").val("");
    $("#txtNotoficationHeaderSelectedFileDisplay").val("");

    if (!IsFileInit) {
        IsFileInit = true;
        $("#btnCustomHeaderBrowseFile").click(function () { $("#flCustomHeader").trigger('click'); });
        $("#flCustomHeader").change(function () {

            var selectedfileType = $("#flCustomHeader").val().substring($("#flCustomHeader").val().lastIndexOf('.') + 1).toLowerCase();
            if ($.inArray(selectedfileType, imageFileTypes) != -1) {
                $("#txtCustomHeaderSelectedFileDisplay").val($(this).val());
                $("#spanCustomHeader").html("").hide();
                setimagepreview();
            }
            else {
                $("#txtCustomHeaderSelectedFileDisplay").val("");
                $("#spanCustomHeader").html(_msgSelectValidUGCFileType).show();
                $("#imgthumb").attr('src', '').hide();
            }
        });
    }
}


function GetReportImages() {
    var jsonPostData = {};
    $.ajax({
        url: _urlClientCustomImageDisplayReportImages,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        cache: false,
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            IsFileInit = false;
            if (result != null && result.isSuccess) {
                $("#divSetupContent").html(result.HTML);
                $(".imgHeader").rlightbox();
            }
            else {
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured,a);
        }
    });
}

function DeleteReportImage(reportkey,imagename) {
    getConfirm("Delete Report Image", CONST_DeleteNote + "<br/><br/>" + _msgConfirmReportImageDelete, "Confirm Delete", "Cancel", function (res) {
        if (res) {
            var jsonPostData = { p_ID: reportkey, p_Image: imagename }

            $.ajax({
                url: _urlClientCustomImageDeleteReportImage,
                contentType: "application/json; charset=utf-8",
                type: "post",
                dataType: 'json',
                data: JSON.stringify(jsonPostData),
                success: function (result) {
                    IsFileInit = false;
                    CheckForSessionExpired(result);

                    if (result != null && result.isSuccess) {
                        GetReportImages();
                        ShowNotification(_msgReportImageDeleted);
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

var xhr;
var xhrIE;
var IsUploadActive = false;
function SaveReportImage() {
    if (ValidateReportImage()) {
        if (!IsBrowserIE()) {
            //IsFileInit = false;
            IsUploadActive = true;
            xhr = new XMLHttpRequest();
           
            ShowLoading();

            var formdata = new FormData($('#frmReportImage')[0]);

            xhr.addEventListener("progress", function (e) {
                var done = e.position || e.loaded, total = e.totalSize || e.total;
            }, false);

            if (xhr.upload) {
                xhr.upload.onprogress = function (e) {
                    var done = e.position || e.loaded, total = e.totalSize || e.total;
                    //$("#divProgressbar").progressbar("value", (Math.floor(done / total * 1000) / 10));
                };
            }

            xhr.addEventListener("error", function (evt) {
                IsUploadActive = false;
                IsFileInit = false;
                CancelFileUpload();
                ShowNotification(_msgErrorWhileSavingUGCContent);
            }, false);

            xhr.addEventListener("load", function (e) {
            }, false);

            xhr.onreadystatechange = function (e) {
                if (4 == this.readyState) {
                    CheckForSessionExpired(this.response);
                    IsUploadActive = false;
                    var res = { success: false, error: "", clientId: "0" };
                    // chrome not allow directly to access json object, so we need to parse JSON and use afterwards. 
                    // Firefox allows json object so no need to do same.

                    if (this.response.isSuccess == undefined) {
                        var jsonObj = $.parseJSON(this.response);
                        res.success = jsonObj.isSuccess;
                        res.error = jsonObj.errorMsg;
                        res.clientId = jsonObj.clientId;
                    }
                    else {
                        res.success = this.response.isSuccess;
                        res.error = this.response.errorMsg;
                    }

                    //$("#divProgressbar").hide();
                    CancelFileUpload();
                    if (res.success == true) {
                        IsFileInit = false;
                        ShowNotification(_msgReportImageAdded);
                        GetReportImages();
                    }
                    else {
                        ShowNotification(res.error);
                        HideLoading();
                    }
                }
            };

            xhr.open("post", _urlClientCustomImageInsertReportImage, true);
            xhr.responseType = "json";
            xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
            xhr.send(formdata);
        }
        else {
            IsUploadActive = true;
            if ($("#flCustomHeader").val() == "") {
                $("#hfCustomHeaderImage").val($("#txtCustomHeaderSelectedFileDisplay").val());
            }
            else {
                $("#hfCustomHeaderImage").val("");
            }



            $("#frmReportImage").ajaxSubmit({
                target: "",
                beforeSend: function (jqXHR, settings) {
                    xhrIE = jqXHR;
                },
                success: function (res) {
                    IsUploadActive = false;
                    CheckForSessionExpired(res);
                    //$("#divIEProgressbar").hide();
                    CancelFileUpload();
                    var obj = $.parseJSON(res);
                    if (obj.isSuccess) {
                        IsFileInit = false;
                        ShowNotification(_msgReportImageAdded);
                        GetReportImages();
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

function CancelFileUpload() {
    if (IsUploadActive) {
        if (IsBrowserIE()) {
            xhrIE.abort();
        }
        else {
            xhr.abort();
        }
    }
    $("#divReportImageAddPopup").css({ "display": "none" });
    $("#divReportImageAddPopup").modal("hide");
}

function OpenAddReportImage() {
    
    SetFileSelection();
    $("#imgthumb").attr('src', '').hide();
    $("#spanCustomHeader").html("").hide();
    $("#chkIsReplaceImage").prop("checked", true);

    cropX = null;
    cropY = null;
    cropWidth = null;
    cropHeight = null;

    $("#hdnCropWidth").val(cropWidth);
    $("#hdnCropHeight").val(cropHeight);
    $("#hdnCropX").val(cropX);
    $("#hdnCropY").val(cropY);

    $("#divReportImageAddPopup").modal({
        backdrop: 'static',
        keyboard: true,
        dynamic: true
    });
}

function ValidateReportImage() {
    var flag = true;

    $("#spanCustomHeader").html('').hide();

    if ($("#flCustomHeader").val().trim() == "") {
        $("#spanCustomHeader").html(_msgNoFileToUpload).show();
        flag = false;
    }
    else if ($("#flCustomHeader").val().trim() != "") {
        var selectedfileType = $("#flCustomHeader").val().substring($("#flCustomHeader").val().lastIndexOf('.') + 1).toLowerCase();
        if ($.inArray(selectedfileType, imageFileTypes) == -1) {
            $("#txtCustomHeaderSelectedFileDisplay").val("");
            $("#spanCustomHeader").html(_msgSelectValidUGCFileType).show();
            flag = false;
        }
    }
    return flag;
}

function IsBrowserIE() {
    return navigator.userAgent.toLowerCase().indexOf("msie") > -1;
}

function SetDefaultImage(imagekey) {
    var jsonPostData = { p_ID: imagekey }

    $.ajax({
        url: _urlClientCustomImageUpdateIsDefault,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: 'json',
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            IsFileInit = false;
            if (result != null && result.isSuccess) {
                GetReportImages();
                ShowNotification(_msgReportImageSetDefault);
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

function SetDefaultEmailImage(imagekey) {
    var jsonPostData = { p_ID: imagekey }

    $.ajax({
        url: _urlClientCustomImageUpdateIsDefaultEmail,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: 'json',
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            IsFileInit = false;
            if (result != null && result.isSuccess) {
                GetReportImages();
                ShowNotification(_msgReportImageSetDefaultEmail);
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


var cropWidth = null;
var cropHeight = null;
var cropX = null;
var cropY = null;
var orgImageWidth = null;
var orgImageHeight = null;
var porcX = 1;
var porcY = 1;
var cropImg = new Image();
var instAreaSelect = null;
function setimagepreview() {
    var varfiles = document.getElementById('flCustomHeader');
    if (varfiles.files && varfiles.files[0]) {
        var filerdr = new FileReader();
        filerdr.onload = function (e) {
            $('#imgprvw').attr('src', e.target.result);
            $('#imgthumb').attr('src', e.target.result);

            cropImg.src = $('#imgprvw').attr('src');
            cropImg.onload = function () {
                orgImageWidth = cropImg.width;
                orgImageHeight = cropImg.height;
                clearcrop(true);
            }

        }
        filerdr.readAsDataURL(varfiles.files[0]);
    }
}

function showcropdialog() {
    var varfiles = document.getElementById('flCustomHeader');
    if (varfiles.files && varfiles.files[0]) {
        $("#divReportImagePreview").modal({
            backdrop: 'static',
            keyboard: true,
            dynamic: true
        });
    }
    else {
        ShowNotification(_msgNoFileToPreview)
    }
//    if (IsBrowserIE()) {
//        if (varfiles.value.trim() != "") {
//            var newPreview = document.getElementById("preview_ie");
//            newPreview.filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = varfiles.value;
//            newPreview.style.width = "100%";
//            newPreview.style.height = "100%";

//            $("#divReportImagePreview").modal({
//                backdrop: 'static',
//                keyboard: true,
//                dynamic: true
//            });
//        }
//        else {
//            ShowNotification(_msgNoFileToPreview)
//        }
//    }
//    else
//    {
//        if (varfiles.files && varfiles.files[0]) {
//            var filerdr = new FileReader();
//            filerdr.onload = function (e) {
//                $('#imgprvw').attr('src', e.target.result);
//                $('#imgthumb').attr('src', e.target.result);
//            }
//            filerdr.readAsDataURL(varfiles.files[0]);

//            $('#imgprvw').imgAreaSelect({
//                x1: 10,
//                y1: 10,
//                x2: 40,
//                y2: 40,
//                handles: true,
//                parent: '#divLogo',
//                onSelectChange: function (img, selection) {
//                    cropWidth = selection.width;
//                    cropHeight = selection.height;
//                    cropX = selection.x1;
//                    cropY = selection.y1;
//                }
//            });

//            $("#divReportImagePreview").modal({
//                backdrop: 'static',
//                keyboard: true,
//                dynamic: true
//            });
//        }
//        else {
//            ShowNotification(_msgNoFileToPreview)
//        }
//    }
}

function cropimage() {
    $("#hdnCropWidth").val(cropWidth);
    $("#hdnCropHeight").val(cropHeight);
    $("#hdnCropX").val(cropX);
    $("#hdnCropY").val(cropY);
    preview();
    closeModal('divReportImagePreview');
}

function preview() {
//    var scaleX = 200 / cropWidth;
//    var scaleY = 200 / cropHeight;
    
//    $('#imgthumb').show();

//    $('#imgthumb').css({
//        width: Math.round(scaleX * orgImageWidth) + 'px',
//        height: Math.round(scaleY * orgImageHeight) + 'px',
//        marginLeft: '-' + Math.round(scaleX * xPos) + 'px',
//        marginTop: '-' + Math.round(scaleY * yPos) + 'px'
    //    });

    var xPos = cropX == null ? 0 : cropX;
    var yPos = cropY == null ? 0 : cropY;

    var temp_ctx, temp_canvas;
    temp_canvas = document.createElement('canvas');
    temp_ctx = temp_canvas.getContext('2d');
    temp_canvas.width = cropWidth;
    temp_canvas.height = cropHeight;
    temp_ctx.drawImage(cropImg, xPos, yPos, cropWidth, cropHeight, 0, 0, cropWidth, cropHeight);
    var vData = temp_canvas.toDataURL();
    $('#imgthumb').attr('src', vData);
    $('#imgthumb').show();
}

function ShowFullPreview() {
    $('#imgthumbFull').attr('src', $('#imgthumb').attr('src'));
    $("#divReportCropImagePreview").modal({
        backdrop: 'static',
        keyboard: true,
        dynamic: true
    });
}

function clearcrop(isReset) {

    cropWidth = orgImageWidth;
    cropHeight = orgImageHeight;
    cropX = null;
    cropY = null;

    if (typeof (isReset) != 'undefined' && isReset) {
        $('#imgprvw').imgAreaSelect({ remove: true });
        instAreaSelect = $('#imgprvw').imgAreaSelect({
            //aspectRatio : orgImageWidth + ':' + orgImageHeight,
            x1: 10,//cropX == null ? 10 : Math.round(cropX / porcX),
            y1: 10,//cropY == null ? 10 : Math.round(cropY / porcY),
            x2: 40,//cropX == null ? 40 : Math.round(cropX + cropWidth) / porcX,
            y2: 40,//cropY == null ? 40 : Math.round((cropY + cropHeight) / porcY),
            handles: true,
            parent: '#divLogo',
            instance: true,
            show : true,
            imageHeight: orgImageHeight,
            imageWidth: orgImageWidth,
            onSelectEnd: function (img, selection) {
                if (!selection.width || !selection.height) {
                    return;
                }

                porcX = img.naturalWidth / img.width;
                porcY = img.naturalHeight / img.height;

                cropX = Math.round(selection.x1 * porcX);
                cropY = Math.round(selection.y1 * porcY);
                cropWidth = Math.round(selection.width * porcX);
                cropHeight = Math.round(selection.height * porcY);
            }
        });
    }

    instAreaSelect.cancelSelection();   
    preview();
}