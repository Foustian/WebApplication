var _Fliq_ClientName = null;
var _Fliq_CustomerName = null;

$(document).ready(function () {
    $("#txtFliq_Customer").keypress(function (e) {
        if (e.keyCode == 13) {
            SearchFliq_Customer();
        }
    });
    $("#txtFliq_Customer").blur(function () {
        SearchFliq_Customer();
    });
});

function GetFliq_Customers(customerid, isNextPage) {
    var jsonPostData = {};
    if (_Fliq_ClientName != null || _Fliq_CustomerName != null) {
        if (typeof (isNextPage) === 'undefined') {
            jsonPostData = {
                p_ClientName: _Fliq_ClientName,
                p_CustomerName: _Fliq_CustomerName
            };
        }
        else {
            jsonPostData = {
                p_IsNext: isNextPage,
                p_ClientName: _Fliq_ClientName,
                p_CustomerName: _Fliq_CustomerName
            };
        }
    }
    else {
        _Fliq_ClientName = '';
        _Fliq_CustomerName = '';
    }


    $.ajax({
        url: _urlFliqCustomerFliq_DisplayCustomers,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {

                $("#liFliq_Customer").ActiveNav();
                $("#divFliq_CustomerPreviousNext").show();
                $(".span9-custom > div").hide();
                $("#divFliq_Customers").show();

                $("#divFliq_Customers_Content").html(result.HTML);

                $("#divFliq_Customers_ScrollContent").css("height", documentHeight - 420);

                /*$("#divFliq_Customers_ScrollContent").mCustomScrollbar({
                    horizontalScroll: true,
                    advanced: {
                        autoExpandHorizontalScroll: true
                    }
                });*/

                $("#divFliq_Customers_ScrollContent").enscroll({
                    verticalTrackClass: 'track4',
                    verticalHandleClass: 'handle4',
                    margin: '0 0 0 10px'
                });

                showFliq_CustomerNoOfRecords(result);
                $("#ddlFliq_Clients").val(_Fliq_ClientName);
                $("#txtFliq_Customer").val(_Fliq_CustomerName);
                $('#ddlFliq_Clients').trigger("chosen:updated");
                
            }
            else {
                if (typeof (isNextPage) === 'undefined') {
                    ClearResultsOnError('divFliq_Customers_Content', 'divFliq_CustomerPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetFliq_Customers(" + customerid + "," + isNextPage + ")"));
                }
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            if (typeof (isNextPage) === 'undefined') {
                ClearResultsOnError('divFliq_Customers_Content', 'divFliq_CustomerPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetFliq_Customers(" + customerid + "," + isNextPage + ")"));
            }
            ShowNotification(_msgErrorOccured);
        }
    });
}

function SearchFliq_Customer() {
    if (_Fliq_ClientName != $("#ddlFliq_Clients").val() || _Fliq_CustomerName != $("#txtFliq_Customer").val().trim()) {
        _Fliq_ClientName = $("#ddlFliq_Clients").val();
        _Fliq_CustomerName = $("#txtFliq_Customer").val().trim();
        GetFliq_Customers(0);
    }
}

function ClearFliq_CustomerSearch() {
    if (_Fliq_ClientName != '' || _Fliq_CustomerName != '') {
        _Fliq_ClientName = ''
        _Fliq_CustomerName = ''
        GetFliq_Customers(0);
    }
}

function showFliq_CustomerNoOfRecords(result) {
    if (result.hasMoreRecords) {
        $('#btnFliq_CustomerNextPage').show();
    }
    else {
        $('#btnFliq_CustomerNextPage').hide();
    }

    if (result.hasPreviousRecords) {
        $('#btnFliq_CustomerPreviousPage').show();
    }
    else {
        $('#btnFliq_CustomerPreviousPage').hide();
    }
    if (result.recordLabel != '') {
        $('#lblFliq_CustomerRecords').html(result.recordLabel);
    }
}

function DeleteFliq_Customer(customerId) {

    var jsonPostData = {
        p_CustomerKey: customerId
    }

    $.ajax({
        url: _urlFliqCustomerFliq_DeleteCustomer,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {
                ShowNotification(_msgCustomerDeleted);
                GetFliq_Customers(0);
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


function GetFliq_CustomerRegistration(customerId) {

    var jsonPostData = {
        p_CustomerKey: customerId
    }

    $.ajax({
        url: _urlFliqCustomerFliq_GetCustomersRegistation,
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
                    $("#customer_ClientID option").each(function () { if ($(this).html() == _Fliq_ClientName) { $(this).attr('selected', 'selected'); } });
                }

                $('#frmFliq_Customer input').keydown(function (e) {
                    if (e.keyCode == 13) {
                        SaveFliq_Customer();
                    }
                });

                $('#customer_ClientID').change(function () {
                    $('#customer_ClientID').find(":selected").click();
                });

                if ($("#imgPassHelp").length > 0) {

                    $("#imgPassHelp").popover();
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

function CancelFliq_CustomerRegistration() {
    $(".span9-custom > div").hide();
    $("#divFliq_Customers").show();
}

function SaveFliq_Customer() {
    if (ValidateFliq_Customer()) {
        $("#frmFliq_Customer").ajaxSubmit({
            target: "",
            success: function (res) {
                HideLoading();
                if (res.isSuccess) {
                    $("#ddlFliq_Clients").val($("#customer_ClientID option:selected").text());
                    $('#ddlFliq_Clients').trigger("chosen:updated");

                    if ($("#customer_CustomerKey").val() == "0") {
                        ShowNotification(_msgCustomerAdded);
                    }
                    else {
                        ShowNotification(_msgCustomerUpdated);
                    }
                    ClearFliq_Customer();
                    _Fliq_ClientName = $("#ddlFliq_Clients").val();
                    GetFliq_Customers(0);
                }
                else {
                    ShowNotification(res.errorMsg);
                }
            }
        });
    }
}

function ClearFliq_Customer() {
    $('#customer_ClientID').get(0).selectedIndex = 0;
    $("#customer_FirstName").val("");
    $("#customer_LastName").val("");
    $("#customer_Email").val("");
    $("#customer_LoginID").val("");
    $("#customer_Password").val("");
    $("#customer_ConfirmPassword").val("");
    $("#customer_ContactNo").val("");
    $("#customer_IsActive").prop("checked", true);
}

function ValidateFliq_Customer() {
    var flag = true;
    var scrollID = '';

    $("#spanClientID").hide();
    $("#spanFirstName").hide();
    $("#spanLastName").hide();
    $("#spanEmail").hide();
    $("#spanLoginID").hide();
    $("#spanPassword").hide();
    $("#spanConfirmPassword").hide();
    $("#spanContactNo").hide();
    $("#spanComment").hide();


    if ($("#customer_ClientID").val().trim() == "0") {
        flag = false;
        $("#spanClientID").html(_msgCustomerRegistrationClientRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanClientID';
        }
    }


    if ($("#customer_FirstName").val().trim() == "") {
        flag = false;
        $("#spanFirstName").html(_msgCustomerRegistrationFirstNameRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanFirstName';
        }
    }
    else if (!TestInput($("#customer_FirstName").val())) {
        flag = false;
        $("#spanFirstName").html(_msgCustomerRegistrationFirstNameInValid).show();
        if (scrollID == '') {
            scrollID = 'spanFirstName';
        }
    }

    if ($("#customer_LastName").val().trim() == "") {
        flag = false;
        $("#spanLastName").html(_msgCustomerRegistrationLastNameRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanLastName';
        }
    }
    else if (!TestInput($("#customer_LastName").val())) {
        flag = false;
        $("#spanLastName").html(_msgCustomerRegistrationLastNameInValid).show();
        if (scrollID == '') {
            scrollID = 'spanLastName';
        }
    }

    if ($("#customer_Email").val().trim() == "") {
        flag = false;
        $("#spanEmail").html(_msgCustomerRegistrationEmailRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanEmail';
        }
    }
    else if (!CheckEmailAddress($("#customer_Email").val().trim())) {
        flag = false;
        $("#spanEmail").html(_msgIncorrectEmail).show();
        if (scrollID == '') {
            scrollID = 'spanEmail';
        }
    }

    if ($("#customer_LoginID").val().trim() == "") {
        flag = false;
        $("#spanLoginID").html(_msgCustomerRegistrationLoginIDRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanLoginID';
        }
    }
    else if (!CheckEmailAddress($("#customer_LoginID").val().trim())) {
        flag = false;
        $("#spanLoginID").html(_msgIncorrectEmail).show();
        if (scrollID == '') {
            scrollID = 'spanLoginID';
        }
    }

    if ($("#customer_Password").val().trim() == "") {
        if ($("#customer_CustomerKey").val() == "0") {
            flag = false;
            $("#spanPassword").html(_msgCustomerRegistrationPasswordRequiredField).show();
            if (scrollID == '') {
                scrollID = 'spanPassword';
            }
        }
    }
    else {
        if (!TestInput($("#customer_Password").val().trim()) || !ValidatePassword($("#customer_Password"))) {
            flag = false;
            $("#spanPassword").html(_msgCustomerRegistrationPasswordInValid).show();
            if (scrollID == '') {
                scrollID = 'spanPassword';
            }
        }
        if ($("#customer_ConfirmPassword").val() != $("#customer_Password").val()) {
            flag = false;
            $("#spanConfirmPassword").html(_msgCustomerRegistrationPasswordMismatch).show();
            if (scrollID == '') {
                scrollID = 'spanConfirmPassword';
            }
        }
    }

    if ($("#customer_ContactNo").val().trim() == "") {
        flag = false;
        $("#spanContactNo").html(_msgClientRegistrationPhoneRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanContactNo';
        }
    }
    else if (!TestPhoneNumber($("#customer_ContactNo").val().trim())) {
        flag = false;
        $("#spanContactNo").html(_msgClientRegistrationPhoneInValid).show();
        if (scrollID == '') {
            scrollID = 'spanContactNo';
        }
    }

    if (!flag) {
        setTimeout(function () { $("#divFliq_CustomerRegistration_ScrollContent").mCustomScrollbar("scrollTo", "#" + scrollID); }, 100);
    }

    return flag;
}

$('body').on('click', function (e) {
    $('[data-toggle="popover"]').each(function () {
        //the 'is' for buttons that trigger popups
        //the 'has' for icons within a button that triggers a popup
        if (!$(this).is(e.target) && $(this).has(e.target).length === 0 && $('.popover').has(e.target).length === 0) {
            $(this).popover('hide');
        }
    });
});