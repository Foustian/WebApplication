var _RadioFromDate = null;
var _RadioToDate = null;
var _MarketName = "";
var _RadioStation = "";
var _RadioIsAsc = false;
var _SearchTermRadio = "";

$(function () {

    $("#divRadioCalender").datepicker({
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            $("#dpRadioFrom").val(dateText);
            $("#dpRadioTo").val(dateText);
            SetRadioDateVariable();
        }
    });

    $("#dpRadioFrom").datepicker({
        showOn: "button",
        buttonImage: "../images/calender.png",
        buttonImageOnly: true,
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            $("#dpRadioFrom").val(dateText);
            SetRadioDateVariable();
        },
        onClose: function (dateText) {
        }
    });

    $("#dpRadioTo").datepicker({
        showOn: "button",
        buttonImage: "../images/calender.png",
        buttonImageOnly: true,
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            $("#dpRadioTo").val(dateText);
            SetRadioDateVariable();
        },
        onClose: function (dateText) {
        }
    });

    $("#txtRadioKeyword").keypress(function (e) {
        if (e.keyCode == 13) {
            SetRadioKeyword();

        }
    });
    $("#txtRadioKeyword").blur(function () {
        SetRadioKeyword();
    });
    $("#imgRadioKeyword").click(function (e) {
        SetRadioKeyword();
    });

    BindMarket();

});

function BindMarket() {
    $.ajax({
        url: _urlTimeshiftSelectAllRadioStations,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: 'json',
        data: {},
        success: function (result) {

            CheckForSessionExpired(result);

            if (result != null && result.isSuccess) {
                if (result.market != null) {

                    var marketHTML = "";
                    var stationHTML = "";

                    $.each(result.market, function (eventID, eventData) {
                        marketHTML = marketHTML + '<li onclick="SetMarket(\'' + eventData.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + '\');" role=\"presentation\"><a href=\"#\" tabindex=\"-1\" role=\"menuitem\">';
                        marketHTML += eventData + '</a></li>';
                    });

                    $.each(result.station, function (eventID, eventData) {
                        stationHTML = stationHTML + '<li onclick="SetRadioStation(\'' + eventData.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + '\');" role=\"presentation\"><a href=\"#\" tabindex=\"-1\" role=\"menuitem\">';
                        stationHTML += eventData + '</a></li>';
                    });

                    if (marketHTML == "") {
                        $('#ulRadioMarket').html('<li role="presentation"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
                    }
                    else {
                        $('#ulRadioMarket').html(marketHTML);
                    }

                    if (stationHTML == "") {
                        $('#ulRadioStation').html('<li role="presentation"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
                    }
                    else {
                        $('#ulRadioStation').html(stationHTML);
                    }
                }
            }
            else {
                BindMarket();
            }
        },
        error: function (a, b, c) {
            BindMarket();
        }
    });
}

function SetRadioDateVariable() {

    if ($("#dpRadioFrom").val() && $("#dpRadioTo").val()) {
        if (_RadioFromDate != $("#dpRadioFrom").val() || _RadioToDate != $("#dpRadioTo").val()) {
            _RadioFromDate = $("#dpRadioFrom").val();
            _RadioToDate = $("#dpRadioTo").val();
            RefreshRadioResults(false, false);
            $("#ulRadio").parent().removeClass('open');
        }
    }
    else {
        if ($("#dpRadioFrom").val() != "" && $("#dpRadioTo").val() == "") {
            $("#dpRadioTo").addClass("warningInput");
        }
        else if ($("#dpRadioTo").val() != "" && $("#dpRadioFrom").val() == "") {
            $("#dpRadioFrom").addClass("warningInput");
        }
    }
}
function SetMarket(p_Market) {
    _MarketName = p_Market;
    RefreshRadioResults(false, false);
}

function SetRadioStation(p_Station) {
    _RadioStation = p_Station;
    RefreshRadioResults(false, false);
}

function RefreshRadioResults(isNext, isNextPrev) {

    var jsonPostData = {
        p_FromDate: _RadioFromDate,
        p_ToDate: _RadioToDate,
        p_Market: _MarketName,
        p_Station: _RadioStation,
        p_IsAsc: _RadioIsAsc,
        p_IsNext: isNext,
        p_IsPrevNext: isNextPrev,
        p_SearchTerm: _SearchTermRadio
    }

    if (RadioDateValidation()) {
        SetRadioActiveFilter();

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlTimeshiftSelectRadioStationResults,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            success: function (result) {

                if (result != null && result.isSuccess) {
                    $("#divRadioResults").html(result.HTML);
                    $("#divRadioPreviousNext").show();
                    if (result.isPrevEnable) {
                        $("#btnRadioPreviousPage").show();
                    }
                    else {
                        $("#btnRadioPreviousPage").hide();
                    }

                    if (result.isNextEnable) {
                        $("#btnRadioNextPage").show();
                    }
                    else {
                        $("#btnRadioNextPage").hide();
                    }
                    if (result.hasResults) {
                        $("#lblRadioRecords").html(result.startRecord + " - " + result.endRecord + " of " + result.totalRecords);
                    }
                    else {
                        $("#lblRadioRecords").html("");
                    }
                }
                else {
                    if (!isNext && !isNextPrev) {
                        ClearResultsOnError('divRadioResults', 'divRadioPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "RefreshRadioResults(" + isNext + "," + isNextPrev + ")"));
                    }
                    ShowNotification(_msgErrorOccured);
                }
            },
            error: function (a, b, c) {
                ShowNotification(_msgErrorOccured, a);
                if (!isNext && !isNextPrev) {
                    ClearResultsOnError('divRadioResults', 'divRadioPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "RefreshRadioResults(" + isNext + "," + isNextPrev + ")"));
                }
            }
        });
    }
}

function SearchRadioResultPaging(isNext) {
    RefreshRadioResults(isNext, true);
}

function RadioDateValidation() {
    $('#dpRadioFrom').removeClass('warningInput');
    $('#dpRadioTo').removeClass('warningInput');


    // if both empty
    if (($('#dpRadioTo').val() == '') && ($('#dpRadioFrom').val() == '')) {
        return true;

    }
    //if one empty not other
    else if (($('#dpRadioFrom').val() != '') && ($('#dpRadioTo').val() == '')
                    ||
                    ($('#dpRadioFrom').val() == '') && ($('#dpRadioTo').val() != '')
                    ) {
        if ($('#dpRadioFrom').val() == '') {

            ShowNotification(_msgFromDateNotSelected);
            $('#dpRadioFrom').addClass('warningInput');
        }

        if ($('#dpRadioTo').val() == '') {

            ShowNotification(_msgToDateNotSelected);
            $('#dpRadioTo').addClass('warningInput');

        }
        return false;
    }
    //if both not empty
    else {
        var isFromDateValid = isValidDate($('#dpRadioFrom').val().toString());
        var isToDateValid = isValidDate($('#dpRadioTo').val().toString());
        if (isFromDateValid && isToDateValid) {
            var fromDate = new Date($('#dpRadioFrom').val());
            var toDate = new Date($('#dpRadioTo').val());
            if (fromDate > toDate) {
                ShowNotification(_msgFromDateLessThanToDate);
                $('#dpRadioFrom').addClass('warningInput');
                $('#dpRadioTo').addClass('warningInput');
                return false;
            }
            else {
                return true;

            }
        }
        else {
            if (!isFromDateValid) {
                $('#dpRadioFrom').addClass('warningInput');
            }
            if (!isToDateValid) {
                $('#dpRadioTo').addClass('warningInput');
            }
            ShowNotification(_msgInvalidDate);
            return false;
        }
    }
}

function SetRadioActiveFilter() {

    var isFilterEnable = false;
    $("#divRadioActiveFilter").html("");

    if (_SearchTermRadio != null && _SearchTermRadio != "") {
        $('#divRadioActiveFilter').append('<div id="divRadioKeywordActiveFilter" class="filter-in">' + EscapeHTML(_SearchTermRadio) + '<span class="cancel" onclick="RemoveRadioFilter(4);"></span></div>');
        isFilterEnable = true;
    }

    if (_MarketName != "") {
        $('#divRadioActiveFilter').append('<div id="divRadioMarketdActiveFilter" class="filter-in">' + _MarketName + '<span class="cancel" onclick="RemoveRadioFilter(1);"></span></div>');
        isFilterEnable = true;
    }
    if ((_RadioFromDate != null && _RadioFromDate != "") && (_RadioToDate != null && _RadioToDate != "")) {
        $('#divRadioActiveFilter').append('<div id="divRadioDateActiveFilter" class="filter-in">' + _RadioFromDate + ' To ' + _RadioToDate + '<span class="cancel" onclick="RemoveRadioFilter(2);"></span></div>');
        isFilterEnable = true;
    }
    if (_RadioStation != "") {
        $('#divRadioActiveFilter').append('<div id="divRadioStationActiveFilter" class="filter-in">' + _RadioStation + '<span class="cancel" onclick="RemoveRadioFilter(3);"></span></div>');
        isFilterEnable = true;
    }
    if (isFilterEnable) {
        $("#divRadioActiveFilter").css({ 'border-bottom': '1px solid rgb(236, 236, 236)', 'margin-bottom': '5px' });
    }
    else {
        $("#divRadioActiveFilter").css({ 'border-bottom': '' });
    }
}

function RemoveRadioFilter(filterType) {

    // Represent Market
    if (filterType == 1) {
        _MarketName = "";
        RefreshRadioResults(false, false);
    }

    // Represent Date filter(From Date,To Date)
    if (filterType == 2) {

        $("#dpRadioFrom").datepicker("setDate", null);
        $("#dpRadioTo").datepicker("setDate", null);
        $("#divRadioCalender").datepicker("setDate", null);

        $('#aRadioDuration').html(_msgAll + '&nbsp;&nbsp;<span class="caret"></span>');

        _RadioFromDate = null;
        _RadioToDate = null;
        RefreshRadioResults(false, false);
    }

    if (filterType == 3) {
        _RadioStation = "";
        RefreshRadioResults(false, false);
    }

    if (filterType == 4) {
        $("#txtRadioKeyword").val("");
        _SearchTermRadio = "";
        RefreshRadioResults(false, false);
    }
}

function RadioSortDirection(p_IsAsc) {
    if (p_IsAsc != _RadioIsAsc) {
        _RadioIsAsc = p_IsAsc;

        if (_RadioIsAsc) {
            $('#aRadioSortDirection').html(_msgOldestFirst + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (!_RadioIsAsc) {
            $('#aRadioSortDirection').html(_msgMostRecent + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        RefreshRadioResults(false, false);
    }
}

function GetRadioResultOnDuration(duration) {

    $("#dpRadioFrom").removeClass("warningInput");
    $("#dpRadioTo").removeClass("warningInput");
    var dtcurrent = new Date();
    var fDate;

    // All
    if (duration == 0) {
        $("#dpRadioFrom").val("");
        $("#dpRadioTo").val("");
        dtcurrent = "";
        RemoveFilter(2);
        $('#aRadioDuration').html(_msgAll + '&nbsp;&nbsp;<span class="caret"></span>');
    }

    // 24 hours
    else if (duration == 1) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth(), dtcurrent.getDate() - 1);
        $('#aRadioDuration').html(_msgLast24Hours + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    // Last week
    else if (duration == 2) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth(), dtcurrent.getDate() - 7);
        $('#aRadioDuration').html(_msgLastWeek + '&nbsp;&nbsp;<span class="caret"></span>');
    }

    // Last Month
    else if (duration == 3) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth() - 1, dtcurrent.getDate());
        $('#aRadioDuration').html(_msgLastMonth + '&nbsp;&nbsp;<span class="caret"></span>');
    }

    // Last 3 Months
    else if (duration == 4) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth() - 3, dtcurrent.getDate());
        $('#aRadioDuration').html(_msgLast3Month + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    // Custom
    else if (duration == 5) {

        dtcurrent = null;
        if ($("#dpRadioFrom").val() != "" && $("#dpRadioTo").val() != "") {
            $('#aDuration').html(_msgCustom + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        else {
            if ($("#dpRadioFrom").val() == "") {
                $("#dpRadioFrom").addClass("warningInput");
            }
            if ($("#dpRadioTo").val() == "") {
                $("#dpRadioTo").addClass("warningInput");
            }
            ShowNotification(_msgSelectDate);
        }
    }

    $("#dpRadioFrom").datepicker("setDate", fDate);
    $("#dpRadioTo").datepicker("setDate", dtcurrent);

    if ($("#dpRadioFrom").val() != "" && $("#dpRadioTo").val() != "") {

        if (_RadioFromDate != $("#dpRadioFrom").val() || _RadioToDate != $("#dpRadioTo").val()) {
            _RadioFromDate = $("#dpRadioFrom").val();
            _RadioToDate = $("#dpRadioTo").val();
            RefreshRadioResults(false, false);
        }
    }
}

function SetRadioKeyword() {
    if ($("#txtRadioKeyword").val() != "" && _SearchTermRadio != $("#txtRadioKeyword").val()) {
        _SearchTermRadio = $("#txtRadioKeyword").val();

        RefreshRadioResults(false, false);
    }
}

var LoadRadioPlayer = function (p_GUID,p_Market,p_DateTime,p_Timezone) {

    LoadPlayerbyGuidTSRadio(p_GUID, _SearchTermRadio, p_Market, p_DateTime, p_Timezone);

}
