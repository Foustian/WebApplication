// Add-on file to VideoPlayer.js. Separates out the functionality for building the MarTek chart.
// Eventually, the Kantor chart functionality should be moved in here as well.

var _searchTermHits = [];
var numOfVisibleLogos;
var yAxisInfoList;
var tAdsResultsJson;

function SetSearchTermHits(result) {
    if (result.SearchTermHits != null) {
        _searchTermHits = result.SearchTermHits
    }
}

function SetPlayerChartContent(result, type) {
    if (result.hasTAdsResults && result.tAdsResultsJson.length > 0) {
        $('.ads-chart-content').html('');

        numOfVisibleLogos = result.yAxisCompanies.length;
        yAxisInfoList = [];
        $.each(result.yAxisCompanies, function (index, value) {
            var yAxisInfo = {};
            yAxisInfo.ID = [];
            yAxisInfo.Position = [];
            yAxisInfo.ID.push(index + 1);
            yAxisInfo.Position.push(index + 1);
            yAxisInfo.yAxisCompany = result.yAxisCompanies[index];

            yAxisInfoList.push(yAxisInfo);
        });
        tAdsResultsJson = result.tAdsResultsJson;

        var paddingLeft = type == "Feeds" ? 138 : 163;
        var height = (numOfVisibleLogos * 30) + 90;
        var chartWidth = $(".video-player-row-one").width() - 163;

        $('.ads-chart-content').append('<div class="float-left" id="ads-results-container" style="width:' + chartWidth + 'px; padding-left:' + paddingLeft + 'px; background-color: #FFFFFF;"></div>');
        $("#ads-results-container").append('<div id="ads-results" style="width:100%; height:' + height + 'px; margin:0 auto;"></div>');

        RenderHighCharts(result.tAdsResultsJson, 'ads-results');
    }
    else {
        $('.ads-chart-content').append('<table style="width:100%; background-color:#c0c0c0"><tr><td>There is no available data.</td></tr></table>');
    }
}

function HandleSeriesLogoShowHide() {
    if (!_IsManualHover) {
        var hidePos = this._i + 1;

        $.each(yAxisInfoList, function (index, value) {
            if (value.ID == hidePos) {
                if (value.Position == 0) {
                    yAxisInfoList[index].Position = hidePos;
                }
                else {
                    yAxisInfoList[index].Position = 0;
                }
            }
        });

        this.chart.ignoreHiddenSeries = true;
        this.chart.redraw();
    }
}

function FormatLRTooltip() {
    var totalSeconds = this.x;
    var minutes = Math.floor(totalSeconds / 60);
    var seconds = totalSeconds - minutes * 60;
    minutes = minutes < 10 ? '0' + minutes : minutes;
    seconds = seconds < 10 ? '0' + seconds : seconds;

    var str = '';
    if (this.point.SearchName != null && this.point.SearchName != "") {
        if (this.point.Medium != null && this.point.Medium.trim() != "") {
            str += '<div style="min-height: 100px; min-width:100px;"><p><b>' + this.point.SearchName + '</b> - ' + minutes + ':' + seconds + '</p><br/><div><img src="' + this.point.Medium + '" style="display: block; margin: auto; vertical-align: middle; height: auto; width:auto;"/></div></div>';
        }
        else {
            str += '<div style="min-height: 20px; min-width:100px;"><p><b>' + this.point.SearchName + '</b> - ' + minutes + ':' + seconds + '</p></div>';
        }
    }
    else {
        str += '<div><p><b>' + this.point.SearchName + '</b> - ' + minutes + ':' + seconds + '</p></div>';
    }
    return str;
}

function FormatLRBrandLabel() {
    var label = "";
    if (this.value > 0 && this.value === parseInt(this.value, 10) && this.value <= numOfVisibleLogos) {
        var yAxisInfo = yAxisInfoList[this.value - 1];
        if (yAxisInfo.Position != 0) {
            var company = yAxisInfo.yAxisCompany;
            var tooltip = company;
            var display = "";

            if (company.trim().substring(0, 9) == "IQ_CC_Key") {
                display = "Ads";
            }
            else if (company.trim().length > 30) {
                display = company.trim().substring(0, 30) + "...";
            }
            else {
                display = company;
            }

            label = '<div title="' + tooltip + '" style="width:200px; height:15px; font-size:12px; float:left;"><span style="font-weight:600; float:right; text-align:right; height:100%; width:100%;">' + display + '</span></div>';
        }
        else {
            label = '<div style="width:100px; height:15px;"></div>'

        }
    }
    return label;
}

function LineChartClickWithOffset() {
    var offset = 0;
    if (this.category > 3) offset = 3;

    setSeekPoint(this.category - offset);
}