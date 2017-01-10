$(document).ready(function () {
    $('#divSavedIQReportFoldersScrollContent').enscroll({
        horizontalScrolling: true,
        verticalScrolling: true,
        verticalTrackClass: 'track4',
        verticalHandleClass: 'handle4',
        horizontalTrackClass: 'horizontal-track2',
        horizontalHandleClass: 'horizontal-handle2',
        margin:'0 0 0 10px',
        pollChanges: true,
        addPaddingToPane : true
    });

    GetClientReportFolder();
    changeTab('1');
});

function GetClientReportFolder() {
    var jsonPostData = {};

    $.ajax({
        url: _urlLibraryLoadReportFolder,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: ReportFolderComplete,
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured);
        }
    });
}
var isCreateNode = false;
var isDragStart = false;
var IsOperationSuccess = false;
var objid;
var editObj;
var editObj_OldVal = '';
function ReportFolderComplete(result) {

    if (result.isSuccess) {
        $('#tree').jstree('destroy');
        var HTML = GenerateHTMLFolders(result.jsonFolders,'#');
        $('#tree').html(HTML);
        if ($("#divSavedIQReports").html() != "") {
            $("#imgSavedReportLoading").hide();
            $(".displayReportGrid").show();
            setTimeout(function () {
                $("#divSavedIQReports").width($("#divSavedIQReports").next().width());
            }, 500);
        }
        $('#tree').jstree({
            'core': {
                'check_callback': true,
                'multiple': false
            },
            'plugins': ['unique', 'state', 'dnd', 'types', 'contextmenu', 'html_data', 'wholerow', 'core'],
            'contextmenu': { items: customMenu },
            'types': {
                'default': { 'icon': 'jstree-folder-custom' },
                'folder': { 'icon': 'jstree-folder-custom' },
                'file': { 'valid_children': [], 'icon': 'jstree-file-custom' }
            }
        })
        

		$(document).bind("dnd_start.vakata", function (e, data) {
		    if (data.data.jstree) {
		        isDragStart = true;
		        IsOperationSuccess = false;
		        objid = data.data.nodes[0];
		    }
		});

		$("body").mouseup(function (e) {
		    if (isDragStart) {
		        if ($(e.target).parents(".jstree-node").size() > 0 && $(e.target).parents(".reportListScroller").size() > 0) {
		            parentid = $(e.target).closest("li").attr("id");
		            var type = objid.indexOf("_1") > 0 ? "file" : "folder";
		            isDragStart = false;
		            if (parentid.indexOf("_1") < 0 && objid != parentid) {
		                MoveNode(objid, parentid, type);
		                if (IsOperationSuccess == false) {
		                    e.stopPropagation();
		                    $("#jstree-marker").hide();
		                    $.vakata.dnd._clean();
		                }
		            }
		            else {
		                e.stopPropagation();
		                $("#jstree-marker").hide();
		                $.vakata.dnd._clean();
		            }

		        }

		    }
		});
    }
    else {
        ShowNotification(_msgErrorOccured);
    }
}


function GenerateHTMLFolders(items, parentid) {
    var htmlTree = "";

    var childItems = $.grep(items, function (childItem) {
        return childItem.parent == parentid;
    });
    if (childItems.length > 0) {
        htmlTree = "<ul>";
        $.each(childItems, function (index, item) {
            if (item.type == "File") {
                htmlTree = htmlTree + "<li id=\"" + item.id + "_1\" title=\"" + EscapeHTML(item.text) + "\" onclick=\"GenerateLibraryReport('" + item.id + "');\"  data-jstree='{\"type\":\"file\"}'>" + EscapeHTML(item.text) + "</li>";
            }
            else {
                if (item.parent == "#") {
                    htmlTree = htmlTree + "<li id=\"" + item.id + "\" data-jstree='{\"type\":\"folder\",\"opened\":true,\"selected\":true}'>" + EscapeHTML(item.text);
                }
                else {
                    htmlTree = htmlTree + "<li id=\"" + item.id + "\" data-jstree='{\"type\":\"folder\"}'>" + EscapeHTML(item.text);

                }
                htmlTree = htmlTree + GenerateHTMLFolders(items, item.id) + "</li>";
            }
        });
        htmlTree += "</ul>";
    }

    return htmlTree;
}


function BindBlurForEdit() {
    $('input.jstree-rename-input').blur(function (e) {
        if (isCreateNode) {
            IsOperationSuccess = false;
            CreateNode(editObj, this.value);
        }
        else {
            if (editObj.parent != '#') {
                IsOperationSuccess = false;
                RenameNode(editObj.id, this.value);
            }
            else {
                alert("Root can not be renamed.");
                return false;
            }
        }
        if (IsOperationSuccess == false) {
            e.stopImmediatePropagation();
            if (isCreateNode) {
                $('#tree').jstree('delete_node', editObj);
            }
            else {
                $('#tree').jstree('rename_node', editObj, editObj_OldVal);
            }
        }

        if (isCreateNode) {
            isCreateNode = false;
        }
    });


    if ($('input.jstree-rename-input').length > 0) {
        $._data($('input.jstree-rename-input')[0], 'events')["blur"].reverse();
    }
}

function customMenu(node) {
    // The default set of all items
    var items = {
        "Create": {
            "label": "Create",
            "action": function (data) {
                var inst = $.jstree.reference(data.reference),
                                    obj = inst.get_node(data.reference);
                inst.create_node(obj, {}, "first", function (new_node) {
                    new_node.data = { file: false };
                    editObj = new_node;
                    setTimeout(function () {
                        isCreateNode = true;
                        inst.edit(new_node);
                        BindBlurForEdit();
                    }, 0);

                });
            }
        },
        "Rename": {
            "label": "Rename",
            "action": function (data) {
                var inst = $.jstree.reference(data.reference),
                            obj = inst.get_node(data.reference);
                editObj = obj;
                editObj_OldVal = obj.text;
                inst.edit(obj);
                BindBlurForEdit();
            }
        },
        "Delete": {
            "label": "Delete",
            "action": function (data) {
                var inst = $.jstree.reference(data.reference),
                            obj = inst.get_node(data.reference);

                IsOperationSuccess = false;
                if (inst.is_selected(obj)) {
                    select_node = inst.get_selected();
                    DeleteNode(select_node[0]);
                    if (IsOperationSuccess == true) {
                        inst.delete_node(select_node);
                    }
                }
                else {
                    select_node = obj;
                    DeleteNode(obj.id);
                    if (IsOperationSuccess == true) {
                        inst.delete_node(obj);
                    }
                }


            }
        },
        "CCP": {
            'label': 'Edit',
            'action': false,
            'submenu': {
                'cut': {
                    'label': 'Cut',
                    'action': function (data) {
                        var inst = $.jstree.reference(data.reference),
									obj = inst.get_node(data.reference);
                        if (inst.is_selected(obj)) {
                            inst.cut(inst.get_selected());
                        }
                        else {
                            inst.cut(obj);
                        }
                    }
                },
                'paste': {
                    'label': 'Paste',
                    "_disabled": function (data) {
                        return !$.jstree.reference(data.reference).can_paste();
                    },
                    'action': function (data) {
                        var inst = $.jstree.reference(data.reference),
                        obj = inst.get_node(data.reference);
                        IsOperationSuccess = false;
                        var buffer_data = inst.get_buffer();
                        if (buffer_data.node && buffer_data.node[0].id != obj.id) {
                            MoveNode(buffer_data.node[0].id, obj.id, buffer_data.node[0].type);
                        }
                        if (IsOperationSuccess == true) {
                            inst.paste(obj);
                        }
                        else {
                            inst.reset_buffer();
                        }


                    }
                }
            }
        }
    };


    var search_term = "_1"; // search term
    var search = new RegExp(search_term, "gi");
    var arr = $.grep(node.children_d, function (value) {
        return search.test(value);
    });

    if (node.parent == '#') {
        // Delete the "delete" menu item
        delete items.Delete;
        delete items.Rename;
        delete items.CCP.submenu.cut;

    }
    else if (node.type != "file" && node.children.length > 0 && arr.length > 0) {
        delete items.Delete;
    }
    else if (node.type == "file") {
        delete items.Create;
        delete items.Rename;
        delete items.Delete;
        delete items.CCP.submenu.paste;
    }

    return items;
}

function MoveNode(id,parentid,type) {
    var jsonPostData = {
        p_ID: id,
        p_ParentID: parentid,
        p_Type: type
    };
    $.ajax({
        url: _urlLibraryMoveReportFolder,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        async: false,
        data: JSON.stringify(jsonPostData),
        success: function (res) {
            if (res.isSuccess) {
                if (type == "file") {
                    ShowNotification(_msgReportMoved);
                }
                else {
                    ShowNotification(_msgFolderMoved);
                }
                IsOperationSuccess = true;
            }
            else {
                if (res.isDuplicate) {
                    ShowNotification(_msgFolderAlreadyExist);
                }
                else {
                    ShowNotification(_msgErrorOccured);
                }
                IsOperationSuccess = false;
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured);
            IsOperationSuccess = false;
        }
    });
}

function CreateNode(data,text) {
    var jsonPostData = {
        p_Name: text,
        p_ParentID: data.parent
    };

    $.ajax({
        url: _urlLibraryCreateReportFolder,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        async: false,
        data: JSON.stringify(jsonPostData),
        success: function (res) {
            if (res.isSuccess) {
                //data.instance.set_id(data.node, res.id);
                IsOperationSuccess = true;
                $('#tree').jstree('set_id', data, res.id);
                GetReportFolders();
                /*var temp = [];
                temp.push({ v: text, k: res.id });
                $.each($("#ddlReportFolder option"), function (index, option) {
                    temp.push({ v: option.text, k: option.value });
                });

                temp.sort(function (a, b) {
                    if (a.v > b.v) { return 1 }
                    if (a.v < b.v) { return -1 }
                    return 0;
                });

                var reportOptions = '';

                $.each(temp, function (index, item) {
                    var selected = item.v == rootFolderName ? 'selected="selected"' : '';
                    reportOptions = reportOptions + '<option value="' + item.k + '" ' + selected + ' >' + item.v + '</option>';
                });

                $("#ddlReportFolder").html(reportOptions);*/

                //data.instance.load_node(res.id); 
                ShowNotification(_msgFolderCreated);
            }
            else {
                if (res.isDuplicate) {
                    ShowNotification(_msgFolderAlreadyExist);
                }
                else {
                    ShowNotification(_msgErrorOccured);
                }
                IsOperationSuccess = false;
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured);
            IsOperationSuccess = false;
        }
    });
}

function RenameNode(id,text) {
    var jsonPostData = {
        p_ID: id,
        p_Name: text
    };
    $.ajax({
        url: _urlLibraryRenameReportFolder,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        async: false,
        data: JSON.stringify(jsonPostData),
        success: function (res) {
            if (res.isSuccess) {
                ShowNotification(_msgFolderRenamed);
                //$('#ddlReportFolder option[value="' + id + '"]').text(text);
                GetReportFolders();
                IsOperationSuccess = true;
            }
            else {
                if (res.isDuplicate) {
                    ShowNotification(_msgFolderAlreadyExist);
                }
                else {
                    ShowNotification(_msgErrorOccured);
                }
                IsOperationSuccess = false;
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured);
            IsOperationSuccess = false;
        }
    });
}

function DeleteNode(id) {
    var jsonPostData = {
        p_ID: id
    };

    $.ajax({
        url: _urlLibraryDeleteReportFolder,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        async: false,
        data: JSON.stringify(jsonPostData),
        success: function (res) {
            if (res.isSuccess) {
                ShowNotification(_msgFolderDeleted);
                var node = $('#tree').jstree('get_node', id);
                GetReportFolders();
                /*$('#ddlReportFolder option[value="' + id + '"]').remove();    
                $.each(node.children_d, function (index, item) {
                    $('#ddlReportFolder option[value="' + item + '"]').remove();    
                });*/
                IsOperationSuccess = true;
            }
            else {
                ShowNotification(_msgErrorOccured);
                IsOperationSuccess = false;
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured);
            IsOperationSuccess = false;
        }
    });
}


function GetReportFolders() {
    var jsonPostData = {};
    $("#divCreateReport").addClass("blurOnlyControls");
    $("#divCreateReportMsg").html("Please Wait...");

    $.ajax({
        url: _urlLibraryGetReportFolders,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        async: false,
        data: JSON.stringify(jsonPostData),
        success: function (res) {
            if (res.isSuccess) {
                $("#divCreateReport").removeClass("blurOnlyControls");
                $("#divCreateReportMsg").html("");
                var reportOptions = '';
                $.each(res.reportFolders, function (index, item) {
                    var selected = item.text == rootFolderName ? 'selected="selected"' : '';
                    reportOptions = reportOptions + '<option value="' + item.id + '" ' + selected + ' >' + item.text + '</option>';
                });

                $("#ddlReportFolder").html(reportOptions);
            }
        },
        error: function (a, b, c) {
        }
    });
}

function changeTab(tabNumber) {
    if (tabNumber == '1') {
        $('#divSavedIQReportsFolders').hide();
        $('#headerReportFolder').removeClass('pieChartActive');

        $('#headerReportAll').addClass('pieChartActive');
        $('#divSavedIQReportsScrollContent').show();
    }
    else {
        $('#divSavedIQReportsScrollContent').hide();
        $('#headerReportAll').removeClass('pieChartActive');

        $('#headerReportFolder').addClass('pieChartActive');
        $('#divSavedIQReportsFolders').show();
    }

}