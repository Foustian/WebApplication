var globalMediaResultID = ''
var msg = ''
var inProgress = false;
var globalSubMediaType = '';
var globalSearchTerm = '';


function ShowArticle(articleURL, mediaResultID) {

    globalMediaResultID = mediaResultID;

    var divIframeHTML = '<div id="diviFrameArticle" runat="server" class="modal hide fade resizable modalPopupDiv iframePopup" style="display: none;">'
                + '<div class=\"divIframeClose\" id="divCloseNews">'
                     + '<div><img id="img1" src="../Images/close-icon.png" class="popup-top-close" onclick="CloseArticle(\'diviFrameArticle\');" /></div></div>'
                + '<iframe id="iFrameArticle" style="width: 100%; height: 100%;" scrolling="auto" frameborder="0" runat="server" src=""></iframe>'
                 + '<div class=\"divIframeButton\" >'
                    + '<input id="btnSave" Type="button" value="Save Article" class="btn-green" onclick="ShowSaveArticlePopUp();" />'
                     + '<input id="btnPrint"  Type="button" value="Print Article" class="btn-green" runat="server" onclick="javascript:PrintIframe(\'' + articleURL + '\');return false;" /></div></div>'



    $(document.body).append(divIframeHTML);
    $('#iFrameArticle').attr('src', articleURL);
    $('#diviFrameArticle').modal({
        backdrop: 'static',
        keyboard: true,
        dynamic: true
    });

}

function OnFail(result) {

}

function CloseArticle(divID) {
    $('#' + divID).css({ "display": "none" });
    $('#' + divID).modal('hide');
    $('#' + divID).remove();
}

function PrintIframe(articleURL) {
    var docprint = window.open(articleURL, "_blank");
}


function ShowSaveArticlePopUp() {
    var categoryOptions = '<option value="0">Select Category</option>';

    //    var saveArticlePopUpHTML = '<div ID="pnlSaveArticle" class="modal hide fade resizable modalPopupDiv height80p" Style="display: none;">'
    //                            + '<div class="popContainMain"><div class="popup-hd"><span id="spnSaveArticleTitle">Article Details</span>'
    //                            + '<div style="width: 27px; float: right;" id="div7" runat="server"><img id="btnClose" src="../Images/close-icon.png" onclick="CloseArticle(\'pnlSaveArticle\');" /></div></div>'
    //                            + '<div class="blue-content-bg height90p"><div><div id="lblSaveArticleMsg" Visible="true" CssClass="error-msg"></div></div>'
    //                                + '<ul class="registration liststyleNone"><li><label>Title :</label>'
    //                                + '<input id=\'txtArticleTitle\' name=\'txtArticleTitle\' style=\'width:74%;\' type="text" /></li>'
    //                                + '<li><label>Primary Category :</label>'
    //                                + '<select id="ddlPCategory" onchange="UpdateSubCategory1(this.id,true);"></select></li>'
    //                         + '<li><label>Sub Category 1 :</label>'
    //                        + '<select id="ddlSubCategory1" onchange="UpdateSubCategory1(this.id,true);"/></select></li>'
    //                                + '<li><label>Sub Category 2 :</label>'
    //                        + '<select id="ddlSubCategory2" onchange="UpdateSubCategory1(this.id,true);"/></select></li>'
    //                        + '<li><label>Sub Category 3 :</label>'
    //                        + '<select id="ddlSubCategory3" onchange="UpdateSubCategory1(this.id,true);"/></select></li>'
    //                            + '<li><label>Description :</label>'
    //                             + '<input id=\'txtDescription\' style=\'width:74%;height: 145px; overflow: auto;\' type="text" /></li>'
    //                         + '<li><label>Keywords :</label>'
    //                         + '<input id=\'txtKeywords\' style=\'width:74%;height: 145px; overflow: auto;\' type="text" /></li>'

    //                           + '<li><label>Rate This Article :</label>'
    //                            + '<div><div style="width: 30%;clear:both;"><span class="float-right RateArticleNumberRight" style="margin-top:-4px;">6</span><span class="float-left RateArticleNumberLeft" style="margin-top:-4px;">1</span> <div style="margin-left:15px;margin-right:15px;margin-top:4px;" id="divSlider">'
    //                                    + '</div><span class="float-left prefferredcheckbox"></span></div>'
    //                         + '</div></li></ul>'
    //     + '<div class="text-align-center"><input type="button" id="Button1" class="btn-blue2" value="Cancel" onclick="CloseArticle(\'pnlSaveArticle\');" /><input type="submit" class="btn-blue2" id="btnSaveArticle" onclick="SaveArticle();"  value="Save" ></div></div></div><div>'


    var saveArticlePopUpHTML = '<div ID="pnlSaveArticle" class="modal hide fade resizable modalPopupDiv height80p" Style="display: none;">'
                            + '<div class="popContainMain"><div class="popup-hd"><span id="spnSaveArticleTitle">Article Details</span>'
                            + '<div style="width: 27px; float: right;" id="div7" runat="server"><img id="btnClose" src="../Images/close-icon.png" onclick="CloseArticle(\'pnlSaveArticle\');" /></div></div>'
                            + '<div class="blue-content-bg height90p"><div><div id="lblSaveArticleMsg" Visible="true" CssClass="error-msg"></div></div>'
                            + '<form class="bs-docs-example form-horizontal">'
                            + '<div class="control-group"><label class="control-label">Title :</label>'
                            + '<div class="controls"><input type="text" id=\'txtArticleTitle\' name=\'txtArticleTitle\' style=\'width: 74%;\' /></div></div>'

                            + '<div class="control-group"><label class="control-label">Primary Category :</label>'
                            + '<div class="controls"><select id="ddlPCategory" onchange="UpdateSubCategory1(this.id,true);"></select></div></div>'

                            + '<div class="control-group"><label class="control-label">Sub Category 1 :</label>'
                            + '<div class="controls"><select id="ddlSubCategory1" onchange="UpdateSubCategory1(this.id,true);"/></select></div></div>'

                            + '<div class="control-group"><label class="control-label">Sub Category 2 :</label>'
                            + '<div class="controls"><select id="ddlSubCategory2" onchange="UpdateSubCategory1(this.id,true);"/></select></div></div>'

                            + '<div class="control-group"><label class="control-label">Sub Category 3 :</label>'
                            + '<div class="controls"><select id="ddlSubCategory3" onchange="UpdateSubCategory1(this.id,true);"/></select></div></div>'

                            + '<div class="control-group"><label class="control-label">Description :</label>'
                            + '<div class="controls"><input id=\'txtDescription\' style=\'width:74%;height: 145px; overflow: auto;\' type="text" /></div></div>'

                            + '<div class="control-group"><label class="control-label">Keywords :</label>'
                            + '<div class="controls"><input id=\'txtKeywords\' style=\'width:74%;height: 145px; overflow: auto;\' type="text" /></div></div>'

                            + '<div class="control-group"><label class="control-label">Rate This Article :</label>'
                            + '<div class="controls"><div style="width: 30%;margin-top:13px;"><span class="float-right RateArticleNumberRight" style="margin-top:-8px;">6</span><span class="float-left RateArticleNumberLeft" style="margin-top:-8px;">1</span> <div style="margin-left:15px;margin-right:15px;margin-top:4px;" id="divSlider">'
                                    + '</div><span class="float-left prefferredcheckbox"></span></div>'
                            + '</div></div>'


                             + '<div class="control-group"><div class="controls"><input type="button" id="btnCloseArticle" class="btn-blue2" value="Cancel" onclick="CloseArticle(\'pnlSaveArticle\');" /><input type="button" class="btn-blue2" id="btnSaveArticle" onclick="SaveArticle();" style="margin-left:10px;"  value="Save" /></div></div></form>'

    $(document.body).append(saveArticlePopUpHTML);
    if (customCategoryObject != null) {
        $.each(customCategoryObject, function (eventID, eventData) {
            categoryOptions = categoryOptions + '<option value="' + eventData.CategoryGUID + '">' + EscapeHTML(eventData.CategoryName) + '</option>';
        });
    }

    $("#divSlider").slider({
        value: 2,
        min: 1,
        max: 6,
        step: 1

    });
    $('#ddlPCategory').append(categoryOptions);
    $('#ddlSubCategory1').append(categoryOptions);
    $('#ddlSubCategory2').append(categoryOptions);
    $('#ddlSubCategory3').append(categoryOptions);

    CloseArticle('diviFrameArticle');
    $('#pnlSaveArticle').modal({
        backdrop: 'static',
        keyboard: true,
        dynamic: true
    });


}

function UpdateSubCategory1(ddl_id, IsEdit) {

    var PCatId;
    var SubCat1Id;
    var SubCat2Id;
    var SubCat3Id;
    var LblMessageID;
    if (!IsEdit) {
        PCatId = "ddlPCategory";
        SubCat1Id = "ddlSubCategory1";
        SubCat2Id = "ddlSubCategory2";
        SubCat3Id = "ddlSubCategory3";
        LblMessageID = "lblSaveArticleMsg";
    }
    else {
        PCatId = "ddlPCategory";
        SubCat1Id = "ddlSubCategory1";
        SubCat2Id = "ddlSubCategory2";
        SubCat3Id = "ddlSubCategory3";
        LblMessageID = "lblSaveArticleMsg";

    }

    var PCatSelectedValue = $("#" + PCatId + "").val();
    var Cat1SelectedValue = $("#" + SubCat1Id + "").val();
    var Cat2SelectedValue = $("#" + SubCat2Id + "").val();
    var Cat3SelectedValue = $("#" + SubCat3Id + "").val();
    document.getElementById(LblMessageID).innerHTML = "";

    if (ddl_id == PCatId) {

        $("#" + SubCat1Id + " option").removeAttr("disabled");
        $("#" + SubCat2Id + " option").removeAttr("disabled");
        $("#" + SubCat3Id + " option").removeAttr("disabled");

        if (PCatSelectedValue != 0) {
            $("#" + SubCat1Id + " option[value='" + PCatSelectedValue + "']").attr("disabled", "disabled");
            $("#" + SubCat2Id + " option[value='" + PCatSelectedValue + "']").attr("disabled", "disabled");
            $("#" + SubCat3Id + " option[value='" + PCatSelectedValue + "']").attr("disabled", "disabled");


            if (PCatSelectedValue == Cat1SelectedValue) {
                $("#" + SubCat1Id + "").val(0);
                $("#" + SubCat2Id + "").val(0);
                $("#" + SubCat3Id + "").val(0);
            }
            else if (PCatSelectedValue == Cat2SelectedValue) {
                $("#" + SubCat2Id + "").val(0);
                $("#" + SubCat3Id + "").val(0);
            }
            else if (PCatSelectedValue == Cat3SelectedValue) {
                $("#" + SubCat3Id + "").val(0);
            }


            if ($("#" + SubCat1Id + "").val() != 0) {
                $("#" + SubCat2Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
                $("#" + SubCat3Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
            }

            if ($("#" + SubCat2Id + "").val() != 0) {
                $("#" + SubCat3Id + " option[value='" + $("#" + SubCat2Id + "").val() + "']").attr("disabled", "disabled");
            }

        }
        else {
            $("#" + SubCat1Id + "").val(0);
            $("#" + SubCat2Id + "").val(0);
            $("#" + SubCat3Id + "").val(0);
        }
    }
    else if (ddl_id == SubCat1Id) {
        if (PCatSelectedValue != 0) {
            $("#" + SubCat2Id + " option").removeAttr("disabled");
            $("#" + SubCat3Id + " option").removeAttr("disabled");

            $("#" + SubCat2Id + " option[value='" + $("#" + PCatId + "").val() + "']").attr("disabled", "disabled");
            $("#" + SubCat3Id + " option[value='" + $("#" + PCatId + "").val() + "']").attr("disabled", "disabled");

            if (Cat1SelectedValue != 0) {

                $("#" + SubCat2Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");

                $("#" + SubCat3Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");

                if (Cat1SelectedValue == Cat2SelectedValue) {
                    $("#" + SubCat2Id + "").val(0);
                    $("#" + SubCat3Id + "").val(0);
                }
                else if (Cat1SelectedValue == Cat3SelectedValue) {
                    $("#" + SubCat3Id + "").val(0);
                }

                if ($("#" + SubCat1Id + "").val() != 0) {
                    $("#" + SubCat2Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
                    $("#" + SubCat3Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
                }

                if ($("#" + SubCat2Id + "").val() != 0) {
                    $("#" + SubCat3Id + " option[value='" + $("#" + SubCat2Id + "").val() + "']").attr("disabled", "disabled");
                }
            }
            else {
                $("#" + SubCat2Id + "").val(0);
                $("#" + SubCat3Id + "").val(0);
            }

        }
        else {
            $("#" + SubCat1Id + "").val(0);
            $("#" + SubCat2Id + "").val(0);
            $("#" + SubCat3Id + "").val(0);
            document.getElementById(LblMessageID).innerHTML = "<ul><li>" + _msgFirstSelectPrecedingCat + "</li></ul>";
        }
    }
    else if (ddl_id == SubCat2Id) {
        if (PCatSelectedValue != 0 && Cat1SelectedValue != 0) {
            $("#" + SubCat3Id + " option").removeAttr("disabled");

            $("#" + SubCat3Id + " option[value='" + $("#" + PCatId + "").val() + "']").attr("disabled", "disabled");
            $("#" + SubCat3Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");

            if (Cat2SelectedValue != 0) {

                $("#" + SubCat3Id + " option[value='" + $("#" + SubCat2Id + "").val() + "']").attr("disabled", "disabled");

                if (Cat2SelectedValue == Cat3SelectedValue) {
                    $("#" + SubCat3Id + "").val(0);
                }

                if ($("#" + SubCat1Id + "").val() != 0) {
                    $("#" + SubCat2Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
                    $("#" + SubCat3Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
                }

                if ($("#" + SubCat2Id + "").val() != 0) {
                    $("#" + SubCat3Id + " option[value='" + $("#" + SubCat2Id + "").val() + "']").attr("disabled", "disabled");
                }
            }
            else {
                $("#" + SubCat3Id + "").val(0);
            }
        }
        else {
            $("#" + SubCat2Id + "").val(0);
            $("#" + SubCat3Id + "").val(0);
            document.getElementById(LblMessageID).innerHTML = "<ul><li>" + _msgFirstSelectPrecedingCat + "</li></ul>";
        }
    }
    else if (ddl_id == SubCat3Id) {
        if (PCatSelectedValue == 0 || Cat1SelectedValue == 0 || Cat2SelectedValue == 0) {
            $("#" + SubCat3Id + "").val(0);
            document.getElementById(LblMessageID).innerHTML = "<ul><li>" + _msgFirstSelectPrecedingCat + "</li></ul>";
        }
    }
}


function ValidateInput() {

    $('#lblSaveArticleMsg').html('');
    msg = ''
    var isValid = true;

    /*if ($('#txtArticleTitle').val() == '') {
    msg = msg + '<li>Title is required</li>';
    isValid = false;

    }*/


    if ($('#ddlPCategory').val() <= 0) {
        msg = _msgSelectCategory;
        isValid = false;
    }


    /*  if ($('#txtDescription').val() == '') {
    msg = msg + '<li>Description is required</li>';
    isValid = false;
    }

    if ($('#txtKeywords').val() == '') {
    msg = msg + '<li>Keyword is required</li>';
    isValid = false;
    }

    msg = msg + '</ul>';

    if (!isValid) {
    $('#lblSaveArticleMsg').html(msg);
    }*/

    return isValid;

}


function SaveArticle() {

    if (!inProgress && ValidateInput()) {
        $('.saveMedia').attr('title', _msgOtherProcessProgress); //'Other process in progress');
        $('.saveMedia').tooltip({
            trigger: 'hover',
            placement: 'right'
        });

        var parentID = $("#hdnParentID_" + globalMediaResultID).val();
        if (parentID === "0") {
            parentID = globalMediaResultID;
        }

        var jsonPostData = {
            CategoryGuid: $('#ddlPCategory').val(),
            MediaResultID: globalMediaResultID,
            Keywords: $('#txtPKeywords').val(),
            Description: $('#txtPDescription').val(),
            ParentID: parentID
        }

        inProgress = true;

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlFeedsInsert_ArchiveData,
            contentType: 'application/json; charset=utf-8',
            global: false,
            data: JSON.stringify(jsonPostData),
            success: OnInsertArchiveDataComplete,
            error: OnInsertArchiveDataFail
        });
        $('#imgSaveTweetLoading').show();
    }
    else {
        $('#ddlPCategory').css('border', '1px red solid');
        $('#ddlPCategory').addClass('boxshadow');

        setTimeout(function () {
            $('#ddlPCategory').removeClass('boxshadow');
        }, 2000);
    }

}


function OnInsertArchiveDataComplete(result) {

    inProgress = false;
    $('.saveMedia').removeAttr('title');
    //$('.saveMedia').removeAttr('data-original-title');
    $('.saveMedia').tooltip('destroy');
    $('#imgSaveTweetLoading').hide();

    if (result.isSuccess) {
        $('#divPopover').remove();
        ShowNotification(result.message);
    }
    else {

        if (typeof result.isAuthorized != 'undefined' && !result.isAuthorized) {
            //ShowLogINPopup();
            //window.location.href = '../Home/Index';
            RedirectToUrl(result.redirectURL);
        }
        else {
            ShowErrorMessage();
        }
    }
}

function SaveArticleDiscovery() {

    if (!inProgress && ValidateInput()) {
        $('.saveMedia').attr('title', _msgOtherProcessProgress);
        $('.saveMedia').tooltip({
            trigger: 'hover',
            placement: 'right'
        });

        var jsonPostData = {

            categoryGuid: $('#ddlPCategory').val(),
            articleID: globalMediaResultID,
            subMediaType: globalSubMediaType,
            searchTem: globalSearchTerm,
            keywords: $('#txtPKeywords').val(),
            description: $('#txtPDescription').val()
        }

        inProgress = true;

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlDiscoverySaveArticle,
            contentType: 'application/json; charset=utf-8',
            global: false,
            data: JSON.stringify(jsonPostData),
            success: OnInsertArchiveDataComplete,
            error: OnInsertArchiveDataFail
        });
        $('#imgSaveTweetLoading').show();
    }
    else {
        $('#ddlPCategory').css('border', '1px red solid');
        $('#ddlPCategory').addClass('boxshadow');

        setTimeout(function () {
            $('#ddlPCategory').removeClass('boxshadow');
        }, 2000);
    }

}

function OnInsertArchiveDataFail(result) {

    inProgress = false;

    $('.saveMedia').removeAttr('title');
    $('.saveMedia').removeAttr('data-original-title');

    if (typeof result.isAuthorized != 'undefined' && !result.isAuthorized) {
        //ShowLogINPopup();
        //window.location.href = '../Home/Index';
        RedirectToUrl(result.redirectURL);
    }
    else {
        ShowErrorMessage();
    }
    $('#imgSaveTweetLoading').hide();
}


function ShowSaveArticle(mediaResultID, elementID) {

    if (inProgress) {
        //$('.saveMedia').tooltip('show');
    }
    else {
        globalMediaResultID = mediaResultID;


        var categoryOptions = '<option value="0">Select Category</option>';

        $('#divPopover').remove();

        if (customCategoryObject != null) {
            $.each(customCategoryObject, function (eventID, eventData) {
                categoryOptions = categoryOptions + '<option value="' + eventData.CategoryGUID + '">' + EscapeHTML(eventData.CategoryName) + '</option>';
            });
        }
        
        if (screen.height <= 480 && screen.height > 320) {
            $('#' + elementID).popover({
                trigger: 'manual',
                html: true,
                title: '',
                placement: 'bottom',
                template: '<div id="divPopover" style="width:290px;" class="popover articlePopover"><div class="arrow"></div><div class="popover-inner"><div class="popover-content"><p></p></div></div></div>',
                content: '<div><a class="cursorPointer float-left margintop5 marginRight10" onclick="CreateAndOpenAddCategoryPopup(GetCustomCategory,\'bindArticleCategory\',true);">Add&nbsp;</a><select id="ddlPCategory"></select></div><div><input type="text" id="txtPKeywords" placeholder="Keywords" style="width:245px;"/></div><div><input type="text" id="txtPDescription" placeholder="Description" style="width:245px;"/></div><div><input type="button"  class="btn-blue2" value="Cancel"  onclick="$(\'#divPopover\').remove();" /><input type="button" id="btnSaveArticle" class="btn-blue2 divSaveArticleButton" value="Submit" onclick=\'SaveArticle();\' /><img src="../../Images/Loading_1.gif" class="displayNone" id="imgSaveTweetLoading" /></div>'
            });

            

            $('#' + elementID).popover('show');



            var popOverLeftPosition = $('#' + elementID).position().left - $('#divPopover').width() + 35;
            var arrowPosition = '78px';
            if (popOverLeftPosition < 0) {
                popOverLeftPosition = '23px';
                arrowPosition = '52px';
            }
            $('.articlePopover').css({ 'left': popOverLeftPosition });
            $('.popover.bottom .arrow').css({ 'margin-left': arrowPosition })
        }
        else {
            $('#' + elementID).popover({
                trigger: 'manual',
                html: true,
                title: '',
                placement: 'left',
                template: '<div id="divPopover" style="width:290px;" class="popover articlePopover"><div class="arrow"></div><div class="popover-inner"><div class="popover-content"><p></p></div></div></div>',
                content: '<div><a class="cursorPointer float-left margintop5 marginRight10" onclick="CreateAndOpenAddCategoryPopup(GetCustomCategory,\'bindArticleCategory\',true);">Add&nbsp;</a><select id="ddlPCategory"></select></div><div><input type="text" id="txtPKeywords" placeholder="Keywords" style="width:245px;"/></div><div><input type="text" id="txtPDescription" placeholder="Description" style="width:245px;"/></div><div><input type="button"  class="btn-blue2" value="Cancel"  onclick="$(\'#divPopover\').remove();" /><input type="button" id="btnSaveArticle" class="btn-blue2 divSaveArticleButton" value="Submit" onclick=\'SaveArticle();\' /><img src="../../Images/Loading_1.gif" class="displayNone" id="imgSaveTweetLoading" /></div>'
            });
            $('#' + elementID).popover('show');
        }

        

        $('#ddlPCategory').html(categoryOptions);

        if (customCategoryObject == '' || customCategoryObject == null) {
            GetCustomCategory(bindArticleCategory);
        }

        /*$('.popover.bottom .arrow').css({ 'margin-left': '20px' })
        $('.articlePopover').css({ 'max-width': '57%' });
        $('.articlePopover select').css({ 'width': '95%' });
        $('.articlePopover input').css({ 'width': '40%' });
        $('.articlePopover .divSaveArticleButton').css({ 'margin-right': '0px' });*/
    }
}

function ShowSaveArticleDiscovery(mediaResultID, elementID, subMediaType, searchTerm) {

    if (inProgress) {
        //$('.saveMedia').tooltip('show');
    }
    else {
        globalMediaResultID = mediaResultID;
        globalSubMediaType = subMediaType;
        globalSearchTerm = searchTerm;

        var categoryOptions = '<option value="0">Select Category</option>';

        $('#divPopover').remove();
        $('#' + elementID).popover({
            trigger: 'manual',
            html: true,
            title: '',
            placement: 'left',
            template: '<div id="divPopover" style="width:290px;" class="popover"><div class="arrow"></div><div class="popover-inner"><div class="popover-content"><p></p></div></div></div>',
            content: '<div><a class="cursorPointer float-left margintop5 marginRight10" onclick="CreateAndOpenAddCategoryPopup(GetCustomCategory,\'bindArticleCategory\',true);">Add&nbsp;</a><select id="ddlPCategory"></select></div><div><input type="text" id="txtPKeywords" placeholder="Keywords" style="width:245px;"/></div><div><input type="text" id="txtPDescription" placeholder="Description" style="width:245px;"/></div><div><input type="button"  class="btn-blue2" value="Cancel"  onclick="$(\'#divPopover\').remove();" /><input type="button" id="btnSaveArticle" class="btn-blue2 divSaveArticleButton" value="Submit" onclick=\'SaveArticleDiscovery();\' /><img src="../../Images/Loading_1.gif" class="displayNone" id="imgSaveTweetLoading" /></div>'
        });

        if (customCategoryObject != null) {
            $.each(customCategoryObject, function (eventID, eventData) {
                categoryOptions = categoryOptions + '<option value="' + eventData.CategoryGUID + '">' + EscapeHTML(eventData.CategoryName) + '</option>';
            });
        }


        $('#' + elementID).popover('show');
        $('#ddlPCategory').html(categoryOptions);

        if (customCategoryObject == '' || customCategoryObject == null) {
            GetCustomCategory(bindArticleCategory);
        }

    }
}

function bindArticleCategory() {
    var categoryOptions = '<option value="0">Select Category</option>';
    if (customCategoryObject != null) {
        $.each(customCategoryObject, function (eventID, eventData) {
            categoryOptions = categoryOptions + '<option value="' + eventData.CategoryGUID + '">' + EscapeHTML(eventData.CategoryName) + '</option>';
        });
    }

    $('#ddlPCategory').html(categoryOptions);
}