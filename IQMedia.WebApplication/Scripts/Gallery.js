var gridster = null;
var base_size = 25;
var cols;
var offset;


var _SearchTerm = "";
var _FromDate = null;
var _ToDate = null;
var _GalleryCategoryGUID = [];
var _GalleryCategoryNameList = [];
var _CategoryName = "";
var _Date = null;
var _Customer = "";
var _CustomerName = "";
var _IsAsc = false;
var _SortColumn = "CreatedDate";
var _Duration = null;
var _PageSize = null;
var _SelectionType = 'OR';
var _OldCategoryGUID = [];
var _CurrentCategoryFilter = new Array();
var widgetclipid =0;
var IsLibraryLoad = false;
var isDragging = false;
var isStoppedDrag = true;
var isResizeRegister = false;
var _WgtHBID=0;
var _WgtVBID=0;


function CheckForGallarySessionExpired(result) {
    if (result != null) {
        if (result.isSuccess == false && result.isAuthorized == false) {
            RedirectToUrl(result.redirectURL);
        }
    }
}

$(function () {

    if (screen.height >= 768) {
        $("#divGallaryResultScrollContent").css("height", documentHeight - 270);
        $("#divGallaryArea").css("min-height", documentHeight - 270);
    }
    else
    {
        $("#divGallaryArea").css("min-height", documentHeight -100);
    }
    
    UpdateGallaryResultScrollbarContent();


    $('#divGallaryMessage').html('');
    $('#dpGallaryFrom').val('');
    $('#dpGallaryTo').val('');

    $("#GallarytxtKeyword").keypress(function (e) {
        if (e.keyCode == 13) {
            SetGallaryKeyword();
        }
    });

    $("#imgGallaryKeyword").click(function (e) {
        SetGallaryKeyword();
    });

    $("#GallarytxtKeyword").blur(function () {
        SetGallaryKeyword();
    });

    $("body").click(function (e) {
        if ((e.target.id != "aGallaryPageSize" && e.target.id != "divGallaryPopover") && $(e.target).parents("#divGallaryPopover").size() <= 0) {
            $('#divGallaryPopover').remove();
        }

        if (e.target.id == "liGallaryCategoryFilter" || $(e.target).parents("#liGallaryCategoryFilter").size() > 0) {
            if ($('#ulGallaryCategory').is(':visible')) {
                $('#ulGallaryCategory').hide();
            }
            else {
                $('#ulGallaryCategory').show();
            }
        }
        else if ((e.target.id !== "liGallaryCategoryFilter" && e.target.id !== "ulGallaryCategory" && $(e.target).parents("#ulGallaryCategory").size() <= 0) || e.target.id == "btnGallarySearchCategory") {
            $('#ulGallaryCategory').hide();
            if (e.target.id != "btnGallarySearchCategory") {
                _GalleryCategoryGUID = [];
                _GalleryCategoryNameList = [];
                var categoriesHTML = "";
                if (_SelectionType == $('#rdGallaryAnd').val()) {
                    $('#rdGallaryAnd').prop("checked", true);
                }
                else {
                    $('#rdGallaryOr').prop("checked", true);
                }

                $.each(_CurrentCategoryFilter, function (eventID, eventData) {
                    if (_OldCategoryGUID.length > 0 && $.inArray(eventData.CategoryGUID, _OldCategoryGUID) !== -1) {
                        _GalleryCategoryGUID.push(eventData.CategoryGUID);
                        _GalleryCategoryNameList.push(eventData.CategoryName);
                    }
                    var liStyle = "";
                    if ($.inArray(eventData.CategoryGUID, _GalleryCategoryGUID) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    categoriesHTML = categoriesHTML + '<li role=\"presentation\"><a ' + liStyle + '  href=\"javascript:void(0)\" onclick="SetGallaryCategory(this,\'' + eventData.CategoryGUID + '\',\'' + eventData.CategoryName.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + '\');" tabindex=\"-1\" role=\"menuitem\">';
                    categoriesHTML += EscapeHTML(eventData.CategoryName) + ' (' + eventData.RecordCountFormatted + ') </a></li>';
                });
                if (categoriesHTML == '') {
                    $('#ulGallaryCategoryList').html('<li role="presentation"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
                }
                else {
                    $("#ulGallaryCategoryList").html(categoriesHTML);
                }
            }
        }
    });

    $("#divGallaryCalender").datepicker({
        beforeShowDay: enableAllTheseDays,
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            $('#dpGallaryFrom').val(dateText);
            $('#dpGallaryTo').val(dateText);
            SetGallaryDateVariable();
        }

    });

    $('.ndate').click(function () {
        $("#divGallaryCalender").datepicker("refresh");
    });


    $("#dpGallaryFrom").datepicker({
        //beforeShowDay: enableAllTheseDays,
        showOn: "button",
        buttonImage: "../images/calender.png",
        buttonImageOnly: true,
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            $('#dpGallaryFrom').val(dateText);
            SetGallaryDateVariable();
        },
        onClose: function (dateText) {
            //$('#dpGallaryFrom').focus();
            //SetGallaryDateVariable();
        }
    });

    $("#dpGallaryTo").datepicker({
        showOn: "button",
        buttonImage: "../images/calender.png",
        buttonImageOnly: true,
        changeMonth: true,
        changeYear: true,
        //beforeShowDay: enableAllTheseDays,
        onSelect: function (dateText, inst) {
            $('#dpGallaryTo').val(dateText);
            SetGallaryDateVariable();
        },
        onClose: function (dateText) {
            //$('#dpGallaryTo').focus();
            //SetGallaryDateVariable();
        }
    });

    // below function is called from IQMediaCommon.js
    // to set Height of content of the modal popup
    SetModalBodyScrollBarForPopUp();

    SetImageSrc();
    SetGallaryMediaClickEvent();

    $.fn.modal.Constructor.prototype.enforceFocus = function() {
        var $modalElement = this.$element;
        $(document).on('focusin.modal',function(e) {
                var $parent = $(e.target.parentNode);
                if ($modalElement[0] !== e.target
                                && !$modalElement.has(e.target).length
                                && $(e.target).parentsUntil('*[role="dialog"]').length === 0) {
                        $modalElement.focus();
                }
        });
};

});


function _resize_gridster(){

    if (screen.height >= 768) {
        $("#divGallaryResultScrollContent").css("height", documentHeight - 270);
        $("#divGallaryArea").css("min-height", documentHeight - 270);
    }
    else{
        $("#divGallaryArea").css("min-height", documentHeight -100);
    }

    $('object').css({'width':'100%','height':'100%'});

    //if(!isResizeRegister)
    //{
        //isResizeRegister = true;
        //var newcols = Math.floor($("#divTilesArea").width() / ((base_size * 4) + 10));
        //var newBaseSize = parseInt((newcols /cols)*base_size);

        if(screen.width >= 1200)
        {
            base_size = 25;
        }
        else
        {
            // considering 8 columns per row will accomodate. 
            // -10 done due to margin settings [5,5] as it also doing in gridster calculation
            // generated from col calculation 
            /*
                cols = xWidth / ((baseSize * 4) + 10)
                so, baseSize = ((xWidth / cols)  - 10) / 4
                cols = 8
            */
            base_size = Math.floor((($("#divGallaryArea").width()/8) - 10)/4);
        }

        cols = Math.floor($("#divGallaryArea").width() / ((base_size * 4) + 10));
		if(gridster != null)
        {
	        gridster.resize_widget_dimensions({
		        widget_base_dimensions: [base_size * 4, base_size * 3],
		        widget_margins: [5, 5],
		        max_cols : cols
	        });
        }
    //}
}

function OpenGallaryPopup(ID)
{
    $("#txtGallaryName").css("border-color", "#cccccc");
    $("#txtGallaryDescription").css("border-color", "#cccccc");
    $("#txtGallaryTitle").css("border-color", "#cccccc");
    if(ID > 0)
    {
        OpenEditGallaryPopup(ID);
    }
    else
    {
        OpenAddGallaryPopup();
    }
    CKEDITOR.replace('txtGallaryTitle',
    {
        height:'100%',
        width:'94.5%'
    }
    );
    //$("#txtGallaryTitle").jqte({ol: false, ul: false, sub:false, sup:false, outdent:false,indent:false,strike:false,link:false,remove:false,source:false,rule:false,unlink:false});
}

function OpenAddGallaryPopup(){
    $('#divGallaryPopup').modal({
        backdrop: 'static',
        keyboard: true,
        dynamic: true
    });
    
    if(gridster == null)
    {
        if(screen.width >= 1200)
        {
            base_size = 25;
        }
        else
        {
            // considering 8 columns per row will accomodate. 
            // -10 done due to margin settings [5,5] as it also doing in gridster calculation
            // generated from col calculation 
            /*
                cols = xWidth / ((baseSize * 4) + 10)
                so, baseSize = ((xWidth / cols)  - 10) / 4
                cols = 8
            */
            base_size = Math.floor((($("#divGallaryArea").width()/8) - 10)/4);
        }

        cols = Math.floor($("#divGallaryArea").width() / ((base_size * 4) + 10));
        offset  =10;
	  
        gridster = $(".gridster > ul").gridster({
            widget_margins: [5, 5],
            widget_base_dimensions: [base_size * 4, base_size * 3],
		    max_cols:cols,
            //extra_cols: 1,
            draggable: 
		    {
                //handle : '.div-player',
                start : function(event, ui) {
                    isDragging = true;
                    isStoppedDrag = false;
                },
			    stop: function(event, ui) {
                    isStoppedDrag = true;
                    setTimeout(function(){
                        if(isStoppedDrag){
                            isDragging = false;
                        }
                    },500);
			    }
		    }	
        }).data('gridster');
	  
		
	    $(document).on("click", ".gridster .CancelIcon", function (event) { 
		    if($(this).closest('li').find('object').length > 0)
            {
                $(this).closest('li').find('.div-player').html('');
                $(this).closest('li').find('.center-align-control').show();
            }
            else
            {
                gridster.remove_widget($(this).closest('li')[0]);
                widgetclipid = widgetclipid - 1;
            }
	    });
		
	
	    /* we're ready for the show */
	    $(window).on('resize load',_resize_gridster);
    }
}

function OpenEditGallaryPopup(ID){
    $("#hdnGalleryID").val(ID);
    $('#divGallaryPopup').modal({
        backdrop: 'static',
        keyboard: true,
        dynamic: true
    });
    
    if(gridster == null)
    {
//        if(screen.width >= 1200)
//        {
//            base_size = 25;
//        }
//        else
//        {
//            // considering 8 columns per row will accomodate. 
//            // -10 done due to margin settings [5,5] as it also doing in gridster calculation
//            // generated from col calculation 
//            /*
//                cols = xWidth / ((baseSize * 4) + 10)
//                so, baseSize = ((xWidth / cols)  - 10) / 4
//                cols = 8
//            */
//            base_size = Math.floor((($("#divGallaryArea").width()/8) - 10)/4);
//        }
        
        //cols = Math.floor($("#divGallaryArea").width() / ((base_size * 4) + 10));
        offset  =10;
	  
        gridster = $(".gridster > ul").gridster({
            widget_margins: [5, 5],
            widget_base_dimensions: [base_size * 4, base_size * 3],
		    max_cols:cols,
            //extra_cols: 1,
            draggable: 
		    {
                //handle : '.div-player',
                start : function(event, ui) {
                    isDragging = true;
                    isStoppedDrag = false;
                },
			    stop: function(event, ui) {
                    isStoppedDrag = true;
                    setTimeout(function(){
                        if(isStoppedDrag){
                            isDragging = false;
                        }
                    },500);
			    }
		    }	
        }).data('gridster');
	  
		
	    $(document).on("click", ".gridster .CancelIcon", function (event) { 
		    if($(this).closest('li').find('object').length > 0)
            {
                $(this).closest('li').find('.div-player').html('');
                $(this).closest('li').find('.center-align-control').show();
            }
            else
            {
                gridster.remove_widget($(this).closest('li')[0]); 
            }
	    });
		
	
        if(ID > 0)
        {
            var jsonPostData = { p_ID: ID }

            $.ajax({

                type: "post",
                dataType: "json",
                url: _urlLibraryGetGalleryByID,
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(jsonPostData),
                success: function(result){
                    if(result.isSuccess){
                        $("#txtGallaryName").val(result.gallery.Name);
                        //$(".jqte_editor").html(result.gallery.Title)
                        //CKEDITOR.instances.txtGallaryTitle.getData()
                        CKEDITOR.instances.txtGallaryTitle.setData(result.gallery.Title)
                        //$("#txtGallaryTitle").html(result.gallery.Title);
                        $("#txtGallaryDescription").val(result.gallery.Description);
                        $.each(result.gallery.Json, function (i, gallery) {
                            if(gallery.Type =='TVBlock'){
                                gridster.add_widget('<li id=' + gallery.ID + '><div class="CancelIcon">X</div><div class="content" id="clipimg_'+ i +'" ><div class="div-player"></div></div></li>', gallery.size_x, gallery.size_y, gallery.Col, gallery.Row);
                                widgetclipid = widgetclipid + 1;
                                $("#clipimg_" + i).css({'background-image':'url("'+ gallery.TVThumbUrl +'")','background-size':'100%'});
                                $("#clipimg_" + i).closest('.content').attr('onclick','RenderClipObject("'+ gallery.ClipID +'",this);');
                            } 
                            else if(gallery.Type =='HorizontalTextBlock')
                            {
                                if(navigator.userAgent.toLowerCase().indexOf("firefox") > 0)
                                {
                                    gridster.add_widget('<li id="HorizontalTextBlock" style="height:140px !important;" name="' + gallery.Type + '"><div class="CancelIcon">X</div><div class="contol-content"><textarea style="height:100%;width:100%;" class="editor center-align-control" type ="text" name="HorizontalTextBlock" id="hb_'+_WgtHBID+'">'+gallery.MetaData+'</textarea></div></li>', gallery.size_x, gallery.size_y);
                                }
                                else if(navigator.userAgent.toLowerCase().indexOf("chrome") > 0)
                                {
                                    gridster.add_widget('<li id="HorizontalTextBlock" style="height:120px !important;" name="' + gallery.Type + '"><div class="CancelIcon">X</div><div class="contol-content"><textarea style="height:100%;width:100%;" class="editor center-align-control" type ="text" name="HorizontalTextBlock" id="hb_'+_WgtHBID+'">'+gallery.MetaData+'</textarea></div></li>', gallery.size_x, gallery.size_y);
                                }
                                else
                                {
                                    gridster.add_widget('<li id="HorizontalTextBlock" name="' + gallery.Type + '"><div class="CancelIcon">X</div><div class="contol-content"><textarea style="height:100%;width:100%;" class="editor center-align-control" type ="text" name="HorizontalTextBlock" id="hb_'+_WgtHBID+'">'+gallery.MetaData+'</textarea></div></li>', gallery.size_x, gallery.size_y);
                                }
                                //gridster.add_widget('<li name="' + gallery.Type + '"><div class="CancelIcon">X</div><div class="contol-content"><input class="editor center-align-control" type ="text" name="HorizontalTextBlock" value="' + gallery.MetaData + '"></div></li>', gallery.size_x, gallery.size_y);
                               CKEDITOR.replace('hb_' + _WgtHBID);
                               //$("#hb_"+_WgtHBID).jqte({ol: false, ul: false, sub:false, sup:false, outdent:false,indent:false,strike:false,link:false,remove:false,source:false,rule:false,unlink:false});
                               _WgtHBID=_WgtHBID+1;
                            }
                            else
                            {
                                if(navigator.userAgent.toLowerCase().indexOf("firefox") > 0)
                                {
                                    gridster.add_widget('<li id="VerticalTextBlock" style="height:330 !important;" name="' + gallery.Type + '"><div class="CancelIcon">X</div><div class="contol-content"><textarea style="height:100%;width:100%;" class="editor center-align-control" type ="text" id="vb_'+_WgtVBID+'" name="VerticalTextBlock">'+gallery.MetaData+'</textarea></div></li>', gallery.size_x, gallery.size_y);
                                }
                                else if(navigator.userAgent.toLowerCase().indexOf("chrome") > 0)
                                {
                                    gridster.add_widget('<li id="VerticalTextBlock" style="height:285px !important;" name="' + gallery.Type + '"><div class="CancelIcon">X</div><div class="contol-content"><textarea style="height:100%;width:100%;" class="editor center-align-control" type ="text" id="vb_'+_WgtVBID+'" name="VerticalTextBlock">'+gallery.MetaData+'</textarea></div></li>', gallery.size_x, gallery.size_y);
                                }
                                else
                                {
                                    gridster.add_widget('<li id="VerticalTextBlock" name="' + gallery.Type + '"><div class="CancelIcon">X</div><div class="contol-content"><textarea style="height:100%;width:100%;" class="editor center-align-control" type ="text" id="vb_'+_WgtVBID+'" name="VerticalTextBlock">'+gallery.MetaData+'</textarea></div></li>', gallery.size_x, gallery.size_y);
                                }
                                CKEDITOR.replace('vb_' + _WgtVBID);
                                //$("#vb_"+_WgtVBID).jqte({ol: false, ul: false, sub:false, sup:false, outdent:false,indent:false,strike:false,link:false,remove:false,source:false,rule:false,unlink:false});
                                _WgtVBID=_WgtVBID+1;
                            }

                        });
                    }else{
                        ShowNotification(_msgSomeErrorProcessing);
                    }
                },
                error: function(a,b,c){
                    ShowNotification(_msgSomeErrorProcessing);
                }
            });
        }
        $(window).on('resize load',_resize_gridster);
    }
}

function CancelGallaryPopup(){
    if(CKEDITOR.instances.txtGallaryTitle != undefined)
    {
        CKEDITOR.instances.txtGallaryTitle.destroy()
    }
    $("#divGallaryPopup").css({ "display": "none" });
    $("#divGallaryPopup").modal("hide");
    $("#txtGallaryName").val('');
    $("#txtGallaryTitle").val('');
    $("#txtGallaryDescription").val('');
    $("#hdnGalleryID").val('');
    $(".gridster").html('<ul></ul>');
    $('.CancelIcon').click();
    widgetclipid = 0;
    _WgtHBID=0;
    _WgtVBID=0;
    gridster = null;
}

function AddGallary() {
    var type = $("#ddlGalleryType").val();
    if(type =='TVBlock'){
        gridster.add_widget('<li name="TVBlock" ><div class="CancelIcon">X</div><div class="content" onclick="OpenLibraryPopup(this);" id="clipimg_'+ widgetclipid +'" ><div class="div-player"></div></div></li>', 4, 4);
        widgetclipid = widgetclipid + 1;
    }
    else if(type =='HorizontalTextBlock'){
        if(navigator.userAgent.toLowerCase().indexOf("firefox") > 0)
        {
            gridster.add_widget('<li id="HorizontalTextBlock" name="HorizontalTextBlock" style="height:140px;"><div class="CancelIcon">X</div><div class="contol-content" style="height:100% !important;"><textarea style="height:183px;width:440px;" class="editor center-align-control" id="hb_'+_WgtHBID+'"></textarea></div></li>', 4, 2);
        }
        else if(navigator.userAgent.toLowerCase().indexOf("chrome") > 0)
        {
            gridster.add_widget('<li id="HorizontalTextBlock" name="HorizontalTextBlock" style="height:120px;"><div class="CancelIcon">X</div><div class="contol-content" style="height:100% !important;"><textarea style="height:183px;width:440px;" class="editor center-align-control" id="hb_'+_WgtHBID+'"></textarea></div></li>', 4, 2);
        }
        else
        {
            gridster.add_widget('<li id="HorizontalTextBlock" name="HorizontalTextBlock" style="height:140px;"><div class="CancelIcon">X</div><div class="contol-content" style="height:100% !important;"><textarea style="height:183px;width:440px;" class="editor center-align-control" id="hb_'+_WgtHBID+'"></textarea></div></li>', 4, 2);
        }
        //gridster.add_widget('<li name="HorizontalTextBlock"><div class="CancelIcon">X</div><div class="contol-content"><input class="editor center-align-control" type ="text" name="HorizontalTextBlock" value=""></div></li>', 4, 2);        
        CKEDITOR.replace( 'hb_' + _WgtHBID);

        //$("#hb_"+_WgtHBID).jqte({ol: false, ul: false, sub:false, sup:false, outdent:false,indent:false,strike:false,link:false,remove:false,source:false,rule:false,unlink:false});
        _WgtHBID=_WgtHBID+1;
//        $(".editor").jqte();
//        $(".editor").jqte();
    }
    else if(type =='VerticalTextBlock')
    {
        if(navigator.userAgent.toLowerCase().indexOf("firefox") > 0)
        {
            gridster.add_widget('<li id="VerticalTextBlock" name="VerticalTextBlock" style="height:330px;"><div class="CancelIcon">X</div><div class="contol-content"><textarea style="height:100%;width:100%;" class="editor center-align-control" id="vb_'+_WgtVBID+'"></textarea></div></li>', 2, 4);
        }
        else if(navigator.userAgent.toLowerCase().indexOf("chrome") > 0)
        {
            gridster.add_widget('<li id="VerticalTextBlock" name="VerticalTextBlock" style="height:285px;"><div class="CancelIcon">X</div><div class="contol-content"><textarea style="height:100%;width:100%;" class="editor center-align-control" id="vb_'+_WgtVBID+'"></textarea></div></li>', 2, 4);
        }
        else
        {
            gridster.add_widget('<li id="VerticalTextBlock" name="VerticalTextBlock" style="height:285px;"><div class="CancelIcon">X</div><div class="contol-content"><textarea style="height:100%;width:100%;" class="editor center-align-control" id="vb_'+_WgtVBID+'"></textarea></div></li>', 2, 4);
        }
        CKEDITOR.replace('vb_' + _WgtVBID);
        //$("#vb_"+_WgtVBID).jqte({ol: false, ul: false, sub:false, sup:false, outdent:false,indent:false,strike:false,link:false,remove:false,source:false,rule:false,unlink:false});
        _WgtVBID=_WgtVBID+1;
    }
}

function SetClipSelection(){
    var lstGallery = $(".media input:checked")
    var lstAssignGallery = [];
    var isExist = false;

    var emptyCount = 0;
    $.each(lstGallery,function(i,gallery) {
        $.each($(".gridster > ul > li"),function(i,obj) {
            if(gallery.value == obj.id)
            {
                ShowNotification(_msgGalleryRecordAlreadyExists.replace(/@@record@@/g, gallery.name));
                isExist = true;
                return;
            }
        });
    });
    if(!isExist)
    {
        $.each($(".gridster > ul > li"),function(i,obj) {
            if(obj.id == "" && !(obj.name == "VerticalTextBlock" || obj.name == "HorizontalTextBlock"))
            {
                emptyCount = emptyCount + 1;
            }
            else if(!(obj.id == "VerticalTextBlock" || obj.id == "HorizontalTextBlock"))
            {
                lstAssignGallery.push(obj.id);
            }
        });
        if(!(lstGallery.length <= emptyCount))
        {
            for (var i = 0; i < lstGallery.length - emptyCount; i++) {
                AddGallary();
            }
        }
        var selectedGalleryCount = 0;
        var clipImageCount = 0;
        $.each($(".gridster > ul > li"),function(i,obj) {
            if(obj.id == "")
            {
                if(lstGallery[selectedGalleryCount] != undefined && !(lstGallery[selectedGalleryCount].value in lstAssignGallery))
                {
                    $("#clipimg_" + (clipImageCount)).parent().attr('id',lstGallery[selectedGalleryCount].value);
                    if($("#tvthumb_" + lstGallery[selectedGalleryCount].value).is('input'))
                    {
                            $("#clipimg_" + (clipImageCount)).css({'background-image':'url("'+$("#tvthumb_" + lstGallery[selectedGalleryCount].value).val()+'")','background-size':'100%'});
                            $("#clipimg_" + (clipImageCount)).closest('.content').attr('onclick','RenderClipObject("'+ $("#hdnclipid_" + lstGallery[selectedGalleryCount].value).val() +'",this);');
                    }
                    else
                    {
                            $("#clipimg_" + (clipImageCount)).css({'background-image':'url("'+$("#tvthumb_" + lstGallery[selectedGalleryCount].value).attr('src')+'")','background-size':'100%'});
                            $("#clipimg_" + (clipImageCount)).closest('.content').attr('onclick','RenderClipObject("'+ $("#hdnclipid_" + lstGallery[selectedGalleryCount].value).val() +'",this);');
                    }
                    selectedGalleryCount = selectedGalleryCount + 1;
                    clipImageCount = clipImageCount + 1;
                }
            }
            else if(!(obj.id == "VerticalTextBlock" || obj.id == "HorizontalTextBlock"))
            {
                clipImageCount = clipImageCount + 1;
            }
        });
        CancelLibraryPopup();
    }
}

function RefreshTVResults()
{
    $.ajax({
        url: _urlGalleryRefreshTVResults,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: 'json',
        data: {},
        success: function (result) {

            //CheckForGallarySessionExpired(result);

            // Set filter irrespective of success or failure
            //SetGallaryActiveFilter();
            
            if (result.isSuccess) {

                _SearchTerm = "";
                _FromDate = null;
                _ToDate = null;
                _GalleryCategoryGUID = [];
                _GalleryCategoryNameList = [];
                _CategoryName = "";
                _Date = null;
                _Customer = "";
                _CustomerName = "";
                _IsAsc = false;
                _SortColumn = "CreatedDate";
                _Duration = null;
                _PageSize = null;
                _SelectionType = 'OR';
                _OldCategoryGUID = [];
                _CurrentCategoryFilter = new Array();

                $("#divShowGallaryResult").show();
                $("#divGallaryRecordCount").show();
                $("#ulIQArchieveGallaryResults").html(result.HTML);
                ModifyGallaryFilters(result.filter);
                _CurrentCategoryFilter = result.filter.Categories.slice(0);
                ShowHideGallaryMoreResults(result);
                ShowNoofGallaryRecords(result);
                SetGallaryMediaClickEvent();
                    
                if (screen.height >= 768) {
                    setTimeout(function () { $("#divGallaryResultScrollContent").mCustomScrollbar("scrollTo", "top"); }, 200);
                }

                $(".media input[type=checkbox]").each( function(){ 
                    $(this).prop("checked", false);
                    $(this).closest('.media').css('background', '');
                });

                SetImageSrc();

//                if(isOpenPopup)
//                {
//                    IsLibraryLoad = true;
//                    $('#divLibraryPopup').modal({
//                        backdrop: 'static',
//                        keyboard: true,
//                        dynamic: true
//                    });
//                }
            $("#divGallaryActiveFilter").html("");
            }
            else {
                ShowNotification(_msgSomeErrorProcessing);
                ClearResultsOnError('ulIQArchieveGallaryResults', 'divGallaryRecordCount', 'divShowGallaryResult', _msgErrorOnSearch.replace(/@@MethodName@@/g, "RefreshGallaryResults()"));
            }
        },
        error: function (a, b, c) {
                
            ShowNotification(_msgSomeErrorProcessing, a);
            ClearResultsOnError('ulIQArchieveGallaryResults', 'divGallaryRecordCount', 'divShowGallaryResult', _msgErrorOnSearch.replace(/@@MethodName@@/g, "RefreshGallaryResults()"));
        }
    });
    
}

function RenderClipObject(clipID,container){
    
    if(!isDragging){

        var jsonPostData = { ClipID: clipID }

        if($(container).find('object').length <= 0){
            $.ajax({

                type: "post",
                dataType: "json",
                url: _urlGalleryRenderClipObject,
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(jsonPostData),
                success: function(result){
                    if(result.isSuccess){
                        $(container).find('.div-player').html(result.clipHTML)
                        $(container).find('img').hide();
                    }else{
                        ShowNotification(_msgSomeErrorProcessing);
                    }
                },
                error: function(a,b,c){
                    ShowNotification(_msgSomeErrorProcessing);
                }
            });
        }
    }
}

function UpdateGallaryResultScrollbarContent() {
    if (screen.height >= 768) {
        $("#divGallaryResultScrollContent").mCustomScrollbar({
            advanced: {
                updateOnContentResize: true,
                autoScrollOnFocus: false
            }
        });
    }
}

function OpenLibraryPopup(divObj){
    
    if(!isDragging)
    {
        $("#hdnSquareBoxId").val(divObj.id);

        if(IsLibraryLoad){
            $('#divLibraryPopup').modal({
                backdrop: 'static',
                keyboard: true,
                dynamic: true
            });
        }
        else{
            RefreshGallaryResults(true);
        }
    }
}

function CancelLibraryPopup(){
    $("#divLibraryPopup").css({ "display": "none" });
    $("#divLibraryPopup").modal("hide");
    $("#hdnSquareBoxId").val('');
    $(".media input[type=checkbox]").each( function(){ 
        $(this).prop("checked", false);
        $(this).closest('.media').css('background', '');
    });
}


function enableAllTheseDays(date) {
    date = $.datepicker.formatDate("mm/dd/yy", date);
    return [$.inArray(date, disabledDays) !== -1];
}

function SetGallaryKeyword() {
    if ($("#GallarytxtKeyword").val() != "" && _SearchTerm != $("#GallarytxtKeyword").val()) {
        _SearchTerm = $("#GallarytxtKeyword").val();
        RefreshGallaryResults();
    }
}

function SetGallaryDateVariable() {

    if ($("#dpGallaryFrom").val() && $("#dpGallaryTo").val()) {
        if (_FromDate != $("#dpGallaryFrom").val() || _ToDate != $("#dpGallaryTo").val()) {
            _FromDate = $("#dpGallaryFrom").val();
            _ToDate = $("#dpGallaryTo").val();
            RefreshGallaryResults();
            $('#ulGallaryCalender').parent().removeClass('open');
            $('#aGallaryDuration').html(_msgCustom + '&nbsp;&nbsp;<span class="caret"></span>');
        }
    }
    else
        if ($("#dpGallaryFrom").val() != "" && $("#dpGallaryTo").val() == "") {
            $("#dpGallaryTo").addClass("warningInput");
        }
        else if ($("#dpGallaryTo").val() != "" && $("#dpGallaryFrom").val() == "") {
            $("#dpGallaryFrom").addClass("warningInput");
        }
}

function SetGallaryCustomer(p_Customer, p_CustomerName) {
    if (_Customer != p_Customer) {
        _Customer = p_Customer;
        _CustomerName = p_CustomerName;
        RefreshGallaryResults();
    }
}

function SetGallaryCategory(eleCategory, p_CategoryGUID, p_CategoryName) {
    if ($.inArray(p_CategoryGUID, _GalleryCategoryGUID) == -1) {
        _GalleryCategoryGUID.push(p_CategoryGUID);
        _GalleryCategoryNameList.push(p_CategoryName);
        $(eleCategory).css("background-color", "#E9E9E9");
    }
    else {
        var catIndex = _GalleryCategoryGUID.indexOf(p_CategoryGUID);
        if (catIndex > -1) {
            _GalleryCategoryGUID.splice(catIndex, 1);
            _GalleryCategoryNameList.splice(catIndex, 1);
            $(eleCategory).css("background-color", "#ffffff");
        }
    }

    if ($('#rdGallaryOr').is(":checked")) {
        $("#ulGallaryCategoryList li").each(function () {
            var _Category = $(this).find('a').attr("onclick").replace('SetGallaryCategory(this,', '').replace(');', '').split(',')[0].replace(/'/g, '');
            if ($.inArray(_Category, _GalleryCategoryGUID) == -1) {
                $(this).find('a').addClass('blur');
            }
        });

        GetGallaryCategoryFilter(false);
    }
}

function GetGallaryCategoryFilter(isClear) {

    var jsonPostData = {
        p_CustomerGuid: _Customer,
        p_FromDate: _FromDate,
        p_ToDate: _ToDate,
        p_SearchTerm: _SearchTerm,
        p_CategoryGUID: _GalleryCategoryGUID
    }

    $.ajax({
        url: _urlGalleryFilterCategory,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: 'json',
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess) {
                var categoriesHTML = "";
                $.each(result.categoryFilter, function (eventID, eventData) {
                    var liStyle = "";
                    if ($.inArray(eventData.CategoryGUID, _GalleryCategoryGUID) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    categoriesHTML = categoriesHTML + '<li role=\"presentation\"><a ' + liStyle + '  href=\"javascript:void(0)\" onclick="SetGallaryCategory(this,\'' + eventData.CategoryGUID + '\',\'' + eventData.CategoryName.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + '\');" tabindex=\"-1\" role=\"menuitem\">';
                    categoriesHTML += EscapeHTML(eventData.CategoryName) + ' (' + eventData.RecordCountFormatted + ') </a></li>';
                });

                $("#ulGallaryCategoryList").html(categoriesHTML);
            }
            else {
                ShowNotification(_msgSomeErrorProcessing);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgSomeErrorProcessing, a);
        }
    });
}

function SearchGallaryCategory() {
    var _CurrentSelectionType;
    var IsPost = false;
    if ($('#rdGallaryAnd').is(":checked")) {
        _CurrentSelectionType = $('#rdGallaryAnd').val();
    }
    else {
        _CurrentSelectionType = $('#rdGallaryOr').val();
    }
    if ($(_GalleryCategoryGUID).not(_OldCategoryGUID).length != 0 || $(_OldCategoryGUID).not(_GalleryCategoryGUID).length != 0) {
        IsPost = true;
    }
    else if (_GalleryCategoryGUID.length > 1 && (_SelectionType != '' && _SelectionType != _CurrentSelectionType)) {
        IsPost = true;
    }

    if (IsPost) {
        _OldCategoryGUID = _GalleryCategoryGUID.slice(0);
        _SelectionType = _CurrentSelectionType;
        _CategoryName = "";
        $.each(_GalleryCategoryNameList, function (index, val) {
            if (_CategoryName == "") {
                _CategoryName = val;
            }
            else {
                _CategoryName = _CategoryName + ' "' + _SelectionType + '" ' + val;
            }
        });
        RefreshGallaryResults();
    }
}

function SetGallarySelectionType(IsClear) {
    if (_GalleryCategoryGUID.length > 0 || _OldCategoryGUID.length > 0) {
        if ($('#rdGallaryOr').is(":checked")) {
            _GalleryCategoryGUID = [];
            _GalleryCategoryNameList = [];
            GetGallaryCategoryFilter();
        }
        else {
            _GalleryCategoryGUID = [];
            _GalleryCategoryNameList = [];
            GetGallaryCategoryFilter();
        }
    }
}

function GetMoreGallaryResults() {

    var jsonPostData = {
        p_CustomerGuid: _Customer,
        p_FromDate: _FromDate,
        p_ToDate: _ToDate,
        p_SearchTerm: _SearchTerm,
        p_CategoryGUID: _GalleryCategoryGUID,
        p_SelectionType: _SelectionType,
        p_IsAsc: _IsAsc,
        p_SortColumn: _SortColumn,
        p_PageSize: _PageSize
    }

    $.ajaxSetup({ cache: false });

    ShowHideGalleryProgressNearButton(true);


    $("#btnShowMoreResults").attr("disabled", "disabled");
    $("#btnShowMoreResults").attr("class", "disablebtn");

    $.ajax({
        url: _urlGalleryGetMoreResults,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: 'json',
        data: JSON.stringify(jsonPostData),
        success: function (result) {

            CheckForGallarySessionExpired(result);

            if (result.isSuccess) {
                $("#ulIQArchieveGallaryResults").append(result.HTML);
                ShowHideGallaryMoreResults(result);
                ShowNoofGallaryRecords(result);
                SetGallaryMediaClickEvent();
                SetImageSrc();
            }
            else {
                ShowNotification(_msgSomeErrorProcessing);
            }
            ShowHideGalleryProgressNearButton(false);
            $("#btnShowMoreResults").removeAttr("disabled");
            $("#btnShowMoreResults").attr("class", "loadmore");
        },
        error: function (a, b, c) {
            ShowHideGalleryProgressNearButton(false);
            ShowNotification(_msgSomeErrorProcessing, a);

            $("#btnShowMoreResults").removeAttr("disabled");
            $("#btnShowMoreResults").attr("class", "loadmore");
        },
        global: false // This will not enable ".ajaxStart() global event handler and so global progress bar will not fire.
    });


}

function RefreshGallaryResults(isOpenPopup) {

    var jsonPostData = {
        p_CustomerGuid: _Customer,
        p_FromDate: _FromDate,
        p_ToDate: _ToDate,
        p_SearchTerm: _SearchTerm,
        p_CategoryGUID: _GalleryCategoryGUID,
        p_SelectionType: _SelectionType,
        p_IsAsc: _IsAsc,
        p_SortColumn: _SortColumn,
        p_PageSize: _PageSize
    }


    if (GallaryDateValidation()) {

        $.ajax({
            url: _urlGallerySearchLibraryResults,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: 'json',
            data: JSON.stringify(jsonPostData),
            success: function (result) {

                //CheckForGallarySessionExpired(result);

                // Set filter irrespective of success or failure
                SetGallaryActiveFilter();

                if (result.isSuccess) {
                    $("#divShowGallaryResult").show();
                    $("#divGallaryRecordCount").show();
                    $("#ulIQArchieveGallaryResults").html(result.HTML);
                    ModifyGallaryFilters(result.filter);
                    _CurrentCategoryFilter = result.filter.Categories.slice(0);
                    ShowHideGallaryMoreResults(result);
                    ShowNoofGallaryRecords(result);
                    SetGallaryMediaClickEvent();
                    
                    if (screen.height >= 768) {
                        setTimeout(function () { $("#divGallaryResultScrollContent").mCustomScrollbar("scrollTo", "top"); }, 200);
                    }

                    $(".media input[type=checkbox]").each( function(){ 
                        $(this).prop("checked", false);
                        $(this).closest('.media').css('background', '');
                    });

                    SetImageSrc();

                    if(isOpenPopup)
                    {
                        IsLibraryLoad = true;
                        $('#divLibraryPopup').modal({
                            backdrop: 'static',
                            keyboard: true,
                            dynamic: true
                        });
                    }
                }
                else {
                    ShowNotification(_msgSomeErrorProcessing);
                    ClearResultsOnError('ulIQArchieveGallaryResults', 'divGallaryRecordCount', 'divShowGallaryResult', _msgErrorOnSearch.replace(/@@MethodName@@/g, "RefreshGallaryResults()"));
                }
            },
            error: function (a, b, c) {
                
                ShowNotification(_msgSomeErrorProcessing, a);
                ClearResultsOnError('ulIQArchieveGallaryResults', 'divGallaryRecordCount', 'divShowGallaryResult', _msgErrorOnSearch.replace(/@@MethodName@@/g, "RefreshGallaryResults()"));
            }
        });
    }
}

function ShowNoofGallaryRecords(result) {
    if (result != null && result.totalRecords != null && result.totalRecords != "0" && result.currentRecords != null) {
        $("#spanGallaryRecordsLabel").show();
        if (Isv4LibraryRollup) {
            $("#spanGallaryCurrentRecords").html(result.currentRecords + " (" + result.currentRecordsDisplay + ")");
            $("#spanGallaryTotalRecords").html(result.totalRecords + " (" + result.totalRecordsDisplay + ")");
        }
        else {
            $("#spanGallaryCurrentRecords").html(result.currentRecords);
            $("#spanGallaryTotalRecords").html(result.totalRecords);
        }
        $("#aGallaryPageSize").show();
        $("#imgMoreGallaryResultLoading").addClass("offset2")
    }
    else {
        $("#spanGallaryRecordsLabel").hide();
        $("#aGallaryPageSize").hide();
        $("#imgMoreGallaryResultLoading").removeClass("offset2")
    }
}

function ModifyGallaryFilters(filter) {

    disabledDays = [];
    if (filter != null && filter.Dates.length > 0) {
        $.each(filter.Dates, function (id, date) {
            disabledDays.push(date);
        });
    }

    if (filter != null && filter.SubMediaTypes != null) {

        var subMediaTypeHTML = "";
        $.each(filter.SubMediaTypes, function (eventID, eventData) {
            subMediaTypeHTML = subMediaTypeHTML + '<li onclick="SetSubMediaType(\'' + eventData.SubMediaType + '\',\'' + eventData.SubMediaTypeDescription + '\');" role=\"presentation\"><a href=\"#\" tabindex=\"-1\" role=\"menuitem\">';
            subMediaTypeHTML += eventData.SubMediaTypeDescription + ' (' + eventData.RecordCountFormatted + ') </a></li>';
        });

        if (subMediaTypeHTML == '') {
            $('#ulSubMediaType').html('<li role="presentation"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
        }
        else {
            $('#ulSubMediaType').html(subMediaTypeHTML);
        }
    }

    if (filter != null && filter.Categories != null) {

        var categoriesHTML = "";
        var strAndSelection = "";
        var strOrSelection = ""
        if (_SelectionType == 'AND') {
            strAndSelection = "checked=\"checked\"";
        }
        else {
            strOrSelection = "checked=\"checked\"";
        }

        //var liCatSearchType ="<div class=\"fleft margintop5\"><input type=\"radio\" onclick=\"SetGallarySelectionType()\" value=\"AND\" id=\"rdGallaryOr\" style=\"margin-top: -3px\" name=\"rdSelectionType\" "+strAndSelection+" />AND &nbsp;<input type=\"radio\" onclick=\"SetGallarySelectionType()\" value=\"OR\" id=\"rdGallaryOr\" style=\"margin-top: -3px\" name=\"rdSelectionType\" "+strOrSelection+" />OR</div>";
        //var liSearchBtn = "<div><input type=\"button\" value=\"Done\" id=\"btnGallarySearchCategory\" class=\"button\" style=\"margin: 0px\" onclick=\"SearchGallaryCategory();\"></div>"
        //var liCatSearchDone ="<li role=\"presentation\" style=\"padding: 0px;\"><ul class=\"sideMenu sub-submenu\" aria-labelledby=\"drop2\" role=\"menu\"><li role=\"presentation\" style=\"text-align: right\"><div>" + liCatSearchType + liSearchBtn + "</div></li></ul></li>";
        $.each(filter.Categories, function (eventID, eventData) {
            var liStyle = "";
            if ($.inArray(eventData.CategoryGUID, _GalleryCategoryGUID) !== -1) {
                liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
            }
            categoriesHTML = categoriesHTML + '<li role=\"presentation\"><a ' + liStyle + '  href=\"javascript:void(0)\" onclick="SetGallaryCategory(this,\'' + eventData.CategoryGUID + '\',\'' + eventData.CategoryName.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + '\');" tabindex=\"-1\" role=\"menuitem\">';
            categoriesHTML += EscapeHTML(eventData.CategoryName) + ' (' + eventData.RecordCountFormatted + ') </a></li>';
        });

        if (categoriesHTML == '') {
            $('#ulGallaryCategoryList').html('<li role="presentation"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
            $('#liGallaryCategorySearch').hide();
        }
        else {
            //var StrLiStart ="<li role=\"presentation\" style=\"padding: 0px;\"><ul id=\"ulCategoryList\" class=\"sideMenu sub-submenu\" aria-labelledby=\"drop2\" role=\"menu\">";
            //var StrLiEnd ="</ul></li>"
            $('#ulGallaryCategoryList').html(categoriesHTML);
            $('#liGallaryCategorySearch').show();
        }
    }

    if (filter != null && filter.Customers != null) {

        var customerHTML = "";
        $.each(filter.Customers, function (eventID, eventData) {
            customerHTML = customerHTML + '<li onclick="SetGallaryCustomer(\'' + eventData.CustomerKey + '\',\'' + eventData.CustomerName.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + '\');" role=\"presentation\"><a href=\"#\" tabindex=\"-1\" role=\"menuitem\">';
            customerHTML += eventData.CustomerName + ' (' + eventData.RecordCountFormatted + ') </a></li>';
        });

        if (customerHTML == "") {
            $('#ulGallaryCustomers').html('<li role="presentation"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
        }
        else {
            $('#ulGallaryCustomers').html(customerHTML);
        }
    }
}

function SetGallaryActiveFilter() {

    var isFilterEnable = false;
    $("#divGallaryActiveFilter").html("");


    if (_SearchTerm != "") {
        $('#divGallaryActiveFilter').append('<div id="divKeywordActiveFilter" class="filter-in">' + EscapeHTML(_SearchTerm) + '<span class="cancel" onclick="RemoveGallaryFilter(1);"></span></div>');
        isFilterEnable = true;
    }
    if ((_FromDate != null && _FromDate != "") && (_ToDate != null && _ToDate != "")) {
        $('#divGallaryActiveFilter').append('<div id="divDateActiveFilter" class="filter-in">' + _FromDate + ' To ' + _ToDate + '<span class="cancel" onclick="RemoveGallaryFilter(2);"></span></div>');
        isFilterEnable = true;
    }
    if (_Customer != "") {
        $('#divGallaryActiveFilter').append('<div id="divCustomerActiveFilter" class="filter-in">' + _CustomerName + '<span class="cancel" onclick="RemoveGallaryFilter(4);"></span></div>');
        isFilterEnable = true;
    }
    if (_GalleryCategoryGUID.length > 0) {
        $('#divGallaryActiveFilter').append('<div id="divCategoryActiveFilter" class="filter-in">' + EscapeHTML(_CategoryName) + '<span class="cancel" onclick="RemoveGallaryFilter(5);"></span></div>');
        isFilterEnable = true;
    }


    if (isFilterEnable) {
        $("#divGallaryActiveFilter").css({ 'border-bottom': '1px solid rgb(236, 236, 236)' });
    }
    else {
        $("#divGallaryActiveFilter").css({ 'border-bottom': '' });
    }
}

function RemoveGallaryFilter(filterType) {

    // Represent SearchKeyword
    if (filterType == 1) {

        $("#GallarytxtKeyword").val("");
        _SearchTerm = "";
        RefreshGallaryResults();
    }

    // Represent Date filter(From Date,To Date)
    if (filterType == 2) {

        $("#dpGallaryFrom").datepicker("setDate", null);
        $("#dpGallaryTo").datepicker("setDate", null);
        $("#divGallaryCalender").datepicker("setDate", null);

        $('#aGallaryDuration').html(_msgAll + '&nbsp;&nbsp;<span class="caret"></span>');

        _FromDate = null;
        _ToDate = null;
        RefreshGallaryResults();
    }

    // Represent Customer Filter
    if (filterType == 4) {
        _Customer = "";
        _CustomerName = "";
        RefreshGallaryResults();
    }

    // Represent Category Filter
    if (filterType == 5) {
        _GalleryCategoryGUID = [];
        _GalleryCategoryNameList = [];
        _OldCategoryGUID = [];
        _CategoryName = "";
        _SelectionType = $('#rdGallaryOr').val();
        RefreshGallaryResults();
    }

}

function SortGallaryDirection(p_SortColumn, isAsc) {
    if (isAsc != _IsAsc || _SortColumn != p_SortColumn) {
        _IsAsc = isAsc;
        _SortColumn = p_SortColumn;

        if (_IsAsc && _SortColumn == "MediaDate") {
            $('#aGallarySortDirection').html(_msgOldestFirst + ' (Media Date)&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (!_IsAsc && _SortColumn == "MediaDate") {
            $('#aGallarySortDirection').html(_msgMostRecent + ' (Media Date)&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (_IsAsc && _SortColumn == "CreatedDate") {
            $('#aGallarySortDirection').html(_msgOldestFirst + ' (Created Date)&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (!_IsAsc && _SortColumn == "CreatedDate") {
            $('#aGallarySortDirection').html(_msgMostRecent + ' (Created Date)&nbsp;&nbsp;<span class="caret"></span>');
        }
        RefreshGallaryResults();
    }
}

function ShowHideGalleryProgressNearButton(value) {
    if (value == true) {
        $("#imgMoreGallaryResultLoading").removeClass("visibilityHidden");
    }
    else {
        $("#imgMoreGallaryResultLoading").addClass("visibilityHidden");
    }
}

function ShowHideGallaryMoreResults(result) {
    if (result.hasMoreResults == true) {
        $('#btnShowMoreGallaryResults').attr('value', _msgShowMoreResults);
        $('#btnShowMoreGallaryResults').attr('onclick', 'GetMoreGallaryResults();');
    }
    else {
        $('#btnShowMoreGallaryResults').attr('value', _msgNoMoreResult);
        $('#btnShowMoreGallaryResults').removeAttr('onclick');
    }
}

function GallaryDateValidation() {
    $('#dpGallaryFrom').removeClass('warningInput');
    $('#dpGallaryTo').removeClass('warningInput');


    // if both empty
    if (($('#dpGallaryTo').val() == '') && ($('#dpGallaryFrom').val() == '')) {
        return true;

    }
    //if one empty not other
    else if (($('#dpGallaryFrom').val() != '') && ($('#dpGallaryTo').val() == '')
                    ||
                    ($('#dpGallaryFrom').val() == '') && ($('#dpGallaryTo').val() != '')
                    ) {
        if ($('#dpGallaryFrom').val() == '') {

            ShowNotification(_msgFromDateNotSelected);
            $('#dpGallaryFrom').addClass('warningInput');
        }

        if ($('#dpGallaryTo').val() == '') {

            ShowNotification(_msgToDateNotSelected);
            $('#dpGallaryTo').addClass('warningInput');

        }
        return false;
    }
    //if both not empty
    else {
        var isFromDateValid = isValidDate($('#dpGallaryFrom').val().toString());
        var isToDateValid = isValidDate($('#dpGallaryTo').val().toString());
        if (isFromDateValid && isToDateValid) {
            var fromDate = new Date($('#dpGallaryFrom').val());
            var toDate = new Date($('#dpGallaryTo').val());
            if (fromDate > toDate) {
                ShowNotification(_msgFromDateLessThanToDate);
                $('#dpGallaryFrom').addClass('warningInput');
                $('#dpGallaryTo').addClass('warningInput');
                return false;
            }
            else {
                return true;

            }
        }
        else {
            if (!isFromDateValid) {
                $('#dpGallaryFrom').addClass('warningInput');
            }
            if (!isToDateValid) {
                $('#dpGallaryTo').addClass('warningInput');
            }
            ShowNotification(_msgInvalidDate);
            return false;
        }
    }
}

function isValidDate(s) {
    var bits = s.split('/');
    var y = bits[2], m = bits[0], d = bits[1];

    // Assume not leap year by default (note zero index for Jan) 
    var daysInMonth = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

    // If evenly divisible by 4 and not evenly divisible by 100, 
    // or is evenly divisible by 400, then a leap year 
    if ((!(y % 4) && y % 100) || !(y % 400)) {
        daysInMonth[1] = 29;
    }

    return d <= daysInMonth[--m]
}

function GetGallaryResultOnDuration(duration) {

    $("#dpGallaryFrom").removeClass("warningInput");
    $("#dpGallaryTo").removeClass("warningInput");
    var dtcurrent = new Date();
    var fDate;
    _Duration = duration;

    // All
    if (duration == 0) {
        $("#dpGallaryFrom").val("");
        $("#dpGallaryTo").val("");
        dtcurrent = "";
        RemoveGallaryFilter(2);
        $('#aGallaryDuration').html(_msgAll + '&nbsp;&nbsp;<span class="caret"></span>');
    }

    // 24 hours
    else if (duration == 1) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth(), dtcurrent.getDate() - 1);
        $('#aGallaryDuration').html(_msgLast24Hours + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    // Last week
    else if (duration == 2) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth(), dtcurrent.getDate() - 7);
        $('#aGallaryDuration').html(_msgLastWeek + '&nbsp;&nbsp;<span class="caret"></span>');
    }

    // Last Month
    else if (duration == 3) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth() - 1, dtcurrent.getDate());
        $('#aGallaryDuration').html(_msgLastMonth + '&nbsp;&nbsp;<span class="caret"></span>');
    }

    // Last 3 Months
    else if (duration == 4) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth() - 3, dtcurrent.getDate());
        $('#aGallaryDuration').html(_msgLast3Month + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    // Custom
    else if (duration == 5) {

        dtcurrent = null;
        if ($("#dpGallaryFrom").val() != "" && $("#dpGallaryTo").val() != "") {
            $('#aGallaryDuration').html(_msgCustom + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        else {
            if ($("#dpGallaryFrom").val() == "") {
                $("#dpGallaryFrom").addClass("warningInput");
            }
            if ($("#dpGallaryTo").val() == "") {
                $("#dpGallaryTo").addClass("warningInput");
            }
            ShowNotification(_msgSelectDate);
        }
    }

    $("#dpGallaryFrom").datepicker("setDate", fDate);
    $("#dpGallaryTo").datepicker("setDate", dtcurrent);

    if ($("#dpGallaryFrom").val() != "" && $("#dpGallaryTo").val() != "") {

        if (_FromDate != $("#dpGallaryFrom").val() || _ToDate != $("#dpGallaryTo").val()) {
            _FromDate = $("#dpGallaryFrom").val();
            _ToDate = $("#dpGallaryTo").val();
            RefreshGallaryResults();
        }
    }
}

/*---------------------------------------------- Clip Player Scripts ---------------------------------------------------------------------*/

function LoadGalleryClipPlayer(clipID) {
    var jsonPostData = { ClipID: clipID }

    $.ajax({

        type: "post",
        dataType: "json",
        url: _urlGalleryLoadClipPlayer,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPostData),
        success: OnLoadGalleryClipPlayerComplete,
        error: OnGalleryClipLoadFail
    });

    if(IsShowTimeSync)
    {
         /* load chart for player */
        $.ajax({

            type: "post",
            dataType: "json",
            url: _urlGalleryGetChart,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(jsonPostData),
            success: OnGetGalleryChartComplete,
            error: OnGetGalleryChartFail
        });
    }
}


function OnGetGalleryChartComplete(result) {
    if (result.isSuccess) {
        if (result.isTimeSync && result.lineChartJson.length > 0) {
            $('.video-chart').closest('li').show();
            $('.chart-tabs').html('');
            $('.chart-tab-content').html('');
            $.each(result.lineChartJson, function (index, obj) {
                $('.chart-tabs').append('<div class="chartTabHeader" id="video-parent-tab-' + index + '"><div class="padding5" id="video-tab-' + index + '" onclick="changeGalleryChartTab(' + index + ');">' + obj.Type + '</div></div>');
                $('.chart-tab-content').append('<div class="float-left" id="video-chart-' + index + '" style="width:900px;"></div>');
                RenderGalleryHighCharts(obj.Data, 'video-chart-' + index);
            });

            changeGalleryChartTab(0);
            $(".chart-tab-content").on("mouseout", function () {
                _IsManualHover = false;
            });

        }
    }
    else {
        ShowNotification(_msgErrorOccured);
    }
}


function OnGetGalleryChartFail(result) {
    ShowNotification(_msgErrorOccured);
}


function OnLoadGalleryClipPlayerComplete(result) {

    CheckForGallarySessionExpired(result);

    if (result != null && result.isSuccess) 
    {
        var clipPlayer = '<div id="diviframe" runat="server" class="modal fade hide resizable modalPopupDiv">'
                        + '<div class="closemodalpopup"><img id="img2" src="../Images/close-icon.png" class="popup-top-close" onclick="ClosePlayer(\'diviframe\');" /></div>'
                        + '<div id="divIFrameMain" class="modalPopupMediaDiv"><div style="position:relative;float:left;"><div id="divCapMain" class="CaptionMain">'
                    + '<div id="divCaption"></div></div>'
                    + '<div id="divShowCaption" class="divShowCaption" onclick="RegisterGalleryCCCallback();">'
                    + '<img src="../../../Images/right_arrow_cc.gif" id="imgCCDirection" alt="Show CC" /></div>';
                if(IsShowTimeSync)
                {
                    clipPlayer += 
                       '<div id= "divShowHideGalleryTimeSync" onclick="ShowHideGalleryTimeSync();" style="position: absolute; right: 0px; top: 325px;cursor:pointer;" title="Show Time Sync">'
                    +       '<img src="../images/show.png" id="imgSync" alt="Show Time Sync">'
                    + '</div>';
                }
       clipPlayer += '</div><div class="modalPopupPlayer" id="divClipPlayer"></div>'
                    + '<div id="time" class="clear" style="margin-left: 38.5%;">'
                    + '</div>'
                    + '</div>';
                if(IsShowTimeSync)
                {
                    clipPlayer += '<div class="video-chart-row" style="display:none;">'
                    + '<div class="margintop10">'
                    + '<div class="clear">'
                    + '<div class="chart-tabs" >'
                    + '</div>'
                    + '</div>'
                    + '<div class="clear chart-tab-content">'
                    + '</div>'
                    + '</div>'
                    + '</div>';
               }
       clipPlayer += '</div>';

        $(document.body).append(clipPlayer);
        $('#divClipPlayer').html(result.clipHTML);
        $('#divCaption').html(result.closedCaption);
        RegisterGalleryCCCallback();
        $('#diviframe').modal({
            backdrop: 'static',
            keyboard: true,
            dynamic: true
        });
    }
    else
    {
         ShowNotification(_msgErrorWhileLoadingPlayer);
    }
}

function OnGalleryClipLoadFail(a,b,c)
{
    ShowNotification(_msgSomeErrorProcessing);
}

function RegisterGalleryCCCallback() {

        if ($('#divCapMain').is(':visible')) {            
            
            if($('.video-chart-row').length == 0 || $('.video-chart-row').is(':visible') == false){
                $('#divCapMain').hide(500);

                $('#diviframe').animate({
                    width: '585px',                
                }, 1000, function () {
                });


                $('#divIFrameMain').animate({
                    width: '565px'

                }, 1000, function () {
                    $('#imgCCDirection').attr('src', '../../images/left_arrow_cc.gif');
                    var divmarleft = parseInt(($(window).width() / 2) - (600 / 2)).toString() + 'px';
                    $('#time').attr('style', 'margin-left:3.7%;')

                    $('#diviframe').animate({
                        left: divmarleft,
                        position: "fixed" 
                    });




                });
            }


        }
        else {

            $('#diviframe').animate({
                width: '900px'
            }, 1000, function () {

            });
            
            $('#divIFrameMain').animate({
                width: '885px'

            }, 1000, function () {
                $('#imgCCDirection').attr('src', '../../images/right_arrow_cc.gif');
                $('#time').attr('style', 'margin-left:38.5%;')
                var divmarleft = parseInt(($(window).width() / 2) - (920 / 2)).toString() + 'px';
                $('#diviframe').animate({
                    left: divmarleft,
                     position : "fixed" 
                });
                $('#divCapMain').show();
            });

        }

    }

function ClosePlayer(divID)
{
    $('#' + divID).css({ "display": "none" });
    $('#' + divID).modal('hide');
    $('#' + divID).remove();
}

function SelectGalleryPageSize(pageSize) {
    if (_PageSize != pageSize) {
        _PageSize = pageSize;
        $('#aGallaryPageSize').html(_PageSize + ' Items Per Page&nbsp;&nbsp;<span class="caret"></span>');
         $('#divGallaryPopover').remove();
        RefreshGallaryResults();
    }
}

function showGallaryPopupOver() {
    if ($('#divGallaryPopover').length <= 0) {
        var drphtml = $("#divGallaryPageSizeDropDown").html();
        $('#aGallaryPageSize').popover({
            trigger: 'manual',
            html: true,
            title: '',
            placement: 'top',
            template: '<div id="divGallaryPopover" class="popover" style="width:125px;"><div class="popover-inner"><div class="popover-content"><p></p></div></div></div>',
            content: drphtml
        }); 
        $('#aGallaryPageSize').popover('show');
    }
    else{
        $('#divGallaryPopover').remove();
    }
}

//function ShowChild(parentid) {

//    if ($('#divChildMedia_' + parentid).is(':visible')) {
//        $('#divChildMedia_' + parentid).hide();
//        $('#expand_' + parentid).attr('src', '../images/expand.png');
//        
//    }
//    else {
//        $('#divChildMedia_' + parentid).show();
//        $('#expand_' + parentid).attr('src', '../images/collapse.png');
//    }
//}

function SetGallaryMediaClickEvent() {

    $("#divGallaryResultScrollContent .media").click(function (e) {
        if ($(e.target).closest("a").length <= 0 && e.target.type != "checkbox") {
            e.stopPropagation();
            if ($(e.target).closest('.media').find('input').is(':checked')) {
                $(e.target).closest('.media').find('input').removeAttr('checked');
                $(this).css("background", "");
            }
            else {
//                $(".media input[type=checkbox]").each( function(){ 
//                        $(this).prop("checked", false);
//                        $(this).closest('.media').css('background', '');
//                });
                $(e.target).closest('.media').find('input').prop('checked', true);
                $(this).css("background", "#F4F4F4");
            }
        }
    });
}

function RenderGalleryHighCharts(jsonLineChartData, chartID) {

    var JsonLineChart = JSON.parse(jsonLineChartData);
    JsonLineChart.plotOptions.series.point.events.click = GalleryLineChartClick;
    JsonLineChart.xAxis.labels.formatter = FormatTime
    //JsonLineChart.tooltip.positioner = TooltipSetY
    JsonLineChart.tooltip.formatter = GallerytooltipFormat
    JsonLineChart.plotOptions.series.events.show = HandleGallerySeriesShowHide;
    JsonLineChart.plotOptions.series.events.hide = HandleGallerySeriesShowHide;
    JsonLineChart.plotOptions.series.point.events.mouseOver = GalleryChartHoverManage;
    JsonLineChart.plotOptions.series.point.events.mouseOut = GalleryChartHoverOutManage;

    //    JsonLineChart.chart.events = new Object();
    //    JsonLineChart.chart.events.load = new Object(function () {
    //        var chartContiner = this;
    //        setTimeout(function () {
    //            // solution: 
    //            chartContiner.setSize($('#' + chartID).width(), $('#' + chartID).height());
    //        }, 1000);
    //    });



    //    JsonLineChart.legend.x = -400;
    //    JsonLineChart.xAxis.tickInterval = parseInt(Math.floor(JsonLineChart.xAxis.categories.length / 12)),
    //    JsonLineChart.chart.width = null;


    $('#' + chartID).highcharts(JsonLineChart);
}


function HandleGallerySeriesShowHide() {

    if (!_IsManualHover) {
        var chart = this.chart;
        xIndex = chart.axes[0].categories.indexOf(_currentTimeInt.toString());
        if (chart.series[0].visible || chart.series[1].visible) {
            if (chart.series[0].visible && chart.series[1].visible) {
                chart.tooltip.refresh([chart.series[0].data[xIndex], chart.series[1].data[xIndex]]);
            }
            else if (chart.series[0].visible) {
                chart.tooltip.refresh([chart.series[0].data[xIndex]]);
            }
            else {
                chart.tooltip.refresh([chart.series[1].data[xIndex]]);
            }
        }
        else {
            chart.series[0].data[xIndex].setState('');
            chart.series[1].data[xIndex].setState('');
            //chart.tooltip.refresh([chart.series[0].data[xIndex], chart.series[1].data[xIndex]]);
            chart.tooltip.hide();
        }
    }

}

function GallerytooltipFormat() {
    var s = [];

    var totalSeconds = this.x;
    var minutes = Math.floor(totalSeconds / 60);
    var seconds = totalSeconds - minutes * 60;
    minutes = minutes < 10 ? '0' + minutes : minutes;
    seconds = seconds < 10 ? '0' + seconds : seconds;


    str = minutes + ':' + seconds;
    $.each(this.points, function (i, point) {
        var seriesName = this.series.tooltipOptions.valuePrefix;

        /*if (point.series.index == 0) {
        seriesName = 'Kantar (second by second)';
        }
        else {
        seriesName = 'Nielsen (minute by minute)';
        }*/

        str += '<br/><span style="color:' + point.series.color + ';font-weight:bold;">' + seriesName + '</span><span style="color:' + point.series.color + ';"> = ' +
                    numberWithCommas(point.y) + '</span>';
    });
    return str;
}

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function FormatTime() {
    var minutes = Math.floor(this.value / 60);
    var seconds = this.value - minutes * 60;

    minutes = minutes < 10 ? '0' + minutes : minutes;
    seconds = seconds < 10 ? '0' + seconds : seconds;
    return minutes + ':' + seconds;
}

function GalleryChartHoverOutManage() {
    _IsManualHover = false;
    console.log("chart hover out");
}

function GalleryChartHoverManage() {
    _IsManualHover = true;
    console.log("chart hover");
    //$('#divKantorChart').scrollLeft(this.plotX);
}


function GalleryLineChartClick() {
    setSeekPoint(this.category);
}



function changeGalleryChartTab(tabNumber) {

    // hide all tabs
    $("div[id ^= 'video-chart-']").css({ opacity: 0 })
    $("div[id ^= 'video-chart-']").css({ height: "0" });

    $("div[id ^= 'video-tab-']").removeClass('playerChartTabActive');
    //$("div[id ^= 'video-parent-tab-']").removeClass('playerChartTabActiveParent');


    // show current tab
    $('#video-tab-' + tabNumber).addClass('playerChartTabActive');
    //$('#video-parent-tab-' + tabNumber).addClass('playerChartTabActiveParent');
    $('#video-chart-' + tabNumber).css({ height: "auto", opacity: 1 })
}

function Gallery(id,type,col,row,size_x,size_y,MetaData) {
  this.id = id;
  this.type = type,
  this.col = col;
  this.row=row;
  this.size_x=size_x;
  this.size_y=size_y;
  this.MetaData=MetaData;
}

function SaveGallary()
{

    if (ValidateGallary()) {

    var isValidGallery = false;
    var jsonObj = [];
    var jsonObjList = [];
    var lstObj = $(".gridster > ul > li");
    var data = $(".gridster > ul").gridster().data('gridster').serialize();
    $.each(data,function(i,obj) {
           if(lstObj[i].id.length > 0 && lstObj[i].id != "HorizontalTextBlock" && lstObj[i].id != "VerticalTextBlock")
           {
               var g=new Gallery(lstObj[i].id,"TVBlock",obj["col"],obj["row"],obj["size_x"],obj["size_y"],"");
               jsonObj.push(g);
               isValidGallery = true;
           }
    });
    var HBCount = 0;
    var VBCount = 0;
    $.each($(".gridster > ul > li"),function(i,obj) {
      if($(this).attr("name") == "HorizontalTextBlock" || $(this).attr("name") == "VerticalTextBlock")
      {
           var strBlock;
           if($(this).attr("name") == "HorizontalTextBlock")
           {
               strBlock = CKEDITOR.instances['hb_' + HBCount].getData();
               HBCount = HBCount + 1;
           }
           else
           {
               strBlock = CKEDITOR.instances['vb_' + VBCount].getData();
               VBCount = VBCount + 1;
           }
           var g=new Gallery(0,$(this).attr("name"),$(this).attr("data-col"),$(this).attr("data-row"),$(this).attr("data-sizex"),$(this).attr("data-sizey"),strBlock);
           
           jsonObj.push(g);
           isValidGallery = true;
      }
    });
    if(isValidGallery)
    {
        var galleryType = $("#ddlGalleryType").val();
        if($("#hdnGalleryID").val() > 0)
        {
            var jsonPostData = {
                p_ID: $("#hdnGalleryID").val(),
                p_Name: $("#txtGallaryName").val(),
                p_Title: CKEDITOR.instances.txtGallaryTitle.getData(),
                p_Description: $("#txtGallaryDescription").val(),
                p_GalleryType: galleryType,
                p_Json: JSON.stringify(jsonObj)
            }

            $.ajax({
                url: _urlGalleryUpdate,
                contentType: "application/json; charset=utf-8",
                type: "post",
                dataType: "json",
                data: JSON.stringify(jsonPostData),
                success: function (result) {
                    if (result != null && result.isSuccess) {
                        CancelGallaryPopup();
                        CollapseExpandLeftSection(4);
                        ShowNotification(_msgGalleryUpdated);
                    }
                    else if(result != null && !result.isSuccess && result.isExist){
                        ShowNotification(_msgGalleryAlreadyExists);
                    }
                    else{
                        ShowNotification(_msgErrorOccured);
                    }
                },
                error: function (a, b, c) {
                    ShowNotification(_msgErrorOccured);
                }
            });
        }
        else
        {
            var jsonPostData = {
                p_Name: $("#txtGallaryName").val(),
                p_Title: CKEDITOR.instances.txtGallaryTitle.getData(),
                p_Description: $("#txtGallaryDescription").val(),
                p_GalleryType: galleryType,
                p_Json: JSON.stringify(jsonObj)
            }

            $.ajax({
                url: _urlGalleryInsert,
                contentType: "application/json; charset=utf-8",
                type: "post",
                dataType: "json",
                data: JSON.stringify(jsonPostData),
                success: function (result) {
                    if (result != null && result.isSuccess) {
                        ShowNotification(_msgGalleryAdded);
                        $("#txtGallaryName").val('');
                        $("#txtGallaryTitle").val('');
                        $("#txtGallaryDescription").val('');
                        $(".gridster > ul").html('');
                        CancelGallaryPopup();
                        CollapseExpandLeftSection(4);
                    }
                    else if(result != null && !result.isSuccess && result.isExist){
                        ShowNotification(_msgGalleryAlreadyExists);
                    }
                    else{
                        ShowNotification(_msgErrorOccured);
                    }
                },
                error: function (a, b, c) {
                    ShowNotification(_msgErrorOccured);
                }
            });
        }
    }
    else
    {
        ShowNotification(_msgGalleryAtleastOneRecordSelect);
    }
    }
}

function ValidateGallary()
{
    var flag = true;

    if ($("#txtGallaryName").val().trim() == "") {
        flag = false;
        $("#txtGallaryName").css("border-color", "#FF0000");
    }
    else if($("#txtGallaryName").val().trim().length > 50)
    {
        flag = false;
        ShowNotification(_msgGalleryTitleMax50ChartLong);
    }
    else
    {
        $("#txtGallaryName").css("border-color", "#cccccc");
    }

//    if ($("#txtGallaryTitle").val().trim() == "") {
//        flag = false;
//        $("#txtGallaryTitle").css("border-color", "#FF0000");
//    }
//    else
//    {
//        $("#txtGallaryTitle").css("border-color", "#cccccc");
//    }

//    if (CKEDITOR.instances.txtGallaryTitle.getData() == "") {
//        flag = false;
//        $("#txtGallaryTitle").css("border-color", "#FF0000");
//    }
//    else
//    {
//        $("#txtGallaryTitle").css("border-color", "#cccccc");
//    }

//    if ($("#txtGallaryDescription").val().trim() == "") {
//        flag = false;
//        $("#txtGallaryDescription").css("border-color", "#FF0000");
//    }
//    else if($("#txtGallaryDescription").val().trim().length > 255)
//    {
//        flag = false;
//        ShowNotification(_msgGalleryDescriptionMax255ChartLong);
//    }
//    else
//    {
//        $("#txtGallaryDescription").css("border-color", "#cccccc");
//    }

    return flag;
}

function ShowHideGalleryTimeSync()
{
    if($(".video-chart-row").is(':visible'))
    {
        $("#divShowHideGalleryTimeSync").attr("title","Hide Time Snyc");
        $("#imgSync").attr("src","../images/show.png");
        $(".video-chart-row").hide('slow');
    }
    else{
        $("#divShowHideGalleryTimeSync").attr("title","Show Time Sync");
        $("#imgSync").attr("src","../images/hiden.png");
        $(".video-chart-row").show('slow');
    }
}