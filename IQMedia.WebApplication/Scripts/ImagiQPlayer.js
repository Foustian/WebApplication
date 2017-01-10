var _PreviousPlayState = 0;
var _PlayState = 0;
var _VideoMetaData = "";
var _Start = 0;
var _Stop = 0;
var _Width = 545;
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
var _ISFScr = false;

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

var ChangeTabPlayer = function (tabindex) {

    $('#divCaptionHeader div').each(function () {
        $(this).removeClass("active");
    });

    /*$('#divCapContent > div').each(function () {
    $(this).hide();
    });*/

    $("#divCapHighlightContent").hide();
    $("#divCapAllContent").hide();
    $("#divCapDescriptionContent").hide();

    $('#divCaptionHeader').children().eq(tabindex).attr("class", "active");

    /*
    $('#divCapContent').children().eq(tabindex).show();
    */

    if (tabindex == 0) {
        $("#divCapHighlightContent").show();
    }
    if (tabindex == 1) {
        $("#divCapDescriptionContent").show();
    }
    else if (tabindex == 2) {
        $("#divCapAllContent").show();
    }
}

var PlayPause = function () {

    if (_PlayState == 0) {
        _PlayState = 1;
        if (_flash != null) {
            _flash.externalPlay(_PlayState);
        }
        /* $("#play").removeClass("video-pause");
        $("#play").addClass("video-play");
        */
    }
    else {
        _PlayState = 0;
        if (_flash != null) {
            _flash.externalPlay(_PlayState);
        }
        /*$("#play").removeClass("video-play");
        $("#play").addClass("video-pause");
        */
    }
}

var SetPlayState = function () {

    _flash.externalPlay(_PreviousPlayState);
}

var LoadPlayerbyAgentID = function (iqagentID) {

    videotype = "rawmedia";

    var jsonPostData = {
        iqagentTVResultID: iqagentID
    }

    $.when($.ajax({

        type: 'POST',
        dataType: 'json',
        url: _urlFeedsLoadPlayer,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),

        success: LoadNPlayer,
        error: OnFail
    })
    ).then(function () {

        $.when($.ajax({

            type: 'GET',
            dataType: 'jsonp',
            url: _urlVideoMetaData + _agentGuid + "&Type=" + videotype,
            contentType: 'application/json; charset=utf-8',
            success: OnParseMetaData,
            error: OnFailMetaData
        })
        ).then(function () {
            SetPlayerMetaData();
        });

    });

    //    $.ajax({

    //        type: 'GET',
    //        dataType: 'jsonp',
    //        url: _urlVideoCategory,
    //        contentType: 'application/json; charset=utf-8',
    //        success: OnParseCategoryData,
    //        error: OnFailCategoryData
    //    });

    if (_categoryData == null) {
        $.ajax({

            type: 'GET',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            url: _urlVideoCategory,
            xhrFields: {
                withCredentials: true
            },
            success: OnParseCategoryData,
            error: OnFailCategoryData,
            crossDomain: true

        });
    }
}

var LoadPlayerbyGuidTS = function (itemGuid, title120, searchTerm, isUGC) {

    var _Title120 = null;
    if (typeof (title120) === 'undefined') {
        _Title120 = null;
    }
    else {
        _Title120 = title120;
    }


    if (typeof (searchTerm) !== 'undefined' && searchTerm != "" && searchTerm != null) {
        _SearchTerm = searchTerm;
    }

    videotype = 'rawmedia'
    if (typeof (isUGC) !== 'undefined' && isUGC) {
        videotype = 'ugc'
    }


    _ID = itemGuid;

    var jsonPostData = {
        p_ItemGuid: itemGuid,
        p_SearchTerm: _SearchTerm,
        p_Title120: _Title120,
        p_IsOptiQ: true
    }

    $.when($.ajax({

        type: 'GET',
        dataType: 'jsonp',
        url: _urlVideoMetaData + itemGuid + "&Type=" + videotype,
        contentType: 'application/json; charset=utf-8',
        success: OnParseMetaData,
        error: OnFailMetaData
    }),
    $.ajax({

        type: 'POST',
        dataType: 'json',
        url: _urlCommonLoadBasicPlayerByGuidnSearchTerm,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),

        success: LoadNPlayer,
        error: OnFail
    })
    ).then(function () {

        SetPlayerMetaData();

    });

    //    $.ajax({

    //        type: 'GET',
    //        dataType: 'jsonp',
    //        url: _urlVideoCategory,
    //        contentType: 'application/json; charset=utf-8',
    //        success: OnParseCategoryData,
    //        error: OnFailCategoryData
    //    });

    if (_categoryData == null) {
        $.ajax({

            type: 'GET',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            url: _urlVideoCategory,
            xhrFields: {
                withCredentials: true
            },
            success: OnParseCategoryData,
            error: OnFailCategoryData,
            crossDomain: true
        });
    }
}

var LoadNPlayer = function (result) {
    if (result.videoGuid != null && result.videoGuid != undefined) {
        _agentGuid = result.videoGuid;
        _ID = result.videoGuid;
    }
    var rawPlayer1 = '<div class="container-narrow modalPlayer fade hide resizable select-none" id="divPlayer"><div class="player-row-one"><div class="caption-info fleft"><div class="caption"><img width="387" height="330" alt="Caption" src="images/Video-Player/caption.jpg"></div><div class="info"><img width="57" height="54" class="fleft" alt="Fox" src="images/Video-Player/fox.jpg">        <div class="video-info fleft">    	<h4>KCRA NBC3 Sacramento</h4>        <span class="program">Sacramento Stkton-Modesto</span>        <span class="aired"><label>Aired:</label>2012-04-30 11:00 PM</span>    </div>        <div class="stats fright">    	<span class="views">75,532</span>        <span class="value">45,139</span>        <span class="rank">020</span>    </div></div>  </div><div class="player fleft"><div class="video"><img alt="video" src="images/Video-Player/video.jpg"></div><div class="player-controls">	<div class="seek">    	<span class="active" onclick="Seek(0);">00:00</span>        <span onclick="Seek(10);">10:00</span>        <span onclick="Seek(20);">20:00</span>        <span onclick="Seek(30);">30:00</span>        <span onclick="Seek(40);">40:00</span>        <span onclick="Seek(50);">50:00</span>      <div class="clear"></div>    </div>        <div class="progress-bar">    	<div class="current-time">1:16</div>    <div class="duration">5:44</div>      	<div style="width:20%;" class="play-bar"></div>        <div style="left:17.5%;" class="seek-knob"><span></span></div>    </div><div class="control-holder">   	  <ul class="controls control-holder">        	<li class="fast-backward"></li>  <li class="backward"></li>      <li class="play"></li>  <li class="forward"></li>  <li class="fast-forward"></li>    </ul>      <div class="volume control-holder"><img width="186" height="27" alt="volume" src="images/Video-Player/volume.png"></div>      <div class="control-holder fleft"><span class="fullscreen"></span></div>    </div></div>      </div><div class="about-clip fleft">    		<ul class="action">	<li><a class="about" href="#">About</a></li>    <li class="active"><a class="make-a-clip" href="#">Make a clip</a></li></ul>    </div>     <div class="clear"></div>        </div>        <div class="clip-row"><div class="form fleft">    	<input type="text" placeholder="Clip Title" class="clip-title">      <input type="text" placeholder="Keywords" class="keywords">      <input type="text" placeholder="Category" class="category">      <textarea placeholder="Description" class="textarea">        </textarea>        </div>  <div class="clip-controls fleft">      	<div class="preview fleft"><img width="9" height="11" alt="Preview" src="images/Video-Player/preview.png"></div><br> <br><div class="thumb fleft"><img width="15" height="9" alt="Make Thumbnail" src="images/Video-Player/thumb.png"></div><div class="clear"></div><br>        <div class="clip-holder">	<div style="margin-left:25px;" class="dragger">         	  	<img width="11" height="16" class="left" alt="Drag Left" src="images/Video-Player/drag-left.png">    <img width="11" height="16" class="right" alt="Drag Right" src="images/Video-Player/drag-right.png">  	</div><br><img width="12" height="21" src="images/Video-Player/knob.png"></div>    </div>  <div class="button-thumb fleft">  	<div class="action-buttons"><input type="submit" class="cancel" value="Cancel"><input type="submit" class="save" value="Save"></div>     		 <img width="166" height="100" class="thumbnail" alt="Thumbnail" src="images/Video-Player/thumbnail.jpg">        </div></div>  </div>';
    var rawPlayer = '<div class="video-modalPlayer hide resizable select-none" id="divPlayer">'
        + '<div class="video-close">X</div>'
            + '<div class="video-container-narrow">'
                + '<div class="video-player-row-one">'
                    + '<div class="video-caption-info video-fleft">'
                        + '<div class="video-caption">'
                            + '<div id="divCaptionHeader" class="video-caption-tab">'
                                + '<div align="center" id="divHighlightTab" style="cursor:pointer;min-width:50px;" onclick="ChangeTabPlayer(0)">Highlights</div>'
                                + '<div align="center" id="divDescriptionTab" style="cursor:pointer;min-width:50px;" onclick="ChangeTabPlayer(1)">Description</div>'
		                        + '<div align="center" id="divCaptionTab" style="cursor:pointer;min-width:50px;" onclick="ChangeTabPlayer(2)" class="active">Captions</div>'
                            + '</div>'
                            + '<div id="divCapContent" class="video-caption-content">'
                                    + '<div id="divCapHighlightContent" style="width:auto;display:none;" class="select-text" ></div>'
                                    + '<div id="divCapDescriptionContent" style="width:auto;display:none;"  class="select-text"></div>'
                                    + '<div id="divCapAllContent" style="width:auto;display:none;"  class="select-text"></div>'
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
                    + '</div>'
                    + '<div class="video-player video-fleft">'
                        + '<div class="video-video">'
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
                            + '<div class="video-seek">'
                                + '<span class="video-active" onclick="Seek(0,this);">00:00</span>'
                                + '<span onclick="Seek(10,this);">10:00</span>'
                                + '<span onclick="Seek(20,this);">20:00</span>'
                                + '<span onclick="Seek(30,this);">30:00</span>'
                                + '<span onclick="Seek(40,this);">40:00</span>'
                                + '<span onclick="Seek(50,this);">50:00</span>'
                                + '<div class="video-clear"></div>'
                            + '</div>'
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
                            + '<div class="video-control-holder">'
                                + '<ul class="video-controls video-control-holder">'
                                    + '<li class="video-fast-backward"></li>'
                                    + '<li class="video-backward"></li>'
                                    + '<li class="video-pause" id="play" onclick="PlayPause();"></li>'
                                    + '<li class="video-forward"></li>'
                                    + '<li class="video-fast-forward"></li>'
                                + '</ul>'
                                + '<div class="video-volume video-control-holder">'
                                    + '<div class="video-vol-mute">&nbsp;</div>'
                                    + '<div id="video-vol-slider"></div>'
                                    + '<div class="video-vol-marker">&nbsp;</div>'
                                + '</div>'
                                + '<div class="video-control-holder video-fleft"><span class="video-fullscreen"></span>'
                                + '</div>'
                            + '</div>'
                            + '<div class="video-player-controls-overlay"></div>'
                            + '</div>'
                        + '</div>'
                        + '<div class="video-about-clip video-fleft">'
                            + '<ul class="video-action">'
                            + '<li><a class="video-about" href="#">About</a></li>'
                            + '<li><a class="video-make-a-clip" href="#">Make a clip</a></li>'
                            + '<li style="display:none;" ><a class="video-chart" href="#">Time Sync</a></li>'
                            + '</ul>'
                        + '</div>'
                        + '<div class="video-clear">'
                        + '</div>'
                    + '</div>'
                    + '<div class="video-logo-chart-row" style="">'
                        + '<div class="margintop10">'
                            + '<div class="clear logo-chart-content" style="margin:0 auto;">'
                            + '</div>'
                        + '</div>'
                    + '</div>'
                    + '<div class="video-chart-row" style="display:none;">'
                        + '<div class="margintop10">'
                        + '<div class="clear">'
                            + '<div class="chart-tabs" >'
                            + '</div>'
                        + '</div>'
                        + '<div class="clear chart-tab-content">'
                        + '</div>'
                    + '</div>'
                + '</div>'
                + '<div id="divMkClip" class="video-clip-row" style="display:none;">'
                + '<div class="video-form video-fleft">'
                + '<input type="text" placeholder="Clip Title" class="video-clip-title">'
                + '<input type="text" placeholder="Keywords" class="video-keywords">'
                + '<Select class="video-category"></Select>'
                + '<textarea placeholder="Description" class="video-textarea"></textarea>'
                + '</div>'
                + '<div class="video-clip-controls video-fleft">'
                    + '<div class="video-clip-controls-overlay" style="display:none;"></div>'
                    + '<div id="divMinTimeDragger" style="height:56px;padding-top:32px;">'
                    + '<div class="video-clip-holder">'
                            + '<div style="position:absolute;top:18px;" id="divClipStart">'
                                + '<input type="text" id="txtClipStart" style="width:32px;color:green;font-weight:bold;font-size:12px;" value="10:00">'
                            + '</div>'
                            + '<div style="position: absolute; left: 20%;" class="video-dragger">'
		                        + '<div style="left: -10px; height:14px; float: left;position: absolute;cursor:pointer;" id="divStartDe">'
			                        + '<img src="../images/Video-Player/drag-left.png" alt="Drag Left" class="video-left">'
		                        + '</div>'
		                        + '<div style="left: 7px;height:14px;position: absolute;cursor:pointer;"  id="divStartIn">'
			                        + '<img src="../images/Video-Player/drag-right.png" alt="Drag Right" class="video-right">'
		                        + '</div>'
		                        + '<div style="position: absolute;height:14px; right: 7px;cursor:pointer;"  id="divEndDe">'
			                        + '<img src="../images/Video-Player/drag-left-white.png" alt="Drag Left" class="video-left">'
		                        + '</div>'
		                        + '<div style="position: absolute;height:14px;right: -10px;cursor:pointer;"  id="divEndIn">'
			                        + '<img src="../images/Video-Player/drag-right-black.png" alt="Drag Right" class="video-right">'
		                        + '</div>'
                                + '<div style="position: absolute; top: -32px;left:-16px;" id="divPreview">'
                                    + '<div class="video-preview video-fleft">'
                //                            + '<img width="9" height="11" alt="Preview" src="../images/Video-Player/preview.png">'
                                    + '</div>'
                                    + '<div align="center" style="border: 1px solid rgb(0, 0, 0); width: 1px; -moz-box-sizing: border-box; clear: both; margin: 0px auto; height: 12px;">'
                                        + '&nbsp;'
                                    + '</div>'
                                    + '<div class="video-preview-time">10:00</div>'
                                + '</div>'
                                + '<div style="position:absolute;top:15px;left:-16px;" id="divThumb">'
                                    + '<div style="width: 1px; clear: both; height: 12px; border: 1px solid rgb(0, 0, 0); -moz-box-sizing: border-box; margin: 0px auto;">'
                                        + '&nbsp;'
                                    + '</div>'
                                    + '<div class="video-thumb video-fleft">'
                                        + '<div style="left: -10px; float: left;position: absolute;cursor:pointer;" id="divThumbDe">'
			                                + '<img src="../images/Video-Player/drag-left.png" alt="Drag Left" style="margin:0 0;">'
		                                + '</div>'
                                        + '<div style="position: absolute;right: -10px;cursor:pointer;"  id="divThumbIn">'
			                                + '<img src="../images/Video-Player/drag-right-black.png" alt="Drag Right" style="margin:0 0;">'
		                                + '</div>'
                //                        + '<img width="15" height="9" alt="Make Thumbnail" src="../images/Video-Player/thumb.png">'
                                    + '</div>'
                                    + '<div id="thumbOffset" style="font-size:smaller;">'
                                        + '10:01'
                                    + '</div>'
                                + '</div>'
	                        + '</div>'
                            + '<div style="position:absolute;top:18px;right:0px;" id="divClipEnd">'
                                + '<input type="text" id="txtClipEnd" style="width:32px;color:red;font-weight:bold;;font-size:12px;" value="20:00">'
                           + '</div>'
                        + '</div>'
                    + '</div>'
                    + '<div class="video-dragger-l">'
                        + '<div class="video-dragger-l-st"></div>'
                        + '<div class="video-dragger-l-lk"></div>'
                        + '<div class="video-dragger-l-rk"></div>'
                        + '<div class="video-dragger-l-et"></div>'
                    + '</div>'
                //    + '<div class="video-preview video-fleft"><img width="9" height="11" alt="Preview" src="../images/Video-Player/preview.png">'
                //    + '</div>'
                //    + '<br> <br>'
                //    + '<div class="video-thumb video-fleft"><img width="15" height="9" alt="Make Thumbnail" src="../images/Video-Player/thumb.png">'
                //    + '</div>'
                //    + '<div class="video-clear">'
                //    + '</div>'
                //    + '<br>'
                //    + '<div class="video-clip-holder">'
                //    + '<div style="margin-left:25px;" class="video-dragger">'
                //    + '<img width="11" height="16" class="video-left" alt="Drag Left" src="../images/Video-Player/drag-left.png">'
                //    + '<img width="11" height="16" class="video-right" alt="Drag Right" src="../images/Video-Player/drag-right.png">'
                //    + '</div>'
                //    + '<br>'
                //    + '<img width="12" height="21" src="../images/Video-Player/knob.png">'
                //    + '</div>'
                + '</div>'
                + '<div class="video-button-thumb video-fleft">'
                    + '<div class="video-action-buttons" align="right">'
                /*+ '<input type="submit" class="video-cancel" value="Cancel">'
                + '<input type="submit" class="video-save" value="Save" onclick="SetThumb();">'*/
                        + '<a id="ancClipCancel" href="javascript:void(0);"><img src="../../images/video-player/close.png" alt="cancel" /></a>'
                        + '<a id="ancClipSave" href="javascript:void(0);"><img src="../../images/video-player/save.png" alt="save" /></a>'
                    + '</div>'
                    + '<div class="video-thumbContainer">'
                        + '<div class="captureThumbOffset">'
                            + '<img alt="Capture" src="../../Images/video-player/camera.png">'
                        + '</div>'
                        + '<div style="clear: both; text-align: center; word-wrap: break-word; width: 169px;">Click to capture Thumbnail</div>'
                    + '</div>'
                + '</div>'
            + '</div>'
        + '</div>'
    + '</div>'
    + '<audio id="audioCapture" src="/Audio/camera_flick.mp3" style="display: none;"></audio>';

    $(document.body).append(rawPlayer);
    $(document.body).append("<img id='imgFPlay' src='/images/video-player/play_f.png' style='display:none;position:absolute;top:0;right:0;bottom:0;left:0;margin:auto;z-index:2147483647;' />");
    $(document.body).append("<img id='imgFPause' src='/images/video-player/pause_f.png' style='display:none;position:absolute;top:0;right:0;bottom:0;left:0;margin:auto;z-index:2147483647;' />");

    _firstCall = true;

    $("#video-vol-slider").slider({
        orientation: "horizontal",
        range: "min",
        min: 0,
        max: 100,
        value: 75,
        slide: UpdateVolume,
        change: UpdateVolume
    });
    $(".video-vol-marker").css("background-position", "-153px -168px");

    $(".video-close").click(function (e) {

        if ($(".video-video") != null || $(".video-video") != undefined) {
            $(".video-video").html('');
        }

        $('#divPlayer').css({ "display": "none" });
        $('#divPlayer').modal('hide');
        $('#divPlayer').remove();

        ClearPlayerData();
    });


    $("#txtClipStart").blur(function () {
        var time = hmsToSecondsOnly($(this).val());
        if (time != _clipStart) {
            if (time <= _clipEnd && CheckClipDuration(time, true)) {
                _clipStart = time;
                SetThumbOffsetToStart();
                UpdateLKnob();
                SetPreviewTimeToStart();
                SeekThumb(_clipThumbOffset);
            }
            else {
                $("#txtClipStart").val(_clipStart.toHHMMSS());
            }
        }
    });

    $("#txtClipEnd").blur(function () {
        var time = hmsToSecondsOnly($(this).val());
        if (time != _clipEnd) {
            if (time >= _clipStart && CheckClipDuration(time, false)) {
                _clipEnd = time;
                UpdateRKnob();
                SeekThumb(_clipThumbOffset);
            }
            else {
                $("#txtClipEnd").val(_clipEnd.toHHMMSS());
            }
        }
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

    _previewLoaded = false;
    _processedTitle120 = -1;
    _programTitle = "";

    console.log("..calling SP data...");

    $('.video-video').append(result.rawMediaObjectHTML);



    if (videotype == 'rawmedia') {
        _IsCheckSettings = true;
        if (result.offset > 0) {
            _CurrentTotalPlaySeconds = 0;
        }
        else {
            _CurrentTotalPlaySeconds = 1;
        }
        $('#divDescriptionTab').hide();
        $('#divCapDescriptionContent').hide();
        $('#divCapHighlightContent').html(result.HighlightHTML);
        $('#divCapAllContent').html(result.CaptionHTML);

        // commented for now, uncomment below if need to pause by absolute time
        //_PlaySecondsSetting = result.offset + _RawPlaySeconds;

        if (result.HighlightHTML != null && $.trim(result.HighlightHTML) != "") {
            ChangeTabPlayer(0);
        }
        else if (result.CaptionHTML != null && $.trim(result.CaptionHTML) != "") {
            ChangeTabPlayer(2);
        }
    }
    else if (videotype == 'ugc') {
        $('#divHighlightTab').hide();
        $('#divCapHighlightContent').hide();
        $('#divCaptionTab').hide();
        $('#divCapAllContent').hide();
        if (_VideoMetaData != null && _VideoMetaData[0] != null && _VideoMetaData[0].VideoMetaData != null && _VideoMetaData[0].VideoMetaData.Description != null) {
            ChangeTabPlayer(1);
        }
    }

    _ForceCategorySelection = result.forceCategorySelection;

    $(".select-text > div").css("cursor", "pointer");

    //SetPlayerMetaData();

    $("#divCapHighlightContent").mCustomScrollbar({
        scrollInertia: 1500,
        theme: "light-2",
        advanced: {
            updateOnContentResize: true,
            autoScrollOnFocus: false
        },
        mouseWheel: { scrollAmount: 100 }
    });

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
    $("#divCapHighlightContent").enscroll({
    verticalTrackClass: 'track4',
    verticalHandleClass: 'handle4',
    pollChanges: true
    });

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

    $('#divPlayer').modal({
        backdrop: 'static',
        keyboard: true
    });
}

var OnParseMetaData = function (result) {

    _VideoMetaData = result;
}

var OnParseCategoryData = function (result) {

    if (result != null && result.Status == 0) {
        _categoryData = result;
    }
}

var SetCategoryData = function () {

    if (_categoryData != null && _categoryData.Status == 0 && _categoryData.Category.length > 0 && $('.video-category option').length == 0) {

        //var options = "";

        if (_ForceCategorySelection) {
            $(".video-category").append("<option value=''></option>");
        }

        $.each(_categoryData.Category, function (eventID, eventData) {

            $(".video-category").append("<option value='" + eventData.Value + "'>" + eventData.Key + "</option>");
        });

        //$(".video-category").html(options);
        if (!_ForceCategorySelection) {
            $('.video-category option').filter(function () { return $(this).html() == "Default"; }).prop("selected", "selected");
        }
    }
}

var OnFailCategoryData = function (jqXHR, textStatus, errorThrown) { }

var GetNielsenData = function (dmanum) {

    if (videotype == 'rawmedia') {

        $.ajax({

            type: 'GET',
            dataType: 'jsonp',
            url: _urlVideoNielsenData + _ID + "&Type=true&SP=1&Num=" + dmanum,
            contentType: 'application/json; charset=utf-8',
            success: OnParseNielsenData,
            error: OnFailNielsenData
        });
    }
    else {
        var jsonPostData = {
            Guid: _ID,
            ClientGuid: _clientGuid,
            IsRawMedia: false,
            IQ_Start_Point: 4,
            IQ_Dma_Num: dmanum
        }

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
}

var OnParseNielsenData = function (result) {

    _nielsenData = result;

    if (videotype == 'clip') {
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
}

var OnFailNielsenData = function (jqXHR, textStatus, errorThrown) {
    console.log(errorThrown);
}

var OnFailMetaData = function (jqXHR, textStatus, errorThrown) {
    console.log(errorThrown);
}

var SetPlayerMetaData = function () {


    if (_VideoMetaData[0] != null && _VideoMetaData[0].Status == 0) {

        console.log("..SetPlayerdata...");

        if (videotype == 'ugc') {
            $("#video-ptitle").html(_VideoMetaData[0].VideoMetaData.Title);
            $(".video-clip-title").val(_VideoMetaData[0].VideoMetaData.Title);
            $(".video-aired").html("<label>Aired: </label>" + _VideoMetaData[0].VideoMetaData.AirDate);
            $('#divCapDescriptionContent').html(_VideoMetaData[0].VideoMetaData.Description);
            $(".video-program").html('Market: NA');
            $(".video-rank").html('NA');
        }
        else {
            $("#video-ptitle").html(_VideoMetaData[0].VideoMetaData.Title120s[0]);
            $(".video-clip-title").val(_VideoMetaData[0].VideoMetaData.Title120s[0]);
            $(".video-aired").html("<label>Aired: </label>" + _VideoMetaData[0].VideoMetaData.IQ_Local_Air_Date);
            $(".video-program").html(_VideoMetaData[0].VideoMetaData.StationCallSign + ' (' + _VideoMetaData[0].VideoMetaData.StationAffiliate + ') - ' + _VideoMetaData[0].VideoMetaData.IQ_Dma_Name);
            $(".video-rank").html(_VideoMetaData[0].VideoMetaData.IQ_Dma_Num);
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
        $('#HUY').css('height', screen.height);

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

        $('#HUY').css('width', 545);
        $('#HUY').css('height', 312);

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

    _flash.externalUpdateVol((_Vol / 100));

    if (_Vol == 0) {
        $(".video-vol-marker").css("background-position", "-28px -168px");
        $(".video-vol-mute").css("background-position", "-220px -168px");
    }
    else {
        $(".video-vol-mute").css("background-position", "-4px -168px");

        if (_Vol > 67) {
            $(".video-vol-marker").css("background-position", "-153px -168px");
        }
        else if (_Vol > 33) {
            $(".video-vol-marker").css("background-position", "-111px -168px");
        }
        else {
            $(".video-vol-marker").css("background-position", "-69px -168px");
        }
    }
}



var AddPlayerEvtListners = function () {

    var isMediaPaused = false;
    var isSeekKnobSelected = false;

    $(".video-forward").click(function () {

        _flash.externalFFFR(0, 3);
    });

    $(".video-fast-forward").click(function () {

        _flash.externalFFFR(0, 6);
    });

    $(".video-backward").click(function () {

        _flash.externalFFFR(1, 3);
    });

    $(".video-fast-backward").click(function () {

        _flash.externalFFFR(1, 6);
    });


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

        console.log("Seek...video-progress-bar");
        _flash.setSeekPoint(_Start + _SeekSecond);

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

            console.log("Seek...video-slider-knob.mousedown");
            _flash.setSeekPoint(_SeekSecond);

            SetPlayState();
        });


    });

    if (videotype != 'ugc') {
        $(".video-chart").click(function () {
            $(".video-clip-row").hide();
            $(".video-player-controls-overlay").remove();
            var v = $(".video-chart-row").is(":visible");
            $(".video-chart-row").slideToggle();

            if (v) {
                $(".video-chart").parent("li").removeClass("video-active");
            }
            else {
                $(".video-chart").parent("li").siblings().removeClass("video-active");
                $(".video-chart").parent("li").addClass("video-active");
            }
        });
    }
    if (videotype == 'rawmedia' || videotype == 'ugc') {
        $(".video-make-a-clip").click(function () {

            $(".video-dragger-l-st").html(_Start.toHHMMSS());
            $(".video-dragger-l-et").html(_Stop.toHHMMSS());

            $(".video-chart-row").hide();
            var v = $(".video-clip-row").is(":visible");

            $(".video-clip-row").slideToggle();

            if (v) {
                _previewLoaded = false;
                $(".video-player-controls-overlay").remove();
                $(".video-make-a-clip").parent("li").removeClass("video-active");

                ClearClip();
            }
            else {

                $(".select-text > div").css("cursor", "text");

                $(".video-player-controls").append("<div class=\"video-player-controls-overlay\"></div>");

                $(".video-make-a-clip").parent("li").siblings().removeClass("video-active");
                $(".video-make-a-clip").parent("li").addClass("video-active");

                _PlayState = 1;
                if (_flash != null) {
                    _flash.externalPlay(_PlayState);
                }

                _clipStart = _currentTimeInt;

                if ((_clipStart + 60) <= _Stop) {

                    _clipEnd = _clipStart + 60;
                }
                else if ((_clipStart + 1) <= _Stop) {

                    _clipEnd = _clipStart + (_Stop - _clipStart);
                }
                else {

                    if ((_clipStart - 1) >= _Start) {

                        _clipStart = _clipStart - 1;
                        _clipEnd = _currentTimeInt;
                    }
                }

                $("#txtClipStart").val(_clipStart.toHHMMSS());
                $("#txtClipEnd").val(_clipEnd.toHHMMSS());

                SetCategoryData();

                ResetThumb();
                ResetPreview();

                $(".video-keywords").css("border", "");
                $(".video-clip-title").css("border", "");
                $(".video-category").css("border", "");
                $(".video-textarea").css("border", "");
                $(".video-thumbContainer").css("border", "");

                //_clipCaptureThumbOffset = _clipThumbOffset;
            }

            UpdateLKnob();
            UpdateRKnob();

        });
    }

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

var _draggerLeft = 20;
var _draggerWidth = 60;
var _clipStart = 0;
var _clipEnd = 0;
var _clipThumbOffset = 0;
var _timerDragger = null;
var _timerClipStart = null;
var _draggerLWidth = $(".video-dragger-l").width();
var _draggerLLeftKnob = -6;
var _draggerLRightKnob = _draggerLWidth + 6;
var _timerLDragger = null;
var _clipCaptureThumbOffset = null;
var _previewPlay = 0;
var _previewLoaded = false;
var _isCCAllSelection = false;
var _isCCHighlightSelection = false;

var ClippingEvts = function () {


    $("#divEndIn").mousedown(function (e) {

        e.preventDefault();

        DivEndInMouseDown();

    });

    $("#divEndDe").mousedown(function (e) {

        e.preventDefault();

        DivEndDeMouseDown();

    });

    $("#divStartIn").mousedown(function (e) {

        e.preventDefault();

        DivStartInMouseDown();

    });

    $("#divStartDe").mousedown(function (e) {

        e.preventDefault();

        DivStartDeMouseDown();

    });

    $(".video-dragger-l-lk").mousedown(function () {

        VideoDraggerllkMouseDown();

    });

    $(".video-dragger-l-rk").mousedown(function () {

        VideoDraggerlrkMouseDown();

    });

    $("#divPreview").mousedown(function () {

        DivPreviewMouseDown();

    });

    $("#divThumb").mousedown(function () {

        DivThumbMouseDown();

    });

    $("#divThumbIn").click(function () {

        DivThumbInClick();

    });

    $("#divThumbDe").click(function () {

        DivThumbDeClick();

    });

    $(".captureThumbOffset").click(function () {

        CaptureThumbOffsetClick();
    });

    $(".video-preview").click(function () {

        VideoPreviewClick();
    });

    // mouseup event on closed caption selection, to fetch start and endoffset of selection text
    $("#divCapAllContent").mousedown(function (e) {

        DivCapAllContentMouseDown();
    });

    // mouseup event on highlighting selection, to fetch start and endoffset of selection text
    $("#divCapHighlightContent").mousedown(function () {

        DivCapHighlightContentMouseDown();

    });

    $("#ancClipSave").click(function () {

        $(".video-clip-title").bind("animationend webkitAnimationEnd oAnimationEnd MSAnimationEnd", function () {
            $(this).removeClass("video-input-error");
        });

        $(".video-keywords").bind("animationend webkitAnimationEnd oAnimationEnd MSAnimationEnd", function () {
            $(this).removeClass("video-input-error");
        });

        $(".video-textarea").bind("animationend webkitAnimationEnd oAnimationEnd MSAnimationEnd", function () {
            $(this).removeClass("video-input-error");
        });

        $(".video-category").bind("animationend webkitAnimationEnd oAnimationEnd MSAnimationEnd", function () {
            $(this).removeClass("video-input-error");
        });

        $(".video-thumbContainer").bind("animationend webkitAnimationEnd oAnimationEnd MSAnimationEnd", function () {
            $(this).removeClass("video-input-error");
        });

        if (ValidClipForm()) {
            CreateClip();
        }
    });

    var ValidClipForm = function () {

        var isValid = true;

        if (!$(".video-clip-title").val().trim().length > 0) {

            $(".video-clip-title").addClass("video-input-error");
            $(".video-clip-title").css("border", "1px solid #CB1C1C");

            isValid = false;
        }

        /*if (!$(".video-keywords").val().trim().length > 0) {

        $(".video-keywords").addClass("video-input-error");
        $(".video-keywords").css("border", "1px solid #CB1C1C");

        isValid = false;
        }

        if (!$(".video-textarea").val().trim().length > 0) {

        $(".video-textarea").addClass("video-input-error");
        $(".video-textarea").css("border", "1px solid #CB1C1C");

        isValid = false;
        }*/

        if ($(".video-category").val() == null || !$(".video-category").val().trim().length > 0) {

            $(".video-category").addClass("video-input-error");
            $(".video-category").css({
                "border": "1px solid #CB1C1C",
                "border-top": "none"
            });

            isValid = false;
        }

        /*if (_clipCaptureThumbOffset == null) {

        $(".video-thumbContainer").addClass("video-input-error");
        $(".video-thumbContainer").css("border", "1px solid #CB1C1C");            
        return false;

        }*/

        return isValid;
    }

    $("#ancClipCancel").click(function () {

        $(".video-clip-row").slideUp();

        _previewLoaded = false;
        $(".video-player-controls-overlay").remove();
        $(".video-make-a-clip").parent("li").removeClass("video-active");

        ClearClip();
    });

    UpdateLKnob();
    UpdateRKnob();
    SetThumbOffset();

}

var DivEndInMouseDown = function () {

    StopPreview();
    ResetThumb();
    ResetPreview();

    $(document).mouseup(function () {

        DocumentMouseUp();

    });

    if (!_timerDragger) {

        _timerDragger = setInterval(DraggerEndIn, 100);
    }
    else {
        clearInterval(_timerDragger);
        _timerDragger = setInterval(DraggerEndIn, 100);
    }

    if (!_timerClipStart) {
        _timerClipStart = setInterval(function () { InClipEnd() }, 100);
    }
    else {
        clearInterval(_timerClipStart);
        _timerClipStart = setInterval(function () { InClipEnd() }, 100);
    }

}

var DivEndDeMouseDown = function () {

    StopPreview();
    ResetThumb();
    ResetPreview();

    $(document).mouseup(function () {

        DocumentMouseUp();
    });

    if (!_timerDragger) {

        _timerDragger = setInterval(DraggerEndDe, 100);
    }
    else {
        clearInterval(_timerDragger);
        _timerDragger = setInterval(DraggerEndDe, 100);
    }

    if (!_timerClipStart) {
        _timerClipStart = setInterval(function () { DeClipEnd() }, 100);
    }
    else {
        clearInterval(_timerClipStart);
        _timerClipStart = setInterval(function () { DeClipEnd() }, 100);
    }

}

var DivStartInMouseDown = function () {

    StopPreview();
    ResetThumb();
    ResetPreview();

    $(document).mouseup(function () {

        DocumentMouseUp();
    });

    if (!_timerDragger) {

        _timerDragger = setInterval(DraggerStartIn, 100);
    }
    else {
        clearInterval(_timerDragger);
        _timerDragger = setInterval(DraggerStartIn, 100);
    }

    if (!_timerClipStart) {
        _timerClipStart = setInterval(function () { InClipStart() }, 100);
    }
    else {
        clearInterval(_timerClipStart);
        _timerClipStart = setInterval(function () { InClipStart() }, 100);
    }
}

var DivStartDeMouseDown = function () {

    StopPreview();
    ResetThumb();
    ResetPreview();

    $(document).mouseup(function () {

        DocumentMouseUp();
    });

    if (!_timerDragger) {

        _timerDragger = setInterval(DraggerStartDe, 100);
    }
    else {
        clearInterval(_timerDragger);
        _timerDragger = setInterval(DraggerStartDe, 100);
    }

    if (!_timerClipStart) {
        _timerClipStart = setInterval(function () { DeClipStart() }, 100);
    }
    else {
        clearInterval(_timerClipStart);
        _timerClipStart = setInterval(function () { DeClipStart() }, 100);
    }

}

var VideoDraggerllkMouseDown = function () {

    StopPreview();
    ResetThumb();
    ResetPreview();

    $(".video-clip-controls-overlay").show();
    $(".video-dragger-l").addClass('clipdrag-start');
    $("#divClipStart").addClass('clipdrag-start');
    $(".video-dragger-l-lk").addClass('clipdrag-start');

    $(document).mouseup(function () {
        $(".video-dragger-l").unbind("mousemove");

        $(".video-clip-controls-overlay").hide();
        $(".video-dragger-l").removeClass('clipdrag-start');
        $("#divClipStart").removeClass('clipdrag-start');
        $(".video-dragger-l-lk").removeClass('clipdrag-start');

        $(document).unbind("mouseup");
        SeekThumb(_clipThumbOffset);
    });

    $(".video-dragger-l").mousemove(function (e) {

        console.log(" e.pgeX: " + e.pageX + " lk offset left: " + $(".video-dragger-l-lk").offset().left);

        var posLeft = e.pageX - $(".video-dragger-l").offset().left;
        var posLeftRK = $(".video-dragger-l-rk").position().left;

        console.log("RK: " + posLeftRK);

        if (posLeft > 0 && (posLeftRK - (posLeft - 6)) >= 1) {

            $(".video-dragger-l-lk").css("left", posLeft - (6));

            var time = 0;
            var timeWidth = parseInt(posLeft);

            if (posLeft >= 0) {
                time = ((timeWidth * _Stop) / ($(".video-dragger-l").width() + 3));
            }

            if (CheckClipDuration(parseInt(time), true)) {
                _clipStart = parseInt(time);

                console.log("dragger width: " + $(".video-dragger-l").width() + " Stop:" + _Stop);
                console.log("..timeWidth...: " + timeWidth + "...time..." + time);
                $("#txtClipStart").val(time.toHHMMSS());

                SetThumbOffsetToStart();
                SetPreviewTimeToStart();
            }
            else {
                UpdateLKnob();
            }
        }

        return false;

    });

}

var VideoDraggerlrkMouseDown = function () {

    StopPreview();
    ResetThumb();
    ResetPreview();

    $(".video-clip-controls-overlay").show();
    $(".video-dragger-l").addClass('clipdrag-start');
    $("#divClipEnd").addClass('clipdrag-start');
    $(".video-dragger-l-rk").addClass('clipdrag-start');

    $(document).mouseup(function () {
        $(".video-dragger-l").unbind("mousemove");

        $(".video-clip-controls-overlay").hide();
        $(".video-dragger-l").removeClass('clipdrag-start');
        $("#divClipEnd").removeClass('clipdrag-start');
        $(".video-dragger-l-rk").removeClass('clipdrag-start');

        $(document).unbind("mouseup");
        SeekThumb(_clipThumbOffset);
    });

    $(".video-dragger-l").mousemove(function (e) {

        var posLeft = e.pageX - ($(".video-dragger-l").offset().left + 2);
        var posRight = $(".video-dragger-l").width() - 10 - posLeft;
        var posLeftLK = $(".video-dragger-l-lk").position().left;

        if (posRight < -6) {
            posRight = -6;
        }

        if (posRight >= -6 && (posLeft - posLeftLK) >= 1) {
            $(".video-dragger-l-rk").css("right", (posRight));
            console.log(" e.pgeX: " + e.pageX + " posLeft: " + posLeft + " posRight: " + posRight + " posLeftLK: " + posLeftLK);

            var time = 0;
            var timeWidth = parseInt(posRight + 6);
            timeWidth = $(".video-dragger-l").width() + 3 - timeWidth;


            time = ((timeWidth * _Stop) / ($(".video-dragger-l").width() + 3));

            if (CheckClipDuration(parseInt(time), false)) {
                _clipEnd = parseInt(time);

                console.log("..timeWidth...: " + timeWidth + "...time..." + time);
                $("#txtClipEnd").val(time.toHHMMSS());

                //SetThumbOffsetToStart();
                //SetPreviewTimeToStart();
            }
            else {
                UpdateRKnob();
            }
        }

        /*var posLeftRK = $(".video-dragger-l-rk").position().left;

        console.log("RK: " + posLeftRK);

        if (posLeft > 0 && (posLeftRK - (posLeft - 6)) >= 1) {

        $(".video-dragger-l-lk").css("left", posLeft - (6));

        }*/

    });

}

var DivPreviewMouseDown = function () {

    console.log("..preview mousedown..");

    $(document).mouseup(function () {

        $("#divMinTimeDragger").unbind("mousemove");
    });

    $("#divMinTimeDragger").mousemove(function (e) {

        _previewLoaded = false;
        StopPreview();
        e.preventDefault();

        var posLeft = e.pageX - ($(".video-dragger").offset().left);

        console.log("posLeft: " + posLeft);

        if (posLeft < -18) {
            posLeft = -18;
        }

        if (posLeft > $(".video-dragger").width() - 17) {
            posLeft = $(".video-dragger").width() - 17;
        }

        console.log("posLeft: " + posLeft);

        $("#divPreview").css("left", posLeft);

        SetPreviewTime();

    });

}

var DivThumbMouseDown = function () {

    StopPreview();

    $(document).mouseup(function () {

        $("#divMinTimeDragger").unbind("mousemove");
        $(document).unbind("mouseup");
        SeekThumb(_clipThumbOffset);

    });

    $("#divMinTimeDragger").mousemove(function (e) {

        e.preventDefault();

        var posLeft = e.pageX - ($(".video-dragger").offset().left);

        console.log("posLeft: " + posLeft);

        var halfDivWidth = $("#divThumb").width() / 2;

        var leftBorder = -(halfDivWidth - 1);
        var rightBorder = (($(".video-dragger").width() - 1) - (halfDivWidth - 1));

        if (posLeft < leftBorder) {
            posLeft = leftBorder;
        }

        if (posLeft > rightBorder) {
            posLeft = rightBorder;
        }

        console.log("left border: " + leftBorder);
        console.log("right border: " + rightBorder);

        $("#divThumb").css("left", posLeft);

        SetThumbOffset();

    });
}

var DivThumbInClick = function () {

    StopPreview();

    if (_clipThumbOffset < _clipEnd) {

        console.log("thumb offset: " + _clipThumbOffset + "   clipend: " + _clipEnd);

        _clipThumbOffset = _clipThumbOffset + 1;

        var posLeft = (((_clipThumbOffset - _clipStart) * $(".video-dragger").width()) / (_clipEnd - _clipStart));

        console.log("pl: " + posLeft);

        posLeft = posLeft - ($("#divThumb").width() / 2);

        console.log("pl2: " + posLeft);

        $("#divThumb").css("left", posLeft);

        $("#thumbOffset").html(_clipThumbOffset.toHHMMSS());

        DeCaptureThumbOffset();
        SeekThumb(_clipThumbOffset);
    }

}

var DivThumbDeClick = function () {

    StopPreview();

    if (_clipThumbOffset > _clipStart) {

        _clipThumbOffset = _clipThumbOffset - 1;

        var posLeft = (((_clipThumbOffset - _clipStart) * $(".video-dragger").width()) / (_clipEnd - _clipStart));

        console.log("pl: " + posLeft);

        posLeft = posLeft - ($("#divThumb").width() / 2);

        console.log("pl2: " + posLeft);

        $("#divThumb").css("left", posLeft);

        $("#thumbOffset").html(_clipThumbOffset.toHHMMSS());

        DeCaptureThumbOffset();

        SeekThumb(_clipThumbOffset);
    }
}

var VideoPreviewClick = function () {

    console.log("...preview click... + preview play" + _previewPlay + "..play state.." + _PlayState);

    var time = null;

    if (!_previewLoaded) {

        _previewLoaded = true;

        var posLeft = $("#divPreview").position().left;
        var halfDivWidth = $("#divPreview").width() / 2;

        var duration = _clipEnd - _clipStart;
        time = ((posLeft + (halfDivWidth - 1)) * duration) / ($(".video-dragger").width() - 1);
        time = parseInt(time) + _clipStart;

        if (_currenttime < _clipEnd || time <= _clipEnd) {
            if (time >= 0) {
                SeekThumb(time);
            }
        }
    }

    if (_currenttime < _clipEnd || (time != null && time < _clipEnd)) {
        if (_previewPlay == 0) {
            _previewPlay = 1;

            _PlayState = 0;
            if (_flash != null) {
                _flash.externalPlay(_PlayState);
            }
        }
        else {

            _previewPlay = 0;
            _PlayState = 1;
            if (_flash != null) {
                _flash.externalPlay(_PlayState);
            }
        }
    }

    console.log("...after click... + preview play" + _previewPlay + "..play state.." + _PlayState);
}

var DivCapAllContentMouseDown = function () {

    if ($(".video-clip-row").is(":visible")) {
        StopPreview();
        ResetThumb();
        ResetPreview();

        $(document).mouseup(function () {

            $("#divCapAllContent").unbind("mousemove");
            $("#divCapAllContent").unbind("mouseup");

            ResetThumb();
            ResetPreview();
        });

        $("#divCapAllContent").mousemove(function (e) {

            $("#divCapAllContent").unbind("mousemove");

            if (!_isCCAllSelection) {

                _isCCAllSelection = true;
                $("#divCapAllContent").mouseup(function (ev) {

                    _isCCAllSelection = false;

                    $("#divCapAllContent").unbind("mouseup");

                    var startOffset = null, endOffset = null;
                    var selection = GetSelectedHTML(this);
                    var res = selection[0].match(/setSeekPoint\((\d*)\)/g);
                    if (res != null && res.length > 0) {
                        startOffset = parseInt(res[0].replace("setSeekPoint(", "").replace(")", ""));
                        endOffset = parseInt(res[res.length - 1].replace("setSeekPoint(", "").replace(")", ""));
                    }
                    else {
                        res = selection[1].outerHTML.match(/setSeekPoint\((\d*)\)/g);
                        if (res != null && res.length > 0) {
                            startOffset = parseInt(res[0].replace("setSeekPoint(", "").replace(")", ""));
                            endOffset = parseInt(res[res.length - 1].replace("setSeekPoint(", "").replace(")", ""));
                        }
                    }

                    if (startOffset != null && endOffset != null) {
                        _clipStart = startOffset;
                        _clipEnd = endOffset + _captionDelay;

                        $("#txtClipStart").val(_clipStart.toHHMMSS());
                        $("#txtClipEnd").val(_clipEnd.toHHMMSS());

                        UpdateLKnob();
                        UpdateRKnob();
                    }
                });
            }
        });
    }
}

var DivCapHighlightContentMouseDown = function () {

    if ($(".video-clip-row").is(":visible")) {
        StopPreview();
        ResetThumb();
        ResetPreview();

        $(document).mouseup(function () {

            $("#divCapHighlightContent").unbind("mousemove");
            $("#divCapHighlightContent").unbind("mouseup");

            ResetThumb();
            ResetPreview();
        });

        $("#divCapHighlightContent").mousemove(function (e) {

            console.log("..mouse move..");

            $("#divCapHighlightContent").unbind("mousemove");

            if (!_isCCHighlightSelection) {

                _isCCHighlightSelection = true;

                $("#divCapHighlightContent").mouseup(function (e) {

                    _isCCHighlightSelection = false;

                    console.log("..mouse up..");

                    $("#divCapHighlightContent").unbind("mouseup");

                    var startOffset = null, endOffset = null;
                    var selection = GetSelectedHTML(this);
                    var res = selection[0].match(/setSeekPoint\((\d*)\)/g);
                    if (res != null && res.length > 0) {
                        startOffset = parseInt(res[0].replace("setSeekPoint(", "").replace(")", ""));
                        endOffset = parseInt(res[res.length - 1].replace("setSeekPoint(", "").replace(")", ""));
                    }
                    else {
                        res = selection[1].outerHTML.match(/setSeekPoint\((\d*)\)/g);
                        if (res != null && res.length > 0) {
                            startOffset = parseInt(res[0].replace("setSeekPoint(", "").replace(")", ""));
                            endOffset = parseInt(res[res.length - 1].replace("setSeekPoint(", "").replace(")", ""));
                        }
                    }

                    if (startOffset != null && endOffset != null) {
                        _clipStart = startOffset;
                        _clipEnd = endOffset + _captionDelay;

                        $("#txtClipStart").val(_clipStart.toHHMMSS());
                        $("#txtClipEnd").val(_clipEnd.toHHMMSS());

                        UpdateLKnob();
                        UpdateRKnob();
                    }

                });
            }

        });
    }

}




var CheckClipDuration = function (time, isStart) {

    if (isStart) {

        return ((_clipEnd - time) <= (_clipLimit - 1));
    }
    else {

        return ((time - _clipStart) <= (_clipLimit - 1));
    }
}

var CreateClip = function () {

    var Meta = [];
    Meta.push('{\"Field\": \"iQCategory\", \"Value\": \"' + $(".video-category").val() + '\"}');

    if (_KeyValues.length > 0) {
        Meta.push(_KeyValues);
    }

    var objMeta = jQuery.parseJSON(JSON.stringify(Meta));

    var request = "{\"ID\":\"" + _ID + "\",\"Start\":\"" + parseInt(_clipStart) + "\",\"End\":\"" + parseInt(_clipEnd) + "\","
                    + "\"Category\":\"PR\",\"Title\":\"" + $(".video-clip-title").val() + "\",\"Description\":\"" + $(".video-textarea").val() + "\",\"Keywords\":\"" + $(".video-keywords").val() + "\","
                    + "\"Meta\":[" + objMeta + "]"
                    + "}"
    $.ajax({

        type: 'POST',
        dataType: 'json',
        contentType: "application/json",
        url: _urlVideoCreateClip,
        data: request,
        xhrFields: {
            withCredentials: true
        },
        success: OnParseCreateClip,
        error: OnFailCreateClip

    });

}

var OnParseCreateClip = function (result) {

    if (result != null && result.Status == 0) {

        _previewLoaded = false;
        $(".video-player-controls-overlay").remove();
        $(".video-make-a-clip").css("background-position", "-64px 0px");
        $(".video-clip-row").slideToggle("slow");

        $(".video-player-controls-overlay").remove();
        $(".video-make-a-clip").css("background-position", "-64px 0px");

        ShowNotification(_msgClipCreationSuccess);

        var clip = result.Clip;
        CallThumbGen(clip);
        CallExport(clip);
        CallIOSExport(clip);
        /* insert clip timesync call */
        CallClipSync(clip, _clipStart, _clipEnd);
        ClearClip();
    }
    else {
        ShowNotification(_msgClipCreationFail);
    }
}

var CallThumbGen = function (fid) {

    if (_clipCaptureThumbOffset == null) {

        _clipCaptureThumbOffset = _clipStart + 10;

    }

    $.ajax({

        type: 'GET',
        dataType: 'json',
        url: _urlClipThumbgen + fid + "&Offset=" + (parseInt(_clipCaptureThumbOffset - _clipStart)),
        xhrFields: {
            withCredentials: true
        }
    });
}



var CallExport = function (fid) {

    $.ajax({

        type: 'GET',
        dataType: 'json',
        url: _urlClipExport + fid,
        xhrFields: {
            withCredentials: true
        }
    });
}

var CallIOSExport = function (fid) {

    $.ajax({

        type: 'GET',
        dataType: 'json',
        url: _urlClipIOSExport + fid,
        xhrFields: {
            withCredentials: true
        }
    });

}

var CallClipSync = function (fid, clipStart, clipEnd) {

    var request = "{\"ClipGuid\":\"" + fid + "\",\"StartOffset\":\"" + parseInt(_clipStart) + "\",\"EndOffset\":\"" + parseInt(_clipEnd) + "\"}";

    $.ajax({

        type: 'POST',
        dataType: 'json',
        contentType: "application/json",
        url: _urlClipTimeSync,
        data: request,
        xhrFields: {
            withCredentials: true
        }
    });
}

var OnFailCreateClip = function () {

    ShowNotification(_msgClipCreationFail);
}

var ClearPlayerData = function () {

    _PreviousPlayState = 0;
    _PlayState = 0;
    _VideoMetaData = "";
    _Start = 0;
    _Stop = 0;
    _Width = 545;
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

    $("#imgFPlay").remove();
    $("#imgFPause").remove();
}

var ClearClip = function () {

    //$(".video-clip-title").val(_VideoMetaData[0].VideoMetaData.Title120s[0]);
    $(".video-keywords").val("");
    $(".video-textarea").val("");
    if (!_ForceCategorySelection) {
        $('.video-category option').filter(function () { return $(this).html() == "Default"; }).prop("selected", "selected");
    }
    else {
        $('.video-category option').filter(function () { return $(this).html() == ""; }).prop("selected", "selected");
    }

    _clipStart = 0;
    _clipEnd = 0;

    UpdateLKnob();
    UpdateRKnob();
    SetThumbOffset();
    SetPreviewTime();

    $(".select-text > div").css("cursor", "pointer");
}

var OnParseThumbGeneration = function (result) {

    var thumbData = result[0];

    if (thumbData.Status == 0) {

        $(".video-thumbContainer").html("<img class=\"video-thumbnail\" src=\"" + thumbData.Location + "?v=" + (new Date()).getTime() + "\" />");
    }
    else {
        $(".video-thumbContainer").html('<div>Error</div');
    }

}

var OnFailThumbGeneration = function () {

    $(".video-thumbContainer").html('<div>Error</div');

}

var SetThumbOffset = function () {

    var posLeft = $("#divThumb").position().left;
    var halfDivWidth = $("#divThumb").width() / 2;

    var duration = _clipEnd - _clipStart;
    var time = ((posLeft + (halfDivWidth - 1)) * duration) / ($(".video-dragger").width() - 1);
    time = parseInt(time) + _clipStart;

    if (time >= 0) {
        _clipThumbOffset = time;
    }

    $("#thumbOffset").html(time.toHHMMSS());

    DeCaptureThumbOffset();

}

var SetThumbOffsetToStart = function () {

    _clipThumbOffset = _clipStart;
    $("#thumbOffset").html(_clipThumbOffset.toHHMMSS());
}

var SetPreviewTime = function () {

    var posLeft = $("#divPreview").position().left;
    var halfDivWidth = $("#divPreview").width() / 2;

    var duration = _clipEnd - _clipStart;
    var time = ((posLeft + (halfDivWidth - 1)) * duration) / ($(".video-dragger").width() - 1);
    time = parseInt(time) + _clipStart;

    $(".video-preview-time").html(time.toHHMMSS());
}

var SetPreviewTimeToStart = function () {

    $(".video-preview-time").html(_clipStart.toHHMMSS());

}

var DeCaptureThumbOffset = function () {

    _clipCaptureThumbOffset = null;
    $(".captureThumbOffset > div").css("background-color", "#2A76D9");
    $(".video-video").removeClass("video-thumb-capture");
    $(".video-thumbContainer").html('<div class="captureThumbOffset"><img alt="Capture" src="../../Images/video-player/camera.png" /></div><div style="clear: both; text-align: center; word-wrap: break-word; width: 169px;">Click to capture Thumbnail</div>');

    $(".captureThumbOffset").click(function () {

        CaptureThumbOffsetClick();
    });
}

var InClipEnd = function () {

    var time = _clipEnd + 1;

    if (CheckClipDuration(time, false)) {

        _clipEnd = time;

        if (_clipEnd > _Stop) {

            _clipEnd = _Stop;

            clearInterval(_timerClipStart);
        }

        $("#txtClipEnd").val(_clipEnd.toHHMMSS());

        SetThumbOffsetToStart();
        UpdateRKnob();
    }
}

var DraggerEndIn = function () {

    _draggerWidth = _draggerWidth + 1;

    if ((_draggerWidth - _draggerLeft) <= 50) {

        $(".video-dragger").css("width", _draggerWidth + "%");
    }
    else {
        if (_timerDragger) {
            clearInterval(_timerDragger);
        }
    }
}

var DeClipEnd = function () {

    _clipEnd = _clipEnd - 1;
    if (_clipEnd <= _clipStart) {

        _clipEnd = _clipStart + 1;
        clearInterval(_timerClipStart);
    }
    else {
        $("#txtClipEnd").val(_clipEnd.toHHMMSS());
    }

    SetThumbOffsetToStart();
    UpdateRKnob();
}

var DraggerEndDe = function () {

    _draggerWidth = _draggerWidth - 1;

    if ((_draggerWidth - _draggerLeft) >= 30) {

        /*$(".video-dragger").css("width", _draggerWidth + "%");
        $(".video-dragger").css("left", _draggerLeft + "%");*/

        $(".video-dragger").css({

            "width": _draggerWidth + "%",
            "left": _draggerLeft + "%"

        });

    }
    else {
        if (_timerDragger) {
            clearInterval(_timerDragger);
        }
    }
}

var DocumentMouseUp = function () {

    // remove all timers

    console.log("..DocumentMouseUp..");

    if (_timerDragger) {
        clearInterval(_timerDragger);
    }

    if (_timerClipStart) {
        clearInterval(_timerClipStart);
    }

    _draggerLeft = 20;
    _draggerWidth = 60;

    $(".video-dragger").animate({
        left: _draggerLeft + "%",
        width: _draggerWidth + "%"
    }, 500, function () { });

    SeekThumb(_clipThumbOffset);
    $(document).unbind("mouseup");
}

var InClipStart = function () {

    _clipStart = _clipStart + 1;
    if (_clipStart >= _clipEnd) {

        _clipStart = _clipEnd - 1;
        clearInterval(_timerClipStart);
    }
    else {
        $("#txtClipStart").val(_clipStart.toHHMMSS());
    }

    SetThumbOffsetToStart();
    UpdateLKnob();
    SetPreviewTimeToStart();
}

var DraggerStartIn = function () {

    _draggerLeft = _draggerLeft + 1;
    _draggerWidth = _draggerWidth - 1;

    if (_draggerLeft <= 30) {

        $(".video-dragger").css("width", _draggerWidth + "%");
        $(".video-dragger").css("left", _draggerLeft + "%");

    }
    else {
        if (_timerDragger) {
            clearInterval(_timerDragger);
        }
    }
}

var DeClipStart = function () {

    var time = _clipStart - 1;

    if (CheckClipDuration(time, true)) {

        _clipStart = time;

        if (_clipStart < 0) {

            _clipStart = 0;

            clearInterval(_timerClipStart);
        }

        $("#txtClipStart").val(_clipStart.toHHMMSS());

        SetThumbOffsetToStart();
        UpdateLKnob();
        SetPreviewTimeToStart();
    }
}

var DraggerStartDe = function () {

    _draggerLeft = _draggerLeft - 1;
    _draggerWidth = _draggerWidth + 1;

    if (_draggerLeft >= 10) {

        $(".video-dragger").css("width", _draggerWidth + "%");
        $(".video-dragger").css("left", _draggerLeft + "%");

    }
    else {
        if (_timerDragger) {
            clearInterval(_timerDragger);
        }
    }
}

var UpdateLKnob = function () {

    var pos = (($(".video-dragger-l").width() * _clipStart) / _Stop);

    $(".video-dragger-l-lk").css("left", pos - 6);
};

var UpdateRKnob = function () {

    var pos = $(".video-dragger-l").width() - (($(".video-dragger-l").width() * _clipEnd) / _Stop);

    $(".video-dragger-l-rk").css("right", pos - 6);
}

var Seek = function (seekPoint, obj) {

    seekPoint = (seekPoint * 60);
    if (seekPoint < _Stop) {

        console.log("Seek...Seek");

        _flash.setSeekPoint(seekPoint);

        $(".video-seek > span").each(
            function () { $(this).removeClass("video-active"); }
            );

        $(obj).addClass("video-active");
    }
}

var SeekThumb = function (seekPoint) {

    console.log("Seek...SeekThumb");
    _flash.setSeekPoint(seekPoint);
}

// method to fetch selected text of element

/*

var GetSelectedHTML= function(el) {
// this gives me a cross browser start and end position
// for the text selection
var start = 0, end = 0, parentEl = null;
var sel, range, priorRange;
if (typeof window.getSelection != "undefined") {
range = window.getSelection().getRangeAt(0);
priorRange = range.cloneRange();
priorRange.selectNodeContents(el);
priorRange.setEnd(range.startContainer, range.startOffset);
start = priorRange.toString().length;
end = start + range.toString().length;

parentEl = range.commonAncestorContainer;
if (parentEl.nodeType != 1) {
parentEl = parentEl.parentNode;
}

if (parentEl.onclick == null || parentEl.onclick.toString().indexOf("setSeekPoint") <= 0) {
parentEl = $(parentEl).closest("div[onclick^='setSeekPoint']")[0]
}
}
else if (typeof document.selection != "undefined" && (sel = document.selection).type != "Control") {
range = sel.createRange();
priorRange = document.body.createTextRange();
priorRange.moveToElementText(el);
priorRange.setEndPoint("EndToStart", range);
start = priorRange.text.length;
end = start + range.text.length;
parentEl = range.parentElement();

if (parentEl.onclick == null || parentEl.onclick.toString().indexOf("setSeekPoint") <= 0) {
parentEl = $(parentEl).closest("div[onclick^='setSeekPoint']")[0]
}
}

// now, get this in terms of html selection
var html = el.outerHTML, text = 1, htmlstart = 0, htmlend = 0;
for (var i = 0, t = 0; i < html.length; ++i) {
if (html[i] == '<' || html[i] == '>') {
text = text ? 0 : 1;
continue;
}
if (text)
t++;
if (t == start)
htmlstart = i + 1;
if (t == end) {
htmlend = i + 1;
break;
}
}

if (el.id == "divCapHighlightContent") {
var beforeHtml = html.substring(0, htmlstart);
if (beforeHtml.lastIndexOf("setSeekPoint") > 0) {
htmlstart = beforeHtml.lastIndexOf("setSeekPoint");
}
}

return [html.substring(htmlstart, htmlend), parentEl];
}

*/

var GetSelectedHTML = function (el) {
    // this gives me a cross browser start and end position
    // for the text selection
    var start = 0, end = 0, parentEl = null;
    var sel, range, priorRange;
    if (typeof window.getSelection != "undefined") {
        range = window.getSelection().getRangeAt(0);
        priorRange = range.cloneRange();
        priorRange.selectNodeContents(el);
        priorRange.setEnd(range.startContainer, range.startOffset);

        var tempPriorString = priorRange.toString();
        tempPriorString = tempPriorString.replace(/</g, '&lt;');
        tempPriorString = tempPriorString.replace(/>/g, '&gt;');

        var currentString = range.toString();
        currentString = currentString.replace(/</g, '&lt;');
        currentString = currentString.replace(/>/g, '&gt;');

        start = tempPriorString.length;
        end = start + currentString.length;

        parentEl = range.commonAncestorContainer;

    }
    else if (typeof document.selection != "undefined" && (sel = document.selection).type != "Control") {
        range = sel.createRange();
        priorRange = document.body.createTextRange();
        priorRange.moveToElementText(el);
        priorRange.setEndPoint("EndToStart", range);

        var tempPriorString = priorRange.toString();
        tempPriorString = tempPriorString.replace(/</g, '&lt;');
        tempPriorString = tempPriorString.replace(/>/g, '&gt;');

        var currentString = range.text;
        currentString = currentString.replace(/</g, '&lt;');
        currentString = currentString.replace(/>/g, '&gt;');

        start = tempPriorString.length;
        end = start + currentString.length;

        parentEl = range.parentElement();


    }

    // now, get this in terms of html selection
    var html = el.outerHTML, text = 1, htmlstart = 0, htmlend = 0;
    for (var i = 0, t = 0; i < html.length; ++i) {
        if (html[i] == '<') {
            text = 0
            continue;
        }
        else if (html[i] == '>') {
            text = 1;
            continue;
        }

        /*if (html[i] == '<' || html[i] == '>') {
        text = text ? 0 : 1;
        continue;
        }*/
        if (text)
            t++;
        if (t == start)
            htmlstart = i + 1;
        if (t == end) {
            htmlend = i + 1;
            break;
        }
    }

    /*if (el.id == "DivHighlight") {*/
    var beforeHtml = html.substring(0, htmlstart);
    if (beforeHtml.lastIndexOf("setSeekPoint") > 0) {
        htmlstart = beforeHtml.lastIndexOf("setSeekPoint");
    }
    /*}*/

    return [html.substring(htmlstart, htmlend), parentEl];
}

var CaptureThumbOffsetClick = function () {

    if (_clipCaptureThumbOffset == null) {
        _clipCaptureThumbOffset = _clipThumbOffset;

        var audio = document.getElementById("audioCapture");
        audio.play();

        //$(".captureThumbOffset > div").css("background-color", "#ED2275");

        //$(".video-video").css("box-shadow", "0 -99px 60px 31px #FF0000");

        $(".video-video").addClass("video-thumb-capture");
        $(".video-thumbContainer").css("border", "");

        $.ajax({

            type: 'GET',
            dataType: 'jsonp',
            url: _urlVideoThumb + _ID + "&Offset=" + parseInt(_clipCaptureThumbOffset),
            contentType: 'application/json; charset=utf-8',
            success: OnParseThumbGeneration,
            error: OnFailThumbGeneration
        });

        $(".video-thumbContainer").html('<div>Generating...</div');
    }

}

var UpdateTitle120 = function (currenttime) {

    for (var i = 0; i < _VideoMetaData[0].VideoMetaData.IQ_Start_Points.length; i++) {

        var startPoint = _VideoMetaData[0].VideoMetaData.IQ_Start_Points[i];
        var startMinute = _VideoMetaData[0].VideoMetaData.IQ_Start_Minutes[i];
        var currentMinute = currenttime / 60;

        if (currentMinute < 15 && startPoint == 1 && currentMinute >= startMinute) {

            _programTitle = _VideoMetaData[0].VideoMetaData.Title120s[i];
            _processedTitle120 = i;
            break;
        }
        else if (currentMinute >= 15 && currentMinute < 30 && startPoint == 2 && currentMinute >= startMinute) {
            _programTitle = _VideoMetaData[0].VideoMetaData.Title120s[i];
            _processedTitle120 = i;
            break;
        }
        else if (currentMinute >= 30 && currentMinute < 45 && startPoint == 3 && currentMinute >= startMinute) {
            _programTitle = _VideoMetaData[0].VideoMetaData.Title120s[i];
            _processedTitle120 = i;
            break;
        }
        else if (currentMinute >= 45 && currentMinute < 60 && startPoint == 4 && currentMinute >= startMinute) {
            _programTitle = _VideoMetaData[0].VideoMetaData.Title120s[i];
            _processedTitle120 = i;
            break;
        }
    }

    if (_programTitle.trim() == "") {
        $("#video-ptitle").html("Title: NA");
    }
    else {
        $("#video-ptitle").html(_programTitle);

        if ($.inArray($(".video-clip-title").val(), _VideoMetaData[0].VideoMetaData.Title120s) > -1) {

            $(".video-clip-title").val(_programTitle);
        }
    }

}

var UpdateNielsenData = function (currenttime) {

    var currentMinute = currenttime / 60;
    var nielsenAudience = null;
    var nielsenMediaValue = null;

    if (_nielsenData[0].Status == 0 && _nielsenData[0].NielSenData.length > 0) {

        if (currentMinute < 15) {

            nielsenAudience = _nielsenData[0].NielSenData[0].AUDIENCE;
            nielsenMediaValue = _nielsenData[0].NielSenData[0].SQAD_SHAREVALUE;
        }
        else if (currentMinute < 30 && _nielsenData[0].NielSenData.length > 1) {

            nielsenAudience = _nielsenData[0].NielSenData[1].AUDIENCE;
            nielsenMediaValue = _nielsenData[0].NielSenData[1].SQAD_SHAREVALUE;

        }
        else if (currentMinute < 45 && _nielsenData[0].NielSenData.length > 2) {

            nielsenAudience = _nielsenData[0].NielSenData[2].AUDIENCE;
            nielsenMediaValue = _nielsenData[0].NielSenData[2].SQAD_SHAREVALUE;

        }
        else if (currentMinute >= 45 && _nielsenData[0].NielSenData.length > 3) {

            nielsenAudience = _nielsenData[0].NielSenData[3].AUDIENCE;
            nielsenMediaValue = _nielsenData[0].NielSenData[3].SQAD_SHAREVALUE;

        }
    }

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

var UpdatePreviewPosition = function (currentTime) {

    var duration = _clipEnd - _clipStart;
    var halfDivWidth = $("#divPreview").width() / 2;
    var posLeft = ((currentTime - _clipStart) * ($(".video-dragger").width() - 1)) / duration;
    posLeft = posLeft - (halfDivWidth - 1);

    $("#divPreview").css("left", posLeft);

    $(".video-preview-time").html(currentTime.toHHMMSS());
}

var StopPreview = function () {

    if (_PlayState == 0) {

        _PlayState = 1;
        _previewPlay = 0;
        if (_flash != null) {
            _flash.externalPlay(_PlayState);
        }
    }

}

var ResetThumb = function () {

    $("#divThumb").css("left", -16);
    _clipThumbOffset = _clipStart;
    $("#thumbOffset").html(_clipThumbOffset.toHHMMSS());
}

var ResetPreview = function () {

    $("#divPreview").css("left", -16);
    $(".video-preview-time").html(_clipStart.toHHMMSS());
}

var setSeekPoint = function (C, B) {
    var Flash = document.getElementById('HUY');
    if (Flash != null) {
        Flash.setSeekPoint(C);
    }

    return false;
}

//__________________________________________________________External Interface______________________________________________________________________________________

var GetRef = function () {
    var url = (window.location != window.parent.location) ? document.referrer : document.location.href;
    return url;
}

var SetPlayerInfo = function (logo, title) {

    $("#video-source").attr("src", logo);
    if (typeof (title) !== 'undefined' && videotype == 'ugc') {
        $(".video-program").html(title);
    }
}

var SetTimings = function (start, stop) {
    _Start = start;
    _Stop = stop;

    _TotalTime = _Stop - _Start;

    $(".video-duration").html(_TotalTime.toHHMMSS());
}

var UpdateTimeDuration = function (currentTime) {

    if (videotype == 'rawmedia') {
        if (parseInt(currentTime) > _currentTimeInt) {
            _CurrentTotalPlaySeconds = _CurrentTotalPlaySeconds + 1;
        }
    }

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
    var progTimelength = ((progTime * 99.6316758747698) / intWidth);
    $(".video-current-time").html(currentTime.toHHMMSS());
    $(".video-play-bar").css("width", progTimelength + "%");

    if (videotype == 'rawmedia' && _previewLoaded == true) {

        if (currentTime >= _clipEnd) {

            _PlayState = 1;
            _previewPlay = 0;
            if (_flash != null) {
                _flash.externalPlay(_PlayState);
            }
        }

        UpdatePreviewPosition(currentTime);
    }

    if (videotype == 'rawmedia' && _IsCheckSettings) {
        if (_CurrentTotalPlaySeconds == _PlaySecondsSetting) {
            _IsCheckSettings = false;
            _PlayState = 1;
            if (_flash != null) {
                _flash.externalPlay(_PlayState);
            }
        }
    }


    // commented for now, uncomment below if need to pause by absolute time
    /*if (videotype == 'rawmedia') {
    if (_currentTimeInt == _PlaySecondsSetting) {
    _PlayState = 1;
    _flash.externalPlay(_PlayState);
    }
    }*/

    if (videotype == 'rawmedia') {
        if ($("[id ^= 'video-chart-']").length > 0) {
            if (_IsManualHover == false) {
                var charts = $("[id ^= 'video-chart-']");
                if (LastIndex > 0) {
                    $.each(charts, function (index, objChart) {
                        var highChartEle = $(objChart).highcharts();
                        if (highChartEle.series[0].data[LastIndex] != undefined) {
                            highChartEle.series[0].data[LastIndex].setState('');
                            highChartEle.series[1].data[LastIndex].setState('');
                            highChartEle.tooltip.hide();
                        }
                    });
                }

                if (charts != null && charts != undefined && charts.length > 0) {
                    LastIndex = xIndex;
                    xIndex = $(charts[0]).highcharts().axes[0].categories.indexOf(_currentTimeInt.toString());

                    $.each(charts, function (index, objChart) {
                        var highChartEle = $(objChart).highcharts();
                        if (highChartEle.series[0].data[xIndex] != undefined) {
                            if (highChartEle.series[0].visible) {
                                highChartEle.series[0].data[xIndex].setState('hover');
                            }

                            if (highChartEle.series[1].visible) {
                                highChartEle.series[1].data[xIndex].setState('hover');
                            }

                            if (highChartEle.series[0].visible || highChartEle.series[1].visible) {
                                if (highChartEle.series[0].visible && highChartEle.series[1].visible) {
                                    highChartEle.tooltip.refresh([highChartEle.series[0].data[xIndex], highChartEle.series[1].data[xIndex]]);
                                }
                                else if (highChartEle.series[0].visible) {
                                    highChartEle.tooltip.refresh([highChartEle.series[0].data[xIndex]]);
                                }
                                else {
                                    highChartEle.tooltip.refresh([highChartEle.series[1].data[xIndex]]);
                                }
                            }
                        }
                    });
                }
            }
        }
    }

    if (videotype == 'rawmedia') {
        UpdateTitle120(currentTime);
        UpdateNielsenData(currentTime);
    }

    /*
   

    
    console.log("progTime: " + progTime + " progTimeLth: " + progTimelength + " width: " + progTimelength + "%");*/
}

var UpdatePlayerState = function (state) {

    $(".video-buffering").hide();

    switch (state) {
        case -1:
            break;
        case 0:
            _PlayState = 0;
            $("#play").removeClass("video-play");
            $("#play").addClass("video-pause");
            $(".video-preview").css("background-image", "url('../images/Video-Player/preview-pause.png'), linear-gradient(to bottom, rgb(255, 255, 255) 0%, rgb(233, 233, 233) 100%)");
            break;
        case 1:
            _PlayState = 1;
            $("#play").removeClass("video-pause");
            $("#play").addClass("video-play");
            $(".video-preview").css("background-image", "url('../images/Video-Player/preview.png'), linear-gradient(to bottom, rgb(255, 255, 255) 0%, rgb(233, 233, 233) 100%)");
            break;
        case 2:
            break;
        case 3:
            $(".video-buffering").show();
            break;
        case 4:
            break;
    }
}

var setImage = function (baseSixtyFourEncodedImage) {
    $(".video-thumbnail").attr("src", "data:" + baseSixtyFourEncodedImage);
}

var SetThumb = function () {
    var soundHandle = document.getElementById('audioCapture');
    soundHandle.play();
}

var _Callback;

function LoadClipPlayer(clipID, callback) {
    videotype = 'clip';
    _Callback = callback;
    _ID = clipID;
    var jsonPostData = { ClipID: clipID }

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

var OnParsePlayerMetaData = function (result) {
    _VideoMetaData = result;
}

var OnFailPlayerMetaData = function (jqXHR, textStatus, errorThrown) {
    console.log(errorThrown);
}

var LibraryLoadNPlayer = function (result) {
    if (result.isSuccess) {
        _clientGuid = result.clientGuid;
        _PlayerFromEmail = result.email;
        _FileName = result.title;
        var rawPlayer = '<div class="video-modalPlayer hide resizable select-none" id="divPlayer">'
    + '<div class="video-close">X</div>'
    + '<div class="video-container-narrow">'
    + '<div class="video-player-row-one">'
    + '<div class="video-caption-info video-fleft">'
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
    + '</div>'
    + '<div class="video-player video-fleft" style="margin-top:13px;">'
    + '<div class="video-video">'
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
          + '<div class="video-control-holder">'
            + '<ul class="video-controls video-control-holder">'
              + '<li class="video-fast-backward"></li>'
              + '<li class="video-backward"></li>'
              + '<li class="video-pause" id="play" onclick="PlayPause();"></li>'
              + '<li class="video-forward"></li>'
              + '<li class="video-fast-forward"></li>'
            + '</ul>'
            + '<div class="video-volume video-control-holder">'
              + '<div class="video-vol-mute">&nbsp;</div>'
              + '<div id="video-vol-slider"></div>'
              + '<div class="video-vol-marker">&nbsp;</div>'
            + '</div>'
            + '<div class="video-control-holder video-fleft">'
              + '<span class="video-fullscreen"></span>'
            + '</div>'
          + '</div>'
          + '<div class="video-player-controls-overlay"></div>'
        + '</div>'
    + '</div>'
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
    + '</div>'
    + '<div class="video-clear">'
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
                   + '<div class="video-share-left">'
            if (result.isEmailSharing) {
                rawPlayer += '<div id="divEmailForm" style="display:none;">'
                                + '<div class="email-form">'
                                    + '<input type="text" placeholder="Email Address" id="txtPlayerFromEmail" value="' + result.email + '" class="video-clip-email-from">'
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
                                    + '<textarea class="video-clip-embed" id="txtLink" readOnly="true">' + _urlClipPlayer + _ID + '</textarea>'
                                + '</div>'
                                + '<div style="text-align:right; margin-right:0px;">'
                                    + '<input type=\"button\" class="cancelButton" value=\"Cancel\" id=\"btnCancelLink\" onclick=\"CancelLink();\">'
                                    + '<input type=\"button\" class="blueButton" value=\"Select\" id=\"btnCopyLink\" onclick=\"CopyLink();\">'
                                + '</div>'
                            + '</div>'
                rawPlayer += '<div id="divEmbedForm" style="display:none">'
                                + '<div class="embed-form">'
                                    + '<textarea class="video-clip-embed" id="txtEmbed" readOnly="true"><iframe src="' + _urlClipPlayer + _ID + '&ps=1" width="545" height="320" /></textarea>'
                                + '</div>'
                                + '<div style="text-align:right; margin-right:0px;">'
                                    + '<input type=\"button\" class="cancelButton" value=\"Cancel\" id=\"btnCancelEmbed\" onclick=\"CancelEmbed();\">'
                                    + '<input type=\"button\" class="blueButton" value=\"Select\" id=\"btnCopyEmbed\" onclick=\"CopyEmbed();\">'
                                + '</div>'
                            + '</div>'
                            + '<div id="divWordPressForm" style="display:none">'
                                + '<div class="embed-form">'
                                    + '<textarea class="video-clip-embed" id="txtWordPress" readOnly="true"><iframe src="' + _urlClipPlayer + _ID + '&ps=1" width="545" height="320" /></textarea>'
                                + '</div>'
                                + '<div style="text-align:right; margin-right:0px;">'
                                    + '<input type=\"button\" class="cancelButton" value=\"Cancel\" id=\"btnCancelWordPress\" onclick=\"CancelWordPress();\">'
                                    + '<input type=\"button\" class="blueButton" value=\"Select\" id=\"btnCopyWordPress\" onclick=\"CopyWordPress();\">'
                                + '</div>'
                            + '</div>'
            }
            rawPlayer += '</div>'
                   + '<div class="video-share-right">'
            if (result.isEmailSharing) {
                rawPlayer += '<div class="video-share-email">'
                                + '<span style="margin-left:20px;margin-top:10px;">Email</span></br>'
                                + '<img src="/images/MediaIcon/Email_Share.png" alt="email" style="margin-left:15px;margin-top:10px;cursor:pointer;" id="imgEmail">'
                            + '</div>'
            }
            else {
                rawPlayer += '<div class="video-share-email">'
                                + '<span style="margin-left:15x;margin-top:10px;"></span></br>'
                            + '</div>'
            }
            if (result.isSharing) {
                rawPlayer += '<div class="video-share-link">'
                                + '<span style="margin-left:25px;margin-top:10px;">Link</span></br>'
                                + '<img src="/images/MediaIcon/link.png" alt="Link" style="margin-left:15px;margin-top:10px;cursor:pointer;" id="imgLink">'
                            + '</div>'
                rawPlayer += '<div class="video-share-social">'
                                + '<span style="margin-left:20px;margin-top:10px;">Social</span></br>'
                                + '<a href="' + _urlLibraryShareClip + _ID + '&p_SourceType=FB" target="_blank"><img alt="facebook" src="/images/MediaIcon/facebook_Share.png" style="margin-left:15px;margin-top:10px;cursor:pointer;" id="imgFacebook"></a>'
                                + '<a href="' + _urlLibraryShareClip + _ID + '&p_SourceType=TW" target="_blank"><img alt="twitter" src="/images/MediaIcon/twitter_share.png" style="margin-left:20px;margin-top:10px;cursor:pointer;" id="imgTwitter"></a>'
                                + '<a href="' + _urlLibraryShareClip + _ID + '&p_SourceType=GP" target="_blank"><img alt="google plus" src="/images/MediaIcon/google-plus_share.png" style="margin-left:20px;margin-top:10px;cursor:pointer;" id="imgGoogle"></a>'
                                + '<a href="' + _urlLibraryShareClip + _ID + '&p_SourceType=TR" target="_blank"><img alt="tumblr" src="/images/MediaIcon/Tumblr_Share.png" style="margin-left:20px;margin-top:10px;cursor:pointer;" id="imgTest"></a>'
                            + '</div>'
                            + '<div class="video-share-web">'
                                + '<span style="margin-left:18px;margin-top:10px;">Website</span><span style="margin-left:20px;margin-top:10px;">Embed</span></br>'
                                + '<img alt="embed" src="/images/MediaIcon/Dribbble_share.png" style="margin-left:20px;margin-top:10px;cursor:pointer;" id="imgEmbed">'
                                + '<img alt="word press" src="/images/MediaIcon/WordPress_Share.png" style="margin-left:20px;margin-top:10px;cursor:pointer;" id="imgWordPress">'
                            + '</div>'
            }
            else {
                rawPlayer += '<div class="video-share-social">'
                            + '</div>'
            }
            rawPlayer += '</div>'
        + '</div>'
        }
        rawPlayer += '</div>'
    + '</div>'
    + '</div>'
    + '<audio id="audioCapture" src="/Audio/camera_flick.mp3" style="display: none;"></audio>';

        $(document.body).append(rawPlayer);
        $(document.body).append("<img id='imgFPlay' src='/images/video-player/play_f.png' style='display:none;position:absolute;top:0;right:0;bottom:0;left:0;margin:auto;z-index:2147483647;' />");
        $(document.body).append("<img id='imgFPause' src='/images/video-player/pause_f.png' style='display:none;position:absolute;top:0;right:0;bottom:0;left:0;margin:auto;z-index:2147483647;' />");

        _firstCall = true;

        $("#video-vol-slider").slider({
            orientation: "horizontal",
            range: "min",
            min: 0,
            max: 100,
            value: 75,
            slide: UpdateVolume,
            change: UpdateVolume
        });
        $(".video-vol-marker").css("background-position", "-153px -168px");

        $(".video-close").click(function (e) {

            if ($(".video-video") != null || $(".video-video") != undefined) {
                $(".video-video").html('');
            }

            $('#divPlayer').css({ "display": "none" });
            $('#divPlayer').modal('hide');
            $('#divPlayer').remove();

        });

        $(".video-chart").click(function () {
            $(".video-clip-row").hide();
            $(".video-player-controls-overlay").remove();
            var v = $(".video-chart-row").is(":visible");
            $(".video-chart-row").slideToggle();

            if (v) {
                $(".video-chart").parent("li").removeClass("video-active");
            }
            else {
                $(".video-share-main").css('display', 'none');
                $(".video-chart").parent("li").siblings().removeClass("video-active");
                $(".video-chart").parent("li").addClass("video-active");
            }
        });

        $("#txtClipStart").blur(function () {
            var time = hmsToSecondsOnly($(this).val());
            if (time != _clipStart) {
                if (time <= _clipEnd && CheckClipDuration(time, true)) {
                    _clipStart = time;
                    SetThumbOffsetToStart();
                    UpdateLKnob();
                    SetPreviewTimeToStart();
                    SeekThumb(_clipThumbOffset);
                }
                else {
                    $("#txtClipStart").val(_clipStart.toHHMMSS());
                }
            }
        });

        $("#txtClipEnd").blur(function () {
            var time = hmsToSecondsOnly($(this).val());
            if (time != _clipEnd) {
                if (time >= _clipStart && CheckClipDuration(time, false)) {
                    _clipEnd = time;
                    UpdateRKnob();
                    SeekThumb(_clipThumbOffset);
                }
                else {
                    $("#txtClipEnd").val(_clipEnd.toHHMMSS());
                }
            }
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
                $("#txtPlayerFromEmail").val(_PlayerFromEmail);
                $("#txtPlayerFromEmail").focus();
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

        console.log("..calling SP data...");

        $('.video-video').append(result.clipHTML);

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
            keyboard: true
        });
    }

}

function OnClipLoadFail(XHR, Status, Error) {
    ShowNotification(_msgSomeErrorProcessing);
}

var SetLibraryPlayerMetaData = function () {


    if (_VideoMetaData[0] != null && _VideoMetaData[0].Status == 0) {

        console.log("..SetPlayerdata...");

        if (videotype == 'ugc') {
            $("#video-ptitle").html(_VideoMetaData[0].VideoMetaData.Title);
            $(".video-clip-title").val(_VideoMetaData[0].VideoMetaData.Title);
            $(".video-aired").html("<label>Aired: </label>" + _VideoMetaData[0].VideoMetaData.AirDate);
            $('#divCapDescriptionContent').html(_VideoMetaData[0].VideoMetaData.Description);
            $(".video-program").html("Market: NA");
            $(".video-rank").html('NA');
        }
        else {
            $("#video-ptitle").html(_VideoMetaData[0].VideoMetaData.Title120s[0]);
            $(".video-clip-title").val(_VideoMetaData[0].VideoMetaData.Title120s[0]);
            $(".video-aired").html("<label>Aired: </label>" + _VideoMetaData[0].VideoMetaData.IQ_Local_Air_Date);
            $(".video-program").html(_VideoMetaData[0].VideoMetaData.StationCallSign + ' (' + _VideoMetaData[0].VideoMetaData.StationAffiliate + ') - ' + _VideoMetaData[0].VideoMetaData.IQ_Dma_Name);
            $(".video-rank").html(_VideoMetaData[0].VideoMetaData.IQ_Dma_Num);
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

var numOfVisibleLogos;
var yAxisInfoList;
var LRResultsJson;
function OnGetChartComplete(result) {
    if (result.isSuccess) {
        if (result.hasLRResults && result.LRResultsJson.length > 0) {
            $('.logo-chart-content').html('');
            $('.logo-chart-content').append('<div class="float-left" id="logo-results" style="width:100%; padding-right:20px; padding-left:20px; background-color: #FFFFFF""></div>');
            numOfVisibleLogos = result.yAxisCompanies.length;
            yAxisInfoList = [];
            $.each(result.yAxisCompanies, function (index, value) {
                var yAxisInfo = {};
                yAxisInfo.ID = [];
                yAxisInfo.Position = [];
                yAxisInfo.ID.push(index + 1);
                yAxisInfo.Position.push(index + 1);
                yAxisInfo.yAxisCompany = result.yAxisCompanies[index];
                yAxisInfo.yAxisLogoPath = result.yAxisLogoPaths[index];

                yAxisInfoList.push(yAxisInfo);
            });
            LRResultsJson = result.LRResultsJson;

            RenderHighCharts(result.LRResultsJson, 'logo-results');
        }
        else {
            $('.logo-chart-content').append('<table style="width:100%; background-color:#c0c0c0"><tr><td>There is no available Logo data.</td></tr></table>');
        }
        
        if (result.isTimeSync && result.lineChartJson.length > 0) {
            $('.video-chart').closest('li').show();
            $('.chart-tabs').html('');
            $('.chart-tab-content').html('');
            $.each(result.lineChartJson, function (index, obj) {
                $('.chart-tabs').append('<div class="chartTabHeader" id="video-parent-tab-' + index + '"><div class="padding5" id="video-tab-' + index + '" onclick="changeChartTab(' + index + ');">' + obj.Type + '</div></div>');
                $('.chart-tab-content').append('<div class="float-left" id="video-chart-' + index + '" style="width:1000px;"></div>');
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
        $(".highcharts-container").css('left', '0px')
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
    JsonLineChart.xAxis.labels.formatter = FormatTime;
    JsonLineChart.tooltip.formatter = tooltipFormat;
    JsonLineChart.yAxis[0].labels.formatter = FormatLRBrandLabel;
    if (chartID == 'logo-results') {
        JsonLineChart.plotOptions.series.events.show = HandleSeriesLogoShowHide;
        JsonLineChart.plotOptions.series.events.hide = HandleSeriesLogoShowHide;
    }
    else {
        JsonLineChart.plotOptions.series.events.show = HandleSeriesShowHide;
        JsonLineChart.plotOptions.series.events.hide = HandleSeriesShowHide;
    }
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

function tooltipFormat() {
    var s = [];

    var totalSeconds = this.x;
    var minutes = Math.floor(totalSeconds / 60);
    var seconds = totalSeconds - minutes * 60;
    minutes = minutes < 10 ? '0' + minutes : minutes;
    seconds = seconds < 10 ? '0' + seconds : seconds;

    str = '<div style="min-height: 100px; min-width:100px;">'
    + '<p><b>' + this.point.SearchName + '</b> - ' + minutes + ':' + seconds + '</p><br/>'
    + '<div><img src="' + this.point.Medium + '" style="display: block; margin: auto; vertical-align: middle; height: auto; width:auto;"/></div></div>';
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

function FormatLRBrandLabel() {
    var label = "";
    if (this.value > 0 && this.value === parseInt(this.value, 10) && this.value <= numOfVisibleLogos) {
        var yAxisInfo = yAxisInfoList[this.value - 1];
        if (yAxisInfo.Position != 0) {
            var company = yAxisInfo.yAxisCompany;
            var logoPath = yAxisInfo.yAxisLogoPath;

            label = '<div title="' + company + '" style="width:150px; height:50px;"><img src="' + logoPath + '" style="display: block; float:right; vertical-align: middle; height: auto; width:auto;"/></div>'
        }
        else {
            label = '<div title="" style="width:150px; height:50px;"></div>'
            
        }
    }
    return label;
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

function CancelEmail() {
    $("#divEmailForm").slideUp(1000)
    $("#txtPlayerFromEmail").val('');
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
                    $("#txtPlayerFromEmail").val('');
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

var SetPlayerInfo = function (logo, title) {

    $("#video-source").attr("src", logo);
    if (typeof (title) !== 'undefined' && videotype == 'ugc') {
        $(".video-program").html(title);
    }
}

function CheckEmailAddress(email) {
    //var emailPattern = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
    var emailPattern = /^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+$/;
    return emailPattern.test(email);
}
