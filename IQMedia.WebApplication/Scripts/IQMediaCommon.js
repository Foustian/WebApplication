// Global Variable to give height of current window. This variable is used in all the othe js files to calculate height
var documentHeight = $(window).height();
var ajaxRequestCount = 0;
var _KeyValues = '';

function removeActiveFilter(span) {
    //$(span).parent("div").remove();
}

$(document).ready(function () {
    if (!String.prototype.trim) {
        (function () {
            // Make sure we trim BOM and NBSP
            var rtrim = /^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g;
            String.prototype.trim = function () {
                return this.replace(rtrim, "");
            }
        })();
    }

    $('div.modalPopupDiv').on('shown', function () {
        var modalid = $(this).attr("id")

        $("#" + modalid + " input").keypress(function (event) {
            if (modalid != 'divIQAgentSetupIQNotificationPopup') {
                if (event.keyCode == 13) {
                    $("#" + modalid + " .button").click();
                    event.preventDefault();
                }
            }
        });

        $("#" + modalid + " select").keypress(function (event) {
            if (modalid != 'divIQAgentSetupIQNotificationPopup') {
                if (event.keyCode == 13) {
                    $("#" + modalid + " .button").click();
                }
            }
        });

        $("#" + modalid + " .button").click(function () {
            var button = this;
            setTimeout(function () {
                if (ajaxRequestCount > 0) {
                    if (modalid != 'divIQAgentSetupIQNotificationPopup' && modalid != 'divConfirmationPopup' && modalid != 'divPlayLogNSummaryPopup' && modalid != 'divGallaryPopup') {
                        $(button).attr("disabled", true);
                    }
                }
                else {
                    $(button).removeAttr("disabled");
                }
            }, 100)
        });




    });

    $('div.modalPopupDiv').on('hidden', function () {
        var modalid = $(this).attr("id")
        $("#" + modalid + " .button").removeAttr("disabled");
        $("#" + modalid + " input").unbind("mouseup");
        $("#" + modalid + " input").unbind("keypress");
    });

    $('#divMessage').html('');

    $(document).ajaxStart(function () {
        ShowLoading();
        ajaxRequestCount = ajaxRequestCount + 1;
    });

    $(document).ajaxComplete(function () {
        HideLoading();
        if (ajaxRequestCount > 0) {
            ajaxRequestCount = ajaxRequestCount - 1;
        }
    });

    $(document).ajaxError(function () {
        HideLoading();
        if (ajaxRequestCount > 0) {
            ajaxRequestCount = ajaxRequestCount - 1;
        }
    });

    $(document).ajaxSuccess(function (event, xhr, settings) {
        try {
            if (xhr.responseText != "") {
                var obj = $.parseJSON(xhr.responseText);
                if (obj != null && obj.isSuccess != undefined && obj.isAuthorized != undefined) {
                    if (obj.isSuccess == false && obj.isAuthorized == false) {

                        if (window.self === window.top) {
                            window.location = obj.redirectURL;
                        }
                        else {
                            window.top.location.href = obj.redirectURL;
                        }

                    }
                }
            }
        }
        catch (ex) { var p = ex; }

    });

    $('.ulCalenderFilter').click(function (e) {
        e.stopPropagation();
    });

    $("body").click(function (e) {
        $("#rmenu .submenu").hide();
    });


    $("#rmenu li").click(function (e) {
        e.preventDefault();
        e.stopPropagation();
        $("#rmenu li").removeClass("active");

        if ($(this).children("div").is(":visible")) {

            $(this).children("div").hide();
        } else {
            $("#rmenu .submenu").hide();
            $(this).children("div").show();
        }

        $(this).addClass("active");
    });

    $('#ulSetupChild').hover(
    function () {

        $('.nav .setup').css({ 'background': '#111 url(../images/setup-icon.png) no-repeat center -46px' });
    },
    function () {

        $('.nav .setup').css({ 'background': '' });
    }
    );

    if (typeof ($('input[placeholder], textarea[placeholder]').placeholder) != undefined) {
        $('input[placeholder], textarea[placeholder]').placeholder();
    }

});


function ShowModal(pnlid) {
    $('[id$="_' + pnlid + '"]').modal({
        backdrop: 'static',
        keyboard: true,
        dynamic: true
    });
}
function closeModal(pnlid) {
    $('#' + pnlid).css({ "display": "none" });
    $('#' + pnlid).modal('hide');
}

function CheckForSessionExpired(result) {
    if (result != null) {
        if (result.isSuccess == false && result.isAuthorized == false) {
            RedirectToUrl(result.redirectURL);
        }
    }
}

function ShowNotification(message, req) {

    if (typeof (req) != 'undefined' && (req.readyState == 0 || req.status == 0)) {
    }
    else {

        var notificationHTML = '<div id="divNotification" class="notificationdiv"><span>' + message + '</span></div>';

        if ($('#divNotification')[0]) {

        }
        else {
            $(document.body).append(notificationHTML);
            $('#divNotification').fadeIn(500).delay(2000).hide(1000);

            setTimeout(function () {
                $('#divNotification').remove();
            }, 3000);
        }
    }

}

function validateGrid(divID) {
    var isValid = false;
    $('#' + divID + ' input[type=checkbox]').each(function () {
        if (this.checked) {
            isValid = true;
        }
    });

    return isValid;
}

function setNoMoreResult() {
    $('#divMoreResults').html('No Results Found');
    $('#divMoreResults').removeAttr('onclick');
}

function ShowLoading() {
    var loadingHTML = '<div id="divLoading" class="loadingdiv"><span>Loading<img src=\'../images/1.gif\' /></span></div>';

    $(document.body).append(loadingHTML);
    $('#divLoading').fadeIn(500);


}

function HideLoading() {
    //setTimeout(function () {
    $('#divLoading').remove();
    // }, 1000);
}

function getConfirm(headertitle, message, okbuttontext, cancelbuttontext, confirmcallback) {

    $("#divConfirmationPopup .divconfirmationpop-content").html(message);

    if (okbuttontext != "") {
        $("#divConfirmationPopup button[button-type=ok]").html(okbuttontext);

        // After the OK button is clicked, it is automatically disabled the next time the popup is shown
        $("#divConfirmationPopup button[button-type=ok]").removeAttr('disabled', 'disabled');
    }
    if (cancelbuttontext != "") {
        $("#divConfirmationPopup button[button-type=cancel]").html(cancelbuttontext);
    }
    if (headertitle != "") {
        $("#divConfirmationPopup .modalpopheadertitle").html(headertitle);
    }
    $("#divConfirmationPopup").modal({ show: true,
        backdrop: true,
        keyboard: false,
        dynamic: true
    });

    $("#divConfirmationPopup button[button-type=ok]").click(function () {
        $("#divConfirmationPopup").modal("hide");
        if (confirmcallback != null) {
            confirmcallback(true);
            confirmcallback = null;
        }
    });
    $("#divConfirmationPopup button[button-type=cancel]").click(function () {
        $("#divConfirmationPopup").modal("hide");
        if (confirmcallback != null) {
            confirmcallback(false);
            confirmcallback = null;
        }
    });
    $("#divConfirmationPopup .popup-top-close").click(function () {
        $("#divConfirmationPopup").modal("hide");
    });
}

function DoLogOut() {
    window.location = "/LogOut/LogOut/";
}

function RedirectToUrl(url) {
    window.location.href = url;
}

function CheckEmailAddress(email) {
    //var emailPattern = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
    var emailPattern = /^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
    return emailPattern.test(email);
}

function CheckEmailAddressWithAmpersand(email) {
    //var emailPattern = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
    var emailPattern = /^([a-zA-Z0-9-_.\&])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
    return emailPattern.test(email);
}

function CheckForAuthentication(result, notificationMessage) {
    if (typeof result.isAuthorized != 'undefined' && !result.isAuthorized) {
        RedirectToUrl(result.redirectURL);
    }
    else {
        ShowNotification(notificationMessage);
    }
}
function RenderLineChart(jsonLineChartData, divLineChartID, ChartID) {

    //jsonLineChartData = "{\"chart\": {\"caption\": \"Daily Visits\",\"subcaption\": \"(from 8/6/2006 to 8/12/2006)\",\"linethickness\": \"1\",\"showvalues\": \"0\",\"formatnumberscale\": \"0\",\"anchorradius\": \"2\",\"divlinealpha\": \"20\",\"divlinecolor\": \"CC3300\",\"divlineisdashed\": \"1\",\"showalternatehgridcolor\": \"1\",\"alternatehgridcolor\": \"CC3300\",\"shadowalpha\": \"40\",        \"labelstep\": \"2\",\"numvdivlines\": \"5\",\"chartrightmargin\": \"35\",\"bgcolor\": \"FFFFFF,CC3300\",\"bgangle\": \"270\",\"bgalpha\": \"10,10\",\"alternatehgridalpha\": \"5\",\"legendposition\": \"RIGHT \"},\"categories\": [        {\"category\": [{\"label\": \"8/6/2006\"},{\"label\": \"8/7/2006\"},{\"label\": \"8/8/2006\"},{\"label\": \"8/9/2006\"},{\"label\": \"8/10/2006\"},{\"label\": \"8/11/2006\"},{\"label\": \"8/12/2006\"}]}],\"dataset\": [{\"seriesname\": \"Offline Marketing\",\"color\": \"1D8BD1\",\"anchorbordercolor\": \"1D8BD1\",\"anchorbgcolor\": \"1D8BD1\",\"data\": [{\"value\": \"1327\"},{\"value\": \"1826\"},{\"value\": \"1699\"},{\"value\": \"1511\"},{\"value\": \"1904\"},{\"value\": \"1957\"},{\"value\": \"1296\"}]},{\"seriesname\": \"Search\",\"color\": \"F1683C\",\"anchorbordercolor\": \"F1683C\",\"anchorbgcolor\": \"F1683C\",\"data\": [{\"value\": \"2042\"},{\"value\": \"3210\"},{\"value\": \"2994\"},{\"value\": \"3115\"},{\"value\": \"2844\"},{\"value\": \"3576\"},{\"value\": \"1862\"}]},{\"seriesname\": \"Paid Search\",\"color\": \"2AD62A\",\"anchorbordercolor\": \"2AD62A\",\"anchorbgcolor\": \"2AD62A\",\"data\": [{\"value\": \"850\"},{\"value\": \"1010\"},{\"value\": \"1116\"},{\"value\": \"1234\"},{\"value\": \"1210\"},{\"value\": \"1054\"},{\"value\": \"802\"}]},{\"seriesname\": \"From Mail\",\"color\": \"DBDC25\",\"anchorbordercolor\": \"DBDC25\",\"anchorbgcolor\": \"DBDC25\",\"data\": [{\"value\": \"541\"},{\"value\": \"781\"},{\"value\": \"920\"},{\"value\": \"754\"},{\"value\": \"840\"},{\"value\": \"893\"},{\"value\": \"451\"}]}],\"styles\": {\"definition\": [{\"name\": \"CaptionFont\",\"type\": \"font\",\"size\": \"12\"}],\"application\": [{\"toobject\": \"CAPTION\",\"styles\": \"CaptionFont\"},{\"toobject\": \"SUBCAPTION\",\"styles\": \"CaptionFont\"}]}}";
    //jsonLineChartData = "<chart caption='Business Results 2005 v 2006' xAxisName='Month' yAxisName='Revenue' showValues='0' numberPrefix='$'>   <categories>      <category label='Jan' />      <category label='Feb' />      <category label='Mar' />      <category label='Apr' />      <category label='May' />      <category label='Jun' />      <category label='Jul' />      <category label='Aug' />      <category label='Sep' />      <category label='Oct' />      <category label='Nov' />      <category label='Dec' />   </categories>   <dataset seriesName='2006'>      <set value='27400' />      <set value='29800'/>      <set value='25800' />      <set value='26800' />      <set value='29600' />      <set value='32600' />      <set value='31800' />      <set value='36700' />      <set value='29700' />      <set value='31900' />      <set value='34800' />      <set value='24800' />   </dataset>   <dataset seriesName='2005'>      <set value='10000'/>      <set value='11500'/>      <set value='12500'/>      <set value='15000'/>      <set value='11000' />      <set value='9800' />      <set value='11800' />      <set value='19700' />      <set value='21700' />      <set value='21900' />      <set value='22900' />      <set value='20800' />   </dataset>   <trendlines>      <line startValue='26000' color='91C728' displayValue='Target' showOnTop='1'/>   </trendlines>   <styles>      <definition>         <style name='CanvasAnim' type='animation' param='_xScale' start='0' duration='1' />      </definition>      <application>         <apply toObject='Canvas' styles='CanvasAnim' />      </application>      </styles></chart> ";
    FusionCharts.setCurrentRenderer('javascript');

    var myChart = new FusionCharts("MSLine", ChartID, "100%", "300");
    myChart.render(divLineChartID);
    myChart.setJSONData(jsonLineChartData);


}
function RenderSmallLineChart(jsonLineChartData, divLineChartID, ChartID) {

    //jsonLineChartData = "{\"chart\": {\"caption\": \"Daily Visits\",\"subcaption\": \"(from 8/6/2006 to 8/12/2006)\",\"linethickness\": \"1\",\"showvalues\": \"0\",\"formatnumberscale\": \"0\",\"anchorradius\": \"2\",\"divlinealpha\": \"20\",\"divlinecolor\": \"CC3300\",\"divlineisdashed\": \"1\",\"showalternatehgridcolor\": \"1\",\"alternatehgridcolor\": \"CC3300\",\"shadowalpha\": \"40\",        \"labelstep\": \"2\",\"numvdivlines\": \"5\",\"chartrightmargin\": \"35\",\"bgcolor\": \"FFFFFF,CC3300\",\"bgangle\": \"270\",\"bgalpha\": \"10,10\",\"alternatehgridalpha\": \"5\",\"legendposition\": \"RIGHT \"},\"categories\": [        {\"category\": [{\"label\": \"8/6/2006\"},{\"label\": \"8/7/2006\"},{\"label\": \"8/8/2006\"},{\"label\": \"8/9/2006\"},{\"label\": \"8/10/2006\"},{\"label\": \"8/11/2006\"},{\"label\": \"8/12/2006\"}]}],\"dataset\": [{\"seriesname\": \"Offline Marketing\",\"color\": \"1D8BD1\",\"anchorbordercolor\": \"1D8BD1\",\"anchorbgcolor\": \"1D8BD1\",\"data\": [{\"value\": \"1327\"},{\"value\": \"1826\"},{\"value\": \"1699\"},{\"value\": \"1511\"},{\"value\": \"1904\"},{\"value\": \"1957\"},{\"value\": \"1296\"}]},{\"seriesname\": \"Search\",\"color\": \"F1683C\",\"anchorbordercolor\": \"F1683C\",\"anchorbgcolor\": \"F1683C\",\"data\": [{\"value\": \"2042\"},{\"value\": \"3210\"},{\"value\": \"2994\"},{\"value\": \"3115\"},{\"value\": \"2844\"},{\"value\": \"3576\"},{\"value\": \"1862\"}]},{\"seriesname\": \"Paid Search\",\"color\": \"2AD62A\",\"anchorbordercolor\": \"2AD62A\",\"anchorbgcolor\": \"2AD62A\",\"data\": [{\"value\": \"850\"},{\"value\": \"1010\"},{\"value\": \"1116\"},{\"value\": \"1234\"},{\"value\": \"1210\"},{\"value\": \"1054\"},{\"value\": \"802\"}]},{\"seriesname\": \"From Mail\",\"color\": \"DBDC25\",\"anchorbordercolor\": \"DBDC25\",\"anchorbgcolor\": \"DBDC25\",\"data\": [{\"value\": \"541\"},{\"value\": \"781\"},{\"value\": \"920\"},{\"value\": \"754\"},{\"value\": \"840\"},{\"value\": \"893\"},{\"value\": \"451\"}]}],\"styles\": {\"definition\": [{\"name\": \"CaptionFont\",\"type\": \"font\",\"size\": \"12\"}],\"application\": [{\"toobject\": \"CAPTION\",\"styles\": \"CaptionFont\"},{\"toobject\": \"SUBCAPTION\",\"styles\": \"CaptionFont\"}]}}";
    //jsonLineChartData = "<chart caption='Business Results 2005 v 2006' xAxisName='Month' yAxisName='Revenue' showValues='0' numberPrefix='$'>   <categories>      <category label='Jan' />      <category label='Feb' />      <category label='Mar' />      <category label='Apr' />      <category label='May' />      <category label='Jun' />      <category label='Jul' />      <category label='Aug' />      <category label='Sep' />      <category label='Oct' />      <category label='Nov' />      <category label='Dec' />   </categories>   <dataset seriesName='2006'>      <set value='27400' />      <set value='29800'/>      <set value='25800' />      <set value='26800' />      <set value='29600' />      <set value='32600' />      <set value='31800' />      <set value='36700' />      <set value='29700' />      <set value='31900' />      <set value='34800' />      <set value='24800' />   </dataset>   <dataset seriesName='2005'>      <set value='10000'/>      <set value='11500'/>      <set value='12500'/>      <set value='15000'/>      <set value='11000' />      <set value='9800' />      <set value='11800' />      <set value='19700' />      <set value='21700' />      <set value='21900' />      <set value='22900' />      <set value='20800' />   </dataset>   <trendlines>      <line startValue='26000' color='91C728' displayValue='Target' showOnTop='1'/>   </trendlines>   <styles>      <definition>         <style name='CanvasAnim' type='animation' param='_xScale' start='0' duration='1' />      </definition>      <application>         <apply toObject='Canvas' styles='CanvasAnim' />      </application>      </styles></chart> ";
    FusionCharts.setCurrentRenderer('javascript');

    var myChart = new FusionCharts("MSLine", ChartID, "120", "100");
    myChart.render(divLineChartID);
    myChart.setJSONData(jsonLineChartData);


}


function RenderSmallLineChartHighCharts(jsonLineChartData, divLineChartID, ChartID) {

    var JsonLineChart = JSON.parse(jsonLineChartData);
    $('#' + divLineChartID).highcharts(JsonLineChart);


}




function RenderAreaChart(jsonLineChartData, divLineChartID, ChartID) {

    //jsonLineChartData = "{\"chart\": {\"caption\": \"Daily Visits\",\"subcaption\": \"(from 8/6/2006 to 8/12/2006)\",\"linethickness\": \"1\",\"showvalues\": \"0\",\"formatnumberscale\": \"0\",\"anchorradius\": \"2\",\"divlinealpha\": \"20\",\"divlinecolor\": \"CC3300\",\"divlineisdashed\": \"1\",\"showalternatehgridcolor\": \"1\",\"alternatehgridcolor\": \"CC3300\",\"shadowalpha\": \"40\",        \"labelstep\": \"2\",\"numvdivlines\": \"5\",\"chartrightmargin\": \"35\",\"bgcolor\": \"FFFFFF,CC3300\",\"bgangle\": \"270\",\"bgalpha\": \"10,10\",\"alternatehgridalpha\": \"5\",\"legendposition\": \"RIGHT \"},\"categories\": [        {\"category\": [{\"label\": \"8/6/2006\"},{\"label\": \"8/7/2006\"},{\"label\": \"8/8/2006\"},{\"label\": \"8/9/2006\"},{\"label\": \"8/10/2006\"},{\"label\": \"8/11/2006\"},{\"label\": \"8/12/2006\"}]}],\"dataset\": [{\"seriesname\": \"Offline Marketing\",\"color\": \"1D8BD1\",\"anchorbordercolor\": \"1D8BD1\",\"anchorbgcolor\": \"1D8BD1\",\"data\": [{\"value\": \"1327\"},{\"value\": \"1826\"},{\"value\": \"1699\"},{\"value\": \"1511\"},{\"value\": \"1904\"},{\"value\": \"1957\"},{\"value\": \"1296\"}]},{\"seriesname\": \"Search\",\"color\": \"F1683C\",\"anchorbordercolor\": \"F1683C\",\"anchorbgcolor\": \"F1683C\",\"data\": [{\"value\": \"2042\"},{\"value\": \"3210\"},{\"value\": \"2994\"},{\"value\": \"3115\"},{\"value\": \"2844\"},{\"value\": \"3576\"},{\"value\": \"1862\"}]},{\"seriesname\": \"Paid Search\",\"color\": \"2AD62A\",\"anchorbordercolor\": \"2AD62A\",\"anchorbgcolor\": \"2AD62A\",\"data\": [{\"value\": \"850\"},{\"value\": \"1010\"},{\"value\": \"1116\"},{\"value\": \"1234\"},{\"value\": \"1210\"},{\"value\": \"1054\"},{\"value\": \"802\"}]},{\"seriesname\": \"From Mail\",\"color\": \"DBDC25\",\"anchorbordercolor\": \"DBDC25\",\"anchorbgcolor\": \"DBDC25\",\"data\": [{\"value\": \"541\"},{\"value\": \"781\"},{\"value\": \"920\"},{\"value\": \"754\"},{\"value\": \"840\"},{\"value\": \"893\"},{\"value\": \"451\"}]}],\"styles\": {\"definition\": [{\"name\": \"CaptionFont\",\"type\": \"font\",\"size\": \"12\"}],\"application\": [{\"toobject\": \"CAPTION\",\"styles\": \"CaptionFont\"},{\"toobject\": \"SUBCAPTION\",\"styles\": \"CaptionFont\"}]}}";
    //jsonLineChartData = "<chart caption='Business Results 2005 v 2006' xAxisName='Month' yAxisName='Revenue' showValues='0' numberPrefix='$'>   <categories>      <category label='Jan' />      <category label='Feb' />      <category label='Mar' />      <category label='Apr' />      <category label='May' />      <category label='Jun' />      <category label='Jul' />      <category label='Aug' />      <category label='Sep' />      <category label='Oct' />      <category label='Nov' />      <category label='Dec' />   </categories>   <dataset seriesName='2006'>      <set value='27400' />      <set value='29800'/>      <set value='25800' />      <set value='26800' />      <set value='29600' />      <set value='32600' />      <set value='31800' />      <set value='36700' />      <set value='29700' />      <set value='31900' />      <set value='34800' />      <set value='24800' />   </dataset>   <dataset seriesName='2005'>      <set value='10000'/>      <set value='11500'/>      <set value='12500'/>      <set value='15000'/>      <set value='11000' />      <set value='9800' />      <set value='11800' />      <set value='19700' />      <set value='21700' />      <set value='21900' />      <set value='22900' />      <set value='20800' />   </dataset>   <trendlines>      <line startValue='26000' color='91C728' displayValue='Target' showOnTop='1'/>   </trendlines>   <styles>      <definition>         <style name='CanvasAnim' type='animation' param='_xScale' start='0' duration='1' />      </definition>      <application>         <apply toObject='Canvas' styles='CanvasAnim' />      </application>      </styles></chart> ";
    FusionCharts.setCurrentRenderer('javascript');

    var myChart = new FusionCharts("MSArea", ChartID, "100%", "300");
    myChart.render(divLineChartID);
    myChart.setJSONData(jsonLineChartData);


}

function RenderLineChartHighCharts(jsonLineChartData, divLineChartID, ChartID, onClickFunction) {

    var JsonLineChart = JSON.parse(jsonLineChartData);
    if (onClickFunction != null) {
        JsonLineChart.plotOptions.series.point.events.click = onClickFunction;
    }
    else {
        JsonLineChart.plotOptions.series.point.events.click = LineChartClick;
    }
    if (JsonLineChart.xAxis.labels.formatter != null) {
        JsonLineChart.xAxis.labels.formatter = GetMonth;
    }

    $('#' + divLineChartID).highcharts(JsonLineChart);
}

function ShowContactUs() {
    //ShowModal('divContactPage');
    //$("#divContactContent").css("height", documentHeight - 360);
    $('#divContactPage').modal({
        backdrop: 'static',
        keyboard: true,
        dynamic: true
    });

}

function HideContactUs() {
    closeModal('divContactPage');
}


function GetDateFormatFromJsonString(jsonString) {

    var currentTime = new Date(parseInt(jsonString.substr(6)));
    var month = currentTime.getMonth() + 1;
    if (month < 10) {
        month = '0' + month;
    }
    var day = currentTime.getDate();
    if (day < 10) {
        day = '0' + day;
    }
    var year = currentTime.getFullYear();
    var date = month + "/" + day + "/" + year;
    return date;

}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);

    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function ShowPopUpNotification(message) {
    var notificationHTML = '<div id="divPopUpNotification" class="popUpNotificationdiv"><span>' + message + '</span></div>';

    if ($('#divPopUpNotification')[0]) {

    }
    else {
        $(document.body).append(notificationHTML);
        $('#divPopUpNotification').fadeIn(500).delay(2000).hide(1000);

        setTimeout(function () {
            $('#divPopUpNotification').remove();
        }, 3000);
    }

}


function SetModalBodyScrollBarForPopUp() {

    //    if (screen.height >= 768) {
    //        $('.modalBody').css({ 'max-height': documentHeight - 150 });
    //    }
    //    else {
    //        $('.modalBody').css({ 'max-height': documentHeight - 70 });
    //    }

    //    $(".modalBody").mCustomScrollbar({
    //        advanced: {
    //            updateOnContentResize: true,
    //            autoScrollOnFocus: false
    //        }
    //    });
}

function ValidateUrl(str) {
    var pattern = /(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/;
    return pattern.test(str);
}

function ClearResultsOnError(divMainResultId, divHeaderId, divFooterId, divErrorMsg) {
    $("#" + divMainResultId).html(divErrorMsg);
    if (divHeaderId != '') {
        $("#" + divHeaderId).hide();
    }

    if (divFooterId != '') {
        $("#" + divFooterId).hide();
    }
}


function ValidateVersion(str) {
    var pattern = /\d+(\.\d+)*$/;
    return pattern.test(str);
}

function ValidateIPAddress(str) {
    var pattern = /^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$/;
    return pattern.test(str);
}

function ValidateDomainName(str) {
    var pattern = /^(([0-9A-Za-z\-])*\.)+([0-9A-Za-z]){2,3}$/;
    return pattern.test(str);
}

function TestDomain(inputval) {
    if (/(?!www.)^(?:[-A-Za-z0-9]+\.)+[A-Za-z]{2,6}$/i.test(inputval))
        return true;
    else
        return false;
}

function TestWildInput(inputval) {
    if (/^((\*)?([-A-Za-z0-9-_.])+(\*)?([-A-Za-z0-9-_.])*)*$/i.test(inputval)) {
        return true;
    }
    else {
        return false;
    }
}

function ValidateTime(str) {
    var pattern = /^([1-9]|0[1-9]|1[0-2]):([0-5]\d)\s?(AM|PM)?$/i;
    return pattern.test(str);
}

function ValidateZipCode(str) {
    var pattern = /^\d{5}$/;
    return pattern.test(str);
}

function CreateAndOpenAddCategoryPopup(callback) {

    if ($('#divCreateCustomCategoryPopup').length > 0) {
        CancelCustomCategoryAddPopup();
    }

    var categoryPopup =
    '<div class="modal fade hide resizable modalPopupDiv in" id="divCreateCustomCategoryPopup" style="display: block;" aria-hidden="false">'
        + '<div class="closemodalpopup">'
            + '<img onclick="CancelCustomCategoryAddPopup();" class="popup-top-close" alt="close" src="/images/close-icon.png" id="imgCloseEditPopup">'
        + '</div>'
        + '<div style="padding-top: 20px;" id="divCustomCategoryAddEditHTML">'
            + '<form class="form-horizontal">'
                + '<div style="padding-bottom: 5px; border-bottom: 1px solid #dadada; margin: 1px 15px 1px 15px;font-weight: bold;">'
                    + 'Create Category'
                + '</div>'
                + '<br>'
                + '<input type="hidden" value="0" id="hdCustomCategoryKey">'
                + '<div class="control-group">'
                    + '<label for="txtTitle" class="control-label">Category Name</label>'
                    + '<div class="controls">'
                        + '<input type="text" value="" style="width:500px;" placeholder="Category Name" name="txtCategoryName" id="txtCategoryName">'
                        + '<span style="color: #FF0000; display: none;" id="spanCategoryName" class="help-inline"></span>'
                    + '</div>'
                + '</div>'
                + '<div class="control-group">'
                    + '<label for="txtDescription" class="control-label">Category Description</label>'
                    + '<div class="controls">'
                        + '<textarea style="width:500px;" rows="4" placeholder="Category Description" name="txtCategoryDescription" id="txtCategoryDescription" cols="50"></textarea>'
                    + '</div>'
                + '</div>'
                + '<div class="control-group">'
                    + '<div class="controls">'
                        + '<button class="cancelButton" onclick="CancelCustomCategoryAddPopup();" id="btnCancel" type="button">Cancel</button> ';
    if (typeof (callback) != 'undefined') {
        categoryPopup += '<button class="button" onclick="CreateNewCustomCategory(' + callback.name + ',' + arguments[1] + ',' + arguments[2] + ');" id="btnUpdate" type="button">Create</button>';
    }
    else {
        categoryPopup += '<button class="button" onclick="CreateNewCustomCategory();" id="btnUpdate" type="button">Create</button>';
    }

    categoryPopup += '</div>'
                + '</div>'
            + '</form>'
        + '</div>'
    + '</div>';

    $(document.body).append(categoryPopup);

    $("#divCreateCustomCategoryPopup").modal({
        backdrop: 'static',
        keyboard: true,
        dynamic: true
    });

    $('#divCreateCustomCategoryPopup').on('hidden', function () {
        console.log('hideen called');
        setTimeout(function () {
            $('#divCreateCustomCategoryPopup').detach();
        }, 100);
    })
}


function CancelCustomCategoryAddPopup() {

    $("#divCreateCustomCategoryPopup").css({ "display": "none" });
    $("#divCreateCustomCategoryPopup").modal("hide");
}

function CreateNewCustomCategory(callback, arg1, arg2) {

    if (ValidateCreateNewCustomCategory()) {

        var jsonPostData =
        {
            p_CustomCategoryKey: $("#hdCustomCategoryKey").val(),
            p_CategoryName: $("#txtCategoryName").val(),
            p_CategoryDescription: $("#txtCategoryDescription").val()
        }

        $("#divCustomCategoryAddEditHTML button[type=button]").prop("disabled", "disabled");

        $.ajax({
            url: _urlSetupSaveCustomCategory,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: 'json',
            data: JSON.stringify(jsonPostData),
            success: function (result) {

                CheckForSessionExpired(result);

                $("#divCustomCategoryAddEditHTML button[type=button]").removeAttr("disabled");

                if (result != null && result.isSuccess) {

                    if (result.isDuplicateCategory) {
                        $("#spanCategoryName").html(_msgCategoryAlreadyExists).show();
                        $("#txtCategoryName").focus();
                    }
                    else {
                        CancelCustomCategoryAddPopup();
                        ShowNotification(_msgCategorySaved);
                        if (typeof (callback) != 'undefined') {
                            callback(arg1, arg2);
                        }
                    }
                }
            },
            error: function (a, b, c) {
                ShowNotification(_msgErrorOccured);
                $("#divCustomCategoryAddEditHTML button[type=button]").removeAttr("disabled");
            }
        });
    }
}

function ValidateCreateNewCustomCategory() {

    var flag = true;

    $("#spanCategoryName").html("");

    if ($.trim($("#txtCategoryName").val()) == "") {
        $("#spanCategoryName").html(_msgCatNameRequired).show();
        flag = false;
    }
    return flag;
}


function SetImageSrc() {
    $('img[csrc]').each(function (index, obj) {
        var imgSrc = $(this).attr('csrc');
        $(this).attr('src', imgSrc);
        $(this).removeAttr('csrc');
    });
}

function OpenDashboardPopup(source, isOnlyParents, sinceID) {
    $('#divDashboardPopup').modal({
        backdrop: 'static',
        keyboard: true,
        dynamic: true
    });

    $("#divDashboardPopup").resizable({
        handles: 's',
        iframeFix: true,
        start: function () {
            ifr = $('#iFrameDashboard');
            var d = $('<div></div>');

            $('#divDashboardPopup').append(d[0]);
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
            $("#iFrameDashboard").width(newWd).height(newHt);
        }
    }).draggable({
        iframeFix: true,
        start: function () {
            ifr = $('#iFrameDashboard');
            var d = $('<div></div>');

            $('#divDashboardPopup').append(d[0]);
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

    var queryParams = "?source=" + source;
    if (isOnlyParents != undefined && sinceID != undefined) {
        queryParams += "&isOnlyParents=" + isOnlyParents + "&sinceID=" + sinceID;
    }
    $('#iFrameDashboard').attr("src", "//" + window.location.hostname + "/Dashboard" + queryParams);

    $('#divDashboardPopup').css("position", "");
    $('#divDashboardPopup').css("height", documentHeight - 200);
    $('#iFrameDashboard').css("height", documentHeight - 200);
}

function CloseDashboardPopup() {
    $("#divDashboardPopup").css({ "display": "none" });
    $("#divDashboardPopup").modal("hide");
    $('#iFrameDashboard').attr("src", "");
}

function SetDashboardOverviewHTML(result, divChartDataID, onClickFunction, parentDivID) {
    if (parentDivID) {
        parentDivID = parentDivID + " #";
    }
    else {
        parentDivID = '';
    }

    $('#' + divChartDataID).html('');
    $('#' + divChartDataID).html(result.HTML);

    $('#' + parentDivID + 'divLineChartMedia').html('');
    RenderLineChartHighCharts(result.jsonMediaRecord, parentDivID + 'divLineChartMedia', 'MediaLineChart', onClickFunction);

    $('#' + parentDivID + 'divLineChartSubMedia').html('');
    RenderLineChartHighCharts(result.jsonSubMediaRecord, parentDivID + 'divLineChartSubMedia', 'SubMediaLineChart', onClickFunction);
    RenderPieChartHighChart(result.jsonPieChartSubMedia, result.totalHits);

    $.each(result.ReportMediumList, function (i, v) {

        v.IsprevSummaryEnoughData = result.IsprevSummaryEnoughData;

    });

    var sparkCharts = $("#tmplSparkChart").render(result.ReportMediumList);

    $("#ulOvSubMediaCharts li:first-child").before(sparkCharts);

    /*
    if (result.jsonTVRecord != null) {
        $('#divTVLineChart').html('');
        RenderSparkHighChart(result.jsonTVRecord, 'divTVLineChart', 'TVLineChart', onClickFunction);
    }
    else {
        $("#liTVChart").hide();
    }

    if (result.jsonNMRecord != null) {
        $('#divNMLineChart').html('');
        RenderSparkHighChart(result.jsonNMRecord, 'divNMLineChart', 'NMLineChart', onClickFunction);
    }
    else {
        $("#liNMChart").hide();
    }

    if (result.jsonTWRecord != null) {
        $('#divTWLineChart').html('');
        RenderSparkHighChart(result.jsonTWRecord, 'divTWLineChart', 'TWLineChart', onClickFunction);
    }
    else {
        $("#liTWChart").hide();
    }

    if (result.jsonForumRecord != null) {
        $('#divForumLineChart').html('');
        RenderSparkHighChart(result.jsonForumRecord, 'divForumLineChart', 'ForumLineChart', onClickFunction);
    }
    else {
        $("#liForumChart").hide();
    }

    if (result.jsonSocialMRecord != null) {
        $('#divSocialMediaLineChart').html('');
        RenderSparkHighChart(result.jsonSocialMRecord, 'divSocialMediaLineChart', 'SocialMediaLineChart', onClickFunction);
    }
    else {
        $("#liSocialMediaChart").hide();
    }

    if (result.jsonBlogRecord != null) {
        $('#divBlogLineChart').html('');
        RenderSparkHighChart(result.jsonBlogRecord, 'divBlogLineChart', 'BlogLineChart', onClickFunction);
    }
    else {
        $("#liBlogChart").hide();
    }
    */
    if (result.jsonAudienceRecord != null) {
        $('#divAudienceLineChart').html('');
        RenderSparkHighChart(result.jsonAudienceRecord, 'divAudienceSparkChart', 'AudienceSparkChart');
    }
    else {
        $("#liAudienceChart").hide();
    }

    if (result.jsonIQMediaValueRecords != null) {
        $('#divIQMediaValueLineChart').html('');
        RenderSparkHighChart(result.jsonIQMediaValueRecords, 'divIQMediaValueSparkChart', 'IQMediaValueSparkChart');
    }
    else {
        $("#liMediaValueChart").hide();
    }

    if (result.jsonPMRecord != null) {
        $('#divPMLineChart').html('');
        RenderSparkHighChart(result.jsonPMRecord, 'divPMLineChart', 'PMLineChart', onClickFunction);
    }
    else {
        $("#liPMChart").hide();
    }

    if (result.jsonTMRecord != null) {
        $('#divTMLineChart').html('');
        RenderSparkHighChart(result.jsonTMRecord, 'divTMLineChart', 'TMLineChart', onClickFunction);
    }
    else {
        $("#liRadioChart").hide();
    }

    if (result.jsonMSRecord != null) {
        $('#divMSLineChart').html('');
        RenderSparkHighChart(result.jsonMSRecord, 'divMSLineChart', 'MSLineChart', onClickFunction);
    }
    else {
        $("#liMSChart").hide();
    }
}

function SetDashboardMediumHTML(result, medium, parentDivID) {
    if (parentDivID) {
        parentDivID = parentDivID + " #";
    }
    else {
        parentDivID = '';
    }

    RenderLineChartHighCharts(result.noOfDocsJson, parentDivID + 'divLineChartMedia', 'DocChart');

    if (result.noOfHitsJson) {
        RenderSparkHighChart(result.noOfHitsJson, parentDivID + 'divNoOfHitsChart', 'HitsChart');
        $('#' + parentDivID + 'liNumberOfHits').show();
    }
    else {
        $('#' + parentDivID + 'liNumberOfHits').hide();
    }

    if (result.noOfViewJson) {
        RenderSparkHighChart(result.noOfViewJson, parentDivID + 'divNoOfViewsChart', 'ViewChart');
        $('#' + parentDivID + 'liViews').show();        
    }
    else {
        $('#' + parentDivID + 'liViews').hide();
    }

    if (result.sentimentChart) {
        RenderSmallLineChartHighCharts(result.sentimentChart, parentDivID + 'divSentimentChart', 'SentiMentChart');
        $('#' + parentDivID + 'divSentimentChart span').css('top', '7px');
    }
    else {
        $('#' + parentDivID + 'liSentiment').hide();
    }

    if (result.noOfMinOfAiringJson) {
        RenderSparkHighChart(result.noOfMinOfAiringJson, parentDivID + 'divMinOfAiringChart', 'AiringChart');
        $('#' + parentDivID + 'liMinutesOfAiring').show();
    }
    else {
        $('#' + parentDivID + 'liMinutesOfAiring').hide();
    }

    if (result.noOfAdJson) {
        RenderSparkHighChart(result.noOfAdJson, parentDivID + 'divAdChart', 'AdChart');
        $('#' + parentDivID + 'liAd').show();
    }
    else {
        $('#' + parentDivID + 'liAd').hide();
    }

    $("#" + parentDivID + "divTVDmaMain").hide();
    $("#" + parentDivID + "imgCountryList").hide();
    $("#" + parentDivID + "divCommonList").hide();

    if (result.DataLists) {

        $(result.DataLists).each(function (i, v) {

            switch (v.Item1) {
                case "TVDMA":
                    $("#" + parentDivID + "divTopDmas").html(v.Item2);
                    $("#" + parentDivID + "divTVDmaMain").show();
                    break;
                case "TVStation":
                    $("#" + parentDivID + "divTopStations").html(v.Item2);
                    $("#" + parentDivID + "divTVDmaMain").show();
                    break;
                case "TVCountry":
                    $("#" + parentDivID + "divTopCountries").html(v.Item2);
                    $("#" + parentDivID + "divTVDmaMain").show();
                    $("#" + parentDivID + "imgCountryList").show();
                    break;
                case "NMDMA":
                    $("#" + parentDivID + "divTopDmas").html(v.Item2);
                    $("#" + parentDivID + "divTVDmaMain").show();
                    break;
                case "Common":
                    $("#" + parentDivID + "divCommonList").append("<div>" + v.Item2 + "</div>");
                    $("#" + parentDivID + "divCommonList").show();
                    break;
            }

        });

    }

    if (result.dmaMapJson) {
        RenderDmaMapChart(result.dmaMapJson, parentDivID + 'divUsaMap', 'cUsaMap');
    }

    if (result.canadaMapJson) {
        RenderCanadaMapChart(result.canadaMapJson, parentDivID + 'divCanadaMap', 'cCanadaMap');
    }
        
    /*
    if (medium == 'TV') {
        
        $("#" + parentDivID + "marketHeader").html('Top Broadcast Markets');
        $("#" + parentDivID + "divTopDmas").html(result.p_TopDmasHTML);
        $("#" + parentDivID + "divTopStations").html(result.p_TopStationsHTML);
        $("#" + parentDivID + "divTopCountries").html(result.p_TopCountriesHTML);

        if (result.dmaMapJson) {
            RenderDmaMapChart(result.dmaMapJson, parentDivID + 'divUsaMap', 'cUsaMap');
        }

        if (result.canadaMapJson) {
            RenderCanadaMapChart(result.canadaMapJson, parentDivID + 'divCanadaMap', 'cCanadaMap');
        }
    }
    else if (medium == 'PM') {
        if (result.p_TopPrintPublicationsHTML) {
            $("#" + parentDivID + "divTopStations").html(result.p_TopPrintPublicationsHTML);
        }
        if (result.p_TopPrintAuthorsHTML) {
            $("#" + parentDivID + "divTopAuthors").html(result.p_TopPrintAuthorsHTML);
        }
    }
    else {
        if (medium == 'NM') {
            $("#" + parentDivID + "marketHeader").html('Top Online News Markets');
            $("#" + parentDivID + "divTopDmas").html(result.p_TopOnlineNewsDmasHTML)

            if (result.dmaMapJson) {
                RenderDmaMapChart(result.dmaMapJson, parentDivID + 'divUsaMap', 'cUsaMap');
            }

            if (result.canadaMapJson) {
                RenderCanadaMapChart(result.canadaMapJson, parentDivID + 'divCanadaMap', 'cCanadaMap');
            }
        }
        $("#" + parentDivID + "divTopStations").html(result.p_TopOnlineNewsSitesHTML)
    }
    */
    /*
    if (medium != 'TV' && medium != 'NM') {
        $("#" + parentDivID + "divTVDmaMain").hide();
    }
    else if (medium != 'TV') {
        $("#" + parentDivID + "imgCountryList").hide();
    }
    if (medium != 'PM') {
        $("#" + parentDivID + "divTopAuthors").hide();
    }
    */

    /*
    if (medium == 'TV' || medium == 'NM' || medium == 'Blog') {
        if (result.noOfAdJson) {
            RenderSparkHighChart(result.noOfAdJson, parentDivID + 'divAdChart', 'AdChart');
            $('#' + parentDivID + 'liAd').show();
        }
        else {
            $('#' + parentDivID + 'liAd').hide();
        }
    }
    else {
        $('#' + parentDivID + 'liAd').hide();
    }
    */
}

function RenderPieChartHighChart(jsonPieChartData, totalHits) {
    $('#divPieChartData').html('');
    var jsonPieChart = JSON.parse(jsonPieChartData);
    $("#divPieChartData").highcharts(jsonPieChart);

    _PieChartTotalHits = totalHits;

    $($("#divPieChartData").highcharts().series[0].data).each(function (i, e) {
        e.legendItem.on("click", function () {
            if (e.sliced) {
                _PieChartTotalHits = Number(_PieChartTotalHits) + Number(e.y);
            }
            else {
                _PieChartTotalHits = Number(_PieChartTotalHits) - Number(e.y);
            }
            $("#totalHits").remove();
            SetPieChartInnerText("divPieChartData", "divPieChartText", "totalHits", _PieChartTotalHits, 14);
            e.slice(!e.sliced);
        });
    });

    SetPieChartInnerText("divPieChartData", "divPieChartText", "totalHits", totalHits, 14);
}

function RenderSparkHighChart(jsonIQMediaValueRecords, divSparkChartID, ChartID, onClickFunction) {

    var JsonLineChart = JSON.parse(jsonIQMediaValueRecords);
    if (JsonLineChart.chart.events != undefined && JsonLineChart.chart.events.click != undefined) {
        if (onClickFunction != null) {
            JsonLineChart.chart.events.click = onClickFunction;
            JsonLineChart.plotOptions.spline.point.events.click = onClickFunction;
        }
        else {
            JsonLineChart.chart.events.click = ChangeMediumType;
            JsonLineChart.plotOptions.spline.point.events.click = ChangeMediumTypeOnPointClick;
        }
    }
    if (JsonLineChart.plotOptions.spline.events != null) {
        JsonLineChart.plotOptions.spline.events.mouseOver = HandleChartMouseHover;
        JsonLineChart.plotOptions.spline.events.mouseOut = HandleChartMouseOut;
    }

    $('#' + divSparkChartID).highcharts(JsonLineChart);
}

function SetPieChartInnerText(divPieChartID, divPieChartTextID, textID, totalHits, fontSizeOverride) {
    if (totalHits > 0) {
        var chart = $('#' + divPieChartID).highcharts();
        var fontSize = 13;
        if (fontSizeOverride) {
            fontSize = fontSizeOverride;
        }

        var spanHTML = '<span id="pieChartInfoText">';
        spanHTML += '<span style="font-size: ' + fontSize + 'px; font-weight:bold; font-family:Arial; color:#999999;">' + FormatNumericSuffix(totalHits) + ' Hits</span><br>';
        spanHTML += '</span>';

        $('#' + divPieChartTextID).append(spanHTML);
        var span = $('#pieChartInfoText');

        chart.renderer.text(spanHTML, chart.plotLeft + (chart.plotWidth / 2) - (span.width() / 2), chart.plotTop + (chart.plotHeight / 2) + 3).css({
            width: chart.plotWidth,
            height: chart.plotHeight
        }).attr({
            zIndex: 999,
            id: textID
        }).add();

        $('#' + divPieChartTextID).html('');
    }
}

function FormatNumericSuffix(num) {
    var result;
    var suffix;

    if (num >= 1000000000) {
        suffix = 'B';
        result = (num / 1000000000).toString();
    }
    else if (num >= 1000000) {
        suffix = 'M';
        result = (num / 1000000).toString();
    }
    else if (num >= 1000) {
        suffix = 'K';
        result = (num / 1000).toString();
    }
    else {
        return num;
    }

    result = result.substring(0, result.indexOf(".") + 2);
    return result.replace(/\.0$/, '') + suffix;
}

var ValidatePassword = function (pwd) {
    if (/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,30}$/.test(pwd.val().trim())) {
        return true;
    }
    else {
        return false;
    }
}

function EscapeHTML(input) {
    var entityMap = {
        "&": "&amp;",
        "<": "&lt;",
        ">": "&gt;",
        '"': '&quot;',
        "'": '&#39;',
        "/": '&#x2F;'
    };

    if (input != null) {
        return String(input).replace(/[&<>"'\/]/g, function (s) {
            return entityMap[s];
        });
    }
    else {
        return input;
    }
}

function ChangeMediumType() {
    GetDataMediumWise(this.options.series[0].data[0].Value, this.options.series[0].data[0].SearchTerm);
}

function ChangeMediumTypeOnPointClick() {
    GetDataMediumWise(this.options.Value, this.options.SearchTerm);
}