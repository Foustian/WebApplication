function ChangeTab(tabindex) {
    $('#divPlayerHeader div').each(function () {
        $(this).removeAttr("class");
    });

    $('#divPlayer > div').each(function () {
        $(this).hide();
    });

    $('#divPlayerHeader').children().eq(tabindex).attr("class", "pieChartActive");
    $('#divPlayer').children().eq(tabindex).show();
}

function LoadPlayer(iqagentID) {
    var jsonPostData = {
        iqagentTVResultID: iqagentID
    }

    $.ajax({

        type: 'POST',
        dataType: 'json',
        url: _urlFeedsLoadPlayer,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),

        success: OnLoadPlayerComplete,
        error: OnFail
    });



}
function LoadPlayerbyGuid(itemGuid,title120) {
    
    var _Title120 =null;
    if (typeof (title120) === 'undefined') {
        _Title120 = null;
    }
    else{
        _Title120 = title120;
    }

    var jsonPostData = {
        p_ItemGuid: itemGuid,
        p_SearchTerm: _SearchTerm,
        p_Title120 : _Title120
    }

    $.ajax({

        type: 'POST',
        dataType: 'json',
        url: _urlCommonLoadPlayerByGuidnSearchTerm,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),

        success: OnLoadPlayerComplete,
        error: OnFail
    });
}


function LoadPlayerbyGuidnSearchTerm(itemGuid, searchTerm) {
    
    var _Title120 =null;
    /*if (typeof (title120) === 'undefined') {
        _Title120 = null;
    }
    else{
        _Title120 = title120;
    }*/
    
    var jsonPostData = {
        p_ItemGuid: itemGuid,
        p_SearchTerm: searchTerm,
        p_Title120: _Title120
    }

    $.ajax({

        type: 'POST',
        dataType: 'json',
        url: _urlCommonLoadPlayerByGuidnSearchTerm,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),

        success: OnLoadPlayerComplete,
        error: OnFail
    });
}



function OnLoadPlayerComplete(result) {
    var hideClass = '';
    //result.hasCaptionString = false;

    if (!result.hasCaptionString) {
        hideClass = 'displayNone';
    }

    if (result != null && result.isSuccess) {
        var rawPlayer = '<div id="diviframe" runat="server" class="modal fade hide resizable modalPopupDiv">'
                        + '<div class="closemodalpopup"><img id="img2" src="../Images/close-icon.png" class="popup-top-close" onclick="ClosePlayer(\'diviframe\');" /></div>'
                        + '<div id="divIFrameMain" class="modalPopupMediaDiv"><div id="divCapMain" class="CaptionMain ' + hideClass + '"  style="height:auto;">'
                        + '<div class="margintop5 discoveryTabParent" id="divPlayerHeader">'
                        + '<div align="center" onclick="ChangeTab(0)" class="pieChartActive" style="float: left; padding: 5px;cursor:pointer;min-width:50px;" id="divPlayerHighlightTab">Highlights</div>'
                        + '<div align="center" onclick="ChangeTab(1)" style="float: left; padding: 5px;cursor:pointer;min-width:50px;" id="divPlayerCaptionTab">Captions</div>'
                        + '</div>'
                        + '<div id="divPlayerContent" class="clear"><div class="clear" id="divPlayer"><div id="divPlayer_HighlightContent">'
                    + '<div id="DivHighlight" style="height: 293px;overflow:auto;"></div>'
                    + '</div><div id="divPlayer_CaptionContent" style="display:none;"><div id="DivCaption" class="padding5" style="height: 293px;overflow:auto;"></div></div></div></div>'
                    + ' </div>'
                    + '<div id="divShowCaption" class="divShowCaption" onclick="RegisterCCCallback();">'
                    + '<img src="../../../Images/right_arrow_cc.gif" id="imgCCDirection" alt="Show CC" /></div>'
                    + '<div class="modalPopupPlayer" id="divRawMedia"></div>'
                    + '<div id="time" class="clear" style="margin-left: 38.5%;">'
                    + '<div id="timeBar" style="display: block; padding: 0;">'
                    + '<img id="time0" class="hourSeek" style="cursor: pointer" src="../images/time00.GIF" onclick="setSeekPoint(0, this);" />'
                    + '<img id="time10" class="hourSeek" style="opacity: 0.4; cursor: pointer" src="../images/time10.GIF" onclick="setSeekPoint(600, this);" />'
                    + '<img id="time20" class="hourSeek" style="opacity: 0.4; cursor: pointer" src="../images/time20.GIF" onclick="setSeekPoint(1200, this);" />'
                    + '<img id="time30" class="hourSeek" style="opacity: 0.4; cursor: pointer" src="../images/time30.GIF" onclick="setSeekPoint(1800, this);" />'
                    + '<img id="time40" class="hourSeek" style="opacity: 0.4; cursor: pointer" src="../images/time40.GIF" onclick="setSeekPoint(2400, this);" />'
                    + '<img id="time50" class="hourSeek" style="opacity: 0.4; cursor: pointer" src="../images/time50.GIF" onclick="setSeekPoint(3000, this);" /></div></div></div></div>'



        $(document.body).append(rawPlayer);
        if (!result.hasCaptionString) {
            $('#divIFrameMain').css({ 'width': 565 });
            $('#diviframe').css({ 'width': 585 });
            $('#imgCCDirection').attr('src', '../../images/left_arrow_cc.gif');
            var divmarleft = parseInt(($(window).width() / 2) - (600 / 2)).toString() + 'px';
            $('#time').attr('style', 'margin-left:3.7%;')

            $('#diviframe').animate({
                left: divmarleft,
                position: "fixed"
            });
        }
        else {
            $('#divIFrameMain').css({ 'width': 885 });
            $('#diviframe').css({ 'width': 900 });
            $('#imgCCDirection').attr('src', '../../images/right_arrow_cc.gif');
            $('#time').attr('style', 'margin-left:38.5%;')
            var divmarleft = parseInt(($(window).width() / 2) - (920 / 2)).toString() + 'px';
            $('#diviframe').animate({
                left: divmarleft,
                position: "fixed"
            });
        }
        $('#divRawMedia').html(result.rawMediaObjectHTML);
        $('#DivHighlight').html(result.HighlightHTML);
        $('#DivCaption').html(result.CaptionHTML);

        if (result.HighlightHTML != null && $.trim(result.HighlightHTML) != "") {
            ChangeTab(0);
        }
        else if (result.CaptionHTML != null && $.trim(result.CaptionHTML) != "") {
            ChangeTab(1);
        }

        $("#DivHighlight").mCustomScrollbar({
            advanced: {
                updateOnContentResize: true,
                autoScrollOnFocus: false
            }
        });

        $("#DivCaption").mCustomScrollbar({
            advanced: {
                updateOnContentResize: true,
                autoScrollOnFocus: false
            }
        });

        //RegisterCCCallback();
        $('#diviframe').modal({
            backdrop: 'static',
            keyboard: true,
            dynamic: true
        });
    }
    else {
        if (typeof result.isAuthorized != 'undefined' && !result.isAuthorized) {
            //ShowLogINPopup();
            //window.location.href = '../Home/Index';
            RedirectToUrl(result.redirectURL);
        }
        else {
            ShowNotification(_msgErrorOccured);
        }
    }
}

function OnFail(result) {

}


function RegisterCCCallback() {

    if ($('#divCapMain').is(':visible')) {

        if ($('.video-chart-row').length == 0 || $('.video-chart-row').is(':visible') == false) {
            $('#divCapMain').hide(500);

            $('#diviframe').animate({
                width: '585px'
            }, 1000, function () {
            });


            $('#divIFrameMain').animate({
                width: '565px'
            }, 1000, function () {
                $('#imgCCDirection').attr('src', '../../images/left_arrow_cc.gif');
                var divmarleft = parseInt(($(window).width() / 2) - (600 / 2)).toString() + 'px';
                $('#time').attr('style', 'margin-left:3.7%;')

                $('#diviframe').animate({
                    left: divmarleft,
                    position: "fixed"
                });

            });
        }
    }
    else {

        $('#diviframe').animate({
            width: '900px'
        }, 1000, function () {

        });

        $('#divIFrameMain').animate({
            width: '885px'

        }, 1000, function () {
            $('#imgCCDirection').attr('src', '../../images/right_arrow_cc.gif');
            $('#time').attr('style', 'margin-left:38.5%;')
            var divmarleft = parseInt(($(window).width() / 2) - (920 / 2)).toString() + 'px';
            $('#diviframe').animate({
                left: divmarleft,
                position: "fixed"
            });
            $('#divCapMain').show();
        });

    }

}

function ClosePlayer(divID) {

    if (document.getElementById("divRawMedia") != null || document.getElementById("divRawMedia") != undefined) {
        $("#divRawMedia").html('');
    }
    else if (document.getElementById("divClipPlayer") != null || document.getElementById("divClipPlayer") != undefined) {
        $("#divClipPlayer").html('');
    }

    $('#' + divID).css({ "display": "none" });
    $('#' + divID).modal('hide');
    $('#' + divID).remove();
}

function setSeekPoint(C, B) {
    var Flash = document.getElementById('HUY');
    if (Flash != null) {
        Flash.setSeekPoint(C);
    }
    if (B) { $(B).addClass("selected"); $("#timeBar").children(":not(.selected)").fadeTo(400, 0.4); $(B).removeClass("selected"); $(B).fadeTo(400, 1); }
}