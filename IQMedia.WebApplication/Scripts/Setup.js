$(function () {

    $("#ulSideMenu li").removeAttr("class");
    $("#liSetup").attr("class", "active");

    $("#divCustomCategoryAddEditPopup").on("shown", function () {
        $("#txtCategoryName").focus();
    })
});

$(window).resize(function () {
    if (screen.height >= 768) {
        $('#divSetupContent').css({ 'height': documentHeight - 100 });
    }
});

function OpenCustomCategoryAddEditPopup() 
{
    $("#spanCategoryName").html("").hide();
    $("#divCustomCategoryAddEditHTML button[type=button]").removeAttr("disabled");

    $("#divCustomCategoryAddEditPopup").modal({
        backdrop: 'static',
        keyboard: true,
        dynamic: true
    });
}

function CancelCustomCategoryAddEditPopup() 
{
    $("#divCustomCategoryAddEditPopup").css({ "display": "none" });
    $("#divCustomCategoryAddEditPopup").modal("hide");
}

function GetCustomCategoryContent() {

    $("#divSetupContent").html("");
    DisplayCustomCategory();
}

function DisplayCustomCategory() {

    $.ajax({
        url: _urlSetupSelectCustomCategories,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: 'json',
        data: {},
        success: function (result) {

            CheckForSessionExpired(result);

            if (result != null && result.isSuccess) {
                $("#divSetupContent").html(result.HTML);
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

function GetAddEditCustomCategory(categorykey) {

    var jsonPostData = { p_CustomCategoryKey: categorykey }

    $.ajax({
        url: _urlSetupGetAddEditCustomCategory,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: 'json',
        data: JSON.stringify(jsonPostData),
        success: function (result) {

            CheckForSessionExpired(result);

            if (result != null && result.isSuccess) {
                $("#divCustomCategoryAddEditHTML").html(result.HTML);
                OpenCustomCategoryAddEditPopup();
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

function SaveCustomCategory() {

    if (ValidateCustomCategoryAddEdit()) {

        var jsonPostData =
        {
            p_CustomCategoryKey: $("#hdCustomCategoryKey").val(),
            p_CategoryName: $("#txtCategoryName").val(),
            p_CategoryDescription: $("#txtCategoryDescription").val()
        }

        $("#divCustomCategoryAddEditHTML button[type=button]").prop("disabled", "disabled");

        $.ajax({
            url: _urlSetupSaveCustomCategory,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: 'json',
            data: JSON.stringify(jsonPostData),
            success: function (result) {

                CheckForSessionExpired(result);

                $("#divCustomCategoryAddEditHTML button[type=button]").removeAttr("disabled");

                if (result != null && result.isSuccess) {

                    if (result.isDuplicateCategory) {
                        $("#spanCategoryName").html(_msgCategoryAlreadyExists).show();
                        $("#txtCategoryName").focus();
                    }
                    else {
                        DisplayCustomCategory();
                        CancelCustomCategoryAddEditPopup();
                        ShowNotification(_msgCategorySaved);
                    }
                }
            },
            error: function (a, b, c) {
                ShowNotification(_msgErrorOccured);
                $("#divCustomCategoryAddEditHTML button[type=button]").removeAttr("disabled");
            }
        });
    }
}

function DeleteCustomCategory(categorykey) {
    getConfirm("Delete Category", _msgConfirmCategoryDelete, "Confirm Delete", "Cancel", function (res) {
        if (res) {
            var jsonPostData = { p_CustomCategoryKey: categorykey }

            $.ajax({
                url: _urlSetupDeleteCustomCategory,
                contentType: "application/json; charset=utf-8",
                type: "post",
                dataType: 'json',
                data: JSON.stringify(jsonPostData),
                success: function (result) {

                    CheckForSessionExpired(result);

                    if (result != null && result.isSuccess) {

                        if (result.isChildRecordExists) {
                            ShowNotification(_msgCategoryValidation);
                        }
                        else {
                            DisplayCustomCategory();
                            ShowNotification(_msgCategoryDeleted);
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
    });
}

function ValidateCustomCategoryAddEdit() {

    var flag = true;

    $("#spanCategoryName").html("");

    if ($.trim($("#txtCategoryName").val()) == "") {
        $("#spanCategoryName").html(_msgCatNameRequired).show();
        flag = false;
    }
    return flag;
}


function GetClientCustomSettings() {
    $.ajax({
        url: _urlSetupDisplayCustomSettings,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: 'json',
        data: {},
        success: function (result) {

            CheckForSessionExpired(result);

            if (result != null && result.isSuccess) {
                $("#divSetupContent").html(result.HTML);
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

function ResetJob(ID, requestID, resetProcedureName) {
    var jsonPostData = {
        ID: ID,
        requestID: requestID,
        resetProcedureName: resetProcedureName
    };

    $.ajax({
        url: _urlSetupResetJob,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: 'json',
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess) {
                ShowNotification("Job successfully reset.");
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

function LoadGoogleSignIn() {
    $.ajax({
        url: _urlSetupGetGoogleAnalytics,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: 'json',
        data: {},
        success: function (result) {
            CheckForSessionExpired(result);

            if (result != null && result.isSuccess) {
                $("#divSetupContent").html(result.HTML);
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

function LoadInstagramSetup() {
    $.ajax({
        url: _urlSetupLoadInstagramSetup,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: 'json',
        data: {},
        success: function (result) {
            CheckForSessionExpired(result);

            if (result != null && result.isSuccess) {
                $("#divSetupContent").html(result.HTML);
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