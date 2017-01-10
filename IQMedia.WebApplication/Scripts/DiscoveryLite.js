var searchTermCount = 0;
var _searchTerm = new Array();
var _searchTermbkp = '';
var _SearchTermValidationMessage = '';


var _SearchMedium = '';
var _SearchMediumDesc = '';
var _SearchMediumbkp = '';

var _SearchTVMarket = '';
var _SearchTVMarketbkp = '';

var _SearchTermIndex = 0;

var _SearchTermResult = '';
var msg = '';
var _NeedToValidateSearchTerm = true;
var _imgLoading = '<img alt="" id="imgSaveSearchLoading" class="marginRight10" src="../../Images/Loading_1.gif" />'

var _fromDate = '';
var _toDate = '';
var _IsDefaultLoad = true;
var _PieChartIndividualTotals = null;
var _PieChartDisplayedTotals = null;

function AddNewSearchTermTextBox() {

    $('#divPopover').remove();
    searchTermCount = searchTermCount + 1;
    $('#ulSearchTerm').append("<li style=\"list-style:none outside none\" id=\"lisearchTerm_" + searchTermCount + "\" ><img id=\"imgRemoveSearchTermTextBox_" + searchTermCount + "\" src=\"../../Images/Delete.png?v=1.1\" alt=\"\" onclick=\"$('#lisearchTerm_" + searchTermCount + "').remove();PushSearchTermintoArray();CheckForMaxSearchTerm();\" class=\"marginTop-9 marginRight9 cursorPointer\" /><input type=\"text\" placeholder=\"Search Term\" id=\"txtSearchTerm_" + searchTermCount + "\" onclick=\"ShowAddSearchTermPopup(this.id);\" readonly=\"readonly\" /></li>");

    ShowAddSearchTermPopup("txtSearchTerm_" + searchTermCount);
    CheckForMaxSearchTerm();
}

function CheckForMaxSearchTerm() {
    if ($('#ulSearchTerm li').length >= 5) {
        $('#divAddItem').hide();
    }
    else {
        $('#divAddItem').show();
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
        content: '<div><input type=\"text\" class=\"popOverTextBox\" placeholder=\"Search Term\" id=\"txtSearchTermPopup_' + elementID.replace('txtSearchTerm_', '') + '\" onkeypress="GetChartData(event);" onblur="SetCurrentSearchTermTextBox(txtSearchTermPopup_' + elementID.replace('txtSearchTerm_', '') + ',txtSearchTerm_' + elementID.replace('txtSearchTerm_', '') + ');" /></div>'
    });


    $('#' + elementID).popover('show');
    $('#txtSearchTermPopup_' + elementID.replace('txtSearchTerm_', '')).focus();
    $('#txtSearchTermPopup_' + elementID.replace('txtSearchTerm_', '')).val($('#' + elementID).val());

}

$(document).ready(function () {
    $('#ulMainMenu li').removeAttr("class");
    $('#liMenuDiscoveryLite').attr("class", "active");

    var _tDate = new Date();
    var _fDate = new Date(_tDate.getFullYear(), _tDate.getMonth() - 3, _tDate.getDate());

    $("#dpFrom").val((_fDate.getMonth() + 1) + "/" + _fDate.getDate() + "/" + _fDate.getFullYear());
    $("#dpTo").val((_tDate.getMonth() + 1) + "/" + _tDate.getDate() + "/" + _tDate.getFullYear());

    _fromDate = $('#dpFrom').val();
    _toDate = $('#dpTo').val();

    $('#imgResult').hover(function () {
        $('#imgResult').attr('src', '../../Images/Result-hover.png');
    }, function () {
        $('#imgResult').attr('src', '../../Images/Result.png');
    });

    $('#imgChart').hover(function () {
        $('#imgChart').attr('src', '../../Images/Chart-hover.png');
    }, function () {
        $('#imgChart').attr('src', '../../Images/Chart.png');
    });

    AddNewSearchTermTextBox();

    $("#divPieChartHeader").delegate('div', 'click', function () {
        ShowPieChart($(this).index());
    });

    SetModalBodyScrollBarForPopUp();

    $('#divChartMain').css({ 'height': documentHeight - 100 });

    $('input:checkbox[name=chkPieChartMedium]').click(ShowHidePieChartMedium);

});

$(window).resize(function () {
    if (screen.height >= 768) {
        $('#divChartMain').css({ 'height': documentHeight - 100 });
    }
});

function SetMedium(mediumName, mediumDesc) {
    if (mediumName != "TV") {
        _SearchTVMarket = '';
        _SearchMediumDesc = '';
    }
    //ClearChartDateVariable();

    _SearchMedium = mediumName;
    _SearchMediumDesc = mediumDesc;
    ResetSearchTermClassToFalse();

    SearchResult();
}

function SetTVMarket(tvMarket) {
    //ClearChartDateVariable();
    _SearchTVMarket = tvMarket;
   
    ResetSearchTermClassToFalse();

    SearchResult();
}

function SetCurrentSearchTermTextBox(popupControlID, searchTermTextBoxID) {

    //alert('blur');
    //alert(_NeedToValidateSearchTerm);
    //setTimeout(function () {
    $('#' + searchTermTextBoxID.id).val($('#' + popupControlID.id).val());

    //if (_NeedToValidateSearchTerm)
    $('#divPopover').remove();
    PushSearchTermintoArray();
    //}, 1000)
}

function PushSearchTermintoArray() {
    //if (_NeedToValidateSearchTerm) {
    $('#divNoDataChart').html('');
    //$('#divNoDataResult').html('');

    _SearchTermIndex = 0;
    _searchTerm = new Array();

    
    var isCurrentSearchTermInArray = false;
    var index = 0;
    //var sTermID = 1;
    $('#ulSearchTerm li input[type=text]').each(function () {
        if (_searchTerm.length < 5) {

            if ($(this).val().trim() == _SearchTermResult) {
                _SearchTermIndex = index;
                isCurrentSearchTermInArray = true;
            }
            index = index + 1;
            var SearchTermClass = new Object();
            SearchTermClass.SearchTerm = $(this).val().trim();
            SearchTermClass.ResultShown = false;
            SearchTermClass.IsCurrentTab = false;

            SearchTermClass.ShownRecords = 0;
            SearchTermClass.AvailableRecords = 0;
            SearchTermClass.TotalRecords = 0;
            SearchTermClass.DisplayPageSize = 0;

            /*SearchTermClass.ID = sTermID;
            sTermID += 1;*/
            _searchTerm.push(SearchTermClass);

        }
    });

    if (!isCurrentSearchTermInArray && _searchTerm.length > 0) {
        _SearchTermResult = _searchTerm[0].SearchTerm;
    }

    
    

    if (_searchTerm.length > 0) {
        SearchResult();
    }
    else {
        $('#divDiscoveryClearAll').hide();
        $('#divDiscoveryUtility').hide();
        ClearAllData();
    }
    // }
    //_NeedToValidateSearchTerm = true;
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
    

    $('#ulMedium').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
    $('#ulTVMarket').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
    _SearchMedium = ''
    _SearchMediumDesc = '';
    _SearchTVMarket = '';
    $('#divActiveFilter').html('');
}

function SearchResult() {
    _SearchTermValidationMessage = '';
    if (ValidateSearchTerm()) {
        _IsDefaultLoad = false;
        $('#divDiscoveryClearAll').show();
        SearchResultAjaxRequest();
    }
    else {
        //$('#divDiscoveryClearAll').hide();
        ShowNotification(_SearchTermValidationMessage);
    }
}

function OnMediaSearchComplete(result) {

    
    $('#divNoDataChart').html('');

    if (result.availableDataChart) {
        SetNoDataAvailableMessage(result.availableDataChart, 'divNoDataChart');
    }
    if (result.isSuccess) {
        if (result.isSearchTermValid) {
            if (!_IsDefaultLoad) {
                $('#divDiscoveryUtility').show();
                _searchTermbkp = _searchTerm;
                _SearchMediumbkp = _SearchMedium;
                _SearchTVMarketbkp = _SearchTVMarket;
                _PieChartIndividualTotals = result.pieChartSearchTermTotals;

                RenderColumnChartHighCharts(result.columnChartJson);
                RenderLineHighChart(result.lineChartJson);
                RenderPieChartHighCharts(result.pieChartMediumJson, result.pieChartSearchTermJson);
                SetMediumFilter(result);
                SetTVMarketFilter(result);

                $('#divChartTotal').html('Total Records :: ' + result.chartTotal);
            }
            else {
                $('#divPieChartHeader').hide();
                $('#divPieChartData').hide();
                $('#divDiscoveryClearAll').hide();
                $('#divDiscoveryUtility').hide();
            }

        } else {
            ShowNotification(_msgSearchTermAlreadyEntered);
        }
    }
    else {
        
        CheckForAuthentication(result, _msgErrorOccured);
        //ShowNotification('Some error occured, try again later');
    }
    setTimeout(function () {
        //$("#divMainContent").mCustomScrollbar("scrollTo", "top");
    }, 200);
}

function OnFail(result) {
    ShowNotification(_msgErrorOccured);
}


function SearchResultAjaxRequest() {

    var mySearchTermArray = new Array();
    for (var zz = 0; zz < _searchTerm.length; zz++) {
        mySearchTermArray.push(_searchTerm[zz].SearchTerm.trim());
    }

    SetActiveFilter();   

    var jsonPostData = {
        searchTerm: mySearchTermArray,
        fromDate: _fromDate,
        toDate: _toDate,
        medium: _SearchMedium,
        tvMarket: _SearchTVMarket,
        isDefaultLoad: _IsDefaultLoad
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlDiscoveryLiteMediaJsonChart,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),

        success: OnMediaSearchComplete,
        error: OnFail
    });
}

function ValidateSearchTerm() {
    var returnValue = true;

    if (_searchTerm.length <= 0) {
        _SearchTermValidationMessage = _msgEnterSearchTerm;
        returnValue = false;
    }

    for (var i = 0; i < _searchTerm.length; i++) {

        if (_searchTerm[i].SearchTerm.trim() == '') {

            returnValue = false;
            _SearchTermValidationMessage = _msgEnterSearchTerm;
            break;
        }

        for (var j = 0; j < _searchTerm.length; j++) {
            if (i != j) {

                if (_searchTerm[i].SearchTerm.trim() == _searchTerm[j].SearchTerm.trim()) {
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

    
    if (_SearchMediumDesc)
        $('#divActiveFilter').append('<div class=\"filter-in\">' + _SearchMediumDesc + '<span onclick="RemoveMediumFilter();" class="cancel"></span></div>');


    if (_SearchTVMarket)
        $('#divActiveFilter').append('<div class=\"filter-in\">' + _SearchTVMarket + '<span onclick="RemoveTVMarketFilter();" class="cancel"></span></div>');

    if ($('#divActiveFilter').html()) {
        $('#divActiveFilter').addClass("bottomBorderColor");
    }
}


function SetNoDataAvailableMessage(message, divID) {
    var noDataHTML = '<div class="alert" id="divNoData">'
    //                    + '<button type="button" class="close" data-dismiss="alert">'
    //                      + '&times;</button>'
                     + '<div id="divNotAvailableDataMessage" class="row-fluid filter margin0">' + message
    //+ '<input value="Go" class="RefreshResult" id="btnRefresh Data" type="button" alt="Go" onclick="RefreshResult();" />'
                     + '</div>'

                 + '</div>';
    $('#' + divID).append(noDataHTML);
    //onclick="var documentHeight = $(window).height();$(\'#divMainContent\').css({ \'height\': documentHeight - 200 });"
    //$('#divNotAvailableDataMessage').html(message);
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
    //if (JsonLineChart.chart.events.load != null) {
        //JsonLineChart.chart.events.load = LoadChartAutoMarker;
        //JsonLineChart.plotOptions.series.point.events.mouseOver = ChartHoverManage;
        //JsonLineChart.plotOptions.series.point.events.mouseOut = ChartHoverOutManage;
    //}
    $('#divLineChart').highcharts(JsonLineChart);
}

//var i = 0;
//var myVar = null;
//function LoadChartAutoMarker() {
//    i = 0;
//    myVar = setInterval(function () { myTimer() }, 1000);
//}

//function ChartHoverOutManage() {

//    myVar = setInterval(function () { myTimer() }, 1000);
//}

//function ChartHoverManage() {

//    if (myVar != null) {

//        clearInterval(myVar);
//        myVar = null;
//        i = this.series.xAxis.categories.indexOf(this.category);
//    }
//}

//function myTimer() {
//    var chart = $('#divLineChart').highcharts();
//    if (i < chart.series[0].data.length) {
//        if (i > 0) {
//            chart.series[0].data[i - 1].setState('');
//            chart.tooltip.refresh(chart.series[0].data[i - 1]);
//        }
//        chart.series[0].data[i].setState('hover');
//        chart.tooltip.refresh(chart.series[0].data[i]);
//        i++;
//    }
//    else {
//        chart.series[0].data[i - 1].setState('');
//        chart.tooltip.refresh(chart.series[0].data[i - 1]);
//        clearInterval(myVar);
//        myVar = null;
//        i = 0;
//        chart.tooltip.hide();
//    }
//}

function RenderPieChartHighCharts(jsonPieChartMediumData, jsonPieChartSearchTermData) {
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
            pieChartHeaderDivHTML = '<div id=\'divPieChart_' + chartCount + '\' align="center" style="float: left; padding: 5px;cursor:pointer;overflow: hidden;padding: 5px;text-overflow: ellipsis;white-space: nowrap;width: 20%" class="pieChartActive" >' + EscapeHTML(eventData.SearchTerm) + '</div>';
            //pieChartChartDivHTML = '<div id=\'divPieChart_Child_' + chartCount + '\'><div class="float-left" id=\'divPieChart_Child_Data_' + chartCount + '\'></div><div class=\'margintop34 float-right width50p wordWrap\' id=\'divPieChart_Child_TopResult_' + chartCount + '\'></div></div>';
        }
        else {
            pieChartHeaderDivHTML = '<div id=\'divPieChart_' + chartCount + '\' align="center" style="float: left; padding: 5px;cursor:pointer;overflow: hidden;padding: 5px;text-overflow: ellipsis;white-space: nowrap;width: 20%" >' + EscapeHTML(eventData.SearchTerm) + '</div>';
            //pieChartChartDivHTML = '<div id=\'divPieChart_Child_' + chartCount + '\' style="visibility:hidden;height:0px;border-top:1px solid gray;"><div id=\'divPieChart_Child_Data_' + chartCount + '\'></div><div id=\'divPieChart_Child_TopResult_' + chartCount + '\'></div></div>';

        }
        //pieChartChartDivHTML = '<div id=\'divPieChart_Child_' + chartCount + '\'><div class="float-left" id=\'divPieChart_Child_Data_' + chartCount + '\'></div><div class=\'margintop10 float-right width50p wordWrap\' id=\'divPieChart_Child_TopResult_' + chartCount + '\'></div></div>';
        pieChartChartDivHTML = '<div id=\'divPieChart_Child_' + chartCount + '\'><div  class="float-left" id=\'divPieChart_Child_Data_' + chartCount + '\'></div><div class=\'margintop10 float-left divtopres wordWrap\' id=\'divPieChart_Child_TopResult_' + chartCount + '\'></div></div>';


        $('#divPieChartHeader').append(pieChartHeaderDivHTML);
        $('#divPieChartHeader').show();

        $('#divPieChartData').append(pieChartChartDivHTML);
        $('#divPieChartData').show();

        $("#divPieChart_Child_Data_" + chartCount).highcharts(JSON.parse(eventData.JsonResult));
        $('#divPieChart_Child_TopResult_' + chartCount).html(eventData.TopResultHtml);

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
            });
        });

        chartCount = chartCount + 1;
    });

    ShowPieChart(0);

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
        var selectedMedium = _SearchMedium;
        if (selectedMedium == "Social Media") {
            selectedMedium = "SocialMedia";
        }

        if (selectedMedium == '') {
            $('input:checkbox[name=chkPieChartMedium]').filter('[value=TV]').prop('checked', true);
            $('input:checkbox[name=chkPieChartMedium]').filter('[value=NM]').prop('checked', true);
            $('input:checkbox[name=chkPieChartMedium]').filter('[value=TW]').prop('checked', false);
            $('input:checkbox[name=chkPieChartMedium]').filter('[value=Blog]').prop('checked', false);
            $('input:checkbox[name=chkPieChartMedium]').filter('[value=Forum]').prop('checked', false);
            $('input:checkbox[name=chkPieChartMedium]').filter('[value=SocialMedia]').prop('checked', false);
            $('input:checkbox[name=chkPieChartMedium]').filter('[value=PQ]').prop('checked', false);
            pieChartChks.removeAttr('disabled');
        }
        else {
            pieChartChks.filter('[value=' + selectedMedium + ']').prop('checked', true);
            pieChartChks.filter('[value!=' + selectedMedium + ']').prop('checked', false);
            pieChartChks.prop('disabled', 'disabled');
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


function SetMediumFilter(result) {
    $('#ulMedium').html('');
    var discoveryMedium = '';
    $.each(result.discoveryMediumFilter, function (eventID, eventData) {

        discoveryMedium = discoveryMedium + ' <li onclick="SetMedium(\'' + eventData.Medium + '\',\'' + eventData.MediumValue + '\');" role="presentation"><a href="#" tabindex="-1" role="menuitem">' + eventData.MediumValue + '</a></li>';
    });

    if (discoveryMedium != '') {
        $('#ulMedium').html(discoveryMedium);
    }
    else {
        $('#ulMedium').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
    }
}

function GetChartData(e) {
    if (e.keyCode == 13) {
        $('#' + $(e.target).attr("id")).blur();
        //$('#divAddItem').focus();
    }
}

function RemoveSearchTerm(divID) {

    var textBoxLiID = divID.replace('divFilterSearchTerm_', 'lisearchTerm_');

    $('#' + textBoxLiID).remove();
    PushSearchTermintoArray();
}


function RemoveMediumFilter() {
    _SearchMedium = '';
    _SearchMediumDesc = ''
    ResetSearchTermClassToFalse();
    SearchResult();
}

function RemoveTVMarketFilter() {
    _SearchTVMarket = '';
    ResetSearchTermClassToFalse();
    SearchResult();
}


function ResetSearchTermClassToFalse() {
    for (var zz = 0; zz < _searchTerm.length; zz++) {
        _searchTerm[zz].ResultShown = false;
    }
}

function GenerateDashboardPDF() {
    var mySearchTermArray = new Array();
    for (var zz = 0; zz < _searchTerm.length; zz++) {
        mySearchTermArray.push(_searchTerm[zz].SearchTerm.trim());
    }

    if (_searchTerm.length > 1) {
        ToggleShareOfVoicePDFDisplay(true);
    }

    var jsonPostData = {
        p_HTML: $("#divChartMain").html(),
        p_FromDate: $("#dpFrom").val(),
        p_ToDate: $("#dpTo").val(),
        p_SearchTerm: mySearchTermArray
    }

    if (_searchTerm.length > 1) {
        ToggleShareOfVoicePDFDisplay(false);
    }

    $.ajax({
        url: _urlDiscoveryLiteGenerateDiscoveryPDF,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess) {
                window.location = _urlDiscoveryLiteDownloadPDFFile;
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

function ToggleShareOfVoicePDFDisplay(isExporting) {
    if (isExporting) {
        $("input:checkbox[name=chkPieChartMedium]:not(:checked)").parent().css({ "text-decoration": "line-through" });
        $("input:checkbox[name=chkPieChartMedium]").hide();
        $("#divPieChartStatic").hide();
        $("#divPieChartDynamic").hide();

        SetPieChartInnerText("divPieChartDynamicPDF", "divPieChartText", "totalHitsDynamicPDF", _PieChartDisplayedTotals[_PieChartDisplayedTotals.length - 1]);
        SetPieChartInnerText("divPieChartStaticPDF", "divPieChartText", "totalHitsStaticPDF", _PieChartDisplayedTotals[_PieChartDisplayedTotals.length - 2]);
    }
    else {
        $("input:checkbox[name=chkPieChartMedium]").parent().css({ "text-decoration": "" });
        $("input:checkbox[name=chkPieChartMedium]").show();
        $("#divPieChartStatic").show();
        $("#divPieChartDynamic").show();
        $("#totalHitsStaticPDF").remove();
        $("#totalHitsDynamicPDF").remove();
    }

    $("#divPieChartStaticPDF").toggleClass("hiddenPieChart");
    $("#divPieChartDynamicPDF").toggleClass("hiddenPieChart");
}

function ShowDashboardEmailPopup() {
    ShowMessage(_msgDiscoveryLiteEmailClick);
}

function ShowSaveSearchDiscovery() {
    ShowMessage(_msgDiscoveryLiteSavedSearchtClick);
}

function ToggleChartResult() {
    ShowMessage(_msgDiscoveryLiteResultClick);
}

function RefreshResult() {
    PushSearchTermintoArray();
}

function ChartClick() {
    ShowMessage(_msgDiscoveryLiteChartClick);
}

function DeleteDiscoverySavedSearchByID(id) {
    ShowMessage(_msgDiscoveryLiteDeleteSavedSearch);
}

function LoadSavedSearch(id) {
    ShowMessage(_msgDiscoveryLiteChartLoadSavedSearch);
}

function ShowMessage(strmessage) {
    $("#divMessageContent").html(strmessage)
    $('#divMessagePopup').modal({
        backdrop: 'static',
        keyboard: true,
        dynamic: true
    });
}

function CancelMessagePopup() {
    $('#divMessagePopup').css({ "display": "none" });
    $('#divMessagePopup').modal('hide');
}

function ClearSearch() {
    _Searchtv = '';
    _SearchTermIndex = 0;
    _searchTerm = new Array();
    searchTermCount = 0;
    $('#ulSearchTerm').html('');
    _IsDefaultLoad = true;
    ClearAllData();
    AddNewSearchTermTextBox();

    var _tDate = new Date();
    var _fDate = new Date(_tDate.getFullYear(), _tDate.getMonth() - 3, _tDate.getDate());

    $("#dpFrom").val((_fDate.getMonth() + 1) + "/" + _fDate.getDate() + "/" + _fDate.getFullYear());
    $("#dpTo").val((_tDate.getMonth() + 1) + "/" + _tDate.getDate() + "/" + _tDate.getFullYear());

    _fromDate = $('#dpFrom').val();
    _toDate = $('#dpTo').val();

    SearchResultAjaxRequest()
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
    var index = 0;
    var totalHits = 0;

    $.each(dataPoints, function () {
        var searchTerm = this.name;
        newDataPoints[index] = [searchTerm, 0];

        var totalResults = $.grep(_PieChartIndividualTotals, function (element, index) {
            return element.searchTerm == searchTerm;
        });

        $.each(totalResults, function () {
            var chk = enabledMediums.filter('[value=' + this.medium + ']');

            if (chk.length == 1) {
                newDataPoints[index][1] += this.totalResult;
            }
        });

        if (this.visible) {
            totalHits += newDataPoints[index][1];
        }

        index++;
    });

    $("#totalHitsDynamic").remove();
    SetPieChartInnerText("divPieChartDynamic", "divPieChartText", "totalHitsDynamic", totalHits);
    _PieChartDisplayedTotals[_PieChartDisplayedTotals.length - 1] = totalHits;

    $("#divPieChartDynamic").highcharts().series[0].setData(newDataPoints, true);
    $("#divPieChartDynamicPDF").highcharts().series[0].setData(newDataPoints, true);
}