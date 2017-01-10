var _PreviousPlayState = 0;
var _PlayState = 0;
var _VideoMetaData = "";
var _Start = 0;
var _Stop = 0;
var _Width = 545;
var _Height = 312;
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
var _ISFScr = false;

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

var PlayPause = function () {

    console.log("play pause clicked");

    if (_PlayState == 0) {
        _PlayState = 1;
        if (_flash != null) {
            _flash.externalPlay(_PlayState);
        }
    }
    else {
        _PlayState = 0;
        if (_flash != null) {
            _flash.externalPlay(_PlayState);
        }
    }
}

var SetPlayState = function () {

    _flash.externalPlay(_PreviousPlayState);
}

var SetFullScreen = function () {

    var elem = document.getElementById("HUY");
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
        $('#HUY').css('width', screen.width);
        $('#HUY').attr('width', screen.width);
        $('#HUY').css('height', screen.height);
        $('#HUY').attr('height', screen.height);

        $(document).mouseup(function (e) { FullScreenClick(); });
        _ISFScr = true;
    }
}

//$(document).on('webkitfullscreenchange mozfullscreenchange fullscreenchange', function (e) {

//    if (_ISFScr) {
//        $(document).click(function () { FullScreenClick(); });
//    }

//});

$(window).resize(function () {
    if (!document.fullscreenElement &&
                    !document.mozFullScreenElement && !document.webkitFullscreenElement && !document.msFullscreenElement) {

        $('#HUY').css('width', _Width);
        $('#HUY').attr('width', _Width);
        $('#HUY').css('height', _Height);
        $('#HUY').attr('height', _Height);

        _ISFScr = false;

        $(document).unbind("mouseup");
        $("#imgFPause").hide();
        $("#imgFPlay").hide();
    }
});

var FullScreenClick = function () {

    if (_ISFScr) {
        if (_PlayState == 0) {

            $("#imgFPause").show();
            $("#imgFPause").animate({ width: "256px", height: "256px", "opacity": "0" }, 1500, function () { $(this).hide(); $("#imgFPause").css({ width: "128px", height: "128px", "opacity": "1" }); });
        }
        else {
            $("#imgFPlay").show();
            $("#imgFPlay").animate({ width: "256px", height: "256px", "opacity": "0" }, 1500, function () { $(this).hide(); $("#imgFPlay").css({ width: "128px", height: "128px", "opacity": "1" }); });
        }

        PlayPause();
    }
}

var UpdateVolume = function () {
    _Vol = $("#video-vol-slider").slider("value");

    if (_flash == null || _flash == undefined) {
        _flash = document.getElementById('HUY');
    }

    _flash.externalUpdateVol((_Vol / 100));

    if (_Vol == 0) {
        $(".video-vol-marker > img").hide();
        $("#imgVideoVolume_0").show();
        $("#imgVideoMute").hide();
        $("#imgVideoMute_1").show();
    }
    else {
        $("#imgVideoMute").show();
        $("#imgVideoMute_1").hide();

        $(".video-vol-marker > img").hide();

        if (_Vol > 67) {

            $("#imgVideoVolume").show();
        }
        else if (_Vol > 33) {
            $("#imgVideoVolume_2").show();
        }
        else {
            $("#imgVideoVolume_1").show();
        }
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

function LoadClipPlayer(clipID, callback, ps, hcc) {
    videotype = 'clip';
    _Callback = callback;
    _ID = clipID;
    _PS = ps;

    var jsonPostData = { ClipID: clipID, HCC: hcc, p_ARSZ: !(typeof _Callback == "function") }

    $.when($.ajax({

        type: 'POST',
        dataType: 'json',
        url: _urlLibraryLoadClipPlayer,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),

        success: LibraryLoadNPlayer,
        error: OnClipLoadFail
    })
    ).then(function () {
        $.when($.ajax({

            type: 'GET',
            dataType: 'jsonp',
            url: _urlVideoMetaData + _ID + "&Type=rawmedia",
            contentType: 'application/json; charset=utf-8',
            success: OnParsePlayerMetaData,
            error: OnFailPlayerMetaData
        })
        ).then(function () {
            SetLibraryPlayerMetaData();
        });

    });

    if (IsShowTimeSync) {
        /* load chart for player */
        $.ajax({

            type: "post",
            dataType: "json",
            url: _urlLibraryGetChart,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(jsonPostData),
            success: OnGetChartComplete,
            error: OnGetChartFail
        });
    }
    return _Player;
}

var LibraryLoadNPlayer = function (result) {

    if (result.isSuccess) {
        _clientGuid = result.clientGuid;
        _PlayerFromEmail = result.email;
        _FileName = result.title;

        var videoWidth = AdjustInitialDimension(1034, 545);
        var capInfoWidth = GetCaptionInfoWidth(videoWidth);
        var totalWidth = GetTotalInitialWidth(videoWidth);

        _Width = 545;
        _Height = 312;

        var ap = 0;

        try {

            ap = parseInt(_ClipEmbedAutoPlay);

        } catch (e) {

        }

        var rawPlayer = '<div class="video-modalPlayer hide resizable select-none" id="divPlayer" style="max-width:' + totalWidth + 'px;">';
        if (_PS == undefined || _PS === 0) {
            rawPlayer = rawPlayer + '<div class="video-close">X</div>'
        }
        rawPlayer = rawPlayer + '<div class="video-container-narrow" style="max-width:' + totalWidth + 'px;">'
    + '<div class="video-player-row-one">';

        if (_PS !== 2) {
            rawPlayer = rawPlayer
        + '<div class="video-caption-info video-fleft" style="width:' + capInfoWidth + 'px;">'
        + '<div class="video-caption">'
        + '<div id="divCaptionHeader" class="video-caption-tab">'
        + '<div align="center" id="divCaptionTab" style="cursor:pointer;min-width:50px;" class="active">Captions</div>'
        + '</div>'
        + '<div id="divCapContent" class="video-caption-content">'
        + '<div id="divCapAllContent" style="width:auto;"  class="select-text"></div>'
        + '</div>'
        + '</div>'
        + '<div class="video-info">'
        + '<img class="video-fleft" id="video-source">'
        + '<div class="video-stats video-fright">'
        + '<span class="video-views">...</span>'
        + '<span class="video-value">...</span>'
        + '<span class="video-rank">...</span>'
        + '</div>'
        + '<div class="video-video-info">'
        + '<div id="video-ptitle"></div>'
        + '<span class="video-program">Market</span>'
        + '<span class="video-aired"><label>Aired:</label>...</span>'
        + '</div>'
        + '</div>'
        + '</div>';
        }

        rawPlayer = rawPlayer + '<div class="video-player video-fleft" style="margin-top:13px;">'
    + '<div class="video-video" id="divClipVideo" style="width:' + videoWidth + 'px;">'
        + '<div class="video-abt-main">'
            + '<div class="video-abt-box">'
                + '<div class="video-abt-version">Video Player<br><span>Version: 1.34.44</span></div>'
                + '<div class="video-abt-logo" align="center"><img alt="iQ media" src="../../images/logo_N.png"></div>'
                + '<div class="video-abt-copyright">&copy; 2014 • iQ media <a target="_blank" href="http://www.iqmediacorp.com">http://www.iqmediacorp.com</a></div>'
            + '</div>'
        + '</div>'
        + '<div class="video-buffering">'
            + '<img src="../../images/video-player/buffering.gif" />'
        + '</div>'
    + '</div>'
    + '<div class="video-player-controls">'
          + '<div class="video-progress-bar">'
            + '<div class="video-current-time">'
            + '</div>'
            + '<div class="video-duration">'
            + '</div>'
            + '<div style="width:0%;" class="video-play-bar">'
            + '</div>'
            + '<div class="video-slider-knob">'
            + '</div>'
            + '<div class="video-slider-time">'
            + '</div>'
          + '</div>'
          + '<div class="video-control-holder" style="height:59px;">'
            + '<div>'
                + '<div style="height:5px;">'
                    + '<div class="video-controls-gradient" style="float:left;width:47%;border-right:2px solid #000;"></div>'
                    + '<div class="video-controls-gradient" style="float:left;width:40%;border-right:2px solid #000;"></div>'
                    + '<div class="video-controls-gradient" style="float:left;width:13%;"></div>'
                + '</div>'
                + '<div class="video-controls video-control-holder" style="width:47%;overflow:hidden;">'
                    + '<div>'
                        + '<a href="javascript:;" id="ancVideoFRew"><span></span><img id="imgVideoFRew" src="/images/video-player/frew.png?v=1.1" /><img id="imgVideoFRewActive" src="/images/video-player/frew_Active.png?v=1.1" style="display:none;" /></a>'
                    + '</div>'
                    + '<div>'
                        + '<a href="javascript:;" id="ancVideoRew"><span></span><img id="imgVideoRew" src="/images/video-player/rew.png?v=1.1" /><img id="imgVideoRewActive" src="/images/video-player/rew_Active.png?v=1.1" style="display:none;" /></a>'
                    + '</div>'
                    + '<div style="width:28%;" class="video-controls-play">'
                        + '<a href="javascript:;" id="ancVideoPlay"><span></span><img id="imgVideoPlay" src="/images/video-player/play.png?v=1.1" style="display:none;" /><img id="imgVideoPause" src="/images/video-player/pause.png?v=1.1" /><img id="imgVideoPlayActive" src="/images/video-player/play_active.png?v=1.1" style="display:none;" /><img id="imgVideoPauseActive" src="/images/video-player/pause_active.png?v=1.1" style="display:none;" /></a>'
                    + '</div>'
                    + '<div>'
                        + '<a href="javascript:;" id="ancVideoFwd"><span></span><img id="imgVideoFwd" src="/images/video-player/fwd.png?v=1.1" /><img id="imgVideoFwdActive" src="/images/video-player/fwd_Active.png?v=1.1" style="display:none;" /></a>'
                    + '</div>'
                    + '<div>'
                        + '<a href="javascript:;" id="ancVideoFFwd"><span></span><img id="imgVideoFFwd" src="/images/video-player/ffwd.png?v=1.1" /><img id="imgVideoFFwdActive" src="/images/video-player/ffwd_Active.png?v=1.1" style="display:none;" /></a>'
                    + '</div>'
                + '</div>'
                + '<div class="video-control-holder video-volume" style="width:40%;">'
                    + '<div class="video-vol-mute">'
                        + '<span></span><img id="imgVideoMute" src="/images/video-player/mute.png" /><img id="imgVideoMute_1" src="/images/video-player/mute_1.png" style="display:none;" /></div>'
                    + '<div id="video-vol-slider"></div>'
                    + '<div class="video-vol-marker">'
                        + '<span></span><img id="imgVideoVolume" src="/images/video-player/volume.png" /><img id="imgVideoVolume_0" src="/images/video-player/volume_0.png" style="display:none;" /><img id="imgVideoVolume_1" src="/images/video-player/volume_1.png" style="display:none;" /><img id="imgVideoVolume_2" src="/images/video-player/volume_2.png" style="display:none;" /></div>'
                + '</div>';

        if (_fscr == 1) {
            rawPlayer = rawPlayer + '<div class="video-control-holder video-fullscreen video-fleft"><div><span></span><img id="imgVideoFullScreen" src="/images/video-player/fullscreen.png" /><img id="imgVideoFullScreenActive" src="/images/video-player/fullscreen_active.png" style="display:none;" /></div>'
                        + '</div>';
        }
        rawPlayer = rawPlayer + '</div></div>'
          + '<div class="video-player-controls-overlay"></div>'
        + '</div>'
    + '</div>';
        if (_PS !== 2) {
            rawPlayer = rawPlayer

+ '<div class="video-about-clip video-fleft">'
    + '<ul class="video-action">'
            //    + '<li><a class="video-about" href="#">About</a></li>'
            if (result.isSharing || result.isEmailSharing) {
                rawPlayer += '<li><a class="video-share" href="#">Share</a></li>';
            }
            if (IsShowTimeSync) {
                rawPlayer += '<li style="display:none;"><a class="video-chart" href="#">Time Sync</a></li>';
            }
            rawPlayer += '</ul>'
    + '</div>';
        }

        rawPlayer = rawPlayer + '<div class="video-clear">'
    + '</div>'
    + '</div>';

        if (IsShowTimeSync) {
            rawPlayer += '<div class="video-chart-row" style="display:none;">'
                    + '<div class="margintop10">'
                    + '<div class="clear">'
                    + '<div class="chart-tabs" >'
                    + '</div>'
                    + '</div>'
                    + '<div class="clear chart-tab-content">'
                    + '</div>'
                    + '</div>'
                    + '</div>';
        }
        if (result.isSharing || result.isEmailSharing) {
            rawPlayer += '<div class="video-share-main">'
                   + '<div class="video-share-left"  style="width:' + capInfoWidth + 'px;">'
            if (result.isEmailSharing) {
                rawPlayer += '<div id="divEmailForm" style="display:none;">'
                                + '<div class="email-form">'
                                    + '<input type="text" placeholder="Email Address" id="txtPlayerFromEmail" readonly="readonly" value="' + result.email + '" class="video-clip-email-from">'
                                    + '<input type="text" placeholder="Email To" id="txtPlayerToEmail" class="video-clip-email-to">'
                                    + '<input type="text" placeholder="Email Subject" id="txtPlayerSubject" class="video-clip-email-subject">'
                                    + '<textarea placeholder="Email Message" id="txtPlayerMessage" class="video-clip-email-message"></textarea>'
                                + '</div>'
                                + '<div style="text-align:right; margin-right:0px;">'
                                    + '<span class="help-inline" id="spanEmailMessage" style="color: #FF0000; display: none; margin-right:2px;"></span>'
                                    + '<input type=\"button\" value=\"Cancel\" class="cancelButton" id=\"btnCancel\" onclick=\"CancelEmail();\">'
                                    + '<input type=\"button\" value=\"Send\" id=\"btnSend\" class="blueButton" onclick=\"SendPlayerEmail();\">'
                                + '</div>'
                            + '</div>'

            }
            if (result.isSharing) {
                rawPlayer += '<div id="divLink" style="display:none">'
                                + '<div class="embed-form">'
                                    + '<textarea class="video-clip-embed" id="txtLink" readOnly="true">' + _urlClipPlayer + _ID + '&ps=2' + '</textarea>'
                                + '</div>'
                                + '<div style="text-align:right; margin-right:0px;">'
                                    + '<input type=\"button\" class="cancelButton" value=\"Cancel\" id=\"btnCancelLink\" onclick=\"CancelLink();\">'
                                    + '<input type=\"button\" class="blueButton" value=\"Select\" id=\"btnCopyLink\" onclick=\"CopyLink();\">'
                                + '</div>'
                            + '</div>'
                rawPlayer += '<div id="divEmbedForm" style="display:none">'
                                + '<div class="embed-form">'
                                    + '<textarea class="video-clip-embed" id="txtEmbed" readOnly="true"><iframe src="' + _urlClipPlayer + _ID + '&ps=2" width="547" height="411" allowfullscreen="true" style="border:0 none;" /></textarea>'
                                + '</div>'
                                + '<div style="text-align:right; margin-right:0px;">'
                                    + '<input type=\"button\" class="cancelButton" value=\"Cancel\" id=\"btnCancelEmbed\" onclick=\"CancelEmbed();\">'
                                    + '<input type=\"button\" class="blueButton" value=\"Select\" id=\"btnCopyEmbed\" onclick=\"CopyEmbed();\">'
                                + '</div>'
                            + '</div>'
                            + '<div id="divWordPressForm" style="display:none">'
                                + '<div class="embed-form">'
                                    + '<textarea class="video-clip-embed" id="txtWordPress" readOnly="true"><iframe src="' + _urlClipPlayer + _ID + '&ps=2&ap=' + ap + '" width="547" height="411" allowfullscreen="true" style="border:0 none;" /></textarea>'
                                + '</div>'
                                + '<div style="text-align:right; margin-right:0px;">'
                                    + '<input type=\"button\" class="cancelButton" value=\"Cancel\" id=\"btnCancelWordPress\" onclick=\"CancelWordPress();\">'
                                    + '<input type=\"button\" class="blueButton" value=\"Select\" id=\"btnCopyWordPress\" onclick=\"CopyWordPress();\">'
                                + '</div>'
                            + '</div>'
            }
            rawPlayer += '</div>'
                   + '<div class="video-share-right">';

            rawPlayer = rawPlayer + '<div style="clear:both;width:100%;">';

            if (result.isEmailSharing) {
                rawPlayer = rawPlayer + '<div style="float:left;width:10%;">Email</div>'
                                      + '<div style="float:left;width:2%;">&nbsp;</div>';
            }

            if (result.isSharing) {
                rawPlayer = rawPlayer + '<div style="float:left;width:10%;">Link</div>'
                                      + '<div style="float:left;width:3%;">&nbsp;</div>'
                                      + '<div style="float:left;width:53%;text-align:left;">Social</div>'
                                      + '<div style="float:left;width:10%;">Website</div>'
                                      + '<div style="float:left;width:2%;">&nbsp;</div>'
                                      + '<div style="float:left;width:10%;">Embed</div>';
            }

            rawPlayer = rawPlayer + '</div>';

            rawPlayer = rawPlayer + '<div style="clear:both;width:100%;margin-top:10px;">';

            if (result.isEmailSharing) {
                rawPlayer += '<div class="video-share-email">'
                                + '<img src="/images/MediaIcon/Email_Share.png" alt="email" style="cursor:pointer;" id="imgEmail">'
                            + '</div>'
                            + '<div style="width:2%;">&nbsp;</div>';
            }
            else {
                rawPlayer += '<div class="video-share-email">'
                                + '<span style="margin-left:15x;"></span>'
                            + '</div>';
            }
            if (result.isSharing) {
                rawPlayer += '<div class="video-share-link">'
                                + '<img src="/images/MediaIcon/link.png" alt="Link" style="cursor:pointer;" id="imgLink">'
                            + '</div>'
                            + '<div style="width:3%;">&nbsp;</div>';
                rawPlayer += '<div><a href="' + _urlLibraryShareClip + _ID + '&p_SourceType=FB" target="_blank"><img alt="facebook" src="/images/MediaIcon/facebook_Share.png" style="cursor:pointer;" id="imgFacebook"></a></div>'
                                + '<div style="width:2%;">&nbsp;</div>'
                                + '<div><a href="' + _urlLibraryShareClip + _ID + '&p_SourceType=TW" target="_blank"><img alt="twitter" src="/images/MediaIcon/twitter_share.png" style="cursor:pointer;" id="imgTwitter"></a></div>'
                                + '<div style="width:2%;">&nbsp;</div>'
                                + '<div><a href="' + _urlLibraryShareClip + _ID + '&p_SourceType=GP" target="_blank"><img alt="google plus" src="/images/MediaIcon/google-plus_share.png" style="cursor:pointer;" id="imgGoogle"></a></div>'
                                + '<div style="width:2%;">&nbsp;</div>'
                                + '<div><a href="' + _urlLibraryShareClip + _ID + '&p_SourceType=TR" target="_blank"><img alt="tumblr" src="/images/MediaIcon/Tumblr_Share.png" style="cursor:pointer;" id="imgTest"></a></div>'
                            + '<div style="width:7%;">&nbsp;</div>'
                            + '<div>'
                                 + '<img alt="embed" src="/images/MediaIcon/Dribbble_share.png" style="cursor:pointer;" id="imgEmbed">'
                            + '</div>'
                            + '<div style="width:2%;">&nbsp;</div>'
                            + '<div>'
                                + '<img alt="word press" src="/images/MediaIcon/WordPress_Share.png" style="cursor:pointer;" id="imgWordPress">'
                            + '</div>';

            }
            else {
                rawPlayer += '<div class="video-share-social">'
                            + '</div>'
            }
            rawPlayer += '</div></div>'
        + '</div>'
        }
        rawPlayer += '</div>'
    + '</div>'
    + '</div>';


        if (document.documentMode == undefined || document.documentMode >= 9) {
            $(document.body).append(rawPlayer);
        }

        $(document.body).append("<img id='imgFPlay' src='/images/video-player/play_f.png' style='display:none;position:absolute;top:0;right:0;bottom:0;left:0;margin:auto;z-index:2147483647;' />");
        $(document.body).append("<img id='imgFPause' src='/images/video-player/pause_f.png' style='display:none;position:absolute;top:0;right:0;bottom:0;left:0;margin:auto;z-index:2147483647;' />");

        _flash = document.getElementById('HUY');
        _firstCall = true;

        if (document.documentMode == undefined || document.documentMode >= 9) {
            $("#video-vol-slider").slider({
                orientation: "horizontal",
                range: "min",
                min: 0,
                max: 100,
                value: 75,
                slide: UpdateVolume,
                change: UpdateVolume
            });
        }

        $(".video-vol-marker > img").hide();
        $("#imgVideoVolume").show();

        $(".video-close").click(function (e) {

            $('#divPlayer').modal('hide');

        });

        $(".video-about").click(function () {
            if ($(".video-abt-main").is(":visible")) {

                $(".video-abt-box").fadeOut("slow", function () { $(".video-abt-main").slideUp(); });

            }
            else {
                $(".video-abt-main").slideDown(function () { $(".video-abt-box").fadeIn("slow"); });
            }
        }
            );

        $("#imgEmail").click(function () {

            if ($("#divEmailForm").is(":visible")) {
                $("#divEmailForm").slideUp(1000)
            }
            else {
                $("#txtPlayerToEmail").focus();
                $("#divEmailForm").slideDown(1000)
                $("#divEmbedForm").css('display', 'none')
                $("#divWordPressForm").css('display', 'none')
                $("#divLink").css('display', 'none')
            }
        }
            );

        $("#imgEmbed").click(function () {

            if ($("#divEmbedForm").is(":visible")) {
                $("#divEmbedForm").slideUp(1000)
            }
            else {
                $("#divEmbedForm").slideDown(1000)
                $("#divEmailForm").css('display', 'none')
                $("#divWordPressForm").css('display', 'none')
                $("#divLink").css('display', 'none')
            }
        }
            );

        $("#imgWordPress").click(function () {

            if ($("#divWordPressForm").is(":visible")) {
                $("#divWordPressForm").slideUp(1000)
            }
            else {
                $("#divWordPressForm").slideDown(1000)
                $("#divEmailForm").css('display', 'none')
                $("#divEmbedForm").css('display', 'none')
                $("#divLink").css('display', 'none')
            }
        }
            );

        $("#imgLink").click(function () {

            if ($("#divLink").is(":visible")) {
                $("#divLink").slideUp(1000)
            }
            else {
                $("#divLink").slideDown(1000)
                $("#divEmailForm").css('display', 'none')
                $("#divEmbedForm").css('display', 'none')
                $("#divWordPressForm").css('display', 'none')
            }
        }
            );

        _processedTitle120 = -1;
        _programTitle = "";

        //console.log("..calling SP data...");

        if (document.documentMode != undefined && document.documentMode < 9) {
            _PlayerObj = result.clipHTML;
        }
        else {
            $('.video-video').append(result.clipHTML);
        }

        $('#divCapAllContent').append(result.closedCaption);

        $('#divCapAllContent').mCustomScrollbar({
            scrollInertia: 1500,
            theme: "light-2",
            advanced: {
                updateOnContentResize: true,
                autoScrollOnFocus: false
            },
            mouseWheel: { scrollAmount: 100 }
        });

        $('#divCapDescriptionContent').mCustomScrollbar({
            scrollInertia: 1500,
            theme: "light-2",
            advanced: {
                updateOnContentResize: true,
                autoScrollOnFocus: false
            },
            mouseWheel: { scrollAmount: 100 }
        });

        /*
        $('#divCapAllContent').enscroll({
        verticalTrackClass: 'track4',
        verticalHandleClass: 'handle4',
        pollChanges: true
        });

        $('#divCapDescriptionContent').enscroll({
        verticalTrackClass: 'track4',
        verticalHandleClass: 'handle4',
        pollChanges: true
        });
        */
    }
    else {
        ShowNotification(_msgErrorOccured);
    }
    _Player = rawPlayer;

    if (typeof _Callback == "function") {
        _Callback();
    } else {
        $('#divPlayer').modal({
            backdrop: 'static',
            keyboard: false
        });

        $('#divPlayer').on('hidden', function (e) {

            ClearPlayerData();

        });
    }

    // Placeholder for older browser
    if (typeof ($('input[placeholder], textarea[placeholder]').placeholder) != undefined) {
        $('input[placeholder], textarea[placeholder]').placeholder();
    }
}

function OnClipLoadFail(XHR, Status, Error) {
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

        if (_VideoMetaData[0].VideoMetaData.Title120s != null) {
            $("#video-ptitle").html(_VideoMetaData[0].VideoMetaData.Title120s[0]);
            $(".video-clip-title").val(_VideoMetaData[0].VideoMetaData.Title120s[0]);
        }
        $(".video-aired").html("<label>Aired: </label>" + _VideoMetaData[0].VideoMetaData.IQ_Local_Air_Date);

        if (_VideoMetaData[0].VideoMetaData.StationCallSign != null) {
            $(".video-program").html(_VideoMetaData[0].VideoMetaData.StationCallSign + ' (' + _VideoMetaData[0].VideoMetaData.StationAffiliate + ') - ' + _VideoMetaData[0].VideoMetaData.IQ_Dma_Name);
        }
        else {
            $(".video-program").html(_VideoMetaData[0].VideoMetaData.IQ_Dma_Name);
        }

        if (_VideoMetaData[0].VideoMetaData.IQ_Dma_Num != null) {
            $(".video-rank").html(_VideoMetaData[0].VideoMetaData.IQ_Dma_Num);
        }
        else {
            $(".video-rank").html("NA");
        }

        GetNielsenData(_VideoMetaData[0].VideoMetaData.IQ_Dma_Num);
    }
    else {
        $(".video-views").html("NA");
        $(".video-value").html("NA");
        $(".video-rank").html('NA');
        $(".video-aired").html("<label>Aired: NA</label>");
        $(".video-program").html("Market: NA");
    }
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

            $(".video-views").html(nielsenAudience);

        }
        else {
            $(".video-views").html("NA");
        }

        if (nielsenMediaValue != null) {

            $(".video-value").html(nielsenMediaValue);

        }
        else {
            $(".video-value").html("NA");
        }
    }
    else {
        $(".video-views").html("NA");
        $(".video-value").html("NA");
    }

}

var OnFailNielsenData = function (jqXHR, textStatus, errorThrown) {
    //console.log(errorThrown);
}

var AddPlayerEvtListners = function () {

    var isMediaPaused = false;
    var isSeekKnobSelected = false;

    $("#ancVideoPlay").hover(function () {

        $("#ancVideoPlay > img").hide();

        if (_PlayState == 1) {

            $("#imgVideoPlayActive").show();
        }
        else { $("#imgVideoPauseActive").show(); }

    }, function () {
        $("#ancVideoPlay > img").hide();

        if (_PlayState == 1) {

            $("#imgVideoPlay").show();
        }
        else { $("#imgVideoPause").show(); }
    });

    $("#ancVideoFRew").hover(function () { $("#ancVideoFRew > img").hide(); $("#imgVideoFRewActive").show(); }, function () { $("#ancVideoFRew > img").hide(); $("#imgVideoFRew").show(); });

    $("#ancVideoRew").hover(function () { $("#ancVideoRew > img").hide(); $("#imgVideoRewActive").show(); }, function () { $("#ancVideoRew > img").hide(); $("#imgVideoRew").show(); });

    $("#ancVideoFwd").hover(function () { $("#ancVideoFwd > img").hide(); $("#imgVideoFwdActive").show(); }, function () { $("#ancVideoFwd > img").hide(); $("#imgVideoFwd").show(); });

    $("#ancVideoFFwd").hover(function () { $("#ancVideoFFwd > img").hide(); $("#imgVideoFFwdActive").show(); }, function () { $("#ancVideoFFwd > img").hide(); $("#imgVideoFFwd").show(); });

    $("#ancVideoPlay").click(function (e) { PlayPause(); e.stopPropagation(); $("#ancVideoPlay").blur(); });

    $("#ancVideoFwd").click(function () {
        _flash.externalFFFR(0, 3);
    });

    $("#ancVideoFFwd").click(function () {

        _flash.externalFFFR(0, 6);
    });

    $("#ancVideoRew").click(function () {

        _flash.externalFFFR(1, 3);
    });

    $("#ancVideoFRew").click(function () {

        _flash.externalFFFR(1, 6);
    });

    $(".video-fullscreen").hover(function () { $(".video-fullscreen img").hide(); $("#imgVideoFullScreenActive").show(); }, function () { $(".video-fullscreen img").hide(); $("#imgVideoFullScreen").show(); });

    $(".video-progress-bar").mouseout(function () {
        $('.video-slider-time').addClass("hide");
    });

    $(".video-progress-bar").click(function (e1) {

        _PreviousPlayState = _PlayState;

        _flash.externalPlay(1);

        var innerPointX = (e1.pageX - $(this).offset().left);
        var intWidth = (_Width - 2);
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

        $('.video-slider-time').html(_SeekSecond.toHHMMSS());

        var progTime = ((_SeekSecond * intWidth) / _TotalTime);
        var progTimelength = ((progTime * 99.6316758747698) / intWidth);

        $('.video-play-bar').css({ 'width': progTimelength + "%" });

        $('.video-slider-time').css({ 'left': innerPointX });
        $('.video-slider-time').removeClass("hide");

        //console.log("Seek...video-progress-bar");
        //_flash.setSeekPoint(_Start + _SeekSecond);
        setSeekPoint(_Start + _SeekSecond);

        SetPlayState();
    });

    $(".video-progress-bar").mousemove(function (ev) {

        if (!isMediaPaused && isSeekKnobSelected) {

            _flash.externalPlay(1);

            isMediaPaused = true;
        }

        var innerPointX = (ev.pageX - $(this).offset().left);
        var intWidth = (_Width - 2);
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

        $('.video-slider-time').html(_SeekSecond.toHHMMSS());

        if (isSeekKnobSelected) {

            var progTime = ((_SeekSecond * intWidth) / _TotalTime);
            var progTimelength = ((progTime * 99.6316758747698) / intWidth);

            $('.video-play-bar').css({ 'width': progTimelength + "%" });
        }

        $('.video-slider-time').css({ 'left': innerPointX });
        $('.video-slider-time').removeClass("hide");

    });

    $(".video-slider-knob").mousedown(function (event) {

        isSeekKnobSelected = true;

        $(document).mouseup(function (event) {

            isSeekKnobSelected = false;

            $(".video-progress-bar").unbind("mousemove");
            $(document).unbind("mouseup");
            $('.video-slider-time').addClass("hide");

            //console.log("Seek...video-slider-knob.mousedown");
            //_flash.setSeekPoint(_SeekSecond);
            setSeekPoint(_SeekSecond);

            SetPlayState();
        });


    });


    $(".video-chart").click(function () {
        $(".video-clip-row").hide();
        $(".video-player-controls-overlay").remove();
        var v = $(".video-chart-row").is(":visible");

        $(".video-chart-row").slideToggle();
        $(window).resize();

        if (v) {
            $(".video-chart").parent("li").removeClass("video-active");
        }
        else {
            $(".video-share-main").css('display', 'none');
            $(".video-chart").parent("li").siblings().removeClass("video-active");
            $(".video-chart").parent("li").addClass("video-active");
        }
    });

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

    $("#video-source").attr("src", logo);
}

var SetTimings = function (start, stop) {
    _Start = start;
    _Stop = stop;

    _TotalTime = _Stop - _Start;

    $(".video-duration").html(_TotalTime.toHHMMSS());
}

var UpdateTimeDuration = function (currentTime) {

    _currenttime = currentTime;
    _currentTimeInt = parseInt(currentTime);

    if (_firstCall) {
        _firstCall = false;

        $(".video-player-controls-overlay").remove();

        $(".video-fullscreen").click(function (e) {
            SetFullScreen();
        });

        _flash = document.getElementById('HUY');

        if (_flash != null) {

            AddPlayerEvtListners();
            ClippingEvts();
        }

    }

    var intWidth = (_Width - 2);

    var progTime = ((currentTime * intWidth) / _TotalTime);
    var progTimelength = ((progTime * 100) / _Width);
    $(".video-current-time").html(currentTime.toHHMMSS());
    $(".video-play-bar").css("width", progTimelength + "%");

    try {
        if ((_currentTimeInt % 60) == 0 && _currentTimeInt != _EvtCallbackProcessedTime) {
            //console.log('..sync player' + _currentTimeInt);
            _EvtCallbackProcessedTime = _currentTimeInt;
        }
    } catch (e) {

    }

}

var UpdatePlayerState = function (state) {

    $(".video-buffering").hide();

    switch (state) {
        case -1:
            break;
        case 0:
            _PlayState = 0;
            /*
            $("#play").removeClass("video-play");
            $("#play").addClass("video-pause");
            */
            $("#ancVideoPlay>img").hide();
            $("#ancVideoPlay").blur();
            $("#imgVideoPause").show();
            $(".video-preview").css("background-image", "url('../images/Video-Player/preview-pause.png'), linear-gradient(to bottom, rgb(255, 255, 255) 0%, rgb(233, 233, 233) 100%)");

            break;
        case 1:
            _PlayState = 1;
            /*
            $("#play").removeClass("video-pause");
            $("#play").addClass("video-play");
            */
            $("#ancVideoPlay>img").hide();
            $("#ancVideoPlay").blur();
            $("#imgVideoPlay").show();
            $(".video-preview").css("background-image", "url('../images/Video-Player/preview.png'), linear-gradient(to bottom, rgb(255, 255, 255) 0%, rgb(233, 233, 233) 100%)");

            break;
        case 2:

            if (_firstCall === false) {
            }
            /*
            $("#play").removeClass("video-pause");
            $("#play").addClass("video-play");
            */
            $("#ancVideoPlay>img").hide();
            $("#ancVideoPlay").blur();
            $("#imgVideoPlay").show();
            $(".video-preview").css("background-image", "url('../images/Video-Player/preview.png'), linear-gradient(to bottom, rgb(255, 255, 255) 0%, rgb(233, 233, 233) 100%)");

            break;
        case 3:
            $(".video-buffering").show();
            break;
        case 4:
            break;
    }
}

var ClearPlayerData = function () {

    if ($(".video-video") != null || $(".video-video") != undefined) {
        $(".video-video").html('');
    }

    _PreviousPlayState = 0;
    _PlayState = 0;
    _VideoMetaData = "";
    _Start = 0;
    _Stop = 0;
    _Width = 545;
    _Height = 312;
    _Vol = 0;
    _SeekSecond = 0;
    _ID = null;
    _firstCall = false;
    _currenttime = 0;
    _currentTimeInt = 0;
    _processedTitle120 = -1;
    _programTitle = "NA";
    _nielsenData = null;
    _categoryData = null;
    _clipID = null;
    _flash = null;
    _Player = "";

    if ($('#divPlayer').parent().is('div')) {
        $('#divPlayer').parent().remove();
    }
    else {
        $('#divPlayer').remove();
    }

    $("#imgFPlay").remove();
    $("#imgFPause").remove();
}

/*______________________________________________________ Chart ______________________________________________________________________________________________*/


function OnGetChartComplete(result) {
    if (result.isSuccess) {
        if (result.isTimeSync && result.lineChartJson.length > 0) {
            $('.video-chart').closest('li').show();
            $('.chart-tabs').html('');
            $('.chart-tab-content').html('');

            $.each(result.lineChartJson, function (index, obj) {
                $('.chart-tabs').append('<div class="chartTabHeader" id="video-parent-tab-' + index + '"><div class="padding5" id="video-tab-' + index + '" onclick="changeChartTab(' + index + ');">' + obj.Type + '</div></div>');
                $('.chart-tab-content').append('<div class="float-left" id="video-chart-' + index + '" style="width:100%;"></div>');
                RenderHighCharts(obj.Data, 'video-chart-' + index);
            });

            changeChartTab(0);
            $(".chart-tab-content").on("mouseout", function () {
                _IsManualHover = false;
            });
        }
        else {
            $('.chart-tab-content').append('<div clas="help-inline" style="color:#ff0000;text-align: center; padding: 15px 0px;">' + _msgNoTimeSyncData + '</div>');
        }
        $(".highcharts-container").css('left', '25px')
    }
    else {
        ShowNotification(_msgErrorOccured);
    }
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
    //console.log("chart hover out");
}

function ChartHoverManage() {
    _IsManualHover = true;
    //console.log("chart hover");
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

function ShowHideTimeSync() {
    if ($(".video-chart-row").is(':visible')) {
        $("#divShowHideTimeSync").attr("title", "Hide Time Snyc");
        $("#imgSync").attr("src", "../images/show.png");
        $(".video-chart-row").hide('slow');
    }
    else {
        $("#divShowHideTimeSync").attr("title", "Show Time Sync");
        $("#imgSync").attr("src", "../images/hiden.png");
        $(".video-chart-row").show('slow');
    }
}

/*______________________________________________________ || Chart ______________________________________________________________________________________________*/

/*______________________________________________________  Email ______________________________________________________________________________________________*/

function CancelEmail() {
    $("#divEmailForm").slideUp(1000)
    $("#txtPlayerToEmail").val('');
    $("#txtPlayerSubject").val('');
    $("#txtPlayerMessage").val('');
    $("#txtPlayerFromEmail").css({ 'border': '1px solid #212121' });
    $("#txtPlayerToEmail").css({ 'border': '1px solid #212121' });
    $("#txtPlayerSubject").css({ 'border': '1px solid #212121' });
    $("#txtPlayerMessage").css({ 'border': '1px solid #212121' });
    $("#spanEmailMessage").hide().html("");
}

function SendPlayerEmail() {

    $("#txtPlayerFromEmail").bind("animationend webkitAnimationEnd oAnimationEnd MSAnimationEnd", function () {
        $(this).removeClass("video-input-error");
    });

    $("#txtPlayerToEmail").bind("animationend webkitAnimationEnd oAnimationEnd MSAnimationEnd", function () {
        $(this).removeClass("video-input-error");
    });

    $("#txtPlayerSubject").bind("animationend webkitAnimationEnd oAnimationEnd MSAnimationEnd", function () {
        $(this).removeClass("video-input-error");
    });

    $("#txtPlayerMessage").bind("animationend webkitAnimationEnd oAnimationEnd MSAnimationEnd", function () {
        $(this).removeClass("video-input-error");
    });

    if (ValidatePlayerSendEmail()) {
        $("#btnSend").attr("disabled", true);
        $(".cancelButton").attr("disabled", true);
        $("#spanEmailMessage").show().html("Sending...");
        var _ImagePath = _IQArchieveTVThumbUrl.replace('amp;', '') + _ID;
        var jsonPostData = {
            From: $("#txtPlayerFromEmail").val(),
            To: $("#txtPlayerToEmail").val(),
            Subject: $("#txtPlayerSubject").val(),
            Body: $("#txtPlayerMessage").val(),
            _imagePath: _ImagePath,
            FileName: _FileName,
            FileID: _ID,
            PageName: ''
        }

        if (!_IEVB10) {

            $.ajax({
                type: "post",
                dataType: "json",
                url: _urlSendEmail,
                global: false,
                contentType: "application/json; charset=utf-8",
                xhrFields: {
                    withCredentials: true
                },
                data: JSON.stringify(jsonPostData),
                success: function (result) {
                    if (result == "0") {
                        $("#btnSend").removeAttr("disabled");
                        $(".cancelButton").removeAttr("disabled");
                        $("#spanEmailMessage").show().html("Email sent successfully to " + _EmailCount + " person(s)").delay('1000').fadeOut(1000);
                        _EmailCount = 0;
                        $("#txtPlayerToEmail").val('');
                        $("#txtPlayerSubject").val('');
                        $("#txtPlayerMessage").val('');
                    }
                    else {
                        $("#btnSend").removeAttr("disabled");
                        $(".cancelButton").removeAttr("disabled");
                        $("#spanEmailMessage").show().html(_msgSomeErrorProcessing).delay(2000).delay('1000').fadeOut(1000);
                    }
                },
                error: function (a, b, c) {
                    $("#btnSend").removeAttr("disabled");
                    $(".cancelButton").removeAttr("disabled");
                    $("#spanEmailMessage").show().html(_msgSomeErrorProcessing).delay(2000).delay('1000').fadeOut(1000);
                }
            });
        }
        else {

            $.ajax({
                type: "post",
                dataType: "json",
                url: _urlSendMailWoXd,
                global: false,
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(jsonPostData),
                success: function (result) {
                    if (result == "0") {
                        $("#btnSend").removeAttr("disabled");
                        $(".cancelButton").removeAttr("disabled");
                        $("#spanEmailMessage").show().html("Email sent successfully to " + _EmailCount + " person(s)").delay('1000').fadeOut(1000);
                        _EmailCount = 0;
                        $("#txtPlayerToEmail").val('');
                        $("#txtPlayerSubject").val('');
                        $("#txtPlayerMessage").val('');
                    }
                    else {
                        $("#btnSend").removeAttr("disabled");
                        $(".cancelButton").removeAttr("disabled");
                        $("#spanEmailMessage").show().html(_msgSomeErrorProcessing).delay(2000).delay('1000').fadeOut(1000);
                    }
                },
                error: function (a, b, c) {
                    $("#btnSend").removeAttr("disabled");
                    $(".cancelButton").removeAttr("disabled");
                    $("#spanEmailMessage").show().html(_msgSomeErrorProcessing).delay(2000).delay('1000').fadeOut(1000);
                }
            });

        }
    }
}

function CancelEmbed() {
    $("#divEmbedForm").slideUp(1000)
}

function CancelLink() {
    $("#divLink").slideUp(1000)
}

function CancelWordPress() {
    $("#divWordPressForm").slideUp(1000)
}

function CopyEmbed() {
    $("#txtEmbed").select();
    $("#txtEmbed").css('color', '#fff');
    $("#txtEmbed").css('background-color', '#c0c0c0');
}

function CopyLink() {
    $("#txtLink").select();
    $("#txtLink").css('color', '#fff');
    $("#txtLink").css('background-color', '#c0c0c0');
}

function CopyWordPress() {
    $("#txtWordPress").select();
    $("#txtWordPress").css('color', '#fff');
    $("#txtWordPress").css('background-color', '#c0c0c0');
}

function ValidatePlayerSendEmail() {
    var isValid = true;

    if ($("#txtPlayerFromEmail").val() == "") {
        $("#txtPlayerFromEmail").addClass("video-input-error")
        $("#txtPlayerFromEmail").css({ 'border': '1px solid #CB1C1C' });
        isValid = false;
    }
    else if (!CheckEmailAddress($("#txtPlayerFromEmail").val())) {
        $("#txtPlayerFromEmail").addClass("video-input-error")
        $("#txtPlayerFromEmail").css({ 'border': '1px solid #ff0000' });
        ShowNotification(_msgIncorrectEmail);
        isValid = false;
    }
    else {
        $("#txtPlayerFromEmail").css({ 'border': '1px solid #212121' });
    }

    if ($("#txtPlayerToEmail").val() == "") {
        $("#txtPlayerToEmail").addClass("video-input-error")
        $("#txtPlayerToEmail").css({ 'border': '1px solid #CB1C1C' });
        isValid = false;
    }
    else if ($("#txtPlayerToEmail").val() != "") {
        $("#txtPlayerToEmail").css({ 'border': '1px solid #212121' });
        var Toemail = $("#txtPlayerToEmail").val();
        if (Toemail.substr(Toemail.length - 1) == ";") {
            Toemail = Toemail.slice(0, -1);
        }
        _EmailCount = Toemail.split(';').length;
        $(Toemail.split(';')).each(function (index, value) {
            if (value.indexOf(',') > 0) {
                $("#txtPlayerToEmail").addClass("video-input-error")
                $("#txtPlayerToEmail").css({ 'border': '1px solid #CB1C1C' });
                ShowNotification(_msgEmailNotCommaSeprated);
                isValid = false;
                return;

            }
            else {
                if (!CheckEmailAddress($.trim(value))) {
                    $("#txtPlayerToEmail").addClass("video-input-error")
                    $("#txtPlayerToEmail").css({ 'border': '1px solid #CB1C1C' });
                    ShowNotification(_msgOneEmailAddressInCorrect);
                    isValid = false;
                    return;
                }
            }

        });
    }

    if ($("#txtPlayerSubject").val() == "") {
        $("#txtPlayerSubject").addClass("video-input-error")
        $("#txtPlayerSubject").css({ 'border': '1px solid #CB1C1C' });
        isValid = false;
    }
    else {
        $("#txtPlayerSubject").css({ 'border': '1px solid #212121' });
    }

    if ($("#txtPlayerMessage").val() == "") {
        $("#txtPlayerMessage").addClass("video-input-error")
        $("#txtPlayerMessage").css({ 'border': '1px solid #CB1C1C' });
        isValid = false;
    }
    else {
        $("#txtPlayerMessage").css({ 'border': '1px solid #212121' });
    }



    return isValid;
}

function CheckEmailAddress(email) {
    //var emailPattern = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
    var emailPattern = /^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+$/;
    return emailPattern.test(email);
}

/*______________________________________________________ || Email ______________________________________________________________________________________________*/

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
        var diffHeight = GetDiffHeight(height);

        var capInfoWidth = GetCaptionInfoWidth(width);

        var divWidth = GetTotalWidth(width);
        var divHeight = $(".video-about-clip").height() + diffHeight + 14;

        var dimension = AdjustDimension(divWidth, divHeight, width, height);

        width = dimension.width;
        height = dimension.height;

        diffHeight = GetDiffHeight(height);
        capInfoWidth = GetCaptionInfoWidth(width);

        divWidth = GetTotalWidth(width);

        var capHeight = $(".video-caption").height();
        var capContentHeight = $(".video-caption-content > div").height();
        var aboutHeight = $(".video-about-clip").height();

        $("#HUY").attr("width", width);
        $("#HUY").attr("height", height);

        $("#HUY").css({ "width": width, "height": height });
        $(".video-caption-info").css("width", capInfoWidth);
        $(".video-share-left").css("width", capInfoWidth);
        $(".video-share-right").css("width", (divWidth - capInfoWidth));

        $(".video-video").animate({ height: height }, 1500, function () { });
        $(".video-video").css("width", width);
        $(".video-caption-content > div").animate({ height: (capContentHeight + diffHeight) }, 1500);
        $(".video-caption").animate({ height: (capHeight + diffHeight) }, 1500);
        $(".video-about-clip").animate({ height: (aboutHeight + diffHeight) }, 1500);
        /*
        $("#divPlayer").animate({ "max-width": divWidth }, 1500);
        $(".video-container-narrow").animate({ "max-width": divWidth }, 1500);
        */
        $("#divPlayer").css("max-width", divWidth);
        $(".video-container-narrow").css("max-width", divWidth);
        /*$('.chart-tab-content > div').css("width", GetChartWidth());
        $('.chart-tab-content > div > div').css("width", GetChartWidth());*/

        _Width = width;
        _Height = height;
    }
}


var GetDiffHeight = function (height) {

    var diffHeight = 0;

    if (height > _Height) {

        diffHeight = (height - _Height);
    }
    else {

        diffHeight = -(_Height - height);
    }

    return diffHeight;
}

var GetCaptionInfoWidth = function (videoWidth) {

    var capInfoWidth = (80 * videoWidth / 100);

    if (capInfoWidth > 387) {
        capInfoWidth = 387;
    }

    return capInfoWidth;
}

var GetTotalWidth = function (videoWidth) {

    var capInfoWidth = GetCaptionInfoWidth(videoWidth);
    var totalWidth = videoWidth + capInfoWidth + $(".video-about-clip").width() + 4;
    return totalWidth;

}

var GetTotalInitialWidth = function (videoWidth) {

    var capInfoWidth = GetCaptionInfoWidth(videoWidth);
    var totalWidth = videoWidth + capInfoWidth + 84 + 4;
    return totalWidth;

}

var AdjustDimension = function (totalWidth, totalHeight, playerWidth, playerHeight) {

    var winWidth = $(window).width();
    var winHeight = $(window).height();

    var calPlayerWidth = playerWidth;
    var calPlayerHeight = playerHeight;

    if (winWidth > 980) {

        winWidth = winWidth - 50;

        if (winWidth < totalWidth || winHeight < totalHeight) {

            var isFit = false;

            if (winWidth < totalWidth) {
                while (!isFit) {

                    calPlayerWidth = calPlayerWidth - 20;

                    if (calPlayerWidth < 400) {

                        calPlayerWidth = 400;
                        calPlayerHeight = (calPlayerWidth * playerHeight / playerWidth);

                        break;
                    }

                    var tmpTotalWidth = GetTotalWidth(calPlayerWidth);

                    if (winWidth < tmpTotalWidth) {

                        continue;
                    }
                    else {

                        isFit = true;
                        calPlayerHeight = (calPlayerWidth * playerHeight / playerWidth);
                        break;
                    }
                }
            }

            isFit = false;
            var calTotalHeight = totalHeight - (playerHeight - calPlayerHeight);

            if (winHeight < calTotalHeight) {

                while (!isFit) {

                    calPlayerHeight = calPlayerHeight - 20;

                    if (calPlayerHeight < 300) {

                        calPlayerHeight = 300;
                        calPlayerWidth = (calPlayerHeight * playerWidth / playerHeight);

                        break;
                    }

                    var tmpTotalHeight = totalHeight - (playerHeight - calPlayerHeight);

                    if (winHeight < tmpTotalHeight) {

                        continue;
                    }
                    else {

                        isFit = true;
                        calPlayerWidth = (calPlayerHeight * playerWidth / playerHeight);
                        break;
                    }
                }
            }

            if (calPlayerWidth < 400) {
                calPlayerWidth = 400;
            }

            if (calPlayerHeight < 300) {
                calPlayerHeight = 300;
            }

        }
    }

    return { "width": calPlayerWidth, "height": calPlayerHeight };
}

var AdjustInitialDimension = function (totalWidth, playerWidth) {

    var winWidth = $(window).width();
    var calPlayerWidth = playerWidth;

    if (winWidth > 980 && winWidth < totalWidth) {

        winWidth = winWidth - 50;
        var isFit = false;

        while (!isFit) {

            calPlayerWidth = calPlayerWidth - 10;

            if (calPlayerWidth < 400) {

                calPlayerWidth = 400;
                break;
            }

            var tmpTotalWidth = GetTotalInitialWidth(calPlayerWidth);

            if (winWidth < tmpTotalWidth) {

                continue;
            }
            else {

                isFit = true;
                break;
            }
        }
    }

    return calPlayerWidth;
}