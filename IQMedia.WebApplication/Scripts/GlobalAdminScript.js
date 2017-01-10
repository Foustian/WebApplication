var _ClientList;
var _FliqClientList;
var selects;
var _ActiveUserSortColumn = '';
var _ActiveUserSortDirectionIsAsc = false;
var _ActiveUserSearchTerm = '';
var _GroupClientList;
var CONST_ZERO = "0";
var _urlGlobalAdmin_GroupSettings = "/GlobalAdmin/GetAllActiveClient/";
var _urlGlobalAdmin_AddSC = "/GlobalAdmin/GroupAddSubClient";
var _urlGlobalAdmin_RemoveSC = "/GlobalAdmin/GroupRemoveSubClient";
var _urlGlobalAdmin_GetClCust = "/GlobalAdmin/GroupGetCLCUST";
var _urlGlobalAdmin_AddSCust = "/GlobalAdmin/GroupAddSubCustomer";
var _urlGlobalAdmin_GetSCust = "/GlobalAdmin/GroupGetSubCustomer";
var _urlGlobalAdmin_RemoveSCust = "/GlobalAdmin/GroupRemoveSubCustomer";
var _msgS_AddSubClient = "Sub Clients added successfully.";
var _msgGrp_RemoveSubClient = "Sub Client removed successfully.";
var _msgS_AddSubCustomer = "Sub Customer added successfully.";
var _msgGrp_RemoveSubCustomer = "Sub Customer removed successfully.";
var _msgGrp_ConfirmRemoveSubCustomer = "Are you sure to remove &quot;[[0]]&quot; Sub Customer?";
var _msgGrp_ConfirmRemoveSubClient = "Are you sure to remove &quot;[[0]]&quot; Sub Client?";

$(function () {

    $("#ulSideMenu li").removeAttr("class");
    $("#liMenuGlobalAdmin").attr("class", "active");
    if (_ClientList == undefined) {
        FillClientList();
    }
    if (_FliqClientList == undefined) {
        FillFliqClientList();
    }

    $("#txtActiveUserSearchTerm").keypress(function (e) {
        if (e.keyCode == 13) {
            SearchActiveUsers();
        }
    });
    $("#txtActiveUserSearchTerm").blur(function () {
        SearchActiveUsers();
    });

    $("#divResult_ScrollContent").css("height", documentHeight - 260);

    $("#divResult_ScrollContent").enscroll({
        verticalTrackClass: 'track4',
        verticalHandleClass: 'handle4',
        margin: '0 0 0 10px'
    });
});


function RemoveUser(EmailAddress, SessionID) {

    var jsonPostData = {
        p_EmailAddress: EmailAddress,
        p_SessionID: SessionID

    }
    $.ajax({

        type: 'POST',
        dataType: 'json',
        url: _urlGlobalAdminRemoveUserFromCache,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),

        success: OnRemoveUserComplete,
        error: OnRemoveUserFail
    });
}

function OnRemoveUserComplete(result) {
    if (result.isSuccess) {
        $('#divResult').html(result.globalAdminHTML);
        ShowNotification('User reset successfully');
    }
    else {
        ShowNotification('Some error occured, try again later');
    }
}

function OnRemoveUserFail(result) {
    ShowNotification('Some error occured, try again later');
}

function RemoveAllUsers() {
    getConfirm("Reset All Users", _msgConfirmRemoveAllUsers, "Confirm Reset", "Cancel", function (res) {
        if (res) {
            $.ajax({

                type: 'POST',
                dataType: 'json',
                url: _urlGlobalAdminRemoveAllUsers,
                contentType: 'application/json; charset=utf-8',
                success: OnRemoveAllUsersComplete,
                error: OnRemoveAllUsersFail
            });
        }
    });
}

function OnRemoveAllUsersComplete(result) {
    if (result.isSuccess) {

        setTimeout(function () { window.location = "/sign-in" }, 3000);

        ShowNotification('All Users have been reset successfully');
    }
    else {
        ShowNotification('Some error occured, try again later');
    }
}

function OnRemoveAllUsersFail(result) {
    ShowNotification('Some error occured, try again later');
}

function GetActiveUsers() {
    var jsonPostData = {
        p_SearchTerm: _ActiveUserSearchTerm,
        p_SortColumn: _ActiveUserSortColumn,
        p_IsAsc: _ActiveUserSortDirectionIsAsc
    }
    $.ajax({

        type: 'POST',
        dataType: 'json',
        url: _urlGlobalAdminGetActiveUsersFromCache,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {

                $("#liActiveUsers").ActiveNav();
                $(".span9-custom > div").hide();
                $("#divResult").show();

                $("#divResult_Content").html(result.HTML);

                $("#divResult_ScrollContent").css("height", documentHeight - 260);


                $("#divResult_ScrollContent").enscroll({
                    verticalTrackClass: 'track4',
                    verticalHandleClass: 'handle4',
                    margin: '0 0 0 10px'
                });

                $("#txtActiveUserSearchTerm").val(_ActiveUserSearchTerm);

            }
            else {
                if (typeof (isNextPage) === 'undefined') {
                    ClearResultsOnError('divResult_Content', null, '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetActiveUsers()"));
                }
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            if (typeof (isNextPage) === 'undefined') {
                ClearResultsOnError('divResult_Content', null, '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "GetActiveUsers()"));
            }
            ShowNotification(_msgErrorOccured);
        }
    });
}


function SearchActiveUsers() {
    if (_ActiveUserSearchTerm != $("#txtActiveUserSearchTerm").val().trim()) {
        _ActiveUserSearchTerm = $("#txtActiveUserSearchTerm").val().trim();
        GetActiveUsers();
    }
}

function ClearActiveUser() {
    if (_ActiveUserSearchTerm != '') {
        _ActiveUserSearchTerm = ''
        GetActiveUsers();
    }
}

function SortActiveUser(sortColumn, sortDirection) {
    if (sortColumn != _ActiveUserSortColumn || sortDirection != _ActiveUserSortDirectionIsAsc) {
        _ActiveUserSortColumn = sortColumn;
        _ActiveUserSortDirectionIsAsc = sortDirection;
        SetDirectionHTMLActiveUser();
        GetActiveUsers();
    }
}

function SetDirectionHTMLActiveUser() {
    if (_ActiveUserSortColumn == 'LastAccessTime' && _ActiveUserSortDirectionIsAsc) {
        $('#aSortDirectionActiveUser').html(_msgLastAccessTimeAscending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_ActiveUserSortColumn == 'LastAccessTime' && !_ActiveUserSortDirectionIsAsc) {
        $('#aSortDirectionActiveUser').html(_msgLastAccessTimeDescending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_ActiveUserSortColumn == 'LoginID' && _ActiveUserSortDirectionIsAsc) {
        $('#aSortDirectionActiveUser').html(_msgEmailAscending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_ActiveUserSortColumn == 'LoginID' && !_ActiveUserSortDirectionIsAsc) {
        $('#aSortDirectionActiveUser').html(_msgEmailDescending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_ActiveUserSortColumn == 'FirstName' && _ActiveUserSortDirectionIsAsc) {
        $('#aSortDirectionActiveUser').html(_msgFirstNameAscending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_ActiveUserSortColumn == 'FirstName' && !_ActiveUserSortDirectionIsAsc) {
        $('#aSortDirectionActiveUser').html(_msgFirstNameDescending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_ActiveUserSortColumn == 'LastName' && _ActiveUserSortDirectionIsAsc) {
        $('#aSortDirectionActiveUser').html(_msgLastNameAscending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_ActiveUserSortColumn == 'LastName' && !_ActiveUserSortDirectionIsAsc) {
        $('#aSortDirectionActiveUser').html(_msgLastNameDescending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_ActiveUserSortColumn == 'Server' && !_ActiveUserSortDirectionIsAsc) {
        $('#aSortDirectionActiveUser').html(_msgServerDescending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else if (_ActiveUserSortColumn == 'Server' && _ActiveUserSortDirectionIsAsc) {
        $('#aSortDirectionActiveUser').html(_msgServerAscending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    else {
        $('#aSortDirectionActiveUser').html(_msgLastAccessTimeDescending + '&nbsp;&nbsp;<span class="caret"></span>');
    }
}

function FillClientList() {
    $.ajax({
        url: _urlGlobalAdminGetClientsList,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: {},
        success: function (result) {
            if (result != null && result.isSuccess) {
                _ClientList = result.clientList;

                $("#txtClients").autocomplete({
                    source: _ClientList
                });

                $(".ui-autocomplete").css("max-height", documentHeight - 300);
                $(".ui-autocomplete").css("overflow", "auto");

                var optClients = '<option value="">All</option>';
                $.each(_ClientList, function (index, item) {
                    optClients = optClients + '<option value="' + item + '">' + item + '</option>';
                });

                $("#ddlClients").html(optClients);
                $("#ddlUGCMap_Client").html(optClients);

                selects = $(".chosen-select").chosen({
                    width: "93%"
                });

                $('#ddlClients').trigger("chosen:updated");
                $('#ddlUGCMap_Client').trigger("chosen:updated");
            }
            else {
                ShowNotification(_msgErrorOccured);
                FillClientList();
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured, a);
            FillClientList();
        }
    });
}

function FillFliqClientList() {
    $.ajax({
        url: _urlFliqCustomerGetFliqClientsList,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: {},
        success: function (result) {
            if (result != null && result.isSuccess) {
                _FliqClientList = result.clientList;
                var optClients = '<option value="">All</option>';
                $.each(_FliqClientList, function (index, item) {
                    optClients = optClients + '<option value="' + item + '">' + item + '</option>';
                });

                $("#ddlFliq_Clients").html(optClients);
                $("#ddlFliq_AppClients").html(optClients);
                $("#ddlFliq_AppClients2").html(optClients);


                selects = $(".chosen-select").chosen({
                    width: "93%"
                });

                $('#ddlFliq_Clients').trigger("chosen:updated");
                $('#ddlFliq_AppClients').trigger("chosen:updated");
                $('#ddlFliq_AppClients2').trigger("chosen:updated");
            }
            else {
                ShowNotification(_msgErrorOccured);
                var optClients = '<option value="">All</option>';
                $("#ddlFliq_Clients").html(optClients);
                $("#ddlFliq_AppClients").html(optClients);
                $("#ddlFliq_AppClients2").html(optClients);


                selects = $(".chosen-select").chosen({
                    width: "93%"
                });

                $('#ddlFliq_Clients').trigger("chosen:updated");
                $('#ddlFliq_AppClients').trigger("chosen:updated");
                $('#ddlFliq_AppClients2').trigger("chosen:updated");
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured, a);
            var optClients = '<option value="">All</option>';
            $("#ddlFliq_Clients").html(optClients);
            $("#ddlFliq_AppClients").html(optClients);
            $("#ddlFliq_AppClients2").html(optClients);

            selects = $(".chosen-select").chosen({
                width: "93%"
            });

            $('#ddlFliq_Clients').trigger("chosen:updated");
            $('#ddlFliq_AppClients').trigger("chosen:updated");
            $('#ddlFliq_AppClients2').trigger("chosen:updated");
        }
    });
}

function ShowElevatedSupportMessage(chkRole, roleName) {
    if ($(chkRole).is(":checked") && (roleName == "v4UGC" || roleName == "v4API")) {
        ShowNotification("Please contact elevated support to fully enable this role");
    }
}

function ResetPasswordAttempts(customerKey) {
    var jsonPostData = {
        p_CustomerKey: customerKey
    }

    $.ajax({
        url: _urlGlobalAdminResetPasswordAttempts,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess) {
                ShowNotification("Login successfully reset");
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

function GetGroupSettings() {

    $.ajax({
        url: _urlGlobalAdmin_GroupSettings,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        success: function (result) {
            if (result.isSuccess) {

                $("#liGroupAdmin").ActiveNav();
                $(".span9-custom > div").hide();
                $("#divGroup").show();
                $("#divGroup").html(result.html);                

                _GroupClientList = result.clients;

                $(".chosen-select").chosen({ width: "100%" });

                $("#ddlMC").val(CONST_ZERO).trigger("chosen:updated");
                $("#ddlSC").trigger("chosen:updated");

                $("#ddlMC").chosen().change(function (event) {
                    GroupClientChange();
                    PopulateSC();
                });

                PopulateMCG();

                $("#ddlMCG").chosen().change(function (event) {

                    GrpMCGChange();

                });
                $("#ddlMCC").chosen().change(function (event) {

                    GrpGetClCust(true);
                });
                $("#ddlSCC").chosen().change(function (event) {

                    GrpGetClCust(false);
                });
                $("#ddlMCust").chosen().change(function (event) {
                    
                    GrpPopulateCustCl(true);
                    PopulateSubCust();
                });

                $("#btnCA").click(function () { GroupClientAdd(); });
                $("#btnCustA").click(function () { GroupCustomerAdd(); });
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

function GroupClientChange() {

    var mc = $("#ddlMC").val();

    $("#ddlSC option").remove();

    if (mc > 0) {

        var cl = $.parseJSON(_GroupClientList);

        $.each(cl, function (index, value) {

            var isMC = $.grep(cl, function (n, i) {
                return (n.RID == value.ID && n.ID != value.ID);
            });

            if ((value.RID == 0 || value.RID == value.ID) && isMC.length == 0) {
                $("#ddlSC").append($('<option>', { value: value.ID, text: value.Name }));
            }

        });

        $("#ddlSC option[value=" + mc + "]").remove();
    }

    $("#ddlSC").trigger("chosen:updated");
}

function GroupClientAdd() {

    var isValid = false;

    if ($("#ddlMC").val() == 0) {

        $("#ddlMC").parent().css({ "border": "1px solid #FF0000" });
        return;
    }

    if ($("#ddlSC").val() == null) {

        $("#ddlSC").parent().css({ "border": "1px solid #FF0000" });
        return;
    }

    isValid = true;

    if (isValid) {

        $("#ddlMC").parent().css({ "border": "" });
        $("#ddlSC").parent().css({ "border": "" });

        var data = new Object();

        data.p_ID = $("#ddlMC").val();
        data.p_RIDList = $("#ddlSC").val();

        $.ajax({
            url: _urlGlobalAdmin_AddSC,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (result) {
                if (result.isSuccess) {

                    $("#ddlMC option").remove();
                    $("#ddlSC option").remove();
                    GetGroupSettings();

                    ShowNotification(_msgS_AddSubClient);
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

}

function PopulateSC() {

    var mc = $("#ddlMC").chosen().val();

    if (mc > 0) {

        $("#divSCList").show();
        $('#tblSC').html('<th style="width:80%;text-align:left;">Client</th><th></th></tr>');

        $.each($.parseJSON(_GroupClientList), function (index, value) {

            if (value.RID == mc && value.ID != mc) {

                $('#tblSC').append('<tr><tr><td> ' + value.Name + ' </td> <td><a href="javascript:void(0);" onclick="RemoveSC(' + value.ID + ',\''+value.Name+'\')";>Remove</a></td></tr>');

            }

        });

        if ($("#tblSC tr").length == 0) {
            $('#tblSC').append('<tr><td colspan="2">No results found</td></tr>');
        }
    }
    else {

        $('#tblSC').html('');
        $("#divSCList").hide();
    }
}

function RemoveSC(RID,Name) {
getConfirm("Confirm Remove", _msgGrp_ConfirmRemoveSubClient.replace("[[0]]",Name), "Continue", "Cancel", function (res) {
                    if (res == true) {

    var isValid = false;
    var mc = $("#ddlMC").val();

    if (mc == 0) {

        $("#ddlMC").parent().css({ "border": "1px solid #FF0000" });
        return;
    }

    if (mc == RID) {

        ShowNotification(_msgErrorOccured);
        return;
    }

    var data = new Object();

    data.p_ID = $("#ddlMC").val();
    data.p_RID = RID;

    $.ajax({
        url: _urlGlobalAdmin_RemoveSC,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(data),
        success: function (result) {
            if (result.isSuccess) {

                $("#ddlMC option").remove();
                $("#ddlSC option").remove();
                $('#tblSC').html('');
                $("#divSCList").hide();
                GetGroupSettings();

                ShowNotification(_msgGrp_RemoveSubClient);
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

function PopulateMCG() {

    var cl = $.parseJSON(_GroupClientList);

    $.each(cl, function (index, value) {

        var isMC = $.grep(cl, function (n, i) {
            return (n.RID == value.ID && n.ID != value.ID);
        });

        if (isMC.length > 0) {
            $("#ddlMCG").append($('<option>', { value: value.ID, text: value.Name }));
        }
    });

    $("#ddlMCG").trigger("chosen:updated");
}

function GrpMCGChange() {

    $("#ddlSCC option").remove();
    $("#ddlSCC").trigger("chosen:updated");
    GrpPopulateCustCl(false);
}

function GrpPopulateCustCl(isSC) {

    var cl = $.parseJSON(_GroupClientList);
    var mcg = $("#ddlMCG").chosen().val();
    var ddl;
    var ddlCust;

    if (isSC) {
        ddl = $("#ddlSCC");
        $("#ddlSCC option").remove();
        $("#ddlSCust option").remove();
        ddlCust = $("#ddlSCust");        
    }
    else {
        ddl = $("#ddlMCC");
        $("#ddlMCC option").remove();
        $("#ddlMCust option").remove();
        ddlCust = $("#ddlMCust");
    }

    $("#tblSCust").html('');
    $("#divSCustList").hide();

    ddl.append($('<option>', { value: 0, text: "Select", selected: "selected" }));
    ddlCust.append($('<option>', { value: 0, text: "Select", selected: "selected" }));    

    if (mcg > 0) {
        var mCustC = $.grep(cl, function (n, i) {
            return (n.RID == mcg || n.ID == mcg);
        });

        $.each(mCustC, function (mCCi, mCCV) {

            ddl.append($('<option>', { value: mCCV.ID, text: mCCV.Name }));

        });

        if(isSC)
        {
            if ($("#ddlMCC").chosen().val() > 0) {
                $("#ddlSCC option[value=" + $("#ddlMCC").chosen().val() + "]").remove();
            }
        }
    }

    ddl.trigger("chosen:updated");
    ddlCust.trigger("chosen:updated");   
}

function GrpGetClCust(isMCC) {

    var mcc = $("#ddlMCC").val();
    var scc = $("#ddlSCC").val();
    var isValid = false;

    var data = new Object();    

    if (!isMCC) {
        data.p_CID = $("#ddlMCust").val();
        data.p_ID = scc;
        isValid = data.p_CID > 0;

        if (!isValid) {
            ShowNotification("Select Master Customer");
            $("#ddlSCC").val(CONST_ZERO).trigger("chosen:updated");
        }
    }
    else {
        data.p_ID = mcc;
        GrpPopulateCustCl(true);
        isValid = true;
    }

    if (data.p_ID > 0 && isValid) {
        $.ajax({
            url: _urlGlobalAdmin_GetClCust,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (result) {
                if (result.isSuccess) {

                    var ddl;

                    if (isMCC) {
                        $("#ddlMCust option").remove();
                        ddl = $("#ddlMCust");
                    }
                    else {
                        $("#ddlSCust option").remove();
                        ddl = $("#ddlSCust");
                    }

                    ddl.append($('<option>', { value: 0, text: "Select", selected: "selected" }));

                    $.each($.parseJSON(result.customers), function (index, value) {

                        ddl.append($('<option>', { value: value.ID, text: value.Name }));

                    });
                    ddl.trigger("chosen:updated");
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

}

function GroupCustomerAdd() {

    $("#divGrpCust select").each(function (i, v) {

        if ($(v).val() == null || $(v).val() == 0) {

            $(this).parent().css({ "border": "1px solid #FF0000" });
            return false;
        }

    });

    var data = new Object();

    data.p_GrpID = $("#ddlMCG").val();
    data.p_MCID = $("#ddlMCC").val();
    data.p_SCID = $("#ddlSCC").val();
    data.p_MCCID = $("#ddlMCust").val();
    data.p_SCCID = $("#ddlSCust").val();

    $.ajax({
        url: _urlGlobalAdmin_AddSCust,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(data),
        success: function (result) {
            if (result.isSuccess) {
                ShowNotification(_msgS_AddSubCustomer);
                $("#ddlMCG").val(CONST_ZERO).trigger("chosen:updated");
                GrpMCGChange();
                GrpPopulateCustCl(true);
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

function PopulateSubCust() {

    var mcc = $("#ddlMCust").val();

    if (mcc > 0) {

        var data = new Object();

        data.p_MCustID = mcc;

        $.ajax({
            url: _urlGlobalAdmin_GetSCust,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (result) {
                if (result.isSuccess) {

                    $('#tblSCust').html('<th style="width:50%;text-align:left;">Client</th><th style="width:30%;text-align:left;">LoginID</th><th></th></tr>');
                    if ($.parseJSON(result.customers).length > 0) {

                        var cl = $.parseJSON(_GroupClientList);

                        $.each($.parseJSON(result.customers), function (index, value) {

                            var clname = $.grep(cl, function (n, i) {
                                return (n.ID == value.CID);
                            });

                            $("#ddlSCC option[value=" + clname[0].ID + "]").remove();

                            $('#tblSCust').append('<tr><td> ' + clname[0].Name +'</td><td>'+  value.LID + ' </td><td><a href="javascript:void(0);" onclick="RemoveSCust(' + value.ID + ',\''+value.LID+'\')";>Remove</a></td></tr>');

                        });

                        $("#ddlSCC").trigger("chosen:updated");
                    }
                    else {
                        $('#tblSCust').append('<tr><td colspan="2">No results found</td></tr>');
                    }
                    $("#divSCustList").show();
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

        $('#tblSCust').html('');
        $("#divSCustList").hide();
    }
}

function RemoveSCust(p_SCustID,p_Name) {

    getConfirm("Confirm Remove", _msgGrp_ConfirmRemoveSubCustomer.replace("[[0]]",p_Name), "Continue", "Cancel", function (res) {
        if (res == true) {            

            var mcc = $("#ddlMCust").val();

            if (mcc > 0 && p_SCustID > 0) {

                var data = new Object();

                data.p_MCustID = mcc;
                data.p_SCustID = p_SCustID;

                $.ajax({
                    url: _urlGlobalAdmin_RemoveSCust,
                    contentType: "application/json; charset=utf-8",
                    type: "post",
                    dataType: "json",
                    data: JSON.stringify(data),
                    success: function (result) {
                        if (result.isSuccess) {

                            ShowNotification(_msgGrp_RemoveSubCustomer);
                            $("#ddlMCG").val(CONST_ZERO).trigger("chosen:updated");
                            GrpMCGChange();
                            GrpPopulateCustCl(true);
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
        }
    });

}

function GroupSetOption(type) {

    $("#divGrpCust").hide();
    $("#divGrpClient").hide();

    if (type == null) {
        $('#aGroupOption').html('Group Client&nbsp;&nbsp;<span class="caret"></span>');
        $("#divGrpClient").show();
    }
    else if (type == 'Client') {
        $('#aGroupOption').html('Group Client&nbsp;&nbsp;<span class="caret"></span>');
        $("#divGrpClient").show();
    }
    else if (type == 'Cust') {
        $('#aGroupOption').html('Group Customer&nbsp;&nbsp;<span class="caret"></span>');
        $("#divGrpCust").show();
    }

}
