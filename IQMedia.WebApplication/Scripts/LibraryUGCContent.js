var HasUGCResultsLoadedFirstTime = false;
var _IQUGCArchiveSearchTerm = "";
var _IQUGCArchiveFromDate = null;
var _IQUGCArchiveToDate = null;
var _IQUGCArchiveCategoryGUID = [];
var _IQUGCArchiveCategoryNameList = [];
var _IQUGCArchiveCategoryName = "";
var _IQUGCArchiveCustomer = "";
var _IQUGCArchiveCustomerName = "";
var _IQUGCArchiveIsAsc = false;
var _IQUGCArchiveDuration = null;
var _IQUGCArchiveSortColumn = "AirDate";
var disabledDays_IQArchiveUGC = null;
var IsUploadActive = false;
var _IQUGCArchivePageSize = null;
var _PreviousSelectedFileName = '';
var _IQUGCArchiveSelectionType = 'OR';
var _IQUGCArchiveOldCategoryGUID = [];
var _IQUGCArchiveCurrentCategoryFilter = new Array();
var _IsCategoryLoaded = false;
var _IsCategoryRequestOnProgress = false;
var _IQUGCArchiveMediaType = "";
var _IQUGCArchiveMediaTypeDesc ="";
$(function () {

    disabledDays_IQArchiveUGC = [];

    BindCustomCategoryDropDown();

    //$("#divUGCContentFormScrollContent").css("height", documentHeight - 250);

    $("#dpUGCAirDate").datepicker({
        showOn: "button",
        buttonImage: "../images/calender.png",
        buttonImageOnly: true
    });

    $("#txtIQUGCArchiveKeyword").keypress(function (e) {
        if (e.keyCode == 13) {
            SetKeyword_IQUGCArchive();
        }
    });

    $("#txtIQUGCArchiveKeyword").blur(function () {
        SetKeyword_IQUGCArchive();
    });

    $("#imgIQUGCArchiveKeyword").click(function (e) {
        SetKeyword_IQUGCArchive();
    });

    $("#divIQUGCArchiveCalender").datepicker({
        beforeShowDay: enableAllTheseDays_IQArchiveUGC,
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            $("#dpIQUGCArchiveFrom").val(dateText);
            $("#dpIQUGCArchiveTo").val(dateText);
            SetDateVariable_IQUGCArchive();
        }
    });

    $('.ndate').click(function () {
        $("#divIQUGCArchiveCalender").datepicker("refresh");
    });

    $("body").click(function(e) {
        
        if ((e.target.id != "aUGCPageSize" && e.target.id != "divUGCPopover") && $(e.target).parents("#divUGCPopover").size() <= 0) {
            $('#divUGCPopover').remove();
        }
        
        if (e.target.id == "liIQUGCArchiveCategoryFilter" || $(e.target).parents("#liIQUGCArchiveCategoryFilter").size() > 0)
        {
            if ($('#ulIQUGCArchiveCategory').is(':visible')) {            
                $('#ulIQUGCArchiveCategory').hide();
            }
            else {
                $('#ulIQUGCArchiveCategory').show();
            }
        }
        else if ((e.target.id !== "liIQUGCArchiveCategoryFilter" && e.target.id !== "ulIQUGCArchiveCategory"   && $(e.target).parents("#ulIQUGCArchiveCategory").size() <= 0) || e.target.id == "btnArchiveUGCSearchCategory") { 
            $('#ulIQUGCArchiveCategory').hide();
            if(e.target.id != "btnArchiveUGCSearchCategory")
            {
                _IQUGCArchiveCategoryGUID = [];
                _IQUGCArchiveCategoryNameList = [];
                var categoriesHTML  ="";
                if(_IQUGCArchiveSelectionType == $('#rdArchiveUGCAnd').val())
                {
                    $('#rdArchiveUGCAnd').prop("checked", true);
                }
                else
                {
                    $('#rdArchiveUGCOr').prop("checked", true);
                }

                $.each(_IQUGCArchiveCurrentCategoryFilter, function (eventID, eventData) {
                    if(_IQUGCArchiveOldCategoryGUID.length > 0 && $.inArray(eventData.CategoryGUID, _IQUGCArchiveOldCategoryGUID) !== -1)
                    {
                        _IQUGCArchiveCategoryGUID.push(eventData.CategoryGUID);
                        _IQUGCArchiveCategoryNameList.push(eventData.CategoryName);
                    }
                    var liStyle= "";
                    if ($.inArray(eventData.CategoryGUID, _IQUGCArchiveCategoryGUID) !== -1) 
                    {
                        liStyle ="style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    categoriesHTML = categoriesHTML + '<li role=\"presentation\"><a '+liStyle+' href=\"javascript:void(0)\" onclick="SetCategory_IQUGCArchive(this,\'' + eventData.CategoryGUID + '\',\'' + eventData.CategoryName.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + '\');" tabindex=\"-1\" role=\"menuitem\">';
                    categoriesHTML += EscapeHTML(eventData.CategoryName) + ' (' + eventData.RecordCountFormatted + ') </a></li>';
                });
                if (categoriesHTML == '') {
                    $('#ulIQUGCArchiveCategoryList').html('<li role="presentation"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
                }
                else {
                    $("#ulIQUGCArchiveCategoryList").html(categoriesHTML);
                }
            }      
        }
        });

    $("#dpIQUGCArchiveFrom").datepicker({
        beforeShowDay: enableAllTheseDays_IQArchiveUGC,
        showOn: "button",
        buttonImage: "../images/calender.png",
        buttonImageOnly: true,
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            $('#dpIQUGCArchiveFrom').val(dateText);
            SetDateVariable_IQUGCArchive();
        },
        onClose: function (dateText) {
        }
    });

    $("#dpIQUGCArchiveTo").datepicker({
        showOn: "button",
        buttonImage: "../images/calender.png",
        buttonImageOnly: true,
        changeMonth: true,
        changeYear: true,
        beforeShowDay: enableAllTheseDays_IQArchiveUGC,
        onSelect: function (dateText, inst) {
            $('#dpIQUGCArchiveTo').val(dateText);
            SetDateVariable_IQUGCArchive();
        },
        onClose: function (dateText) {
        }
    });

    var progressbar = $("#divProgressbar"),
    progressLabel = $("#divProgressbar .progress-label");
    progressbar.progressbar({
        value: false,
        change: function () {
            progressLabel.text(progressbar.progressbar("value") + "%");
        },
        complete: function () {
            progressLabel.text("Complete!");
        }
    });
    $("#btnUGCBrowseFile").click(function () { $("#fucUGCFile").trigger('click'); });
    $("#fucUGCFile").change(function () {

        ClearFTPFileSelection();
        var selectedfileType = $("#fucUGCFile").val().substring($("#fucUGCFile").val().lastIndexOf('.') + 1).toLowerCase();

        var mediatype = ugcFileTypes[selectedfileType];

        if (mediatype != undefined) {
            
            if(mediatype == ugcMediaType)
            {
                $("#chkAutoClip").closest('.control-group').show();
            }
            else
            {
                $("#chkAutoClip").closest('.control-group').hide();
            }

            $("#txtUGCSelectedFileDisplay").val($(this).val());

            if ($("#txtUGCTitle").val() == "" || $("#txtUGCTitle").val() == _PreviousSelectedFileName) {
                $("#txtUGCTitle").val($(this).val().substring(0, $(this).val().lastIndexOf('.')));
                _PreviousSelectedFileName = $(this).val().substring(0, $(this).val().lastIndexOf('.'));
            }
            $("#spanUGCFile").html("").hide();
        }
        
        else {
            $("#txtUGCSelectedFileDisplay").val("");
            $("#txtUGCTitle").val("");
            $("#spanUGCFile").html(_msgSelectValidUGCFileType).show();
        }
    });


//    $("#divUGCContentFormScrollContent").mCustomScrollbar({
//        advanced: {
//            updateOnContentResize: true,
//            autoScrollOnFocus: false
//        }
//    });

    if (screen.height >= 768) {
        $("#divIQUGCArchiveResultsScrollContent").css("height", documentHeight - 240);
        $("#divIQUGCArchiveResultsScrollContent").mCustomScrollbar({
            advanced: {
                updateOnContentResize: true,
                autoScrollOnFocus: false
            },
            scrollInertia: 60
        });
    }

    $("#chkIQUGCArchiveAll").click(function () {
        var c = this.checked;
        $("#divIQUGCArchiveResultsHTML input[type=checkbox]").each(function () {
            $(this).prop("checked", c);
        })
    });

    // if all checkbox are selected, check the selectall checkbox and viceversa

    $(document).on("click", "#divIQUGCArchiveResultsHTML input[type=checkbox]", function (event) {
        if ($("#divIQUGCArchiveResultsHTML input[type=checkbox]").length == $("#divIQUGCArchiveResultsHTML input[type=checkbox]:checked").length) {
            $("#chkIQUGCArchiveAll").prop("checked", true);
        } else {
            $("#chkIQUGCArchiveAll").prop("checked", false);
        }
    });

});

function enableAllTheseDays_IQArchiveUGC(date) {
    date = $.datepicker.formatDate("mm/dd/yy", date);
    return [$.inArray(date, disabledDays_IQArchiveUGC) !== -1];
}

function SetKeyword_IQUGCArchive() {
    if ($("#txtIQUGCArchiveKeyword").val() != "" && _IQUGCArchiveSearchTerm != $("#txtIQUGCArchiveKeyword").val()) {
        // The full-text search used in SQL to search on the keyword will break if there are any " in the text
        var keyword = $("#txtIQUGCArchiveKeyword").val().replace(/"/g, '');
        if (keyword != "") {
            _IQUGCArchiveSearchTerm = keyword;
            LoadIQUGCArchiveResults(false);
        }
    }
}

function SetDateVariable_IQUGCArchive() {

    if ($("#dpIQUGCArchiveFrom").val() && $("#dpIQUGCArchiveTo").val()) {
        if (_IQUGCArchiveFromDate != $("#dpIQUGCArchiveFrom").val() || _IQUGCArchiveToDate != $("#dpIQUGCArchiveTo").val()) {
            _IQUGCArchiveFromDate = $("#dpIQUGCArchiveFrom").val();
            _IQUGCArchiveToDate = $("#dpIQUGCArchiveTo").val();
            LoadIQUGCArchiveResults(false);
            $('#ulIQUGCArchiveCalender').parent().removeClass('open');
            $('#aDurationIQUGCArchive').html('Custom&nbsp;&nbsp;<span class="caret"></span>');
        }
    }
    else
        if ($("#dpIQUGCArchiveFrom").val() != "" && $("#dpIQUGCArchiveTo").val() == "") {
            $("#dpIQUGCArchiveTo").addClass("warningInput");
        }
        else if ($("#dpIQUGCArchiveTo").val() != "" && $("#dpIQUGCArchiveFrom").val() == "") {
            $("#dpIQUGCArchiveFrom").addClass("warningInput");
        }
}

function SetCustomer_IQUGCArchive(p_Customer, p_CustomerName) {
    if (_IQUGCArchiveCustomer != p_Customer) {
        _IQUGCArchiveCustomer = p_Customer;
        _IQUGCArchiveCustomerName = p_CustomerName;
        LoadIQUGCArchiveResults(false);
    }
}

function SetMediaType_IQUGCArchive(p_MediaType,p_MediaTypeDesc)
{
    if (_IQUGCArchiveMediaType != p_MediaType) {
        _IQUGCArchiveMediaType = p_MediaType;
        _IQUGCArchiveMediaTypeDesc = p_MediaTypeDesc;
        LoadIQUGCArchiveResults(false);
    }
}

function SetCategory_IQUGCArchive(eleCategory,p_CategoryGUID, p_CategoryName) {
    if ($.inArray(p_CategoryGUID, _IQUGCArchiveCategoryGUID) == -1) {
        _IQUGCArchiveCategoryGUID.push(p_CategoryGUID);
        _IQUGCArchiveCategoryNameList.push(p_CategoryName);
        $(eleCategory).css("background-color", "#E9E9E9");
    }
    else {
        var catIndex = _IQUGCArchiveCategoryGUID.indexOf(p_CategoryGUID);
        if (catIndex > -1) {
            _IQUGCArchiveCategoryGUID.splice(catIndex, 1);
            _IQUGCArchiveCategoryNameList.splice(catIndex, 1);
            $(eleCategory).css("background-color", "#ffffff");
        }
    }

    if ($('#rdArchiveUGCAnd').is(":checked")) {
        $("#ulIQUGCArchiveCategoryList li").each(function () {
            var _Category = $(this).find('a').attr("onclick").replace('SetCategory_IQUGCArchive(this,', '').replace(');', '').split(',')[0].replace(/'/g, '');
            if ($.inArray(_Category, _IQUGCArchiveCategoryGUID) == -1) {
                $(this).find('a').addClass('blur');
            }
        });

        GetIQArchiveUGCCategoryFilter(false);
    }
}

function SearchArchiveUGCCategory() {
    var _CurrentArchiveUGCSelectionType;
    var IsPost = false;
    if ($('#rdArchiveUGCAnd').is(":checked")) {
        _CurrentArchiveUGCSelectionType = $('#rdArchiveUGCAnd').val();
    }
    else {
        _CurrentArchiveUGCSelectionType = $('#rdArchiveUGCOr').val();
    }
    if ($(_IQUGCArchiveCategoryGUID).not(_IQUGCArchiveOldCategoryGUID).length != 0 || $(_IQUGCArchiveOldCategoryGUID).not(_IQUGCArchiveCategoryGUID).length != 0) {
        IsPost = true;
    }
    else if (_IQUGCArchiveCategoryGUID.length > 1 && (_IQUGCArchiveSelectionType != '' && _IQUGCArchiveSelectionType != _CurrentArchiveUGCSelectionType)) {
        IsPost = true;
    }

    if (IsPost) {
        _IQUGCArchiveOldCategoryGUID = _IQUGCArchiveCategoryGUID.slice(0);
        _IQUGCArchiveSelectionType = _CurrentArchiveUGCSelectionType;
        _IQUGCArchiveCategoryName = "";
        $.each(_IQUGCArchiveCategoryNameList, function (index, val) {
            if (_IQUGCArchiveCategoryName == "") {
                _IQUGCArchiveCategoryName = val;
            }
            else {
                _IQUGCArchiveCategoryName = _IQUGCArchiveCategoryName + ' "' + _IQUGCArchiveSelectionType + '" ' + val;
            }
        });
        LoadIQUGCArchiveResults(false);
    }
}

function SetArchiveUGCSelectionType(IsClear) {
    if (_IQUGCArchiveCategoryGUID.length > 0) {
        _IQUGCArchiveCategoryGUID = [];
        _IQUGCArchiveCategoryNameList = [];
        GetIQArchiveUGCCategoryFilter();
    }
}

function GetIQArchiveUGCCategoryFilter() {
    var jsonPostData = {
        p_FromDate: _IQUGCArchiveFromDate,
        p_ToDate: _IQUGCArchiveToDate,
        p_SearchTerm: _IQUGCArchiveSearchTerm,
        p_CustomerGuid: _IQUGCArchiveCustomer,
        p_CategoryGUID: _IQUGCArchiveCategoryGUID,
        p_MediaType : _IQUGCArchiveMediaType
    }

    $.ajax({
        url: _urlLibraryFilterIQUGCArchiveCategory,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: 'json',
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess) {
                var categoriesHTML = "";
                $.each(result.categoryFilter, function (eventID, eventData) {
                    var liStyle = "";
                    if ($.inArray(eventData.CategoryGUID, _IQUGCArchiveCategoryGUID) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    categoriesHTML = categoriesHTML + '<li role=\"presentation\"><a ' + liStyle + ' href=\"javascript:void(0)\" onclick="SetCategory_IQUGCArchive(this,\'' + eventData.CategoryGUID + '\',\'' + eventData.CategoryName.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + '\');" tabindex=\"-1\" role=\"menuitem\">';
                    categoriesHTML += EscapeHTML(eventData.CategoryName) + ' (' + eventData.RecordCountFormatted + ') </a></li>';
                });

                if (categoriesHTML == '') {
                    $('#ulIQUGCArchiveCategoryList').html('<li role="presentation"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
                    $('#liIQUGCArchiveCategorySearch').hide();
                }
                else {
                    $('#ulIQUGCArchiveCategoryList').html(categoriesHTML);
                    $('#liIQUGCArchiveCategorySearch').show();
                }
            }
            else {
                ShowNotification(_msgSomeErrorProcessing);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgSomeErrorProcessing, a);
        }
    });
}



function BindCustomCategoryDropDown(isUpdateAll) {
    _IsCategoryRequestOnProgress = true;
    $("#divUGCContentFormContent").addClass("blurOnlyControls");
    $("#divCloudUGCLoadMsg").html("Please Wait...");
    
    $.ajax({

        type: "post",
        dataType: "json",
        url: _urlLibraryBindCustomCategoryDropDown,
        contentType: "application/json; charset=utf-8",
        global: false,
        success: function (result) {
            if (result.isSuccess && result.customCategories != null) {
                var categoryOptions = '<option value="">&lt;Blank&gt;</option>';

                $.each(result.customCategories, function (eventID, eventData) {
                    categoryOptions = categoryOptions + '<option value="' + eventData.CategoryGUID + '">' + EscapeHTML(eventData.CategoryName) + '</option>';
                });

                $("#ddlUGCCategory").html(categoryOptions);
                $("#ddlUGCSubCategory1").html(categoryOptions);
                $("#ddlUGCSubCategory2").html(categoryOptions);
                $("#ddlUGCSubCategory3").html(categoryOptions);


                if(typeof(isUpdateAll) != 'undefined' && isUpdateAll == true){
                    $("#ddlUGCCategoryEdit").html(categoryOptions);
                    $("#ddlUGCSubCategory1Edit").html(categoryOptions);
                    $("#ddlUGCSubCategory2Edit").html(categoryOptions);
                    $("#ddlUGCSubCategory3Edit").html(categoryOptions);

                    $("#ddlCategory").html(categoryOptions);
                    $("#ddlSubCategory1").html(categoryOptions);
                    $("#ddlSubCategory2").html(categoryOptions);
                    $("#ddlSubCategory3").html(categoryOptions);
                }

                _IsCategoryLoaded = true;
                _IsCategoryRequestOnProgress = false;
                $("#divUGCContentFormContent").removeClass("blurOnlyControls");
                $("#divCloudUGCLoadMsg").html("");
            }
            else
            {
                _IsCategoryLoaded = false;
                _IsCategoryRequestOnProgress = false;
            }
        },
        error: function (a, b, c) {
            _IsCategoryLoaded = false;
            _IsCategoryRequestOnProgress = false;
        }
    });
}

function OpenUGCContentModalPopup() {

    if(!_IsCategoryLoaded && !_IsCategoryRequestOnProgress)
    {
        BindCustomCategoryDropDown();
    }

    $("#divCloudUGCUploadPopup").modal({
        backdrop: 'static',
        keyboard: true,
        dynamic : true
    });

    $("#divUGCContentFormContent").show();
    $("#divUGCProgressbar").hide();

    $("#divCloudUGCUploadPopup span").html("").hide();
    $("#divCloudUGCUploadPopup input[type=text]").val("");
    $("#divCloudUGCUploadPopup select").val("0");
    $("#txtUGCDescription").val("");
    $("#divUGCCommonErrorMsg").html("").hide();
    $("#txtDateUploaded").val(getCurrentDateTimeAMPM());
    $("#dpUGCAirDateTime").datepicker("setDate", new Date());
    $("#ddlUGCtimeZone").val(DefaultTimeZone);
    $("#ddlUGCCategory option").filter(function () { return $(this).html() == "Default" }).prop("selected", "selected");
    
    $("#chkAutoClip").closest('.control-group').show();

    if (!IsBrowserIE()) {
        $("#divCloudUGCUploadPopup input[type=file]").val("");
    }
    else {
        $("#fucUGCFile").replaceWith($("#fucUGCFile").clone(true));
    }
    $("#chkAutoClip").prop("checked", false);
    $("#divProgressbar").hide();
    $("#divIEProgressbar").hide();
    ClearUGCFileSelection();


    if (IsBrowserIE()) {
        $("#txtUGCSelectedFileDisplay").hide();
        $("#fucUGCFile").show();
    }
}

function CancelCloudUGCUploadPopup() {
    if (IsUploadActive) {
        if (IsBrowserIE()) {
            $("#txtUGCSelectedFileDisplay").hide();
            $("#fucUGCFile").show();
            xhrIE.abort();
        }
        else {
            xhr.abort();
        }
    }
    $("#divCloudUGCUploadPopup").css({ "display": "none" });
    $("#divCloudUGCUploadPopup").modal("hide");
    $("#divProgressbar").hide();
    $("#divIEProgressbar").hide();
    
}

function getCurrentDateTimeAMPM() {

    var date = new Date();
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var seconds = date.getSeconds();
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strDateTime = (date.getMonth() + 1) + "/" + date.getDate() + "/" + date.getFullYear() + " ";
    strDateTime += hours + ':' + minutes + ':' + seconds + ' ' + ampm;
    return strDateTime;
}

var xhr;
var xhrIE;
function SaveCloudUGCContent() {
    if (ValidateUGCContent()) {

        $("#divUGCUploadingFileName").html("");
        if (!IsBrowserIE()) {
            IsUploadActive = true;
            var file = document.getElementById("fucUGCFile").files[0];
            var fileName = "";

            if ((file == null || file == undefined) && $("#txtUGCSelectedFileDisplay").val() == "") {
                $("#divUGCCommonErrorMsg").html(_msgNoFileToUpload).show();
                return;
            }

            $("#divUGCContentFormContent").hide();
            $("#divUGCProgressbar").show();
            $("#divUGCUploadingFileName").html($("#txtUGCSelectedFileDisplay").val());

            xhr = new XMLHttpRequest();


            var formdata = new FormData();

            if (file != null && file != undefined) {
                fileName = file.name;
                formdata.append(file.name, file);
                $("#divProgressbar").show();
            }
            else {
                // If not file upload then show normal progress bar
                $("#divIEProgressbar").show();
                $("#divIEProgressbar").html('<img src="../../Images/Loading_1.gif" alt="Please Wait"/> <span>' + _msgUploading + '</span>&nbsp;&nbsp;<a class="padding5" onclick="CancelUGCUpload()" href="javascript:;" style="z-index: 2000; text-decoration: none;">Cancel Upload</a>');
                // for IE
            }
            formdata.append("chkAutoClip", $("#chkAutoClip").is(":checked") ? true : false);
            formdata.append("txtUGCSelectedFileDisplay", (fileName != "" ? "" : $("#txtUGCSelectedFileDisplay").val()));
            formdata.append("txtUGCTitle", $("#txtUGCTitle").val());
            formdata.append("txtUGCKeywords", $("#txtUGCKeywords").val());
            formdata.append("ddlUGCCategory", $("#ddlUGCCategory").val());
            formdata.append("ddlUGCSubCategory1", $("#ddlUGCSubCategory1").val());
            formdata.append("ddlUGCSubCategory2", $("#ddlUGCSubCategory2").val());
            formdata.append("ddlUGCSubCategory3", $("#ddlUGCSubCategory3").val());
            formdata.append("dpUGCAirDateTime", $("#dpUGCAirDateTime").val());
            formdata.append("ddlUGCtimeZone", $("#ddlUGCtimeZone").val());
            formdata.append("txtUGCDescription", $("#txtUGCDescription").val());

            xhr.addEventListener("progress", function (e) {
                var done = e.position || e.loaded, total = e.totalSize || e.total;
            }, false);

            if (xhr.upload) {
                xhr.upload.onprogress = function (e) {
                    var done = e.position || e.loaded, total = e.totalSize || e.total;
                    $("#divProgressbar").progressbar("value", (Math.floor(done / total * 1000) / 10));
                };
            }

            xhr.addEventListener("error", function (evt) {
                IsUploadActive = false;
                CancelCloudUGCUploadPopup();
                ShowNotification(_msgErrorWhileSavingUGCContent);
            }, false);

            xhr.addEventListener("load", function (e) {
            }, false);

            xhr.onreadystatechange = function (e) {
                if (4 == this.readyState) {
                    IsUploadActive = false;
                    var res = { success: false, error: "" };

                    // chrome not allow directly to access json object, so we need to parse JSON and use afterwards. 
                    // Firefox allows json object so no need to do same.

                    if (this.response.isSuccess == undefined) {
                        var jsonObj = $.parseJSON(this.response);
                        res.success = jsonObj.isSuccess;
                        res.error = jsonObj.errorMsg;
                    }
                    else {
                        res.success = this.response.isSuccess;
                        res.error = this.response.errorMsg;
                    }

                    $("#divProgressbar").hide();
                    CancelCloudUGCUploadPopup();
                    if (res.success == true) {
                        $("#divIEProgressbar").hide();
                        ShowNotification(_msgUGCUploadedSuccessfully);
                    }
                    else {
                        ShowNotification(res.error);
                    }
                }
            };

            xhr.open("post", "/Library/UGCUploadContent/", true);
            xhr.responseType = "json";
            xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
            xhr.send(formdata);
        }
        else {
            IsUploadActive = true;
            $("#divUGCContentFormContent").hide();
            $("#divUGCProgressbar").show();
            $("#divUGCUploadingFileName").html($("#txtUGCSelectedFileDisplay").val());

            $("#divIEProgressbar").show();
            $("#divIEProgressbar").html('<img src="../../Images/Loading_1.gif" alt="Please Wait"/> <span>Uploading...</span>&nbsp;&nbsp;<a class="padding5" onclick="CancelUGCUpload()" href="javascript:;" style="z-index: 2000; text-decoration: none;">Cancel Upload</a>');

            if ($("#fucUGCFile").val() == "") {
                $("#hdnUGCSelectedFileDisplay").val($("#txtUGCSelectedFileDisplay").val());
            }
            else {
                $("#hdnUGCSelectedFileDisplay").val("");
            }

            $("#ugcContentForm").ajaxSubmit({
                target: "",
                beforeSend: function (jqXHR, settings) {
                    xhrIE = jqXHR;
                },
                success: function (res) {
                    IsUploadActive = false;
                    $("#divIEProgressbar").hide();
                    CancelCloudUGCUploadPopup();
                    var obj = $.parseJSON(res);
                    if (obj.isSuccess) {
                        ShowNotification(_msgUGCUploadedSuccessfully);
                    }
                    else {
                        ShowNotification(obj.errorMsg);
                    }
                }
            });
        }
    }
}

function IsBrowserIE() {
    return navigator.userAgent.toLowerCase().indexOf("msie") > -1;
}

function ValidateUGCContent() {
    var flag = true;

    $("#divCloudUGCUploadPopup span").html("").hide();

    var fileCount = $("#divCloudUGCUploadPopup input[type=file]").filter(function () {
        return this.value
    }).length;

    if (fileCount == 0 && $("#txtUGCSelectedFileDisplay").val() == "") {
        $("#spanUGCFile").html("Please select file or FTP file").show();
        flag = false;
    }
    if ($.trim($("#txtUGCTitle").val()) == "") {
        $("#spanUGCTitle").show().html(_msgTitleRequired);
        flag = false;
    }
    if ($("#ddlUGCCategory").val() == "" || $("#ddlUGCCategory").val() == null) {
        $("#spanUGCCategory").show().html(_msgCategoryRequired);
        flag = false;
    }
    if ($("#ddlUGCtimeZone").val() == "") {
        $("#spanUGCTimeZone").show().html(_msgTimeZoneRequired);
        flag = false;
    }
    if ($("#dpUGCAirDateTime").val() == "") {
        $("#spanUGCAirDateTime").show().html(_msgAirDateRequired);
        flag = false;
    }
    return flag;
}

function OnSelectOfCategory_UGCcontent(ddl_id, ddlCatID, ddlSubCat1ID, ddlSubCat2ID, ddlSubCat3ID, divError) {

    var PCatId = ddlCatID;
    var SubCat1Id = ddlSubCat1ID;
    var SubCat2Id = ddlSubCat2ID;
    var SubCat3Id = ddlSubCat3ID;

    $("#" + divError).html("").hide();

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

            $("#" + divError).html(_msgFirstSelectPrecedingCat).show();
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

            $("#" + divError).html(_msgFirstSelectPrecedingCat).show();

        }
    }
    else if (ddl_id == SubCat3Id) {
        if (PCatSelectedValue == "" || Cat1SelectedValue == "" || Cat2SelectedValue == "") {
            $("#" + SubCat3Id + "").val("");

            $("#" + divError).html(_msgFirstSelectPrecedingCat).show();
        }
    }
}

function OpenCloudUGCFTPUploadPopup() {

    var jsonPostData = { p_FolderName: "" }
    $("#txtFTPSelectedFile").val("");
    $.ajax({

        type: "post",
        dataType: "json",
        url: _urlLibraryGetUGCUploadFTPContent,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess) {
                $("#divUGCFTPExplorer").html(result.HTML);
                $("#txtFTPSelectedFile").val("");
                $("#divCloudUGCFTPUploadPopup").modal({
                    backdrop: 'static',
                    keyboard: true,
                    dynamic: true
                });
            }
            else {
                ShowNotification(_msgErrorWhileFetchingData);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgSomeErrorProcessing);
        }
    });
}

function GetFolderContentByPath(path) {

    var jsonPostData = { p_FolderName: path }

    $.ajax({

        type: "post",
        dataType: "json",
        url: _urlLibraryGetUGCUploadFTPContent,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess) {
                $("#divUGCFTPExplorer").html(result.HTML);
                $("#txtFTPSelectedFile").val("");
            }
            else {
                ShowNotification(_msgErrorWhileFetchingData);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgSomeErrorProcessing);
        }
    });
}

function CancelCloudUGCFTPUploadPopup() {
    $("#divCloudUGCFTPUploadPopup").css({ "display": "none" });
    $("#divCloudUGCFTPUploadPopup").modal("hide");
}

function SetFTPSelectedFile(filename) {
    $("#txtFTPSelectedFile").val(filename);
}

function SelectFTPSelectedFile() {
    if ($("#txtFTPSelectedFile").val() != "") {

        var selectedfileType = $("#txtFTPSelectedFile").val().substring($("#txtFTPSelectedFile").val().lastIndexOf('.') + 1).toLowerCase();

        var mediatype = ugcFileTypes[selectedfileType];

        if (mediatype != undefined) {
            
            if(mediatype == ugcMediaType)
            {
                $("#chkAutoClip").closest('.control-group').show();
            }
            else
            {
                $("#chkAutoClip").closest('.control-group').hide();
            }

            $("#spanUGCFile").html("").hide();
            $("#txtUGCSelectedFileDisplay").val($("#txtFTPSelectedFile").val());

            if ($("#txtUGCTitle").val() == "" || $("#txtUGCTitle").val() == _PreviousSelectedFileName) {
                $("#txtUGCTitle").val($("#txtFTPSelectedFile").val().substring(0, $("#txtFTPSelectedFile").val().lastIndexOf('.')));
                _PreviousSelectedFileName = $("#txtFTPSelectedFile").val().substring(0, $("#txtFTPSelectedFile").val().lastIndexOf('.'));
            }
            
            $("#txtFTPSelectedFile").val("");
            ClearNormalFileSelection();
        }
        else {
            $("#spanUGCFile").html(_msgSelectValidUGCFileType).show();
        }
        $("#divCloudUGCFTPUploadPopup").css({ "display": "none" });
        $("#divCloudUGCFTPUploadPopup").modal("hide");
    }
}
function ClearNormalFileSelection() {
    if (!IsBrowserIE()) {
        $("#divCloudUGCUploadPopup input[type=file]").val("");
    }
    else {
        $("#fucUGCFile").replaceWith($("#fucUGCFile").clone(true));
    }
    $("#txtFTPSelectedFile").val("");
    $("#btnUGCBrowseFile").hide();
    $("#btnUGCBrowseFTP").show();
    $("#fucUGCFile").hide();
    $("#txtUGCSelectedFileDisplay").show();
}
function ClearFTPFileSelection() {
    $("#txtFTPSelectedFile").val("");
    $("#btnUGCBrowseFTP").hide();

    if (!IsBrowserIE()) {
        $("#btnUGCBrowseFile").show();
        $("#txtUGCSelectedFileDisplay").show();
    }
    else {
        $("#fucUGCFile").show();
        $("#txtUGCSelectedFileDisplay").hide();
    }
}
function ClearUGCFileSelection() {

    if (!IsBrowserIE()) {
        $("#divCloudUGCUploadPopup input[type=file]").val("");
        $("#btnUGCBrowseFile").show();
        $("#txtUGCSelectedFileDisplay").show();
        $("#fucUGCFile").hide();
    }
    else {
        $("#fucUGCFile").replaceWith($("#fucUGCFile").clone(true));
        $("#txtUGCSelectedFileDisplay").hide();
        $("#fucUGCFile").show();
        $("#btnUGCBrowseFile").hide();
    }
    $("#btnUGCBrowseFTP").show();
    $("#txtUGCSelectedFileDisplay").val("");
    $("#hdnUGCSelectedFileDisplay").val("");
    $("#spanUGCFile").html("").hide();
    $("#chkAutoClip").closest('.control-group').show();

    if ($("#txtUGCTitle").val() == "" || $("#txtUGCTitle").val() == _PreviousSelectedFileName) {
        $("#txtUGCTitle").val("");
        _PreviousSelectedFileName = "";
    }
}

function LoadIQUGCArchiveResults(isLoadMoreResults) {

    if (!isLoadMoreResults && !DateValidation_IQUGCArchive()) {
        return;
    }

    var jsonPostData = {
        p_IsLoadMoreResults: isLoadMoreResults,
        p_FromDate: _IQUGCArchiveFromDate,
        p_ToDate: _IQUGCArchiveToDate,
        p_SearchTerm: _IQUGCArchiveSearchTerm == "" ? _IQUGCArchiveSearchTerm : '"' + _IQUGCArchiveSearchTerm + '"',
        p_CustomerGuid: _IQUGCArchiveCustomer,
        p_CategoryGuid: _IQUGCArchiveCategoryGUID,
        p_SelectionType : _IQUGCArchiveSelectionType,
        p_MediaType : _IQUGCArchiveMediaType,
        p_Sortcolumn: _IQUGCArchiveSortColumn,
        p_IsAsc: _IQUGCArchiveIsAsc,
        p_PageSize : _IQUGCArchivePageSize
    }

    if (!isLoadMoreResults) {
        SetActiveFilterIQUGCArchive();
    }
    else {
        $("#btnUGCShowMoreResults").attr("disabled", "disabled");
        $("#btnUGCShowMoreResults").attr("class", "disablebtn");
        ShowHideProgressNearButtonIQUGCArchive(true);
    }

    $.ajax({

        type: "post",
        dataType: "json",
        url: _urlLibraryDisplayIQUGCArchiveResults,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess) {
                if (!isLoadMoreResults) {
                    $("#divIQUGCArchiveResultsHTML").html(result.HTML);
                    ModifyFiltersIQArchiveUGC(result.filter);
                    _IQUGCArchiveCurrentCategoryFilter = result.filter.Categories.slice(0);
                }
                else {
                    $("#divIQUGCArchiveResultsHTML").append(result.HTML);
                    $("#divRecordCountIQUGCArchive").show();
                    $("#divUGCShowResult").show();
                    $("#btnUGCShowMoreResults").removeAttr("disabled");
                    $("#btnUGCShowMoreResults").attr("class", "loadmore");
                    ShowHideProgressNearButtonIQUGCArchive(false);
                }
                ShowHideMoreResultsIQUGCArchive(result);
                ShowNoofRecordsIQUGCArchive(result);
                if (!isLoadMoreResults) {
                    if (screen.height >= 768) {
                        setTimeout(function () { $("#divIQUGCArchiveResultsScrollContent").mCustomScrollbar("scrollTo", "top"); }, 200);
                    }
                }
                else {
                    if ($("#chkIQUGCArchiveAll").is(":checked")) {
                        $("#divIQUGCArchiveResultsHTML input[type=checkbox]").each(function () {
                            $(this).prop("checked", true);
                        })
                    }
                }
            }
            else {
                if(!isLoadMoreResults)
                {
                    ClearResultsOnError('divIQUGCArchiveResultsHTML','divRecordCountIQUGCArchive','divUGCShowResult', _msgErrorOnSearch.replace(/@@MethodName@@/g, "LoadIQUGCArchiveResults("+isLoadMoreResults+")"));
                }

                ShowNotification(_msgErrorWhileFetchingData);
            }
        },
        error: function (a, b, c) {
            if(!isLoadMoreResults)
            {
                    ClearResultsOnError('divIQUGCArchiveResultsHTML','divRecordCountIQUGCArchive','divUGCShowResult', _msgErrorOnSearch.replace(/@@MethodName@@/g, "LoadIQUGCArchiveResults("+isLoadMoreResults+")"));
            }
            ShowNotification(_msgSomeErrorProcessing);
            $("#btnUGCShowMoreResults").removeAttr("disabled");
            $("#btnUGCShowMoreResults").attr("class", "loadmore");
        }
    });
}


function RefreshIQUGCArchiveResults() {

    _IQUGCArchiveFromDate = null;
    _IQUGCArchiveToDate = null;
    _IQUGCArchiveSearchTerm = "";
    _IQUGCArchiveCustomer = "";
    _IQUGCArchiveCustomerName = "";
    _IQUGCArchiveCategoryName = "";
    _IQUGCArchiveCategoryGUID = "";
    _IQUGCArchiveSortColumn = "AirDate";
    _IQUGCArchiveDuration = null;
    _IQUGCArchiveIsAsc = false;
    _IQUGCArchiveMediaType ="";
    _IQUGCArchiveMediaTypeDesc = "";
    _IQUGCArchiveCurrentCategoryFilter = new Array();

    SetActiveFilterIQUGCArchive();

    $.ajax({

        type: "post",
        dataType: "json",
        url: _urlLibraryRefreshIQUGCArchiveResults,
        contentType: "application/json; charset=utf-8",
        data: {},
        success: function (result) {
            if (result.isSuccess) {
                $("#divIQUGCArchiveResultsHTML").html(result.HTML);
                ModifyFiltersIQArchiveUGC(result.filter);
                _IQUGCArchiveCurrentCategoryFilter = result.filter.Categories.slice(0);
                ShowHideMoreResultsIQUGCArchive(result);
                ShowNoofRecordsIQUGCArchive(result);
            }
            else {
                ShowNotification(_msgErrorWhileFetchingData);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgSomeErrorProcessing);
        }
    });
}

function ShowHideProgressNearButtonIQUGCArchive(value) {
    if (value == true) {
        $("#imgUGCMoreResultLoading").removeClass("visibilityHidden");
    }
    else {
        $("#imgUGCMoreResultLoading").addClass("visibilityHidden");
    }
}

function ShowHideMoreResultsIQUGCArchive(result) {
    if (result != null) {

        $("#btnUGCShowMoreResults").show();

        if (result.hasMoreResults == true) {
            $("#btnUGCShowMoreResults").attr("value", _msgShowMoreResults);
            $("#btnUGCShowMoreResults").attr("onclick", "LoadIQUGCArchiveResults(true);");
        }
        else {
            $("#btnUGCShowMoreResults").attr("value", _msgNoMoreResult);
            $("#btnUGCShowMoreResults").removeAttr("onclick");
        }
    }
}

function ShowNoofRecordsIQUGCArchive(result) {
    if (result != null && result.totalRecords != null && result.totalRecords > 0 && result.currentRecords != null) {

        $("#divRecordCountIQUGCArchive").show();
        $("#divUGCShowResult").show();

        $("#spanIQUGCArchiveCurrentRecords").html(numberWithCommas(result.currentRecords));
        $("#spanIQUGCArchiveTotalRecords").html(numberWithCommas(result.totalRecords));
        $("#aUGCPageSize").show();
    }
    else {
        $("#divRecordCountIQUGCArchive").hide();
        $("#divUGCShowResult").hide();
        $("#aUGCPageSize").hide();
    }
}

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function ModifyFiltersIQArchiveUGC(filter) {

    disabledDays_IQArchiveUGC = [];
    if (filter != null && filter.CreateDTList.length > 0) {
        $.each(filter.CreateDTList, function (id, date) {
            disabledDays_IQArchiveUGC.push(date);
        });
    }

    if (filter != null && filter.Categories != null) {

        var categoriesHTML = "";
        $.each(filter.Categories, function (eventID, eventData) {
            var liStyle = "";
            if ($.inArray(eventData.CategoryGUID, _IQUGCArchiveCategoryGUID) !== -1) {
                liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
            }
            categoriesHTML = categoriesHTML + '<li role=\"presentation\"><a ' + liStyle + ' href=\"javascript:void(0)\" onclick="SetCategory_IQUGCArchive(this,\'' + eventData.CategoryGUID + '\',\'' + eventData.CategoryName.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + '\');" tabindex=\"-1\" role=\"menuitem\">';
            categoriesHTML += EscapeHTML(eventData.CategoryName) + ' (' + eventData.RecordCountFormatted + ') </a></li>';
        });

        if (categoriesHTML == '') {
            $('#ulIQUGCArchiveCategoryList').html('<li role="presentation"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
            $('#liIQUGCArchiveCategorySearch').hide();
        }
        else {
            $('#ulIQUGCArchiveCategoryList').html(categoriesHTML);
            $('#liIQUGCArchiveCategorySearch').show();
        }
    }

    if (filter != null && filter.Customers != null) {

        var customerHTML = "";
        $.each(filter.Customers, function (eventID, eventData) {
            customerHTML = customerHTML + '<li onclick="SetCustomer_IQUGCArchive(\'' + eventData.CustomerKey + '\',\'' + eventData.CustomerName.replace(/"/g, '&quot;').replace(/"/g, '&quot;').replace(/'/g, '\\\'') + '\');" role=\"presentation\"><a href=\"#\" tabindex=\"-1\" role=\"menuitem\">';
            customerHTML += eventData.CustomerName + ' (' + eventData.RecordCountFormatted + ') </a></li>';
        });

        if (customerHTML == "") {
            $('#ulIQUGCArchiveCustomers').html('<li role="presentation"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
        }
        else {
            $('#ulIQUGCArchiveCustomers').html(customerHTML);
        }
    }

    if (filter != null && filter.MediaTypes != null) {

        var MediaTypesHTML = "";
        $.each(filter.MediaTypes, function (eventID, eventData) {
            MediaTypesHTML = MediaTypesHTML + '<li onclick="SetMediaType_IQUGCArchive(\'' + eventData.MediaType + '\',\'' + eventData.MediaTypeDesc.replace(/"/g, '&quot;').replace(/"/g, '&quot;').replace(/'/g, '\\\'') + '\');" role=\"presentation\"><a href=\"#\" tabindex=\"-1\" role=\"menuitem\">';
            MediaTypesHTML += eventData.MediaTypeDesc + ' (' + eventData.RecordCountFormatted + ') </a></li>';
        });

        if (MediaTypesHTML == "") {
            $('#ulIQUGCArchiveMediaTypes').html('<li role="presentation"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
        }
        else {
            $('#ulIQUGCArchiveMediaTypes').html(MediaTypesHTML);
        }
    }
}

function SetActiveFilterIQUGCArchive() {

    var isFilterEnable = false;
    $("#divActiveFilterIQUGCArchive").html("");

    if (_IQUGCArchiveSearchTerm != "") {
        $('#divActiveFilterIQUGCArchive').append('<div id="divKeywordActiveFilter" class="filter-in">' + EscapeHTML(_IQUGCArchiveSearchTerm) + '<span class="cancel" onclick="RemoveFilterIQUGCArchive(1);"></span></div>');
        isFilterEnable = true;
    }
    if ((_IQUGCArchiveFromDate != null && _IQUGCArchiveFromDate != "") && (_IQUGCArchiveToDate != null && _IQUGCArchiveToDate != "")) {
        $('#divActiveFilterIQUGCArchive').append('<div id="divIQUGCArchiveDateActiveFilter" class="filter-in">' + _IQUGCArchiveFromDate + ' To ' + _IQUGCArchiveToDate + '<span class="cancel" onclick="RemoveFilterIQUGCArchive(2);"></span></div>');
        isFilterEnable = true;
    }
    if (_IQUGCArchiveCategoryGUID != "") {
        $('#divActiveFilterIQUGCArchive').append('<div id="divIQUGCArchiveCategoryActiveFilter" class="filter-in">' + EscapeHTML(_IQUGCArchiveCategoryName) + '<span class="cancel" onclick="RemoveFilterIQUGCArchive(3);"></span></div>');
        isFilterEnable = true;
    }
    if (_IQUGCArchiveCustomer != "") {
        $('#divActiveFilterIQUGCArchive').append('<div id="divIQUGCArchiveCustomerActiveFilter" class="filter-in">' + _IQUGCArchiveCustomerName + '<span class="cancel" onclick="RemoveFilterIQUGCArchive(4);"></span></div>');
        isFilterEnable = true;
    }

    if (_IQUGCArchiveMediaTypeDesc != "") {
        $('#divActiveFilterIQUGCArchive').append('<div id="divIQUGCArchiveMediaTypeActiveFilter" class="filter-in">' + _IQUGCArchiveMediaTypeDesc + '<span class="cancel" onclick="RemoveFilterIQUGCArchive(5);"></span></div>');
        isFilterEnable = true;
    }

    

    if (isFilterEnable) {
        $("#divActiveFilterIQUGCArchive").css({ 'border-bottom': '1px solid rgb(236, 236, 236)' });
    }
    else {
        $("#divActiveFilterIQUGCArchive").css({ 'border-bottom': '' });
    }
}

function RemoveFilterIQUGCArchive(filterType) {

    // Represent SearchKeyword
    if (filterType == 1) {

        $("#txtIQUGCArchiveKeyword").val("");
        _IQUGCArchiveSearchTerm = "";
        LoadIQUGCArchiveResults(false);
    }

    // Represent Date filter(From Date,To Date)
    if (filterType == 2) {

        $("#dpIQUGCArchiveFrom").datepicker("setDate", null);
        $("#dpIQUGCArchiveTo").datepicker("setDate", null);
        $("#divIQUGCArchiveCalender").datepicker("setDate", null);

        $('#aDurationIQUGCArchive').html(_msgAll + '&nbsp;&nbsp;<span class="caret"></span>');

        _IQUGCArchiveFromDate = null;
        _IQUGCArchiveToDate = null;
        LoadIQUGCArchiveResults(false);
    }

    // Represent Category Filter
    if (filterType == 3) {
        _IQUGCArchiveCategoryGUID = [];
        _IQUGCArchiveCategoryNameList = [];
        _IQUGCArchiveOldCategoryGUID = [];
        _IQUGCArchiveCategoryName = "";
        _IQUGCArchiveSelectionType = $('#rdArchiveUGCOr').val();
        LoadIQUGCArchiveResults(false);
    }

    // Represent Customer Filter
    if (filterType == 4) {
          _IQUGCArchiveCustomer = "";
        _IQUGCArchiveCustomerName = "";
        LoadIQUGCArchiveResults(false);
    }

    if(filterType == 5)
    {
        _IQUGCArchiveMediaType = "";
        _IQUGCArchiveMediaTypeDesc = ""
        LoadIQUGCArchiveResults(false);
    }

}

function SortDirectionIQUGCArchiveResults(p_SortColumn, p_isAsc) {
    if (p_isAsc != _IQUGCArchiveIsAsc || _IQUGCArchiveSortColumn != p_SortColumn) {

        _IQUGCArchiveIsAsc = p_isAsc;
        _IQUGCArchiveSortColumn = p_SortColumn;

        if (_IQUGCArchiveIsAsc && _IQUGCArchiveSortColumn == "AirDate") {
            $('#aSortDirectionIQUGCArchive').html(_msgOldestFirst + ' (Air Date)&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (!_IQUGCArchiveIsAsc && _IQUGCArchiveSortColumn == "AirDate") {
            $('#aSortDirectionIQUGCArchive').html(_msgMostRecent + ' (Air Date)&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (_IQUGCArchiveIsAsc && _IQUGCArchiveSortColumn == "CreatedDate") {
            $('#aSortDirectionIQUGCArchive').html(_msgOldestFirst + ' (Created Date)&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (!_IQUGCArchiveIsAsc && _IQUGCArchiveSortColumn == "CreatedDate") {
            $('#aSortDirectionIQUGCArchive').html(_msgMostRecent + ' (Created Date)&nbsp;&nbsp;<span class="caret"></span>');
        }
        LoadIQUGCArchiveResults(false);
    }
}

function DateValidation_IQUGCArchive() {
    $('#dpIQUGCArchiveFrom').removeClass('warningInput');
    $('#dpIQUGCArchiveTo').removeClass('warningInput');

    // if both empty
    if (($('#dpIQUGCArchiveTo').val() == '') && ($('#dpIQUGCArchiveFrom').val() == '')) {
        return true;

    }
    //if one empty not other
    else if (($('#dpIQUGCArchiveFrom').val() != '') && ($('#dpIQUGCArchiveTo').val() == '')
                    ||
                    ($('#dpIQUGCArchiveFrom').val() == '') && ($('#dpIQUGCArchiveTo').val() != '')
                    ) {
        if ($('#dpIQUGCArchiveFrom').val() == '') {

            ShowNotification(_msgFromDateNotSelected);
            $('#dpIQUGCArchiveFrom').addClass('warningInput');
        }

        if ($('#dpIQUGCArchiveTo').val() == '') {

            ShowNotification(_msgToDateNotSelected);
            $('#dpIQUGCArchiveTo').addClass('warningInput');

        }
        return false;
    }
    //if both not empty
    else {
        var isFromDateValid = isValidDate($('#dpIQUGCArchiveFrom').val().toString());
        var isToDateValid = isValidDate($('#dpIQUGCArchiveTo').val().toString());
        if (isFromDateValid && isToDateValid) {
            var fromDate = new Date($('#dpIQUGCArchiveFrom').val());
            var toDate = new Date($('#dpIQUGCArchiveTo').val());
            if (fromDate > toDate) {
                ShowNotification(_msgFromDateLessThanToDate);
                $('#dpIQUGCArchiveFrom').addClass('warningInput');
                $('#dpIQUGCArchiveTo').addClass('warningInput');
                return false;
            }
            else {
                return true;

            }
        }
        else {
            if (!isFromDateValid) {
                $('#dpIQUGCArchiveFrom').addClass('warningInput');
            }
            if (!isToDateValid) {
                $('#dpIQUGCArchiveTo').addClass('warningInput');
            }
            ShowNotification('Invalid Date');
            return false;
        }
    }
}

function GetResultOnDurationIQUGCArchive(duration) {

    $("#dpIQUGCArchiveFrom").removeClass("warningInput");
    $("#dpIQUGCArchiveTo").removeClass("warningInput");
    var dtcurrent = new Date();
    var fDate;
    _IQUGCArchiveDuration = duration;

    // All
    if (duration == 0) {
        $("#dpIQUGCArchiveFrom").val("");
        $("#dpIQUGCArchiveTo").val("");
        dtcurrent = "";
        RemoveFilter(2);
        $('#aDurationIQUGCArchive').html(_msgAll + '&nbsp;&nbsp;<span class="caret"></span>');
    }

    // 24 hours
    else if (duration == 1) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth(), dtcurrent.getDate() - 1);
        $('#aDurationIQUGCArchive').html(_msgLast24Hours + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    // Last week
    else if (duration == 2) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth(), dtcurrent.getDate() - 7);
        $('#aDurationIQUGCArchive').html(_msgLastWeek + '&nbsp;&nbsp;<span class="caret"></span>');
    }

    // Last Month
    else if (duration == 3) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth() - 1, dtcurrent.getDate());
        $('#aDurationIQUGCArchive').html(_msgLastMonth + '&nbsp;&nbsp;<span class="caret"></span>');
    }

    // Last 3 Months
    else if (duration == 4) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth() - 3, dtcurrent.getDate());
        $('#aDurationIQUGCArchive').html(_msgLast3Month + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    // Custom
    else if (duration == 5) {

        dtcurrent = null;
        if ($("#dpIQUGCArchiveFrom").val() != "" && $("#dpIQUGCArchiveTo").val() != "") {
            $('#aDurationIQUGCArchive').html(_msgCustom + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        else {
            if ($("#dpIQUGCArchiveFrom").val() == "") {
                $("#dpIQUGCArchiveFrom").addClass("warningInput");
            }
            if ($("#dpIQUGCArchiveTo").val() == "") {
                $("#dpIQUGCArchiveTo").addClass("warningInput");
            }
            ShowNotification(_msgSelectDate);
        }
    }

    $("#dpIQUGCArchiveFrom").datepicker("setDate", fDate);
    $("#dpIQUGCArchiveTo").datepicker("setDate", dtcurrent);

    if ($("#dpIQUGCArchiveFrom").val() != "" && $("#dpIQUGCArchiveTo").val() != "") {

        if (_IQUGCArchiveFromDate != $("#dpIQUGCArchiveFrom").val() || _IQUGCArchiveToDate != $("#dpIQUGCArchiveTo").val()) {
            _IQUGCArchiveFromDate = $("#dpIQUGCArchiveFrom").val();
            _IQUGCArchiveToDate = $("#dpIQUGCArchiveTo").val();
            LoadIQUGCArchiveResults(false);
        }
    }
}

function DeleteIQUGCArchiveByID(id) {

    if (id != "") {
        DeleteIQUGCArchive(id);
    }
}

function DeleteIQUGCArchiveRecords() {
    var output = $.map($("#divIQUGCArchiveResultsHTML input[type=checkbox]:checked"), function (n, i) {
        return n.value;
    }).join(',');
    if (output != "") {
        DeleteIQUGCArchive(output);
    }
    else {
        ShowNotification(_msgSelectRecordToDelete);
    }
}

function DeleteIQUGCArchive(items) {

    var jsonPostData =
          {
              p_UGCArchiveIDs: items,
              p_FromDate: _IQUGCArchiveFromDate,
              p_ToDate: _IQUGCArchiveToDate,
              p_SearchTerm: _IQUGCArchiveSearchTerm,
              p_CustomerGuid: _IQUGCArchiveCustomer,
              p_CategoryGuid: _IQUGCArchiveCategoryGUID,
              p_SelectionType : _IQUGCArchiveSelectionType,
              p_MediaType : _IQUGCArchiveMediaType
          }

    getConfirm("Delete UGC Item", "Remove Selected Item(s)", "Confirm Deletion", "Cancel", function (res) {
        if (res == true) {
            $.ajax({
                url: _urlLibraryDeleteIQUGCArchiveResults,
                contentType: "application/json; charset=utf-8",
                type: "post",
                dataType: "json",
                data: JSON.stringify(jsonPostData),
                success: function (result) {

                    CheckForSessionExpired(result);

                    if (result.isSuccess) {
                        var returnIDs = result.ugcarchiveIDs.split(',');
                        var count = 0;
                        if (returnIDs != "") {
                            $.each(returnIDs, function (index, val) {
                                $("#divIQUGCArchive_" + val).remove();
                                count++;
                            });

                            if (result.filter != null) {
                                ModifyFiltersIQArchiveUGC(result.filter);
                                _IQUGCArchiveCurrentCategoryFilter = result.filter.Categories.slice(0);
                            }

                            ShowNoofRecordsIQUGCArchive(result);
                        }

                        ShowNotification(count + " record(s) deleted successfully...");

                        $("#divIQUGCArchiveResultsHTML input[type=checkbox]").each(function () {
                            $(this).prop("checked", false);
                        });

                        $("#chkIQUGCArchiveAll").prop("checked", false);

                    }
                    else {
                        ShowNotification(_msgErrorWhileDeleting);
                    }
                },
                error: function (a, b, c) {
                    ShowNotification(_msgErrorWhileDeleting);
                }
            });
        }
    });
}

function GetUGCContentEditForm(iqUGCArchiveKey) {
    var jsonPostData =
          {
              p_IQUGCArchiveKey: iqUGCArchiveKey
          }

    $.ajax({
        url: _urlLibraryGetIQUGCArchiveEdit,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {

            CheckForSessionExpired(result);

            if (result.isSuccess) {

                $("#divEditIQUGCArchiveHTML").html(result.HTML);
                $("#divEditIQUGCArchivePopup").modal({
                    backdrop: "static",
                    keyboard: true,
                    dynamic: true
                });
                CheckIQUGCCategoryValuesOnLoad();
            }
            else {
                ShowNotification(_msgErrorWhileFetchingData);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorWhileDeleting);
        }
    });
}

function CheckIQUGCCategoryValuesOnLoad() {
    if ($("#ddlUGCCategoryEdit").val() != "") {
        OnSelectOfCategory_UGCcontent("ddlUGCCategoryEdit", "ddlUGCCategoryEdit", "ddlUGCSubCategory1Edit", "ddlUGCSubCategory2Edit", "ddlUGCSubCategory3Edit", "divIQUGCArchiveEditErrorMsg");
    }
    if ($("#ddlUGCSubCategory1Edit").val() != "") {
        OnSelectOfCategory_UGCcontent("ddlUGCSubCategory1Edit", "ddlUGCCategoryEdit", "ddlUGCSubCategory1Edit", "ddlUGCSubCategory2Edit", "ddlUGCSubCategory3Edit", "divIQUGCArchiveEditErrorMsg");
    }
    if ($("#ddlUGCSubCategory2Edit").val() != "") {
        OnSelectOfCategory_UGCcontent("ddlUGCSubCategory2Edit", "ddlUGCCategoryEdit", "ddlUGCSubCategory1Edit", "ddlUGCSubCategory2Edit", "ddlUGCSubCategory3Edit", "divIQUGCArchiveEditErrorMsg");
    }
}

function CancelIQUGCArchiveEditPopup() {
    $("#divEditIQUGCArchivePopup").css({ "display": "none" });
    $("#divEditIQUGCArchivePopup").modal("hide");
}

function UpdateIQUGCArchiveEdit() {
    if (ValidateIQUGCUpdate()) {

        if ($("#hdnEditIQUGCArchiveKey").val() == undefined || $("#hdnEditIQUGCArchiveKey").val() == "") {
            ShowNotification("No record found to edit");
            return;
        }

        var jsonPostData =
          {
              p_IQUGCArchiveKey: $("#hdnEditIQUGCArchiveKey").val(),
              p_Title: $("#txtUGCTitleEdit").val(),
              p_Keywords: $("#txtUGCKeywordsEdit").val(),
              p_Customer: $("#ddlUGCCustomerEdit").val(),
              p_Category: $("#ddlUGCCategoryEdit").val(),
              p_Subcategory1: $("#ddlUGCSubCategory1Edit").val(),
              p_Subcategory2: $("#ddlUGCSubCategory2Edit").val(),
              p_Subcategory3: $("#ddlUGCSubCategory3Edit").val(),
              p_Description: $("#txtUGCDescriptionEdit").val()
          }

        $.ajax({
            url: _urlLibraryUpdateIQUGCArchive,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: function (result) {

                CheckForSessionExpired(result);

                if (result.isSuccess) {

                    if ($("#spanUGCTitle_" + $("#hdnEditIQUGCArchiveKey").val()) != undefined) 
                    {
                        $("#spanUGCTitle_" + $("#hdnEditIQUGCArchiveKey").val()).html($("#txtUGCTitleEdit").val());
                        $("#divUGCDescription_" + $("#hdnEditIQUGCArchiveKey").val()).html($("#txtUGCDescriptionEdit").val());
                    }
                    CancelIQUGCArchiveEditPopup();
                    $("#divEditIQUGCArchiveHTML").html("");
                    ShowNotification(_msgUGCRecordUpdated);
                    LoadIQUGCArchiveResults(false);
                }
                else {
                    ShowNotification(_msgErrorWhileUpdatingUGC);
                }
            },
            error: function (a, b, c) {
                ShowNotification(_msgErrorWhileUpdatingUGC);
            }
        });
    }
}

function ValidateIQUGCUpdate() {

    var flag = true;

    $("#formIQUGCArchiveUpdate span").html("");

    if ($.trim($("#txtUGCTitleEdit").val()) == "") {
        $("#spanUGCTitleEdit").html(_msgTitleRequired).show();
        flag = false;
    }
    if ($("#ddlUGCCustomerEdit").val() == "") {
        $("#spanUGCCustomerEdit").html(_msgCustomerRequired).show();
        flag = false;
    }
    if ($("#ddlUGCCategoryEdit").val() == "") {
        $("#spanUGCCategoryEdit").html(_msgCategoryRequired).show();
        flag = false;
    }

    return flag;
}

function DownloadUGCFile(iqUGCArchiveKey) {
    var jsonPostData =
          {
              p_IQUGCArchiveKey: iqUGCArchiveKey
          }

    $.ajax({
        url: _urlLibraryIQUGCArchiveCheckForUGCFile,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            CheckForSessionExpired(result);

            if (result.isSuccess) {
                window.location = _urlLibraryIQUGCArchiveDownloadFile;
            }
            else {
                ShowNotification(result.errorMessage);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorWhileDownloadingUGCFile);
        }
    });
}

function CancelUGCUpload() {
    getConfirm("Cancel Upload", "Are you sure to cancel upload?", "Cancel Upload", "Reject", function (res) {
        if (res && IsUploadActive) {
            if (IsBrowserIE()) {
                $("#txtUGCSelectedFileDisplay").hide();
                $("#fucUGCFile").show();
                xhrIE.abort();
            }
            else {
                xhr.abort();
            }

            $("#divProgressbar").hide();
            $("#divIEProgressbar").hide();
            $("#divUGCProgressbar").hide();

            $("#divCloudUGCUploadPopup span").html("").hide();
            $("#divCloudUGCUploadPopup input[type=text]").val("");
            $("#divCloudUGCUploadPopup select").val("0");
            $("#txtUGCDescription").val("");
            $("#divUGCCommonErrorMsg").html("").hide();
            $("#txtDateUploaded").val(getCurrentDateTimeAMPM());

            if (!IsBrowserIE()) {
                $("#divCloudUGCUploadPopup input[type=file]").val("");
            }
            else {
                $("#fucUGCFile").replaceWith($("#fucUGCFile").clone(true));
            }
            $("#chkAutoClip").prop("checked", false);

            ClearUGCFileSelection();
            $("#divUGCContentFormContent").show();
        }
    });
}

function UGCSelectPageSize(pageSize) {
    if (_IQUGCArchivePageSize != pageSize) {
        _IQUGCArchivePageSize = pageSize;
        $('#aUGCPageSize').html(_IQUGCArchivePageSize + ' Items Per Page&nbsp;&nbsp;<span class="caret"></span>');
        $('#divUGCPopover').remove();
        LoadIQUGCArchiveResults(false);
    }
}

function showIQUGArchiveCPopupOver() {
    if ($('#divUGCPopover').length <= 0) {
        var drphtml = $("#divIQUGCArchivePageSizeDropDown").html();
        $('#aUGCPageSize').popover({
            trigger: 'manual',
            html: true,
            title: '',
            placement: 'top',
            template: '<div id="divUGCPopover" class="popover" style="width:125px;"><div class="popover-inner"><div class="popover-content"><p></p></div></div></div>',
            content: drphtml
        });
        $('#aUGCPageSize').popover('show');
    }
    else {
        $('#divUGCPopover').remove();
    }
}

function AddIQUGCToLibrary() {
    if (ValidateCheckBoxSelection()) {
        var mediaIDs = new Array();
        $("#divIQUGCArchiveResultsHTML input[type=checkbox]").each(function () {
            if ($(this).is(':checked')) {
                mediaIDs.push($(this).val());
            }
        });

        var jsonPostData = {
            mediaIDs: mediaIDs
        }

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlLibraryAddIQUGCToLibrary,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            global: false,
            success: function (result) {
                if (result.isSuccess) {
                    ShowNotification(result.recordCount + " " + _msgRecordAddedToLibrary);
                }
                else {
                    ShowNotification(result.errorMessage);
                }
            },
            error: function (a, b, c) {
                ShowErrorMessage();
            }
        });
    }
    else {
        ShowNotification(_msgAtleastOneRecordSelect);
    }
}

function ValidateCheckBoxSelection() {
    var isChecked = false;
    $("#divIQUGCArchiveResultsHTML input[type=checkbox]").each(function () {
        if (this.checked) {
            isChecked = true;
        }
    });
    return isChecked;
}