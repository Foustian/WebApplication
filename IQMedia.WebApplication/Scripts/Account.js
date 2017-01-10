var GetChangePwdFrm = function () {

    $.ajax({
        url: _urlGlobalAdmin_ChangePwd,
        contentType: "application/json; charset=utf-8",
        type: "get",
        success: function (result) {

            $("#divSetupContent").html(result);
        },
        error: function () {
            ShowNotification(_msgErrorOccured);
        }
    });
}

var ChangePwd = function () {

    if (ValidateAcntChPwd()) {

        $("#frmPwd").ajaxSubmit({
            target: "",
            success: function (result) {

                $("#divSetupContent").html(result);

            },
            error: function () {

                ShowNotification(_msgErrorOccured);

            }
        });
    }
}

var RegRsetPwdClick = function () {

    $("#btnSubmitRsetPwd2").click(function () { RsetPwd2(); });
}

var RsetPwd = function () {

    if (ValidateAcntRsetPwd()) {

        $("#frmRsetPwd").submit();
    }
}

var RsetPwd2 = function () {

    $("#btnSubmitRsetPwd2").unbind("click");

    if (ValidateAcntRsetPwd2()) {

        $("#frmRsetPwd").submit();
    }
    else {

        RegRsetPwdClick();
    }
}

var ValidateAcntRsetPwd2 = function () {

    var isValid = false;

    $("#spanPwdError").html("").hide();

    var lID = $("#p_LoginID").val().trim();    
    var pwd = $("#p_Pwd").val().trim();
    var cnfmPwd = $("#p_CnfmPwd").val().trim();

    if (lID == "" || pwd =="" || cnfmPwd == "") {
        $("#spanPwdError").html(_msgAcntRsetPwdRequired).show();
    }
    else if (!CheckEmailAddress(lID)) {
        $("#spanPwdError").html(_msgAcntRsetPwdEmail).show();
    }
    else if ((!TestInput(pwd)) || (!TestInput(cnfmPwd))) {
        $("#spanPwdError").html(_msgAcntRsetPwdInvalid).show();
    }
    else if(pwd != cnfmPwd)
    {
        $("#spanPwdError").html(_msgAcntChPwdConfirmPwdNotMtch).show();
    }
    else if (!ValidatePassword($("#p_Pwd"))) {
        $("#spanPwdError").html(_msgAcntChPwdCriteria).show();
    }
    else {
        isValid = true;
    }

    return isValid;
}

var ValidateAcntRsetPwd = function () {

    var isValid = false;

    $("#spanPwdError").html("").hide();

    var lID = $("#p_LoginID").val().trim();    

    if (lID == "") {
        $("#spanPwdError").html(_msgAcntRsetPwdRequired).show();
    }
    else if (!CheckEmailAddress(lID)) {
        $("#spanPwdError").html(_msgAcntRsetPwdEmail).show();
    }    
    else {
        isValid = true;
    }

    return isValid;
}

var ValidateAcntChPwd = function () {

    var isValid = false;

    $("#spanPwdError").html("").hide();

    var currentPwd = $("#pwd_Current").val().trim();
    var newPwd = $("#pwd_New").val().trim();
    var cnfmPwd = $("#pwd_Confirm").val().trim();

    if (currentPwd == "" || newPwd == "" || cnfmPwd == "") {

        $("#spanPwdError").html(_msgAcntChPwdRequired).show();

    }
    else if (!(newPwd == cnfmPwd)) {
        $("#spanPwdError").html(_msgAcntChPwdConfirmPwdNotMtch).show();
    }
    else if (!TestInput(currentPwd) || !TestInput(newPwd)) {
        $("#spanPwdError").html(_msgAcntChPwdInvalid).show();
    }
    else if (!ValidatePassword($("#pwd_New"))) {
        $("#spanPwdError").html(_msgAcntChPwdCriteria).show();
    }
    else if (currentPwd == newPwd) {
        $("#spanPwdError").html(_msgAcntChPwdSame).show();
    }
    else {

        isValid = true;
    }

    return isValid;

}

function TestInput(inputval) {
    if (/^[0-9a-z\s][^<>]*$/i.test(inputval)) {
        return true;
    }
    else {
        return false;
    }
}

$(function () {
    $("#p_LoginID").focus();
    RegRsetPwdClick();
    $("#frmRsetPwd input").keypress(function (e) {
        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
            $('#btnSubmitRsetPwd2').click();
            return true;
        }
    });
});