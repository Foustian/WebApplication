var st;
var pn = 0;

var _urlIFrameMicrosite = window.location;
var _urlIFrameMicrositeDownloadClip = "/IFrameMicrosite/DownloadClip/" + window.location.search;
var _urlIFrameMicrositeLoadClipPlayer = "/IFrameMicrosite/LoadClipPlayer/" + window.location.search;

var _msgErrorOccured = "Some error occured, try again later";

function ShowClipDetail(divID) {

    $('#' + divID).show();
}

function HideClipDetail(divID) {

    $('#' + divID).hide();
}

function GetResult() {

    var jsonPostData = {
        st: st,
        pn: pn
    }

    $.ajax({

        type: 'POST',
        dataType: 'json',
        url: _urlIFrameMicrosite,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),

        success: OnResultComplete,
        error: OnResultFail
    });
}

function IFrameMicrositeResultPaging(isNext) {

    if (isNext) {
        pn = pn + 1;
    }
    else {
        pn = pn - 1;
    }

    GetResult();
}

function OnResultComplete(result) {
    if (result.isSuccess) {
        $('#divResult').html(result.HTML);
        if (result.hasMoreResults) {
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

        if (result.MaxWidth) {
            $('#divPreviousNext').width(result.MaxWidth);
            $('#divMainContent').width(result.MaxWidth);
            $('#divMainContent').css('margin', '0 auto');
        }

    }
    else {
        ShowNotification(_msgErrorOccured);
    }
}

function OnResultFail(result) {
    ShowNotification(_msgErrorOccured);
}


function SetSearchTerm(e) {

    if (e.keyCode == 13) {
        if ($('#txtKeyword').val().trim()) {
            st = $('#txtKeyword').val().trim();
            GetResult();
        }
    }
}

function ClearSearch() {
    $('#txtKeyword').val('');
    st = '';
    pn = 0;
    GetResult();
}

function ShowHidedivCaption(needToshow) {
    if (needToshow) {
        $('#divCaption').show('slow');
    }
    else {
        $('#divCaption').hide('slow');
    }
}


function LoadPlayerbyGuid(itemGuid) {

    var jsonPostData = { ClipID: itemGuid }

    $.ajax({

        type: 'POST',
        dataType: 'json',
        url: _urlIFrameMicrositeLoadClipPlayer + window.location.search,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),

        success: OnLoadPlayerComplete,
        error: OnFail
    });
}

function OnLoadPlayerComplete(result) {
    if (result.isSuccess) {
        if (result.isRedirect) {
            //var playerwindow = window.open(result.RedirectUrl, "_blank");
            //popupBlockerChecker.check(playerwindow);

            var videohtml = '<video  id="myvideo" height="270" width="480" controls autoplay ></video>';
            //var iframehtml = '<iframe frameborder="0" style="width:545px;height:400px" src="' + result.RedirectUrl + '?rel=1&amp;autoplay=1" allowfullscreen="1" mozallowfullscreen="1" webkitallowfullscreen="1"></iframe>';
            $('#divPlayer').html(videohtml);
            $('#divPlayer').show('slow');

            var vdo = document.getElementById('myvideo');
            vdo.src = result.RedirectUrl;
            vdo.load(); // video must be loaded
            vdo.play(); // after success load it can be played
        }
        else {
            $('#divPlayer').html(result.clipHTML);
            $('#divPlayer').show('slow');
            $('#divCaptionContent').html(result.closedCaption);
            $('#divCaptionHeader').show('slow');
        }
    }
    else {
        ShowNotification(_msgErrorOccured);
    }
}

function OnFail(result) {
    ShowNotification(_msgErrorOccured);
}

function IFrameMicrositeDownloadClip(clipGuid, clipTitle) {
    var jsonPostData = {
        p_ClipGuid: clipGuid
    }

    $.ajax({

        type: 'POST',
        dataType: 'json',
        url: _urlIFrameMicrositeDownloadClip,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),

        success: function (result) {
            if (result.isSuccess) {
                window.location = _urlIFrameMicrositeDownloadClip + '&p_ClipGUID=' + clipGuid + '&p_ClipTitle=' + clipTitle + '';
            }
            else {
                ShowNotification(result.message);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured);
        }
    });
}

function OnDownloadComplete(result) {
    if (result.isSuccess) {
        ShowNotification(result.message);
    }
    else {
        ShowNotification(result.message);
    }

}

function OnDownloadFail(result) {
    ShowNotification(_msgErrorOccured);
}

function IsPopupBlocked(newWin, callback) {
    var result = false;
    if (!newWin || newWin.closed || typeof newWin.closed == 'undefined') {
        result = true;
    }
    callback(result);
}

var callback = function (value) {
    if (value == true) {
        alert('Popup is Blocked');
    }
    else {
    }
};

var popupBlockerChecker = {
    check: function (popup_window) {
        var _scope = this;
        if (popup_window) {
            if (/chrome/.test(navigator.userAgent.toLowerCase())) {
                setTimeout(function () {
                    _scope._is_popup_blocked(_scope, popup_window);
                }, 200);
            } else {
                popup_window.onload = function () {
                    _scope._is_popup_blocked(_scope, popup_window);
                };
            }
        } else {
            _scope._displayError();
        }
    },
    _is_popup_blocked: function (scope, popup_window) {
        if ((popup_window.innerHeight > 0) == false) { scope._displayError(); }
    },
    _displayError: function () {
        alert("Popup Blocker is enabled! Please add this site to your exception list.");
    }
};



