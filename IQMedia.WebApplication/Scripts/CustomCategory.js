$(document).ready(function () {
    $('#divCustomCategoryScroll').css({ 'height': 300 });
    $('#divCustomCategoryScroll').mCustomScrollbar({
        advanced: {
            updateOnContentResize: true,
            autoScrollOnFocus: false
        }
    });
});

function OpenCustomCategoryRankingPopup() {
    $.ajax({
        url: '/Setup/GetCustomCategoriesForRanking',
        contentType: "application/json; charset=utf-8",
        type: "post",
        success: BuildCustomCategoryTree,
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured);
        }
    });
}

function CancelCustomCategoryRankingPopup() {
    $("#divCustomCategoryRankingPopup").css({ "display": "none" });
    $("#divCustomCategoryRankingPopup").modal("hide");
}

function BuildCustomCategoryTree(result) {
    if (result.isSuccess) {
        var jsonCategories = JSON.parse(result.jsonCategories);
        jsonCategories = $.map(jsonCategories, function (obj, index) {
            obj.text = EscapeHTML(obj.text);
            return obj;
        });

        $("#customCategoryTree").jstree({
            'core': {
                'data': jsonCategories,
                'check_callback': true,
                'multiple': false
            },
            'plugins': ['unique', 'dnd', 'wholerow', 'types'],
            'types': {
                'default': { 'icon': 'jstree-category-custom' }
            }
        });

        $('#customCategoryTree').jstree().deselect_all();

        // If a category is moved inside another category, revert it back to it's original location
        $('#customCategoryTree').bind("move_node.jstree", function (e, data) {
            if (data.parent != "#") {
                $('#customCategoryTree').jstree().move_node(data.node, data.old_parent, data.old_position);
            }
        });

        $("#divCustomCategoryRankingPopup").modal({
            backdrop: 'static',
            keyboard: true,
            dynamic: true
        });
    }
    else {
        ShowNotification(result.error);
    }
}

function UpdateCustomCategoryRankings() {
    var categoryGUIDs = [];
    $.each($('#customCategoryTree').jstree().get_json(), function (index, element) {
        categoryGUIDs.push(element.id);
    });

    var jsonPostData = {
        categoryGUIDs: categoryGUIDs
    };
    $.ajax({
        url: '/Setup/UpdateCustomCategoryRankings',
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess) {
                ShowNotification("Category rankings successfully updated.");
                CancelCustomCategoryRankingPopup();
            }
            else {
                ShowNotification(result.errorMsg);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured);
        }
    });
}