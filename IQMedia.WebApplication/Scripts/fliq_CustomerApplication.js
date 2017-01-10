var _Fliq_AppCustomerClientName = null;
var _Fliq_AppCustomerName = null;

var _IsAscCustomerApp = true;
var _SortColumnCustomerApp = '';

$(document).ready(function () {
    $("#txtFliq_ClientCustomer").keypress(function (e) {
        if (e.keyCode == 13) {
            SearchFliq_CustomerApplication();
        }
    });
    $("#txtFliq_ClientCustomer").blur(function () {
        SearchFliq_CustomerApplication();
    });
});

function GetFliq_CustomerApplication(appid, isNextPage) {
    var jsonPostData = {};
    if (_Fliq_AppCustomerClientName != null || _Fliq_AppCustomerName != null) {
        if (typeof (isNextPage) === 'undefined') {
            jsonPostData = {
                p_ClientName: _Fliq_AppCustomerClientName,
                p_CustomerName: _Fliq_AppCustomerName,
                p_IsAsc : _IsAscCustomerApp,
                p_SortColumn : _SortColumnCustomerApp
            };
        }
        else {
            jsonPostData = {
                p_IsNext: isNextPage,
                p_ClientName: _Fliq_AppCustomerClientName,
                p_CustomerName: _Fliq_AppCustomerName,
                p_IsAsc : _IsAscCustomerApp,
                p_SortColumn : _SortColumnCustomerApp
            };
        }
    }
    else {
        _Fliq_AppCustomerClientName = '';
        _Fliq_AppCustomerName = '';
        jsonPostData = {
            p_IsAsc: _IsAscCustomerApp,
            p_SortColumn: _SortColumnCustomerApp
        };
    }


    $.ajax({
        url: _urlFliqCustomerFliq_DisplayCustomerApplications,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {

                $("#liFliq_CustomerApplication").ActiveNav();
                $("#divFliq_CustomerApplicationPreviousNext").show();
                $(".span9-custom > div").hide();
                $("#divFliq_CustomerApplication").show();

                $("#divFliq_CustomerApplication_Content").html(result.HTML);

                $("#divFliq_CustomerApplication_ScrollContent").css("height", documentHeight - 420);


                $("#divFliq_CustomerApplication_ScrollContent").enscroll({
                    verticalTrackClass: 'track4',
                    verticalHandleClass: 'handle4',
                    margin: '0 0 0 10px'
                });

                showFliq_CustomerApplicationNoOfRecords(result);
                $("#ddlFliq_AppClients2").val(_Fliq_AppCustomerClientName);
                $("#txtFliq_ClientCustomer").val(_Fliq_AppCustomerName);
                $('#ddlFliq_AppClients2').trigger("chosen:updated");

            }
            else {
                if (typeof (isNextPage) === 'undefined') {
                    ClearResultsOnError('divFliq_CustomerApplication_Content', 'divFliq_CustomerApplicationPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetFliq_CustomerApplication(" + appid + "," + isNextPage + ")"));
                }
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            if (typeof (isNextPage) === 'undefined') {
                ClearResultsOnError('divFliq_CustomerApplication_Content', 'divFliq_CustomerApplicationPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetFliq_CustomerApplication(" + appid + "," + isNextPage + ")"));
            }
            ShowNotification(_msgErrorOccured);
        }
    });
}

function SearchFliq_CustomerApplication() {
    if (_Fliq_AppCustomerClientName != $("#ddlFliq_AppClients2").val() || _Fliq_AppCustomerName != $("#txtFliq_ClientCustomer").val().trim()) {
        _Fliq_AppCustomerClientName = $("#ddlFliq_AppClients2").val();
        _Fliq_AppCustomerName = $("#txtFliq_ClientCustomer").val().trim();
        GetFliq_CustomerApplication(0);
    }
}

function SortDirectionCustomerApp(p_SortColumn, p_IsAsc) {

    if (p_IsAsc != _IsAscCustomerApp || _SortColumnCustomerApp != p_SortColumn) {
        _IsAscCustomerApp = p_IsAsc;
        _SortColumnCustomerApp = p_SortColumn;
        SetDirectionHTMLCustomerApp();
        GetFliq_CustomerApplication(0);
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

function DeleteFliq_CustomerApplication(id) {

    var jsonPostData = {
        p_ID: id
    }

    $.ajax({
        url: _urlFliqCustomerFliq_DeleteCustomerApplication,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {
                ShowNotification(_msgCustomerApplicationDeleted);
                GetFliq_CustomerApplication(0);
            }
            else {
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured);
        }
    });


}


function GetFliq_CustomerApplicationRegistration(id) {

    var jsonPostData = {
        p_ID: id
    }

    $.ajax({
        url: _urlFliqCustomerFliq_GetCustomerApplicationRegistation,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {

                $(".span9-custom > div").hide();

                $("#divRegistration").html(result.HTML);
                $("#divRegistration").show();

                if (_Fliq_ClientName != null && _Fliq_ClientName != "") {
                    $("#customerApplication_ClientID option:contains(" + _Fliq_AppCustomerClientName + ")").attr('selected', 'selected');
                }

                $('#customerApplication_ClientID').change(function () {
                    $('#customerApplication_CustomerID').val("0");
                    GetFliq_CustomerApplicationDropDowns();
                });

            }
            else {
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured);
        }
    });
}

function CancelFliq_CustomerApplicationRegistration() {
    $(".span9-custom > div").hide();
    $("#divFliq_CustomerApplication").show();
}

function SaveFliq_CustomerApplication() {
    if (ValidateFliq_CustomerApplication()) {
        $("#frmFliq_CustomerApplication").ajaxSubmit({
            target: "",
            global: true,
            success: function (res) {
                if (res.isSuccess) {

                    if ($("#customerApplication_ID").val() == "0") {
                        ShowNotification(_msgCustomerApplicationAdded);
                    }
                    else {
                        ShowNotification(_msgCustomerApplicationUpdated);
                    }
                    ClearFliq_CustomerApplicationRegistraion();
                    GetFliq_CustomerApplication(0);
                }
                else {
                    ShowNotification(res.errorMsg);
                }
            }
        });
    }
}


function GetFliq_CustomerApplicationDropDowns() {
    var jsonPostData = {
        p_ClientID: $("#customerApplication_ClientID").val()
    }

    $.ajax({
        url: _urlFliqCustomerFliq_GetCustomerApplicationDropDowns,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {

                if (result.Fliq_Customers != null) {
                    var optCustomers = '<option value="0">Select</option>';
                    $.each(result.Fliq_Customers, function (index, item) {
                        optCustomers = optCustomers + '<option value="' + item.CustomerKey + '">' + item.FirstName + ' ' + item.LastName + '</option>';
                    });

                    $("#customerApplication_CustomerID").html(optCustomers);
                    $("#customerApplication_CustomerID").removeAttr("disabled");
                }
                

                if (result.Fliq_Application != null) {
                    var optApps = '<option value="0">Select</option>';
                    $.each(result.Fliq_Application, function (index, item) {
                        optApps = optApps + '<option value="' + item.ID + '">' + item.Application + '</option>';
                    });

                    $("#customerApplication_FliqApplicationID").html(optApps);
                    $("#customerApplication_FliqApplicationID").removeAttr("disabled");
                }
                
            }
            else {
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured);
        }
    });
}

function ClearFliq_CustomerApplicationRegistraion() {

    $("#customerApplication_ClientID").val("0");
    $("#customerApplication_CustomerID").val("0");
    $("#customerApplication_FliqApplicationID").val("0");
}

function ClearFliq_CustomerApplication() {
    if (_Fliq_AppCustomerClientName != '' || _Fliq_AppCustomerName != '') {
        _Fliq_AppCustomerClientName = ''
        _Fliq_AppCustomerName = ''
        GetFliq_CustomerApplication(0);
    }
}

function ValidateFliq_CustomerApplication() {
    var flag = true;
    var scrollID = '';

    $("#spancustomerApplication_ClientID").hide();
    $("#spancustomerApplication_CustomerID").hide();
    $("#spancustomerApplication_FliqApplicationID").hide();



    if ($("#customerApplication_ClientID").val() == "0") {
        flag = false;
        $("#spancustomerApplication_ClientID").html(_msgCustomerRegistrationClientRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spancustomerApplication_ClientID';
        }
    }


    if ($("#customerApplication_CustomerID").val() == "0") {
        flag = false;
        $("#spancustomerApplication_CustomerID").html(_msgCustomerApplicationRegistrationCustomerRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spancustomerApplication_CustomerID';
        }
    }


    if ($("#customerApplication_FliqApplicationID").val() == "0") {
        flag = false;
        $("#spancustomerApplication_FliqApplicationID").html(_msgClientApplicationRegistrationApplicationRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spancustomerApplication_FliqApplicationID';
        }
    }
    
    if (!flag) {
        setTimeout(function () { $("#divFliq_ClientApplicationRegistration_ScrollContent").mCustomScrollbar("scrollTo", "#" + scrollID); }, 100);
    }

    return flag;
}

