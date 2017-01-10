var _IsDrag = false;
var _ReportID;

$(function () {
    interact('.draggable').draggable({
        inertia: true,
        restrict: {
            restriction: ".restrictZone",
            endOnly: false,
            elementRect: { top: 0, left: 0, bottom: 1, right: 1 }
        },
        autoScroll: true,
        onstart: dragStartListener,
        onmove: dragMoveListener,
        onend: dragEndListener
    });

    interact('.media').dropzone({
        overlap: 0.25,
        ondragenter: dragEnterListener,
        ondragleave: dragLeaveListener,
        ondrop: dropListener
    });

    $("#divChartData").hide();
    $("a[id^='aEdit_']").hide();
    $("a[id^='aPlay_']").removeAttr("onclick");
    $("a").click(function (e) {
        // Prevent the activation of links while dragging
        if (_IsDrag) {
            e.preventDefault();
        }
    });

    $("#btnCancelReportSort").click(function () {
        parent.CloseSortReportPopup();
    });

    _ReportID = getParameterByName("reportID");
});

function dragStartListener(event) {
    _IsDrag = true;

    $(event.target).addClass("dragActive");

    event.target.style.zIndex = 1;
    event.target.style.position = 'relative'; 
}

function dragMoveListener(event) {
    var target = event.target,
    // keep the dragged position in the data-x/data-y attributes
        x = (parseFloat(target.getAttribute('data-x')) || 0) + event.dx,
        y = (parseFloat(target.getAttribute('data-y')) || 0) + event.dy;

    // translate the element
    target.style.webkitTransform =
    target.style.transform =
      'translate(' + x + 'px, ' + y + 'px)';

    // update the posiion attributes
    target.setAttribute('data-x', x);
    target.setAttribute('data-y', y);
}

function dragEndListener(event) {
    var target = event.target;

    $(target).removeClass("dragActive");

    target.style.webkitTransform =
    target.style.transform =
        'translate(0px, 0px)';

    target.setAttribute('data-x', x);
    target.setAttribute('data-y', y);

    target.style.zIndex = 0;

    setTimeout(function () {
        _IsDrag = false;
    }, 500);
}

function dragEnterListener(event) {
    $(event.target).addClass("dropzoneActive");
}

function dragLeaveListener(event) {
    $(event.target).removeClass("dropzoneActive");
}

function dropListener(event) {
    var dragTarget = event.relatedTarget;
    var dropTarget = event.target;
    var dragParent = dragTarget.parentNode;
    var dropParent = dropTarget.parentNode;

    dragParent.removeChild(dragTarget);

    if (event.dragEvent.dy > 0) {
        // If dragging down, move the element below the target
        if (dropTarget.nextSibling) {
            dropParent.insertBefore(dragTarget, dropTarget.nextSibling);
        }
        else {
            dropParent.appendChild(dragTarget);
        }
    }
    else {
        // If dragging up, move the element above the target
        dropParent.insertBefore(dragTarget, dropTarget);
    }

    // If the source section no longer has any children, display the placeholder to allow items to be moved into it
    if ($(dragParent).children(".draggable").length == 0) {
        $(dragParent).children(".placeholder").toggleClass("displayNone");
    }

    // If the destination is a placeholder, hide it
    if ($(dropTarget).hasClass("placeholder")) {
        $(dropTarget).toggleClass("displayNone");
    }

    parent._HasPendingSortChanges = true;
    $(dropTarget).removeClass("dropzoneActive");
}

function SaveReportItemPositions(isClose) {
    var reportItems = [];
    var groupTier1Value;
    var groupTier2Value;
    var mediaID;

    $.each($(".groupContainer"), function (index, obj) {
        groupTier1Value = $(obj).attr("groupTier1");
        groupTier2Value = $(obj).attr("groupTier2");

        $.each($(obj).find("input"), function (index2, obj2) {
            mediaID = $(obj2).val();

            reportItems.push({ groupTier1Value: groupTier1Value, groupTier2Value: groupTier2Value, mediaID: mediaID });
        });
    });

    var jsonPostData = {
        reportID: _ReportID,
        reportItems: reportItems
    }

    $.ajax({
        url: _urlLibrarySaveReportItemPositions,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: 'json',
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess) {
                parent._HasPendingSortChanges = false;
                parent.ShowNotification(_msgReportSaved);

                if (isClose) {
                    parent.CloseSortReportPopup();
                }
            }
            else {
                parent.ShowNotification(_msgErrorWhileSavingReport);
            }
        },
        error: function (a, b, c) {
            parent.ShowNotification(_msgErrorOccured);
        }
    });
}

function RevertReportItemPositions() {
    parent.getConfirm("Reset Changes", _msgReportConfirmResetChanges, "Confirm", "Cancel", function (res) {
        if (res) {
            var jsonPostData = {
                reportID: _ReportID
            }

            $.ajax({
                url: _urlLibraryRevertReportItemPositions,
                contentType: "application/json; charset=utf-8",
                type: "post",
                dataType: 'json',
                data: JSON.stringify(jsonPostData),
                success: function (result) {
                    if (result.isSuccess) {
                        parent._HasPendingSortChanges = false;

                        parent.ShowNotification(_msgReportResetSucessfully);
                        window.location.href = window.location.href;
                    }
                    else {
                        parent.ShowNotification(_msgReportResetError);
                    }
                },
                error: function (a, b, c) {
                    parent.ShowNotification(_msgErrorOccured);
                }
            });
        }
    });
}