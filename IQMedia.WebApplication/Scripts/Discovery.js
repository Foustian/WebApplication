var CONST_ZERO = "0";
var IsChartActive = 1;
var IsResultInitialLoad = 1;
var IsChartUpdated = false;
var searchTermCount = 0;
var _IsTabChange = false;
var _searchTerm = new Array();
var _searchTermbkp = '';
var _SearchTermValidationMessage = '';
var disabledDays = new Array();
var _SaveSearchTitle = '';

var _IsToggle = false;

var _SearchDate = '';
var _SearchDatebkp = '';
var _fromDate = '';
var _toDate = '';

var _SearchMediums = [];

var _SearchTVMarket = '';
var _SearchTVMarketbkp = '';

var _SearchTermIndex = 0;

var _SearchTermResult = '';
var msg = '';
var _NeedToValidateSearchTerm = true;
var _imgLoading = '<img alt="" id="imgSaveSearchLoading" class="marginRight10" src="../../Images/Loading_1.gif" />'
var _DateMessage = '';
var _ChartDate = '';
var _IsDefaultLoad = true;
var _MasterMediaTypes = []; // Array of submedia type objects with the structure [MediaType, SubMediaType, SubMediaType Display Name]

var _MaxDiscoveryReportItems = 0;
var _MaxDiscoveryExportItems = 0;
var _MaxDiscoveryHistory = 0;
var selectedCheckboxArray = new Array();

var _PieChartIndividualTotals = null;
var _PieChartDisplayedTotals = null;

// advance search variables
var _IsActiveAdvanceSearch = false;
var _UseAdvanceSearchDefault = true;
var AdvancedSearchList;
var AdvancedSearchListTemp;
var _IsAsc = false;
var _NextSearchTermID = 1;

var _PageSize = null;

var customCategoryObject = '';

var removeOriginalSearchTB = true; //Used to know if original "Search Term" textbox should be removed when "Add Agent Search Term" is clicked

function AddNewSearchTermTextBox() {

    $('#divPopover').remove();
    searchTermCount++;
    $('#ulSearchTerm').append("<li style=\"list-style:none outside none\" id=\"lisearchTerm_" + searchTermCount + "\" ><img id=\"imgRemoveSearchTermTextBox_" + searchTermCount + "\" src=\"../../Images/Delete.png?v=1.1\" alt=\"\" onclick=\"$('#lisearchTerm_" + searchTermCount + "').remove();PushSearchTermintoArray();CheckForMaxSearchTerm();\" class=\"marginTop-9 marginRight9 cursorPointer\" /><input type=\"text\" placeholder=\"Search Term\" id=\"txtSearchTerm_" + searchTermCount + "\" onclick=\"ShowAddSearchTermPopup(this.id);\" readonly=\"readonly\" /></li>");

    ShowAddSearchTermPopup("txtSearchTerm_" + searchTermCount);
    CheckForMaxSearchTerm();
}

function AddNewSearchAgentTextBox() {
    if (removeOriginalSearchTB && _searchTerm.length == 1 && _searchTerm[0].SearchTerm == "") {
        //if we can remove original tb, there is only one tb, and that tb is empty then clear the search without adding another tb
        $('#ulSearchTerm').html('');
        searchTermCount = 0;
    }

    $('#divPopover').remove();
    searchTermCount++;
    $('#ulSearchTerm').append("<li style=\"list-style:none outside none\" id=\"lisearchTerm_" + searchTermCount + "\" ><img id=\"imgRemoveSearchTermTextBox_" + searchTermCount + "\" src=\"../../Images/Delete.png?v=1.1\" alt=\"\" onclick=\"$('#lisearchTerm_" + searchTermCount + "').remove();PushSearchTermintoArray();CheckForMaxSearchTerm();\" class=\"marginTop-9 marginRight9 cursorPointer\" /><input type=\"text\" placeholder=\"Search Agent\" id=\"txtSearchTerm_" + searchTermCount + "\" onclick=\"ShowAddSearchAgentPopup(this.id);\" readonly=\"readonly\" /></li>");

    ShowAddSearchAgentPopup("txtSearchTerm_" + searchTermCount);
    CheckForMaxSearchTerm();
}

function CheckForMaxSearchTerm() {
    if ($('#ulSearchTerm li').length >= 5) {
        $('#divAddItem').hide();
        $('#divAddAgent').hide();
    }
    else {
        $('#divAddItem').show();
        $('#divAddAgent').show();
    }
}


function ShowAddSearchTermPopup(elementID) {

    $('#divPopover').remove();
    $('#' + elementID).popover({
        trigger: 'manual',
        html: true,
        title: '',
        placement: 'right',
        template: '<div id="divPopover" class="popover width50p"><div class="arrow"></div><div class="popover-inner"><div class="popover-content"><p></p></div></div></div>',
        content: '<div><input type=\"text\" class=\"popOverTextBox\" placeholder=\"Search Term\" id=\"txtSearchTermPopup_' + elementID.replace('txtSearchTerm_', '') + '\" onkeypress="GetChartData(event);" onblur="SetCurrentSearchTermTextBox(this.value, txtSearchTerm_' + elementID.replace('txtSearchTerm_', '') + ');" /></div>'
    });

    $('#' + elementID).popover('show');
    $('#txtSearchTermPopup_' + elementID.replace('txtSearchTerm_', '')).focus();
    $('#txtSearchTermPopup_' + elementID.replace('txtSearchTerm_', '')).val($('#' + elementID).val());

}

function ShowAddSearchAgentPopup(elementID) {

    $('#divPopover').remove();

    var agentString = '';
    if (_AgentList.length > 0) {
        agentString += '<div><select class="agent-popover" id=\"txtSearchTerm_' + elementID.replace('txtSearchTerm_', '') + '\" onchange="SetCurrentAgentTermTextBox(this.value, this.options[this.selectedIndex].text, this.options[this.selectedIndex].getAttribute(\'searchID\'), \'txtSearchTerm_' + elementID.replace('txtSearchTerm_', '') + '\')">';
        agentString += '<option class="dropdown-agent-popover" value="1">Select IQ Agent</option>';

        $.each(_AgentList, function (index, value) {
            var agentFull = '' + value;
            var agentArray = agentFull.split("|||");
            agentString += '<option class="dropdown-agent-popover" value="' + agentArray[1].toString().replace(/"/g, "&quot;") + '" searchID="' + agentArray[2].toString() + '">' + EscapeHTML(agentArray[0].toString()) + '</option>'
        });

        agentString += '<select></div>';
    }

    $('#' + elementID).popover({
        trigger: 'manual',
        html: true,
        title: '',
        placement: 'right',
        template: '<div id="divPopover" class="popover width50p"><div class="arrow"></div><div class="popover-inner"><div class="popover-content"><p></p></div></div></div>',
        content: agentString
    });

    $('#' + elementID).popover('show');
    $('#txtSearchTermPopup_' + elementID.replace('txtSearchTerm_', '')).focus();
    $('#txtSearchTermPopup_' + elementID.replace('txtSearchTerm_', '')).val($('#' + elementID).val());
}

$(document).ready(function () {
    // Set thousands separator for highcharts
    Highcharts.setOptions({
        lang: {
            thousandsSep: ","
        }
    });
    $('#narResDatepicker').click(function () { setDateVariableOnClick('#divCalender'); })
    $('#ulMainMenu li').removeAttr("class");
    $('#liMenuDiscovery').attr("class", "active");

    $("#dpFrom").datepicker({
        //beforeShowDay: enableAllTheseDays,
        showOn: "button",
        buttonImage: "../images/calender.png",
        buttonImageOnly: true,
        minDate: new Date(_constTVContentMinDate),
        changeMonth: true,
        changeYear: true,
        beforeShow: function () { setDateVariableOnClick('#dpFrom'); },
        onClose: function (dateText) {
            //$('#dpFrom').focus();
            // _fromDate = dateText;
            _SearchDate = '';
            SetDateVariable();
        }
    });
    $("#dpTo").datepicker({
        //beforeShowDay: enableAllTheseDays,   
        showOn: "button",
        buttonImage: "../images/calender.png",
        buttonImageOnly: true,
        minDate: new Date(_constTVContentMinDate),
        changeMonth: true,
        changeYear: true,
        beforeShow: function () { setDateVariableOnClick('#dpTo'); },
        onClose: function (dateText) {
            //$('#dpTo').focus();
            //_toDate = dateText;
            _SearchDate = '';
            SetDateVariable();
        }
    });
    $("#divCalender").datepicker({
        minDate: new Date(_constTVContentMinDate),
        changeMonth: true,
        changeYear: true,
        beforeShowDay: enableAllTheseDays,
        onSelect: function (dateText, inst) {
            IsChartUpdated = false;
            ResetSearchTermClassToFalse();
            _SearchDate = dateText;
            $("#dpFrom").datepicker("setDate", dateText);
            $("#dpTo").datepicker("setDate", dateText);
            SetDateVariable();
            $('#ulCalender').parent().removeClass('open');
        }
    });

    $('#imgResult').hover(function () {
        $('#imgResult').attr('src', '../../Images/Result-hover.png');
    }, function () {
        $('#imgResult').attr('src', '../../Images/Result.png');
    });

    if (getParameterByName("SearchTermTopic") == '' && getParameterByName("SearchNameTopic") == '') {
        var _tDate = new Date();
        var _fDate = new Date(_tDate.getFullYear(), _tDate.getMonth() - 3, _tDate.getDate());


        $("#dpFrom").datepicker("setDate", _fDate);
        $("#dpTo").datepicker("setDate", _tDate);

        _fromDate = $('#dpFrom').val();
        _toDate = $('#dpTo').val();

        $('#imgChart').hover(function () {
            $('#imgChart').attr('src', '../../Images/Chart-hover.png');
        }, function () {
            $('#imgChart').attr('src', '../../Images/Chart.png');
        });

        AddNewSearchTermTextBox();

        AdvancedSearchList = {};
        AdvancedSearchListTemp = {};
    }
    else {
        $('#ulSearchTerm').append("<li style=\"list-style:none outside none\" id=\"lisearchTerm_1\" ><input type=\"text\" placeholder=\"Search Term\" id=\"txtSearchTerm_1\" readonly=\"readonly\" style=\"height:30px; width:90%;\" /></li>");

        //set medium based on drop down
        _SearchMediums = [getParameterByName("TopicMedium")];

        //set other filters based on parent
        _SearchTVMarket = parent._SearchTVMarket;
        _SearchTVMarketbkp = parent._SearchTVMarketbkp;
        _SearchDate = parent._SearchDate;
        _SearchDatebkp = parent._SearchDatebkp;
        _fromDate = parent._fromDate;
        _toDate = parent._toDate;
        _IsActiveAdvanceSearch = parent._IsActiveAdvanceSearch;
        _UseAdvanceSearchDefault = parent._UseAdvanceSearchDefault;
        AdvancedSearchList = parent.AdvancedSearchList;
        _IsAsc = parent._IsAsc;
        $("#dpFrom").datepicker("setDate", _fromDate);
        $("#dpTo").datepicker("setDate", _toDate);

        SetCurrentAgentTermTextBox(getParameterByName("SearchTermTopic"), getParameterByName("SearchNameTopic"), "Z1", 'txtSearchTerm_1');

        $('#txtSearchTerm_1').val(getParameterByName("Topic"));
        $('#txtSearchTerm_1').attr('SearchTerm', '');
        $('#txtSearchTerm_1').attr('SearchID', 'Z1');
        $('#txtSearchTerm_1').attr('title', '');

        // logic so that loading message appears in popup
        // is hidden in OnResultSearchComplete()
        var loadingHTML = '<div id="divLoading1" class="loadingdiv"><span>Loading<img src=\'../images/1.gif\' /></span></div>';
        $(document.body).append(loadingHTML);
        $('#divLoading1').fadeIn(500);

        //show results tab
        ToggleChartResult();
    }

    $("#divPieChartHeader").delegate('div', 'click', function () {
        ShowPieChart($(this).index());
    });


    $("#divResultHeader").delegate('div', 'click', function () {

        _SearchTermIndex = $(this).index();
        GetDataOnTabChange();
    });

    $('.ndate').click(function () {
        $("#divCalender").datepicker("refresh");
    });

    $(".chosen-select").chosen({
        display_disabled_options: true,
        default_item: CONST_ZERO,
        width: "93%"
    });

    GetCustomCategory();
    GetSavedSearch(false, true);
    GetDiscoveryReportLimit();
    GetDiscoveryReport();

    // below function is called from IQMediaCommon.js
    // to set Height of content of the modal popup
    SetModalBodyScrollBarForPopUp();

    $('#divChartMain').css({ 'height': documentHeight - 100 });

    $('input:checkbox[name=chkPieChartMedium]').click(ShowHidePieChartMedium);
});

$(window).resize(function () {
    if (screen.height >= 768) {
        $('#divChartMain').css({ 'height': documentHeight - 100 });
    }
});

function GetCustomCategory(callback, forceBind) {
    if (customCategoryObject == '' || customCategoryObject == null || forceBind == true) {
        $(".media #divPopover .popover-content").addClass("blurOnlyControls");
        $("#imgSaveTweetLoading").show();

        $.ajax({

            type: 'POST',
            dataType: 'json',
            url: _urlCommonBindCategoryDropDown,
            contentType: 'application/json; charset=utf-8',
            global: false,

            success: function (res) {
                OnCategoryBindComplete(res, callback);
            },
            error: function (a, b, c) {
                ShowNotification(_msgErrorOccured, a);
            }
        });
    }
}

// When filtering on a media type, set the actual filter as every submedia type associated to it
function SetMediaType(event, mediaType) {
    // This event is fired whenever a child submedia type is selected. Only run it if a media type was selected.
    var event = window.event || event;
    var target = event.target || event.srcElement;

    if (target.name == "aMediaType") {
        if (mediaType != "TV") {
            _SearchTVMarket = '';
        }

        var subMediaTypes = $.map($.grep(_MasterMediaTypes, function (obj) {
            return obj[0] == mediaType;
        }), function (obj1, index) {
            return obj1[1]; // SubMedia Type abbreviations (TV, NM, Blog, PQ, etc.)
        });

        _SearchMediums = subMediaTypes;
        IsChartUpdated = false;
        _IsToggle = false;
        ResetSearchTermClassToFalse();

        SearchResult();
    }
}

function SetSubMediaType(subMediaType) {
    if (subMediaType != "TV") {
        _SearchTVMarket = '';
    }

    _SearchMediums = [subMediaType];
    IsChartUpdated = false;
    _IsToggle = false;
    ResetSearchTermClassToFalse();

    SearchResult();
}

function OnCategoryBindComplete(result, callback) {
    if (result.isSuccess) {
        $(".media #divPopover .popover-content").removeClass("blurOnlyControls");
        $("#imgSaveTweetLoading").hide();
        customCategoryObject = result.customCategory;
        var categoryOptions = '<option value="0">Select Category</option>';
        $.each(customCategoryObject, function (eventID, eventData) {
            categoryOptions = categoryOptions + '<option value="' + eventData.CategoryGUID + '">' + EscapeHTML(eventData.CategoryName) + '</option>';
        });

        $('#ddlReportCategory').html(categoryOptions);
        $('#ddlLibraryCategory').html(categoryOptions);

        if (typeof (callback) != 'undefined') {
            callback();
        }
    }
    else {
        if (typeof result.isAuthorized != 'undefined' && !result.isAuthorized) {
            RedirectToUrl(result.redirectURL);
        }
        else {
            ShowNotification(_msgErrorOccured);
        }
    }

}

function SetTVMarket(tvMarket) {
    _SearchTVMarket = tvMarket;

    IsChartUpdated = false;
    _IsToggle = false;
    ResetSearchTermClassToFalse();

    SearchResult();
}

function SetCurrentSearchTermTextBox(content, searchTermTextBoxID) {
    //do not use smart quotes
    content = content.trim().replace(/[\u201C\u201D]/g, '"');
    var tooltiptext = content.length > 100 ? tooltiptext = content.replace(/"/g, "\"").substring(0, 100) + "..." : tooltiptext = content.replace(/"/g, "\"");

    $('#' + searchTermTextBoxID.id).val(content);
    $('#' + searchTermTextBoxID.id).attr('SearchTerm', content);
    $('#' + searchTermTextBoxID.id).attr('SearchID', "Z" + _NextSearchTermID);
    $('#' + searchTermTextBoxID.id).attr('title', tooltiptext);

    $('#divPopover').remove();
    PushSearchTermintoArray();

    _NextSearchTermID++;
}

function SetCurrentAgentTermTextBox(content, display, agentID, searchTermTextBoxID) {
    content = content.trim().replace(/[\u2018\u2019]/g, "'").replace(/[\u201C\u201D]/g, '"');
    display = display.trim().replace(/[\u2018\u2019]/g, "'").replace(/[\u201C\u201D]/g, '"');
    var tooltiptext = content.length > 100 ? tooltiptext = content.replace(/"/g, "\"").substring(0, 100) + "..." : tooltiptext = content.replace(/"/g, "\"");

    $('#' + searchTermTextBoxID)
    $('#' + searchTermTextBoxID).val(display);
    $('#' + searchTermTextBoxID).attr('SearchTerm', content);
    $('#' + searchTermTextBoxID).attr('SearchID', agentID);
    $('#' + searchTermTextBoxID).attr('title', tooltiptext);

    $('#divPopover').remove();

    // Attach a done, fail, and progress handler for the asyncEvent
    if (isFinite(String(agentID).trim() || NaN) && parseInt(agentID) > -1) {
        $.when(AddAgentAdvancedSearch(agentID)).then(
            function (status) {
                PushSearchTermintoArray();
            },
            function (status) {
                ShowNotification(_msgErrorOccured);
            },
            function (status) {
            }
        );
    }
    else {
        PushSearchTermintoArray();
    }
}

function SortDirection(p_IsAsc) {
    if (_IsAsc != p_IsAsc) {
        _IsAsc = p_IsAsc;
        if (_IsAsc) {
            $('#aSortDirection').html(_msgOldestFirst + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        else {
            $('#aSortDirection').html(_msgMostRecent + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        SearchResult();
    }
}

function PushSearchTermintoArray() {
    //if (_NeedToValidateSearchTerm) {
    $('#divNoDataChart').html('');
    //$('#divNoDataResult').html('');

    _SearchTermIndex = 0;
    _searchTerm = new Array();

    IsChartUpdated = false;
    _IsToggle = false;

    var isCurrentSearchTermInArray = false;
    var index = 0;
    //var sTermID = 1;

    $('#ulSearchTerm li input[type=text]').each(function () {
        if (_searchTerm.length < 5) {
            if ($(this).attr('SearchTerm') == _SearchTermResult) {
                _SearchTermIndex = index;
                isCurrentSearchTermInArray = true;
            }
            index = index + 1;
            var SearchTermClass = new Object();
            SearchTermClass.SearchTerm = $(this).attr('SearchTerm');
            SearchTermClass.SearchID = $(this).attr('SearchID');
            SearchTermClass.SearchName = $(this).val();
            SearchTermClass.ResultShown = false;
            SearchTermClass.IsCurrentTab = false;

            SearchTermClass.ShownRecords = 0;
            SearchTermClass.AvailableRecords = 0;
            SearchTermClass.TotalRecords = 0;
            SearchTermClass.DisplayPageSize = 0;

            _searchTerm.push(SearchTermClass);
        }
    });
    if (!isCurrentSearchTermInArray && _searchTerm.length > 0) {
        _SearchTermResult = _searchTerm[0].SearchTerm;
    }

    GenerateSearchTermTab();
    if (_searchTerm.length > 0) {
        SearchResult();
    }
    else {
        $('#divDiscoveryClearAll').hide();
        $('#divDiscoveryUtility').hide();
        ClearAllData();
    }
}

function ClearAllData() {
    $('#divColumnChart').html('');
    $('#divLineChart').html('');
    $('#divPieChartHeader').html('');

    $('#divPieChartData').html('');
    $('#divPieChartData').removeAttr('style');

    $('#divPieChartStatic').html('');
    $('#divPieChartStaticPDF').html('');
    $('#divPieChartDynamic').html('');
    $('#divPieChartDynamicPDF').html('');
    $('#divPieChartChks').hide();

    $('#divChartTotal').html('');
    $('#divResult').html('');

    $('#ulMedium').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
    $('#ulMedium').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
    disabledDays = [];
    _SearchMediums = [];
    _SearchTVMarket = '';
    _SearchDate = '';
    $("#dpFrom").datepicker('option', 'beforeShowDay', '');
    $("#dpTo").datepicker('option', 'beforeShowDay', '');
    $('#divActiveFilter').html('');
}

function SearchResult() {
    _SearchTermValidationMessage = '';
    if (DateValidation()) {
        if (ValidateSearchTerm()) {
            removeOriginalSearchTB = false; //after a search has been performed do not allow the removal of original tb

            _IsDefaultLoad = false;
            if (ShowDateRangeExpansion()) {
                $('#divDiscoveryClearAll').show();
                SearchResultAjaxRequest();
            }
            else {
                getConfirm("Confirm Search", _DateMessage, "Continue", "Cancel", function (res) {
                    if (res == true) {
                        $('#divDiscoveryClearAll').show();
                        SearchResultAjaxRequest();
                    }
                });
            }
        }
        else {
            ShowNotification(_SearchTermValidationMessage);
        }
    }
}

function OnResultSearchFail(result) {
    ClearResultsOnError('divResult_Child_Data_' + _SearchTermIndex, 'spnNoOfRecords_' + _SearchTermIndex, 'divMoreResult_' + _SearchTermIndex, _msgErrorOnSearch.replace(/@@MethodName@@/g, "SearchResult()"));
    _IsTabChange = false;

    ShowNotification(_msgErrorOccured);
}
function OnResultSearchComplete(result) {
    $('#divResult_Child_NoData_' + result.searchedIndex).html('');

    if (result.availableDataResult) {
        SetNoDataAvailableMessage(result.availableDataResult, 'divResult_Child_NoData_' + result.searchedIndex);
    }

    if (result.isSuccess) {
        $('#divDiscoveryUtility').show();
        $("#spnNoOfRecords_" + result.searchedIndex).show();
        $("#divMoreResult_" + result.searchedIndex).show();
        SetDiscoveryResultStatus(result);
        
        _searchTerm[result.searchedIndex].IsCurrentTab = true;
        _searchTerm[result.searchedIndex].ResultShown = true;
        _searchTerm[result.searchedIndex].ShownRecords = result.searchTermShownRecords;
        _searchTerm[result.searchedIndex].AvailableRecords = result.searchTermAvailableRecords;
        _searchTerm[result.searchedIndex].TotalRecords = result.searchTermTotalRecords;
        _searchTerm[result.searchedIndex].DisplayPageSize = result.displayPageSize;
        _searchTerm[result.searchedIndex].DisplayPageSizeOptions = result.displayPageSizeOptions;

        $('#divResult_Child_Data_' + result.searchedIndex + ' > div').slice(0, _searchTerm[result.searchedIndex].ShownRecords).removeClass("displayNone");

        if ((_searchTerm[result.searchedIndex].ShownRecords < _searchTerm[result.searchedIndex].AvailableRecords)
               ||
                (_searchTerm[result.searchedIndex].ShownRecords < _searchTerm[result.searchedIndex].TotalRecords)) {
            if (_searchTerm[result.searchedIndex].ShownRecords < _MaxRecordShownLimit) {
                result.hasMoreResults = true;
            }
            else {
                result.hasMoreResults = false;
            }
        }
        else {
            result.hasMoreResults = false;
        }

        var _PageSizeArray = JSON.parse("[" + _searchTerm[result.searchedIndex].DisplayPageSizeOptions + "]");
        $('#divMoreResult_' + result.searchedIndex).remove();
        $('#divResult_Child_Data_' + result.searchedIndex).append('<div id="divMoreResult_' + result.searchedIndex + '" align="center" style="width:75%"><img alt="" id="imgMoreResultLoading_' + result.searchedIndex + '" class="marginRight10 visibilityHidden" src="../../Images/Loading_1.gif"><input value="No More Results" class="loadmore displayNone" id="btnShowMoreResults_' + result.searchedIndex + '" style="display: inline;" type="button"></div></div>');
        var _stringPageSize = '<div class="padingDropdown"><a href="#" data-toggle="dropdown fleft" onclick="showPopupOver(' + result.searchedIndex + ')" style="" class="btn dropdown-toggle no-background margintop10 marginRight10 float-right" id="aPageSize_' + result.searchedIndex + '">' + _searchTerm[result.searchedIndex].DisplayPageSize + ' Items Per Page&nbsp;&nbsp;<span class="caret"></span></a><div class="displayNone" id="divPageSizeDropDown_' + result.searchedIndex + '"><ul style="width: auto;" class="dropdown-popover">';
        for (var i = 0; i < _PageSizeArray.length; i++) {
            _stringPageSize = _stringPageSize + '<li><a onclick="SelectPageSize(' + _PageSizeArray[i] + ',' + result.searchedIndex + ');" class="cursorPointer">' + _PageSizeArray[i] + '</a></li>'
        }
        _stringPageSize = _stringPageSize + '</ul></div></div>';
        $('#divResult_Child_Data_' + result.searchedIndex).append(_stringPageSize);
        $('#btnShowMoreResults' + result.searchedIndex).show();
        if (!result.hasMoreResults) {

            $('#btnShowMoreResults_' + result.searchedIndex).attr('value', _msgNoMoreResult);
            $('#btnShowMoreResults_' + result.searchedIndex).removeAttr('onclick');

        }
        else {

            $('#btnShowMoreResults_' + result.searchedIndex).attr('value', _msgShowMoreResults);
            $('#btnShowMoreResults_' + result.searchedIndex).attr('onclick', 'ShowMoreResult();');
        }

        if (screen.height >= 768) {
            if (getParameterByName("SearchTermTopic") == '' && getParameterByName("SearchNameTopic") == '') {
                $('#divResult_Child_Scroll_' + result.searchedIndex).css({ 'height': documentHeight - 200 });
            }
            else {
                $('#divResult_Child_Scroll_' + result.searchedIndex).css({ 'height': documentHeight - 135 });
            }
            $('#divResult_Child_Scroll_' + result.searchedIndex).mCustomScrollbar("destroy");
            $('#divResult_Child_Scroll_' + result.searchedIndex).mCustomScrollbar({
                advanced: {
                    updateOnContentResize: true,
                    autoScrollOnFocus: false
                },
                scrollInertia: 60
            });


            setTimeout(function () {
                $('#divResult_Child_Scroll_' + result.searchedIndex).mCustomScrollbar("scrollTo", "top");
            }, 200);
        }

        _SearchDatebkp = _SearchDate;
        _searchTermbkp = _searchTerm;
        _SearchTVMarketbkp = _SearchTVMarket;

        if (!_IsToggle) {
            $('#divChartTotal').html('Total Records :: ' + result.chartTotal);
            _PieChartIndividualTotals = result.pieChartSearchTermTotals;
            RenderColumnChartHighCharts(result.columnChartJson);
            RenderLineHighChart(result.lineChartJson);
            RenderSearchTermTabs(result.pieChartMediumJson, result.pieChartSearchTermJson, result.lineChartMediumJson);
            SetDateFilter(result);
            SetMediumFilter(result);

            $('#divNoDataChart').html('');

            if (result.availableDataChart) {
                SetNoDataAvailableMessage(result.availableDataChart, 'divNoDataChart');
            }
            _IsToggle = true;
        }

        if (getParameterByName("SearchTermTopic") != '' || getParameterByName("SearchNameTopic") != '') {
            //remove loading message for popup that was manually set in $(document).ready
            $("#divLoading1").remove();

            SetDateFilter(result);
            SetMediumFilter(result);
        }

        _IsTabChange = false;

        SetImageSrc();

        SetMediaClickEvent();
    }
    else {
        CheckForAuthentication(result, "An error occurred. Please save your search and reload.");
        ClearResultsOnError('divResult_Child_Data_' + _SearchTermIndex, 'spnNoOfRecords_' + _SearchTermIndex, 'divMoreResult_' + _SearchTermIndex, _msgErrorOnSearch.replace(/@@MethodName@@/g, "SearchResult()"));
    }
}

function OnMediaSearchComplete(result) {
    IsChartUpdated = true;
    $('#divNoDataChart').html('');

    if (result.availableDataChart) {
        SetNoDataAvailableMessage(result.availableDataChart, 'divNoDataChart');
    }
    if (result.isSuccess) {
        if (result.isSearchTermValid) {
            if (!_IsDefaultLoad) {
                $('#divDiscoveryUtility').show();
                _SearchDatebkp = _SearchDate;
                _searchTermbkp = _searchTerm;
                _SearchTVMarketbkp = _SearchTVMarket;
                _PieChartIndividualTotals = result.pieChartSearchTermTotals;

                RenderColumnChartHighCharts(result.columnChartJson);
                RenderLineHighChart(result.lineChartJson);
                RenderSearchTermTabs(result.pieChartMediumJson, result.pieChartSearchTermJson, result.lineChartMediumJson);
                SetDateFilter(result);
                SetMediumFilter(result);

                $('#divChartTotal').html('Total Records :: ' + result.chartTotal);
            }
            else {
                $('#divPieChartHeader').hide();
                $('#divPieChartData').hide();
                $('#divDiscoveryClearAll').hide();
                $('#divDiscoveryUtility').hide();
            }
            _IsToggle = true;
            SetImageSrc();
            SetMediaClickEvent();
        } else {
            ShowNotification(_msgSearchTermAlreadyEntered);
        }
    }
    else {
        CheckForAuthentication(result, "An error occurred. Please save your search and reload.");
    }
    setTimeout(function () {
    }, 200);
}

function OnFail(result) {
    _IsTabChange = false;
    _IsToggle = false;

    ShowNotification(_msgErrorOccured);
}

function SetMediaClickEvent() {
    $("#divResult .media").click(function (e) {
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
        }
    });
}

function SearchResultAjaxRequest() {
    var mySearchTermArray = new Array();
    var mySearchNameArray = new Array();
    var mySearchIDArray = new Array();
    for (var zz = 0; zz < _searchTerm.length; zz++) {
        mySearchTermArray.push(_searchTerm[zz].SearchTerm.trim());
        mySearchNameArray.push(_searchTerm[zz].SearchName.trim());
        mySearchIDArray.push(_searchTerm[zz].SearchID.trim());
    }

    var advanceSearchesArray = [];
    $.each(AdvancedSearchList, function () {
        advanceSearchesArray.push(this);
    });
    var advanceSearchIDsArray = [];
    $.each(AdvancedSearchList, function (item, index) {
        advanceSearchIDsArray.push(item);
    });

    SetActiveFilter();
    ClearCheckboxSelection();

    if (IsChartActive == 0) {
        var jsonPostData = {
            searchTermIndex: _SearchTermIndex,
            searchTermArray: mySearchTermArray,
            searchNameArray: mySearchNameArray,
            searchIDArray: mySearchIDArray,
            fromDate: _fromDate,
            toDate: _toDate,
            mediums: _SearchMediums,
            IsTabChange: _IsTabChange,
            IsToggle: _IsToggle,
            IsAsc: _IsAsc,
            PageSize: _PageSize,
            advanceSearches: _IsActiveAdvanceSearch == true ? advanceSearchesArray : new Object(),
            advanceSearchIDs: _IsActiveAdvanceSearch == true ? advanceSearchIDsArray : new Object(),
            useAdvancedSearchDefault: _UseAdvanceSearchDefault
        }

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlDiscoveryMediaJsonResults,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),

            success: OnResultSearchComplete,
            error: OnResultSearchFail
        });
    }
    else {
        var jsonPostData = {
            searchTerm: mySearchTermArray,
            searchName: mySearchNameArray,
            searchID: mySearchIDArray,
            fromDate: _fromDate,
            toDate: _toDate,
            mediums: _SearchMediums,
            isDefaultLoad: _IsDefaultLoad,
            advanceSearches: _IsActiveAdvanceSearch == true ? advanceSearchesArray : new Object(),
            advanceSearchIDs: _IsActiveAdvanceSearch == true ? advanceSearchIDsArray : new Object(),
            useAdvancedSearchDefault: _UseAdvanceSearchDefault
        }

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlDiscoveryMediaJsonChart,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),

            success: OnMediaSearchComplete,
            error: OnFail
        });
    }
}

function ShowMoreResult() {
    if (_searchTerm[_SearchTermIndex].ShownRecords < _searchTerm[_SearchTermIndex].AvailableRecords) {

        $('#imgMoreResultLoading_' + _SearchTermIndex).removeClass("visibilityHidden");
        $('#btnShowMoreResults_' + _SearchTermIndex).attr("disabled", "disabled");
        $('#btnShowMoreResults_' + _SearchTermIndex).attr("class", "disablebtn");

        setTimeout(function () {
            _searchTerm[_SearchTermIndex].ShownRecords += _searchTerm[_SearchTermIndex].DisplayPageSize;

            if (_searchTerm[_SearchTermIndex].ShownRecords > _searchTerm[_SearchTermIndex].TotalRecords) {
                _searchTerm[_SearchTermIndex].ShownRecords = _searchTerm[_SearchTermIndex].TotalRecords;
            }

            if (_searchTerm[_SearchTermIndex].AvailableRecords > _searchTerm[_SearchTermIndex].TotalRecords) {
                _searchTerm[_SearchTermIndex].AvailableRecords = _searchTerm[_SearchTermIndex].TotalRecords;
            }

            $('#divResult_Child_Data_' + _SearchTermIndex + ' > div').slice(0, _searchTerm[_SearchTermIndex].ShownRecords).removeClass("displayNone");
            if (_searchTerm[_SearchTermIndex].TotalRecords > 0) {
                $('#spnNoOfRecords_' + _SearchTermIndex).html(numberWithCommas(_searchTerm[_SearchTermIndex].ShownRecords) + ' of ' + numberWithCommas(_searchTerm[_SearchTermIndex].TotalRecords));
            }

            var hasMoreResults = false;
            if ((_searchTerm[_SearchTermIndex].ShownRecords < _searchTerm[_SearchTermIndex].AvailableRecords)
               ||
                (_searchTerm[_SearchTermIndex].ShownRecords < _searchTerm[_SearchTermIndex].TotalRecords)) {
                hasMoreResults = true;
            }
            else {
                hasMoreResults = false;
            }

            $('#imgMoreResultLoading_' + _SearchTermIndex).addClass("visibilityHidden");
            $('#btnShowMoreResults_' + _SearchTermIndex).attr("class", "loadmore");
            $('#btnShowMoreResults_' + _SearchTermIndex).removeAttr("disabled");

            if (!hasMoreResults) {

                $('#btnShowMoreResults_' + _SearchTermIndex).attr('value', _msgNoMoreResult);
                $('#btnShowMoreResults_' + _SearchTermIndex).removeAttr('onclick');

            }
            else {

                $('#btnShowMoreResults_' + _SearchTermIndex).attr('value', _msgShowMoreResults);
                $('#btnShowMoreResults_' + _SearchTermIndex).attr('onclick', 'ShowMoreResult();');
            }

            if ($("#chkInputAll").is(":checked")) {
                checkUncheckAll('divResult', 'chkInputAll');
            }
        }, 500);

    }
    else {

        var mySearchTermArray = new Array();
        for (var zz = 0; zz < _searchTerm.length; zz++) {
            mySearchTermArray.push(_searchTerm[zz].SearchTerm.trim());
        }

        selectedCheckboxArray = new Array();
        $("#divResult div.media:not(.displayNone) input[type=checkbox]").each(function () {
            if (this.checked)
            { selectedCheckboxArray.push(this.id); }
        });

        var jsonPostData = {
            searchTermIndex: _SearchTermIndex,
            fromDate: _fromDate,
            toDate: _toDate,
            mediums: _SearchMediums,
            searchTermArray: mySearchTermArray,
            IsAsc: _IsAsc,
            PageSize: _PageSize,
            advanceSearch: _IsActiveAdvanceSearch == true ? getAdvancedSearchByTermIndex(_SearchTermIndex) : new Object()
        }

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlDiscoveryMoreResult,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            global: false,
            success: OnMoreResultComplete,
            error: OnMoreResultFail
        });

        $('#imgMoreResultLoading_' + _SearchTermIndex).removeClass("visibilityHidden");
        $('#btnShowMoreResults_' + _SearchTermIndex).attr("disabled", "disabled");
        $('#btnShowMoreResults_' + _SearchTermIndex).attr("class", "disablebtn");
    }
}

function OnMoreResultComplete(result) {

    $('#imgMoreResultLoading_' + result.searchedIndex).addClass("visibilityHidden");
    $('#divResult_Child_NoData_' + result.searchedIndex).html('');
    
    if (result.isSuccess) {

        if (result.availableData) {
            SetNoDataAvailableMessage(result.availableData, 'divResult_Child_NoData_' + result.searchedIndex);
        }
        else {
            $('#divResult_Child_NoData_' + result.searchedIndex).html('');
        }

        SetDiscoveryResultStatus(result);


        $('#divResult_Child_Data_' + result.searchedIndex).html(result.HTML);

        _searchTerm[result.searchedIndex].ShownRecords += result.searchTermShownRecords;
        _searchTerm[result.searchedIndex].AvailableRecords = result.searchTermAvailableRecords;
        _searchTerm[result.searchedIndex].TotalRecords = result.searchTermTotalRecords;

        if (_searchTerm[result.searchedIndex].ShownRecords > _searchTerm[result.searchedIndex].TotalRecords) {
            _searchTerm[result.searchedIndex].ShownRecords = _searchTerm[result.searchedIndex].TotalRecords;
        }

        if (_searchTerm[result.searchedIndex].AvailableRecords > _searchTerm[result.searchedIndex].TotalRecords) {
            _searchTerm[result.searchedIndex].AvailableRecords = _searchTerm[result.searchedIndex].TotalRecords;
        }
        _searchTerm[result.searchedIndex].TotalRecords = result.searchTermTotalRecords;

        $('#divResult_Child_Data_' + result.searchedIndex + ' > div').slice(0, _searchTerm[result.searchedIndex].ShownRecords).removeClass("displayNone");

        if ((_searchTerm[result.searchedIndex].ShownRecords < _searchTerm[result.searchedIndex].AvailableRecords)
               ||
                (_searchTerm[result.searchedIndex].ShownRecords < _searchTerm[result.searchedIndex].TotalRecords)
               ) {
            if (_searchTerm[result.searchedIndex].ShownRecords < _MaxRecordShownLimit) {
                result.hasMoreResults = true;
            }
            else {
                result.hasMoreResults = false;
            }
        }
        else {
            result.hasMoreResults = false;
        }

        _searchTerm[result.searchedIndex].DisplayPageSize = result.displayPageSize;

        if (result.isAnyDataAvailable && _searchTerm[result.searchedIndex].TotalRecords > 0) {
            $('#spnNoOfRecords_' + result.searchedIndex).html(numberWithCommas(_searchTerm[result.searchedIndex].ShownRecords) + ' of ' + numberWithCommas(_searchTerm[result.searchedIndex].TotalRecords));
        }


        var _PageSizeArray = JSON.parse("[" + _searchTerm[result.searchedIndex].DisplayPageSizeOptions + "]");
        $('#divMoreResult_' + result.searchedIndex).remove();
        $('#divResult_Child_Data_' + result.searchedIndex).append('<div id="divMoreResult_' + result.searchedIndex + '" align="center"><img alt="" id="imgMoreResultLoading_' + result.searchedIndex + '" class="marginRight10 visibilityHidden" src="../../Images/Loading_1.gif"><input value="No More Results" class="loadmore displayNone" id="btnShowMoreResults_' + result.searchedIndex + '" style="display: inline;" type="button"></div></div>');
        var _stringPageSize = '<div class="padingDropdown"><a href="#" data-toggle="dropdown fleft" onclick="showPopupOver(' + result.searchedIndex + ')" style="" class="btn dropdown-toggle no-background margintop10 marginRight10 float-right" id="aPageSize_' + result.searchedIndex + '">' + _searchTerm[result.searchedIndex].DisplayPageSize + ' Items Per Page&nbsp;&nbsp;<span class="caret"></span></a><div class="displayNone" id="divPageSizeDropDown_' + result.searchedIndex + '"><ul style="width: auto;" class="dropdown-popover">';
        for (var i = 0; i < _PageSizeArray.length; i++) {
            _stringPageSize = _stringPageSize + '<li><a onclick="SelectPageSize(' + _PageSizeArray[i] + ',' + result.searchedIndex + ');" class="cursorPointer">' + _PageSizeArray[i] + '</a></li>'
        }
        _stringPageSize = _stringPageSize + '</ul></div></div>';
        $('#divResult_Child_Data_' + result.searchedIndex).append(_stringPageSize);
        $('#btnShowMoreResults' + result.searchedIndex).show();
        if (!result.hasMoreResults) {

            $('#btnShowMoreResults_' + result.searchedIndex).attr('value', _msgNoMoreResult);
            $('#btnShowMoreResults_' + result.searchedIndex).removeAttr('onclick');
        }
        else {
            $('#btnShowMoreResults_' + result.searchedIndex).attr('value', _msgShowMoreResults);
            $('#btnShowMoreResults_' + result.searchedIndex).attr('onclick', 'ShowMoreResult();');
        }

        if ($("#chkInputAll").is(":checked")) {
            checkUncheckAll('divResult', 'chkInputAll');
        }
        else {
            for (var i = 0; i < selectedCheckboxArray.length; i++) {
                $("#" + selectedCheckboxArray[i]).prop("checked", true);
            }
        }

        SetImageSrc();
        SetMediaClickEvent();
    }
    else {
        CheckForAuthentication(result, 'Some error occured, try again later');
    }
}

function OnMoreResultFail(result) {
    $('#imgMoreResultLoading_' + result.searchedIndex).addClass("visibilityHidden");
    ShowNotification(_msgErrorOccured);
}

function ValidateSearchTerm() {
    var returnValue = true;

    if (_searchTerm.length <= 0) {
        _SearchTermValidationMessage = _msgEnterSearchTerm;
        returnValue = false;
    }

    for (var i = 0; i < _searchTerm.length; i++) {
        var searchTerm = _searchTerm[i].SearchTerm.trim();
        var openParens = 0;
        var doubleQuotes = 0;

        if (searchTerm == '') {

            returnValue = false;
            _SearchTermValidationMessage = _msgEnterSearchTerm;
            break;
        }
        else {
            // Basic verification for matching numbers of " and (), to help prevent errors in solr
            for (var k = 0; k < searchTerm.length; k++) {
                if (searchTerm[k] == "(") {
                    openParens++;
                }
                else if (searchTerm[k] == ")") {
                    openParens--;
                }
                else if (searchTerm[k] == "\"") {
                    doubleQuotes++;
                }
            }

            if (doubleQuotes % 2 != 0) {
                returnValue = false;
                _SearchTermValidationMessage = _msgSearchTermInvalidDblQuotes;
                break;
            }
            if (openParens != 0) {
                returnValue = false;
                _SearchTermValidationMessage = _msgSearchTermInvalidParens;
                break;
            }
        }

        for (var j = 0; j < _searchTerm.length; j++) {
            if (i != j) {

                if (searchTerm == _searchTerm[j].SearchTerm.trim()) {
                    returnValue = false;
                    _SearchTermValidationMessage = _msgSearchTermAlreadyEntered;
                    break;
                }
            }
        }
    }

    return returnValue;
}

function SetTVMarketFilter(result) {
    $('#ulTVMarket').html('');

    var tvMarketLI = '';
    if (result.discoveryTVMarketFilter) {
        $.each(result.discoveryTVMarketFilter, function (eventID, eventData) {
            tvMarketLI = tvMarketLI + '<li onclick="SetTVMarket(\'' + eventData + '\');" role="presentation"><a href="#" tabindex="-1" role="menuitem">' + eventData + '</a></li>';
        });

        if (tvMarketLI != '') {
            $('#ulTVMarket').html(tvMarketLI);
        }
        else {
            $('#ulTVMarket').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
        }
    }
    else {
        $('#ulTVMarket').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
    }

}
function SetActiveFilter() {
    $('#divActiveFilter').html('');
    $('#divActiveFilter').removeClass("bottomBorderColor");

    if (_IsActiveAdvanceSearch) {
        $('#divActiveFilter').append('<div class=\"filter-in\" onclick="OpenAdvanceSearchPopup();" id="divFilterAdvanceSearch" >Advanced Search Active<span class="cancel"></span></div>');

        $("#divFilterAdvanceSearch span").click(function (event) {
            RemoveAdvanceSearchFilter();
            event.stopPropagation();
        });
    }

    // Display individual filters for each submedia type, so that they can be disabled independently
    if (_SearchMediums != null && _SearchMediums.length > 0) {
        var filterHTML = "";
        var subMediaTypes = $.grep(_MasterMediaTypes, function (obj) {
            return $.inArray(obj[1], _SearchMediums) > -1;
        });

        $.each(subMediaTypes, function (index, obj) { // See _MasterMediaTypes declaration for structure
            filterHTML += '<div id="divSubMediaTypeActiveFilter_' + obj[1] + '" class="filter-in">' + obj[2] + '<span class="cancel" onclick="RemoveMediumFilter(\'' + obj[1] + '\');"></span></div>';
        });
        $('#divActiveFilter').append(filterHTML);

        $("div[name='advSearchTabContent']").addClass('blurOnlyControls');

        $.each(_SearchMediums, function (eventID, eventData) {
            switch (eventData) {
                case 'TV':
                    $("#divTVTabContent").removeClass('blurOnlyControls');
                    break;
                case 'NM':
                    $("#divOnlineNewsTabContent").removeClass('blurOnlyControls');
                    break;
                case 'Blog':
                    $("#divBLTabContent").removeClass('blurOnlyControls');
                    break;
                case 'Forum':
                    $("#divFOTabContent").removeClass('blurOnlyControls');
                    break;
                case 'PQ':
                    $("#divProQuestTabContent").removeClass('blurOnlyControls');
                    break;
                case 'LN':
                    $("#divLNTabContent").removeClass('blurOnlyControls');
                    break;
            }
        });
    }
    else {
        $("div[name='advSearchTabContent']").removeClass('blurOnlyControls');
    }

    if (_SearchDate)
        $('#divActiveFilter').append('<div class=\"filter-in\" id=\"divMainDateFilter\">' + _SearchDate + '<span onclick="RemoveDateFilter();" class="cancel"></span></div>');
        
    if (_SearchTVMarket)
        $('#divActiveFilter').append('<div class=\"filter-in\">' + _SearchTVMarket + '<span onclick="RemoveTVMarketFilter();" class="cancel"></span></div>');


    if ($('#divActiveFilter').html()) {
        $('#divActiveFilter').addClass("bottomBorderColor");
    }

    if (_ChartDate) {
        //      alert('main date wil be hide');
        $('#divMainDateFilter').hide();
    }
    else {
        //        alert('main date wil be shown');
        $('#divMainDateFilter').show();
    }
}

function SetNoDataAvailableMessage(message, divID) {
    var noDataHTML = '<div class="alert" id="divNoData">'
                     + '<div id="divNotAvailableDataMessage" class="row-fluid filter margin0">' + message
                     + '</div>'
                 + '</div>';
    $('#' + divID).append(noDataHTML);
}

function ShowPieChart(elementIndex) {
    $('#divPieChartHeader div').each(function () {
        $(this).removeAttr("class");
    });

    $('#divPieChartData > div').each(function () {
        $(this).hide();
    });

    $('#divPieChartHeader').children().eq(elementIndex).attr("class", "pieChartActive");
    $('#divPieChartData').children().eq(elementIndex).css("visibility", "");
    $('#divPieChartData').children().eq(elementIndex).css("height", "");
    $('#divPieChartData').children().eq(elementIndex).show();
}

function RenderColumnChartHighCharts(jsonChartData) {
    $('#divColumnChart').highcharts(JSON.parse(jsonChartData));
}

function RenderLineHighChart(jsonLineChartData) {

    var JsonLineChart = JSON.parse(jsonLineChartData);
    JsonLineChart.plotOptions.series.point.events.click = ChartClick;
    JsonLineChart.plotOptions.series.events.legendItemClick = ShowHideChartSeries;
    $('#divLineChart').highcharts(JsonLineChart);
}

function RenderSearchTermTabs(jsonPieChartMediumData, jsonPieChartSearchTermData, jsonLineChartMediumData) {
    _PieChartDisplayedTotals = new Array();

    // Search Term-Specific Pie Charts
    var chartCount = 0;
    var pieChartHeaderDivHTML = '';
    var pieChartChartDivHTML = '';

    //$('#divPieChart').html('');
    $('#divPieChartHeader').html('');
    $('#divPieChartData').html('');
    //FusionCharts.setCurrentRenderer('javascript');

    $.each(jsonPieChartMediumData, function (eventID, eventData) {
        _PieChartDisplayedTotals[chartCount] = 0;
        var searchTermTotals = $.grep(_PieChartIndividualTotals, function (element, index) {
            return element.searchTerm == eventData.SearchTerm;
        });
        $.each(searchTermTotals, function () {
            _PieChartDisplayedTotals[chartCount] += this.totalResult;
        });

        if (chartCount == 0) {
            pieChartHeaderDivHTML = '<div id=\'divPieChart_' + chartCount + '\' align="center" style="float: left; padding: 5px;cursor:pointer;overflow: hidden;padding: 5px;text-overflow: ellipsis;white-space: nowrap;width: 20%" class="pieChartActive" >' + EscapeHTML(eventData.SearchName) + '</div>';
        }
        else {
            pieChartHeaderDivHTML = '<div id=\'divPieChart_' + chartCount + '\' align="center" style="float: left; padding: 5px;cursor:pointer;overflow: hidden;padding: 5px;text-overflow: ellipsis;white-space: nowrap;width: 20%" >' + EscapeHTML(eventData.SearchName) + '</div>';
        }

        var topResultsTabHeaders = '<div id="divTopResultsHeader_' + chartCount + '"><div id="divTopResultsTab_' + chartCount + '" class="topResultsTab topResultsActive" onclick="ShowHideTopResults(' + chartCount + ', true);" >Most Relevant Coverage</div>';
        topResultsTabHeaders += '<div id="divTopicsTab_' + chartCount + '" class="topResultsTab" onclick="ShowHideTopResults(' + chartCount + ',false);">Most Relevant Topics</div></div>';
        var topResultsTabData = '<div class="topResultsContainer"><div id="divTopResultsData_' + chartCount + '"></div><div id="divTopicsData_' + chartCount + '" class="displayNone" style="width:100%;"></div></div>';

        pieChartChartDivHTML = '<div id=\'divPieChart_Child_' + chartCount + '\'><div style=\'padding-bottom:30px;\' id=\'divLineChart_Child_Data_' + chartCount + '\'></div><div  class="float-left" id=\'divPieChart_Child_Data_' + chartCount + '\'></div><div class=\'margintop10 float-left divtopres wordWrap\' id=\'divPieChart_Child_TopResult_' + chartCount + '\'>' + topResultsTabHeaders + topResultsTabData + '</div></div>';

        $('#divPieChartHeader').append(pieChartHeaderDivHTML);
        $('#divPieChartHeader').show();

        $('#divPieChartData').append(pieChartChartDivHTML);
        $('#divPieChartData').show();

        $("#divPieChart_Child_Data_" + chartCount).highcharts(JSON.parse(eventData.JsonResult));
        $('#divTopResultsData_' + chartCount).html(eventData.TopResultHtml);

        SetPieChartInnerText("divPieChart_Child_Data_" + chartCount, "divPieChartText", "totalHits" + chartCount, _PieChartDisplayedTotals[chartCount]);

        $($("#divPieChart_Child_Data_" + chartCount).highcharts().series[0].data).each(function (i, e) {
            e.id = chartCount;
            e.legendItem.on("click", function () {
                if (e.sliced) {
                    _PieChartDisplayedTotals[e.id] += e.y;
                }
                else {
                    _PieChartDisplayedTotals[e.id] -= e.y;
                }
                $("#totalHits" + e.id).remove();
                SetPieChartInnerText("divPieChart_Child_Data_" + e.id, "divPieChartText", "totalHits" + e.id, _PieChartDisplayedTotals[e.id]);
                e.slice(!e.sliced);

                // Show/hide the corresponding line chart series
                var lineSeries = $.grep($("#divLineChart_Child_Data_" + e.id).highcharts().series, function (element, index) {
                    return element.name == e.name;
                })[0];

                if (lineSeries.visible) {
                    lineSeries.hide();
                }
                else {
                    lineSeries.show();
                }
            });
        });

        // Render medium-specific line chart
        var lineChartMedium = $.grep(jsonLineChartMediumData, function (element, index) {
            return element.SearchTerm == eventData.SearchTerm;
        })[0];

        var jsonLineChartMedium = JSON.parse(lineChartMedium.JsonResult);
        jsonLineChartMedium.plotOptions.series.point.events.click = MediumChartClick;
        jsonLineChartMedium.plotOptions.series.events.legendItemClick = function (i, e) {
            // Show/hide the corresponding pie chart slice
            var seriesName = this.name;
            var pieChartData = $.grep($("#divPieChart_Child_Data_" + this.id).highcharts().series[0].data, function (element, index) {
                return element.name == seriesName;
            })[0];

            if (pieChartData.sliced) {
                _PieChartDisplayedTotals[this.id] += pieChartData.y;
            }
            else {
                _PieChartDisplayedTotals[this.id] -= pieChartData.y;
            }
            $("#totalHits" + pieChartData.id).remove();
            SetPieChartInnerText("divPieChart_Child_Data_" + this.id, "divPieChartText", "totalHits" + this.id, _PieChartDisplayedTotals[this.id]);

            pieChartData.slice(!pieChartData.sliced);
            pieChartData.setVisible(!pieChartData.visible);
        };

        $("#divLineChart_Child_Data_" + chartCount).highcharts(jsonLineChartMedium);
        $($("#divLineChart_Child_Data_" + chartCount).highcharts().series).each(function (i, e) {
            // Set the chart count here so that it can be accessed in the legendItemClick function above
            e.id = chartCount;
        });

        chartCount = chartCount + 1;
    });

    ShowPieChartHighCharts(0);

    // Overall Pie Charts
    if (_searchTerm.length > 1) {
        $('#divPieChartSearchTerm').show();

        var index = _searchTerm.length;
        _PieChartDisplayedTotals[index] = 0;
        $.each(_PieChartIndividualTotals, function () {
            _PieChartDisplayedTotals[index] += this.totalResult;
        });
        _PieChartDisplayedTotals[index + 1] = _PieChartDisplayedTotals[index];

        $("#divPieChartStatic").highcharts(JSON.parse(jsonPieChartSearchTermData));
        $("#divPieChartStatic").highcharts().setTitle({ text: "Share of Voice" });
        SetPieChartInnerText("divPieChartStatic", "divPieChartText", "totalHitsStatic", _PieChartDisplayedTotals[index]);

        $("#divPieChartStaticPDF").highcharts(JSON.parse(jsonPieChartSearchTermData));
        $("#divPieChartStaticPDF").highcharts().setTitle({ text: "Share of Voice" });

        var pieChartChks = $('input:checkbox[name=chkPieChartMedium]');

        if (_SearchMediums == null || _SearchMediums.length == 0) {
            $('input:checkbox[name=chkPieChartMedium]').prop('checked', false);
            $('input:checkbox[name=chkPieChartMedium]').filter('[value=TV]').prop('checked', true);
            $('input:checkbox[name=chkPieChartMedium]').filter('[value=NM]').prop('checked', true);
            pieChartChks.removeAttr('disabled');
        }
        else {
            $('input:checkbox[name=chkPieChartMedium]').prop('checked', false);

            $.each(_SearchMediums, function (eventID, eventData) {
                pieChartChks.filter('[value=' + eventData + ']').prop('checked', true);
                pieChartChks.prop('disabled', 'disabled');
            });
        }

        $("#divPieChartDynamic").highcharts(JSON.parse(jsonPieChartSearchTermData));
        $("#divPieChartDynamic").highcharts().setTitle({ text: "Share of Voice (Medium)" });

        $("#divPieChartDynamicPDF").highcharts(JSON.parse(jsonPieChartSearchTermData));
        $("#divPieChartDynamicPDF").highcharts().setTitle({ text: "Share of Voice (Medium)" });

        var option = $("#divPieChartStaticPDF").highcharts().series[0].options;
        option.dataLabels.enabled = true;
        option.dataLabels.distance = 15;
        option.dataLabels.formatter = function () { return Math.round(this.percentage * 100) / 100 + ' %'; };
        $("#divPieChartStaticPDF").highcharts().series[0].update(option);
        $("#divPieChartDynamicPDF").highcharts().series[0].update(option);

        // Initialize dynamic pie charts with total data, then update based on selected mediums
        ShowHidePieChartMedium();
    }
    else {
        $('#divPieChartSearchTerm').hide();
    }
}

function ShowPieChartHighCharts(elementIndex) {
    $('#divPieChartHeader div').each(function () {
        $(this).removeAttr("class");
    });

    $('#divPieChartData > div').each(function () {
        $(this).hide();
    });

    $('#divPieChartHeader').children().eq(elementIndex).attr("class", "pieChartActive");
    $('#divPieChartData').children().eq(elementIndex).css("visibility", "");
    $('#divPieChartData').children().eq(elementIndex).css("height", "");
    $('#divPieChartData').children().eq(elementIndex).show();
}

function SetDateFilter(result) {
    disabledDays = [];
    $.each(result.discoveryDateFilter, function (eventID, eventData) {
        disabledDays.push(eventData.Date);
    });
}

function SetMediumFilter(result) {
    $('#ulMedium').html('');
    var discoveryMedium = '';
    $.each(result.discoveryMediumFilter, function (eventID, eventData) {
        var mediaTypeClass = eventData.SubMediaTypeFilterData != null && eventData.SubMediaTypeFilterData.length > 0 ? ' class="dropdown-submenu"' : "";
        discoveryMedium += '<li onclick="SetMediaType(event, \'' + eventData.MediaType + '\');"' + mediaTypeClass + '><a data-toggle="dropdown" class="dropdown-toggle" href="#" role="button" name="aMediaType">' + eventData.MediaTypeName + '</a>';
        if (eventData.SubMediaTypeFilterData != null && eventData.SubMediaTypeFilterData.length > 0) {
            discoveryMedium += '<ul aris-labelledby="drop2" role="menu" class="dropdown-menu sideMenu" id="ulSubMedium">'
            $.each(eventData.SubMediaTypeFilterData, function (eventID2, eventData2) {
                discoveryMedium += '<li onclick="SetSubMediaType(\'' + eventData2.SubMediaType + '\');" role="presentation"><a href="#" tabindex="-1" role="menuitem">' + eventData2.SubMediaTypeName + '</a></li>';

            });
            discoveryMedium += '</ul>';
        }
        discoveryMedium += '</li>';
    });

    if (discoveryMedium != '') {
        $('#ulMedium').html(discoveryMedium);
    }
    else {
        $('#ulMedium').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
    }
}

function enableAllTheseDays(date) {
    date = $.datepicker.formatDate('mm/dd/yy', date);
    return [$.inArray(date, disabledDays) !== -1];
}

function GetChartData(e) {
    if (e.keyCode == 13) {
        $('#' + $(e.target).attr("id")).blur();
    }
}

function RemoveSearchTerm(divID) {
    var textBoxLiID = divID.replace('divFilterSearchTerm_', 'lisearchTerm_');

    $('#' + textBoxLiID).remove();
    PushSearchTermintoArray();
}

function RemoveDateFilter() {
    _SearchDate = '';

    var _tDate = new Date();
    var _fDate = new Date(_tDate.getFullYear(), _tDate.getMonth() - 3, _tDate.getDate());

    $("#dpFrom").datepicker("setDate", _fDate);
    $("#dpTo").datepicker("setDate", _tDate);

    _fromDate = $('#dpFrom').val();
    _toDate = $('#dpTo').val();

    IsChartUpdated = false;
    _IsToggle = false;
    ResetSearchTermClassToFalse();
    SearchResult();
}

function RemoveMediumFilter(subMediaType) {
    if (_SearchMediums.length > 1) {
        _SearchMediums.splice($.inArray(subMediaType, _SearchMediums), 1);
    }
    else {
        _SearchMediums = [];
    }

    IsChartUpdated = false;
    _IsToggle = false;
    ResetSearchTermClassToFalse();
    SearchResult();
}

function RemoveTVMarketFilter() {
    _SearchTVMarket = '';

    IsChartUpdated = false;
    _IsToggle = false;
    ResetSearchTermClassToFalse();
    SearchResult();
}

function RemoveAdvanceSearchFilter() {
    _IsActiveAdvanceSearch = false;
    _UseAdvanceSearchDefault = true;
    AdvancedSearchList = new Object();
    IsChartUpdated = false;
    _IsToggle = false;
    ResetSearchTermClassToFalse();
    SearchResult();
}

function ToggleChartResult() {
    if (IsChartActive == 0) {
        IsChartActive = 1;

        $('#divChartMain').removeAttr('class');
        $('#divResultMain').attr('class', 'heightWidth0');
    }
    else {
        IsChartActive = 0;

        $('#divChartMain').attr('class', 'heightWidth0');
        $('#divResultMain').removeAttr('class');

        for (var i = 0; i < _searchTerm.length; i++) {
            if (_searchTerm[i].SearchTerm.trim() == _SearchTermResult && _searchTerm[i].ResultShown == false) {
                _searchTerm[i].IsCurrentTab = true;
                SearchResult();
            }
        }
    }
}

function GenerateSearchTermTab() {
    $('#divResultHeader').html('');
    $('#divResult').html('');
    for (var i = 0; i < _searchTerm.length; i++) {
        if (_searchTerm[i].SearchTerm.trim()) {
            if (getParameterByName("SearchTermTopic") == '' && getParameterByName("SearchNameTopic") == '') {
                if (_searchTerm[i].SearchTerm.trim() == _SearchTermResult) {
                    $('#divResultHeader').append('<div id=\'divResult_' + i + '\' align="center" style="float: left; padding: 5px;cursor:pointer;overflow: hidden;padding: 5px;text-overflow: ellipsis;white-space: nowrap;width: 20%;" class="pieChartActive" >' + EscapeHTML(_searchTerm[i].SearchName.trim()) + '</div>');
                }
                else {
                    $('#divResultHeader').append('<div id=\'divResult_' + i + '\'  align="center" style="float: left; padding: 5px;cursor:pointer;overflow: hidden;padding: 5px;text-overflow: ellipsis;white-space: nowrap;width: 20%;">' + EscapeHTML(_searchTerm[i].SearchName.trim()) + '</div>');
                }
            }
            else {
                $('#divResultHeader').attr("class", "margintop5 marginbottom5 discoveryPopupTabParent");
            }
            $('#divResult').append('<div id=\'divResult_Child_' + i + '\'><div id=\'divResult_Child_NoData_' + i + '\'></div><span class="resultTotal float-right" id="spnNoOfRecords_' + i + '"></span><div class="clear" id="divResult_Child_Scroll_' + i + '"><div class="posts" id="divResult_Child_Data_' + i + '"></div></div></div>');
        }
    }
}

function GetDataOnTabChange() {
    $('#divResultHeader div').each(function () {
        $(this).removeAttr("class");
    });

    $('#divResult > div').each(function () {
        $(this).hide();
    });

    _SearchTermResult = _searchTerm[_SearchTermIndex].SearchTerm.trim();

    if (!_searchTerm[_SearchTermIndex].ResultShown) {

        IsChartUpdated = true;
        _IsTabChange = true;
        SearchResult();
    }

    $('#divResultHeader').children().eq(_SearchTermIndex).attr("class", "pieChartActive");
    $('#divResult').children().eq(_SearchTermIndex).show();
}

function ResetSearchTermClassToFalse() {
    for (var zz = 0; zz < _searchTerm.length; zz++) {
        _searchTerm[zz].ResultShown = false;
    }
}

function RefreshResult() {
    PushSearchTermintoArray();
}

function ClearFilterVariable() {
    _SearchMediums = [];
    _SearchDate = '';

    _Searchtv = '';
    _SearchTermIndex = 0;

    _searchTerm = new Array();
    searchTermCount = 0;
}

function setDateVariableOnClick(selector) {
    if (_MaxDiscoveryHistory != -1) {
        var todaysDate = new Date();
        $(selector).datepicker("option", "minDate", new Date(todaysDate.setMonth(todaysDate.getMonth() - _MaxDiscoveryHistory)));
       } 
}

function SetDateVariable() {
    _ChartDate = '';

    if ($("#dpFrom").val() && $("#dpTo").val()) {

        if (_fromDate != $("#dpFrom").val() || _toDate != $("#dpTo").val()) {
            _fromDate = $("#dpFrom").val();
            _toDate = $("#dpTo").val();
            _IsToggle = false;
            SearchResult();
            $('#ulCalender').parent().removeClass('open');
        }
    }
    else
        if ($("#dpFrom").val() != "" && $("#dpTo").val() == "") {
            $("#dpTo").addClass("warningInput");
        }
        else if ($("#dpTo").val() != "" && $("#dpFrom").val() == "") {
            $("#dpFrom").addClass("warningInput");
        }
}

function DateValidation() {
    $('#dpFrom').removeClass('warningInput');
    $('#dpTo').removeClass('warningInput');

    // if both empty
    if (($('#dpTo').val() == '') && ($('#dpFrom').val() == '')) {
        return true;

    }
    //if one empty not other
    else if (($('#dpFrom').val() != '') && ($('#dpTo').val() == '')
                    ||
                    ($('#dpFrom').val() == '') && ($('#dpTo').val() != '')
                    ) {
        if ($('#dpFrom').val() == '') {

            ShowNotification(_msgFromDateNotSelected);
            $('#dpFrom').addClass('warningInput');
        }

        if ($('#dpTo').val() == '') {

            ShowNotification(_msgToDateNotSelected);
            $('#dpTo').addClass('warningInput');

        }
        return false;
    }
    //if both not empty
    else {
        var isFromDateValid = isValidDate($('#dpFrom').val().toString());
        var isToDateValid = isValidDate($('#dpTo').val().toString());
        if (isFromDateValid && isToDateValid) {
            var fromDate = new Date($('#dpFrom').val());
            var toDate = new Date($('#dpTo').val());
            if (fromDate > toDate) {
                ShowNotification(_msgFromDateLessThanToDate);
                $('#dpFrom').addClass('warningInput');
                $('#dpTo').addClass('warningInput');
                return false;
            }
            else {
                return true;

            }
        }
        else {
            if (!isFromDateValid) {
                $('#dpFrom').addClass('warningInput');
            }
            if (!isToDateValid) {
                $('#dpTo').addClass('warningInput');
            }
            ShowNotification(_msgInvalidDate);
            return false;
        }
    }
}

function isValidDate(s) {
    var bits = s.split('/');
    var y = bits[2], m = bits[0], d = bits[1];

    // Assume not leap year by default (note zero index for Jan) 
    var daysInMonth = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

    // If evenly divisible by 4 and not evenly divisible by 100, 
    // or is evenly divisible by 400, then a leap year 
    if ((!(y % 4) && y % 100) || !(y % 400)) {
        daysInMonth[1] = 29;
    }

    return d <= daysInMonth[--m]
}

function ShowDateRangeExpansion() {
    _DateMessage = '';
    if (_SearchMediums.length == 0 || $.inArray('Blog', _SearchMediums) > -1) {
        var smDate = new Date(_constSMContentMinDate);

        if (new Date(_fromDate) < smDate) {
            if (_DateMessage) {
                _DateMessage = _DateMessage + '<br/>';
            }
            _DateMessage = _DateMessage + 'Blog content is not available prior to July 1st, 2012, would you like to continue with your search?';
        }
    }
    if (_SearchMediums.length == 0 || $.inArray('Forum', _SearchMediums) > -1) {
        var smDate = new Date(_constSMContentMinDate);

        if (new Date(_fromDate) < smDate) {
            if (_DateMessage) {
                _DateMessage = _DateMessage + '<br/>';
            }
            _DateMessage = _DateMessage + 'Forum content is not available prior to July 1st, 2012, would you like to continue with your search?';
        }
    }

    if (_SearchMediums.length == 0 || $.inArray('NM', _SearchMediums) > -1) {
        var nmDate = new Date(_constNMContentMinDate);

        if (new Date(_fromDate) < nmDate) {
            if (_DateMessage) {
                _DateMessage = _DateMessage + '<br/>';
            }
            _DateMessage = _DateMessage + 'Online News content is not available prior to July 1st, 2012, would you like to continue with your search?';
        }
    }

    if (_SearchMediums.length == 0 || $.inArray('PQ', _SearchMediums) > -1) {
        var nmDate = new Date(_constPQContentMinDate);

        if (new Date(_fromDate) < nmDate) {
            if (_DateMessage) {
                _DateMessage = _DateMessage + '<br/>';
            }
            _DateMessage = _DateMessage + 'ProQuest content is not available prior to January 1st, 2013, would you like to continue with your search?';
        }
    }

    if (_SearchMediums.length == 0 || $.inArray('LN', _SearchMediums) > -1) {
        var nmDate = new Date(_constLNContentMinDate);

        if (new Date(_fromDate) < nmDate) {
            if (_DateMessage) {
                _DateMessage = _DateMessage + '<br/>';
            }
            _DateMessage = _DateMessage + 'LexisNexis content is not available prior to September 1st, 2015, would you like to continue with your search?';
        }
    }

    if (_DateMessage) {
        return false;
    }
    else {
        return true;
    }
}

function GenerateDashboardPDF() {
    var mySearchTermArray = new Array();
    for (var zz = 0; zz < _searchTerm.length; zz++) {
        mySearchTermArray.push(_searchTerm[zz].SearchTerm.trim());
    }

    ToggleExportStyles(true);

    var jsonPostData = {
        p_HTML: $("#divChartMain").html(),
        p_FromDate: $("#dpFrom").val(),
        p_ToDate: $("#dpTo").val(),
        p_SearchTerm: mySearchTermArray
    }

    ToggleExportStyles(false);

    $.ajax({
        url: _urlDiscoveryGenerateDiscoveryPDF,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess) {
                window.location = _urlDiscoveryDownloadPDFFile;
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

function ToggleExportStyles(isExporting) {
    if (isExporting) {
        // Replace Topics ddl with text
        for (var i = 0; i < _searchTerm.length; i++) {
            $("#spnTopicsMedium_" + i).html("- " + $("#ddlTopicsMedium_" + i + " :selected").text());
        }

        // Replace Share of Voice pie charts with export-compatible versions
        if (_searchTerm.length > 1) {
            $("input:checkbox[name=chkPieChartMedium]:not(:checked)").parent().css({ "text-decoration": "line-through" });
            $("input:checkbox[name=chkPieChartMedium]").hide();
            $("#divPieChartStatic").hide();
            $("#divPieChartDynamic").hide();

            SetPieChartInnerText("divPieChartDynamicPDF", "divPieChartText", "totalHitsDynamicPDF", _PieChartDisplayedTotals[_PieChartDisplayedTotals.length - 1]);
            SetPieChartInnerText("divPieChartStaticPDF", "divPieChartText", "totalHitsStaticPDF", _PieChartDisplayedTotals[_PieChartDisplayedTotals.length - 2]);
        }
    }
    else if (_searchTerm.length > 1) {
        $("input:checkbox[name=chkPieChartMedium]").parent().css({ "text-decoration": "" });
        $("input:checkbox[name=chkPieChartMedium]").show();
        $("#divPieChartStatic").show();
        $("#divPieChartDynamic").show();
        $("#totalHitsStaticPDF").remove();
        $("#totalHitsDynamicPDF").remove();

        $("input[id^=ddlTopicsMedium_]").removeAttr("readonly");
    }

    $("select[id^=ddlTopicsMedium_]").toggleClass("displayNone");
    $("span[id^=spnTopicsMedium_]").toggleClass("displayNone");

    if (_searchTerm.length > 1) {
        $("#divPieChartStaticPDF").toggleClass("hiddenPieChart");
        $("#divPieChartDynamicPDF").toggleClass("hiddenPieChart");
    }
}

function ShowDashboardEmailPopup() {

    $('#txtFromEmail').val($('#hdnDefaultSender').val());

    $("#txtToEmail").val('');
    $("#txtBCCEmail").val('');
    $("#txtSubject").val('');
    $("#txtMessage").val('');
    $('#divEmailPopup').modal({
        backdrop: 'static',
        keyboard: true,
        dynamic: true
    });
}

function CancelEmailpopup() {
    $("#divEmailPopup").css({ "display": "none" });
    $("#divEmailPopup").modal("hide");
}

function ValidateSendEmail() {
    var isValid = true;

    $("#spanFromEmail").html("").hide();
    $("#spanToEmail").html("").hide();
    $("#spanBCCEmail").html("").hide();
    $("#spanSubject").html("").hide();
    $("#spanMessage").html("").hide();

    if ($("#txtFromEmail").val() == "") {
        $("#spanFromEmail").show().html(_msgFromEmailRequired);
        isValid = false;
    }
    if ($("#txtToEmail").val() == "") {
        $("#spanToEmail").show().html(_msgToEmailRequired);
        isValid = false;
    }

    if ($("#txtSubject").val() == "") {
        $("#spanSubject").show().html(_msgSubjectRequired);
        isValid = false;
    }
    if ($("#txtMessage").val() == "") {
        $("#spanMessage").show().html(_msgMessageRequired);
        isValid = false;
    }

    if ($("#txtFromEmail").val() != "" && !CheckEmailAddress($("#txtFromEmail").val())) {
        $("#spanFromEmail").show().html(_msgIncorrectEmail);
        isValid = false;
    }

    if ($("#txtToEmail").val() != "") {
        var Toemail = $("#txtToEmail").val();
        if (Toemail.substr(Toemail.length - 1) == ";") {
            Toemail = Toemail.slice(0, -1);
        }
        $(Toemail.split(';')).each(function (index, value) {
            if (!CheckEmailAddress(value)) {
                $("#spanToEmail").show().html(_msgOneEmailAddressInCorrect);
                isValid = false;
                return;
            }
        });

        if (Toemail.split(';').length > _MaxEmailAdressAllowed) {
            $("#spanToEmail").show().html(_msgMaxEmailAdressLimitExceeds.replace(/@@MaxLimit@@/g, _MaxEmailAdressAllowed));
            isValid = false;
        }
    }

    if ($("#txtBCCEmail").val() != "") {
        if ($("#txtToEmail").val() == "") {
            $("#spanBCCEmail").show().html(_msgBCCEmailMissingTo);
            isValid = false;
        }
        else {
            var BCCemail = $("#txtBCCEmail").val();
            if (BCCemail.substr(BCCemail.length - 1) == ";") {
                BCCemail = BCCemail.slice(0, -1);
            }

            $(BCCemail.split(';')).each(function (index, value) {
                if (!CheckEmailAddress(value)) {
                    $("#spanBCCEmail").show().html(_msgOneEmailAddressInCorrect);
                    isValid = false;
                    return;
                }
            });

            if (BCCemail.split(';').length > _MaxEmailAdressAllowed) {
                $("#spanBCCEmail").show().html(_msgMaxEmailAdressLimitExceeds.replace(/@@MaxLimit@@/g, _MaxEmailAdressAllowed));
                isValid = false;
            }
        }
    }

    return isValid;
}

function CheckEmailAddress(email) {
    var emailPattern = /^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
    return emailPattern.test(email);
}

function SendEmail() {
    var mySearchTermArray = new Array();
    for (var zz = 0; zz < _searchTerm.length; zz++) {
        mySearchTermArray.push(_searchTerm[zz].SearchTerm.trim());
    }
    if (ValidateSendEmail()) {

        ToggleExportStyles(true);

        var jsonPostData = {
            p_HTML: $("#divChartMain").html(),
            p_FromDate: $("#dpFrom").val(),
            p_ToDate: $("#dpTo").val(),
            p_FromEmail: $("#txtFromEmail").val(),
            p_ToEmail: $("#txtToEmail").val(),
            p_BCCEmail: $("#txtBCCEmail").val(),
            p_Subject: $("#txtSubject").val(),
            p_UserBody: $("#txtMessage").val(),
            p_SearchTerm: mySearchTermArray
        }

        ToggleExportStyles(false);

        $.ajax({
            url: _urlDiscoverSendEmail,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: OnEmailSendComplete,
            error: OnEmailSendFail
        });
    }
}

function OnEmailSendComplete(result) {
    CancelEmailpopup();
    if (result.isSuccess) {
        ShowNotification(_msgEmailSent.replace(/@@emailSendCount@@/g, result.emailSendCount));

    }
    else {
        ShowNotification(result.errorMessage);
    }
}

function OnEmailSendFail(result) {

    CancelEmailpopup();
    ShowNotification(_msgErrorOccured);
}

function SearchByChartDate(cDate, sTerm) {
    _fromDate = cDate.substring(0, 10);
    _toDate = cDate.substring(0, 10);
    _SearchTermResult = sTerm;

    _SearchDate = cDate.substring(0, 10);
    $("#dpFrom").datepicker("setDate", _SearchDate);
    $("#dpTo").datepicker("setDate", _SearchDate);

    _IsToggle = false;
    IsChartActive = 0;
    $('#divChartMain').attr('class', 'heightWidth0');
    $('#divResultMain').removeAttr('class');
    PushSearchTermintoArray();
}

function RemoveChartDate() {
    IsChartUpdated = false;
    _IsToggle = false;
    ResetSearchTermClassToFalse();
    SearchResult();
}

function ClearChartDateVariable() {
    _ChartDate = '';

    _fromDate = $('#dpFrom').val();
    _toDate = $('#dpTo').val();
}

function InsertDiscoveryReport() {
    if (ValidateReportInputs()) {
        $('#imgCreateReportLoading').show();
        var mediaID = new Array();
        $("#divResult input[type=checkbox]").each(function () {
            if ($(this).is(':checked')) {
                var tempIDValue = $(this).val().trim().split(',');

                var mediaIDClass = new Object();
                mediaIDClass.MediaID = tempIDValue[0];
                mediaIDClass.SubMediaType = tempIDValue[1];

                var _MediaSearchIndex = eval($(this).parents('[id^="divResult_Child_Data_"]').attr('id').replace('divResult_Child_Data_', ''));
                mediaIDClass.SearchTerm = _searchTerm[_MediaSearchIndex].SearchTerm;
                mediaID.push(mediaIDClass);
            }
        });

        var jsonPostData = {
            p_Title: $('#txtReportTitle').val(),
            p_Keywords: $('#txtReportKeywords').val(),
            p_Description: $('#txtReportDescription').val(),
            p_CategoryGuid: $('#ddlReportCategory').val(),
            p_MediaID: mediaID,
            p_ReportImage: $("#ddlReportImage").val(),
            p_FolderID: $("#ddlReportFolder").val()
        }

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlDiscoverInsert_DiscoveryReport,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            global: false,
            success: OnInsertDiscoveryReportSuccess,
            error: OnInsertDiscoveryReportFail
        });
    }
}

function OnInsertDiscoveryReportSuccess(result) {
    $('#imgCreateReportLoading').hide();
    if (result.isSuccess) {
        GetDiscoveryReport();
        ClosePopUp('divReportPopup');
        ShowNotification(result.message);
        ClearCheckboxSelection();
    }
    else {
        ClosePopUp('divReportPopup');
        ShowNotification(_msgErrorOccured);
    }
}

function OnInsertDiscoveryReportFail(result) {
    $('#imgCreateReportLoading').hide();
    ShowNotification(_msgErrorOccured);
}

function ValidateReportInputs() {
    var isValid = true;
    $("#txtReportTitle").removeClass('warningInput');
    $("#txtReportKeywords").removeClass('warningInput');
    $("#txtReportDescription").removeClass('warningInput');
    $("#ddlReportCategory").removeClass('warningInput');
    $("#ddlReportFolder").removeClass('warningInput');

    if (!$('#txtReportTitle').val()) {
        isValid = false;
        $("#txtReportTitle").addClass('warningInput');
    }

    if (!$('#txtReportKeywords').val()) {
        isValid = false;
        $("#txtReportKeywords").addClass('warningInput');
    }

    if (!$('#txtReportDescription').val()) {
        isValid = false;
        $("#txtReportDescription").addClass('warningInput');
    }

    if ($('#ddlReportCategory').val() <= 0) {
        isValid = false;
        $("#ddlReportCategory").addClass('warningInput');
    }

    if ($("#ddlReportFolder").val() == "0") {
        isValid = false;
        $("#ddlReportFolder").addClass('warningInput');
    }

    return isValid;
}


function ShowSaveReportPopup() {
    if (ValidateCheckBoxSelection()) {
        var checkedChecboxCount = 0;
        $("#divResult input[type=checkbox]").each(function () {
            if ($(this).is(':checked')) {
                checkedChecboxCount++;
            }
        });

        $('#txtReportTitle').val('');
        $('#txtReportKeywords').val('');
        $('#txtReportDescription').val('');
        $('#ddlReportCategory').val(0);
        $("#spanReportFolder").html("").hide();
        $('#ddlReportFolder option').filter(function () { return $(this).html() == rootFolderName; }).prop("selected", "selected");
        $('#ddlReportImage').val('');

        $("#txtReportTitle").removeClass('warningInput');
        $("#txtReportKeywords").removeClass('warningInput');
        $("#txtReportDescription").removeClass('warningInput');
        $("#ddlReportCategory").removeClass('warningInput');
        $("#ddlReportFolder").removeClass('warningInput');

        if (_MaxDiscoveryReportItems > 0) {
            if (checkedChecboxCount > _MaxDiscoveryReportItems) {
                getConfirm("Max Limit Exceeded", _DiscoveryMaxDiscoveryReportItemsMessage.replace(/@@MaxDiscoveryReportItems@@/g, _MaxDiscoveryReportItems), "Confirm", "Cancel", function (res) {
                    if (res) {
                        $('#divReportPopup').modal({
                            backdrop: 'static',
                            keyboard: true,
                            dynamic: true
                        });
                    }
                });
            }
            else {
                $('#divReportPopup').modal({
                    backdrop: 'static',
                    keyboard: true,
                    dynamic: true
                });
            }
        }
        else {
            getConfirm("Max Limit Exceeded", _CommonLimitExceedMessage, "Confirm", "Cancel", function (res) {
                if (res) {
                    $('#divReportPopup').modal({
                        backdrop: 'static',
                        keyboard: true,
                        dynamic: true
                    });
                }
            });
        }
    }
    else {
        ShowNotification(_msgAtleastOneRecordSelect);
    }
}

function ValidateCheckBoxSelection() {
    var isChecked = false;
    $("#divResult input[type=checkbox]").each(function () {
        if (this.checked) {
            isChecked = true;
        }
    });
    return isChecked;
}

function ClosePopUp(divID) {
    $('#' + divID).css({ "display": "none" });
    $('#' + divID).unbind().modal();
    $('#' + divID).modal('hide');
}

function ShowAddToReportPopup() {
    if (ValidateCheckBoxSelection()) {
        $('#ddlReportTitle').val(0);
        $('#ddlReportTitle').removeClass('warningInput');
        $('#divAddToReportPopup').modal({
            backdrop: 'static',
            keyboard: true,
            dynamic: true
        });

        GetDiscoveryReport();
    }
    else {
        ShowNotification(_msgAtleastOneRecordSelect);
    }
}

function GetDiscoveryReport() {
    $("#divAddtoReport").addClass("blurOnlyControls");
    $("#divAddtoReportMsg").html("Please Wait...");
    $.ajax({

        type: 'POST',
        dataType: 'json',
        url: _urlDiscoverySelectDiscoveryReport,
        contentType: 'application/json; charset=utf-8',
        global: false,

        success: OnSelectDiscoveryReportComplete,
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured, a);
        }
    });
}



function OnSelectDiscoveryReportComplete(result) {
    if (result.isSuccess) {
        $("#divAddtoReport").removeClass("blurOnlyControls");
        $("#divAddtoReportMsg").html("");

        var reportOptions = '<option value="0">Select Report</option>';

        if (result.reportList != null) {
            $.each(result.reportList, function (eventID, eventData) {
                var status = eventData.Status.toLowerCase();
                var reportTitle = EscapeHTML(eventData.Title);
                if (status == "completed" || status == "queued") {
                    reportOptions = reportOptions + '<option value="' + eventData.ID + '">' + reportTitle + '</option>';
                }
                else if (status == "exception" || status == "failed") {
                    reportOptions = reportOptions + '<option value="' + eventData.ID + '" status="failed" style="color:#aaaaaa;">' + reportTitle + ' (Failed)</option>';
                }
                else {
                    reportOptions = reportOptions + '<option value="' + eventData.ID + '" status="processing" style="color:#aaaaaa;">' + reportTitle + ' (Processing)</option>';
                }
            });
        }

        $('#ddlReportTitle').find('option').remove().end().append(reportOptions);
    }
    else {
        if (typeof result.isAuthorized != 'undefined' && !result.isAuthorized) {
            RedirectToUrl(result.redirectURL);
        }
        else {
            ShowNotification(_msgErrorOccured);
        }
    }
}

function OnSelectDiscoveryReportFail(result) {
    ShowNotification(_msgErrorOccured);
}

function AddToDiscoveryReport() {
    if ($('#ddlReportTitle').val() > 0) {
        var selectedReport = $('#ddlReportTitle option:selected');
        if (selectedReport.attr("status") == "processing") {
            ShowNotification("Report still processing. Please try again later.");
            GetDiscoveryReport();
        }
        else if (selectedReport.attr("status") == "failed") {
            ShowNotification("Report generation failed. View the Job Status page to retry.");
            GetDiscoveryReport();
        }
        else {
            $('#ddlReportTitle').removeClass('warningInput');
            $('#imgAddToReportLoading').show();
            var mediaID = new Array();
            $("#divResult input[type=checkbox]").each(function () {
                if ($(this).is(':checked')) {

                    var tempIDValue = $(this).val().trim().split(',');

                    var mediaIDClass = new Object();
                    mediaIDClass.MediaID = tempIDValue[0];
                    mediaIDClass.SubMediaType = tempIDValue[1];

                    var _MediaSearchIndex = eval($(this).parents('[id^="divResult_Child_Data_"]').attr('id').replace('divResult_Child_Data_', ''));
                    mediaIDClass.SearchTerm = _searchTerm[_MediaSearchIndex].SearchTerm;
                    mediaID.push(mediaIDClass);
                }
            });

            var jsonPostData = {
                p_ReportID: $('#ddlReportTitle').val(),
                p_RecordList: mediaID
            }

            $.ajax({

                type: 'POST',
                dataType: 'json',
                url: _urlDiscoveryAddToDiscoveryReport,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(jsonPostData),
                global: false,
                success: OnAddToDiscoveryReportSuccess,
                error: OnAddToDiscoveryReportFail
            });
        }
    }
    else {
        $('#ddlReportTitle').addClass('warningInput');
        $('#ddlReportTitle').addClass('boxshadow');

        setTimeout(function () {
            $('#ddlReportTitle').removeClass('boxshadow');
        }, 2000);
    }
}

function OnAddToDiscoveryReportSuccess(result) {
    $('#imgAddToReportLoading').hide();
    if (result.isSuccess) {
        ClearCheckboxSelection();
        ClosePopUp('divAddToReportPopup');
        ShowNotification(result.message + " " + _msgRecordAddedToReport);

    }
    else {
        ShowNotification(_msgErrorOccured);
        ClosePopUp('divAddToReportPopup');
    }
}

function OnAddToDiscoveryReportFail(result) {
    $('#imgAddToReportLoading').hide();
    ShowNotification(_msgErrorOccured);
}

function ClearCheckboxSelection() {
    $("#chkInputAll").removeAttr("checked")
    $("#divResult input[type=checkbox]").each(function () {
        this.checked = false;
        $(this).closest('.media').css('background', '');
    });
}

function GetDiscoveryReportLimit() {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlDiscoveryGetDiscoveryReportLimit,
        contentType: 'application/json; charset=utf-8',

        global: false,
        success: OnGetDiscoveryReportLimitSuccess,
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured, a);
        }
    });
}

function OnGetDiscoveryReportLimitSuccess(result) {
    if (result.isSuccess) {
        _MaxDiscoveryReportItems = result.MaxDiscoveryReportItems;
        _MaxDiscoveryExportItems = result.MaxDiscoveryExportItems;
        _MaxDiscoveryHistory = result.MaxDiscoveryHistory;
    }
}

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function checkUncheckAll(divID, mainCheckBox) {
    var checkBoxValue = false;
    checkBoxValue = $("#" + mainCheckBox).is(":checked");

    $("#" + divID + " div.media:not(.displayNone) input[type=checkbox]").each(function () {
        this.checked = checkBoxValue;
        if (checkBoxValue) {
            $(this).closest('.media').css('background', '#F4F4F4');
        }
        else {
            $(this).closest('.media').css('background', '');
        }
    });
}

function CheckUncheckMasterCheckBox(checkbox, divID, mainCheckBox) {
    if (!$(checkbox).is(":checked")) {
        $("#" + mainCheckBox).prop("checked", false);
        $(checkbox).closest('.media').css('background', '');
    }
    else {
        var isChecked = true;
        $(checkbox).closest('.media').css('background', '#F4F4F4');

        for (var i = 0; i < _searchTerm.length; i++) {

            $("#divResult_Child_" + i + " input[type=checkbox]").slice(0, _searchTerm[i].ShownRecords).each(function () {
                if (!this.checked) {
                    isChecked = false;
                }
            });
        }

        if (isChecked == true) {
            $("#" + mainCheckBox).prop("checked", true);
        }
        else {
            $("#" + mainCheckBox).prop("checked", false);
        }
    }
}

function SetDiscoveryResultStatus(result) {
    if (result.isAnyDataAvailable) {
        if (result.searchTermShownRecords <= 0) {

            result.searchTermTotalRecords = 0;
            result.searchTermAvailableRecords = 0;
            result.searchTermTotalRecords = 0;
        }

        var totalRecordHTML = '';
        if (result.searchTermTotalRecords > 0) {
            totalRecordHTML = numberWithCommas(result.searchTermShownRecords) + ' of ' + numberWithCommas(result.searchTermTotalRecords);
        }

        $('#spnNoOfRecords_' + result.searchedIndex).html(totalRecordHTML);
        $('#divResult_Child_Data_' + result.searchedIndex).html(result.HTML);
    }
    else {
        result.searchTermTotalRecords = 0;
        result.searchTermAvailableRecords = 0;
        result.searchTermTotalRecords = 0;
        $('#spnNoOfRecords_' + result.searchedIndex).html('');
    }
}

function ClearSearch() {
    _Searchtv = '';
    _SearchTermIndex = 0;
    _searchTerm = new Array();
    searchTermCount = 0;
    IsChartActive = 1;
    _IsDefaultLoad = true;
    _IsToggle = false;
    AdvancedSearchList = new Object();
    _IsActiveAdvanceSearch = false;
    _UseAdvanceSearchDefault = true;
    $('#ulSearchTerm').html('');

    ClearAllData();

    AddNewSearchTermTextBox();
    removeOriginalSearchTB = true; //set to true because we are adding a default tb

    var _tDate = new Date();
    var _fDate = new Date(_tDate.getFullYear(), _tDate.getMonth() - 3, _tDate.getDate());

    $("#dpFrom").datepicker("setDate", _fDate);
    $("#dpTo").datepicker("setDate", _tDate);

    _fromDate = $('#dpFrom').val();
    _toDate = $('#dpTo').val();

    SearchResultAjaxRequest()
    GetSavedSearch(false, true);
}

function ShowAddToLibraryPopup() {
    if (ValidateCheckBoxSelection()) {

        $('#txtLibraryKeywords').removeClass('warningInput');
        $('#txtLibraryDescription').removeClass('warningInput');
        $('#ddlLibraryCategory').removeClass('warningInput');

        $('#txtLibraryKeywords').val('');
        $('#txtLibraryDescription').val('');
        $('#ddlLibraryCategory').val(0);

        $('#divAddToLibraryPopup').modal({
            backdrop: 'static',
            keyboard: true,
            dynamic: true
        });
    }
    else {
        ShowNotification(_msgAtleastOneRecordSelect);
    }
}

function ValidateLibraryInputs() {
    var isValid = true;

    $('#txtLibraryKeywords').removeClass('warningInput');
    $('#txtLibraryDescription').removeClass('warningInput');
    $('#ddlLibraryCategory').removeClass('warningInput');

    if ($.trim($('#txtLibraryKeywords').val()) == '') {
        isValid = false;
        $('#txtLibraryKeywords').addClass('warningInput');
        $('#txtLibraryKeywords').addClass('boxshadow');

        setTimeout(function () {
            $('#txtLibraryKeywords').removeClass('boxshadow');
        }, 2000);
    }

    if ($.trim($('#txtLibraryDescription').val()) == '') {
        isValid = false;
        $('#txtLibraryDescription').addClass('warningInput');
        $('#txtLibraryDescription').addClass('boxshadow');

        setTimeout(function () {
            $('#txtLibraryDescription').removeClass('boxshadow');
        }, 2000);
    }

    if ($('#ddlLibraryCategory').val() == 0) {
        isValid = false;
        $('#ddlLibraryCategory').addClass('warningInput');
        $('#ddlLibraryCategory').addClass('boxshadow');

        setTimeout(function () {
            $('#ddlLibraryCategory').removeClass('boxshadow');
        }, 2000);
    }

    return isValid;
}

function AddToDiscoveryLibrary() {
    if (ValidateLibraryInputs()) {

        $('#txtLibraryKeywords').removeClass('warningInput');
        $('#txtLibraryDescription').removeClass('warningInput');
        $('#ddlLibraryCategory').removeClass('warningInput');

        $('#imgAddToLibraryLoading').show();
        var mediaID = new Array();
        $("#divResult input[type=checkbox]").each(function () {
            if ($(this).is(':checked')) {
                var tempIDValue = $(this).val().trim().split(',');

                var mediaIDClass = new Object();
                mediaIDClass.MediaID = tempIDValue[0];
                mediaIDClass.SubMediaType = tempIDValue[1];

                var _MediaSearchIndex = eval($(this).parents('[id^="divResult_Child_Data_"]').attr('id').replace('divResult_Child_Data_', ''));
                mediaIDClass.SearchTerm = _searchTerm[_MediaSearchIndex].SearchTerm;
                mediaID.push(mediaIDClass);
            }
        });

        var jsonPostData = {

            p_Keywords: $.trim($('#txtLibraryKeywords').val()),
            p_Description: $.trim($('#txtLibraryDescription').val()),
            p_CategoryGuid: $('#ddlLibraryCategory').val(),
            p_MediaID: mediaID
        }

        $.ajax({

            type: 'POST',
            dataType: 'json',
            url: _urlDiscoveryAddToDiscoveryLibrary,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            global: false,
            success: OnAddToDiscoveryLibrarySuccess,
            error: OnAddToDiscoveryLibraryFail
        });
    }
    else {
        $('#ddlLibraryCategory').addClass('warningInput');
        $('#ddlLibraryCategory').addClass('boxshadow');

        setTimeout(function () {
            $('#ddlLibraryCategory').removeClass('boxshadow');
        }, 2000);
    }
}

function OnAddToDiscoveryLibrarySuccess(result) {
    $('#imgAddToLibraryLoading').hide();
    if (result.isSuccess) {
        ClearCheckboxSelection();
        ClosePopUp('divAddToLibraryPopup');
        ShowNotification(result.message + " " + _msgRecordAddedToLibrary);
    }
    else {
        ShowNotification(_msgErrorOccured);
        ClosePopUp('divAddToReportPopup');
    }
}

function OnAddToDiscoveryLibraryFail(result) {

    $('#imgAddToLibraryLoading').hide();
    ShowNotification(_msgErrorOccured);
}

function ChartClick() {
    _fromDate = this.category;
    _toDate = this.category;
    _SearchTermResult = this.SearchTerm;

    _SearchDate = this.category;
    $("#dpFrom").datepicker("setDate", _SearchDate);
    $("#dpTo").datepicker("setDate", _SearchDate);

    _IsToggle = false;
    IsChartActive = 0;
    $('#divChartMain').attr('class', 'heightWidth0');
    $('#divResultMain').removeAttr('class');
    PushSearchTermintoArray();

}

function MediumChartClick() {
    _fromDate = this.category;
    _toDate = this.category;
    _SearchTermResult = this.SearchTerm;
    _SearchMediums = [this.Type];

    _SearchDate = this.category;
    $("#dpFrom").datepicker("setDate", _SearchDate);
    $("#dpTo").datepicker("setDate", _SearchDate);

    _IsToggle = false;
    IsChartActive = 0;
    $('#divChartMain').attr('class', 'heightWidth0');
    $('#divResultMain').removeAttr('class');
    PushSearchTermintoArray();
}

function ShowHideChartSeries() {
    var seriesIndex = this.index;

    var columnSeries = $('#divColumnChart').highcharts().series[this.index];
    if (columnSeries.visible) {
        columnSeries.hide();
    } else {
        columnSeries.show();
    }

    var dynamicIndex = _PieChartDisplayedTotals.length - 1;
    var staticIndex = dynamicIndex - 1;

    var piePoint = $('#divPieChartStatic').highcharts().series[0].points[seriesIndex];
    if (piePoint.visible) {
        _PieChartDisplayedTotals[staticIndex] -= piePoint.y;
    }
    else {
        _PieChartDisplayedTotals[staticIndex] += piePoint.y;
    }
    $("#totalHitsStatic").remove();
    SetPieChartInnerText("divPieChartStatic", "divPieChartText", "totalHitsStatic", _PieChartDisplayedTotals[staticIndex]);
    piePoint.setVisible(!piePoint.visible);

    piePoint = $('#divPieChartStaticPDF').highcharts().series[0].points[seriesIndex];
    piePoint.setVisible(!piePoint.visible);

    piePoint = $('#divPieChartDynamic').highcharts().series[0].points[seriesIndex];
    if (piePoint.visible) {
        _PieChartDisplayedTotals[dynamicIndex] -= piePoint.y;
    }
    else {
        _PieChartDisplayedTotals[dynamicIndex] += piePoint.y;
    }
    $("#totalHitsDynamic").remove();
    SetPieChartInnerText("divPieChartDynamic", "divPieChartText", "totalHitsDynamic", _PieChartDisplayedTotals[dynamicIndex]);
    piePoint.setVisible(!piePoint.visible);

    piePoint = $('#divPieChartDynamicPDF').highcharts().series[0].points[seriesIndex];
    piePoint.setVisible(!piePoint.visible);
}

function ShowHidePieChartMedium() {
    var enabledMediums = $('input:checkbox[name=chkPieChartMedium]:checked');

    var dataPoints = $("#divPieChartDynamic").highcharts().series[0].points;
    var newDataPoints = new Array(dataPoints.length);
    var dataPointsLine = $("#divLineChart").highcharts().series[0].points;
    var newDataPointsLine = new Array(dataPointsLine.length);
    var index = 0;
    var totalHits = 0;

    $.each(dataPoints, function () {
        var searchTerm = this.name;
        newDataPoints[index] = [searchTerm, 0];

        //get all data points for the search term
        var totalResults = $.grep(_PieChartIndividualTotals, function (element, index) {
            //searchTerm being passed in is the queryName so it must be mapped to the element.searchName not the element.searchTerm 
            return element.searchName == searchTerm;
        });

        //get data to populate pie chart
        //get array of data of the form [['searchTerm','Total'],...] (there should only be one entry per searchTerm)
        $.each(totalResults, function () {
            var chk = enabledMediums.filter('[value=' + this.medium + ']');

            if (chk.length == 1) {
                newDataPoints[index][1] += this.totalResult;
            }
        });
        if (this.visible) {
            totalHits += newDataPoints[index][1];
        }

        //get data to populate line chart
        //get array of data of the form ['Total',...] (there should be an entry for every date)
        var indexLine = 0;
        var unique = {}; //used to get distinct dates
        var disDates = []; //distinct dates
        for (var i in totalResults) {
            if (typeof (unique[totalResults[i].date]) == "undefined") {
                disDates.push(totalResults[i].date);
            }

            unique[totalResults[i].date] = 0;
        }
        $.each(disDates, function () {
            var date = this.toString();
            newDataPointsLine[indexLine] = 0;

            var resultsForDate = $.grep(totalResults, function (element, index) {
                return element.date == date;
            });

            $.each(resultsForDate, function () {
                var chk = enabledMediums.filter('[value=' + this.medium + ']');

                if (chk.length == 1) {
                    newDataPointsLine[indexLine] += this.totalResult;
                }
            });

            indexLine++;
        });
        //populate the line chart
        $("#divLineChart").highcharts().series[index].setData(newDataPointsLine, true);

        //populate column chart using pie chart data 
        $("#divColumnChart").highcharts().series[index].setData([newDataPoints[index][1]], true);

        index++;
    });

    $("#totalHitsDynamic").remove();
    SetPieChartInnerText("divPieChartDynamic", "divPieChartText", "totalHitsDynamic", totalHits);
    _PieChartDisplayedTotals[_PieChartDisplayedTotals.length - 1] = totalHits;

    //populate pie chart
    $("#divPieChartDynamic").highcharts().series[0].setData(newDataPoints, true);
    $("#divPieChartDynamicPDF").highcharts().series[0].setData(newDataPoints, true);
}

function ExportDiscoveryToCSV() {
    if ($("#divResult_Child_Data_" + _SearchTermIndex + " input[type=checkbox]:checked").length > 0) {
        var mediaID = new Array();
        var isSelectAll = false;

        if ($("#chkInputAll").is(":checked")) {
            isSelectAll = true;
        }
        else {
            $("#divResult_Child_Data_" + _SearchTermIndex + " input[type=checkbox]").each(function () {
                if ($(this).is(':checked')) {
                    var tempIDValue = $(this).val().trim().split(',');

                    var mediaIDClass = new Object();
                    mediaIDClass.MediaID = tempIDValue[0];
                    mediaIDClass.SubMediaType = tempIDValue[1];

                    mediaID.push(mediaIDClass);
                }
            });
            isSelectAll = false;
        }

        var mySearchTermArray = new Array();
        for (var zz = 0; zz < _searchTerm.length; zz++) {
            mySearchTermArray.push(_searchTerm[zz].SearchTerm.trim());
        }

        var jsonPostData = {
            p_MediaID: mediaID,
            p_SelectAll: isSelectAll,
            searchTermIndex: _SearchTermIndex,
            searchTermArray: mySearchTermArray,
            fromDate: _fromDate,
            toDate: _toDate,
            subMediaTypes: _SearchMediums,
            advanceSearch: getAdvancedSearchByTermIndex(_SearchTermIndex)
        }

        var confirmTitleExport = '';
        var confirmMsgExport = '';
        if (isSelectAll || (isSelectAll == false && mediaID.length > _MaxDiscoveryExportItems)) {
            if (isSelectAll == false && mediaID.length <= _MaxDiscoveryExportItems) {
                confirmMsgExport = _FeedsDiscoveryExportItemsMessage
                confirmTitleExport = 'Discovery Export Confirmation'
                confirmMsgExport = confirmMsgExport.replace(/@@DiscoveryExportItems@@/g, _MaxDiscoveryExportItems)
            }
            else {
                confirmMsgExport = _FeedsSelectAllMaxDiscoveryExportItemsMessage
                confirmTitleExport = 'Discovery Export Confirmation'
                confirmMsgExport = confirmMsgExport.replace(/@@MaxDiscoveryExportItems@@/g, _MaxDiscoveryExportItems)
            }
            getConfirm(confirmTitleExport, confirmMsgExport, "Confirm", "Cancel", function (res) {
                if (res) {
                    $.ajax({
                        type: 'POST',
                        dataType: 'json',
                        url: _urlDiscoveryExportCSV,
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify(jsonPostData),
                        success: function (result) {
                            if (result.isSuccess) {
                                ClearCheckboxSelection()
                                $('#divJobStatusExportedPopup').modal({
                                    backdrop: 'static',
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
            });
        }
        else {
            confirmTitleExport = 'Discovery Export Confirmation'
            confirmMsgExport = _FeedsDiscoveryExportItemsMessage
            getConfirm(confirmTitleExport, confirmMsgExport.replace(/@@DiscoveryExportItems@@/g, mediaID.length), "Confirm", "Cancel", function (res) {
                if (res) {
                    $.ajax({

                        type: 'POST',
                        dataType: 'json',
                        url: _urlDiscoveryExportCSV,
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify(jsonPostData),
                        success: function (result) {
                            if (result.isSuccess) {
                                ClearCheckboxSelection()
                                $('#divJobStatusExportedPopup').modal({
                                    backdrop: 'static',
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
            });
        }
    }
    else {
        ShowNotification(_msgAtleastOneRecordSelect);
    }
}

function GetExportCSVStatus() {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlDiscoveryGetExportCSVStatus,
        contentType: 'application/json; charset=utf-8',
        success: function (result) {

            $('#imgSaveSearchLoading').hide();
            if (result.isSuccess) {

                var exportHtml = '<table width="100%" class="timeshiftpg">'
                                    + '<tbody>'
                                        + '<tr>'
                                            + '<th style="text-align: center; width: 15%;">'
                                                + 'Search Term'
                                            + '</th>'
                                            + '<th style="text-align: left; width: 15%;">'
                                                + 'Created Date'
                                            + '</th>'
                                            + '<th style="text-align: left; width: 15%;">'
                                                + 'Status'
                                            + '</th>'
                                            + '<th style="text-align: right; width: 15%;">'

                                            + '</th>'
                                        + '</tr>'
                var createddate = "";
                var tr = "";
                var downloadPath = "";

                $.each(result.discoveryExportDetails, function (i, item) {

                    createddate = new Date(parseInt(item.CreatedDate.substr(6)));
                    downloadPath = "";

                    if (item.DownloadPath != '') {
                        downloadPath = '<a href="' + item.DownloadPath + '" target="_blank">Download</a>';
                    }

                    tr = '<tr>'
                                                + '<td>'
                                                    + item.SearchTerm
                                                + '</td>'
                                                + '<td>'
                    + createddate.toLocaleString("en-us")
                                                + '</td>'
                                                + '<td>'
                    + item.Status
                                                + '</td>'
                                                + '<td>'
                                                    + downloadPath
                                                + '</td>'
                                            + '</tr>';

                    exportHtml = exportHtml + tr;
                });

                exportHtml = exportHtml + "</tbody></table>";
                $('#divExportCSVStatusPopup > .modalBody').html(exportHtml);

                $('#divExportCSVStatusPopup').modal({
                    backdrop: 'static',
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

function ShowAddToFeedsPopup() {
    if ($("#divResult_Child_Data_" + _SearchTermIndex + " input[type=checkbox]:checked").length > 0) {
        $('#ddlIQAgent').val('');
        $('#ddlIQAgent').removeClass('warningInput');
        $('#spanItemsSelectedCount').html($("#divResult_Child_Data_" + _SearchTermIndex + " input[type=checkbox]:checked").length);

        // After clicking the OK button, it is temporarily disabled to prevent duplicate submissions. However, it occassionally remains disabled.
        // I'm not sure why this happens, so just enable it every time the popup is shown.
        $('#btnAddToFeeds').removeAttr("disabled");

        $('#divAddToFeedsPopup').modal({
            backdrop: 'static',
            keyboard: true,
            dynamic: true
        });
    }
    else {
        ShowNotification(_msgAtleastOneRecordSelect);
    }
}

function AddToFeeds() {
    if ($("#divResult_Child_Data_" + _SearchTermIndex + " input[type=checkbox]:checked").length > 0) {
        if ($('#ddlIQAgent').val() > 0) {
            $('#ddlIQAgent').removeClass('warningInput');
            var mediaID = new Array();
            $("#divResult_Child_Data_" + _SearchTermIndex + " input[type=checkbox]").each(function () {
                if ($(this).is(':checked')) {
                    var tempIDValue = $(this).val().trim().split(',');


                    var mediaIDClass = new Object();
                    mediaIDClass.MediaID = tempIDValue[0];
                    mediaIDClass.SubMediaType = tempIDValue[1];

                    mediaID.push(mediaIDClass);
                }
            });

            var mySearchTermArray = new Array();
            for (var zz = 0; zz < _searchTerm.length; zz++) {
                mySearchTermArray.push(_searchTerm[zz].SearchTerm.trim());
            }

            var jsonPostData = {
                p_MediaID: mediaID,
                p_SearchRequestID: $("#ddlIQAgent").val(),
                searchTermIndex: _SearchTermIndex,
                searchTermArray: mySearchTermArray
            }

            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: _urlDiscoveryAddToFeeds,
                data: JSON.stringify(jsonPostData),
                contentType: 'application/json; charset=utf-8',
                success: function (result) {
                    if (result.isSuccess) {
                        ClosePopUp('divAddToFeedsPopup');
                        ShowNotification(result.message + " " + _msgItemsAddToFeedsSuccessfully);
                        ClearCheckboxSelection();
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
            $('#ddlIQAgent').addClass('warningInput');
        }
    }
    else {
        ShowNotification(_msgAtleastOneRecordSelect);
    }

}
function showPopupOver(index) {
    if ($('#divPageSizePopover_' + index).length <= 0) {

        var drphtml = $("#divPageSizeDropDown_" + index).html();
        $('#aPageSize_' + index).popover({
            trigger: 'manual',
            html: true,
            title: '',
            placement: 'top',
            template: '<div id="divPageSizePopover_' + index + '" class="popover" style="width:125px;"><div class="popover-inner"><div class="popover-content" style="padding:0px;" ><p></p></div></div></div>',
            content: drphtml
        });
        $('#aPageSize_' + index).popover('show');
    }
    else {
        $('#divPageSizePopover_' + index).remove();
    }
}

function SelectPageSize(pageSize, index) {
    if (_PageSize != pageSize) {
        _PageSize = pageSize;
        $('#aPageSize_' + index).html(_PageSize + ' Items Per Page&nbsp;&nbsp;<span class="caret"></span>');
        $('#divPageSizePopover_' + index).remove();
        SearchResultAjaxRequest();
    }
}

function ShowViewArticleDiscovery(articleID) {
    var jsonPostData = {
        articleID: articleID
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlDiscoveryGetProQuestResultByID,
        data: JSON.stringify(jsonPostData),
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            if (result.isSuccess) {
                $("#divPQTitle").html(result.title);
                $("#divPQMediaDate").html(result.mediaDate);
                $("#divPQAuthor").html(result.authors);
                $("#divPQPublication").html(result.publication);
                $("#divPQContent").html(result.content);
                $("#divPQCopyright").html(result.copyright);

                $('#divViewArticlePopup').modal({
                    backdrop: 'static',
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

function GetTopics(ddlMedium, index, isInitialLoad) {
    var jsonPostData = {
        index: index,
        searchTerm: _searchTerm[index].SearchTerm.trim(),
        searchName: _searchTerm[index].SearchName.trim(),
        fromDate: _fromDate,
        toDate: _toDate,
        mediums: isInitialLoad ? _SearchMediums : [$(ddlMedium).val()],
        advanceSearches: _IsActiveAdvanceSearch == true ? getAdvancedSearchByTermIndex(index) : new Object(),
        isInitialLoad: isInitialLoad
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: '/Discovery/GetTopics/',
        data: JSON.stringify(jsonPostData),
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            if (result.isSuccess) {
                if (isInitialLoad) {
                    $("#divTopicsData_" + index).html(result.HTML);
                }

                if (result.chartHTML != "") {
                    var JsonLineChart = JSON.parse(result.chartHTML);

                    JsonLineChart.tooltip = { enabled: false }; // Has to be set in JS. Doesn't work if set in DiscoveryLogic.
                    JsonLineChart.plotOptions.series.point.events.click = TopicClick;

                    $("#divTopicsChart_" + index).html('');
                    $("#divTopicsChart_" + index).highcharts(JsonLineChart);
                }
                else {
                    $("#divTopicsChart_" + index).html('<div style="text-align:center; padding-top:50px; font-weight:bold;">No topics found. Please widen search.</div>');
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

function ShowHideTopResults(index, isShowResults) {
    if ((isShowResults && $("#divTopResultsData_" + index).hasClass("displayNone")) ||
        (!isShowResults && $("#divTopicsData_" + index).hasClass("displayNone"))) {
        $("#divTopResultsTab_" + index).toggleClass("topResultsActive");
        $("#divTopicsTab_" + index).toggleClass("topResultsActive");
        $("#divTopResultsData_" + index).toggleClass("displayNone");
        $("#divTopicsData_" + index).toggleClass("displayNone");
    }

    // If showing the Topics tab for the first time, load the data
    if (!isShowResults && $("#divTopicsData_" + index).html() == "") {
        GetTopics(null, index, true);
    }
}

function TopicClick() {
    var newTopic = EscapeUrlCharacters(this.Value.toString().trim());
    var newSearchTermVar = EscapeUrlCharacters("\"" + newTopic + "\" AND (" + this.SearchTerm.toString().trim() + ")");
    var newValueVar = EscapeUrlCharacters("\"" + newTopic + "\" AND (" + this.SearchName.toString().trim() + ")");
    var newMedium = this.Medium.toString().trim();

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlDiscoveryOpenIFrame,
        contentType: 'application/json; charset=utf-8',

        success: function (result) {
            if (result.isSuccess) {
                $('#divDiscoveryResultsPage').modal({
                    backdrop: 'static',
                    keyboard: true,
                    dynamic: true
                });

                $("#divDiscoveryResultsPage").resizable({
                    handles: 'e,se,s,w',
                    iframeFix: true,
                    start: function () {
                        ifr = $('#iFrameDiscoveryResults');
                        var d = $('<div></div>');

                        $('#divDiscoveryResultsPage').append(d[0]);
                        d[0].id = 'temp_div';
                        d.css({ position: 'absolute' });
                        d.css({ top: ifr.position().top, left: 0 });
                        d.height(ifr.height());
                        d.width('100%');
                    },
                    stop: function () {
                        $('#temp_div').remove();
                    },
                    resize: function (event, ui) {
                        var newWd = ui.size.width - 10;
                        var newHt = ui.size.height - 20;
                        $("#iFrameDiscoveryResults").width(newWd).height(newHt);
                    }
                }).draggable({
                    iframeFix: true,
                    start: function () {
                        ifr = $('#iFrameDiscoveryResults');
                        var d = $('<div></div>');

                        $('#divDiscoveryResultsPage').append(d[0]);
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

                $('#iFrameDiscoveryResults').attr("src", "//" + window.location.hostname + "/Discovery?SearchTermTopic=" + newSearchTermVar + "&SearchNameTopic=" + newValueVar + "&Topic=" + newTopic + "&TopicMedium=" + newMedium);
                $('#divDiscoveryResultsPage').css("position", "");
                $('#divDiscoveryResultsPage').css("height", documentHeight - 200);
                $('#iFrameDiscoveryResults').css("height", documentHeight - 200);

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

function EscapeUrlCharacters(term) {
    return term.replace(/#/g, "%23").replace(/&/g, "%26").replace(/\?/g, "%3F");
}

function CancelIFramePopup() {
    $("#divDiscoveryResultsPage").css({ "display": "none" });
    $("#divDiscoveryResultsPage").modal("hide");
    $('#iFrameDiscoveryResults').attr("src", "");

    _IsToggle = false;
}


//Saved Search
function SaveSearch() {
    var mySearchIDArray = new Array();
    for (var zz = 0; zz < _searchTerm.length; zz++) {
        mySearchIDArray.push(_searchTerm[zz].SearchID.trim());
    }

    var advanceSearchesArray = [];
    var advanceSearchIDsArray = [];
    $.each(AdvancedSearchList, function () {
        advanceSearchesArray.push(this);
    });
    $.each(AdvancedSearchList, function (item, index) {
        advanceSearchIDsArray.push(item);
    });

    if (ValidateSaveSearchInput()) {
        $('#imgSaveSearchLoading').show();
        var mySearchTermArray = new Array();
        var mySearchIDArray = new Array();
        for (var zz = 0; zz < _searchTerm.length; zz++) {
            mySearchTermArray.push(_searchTerm[zz].SearchTerm);
            mySearchIDArray.push(_searchTerm[zz].SearchID);
        }
        var jsonPostData = {
            title: $('#txtSaveSearchPopup').val().trim(),
            SearchID: mySearchIDArray,
            searchTerm: mySearchTermArray,
            mediums: _SearchMediums,
            advanceSearchList: advanceSearchesArray,
            advanceSearchIDList: advanceSearchIDsArray
        }

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlDiscoverySaveSearch,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),

            success: OnSaveSearchComplete,
            error: OnSaveSearchFail
        });
    }
    else {
        if ($('#txtSaveSearchPopup').val().trim() == '') {
            $('#txtSaveSearchPopup').css('border', '1px red solid');
            $('#txtSaveSearchPopup').addClass('boxshadow');

            setTimeout(function () {
                $('#txtSaveSearchPopup').removeClass('boxshadow');
            }, 2000);
        }

        if (msg != '') {
            ShowNotification(msg);
        }
    }
}

function ValidateSaveSearchInput() {
    var isValid = true;
    msg = '';

    if (_searchTerm.length <= 0) {
        isValid = false;
        msg = _msgEnterSearchTerm;
    }
    if ($('#txtSaveSearchPopup').val().trim() == '') {
        msg = _msgEnterSaveSearchTitle;
        isValid = false;
    }

    if (isValid) {
        for (var zz = 0; zz < _searchTerm.length; zz++) {
            if (!_searchTerm[zz].SearchTerm.trim()) {
                msg = _msgEnterSearchTerm;
                isValid = false;
                break;
            }
        }
    }

    return isValid;
}

function OnSaveSearchComplete(result) {
    $('#imgSaveSearchLoading').hide();
    $('#divPopover').remove();
    if (result.isSuccess) {
        ShowNotification(result.message);
        GetSavedSearch(false, true);
    }
    else {

        CheckForAuthentication(result, _msgErrorOccured);
    }
}

function OnSaveSearchFail(result) {
    $('#imgSaveSearchLoading').hide();
    $('#divPopover').remove();
    ShowNotification(_msgErrorOccured);
}

//Populate saved searches on sidebar
function ShowSaveSearchDiscovery() {
    $('#divPopover').remove();
    $('#aSaveSearch').popover({
        trigger: 'manual',
        html: true,
        title: '',
        placement: 'right',
        template: '<div id="divPopover" class="popover"><div class="arrow"></div><div class="popover-inner"><div class="popover-content"><p></p></div></div></div>',
        content: '<input type="text" placeholder="Save Search Title" id="txtSaveSearchPopup" /><div><input type="button"  class="cancelButton marginbottom0"  style="margin-top:0px !important;" value="Cancel"  onclick="$(\'#divPopover\').remove();" /><input type="button" id="btnSaveSearch" class="button marginbottom0" style="margin-left:10px !important;margin-top:0px !important;" value="Submit" onclick="SaveSearch();" /><img src="../../Images/Loading_1.gif" class="displayNone" id="imgSaveSearchLoading" /></div>'
    });

    $('#aSaveSearch').popover('show');
}

function GetSavedSearch(p_isNext, p_isInitialize) {
    $('#divSavedSearch').html(_imgLoading);
    $('#divSavedSearch').addClass('text-align-center');
    var jsonPostData = {
        isNext: p_isNext,
        isInitialize: p_isInitialize
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlDiscoveryGetSaveSearch,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),
        global: false,
        success: OnGetSaveSearchComplete,
        error: OnGetSaveSearchFail
    });

    return false;
}
function OnGetSaveSearchComplete(result) {

    if (result.isSuccess) {
        SetSavedSearchHTML(result);
    }
    else {
        $('#divSavedSearch').html('An error occured,<a class="cursorPointer" onclick="GetSavedSearch(false, true);">try again</a>');
    }
}

function SetSavedSearchHTML(result) {
    $('#divSavedSearch').removeClass('text-align-center');
    $('#divSavedSearch').html(result.HTML);

    if (result.isPreviousAvailable) {
        $('#aSavedSearchPrevious').attr("onclick", "GetSavedSearch(false,false);");
        $('#aSavedSearchPrevious').show();
    }
    else {
        $('#aSavedSearchPrevious').removeAttr("onclick");
        $('#aSavedSearchPrevious').hide();
    }

    if (result.HasMoreResult) {
        $('#aSavedSearchNext').attr("onclick", "GetSavedSearch(true,false);");
        $('#aSavedSearchNext').show();
    }
    else {
        $('#aSavedSearchNext').removeAttr("onclick");
        $('#aSavedSearchNext').hide();
    }

    $('#spnSavedSearchRecordDetail').html(result.saveSearchRecordDetail);
}

function OnGetSaveSearchFail(result) {
    $('#divSavedSearch').html('An error occured, <a class="cursorPointer" onclick="GetSavedSearch(false, true);">try again</a>');
}

//Accessed when a saved search is clicked
function LoadSavedSearch(ID) {
    IsChartActive = 1;

    ClearFilterVariable();
    //_IsToggle = 0;
    var jsonPostData = {
        p_ID: ID
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlDiscoveryLoadSavedSearch,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),

        success: OnLoadSaveSearchComplete,
        error: OnLoadSaveSearchFail
    });
}

function OnLoadSaveSearchComplete(result) {
    if (result.isSuccess) {
        SetSavedSearchHTML(result)
        _searchTerm = new Array();
        _SearchMediums = result.discovery_SavedSearch.Mediums == null ? new Array() : result.discovery_SavedSearch.Mediums;
        _SearchDate = '';
        _SearchTVMarket = '';

        var invalidAgents = new Array();
        var _tDate = new Date();
        var _fDate = new Date(_tDate.getFullYear(), _tDate.getMonth() - 3, _tDate.getDate());

        $("#dpFrom").datepicker("setDate", _fDate);
        $("#dpTo").datepicker("setDate", _tDate);

        _fromDate = $('#dpFrom').val();
        _toDate = $('#dpTo').val();

        $('#ulSearchTerm').html('');
        searchTermCount = 0;
        for (var zz = 0; zz < result.discovery_SavedSearch.SearchTermArray.length; zz++) {
            var SearchTermClass = new Object();
            SearchTermClass.SearchTerm = result.discovery_SavedSearch.SearchTermArray[zz];
            SearchTermClass.SearchID = result.discovery_SavedSearch.SearchIDArray[zz];
            SearchTermClass.ResultShown = false;
            SearchTermClass.IsCurrentTab = false;

            var addEntry = true;
            var id = result.discovery_SavedSearch.SearchIDArray[zz].toString();
            var content = result.discovery_SavedSearch.SearchTermArray[zz].toString();
            var display = content;

            //if the item is an agent find it's information else skip
            if (isFinite(String(id).trim() || NaN) && parseInt(id) > -1) {
                var count = 0;
                $.each(_AgentList, function (index, value) {
                    //_AgentList is populated with "QueryName|||SearchTerm|||ID"
                    //run through all agents and find the agent with the matching id
                    var agentFull = '' + value;
                    var agentArray = agentFull.split("|||");
                    if (agentArray[2] == id) {
                        //once we find the correct query name and search term stop searching
                        count++;
                        content = agentArray[1].toString().replace(/"/g, "\"");
                        display = agentArray[0].toString();
                        return false;
                    }
                });

                //if agent is invalid do not add the search entry
                if (count == 0) {
                    addEntry = false;

                    var invalidName = "with ID " + id; //if we cannot find a query name to match the id display "with ID #" 
                    $.each(_AgentListAll, function (index, value) {
                        //_AgentListAll is populated with "QueryName|||SearchTerm|||ID"
                        //run through all agents and find the agent with the matching id
                        var agentFull = '' + value;
                        var agentArray = agentFull.split("|||");
                        if (agentArray[2] == id) {
                            //once we find the correct query name stop searching
                            invalidName = agentArray[0].toString();
                            return false;
                        }
                    });

                    //add agent to invalid list
                    invalidAgents.push(invalidName);
                }
            }
            else {
                // Make sure newly created search terms don't get assigned the same ID as a saved term
                var tempID = id.replace("Z", "");
                if (isFinite(String(tempID).trim() || NaN) && parseInt(tempID) >= _NextSearchTermID) {
                    _NextSearchTermID = parseInt(tempID) + 1;
                }
            }

            if (addEntry) {
                searchTermCount++;
                _searchTerm.push(SearchTermClass);

                if (isFinite(String(id).trim() || NaN) && parseInt(id) > -1) {
                    $('#ulSearchTerm').append("<li style=\"list-style:none outside none\" id=\"lisearchTerm_" + searchTermCount + "\" ><img id=\"imgRemoveSearchTermTextBox_" + searchTermCount + "\" src=\"../../Images/Delete.png?v=1.1\" alt=\"\" onclick=\"$('#lisearchTerm_" + searchTermCount + "').remove();PushSearchTermintoArray();CheckForMaxSearchTerm();\" class=\"marginTop-9 marginRight9 cursorPointer\" /><input type=\"text\" placeholder=\"Search Agent\" id=\"txtSearchTerm_" + searchTermCount + "\" onclick=\"ShowAddSearchAgentPopup(this.id);\" readonly=\"readonly\" /></li>");
                }
                else {
                    $('#ulSearchTerm').append("<li style=\"list-style:none outside none\" id=\"lisearchTerm_" + searchTermCount + "\" ><img id=\"imgRemoveSearchTermTextBox_" + searchTermCount + "\" src=\"../../Images/Delete.png?v=1.1\" alt=\"\" onclick=\"$('#lisearchTerm_" + searchTermCount + "').remove();PushSearchTermintoArray();CheckForMaxSearchTerm();\" class=\"marginTop-9 marginRight9 cursorPointer\" /><input type=\"text\" placeholder=\"Search Term\" id=\"txtSearchTerm_" + searchTermCount + "\" onclick=\"ShowAddSearchTermPopup(this.id);\" readonly=\"readonly\" /></li>");
                }

                var tooltiptext = content.length > 100 ? tooltiptext = content.replace(/"/g, "\"").substring(0, 100) + "..." : tooltiptext = content.replace(/"/g, "\"");
                $('#txtSearchTerm_' + searchTermCount).val(display);
                $('#txtSearchTerm_' + searchTermCount).attr('SearchTerm', content);
                $('#txtSearchTerm_' + searchTermCount).attr('SearchID', id);
                $('#txtSearchTerm_' + searchTermCount).attr('title', tooltiptext);
            }
        }

        _IsActiveAdvanceSearch = false;
        SetActiveAdvanceSearchValues();
        for (var x = 0; x < result.discovery_SavedSearch.AdvanceSearchSettingsList.length; x++) {
            var AdvanceSearch = result.discovery_SavedSearch.AdvanceSearchSettingsList[x];
            var agentID = result.discovery_SavedSearch.AdvanceSearchSettingIDsList[x].toString();
            AdvancedSearchList[agentID] = AdvanceSearch;
            _IsActiveAdvanceSearch = true;
        }

        CheckForMaxSearchTerm();
        PushSearchTermintoArray();

        _SearchTermResult = result.discovery_SavedSearch.SearchTermArray[0];
        GenerateSearchTermTab();
        $('#divChartMain').removeAttr('class');
        $('#divResultMain').attr('class', 'heightWidth0');

        $('#imgChartResult').attr('src', '../../Images/Result.png');

        setTimeout(function () {
            SearchResult();
        }, 1);

        if (invalidAgents.length > 0) {
            var message = "";
            if (invalidAgents.length == 1) message += "The agent " + invalidAgents[0].toString() + " was removed from your saved search because it is no longer active or it is suspended.";
            else message += "The agents " + invalidAgents.join(", ").replace(/,(?=[^,]*$)/, ' and') + " were removed from your saved search because they are no longer active or they are suspended.";
            message += " Would you like to update your saved search?"

            getConfirm("Inactive or Suspended Agents in Saved Search", message, "Yes", "No", function (res) {
                if (res == true) {
                    UpdateDiscoverySavedSearchInner(result.discovery_SavedSearch.ID);
                }
            });
        }
    }
    else {
        CheckForAuthentication(result, _msgErrorOccured);
    }
}

function OnLoadSaveSearchFail(result) {
    ShowNotification(_msgErrorOccured);
}

function UpdateDiscoverySavedSearch(p_id) {
    if (_searchTerm.length > 0) {
        getConfirm("Update Saved Search", "Are you sure you would like to update your saved search?", "Continue", "Cancel", function (res) {
            if (res == true) {
                UpdateDiscoverySavedSearchInner(p_id);
            }
        });
    }
    else {
        ShowNotification(_msgEnterSearchTerm);
    }
}

function UpdateDiscoverySavedSearchInner(p_id) {
    $('#imgSaveSearchLoading').show();
    var mySearchTermArray = new Array();
    var mySearchIDArray = new Array();
    for (var zz = 0; zz < _searchTerm.length; zz++) {
        mySearchTermArray.push(_searchTerm[zz].SearchTerm);
        mySearchIDArray.push(_searchTerm[zz].SearchID);
    }

    var advanceSearchesArray = [];
    var advanceSearchIDsArray = [];
    $.each(AdvancedSearchList, function () {
        advanceSearchesArray.push(this);
    });
    $.each(AdvancedSearchList, function (item, index) {
        advanceSearchIDsArray.push(item);
    });

    var jsonPostData = {
        p_ID: p_id,
        p_SearchTerm: mySearchTermArray,
        p_SearchID: mySearchIDArray,
        mediums: _SearchMediums,
        advanceSearchList: advanceSearchesArray,
        advanceSearchIDList: advanceSearchIDsArray
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlDiscoveryUpdateSavedSearch,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),
        success: function (result) {

            $('#imgSaveSearchLoading').hide();
            if (result.isSuccess) {
                ShowNotification(result.message);
                GetSavedSearch(false, true);
            }
            else {
                CheckForAuthentication(result, _msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured);
        }
    });
}

function DeleteDiscoverySavedSearchByID(ID) {
    var jsonPostData = {
        p_ID: ID
    }

    getConfirm("Delete Saved Search", _msgConfirmSavedSearchDelete, "Confirm Delete", "Cancel", function (res) {
        if (res) {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: _urlDiscoveryDeleteDiscoverySavedSearchByID,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(jsonPostData),

                success: OnDeleteSaveSearchComplete,
                error: OnDeleteSaveSearchFail
            });
        }
    });
}

function OnDeleteSaveSearchComplete(result) {

    if (result.isSuccess) {
        ShowNotification(result.message);
        setTimeout(function () {
            GetSavedSearch(false, true);
        }, 1);
    }
    else {
        CheckForAuthentication(result, _msgErrorOccured);
    }
}
function OnDeleteSaveSearchFail(result) {
    ShowNotification(_msgErrorOccured);
}



//Advanced Search
var mainTabID = -2; //cannot be defaultPreviousTabID or a positive number (could be a possible agent id) (if changed it MUST be changed when used in Discovery Controller in relation to the Advanced Search)
var previousTabID; //used to keep tab of change tab event
var defaultPreviousTabID = -3; //cannot equal mainTabID or a positive number (could be a possible agent id)
var changedAdvSearch; //were any of the Advanced Search tabs updated?

function OpenAdvanceSearchPopup() {
    previousTabID = defaultPreviousTabID;
    changedAdvSearch = false;

    AdvancedSearchListTemp = AdvancedSearchList;

    //populate the popup
    PopulateAdvanceSearchTabHeaders();

    //select main tab and load it's content
    GetAdvancedSearchTabData(mainTabID);

    //display popup
    $("#divAdvanceSearchPopup .button").removeAttr("disabled");

    $("#divAdvanceSearchPopup").modal({
        backdrop: 'static',
        keyboard: true,
        dynamic: true
    });

    //show/hide subsections
    if ((_SearchMediums == '' || $.inArray('TV', _SearchMediums) > -1) && $("#divTVSetup").length > 0) {
        ShowHideTabdiv(0, true);
    }
    else if ((_SearchMediums == '' || $.inArray('NM', _SearchMediums) > -1) && $("#divNMSetup").length > 0) {
        ShowHideTabdiv(1, true);
    }
    else if ((_SearchMediums == '' || $.inArray('Blog', _SearchMediums) > -1) && $("#divBLSetup").length > 0) {
        ShowHideTabdiv(2, true);
    }
    else if ((_SearchMediums == '' || $.inArray('Forum', _SearchMediums) > -1) && $("#divFOSetup").length > 0) {
        ShowHideTabdiv(3, true);
    }
    else if ((_SearchMediums == '' || $.inArray('PQ', _SearchMediums) > -1) && $("#divPQSetup").length > 0) {
        ShowHideTabdiv(4, true);
    }
    else if ((_SearchMediums == '' || $.inArray('LN', _SearchMediums) > -1) && $("#divLNSetup").length > 0) {
        ShowHideTabdiv(5, true);
    }

    //disable other tabs on load if 
    ToggleTabEnable(mainTabID);
}

function PopulateAdvanceSearchTabHeaders() {
    $('#divAdvSearchTabHeader').html('');
    var html =
        "<div id='divAdvSearchTabHeader' class='margintop5 discoveryTabParent' style='width: 100%;'>" +
            "<div class='advSearchTab' id='divAdvSearchTab_" + mainTabID + "' onclick='GetAdvancedSearchTabData(\"" + mainTabID + "\");' SearchTerm='' SearchID='" + mainTabID + "'>Main</div>";

    $('#ulSearchTerm li input[type=text]').each(function () {
        var id = $(this).attr('SearchID').replace(/"/g, "").replace(/'/g, "").replace(/\s+/g, '');
        var term = $(this).attr('SearchTerm').replace(/"/g, "&quot;");
        var name = $(this).val();
        var title = $(this).attr('title').replace(/"/g, "&quot;");

        html += '<div class="advSearchTab" id="divAdvSearchTab_' + id + '" onclick="GetAdvancedSearchTabData(\'' + id + '\');" SearchTerm="' + term + '" SearchID="' + id + '" title="' + title + '">' + EscapeHTML(name) + '</div>';
    });

    $('#divAdvSearchTabHeader').append(html);
}

function ClearAdvancedSearchTab() {
    $('[name="txtClearAdvSearch"]').val('');
    $('[name="ddlClearAdvSearch"]').val(CONST_ZERO).trigger("chosen:updated");
    $('[name="ddlAdvSearchLanguage"]').val(CONST_ZERO).trigger("chosen:updated");
}

function GetAdvancedSearchTabData(id) {
    if (previousTabID != id) {
        if (previousTabID == defaultPreviousTabID || SetAdvanceSearch(previousTabID)) {
            //select the tab with the passed in id
            $('#divAdvSearchTabHeader div').each(function () {
                $(this).toggleClass("pieChartActive", false);
            });
            $('#divAdvSearchTab_' + id).toggleClass("pieChartActive", true);

            ClearAdvancedSearchTab();

            //if it's the main tab load regular advanced search else load the agent advanced search
            SetActiveAdvanceSearchValues(id);

            //check to see if tab should be greyed out
            var isGreyOut = $('#divAdvSearchTab_' + id).hasClass("greyOutTabBackground");
            $('[name="txtClearAdvSearch"]').attr('readOnly', isGreyOut);
            $('[name="txtClearAdvSearch"]').toggleClass('greyOut', isGreyOut);
            $('[name="ddlClearAdvSearch"]').prop('disabled', isGreyOut).trigger("chosen:updated");
            $('[name="ddlLangClearAdvSearch"]').prop('disabled', isGreyOut).trigger("chosen:updated");

            previousTabID = id;
        }
    }
}

function ToggleTabEnable(ID) {
    if (ID == mainTabID) {
        //if main tab is not default then the other tabs cannot be edited
        var isGreyOut = isDefaultAdvancedSearch(AdvancedSearchListTemp[ID]) == false;

        $('#divAdvSearchTabHeader div').each(function () {
            if ($(this).attr('searchID') != null && $(this).attr('searchID').toString() != mainTabID.toString()) {
                $(this).toggleClass('greyOutTabBackground', isGreyOut);
                $(this).toggleClass('greyOut', isGreyOut);
            }
        });
    }
}

function SetActiveAdvanceSearchValues(id) {
    var AdvanceSearch = AdvancedSearchListTemp[id];

    if (id == mainTabID) {
        // If main tab do not allow search terms to be edited and include message to user

        $('#AdvancedSearchTabNote').text('NOTE: Changing the "Main" search criteria will override all other search tabs.');
        $('#AdvancedSearchTabNote').toggleClass('AdvancedSearchTabNote', true);

        $('[name="searchTermDIV"]').hide();
    }
    else {
        // since this is a search term/agent tab search terms should be editable
        // display message only for agent tabs

        if (isFinite(String(id).trim() || NaN)) {
            $('#AdvancedSearchTabNote').text('NOTE: Changing search criteria will NOT affect the IQ Agent Setup.');
            $('#AdvancedSearchTabNote').toggleClass('AdvancedSearchTabNote', true);
        }
        else {
            $('#AdvancedSearchTabNote').text('');
            $('#AdvancedSearchTabNote').toggleClass('AdvancedSearchTabNote', false);
        }

        $('[name="searchTermDIV"]').show();
    }

    if (AdvanceSearch != null) {
        if ((_SearchMediums == '' || $.inArray('TV', _SearchMediums) > -1) && $("#divTVSetup").length > 0) {
            if (AdvanceSearch.TVSettings != null) {
                $("#txtSearchTerm_TV").val(AdvanceSearch.TVSettings.SearchTerm);
                $("#txtProgramTitle").val(AdvanceSearch.TVSettings.ProgramTitle);

                $("#txtAppearing").val(AdvanceSearch.TVSettings.Appearing);

                if (AdvanceSearch.TVSettings.CategoryList != null && AdvanceSearch.TVSettings.CategoryList.length > 0) {
                    $("#ddlCategory_TV").val(AdvanceSearch.TVSettings.CategoryList).trigger("chosen:updated");
                }

                if (AdvanceSearch.TVSettings.IQDmaList != null && AdvanceSearch.TVSettings.IQDmaList.length > 0) {
                    $("#ddlDMA_TV").val(AdvanceSearch.TVSettings.IQDmaList).trigger("chosen:updated");
                }

                if (AdvanceSearch.TVSettings.StationList != null && AdvanceSearch.TVSettings.StationList.length > 0) {
                    $("#ddlStation_TV").val(AdvanceSearch.TVSettings.StationList).trigger("chosen:updated");
                }

                if (AdvanceSearch.TVSettings.AffiliateList != null && AdvanceSearch.TVSettings.AffiliateList.length > 0) {
                    $("#ddlAffiliate_TV").val(AdvanceSearch.TVSettings.AffiliateList).trigger("chosen:updated");
                }

                if (AdvanceSearch.TVSettings.RegionList != null && AdvanceSearch.TVSettings.RegionList.length > 0) {
                    $("#ddlRegion_TV").val(AdvanceSearch.TVSettings.RegionList).trigger("chosen:updated");
                }

                if (AdvanceSearch.TVSettings.CountryList != null && AdvanceSearch.TVSettings.CountryList.length > 0) {
                    $("#ddlCountry_TV").val(AdvanceSearch.TVSettings.CountryList).trigger("chosen:updated");
                }
            }
            else {
                AdvanceSearch.TVSettings = new Object();
                AdvanceSearch.TVSettings.ProgramTitle = null;
                AdvanceSearch.TVSettings.Appearing = null;
                AdvanceSearch.TVSettings.CategoryList = [];
                AdvanceSearch.TVSettings.IQDmaList = [];
                AdvanceSearch.TVSettings.StationList = [];
                AdvanceSearch.TVSettings.AffiliateList = [];
                AdvanceSearch.TVSettings.RegionList = [];
                AdvanceSearch.TVSettings.CountryList = [];
            }
        }

        if ((_SearchMediums == '' || $.inArray('NM', _SearchMediums) > -1) && $("#divNMSetup").length > 0) {
            if (AdvanceSearch.NewsSettings != null) {
                $("#txtSearchTerm_NM").val(AdvanceSearch.NewsSettings.SearchTerm);

                var output = $.map(AdvanceSearch.NewsSettings.PublicationList, function (obj, index) { return obj; }).join('; ');
                $("#txtPublication_NM").val(output);

                if (AdvanceSearch.NewsSettings.CategoryList != null && AdvanceSearch.NewsSettings.CategoryList.length > 0) {
                    $("#ddlCategory_NM").val(AdvanceSearch.NewsSettings.CategoryList).trigger("chosen:updated");
                }

                if (AdvanceSearch.NewsSettings.PublicationCategoryList != null && AdvanceSearch.NewsSettings.PublicationCategoryList.length > 0) {
                    $("#ddlPublicationCategory_NM").val(AdvanceSearch.NewsSettings.PublicationCategoryList).trigger("chosen:updated");
                }

                if (AdvanceSearch.NewsSettings.GenreList != null && AdvanceSearch.NewsSettings.GenreList.length > 0) {
                    $("#ddlGenre_NM").val(AdvanceSearch.NewsSettings.GenreList).trigger("chosen:updated");
                }
                if (AdvanceSearch.NewsSettings.MarketList != null && AdvanceSearch.NewsSettings.MarketList.length > 0) {
                    $("#ddlMarket_NM").val(AdvanceSearch.NewsSettings.MarketList).trigger("chosen:updated");
                }

                if (AdvanceSearch.NewsSettings.RegionList != null && AdvanceSearch.NewsSettings.RegionList.length > 0) {
                    $("#ddlRegion_NM").val(AdvanceSearch.NewsSettings.RegionList).trigger("chosen:updated");
                }

                if (AdvanceSearch.NewsSettings.CountryList != null && AdvanceSearch.NewsSettings.CountryList.length > 0) {
                    $("#ddlCountry_NM").val(AdvanceSearch.NewsSettings.CountryList).trigger("chosen:updated");
                }

                if (AdvanceSearch.NewsSettings.LanguageList != null && AdvanceSearch.NewsSettings.LanguageList.length > 0) {
                    $("#ddlLanguage_NM").val(AdvanceSearch.NewsSettings.LanguageList).trigger("chosen:updated");
                }
                else $("#ddlLanguage_NM").val(CONST_ZERO).trigger("chosen:updated");

                output = $.map(AdvanceSearch.NewsSettings.ExcludeDomainList, function (obj, index) { return obj; }).join('; ');
                $("#txtExcludeDomains_NM").val(output);
            }
            else {
                AdvanceSearch.NewsSettings = new Object();
                AdvanceSearch.NewsSettings.PublicationList = [];
                AdvanceSearch.NewsSettings.CategoryList = [];
                AdvanceSearch.NewsSettings.PublicationCategoryList = [];
                AdvanceSearch.NewsSettings.GenreList = [];
                AdvanceSearch.NewsSettings.MarketList = [];
                AdvanceSearch.NewsSettings.RegionList = [];
                AdvanceSearch.NewsSettings.CountryList = [];
                AdvanceSearch.NewsSettings.LanguageList = [];
                AdvanceSearch.NewsSettings.ExcludeDomainList = [];
            }
        }

        if ((_SearchMediums == '' || $.inArray('Blog', _SearchMediums) > -1) && $("#divBLSetup").length > 0) {
            if (AdvanceSearch.BlogSettings != null) {
                $("#txtSearchTerm_BL").val(AdvanceSearch.BlogSettings.SearchTerm);                     
                $("#txtAuthor_BL").val(AdvanceSearch.BlogSettings.Author);
                $("#txtTitle_BL").val(AdvanceSearch.BlogSettings.Title);

                var output = $.map(AdvanceSearch.BlogSettings.SourceList, function (obj, index) { return obj; }).join('; ');
                $("#txtSource_BL").val(output);

                output = $.map(AdvanceSearch.BlogSettings.ExcludeDomainList, function (obj, index) { return obj; }).join('; ');
                $("#txtExcludeDomains_BL").val(output);
            }
            else {
                AdvanceSearch.BlogSettings = new Object();
                AdvanceSearch.BlogSettings.Author = null;
                AdvanceSearch.BlogSettings.Title = null;
                AdvanceSearch.BlogSettings.SourceList = [];
                AdvanceSearch.BlogSettings.ExcludeDomainList = [];
            }
        }

        if ((_SearchMediums == '' || $.inArray('Forum', _SearchMediums) > -1) && $("#divFOSetup").length > 0) {
            if (AdvanceSearch.ForumSettings != null) {
                $("#txtSearchTerm_FO").val(AdvanceSearch.ForumSettings.SearchTerm);
                $("#txtAuthor_FO").val(AdvanceSearch.ForumSettings.Author);
                $("#txtTitle_FO").val(AdvanceSearch.ForumSettings.Title);

                var output = $.map(AdvanceSearch.ForumSettings.SourceList, function (obj, index) { return obj; }).join('; ');
                $("#txtSource_FO").val(output);

                if (AdvanceSearch.ForumSettings.SourceTypeList != null && AdvanceSearch.ForumSettings.SourceTypeList.length > 0) {
                    $("#ddlSourceType_FO").val(AdvanceSearch.ForumSettings.SourceTypeList).trigger("chosen:updated");
                }

                output = $.map(AdvanceSearch.ForumSettings.ExcludeDomainList, function (obj, index) { return obj; }).join('; ');
                $("#txtExcludeDomains_FO").val(output);
            }
            else {
                AdvanceSearch.ForumSettings = new Object();
                AdvanceSearch.ForumSettings.Author = null;
                AdvanceSearch.ForumSettings.Title = null;
                AdvanceSearch.ForumSettings.SourceList = [];
                AdvanceSearch.ForumSettings.SourceTypeList = [];
                AdvanceSearch.ForumSettings.ExcludeDomainList = [];
            }
        }

        if ((_SearchMediums == '' || $.inArray('PQ', _SearchMediums) > -1) && $("#divPQSetup").length > 0) {
            if (AdvanceSearch.ProQuestSettings != null) {
                $("#txtSearchTerm_PQ").val(AdvanceSearch.ProQuestSettings.SearchTerm);

                var output = $.map(AdvanceSearch.ProQuestSettings.PublicationList, function (obj, index) { return obj; }).join('; ');
                $("#txtPublication_PQ").val(output);

                output = $.map(AdvanceSearch.ProQuestSettings.AuthorList, function (obj, index) { return obj; }).join('; ');
                $("#txtAuthor_PQ").val(output);

                if (AdvanceSearch.ProQuestSettings.LanguageList != null && AdvanceSearch.ProQuestSettings.LanguageList.length > 0) {
                    $("#ddlLanguage_PQ").val(AdvanceSearch.ProQuestSettings.LanguageList).trigger("chosen:updated");
                }
                else $("#ddlLanguage_PQ").val(CONST_ZERO).trigger("chosen:updated");
            }
            else {
                AdvanceSearch.ProQuestSettings = new Object();
                AdvanceSearch.ProQuestSettings.PublicationList = [];
                AdvanceSearch.ProQuestSettings.AuthorList = [];
                AdvanceSearch.NewsSettings.LanguageList = [];
            }
        }

        if ((_SearchMediums == '' || $.inArray('LN', _SearchMediums) > -1) && $("#divLNSetup").length > 0) {
            if (AdvanceSearch.LexisNexisSettings != null) {
                $("#txtSearchTerm_LN").val(AdvanceSearch.LexisNexisSettings.SearchTerm);

                var output = $.map(AdvanceSearch.LexisNexisSettings.PublicationList, function (obj, index) { return obj; }).join('; ');
                $("#txtPublication_LN").val(output);

                if (AdvanceSearch.LexisNexisSettings.CategoryList != null && AdvanceSearch.LexisNexisSettings.CategoryList.length > 0) {
                    $("#ddlCategory_LN").val(AdvanceSearch.LexisNexisSettings.CategoryList).trigger("chosen:updated");
                }

                if (AdvanceSearch.LexisNexisSettings.PublicationCategoryList != null && AdvanceSearch.LexisNexisSettings.PublicationCategoryList.length > 0) {
                    $("#ddlPublicationCategory_LN").val(AdvanceSearch.LexisNexisSettings.PublicationCategoryList).trigger("chosen:updated");
                }

                if (AdvanceSearch.LexisNexisSettings.GenreList != null && AdvanceSearch.LexisNexisSettings.GenreList.length > 0) {
                    $("#ddlGenre_LN").val(AdvanceSearch.LexisNexisSettings.GenreList).trigger("chosen:updated");
                }

                if (AdvanceSearch.LexisNexisSettings.RegionList != null && AdvanceSearch.LexisNexisSettings.RegionList.length > 0) {
                    $("#ddlRegion_LN").val(AdvanceSearch.LexisNexisSettings.RegionList).trigger("chosen:updated");
                }

                if (AdvanceSearch.LexisNexisSettings.CountryList != null && AdvanceSearch.LexisNexisSettings.CountryList.length > 0) {
                    $("#ddlCountry_LN").val(AdvanceSearch.LexisNexisSettings.CountryList).trigger("chosen:updated");
                }

                if (AdvanceSearch.LexisNexisSettings.LanguageList != null && AdvanceSearch.LexisNexisSettings.LanguageList.length > 0) {
                    $("#ddlLanguage_LN").val(AdvanceSearch.LexisNexisSettings.LanguageList).trigger("chosen:updated");
                }
                else $("#ddlLanguage_LN").val(CONST_ZERO).trigger("chosen:updated");

                output = $.map(AdvanceSearch.LexisNexisSettings.ExcludeDomainList, function (obj, index) { return obj; }).join('; ');
                $("#txtExcludeDomains_LN").val(output);
            }
            else {
                AdvanceSearch.LexisNexisSettings = new Object();
                AdvanceSearch.LexisNexisSettings.PublicationList = [];
                AdvanceSearch.LexisNexisSettings.CategoryList = [];
                AdvanceSearch.LexisNexisSettings.PublicationCategoryList = [];
                AdvanceSearch.LexisNexisSettings.GenreList = [];
                AdvanceSearch.LexisNexisSettings.RegionList = [];
                AdvanceSearch.LexisNexisSettings.CountryList = [];
                AdvanceSearch.LexisNexisSettings.LanguageList = [];
                AdvanceSearch.LexisNexisSettings.ExcludeDomainList = [];
            }
        }
    }
}

function ShowHideTabdiv(elementIndex, isClearOther) {

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
            EleID = 'divBLTabContent';
            HeaderID = 'divBLSetup';
            break;
        case 3:
            EleID = 'divFOTabContent';
            HeaderID = 'divFOSetup';
            break;
        case 4:
            EleID = 'divProQuestTabContent';
            HeaderID = 'divPQSetup';
            break;
        case 5:
            EleID = 'divLNTabContent';
            HeaderID = 'divLNSetup';
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
        for (idx = 0; idx <= 5; idx++) {
            if (idx != elementIndex) {
                $("#divAdvanceSearchTabs").children().eq(idx * 2 + 1).hide();
                $("#divAdvanceSearchTabs").children().eq(idx * 2).find('img').attr('src', '../images/show.png')
            }
        }
    }
}

function AddAgentAdvancedSearch(ID) {
    var deferred = $.Deferred();
    var jsonPostData = {
        agentID: ID
    }

    // Manually show loading message so that it will continue to appear for follow-up call to get results
    ShowLoading();

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlDiscoveryGetAgentAdvancedSearch,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),

        success: function (result) {
            OnLoadAgentAdvancedSearchSuccess(result);
            deferred.resolve("success"); // this will allow to execute .done method. 
        },
        error: function (result) {
            deferred.reject("fail");
        },
        global: false
    });

    return deferred.promise();
}
function OnLoadAgentAdvancedSearchSuccess(result) {
    $('#imgAddToLibraryLoading').show();
    if (result.isSuccess && result.agentAdvancedSearch != null) {
        var searchRequestObject = result.agentAdvancedSearch;

        AdvanceSearch = new Object();

        //TV
        if (searchRequestObject.TV != null && searchRequestObject.TVSpecified == true) {
            AdvanceSearch.TVSettings = new Object();

            //Search Term
            if (searchRequestObject.TV.SearchTerm.SearchTerm != null) {
                AdvanceSearch.TVSettings.SearchTerm = searchRequestObject.TV.SearchTerm.SearchTerm.trim();
            }

            //Program Title
            if (searchRequestObject.TV.ProgramTitle != null) {
                AdvanceSearch.TVSettings.ProgramTitle = searchRequestObject.TV.ProgramTitle.trim().toLowerCase();
            }

            //Appearing
            if (searchRequestObject.TV.Appearing != null) {
                AdvanceSearch.TVSettings.Appearing = searchRequestObject.TV.Appearing.trim().toLowerCase();
            }

            //Category List
            if (searchRequestObject.TV.IQ_Class_Set != null && searchRequestObject.TV.IQ_Class_Set.IsAllowAll == false) {
                var arr_TV_IQ_Class = [];
                $.each(searchRequestObject.TV.IQ_Class_Set.IQ_Class, function (index, obj) {
                    arr_TV_IQ_Class.push(obj.num);
                });
                AdvanceSearch.TVSettings.CategoryList = arr_TV_IQ_Class;
            }

            //DMA List
            if (searchRequestObject.TV.IQ_Dma_Set != null && searchRequestObject.TV.IQ_Dma_Set.IsAllowAll == false) {
                var arr_TV_DMA = [];
                $.each(searchRequestObject.TV.IQ_Dma_Set.IQ_Dma, function (index, obj) {
                    arr_TV_DMA.push(obj.name);
                });
                AdvanceSearch.TVSettings.IQDmaList = arr_TV_DMA;
            }

            //Affiliate List
            if (searchRequestObject.TV.Station_Affiliate_Set != null && searchRequestObject.TV.Station_Affiliate_Set.IsAllowAll == false) {
                var arr_TV_Station_Affil = [];
                $.each(searchRequestObject.TV.Station_Affiliate_Set.Station_Affil, function (index, obj) {
                    arr_TV_Station_Affil.push(obj.name);
                });
                AdvanceSearch.TVSettings.AffiliateList = arr_TV_Station_Affil;
            }

            //Station List
            if (searchRequestObject.TV.IQ_Station_Set != null && searchRequestObject.TV.IQ_Station_Set.IsAllowAll == false) {
                var arr_TV_Station_ID = [];
                $.each(searchRequestObject.TV.IQ_Station_Set.IQ_Station_ID, function (index, obj) {
                    arr_TV_Station_ID.push(obj);
                });
                AdvanceSearch.TVSettings.StationList = arr_TV_Station_ID;
            }

            //Region List
            if (searchRequestObject.TV.IQ_Region_Set != null && searchRequestObject.TV.IQ_Region_Set.IsAllowAll == false) {
                var arr_TV_IQ_Region = [];
                $.each(searchRequestObject.TV.IQ_Region_Set.IQ_Region, function (index, obj) {
                    arr_TV_IQ_Region.push(obj.num);
                });
                AdvanceSearch.TVSettings.RegionList = arr_TV_IQ_Region;
            }

            //Country List
            if (searchRequestObject.TV.IQ_Country_Set != null && searchRequestObject.TV.IQ_Country_Set.IsAllowAll == false) {
                var arr_TV_IQ_Country = [];
                $.each(searchRequestObject.TV.IQ_Country_Set.IQ_Country, function (index, obj) {
                    arr_TV_IQ_Country.push(obj.num);
                });
                AdvanceSearch.TVSettings.CountryList = arr_TV_IQ_Country;
            }
        }

        //Online News
        if (searchRequestObject.News != null && searchRequestObject.NewsSpecified == true) {
            AdvanceSearch.NewsSettings = new Object();

            //Search Term
            if (searchRequestObject.News.SearchTerm.SearchTerm != null) {
                AdvanceSearch.NewsSettings.SearchTerm = searchRequestObject.News.SearchTerm.SearchTerm.trim();
            }

            //Publications
            if (searchRequestObject.News.Publications != null) {
                AdvanceSearch.NewsSettings.PublicationList = searchRequestObject.News.Publications;
            }

            // Category List
            if (searchRequestObject.News.NewsCategory_Set != null && searchRequestObject.News.NewsCategory_Set.IsAllowAll == false) {
                var arr_NM_Category = [];
                $.each(searchRequestObject.News.NewsCategory_Set.NewsCategory, function (index, obj) {
                    arr_NM_Category.push(obj);
                });
                AdvanceSearch.NewsSettings.CategoryList = arr_NM_Category;
            }

            // Publication Category List
            if (searchRequestObject.News.PublicationCategory_Set != null && searchRequestObject.News.PublicationCategory_Set.IsAllowAll == false) {
                var arr_NM_PubCategory = [];
                $.each(searchRequestObject.News.PublicationCategory_Set.PublicationCategory, function (index, obj) {
                    arr_NM_PubCategory.push(obj);
                });
                AdvanceSearch.NewsSettings.PublicationCategoryList = arr_NM_PubCategory;
            }

            // Genre List
            if (searchRequestObject.News.Genre_Set != null && searchRequestObject.News.Genre_Set.IsAllowAll == false) {
                var arr_NM_Genere = [];
                $.each(searchRequestObject.News.Genre_Set.Genre, function (index, obj) {
                    arr_NM_Genere.push(obj);
                });
                AdvanceSearch.NewsSettings.GenreList = arr_NM_Genere;
            }

            // Region List
            if (searchRequestObject.News.Region_Set != null && searchRequestObject.News.Region_Set.IsAllowAll == false) {
                var arr_NM_Region = [];
                $.each(searchRequestObject.News.Region_Set.Region, function (index, obj) {
                    arr_NM_Region.push(obj);
                });
                AdvanceSearch.NewsSettings.RegionList = arr_NM_Region;
            }

            // Country List
            if (searchRequestObject.News.Country_Set != null && searchRequestObject.News.Country_Set.IsAllowAll == false) {
                var arr_NM_Country = [];
                $.each(searchRequestObject.News.Country_Set.Country, function (index, obj) {
                    arr_NM_Country.push(obj);
                });
                AdvanceSearch.NewsSettings.CountryList = arr_NM_Country;
            }

            // Language List
            if (searchRequestObject.News.Language_Set != null && searchRequestObject.News.Language_Set.IsAllowAll == false) {
                var arr_NM_Language = [];
                $.each(searchRequestObject.News.Language_Set.Language, function (index, obj) {
                    arr_NM_Language.push(obj);
                });
                AdvanceSearch.NewsSettings.LanguageList = arr_NM_Language;
            }

            //Exclude Domains
            if (searchRequestObject.News.ExlcudeDomains != null) {
                AdvanceSearch.NewsSettings.ExcludeDomainList = searchRequestObject.News.ExlcudeDomains;
            }
        }

        //Blog
        if (searchRequestObject.Blog != null && searchRequestObject.BlogSpecified == true) {
            AdvanceSearch.BlogSettings = new Object();

            //Search Term
            if (searchRequestObject.Blog.SearchTerm.SearchTerm != null) {
                AdvanceSearch.BlogSettings.SearchTerm = searchRequestObject.Blog.SearchTerm.SearchTerm.trim();
            }

            //Sources
            if (searchRequestObject.Blog.Sources != null) {
                AdvanceSearch.BlogSettings.SourceList = searchRequestObject.Blog.Sources;
            }

            //Author
            if (searchRequestObject.Blog.Author != null) {
                AdvanceSearch.BlogSettings.Author = searchRequestObject.Blog.Author.trim().toLowerCase();
            }

            //Tile
            if (searchRequestObject.Blog.Title != null) {
                AdvanceSearch.BlogSettings.Title = searchRequestObject.Blog.Title.trim().toLowerCase();
            }

            //Exclude Domains
            if (searchRequestObject.Blog.ExlcudeDomains != null) {
                AdvanceSearch.BlogSettings.ExcludeDomainList = searchRequestObject.Blog.ExlcudeDomains;
            }
        } 

        //Forum
        if (searchRequestObject.Forum != null && searchRequestObject.ForumSpecified == true) {
            AdvanceSearch.ForumSettings = new Object();

            //Search Term
            if (searchRequestObject.Forum.SearchTerm.SearchTerm != null) {
                AdvanceSearch.ForumSettings.SearchTerm = searchRequestObject.Forum.SearchTerm.SearchTerm.trim();
            }

            //Sources
            if (searchRequestObject.Forum.Sources != null) {
                AdvanceSearch.ForumSettings.SourceList = searchRequestObject.Forum.Sources;
            }

            //Author
            if (searchRequestObject.Forum.Author != null) {
                AdvanceSearch.ForumSettings.Author = searchRequestObject.Forum.Author.trim().toLowerCase();
            }

            //Tile
            if (searchRequestObject.Forum.Title != null) {
                AdvanceSearch.ForumSettings.Title = searchRequestObject.Forum.Title.trim().toLowerCase();
            }

            // SourceType List
            if (searchRequestObject.Forum.SourceType_Set != null && searchRequestObject.Forum.SourceType_Set.IsAllowAll == false) {
                var arr_FO_SourceType = [];
                $.each(searchRequestObject.Forum.SourceType_Set.SourceType, function (index, obj) {
                    arr_FO_SourceType.push(obj);
                });
                AdvanceSearch.ForumSettings.SourceTypeList = arr_FO_SourceType;
            }

            //Exclude Domains
            if (searchRequestObject.Forum.ExlcudeDomains != null) {
                AdvanceSearch.ForumSettings.ExcludeDomainList = searchRequestObject.Forum.ExlcudeDomains;
            }
        }

        //Publications
        if (searchRequestObject.PQ != null && searchRequestObject.PQSpecified == true) {
            AdvanceSearch.ProQuestSettings = new Object();

            //Search Term
            if (searchRequestObject.PQ.SearchTerm.SearchTerm != null) {
                AdvanceSearch.ProQuestSettings.SearchTerm = searchRequestObject.PQ.SearchTerm.SearchTerm.trim();
            }

            if (searchRequestObject.PQ.Publications != null) {
                AdvanceSearch.ProQuestSettings.PublicationList = searchRequestObject.PQ.Publications;
            }
            if (searchRequestObject.PQ.Authors != null) {
                AdvanceSearch.ProQuestSettings.AuthorList = searchRequestObject.PQ.Authors;
            }
            if (searchRequestObject.PQ.Language_Set != null && searchRequestObject.PQ.Language_Set.IsAllowAll == false) {
                var arr_PQ_Language = [];
                $.each(searchRequestObject.PQ.Language_Set.Language, function (index, obj) {
                    arr_PQ_Language.push(obj);
                });
                AdvanceSearch.ProQuestSettings.LanguageList = arr_PQ_Language;
            }
        }

        //LexisNexis
        if (searchRequestObject.LexisNexis != null && searchRequestObject.LexisNexisSpecified == true) {
            AdvanceSearch.LexisNexisSettings = new Object();

            //Search Term
            if (searchRequestObject.LexisNexis.SearchTerm.SearchTerm != null) {
                AdvanceSearch.LexisNexisSettings.SearchTerm = searchRequestObject.LexisNexis.SearchTerm.SearchTerm.trim();
            }

            //Publications
            if (searchRequestObject.LexisNexis.Publications != null) {
                AdvanceSearch.LexisNexisSettings.PublicationList = searchRequestObject.LexisNexis.Publications;
            }

            // Category List
            if (searchRequestObject.LexisNexis.NewsCategory_Set != null && searchRequestObject.LexisNexis.NewsCategory_Set.IsAllowAll == false) {
                var arr_LN_Category = [];
                $.each(searchRequestObject.LexisNexis.NewsCategory_Set.NewsCategory, function (index, obj) {
                    arr_LN_Category.push(obj);
                });
                AdvanceSearch.LexisNexisSettings.CategoryList = arr_LN_Category;
            }

            // Publication Category List
            if (searchRequestObject.LexisNexis.PublicationCategory_Set != null && searchRequestObject.LexisNexis.PublicationCategory_Set.IsAllowAll == false) {
                var arr_LN_PubCategory = [];
                $.each(searchRequestObject.LexisNexis.PublicationCategory_Set.PublicationCategory, function (index, obj) {
                    arr_LN_PubCategory.push(obj);
                });
                AdvanceSearch.LexisNexisSettings.PublicationCategoryList = arr_LN_PubCategory;
            }

            // Genre List
            if (searchRequestObject.LexisNexis.Genre_Set != null && searchRequestObject.LexisNexis.Genre_Set.IsAllowAll == false) {
                var arr_LN_Genere = [];
                $.each(searchRequestObject.LexisNexis.Genre_Set.Genre, function (index, obj) {
                    arr_LN_Genere.push(obj);
                });
                AdvanceSearch.LexisNexisSettings.GenreList = arr_LN_Genere;
            }

            // Region List
            if (searchRequestObject.LexisNexis.Region_Set != null && searchRequestObject.LexisNexis.Region_Set.IsAllowAll == false) {
                var arr_LN_Region = [];
                $.each(searchRequestObject.LexisNexis.Region_Set.Region, function (index, obj) {
                    arr_LN_Region.push(obj);
                });
                AdvanceSearch.LexisNexisSettings.RegionList = arr_LN_Region;
            }

            // Country List
            if (searchRequestObject.LexisNexis.Country_Set != null && searchRequestObject.LexisNexis.Country_Set.IsAllowAll == false) {
                var arr_LN_Country = [];
                $.each(searchRequestObject.LexisNexis.Country_Set.Country, function (index, obj) {
                    arr_LN_Country.push(obj);
                });
                AdvanceSearch.LexisNexisSettings.CountryList = arr_LN_Country;
            }

            // Language List
            if (searchRequestObject.LexisNexis.Language_Set != null && searchRequestObject.LexisNexis.Language_Set.IsAllowAll == false) {
                var arr_LN_Language = [];
                $.each(searchRequestObject.LexisNexis.Language_Set.Language, function (index, obj) {
                    arr_LN_Language.push(obj);
                });
                AdvanceSearch.LexisNexisSettings.LanguageList = arr_LN_Language;
            }

            //Exclude Domains
            if (searchRequestObject.LexisNexis.ExlcudeDomains != null) {
                AdvanceSearch.LexisNexisSettings.ExcludeDomainList = searchRequestObject.LexisNexis.ExlcudeDomains;
            }
        }

        AdvancedSearchList[result.agentID] = AdvanceSearch;

        var defaultSearch = isDefaultAdvancedSearch(AdvanceSearch);
        if (defaultSearch == false) {
            _IsActiveAdvanceSearch = true;
            SetActiveFilter();
        }

        if (isDefaultAdvancedSearch(AdvancedSearchList[mainTabID]) == true) _UseAdvanceSearchDefault = defaultSearch;
        else _UseAdvanceSearchDefault = true;
    }
    $('#imgAddToLibraryLoading').hide();
}

function isDefaultAdvancedSearch(AdvanceSearch) {
    if (AdvanceSearch != null) {
        //TV
        if (AdvanceSearch.TVSettings != null
        && ((AdvanceSearch.TVSettings.SearchTerm != null && AdvanceSearch.TVSettings.SearchTerm.trim() != '')
        || (AdvanceSearch.TVSettings.ProgramTitle != null && AdvanceSearch.TVSettings.ProgramTitle.trim() != '')
        || (AdvanceSearch.TVSettings.Appearing != null && AdvanceSearch.TVSettings.Appearing.trim() != '')
        || (AdvanceSearch.TVSettings.CategoryList != null && AdvanceSearch.TVSettings.CategoryList.length > 0)
        || (AdvanceSearch.TVSettings.IQDmaList != null && AdvanceSearch.TVSettings.IQDmaList.length > 0)
        || (AdvanceSearch.TVSettings.StationList != null && AdvanceSearch.TVSettings.StationList.length > 0)
        || (AdvanceSearch.TVSettings.AffiliateList != null && AdvanceSearch.TVSettings.AffiliateList.length > 0)
        || (AdvanceSearch.TVSettings.RegionList != null && AdvanceSearch.TVSettings.RegionList.length > 0)
        || (AdvanceSearch.TVSettings.CountryList != null && AdvanceSearch.TVSettings.CountryList.length > 0))) {
            return false;
        }

        //Online News
        if (AdvanceSearch.NewsSettings != null
        && ((AdvanceSearch.NewsSettings.SearchTerm != null && AdvanceSearch.NewsSettings.SearchTerm.trim() != '')
        || (AdvanceSearch.NewsSettings.PublicationList != null && AdvanceSearch.NewsSettings.PublicationList.length > 0)
        || (AdvanceSearch.NewsSettings.CategoryList != null && AdvanceSearch.NewsSettings.CategoryList.length > 0)
        || (AdvanceSearch.NewsSettings.MarketList != null && AdvanceSearch.NewsSettings.MarketList.length > 0)
        || (AdvanceSearch.NewsSettings.GenreList != null && AdvanceSearch.NewsSettings.GenreList.length > 0)
        || (AdvanceSearch.NewsSettings.PublicationCategoryList != null && AdvanceSearch.NewsSettings.PublicationCategoryList.length > 0)
        || (AdvanceSearch.NewsSettings.RegionList != null && AdvanceSearch.NewsSettings.RegionList.length > 0)
        || (AdvanceSearch.NewsSettings.CountryList != null && AdvanceSearch.NewsSettings.CountryList.length > 0)
        || (AdvanceSearch.NewsSettings.LanguageList != null && AdvanceSearch.NewsSettings.LanguageList.length > 0)
        || (AdvanceSearch.NewsSettings.ExcludeDomainList != null && AdvanceSearch.NewsSettings.ExcludeDomainList.length > 0))) {
            return false;
        }

        //Blog
        if (AdvanceSearch.BlogSettings != null
        && ((AdvanceSearch.BlogSettings.SearchTerm != null && AdvanceSearch.BlogSettings.SearchTerm.trim() != '')
        || (AdvanceSearch.BlogSettings.Author != null && AdvanceSearch.BlogSettings.Author.trim() != '')
        || (AdvanceSearch.BlogSettings.Title != null && AdvanceSearch.BlogSettings.Title.trim() != '')
        || (AdvanceSearch.BlogSettings.SourceList != null && AdvanceSearch.BlogSettings.SourceList.length > 0)
        || (AdvanceSearch.BlogSettings.ExcludeDomainList != null && AdvanceSearch.BlogSettings.ExcludeDomainList.length > 0))) {
            return false;
        }

        //Forum
        if (AdvanceSearch.ForumSettings != null 
        && ((AdvanceSearch.ForumSettings.SearchTerm != null && AdvanceSearch.ForumSettings.SearchTerm.trim() != '')
        || (AdvanceSearch.ForumSettings.Author != null && AdvanceSearch.ForumSettings.Author.trim() != '')
        || (AdvanceSearch.ForumSettings.Title != null && AdvanceSearch.ForumSettings.Title.trim() != '')
        || (AdvanceSearch.ForumSettings.SourceList != null && AdvanceSearch.ForumSettings.SourceList.length > 0)
        || (AdvanceSearch.ForumSettings.SourceTypeList != null && AdvanceSearch.ForumSettings.SourceTypeList.length > 0)
        || (AdvanceSearch.ForumSettings.ExcludeDomainList != null && AdvanceSearch.ForumSettings.ExcludeDomainList.length > 0))) {
            return false;
        }

        //Publications
        if (AdvanceSearch.ProQuestSettings != null
        && ((AdvanceSearch.ProQuestSettings.SearchTerm != null && AdvanceSearch.ProQuestSettings.SearchTerm.trim() != '')
        || (AdvanceSearch.ProQuestSettings.PublicationList != null && AdvanceSearch.ProQuestSettings.PublicationList.length > 0)
        || (AdvanceSearch.ProQuestSettings.AuthorList != null && AdvanceSearch.ProQuestSettings.AuthorList.length > 0)
        || (AdvanceSearch.ProQuestSettings.LanguageList != null && AdvanceSearch.ProQuestSettings.LanguageList.length > 0))) {
            return false;
        }

        //LexisNexis
        if (AdvanceSearch.LexisNexisSettings != null
        && ((AdvanceSearch.LexisNexisSettings.SearchTerm != null && AdvanceSearch.LexisNexisSettings.SearchTerm.trim() != '')
        || (AdvanceSearch.LexisNexisSettings.PublicationList != null && AdvanceSearch.LexisNexisSettings.PublicationList.length > 0)
        || (AdvanceSearch.LexisNexisSettings.CategoryList != null && AdvanceSearch.LexisNexisSettings.CategoryList.length > 0)
        || (AdvanceSearch.LexisNexisSettings.GenreList != null && AdvanceSearch.LexisNexisSettings.GenreList.length > 0)
        || (AdvanceSearch.LexisNexisSettings.PublicationCategoryList != null && AdvanceSearch.LexisNexisSettings.PublicationCategoryList.length > 0)
        || (AdvanceSearch.LexisNexisSettings.RegionList != null && AdvanceSearch.LexisNexisSettings.RegionList.length > 0)
        || (AdvanceSearch.LexisNexisSettings.CountryList != null && AdvanceSearch.LexisNexisSettings.CountryList.length > 0)
        || (AdvanceSearch.LexisNexisSettings.LanguageList != null && AdvanceSearch.LexisNexisSettings.LanguageList.length > 0)
        || (AdvanceSearch.LexisNexisSettings.ExcludeDomainList != null && AdvanceSearch.LexisNexisSettings.ExcludeDomainList.length > 0))) {
            return false;
        }
    }
    return true;
}
function getAdvancedSearchByTermIndex(index) {
    var advancedSearch = null;
    var advancedSearchID = _UseAdvanceSearchDefault ? mainTabID.toString() : _searchTerm[index].SearchID.trim();
    $.each(AdvancedSearchList, function (item, index) {
        if (item == advancedSearchID) {
            advancedSearch = this;
            return false;
        }
    });
    if (advancedSearch == null) advancedSearch = new Object();

    return advancedSearch;
}

function SetAdvanceSearch(ID) {
    if (IsValidSearch()) {
        if (IsNewSearch(ID)) {
            changedAdvSearch = true;

            AdvanceSearch = new Object();
            if ((_SearchMediums == '' || $.inArray('TV', _SearchMediums) > -1) && $("#divTVSetup").length > 0) {
                AdvanceSearch.TVSettings = new Object();
                AdvanceSearch.TVSettings.SearchTerm = $("#txtSearchTerm_TV").val().trim();
                AdvanceSearch.TVSettings.ProgramTitle = $("#txtProgramTitle").val().trim().toLowerCase();
                AdvanceSearch.TVSettings.Appearing = $("#txtAppearing").val().trim().toLowerCase();
                AdvanceSearch.TVSettings.CategoryList = $.inArray("0", $("#ddlCategory_TV").val()) !== -1 ? null : $("#ddlCategory_TV").val();
                AdvanceSearch.TVSettings.IQDmaList = $.inArray("0", $("#ddlDMA_TV").val()) !== -1 ? null : $("#ddlDMA_TV").val();
                AdvanceSearch.TVSettings.StationList = $.inArray("0", $("#ddlStation_TV").val()) !== -1 ? null : $("#ddlStation_TV").val();
                AdvanceSearch.TVSettings.AffiliateList = $.inArray("0", $("#ddlAffiliate_TV").val()) !== -1 ? null : $("#ddlAffiliate_TV").val();
                AdvanceSearch.TVSettings.RegionList = $.inArray("0", $("#ddlRegion_TV").val()) !== -1 ? null : $("#ddlRegion_TV").val();
                AdvanceSearch.TVSettings.CountryList = $.inArray("0", $("#ddlCountry_TV").val()) !== -1 ? null : $("#ddlCountry_TV").val();
            }

            if ((_SearchMediums == '' || $.inArray('NM', _SearchMediums) > -1) && $("#divNMSetup").length > 0) {
                AdvanceSearch.NewsSettings = new Object();
                AdvanceSearch.NewsSettings.SearchTerm = $("#txtSearchTerm_NM").val().trim();
                AdvanceSearch.NewsSettings.CategoryList = $.inArray("0", $("#ddlCategory_NM").val()) !== -1 ? null : $("#ddlCategory_NM").val();
                AdvanceSearch.NewsSettings.PublicationCategoryList = $.inArray("0", $("#ddlPublicationCategory_NM").val()) !== -1 ? null : $("#ddlPublicationCategory_NM").val();
                AdvanceSearch.NewsSettings.MarketList = $.inArray("0", $("#ddlMarket_NM").val()) !== -1 ? null : $("#ddlMarket_NM").val();
                AdvanceSearch.NewsSettings.GenreList = $.inArray("0", $("#ddlGenre_NM").val()) !== -1 ? null : $("#ddlGenre_NM").val();
                AdvanceSearch.NewsSettings.RegionList = $.inArray("0", $("#ddlRegion_NM").val()) !== -1 ? null : $("#ddlRegion_NM").val();
                AdvanceSearch.NewsSettings.CountryList = $.inArray("0", $("#ddlCountry_NM").val()) !== -1 ? null : $("#ddlCountry_NM").val();
                AdvanceSearch.NewsSettings.LanguageList = $.inArray("0", $("#ddlLanguage_NM").val()) !== -1 ? null : $("#ddlLanguage_NM").val();

                AdvanceSearch.NewsSettings.PublicationList = [];
                $.each($("#txtPublication_NM").val().trim().split(";"), function () {
                    if ($.trim(this) != '') {
                        AdvanceSearch.NewsSettings.PublicationList.push($.trim(this).toLowerCase());
                    }
                });

                AdvanceSearch.NewsSettings.ExcludeDomainList = [];
                $.each($("#txtExcludeDomains_NM").val().trim().split(";"), function () {
                    if ($.trim(this) != '') {
                        AdvanceSearch.NewsSettings.ExcludeDomainList.push($.trim(this).toLowerCase());
                    }
                });
            }

            if ((_SearchMediums == '' || $.inArray('Blog', _SearchMediums) > -1) && $("#divBLSetup").length > 0) {
                AdvanceSearch.BlogSettings = new Object();
                AdvanceSearch.BlogSettings.SearchTerm = $("#txtSearchTerm_BL").val().trim();
                AdvanceSearch.BlogSettings.Author = $("#txtAuthor_BL").val().trim().toLowerCase();
                AdvanceSearch.BlogSettings.Title = $("#txtTitle_BL").val().trim().toLowerCase();

                AdvanceSearch.BlogSettings.SourceList = [];
                $.each($("#txtSource_BL").val().trim().split(";"), function () {
                    if ($.trim(this) != '') {
                        AdvanceSearch.BlogSettings.SourceList.push($.trim(this).toLowerCase());
                    }
                });

                AdvanceSearch.BlogSettings.ExcludeDomainList = [];
                $.each($("#txtExcludeDomains_BL").val().trim().split(";"), function () {
                    if ($.trim(this) != '') {
                        AdvanceSearch.BlogSettings.ExcludeDomainList.push($.trim(this).toLowerCase());
                    }
                });
            }

            if ((_SearchMediums == '' || $.inArray('Forum', _SearchMediums) > -1) && $("#divFOSetup").length > 0) {
                AdvanceSearch.ForumSettings = new Object();
                AdvanceSearch.ForumSettings.SearchTerm = $("#txtSearchTerm_FO").val().trim();
                AdvanceSearch.ForumSettings.Author = $("#txtAuthor_FO").val().trim().toLowerCase();
                AdvanceSearch.ForumSettings.Title = $("#txtTitle_FO").val().trim().toLowerCase();
                AdvanceSearch.ForumSettings.SourceTypeList = $.inArray("0", $("#ddlSourceType_FO").val()) !== -1 ? null : $("#ddlSourceType_FO").val();

                AdvanceSearch.ForumSettings.SourceList = [];
                $.each($("#txtSource_FO").val().trim().split(";"), function () {
                    if ($.trim(this) != '') {
                        AdvanceSearch.ForumSettings.SourceList.push($.trim(this).toLowerCase());
                    }
                });

                AdvanceSearch.ForumSettings.ExcludeDomainList = [];
                $.each($("#txtExcludeDomains_FO").val().trim().split(";"), function () {
                    if ($.trim(this) != '') {
                        AdvanceSearch.ForumSettings.ExcludeDomainList.push($.trim(this).toLowerCase());
                    }
                });
            }

            if ((_SearchMediums == '' || $.inArray('PQ', _SearchMediums) > -1) && $("#divPQSetup").length > 0) {
                AdvanceSearch.ProQuestSettings = new Object();
                AdvanceSearch.ProQuestSettings.SearchTerm = $("#txtSearchTerm_PQ").val().trim();
                AdvanceSearch.ProQuestSettings.LanguageList = $.inArray("0", $("#ddlLanguage_PQ").val()) !== -1 ? null : $("#ddlLanguage_PQ").val();

                AdvanceSearch.ProQuestSettings.PublicationList = [];
                $.each($("#txtPublication_PQ").val().trim().split(";"), function () {
                    if ($.trim(this) != '') {
                        AdvanceSearch.ProQuestSettings.PublicationList.push($.trim(this).toLowerCase());
                    }
                });

                AdvanceSearch.ProQuestSettings.AuthorList = [];
                $.each($("#txtAuthor_PQ").val().trim().split(";"), function () {
                    if ($.trim(this) != '') {
                        AdvanceSearch.ProQuestSettings.AuthorList.push($.trim(this).toLowerCase());
                    }
                });
            }

            if ((_SearchMediums == '' || $.inArray('LN', _SearchMediums) > -1) && $("#divLNSetup").length > 0) {
                AdvanceSearch.LexisNexisSettings = new Object();
                AdvanceSearch.LexisNexisSettings.SearchTerm = $("#txtSearchTerm_LN").val().trim();
                AdvanceSearch.LexisNexisSettings.CategoryList = $.inArray("0", $("#ddlCategory_LN").val()) !== -1 ? null : $("#ddlCategory_LN").val();
                AdvanceSearch.LexisNexisSettings.PublicationCategoryList = $.inArray("0", $("#ddlPublicationCategory_LN").val()) !== -1 ? null : $("#ddlPublicationCategory_LN").val();
                AdvanceSearch.LexisNexisSettings.GenreList = $.inArray("0", $("#ddlGenre_LN").val()) !== -1 ? null : $("#ddlGenre_LN").val();
                AdvanceSearch.LexisNexisSettings.RegionList = $.inArray("0", $("#ddlRegion_LN").val()) !== -1 ? null : $("#ddlRegion_LN").val();
                AdvanceSearch.LexisNexisSettings.CountryList = $.inArray("0", $("#ddlCountry_LN").val()) !== -1 ? null : $("#ddlCountry_LN").val();
                AdvanceSearch.LexisNexisSettings.LanguageList = $.inArray("0", $("#ddlLanguage_LN").val()) !== -1 ? null : $("#ddlLanguage_LN").val();

                AdvanceSearch.LexisNexisSettings.PublicationList = [];
                $.each($("#txtPublication_LN").val().trim().split(";"), function () {
                    if ($.trim(this) != '') {
                        AdvanceSearch.LexisNexisSettings.PublicationList.push($.trim(this).toLowerCase());
                    }
                });

                AdvanceSearch.LexisNexisSettings.ExcludeDomainList = [];
                $.each($("#txtExcludeDomains_LN").val().trim().split(";"), function () {
                    if ($.trim(this) != '') {
                        AdvanceSearch.LexisNexisSettings.ExcludeDomainList.push($.trim(this).toLowerCase());
                    }
                });
            }

            AdvancedSearchListTemp[ID] = AdvanceSearch;
        }

        // Clear out error messages
        $("#spanExcludeDomains_NM").html('').show();
        $("#spanExcludeDomains_BL").html('').show();
        $("#spanExcludeDomains_FO").html('').show();
        $("#spanExcludeDomains_LN").html('').show();

        ToggleTabEnable(ID);
        return true;
    }

    ToggleTabEnable(ID);
    return false;
}

function IsValidSearch() {
    var flag = true;
    var stringToTest;

    $("#spanExcludeDomains_NM").hide();
    $("#spanExcludeDomains_BL").hide();
    $("#spanExcludeDomains_FO").hide();
    $("#spanExcludeDomains_LN").hide();

    if ((_SearchMediums == '' || $.inArray('NM', _SearchMediums) > -1) && $("#divNMSetup").length > 0 && $.trim($("#txtExcludeDomains_NM").val()) != "") {
        var domains = [];
        $.each($("#txtExcludeDomains_NM").val().trim().split(";"), function () {
            if ($.trim(this) != '') {
                domains.push($.trim(this).toLowerCase());
            }
        });

        $.each(domains, function (index, obj) {
            stringToTest = $.trim(obj);
            if ((/^"/).test(stringToTest) && (/"$/).test(stringToTest)) {
                stringToTest = stringToTest.substring(1, stringToTest.length - 1);
            }

            if (!TestWildInput(stringToTest)) {
                $("#spanExcludeDomains_NM").html(_msgInvalidDomain + obj).show();
                flag = false;
            }
        });

    }

    if ((_SearchMediums == '' || $.inArray('Blog', _SearchMediums) > -1) && $("#divBLSetup").length > 0 && $.trim($("#txtExcludeDomains_BL").val()) != "") {
        var domains = [];
        $.each($("#txtExcludeDomains_BL").val().trim().split(";"), function () {
            if ($.trim(this) != '') {
                domains.push($.trim(this).toLowerCase());
            }
        });

        $.each(domains, function (index, obj) {
            stringToTest = $.trim(obj);
            if ((/^"/).test(stringToTest) && (/"$/).test(stringToTest)) {
                stringToTest = stringToTest.substring(1, stringToTest.length - 1);
            }
            if (!TestWildInput(stringToTest)) {
                $("#spanExcludeDomains_BL").html(_msgInvalidDomain + obj).show();
                flag = false;
            }
        });
    }

    if ((_SearchMediums == '' || $.inArray('Forum', _SearchMediums) > -1) && $("#divFOSetup").length > 0 && $.trim($("#txtExcludeDomains_FO").val()) != "") {
        var domains = [];
        $.each($("#txtExcludeDomains_FO").val().trim().split(";"), function () {
            if ($.trim(this) != '') {
                domains.push($.trim(this).toLowerCase());
            }
        });

        $.each(domains, function (index, obj) {
            stringToTest = $.trim(obj);
            if ((/^"/).test(stringToTest) && (/"$/).test(stringToTest)) {
                stringToTest = stringToTest.substring(1, stringToTest.length - 1);
            }
            if (!TestWildInput(stringToTest)) {
                $("#spanExcludeDomains_FO").html(_msgInvalidDomain + obj).show();
                flag = false;
            }
        });
    }

    if ((_SearchMediums == '' || $.inArray('LN', _SearchMediums) > -1) && $("#divLNSetup").length > 0 && $.trim($("#txtExcludeDomains_LN").val()) != "") {
        var domains = [];
        $.each($("#txtExcludeDomains_LN").val().trim().split(";"), function () {
            if ($.trim(this) != '') {
                domains.push($.trim(this).toLowerCase());
            }
        });

        $.each(domains, function (index, obj) {
            stringToTest = $.trim(obj);
            if ((/^"/).test(stringToTest) && (/"$/).test(stringToTest)) {
                stringToTest = stringToTest.substring(1, stringToTest.length - 1);
            }
            if (!TestWildInput(stringToTest)) {
                $("#spanExcludeDomains_LN").html(_msgInvalidDomain + obj).show();
                flag = false;
            }
        });
    }

    return flag;
}
function IsNewSearch(ID) {
    var isNew = false;
    AdvanceSearch = AdvancedSearchListTemp[ID];

    if (AdvanceSearch != null) {
        if ((_SearchMediums == '' || $.inArray('TV', _SearchMediums) > -1) && $("#divTVSetup").length > 0) {
            var tempCategoryList = $.inArray("0", $("#ddlCategory_TV").val()) !== -1 ? null : $("#ddlCategory_TV").val();
            var tempIQDmaList = $.inArray("0", $("#ddlDMA_TV").val()) !== -1 ? null : $("#ddlDMA_TV").val();
            var tempStationList = $.inArray("0", $("#ddlStation_TV").val()) !== -1 ? null : $("#ddlStation_TV").val();
            var tempAffiliateList = $.inArray("0", $("#ddlAffiliate_TV").val()) !== -1 ? null : $("#ddlAffiliate_TV").val();
            var tempRegionList = $.inArray("0", $("#ddlRegion_TV").val()) !== -1 ? null : $("#ddlRegion_TV").val();
            var tempCountryList = $.inArray("0", $("#ddlCountry_TV").val()) !== -1 ? null : $("#ddlCountry_TV").val();

            if ((AdvanceSearch.TVSettings.SearchTerm != $("#txtSearchTerm_TV").val().trim())
            || (AdvanceSearch.TVSettings.ProgramTitle != $("#txtProgramTitle").val().trim().toLowerCase())
            || (AdvanceSearch.TVSettings.Appearing != $("#txtAppearing").val().trim().toLowerCase())
            || ($(AdvanceSearch.TVSettings.CategoryList).not(tempCategoryList).length != 0 || $(tempCategoryList).not(AdvanceSearch.TVSettings.CategoryList).length != 0)
            || ($(AdvanceSearch.TVSettings.IQDmaList).not(tempIQDmaList).length != 0 || $(tempIQDmaList).not(AdvanceSearch.TVSettings.IQDmaList).length != 0)
            || ($(AdvanceSearch.TVSettings.StationList).not(tempStationList).length != 0 || $(tempStationList).not(AdvanceSearch.TVSettings.StationList).length != 0)
            || ($(AdvanceSearch.TVSettings.AffiliateList).not(tempAffiliateList).length != 0 || $(tempAffiliateList).not(AdvanceSearch.TVSettings.AffiliateList).length != 0)
            || ($(AdvanceSearch.TVSettings.RegionList).not(tempRegionList).length != 0 || $(tempRegionList).not(AdvanceSearch.TVSettings.RegionList).length != 0)
            || ($(AdvanceSearch.TVSettings.CountryList).not(tempCountryList).length != 0 || $(tempCountryList).not(AdvanceSearch.TVSettings.CountryList).length != 0)) {
                isNew = true;
            }
        }

        if ((_SearchMediums == '' || $.inArray('NM', _SearchMediums) > -1) && $("#divNMSetup").length > 0) {
            var tempCategoryList = $.inArray("0", $("#ddlCategory_NM").val()) !== -1 ? null : $("#ddlCategory_NM").val();
            var tempGenreList = $.inArray("0", $("#ddlGenre_NM").val()) !== -1 ? null : $("#ddlGenre_NM").val();
            var tempMarketList = $.inArray("0", $("#ddlMarket_NM").val()) !== -1 ? null : $("#ddlMarket_NM").val();
            var tempPublicationCategoryList = $.inArray("0", $("#ddlPublicationCategory_NM").val()) !== -1 ? null : $("#ddlPublicationCategory_NM").val();
            var tempRegionList = $.inArray("0", $("#ddlRegion_NM").val()) !== -1 ? null : $("#ddlRegion_NM").val();
            var tempCountryList = $.inArray("0", $("#ddlCountry_NM").val()) !== -1 ? null : $("#ddlCountry_NM").val();
            var tempLanguageList = $.inArray("0", $("#ddlLanguage_NM").val()) !== -1 ? null : $("#ddlLanguage_NM").val();

            var NMPublicationsList = [];
            $.each($("#txtPublication_NM").val().trim().split(";"), function () {
                if ($.trim(this) != '') {
                    NMPublicationsList.push($.trim(this).toLowerCase());
                }
            });

            var NMExcludeList = [];
            $.each($("#txtExcludeDomains_NM").val().trim().split(";"), function () {
                if ($.trim(this) != '') {
                    NMExcludeList.push($.trim(this).toLowerCase());
                }
            });

            if ((AdvanceSearch.NewsSettings.SearchTerm != $("#txtSearchTerm_NM").val().trim())
            || ($(AdvanceSearch.NewsSettings.PublicationList).not(NMPublicationsList).length != 0 || $(NMPublicationsList).not(AdvanceSearch.NewsSettings.PublicationList).length != 0)
            || ($(AdvanceSearch.NewsSettings.CategoryList).not(tempCategoryList).length != 0 || $(tempCategoryList).not(AdvanceSearch.NewsSettings.CategoryList).length != 0)
            || ($(AdvanceSearch.NewsSettings.MarketList).not(tempMarketList).length != 0 || $(tempMarketList).not(AdvanceSearch.NewsSettings.MarketList).length != 0)
            || ($(AdvanceSearch.NewsSettings.GenreList).not(tempGenreList).length != 0 || $(tempGenreList).not(AdvanceSearch.NewsSettings.GenreList).length != 0)
            || ($(AdvanceSearch.NewsSettings.PublicationCategoryList).not(tempPublicationCategoryList).length != 0 || $(tempPublicationCategoryList).not(AdvanceSearch.NewsSettings.PublicationCategoryList).length != 0)
            || ($(AdvanceSearch.NewsSettings.RegionList).not(tempRegionList).length != 0 || $(tempRegionList).not(AdvanceSearch.NewsSettings.RegionList).length != 0)
            || ($(AdvanceSearch.NewsSettings.CountryList).not(tempCountryList).length != 0 || $(tempCountryList).not(AdvanceSearch.NewsSettings.CountryList).length != 0)
            || ($(AdvanceSearch.NewsSettings.LanguageList).not(tempLanguageList).length != 0 || $(tempLanguageList).not(AdvanceSearch.NewsSettings.LanguageList).length != 0)
            || ($(AdvanceSearch.NewsSettings.ExcludeDomainList).not(NMExcludeList).length != 0 || $(NMExcludeList).not(AdvanceSearch.NewsSettings.ExcludeDomainList).length != 0)) {
                isNew = true;
            }
        }

        if ((_SearchMediums == '' || $.inArray('Blog', _SearchMediums) > -1) && $("#divBLSetup").length > 0) {
            var tempSourceTypeList = $.inArray("0", $("#ddlSourceType_BL").val()) !== -1 ? null : $("#ddlSourceType_BL").val();

            var BLSourcesList = [];
            $.each($("#txtSource_BL").val().trim().split(";"), function () {
                if ($.trim(this) != '') {
                    BLSourcesList.push($.trim(this).toLowerCase());
                }
            });

            var BLExcludeList = [];
            $.each($("#txtExcludeDomains_BL").val().trim().split(";"), function () {
                if ($.trim(this) != '') {
                    BLExcludeList.push($.trim(this).toLowerCase());
                }
            });

            if ((AdvanceSearch.BlogSettings.SearchTerm != $("#txtSearchTerm_BL").val().trim())
            || (AdvanceSearch.BlogSettings.Author != $("#txtAuthor_BL").val().trim().toLowerCase())
            || (AdvanceSearch.BlogSettings.Title != $("#txtTitle_BL").val().trim().toLowerCase())
            || ($(AdvanceSearch.BlogSettings.SourceList).not(BLSourcesList).length != 0 || $(BLSourcesList).not(AdvanceSearch.BlogSettings.SourceList).length != 0)
            || ($(AdvanceSearch.BlogSettings.ExcludeDomainList).not(BLExcludeList).length != 0 || $(BLExcludeList).not(AdvanceSearch.BlogSettings.ExcludeDomainList).length != 0)) {
                isNew = true;
            }
        }

        if ((_SearchMediums == '' || $.inArray('Forum', _SearchMediums) > -1) && $("#divFOSetup").length > 0) {
            var tempSourceTypeList = $.inArray("0", $("#ddlSourceType_FO").val()) !== -1 ? null : $("#ddlSourceType_FO").val();

            var FOSourcesList = [];
            $.each($("#txtSource_FO").val().trim().split(";"), function () {
                if ($.trim(this) != '') {
                    FOSourcesList.push($.trim(this).toLowerCase());
                }
            });

            var FOExcludeList = [];
            $.each($("#txtExcludeDomains_FO").val().trim().split(";"), function () {
                if ($.trim(this) != '') {
                    FOExcludeList.push($.trim(this).toLowerCase());
                }
            });

            if ((AdvanceSearch.ForumSettings.SearchTerm != $("#txtSearchTerm_FO").val().trim())
            || (AdvanceSearch.ForumSettings.Author != $("#txtAuthor_FO").val().trim().toLowerCase())
            || (AdvanceSearch.ForumSettings.Title != $("#txtTitle_FO").val().trim().toLowerCase())
            || ($(AdvanceSearch.ForumSettings.SourceList).not(FOSourcesList).length != 0 || $(FOSourcesList).not(AdvanceSearch.ForumSettings.SourceList).length != 0)
            || ($(AdvanceSearch.ForumSettings.SourceTypeList).not(tempSourceTypeList).length != 0 || $(tempSourceTypeList).not(AdvanceSearch.ForumSettings.SourceTypeList).length != 0)
            || ($(AdvanceSearch.ForumSettings.ExcludeDomainList).not(FOExcludeList).length != 0 || $(FOExcludeList).not(AdvanceSearch.ForumSettings.ExcludeDomainList).length != 0)) {
                isNew = true;
            }
        }

        if ((_SearchMediums == '' || $.inArray('PQ', _SearchMediums) > -1) && $("#divPQSetup").length > 0) {
            var tempLanguageList = $.inArray("0", $("#ddlLanguage_PQ").val()) !== -1 ? null : $("#ddlLanguage_PQ").val();

            var PQPublicationsList = [];
            $.each($("#txtPublication_PQ").val().trim().split(";"), function () {
                if ($.trim(this) != '') {
                    PQPublicationsList.push($.trim(this).toLowerCase());
                }
            });

            var PQAuthorList = [];
            $.each($("#txtAuthor_PQ").val().trim().split(";"), function () {
                if ($.trim(this) != '') {
                    PQAuthorList.push($.trim(this).toLowerCase());
                }
            });

            if ((AdvanceSearch.ProQuestSettings.SearchTerm != $("#txtSearchTerm_PQ").val().trim())
            || ($(AdvanceSearch.ProQuestSettings.PublicationList).not(PQPublicationsList).length != 0 || $(PQPublicationsList).not(AdvanceSearch.ProQuestSettings.PublicationList).length != 0)
            || ($(AdvanceSearch.ProQuestSettings.AuthorList).not(PQAuthorList).length != 0 || $(PQAuthorList).not(AdvanceSearch.ProQuestSettings.AuthorList).length != 0)
            || ($(AdvanceSearch.ProQuestSettings.LanguageList).not(tempLanguageList).length != 0 || $(tempLanguageList).not(AdvanceSearch.ProQuestSettings.LanguageList).length != 0)) {
                isNew = true;
            }
        }

        if ((_SearchMediums == '' || $.inArray('LN', _SearchMediums) > -1) && $("#divLNSetup").length > 0) {
            var tempCategoryList = $.inArray("0", $("#ddlCategory_LN").val()) !== -1 ? null : $("#ddlCategory_LN").val();
            var tempGenreList = $.inArray("0", $("#ddlGenre_LN").val()) !== -1 ? null : $("#ddlGenre_LN").val();
            var tempPublicationCategoryList = $.inArray("0", $("#ddlPublicationCategory_LN").val()) !== -1 ? null : $("#ddlPublicationCategory_LN").val();
            var tempRegionList = $.inArray("0", $("#ddlRegion_LN").val()) !== -1 ? null : $("#ddlRegion_LN").val();
            var tempCountryList = $.inArray("0", $("#ddlCountry_LN").val()) !== -1 ? null : $("#ddlCountry_LN").val();
            var tempLanguageList = $.inArray("0", $("#ddlLanguage_LN").val()) !== -1 ? null : $("#ddlLanguage_LN").val();

            var LNPublicationsList = [];
            $.each($("#txtPublication_LN").val().trim().split(";"), function () {
                if ($.trim(this) != '') {
                    LNPublicationsList.push($.trim(this).toLowerCase());
                }
            });

            var LNExcludeList = [];
            $.each($("#txtExcludeDomains_LN").val().trim().split(";"), function () {
                if ($.trim(this) != '') {
                    LNExcludeList.push($.trim(this).toLowerCase());
                }
            });

            if ((AdvanceSearch.LexisNexisSettings.SearchTerm != $("#txtSearchTerm_LN").val().trim())
            || ($(AdvanceSearch.LexisNexisSettings.PublicationList).not(LNPublicationsList).length != 0 || $(LNPublicationsList).not(AdvanceSearch.LexisNexisSettings.PublicationList).length != 0)
            || ($(AdvanceSearch.LexisNexisSettings.CategoryList).not(tempCategoryList).length != 0 || $(tempCategoryList).not(AdvanceSearch.LexisNexisSettings.CategoryList).length != 0)
            || ($(AdvanceSearch.LexisNexisSettings.GenreList).not(tempGenreList).length != 0 || $(tempGenreList).not(AdvanceSearch.LexisNexisSettings.GenreList).length != 0)
            || ($(AdvanceSearch.LexisNexisSettings.PublicationCategoryList).not(tempPublicationCategoryList).length != 0 || $(tempPublicationCategoryList).not(AdvanceSearch.LexisNexisSettings.PublicationCategoryList).length != 0)
            || ($(AdvanceSearch.LexisNexisSettings.RegionList).not(tempRegionList).length != 0 || $(tempRegionList).not(AdvanceSearch.LexisNexisSettings.RegionList).length != 0)
            || ($(AdvanceSearch.LexisNexisSettings.CountryList).not(tempCountryList).length != 0 || $(tempCountryList).not(AdvanceSearch.LexisNexisSettings.CountryList).length != 0)
            || ($(AdvanceSearch.LexisNexisSettings.LanguageList).not(tempLanguageList).length != 0 || $(tempLanguageList).not(AdvanceSearch.LexisNexisSettings.LanguageList).length != 0)
            || ($(AdvanceSearch.LexisNexisSettings.ExcludeDomainList).not(LNExcludeList).length != 0 || $(LNExcludeList).not(AdvanceSearch.LexisNexisSettings.ExcludeDomainList).length != 0)) {
                isNew = true;
            }
        }
    }
    else {
        isNew = true;
    }
    return isNew;
}

function SubmitAdvancedSearch() {
    $('#divAdvSearchTabHeader div').each(function () {
        if ($(this).hasClass("pieChartActive")) {
            SetAdvanceSearch($(this).attr('searchID'))
        }
    });

    _IsActiveAdvanceSearch = false;

    if (changedAdvSearch) {
        $.each(AdvancedSearchListTemp, function () {
            if (isDefaultAdvancedSearch(this) == false) {
                _IsActiveAdvanceSearch = true;
                return false;
            }
        });

        _UseAdvanceSearchDefault = isDefaultAdvancedSearch(AdvancedSearchListTemp[mainTabID]) == false;

        ResetSearchTermClassToFalse();
        IsChartUpdated = false;
        _IsToggle = false;

        AdvancedSearchList = AdvancedSearchListTemp;
        SearchResult();
    }

    AdvancedSearchListTemp = new Object();
    closeModal("divAdvanceSearchPopup");
}
function CancelAdvancedSearch() {
    AdvancedSearchListTemp = new Object();
    closeModal("divAdvanceSearchPopup");
}

function SetAdvancedSearchLanguage(language) {
    $('[name="ddlAdvSearchLanguage"]').val(language).trigger("chosen:updated");
}
