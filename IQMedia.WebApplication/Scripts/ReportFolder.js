var isCreateNode = false;
var isDragStart = false;
var IsOperationSuccess = false;
var objid;
var editObj;
var editObj_OldVal = '';

$(document).ready(function () {
});

function GetClientReportFolder() {
    var jsonPostData = {};

    $.ajax({
        url: _urlSetupLoadReportFolder,
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

function ReportFolderComplete(result) {

    if (result.isSuccess) {
        $("#divSetupContent").html('<div style="padding-right: 15px;"><div id="tree"></div></div>');
        //$('#tree').jstree('destroy');
        $('#tree').jstree({
            'core': {
                'data': JSON.parse(result.jsonFolders),
                'check_callback': true,
                'multiple': false
            },
            'plugins': ['unique', 'state', 'dnd', 'types', 'contextmenu','wholerow'],
            'contextmenu': { items: customMenu },
            'types': {
                'default': { 'icon': 'jstree-folder-custom' },
                'folder': { 'icon': 'jstree-folder-custom' }
            }
        });

        $(document).bind("dnd_start.vakata", function (e, data) {
            if (data.data.jstree) {
                isDragStart = true;
                IsOperationSuccess = false;
                objid = data.data.nodes[0];
            }
        });

        $("body").mouseup(function (e) {
            if (isDragStart) {
                if ($(e.target).parents(".jstree-node").size() > 0) {
                    parentid = $(e.target).closest("li").attr("id");
                    isDragStart = false;
                    if (parentid.indexOf("_1") < 0 && objid != parentid) {
                        MoveNode(objid, parentid);
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
                            MoveNode(buffer_data.node[0].id, obj.id);
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

    if (node.parent == '#') {
        // Delete the "delete" menu item
        delete items.Delete;
        delete items.Rename;
        delete items.CCP.submenu.cut;

    }
    
    return items;
}

function MoveNode(id, parentid) {
    var jsonPostData = {
        p_ID: id,
        p_ParentID: parentid
    };
    $.ajax({
        url: _urlSetupMoveReportFolder,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        async: false,
        data: JSON.stringify(jsonPostData),
        success: function (res) {
            if (res.isSuccess) {
                ShowNotification(_msgFolderMoved);
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

function CreateNode(data, text) {
    var jsonPostData = {
        p_Name: text,
        p_ParentID: data.parent
    };

    $.ajax({
        url: _urlSetupCreateReportFolder,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        async: false,
        data: JSON.stringify(jsonPostData),
        success: function (res) {
            if (res.isSuccess) {
                IsOperationSuccess = true;
                $('#tree').jstree('set_id', data, res.id);
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

function RenameNode(id, text) {
    var jsonPostData = {
        p_ID: id,
        p_Name: text
    };
    $.ajax({
        url: _urlSetupRenameReportFolder,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        async: false,
        data: JSON.stringify(jsonPostData),
        success: function (res) {
            if (res.isSuccess) {
                ShowNotification(_msgFolderRenamed);
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
        url: _urlSetupDeleteReportFolder,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        async: false,
        data: JSON.stringify(jsonPostData),
        success: function (res) {
            if (res.isSuccess) {
                ShowNotification(_msgFolderDeleted);
                IsOperationSuccess = true;
            }
            else {
                if (res.isHasChildReport) {
                    ShowNotification(_msgDeleteFolderChildExists);
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