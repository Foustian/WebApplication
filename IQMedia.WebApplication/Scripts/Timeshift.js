var _SearchTerm = "";
var _FromDate = null;
var _ToDate = null;
var _Class = "";
var _ClassName = "";
var _Date = null;
var _IsAsc = false;
var _Duration = null;
var _IsNext = false;
var _SortColumn = '';
var _Title = '';
var _IsDefaultLoad = true;
var isFilterEnable = false;
var _CurrentStationFilter = new Array();
var _CurrentDmaFilter = new Array();
var _RequestDma = [];
var _OldRequestDma = [];
var _RequestStation = [];
var _OldRequestStation = [];
var _Country = null;
var _CountryName = '';
var _Region = null;
var _RegionName = '';

var _RequestStationID = [];
var _RequestStationIDDisplay = [];
var _OldRequestStationID = [];
var _CurrentStationIDFilter = [];

var _IsManualHover = false;
var _HasRadioLoaded = false;
var _IsTimeshiftFacet = false;

$(document).ready(function () {

    $('#ulMainMenu li').removeAttr("class");
    $('#liMenuTimeshift').attr("class", "active");

    //SearchResult();  

    var documentHeight = $(window).height();
    $("#divTVScrollContent").css({ "height": documentHeight - 250 });
    $("#divRadioScrollContent").css({ "height": documentHeight - 250 });

    $('#mCSB_1').css({ 'max-height': '' });
    $('#mCSB_2').css({ 'max-height': '' });

    $('#divMessage').html('');
    $('#dpFrom').val('');
    $('#dpTo').val('');

    //    $("#btnSearchTimeshift").click(function () {
    //        //alert('called');
    //        //SetKeyword();
    //        SearchResult();
    //    });
    $("#divCalender").datepicker({
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            $('#dpFrom').val(dateText);
            $('#dpTo').val(dateText);
            SetDateVariable();
        }

    });

    $('.ndate').click(function () {
        $("#divCalender").datepicker("refresh");
        //$("#divRadioCalender").datepicker("refresh");
    });


    $("#dpFrom").datepicker({
        showOn: "button",
        buttonImage: "../images/calender.png",
        buttonImageOnly: true,
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            $('#dpFrom').val(dateText);
            SetDateVariable();
        },
        onClose: function (dateText) {
            $('#dpFrom').focus();
            SetDateVariable();
        }
    });

    $("#dpTo").datepicker({
        showOn: "button",
        buttonImage: "../images/calender.png",
        buttonImageOnly: true,
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            $('#dpTo').val(dateText);
            SetDateVariable();
        },
        onClose: function (dateText) {
            $('#dpTo').focus();
            SetDateVariable();
        }
    });
    $("#txtKeyword").keypress(function (e) {
        if (e.keyCode == 13) {
            SetKeyword();

        }
    });
    $("#txtKeyword").blur(function () {
        SetKeyword();
    });
    $("#imgKeyword").click(function (e) {
        SetKeyword();
    });

    $("#txtTitle").keypress(function (e) {
        if (e.keyCode == 13) {
            SetTitle();

        }
    });
    $("#txtTitle").blur(function () {
        SetTitle();
    });
    $("#imgKeyword").click(function (e) {
        SetTitle();
    });


    $("#divTVScrollContent").mCustomScrollbar({
        advanced: {
            updateOnContentResize: true,
            autoScrollOnFocus: false
        }
    });
    $("#divRadioScrollContent").mCustomScrollbar({
        advanced: {
            updateOnContentResize: true,
            autoScrollOnFocus: false
        }
    });

    $("body").click(function (e) {
        if (e.target.id == "liDmaFilter" || $(e.target).parents("#liDmaFilter").size() > 0) {
            if ($('#ulMarket').is(':visible')) {
                $('#ulMarket').hide();
            }
            else {
                $('#ulMarket').show();
            }
        }
        else if ((e.target.id !== "liDmaFilter" && e.target.id !== "ulMarket" && $(e.target).parents("#ulMarket").size() <= 0) || e.target.id == "btnSearchDma") {
            $('#ulMarket').hide();
            if (e.target.id != "btnSearchDma") {
                var iqdmalist = "";
                $.each(_CurrentDmaFilter, function (eventID, eventData) {
                    if (_OldRequestDma.length > 0 && $.inArray(eventData.Name, _OldRequestDma) !== -1 && $.inArray(eventData.Name, _RequestDma) == -1) {
                        _RequestDma.push(eventData.Name);
                    }
                    var liStyle = "";
                    if ($.inArray(eventData.Name, _RequestDma) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    iqdmalist = iqdmalist + "<li role=\"presentation\" class=\"cursorPointer\"><a " + liStyle + " onclick=\"SetDma(this,'" + eventData.Name.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + eventData.Name + "</a></li>";
                });

                if (iqdmalist != "") {
                    $('#ulMarketList').html(iqdmalist);
                    $('#liDMASearch').show();
                }
                else {
                    $('#ulMarketList').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
                    $('#liDMASearch').hide();
                }
            }
        }

        if (e.target.id == "liStationFilter" || $(e.target).parents("#liStationFilter").size() > 0) {
            if ($('#ulNetwork').is(':visible')) {
                $('#ulNetwork').hide();
            }
            else {
                $('#ulNetwork').show();
            }
        }
        else if ((e.target.id !== "liStationFilter" && e.target.id !== "ulNetwork" && $(e.target).parents("#ulNetwork").size() <= 0) || e.target.id == "btnSearchStation") {
            $('#ulNetwork').hide();
            if (e.target.id != "btnSearchStation") {
                var iqstationlist = "";
                $.each(_CurrentStationFilter, function (eventID, eventData) {
                    if (_OldRequestStation.length > 0 && $.inArray(eventData.Name, _OldRequestStation) !== -1 && $.inArray(eventData.Name, _RequestStation) == -1) {
                        _RequestStation.push(eventData.Name);
                    }
                    var liStyle = "";
                    if ($.inArray(eventData.Name, _RequestStation) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    iqstationlist = iqstationlist + "<li role=\"presentation\" class=\"cursorPointer\"><a " + liStyle + " onclick=\"SetStation(this,'" + eventData.Name.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + eventData.Name + "</a></li>";
                });

                if (iqstationlist != "") {
                    $('#ulNetworkList').html(iqstationlist);
                    $('#liStationSearch').show();
                }
                else {
                    $('#ulNetworkList').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
                    $('#liStationSearch').hide();
                }
            }
        }


        if (e.target.id == "liStationIDFilter" || $(e.target).parents("#liStationIDFilter").size() > 0) {
            if ($('#ulStation').is(':visible')) {
                $('#ulStation').hide();
            }
            else {
                $('#ulStation').show();
            }
        }
        else if ((e.target.id !== "liStationIDFilter" && e.target.id !== "ulStation" && $(e.target).parents("#ulStation").size() <= 0) || e.target.id == "btnSearchStationID") {
            $('#ulStation').hide();
            if (e.target.id != "btnSearchStationID") {
                var iqstationlist = "";
                $.each(_CurrentStationIDFilter, function (eventID, eventData) {
                    if (_OldRequestStationID.length > 0 && $.inArray(eventData.IQ_Station_ID, _OldRequestStationID) !== -1 && $.inArray(eventData.IQ_Station_ID, _RequestStationID) == -1) {
                        _RequestStationID.push(eventData.IQ_Station_ID);
                        _RequestStationIDDisplay.push(eventData.Station_Call_Sign);
                    }
                    var liStyle = "";
                    if ($.inArray(eventData.IQ_Station_ID, _RequestStationID) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    iqstationlist = iqstationlist + "<li role=\"presentation\" class=\"cursorPointer\"><a " + liStyle + " onclick=\"SetStationID(this,'" + eventData.IQ_Station_ID.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "','" + eventData.Station_Call_Sign.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + eventData.Station_Call_Sign + "</a></li>";
                });

                if (iqstationlist != "") {
                    $('#ulStationList').html(iqstationlist);
                    $('#liStationIDSearch').show();
                }
                else {
                    $('#ulStationList').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
                    $('#liStationIDSearch').hide();
                }
            }
        }


    });

    $.ajax({
        type: 'POST',
        url: _urlTimeshiftGetIsTimeshiftFacet,
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            if (result.isSuccess) {
                _IsTimeshiftFacet = result.isTimeshiftFacet;
            }
            else {
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured);
        }
    });

    GetSavedSearch(false, true);
});

$(window).resize(function () {
    if (screen.height >= 768) {
        $("#divTVScrollContent").css({ "height": documentHeight - 250 });
        $("#divRadioScrollContent").css({ "height": documentHeight - 250 });
    }
});

function CollapseExpandLeftSection(sectionid) {

    if (sectionid == 1) {

        $("#divTVSection").show("2000");
        $("#divTVContent").show("2000");

        $("#divRadioSection").hide("2000");
        $("#divRadioContent").hide("200");


        $("#h5TV").removeAttr("class");
        $("#h5TV").addClass("tvheader-active");

        $("#h5Radio").removeAttr("class");
        $("#h5Radio").addClass("radioheader-inactive");

        setTimeout(function () { $("#divTVScrollContent").mCustomScrollbar("scrollTo", "top"); }, 200);
    }
    else {

        $("#divTVSection").hide("2000");
        $("#divTVContent").hide("2000");

        $("#divRadioSection").show("2000");
        $("#divRadioContent").show("200");

        $("#h5Radio").removeAttr("class");
        $("#h5Radio").addClass("radioheader-active");

        $("#h5TV").removeAttr("class");
        $("#h5TV").addClass("tvheader-inactive");

        if (!_HasRadioLoaded) {
            RefreshRadioResults(false, false);
            _HasRadioLoaded = true;
        }
        else {
            setTimeout(function () { $("#divRadioScrollContent").mCustomScrollbar("scrollTo", "top"); }, 200);
        }
    }
}

function SetKeyword() {
    if ($("#txtKeyword").val() != "" && _SearchTerm != $("#txtKeyword").val()) {
        _SearchTerm = $("#txtKeyword").val();
        _IsDefaultLoad = false;
        SearchResult();
        // RefreshResults();
    }
}

function SetTitle() {
    if ($("#txtTitle").val() != "" && _Title != $("#txtTitle").val()) {
        _Title = $("#txtTitle").val();
        _IsDefaultLoad = false;
        SearchResult();
        // RefreshResults();
    }
}

function SetDateVariable() {

    if ($("#dpFrom").val() && $("#dpTo").val()) {
        $('#dpFrom').removeClass('warningInput');
        $('#dpTo').removeClass('warningInput');
        if (_FromDate != $("#dpFrom").val() || _ToDate != $("#dpTo").val()) {
            _FromDate = $("#dpFrom").val();
            _ToDate = $("#dpTo").val();
            _IsDefaultLoad = false;
            SearchResult();
            $('#ulCalender').parent().removeClass('open');
            $('#aDuration').html('Custom&nbsp;&nbsp;<span class="caret"></span>');
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
function SetDma(eleDma, dmaname) {
    if ($.inArray(dmaname, _RequestDma) == -1) {
        _RequestDma.push(dmaname);
        $(eleDma).css("background-color", "#E9E9E9");
    }
    else {
        var catIndex = _RequestDma.indexOf(dmaname);
        if (catIndex > -1) {
            _RequestDma.splice(catIndex, 1);
            $(eleDma).css("background-color", "#ffffff");
        }
    }
}

function SearchDma() {
    if ($(_RequestDma).not(_OldRequestDma).length != 0 || $(_OldRequestDma).not(_RequestDma).length != 0) {
        _OldRequestDma = _RequestDma.slice(0);
        _IsDefaultLoad = false;
        SearchResult();
    }
}

function SetStation(eleStation, stationname) {
    if ($.inArray(stationname, _RequestStation) == -1) {
        _RequestStation.push(stationname);
        $(eleStation).css("background-color", "#E9E9E9");
    }
    else {
        var catIndex = _RequestStation.indexOf(stationname);
        if (catIndex > -1) {
            _RequestStation.splice(catIndex, 1);
            $(eleStation).css("background-color", "#ffffff");
        }
    }
}

function SearchStation() {
    if ($(_RequestStation).not(_OldRequestStation).length != 0 || $(_OldRequestStation).not(_RequestStation).length != 0) {
        _OldRequestStation = _RequestStation.slice(0);
        _IsDefaultLoad = false;
        SearchResult();
    }
}


function SetStationID(eleStation, stationid, stationiddisplay) {
    if ($.inArray(stationid, _RequestStationID) == -1) {
        _RequestStationID.push(stationid);
        _RequestStationIDDisplay.push(stationiddisplay);
        $(eleStation).css("background-color", "#E9E9E9");
    }
    else {
        var catIndex = _RequestStationID.indexOf(stationid);
        if (catIndex > -1) {
            _RequestStationID.splice(catIndex, 1);
            _RequestStationIDDisplay.splice(catIndex, 1);
            $(eleStation).css("background-color", "#ffffff");
        }
    }
}

function SearchStationID() {
    if ($(_RequestStationID).not(_OldRequestStationID).length != 0 || $(_OldRequestStationID).not(_RequestStationID).length != 0) {
        _OldRequestStationID = _RequestStationID.slice(0);
        _IsDefaultLoad = false;
        SearchResult();
    }
}

function SetClass(classnum, classname) {
    _Class = classnum;
    _ClassName = classname;
    _IsDefaultLoad = false;
    SearchResult();
}

function SetCountry(countrynum, countryname) {
    _Country = countrynum;
    _CountryName = countryname;
    _IsDefaultLoad = false;
    SearchResult();
}

function SetRegion(regionnum, regionname) {
    _Region = regionnum;
    _RegionName = regionname;
    _IsDefaultLoad = false;
    SearchResult();
}

function SearchResult() {
    var jsonPostData = {
        p_FromDate: _FromDate,
        p_ToDate: _ToDate,
        p_SearchTerm: _SearchTerm,
        p_Title: _Title,
        p_Dma: _RequestDma,
        p_Station: _RequestStation,
        p_IQStationID: _RequestStationID,
        p_Class: _Class,
        p_Region: _Region,
        p_Country: _Country,
        p_IsAsc: _IsAsc,
        p_SortColumn: _SortColumn,
        p_IsDefaultLoad: _IsDefaultLoad
    }

    // alert('searchcalled');
    if (DateValidation()) {
        SetActiveFilter();

        if (isFilterEnable) {
            $('#divTimeshiftClearAll').show();
        }
        else {
            $('#divTimeshiftClearAll').hide();
        }

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlTimeshiftSearchResults,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            success: OnResultSearchComplete,
            error: OnFail
        });
    }


}
function SearchResultPaging(isNextPage) {
    // alert(isNextPage);
    _IsNext = isNextPage;
    var jsonPostData = {
        p_FromDate: _FromDate,
        p_ToDate: _ToDate,
        p_SearchTerm: _SearchTerm,
        p_Title: _Title,
        p_Dma: _RequestDma,
        p_Station: _RequestStation,
        p_IQStationID: _RequestStationID,
        p_Class: _Class,
        p_Region: _Region,
        p_Country: _Country,
        p_IsAsc: _IsAsc,
        p_IsNext: _IsNext,
        p_SortColumn: _SortColumn
    }

    // alert('searchcalled');
    if (DateValidation()) {
        SetActiveFilter();
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlTimeshiftSearchResultsPaging,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            success: OnResultSearchComplete,
            error: OnFail
        });
    }


}
function OnResultSearchComplete(result) {

    if (result.isSuccess) {
        if (_IsDefaultLoad)
            GetSavedSearch(false, true);
        $("#divPreviousNext").show();
        //alert('done');
        //alert(result.HTML);
        $('#lblRecords').html('');
        $('#ulTimeshiftResults').html('');
        $('#ulTimeshiftResults').html(result.HTML);
        if (result.hasMoreResult) {
            $('#btnNextPage').show();
        }
        else {
            $('#btnNextPage').hide();
        }

        if (result.hasPreviouResult) {
            $('#btnPreviousPage').show();
        }
        else {
            $('#btnPreviousPage').hide();
        }
        if (result.recordNumber != '') {
            $('#lblRecords').html(result.recordNumber);
        }

        if (result.filters != null) {
            if (result.filters["IQ_Dma"] != null) {
                var iqdmalist = "";
                $.each(result.filters["IQ_Dma"], function (index, obj) {
                    var liStyle = "";
                    if ($.inArray(obj.Name, _RequestDma) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    iqdmalist = iqdmalist + "<li role=\"presentation\" class=\"cursorPointer\"><a " + liStyle + " onclick=\"SetDma(this,'" + obj.Name.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + obj.Name + "</a></li>";
                });
                $("#ulMarketList").html(iqdmalist);
                _CurrentDmaFilter = result.filters["IQ_Dma"].slice(0);
            }
            if (result.filters["Station_Affil"] != null) {
                var iqstationlist = "";
                $.each(result.filters["Station_Affil"], function (index, obj) {
                    var liStyle = "";
                    if ($.inArray(obj.Name, _RequestStation) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    iqstationlist = iqstationlist + "<li role=\"presentation\" class=\"cursorPointer\"><a " + liStyle + " onclick=\"SetStation(this,'" + obj.Name.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + obj.Name + "</a></li>";
                });
                $("#ulNetworkList").html(iqstationlist);
                _CurrentStationFilter = result.filters["Station_Affil"].slice(0);
            }


            if (result.filters["IQ_Station"] != null) {
                var iqstationlist = "";
                $.each(result.filters["IQ_Station"], function (index, obj) {
                    var liStyle = "";
                    if ($.inArray(obj.IQ_Station_ID, _RequestStationID) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    iqstationlist = iqstationlist + "<li role=\"presentation\" class=\"cursorPointer\"><a " + liStyle + " onclick=\"SetStationID(this,'" + obj.IQ_Station_ID.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "','" + obj.Station_Call_Sign.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + obj.Station_Call_Sign + "</a></li>";
                });
                $("#ulStationList").html(iqstationlist);
                _CurrentStationIDFilter = result.filters["IQ_Station"].slice(0);

                if (result.filters["IQ_Country"] != null) {
                    var iqcountrylist = "";
                    $.each(result.filters["IQ_Country"], function (index, obj) {
                        iqcountrylist = iqcountrylist + "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetCountry(" + obj.Num + ",'" + obj.Name.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + obj.Name + "</a></li>";
                    });
                    $("#ulCountry").html(iqcountrylist);
                }

                if (result.filters["IQ_Region"] != null) {
                    var iqregionlist = "";
                    $.each(result.filters["IQ_Region"], function (index, obj) {
                        iqregionlist = iqregionlist + "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetRegion(" + obj.Num + ",'" + obj.Name.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + obj.Name + "</a></li>";
                    });
                    $("#ulRegion").html(iqregionlist);
                }
            }
        }

        // For legal reasons, only show additional search fields if a keyword is entered, unless user has a certain role
        if (!_IsTimeshiftFacet) {
            if (_SearchTerm.trim() != "") {
                $("#divAdvancedSearch").show();
            }
            else {
                $("#divAdvancedSearch").hide();
            }
        }
    }
    else {
        ClearResultsOnError('ulTimeshiftResults', 'divPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "SearchResult()"));
    }



}
function OnFail(result) {
    _IsTabChange = false;

    ShowNotification(_msgErrorOccured);
    ClearResultsOnError('ulTimeshiftResults', 'divPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "SearchResult()"));
    if (_IsDefaultLoad)
        GetSavedSearch(false, true);
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
function SetActiveFilter() {

    isFilterEnable = false;
    $("#divActiveFilter").html("");


    if (_SearchTerm != null && _SearchTerm != "") {
        $('#divActiveFilter').append('<div id="divKeywordActiveFilter" class="filter-in">' + EscapeHTML(_SearchTerm) + '<span class="cancel" onclick="RemoveFilter(1);"></span></div>');
        isFilterEnable = true;
    }

    if ((_FromDate != null && _FromDate != "") && (_ToDate != null && _ToDate != "")) {
        $('#divActiveFilter').append('<div id="divDateActiveFilter" class="filter-in">' + _FromDate + ' To ' + _ToDate + '<span class="cancel" onclick="RemoveFilter(2);"></span></div>');
        isFilterEnable = true;
    }
    if (_RequestDma != null && _RequestDma.length > 0) {
        $('#divActiveFilter').append('<div id="divDmaActiveFilter" class="filter-in">' + _RequestDma.join() + '<span class="cancel" onclick="RemoveFilter(3);"></span></div>');
        isFilterEnable = true;
    }
    if (_RequestStation != null && _RequestStation.length > 0) {
        $('#divActiveFilter').append('<div id="divStationActiveFilter" class="filter-in">' + _RequestStation.join() + '<span class="cancel" onclick="RemoveFilter(4);"></span></div>');
        isFilterEnable = true;
    }
    if (_RequestStationIDDisplay != null && _RequestStationIDDisplay.length > 0) {
        $('#divActiveFilter').append('<div id="divStationIDActiveFilter" class="filter-in">' + _RequestStationIDDisplay.join() + '<span class="cancel" onclick="RemoveFilter(7);"></span></div>');
        isFilterEnable = true;
    }
    if (_Class != null && _Class != "") {
        $('#divActiveFilter').append('<div id="divClassActiveFilter" class="filter-in">' + _ClassName + '<span class="cancel" onclick="RemoveFilter(5);"></span></div>');
        isFilterEnable = true;
    }
    if (_Title != null && _Title != "") {
        $('#divActiveFilter').append('<div id="divTitleActiveFilter" class="filter-in">' + EscapeHTML(_Title) + '<span class="cancel" onclick="RemoveFilter(6);"></span></div>');
        isFilterEnable = true;
    }

    if (_Region != null && _Region != "") {
        $('#divActiveFilter').append('<div id="divRegionActiveFilter" class="filter-in">' + _RegionName + '<span class="cancel" onclick="RemoveFilter(8);"></span></div>');
        isFilterEnable = true;
    }
    if (_Country != null && _Country != "") {
        $('#divActiveFilter').append('<div id="divCountryActiveFilter" class="filter-in">' + _CountryName + '<span class="cancel" onclick="RemoveFilter(9);"></span></div>');
        isFilterEnable = true;
    }

    if (isFilterEnable) {
        $("#divActiveFilter").css({ 'border-bottom': '1px solid rgb(236, 236, 236)', 'margin-bottom': '5px' });
    }
    else {
        $("#divActiveFilter").css({ 'border-bottom': '' });
    }
}

function RemoveFilter(filterType) {

    var refreshResults = false;

    _IsDefaultLoad = false;
    // Represent SearchKeyword
    if (filterType == 1) {

        $("#txtKeyword").val("");
        _SearchTerm = "";
        refreshResults = true;
    }

    // Represent Date filter(From Date,To Date)
    if (filterType == 2 || (!_IsTimeshiftFacet && filterType == 1)) {

        $("#dpFrom").datepicker("setDate", null);
        $("#dpTo").datepicker("setDate", null);
        $("#divCalender").datepicker("setDate", null);

        $('#aDuration').html(_msgAll + '&nbsp;&nbsp;<span class="caret"></span>');

        _FromDate = null;
        _ToDate = null;
        refreshResults = true;
    }

    // Represent Dma Filter
    if (filterType == 3 || (!_IsTimeshiftFacet && filterType == 1)) {
        _RequestDma = [];
        _OldRequestDma = [];
        refreshResults = true;
    }

    // Represent Station Filter
    if (filterType == 4 || (!_IsTimeshiftFacet && filterType == 1)) {
        _RequestStation = [];
        _OldRequestStation = [];
        refreshResults = true;
    }
    // Represent Class Filter
    if (filterType == 5 || (!_IsTimeshiftFacet && filterType == 1)) {
        _Class = "";
        _ClassName = "";
        refreshResults = true;
    }

    if (filterType == 6 || (!_IsTimeshiftFacet && filterType == 1)) {

        $("#txtTitle").val("");
        _Title = "";
        refreshResults = true;
    }

    // Represent Station Filter
    if (filterType == 7 || (!_IsTimeshiftFacet && filterType == 1)) {
        _RequestStationID = [];
        _RequestStationIDDisplay = [];
        _OldRequestStationID = [];
        refreshResults = true;
    }

    if (filterType == 8 || (!_IsTimeshiftFacet && filterType == 1)) {
        _Region = null;
        _RegionName = "";
        refreshResults = true;
    }

    if (filterType == 9 || (!_IsTimeshiftFacet && filterType == 1)) {
        _Country = null;
        _CountryName = "";
        refreshResults = true;
    }

    if (refreshResults) {
        SearchResult();
    }
}
function SortDirection(p_SortColumn, p_IsAsc) {

    if (p_IsAsc != _IsAsc || _SortColumn != p_SortColumn) {
        _IsAsc = p_IsAsc;
        _SortColumn = p_SortColumn;

        if (_SortColumn == 'Date' && _IsAsc) {
            $('#aSortDirection').html(_msgOldestFirst + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (_SortColumn == 'Date' && !_IsAsc) {
            $('#aSortDirection').html(_msgMostRecent + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (_SortColumn == 'Market' && _IsAsc) {
            $('#aSortDirection').html(_msgMarketAscending + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (_SortColumn == 'Market' && !_IsAsc) {
            $('#aSortDirection').html(_msgMarketDescending + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        _IsDefaultLoad = false;
        SearchResult();
    }
}
function GetResultOnDuration(duration) {

    $("#dpFrom").removeClass("warningInput");
    $("#dpTo").removeClass("warningInput");
    var dtcurrent = new Date();
    var fDate;
    _Duration = duration;

    // All
    if (duration == 0) {
        $("#dpFrom").val("");
        $("#dpTo").val("");
        dtcurrent = "";
        RemoveFilter(2);
        $('#aDuration').html(_msgAll + '&nbsp;&nbsp;<span class="caret"></span>');
    }

    // 24 hours
    else if (duration == 1) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth(), dtcurrent.getDate() - 1);
        $('#aDuration').html(_msgLast24Hours + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    // Last week
    else if (duration == 2) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth(), dtcurrent.getDate() - 7);
        $('#aDuration').html(_msgLastWeek + '&nbsp;&nbsp;<span class="caret"></span>');
    }

    // Last Month
    else if (duration == 3) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth() - 1, dtcurrent.getDate());
        $('#aDuration').html(_msgLastMonth + '&nbsp;&nbsp;<span class="caret"></span>');
    }

    // Last 3 Months
    else if (duration == 4) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth() - 3, dtcurrent.getDate());
        $('#aDuration').html(_msgLast3Month + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    // Custom
    else if (duration == 5) {

        dtcurrent = null;
        if ($("#dpFrom").val() != "" && $("#dpTo").val() != "") {
            $('#aDuration').html(_msgCustom + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        else {
            if ($("#dpFrom").val() == "") {
                $("#dpFrom").addClass("warningInput");
            }
            if ($("#dpTo").val() == "") {
                $("#dpTo").addClass("warningInput");
            }
            ShowNotification(_msgSelectDate);
        }
    }

    $("#dpFrom").datepicker("setDate", fDate);
    $("#dpTo").datepicker("setDate", dtcurrent);

    if ($("#dpFrom").val() != "" && $("#dpTo").val() != "") {

        if (_FromDate != $("#dpFrom").val() || _ToDate != $("#dpTo").val()) {
            _FromDate = $("#dpFrom").val();
            _ToDate = $("#dpTo").val();
            _IsDefaultLoad = false;
            SearchResult();
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

function GetSavedSearch(p_isNext, p_isInitialize) {
    //$('#divSavedSearch').html(_imgLoading);

    var jsonPostData = {
        isNext: p_isNext,
        isInitialize: p_isInitialize
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlTimeshiftGetSaveSearch,
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
        $('#spnSavedSearchRecordDetail').html(result.saveSearchRecordDetail);
    }
    else {

        //CheckForAuthentication(result, 'Some error occured, try again later');
        $('#divSavedSearch').html('An error occured,<a class="cursorPointer" onclick="GetSavedSearch(false, true);">try again</a>');
        //ShowNotification('Some error occured, try again later');
    }
}

function OnGetSaveSearchFail(result) {
    //alert('GetSaveSearchFail - Local');
    //ShowNotification('Some error occured, try again later');
    $('#divSavedSearch').html('An error occured, <a class="cursorPointer" onclick="GetSavedSearch(false, true);">try again</a>');
}

function SetSavedSearchHTML(result) {
    $('#divSavedSearch').removeClass('text-align-center');
    $('#divSavedSearch').html(result.HTML);

    if (result.isPreviousAvailable) {
        $('#aSavedSearchPrevious').attr("onclick", "GetSavedSearch(false,false);");
        $('#aSavedSearchPrevious').show(); //  removeClass("inactiveLink");
    }
    else {
        $('#aSavedSearchPrevious').removeAttr("onclick");
        $('#aSavedSearchPrevious').hide(); // addClass("inactiveLink");
    }


    if (result.HasMoreResult) {
        $('#aSavedSearchNext').attr("onclick", "GetSavedSearch(true,false);");
        $('#aSavedSearchNext').show(); //  removeClass("inactiveLink");
    }
    else {
        $('#aSavedSearchNext').removeAttr("onclick");
        $('#aSavedSearchNext').hide(); //  addClass("inactiveLink");
    }
}

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

function ValidateSaveSearchInput() {
    var isValid = true;

    if ($('#txtSaveSearchPopup').val().trim() == '') {

        $('#txtSaveSearchPopup').css('border', '1px red solid');
        $('#txtSaveSearchPopup').addClass('boxshadow');

        setTimeout(function () {
            $('#txtSaveSearchPopup').removeClass('boxshadow');
        }, 2000);

        ShowNotification(_msgEnterSaveSearchTitle);

        isValid = false;
    }
    return isValid;
}

function SaveSearch() {
    if (ValidateSaveSearchInput()) {
        $('#imgSaveSearchLoading').show();

        var ListIQ_Station = new Array()
        $.each(_RequestStationID, function (index, obj) {
            ListIQ_Station.push({ Station_Call_Sign: _RequestStationIDDisplay[index], IQ_Station_ID: obj });
        });

        var jsonPostData = {
            p_Title: $('#txtSaveSearchPopup').val().trim(),
            p_SearchTerm: {
                SearchTerm: $('#txtKeyword').val().trim(),
                Title120: $('#txtTitle').val().trim(),
                FromDate: _FromDate,
                ToDate: _ToDate,
                Dma: _RequestDma,
                Station: _RequestStation,
                IQStationID: ListIQ_Station,
                Category: _Class,
                CategoryName: _ClassName,
                Region: _Region,
                RegionName: _RegionName,
                Country: _Country,
                CountryName: _CountryName
            }
        }

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlTimeshiftSaveSearch,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),

            success: OnSaveSearchComplete,
            error: OnSaveSearchFail
        });
    }
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
        //ShowNotification('Some error occured, try again later');
    }
}

function OnSaveSearchFail(result) {
    $('#imgSaveSearchLoading').hide();
    $('#divPopover').remove();
    //alert('On Save Search Fail - Local');
    ShowNotification(_msgErrorOccured);
}



function DeleteSavedSearchByID(ID) {
    var jsonPostData = {
        p_ID: ID
    }

    getConfirm("Delete Saved Search", _msgConfirmSavedSearchDelete, "Confirm Delete", "Cancel", function (res) {
        if (res) {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: _urlTimeshiftDeleteSavedSearchByID,
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
        //ShowNotification('Some error occured, try again later');
    }
}

function OnDeleteSaveSearchFail(result) {
    //alert('OnDeleteSaveSearchFail - Local');
    ShowNotification(_msgErrorOccured);
}

function LoadSavedSearch(ID) {
    var jsonPostData = {
        p_ID: ID
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlTimeshiftLoadSavedSearch,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),

        success: OnLoadSaveSearchComplete,
        error: OnLoadSaveSearchFail
    });
}

function OnLoadSaveSearchComplete(result) {
    if (result.isSuccess) {

        _FromDate = result.timeshift_SavedSearch.SearchTerm.FromDate;
        _ToDate = result.timeshift_SavedSearch.SearchTerm.ToDate;
        _SearchTerm = result.timeshift_SavedSearch.SearchTerm.SearchTerm;
        _RequestDma = result.timeshift_SavedSearch.SearchTerm.Dma;
        _RequestStation = result.timeshift_SavedSearch.SearchTerm.Station;

        _RequestStationID = [];
        _RequestStationIDDisplay = [];

        $.each(result.timeshift_SavedSearch.SearchTerm.IQStationID, function (index, obj) {
            _RequestStationID.push(obj.IQ_Station_ID);
            _RequestStationIDDisplay.push(obj.Station_Call_Sign);
        });
        
        _Class = result.timeshift_SavedSearch.SearchTerm.Category;
        _ClassName = result.timeshift_SavedSearch.SearchTerm.CategoryName;

        _Country = result.timeshift_SavedSearch.SearchTerm.Country;
        _CountryName = result.timeshift_SavedSearch.SearchTerm.CountryName;

        _Region = result.timeshift_SavedSearch.SearchTerm.Region;
        _RegionName = result.timeshift_SavedSearch.SearchTerm.RegionName;

        _Title = result.timeshift_SavedSearch.SearchTerm.Title120;

        $('#txtKeyword').val(_SearchTerm);
        $('#txtTitle').val(_Title);
        $("#dpFrom").datepicker("setDate", _FromDate);
        $("#dpTo").datepicker("setDate", _ToDate);

        SetSavedSearchHTML(result);
        $('#spnSavedSearchRecordDetail').html(result.saveSearchRecordDetail);
        _IsDefaultLoad = false;
        SearchResult();
    }
    else {
        ShowNotification(_msgErrorOccured);
    }
}

function OnLoadSaveSearchFail(result) {
    ShowNotification(_msgErrorOccured);
}

function OpenSavedSearchPopup(p_id, p_title) {
    $('#txtSavedSearchTitle').removeClass('boxshadow');
    $('#txtSavedSearchTitle').css("border", "");
    $('#spnUpdateSavedSearchPopupNote').html('');
    $('#txtSavedSearchTitle').val(p_title);
    $('#hdnSavedSearchID').val(p_id);
    $('#divUpdateSavedSearchPopup').modal({
        backdrop: 'static',
        keyboard: true,
        dynamic: true
    });
}

function UpdateTimeshiftSavedSearch() {
    if ($('#txtSavedSearchTitle').val().trim() != '') {
        $('#imgUpdateSaveSearchLoading').show();

        var ListIQ_Station = new Array()
        $.each(_RequestStationID, function (index, obj) {
            ListIQ_Station.push({ Station_Call_Sign: _RequestStationIDDisplay[index], IQ_Station_ID: obj });
        });

        var jsonPostData = {
            p_ID: $('#hdnSavedSearchID').val(),
            p_Title: $('#txtSavedSearchTitle').val().trim(),
            p_SearchTerm: {
                SearchTerm: $('#txtKeyword').val().trim(),
                Title120: $('#txtTitle').val().trim(),
                FromDate: _FromDate,
                ToDate: _ToDate,
                Dma: _RequestDma,
                Station: _RequestStation,
                IQStationID: ListIQ_Station,
                Category: _Class,
                CategoryName: _ClassName,
                Region: _Region,
                RegionName: _RegionName,
                Country: _Country,
                CountryName: _CountryName
            }
        }

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlTimeshiftUpdateSavedSearch,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                $('#imgUpdateSaveSearchLoading').hide();
                if (result.isError) {
                    ClosePopUp('divUpdateSavedSearchPopup')
                    CheckForAuthentication(result, _msgErrorOccured);
                }
                else if (result.isSuccess) {
                    ClosePopUp('divUpdateSavedSearchPopup')
                    ShowNotification(result.message);
                    GetSavedSearch(false, true);
                }
                else {
                    $('#spnUpdateSavedSearchPopupNote').html(result.message)
                }
            },
            error: function (a, b, c) {
                $('#imgUpdateSaveSearchLoading').hide();
                ShowNotification(_msgErrorOccured);
            }
        });
    }
    else {
        $('#txtSavedSearchTitle').css('border', '1px red solid');
        $('#txtSavedSearchTitle').addClass('boxshadow');

        setTimeout(function () {
            $('#txtSavedSearchTitle').removeClass('boxshadow');
        }, 2000);
    }
}

function ClosePopUp(divID) {
    $('#' + divID).css({ "display": "none" });
    $('#' + divID).modal('hide');
}

function ClearSearch() {
    _FromDate = null;
    _ToDate = null;
    _SearchTerm = "";
    _RequestDma = [];
    _OldRequestDma = [];
    _RequestStation = [];
    _OldRequestStation = [];
    _RequestStationID = [];
    _RequestStationIDDisplay = [];
    _OldRequestStationID = [];
    _Class = "";
    _ClassName = "";
    _Country = null;
    _Region = null;
    _Title = "";
    _IsAsc = false;
    _SortColumn = "";
    _IsDefaultLoad = true;

    $('#aDuration').html(_msgAll + '&nbsp;&nbsp;<span class="caret"></span>');
    $("#dpFrom").datepicker("setDate", null);
    $("#dpTo").datepicker("setDate", null);
    $("#divCalender").datepicker("setDate", null);
    $("#txtTitle").val("");
    $("#txtKeyword").val("");

    SearchResult();
}




function LoadChartNPlayer(rawMediaGuid, iqcckey, title) {
    LoadPlayerbyGuidTS(rawMediaGuid, title);
    var jsonPostData = {
        IQ_CC_KEY: iqcckey
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlTimeShiftGetChart,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),
        success: OnGetChartComplete,
        error: OnGetChartFail
    });


}

function OnGetChartComplete(result) {
    if (result.isSuccess) {
        if (result.isTimeSync && result.lineChartJson.length > 0) {
            $('.video-chart').closest('li').show();
            $('.chart-tabs').html('');
            $('.chart-tab-content').html('');
            $.each(result.lineChartJson, function (index, obj) {
                $('.chart-tabs').append('<div class="chartTabHeader" id="video-parent-tab-' + index + '"><div class="padding5" id="video-tab-' + index + '" onclick="changeChartTab(' + index + ');">' + obj.Type + '</div></div>');
                $('.chart-tab-content').append('<div class="float-left" id="video-chart-' + index + '" style="width:1020px;"></div>');
                RenderHighCharts(obj.Data, 'video-chart-' + index);
            });

            changeChartTab(0);
            $(".chart-tab-content").on("mouseout", function () {
                _IsManualHover = false;
            });
        }
    }
    else {
        ShowNotification(_msgErrorOccured);
    }
}

function OnFail(result) {
    ShowNotification(_msgErrorOccured);
}

function OnGetChartFail(result) {
    ShowNotification(_msgErrorOccured);
}

function RenderHighCharts(jsonLineChartData, chartID) {

    var JsonLineChart = JSON.parse(jsonLineChartData);
    JsonLineChart.plotOptions.series.point.events.click = LineChartClick;
    JsonLineChart.xAxis.labels.formatter = FormatTime
    //JsonLineChart.tooltip.positioner = TooltipSetY
    JsonLineChart.tooltip.formatter = tooltipFormat
    JsonLineChart.plotOptions.series.events.show = HandleSeriesShowHide;
    JsonLineChart.plotOptions.series.events.hide = HandleSeriesShowHide;
    JsonLineChart.plotOptions.series.point.events.mouseOver = ChartHoverManage;
    JsonLineChart.plotOptions.series.point.events.mouseOut = ChartHoverOutManage;

    //    JsonLineChart.chart.events = new Object();
    //    JsonLineChart.chart.events.load = new Object(function () {
    //        var chartContiner = this;
    //        setTimeout(function () {
    //            // solution: 
    //            chartContiner.setSize($('#' + chartID).width(), $('#' + chartID).height());
    //        }, 1000);
    //    });



    //    JsonLineChart.legend.x = -400;
    //    JsonLineChart.xAxis.tickInterval = parseInt(Math.floor(JsonLineChart.xAxis.categories.length / 12)),
    //    JsonLineChart.chart.width = null;


    $('#' + chartID).highcharts(JsonLineChart);
}

function HandleSeriesShowHide() {

    if (!_IsManualHover) {
        var chart = this.chart;
        xIndex = chart.axes[0].categories.indexOf(_currentTimeInt.toString());
        if (chart.series[0].visible || chart.series[1].visible) {
            if (chart.series[0].visible && chart.series[1].visible) {
                chart.tooltip.refresh([chart.series[0].data[xIndex], chart.series[1].data[xIndex]]);
            }
            else if (chart.series[0].visible) {
                chart.tooltip.refresh([chart.series[0].data[xIndex]]);
            }
            else {
                chart.tooltip.refresh([chart.series[1].data[xIndex]]);
            }
        }
        else {
            chart.series[0].data[xIndex].setState('');
            chart.series[1].data[xIndex].setState('');
            //chart.tooltip.refresh([chart.series[0].data[xIndex], chart.series[1].data[xIndex]]);
            chart.tooltip.hide();
        }
    }

}

function tooltipFormat() {
    var s = [];

    var totalSeconds = this.x;
    var minutes = Math.floor(totalSeconds / 60);
    var seconds = totalSeconds - minutes * 60;
    minutes = minutes < 10 ? '0' + minutes : minutes;
    seconds = seconds < 10 ? '0' + seconds : seconds;


    str = minutes + ':' + seconds;
    $.each(this.points, function (i, point) {
        var seriesName = this.series.tooltipOptions.valuePrefix;

        /*if (point.series.index == 0) {
        seriesName = 'Kantar (second by second)';
        }
        else {
        seriesName = 'Nielsen (minute by minute)';
        }*/

        str += '<br/><span style="color:' + point.series.color + ';font-weight:bold;">' + seriesName + '</span><span style="color:' + point.series.color + ';"> = ' +
                    numberWithCommas(point.y) + '</span>';
    });
    return str;
}

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function FormatTime() {
    var minutes = Math.floor(this.value / 60);
    var seconds = this.value - minutes * 60;

    minutes = minutes < 10 ? '0' + minutes : minutes;
    seconds = seconds < 10 ? '0' + seconds : seconds;
    return minutes + ':' + seconds;
}

function ChartHoverOutManage() {
    _IsManualHover = false;
    console.log("chart hover out");
}

function ChartHoverManage() {
    _IsManualHover = true;
    console.log("chart hover");
    //$('#divKantorChart').scrollLeft(this.plotX);
}


function LineChartClick() {
    setSeekPoint(this.category);
}



function changeChartTab(tabNumber) {

    // hide all tabs
    $("div[id ^= 'video-chart-']").css({ opacity: 0 })
    $("div[id ^= 'video-chart-']").css({ height: "0" });

    $("div[id ^= 'video-tab-']").removeClass('playerChartTabActive');
    //$("div[id ^= 'video-parent-tab-']").removeClass('playerChartTabActiveParent');


    // show current tab
    $('#video-tab-' + tabNumber).addClass('playerChartTabActive');
    //$('#video-parent-tab-' + tabNumber).addClass('playerChartTabActiveParent');
    $('#video-chart-' + tabNumber).css({ height: "auto", opacity: 1 })
}