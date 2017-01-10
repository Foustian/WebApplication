var _SearchTerm = '';

$(document).ready(function () {

    $("#txtClients").keypress(function (e) {
        if (e.keyCode == 13) {
            $("#txtClients").autocomplete("close");
            SearchClient();
        }
    });
    $("#txtClients").blur(function () {
        $("#txtClients").autocomplete("close");
        SearchClient();
    });
});

function GetClients(clientid, isNextPage) {
    //$("#divClients_Content").html("");

    var jsonPostData = {};
    if (typeof (isNextPage) === 'undefined') {
        if (_SearchTerm == '') {
            jsonPostData = {};
        }
        else {
            jsonPostData = {
                p_ClientName: _SearchTerm
            };
        }
    }
    else {
        if (_SearchTerm == '') {
            jsonPostData = {
                p_IsNext: isNextPage
            }
        }
        else {
            jsonPostData = {
                p_IsNext: isNextPage,
                p_ClientName: _SearchTerm
            }
        }
    }

    $.ajax({
        url: _urlGlobalAdminDisplayClients,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {

                $("#liClient").ActiveNav();
                $("#divPreviousNext").show();
                $(".span9-custom > div").hide();
                $("#divClients").show();
                $("#divClients_Content").html(result.HTML);
                showNoOfRecords(result);

                $("#divClients_ScrollContent").css("width", $("#divClients_Content").width());

                $("#divClients_ScrollContent").mCustomScrollbar({
                    horizontalScroll: true,
                    advanced: {
                        autoExpandHorizontalScroll: true
                    }
                });
                // Display some animation for record that is added/updated
                if (clientid > 0) {

                    $("#trClient_" + clientid).animate({ backgroundColor: "#EDB5CC" }, 1000, function () {
                        $("#trClient_" + clientid).animate
                                        ({
                                            backgroundColor: '#fff'
                                        }, 1500);

                    });
                }
                $("#txtClients").val(_SearchTerm);
            }
            else {
                if (typeof (isNextPage) === 'undefined') {
                    ClearResultsOnError('divClients_Content', 'divPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetClients(" + clientid + "," + isNextPage + ")"));
                }
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            if (typeof (isNextPage) === 'undefined') {
                ClearResultsOnError('divClients_Content', 'divPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetClients(" + clientid + "," + isNextPage + ")"));
            }
            ShowNotification(_msgErrorOccured);
        }
    });
}

function SearchClient() {
    if ($("#txtClients").val().trim() != "" && _SearchTerm != $("#txtClients").val().trim()) {
        _SearchTerm = $("#txtClients").val().trim();
        GetClients(0);
    }

}

function ClearSearchClient() {
    _SearchTerm = '';
    GetClients(0);
}

function CancelClientRegistration() {
    $(".span9-custom > div").hide();
    $("#divClients").show();
}

function showNoOfRecords(result) {
    if (result.hasMoreRecords) {
        $('#btnNextPage').show();
    }
    else {
        $('#btnNextPage').hide();
    }

    if (result.hasPreviousRecords) {
        $('#btnPreviousPage').show();
    }
    else {
        $('#btnPreviousPage').hide();
    }
    if (result.recordLabel != '') {
        $('#lblRecords').html(result.recordLabel);
    }
}

function DeleteClient(clientId, clientName) {

    getConfirm("Confirm Delete", "Are you sure you want to delete the " + EscapeHTML(clientName) + " client?", "Continue", "Cancel", function (res) {
        if (res == true) {
            var jsonPostData = {
                p_ClientKey: clientId
            }

            $.ajax({
                url: _urlGlobalAdminGetDeleteClient,
                contentType: "application/json; charset=utf-8",
                type: "post",
                dataType: "json",
                data: JSON.stringify(jsonPostData),
                success: function (result) {
                    if (result != null && result.isSuccess) {
                        ShowNotification(_msgClientDeleted);
                        GetClients(0);
                        FillClientList();
                        FillFliqClientList();
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

function GetClientRegistration(clientId) {

    var jsonPostData = {
        p_ClientKey: clientId
    }

    $.ajax({
        url: _urlGlobalAdminGetClientRegistation,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {

                $(".span9-custom > div").hide();
                
                $("#divRegistration").html(result.HTML);
                $("#hdnClientKey").val(clientId);

                $("#divRegistration").show();


                SetFileSelection(true);
                $("#txtMasterClient").autocomplete({
                    source: masterClients
                });
                if (clientId > 0) {
                    $("#aCustomHeader").rlightbox();
                    $("#aPlayerLogo").rlightbox();
                    $("#aNotificationHeader").rlightbox();
                }

                $('#frmClient input').keydown(function (e) {
                    if (e.keyCode == 13) {
                        SaveClient();
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

function SetFileSelection(isInit) {

    if (!IsBrowserIE()) {
        $("#divRegistration input[type=file]").val("");

        $("#btnPlayerLogoBrowseFile").show();
        $("#txtPlayerLogoSelectedFileDisplay").show();
        $("#flPlayerLogo").hide();
    }
    else {
        $("#flPlayerLogo").replaceWith($("#flPlayerLogo").clone(true));
        $("#txtPlayerLogoSelectedFileDisplay").hide();
        $("#flPlayerLogo").show();
        $("#btnPlayerLogoBrowseFile").hide();
    }

    $("#txtPlayerLogoSelectedFileDisplay").val("");
    
    $("#hfPlayerLogoImage").val("");

    $("#spanPlayerLogo").html("").hide();
    
    if (isInit) {
        $("#btnPlayerLogoBrowseFile").click(function () { $("#flPlayerLogo").trigger('click'); });
        $("#flPlayerLogo").change(function () {

            var selectedfileType = $("#flPlayerLogo").val().substring($("#flPlayerLogo").val().lastIndexOf('.') + 1).toLowerCase();
            if ($.inArray(selectedfileType, imageFileTypes) != -1) {
                $("#txtPlayerLogoSelectedFileDisplay").val($(this).val());
                $("#spanPlayerLogo").html("").hide();
            }
            else {
                $("#txtPlayerLogoSelectedFileDisplay").val("");
                $("#spanPlayerLogo").html(_msgSelectValidUGCFileType).show();
            }
        });
    }
}

var xhr;
var xhrIE;
var IsUploadActive = false;
function SaveClient() {
    if (ValidateClient()) {
        // Values of disabled inputs are excluded from form submission, so temporarily enable every role
        $("#divRoles input[type='checkbox']").removeAttr("disabled");

        if (!IsBrowserIE()) {
            IsUploadActive = true;
            xhr = new XMLHttpRequest();

            ShowLoading();

            var formdata = new FormData($('#frmClient')[0]);

            xhr.addEventListener("progress", function (e) {
                var done = e.position || e.loaded, total = e.totalSize || e.total;
            }, false);

            if (xhr.upload) {
                xhr.upload.onprogress = function (e) {
                    var done = e.position || e.loaded, total = e.totalSize || e.total;
                    //$("#divProgressbar").progressbar("value", (Math.floor(done / total * 1000) / 10));
                };
            }

            xhr.addEventListener("error", function (evt) {
                IsUploadActive = false;
                CancelFileUpload();
                ShowNotification(_msgErrorWhileSavingUGCContent);
            }, false);

            xhr.addEventListener("load", function (e) {
            }, false);

            xhr.onreadystatechange = function (e) {
                if (4 == this.readyState) {
                    $("#divRoles input[type='checkbox']").attr("disabled", "disabled");
                    IsUploadActive = false;
                    var res = { success: false, error: "", clientId: "0" };

                    // chrome not allow directly to access json object, so we need to parse JSON and use afterwards. 
                    // Firefox allows json object so no need to do same.

                    if (this.response.isSuccess == undefined) {
                        var jsonObj = $.parseJSON(this.response);
                        res.success = jsonObj.isSuccess;
                        res.error = jsonObj.errorMsg;
                        res.clientId = jsonObj.clientId;
                    }
                    else {
                        res.success = this.response.isSuccess;
                        res.error = this.response.errorMsg;
                    }

                    //$("#divProgressbar").hide();
                    CancelFileUpload();
                    if (res.success == true) {
                        if ($("#hdnClientKey").val() == "0") {
                            ShowNotification(_msgClientAdded);
                            ClearClient();
                        }
                        else {
                            ShowNotification(_msgClientUpdated);
                            ClearClient();
                        }
                        GetClients(res.clientId);
                        FillClientList();
                        FillFliqClientList();
                    }
                    else {
                        ShowNotification(res.error);
                        HideLoading();
                    }
                }
            };

            xhr.open("post", "/GlobalAdmin/ClientRegistration/", true);
            xhr.responseType = "json";
            xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
            xhr.send(formdata);
        }
        else {
            IsUploadActive = true;

            if ($("#flPlayerLogo").val() == "") {
                $("#hfPlayerLogoImage").val($("#txtPlayerLogoSelectedFileDisplay").val());
            }
            else {
                $("#hfPlayerLogoImage").val("");
            }

            $("#frmClient").ajaxSubmit({
                target: "",
                beforeSend: function (jqXHR, settings) {
                    xhrIE = jqXHR;
                },
                success: function (res) {
                    $("#divRoles input[type='checkbox']").attr("disabled", "disabled");
                    IsUploadActive = false;
                    //$("#divIEProgressbar").hide();
                    CancelFileUpload();
                    var obj = $.parseJSON(res);
                    if (obj.isSuccess) {
                        ClearClient();
                        GetClients(obj.clientId);
                        FillClientList();
                        FillFliqClientList();
                    }
                    else {
                        ShowNotification(obj.errorMsg);
                        HideLoading();
                    }
                }
            })
        }
    }
}

function CancelFileUpload() {
    if (IsUploadActive) {
        if (IsBrowserIE()) {
            xhrIE.abort();
        }
        else {
            xhr.abort();
        }
    }

    SetFileSelection(false);

}

function ValidateClient() {
    var flag = true;
    var scrollID = '';
    $("#spanClientName").hide();
    $("#spanAddress1").hide();
    $("#spanCity").hide();
    $("#spanState").hide();
    $("#spanZip").hide();
    $("#spanAttention").hide();
    $("#spanPhone").hide();
    $("#spanIndustry").hide();
    $("#spanBillType").hide();
    $("#spanBillFrequency").hide();

    $("#spanPricingCode").hide();
    $("#spanNoOfUsers").hide();
    $("#spanNoOfNotification").hide();
    $("#spanNoOfIQAgent").hide();

    $("#spanCompeteMultiplier").hide();
    $("#spanOnlineNewsAdRate").hide();
    $("#spanOtherOnlineAdRate").hide();
    $("#spanURLPercentRead").hide();
    $("#spanClientRole").hide();

    $("#spanTimeZone").hide();
    $("#spanNotificationHeaderImage").hide();
    $("#spanMultiplier").hide();
    $("#spanCompeteAudienceMultiplier").hide();
    $("#spanv4MaxFeedsReportItems").hide();
    $("#spanv4MaxFeedsExportItems").hide();
    $("#spanv4MaxLibraryEmailReportItems").hide();
    $("#spanv4MaxLibraryReportItems").hide();
    $("#spanvisibleLRIndustries").hide();
    $("#spanv4MaxDiscoveryReportItems").hide();
    $("#spanv4MaxDiscoveryExportItems").hide();
    $("#spanv4MaxDiscoveryHistory").hide();

    $("#spanTVHighThreshold").hide();
    $("#spanTVLowThreshold").hide();
    $("#spanNMHighThreshold").hide();
    $("#spanNMLowThreshold").hide();
    $("#spanSMHighThreshold").hide();
    $("#spanSMLowThreshold").hide();
    $("#spanTwitterHighThreshold").hide();
    $("#spanTwitterLowThreshold").hide();
    $("#spanPQHighThreshold").hide();
    $("#spanPQLowThreshold").hide();
    
    

    if ($("#txtClientName").val().trim() == "") {
        flag = false;
        $("#spanClientName").html(_msgClientRegistrationClientNameRequiredField).show();
        scrollID = 'spanClientName';
    }
    else if (!TestInput($("#txtClientName").val())) {
        flag = false;
        $("#spanClientName").html(_msgClientRegistrationClientNameInValid).show();
        scrollID = 'spanClientName';
    }

    if ($("#ddlTimeZone").val().trim() == "0") {
        flag = false;
        $("#spanTimeZone").html(_msgClientRegistrationTimeZoneRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanTimeZone';
        }
    }

    if ($("#txtAddress1").val().trim() == "") {
        flag = false;
        $("#spanAddress1").html(_msgClientRegistrationAddress1RequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanAddress1';
        }
    }
    else if (!TestInput($("#txtAddress1").val())) {
        flag = false;
        $("#spanAddress1").html(_msgClientRegistrationAddress1InValid).show();
        if (scrollID == '') {
            scrollID = 'spanAddress1';
        }
    }

    if ($("#txtCity").val().trim() == "") {
        flag = false;
        $("#spanCity").html(_msgClientRegistrationCityRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanCity';
        }
    }
    else if (!TestInput($("#txtCity").val())) {
        flag = false;
        $("#spanCity").html(_msgClientRegistrationCityInValid).show();
        if (scrollID == '') {
            scrollID = 'spanCity';
        }
    }

    if ($("#ddlState").val().trim() == "0") {
        flag = false;
        $("#spanState").html(_msgClientRegistrationStateRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanState';
        }
    }

    if ($("#txtZip").val() == "") {
        flag = false;
        $("#spanZip").html(_msgClientRegistrationZipRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanZip';
        }
    }
    else if (!TestZipCode($("#txtZip").val())) {
        flag = false;
        $("#spanZip").html(_msgClientRegistrationZipInValid).show();
        if (scrollID == '') {
            scrollID = 'spanZip';
        }
    }

    if ($("#txtAttention").val().trim() == "") {
        flag = false;
        $("#spanAttention").html(_msgClientRegistrationAttentionRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanAttention';
        }
    }
    else if (!TestInput($("#txtAttention").val())) {
        flag = false;
        $("#spanAttention").html(_msgClientRegistrationAttentionInValid).show();
        if (scrollID == '') {
            scrollID = 'spanAttention';
        }
    }

    if ($("#txtPhone").val().trim() == "") {
        flag = false;
        $("#spanPhone").html(_msgClientRegistrationPhoneRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanPhone';
        }
    }
    else if (!TestPhoneNumber($("#txtPhone").val())) {
        flag = false;
        $("#spanPhone").html(_msgClientRegistrationPhoneInValid).show();
        if (scrollID == '') {
            scrollID = 'spanPhone';
        }
    }

    if ($("#ddlIndustry").val().trim() == "0") {
        flag = false;
        $("#spanIndustry").html(_msgClientRegistrationIndustryRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanIndustry';
        }
    }

    if ($("#ddlBillType").val().trim() == "0") {
        flag = false;
        $("#spanBillType").html(_msgClientRegistrationBillTypeRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanBillType';
        }
    }

    if ($("#ddlBillFrequency").val().trim() == "0") {
        flag = false;
        $("#spanBillFrequency").html(_msgClientRegistrationBillFrequencyRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanBillFrequency';
        }
    }

    if ($("#ddlPricingCode").val().trim() == "0") {
        flag = false;
        $("#spanPricingCode").html(_msgClientRegistrationPricingCodeRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanPricingCode';
        }
    }

    if ($("#txtNoOfUsers").val().trim() == "") {
        flag = false;
        $("#spanNoOfUsers").html(_msgClientRegistrationNoOfUsersRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanNoOfUsers';
        }
    }
    else if (isNaN($("#txtNoOfUsers").val().trim()) || parseInt($("#txtNoOfUsers").val().trim()) <= 0 || !(eval($("#txtNoOfUsers").val().trim()) === parseInt($("#txtNoOfUsers").val().trim()))) {
        flag = false;
        $("#spanNoOfUsers").html(_msgClientRegistrationNoOfUsersInValid).show();
        if (scrollID == '') {
            scrollID = 'spanNoOfUsers';
        }
    }

    if ($("#txtNoOfNotification").val().trim() == "") {
        flag = false;
        $("#spanNoOfNotification").html(_msgClientRegistrationNoOfNotificationRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanNoOfNotification';
        }
    }
    else if (isNaN($("#txtNoOfNotification").val().trim()) || !(eval($("#txtNoOfNotification").val().trim()) === parseInt($("#txtNoOfNotification").val().trim()))) {
        flag = false;
        $("#spanNoOfNotification").html(_msgClientRegistrationNoOfNotificationInValid).show();
        if (scrollID == '') {
            scrollID = 'spanNoOfNotification';
        }
    }

    if ($("#txtNoOfIQAgent").val().trim() == "") {
        flag = false;
        $("#spanNoOfIQAgent").html(_msgClientRegistrationNoOfIQAgentRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanNoOfIQAgent';
        }
    }
    else if (isNaN($("#txtNoOfIQAgent").val().trim()) || !(eval($("#txtNoOfIQAgent").val().trim()) === parseInt($("#txtNoOfIQAgent").val().trim()))) {
        flag = false;
        $("#spanNoOfIQAgent").html(_msgClientRegistrationNoOfIQAgentInValid).show();
        if (scrollID == '') {
            scrollID = 'spanNoOfIQAgent';
        }
    }

    if ($("#txtCompeteMultiplier").val().trim() == "") {
        flag = false;
        $("#spanCompeteMultiplier").html(_msgClientRegistrationCompeteMultiplierRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanCompeteMultiplier';
        }
    }
    else if (isNaN($("#txtCompeteMultiplier").val().trim())) {
        flag = false;
        $("#spanCompeteMultiplier").html(_msgClientRegistrationCompeteMultiplierInValid).show();
        if (scrollID == '') {
            scrollID = 'spanCompeteMultiplier';
        }
    }

    if ($("#txtOnlineNewsAdRate").val().trim() == "") {
        flag = false;
        $("#spanOnlineNewsAdRate").html(_msgClientRegistrationOnlineNewsAdRateRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanOnlineNewsAdRate';
        }
    }
    else if (isNaN($("#txtOnlineNewsAdRate").val().trim())) {
        flag = false;
        $("#spanOnlineNewsAdRate").html(_msgClientRegistrationOnlineNewsAdRateInValid).show();
        if (scrollID == '') {
            scrollID = 'spanOnlineNewsAdRate';
        }
    }

    if ($("#txtOtherOnlineAdRate").val().trim() == "") {
        flag = false;
        $("#spanOtherOnlineAdRate").html(_msgClientRegistrationOtherOnlineAdRateRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanOtherOnlineAdRate';
        }
    }
    else if (isNaN($("#txtOtherOnlineAdRate").val().trim())) {
        flag = false;
        $("#spanOtherOnlineAdRate").html(_msgClientRegistrationOtherOnlineAdRateInValid).show();
        if (scrollID == '') {
            scrollID = 'spanOtherOnlineAdRate';
        }
    }

    if ($("#txtURLPercentRead").val().trim() == "") {
        flag = false;
        $("#spanURLPercentRead").html(_msgClientRegistrationURLPercentReadRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanURLPercentRead';
        }
    }
    else if (isNaN($("#txtURLPercentRead").val().trim())) {
        flag = false;
        $("#spanURLPercentRead").html(_msgClientRegistrationURLPercentReadRequiredInValid).show();
        if (scrollID == '') {
            scrollID = 'spanURLPercentRead';
        }
    }

    if ($("#flPlayerLogo").val().trim() != "") {
        var selectedfileType = $("#flPlayerLogo").val().substring($("#flPlayerLogo").val().lastIndexOf('.') + 1).toLowerCase();
        if ($.inArray(selectedfileType, imageFileTypes) == -1) {
            $("#txtPlayerLogoSelectedFileDisplay").val("");
            $("#spanPlayerLogo").html(_msgSelectValidUGCFileType).show();
            if (scrollID == '') {
                scrollID = 'spanPlayerLogo';
            }
        }
    }
    
    if ($("#txtMultiplier").val().trim() == "") {
        flag = false;
        $("#spanMultiplier").html(_msgClientRegistrationMultiplierRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanMultiplier';
        }
    }
    else if (isNaN($("#txtMultiplier").val().trim())) {
        flag = false;
        $("#spanMultiplier").html(_msgClientRegistrationMultiplierInValid).show();
        if (scrollID == '') {
            scrollID = 'spanMultiplier';
        }
    }

    if ($("#txtCompeteAudienceMultiplier").val().trim() == "") {
        flag = false;
        $("#spanCompeteAudienceMultiplier").html(_msgClientRegistrationCompeteAudienceMultiplierRequiredField).show();
        if (scrollID == '') {
            scrollID = 'spanCompeteAudienceMultiplier';
        }
    }
    else if (isNaN($("#txtCompeteAudienceMultiplier").val().trim())) {
        flag = false;
        $("#spanCompeteAudienceMultiplier").html(_msgClientRegistrationCompeteAudienceMultiplierInValid).show();
        if (scrollID == '') {
            scrollID = 'spanCompeteAudienceMultiplier';
        }
    }
    if ($("#selectedVisibleLRIndustries") == null || $("#selectedVisibleLRIndustries").length < 1) {
        flag = false;
        $("#spanVisibleLRIndustries").html(_msgClientRegistrationVisibleLRIndustries).show();
        if (scrollID == '') {
            scrollID = 'spanVisibleLRIndustries';
        }
   }

    if ($("#txtv4MaxDiscoveryExportItems").val().trim() != "" && (isNaN($("#txtv4MaxDiscoveryExportItems").val().trim()) || !(eval($("#txtv4MaxDiscoveryExportItems").val().trim()) === parseInt($("#txtv4MaxDiscoveryExportItems").val().trim())))) {
        flag = false;
        $("#spanv4MaxDiscoveryExportItems").html(_msgClientRegistrationMaxDiscoveryExportItemsInValid).show();
        if (scrollID == '') {
            scrollID = 'spanv4MaxDiscoveryExportItems';
        }
    }

    if ($("#txtv4MaxDiscoveryReportItems").val().trim() != "" && (isNaN($("#txtv4MaxDiscoveryReportItems").val().trim()) || !(eval($("#txtv4MaxDiscoveryReportItems").val().trim()) === parseInt($("#txtv4MaxDiscoveryReportItems").val().trim())))) {
        flag = false;
        $("#spanv4MaxDiscoveryReportItems").html(_msgClientRegistrationMaxDiscoveryReportItemsInValid).show();
        if (scrollID == '') {
            scrollID = 'spanv4MaxDiscoveryReportItems';
        }
    }

    if ($("#txtv4MaxDiscoveryHistory").val().trim() == "" || (isNaN($("#txtv4MaxDiscoveryHistory").val().trim()) || $("#txtv4MaxDiscoveryHistory").val().trim() < -1 || $("#txtv4MaxDiscoveryHistory").val().trim() === "0")) {
        flag = false;
        $("#spanv4MaxDiscoveryHistory").html(_msgClientRegistrationMaxDiscoveryHistoryInvalid).show();
        if (scrollID == '') {
            scrollID = 'spanv4MaxDiscoveryHistory';
        }
    }

    if ($("#txtv4MaxFeedsReportItems").val().trim() != "" && (isNaN($("#txtv4MaxFeedsReportItems").val().trim()) || !(eval($("#txtv4MaxFeedsReportItems").val().trim()) === parseInt($("#txtv4MaxFeedsReportItems").val().trim())))) {
        flag = false;
        $("#spanv4MaxFeedsReportItems").html(_msgClientRegistrationMaxFeedsReportItemsInValid).show();
        if (scrollID == '') {
            scrollID = 'spanv4MaxFeedsReportItems';
        }
    }

    if ($("#txtv4MaxFeedsExportItems").val().trim() != "" && (isNaN($("#txtv4MaxFeedsExportItems").val().trim()) || !(eval($("#txtv4MaxFeedsExportItems").val().trim()) === parseInt($("#txtv4MaxFeedsExportItems").val().trim())))) {
        flag = false;
        $("#spanv4MaxFeedsExportItems").html(_msgClientRegistrationMaxFeedsExportItemsInValid).show();
        if (scrollID == '') {
            scrollID = 'spanv4MaxFeedsExportItems';
        }
    }

    if ($("#txtv4MaxLibraryEmailReportItems").val().trim() != "" && (isNaN($("#txtv4MaxLibraryEmailReportItems").val().trim()) || !(eval($("#txtv4MaxLibraryEmailReportItems").val().trim()) === parseInt($("#txtv4MaxLibraryEmailReportItems").val().trim())))) {
        flag = false;
        $("#spanv4MaxLibraryEmailReportItems").html(_msgClientRegistrationMaxLibraryEmailReportItemsInValid).show();
        if (scrollID == '') {
            scrollID = 'spanv4MaxLibraryEmailReportItems';
        }
    }

    if ($("#txtv4MaxLibraryReportItems").val().trim() != "" && (isNaN($("#txtv4MaxLibraryReportItems").val().trim()) || !(eval($("#txtv4MaxLibraryReportItems").val().trim()) === parseInt($("#txtv4MaxLibraryReportItems").val().trim())))) {
        flag = false;
        $("#spanv4MaxLibraryReportItems").html(_msgClientRegistrationMaxLibraryReportItemsInValid).show();
        if (scrollID == '') {
            scrollID = 'spanv4MaxLibraryReportItems';
        }
    }

    if ($("#txtTVHighThreshold").val().trim() != "" && isNaN($("#txtTVHighThreshold").val().trim())) {
        flag = false;
        $("#spanTVHighThreshold").html(_msgClientRegistrationThresholdInValid).show();
        if (scrollID == '') {
            scrollID = 'spanTVHighThreshold';
        }
    }

    if ($("#txtTVLowThreshold").val().trim() != "" && isNaN($("#txtTVLowThreshold").val().trim())) {
        flag = false;
        $("#spanTVLowThreshold").html(_msgClientRegistrationThresholdInValid).show();
        if (scrollID == '') {
            scrollID = 'spanTVLowThreshold';
        }
    }

    if ($("#txtNMHighThreshold").val().trim() != "" && isNaN($("#txtNMHighThreshold").val().trim())) {
        flag = false;
        $("#spanNMHighThreshold").html(_msgClientRegistrationThresholdInValid).show();
        if (scrollID == '') {
            scrollID = 'spanNMHighThreshold';
        }
    }

    if ($("#txtNMLowThreshold").val().trim() != "" && isNaN($("#txtNMLowThreshold").val().trim())) {
        flag = false;
        $("#spanNMLowThreshold").html(_msgClientRegistrationThresholdInValid).show();
        if (scrollID == '') {
            scrollID = 'spanNMLowThreshold';
        }
    }

    if ($("#txtSMHighThreshold").val().trim() != "" && isNaN($("#txtSMHighThreshold").val().trim())) {
        flag = false;
        $("#spanSMHighThreshold").html(_msgClientRegistrationThresholdInValid).show();
        if (scrollID == '') {
            scrollID = 'spanSMHighThreshold';
        }
    }

    if ($("#txtSMLowThreshold").val().trim() != "" && isNaN($("#txtSMLowThreshold").val().trim())) {
        flag = false;
        $("#spanSMLowThreshold").html(_msgClientRegistrationThresholdInValid).show();
        if (scrollID == '') {
            scrollID = 'spanSMLowThreshold';
        }
    }

    if ($("#txtTwitterHighThreshold").val().trim() != "" && isNaN($("#txtTwitterHighThreshold").val().trim())) {
        flag = false;
        $("#spanTwitterHighThreshold").html(_msgClientRegistrationThresholdInValid).show();
        if (scrollID == '') {
            scrollID = 'spanTwitterHighThreshold';
        }
    }

    if ($("#txtTwitterLowThreshold").val().trim() != "" && isNaN($("#txtTwitterLowThreshold").val().trim())) {
        flag = false;
        $("#spanTwitterLowThreshold").html(_msgClientRegistrationThresholdInValid).show();
        if (scrollID == '') {
            scrollID = 'spanTwitterLowThreshold';
        }
    }

    if ($("#txtPQHighThreshold").val().trim() != "" && isNaN($("#txtPQHighThreshold").val().trim())) {
        flag = false;
        $("#spanPQHighThreshold").html(_msgClientRegistrationThresholdInValid).show();
        if (scrollID == '') {
            scrollID = 'spanPQHighThreshold';
        }
    }

    if ($("#txtPQLowThreshold").val().trim() != "" && isNaN($("#txtPQLowThreshold").val().trim())) {
        flag = false;
        $("#spanPQLowThreshold").html(_msgClientRegistrationThresholdInValid).show();
        if (scrollID == '') {
            scrollID = 'spanPQLowThreshold';
        }
    }

    if ($("#txtIQRawMediaExpiration").val().trim() != "" && (isNaN($("#txtIQRawMediaExpiration").val().trim()) || !(eval($("#txtIQRawMediaExpiration").val().trim()) === parseInt($("#txtIQRawMediaExpiration").val().trim())))) {
        flag = false;
        $("#spanIQRawMediaExpiration").html(_msgClientRegistrationIQRawMediaExpirationInValid).show();
        if (scrollID == '') {
            scrollID = 'spanIQRawMediaExpiration';
        }
    }
    else if (parseInt($("#txtIQRawMediaExpiration").val().trim()) > 60) {
        flag = false;
        $("#spanIQRawMediaExpiration").html(_msgClientRegistrationIQRawMediaExpirationTooLarge).show();
        if (scrollID == '') {
            scrollID = 'spanIQRawMediaExpiration';
        }
    }

    var isChecked = false;
    $("#divRoles input[type=checkbox]").each(function () {
        if (this.checked) {
            isChecked = true;
        }
    });

    if (!isChecked) {
        $("#spanClientRole").html(_msgClientRegistrationSelectClientRole).show();
        if (scrollID == '') {
            scrollID = 'spanClientRole';
        }
        flag = false;
    }

    if (!flag) {
        setTimeout(function () { $("#divRegistration_ScrollContent").mCustomScrollbar("scrollTo", "#" + scrollID); }, 100);
    }

    return flag;
}

function TestInput(inputval) {
    if (/^[0-9a-z\s][^<>]*$/i.test(inputval))
        return true;
    else
        return false;
}

function TestZipCode(inputval) {
    if (/\d{5}(-\d{4})?$/i.test(inputval))
        return true;
    else
        return false;
}

function TestPhoneNumber(inputval) {
    if (/^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$/i.test(inputval))
        return true;
    else
        return false;
}

function IsBrowserIE() {
    return navigator.userAgent.toLowerCase().indexOf("msie") > -1;
}


function ClearClient() {
    $("#txtClientName").val('');
    $("#txtMasterClient").val('');
    $("#ddlMCID").val(0);
    $("#txtAddress1").val('');
    $("#txtAddress2").val('');
    $("#txtCity").val('');
    $("#ddlState").val(0);
    $("#txtZip").val('');
    $("#txtAttention").val('');
    $("#txtPhone").val('');
    $("#ddlIndustry").val(0);
    $("#ddlBillType").val(0);
    $("#ddlBillFrequency").val(0);
    $("#ddlPricingCode").val(0);

    $("#txtNoOfUsers").val('');

    $("#chkIsPlayerLogo").prop("checked", true);
    $("#chkIsActive").prop("checked", true);
    

    $("#txtPlayerLogoSelectedFileDisplay").val('');
    $("#hfPlayerLogoImage").val('');
    $("#flPlayerLogo").val('');

    $("#txtNoOfNotification").val('');
    $("#txtNoOfIQAgent").val('');
    $("#txtCompeteMultiplier").val('');
    $("#txtOnlineNewsAdRate").val('');
    $("#txtOnlineOtherAdRate").val('');
    $("#txtURLPercentRead").val('');

    $("#divRoles input[type=checkbox]").each(function () {
        $(this).prop("checked", true);
    });
}

function AddClientToAnewstip(clientKey, clientName) {
    var jsonPostData = {
        clientKey: clientKey,
        clientName: clientName
    }

    $.ajax({
        url: _urlGlobalAdminAddClientToAnewstip,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess) {
                $("#btnClientAddToAnewstip").hide();
                $("#btnClientAddedToAnewstip").show();
                ShowNotification("Client successfully added. Please ensure the Connect role is enabled.");
            }
            else {
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            console.error("AddClientToAnewstip error - " + c);
            ShowNotification(_msgErrorOccured);
        }
    });    
}