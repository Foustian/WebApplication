var HasMCMediaResultsLoadedFirstTime = false;
var _MCMediaDisabledDays = null; 
var _MCMediaSearchTerm = "";
var _MCMediaFromDate = null;
var _MCMediaToDate = null;
var _MCMediaSubMediaType = "";
var _MCMediaSubMediaTypeDescription = "";
var _MCMediaCategoryGUID = [];
var _MCMediaCategoryNameList = [];
var _MCMediaCategoryName = "";
var _MCMediaCustomer = "";
var _MCMediaCustomerName = "";
var _MCMediaIsAsc = false;
var _MCMediaDuration = null;
var _MCMediaSortColumn = "CreatedDate";
var _MCMediaPageSize = null;
var _MCMediaSelectionType = 'OR';
var _MCMediaOldCategoryGUID = [];
var _MCMediaCurrentCategoryFilter = new Array();

$(function () {
    _MCMediaDisabledDays = [];

    $("#txtMCKeyword").keypress(function (e) {
        if (e.keyCode == 13) {
            SetKeyword_MCMedia();
        }
    });

    $("#txtMCKeyword").blur(function () {
        SetKeyword_MCMedia();
    });

    $("#imgMCKeyword").click(function (e) {
        SetKeyword_MCMedia();
    });

    $("#divMCCalender").datepicker({
        beforeShowDay: EnableAllTheseDays_MCMedia,
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            $("#dpMCFrom").val(dateText);
            $("#dpMCTo").val(dateText);
            SetDateVariable_MCMedia();
        }
    });

    $('.ndate').click(function () {
        $("#divMCCalender").datepicker("refresh");
    });

    $("body").click(function (e) {
        if ((e.target.id != "aMCPageSize" && e.target.id != "divMCPopover") && $(e.target).parents("#divMCPopover").size() <= 0) {
            $('#divMCPopover').remove();
        }

        if (e.target.id == "liMCCategoryFilter" || $(e.target).parents("#liMCCategoryFilter").size() > 0) {
            if ($('#ulMCCategory').is(':visible')) {
                $('#ulMCCategory').hide();
            }
            else {
                $('#ulMCCategory').show();
            }
        }
        else if ((e.target.id !== "liMCCategoryFilter" && e.target.id !== "ulMCCategory" && $(e.target).parents("#ulMCCategory").size() <= 0) || e.target.id == "btnMCSearchCategory") {
            $('#ulMCCategory').hide();
            if (e.target.id != "btnMCSearchCategory") {
                _MCMediaCategoryGUID = [];
                _MCMediaCategoryNameList = [];
                var categoriesHTML = "";
                if (_MCMediaSelectionType == $('#rdMCAnd').val()) {
                    $('#rdMCAnd').prop("checked", true);
                }
                else {
                    $('#rdMCOr').prop("checked", true);
                }

                $.each(_MCMediaCurrentCategoryFilter, function (eventID, eventData) {
                    if (_MCMediaOldCategoryGUID.length > 0 && $.inArray(eventData.CategoryGUID, _MCMediaOldCategoryGUID) !== -1) {
                        _MCMediaCategoryGUID.push(eventData.CategoryGUID);
                        _MCMediaCategoryNameList.push(eventData.CategoryName);
                    }
                    var liStyle = "";
                    if ($.inArray(eventData.CategoryGUID, _MCMediaCategoryGUID) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    categoriesHTML = categoriesHTML + '<li role=\"presentation\"><a ' + liStyle + ' href=\"javascript:void(0)\" onclick="SetCategory_MCMedia(this,\'' + eventData.CategoryGUID + '\',\'' + eventData.CategoryName.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + '\');" tabindex=\"-1\" role=\"menuitem\">';
                    categoriesHTML += EscapeHTML(eventData.CategoryName) + ' (' + eventData.RecordCountFormatted + ') </a></li>';
                });
                if (categoriesHTML == '') {
                    $('#ulMCCategoryList').html('<li role="presentation"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
                }
                else {
                    $("#ulMCCategoryList").html(categoriesHTML);
                }
            }
        }
    });

    $("#dpMCFrom").datepicker({
        showOn: "button",
        buttonImage: "../images/calender.png",
        buttonImageOnly: true,
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            $('#dpMCFrom').val(dateText);
            SetDateVariable_MCMedia();
        }
    });

    $("#dpMCTo").datepicker({
        showOn: "button",
        buttonImage: "../images/calender.png",
        buttonImageOnly: true,
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            $('#dpMCTo').val(dateText);
            SetDateVariable_MCMedia();
        }
    });

    if (screen.height >= 768) {
        $("#divMCMediaScrollContent").css("height", documentHeight - 240);
        $("#divMCMediaScrollContent").mCustomScrollbar({
            advanced: {
                updateOnContentResize: true,
                autoScrollOnFocus: false,
                scrollInertia: 60
            }
        });
    }

    $("#chkMCAll").click(function () {
        var c = this.checked;
        var backcolor = c == true ? '#F4F4F4' : '';
        $("input[id^='chkMCResults_']").each(function () {
            $(this).prop("checked", c);
            $(this).closest('.media').css('background', backcolor);
        });
    });

    // if all checkbox are selected, check the selectall checkbox and viceversa
    $(document).on("click", "input[id^='chkMCResults_']", function (event) {
        if ($(this).is(':checked')) {
            $(this).closest('.media').css('background', '#F4F4F4');
        }
        else {
            $(this).closest('.media').css('background', '');
        }
        if ($("input[id^='chkMCResults_']").length == $("input[id^='chkMCResults_']:checked").length) {
            $("#chkMCAll").prop("checked", true);
        } else {
            $("#chkMCAll").prop("checked", false);
        }
    });
});

function EnableAllTheseDays_MCMedia(date) {
    date = $.datepicker.formatDate("mm/dd/yy", date);
    return [$.inArray(date, _MCMediaDisabledDays) !== -1];
}

function SetKeyword_MCMedia() {
    if ($("#txtMCKeyword").val() != "" && _MCMediaSearchTerm != $("#txtMCKeyword").val()) {
        _MCMediaSearchTerm = $("#txtMCKeyword").val();
        LoadResults_MCMedia(false);
    }
}

function SetDateVariable_MCMedia() {
    if ($("#dpMCFrom").val() && $("#dpMCTo").val()) {
        if (_MCMediaFromDate != $("#dpMCFrom").val() || _MCMediaToDate != $("#dpMCTo").val()) {
            _MCMediaFromDate = $("#dpMCFrom").val();
            _MCMediaToDate = $("#dpMCTo").val();
            LoadResults_MCMedia(false);
            $('#ulMCCalender').parent().removeClass('open');
            $('#aDurationMC').html(_msgCustom + '&nbsp;&nbsp;<span class="caret"></span>');
        }
    }
    else
    {
        if ($("#dpMCFrom").val() != "" && $("#dpMCTo").val() == "") {
            $("#dpMCTo").addClass("warningInput");
        }
        else if ($("#dpMCTo").val() != "" && $("#dpMCFrom").val() == "") {
            $("#dpMCFrom").addClass("warningInput");
        }
    }
}

function SetCustomer_MCMedia(p_Customer, p_CustomerName) {
    if (_MCMediaCustomer != p_Customer) {
        _MCMediaCustomer = p_Customer;
        _MCMediaCustomerName = p_CustomerName;
        LoadResults_MCMedia(false);
    }
}

function SetCategory_MCMedia(eleCategory, p_CategoryGUID, p_CategoryName) {
    if ($.inArray(p_CategoryGUID, _MCMediaCategoryGUID) == -1) {
        _MCMediaCategoryGUID.push(p_CategoryGUID);
        _MCMediaCategoryNameList.push(p_CategoryName);
        $(eleCategory).css("background-color", "#E9E9E9");
    }
    else {
        var catIndex = _MCMediaCategoryGUID.indexOf(p_CategoryGUID);
        if (catIndex > -1) {
            _MCMediaCategoryGUID.splice(catIndex, 1);
            _MCMediaCategoryNameList.splice(catIndex, 1);
            $(eleCategory).css("background-color", "#ffffff");
        }
    }

    if ($('#rdMCAnd').is(":checked")) {
        $("#ulMCCategoryList li").each(function () {
            var _Category = $(this).find('a').attr("onclick").replace('SetCategory_MCMedia(this,', '').replace(');', '').split(',')[0].replace(/'/g, '');
            if ($.inArray(_Category, _MCMediaCategoryGUID) == -1) {
                $(this).find('a').addClass('blur');
            }
        });

        GetCategoryFilter_MCMedia();
    }
}

function SearchCategory_MCMedia() {
    var _CurrentMCMediaSelectionType;
    var IsPost = false;
    if ($('#rdMCAnd').is(":checked")) {
        _CurrentMCMediaSelectionType = $('#rdMCAnd').val();
    }
    else {
        _CurrentMCMediaSelectionType = $('#rdMCOr').val();
    }
    if ($(_MCMediaCategoryGUID).not(_MCMediaOldCategoryGUID).length != 0 || $(_MCMediaOldCategoryGUID).not(_MCMediaCategoryGUID).length != 0) {
        IsPost = true;
    }
    else if (_MCMediaCategoryGUID.length > 1 && (_MCMediaSelectionType != '' && _MCMediaSelectionType != _CurrentMCMediaSelectionType)) {
        IsPost = true;
    }

    if (IsPost) {
        _MCMediaOldCategoryGUID = _MCMediaCategoryGUID.slice(0);
        _MCMediaSelectionType = _CurrentMCMediaSelectionType;
        _MCMediaCategoryName = "";
        $.each(_MCMediaCategoryNameList, function (index, val) {
            if (_MCMediaCategoryName == "") {
                _MCMediaCategoryName = val;
            }
            else {
                _MCMediaCategoryName = _MCMediaCategoryName + ' "' + _MCMediaSelectionType + '" ' + val;
            }
        });
        LoadResults_MCMedia(false);
    }
}

function SetSelectionType_MCMedia() {
    if (_MCMediaCategoryGUID.length > 0 || _MCMediaOldCategoryGUID.length > 0) {
        _MCMediaCategoryGUID = [];
        _MCMediaCategoryNameList = [];
        GetCategoryFilter_MCMedia();
    }
}

function SetSubMediaType_MCMedia(p_SubMediaType, p_SubMediaTypeDescription) {
    if (_MCMediaSubMediaType != p_SubMediaType) {
        _MCMediaSubMediaType = p_SubMediaType;
        _MCMediaSubMediaTypeDescription = p_SubMediaTypeDescription;
        LoadResults_MCMedia(false);
    }
}

function GetCategoryFilter_MCMedia() {
    var jsonPostData = {
        p_FromDate: _MCMediaFromDate,
        p_ToDate: _MCMediaToDate,
        p_SearchTerm: _MCMediaSearchTerm,
        p_CustomerGuid: _MCMediaCustomer,
        p_CategoryGUID: _MCMediaCategoryGUID,
        p_SubMediaType: _SubMediaType
    }

    $.ajax({
        url: _urlLibraryFilterMCMediaCategory,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: 'json',
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess) {
                var categoriesHTML = "";
                $.each(result.categoryFilter, function (eventID, eventData) {
                    var liStyle = "";
                    if ($.inArray(eventData.CategoryGUID, _MCMediaCategoryGUID) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    categoriesHTML = categoriesHTML + '<li role=\"presentation\"><a ' + liStyle + ' href=\"javascript:void(0)\" onclick="SetCategory_MCMedia(this,\'' + eventData.CategoryGUID + '\',\'' + eventData.CategoryName.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + '\');" tabindex=\"-1\" role=\"menuitem\">';
                    categoriesHTML += EscapeHTML(eventData.CategoryName) + ' (' + eventData.RecordCountFormatted + ') </a></li>';
                });

                $("#ulMCCategoryList").html(categoriesHTML);
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

function LoadResults_MCMedia(isRefresh) {

    var jsonPostData = {
        p_CustomerGuid: _MCMediaCustomer,
        p_FromDate: _MCMediaFromDate,
        p_ToDate: _MCMediaToDate,
        p_SearchTerm: _MCMediaSearchTerm,
        p_SubMediaType: _MCMediaSubMediaType,
        p_CategoryGUID: _MCMediaCategoryGUID,
        p_SelectionType: _MCMediaSelectionType,
        p_IsAsc: _MCMediaIsAsc,
        p_SortColumn: _MCMediaSortColumn,
        p_PageSize: _MCMediaPageSize,
        p_IsRefresh: isRefresh
    }

    if (DateValidation()) {
        $.ajax({
            url: _urlLibrarySearchMCMediaResults,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: 'json',
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                // Set filter irrespective of success or failure
                SetActiveFilter_MCMedia();

                if (result.isSuccess) {
                    $("#divMCShowResult").show();
                    $("#divMCRecordCount").show();
                    $("#ulMCResults").html(result.HTML);
                    ModifyFilters_MCMedia(result.filter);
                    _MCMediaCurrentCategoryFilter = result.filter.Categories.slice(0);
                    ShowHideMoreResults_MCMedia(result);
                    ShowNoofRecords_MCMedia(result);
                    SetMediaClickEvent_MCMedia();
                    $("#chkMCAll").prop("checked", false);

                    if (screen.height >= 768) {
                        setTimeout(function () { $("#divMCMediaScrollContent").mCustomScrollbar("scrollTo", "top"); }, 200);
                    }

                    SetImageSrc();
                }
                else {
                    ShowNotification(_msgSomeErrorProcessing);
                    ClearResultsOnError('ulMCResults', 'divMCRecordCount', 'divMCShowResult', _msgErrorOnSearch.replace(/@@MethodName@@/g, "LoadResults_MCMedia(true)"));
                }
            },
            error: function (a, b, c) {
                $("#chkMCAll").prop("checked", false);
                ShowNotification(_msgSomeErrorProcessing, a);
                ClearResultsOnError('ulMCResults', 'divMCRecordCount', 'divMCShowResult', _msgErrorOnSearch.replace(/@@MethodName@@/g, "LoadResults_MCMedia(true)"));
            }
        });
    }
}

function SortDirection_MCMedia(sortColumn, isAsc) {
    if (isAsc != _MCMediaIsAsc || _MCMediaSortColumn != sortColumn) {
        _MCMediaIsAsc = isAsc;
        _MCMediaSortColumn = sortColumn;

        if (_MCMediaIsAsc && _MCMediaSortColumn == "MediaDate") {
            $('#aMCSortDirection').html(_msgOldestFirst + ' (Media Date)&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (!_MCMediaIsAsc && _MCMediaSortColumn == "MediaDate") {
            $('#aMCSortDirection').html(_msgMostRecent + ' (Media Date)&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (_MCMediaIsAsc && _MCMediaSortColumn == "CreatedDate") {
            $('#aMCSortDirection').html(_msgOldestFirst + ' (Created Date)&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (!_MCMediaIsAsc && _MCMediaSortColumn == "CreatedDate") {
            $('#aMCSortDirection').html(_msgMostRecent + ' (Created Date)&nbsp;&nbsp;<span class="caret"></span>');
        }
        LoadResults_MCMedia(false);
    }
}

function GetResultOnDuration_MCMedia(duration) {
    $("#dpMCFrom").removeClass("warningInput");
    $("#dpMCTo").removeClass("warningInput");
    var dtcurrent = new Date();
    var fDate;
    _MCMediaDuration = duration;

    // All
    if (duration == 0) {
        $("#dpMCFrom").val("");
        $("#dpMCTo").val("");
        dtcurrent = "";
        RemoveFilter_MCMedia(2);
        $('#aMCDuration').html(_msgAll + '&nbsp;&nbsp;<span class="caret"></span>');
    }

    // 24 hours
    else if (duration == 1) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth(), dtcurrent.getDate() - 1);
        $('#aMCDuration').html(_msgLast24Hours + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    // Last week
    else if (duration == 2) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth(), dtcurrent.getDate() - 7);
        $('#aMCDuration').html(_msgLastWeek + '&nbsp;&nbsp;<span class="caret"></span>');
    }

    // Last Month
    else if (duration == 3) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth() - 1, dtcurrent.getDate());
        $('#aMCDuration').html(_msgLastMonth + '&nbsp;&nbsp;<span class="caret"></span>');
    }

    // Last 3 Months
    else if (duration == 4) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth() - 3, dtcurrent.getDate());
        $('#aMCDuration').html(_msgLast3Month + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    // Custom
    else if (duration == 5) {
        dtcurrent = null;
        if ($("#dpMCFrom").val() != "" && $("#dpMCTo").val() != "") {
            fDate = $("#dpMCFrom").val();
            dtcurrent = $("#dpMCTo").val();
            $('#aMCDuration').html(_msgCustom + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        else {
            if ($("#dpMCFrom").val() == "") {
                $("#dpMCFrom").addClass("warningInput");
            }
            if ($("#dpMCTo").val() == "") {
                $("#dpMCTo").addClass("warningInput");
            }
            ShowNotification(_msgSelectDate);
        }
    }

    $("#dpMCFrom").datepicker("setDate", fDate);
    $("#dpMCTo").datepicker("setDate", dtcurrent);

    if ($("#dpMCFrom").val() != "" && $("#dpMCTo").val() != "") {
        if (_MCMediaFromDate != $("#dpMCFrom").val() || _MCMediaToDate != $("#dpMCTo").val()) {
            _MCMediaFromDate = $("#dpMCFrom").val();
            _MCMediaToDate = $("#dpMCTo").val();
            LoadResults_MCMedia(false);
        }
    }
}

function GetMoreResults_MCMedia() {
    var jsonPostData = {
        p_CustomerGuid: _MCMediaCustomer,
        p_FromDate: _MCMediaFromDate,
        p_ToDate: _MCMediaToDate,
        p_SearchTerm: _MCMediaSearchTerm,
        p_SubMediaType: _MCMediaSubMediaType,
        p_CategoryGUID: _MCMediaCategoryGUID,
        p_SelectionType: _MCMediaSelectionType,
        p_IsAsc: _MCMediaIsAsc,
        p_SortColumn: _MCMediaSortColumn,
        p_PageSize: _MCMediaPageSize
    }

    $.ajaxSetup({ cache: false });

    ShowHideProgressNearButton_MCMedia(true);

    $("#btnMCShowMoreResults").attr("disabled", "disabled");
    $("#btnMCShowMoreResults").attr("class", "disablebtn");

    $.ajax({
        url: _urlLibraryGetMoreMCMediaResults,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: 'json',
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            CheckForSessionExpired(result);

            if (result.isSuccess) {
                $("#ulMCResults").append(result.HTML);
                ShowHideMoreResults_MCMedia(result);
                ShowNoofRecords_MCMedia(result);
                SetMediaClickEvent_MCMedia();

                if ($("#chkMCAll").is(":checked")) {
                    $("input[id^='chkMCResults_']").each(function () {
                        $(this).prop("checked", true);
                        $(this).closest(".media").css("background", "#F4F4F4");
                    });
                }
                SetImageSrc();
            }
            else {
                ShowNotification(_msgSomeErrorProcessing);
            }
            ShowHideProgressNearButton_MCMedia(false);
            $("#btnMCShowMoreResults").removeAttr("disabled");
            $("#btnMCShowMoreResults").attr("class", "loadmore");
        },
        error: function (a, b, c) {
            ShowHideProgressNearButton_MCMedia(false);
            ShowNotification(_msgSomeErrorProcessing, a);

            $("#btnMCShowMoreResults").removeAttr("disabled");
            $("#btnMCShowMoreResults").attr("class", "loadmore");
        },
        global: false // This will not enable ".ajaxStart() global event handler and so global progress bar will not fire.
    });
}

function ShowPopupOver_MCMedia() {
    if ($('#divMCPopover').length <= 0) {
        var drphtml = $("#divMCPageSizeDropDown").html();
        $('#aMCPageSize').popover({
            trigger: 'manual',
            html: true,
            title: '',
            placement: 'top',
            template: '<div id="divMCPopover" class="popover" style="width:125px;"><div class="popover-inner"><div class="popover-content"><p></p></div></div></div>',
            content: drphtml
        });
        $('#aMCPageSize').popover('show');
    }
    else {
        $('#divMCPopover').remove();
    }
}

function SelectPageSize_MCMedia(pageSize) {
    if (_MCMediaPageSize != pageSize) {
        _MCMediaPageSize = pageSize;
        $('#aMCPageSize').html(_MCMediaPageSize + ' Items Per Page&nbsp;&nbsp;<span class="caret"></span>');
        $('#divMCPopover').remove();
        LoadResults_MCMedia(false);
    }
}

function SetActiveFilter_MCMedia() {
    var isFilterEnable = false;
    $("#divMCActiveFilter").html("");

    if (_MCMediaSearchTerm != "") {
        $('#divMCActiveFilter').append('<div id="divMCKeywordActiveFilter" class="filter-in">' + EscapeHTML(_MCMediaSearchTerm) + '<span class="cancel" onclick="RemoveFilter_MCMedia(1);"></span></div>');
        isFilterEnable = true;
    }
    if ((_MCMediaFromDate != null && _MCMediaFromDate != "") && (_MCMediaToDate != null && _MCMediaToDate != "")) {
        $('#divMCActiveFilter').append('<div id="divMCDateActiveFilter" class="filter-in">' + _MCMediaFromDate + ' To ' + _MCMediaToDate + '<span class="cancel" onclick="RemoveFilter_MCMedia(2);"></span></div>');
        isFilterEnable = true;
    }
    if (_MCMediaSubMediaType != "") {
        $('#divMCActiveFilter').append('<div id="divMCSubMediaTypeActiveFilter" class="filter-in">' + _MCMediaSubMediaTypeDescription + '<span class="cancel" onclick="RemoveFilter_MCMedia(3);"></span></div>');
        isFilterEnable = true;
    }
    if (_MCMediaCustomer != "") {
        $('#divMCActiveFilter').append('<div id="divMCCustomerActiveFilter" class="filter-in">' + _MCMediaCustomerName + '<span class="cancel" onclick="RemoveFilter_MCMedia(4);"></span></div>');
        isFilterEnable = true;
    }
    if (_MCMediaCategoryGUID.length > 0) {
        $('#divMCActiveFilter').append('<div id="divMCCategoryActiveFilter" class="filter-in">' + EscapeHTML(_MCMediaCategoryName) + '<span class="cancel" onclick="RemoveFilter_MCMedia(5);"></span></div>');
        isFilterEnable = true;
    }

    if (isFilterEnable) {
        $("#divMCActiveFilter").css({ 'border-bottom': '1px solid rgb(236, 236, 236)' });
    }
    else {
        $("#divMCActiveFilter").css({ 'border-bottom': '' });
    }
}

function RemoveFilter_MCMedia(filterType) {
    // Represent SearchKeyword
    if (filterType == 1) {
        $("#txtMCKeyword").val("");
        _MCMediaSearchTerm = "";
    }

    // Represent Date filter(From Date,To Date)
    if (filterType == 2) {
        $("#dpMCFrom").datepicker("setDate", null);
        $("#dpMCTo").datepicker("setDate", null);
        $("#divMCCalender").datepicker("setDate", null);

        $('#aMCDuration').html(_msgAll + '&nbsp;&nbsp;<span class="caret"></span>');

        _MCMediaFromDate = null;
        _MCMediaToDate = null;
    }

    // Represent SubMediaType Filter
    if (filterType == 3) {
        _MCMediaSubMediaType = "";
        _MCMediaSubMediaTypeDescription = "";
    }

    // Represent Customer Filter
    if (filterType == 4) {
        _MCMediaCustomer = "";
        _MCMediaCustomerName = "";
    }

    // Represent Category Filter
    if (filterType == 5) {
        _MCMediaCategoryGUID = [];
        _MCMediaCategoryNameList = [];
        _MCMediaOldCategoryGUID = [];
        _MCMediaCategoryName = "";
        _MCMediaSelectionType = $('#rdMCOr').val();
    }

    LoadResults_MCMedia(false);
}

function ModifyFilters_MCMedia(filter) {
    _MCMediaDisabledDays = [];
    if (filter != null && filter.Dates.length > 0) {
        $.each(filter.Dates, function (id, date) {
            _MCMediaDisabledDays.push(date);
        });
    }

    if (filter != null && filter.MediaTypes != null) {
        var subMediaTypeHTML = "";
        $.each(filter.MediaTypes, function (eventID, eventData) {
            subMediaTypeHTML += '<li class="dropdown-submenu"><a data-toggle="dropdown" class="dropdown-toggle" href="#" role="button" name="aMCMediaType">';
            subMediaTypeHTML += eventData.MediaTypeDesc + ' (' + eventData.RecordCountFormatted + ') </a>';
            subMediaTypeHTML += '<ul aris-labelledby="drop2" role="menu" class="dropdown-menu sideMenu" id="ulMCSubMediaType">'
            $.each(eventData.SubMediaTypes, function (eventID2, eventData2) {
                subMediaTypeHTML += '<li onclick="SetSubMediaType_MCMedia(\'' + eventData2.SubMediaType + '\', \'' + eventData2.SubMediaTypeDesc + '\');" role="presentation"><a href="#" tabindex="-1" role="menuitem">' + eventData2.SubMediaTypeDesc + ' (' + eventData2.RecordCountFormatted + ') </a></li>';
            });
            subMediaTypeHTML += '</ul></li>';
        });

        if (subMediaTypeHTML == '') {
            $('#ulMCMediaType').html('<li role="presentation"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
        }
        else {
            $('#ulMCMediaType').html(subMediaTypeHTML);
        }
    }

    if (filter != null && filter.Categories != null) {
        var categoriesHTML = "";
        var strAndSelection = "";
        var strOrSelection = ""
        if (_MCMediaSelectionType == 'AND') {
            strAndSelection = "checked=\"checked\"";
        }
        else {
            strOrSelection = "checked=\"checked\"";
        }

        $.each(filter.Categories, function (eventID, eventData) {
            var liStyle = "";
            if ($.inArray(eventData.CategoryGUID, _MCMediaCategoryGUID) !== -1) {
                liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
            }
            categoriesHTML = categoriesHTML + '<li role=\"presentation\"><a ' + liStyle + '  href=\"javascript:void(0)\" onclick="SetCategory_MCMedia(this,\'' + eventData.CategoryGUID + '\',\'' + eventData.CategoryName.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + '\');" tabindex=\"-1\" role=\"menuitem\">';
            categoriesHTML += EscapeHTML(eventData.CategoryName) + ' (' + eventData.RecordCountFormatted + ') </a></li>';
        });

        if (categoriesHTML == '') {
            $('#ulMCCategoryList').html('<li role="presentation"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
            $('#liMCCategorySearch').hide();
        }
        else {
            $('#ulMCCategoryList').html(categoriesHTML);
            $('#liMCCategorySearch').show();
        }
    }

    if (filter != null && filter.Customers != null) {
        var customerHTML = "";
        $.each(filter.Customers, function (eventID, eventData) {
            customerHTML = customerHTML + '<li onclick="SetCustomer_MCMedia(\'' + eventData.CustomerKey + '\',\'' + eventData.CustomerName.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + '\');" role=\"presentation\"><a href=\"#\" tabindex=\"-1\" role=\"menuitem\">';
            customerHTML += eventData.CustomerName + ' (' + eventData.RecordCountFormatted + ') </a></li>';
        });

        if (customerHTML == "") {
            $('#ulMCCustomers').html('<li role="presentation"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
        }
        else {
            $('#ulMCCustomers').html(customerHTML);
        }
    }
}

function ShowHideMoreResults_MCMedia(result) {
    if (result.hasMoreResults == true) {
        $('#btnMCShowMoreResults').attr('value', _msgShowMoreResults);
        $('#btnMCShowMoreResults').attr('onclick', 'GetMoreResults_MCMedia();');
    }
    else {
        $('#btnMCShowMoreResults').attr('value', _msgNoMoreResult);
        $('#btnMCShowMoreResults').removeAttr('onclick');
    }
}

function ShowNoofRecords_MCMedia(result) {
    if (result != null && result.totalRecords != null && result.totalRecords != "0" && result.currentRecords != null) {
        $("#spanMCRecordsLabel").show();
        $("#spanMCCurrentRecords").html(result.currentRecords);
        $("#spanMCTotalRecords").html(result.totalRecords);
        $("#aMCPageSize").show();
        $("#imgMCMoreResultLoading").addClass("offset2")
    }
    else {
        $("#spanMCRecordsLabel").hide();
        $("#aMCPageSize").hide();
        $("#imgMCMoreResultLoading").removeClass("offset2")
    }
}

function SetMediaClickEvent_MCMedia() {
    $("#ulMCResults .media").off("click");

    $("#ulMCResults .media").click(function (e) {
        if ($(e.target).closest("a").length <= 0 && e.target.type != "checkbox") {
            e.stopPropagation();
            if ($(e.target).closest('.media').find('input').is(':checked')) {
                $(e.target).closest('.media').find('input').removeAttr('checked');
                $(this).css("background", "");
            }
            else {
                $(e.target).closest('.media').find('input').prop('checked', true);
                $(this).css("background", "#F4F4F4");
            }

            if ($("input[id^='chkMCResults_']").length == $("input[id^='chkMCResults_']:checked").length) {
                $("#chkMCAll").prop("checked", true);
            } else {
                $("#chkMCAll").prop("checked", false);
            }
        }
    });
}

function ShowHideProgressNearButton_MCMedia(value) {
    if (value == true) {
        $("#imgMCMoreResultLoading").removeClass("visibilityHidden");
    }
    else {
        $("#imgMCMoreResultLoading").addClass("visibilityHidden");
    }
}

function DateValidation_MCMedia() {
    $('#dpMCFrom').removeClass('warningInput');
    $('#dpMCTo').removeClass('warningInput');


    // if both empty
    if (($('#dpMCTo').val() == '') && ($('#dpMCFrom').val() == '')) {
        return true;

    }
    //if one empty not other
    else if (($('#dpMCFrom').val() != '') && ($('#dpMCTo').val() == '')
                    ||
             ($('#dpMCFrom').val() == '') && ($('#dpMCTo').val() != '')
            ) {
        if ($('#dpMCFrom').val() == '') {
            ShowNotification(_msgFromDateNotSelected);
            $('#dpMCFrom').addClass('warningInput');
        }

        if ($('#dpMCTo').val() == '') {
            ShowNotification(_msgToDateNotSelected);
            $('#dpMCTo').addClass('warningInput');
        }
        return false;
    }
    //if both not empty
    else {
        var isFromDateValid = isValidDate($('#dpMCFrom').val().toString());
        var isToDateValid = isValidDate($('#dpMCTo').val().toString());
        if (isFromDateValid && isToDateValid) {
            var fromDate = new Date($('#dpMCFrom').val());
            var toDate = new Date($('#dpMCTo').val());
            if (fromDate > toDate) {
                ShowNotification(_msgFromDateLessThanToDate);
                $('#dpMCFrom').addClass('warningInput');
                $('#dpMCTo').addClass('warningInput');
                return false;
            }
            else {
                return true;
            }
        }
        else {
            if (!isFromDateValid) {
                $('#dpMCFrom').addClass('warningInput');
            }
            if (!isToDateValid) {
                $('#dpMCTo').addClass('warningInput');
            }
            ShowNotification(_msgInvalidDate);
            return false;
        }
    }
}

function AddToReport_MCMedia() {
    if (ValidateCheckBoxSelection_MCMedia()) {
        var mediaIDs;

        if (!$("#chkMCAll").prop("checked")) {
            mediaIDs = $.map($("input[id^='chkMCResults_']:checked"), function (n, i) { return n.value; });
        }

        var jsonPostData = {
            p_MediaIDs: mediaIDs,
            p_CustomerGuid: _MCMediaCustomer,
            p_FromDate: _MCMediaFromDate,
            p_ToDate: _MCMediaToDate,
            p_SearchTerm: _MCMediaSearchTerm,
            p_SubMediaType: _MCMediaSubMediaType,
            p_CategoryGUID: _MCMediaCategoryGUID,
            p_SelectionType: _MCMediaSelectionType
        }

        $.ajax({
            url: _urlLibraryAddToMCMediaReport,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: 'json',
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                if (result.isSuccess) {
                    if (!$("#chkMCAll").prop("checked")) {
                        $.each(mediaIDs, function (eventID, eventData) {
                            $("#publishedDiv_" + eventData).attr('title', 'Published');
                            $("#publishedDiv_" + eventData).addClass("isPublished");
                            $("#publishedDiv_" + eventData).removeClass("isNotPublished");
                        });
                    }
                    else {
                        $("div[id^='publishedDiv_']").attr('title', 'Published');
                        $("div[id^='publishedDiv_']").addClass("isPublished");
                        $("div[id^='publishedDiv_']").removeClass("isNotPublished");
                    }

                    ClearCheckboxSelection_MCMedia();
                    ShowNotification(result.recordCount + _msgMCMediaPublishSuccess);
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
    else {
        ShowNotification(_msgAtleastOneRecordSelect);
    }
}

function RemoveFromReport_MCMedia() {
    if (ValidateCheckBoxSelection_MCMedia()) {
        var mediaIDs;

        if (!$("#chkMCAll").prop("checked")) {
            mediaIDs = $.map($("input[id^='chkMCResults_']:checked"), function (n, i) { return n.value; });
        }

        var jsonPostData = {
            p_MediaIDs: mediaIDs,
            p_CustomerGuid: _MCMediaCustomer,
            p_FromDate: _MCMediaFromDate,
            p_ToDate: _MCMediaToDate,
            p_SearchTerm: _MCMediaSearchTerm,
            p_SubMediaType: _MCMediaSubMediaType,
            p_CategoryGUID: _MCMediaCategoryGUID,
            p_SelectionType: _MCMediaSelectionType
        }

        $.ajax({
            url: _urlLibraryRemoveFromMCMediaReport,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: 'json',
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                if (result.isSuccess) {
                    if (!$("#chkMCAll").prop("checked")) {
                        $.each(mediaIDs, function (eventID, eventData) {
                            $("#publishedDiv_" + eventData).attr('title', 'Not Published');
                            $("#publishedDiv_" + eventData).removeClass("isPublished");
                            $("#publishedDiv_" + eventData).addClass("isNotPublished");
                        });
                    }
                    else {
                        $("div[id^='publishedDiv_']").attr('title', 'Not Published');
                        $("div[id^='publishedDiv_']").removeClass("isPublished");
                        $("div[id^='publishedDiv_']").addClass("isNotPublished");
                    }

                    ClearCheckboxSelection_MCMedia();
                    ShowNotification(result.recordCount + _msgMCMediaUnpublishSuccess);
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
    else {
        ShowNotification(_msgAtleastOneRecordSelect);
    }
}

function GetMCMediaReportGUID(isOpenReport) {
    $.ajax({
        url: _urlLibraryGetMCMediaReportGUID,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: 'json',
        success: function (result) {
            if (result.isSuccess) {
                if (isOpenReport) {
                    if (result.reportGUID != "") {
                        window.open("../report/mcmediaroom?ID=" + result.reportGUID);
                    }
                    else {
                        ShowNotification(_msgMCMediaNoReport);
                    }
                }
            }
            else {
                ShowNotification(result.errorMessage);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgSomeErrorProcessing, a);
        }
    });
}

function ValidateCheckBoxSelection_MCMedia() {
    var isChecked = false;
    $("input[id^='chkMCResults_']").each(function () {
        if (this.checked) {
            isChecked = true;
        }
    });
    return isChecked;
}

function ClearCheckboxSelection_MCMedia() {
    $("#chkMCAll").removeAttr("checked")
    $("input[id^='chkMCResults_']").each(function () {
        this.checked = false;
        $(this).closest('.media').css('background', '');
    });
}
