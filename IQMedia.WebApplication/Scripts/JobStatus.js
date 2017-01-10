var _IsAsc = false;
var _SortColumn = '';

var _IsAscJobStatus = true;
var _SortColumnJobStatus = '';

var _JobTypeID = '';
var _JobTypeDescription = '';
var _IsLoadPartial = true;

function GetJobStatus(isNextPage) {
    var jsonPostData = {};
    if (typeof (isNextPage) !== 'undefined') {
        jsonPostData = {
            p_IsAsc: _IsAscJobStatus,
            p_SortColumn: _SortColumnJobStatus,
            p_JobTypeID: _JobTypeID,
            p_IsLoadPartial: _IsLoadPartial,
            p_IsNext: isNextPage
        };
    }
    else {
        jsonPostData = {
            p_IsAsc: _IsAscJobStatus,
            p_SortColumn: _SortColumnJobStatus
        };

    }

    $.ajax({
        url: _urlSetup_DisplayJobStatusList,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {
                if (!_IsLoadPartial) {
                    $("#divSetupContent").html(result.HTML);
                }
                else {
                    $("#divPartialContent").html(result.HTML);
                }

                $("#divJobStatus_ScrollContent").css("height", documentHeight - 200);

                $("#divJobStatus_ScrollContent").enscroll({
                    verticalTrackClass: 'track4',
                    verticalHandleClass: 'handle4',
                    margin: '0 0 0 10px'
                });

                showJobStatusNoOfRecords(result);

            }
            else {
                if (typeof (isNextPage) === 'undefined') {
                    ClearResultsOnError('divSetupContent', 'divJobStatusPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetJobStatus(" + isNextPage + ")"));
                }
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            if (typeof (isNextPage) === 'undefined') {
                ClearResultsOnError('divSetupContent', 'divJobStatusPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetJobStatus(" + isNextPage + ")"));
            }
            ShowNotification(_msgErrorOccured);
        }
    });
}

function showJobStatusNoOfRecords(result) {
    if (result.hasMoreRecords) {
        $('#btnJobStatusNextPage').show();
    }
    else {
        $('#btnJobStatusNextPage').hide();
    }

    if (result.hasPreviousRecords) {
        $('#btnJobStatusPreviousPage').show();
    }
    else {
        $('#btnJobStatusPreviousPage').hide();
    }
    if (result.recordLabel != '') {
        $('#lblJobStatusRecords').html(result.recordLabel);
    }
}

function SetDirectionHTMLJobStatus() {
    if (_SortColumnJobStatus == 'RequestedBy' && _IsAscJobStatus) {
        $('#aSortDirectionJobStatus').html(_msgRequestedByAscending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_SortColumnJobStatus == 'RequestedBy' && !_IsAscJobStatus) {
        $('#aSortDirectionJobStatus').html(_msgRequestedByDescending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_SortColumnJobStatus == 'RequestedDateTime' && _IsAscJobStatus) {
        $('#aSortDirectionJobStatus').html(_msgOldestFirst + ' (Requested Date)&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_SortColumnJobStatus == 'RequestedDateTime' && !_IsAscJobStatus) {
        $('#aSortDirectionJobStatus').html(_msgMostRecent + ' (Requested Date)&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_SortColumnJobStatus == 'CompletedDateTime' && _IsAscJobStatus) {
        $('#aSortDirectionJobStatus').html(_msgOldestFirst + ' (Completed Date)&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_SortColumnJobStatus == 'CompletedDateTime' && !_IsAscJobStatus) {
        $('#aSortDirectionJobStatus').html(_msgMostRecent + ' (Completed Date)&nbsp;&nbsp;<span class="caret"></span>');
    }
    else {
        $('#aSortDirectionJobStatus').html(_msgMostRecent + ' (Requested Date)&nbsp;&nbsp;<span class="caret"></span>');
    }
}

function SortDirectionJobStatus(p_SortColumn, p_IsAsc) {
    if (p_IsAsc != _IsAscJobStatus || _SortColumnJobStatus != p_SortColumn) {
        _IsAscJobStatus = p_IsAsc;
        _SortColumnJobStatus = p_SortColumn;
        SetDirectionHTMLJobStatus();
        _IsLoadPartial = true;
        GetJobStatus(null);
    }
}

function SetJobType(p_JobTypeID, p_JobTypeDescription) {
    _JobTypeID = p_JobTypeID;
    _JobTypeDescription = p_JobTypeDescription;
    _IsLoadPartial = true;
    $("#aJobType").html(_JobTypeDescription + '&nbsp;&nbsp;<span class="caret"></span>');
    GetJobStatus(null);
}

function LoadJobStatusPartial() {
    _JobTypeID = '';
    _IsLoadPartial = false;
    GetJobStatus(null);
}

function DownloadJobStatusFile(ID) {
    var jsonPostData = { p_ID: ID }
    $.ajax({
        url: _urlJobStatus_IsExistJobStatusFile,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {
                window.location = _urlJobStatus_DownloadJobStatusFile + '?p_ID=' + ID;
            }
            else {
                ShowNotification(result.error);
            }
        },
        error: function () {
            ShowNotification(result.error);
        }
    });
}