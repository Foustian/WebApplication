var _IsAsc = false;
var _SortColumn = '';

var _IsAscCustomerApp = true;
var _SortColumnCustomerApp = '';
var _Fliq_AppCustomerName = null;
var _Fliq_ClientAppName = null;

function GetClientApplication(isNextPage) {
    var jsonPostData = {};
    if (typeof (isNextPage) !== 'undefined') {
        jsonPostData = {
            p_IsNext: isNextPage,
            p_ApplicationName: _Fliq_ClientAppName
        };
    }
    else {
        jsonPostData = {           
            p_ApplicationName: _Fliq_ClientAppName
        };
    }
    
    $.ajax({
        url: _urlSetup_DisplayClientApplications,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {

                $("#divSetupContent").html(result.HTML);

                $("#divFliq_ClientApplication_ScrollContent").css("height", documentHeight - 200);


                $("#divFliq_ClientApplication_ScrollContent").enscroll({
                    verticalTrackClass: 'track4',
                    verticalHandleClass: 'handle4',
                    margin: '0 0 0 10px'
                });

                $("#txtFliq_ClientApplication").val(_Fliq_ClientAppName);

                $("#txtFliq_ClientApplication").keypress(function (e) {
                    if (e.keyCode == 13) {
                        SearchFliq_ClientApplication();
                    }
                });
                $("#txtFliq_ClientApplication").blur(function () {
                    SearchFliq_ClientApplication();
                });

                showFliq_ClientApplicationNoOfRecords(result);
            }
            else {
                if (typeof (isNextPage) === 'undefined') {
                    ClearResultsOnError('divSetupContent', 'divFliq_ClientApplicationPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetClientApplication(" + isNextPage + ")"));
                }
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            if (typeof (isNextPage) === 'undefined') {
                ClearResultsOnError('divSetupContent', 'divFliq_ClientApplicationPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetClientApplication(" + isNextPage + ")"));
            }
            ShowNotification(_msgErrorOccured);
        }
    });
}

function SearchFliq_ClientApplication() {
    if (_Fliq_ClientAppName != $("#txtFliq_ClientApplication").val().trim()) {
        _Fliq_ClientAppName = $("#txtFliq_ClientApplication").val().trim();
        GetClientApplication();
    }
}

function ClearSearchFliq_ClientApplication() {
    if (_Fliq_ClientAppName != '') {
        _Fliq_ClientAppName = ''
        GetClientApplication();
    }
}

function SortDirection(p_SortColumn, p_IsAsc) {

    if (p_IsAsc != _IsAsc || _SortColumn != p_SortColumn) {
        _IsAsc = p_IsAsc;
        _SortColumn = p_SortColumn;
        SetDirectionHTML();
        GetFliqUploads();
    }
}

function SearchFliq_CustomerApplication() {
    if (_Fliq_AppCustomerName != $("#txtFliq_ClientCustomer").val().trim()) {
        _Fliq_AppCustomerName = $("#txtFliq_ClientCustomer").val().trim();
        GetCustomerApplication();
    }
}

function ClearFliq_CustomerApplication() {
    if (_Fliq_AppCustomerName != '') {
        _Fliq_AppCustomerName = ''
        GetCustomerApplication();
    }
}

function SortDirectionCustomerApp(p_SortColumn, p_IsAsc) {

    if (p_IsAsc != _IsAscCustomerApp || _SortColumnCustomerApp != p_SortColumn) {
        _IsAscCustomerApp = p_IsAsc;
        _SortColumnCustomerApp = p_SortColumn;
        SetDirectionHTMLCustomerApp();
        GetCustomerApplication();
    }
}

function SetDirectionHTMLCustomerApp() {
    if (_SortColumnCustomerApp == 'Application' && _IsAscCustomerApp) {
        $('#aSortDirectionCustomerApp').html(_msgApplicationAscending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_SortColumnCustomerApp == 'Application' && !_IsAscCustomerApp) {
        $('#aSortDirectionCustomerApp').html(_msgApplicationDescending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_SortColumnCustomerApp == 'CustomerName' && _IsAscCustomerApp) {
        $('#aSortDirectionCustomerApp').html(_msgCustomerNameAscending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_SortColumnCustomerApp == 'CustomerName' && !_IsAscCustomerApp) {
        $('#aSortDirectionCustomerApp').html(_msgCustomerNameDescending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else {
        $('#aSortDirectionCustomerApp').html(_msgApplicationAscending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
}

function SetDirectionHTML() {
    if (_SortColumn == 'Date' && _IsAsc) {
        $('#aSortDirection').html(_msgUploadDateAscending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_SortColumn == 'Date' && !_IsAsc) {
        $('#aSortDirection').html(_msgUploadDateDescending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_SortColumn == 'CustomerName' && _IsAsc) {
        $('#aSortDirection').html(_msgCustomerNameAscending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_SortColumn == 'CustomerName' && !_IsAsc) {
        $('#aSortDirection').html(_msgCustomerNameDescending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else {
        $('#aSortDirection').html(_msgUploadDateDescending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
}

function GetCustomerApplication(isNextPage) {
    var jsonPostData = {};
    if (typeof (isNextPage) !== 'undefined') {
        jsonPostData = {
            p_CustomerName: _Fliq_AppCustomerName,
            p_IsAsc : _IsAscCustomerApp,
            p_SortColumn : _SortColumnCustomerApp,
            p_IsNext: isNextPage
        };
    }
    else {
        jsonPostData = {
            p_CustomerName: _Fliq_AppCustomerName,
            p_IsAsc : _IsAscCustomerApp,
            p_SortColumn : _SortColumnCustomerApp
        };

    }
    
    $.ajax({
        url: _urlSetup_DisplayCustomerApplications,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {

                $("#divSetupContent").html(result.HTML);

                $("#divFliq_CustomerApplication_ScrollContent").css("height", documentHeight - 200);


                $("#divFliq_CustomerApplication_ScrollContent").enscroll({
                    verticalTrackClass: 'track4',
                    verticalHandleClass: 'handle4',
                    margin: '0 0 0 10px'
                });

                $("#txtFliq_ClientCustomer").keypress(function (e) {
                    if (e.keyCode == 13) {
                        SearchFliq_CustomerApplication();
                    }
                });
                $("#txtFliq_ClientCustomer").blur(function () {
                    SearchFliq_CustomerApplication();
                });

                showFliq_CustomerApplicationNoOfRecords(result);
                $("#txtFliq_ClientCustomer").val(_Fliq_AppCustomerName);
            }
            else {
                if (typeof (isNextPage) === 'undefined') {
                    ClearResultsOnError('divSetupContent', 'divFliq_CustomerApplicationPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetCustomerApplication(" + isNextPage + ")"));
                }
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            if (typeof (isNextPage) === 'undefined') {
                ClearResultsOnError('divSetupContent', 'divFliq_CustomerApplicationPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetCustomerApplication(" + isNextPage + ")"));
            }
            ShowNotification(_msgErrorOccured);
        }
    });
}

function GetFliqUploads(isNextPage) {
    var jsonPostData = {};
    var jsonPostData = {};
    if (typeof (isNextPage) !== 'undefined') {
        jsonPostData = {
            p_IsAsc: _IsAsc,
            p_SortColumn: _SortColumn,
            p_IsNext: isNextPage
        };
    }
    else{
        jsonPostData = {
            p_IsAsc: _IsAsc,
            p_SortColumn: _SortColumn
        };
    }

    $.ajax({
        url: _urlSetup_DisplayFliqUGCUploads,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {

                $("#divSetupContent").html(result.HTML);

                $("#divFliq_FliqUploads_ScrollContent").css("height", documentHeight - 200);


                $("#divFliq_FliqUploads_ScrollContent").enscroll({
                    verticalTrackClass: 'track4',
                    verticalHandleClass: 'handle4',
                    margin: '0 0 0 10px'
                });

                showFliq_FliqUploadsNoOfRecords(result);

            }
            else {
                if (typeof (isNextPage) === 'undefined') {
                    ClearResultsOnError('divSetupContent', 'divFliq_UGCUploadPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetFliqUploads(" + isNextPage + ")"));
                }
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            if (typeof (isNextPage) === 'undefined') {
                ClearResultsOnError('divSetupContent', 'divFliq_UGCUploadPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetFliqUploads(" + isNextPage + ")"));
            }
            ShowNotification(_msgErrorOccured);
        }
    });
}

function showFliq_FliqUploadsNoOfRecords(result) {
    if (result.hasMoreRecords) {
        $('#btnFliq_UGCUploadNextPage').show();
    }
    else {
        $('#btnFliq_UGCUploadNextPage').hide();
    }

    if (result.hasPreviousRecords) {
        $('#btnFliq_UGCUploadPreviousPage').show();
    }
    else {
        $('#btnFliq_UGCUploadPreviousPage').hide();
    }
    if (result.recordLabel != '') {
        $('#lblFliq_UGCUploadRecords').html(result.recordLabel);
    }
}

function showFliq_ClientApplicationNoOfRecords(result) {
    if (result.hasMoreRecords) {
        $('#btnFliq_ClientApplicationNextPage').show();
    }
    else {
        $('#btnFliq_ClientApplicationNextPage').hide();
    }

    if (result.hasPreviousRecords) {
        $('#btnFliq_ClientApplicationPreviousPage').show();
    }
    else {
        $('#btnFliq_ClientApplicationPreviousPage').hide();
    }
    if (result.recordLabel != '') {
        $('#lblFliq_ClientApplicationRecords').html(result.recordLabel);
    }
}

function showFliq_CustomerApplicationNoOfRecords(result) {
    if (result.hasMoreRecords) {
        $('#btnFliq_CustomerApplicationNextPage').show();
    }
    else {
        $('#btnFliq_CustomerApplicationNextPage').hide();
    }

    if (result.hasPreviousRecords) {
        $('#btnFliq_CustomerApplicationPreviousPage').show();
    }
    else {
        $('#btnFliq_CustomerApplicationPreviousPage').hide();
    }
    if (result.recordLabel != '') {
        $('#lblFliq_CustomerApplicationRecords').html(result.recordLabel);
    }
}