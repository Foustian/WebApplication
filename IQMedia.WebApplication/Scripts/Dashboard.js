var _FromDate = null;
var _ToDate = null;
var _Medium = '';
var _MediumDesc = '';
var _SearchType = 1;
var _IsDefaultLoad = true;
var CONST_ZERO = "0";
var _SearchRequests = [];
var _QueryNameList = [];
var _QueryNames = "";
var _OldSearchRequests = [];
var _SelectedDmas = new Array();
var _PieChartTotalHits = 0;
var _SelectedProvinces = new Array();
var _SelectedThirdPartySeries = [];
var _dmaIds = [];

var _isReports = false;

var _DmaChartColors = ["#BDD94E","#E61061"]

var _Months = ["Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec"];

var _cohortChoice = null;
var _hiddenIds = [];
var _reportTab = "";

$(document).ready(function () {
    // Set thousands separator for highcharts
    Highcharts.setOptions({
        lang: {
            thousandsSep: ","
        }
    });

    $('#ulMainMenu li').removeAttr("class");
    $('#liMenuDashboard').attr("class", "active");
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

    $("body").click(function (e) {
        if (e.target.id == "liSearchRequestFilter" || $(e.target).parents("#liSearchRequestFilter").size() > 0)
        {
            if ($('#ulSearchRequest').is(':visible'))
            {
                $('#ulSearchRequest').hide();
            }
            else
            {
                $('#ulSearchRequest').show();
            }
        }
        else if ((e.target.id !== "liSearchRequestFilter" && e.target.id !== "ulSearchRequest" && $(e.target).parents("#ulSearchRequest").size() <= 0) || e.target.id == "btnSearchRequest")
        {
            $('#ulSearchRequest').hide();
        }
    });

    // Initialize selected third-party series, and select the appropriate group checkboxes
    _SelectedThirdPartySeries = $.map($('#ulThirdPartySeries input:checked'), function (obj, index) {
        return $(obj).val();
    });

    $("#ulThirdPartySeries input[name='chkThirdPartyGroup']").each(function (index, obj) {
        var groupID = $(obj).val();
        var numTotal = $("#ulThirdPartySeries input[name='chkThirdPartySeries_" + groupID + "']");
        var numChecked = $("#ulThirdPartySeries input[name='chkThirdPartySeries_" + groupID + "']:checked");

        $(obj).prop("checked", numTotal.length == numChecked.length);
    });

    if (getParameterByName("source") == "")
    {
        // Main Dashboard Page
        GetDataMediumWise('Overview', 'Overview');
        AddHighlightToMenu('Overview');
    }
    else
    {
        // Dashboard IFrame
        GetAdhocSummaryData();

        $("#dpFrom").css({ "margin-top": "5px" });
        $("#dpTo").css({ "margin-top": "5px" });
        $(".ui-datepicker-trigger").hide();
    }

    $('#divPrintableArea').css({ 'min-height': documentHeight - 100 });

    // Cohorts - set up default selected cohort
    _cohortChoice = $("#cohortList").find(".highlightedli").prop("id").split('_')[1];
});

$(window).resize(function () {
    if (screen.height >= 768) {
        $('#divPrintableArea').css({ 'min-height': documentHeight - 100 });
    }
});

function AddHighlightToMenu(mediaType) {

    RemoveHighlightFromMenu();
    if (mediaType == 'Overview') {
        $('#liOverview').addClass('highlightedli');
    }
    else if (mediaType == 'ClientSpecific') {
        $('#liClientSpecific').addClass('highlightedli');
    }
    else if ($("li[data-c-mt='"+mediaType+"']").length > 0)
    {
        $("li[data-c-mt='" + mediaType + "']").addClass('highlightedli');
    }
}
function RemoveHighlightFromMenu() {
    $('#ulMenu li').each(function () {
        $(this).removeClass('highlightedli');
    });

    $("#ulReports li").each(function () {
        $(this).removeClass("highlightedli");
    });
}
function SetDateVariable() {

    if ($("#dpFrom").val() && $("#dpTo").val()) {
        _IsDefaultLoad = false;
        $('#dpFrom').removeClass('warningInput');
        $('#dpTo').removeClass('warningInput');
        if (_FromDate != $("#dpFrom").val() || _ToDate != $("#dpTo").val()) {
            _FromDate = $("#dpFrom").val();
            _ToDate = $("#dpTo").val();

            // change the _SearchType
            // 0-hour 1-day 3-month
            if (DateValidation()) {
                //From Date
                var mdy = _FromDate.split('/');
                var dateFrom = new Date(mdy[2], mdy[0] - 1, mdy[1]);
                dateFrom.setMinutes(dateFrom.getMinutes() - dateFrom.getTimezoneOffset());

                //To Date
                mdy = _ToDate.split('/');
                var dateTo = new Date(mdy[2], mdy[0] - 1, mdy[1]);
                dateTo.setMinutes(dateTo.getMinutes() - dateTo.getTimezoneOffset());

                //Day diff
                var millisecondsPerDay = 24 * 60 * 60 * 1000;
                var dayDiff = Math.round((dateTo - dateFrom) / millisecondsPerDay);

                //Month diff
                var monthDiff = dateTo.getMonth() - dateFrom.getMonth() + (12 * (dateTo.getFullYear() - dateFrom.getFullYear()));
                if (dateTo.getDate() < dateFrom.getDate()) monthDiff--;

                if (_SearchType == 0)//hour
                {
                    if (dayDiff >= 8 && monthDiff < 6) ChangeSearchType(1);
                    else if (monthDiff >= 6) ChangeSearchType(3);
                }
                else if(_SearchType == 1)//day
                {
                    if (dayDiff < 1) ChangeSearchType(0);
                    else if (monthDiff >= 6) ChangeSearchType(3);
                }
                else if (_SearchType == 3)//month
                {
                    if (dayDiff < 1) ChangeSearchType(0);
                    else if (dayDiff >= 1 && monthDiff < 3) ChangeSearchType(1);

                }
            }
            if (_isReports)
            {
                GetOverviewReport($("#Cohorts_" + _reportTab)[0]);
            }
            else
            {
                GetDataMediumWise(_Medium, _MediumDesc);
            }

            $('#ulCalender').parent().removeClass('open');
            $('#aDuration').html(_msgCustom + '&nbsp;&nbsp;<span class="caret"></span>');
        }
    }
    else if ($("#dpFrom").val() != "" && $("#dpTo").val() == "") {
        $("#dpTo").addClass("warningInput");
    }
    else if ($("#dpTo").val() != "" && $("#dpFrom").val() == "") {
        $("#dpFrom").addClass("warningInput");
    }
}

function SetSearchRequest(eleRequest,p_SearchRequestID,p_QueryName)
{
    if ($.inArray(p_SearchRequestID, _SearchRequests) == -1) 
    {
        _SearchRequests.push(p_SearchRequestID);
        _QueryNameList.push(p_QueryName);
        $(eleRequest).css("background-color","#E9E9E9");
    }
    else
    {
        var catIndex = _SearchRequests.indexOf(p_SearchRequestID);
        if (catIndex > -1) {
            _SearchRequests.splice(catIndex, 1);
            _QueryNameList.splice(catIndex, 1);
            $(eleRequest).css("background-color","#ffffff");
        }
    }
}

function SearchRequest()
{
    if($(_SearchRequests).not(_OldSearchRequests).length != 0 || $(_OldSearchRequests).not(_SearchRequests).length != 0)
    {
        _OldSearchRequests = _SearchRequests.slice(0);
        $.each(_QueryNameList, function(index, val){
            if(_QueryNames == "")
            {
                _QueryNames = val;
            }
            else
            {   
            _QueryNames = _QueryNames + ', ' + val;
            }
        });
        GetDataMediumWise(_Medium,_MediumDesc);
    }
}

function ChangeSearchType(sType) {
    _SearchType = sType;
    if (!_isReports)
    {
        GetDataMediumWise(_Medium, _MediumDesc);
    }
}

function GetDataMediumWise(mediumType, mediumDesc) {
    AddHighlightToMenu(mediumType);

    // Make sure to mark _isReports flag as false - method takes user away from this "tab"
    _isReports = false;

    $('#divDateSelector').show();
    _Medium = mediumType;
    _MediumDesc = mediumDesc;

    /*if (_Medium != 'Overview' && _SearchType == 0) {
    _SearchType = 1;
    }*/

    SetActiveDuration();

    if (DateValidation()) {        
        var jsonPostData = {
            p_FromDate: _FromDate,
            p_ToDate: _ToDate,
            p_SearchRequestIDs : _SearchRequests,
            p_Medium: mediumType,
            p_SearchType: _SearchType,
            p_IsDefaultLoad: _IsDefaultLoad,
            p_ThirdPartyDataTypeIDs: _SelectedThirdPartySeries
        }
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlDashboardGetMediumData,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            success: OnDataMediumWiseComplete,
            error: OnDataMediumWiseFail
        });
    }
}

function OnDataMediumWiseComplete(result) {
    CheckForSessionExpired(result);
    $('#imgThirdParty').show();
    $('#imgExcel').hide();
    $('#divHeader').html('<b>' + result.CategoryDescription + '</b>');
    SetActiveFilter();
    /*if (_FromDate == null || _ToDate == null) {
    var currDate = new Date();
    var previousDate = new Date(currDate.getFullYear(), currDate.getMonth(), currDate.getDate() - 29);
    _FromDate = previousDate;
    _ToDate = currDate;
    }*/

    if (_FromDate == null || _FromDate != result.fromDate) {
        _FromDate = result.fromDate;
        $("#dpFrom").datepicker("setDate", _FromDate);
    }

    if (_ToDate == null || _ToDate != result.toDate) {
        _ToDate = result.toDate;
        $("#dpTo").datepicker("setDate", _ToDate);
    }

    if (_Medium == 'Overview') {
        $('#divHeader').html('<b>Source Overview</b>');
        OnResultSearchComplete(result);
    }
    else if (_Medium == 'ClientSpecific') {
        $('#imgThirdParty').hide();
        $('#imgExcel').show();

        if (result.isSuccess) {
            $('#divMediumData').html('');
            $('#divMediumData').html(result.HTML);
            SetActiveDuration();

            // This function must exist in each client-specific view
            LoadClientSpecificView(result);
        }
        else {
            ShowNotification(_msgErrorOccured);
        }
    }
    else {
        if (result.isSuccess) {
            $('#divMediumData').html('');
            $('#divMediumData').html(result.HTML);
            SetActiveDuration();

                _SelectedDmas = new Array();
                _SelectedProvinces = new Array();

            SetDashboardMediumHTML(result, _Medium);
        }
        else {
            ShowNotification(_msgErrorOccured); //'Some error occured, try again later');
        }
    }
}

function OnDataMediumWiseFail(a, b, c) {
    ShowNotification(_msgErrorOccured,a); //'Some error occured, try again later');
}

function SetSentimentValue(negValue, posValue,Position) {
    $('#divNegativeSentimentValue'+Position).html(negValue);
    if (negValue.replace(/,/g, "") > 0) {
        $('#divNegativeSentimentValue'+Position).css({ 'background-color': 'red' });
    }
    else {
        $('#divNegativeSentimentValue'+Position).css({ 'background-color': '#DEDEDE' });
    }

    $('#divPositiveSentimentValue'+Position).html(posValue);
    if (posValue.replace(/,/g, "") > 0) {
        $('#divPositiveSentimentValue'+Position).css({ 'background-color': 'green' });
    }
    else {
        $('#divPositiveSentimentValue'+Position).css({ 'background-color': '#DEDEDE' });
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

            ShowNotification(_msgFromDateNotSelected); //'From Date not selected');
            $('#dpFrom').addClass('warningInput');
        }

        if ($('#dpTo').val() == '') {

            ShowNotification(_msgToDateNotSelected); //'To Date not selected');
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
                ShowNotification(_msgFromDateLessThanToDate); //'From Date should be less than To Date');
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


function OnResultSearchComplete(result, onClickFunction) {
    SetDashboardOverviewHTML(result, "divMediumData", onClickFunction);
    SetActiveDuration();
}

function RenderPieChart(jsonPieChartData) {
    $('#divPieChartData').html('');
    var myChart = new FusionCharts("Pie2D", "SubMediaPieChart", "300", "380");
    myChart.render("divPieChartData");
    myChart.setJSONData(jsonPieChartData);
}

function ShowTooltip()
{
    return '<div class="tooltip">'+this.x+'<br/><b>'+this.series.name+': </b>'+this.y+'</div>';
}

function HandleChartMouseHover()
{
    //if($(this.chart.renderTo).children('.highcharts-container') > 0)
    //{
        $('.highcharts-container').css("z-index","0");
        $(this.chart.renderTo).find('.highcharts-container').css("z-index","1");
    //}
}

function HandleChartMouseOut()
{
     //$(this.chart.renderTo).find('.highcharts-container').css("z-index","0");
}

function RenderSparkChart(jsonIQMediaValueRecords, divSparkChartID, ChartID) {

    var myChart = new FusionCharts("../../Content/Fusioncharts/SparkLine.swf", ChartID, "120", "100", "0", "1");
    // var ss = '{  "chart": {    "palette": "2","caption": "Cisco",    "setadaptiveymin": "1"  },  "dataset": [    {      "data": [         {          "value": "27.26"        },        {          "value": "37.88"        },        {          "value": "38.88"        },        {          "value": "22.9"        },        {          "value": "39.02"        },        {          "value": "23.31"        },        {          "value": "30.85"        },        {          "value": "27.01"        },        {          "value": "33.2"        },        {          "value": "21.93"        },        {          "value": "34.51"        },        {          "value": "24.84"        },        {          "value": "39.32"        },        {          "value": "37.04"        },        {          "value": "27.81"        },        {          "value": "22.95"        },        {          "value": "24.73"        },        {          "value": "37.63"        },        {          "value": "29.75"        },        {          "value": "22.35"        },        {          "value": "34.35"        },        {          "value": "27.6"        },        {          "value": "27.97"        },        {          "value": "32.36"        },        {          "value": "22.56"        },        {          "value": "24.15"        },        {          "value": "24.93"        },        {          "value": "35.82"        },        {          "value": "23.45"        },        {          "value": "37.64"        },        {          "value": "26.99"        },       {          "value": "29.48"        },        {          "value": "36.63"        },        {          "value": "35.58"        },        {         "value": "32.19"        },        {         "value": "27.59"        },        {          "value": "26.94"       },        {          "value": "32.35"        },      {          "value": "22.63"        },        {          "value": "25.97"        },        {          "value": "25.28"        },        {          "value": "26.73"        },        {          "value": "23.47"        },        {          "value": "20.55"        },        {          "value": "34.58"        },        {          "value": "29.16"        },        {          "value": "34.97"        },        {          "value": "24.57"        },        {          "value": "20.7"        },        {          "value": "32.61"        }      ]    }  ]}';
    myChart.setJSONData(jsonIQMediaValueRecords);
    myChart.render(divSparkChartID);

}

function changeTab(tabNumber) {
    if (tabNumber == '1') {
        $('#divLineChartSubMedia').hide();
        $('#divLineChartSubMedia').css("height", "0");
        $('#divLineChartSubMedia').css("width", "0");
        $('#divLineChartSubMedia').css("overflow", "hidden");
        $('#divLineChartMedia').css("height", "300px");
        $('#divLineChartMedia').css("width", "100%");
        $('#divLineChartMedia').css("overflow", "visible");
        $('#imgSingleLine').attr("src", "../../Images/Dashboard/single-line-normal.png");
        $('#imgMultipleLine').attr("src", "../../Images/Dashboard/multiple-line-active.png");

        $('#divLineChartMedia').show();


    }
    else {
        $('#divLineChartMedia').hide();
        $('#divLineChartMedia').css("height", "0");
        $('#divLineChartMedia').css("width", "0");
        $('#divLineChartMedia').css("overflow", "hidden");
        $('#divLineChartSubMedia').css("height", "300px");
        $('#divLineChartSubMedia').css("width", "100%");
        $('#divLineChartSubMedia').css("overflow", "visible");
        $('#imgSingleLine').attr("src", "../../Images/Dashboard/single-line-active.png");
        $('#imgMultipleLine').attr("src", "../../Images/Dashboard/multiple-line-normal.png");

        $('#divLineChartSubMedia').show();

    }

}

function SetActiveDuration() {
    $('#divDuration div').each(function () {
        $(this).removeClass('activeDuration');
    });
    if (_SearchType == 0) {
        $('#divHourlyDuration').addClass('activeDuration');
    }
    else if (_SearchType == 1) {
        $('#divDayDuration').addClass('activeDuration');
    }
    if (_SearchType == 2) {
        $('#divWeekDuration').addClass('activeDuration');
    }
    if (_SearchType == 3) {
        $('#divMonthDuration').addClass('activeDuration');
    }

}


function GenerateDashboardPDF() {
    //FusionCharts("SubMediaLineChart").ref.getSVGString();

    if ($("li.highlightedli").prop("id") !== "ReportOverview")
    {
        $("#imgThirdParty").hide();

        var jsonPostData = {
            p_HTML: $("#divPrintableArea").html(),
            p_FromDate: $("#dpFrom").val(),
            p_ToDate: $("#dpTo").val(),
            p_SearchRequests: _QueryNames
        }

        $.ajax({
            url: _urlDashboardGenerateDashboardPDF,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                if (result.isSuccess)
                {
                    window.location = _urlDashboardDownloadPDFFile;
                }
                else
                {
                    ShowNotification(_msgErroWhileDownloadingFile);
                }
            },
            error: function (a, b, c) {
                ShowNotification(_msgSomeErrorProcessing, a);
            }
        });

        $("#imgThirdParty").show();
    }
    else
    {
        alert("Not yet implemented");
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
    //var emailPattern = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
    var emailPattern = /^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
    return emailPattern.test(email);
}

function SendEmail() {

    if (ValidateSendEmail()) {
        $("#imgThirdParty").hide();

        var jsonPostData = {
            p_HTML: $("#divPrintableArea").html(),
            p_FromDate: $("#dpFrom").val(),
            p_ToDate: $("#dpTo").val(),
            p_FromEmail: $("#txtFromEmail").val(),
            p_ToEmail: $("#txtToEmail").val(),
            p_BCCEmail: $("#txtBCCEmail").val(),
            p_Subject: $("#txtSubject").val(),
            p_UserBody: $("#txtMessage").val(),
            p_SearchRequests: _QueryNames
        }

        $.ajax({
            url: _urlDashboardSendDashBoardEmail,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: OnEmailSendComplete,
            error: OnEmailSendFail
        });

        $("#imgThirdParty").show();
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

function OpenFeed(date, medium, searchRequest, searchRequestDesc) {
    
    var selectedDate = new Date(date); 

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlDashboardOpenIFrame,
        contentType: 'application/json; charset=utf-8',

        success: function (result) {
            if (result.isSuccess) {
                $('#divFeedsPage').modal({
                    backdrop: 'static',
                    keyboard: true,
                    dynamic: true
                });

                $("#divFeedsPage").resizable({
                    handles : 'e,se,s,w',
                    iframeFix: true,
                    start: function(){
                        ifr = $('#iFrameFeeds');
                        var d = $('<div></div>');

                        $('#divFeedsPage').append(d[0]);
                        d[0].id = 'temp_div';
                        d.css({position:'absolute'});
                        d.css({top: ifr.position().top, left:0});
                        d.height(ifr.height());
                        d.width('100%');
                    },
                    stop: function(){
                        $('#temp_div').remove();
                    },
                    resize: function (event, ui) {
                        var newWd = ui.size.width - 10;
                        var newHt = ui.size.height - 20;
                        $("#iFrameFeeds").width(newWd).height(newHt);
                    }
                }).draggable({
                    iframeFix: true,
                    start: function(){
                        ifr = $('#iFrameFeeds');
                        var d = $('<div></div>');

                        $('#divFeedsPage').append(d[0]);
                        d[0].id = 'temp_div';
                        d.css({position:'absolute'});
                        d.css({top: ifr.position().top, left:0});
                        d.height(ifr.height());
                        d.width('100%');
                    },
                    stop: function(){
                        $('#temp_div').remove();
                    }
                });

                $('#iFrameFeeds').attr("src", "//" + window.location.hostname + "/Feeds?date=" + (selectedDate.getMonth() + 1) + "/" + selectedDate.getDate() + "/" + selectedDate.getFullYear() + "&mediatype=" + medium + "&searchrequest=" + searchRequest + "&searchrequestDesc=" + encodeURIComponent(searchRequestDesc.split('+').join(' ')));
                
                $('#divFeedsPage').css("position", "");
                $('#divFeedsPage').css("height", documentHeight - 200);
                $('#iFrameFeeds').css("height", documentHeight - 200);

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

function OpenFeedOutletDma(type,value,submediatype,iqdmaname) {

    
    var station = '';
    var dma = '';
    var competeurl = '';
    var dmaID = '';    
    var handle = '';
    var publication = '';
    var author = '';
    var searchRequestIDs = '[]';
    var searchRequestNames = '[]';

    if (typeof (iqdmaname) === 'undefined') {
        iqdmaname =''    
    }
    
    switch(type)
    {
        case "st" : 
            station = value;
            break;
        case "dma" : 
            dma = value;
            break;
        case "comp" : 
            competeurl = value;
            break;
        case "dmaid" :
            dmaID = value;
            break;
        case "handle" : 
            handle = value;
            break;
        case "pub" :
            publication = value;
            break;
        case "author" :
            author = value;
            break;            
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlDashboardOpenIFrame,
        contentType: 'application/json; charset=utf-8',

        success: function (result) {
            if (result.isSuccess) {
                $('#divFeedsPage').modal({
                    backdrop: 'static',
                    keyboard: true,
                    dynamic: true
                });

                var fromDate = new Date(_FromDate);
                var toDate = new Date(_ToDate);
                $('#iFrameFeeds').attr("src", "//" + window.location.hostname + "/Feeds?station=" + station + "&dma=" + dma + "&competeurl=" + competeurl + "&iqdmaname="+ iqdmaname  +"&iqdmaid=" + dmaID + "&handle=" + handle + "&publication=" + publication + "&author=" + author + "&fromDate=" + (fromDate.getMonth() + 1) + "/" + fromDate.getDate() + "/" + fromDate.getFullYear()+ "&toDate=" + (toDate.getMonth() + 1) + "/" + toDate.getDate() + "/" + toDate.getFullYear() + "&submediatype=[\"" + submediatype + "\"]&searchrequest=" + JSON.stringify(_SearchRequests) + "&searchrequestDesc=" + encodeURIComponent(JSON.stringify(_QueryNameList).split('+').join(' ')));

                $('#divFeedsPage').css("height", documentHeight - 200);
                $('#iFrameFeeds').css("height", documentHeight - 200);

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

function CancelIFramePopup() {
    $("#divFeedsPage").css({ "display": "none" });
    $("#divFeedsPage").modal("hide");
    $('#iFrameFeeds').attr("src", "");
}

function SetActiveFilter() {
    var isFilterEnable = false;
    $("#divActiveFilter").html("");
    
     if (_SearchRequests.length > 0) {
        $.each(_SearchRequests, function(index, val){
            $('#divActiveFilter').append('<div class="filter-in">' + EscapeHTML(_QueryNameList[index]) + '<span class="cancel" onclick="RemoveFilter(\''+ val +'\');"></span></div>');
        });
        
        isFilterEnable = true;
    }


    if (isFilterEnable) {
        $("#divActiveFilter").css({ 'border-bottom': '1px solid rgb(236, 236, 236)' });
    }
    else {
        $("#divActiveFilter").css({ 'border-bottom': '' });
    }
}

function RemoveFilter(requestId) {

    // Represent SearchRequest Filter
    var catIndex = _SearchRequests.indexOf(requestId);
    if (catIndex > -1) {
        _SearchRequests.splice(catIndex, 1);
        _QueryNameList.splice(catIndex, 1);
        _OldSearchRequests = _SearchRequests.slice(0);
        $("#areq_" + requestId).css("background-color","#ffffff");
        GetDataMediumWise(_Medium,_MediumDesc);
    }
}

function LineChartClick() {
    // Don't allow drilldown on client-specific series
    if (this.Type == 'Client') {
        return false;
    }

    var searchRequestIDs = '';
    var searchRequestDescs = '';
    var medium = '';

    if (this.Value != null)
    {
        searchRequestIDs = '["' + this.Value + '"]';
        searchRequestDescs = '["' + this.SearchTerm + '"]';
    }

    if (this.Type == 'Media')
    {
        OpenFeed(this.category, '', searchRequestIDs, searchRequestDescs);
    }
    if (this.Type == 'SubMedia') {
        if(_Medium == 'Overview' || _Medium == 'ClientSpecific')
        {
            OpenFeed(this.category, this.Value, JSON.stringify(_SearchRequests), JSON.stringify(_QueryNameList));
        }
        else
        {   
            OpenFeed(this.category, _Medium, searchRequestIDs, searchRequestDescs);
        }
    }
}

function GetMonth() {
    var date = new Date(this.value);
    return  _Months[date.getMonth()] + ' - ' + date.getFullYear().toString() + '';
}


function ExpandCollapseSparkChart(img,divChartEle)
{
    var chartMain = $('#divLineChartMedia').highcharts();
    var seriesName =$(img).closest('td').text().trim();
    if($(img).attr('src').indexOf('ex.gif') > 0)
    {
    
        var seriesLength = chartMain.series.length;
        for(var i = seriesLength - 1; i > 0; i--)
        {
            if(chartMain.series[i].name != _MediumDesc && $.inArray(chartMain.series[i].name, _QueryNameList) == -1)
            {
                chartMain.series[i].remove();
            }
        }

        var YAxesLength = chartMain.yAxis.length;
        for(var i = YAxesLength - 1; i > 0; i--)
        {
            chartMain.yAxis[i].remove();
        }

        $('.ulSubMediaCharts .broadcastSmallChartHeaderMedium img').attr('src','../images/ex.gif');
       
        var newSeries = new Array();
        $.each($('#'+divChartEle).highcharts().series[0].data,function(index,obj){
            newSeries.push({y:obj.y,category:obj.category});
        });

        chartMain.addAxis({ 
            id: 'oppAxis',
            title: {
                text: seriesName
            },
            min: 0,
            minRange: 0.1,
            gridLineWidth: 0,
            opposite: true
        });

        if(seriesName != 'Sentiment')
        {
            chartMain.addSeries({name:seriesName,data :newSeries,yAxis:'oppAxis'});
        }
        else
        {
            
            chartMain.addSeries({name:'Positive ' + seriesName,data :newSeries,yAxis:'oppAxis',color:'green'});

            var newSeries2 = new Array();
            $.each($('#'+divChartEle).highcharts().series[1].data,function(index,obj){
                newSeries2.push({y:obj.y,category:obj.category});
            });
            chartMain.addSeries({name:'Negative ' + seriesName,data :newSeries2,yAxis:'oppAxis',color:'red'});
        }
        
        
        //chartMain.xAxis[0].setExtremes();
        $(img).attr('src','../images/cl.gif');
    }
    else
    {
        var seriesLength = chartMain.series.length;
        for(var i = seriesLength - 1; i > -1; i--)
        {
            if(seriesName != 'Sentiment')
            {
                if(chartMain.series[i].name ==seriesName)
                {
                    chartMain.series[i].remove();
                    $(img).attr('src','../images/ex.gif');
                    break;
                }
            } 
            else
            {
                if(chartMain.series[i].name =='Positive ' + seriesName || chartMain.series[i].name =='Negative ' + seriesName)
                {
                    chartMain.series[i].remove();
                    $(img).attr('src','../images/ex.gif');
                    
                }
            }       
            
        }

        var YAxesLength = chartMain.yAxis.length;
        for(var i = YAxesLength - 1; i > 0; i--)
        {
            chartMain.yAxis[i].remove();
        }
    }

    if(_QueryNameList != null && _QueryNameList.length <= 0)
    {
        if(chartMain.series.length > 1)
        {
            //$(chartMain.legend.allItems[0].legendItem.element.childNodes)[0].textContent ='Number of hits';
            //$(chartMain.legend.allItems[0].legendItem.element.childNodes).text('Number of hits');
            chartMain.series[0].update({name:"Number of hits"}, false);
        }
        else
        {
            //$(chartMain.legend.allItems[0].legendItem.element.childNodes)[0].textContent =_MediumDesc
            //$(chartMain.legend.allItems[0].legendItem.element.childNodes).text(_MediumDesc);
            chartMain.series[0].update({name:_MediumDesc}, false);
        }
    }

    chartMain.redraw();
}



function CompareDma()
{
    if(_SelectedDmas.length > 0){
        
        var jsonPostData = {
            p_Dmas: _SelectedDmas,
            p_FromDate: $("#dpFrom").val(),
            p_ToDate: $("#dpTo").val(),
            p_SearchRequests : _SearchRequests,
            p_SearchType: _SearchType,
            p_Medium : _Medium
        }

        $.ajax({
            url: _urlDashboardCompareDma,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: OnCompareDmaComplete,
            error: OnCompareDmaFail
        });
    }
    else
    {
        $("#divCompareDma").hide();
    }
}


function OnCompareDmaComplete(result)
{
    if(result.isSuccess)
    {
        RenderSparkHighChart(result.noOfDocsJson,'divDmaHitChart','DmaHitChart');
        RenderSparkHighChart(result.noOfHitsJson,'divDmaMentionChart','DmaMentionChart');
        RenderSparkHighChart(result.noOfViewJson,'divDmaAudienceChart','DmaAudienceChart');

        if(result.noOfMinOfAiringJson != "")
        {
            RenderSparkHighChart(result.noOfMinOfAiringJson,'divDmaAirTimeChart','DmaAirTimeChart');
        }


        $("#divSelectedDmas").html('<br/>');

        var chartMain = $('#divDmaHitChart').highcharts();

        $.each(_SelectedDmas,function(index,object){
            var dmaname =$("#divUsaMap")[0].children[0].FusionCharts.annotations._renderer.entities.items[eval(_SelectedDmas[index].id)].eJSON.label;
            $("#divSelectedDmas").append('<div>DMA ' + (index + 1) + ': <span style="color:'+_SelectedDmas[index].clickColor+'">' + dmaname +'</span></div>');
        });


        $("#divCompareDma").show();
    }
    else
    {
        ShowNotification(_msgErrorOccured);
    }
}

function OnCompareDmaFail(a,b,c)
{
    ShowNotification(_msgErrorOccured);
}

function CompareProvince() {
    if (_SelectedProvinces.length > 0) {

        var jsonPostData = {
            p_Provinces: _SelectedProvinces,
            p_FromDate: $("#dpFrom").val(),
            p_ToDate: $("#dpTo").val(),
            p_SearchRequests: _SearchRequests,
            p_SearchType: _SearchType,
            p_Medium: _Medium
        }

        $.ajax({
            url: _urlDashboardCompareProvince,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: OnCompareProvinceComplete,
            error: OnCompareDmaFail
        });
    }
    else {
        $("#divCompareProvince").hide();
    }
}

function OnCompareProvinceComplete(result) {
    if (result.isSuccess) {
        RenderSparkHighChart(result.noOfDocsJson, 'divProvinceHitChart', 'ProvinceHitChart');
        RenderSparkHighChart(result.noOfHitsJson, 'divProvinceMentionChart', 'ProvinceMentionChart');
        RenderSparkHighChart(result.noOfViewJson, 'divProvinceAudienceChart', 'ProvinceAudienceChart');

        if (result.noOfMinOfAiringJson != "") {
            RenderSparkHighChart(result.noOfMinOfAiringJson, 'divProvinceAirTimeChart', 'ProvinceAirTimeChart');
        }

        $("#divSelectedProvinces").html('<br/>');

        var chartMain = $('#divProvinceHitChart').highcharts();

        $.each(_SelectedProvinces, function (index, object) {
            var provinceName = $("#divCanadaMap")[0].children[0].FusionCharts.annotations._renderer.entities.items[_SelectedProvinces[index].id].eJSON.label;
            $("#divSelectedProvinces").append('<div>Province ' + (index + 1) + ': <span style="color:' + _SelectedProvinces[index].clickColor + '">' + provinceName + '</span></div>');
        });

        $("#divCompareProvince").show();
    }
    else {
        ShowNotification(_msgErrorOccured);
    }
}

function ShowDmaListMap(inp)
{
    $("#divTopCountries").hide();
    $("#divCAMap").hide();
    if(inp == 0)
    {
        $("#divTopDmas").show();
        $("#divDmaMap").hide();
    }

    if(inp == 1)
    {
        $("#divTopDmas").hide();
        $("#divDmaMap").show();
    }
}

function ShowTopCountryList() {
    $("#divDmaMap").hide();
    $("#divTopDmas").hide();
    $("#divCAMap").hide();
    $("#divTopCountries").show();
}

function ShowCanadaMap() {
    $("#divDmaMap").hide();
    $("#divTopDmas").hide();
    $("#divTopCountries").hide();
    $("#divCAMap").show();
}


//function RenderDmaMapChart(jsonMapData, divMapChartID, ChartID) {
//    
//    FusionCharts.setCurrentRenderer('javascript');
//    var chartObj=new FusionCharts("maps/usadma",ChartID,"100%","450","0","1");
//    chartObj.render(divMapChartID);
//    chartObj.setJSONData(jsonMapData);
//    chartObj.addEventListener('entityClick',CheckUncheckDma);
//}


function RenderDmaMapChart(jsonMapData, divMapChartID, ChartID) {
    //FusionCharts.ready(function() {
        
        var populationMap = new FusionCharts({
            type: 'maps/usadma',
            renderAt: divMapChartID,
            width: '100%',
            height: '350',
            dataFormat: 'json',
            dataSource:jsonMapData
        }).render();
        populationMap.addEventListener('entityClick',CheckUncheckDma);
        //populationMap.addEventListener('resized',SetLegendPos);
       // populationMap.addEventListener('drawComplete',SetLegendPos);
        populationMap.addEventListener('entityRollOut', function (evt, data) {
            SetCurrentSelectedItemsFillColor(this, _SelectedDmas);
            hideToolTip()
        });
        populationMap.addEventListener('entityRollOver',function(evt,data){
            SetCurrentSelectedItemsFillColor(this, _SelectedDmas);
            showToolTipOnChart("Market Area Name : " + data.label + "<br/>" + "Mention : " + data.value);
        });
        populationMap.addEventListener('chartRollOver',function(evt,data){
            SetCurrentSelectedItemsFillColor(this, _SelectedDmas);
        });
        populationMap.addEventListener('chartRollOut',function(evt,data){
            SetCurrentSelectedItemsFillColor(this, _SelectedDmas);
        });
        
        
    //});

    //$(".raphael-group-24-dataset").attr("transform","matrix(1,0,0,1,"+Ypos+",0)");
}

function RenderCanadaMapChart(jsonMapData, divMapChartID, ChartID) {
    var populationMap = new FusionCharts({
        type: 'maps/canada',
        renderAt: divMapChartID,
        width: '100%',
        height: '350',
        dataFormat: 'json',
        dataSource: jsonMapData
    }).render();
    populationMap.addEventListener('entityClick', CheckUncheckProvince);
    populationMap.addEventListener('entityRollOut', function (evt, data) {
        SetCurrentSelectedItemsFillColor(this, _SelectedProvinces);
        hideToolTip();
    });
    populationMap.addEventListener('entityRollOver', function (evt, data) {
        SetCurrentSelectedItemsFillColor(this, _SelectedProvinces);
        showToolTipOnChart("Province Name : " + data.label + "<br/>" + "Mention : " + data.value);
    });
    populationMap.addEventListener('chartRollOver', function (evt, data) {
        SetCurrentSelectedItemsFillColor(this, _SelectedProvinces);
    });
    populationMap.addEventListener('chartRollOut', function (evt, data) {
        SetCurrentSelectedItemsFillColor(this, _SelectedProvinces);
    });
}

function SetLegendPos(){
        var Ypos =  ($("#divUsaMap").width() / 2) - ($("g.fusioncharts-legend > rect").attr("width") / 2)
        $("g.fusioncharts-legend").attr("transform","matrix(1,0,0,1,"+Ypos+",0)");    
        //alert('set le pos');
    }

function CheckUncheckDma(evt, data)
{
    var _dmaids = [];
    $.each(_SelectedDmas,function(index,object){
        _dmaids.push(object.id);
    });
    
    var graphEntity = this.annotations._renderer.entities;

    if($.inArray(data.id, _dmaids) == -1)
    {
        
        if(_SelectedDmas.length >= 2) {
            $.each(graphEntity.items[eval(_SelectedDmas[0].id)].svgElems, function (index, svgElem) {
                $(svgElem.graphic[0]).css("fill", "");
            });
            _dmaids.splice(0, 1);
            _SelectedDmas.splice(0, 1);
        }

        _dmaids.push(data.id);

        _SelectedDmas = new Array();
        $.each(_dmaids,function(index,object){
            
            _objDma = new Object();
            _objDma.id = object;
            _objDma.clickColor = _DmaChartColors[index];

            $.each(graphEntity.items[eval(object)].svgElems, function (index1, svgElem) {
                $(svgElem.graphic[0]).css("fill", _DmaChartColors[index]);
            });

            _SelectedDmas.push(_objDma);
        });

        
        CompareDma();
    }
    else
    {
        var catIndex = _dmaids.indexOf(data.id);
        if (catIndex > -1) {
            _dmaids.splice(catIndex, 1);

            _SelectedDmas = new Array();
            $.each(_dmaids,function(index,object){
            
                _objDma = new Object();
                _objDma.id = object;
                _objDma.clickColor = _DmaChartColors[index];;

                $.each(graphEntity.items[eval(object)].svgElems, function (index1, svgElem) {
                    $(svgElem.graphic[0]).css("fill", _DmaChartColors[index]);
                });

                _SelectedDmas.push(_objDma);
            });

            CompareDma();
        }
    }

}

function CheckUncheckProvince(evt, data) {
    var _provinceIDs = [];
    $.each(_SelectedProvinces, function (index, object) {
        _provinceIDs.push(object.id);
    });

    var graphEntity = this.annotations._renderer.entities;
    
    if ($.inArray(data.id, _provinceIDs) == -1) {
        if (_SelectedProvinces.length >= 2) {
            $.each(graphEntity.items[_SelectedProvinces[0].id].svgElems, function (index, svgElem) {
                $(svgElem.graphic[0]).css("fill", "");
            });

            _provinceIDs.splice(0, 1);
            _SelectedProvinces.splice(0, 1);
        }

        _provinceIDs.push(data.id);

        _SelectedProvinces = new Array();
        $.each(_provinceIDs, function (index, object) {
            var _objProvince = new Object();
            _objProvince.id = object;
            _objProvince.clickColor = _DmaChartColors[index];

            $.each(graphEntity.items[object].svgElems, function (index1, svgElem) {
                $(svgElem.graphic[0]).css("fill", _DmaChartColors[index]);
            });

            _SelectedProvinces.push(_objProvince);
        });

        CompareProvince();
    }
    else {
        var catIndex = _provinceIDs.indexOf(data.id);
        if (catIndex > -1) {
            _provinceIDs.splice(catIndex, 1);

            _SelectedProvinces = new Array();
            $.each(_provinceIDs, function (index, object) {
                var _objProvince = new Object();
                _objProvince.id = object;
                _objProvince.clickColor = _DmaChartColors[index];

                $.each(graphEntity.items[object].svgElems, function (index1, svgElem) {
                    $(svgElem.graphic[0]).css("fill", _DmaChartColors[index]);
                });

                _SelectedProvinces.push(_objProvince);
            });

            CompareProvince();
        }
    }
}

var x,y,zInterval;
var Interval=0;
document.onmousemove = setMouseCoords;

function setMouseCoords(e) {
    if(document.all) {
        tooptipx = window.event.clientX;
        tooptipy = window.event.clientY + 600;
       
    } else {
   
        tooptipx = e.pageX;
        tooptipy = e.pageY;
    }
}
function showToolTipOnChart(zText) {
    clearInterval(zInterval);
    zInterval = setTimeout("doShowToolTip('" + zText.trim() + "')",0);
    Interval=0;
}
function doShowToolTip(zText) {
    clearInterval(zInterval);
//    if(zText=='divover'){
//   
//    }
//    else{
    document.getElementById("mapToolTip").style.top = (tooptipy+10) + "px";
    document.getElementById("mapToolTip").style.left = tooptipx + "px";
    document.getElementById("mapToolTip").innerHTML = zText.trim();
    document.getElementById("mapToolTip").style.display = "block";
    zInterval = setTimeout("hideToolTip()",500000);
//    }
}

function hideToolTip1() {
    if(Interval!=1000)
    {
        document.getElementById("mapToolTip").style.display = "none";
        clearInterval(zInterval);
        Interval=0;
    }
}
function hideToolTip() {
    zInterval = setTimeout("hideToolTip1()",0);
    Interval=0;
   
}
function hideToolTipDiv() {
    zInterval = setTimeout("hideToolTip1()",100000);
    Interval=1000;
}

function SetCurrentSelectedItemsFillColor(chartObj, selectedItems)
{
    $.each(selectedItems, function (index, object) {
        $.each(chartObj.annotations._renderer.entities.items[object.id].svgElems, function (index1, svgElem) {
            $(svgElem.graphic[0]).css("fill", selectedItems[index].clickColor);
        }); 
    });
}

function GetAdhocSummaryData() {
    $('#divDateSelector').show();

    var source = getParameterByName("source");
    var isOnlyParents = getParameterByName("isOnlyParents");
    var sinceID = getParameterByName("sinceID");
    var mediaIDs = null;
    var jsonPostData;

    if (parent._MediaIDs != null)
    {
        mediaIDs = parent._MediaIDs.join(",");
    }

    if (isOnlyParents != "" && sinceID != "")
    {
        jsonPostData = {
            mediaIDs: mediaIDs,
            source: source,
            fromDate: parent._fromDate,
            ToDate: parent._toDate,
            searchRequestID: parent._RequestID,
            mediumTypes: parent._SubMediaTypes,
            keyword: parent._Keyword,
            sentiment: parent._Sentiment,
            prominenceValue : parent._ProminenceValue,
            isProminenceAudience : parent._IsProminenceAudience,
            isOnlyParents: isOnlyParents,
            isRead: parent._IsRead,
            sinceID : sinceID,
            dmaIds: parent._DmaIDs,
            isHeardFilter: parent._Heard,
            isSeenFilter: parent._Seen,
            isPaidFilter: parent._Paid,
            isEarnedFilter: parent._Earned,
            usePESHFilters: parent._Heard || parent._Seen || parent._Paid || parent._Earned,
            showTitle: parent._showTitle,
            dayOfWeek: parent._dayOfWeek,
            timeOfDay: parent._timeOfDay,
            useGMT: parent._useGMT,
            isHour: parent._isHourInterval,
            isMonth: parent._isMonthInterval,
            stationAffil: parent._stationAffil
        }
    }
    else
    {
        jsonPostData = {
            mediaIDs: mediaIDs,
            source: source
        }
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlDashboardGetAdhocSummaryData,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),
        success: function(result) { 
            if (result.isSuccess)
            {
                OnResultSearchComplete(result, function() { return false; });

                _FromDate = result.fromDate;
                $("#dpFrom").datepicker("setDate", _FromDate);
                _ToDate = result.toDate;
                $("#dpTo").datepicker("setDate", _ToDate);

                $("#divLineChartOptions").hide();

                $(".liSubMediaCharts").prop("onclick", null);
                $(".liSubMediaCharts").removeClass("cursorPointer");
            }
            else
            {
                ShowNotification(result.errorMessage);
            }
        },
        error: OnDataMediumWiseFail
    });    
}

function OpenThirdPartySeriesPopup() {
    $('#divThirdPartyPopup').modal({
        backdrop: 'static',
        keyboard: true,
        dynamic: true
    });
}

function CancelThirdPartyPopup() {
    $("#divThirdPartyPopup").css({ "display": "none" });
    $("#divThirdPartyPopup").modal("hide");
}

function SelectThirdPartySeries(chk, groupID) {
    if ($(chk).is(":checked")) {
        var numTotal = $("#ulThirdPartySeries input[name='chkThirdPartySeries_" + groupID + "']");
        var numChecked = $("#ulThirdPartySeries input[name='chkThirdPartySeries_" + groupID + "']:checked");

        $("#chkThirdPartyGroup_" + groupID).prop("checked", numTotal.length == numChecked.length);
    }
    else {
        $("#chkThirdPartyGroup_" + groupID).prop("checked", false);
    }
}

function SelectThirdPartySeriesByGroup(chk, groupID) {
    $("#ulThirdPartySeries input[name='chkThirdPartySeries_" + groupID + "']").prop("checked", $(chk).is(":checked"));
}

function SaveThirdPartySeries() {
    var selectedSeries = $.map($('#ulThirdPartySeries input[name^="chkThirdPartySeries_"]:checked'), function (obj, index) {
        return $(obj).val();
    });

    var jsonPostData = {
        dataTypeIDs: selectedSeries
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlDashboardSaveDataTypeSelections,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess) {
                ShowNotification("Series updated successfully");
                CancelThirdPartyPopup();

                // If the selection has changed, reload the page
                if ($(_SelectedThirdPartySeries).not(selectedSeries).length != 0 || $(selectedSeries).not(_SelectedThirdPartySeries).length != 0) {
                    _SelectedThirdPartySeries = selectedSeries;
                    GetDataMediumWise('Overview','Overview');
                }
            }
            else {
                ShowNotification(_msgErrorOccurred);
            }
        },
        error: function (result) {
            ShowNotification(_msgErrorOccurred);
        }
    });   
}

/* Industry Cohorts Code */
var _chartJSON = null;
var _savedRespose = null;
var _noDataDiv = '<div style="font-size:20px;font-weight:bold;padding-top:25%;text-align:center;">No Data Available</div>';
var _subGroup = null;

function ChangeMarket(e) {
    _subGroup = e.id.split('_')[1];
    var postData = {
        cohortID: _cohortChoice,
        startDate: _FromDate,
        endDate: _ToDate,
        report: "Market",
        subGroup: _subGroup
    };

    GetReport(postData);
}

function ChangeNetwork(e) {
    _subGroup = e.id.split('_')[1];

    var postData = {
        cohortID: _cohortChoice,
        startDate: _FromDate,
        endDate: _ToDate,
        report: "Network",
        subGroup: _subGroup
    };

    GetReport(postData);
}

function ChangeProgram(e) {
    _subGroup = e.id.split('_')[1];

    var postData = {
        cohortID: _cohortChoice,
        startDate: _FromDate,
        endDate: _ToDate,
        report: "Program",
        subGroup: _subGroup
    };

    GetReport(postData);
}

function ChangeStation(e) {
    _subGroup = e.id.split('_')[1];

    var postData = {
        cohortID: _cohortChoice,
        startDate: _FromDate,
        endDate: _ToDate,
        report: "Station",
        subGroup: _subGroup
    };

    GetReport(postData);
}

function GetOverviewReport(e) {
    _subGroup = null;
    // Format side nav bar to correctly highlight tab choice
    RemoveHighlightFromMenu();
    _reportTab = e.id.split('_')[1];
    $("#" + e.id).addClass("highlightedli");

    var postData = {
        cohortID: _cohortChoice,
        startDate: _FromDate,
        endDate: _ToDate,
        report: e.id.split("_")[1]
    };

    if (_reportTab !== "Overview")
    {
        $("#secondaryCohortNav").show();
    }
    else
    {
        $("#secondaryCohortNav").hide();
    }

    GetReport(postData);
}

function ChangeCohort(e) {
    _cohortChoice = e.id.split('_')[1];

    if (_reportTab === "Overview")
    {
        $("#cohortReportList").children().removeClass("highlightedli");
        $("#cohortReportList").children("#" + e.id).addClass("highlightedli");
        $("#displayedReport").text($("#" + e.id).text());
    }
    else if (_reportTab === "Brand")
    {
        _subGroup = null;
    }

    $("#cohortList").children().removeClass("highlightedli");
    $("#cohortList").children("#" + e.id).addClass("highlightedli");
    $("#displayedIndustry").text($("#" + e.id).text());

    var postData = {
        cohortID: _cohortChoice,
        startDate: _FromDate,
        endDate: _ToDate,
        report: _reportTab,
        subGroup: _subGroup
    };

    GetReport(postData);
}

function ChangeBrand(e) {
    _subGroup = e.id.split('_')[1];
    var postData = {
        cohortID: _cohortChoice,
        startDate: _FromDate,
        endDate: _ToDate,
        report: "Brand",
        subGroup: _subGroup
    };

    GetReport(postData);
}

function GetReport(postData) {
    console.time("GetReport");
    $.ajax({
        url: "/Dashboard/GetReport/",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        dataType: "json",
        data: JSON.stringify(postData),
        success: function (result) {
            if (result.isSuccess)
            {
                _isReports = true;
                _savedRespose = result;
                $("#divHeader").html("");
                $("#cohortNavBar").show();

                $('#divMediumData').html('');
                $('#divMediumData').html(result.HTML);

                $("#overviewBrands").text(result.Brands);
                $("#overviewAirings").text(result.Airings);
                $("#overviewTVSpend").text(result.AdSpend);
                $("#overviewAudience").text(result.Audience);

                if (result.LineChartAiring !== null && result.LineChartAiring.length > 0)
                {
                    $("#divLineChartAiring").html("");
                    $("#divLineChartAiring").highcharts(RenderHCLineChart(result.LineChartAiring));
                }
                else
                {
                    $("#divLineChartAiring").html(_noDataDiv);
                }

                if (result.PieChartAiring !== null && result.PieChartAiring.length > 0)
                {
                    $("#divPieChartAiring").html("");
                    $("#divPieChartAiring").highcharts(RenderHCPieChart(result.PieChartAiring));

                    if (IsPieEmpty("divPieChartAiring"))
                    {
                        $("#divPieChartAiring").html(_noDataDiv);
                    }
                }
                else
                {
                    $("#divPieChartAiring").html(_noDataDiv);
                }

                if (result.AreaChartShare !== null && result.AreaChartShare.length > 0)
                {
                    $("#divAreaChartShare").html("");
                    $("#divAreaChartShare").highcharts(RenderHCAreaChart(result.AreaChartShare));
                }
                else
                {
                    $("#divAreaChartShare").html(_noDataDiv);
                }

                if (result.PieChartShare !== null && result.PieChartShare.length > 0)
                {
                    $("#divPieChartShare").html("");
                    $("#divPieChartShare").highcharts(RenderHCPieChart(result.PieChartShare));

                    if (IsPieEmpty("divPieChartShare"))
                    {
                        $("#divPieChartShare").html(_noDataDiv);
                    }
                }
                else
                {
                    $("#divPieChartShare").html(_noDataDiv);
                }

                if (result.LineChartAdSpend !== null && result.LineChartAdSpend.length > 0)
                {
                    $("#divLineChartAdSpend").html("");
                    $("#divLineChartAdSpend").highcharts(RenderHCLineChart(result.LineChartAdSpend));
                }
                else
                {
                    $("#divLineChartAdSpend").html(_noDataDiv);
                }

                if (result.PieChartAdSpend !== null && result.PieChartAdSpend.length > 0)
                {
                    $("#divPieChartAdSpend").html("");
                    $("#divPieChartAdSpend").highcharts(RenderHCPieChart(result.PieChartAdSpend));

                    if (IsPieEmpty("divPieChartAdSpend"))
                    {
                        $("#divPieChartAdSpend").html(_noDataDiv);
                    }
                }
                else
                {
                    $("#divPieChartAdSpend").html(_noDataDiv);
                }

                if (result.PieChartGender !== null && result.PieChartGender.length > 0)
                {
                    $("#divPieChartGender").html("");
                    $("#divPieChartGender").highcharts(RenderHCPieChart(result.PieChartGender));

                    if (IsPieEmpty("divPieChartGender"))
                    {
                        $("#divPieChartGender").html(_noDataDiv);
                    }
                }
                else
                {
                    $("#divPieChartGender").html(_noDataDiv);
                }

                if (result.PieChartAge !== null && result.PieChartAge.length > 0)
                {
                    $("#divPieChartAge").html("");
                    $("#divPieChartAge").highcharts(RenderHCPieChart(result.PieChartAge));

                    if (IsPieEmpty("divPieChartAge"))
                    {
                        $("#divPieChartAge").html(_noDataDiv);
                    }
                }
                else
                {
                    $("#divPieChartAge").html(_noDataDiv);
                }

                $("#divMarketTable").html(result.marketTable);
                $("#divNetworkTable").html(result.networkTable);
                $("#divStationTable").html(result.stationTable);
                $("#divProgramTable").html(result.showTable);

                if (postData.report === "Overview" || postData.report === "Market" || postData.report === "Brand")
                {
                    $("#earnedNM").text(addCommas(result.EarnedNM));
                    $("#earnedBL").text(addCommas(result.EarnedBL));
                    $("#earnedPR").text(addCommas(result.EarnedPR));
                }

                if (result.DropDownList !== null)
                {
                    $("#cohortReportList").html(result.DropDownList);
                }

                switch (postData.report)
                {
                    case "Overview":
                        $("#cohortReportText").html("Industry Overview:&nbsp;");
                        break;
                    case "Market":
                        $("#cohortReportText").html("Market Overview:&nbsp;");
                        break;
                    case "Network":
                        $("#cohortReportText").html("Network Overview:&nbsp;");
                        break;
                    case "Program":
                        $("#cohortReportText").html("Program Overview:&nbsp;");
                        break;
                    case "Station":
                        $("#cohortReportText").html("Station Overview:&nbsp;");
                        break;
                    case "Brand":
                        $("#cohortReportText").html("Brand Overview:&nbsp;");
                        break;
                }

                $("#displayedReport").text(result.SelectedOverview);

                UnhideCohortElements();
                HideElements(result.HiddenElements);

                //                $("#divChartOptions0").children().on("click", ChangeDateInterval);
                //                $("#divChartOptions2").children().on("click", ChangeDateInterval);

                console.timeEnd("GetReport");
            }
        },
        error: function (a, b, c) {
            console.error("GetOverviewReport ajax error - " + c);
            ShowNotification(_msgErrorOccured);
        }
    });
}

function RenderHCLineChart(chartJSON) {
    var chart = JSON.parse(chartJSON);
    chart.legend.layout = 'horizontal';
    chart.legend.verticalAlign = "bottom";
    chart.legend.align = "left";
    chart.legend.x = "-5";

    // Legend height should be maxed out at 3 lines
    // Legend line height: 18px
    // Navigation height: 16px
    // HC will consistently add 8 to whatever this is set to - no idea why
    chart.legend.maxHeight = "70";

    //chartJSON.legend.symbolWidth = "16";
    chart.legend.itemStyle = {
        fontSize: "10px"
    };
    chart.yAxis.labels = {
        style: {
            fontSize: "10px"
        }
    };
    chart.xAxis.labels = {
        style: {
            fontSize: "10px"
        }
    };

    chart.tooltip.formatter = FormatSplineTooltip;

    return chart;
}

function RenderHCAreaChart(chartJSON) {
    var chart = JSON.parse(chartJSON);

    chart.chart.type = "area";
    //chart.plotOptions.spline = null;
    var splineOptions = chart.plotOptions.spline;

//    console.dir(chart.plotOptions);
    chart.plotOptions.area = {
        stacking: "normal"
    };

    chart.legend.layout = 'horizontal';
    chart.legend.verticalAlign = "bottom";
    chart.legend.align = "left";
    chart.legend.x = "-5";

    // Legend height should be maxed out at 3 lines
    // Legend line height: 18px
    // Navigation height: 16px
    // HC will consistently add 8 to whatever this is set to - no idea why
    chart.legend.maxHeight = "70";

    //chartJSON.legend.symbolWidth = "16";
    chart.legend.itemStyle = {
        fontSize: "10px"
    };
    chart.yAxis.labels = {
        style: {
            fontSize: "10px"
        }
    };
    chart.xAxis.labels = {
        style: {
            fontSize: "10px"
        }
    };

    chart.tooltip.formatter = FormatSplineTooltip;

    return chart;
}

function RenderHCPieChart(chartJSON) {
    var chart = JSON.parse(chartJSON);
    chart.tooltip.formatter = FormatPieTooltip;
    chart.chart.height = "300";
    chart.chart.width = "425";
    chart.title = "";
    chart.legend.enabled = "true";
    chart.legend.layout = 'horizontal';
    chart.legend.verticalAlign = "bottom";
    chart.legend.x = "-5";
    chart.legend.width = "425";
    chart.legend.labelFormatter = FormatPieLegend;

    // Legend height should be maxed out at 3 lines
    // Legend line height: 18px
    // Navigation height: 16px
    // HC will consistently add 8 to whatever this is set to - no idea why
    chart.legend.maxHeight = "70";

    chart.legend.itemStyle = {
        fontSize: "10px"
    };

//    chart.series[0].dataLabels = {
//        formatter: FormatPieLabel,
//        crop: "false",
//        overflow: "none"
//    };
//    chart.plotOptions.dataLabels = {
//        crop: "false",
//        overflow: "none"
//    };


    return chart;
}

function FormatPieTooltip() {
    //console.log($(this.series.chart.renderTo).prop("id"));
    var chartDiv = $(this.series.chart.renderTo).prop("id");
    var innerIndex = (this.point.x - (this.point.x % 2)) / 2;
    var innerSlice = this.point.series.chart.series[0].points[innerIndex];
    var total = chartDiv == "divPieChartAdSpend" ? addCommas(this.series.total.toFixed(2)) : addCommas(this.series.total);
    var sliceTotal = chartDiv === "divPieChartAdSpend" ? addCommas(this.point.y.toFixed(2)) : addCommas(this.point.y);
    var usdPrefix = chartDiv === "divPieChartAdSpend" ? "$" : "";

    var tooltip = "<span style=\"color:" + this.point.color + "\">\u25CF<span> " + this.point.name + ": <br/><b>" +
        usdPrefix + sliceTotal + "/" + usdPrefix + total + " = " + addCommas(this.point.percentage.toFixed(2)) + "%</b> of total";

    return tooltip;
}

function FormatPieLabel() {
//    return "<span style=\"color:" + this.point.color + "\">\u25CF</span>" + this.point.name + ": " +
//     addCommas(this.point.percentage.toFixed(2)) + "%";

    if (this.y !== 0)
    {
        return "<span style=\"color:" + this.point.color + "\">\u25CF</span>" + addCommas(this.point.percentage.toFixed(2)) + "%";
    }
    else
    {
        return;
    }


}

function FormatPieLegend() {
    return this.name + ": " + this.percentage.toFixed(2) + "%";
}

function FormatSplineTooltip() {
    var pointDate = this.point.Date;
    var chartDiv = $(this.series.chart.renderTo).prop("id");

    var amplificationHeader = "<span style=\"font-size:10px\">" + pointDate + "</span><br/>";
    var commonContent = "<span style=\"color:" + this.series.color + "\">\u25CF<span> " + this.series.name + ": <b>";
    var count = (chartDiv === "divLineChartAdSpend" ? "$" : "") + addCommas((chartDiv === "divLineChartAdSpend" ? this.point.y.toFixed(2) : this.point.y)) + "</b><br/>";
//    var count = (chartDiv === "divChart2" ? "$" : "") + addCommas(this.point.y) + "</b><br/>";

    return amplificationHeader + commonContent + count;
}

// Adds thousand delimiter commas to a number string
function addCommas(str) {
    str = str.toString();
    if (str.toLowerCase() == 'nan' || str == '')
    {
        return 0;
    }
    else
    {
        var parts = (str + "").split("."),
        main = parts[0],
        len = main.length,
        output = "",
        first = main.charAt(0),
        i;

        if (first === '-')
        {
            main = main.slice(1);
            len = main.length;
        } else
        {
            first = "";
        }
        i = len - 1;
        while (i >= 0)
        {
            output = main.charAt(i) + output;
            if ((len - i) % 3 === 0 && i > 0)
            {
                output = "," + output;
            }
            --i;
        }
        // put sign back
        output = first + output;
        // put decimal part back
        if (parts.length > 1)
        {
            output += "." + parts[1];
        }
        return output;
    }
}

// No longer in use - charts are kept to day interval
function ChangeDateInterval() {
    if (!$(this).hasClass("activeDuration"))
    {
        var parent = this.parentElement;
        // Set active interval
        $(parent.children).removeClass("activeDuration");
        $(this).addClass("activeDuration");

        // Get last char in parent element id to select correct chart to render in
        var chartNumber = parent.id.charAt(parent.id.length - 1);

        var sumAds = parent.id === "divChartOptions2" ? "IQMediaValue" : "Docs";

        var postData = {
            dateInterval: $(this).text().trim().toLowerCase(),
            startDate: _FromDate.split(" ")[0],
            endDate: _ToDate.split(" ")[0],
            summation: sumAds
        };

        $.ajax({
            url: "/Dashboard/GetNewLineChart/",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            dataType: "json",
            data: JSON.stringify(postData),
            success: function (result) {
                if (result.isSuccess)
                {
                    $("#divChart" + chartNumber).highcharts(RenderHCLineChart(result.Chart));
                }
                else
                {

                }
            },
            error: function (a, b, c) {
                console.error("GetNewLineChart ajax error - " + c);
                ShowNotification(_msgErrorOccured);
            }
        });
    }
}

function HideElements(elementList) {
    if (elementList !== null && elementList.length > 0)
    {
        for (var i = 0; i < elementList.length; i++)
        {
            _hiddenIds.push(elementList[i]);
            $("#" + elementList[i]).hide();
        }
    }
}

function UnhideCohortElements() {
    if (_hiddenIds !== null && _hiddenIds.length > 0)
    {
        for (var i = 0; i < _hiddenIds.length; i++)
        {
            $("#" + _hiddenIds[i]).show();
        }
    }

    _hiddenIds = [];
}

function IsPieEmpty(container) {
    var chart = $("#" + container).highcharts();
    var series = chart.series[0].data;
    var isEmpty = true;
    $.each(chart.series, function (si, sd) {
        $.each(sd.data, function (i, d) {
            if (d.y !== 0)
            {
                isEmpty = false;
            }
        });
    });

    return isEmpty;
}