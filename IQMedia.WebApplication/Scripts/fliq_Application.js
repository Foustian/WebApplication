var _Fliq_AppName = null;
$(document).ready(function () {
    $("#txtFliq_Application").keypress(function (e) {
        if (e.keyCode == 13) {
            SearchFliq_Application();
        }
    });
    $("#txtFliq_Application").blur(function () {
        SearchFliq_Application();
    });
});

function GetFliq_Application(appid, isNextPage) {
    var jsonPostData = {};
    if (_Fliq_AppName != null) {
        if (typeof (isNextPage) === 'undefined') {
            jsonPostData = {
                p_Application: _Fliq_AppName,
            };
        }
        else {
            jsonPostData = {
                p_IsNext: isNextPage,
                p_Application: _Fliq_AppName
            };
        }
    }
    else {
        _Fliq_AppName = '';
    }


    $.ajax({
        url: _urlFliqCustomerFliq_DisplayApplications,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {

                $("#liFliq_Application").ActiveNav();
                $("#divFliq_ApplicationPreviousNext").show();
                $(".span9-custom > div").hide();
                $("#divFliq_Application").show();

                $("#divFliq_Application_Content").html(result.HTML);

                $("#divFliq_Application_ScrollContent").css("height", documentHeight - 200);

                
                $("#divFliq_Application_ScrollContent").enscroll({
                    verticalTrackClass: 'track4',
                    verticalHandleClass: 'handle4',
                    margin : '0 0 0 10px'
                });

                showFliq_ApplicationNoOfRecords(result);
                $("#txtFliq_Application").val(_Fliq_AppName);

            }
            else {
                if (typeof (isNextPage) === 'undefined') {
                    ClearResultsOnError('divFliq_Application_Content', 'divFliq_ApplicationPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetFliq_Application(" + appid + "," + isNextPage + ")"));
                }
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            if (typeof (isNextPage) === 'undefined') {
                ClearResultsOnError('divFliq_Application_Content', 'divFliq_ApplicationPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetFliq_Application(" + appid + "," + isNextPage + ")"));
            }
            ShowNotification(_msgErrorOccured);
        }
    });
}

function SearchFliq_Application() {
    if (_Fliq_AppName != $("#txtFliq_Application").val().trim()) {
        _Fliq_AppName = $("#txtFliq_Application").val().trim();
        GetFliq_Application(0);
    }
}



function ClearFliq_Application() {
    if (_Fliq_AppName != '') {
        _Fliq_AppName = ''
        GetFliq_Application(0);
    }
}

function showFliq_ApplicationNoOfRecords(result) {
    if (result.hasMoreRecords) {
        $('#btnFliq_ApplicationNextPage').show();
    }
    else {
        $('#btnFliq_ApplicationNextPage').hide();
    }

    if (result.hasPreviousRecords) {
        $('#btnFliq_ApplicationPreviousPage').show();
    }
    else {
        $('#btnFliq_ApplicationPreviousPage').hide();
    }
    if (result.recordLabel != '') {
        $('#lblFliq_ApplicationRecords').html(result.recordLabel);
    }
}

function DeleteFliq_Application(appId) {

    var jsonPostData = {
        p_ApplicationID: appId
    }

    $.ajax({
        url: _urlFliqCustomerFliq_DeleteApplication,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {
                ShowNotification(_msgApplicationDeleted);
                GetFliq_Application(0);
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


function GetFliq_ApplicationRegistration(appId) {

    var jsonPostData = {
        p_ApplicationID: appId
    }

    $.ajax({
        url: _urlFliqCustomerFliq_GetApplicationRegistation,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {

                $(".span9-custom > div").hide();

                $("#divRegistration").html(result.HTML);
                $("#divRegistration").show();

                
                $('#frmFliq_Application input').keydown(function (e) {
                    if (e.keyCode == 13) {
                        SaveFliq_Application();
                    }
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

function CancelFliq_ApplicationRegistration() {
    $(".span9-custom > div").hide();
    $("#divFliq_Application").show();
}

function SaveFliq_Application() {
    if (ValidateFliq_Application()) {
        $("#frmFliq_Application").ajaxSubmit({
            target: "",
            global : true,
            success: function (res) {
                if (res.isSuccess) {
                    
                    if ($("#ID").val() == "0") {
                        ShowNotification(_msgApplicationAdded);
                    }
                    else {
                        ShowNotification(_msgApplicationUpdated);
                    }
                    ClearFliq_ApplicationRegistration();
                    GetFliq_Application(0);
                }
                else {
                    ShowNotification(res.errorMsg);
                }
            }
        });
    }
}

function ClearFliq_ApplicationRegistration() {
    
    $("#Application").val("");
    $("#Version").val("");
    $("#Path").val("");
    $("#Description").val("");
    $("#IsActive").prop("checked", true);
}

function ValidateFliq_Application() {
    var flag = true;
    var scrollID = '';

    $("#spanApplication").hide();
    $("#spanVersion").hide();
    $("#spanPath").hide();
    $("#spanDescription").hide();

   

    if ($("#Application").val().trim() == "") {
        flag = false;
        $("#spanApplication").html(_msgApplicationRegistrationApplicationRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanApplication';
        }
    }
    else if (!TestInput($("#Application").val())) {
        flag = false;
        $("#spanApplication").html(_msgApplicationRegistrationApplicationInValid).show();
        if (scrollID == '') {
            scrollID = 'spanApplication';
        }
    }

    if ($("#Version").val().trim() == "") {
        flag = false;
        $("#spanVersion").html(_msgApplicationRegistrationVersionRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanVersion';
        }
    }
    else if (!ValidateVersion($("#Version").val())) {
        flag = false;
        $("#spanVersion").html(_msgApplicationRegistrationVersionInValid).show();
        if (scrollID == '') {
            scrollID = 'spanVersion';
        }
    }

    if ($("#Path").val().trim() == "") {
        flag = false;
        $("#spanPath").html(_msgApplicationRegistrationPathRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanPath';
        }
    }
    else if (!ValidateUrl($("#Path").val().trim())) {
        flag = false;
        $("#spanPath").html(_msgApplicationRegistrationPathInValid).show();
        if (scrollID == '') {
            scrollID = 'spanPath';
        }
    }
    
    if (!flag) {
        setTimeout(function () { $("#divFliq_ApplicationRegistration_ScrollContent").mCustomScrollbar("scrollTo", "#" + scrollID); }, 100);
    }

    return flag;
}