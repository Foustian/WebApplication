var _ClientName = null;
var _CustomerName = null

$(document).ready(function () {
    $("#txtCustomer").keypress(function (e) {
        if (e.keyCode == 13) {
            SearchCustomer();
        }
    });
    $("#txtCustomer").blur(function () {
        SearchCustomer();
    });
});


function GetCustomers(customerid, isNextPage) {
    //$("#divCustomers_Content").html("");
    var jsonPostData = {};
    if (_ClientName != null || _CustomerName != null) {
        if (typeof (isNextPage) === 'undefined') {
            jsonPostData = {
                p_ClientName: _ClientName,
                p_CustomerName : _CustomerName
            };
        }
        else {
            jsonPostData = {
                p_IsNext: isNextPage,
                p_ClientName: _ClientName,
                p_CustomerName: _CustomerName
            };
        }
    }
    else {
        _ClientName = '';
        _CustomerName = '';
    }


    $.ajax({
        url: _urlGlobalAdminDisplayCustomers,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {

                $("#liCustomer").ActiveNav();
                $("#divCustomerPreviousNext").show();
                $(".span9-custom > div").hide();

                $("#divCustomers").show();

                $("#divCustomers_Content").html(result.HTML);

                $("#divCustomers_ScrollContent").css("width", $("#divCustomers_Content").width());
                $("#divCustomers_ScrollContent").css("height", documentHeight - 420);

                /*$("#divCustomers_ScrollContent").mCustomScrollbar({
                    horizontalScroll: true,
                    advanced: {
                        autoExpandHorizontalScroll: true
                    }
                });*/

                $("#divCustomers_ScrollContent").enscroll({
                    horizontalScrolling: true,
                    verticalScrolling: true,
                    verticalTrackClass: 'track4',
                    verticalHandleClass: 'handle4',
                    horizontalTrackClass: 'horizontal-track2',
                    horizontalHandleClass: 'horizontal-handle2',
                    pollChanges: true,
                    addPaddingToPane: true,
                    margin : '0 0 0 10px'
                });

                showCustomerNoOfRecords(result);
                // Display some animation for record that is added/updated
                if (customerid > 0) {
                    //alert(iqagentkey);

                    $("#trCustomer_" + customerid).animate({ backgroundColor: "#EDB5CC" }, 1000, function () {
                        $("#trCustomer_" + customerid).animate
                                    ({
                                        backgroundColor: '#fff'
                                    }, 1500);

                    });
                }
                $("#ddlClients").val(_ClientName);
                $("#txtCustomer").val(_CustomerName);
                $('#ddlClients').trigger("chosen:updated");
            }
            else {
                if (typeof (isNextPage) === 'undefined') {
                    ClearResultsOnError('divCustomers_Content', 'divCustomerPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetCustomers(" + customerid + "," + isNextPage + ")"));
                }
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            if (typeof (isNextPage) === 'undefined') {
                ClearResultsOnError('divCustomers_Content', 'divCustomerPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetCustomers(" + customerid + "," + isNextPage + ")"));
            }
            ShowNotification(_msgErrorOccured);
        }
    });
}

function SearchCustomer() {
    if (_ClientName != $("#ddlClients").val() || _CustomerName != $("#txtCustomer").val().trim()) {
        _ClientName = $("#ddlClients").val();
        _CustomerName = $("#txtCustomer").val().trim();
        GetCustomers(0);
    }
}

function ClearCustomerSearch() {
    if (_ClientName != '' || _CustomerName != '') {
        _ClientName = ''
        _CustomerName = ''
        GetCustomers(0);
    }
}

function showCustomerNoOfRecords(result) {
    if (result.hasMoreRecords) {
        $('#btnCustomerNextPage').show();
    }
    else {
        $('#btnCustomerNextPage').hide();
    }

    if (result.hasPreviousRecords) {
        $('#btnCustomerPreviousPage').show();
    }
    else {
        $('#btnCustomerPreviousPage').hide();
    }
    if (result.recordLabel != '') {
        $('#lblCustomerRecords').html(result.recordLabel);
    }
}

function DeleteCustomer(customerId, firstName, lastName) {

    getConfirm("Confirm Delete", "Are you sure you want to delete " + EscapeHTML(firstName) + " " + EscapeHTML(lastName) + "?", "Continue", "Cancel", function (res) {
        if (res == true) {
            var jsonPostData = {
                p_CustomerKey: customerId
            }

            $.ajax({
                url: _urlGlobalAdminDeleteCustomer,
                contentType: "application/json; charset=utf-8",
                type: "post",
                dataType: "json",
                data: JSON.stringify(jsonPostData),
                success: function (result) {
                    if (result != null && result.isSuccess) {
                        ShowNotification(_msgCustomerDeleted);
                        GetCustomers(0);
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
    });

}


function GetCustomerRegistration(customerId) {
    
    var jsonPostData = {
        p_CustomerKey: customerId
    }

    $.ajax({
        url: _urlGlobalAdminGetCustomersRegistation,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {

                $(".span9-custom > div").hide();

                $("#divRegistration").html(result.HTML);
                $("#divRegistration").show();

                if (_ClientName != null && _ClientName != "" && $("#customer_ClientID").val() == "0") {
                    $("#customer_ClientID option").each(function () { if ($(this).html() == _ClientName) { $(this).attr('selected', 'selected'); } });
                }

                $('#frmCustomer input').keydown(function (e) {
                    if (e.keyCode == 13) {
                        SaveCustomer();
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

function CancelCustomerRegistration() {
    $(".span9-custom > div").hide();
    $("#divCustomers").show();
}

function SaveCustomer() {
    if (ValidateCustomer()) {
        // Values of disabled inputs are excluded from form submission, so temporarily enable every role
        $("#divCustomerRoles input[type='checkbox']").removeAttr("disabled");

        $("#frmCustomer").ajaxSubmit({
            target: "",
            success: function (res) {
                $("#divCustomerRoles input[type='checkbox']").attr("disabled", "disabled");

                HideLoading();
                if (res.isSuccess) {
                    $("#ddlClients").val($("#customer_ClientID option:selected").text());
                    $('#ddlClients').trigger("chosen:updated");

                    if ($("#customer_CustomerKey").val() == "0") {
                        ShowNotification(_msgCustomerAdded);
                    }
                    else {
                        ShowNotification(_msgCustomerUpdated);
                    }
                    ClearCustomer();
                    _ClientName = $("#ddlClients").val();
                    GetCustomers(0);
                }
                else {
                    ShowNotification(res.errorMsg);
                }
            }
        });
    }
}

function ClearCustomer() {
    $('#customer_ClientID').get(0).selectedIndex = 0;
    //$("#customer_MasterCustomerID").val("");
    $("#customer_FirstName").val("");
    $("#customer_LastName").val("");
    $("#customer_Email").val("");
    $("#customer_LoginID").val("");
    $("#customer_Password").val("");
    $("#customer_ConfirmPassword").val("");
    $("#customer_ContactNo").val("");
    $("#customer_MultiLogin").prop("checked", false);
    $("#customer_IsActive").prop("checked", true);
    $('#customer_DefaultPage').get(0).selectedIndex = 0;

    $("#divCustomerRoles input[type=checkbox]").each(function () {
        $(this).prop("checked", true);
    });

}




function CheckIsFliq(isFliq) {
    if (!isFliq) {
        $("#customer_IsFliqCustomer").prop("checked", false);
        $("#customer_IsFliqCustomer").prop("disabled", "disabled");
    }
    else {
        $("#customer_IsFliqCustomer").removeAttr("disabled");
    }
}

function ValidateCustomer() {
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
    $("#spanCustomerRoles").hide();

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

    var isChecked = false;
    $("#divCustomerRoles input[type=checkbox]").each(function () {
        if (this.checked) {
            isChecked = true;
        }
    });

    if (!isChecked) {
        $("#spanCustomerRoles").html(_msgCustomerRegistrationSelectCustomerRole).show();
        if (scrollID == '') {
            scrollID = 'spanCustomerRoles';
        }
        flag = false;
    }

    if (!flag) {
        setTimeout(function () { $("#divCustomerRegistration_ScrollContent").mCustomScrollbar("scrollTo", "#" + scrollID); }, 100);
    }

    return flag;
}

function SetRolesFromClient() {
    var clientID = $("#customer_ClientID").val();

    if (clientID > 0) {
        var jsonPostData = {
            p_ClientID: clientID
        };

        $.ajax({
            url: _urlGlobalAdminSetRolesFromClient,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                if (result.isSuccess) {
                    $("input[name='chkRoles']").prop("checked", false);

                    $.each(result.activeRoles, function (index, obj) {
                        $("#chkrole_" + obj).prop("checked", true);
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
    else {
        ShowNotification("Please select a client to copy from.");
    }
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

function AddCustomerToAnewstip(customerKey) {
    var jsonPostData = {
        customerKey: customerKey
    }

    $.ajax({
        url: _urlGlobalAdminAddCustomerToAnewstip,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess) {
                $("#btnCustomerAddToAnewstip").hide();
                $("#btnCustomerAddedToAnewstip").show();
                ShowNotification("Customer successfully added. Please ensure the Connect role is enabled.");
            }
            else {
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            console.error("AddCustomerToAnewstip error - " + c);
            ShowNotification(_msgErrorOccured);
        }
    });
}