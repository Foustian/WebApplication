var _Date = null;
var _Station = "";
var _urlSonyMediaSearchResults = "/SonyMedia/SonyMediaSearchResults/"
var _urlSonyMediaSearchResultsPaging = "/SonyMedia/SonyMediaSearchResultsPaging/"
var _urlSonyMediaGetChart = "/SonyMedia/GetChart/"
var _msgErrorOccured = "Some error occured, try again later";
var _msgStationRequired = "Please Select Station";
var _msgDateRequired = "Please Select Date";
var _SearchTerm = "";
var _IsManualHover = false;
var _IsManualScroll = false;


$(document).ready(function () {
    var documentHeight = $(window).height();
    $("#divTVScrollContent").css({ "height": documentHeight - 300 });

    $("#divCalender").datepicker({
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            SetDateVariable(dateText);
        }

    });

    $('.ndate').click(function () {
        $("#divCalender").datepicker("refresh");
        //$("#divRadioCalender").datepicker("refresh");
    });

    $("body").click(function (e) {
        if (e.target.id == "liStationFilter" || $(e.target).parents("#liStationFilter").size() > 0) {
            if ($('#ulStation').is(':visible')) {
                $('#ulStation').hide();
            }
            else {
                $('#ulStation').show();
            }
        }
        else if ((e.target.id !== "liStationFilter" && e.target.id !== "ulNetwork" && $(e.target).parents("#ulNetwork").size() <= 0)) {
            $('#ulStation').hide();
        }
    });

    $("#divTVScrollContent").enscroll({
        verticalTrackClass: 'track4',
        verticalHandleClass: 'handle4',
        pollChanges: true
    });

    $('#divChartParent').scroll(function () {
        _IsManualScroll = true;
        clearTimeout($.data(this, 'scrollTimer'));
        $.data(this, 'scrollTimer', setTimeout(function () {
            _IsManualScroll = false;
        }, 5000));
    });

    $("#divChartParent").on("mouseout", function () {
        _IsManualHover = false;
    });


    //LoadChartNPlayer('2e9ac54a-0c18-44c2-b3ac-910133423cb4', 'KUSA_20120717_2300');
});

function SetDateVariable(dateText) {

    if (dateText) {
        if (_Date != dateText) {
            _Date = dateText
            SearchResult();
            $('#ulCalender').parent().removeClass('open');
            $('#aDuration').html('Custom&nbsp;&nbsp;<span class="caret"></span>');
        }
    }
}


function SetStation(stationname) {
    if (_Station != stationname) {
        _Station = stationname;
        SearchResult();
    }
}


function SearchResult() {
    SetActiveFilter();
    if (ValidateSearch()) {
        var jsonPostData = {
            p_StationID: _Station,
            p_Date: _Date
        }

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlSonyMediaSearchResults,
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
        p_IsNext: _IsNext
    }
    SetActiveFilter();
    // alert('searchcalled');
    if (ValidateSearch()) {
        
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlSonyMediaSearchResultsPaging,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            success: OnResultSearchComplete,
            error: OnFail
        });
    }


}

function OnResultSearchComplete(result) {

    if (result.isSuccess) {
        
        $("#divPreviousNext").show();
        
        $('#lblRecords').html('');
        $('#ulSonyResults').html('');
        $('#ulSonyResults').html(result.HTML);
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
    }
    else {
        ShowNotification(_msgErrorOccured);
       // ClearResultsOnError('ulTimeshiftResults', 'divPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "SearchResult()"));
    }



}
function OnFail(result) {
    ShowNotification(_msgErrorOccured);
    //ClearResultsOnError('ulTimeshiftResults', 'divPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "SearchResult()"));
}

function ValidateSearch() {
    var isValidate = true;
    if (_Station == null || _Station == "") {
        isValidate = false;
        ShowNotification(_msgStationRequired);
        
    }

    if (_Date == null || _Date=="") {
        isValidate = false;
        ShowNotification(_msgDateRequired);
    }

    return isValidate;
}

function SetActiveFilter() {

    var isFilterEnable = false;
    $("#divActiveFilter").html("");


    if (_Station != null && _Station != "") {
        $('#divActiveFilter').append('<div id="divStationActiveFilter" class="filter-in">' + _Station + '</div>');
        isFilterEnable = true;
    }

    if ((_Date != null && _Date != "")) {
        $('#divActiveFilter').append('<div id="divDateActiveFilter" class="filter-in">' + _Date + '</div>');
        isFilterEnable = true;
    }
    

    if (isFilterEnable) {
        $("#divActiveFilter").removeClass('divInActiveFilter');
        $("#divActiveFilter").addClass('divActiveFilter');
    }
    else {
        $("#divActiveFilter").removeClass('divActiveFilter');
        $("#divActiveFilter").addClass('divInActiveFilter');
    }
}


function LoadChartNPlayer(rawMediaGuid, iqcckey) {
    LoadPlayerbyGuidTS(rawMediaGuid);
    var jsonPostData = {
        IQ_CC_KEY: iqcckey
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlSonyMediaGetChart,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),
        success: OnGetChartComplete,
        error: OnGetChartFail
    });

    
}

function OnGetChartComplete(result) {
    if (result.isSuccess) {
        ShowHidePanel(true);
        changeTab('2');
        RenderHighCharts(result.lineChartJson);
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

function RenderHighCharts(jsonLineChartData) {

    var JsonLineChart = JSON.parse(jsonLineChartData);
    JsonLineChart.plotOptions.series.point.events.click = LineChartClick;
    JsonLineChart.xAxis.labels.formatter = FormatTime
    //JsonLineChart.tooltip.positioner = TooltipSetY
    JsonLineChart.tooltip.formatter = tooltipFormat
    JsonLineChart.plotOptions.series.events.show = HandleSeriesShowHide;
    JsonLineChart.plotOptions.series.events.hide = HandleSeriesShowHide;
    JsonLineChart.plotOptions.series.point.events.mouseOver = ChartHoverManage;
    JsonLineChart.plotOptions.series.point.events.mouseOut = ChartHoverOutManage;
    JsonLineChart.chart.zoomType = null;
    $('#divKantorChart').highcharts(JsonLineChart);


    JsonLineChart.legend.x = -400;
    JsonLineChart.xAxis.tickInterval = parseInt(Math.floor(JsonLineChart.xAxis.categories.length / 12)),
    JsonLineChart.chart.zoomType = 'x';
    JsonLineChart.chart.width = null;
    $('#divKantorChart2').highcharts(JsonLineChart);
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

function HandleSeriesHide() {
    var seriesIndex = this.index;
    var columnSeries = $('#divColumnChart').highcharts().series[this.index];
    if (columnSeries.visible) {
        columnSeries.hide();
    } else {
        columnSeries.show();
    }
}


function TooltipSetY(boxWidth, boxHeight, point) {
    return {
        x: point.plotX,
        y: 0
    };
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
        var seriesName = '';
        
        if (point.series.index == 0) {
            seriesName = 'Kantar Ratings';
        }
        else {
            seriesName = 'Kantar Audience';
        }

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
    Seek(this.category);
}


function ShowHidePanel(isShow) {

    if (typeof (isShow) !== 'undefined' && isShow == true) {
        $('#divChartNPlayerContent').css({ visibility: 'visible' }).animate({ opacity: 1.0 }, 1000);
        $('#divChartNPlayerContent').css('height', 'auto');
        $("#imgShowHide").attr('src', '../images/hiden.png')
    }
    else if ($('#divChartNPlayerContent').css('visibility') == 'visible') {
        $('#divChartNPlayerContent').css({ visibility: 'hidden' }).animate({ opacity: 0 }, 1000);
        $('#divChartNPlayerContent').css('height', '0px');
        $("#imgShowHide").attr('src', '../images/show.png')
    }
    else {
        $('#divChartNPlayerContent').css({ visibility: 'visible' }).animate({ opacity: 1.0 }, 1000);
        $('#divChartNPlayerContent').css('height', 'auto');
        $("#imgShowHide").attr('src', '../images/hiden.png')
    }

    if (!$('#divChartNPlayer').is(':visible')) {
        $('#divChartNPlayer').show();
    }
}

function changeTab(tabNumber) {
    if (tabNumber == '1') {
        $('#divKantorChart2').css({ opacity: 0 })
        $('#divKantorChart2').css({ height: "0" })

        $('#headerScrollChart').addClass('pieChartActive');
        $('#headerScrollChartParent').addClass('ActiveParent');
        
        $('#headerZoomChart').removeClass('pieChartActive');
        $('#headerZoomChartParent').removeClass('ActiveParent');

        $('#divChartParent').css({ height: "auto", opacity: 1 })
    }
    else {
        $('#divChartParent').css({ opacity: 0 })
        $('#divChartParent').css({ height: "0" })

        $('#headerZoomChart').addClass('pieChartActive');
        $('#headerZoomChartParent').addClass('ActiveParent');

        $('#headerScrollChart').removeClass('pieChartActive');
        $('#headerScrollChartParent').removeClass('ActiveParent');

        $('#divKantorChart2').css({ height: "auto", opacity: 1 })
        

    }

}