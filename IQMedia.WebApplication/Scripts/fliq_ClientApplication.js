var _Fliq_AppClientName = null;
var _Fliq_AppClientAppName = null;

$(document).ready(function () {
    $("#txtFliq_ClientApplication").keypress(function (e) {
        if (e.keyCode == 13) {
            SearchFliq_ClientApplication();
        }
    });
    $("#txtFliq_ClientApplication").blur(function () {
        SearchFliq_ClientApplication();
    });
});


function GetFliq_ClientApplication(appid, isNextPage) {
    var jsonPostData = {};
    if (_Fliq_AppClientName != null && _Fliq_AppClientAppName != null) {
        if (typeof (isNextPage) === 'undefined') {
            jsonPostData = {
                p_ClientName: _Fliq_AppClientName,
                p_ApplicationName: _Fliq_AppClientAppName
            };
        }
        else {
            jsonPostData = {
                p_IsNext: isNextPage,
                p_ClientName: _Fliq_AppClientName,
                p_ApplicationName: _Fliq_AppClientAppName
            };
        }
    }
    else {
        _Fliq_AppClientName = '';
        _Fliq_AppClientAppName = '';
    }


    $.ajax({
        url: _urlFliqCustomerFliq_DisplayClientApplications,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {

                $("#liFliq_ClientApplication").ActiveNav();
                $("#divFliq_ClientApplicationPreviousNext").show();
                $(".span9-custom > div").hide();
                $("#divFliq_ClientApplication").show();

                $("#divFliq_ClientApplication_Content").html(result.HTML);

                $("#divFliq_ClientApplication_ScrollContent").css("height", documentHeight - 420);

                
                $("#divFliq_ClientApplication_ScrollContent").enscroll({
                    verticalTrackClass: 'track4',
                    verticalHandleClass: 'handle4',
                    margin: '0 0 0 10px'
                });

                showFliq_ClientApplicationNoOfRecords(result);
                $("#ddlFliq_AppClients").val(_Fliq_AppClientName);
                $("#txtFliq_ClientApplication").val(_Fliq_AppClientAppName);
                $('#ddlFliq_AppClients').trigger("chosen:updated");

            }
            else {
                if (typeof (isNextPage) === 'undefined') {
                    ClearResultsOnError('divFliq_ClientApplication_Content', 'divFliq_ClientApplicationPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetFliq_ClientApplication(" + appid + "," + isNextPage + ")"));
                }
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            if (typeof (isNextPage) === 'undefined') {
                ClearResultsOnError('divFliq_ClientApplication_Content', 'divFliq_ClientApplicationPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetFliq_ClientApplication(" + appid + "," + isNextPage + ")"));
            }
            ShowNotification(_msgErrorOccured);
        }
    });
}

function SearchFliq_ClientApplication() {
    if (_Fliq_AppClientName != $("#ddlFliq_AppClients").val() || _Fliq_AppClientAppName != $("#txtFliq_ClientApplication").val().trim()) {
        _Fliq_AppClientName = $("#ddlFliq_AppClients").val();
        _Fliq_AppClientAppName = $("#txtFliq_ClientApplication").val().trim();
        GetFliq_ClientApplication(0);
    }
}

function ClearSearchFliq_ClientApplication() {
    if (_Fliq_AppClientName != '' || _Fliq_AppClientAppName != '') {
        _Fliq_AppClientName = ''
        _Fliq_AppClientAppName = ''
        GetFliq_ClientApplication(0);
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

function DeleteFliq_ClientApplication(id) {

    var jsonPostData = {
        p_ID: id
    }

    $.ajax({
        url: _urlFliqCustomerFliq_DeleteClientApplication,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {
                ShowNotification(_msgClientApplicationDeleted);
                GetFliq_ClientApplication(0);
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


function GetFliq_ClientApplicationRegistration(id) {

    var jsonPostData = {
        p_ID: id
    }

    $.ajax({
        url: _urlFliqCustomerFliq_GetClientApplicationRegistation,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {


                $("#divRegistration").html(result.HTML);

                if (id == 0 && _Fliq_ClientName != null && _Fliq_ClientName != "") {
                    $("#clientApplication_ClientID option:contains(" + _Fliq_AppClientName + ")").attr('selected', 'selected');
                    BindCustomCategoryDropDown($("#clientApplication_ClientID option:contains(" + _Fliq_AppClientName + ")").val());

                }
                else if (id > 0) {
                    BindCustomCategoryDropDown($("#clientApplication_ClientID").val(), $("#editCategoryGuid").val());
                }


                $(".span9-custom > div").hide();
                $("#divRegistration").show();

                $('#frmFliq_ClientApplication input').keydown(function (e) {
                    if (e.keyCode == 13) {
                        SaveFliq_ClientApplication();
                    }
                });



                $('#clientApplication_ClientID').change(function () {
                    BindCustomCategoryDropDown(this.value);
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

function BindCustomCategoryDropDown(clientId,selectVal) {

    var jsonPostData = {
        p_ClientID: eval(clientId)
    }

    $.ajax({

        type: "post",
        dataType: "json",
        url: _urlFliqCustomerFliq_BindCustomCategoryDropDown,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPostData),
        async:false,
        global: false,
        success: function (result) {
            if (result.isSuccess && result.customCategories != null) {
                
                var categoryOptions = '<option value="0">&lt;Blank&gt;</option>';

                $.each(result.customCategories, function (eventID, eventData) {
                    categoryOptions = categoryOptions + '<option value="' + eventData.CategoryGUID + '">' + eventData.CategoryName + '</option>';
                });

                $("#clientApplication_DefaultCategory").html(categoryOptions);

                if (typeof (selectVal) != 'undefined' && selectVal != 'null' && selectVal != '') {
                    $("#clientApplication_DefaultCategory").val(selectVal);
                }
            }
        },
        error: function (a, b, c) {
            
        }
    });
}

function CancelFliq_ClientApplicationRegistration() {
    $(".span9-custom > div").hide();
    $("#divFliq_ClientApplication").show();
}

function SaveFliq_ClientApplication() {
    if (ValidateFliq_ClientApplication()) {
        $("#frmFliq_ClientApplication").ajaxSubmit({
            target: "",
            global : true,
            success: function (res) {
                if (res.isSuccess) {
                    
                    if ($("#ID").val() == "0") {
                        ShowNotification(_msgClientApplicationAdded);
                    }
                    else {
                        ShowNotification(_msgClientApplicationUpdated);
                    }
                    ClearFliq_ClientApplication();
                    GetFliq_ClientApplication(0);
                }
                else {
                    ShowNotification(res.errorMsg);
                }
            }
        });
    }
}



function ClearFliq_ClientApplication() {
    
    $("#Application").val("");
    $("#Version").val("");
    $("#Path").val("");
    $("#Description").val("");
    $("#IsActive").prop("checked", true);
}

function ValidateFliq_ClientApplication() {
    var flag = true;
    var scrollID = '';

    $("#spanclientApplication_ClientID").hide();
    $("#spanclientApplication_FliqApplicationID").hide();
    $("#spanclientApplication_FTPHost").hide();
    $("#spanclientApplication_FTPPath").hide();
    $("#spanclientApplication_FTPLoginID").hide();
    $("#spanclientApplication_FTPPwd").hide();
    $("#spanclientApplication_DefaultCategory").hide();
    $("#spanclientApplication_MaxVideoDuration").hide();
    



    if ($("#clientApplication_ClientID").val().trim() == "0") {
        flag = false;
        $("#spanclientApplication_ClientID").html(_msgCustomerRegistrationClientRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanclientApplication_ClientID';
        }
    }


    if ($("#clientApplication_FliqApplicationID").val() == "0") {
        flag = false;
        $("#spanclientApplication_FliqApplicationID").html(_msgClientApplicationRegistrationApplicationRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanclientApplication_FliqApplicationID';
        }
    }


    if ($("#clientApplication_DefaultCategory").val() == "0") {
        flag = false;
        $("#spanclientApplication_DefaultCategory").html(_msgCategoryRequired).show();
        if (scrollID == '') {
            scrollID = 'spanclientApplication_DefaultCategory';
        }
    }


    if ($("#clientApplication_FTPHost").val().trim() == "") {
        flag = false;
        $("#spanclientApplication_FTPHost").html(_msgClientApplicationRegistrationFTPHostRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanclientApplication_FTPHost';
        }
    }
    else if (!ValidateIPAddress($("#clientApplication_FTPHost").val().trim()) && !ValidateDomainName($("#clientApplication_FTPHost").val().trim())) {
        flag = false;
        $("#spanclientApplication_FTPHost").html(_msgClientApplicationRegistrationFTPHostInvalid).show();
        if (scrollID == '') {
            scrollID = 'spanclientApplication_FTPHost';
        }
    }

    if ($("#clientApplication_FTPPath").val().trim() == "") {
        flag = false;
        $("#spanclientApplication_FTPPath").html(_msgClientApplicationRegistrationFTPPathRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanclientApplication_FTPPath';
        }
    }

    if ($("#clientApplication_FTPLoginID").val().trim() == "") {
        flag = false;
        $("#spanclientApplication_FTPLoginID").html(_msgClientApplicationRegistrationFTPLoginRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanclientApplication_FTPLoginID';
        }
    }
    else if (!TestInput($("#clientApplication_FTPLoginID").val().trim())) {
        flag = false;
        $("#spanclientApplication_FTPLoginID").html(_msgClientApplicationRegistrationFTPLoginInvalid).show();
        if (scrollID == '') {
            scrollID = 'spanclientApplication_FTPLoginID';
        }
    }

    if ($("#clientApplication_FTPPwd").val().trim() == "") {
        flag = false;
        $("#spanclientApplication_FTPPwd").html(_msgClientApplicationRegistrationFTPPwdRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanclientApplication_FTPPwd';
        }
    }
    else if (!TestInput($("#clientApplication_FTPPwd").val().trim())) {
        flag = false;
        $("#spanclientApplication_FTPPwd").html(_msgClientApplicationRegistrationFTPPwdInvalid).show();
        if (scrollID == '') {
            scrollID = 'spanclientApplication_FTPPwd';
        }
    }

    if ($("#clientApplication_MaxVideoDuration").val().trim() == "") {
        flag = false;
        $("#spanclientApplication_MaxVideoDuration").html(_msgClientApplicationRegistrationMaxVideoDurationRequired).show();
        if (scrollID == '') {
            scrollID = 'spanclientApplication_MaxVideoDuration';
        }
    }
    else if (isNaN($("#clientApplication_MaxVideoDuration").val().trim()) || parseInt($("#clientApplication_MaxVideoDuration").val().trim()) <= 0 || !(eval($("#clientApplication_MaxVideoDuration").val().trim()) === parseInt($("#clientApplication_MaxVideoDuration").val().trim()))) {
        flag = false;
        $("#spanclientApplication_MaxVideoDuration").html(_msgClientApplicationRegistrationMaxVideoDurationInvalid).show();
        if (scrollID == '') {
            scrollID = 'spanclientApplication_MaxVideoDuration';
        }
    }
    
    if (!flag) {
        setTimeout(function () { $("#divFliq_ClientApplicationRegistration_ScrollContent").mCustomScrollbar("scrollTo", "#" + scrollID); }, 100);
    }

    return flag;
}