var _ClientName = null;
var _IsAsc = true;
$(function () {
    $("#ulSideMenu li").removeAttr("class");
    $("#liGroup").attr("class", "active");

    $("#txtClientName").keypress(function (e) {
        if (e.keyCode == 13) {
            SearchClient();
        }
    });
    $("#txtClientName").blur(function () {
        SearchClient();
    });
});

function ValidateSwitch(frmid) {
    getConfirm("Switch Client", "You are about to switch from your current group and be connected to your newly selected group.", "Confirm", "Cancel", function (res) {
        if (res) {
            $('#frm_'+frmid).submit();
        }
    });
}

function GetClients() {
    var jsonPostData = {
        p_ClientName : _ClientName,
        p_IsAsc : _IsAsc
    };
    

    $.ajax({
        url: _urlGroupGetClients,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {

                $("#divResult").html(result.HTML);

                $("#divResult_ScrollContent").css("height", documentHeight - 200);


                $("#divResult_ScrollContent").enscroll({
                    verticalTrackClass: 'track4',
                    verticalHandleClass: 'handle4',
                    margin : '0 0 0 10px'
                });

                $("#txtClientName").val(_ClientName);

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

function SearchClient() {
    if (_ClientName != $("#txtClientName").val().trim()) {
        _ClientName = $("#txtClientName").val().trim();
        GetClients();
    }
}

function ClearClient(isAsc) {
    if (_ClientName != '') {
        _ClientName = ''
        GetClients();
    }
}

function SortClient(isAsc) {
    if (_IsAsc != isAsc) {
        _IsAsc = isAsc;

        if (_IsAsc) {
            $('#aSortDirection').html(_msgClientNameAscending + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        else {
            $('#aSortDirection').html(_msgClientNameDescending + '&nbsp;&nbsp;<span class="caret"></span>');
        }

        GetClients();
    }
}

