var _PreviousPlayState = 0;
var _PlayState = 0;
var _VideoMetaData = "";
var _Start = 0;
var _Stop = 0;
var _Width = 704;
var _Height = 396;
var _Vol = 0;
var _SeekSecond = 0;
var _ID = null;
var _firstCall = false;
var _currenttime = 0;
var _currentTimeInt = 0;
var _processedTitle120 = -1;
var _programTitle = "NA";
var _nielsenData = null;
var _categoryData = null;
var _clipID = null;
var _flash = null;
var _agentGuid = null;
var videotype = '';
var xIndex = 0;
var LastIndex = 0;
var _CurrentTotalPlaySeconds = 1;
var _IsCheckSettings = true;
var _ForceCategorySelection = false;
var _TotalTime = 0;
var _FileName = "";
var _Player = "";
var _PlayerObj = "";
var _EvtCallback = null;
var _MkClipVisiblility = false;
var _xdomplayer = false;
var _fscr = 1;
var _abt = 0;
var _clp = 0;
var _EvtCallback = null;
var _PS = 0;
var _EvtCallbackProcessedTime = null;
var _GetClipDetail = 0;
// IE Version below 10
var _IEVB10 = false;
var _VidControlsTimeout = null;
var _SliderTimeTimeout = null;

if (!window.console) {
    var console = {
        log: function () { }
    };
}

if (document.documentMode != undefined && document.documentMode < 10) {
    _IEVB10 = true;
}

Number.prototype.toHHMMSS = function () {

    var sec_num = parseInt(this, 10);
    var hours = Math.floor(sec_num / 3600);
    var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
    var seconds = sec_num - (hours * 3600) - (minutes * 60);

    if (hours > 0) {
        if (hours < 10) { hours = "0" + hours; }
    }
    else {
        hours = "";
    }
    if (minutes < 10) { minutes = "0" + minutes; }
    if (seconds < 10) { seconds = "0" + seconds; }
    if (hours != "") {
        return hours + ':' + minutes + ':' + seconds;
    }
    else {
        return minutes + ':' + seconds;
    }
}

function hmsToSecondsOnly(str) {
    var p = str.split(':'),
        s = 0, m = 1;

    while (p.length > 0) {
        s += m * parseInt(p.pop(), 10);
        m *= 60;
    }

    return s;
}

var PlayPause = function (e) {

    if (_PlayState == 0) {
        _PlayState = 1;

        if (_flash != null) {
            _flash.externalPlay(_PlayState);
        }


        if (_VidControlsTimeout != null) {
            $(".vid-controls-holder").unbind("mouseleave");
            ClearVidControlsTimeout();

            $(".vid-controls-holder").fadeIn();
            $(".vid-controls-background").show();

        }
    }
    else {
        _PlayState = 0;

        SetHoverOnPlayerControls();
        SetMousemoveOnPlayer();
        if (_VidControlsTimeout != null) {

            SetVidControlsTimeout();
        }

        if (_flash != null) {
            _flash.externalPlay(_PlayState);
        }
    }
}

var SetPlayState = function () {

    _flash.externalPlay(_PreviousPlayState);
}

var SetFullScreen = function () {

    var elem = document.getElementById("divClipPlayer");
    if (!document.fullscreenElement &&
                    !document.mozFullScreenElement && !document.webkitFullscreenElement && !document.msFullscreenElement) {


        if (elem.requestFullscreen) {
            elem.requestFullscreen();
        } else if (elem.msRequestFullscreen) {
            elem.msRequestFullscreen();
        } else if (elem.mozRequestFullScreen) {
            elem.mozRequestFullScreen();
        } else if (elem.webkitRequestFullscreen) {
            elem.webkitRequestFullscreen();
        }
        /*$('#HUY').css('width', screen.width);
        $('#HUY').attr('width', screen.width);
        $('#HUY').css('height', screen.height);
        $('#HUY').attr('height', screen.height);*/
    }
}

$(window).resize(function () {
    if (!document.fullscreenElement &&
                    !document.mozFullScreenElement && !document.webkitFullscreenElement && !document.msFullscreenElement) {

        /*$('#HUY').css('width', _Width);
        $('#HUY').attr('width', _Width);
        $('#HUY').css('height', _Height);
        $('#HUY').attr('height', _Height);*/
    }
});

var UpdateVolume = function () {
    _Vol = $("#divVidVolSlider").slider("value");

    if (_flash == null || _flash == undefined) {
        _flash = document.getElementById('HUY');
    }

    _flash.externalUpdateVol((_Vol / 100));
    $("#divVidVolImg > img").hide();

    if (_Vol == 0) {

        $("#imgVidVolMute").show();
    }
    else {
        $("#imgVidVol").show();
    }
}

var setSeekPoint = function (C, B) {
    var Flash = document.getElementById('HUY');
    if (Flash != null) {
        Flash.setSeekPoint(C);
        _EvtCallbackProcessedTime = null;
    }

    return false;
}

function LoadClipPlayer(clipID, ps, hcc, arsz, asz,ap) {
    videotype = 'clip';
    _ID = clipID;
    _PS = ps;

    var jsonPostData = { ClipID: clipID, HCC: hcc, p_ARSZ: arsz, p_ASZ: asz, p_AP: ap }

    $.when($.ajax({

        type: 'POST',
        dataType: 'json',
        url: _urlLibraryLoadClipPlayer,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),
        global: false,
        success: LibraryLoadNPlayer,
        error: OnClipLoadFail
    })
    ).then(function () {
        $.when($.ajax({

            type: 'GET',
            dataType: 'jsonp',
            url: _urlVideoMetaData + _ID + "&Type=rawmedia",
            contentType: 'application/json; charset=utf-8',
            global: false,
            success: OnParsePlayerMetaData,
            error: OnFailPlayerMetaData
        })
        ).then(function () {
            SetLibraryPlayerMetaData();
        });

    });

    $("#imgLoading").show();

    return _Player;
}

var SetClipMetadata = function (clipid) {

    _ID = clipid;

    $.when($.ajax({

        type: 'GET',
        dataType: 'jsonp',
        url: _urlVideoMetaData + _ID + "&Type=rawmedia",
        contentType: 'application/json; charset=utf-8',
        global: false,
        success: OnParsePlayerMetaData,
        error: OnFailPlayerMetaData
    })).then(function () { SetLibraryPlayerMetaData(); });

}

var LibraryLoadNPlayer = function (result) {

    $("#imgLoading").hide();

    if (result.isSuccess) {
        _clientGuid = result.clientGuid;
        _PlayerFromEmail = result.email;
        _FileName = result.title;

        $("#divClipPlayer").append(result.clipHTML);
        InitializePlayerComponents();
    }
    else {
        ShowNotification(_msgErrorOccured);
    }
}

var InitializePlayerComponents = function () {

    _flash = document.getElementById('HUY');
    _firstCall = true;

    $("#divVidVolSlider").slider({
        orientation: "horizontal",
        range: "min",
        min: 0,
        max: 100,
        value: 75,
        slide: UpdateVolume,
        change: UpdateVolume
    });

    _processedTitle120 = -1;
    _programTitle = "";

    // Placeholder for older browser
    if (typeof ($('input[placeholder], textarea[placeholder]').placeholder) != undefined) {
        $('input[placeholder], textarea[placeholder]').placeholder();
    }
}

function OnClipLoadFail(XHR, Status, Error) {
    $("#imgLoading").hide();
    ShowNotification(_msgSomeErrorProcessing);
}

var OnParsePlayerMetaData = function (result) {
    _VideoMetaData = result;
}

var OnFailPlayerMetaData = function (jqXHR, textStatus, errorThrown) {
    //console.log(errorThrown);
}

var SetLibraryPlayerMetaData = function () {

    if (_VideoMetaData[0] != null && _VideoMetaData[0].Status == 0) {

        var dt = new Date(_VideoMetaData[0].VideoMetaData.IQ_Local_Air_Date.replace('T',' ').replace('Z',''));

        if (document.documentMode != undefined && document.documentMode < 9) {

            dt = new Date((_VideoMetaData[0].VideoMetaData.IQ_Local_Air_Date).replace('-', '/').replace('T', ' '));
        }

        var mmm = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        var hour = dt.getHours();
        var zz = hour > 12 ? "pm" : "am";
        hour = hour % 12;
        hour = hour > 0 ? hour : 12;
        var timezone = "";

        if (_VideoMetaData[0].VideoMetaData.TimeZone) {

            timezone = _VideoMetaData[0].VideoMetaData.TimeZone;
        }

        if (_VideoMetaData[0].VideoMetaData.Title120s != null) {
            $(".vid-clp-info-title").html(_VideoMetaData[0].VideoMetaData.Title120s[0]);
        }
        $(".vid-clp-info-aired").html("<label>Aired: </label>" + mmm[dt.getMonth()] + " " + dt.getDate() + ", " + dt.getFullYear() + " at " + hour + " " + zz + " " + timezone);

        if (_VideoMetaData[0].VideoMetaData.StationCallSign != null) {
            $(".vid-clp-info-station").html(_VideoMetaData[0].VideoMetaData.StationCallSign + ' (' + _VideoMetaData[0].VideoMetaData.StationAffiliate + ') - ' + _VideoMetaData[0].VideoMetaData.IQ_Dma_Name);
        }
        else {
            $(".vid-clp-info-station").html(_VideoMetaData[0].VideoMetaData.IQ_Dma_Name);
        }

        if (_VideoMetaData[0].VideoMetaData.IQ_Dma_Num != null) {
            $("#spnRank").html(_VideoMetaData[0].VideoMetaData.IQ_Dma_Num);
        }
        else {
            $("#spnRank").html("NA");
        }

        GetNielsenData(_VideoMetaData[0].VideoMetaData.IQ_Dma_Num);
    }
    else {
        $("#spnViews").html("NA");
        $("#spnValue").html("NA");
        $("#spnRank").html('NA');
        $(".vid-clp-info-aired").html("<label>Aired: NA</label>");
        $(".vid-clp-info-station").html("Market: NA");
    }

    $(".vid-clp-info-nlsn").show();
}

var GetNielsenData = function (dmanum) {

    var jsonPostData = {
        Guid: _ID,
        ClientGuid: _clientGuid,
        IsRawMedia: false,
        IQ_Start_Point: 4,
        IQ_Dma_Num: dmanum
    }

    if (!_IEVB10) {
        $.ajax({

            type: 'POST',
            dataType: 'json',
            url: _GetNielSenDataUrl,
            global: false,
            xhrFields: {
                withCredentials: true
            },
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            success: OnParseNielsenData,
            error: OnFailNielsenData
        });
    }
    else {

        $.ajax({

            type: 'POST',
            dataType: 'json',
            url: _urlNielsenDataWoXd,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            success: OnParseNielsenData,
            error: OnFailNielsenData
        });

    }
}

var OnParseNielsenData = function (result) {

    _nielsenData = result;

    if (_nielsenData.Status == 0 && _nielsenData.NielSenData.length > 0) {
        nielsenAudience = _nielsenData.NielSenData[0].AUDIENCE;
        nielsenMediaValue = _nielsenData.NielSenData[0].SQAD_SHAREVALUE;
        if (nielsenAudience != null) {

            $("#spnViews").html(nielsenAudience);

        }
        else {
            $("#spnViews").html("NA");
        }

        if (nielsenMediaValue != null) {

            $("#spnValue").html(nielsenMediaValue);

        }
        else {
            $("#spnValue").html("NA");
        }
    }
    else {
        $("#spnViews").html("NA");
        $("#spnValue").html("NA");
    }

}

var OnFailNielsenData = function (jqXHR, textStatus, errorThrown) {
    //console.log(errorThrown);
}

var SetVidControlsTimeout = function () {
    $(".vid-controls-holder").fadeIn();
    $(".vid-controls-background").show();
    _VidControlsTimeout = setTimeout(function () {
        $(".vid-controls-holder").fadeOut();
        $(".vid-controls-background").hide();
        $(document).css("cursor", "none");
    }, 3000);
}

var SetMousemoveOnPlayer = function () { $("#divClipPlayer").mousemove(function (e) { clearTimeout(_VidControlsTimeout); SetVidControlsTimeout(); }); }

var SetHoverOnPlayerControls = function () { $(".vid-controls-holder").hover(function (e) { e.stopPropagation(); ClearVidControlsTimeout(); }, function () { SetVidControlsTimeout(); SetMousemoveOnPlayer(); }); };

var ClearVidControlsTimeout = function () {

    $("#divClipPlayer").unbind("mousemove");
    clearTimeout(_VidControlsTimeout);
}

var ShowSliderTime = function (ev, leftPos) {

    var innerPointX = (ev.pageX - leftPos);
    var intWidth = $(".vid-progress-bar").width();
    if (innerPointX > intWidth) {
        innerPointX = intWidth;
    }

    if (innerPointX < 0) {
        innerPointX = 0;
    }

    _SeekSecond = ((innerPointX * _TotalTime) / intWidth);

    if (_SeekSecond < 0) {
        _SeekSecond = 0;
    }

    if (_SeekSecond > _Stop) {
        _SeekSecond = _Stop;
    }

    $('.vid-slider-time').html(_SeekSecond.toHHMMSS());

    if (innerPointX < (intWidth - 40)) {
        $('.vid-slider-time').css({ 'right': "" });
        $('.vid-slider-time').css({ 'left': innerPointX });
    }
    else {
        $('.vid-slider-time').css({ 'left': "" });
        $('.vid-slider-time').css({ 'right': 0 });
    }


    $('.vid-slider-time').show();

}

var AddPlayerEvtListners = function () {

    SetVidControlsTimeout();
    SetMousemoveOnPlayer();

    SetHoverOnPlayerControls();

    $("#divVidVol").hover(function (e) { e.stopPropagation(); $("#divVidVolSlider").fadeIn(); }, function () { $("#divVidVolSlider").fadeOut(); });

    $("#divVidPlay").click(function (e) { PlayPause(); });

    $("#divVidFF").click(function () {
        _flash.externalFFFR(0, 3);
    });

    $("#divVidFF2").click(function () {

        _flash.externalFFFR(0, 6);
    });

    $("#divVidRW").click(function () {

        _flash.externalFFFR(1, 3);
    });

    $("#divVidRW2").click(function () {

        _flash.externalFFFR(1, 6);
    });

    $(".vid-progress-bar").mouseout(function () {
        $('.vid-slider-time').hide();
    });

    $(".vid-progress-bar").click(function (e1) {

        _PreviousPlayState = _PlayState;

        _flash.externalPlay(1);

        var innerPointX = (e1.pageX - $(this).offset().left);
        var intWidth = $(".vid-progress-bar").width();
        if (innerPointX > intWidth) {
            innerPointX = intWidth;
        }

        if (innerPointX < 0) {
            innerPointX = 0;
        }

        _SeekSecond = ((innerPointX * _TotalTime) / intWidth);

        if (_SeekSecond < 0) {
            _SeekSecond = 0;
        }

        if (_SeekSecond > _Stop) {
            _SeekSecond = _Stop;
        }

        $('.vid-slider-time').html(_SeekSecond.toHHMMSS());

        var progTime = ((_SeekSecond * intWidth) / _TotalTime);
        var progTimelength = ((progTime * 99.6316758747698) / intWidth);

        $('.vid-play-bar').css({ 'width': progTimelength + "%" });

        $('.vid-slider-time').css({ 'left': innerPointX });
        $('.vid-slider-time').show('slow');

        //console.log("Seek...video-progress-bar");
        //_flash.setSeekPoint(_Start + _SeekSecond);
        setSeekPoint(_Start + _SeekSecond);

        SetPlayState();
    });

    $(".vid-progress-bar").hover(function (e) {

        consolelog("..hover.." + e.clientX);

        _SliderTimeTimeout = setTimeout(function () {

            $(".vid-progress-bar").mousemove(function (ev) {

                console.log("..mouse move.." + $(this).offset().left);
                ShowSliderTime(ev, $(this).offset().left);

            });
        }, 300);

    }, function () { clearTimeout(_SliderTimeTimeout); $(".vid-progress-bar").unbind("mousemove"); consolelog("..hover out.."); });


    $(".video-share").click(function () {
        var v = $(".video-share-main").is(":visible");

        $(".video-share-main").slideToggle();

        if (v) {
            _previewLoaded = false;
            $(".video-player-controls-overlay").remove();
            $(".video-share").parent("li").removeClass("video-active");
        }
        else {
            $(".video-player-controls").append("<div class=\"video-player-controls-overlay\"></div>");

            $(".video-share").parent("li").siblings().removeClass("video-active");
            $(".video-share").parent("li").addClass("video-active");
            _PlayState = 1;
            if (_flash != null) {
                _flash.externalPlay(_PlayState);
            }
        }
    });
}

var SetPlayerInfo = function (logo, title) {

    $("#imgVidSrc").attr("src", logo);
}

var SetTimings = function (start, stop) {
    _Start = start;
    _Stop = stop;

    _TotalTime = _Stop - _Start;

    $(".vid-time-duration").html(_TotalTime.toHHMMSS());
}

var UpdateTimeDuration = function (currentTime) {

    _currenttime = currentTime;
    _currentTimeInt = parseInt(currentTime);

    var intWidth = $(".vid-controls-holder").width();

    var progTime = ((currentTime * intWidth) / _TotalTime);
    var progTimelength = ((progTime * 100) / intWidth);
    $(".vid-time-current").html(currentTime.toHHMMSS());
    $(".vid-play-bar").css("width", progTimelength + "%");

    if (_firstCall) {
        _firstCall = false;

        $(".video-fullscreen").click(function (e) {
            SetFullScreen();
        });

        _flash = document.getElementById('HUY');

        if (_flash != null) {

            AddPlayerEvtListners();
            ClippingEvts();
        }

        $(".vid-controls-holder").show();
    }

    if (_currenttime >= _TotalTime) {

        ClearVidControlsTimeout();
        $(".vid-controls-holder").unbind("mouseleave");
        $(".vid-controls-holder").fadeIn();
        $(".vid-controls-background").show();
    }

}

var UpdatePlayerState = function (state) {

    $("#imgLoading").hide();

    switch (state) {
        case -1:
            break;
        case 0:
            _PlayState = 0;

            $("#divVidPlay>img").hide();
            $("#imgVidPause").show();

            break;
        case 1:
            _PlayState = 1;

            console.log("..play state 1");

            $("#divVidPlay>img").hide();
            $("#imgVidPlay").show();

            break;
        case 2:

            if (_firstCall === false) {
            }

            $("#divVidPlay>img").hide();
            $("#imgVidPlay").show();

            break;
        case 3:
            $("#imgLoading").show();
            break;
        case 4:
            break;
    }
}

//__________________________________________________________External Interface______________________________________________________________________________________

var GetRef = function () {
    console.log('PlayerClip.js');
    var url = (window.location != window.parent.location) ? document.referrer : document.location.href;
    return url;
}

var consolelog = function (Param) {
    console.log(Param);
}

var ResizeContainer = function (width, height) {

    if (width != 0 && height != 0) {

        $("#divDim").css("max-width", width);

        var relPer = (height * 100 / width);

        $("#divClipPlayer").animate({ "padding-top": relPer + "%" }, 1500);
    }
}
