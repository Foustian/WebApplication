var _SearchTerm = "";
var _FromDate = null;
var _ToDate = null;
var _SubMediaType = "";
var _SubMediaTypeDescription = "";
var _CategoryGUID = [];
var _CategoryNameList = [];
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
var _CurrentCategoryFilter  = new Array();
var _ParentCount = 0;
var _IsSelectAll = false;
var _IsSelectAllParent = true;
var _MediaIDs = [];
var _PieChartTotalHits = 0; // Used by IQMediaCommon.js for Dashboard popup
var _EmailCount = 0;
var _VideoMetaData = "";
var _nielsenData = null;
var _clientGuid = null;
var _PlayerFromEmail = null;

function CheckForSessionExpired(result)
{
    if(result != null)
    {
        if(result.isSuccess == false && result.isAuthorized == false)
        {
            RedirectToUrl(result.redirectURL);
        }
    }
}


$(document).ready(function () {

    $('#ulMainMenu li').removeAttr("class");
    $('#liMenuLibrary').attr("class", "active");
    
    
     if (screen.height >= 768) {    
        $("#divResultScrollContent").css("height",documentHeight - 270);
        $("#divReportScrollContent").css("height",documentHeight - 240);
        $("#divGalleryResultsScrollContent").css("height",documentHeight - 270);
    }
    UpdateResultScrollbarContent();
    UpdateReportScrollbarContent();
    UpdateGalleryResultsScrollContent();
    
    

    $('#mCSB_1').css({ 'max-height': '' });
    $('#mCSB_4').css({ 'max-height': '' });
    
    $('#divMessage').html('');
    $('#dpFrom').val('');
    $('#dpTo').val('');

    $("#txtKeyword").keypress(function (e) {
        if (e.keyCode == 13) {
            SetKeyword();
        }
    });

    $("#imgKeyword").click(function (e) {
        SetKeyword();
    });

    $("#txtKeyword").blur(function () {
        SetKeyword();
    });

//    $("#divSavedIQReportsScrollContent").mCustomScrollbar({
//                            advanced: {
//                                updateOnContentResize: true,
//                                autoScrollOnFocus: false
//                            }
//                        });



    $("body").click(function(e) {
        if ((e.target.id != "aPageSize" && e.target.id != "divPopover") && $(e.target).parents("#divPopover").size() <= 0) {
            $('#divPopover').remove();
        }

        if(!(e.target.id == "btnSaveReportAs" || $(e.target).parents("#divReportPopover").size() > 0))
        {
            $('#divReportPopover').remove();
            $('#txtSaveReportTitle').val("");    
        }
       
        if (e.target.id == "liCategoryFilter" || $(e.target).parents("#liCategoryFilter").size() > 0)
        {
            if ($('#ulCategory').is(':visible')) {            
                $('#ulCategory').hide();
            }
            else {
                $('#ulCategory').show();
            }
        }
        else if ((e.target.id !== "liCategoryFilter" && e.target.id !== "ulCategory"   && $(e.target).parents("#ulCategory").size() <= 0) || e.target.id == "btnSearchCategory") { 
            $('#ulCategory').hide();
            if(e.target.id != "btnSearchCategory")
            {
                _CategoryGUID = [];
                _CategoryNameList = [];
                var categoriesHTML  ="";
                if(_SelectionType == $('#rdAnd').val())
                {
                    $('#rdAnd').prop("checked", true);
                }
                else
                {
                    $('#rdOr').prop("checked", true);
                }
                
                $.each(_CurrentCategoryFilter, function (eventID, eventData) {
                    if(_OldCategoryGUID.length > 0 && $.inArray(eventData.CategoryGUID, _OldCategoryGUID) !== -1)
                    {
                        _CategoryGUID.push(eventData.CategoryGUID);
                        _CategoryNameList.push(eventData.CategoryName);
                    }
                    var liStyle= "";
                    if ($.inArray(eventData.CategoryGUID, _CategoryGUID) !== -1) 
                    {
                        liStyle ="style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    categoriesHTML = categoriesHTML + '<li role=\"presentation\"><a '+liStyle+'  href=\"javascript:void(0)\" onclick="SetCategory(this,\'' + eventData.CategoryGUID + '\',\'' + eventData.CategoryName.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + '\');" tabindex=\"-1\" role=\"menuitem\">';
                    categoriesHTML += EscapeHTML(eventData.CategoryName) + ' (' + eventData.RecordCountFormatted + ') </a></li>';
                });
                if (categoriesHTML == '') {
                    $('#ulCategoryList').html('<li role="presentation"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
                }
                else {
                    $("#ulCategoryList").html(categoriesHTML);
                }
            }
        } 
    });
    
    $("#divCalender").datepicker({
        beforeShowDay: enableAllTheseDays,
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            $('#dpFrom').val(dateText);
            $('#dpTo').val(dateText);
            SetDateVariable();
        }

    }); 

    $('.ndate').click(function () {
        $("#divCalender").datepicker("refresh");
    });


    $("#dpFrom").datepicker({
        //beforeShowDay: enableAllTheseDays,
        showOn: "button",
        buttonImage: "../images/calender.png",
        buttonImageOnly: true,
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            $('#dpFrom').val(dateText);
            SetDateVariable();
        },
        onClose: function (dateText) {
            //$('#dpFrom').focus();
            //SetDateVariable();
        }
    });

    $("#dpTo").datepicker({
        showOn: "button",
        buttonImage: "../images/calender.png",
        buttonImageOnly: true,
        changeMonth: true,
        changeYear: true,
        //beforeShowDay: enableAllTheseDays,
        onSelect: function (dateText, inst) {
            $('#dpTo').val(dateText);
            SetDateVariable();
        },
        onClose: function (dateText) {
            //$('#dpTo').focus();
            //SetDateVariable();
        }
    });

    $("#chkInputAll").click(function()
    {
        var c = this.checked;
        var backcolor = c == true ? '#F4F4F4' : '';
        if (_IsSelectAll) {
            //select all with dupes
            $("#divResultScrollContent .media input[type=checkbox]").each( function()
            { 
                $(this).prop("checked", c);
                $(this).closest('.media').css('background', backcolor);
            });
        }
        else{
            //select all without dupes
            $("#divResultScrollContent .media input[type=checkbox][id^='chkdivResults_']").each( function()
            { 
                $(this).prop("checked", c);
                $(this).closest('.media').css('background', backcolor);
            });
        }
    });

     // if all checkbox are selected, check the selectall checkbox and viceversa

    $(document).on("click",".media input[type=checkbox]",function(event){
        if($(this).is(':checked')){
            $(this).closest('.media').css('background', '#F4F4F4');
        }
        else{
            $(this).closest('.media').css('background', '');
        }

        // Update all child items
        if (_IsSelectAll) {
            $(this).closest('.media').find('input').prop("checked", $(this).is(':checked'));
        }
        
        if($("#h5Library").hasClass("libraryheader-active"))
        {
            if (_IsSelectAll) {
            //check all with dupes
                if($("#divResultScrollContent .media input[type=checkbox]").length == $("#divResultScrollContent  .media input[type=checkbox]:checked").length) {
                    $("#chkInputAll").prop("checked", true);
                } 
                else {
                    $("#chkInputAll").prop("checked", false);
                }
            }
            else{
            //check all without dupes
                if($("#divResultScrollContent  .media input[type=checkbox][id^='chkdivResults_']").length == $("#divResultScrollContent  .media input[type=checkbox][id^='chkdivResults_']:checked").length) {
                    $("#chkInputAll").prop("checked", true);
                } else {
                    $("#chkInputAll").prop("checked", false);
                }
            }
        }
    });

    $("#chkShowHideAll").click(function() {
        var c = this.checked;
        $("input[name=chkShowHide]").each(function()
        {
            $(this).prop("checked", c);
        });
    });

    // If all Show/Hide checkboxes are selected, check the Select All checkbox and vice versa
    $(document).on("click","input[name=chkShowHide]", function(event) {
        if($("input[name=chkShowHide]").length == $("input[name=chkShowHide]:checked").length) {
            $("#chkShowHideAll").prop("checked", true);
        } else {
            $("#chkShowHideAll").prop("checked", false);
        }
    });
    
    $("#chkDashboardItemsAll").click(function() {
        var c = this.checked;
        $("input[name=chkShowHideDashboard]").each(function()
        {
            $(this).prop("checked", c);
        });
    });

    // If all report cover page checkboxes are selected, check the Select All checkbox and vice versa
    $(document).on("click","input[name=chkShowHideDashboard]", function(event) {
        if($("input[name=chkShowHideDashboard]").length == $("input[name=chkShowHideDashboard]:checked").length) {
            $("#chkDashboardItemsAll").prop("checked", true);
        } else {
            $("#chkDashboardItemsAll").prop("checked", false);
        }
    });

    // below function is called from IQMediaCommon.js
    // to set Height of content of the modal popup
    SetModalBodyScrollBarForPopUp();

    SetImageSrc();
    SetMediaClickEvent();
});

$(window).resize(function () {
    if (screen.height >= 768) {    
        $("#divResultScrollContent").css("height",documentHeight - 270);
        $("#divReportScrollContent").css("height",documentHeight - 240);
        $("#divGalleryResultsScrollContent").css("height",documentHeight - 270);
    }
});

function SetCheckboxSelectionType(type) {
    
    
    
    
    if (type == 0) {
        _IsSelectAllParent = true;
        _IsSelectAll = false;
        $('#aSelectAll').html(_msgSelectAll + '&nbsp;&nbsp;<span class="caret"></span>');
    }

    if (type == 1) {
        _IsSelectAll = true;
        _IsSelectAllParent = false;
        $('#aSelectAll').html(_msgSelectAllWithDupes +'&nbsp;&nbsp;<span class="caret"></span>');
        
    }

    if($("#chkInputAll").is(":checked")){
        $(".media input[type=checkbox]").each( function(){ 
            $(this).prop("checked", false);
        });
        $("#chkInputAll").prop("checked", false);
        $("#chkInputAll").click();
    }    
}


function UpdateResultScrollbarContent()
{
 if (screen.height >= 768) {
     $("#divResultScrollContent").mCustomScrollbar({
        advanced: {
            updateOnContentResize: true,
            autoScrollOnFocus: false
        },
        scrollInertia: 60
    });
    }
}

function UpdateGalleryResultsScrollContent()
{
 if (screen.height >= 768) {
     $("#divGalleryResultsScrollContent").mCustomScrollbar({
        advanced: {
            updateOnContentResize: true,
            autoScrollOnFocus: false
        },
        scrollInertia: 60
    });
    }
}

function UpdateReportScrollbarContent()
{
if (screen.height >= 768) {
      $("#divReportScrollContent").mCustomScrollbar({
        advanced: {
            updateOnContentResize: true,
            autoScrollOnFocus: false
        },
        scrollInertia: 60
    });
    }
}

function CollapseExpandLeftSection(sectionid)
{
    if (sectionid==3) {
    $.getScript('/Scripts/VideoPlayer.js?v=1.20');
}
else {
    $.getScript('/Scripts/Player/PlayerClip.js?v=1.3');
}

    if(sectionid == 1)
    {
        $("#divQuickLibrarySection").show("2000");
        $("#divResultContent").show("2000");

        $("#divReportSection").hide("2000");
        $("#divReportContent").hide("2000");

        $("#divCloudSection").hide("2000");
        $("#divCloudContent").hide("2000");

        $("#divGallarySection").hide("2000");
        $("#divGallaryContent").hide("2000");

        $("#divMCMediaSection").hide("2000");
        $("#divMCMediaContent").hide("2000");

        $("#h5Library").removeAttr("class");
        $("#h5Library").addClass("libraryheader-active");

        $("#h5Report").removeAttr("class");
        $("#h5Report").addClass("reportheader-inactive");

        $("#h5Cloud").removeAttr("class");
        $("#h5Cloud").addClass("cloudheader-inactive");

        $("#h5Gallary").removeAttr("class");
        $("#h5Gallary").addClass("gallary-inactive");

        $("#h5MCMedia").removeAttr("class");
        $("#h5MCMedia").addClass("mcmedia-inactive");

         if (screen.height >= 768) {
        setTimeout(function () { $("#divResultScrollContent").mCustomScrollbar("scrollTo", "top"); }, 200);
        }
    }
    else if(sectionid == 2)
    {
        $("#divReportSection").show("2000");
        $("#divReportContent").show("2000");
        
        $("#divQuickLibrarySection").hide("2000");
        $("#divResultContent").hide("2000");

        $("#divCloudSection").hide("2000");
        $("#divCloudContent").hide("2000");

        $("#divGallarySection").hide("2000");
        $("#divGallaryContent").hide("2000");

        $("#divMCMediaSection").hide("2000");
        $("#divMCMediaContent").hide("2000");
        
        $("#h5Report").removeAttr("class");
        $("#h5Report").addClass("reportheader-active");
        
        $("#h5Library").removeAttr("class");
        $("#h5Library").addClass("libraryheader-inactive");

        $("#h5Cloud").removeAttr("class");
        $("#h5Cloud").addClass("cloudheader-inactive");

        $("#h5Gallary").removeAttr("class");
        $("#h5Gallary").addClass("gallary-inactive");

        $("#h5MCMedia").removeAttr("class");
        $("#h5MCMedia").addClass("mcmedia-inactive");

        if (screen.height >= 768) {
        setTimeout(function () { $("#divReportScrollContent").mCustomScrollbar("scrollTo", "top"); }, 200);
        }

        setTimeout(function () { 
            $("#divSavedIQReports").width($("#divSavedIQReports").next().width());
        }, 2000);
    }
    else if(sectionid == 3)
    {
        $("#divCloudSection").show("2000");
        $("#divCloudContent").show("2000");

        $("#divReportSection").hide("2000");
        $("#divReportContent").hide("2000");
        
        $("#divQuickLibrarySection").hide("2000");
        $("#divResultContent").hide("2000");

        $("#divGallarySection").hide("2000");
        $("#divGallaryContent").hide("2000");

        $("#divMCMediaSection").hide("2000");
        $("#divMCMediaContent").hide("2000");

        $("#h5Cloud").removeAttr("class");
        $("#h5Cloud").addClass("cloudheader-active");

        $("#h5Report").removeAttr("class");
        $("#h5Report").addClass("reportheader-inactive");
        
        $("#h5Library").removeAttr("class");
        $("#h5Library").addClass("libraryheader-inactive");

        $("#h5Gallary").removeAttr("class");
        $("#h5Gallary").addClass("gallary-inactive");

        $("#h5MCMedia").removeAttr("class");
        $("#h5MCMedia").addClass("mcmedia-inactive");

        if (screen.height >= 768) {
        setTimeout(function () { $("#divReportScrollContent").mCustomScrollbar("scrollTo", "top"); }, 200);
        }

        // This variable and fuctionality is defined in LibraryUGCContent.js file
        if (!HasUGCResultsLoadedFirstTime) 
        {
            LoadIQUGCArchiveResults(false);
            HasUGCResultsLoadedFirstTime = true;
        }
    }
    else if(sectionid == 4)
    {
        $("#divGallarySection").show("2000");
        $("#divGallaryContent").show("2000");

        $("#divCloudSection").hide("2000");
        $("#divCloudContent").hide("2000");

        $("#divReportSection").hide("2000");
        $("#divReportContent").hide("2000");
        
        $("#divQuickLibrarySection").hide("2000");
        $("#divResultContent").hide("2000");

        $("#divMCMediaSection").hide("2000");
        $("#divMCMediaContent").hide("2000");

        $("#h5Gallary").removeAttr("class");
        $("#h5Gallary").addClass("gallary-active");

        $("#h5Cloud").removeAttr("class");
        $("#h5Cloud").addClass("cloudheader-inactive");

        $("#h5Report").removeAttr("class");
        $("#h5Report").addClass("reportheader-inactive");

        $("#h5Library").removeAttr("class");
        $("#h5Library").addClass("libraryheader-inactive");

        $("#h5MCMedia").removeAttr("class");
        $("#h5MCMedia").addClass("mcmedia-inactive");
        
        if (screen.height >= 768) {
        setTimeout(function () { $("#divGalleryResultsScrollContent").mCustomScrollbar("scrollTo", "top"); }, 200);
        }

        LoadGalleryResults();

//        // This variable and fuctionality is defined in LibraryUGCContent.js file
//        if (!HasUGCResultsLoadedFirstTime) 
//        {
//            LoadIQUGCArchiveResults(false);
//            HasUGCResultsLoadedFirstTime = true;
//        }
    }
    else if(sectionid == 5)
    {        
        $("#divMCMediaSection").show("2000");
        $("#divMCMediaContent").show("2000");   

        $("#divQuickLibrarySection").hide("2000");
        $("#divResultContent").hide("2000");

        $("#divReportSection").hide("2000");
        $("#divReportContent").hide("2000"); 

        $("#divCloudSection").hide("2000");
        $("#divCloudContent").hide("2000"); 

        $("#divGallarySection").hide("2000");
        $("#divGallaryContent").hide("2000");  

        $("#h5MCMedia").removeAttr("class");
        $("#h5MCMedia").addClass("mcmedia-active");

        $("#h5Library").removeAttr("class");
        $("#h5Library").addClass("libraryheader-inactive"); 

        $("#h5Report").removeAttr("class");
        $("#h5Report").addClass("reportheader-inactive");

        $("#h5Cloud").removeAttr("class");
        $("#h5Cloud").addClass("cloudheader-inactive");

        $("#h5Gallary").removeAttr("class");
        $("#h5Gallary").addClass("gallary-inactive");

        if (screen.height >= 768) {
        setTimeout(function () { $("#divMCMediaScrollContent").mCustomScrollbar("scrollTo", "top"); }, 200);
        }
        
        // This variable and fuctionality is defined in LibraryMCMedia.js file
        if (!HasMCMediaResultsLoadedFirstTime) 
        {
            GetMCMediaReportGUID(false);
            LoadResults_MCMedia(false);
            HasMCMediaResultsLoadedFirstTime = true;
        }
    }
     
}

function enableAllTheseDays(date) {
    date = $.datepicker.formatDate("mm/dd/yy", date);
    return [$.inArray(date, disabledDays) !== -1];
}

function SetKeyword() {
    if ($("#txtKeyword").val() != "" && _SearchTerm != $("#txtKeyword").val()) 
    {
        _SearchTerm = $("#txtKeyword").val();
        RefreshResults();
    }
}

function SetDateVariable() {

    if ($("#dpFrom").val() && $("#dpTo").val()) {
        if (_FromDate != $("#dpFrom").val() || _ToDate != $("#dpTo").val()) 
        {
            _FromDate = $("#dpFrom").val();
            _ToDate = $("#dpTo").val();
            RefreshResults();
            $('#ulCalender').parent().removeClass('open');
            $('#aDuration').html(_msgCustom +'&nbsp;&nbsp;<span class="caret"></span>');
        }
    }
    else
        if ($("#dpFrom").val() != "" && $("#dpTo").val() == "") {
            $("#dpTo").addClass("warningInput");
        }
        else if ($("#dpTo").val() != "" && $("#dpFrom").val() == "") {
            $("#dpFrom").addClass("warningInput");
        }
}

function SetSubMediaType(p_SubMediaType,p_SubMediaTypeDescription) {

    if (_SubMediaType != p_SubMediaType) 
    {
        _SubMediaType = p_SubMediaType;
        _SubMediaTypeDescription = p_SubMediaTypeDescription;
        RefreshResults();
    }
}

function SetCustomer(p_Customer, p_CustomerName) {
    if (_Customer != p_Customer) {
        _Customer = p_Customer;
        _CustomerName = p_CustomerName;
        RefreshResults();
    }
}

function SetCategory(eleCategory,p_CategoryGUID,p_CategoryName)
{
    if ($.inArray(p_CategoryGUID, _CategoryGUID) == -1) 
    {
        _CategoryGUID.push(p_CategoryGUID);
        _CategoryNameList.push(p_CategoryName);
        $(eleCategory).css("background-color","#E9E9E9");
    }
    else
    {
        var catIndex = _CategoryGUID.indexOf(p_CategoryGUID);
        if (catIndex > -1) {
            _CategoryGUID.splice(catIndex, 1);
            _CategoryNameList.splice(catIndex, 1);
            $(eleCategory).css("background-color","#ffffff");
        }
    }

    if($('#rdAnd').is(":checked"))
    {
        $("#ulCategoryList li").each(function(){
            var _Category = $(this).find('a').attr("onclick").replace('SetCategory(this,','').replace(');','').split(',')[0].replace(/'/g,'');
            if ($.inArray(_Category, _CategoryGUID) == -1) 
            {
                $(this).find('a').addClass('blur');
            }
        });

        GetCategoryFilter(false);
    }
}


function GetCategoryFilter(isClear)
{
    
    var jsonPostData = {
           p_CustomerGuid: _Customer,
           p_FromDate: _FromDate,
           p_ToDate: _ToDate,
           p_SearchTerm: _SearchTerm,
           p_SubMediaType: _SubMediaType,
           p_CategoryGUID: _CategoryGUID
    }

    $.ajax({
            url: _urlLibraryFilterCategory,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: 'json',
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                if (result.isSuccess) {
                    var categoriesHTML ="";
                    $.each(result.categoryFilter, function (eventID, eventData) {
                        var liStyle= "";
                        if ($.inArray(eventData.CategoryGUID, _CategoryGUID) !== -1) 
                        {
                            liStyle ="style=\"background-color: rgb(233, 233, 233);\"";
                        }
                        categoriesHTML = categoriesHTML + '<li role=\"presentation\"><a '+liStyle+'  href=\"javascript:void(0)\" onclick="SetCategory(this,\'' + eventData.CategoryGUID + '\',\'' + eventData.CategoryName.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + '\');" tabindex=\"-1\" role=\"menuitem\">';
                        categoriesHTML += EscapeHTML(eventData.CategoryName) + ' (' + eventData.RecordCountFormatted + ') </a></li>';
                    });

                    $("#ulCategoryList").html(categoriesHTML);
                }
                else 
                {
                    ShowNotification(_msgSomeErrorProcessing);
                }
            },
            error: function (a, b, c) {
                ShowNotification(_msgSomeErrorProcessing,a);
            }
        });
}

function SearchCategory()
{
    var _CurrentSelectionType;
    var IsPost = false;
    if($('#rdAnd').is(":checked"))
    {
        _CurrentSelectionType = $('#rdAnd').val();
    }
    else
    {
        _CurrentSelectionType = $('#rdOr').val();
    }
    if($(_CategoryGUID).not(_OldCategoryGUID).length != 0 || $(_OldCategoryGUID).not(_CategoryGUID).length != 0)
    { 
        IsPost = true;
    }
    else if(_CategoryGUID.length > 1 && (_SelectionType != '' && _SelectionType != _CurrentSelectionType))
    {
        IsPost = true;
    }

    if(IsPost)
    {
        _OldCategoryGUID = _CategoryGUID.slice(0);
         _SelectionType = _CurrentSelectionType;
         _CategoryName ="";
        $.each(_CategoryNameList, function(index, val){
            if(_CategoryName == "")
            {
                _CategoryName = val;
            }
            else
            {   
                _CategoryName = _CategoryName + ' "' + _SelectionType + '" ' + val;
            }
        });
         RefreshResults();
    }
}

function SetSelectionType(IsClear)
{
    if(_CategoryGUID.length > 0 || _OldCategoryGUID.length > 0)
    {
        if($('#rdAnd').is(":checked"))
        {
            _CategoryGUID = [];
            _CategoryNameList = [];
            GetCategoryFilter();   
        }
        else
        {
            _CategoryGUID = [];
            _CategoryNameList = [];
            GetCategoryFilter();
        }
    }
}

function GetMoreResults() {

    var jsonPostData = {
        p_CustomerGuid: _Customer,
        p_FromDate: _FromDate,
        p_ToDate: _ToDate,
        p_SearchTerm: _SearchTerm,
        p_SubMediaType: _SubMediaType,
        p_CategoryGUID: _CategoryGUID,
        p_SelectionType : _SelectionType,
        p_IsAsc: _IsAsc,
        p_SortColumn: _SortColumn,
        p_PageSize : _PageSize
    }
    
    $.ajaxSetup({ cache: false });

    ShowHideProgressNearButton(true);


    $("#btnShowMoreResults").attr("disabled", "disabled");
    $("#btnShowMoreResults").attr("class", "disablebtn");

    $.ajax({
        url: _urlLibraryGetMoreResults,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: 'json',
        data: JSON.stringify(jsonPostData),
        success: function (result) {

            CheckForSessionExpired(result);

            if (result.isSuccess) 
            {
                $("#ulIQArchieveResults").append(result.HTML);
                ShowHideMoreResults(result);
                ShowNoofRecords(result);
                SetMediaClickEvent();

                if($("#chkInputAll").is(":checked"))
                {
                    if(_IsSelectAll)
                    {
                        $(".media input[type=checkbox]").each( function(){ 
                            $(this).prop("checked", true);
                        });
                    }
                    else{
                        $(".media input[type=checkbox][id^='chkdivResults_']").each( function(){ 
                            $(this).prop("checked", true);
                        });
                    }
                }
                SetImageSrc();
            }
            else {
                ShowNotification(_msgSomeErrorProcessing);
            }
            ShowHideProgressNearButton(false);
            $("#btnShowMoreResults").removeAttr("disabled");
            $("#btnShowMoreResults").attr("class", "loadmore");
        },
        error: function (a, b, c) {
            ShowHideProgressNearButton(false);
            ShowNotification(_msgSomeErrorProcessing,a);

            $("#btnShowMoreResults").removeAttr("disabled");
            $("#btnShowMoreResults").attr("class", "loadmore");
        },
        global: false // This will not enable ".ajaxStart() global event handler and so global progress bar will not fire.
    });


}

function RefreshResults() {

     var jsonPostData = {
           p_CustomerGuid: _Customer,
           p_FromDate: _FromDate,
           p_ToDate: _ToDate,
           p_SearchTerm: _SearchTerm,
           p_SubMediaType: _SubMediaType,
           p_CategoryGUID: _CategoryGUID,
           p_SelectionType : _SelectionType,
           p_IsAsc: _IsAsc,
           p_SortColumn: _SortColumn,
           p_PageSize : _PageSize
       }

       if (DateValidation() === true) {

           $.ajax({
               url: _urlLibrarySearchLibraryResults,
               contentType: "application/json; charset=utf-8",
               type: "post",
               dataType: 'json',
               data: JSON.stringify(jsonPostData),
               success: function (result) {
                   //CheckForSessionExpired(result);
                   
                    // Set filter irrespective of success or failure
                    SetActiveFilter();

                   if (result.isSuccess) {
                       $("#divShowResult").show();
                       $("#divRecordCount").show();
                       $("#ulIQArchieveResults").html(result.HTML);
                       ModifyFilters(result.filter);
                       _CurrentCategoryFilter = result.filter.Categories.slice(0);
                       ShowHideMoreResults(result);
                       ShowNoofRecords(result);
                       SetMediaClickEvent();
                       $("#chkInputAll").prop("checked", false);

                       
                       if (!_EditRecordMode && screen.height >= 768) {
                            setTimeout(function () { $("#divResultScrollContent").mCustomScrollbar("scrollTo", "top");   }, 200);
                       }

                       $(".media input[type=checkbox]").each(function () {
                           $(this).prop("checked", false);
                       });

                       SetImageSrc();
                   }
                   else 
                   {
                       ShowNotification(_msgSomeErrorProcessing);
                       ClearResultsOnError('ulIQArchieveResults', 'divRecordCount', 'divShowResult', _msgErrorOnSearch.replace(/@@MethodName@@/g, "RefreshResults()"));
                   }

                   _EditRecordMode = false;
               },
               error: function (a, b, c) {
                   $("#chkInputAll").prop("checked", false);
                   ShowNotification(_msgSomeErrorProcessing,a);
                   ClearResultsOnError('ulIQArchieveResults', 'divRecordCount', 'divShowResult', _msgErrorOnSearch.replace(/@@MethodName@@/g, "RefreshResults()"));
                   _EditRecordMode = false;
               }
           });
       }
       else _EditRecordMode = false;
       
   }

function DeleteByID(id) {

    if(id != "")
    {
        var msgConfirm = _msgConfirmDeletion;
        if (Isv4LibraryRollup) {
            msgConfirm = _msgConfirmDeletionRollup
        }

        getConfirm("Delete Library", msgConfirm, "Confirm Delete", "Cancel", function (res) {
            if(res)
            {
                _ParentCount = 0;
                if ($("#hdnIsParent_" + id).length == 0 || $("#hdnIsParent_" + id).val() == "true") {
                    _ParentCount = _ParentCount + 1;
                }
                Delete(id);
             }
        });
    }
}

function DeleteRecords()
{
    _ParentCount = 0;
    var output = $.map($("#ulIQArchieveResults input[type=checkbox]:checked"), function(n, i){
      if ($("#hdnIsParent_" + n.value).length == 0 || $("#hdnIsParent_" + n.value).val() == "true") {
        _ParentCount = _ParentCount + 1;
      }
      return n.value; }).join(',');
    if(output != "")
    {
        var msgConfirm = _msgConfirmDeletion;
        if (Isv4LibraryRollup) {
            msgConfirm = _msgConfirmDeletionRollup
        }

        getConfirm("Delete Library", msgConfirm, "Confirm Delete", "Cancel", function (res) {
            if(res)
            {
                Delete(output);
            }
        });
    }
    else
    {
        ShowNotification(_msgSelectRecordToDelete);
    }
}

function Delete(items)
{
    var jsonPostData = 
          {
               ArchiveIDs : items,
               p_CustomerGuid: _Customer,
               p_FromDate: _FromDate,
               p_ToDate: _ToDate,
               p_SearchTerm: _SearchTerm,
               p_SubMediaType: _SubMediaType,
               p_CategoryGUID: _CategoryGUID,
               p_SelectionType : _SelectionType,
               p_ParentCount : _ParentCount
            }

     $.ajax({
        url: _urlLibraryDelete,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: 'json',
        data: JSON.stringify(jsonPostData),
        success: function (result) 
        {

            CheckForSessionExpired(result);

            if (result.isSuccess) 
            {
                var returnIDs = result.archiveIDs.split(',');
                var count = 0;
                if(returnIDs != "")
                {
                    $("#ulTempMediaResults").html(result.HTML);
                    $.each( returnIDs , function(index, val)
                    {
                        count++;
                        var itemID = val;
                        if ($('#divMedia_' + itemID + ' #divChildMedia_' + itemID).length > 0 && $("#hdnIsParent_" + itemID).val() != "true") {
                            //var divActaulParent = $("#divMedia_" + this + " #divChildMedia_" + this + " div[id^='divMedia_'] input[id^='hdnIsParent_'][value='true']").closest("div[id^='divMedia_']");
                            var isFoundParent = false;
                            var parentID = "0";
                            $("#divMedia_" + itemID + " #divChildMedia_" + itemID + " div[id^='divMedia_']").each(function () {
                                if ($.inArray($(this).attr("id").replace("divMedia_", ""), returnIDs) == -1) {
                                    if($("#ulTempMediaResults #" + $(this).attr("id") + " #divChildMedia_" + $(this).attr("id").replace("divMedia_", "")).length > 0)
                                    {
                                        isFoundParent = true;
                                        parentID = $(this).attr("id");
                                    }
                                    else if($("#ulTempMediaResults #" + $(this).attr("id")).length > 0 && !isFoundParent)
                                    {
                                        isFoundParent = true;
                                        parentID = $(this).attr("id");
                                    }
                                }
                            });

                            if (!isFoundParent) {
                                $('#divMedia_' + itemID).animate({ opacity: 0.4 }, 1000).remove();
                            }
                            else
                            {
                                $('#divMedia_' + itemID).replaceWith($("#ulTempMediaResults #" + parentID));
                            }
                        }
                        else if ($('#divMedia_' + itemID).closest("div[id^='divChildMedia_']").length > 0 && $("#hdnIsParent_" + itemID).val() == "true") {
                            $('#divMedia_' + itemID).closest("[id^='divChildMedia_']").closest("[id^='divMedia_']").animate({ opacity: 0.4 }, 1000).remove();
                        }
                        else {
                            $('#divMedia_' + itemID).animate({ opacity: 0.4 }, 1000).remove();
                        }

                    });

                    $('#ulTempMediaResults').html('');

                    if(result.filter != null)
                    {
                        ModifyFilters(result.filter);
                    }

                    ShowNoofRecords(result);
                    SetMediaClickEvent();
                }

                ShowNotification(count + " record(s) deleted successfully");

                SetImageSrc();
      
                $(".media input[type=checkbox]").each( function()
                { 
                    $(this).prop("checked", false);
                    $(this).closest(".media").css("background", "");
                });

                $("#chkInputAll").prop("checked", false);
               
            }
            else 
            {
                ShowNotification(_msgErrorWhileDeleting);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorWhileDeleting);
        }
    });
}

function ShowNoofRecords(result)
{
    if(result != null && result.totalRecords != null && result.totalRecords != "0" && result.currentRecords != null)
    {
        $("#spanRecordsLabel").show();
        if(Isv4LibraryRollup)
        {
            $("#spanCurrentRecords").html(result.currentRecords + " (" + result.currentRecordsDisplay + ")");
            $("#spanTotalRecords").html(result.totalRecords + " (" + result.totalRecordsDisplay + ")");
        }
        else
        {
            $("#spanCurrentRecords").html(result.currentRecords);
            $("#spanTotalRecords").html(result.totalRecords);
        }
        $("#aPageSize").show();
        $("#imgMoreResultLoading").addClass("offset2")
    }
    else
    {
        $("#spanRecordsLabel").hide();
        $("#aPageSize").hide();
        $("#imgMoreResultLoading").removeClass("offset2")
    }
}

function ModifyFilters(filter) {

    disabledDays = [];
    if (filter != null && filter.Dates.length > 0) 
    {
        $.each(filter.Dates, function (id, date) {
            disabledDays.push(date);
        });
    }

    var mediumLI = "";
    if (filter != null && filter.MediaTypes != null) {
        $.each(filter.MediaTypes, function (eventID, eventData) {
            mediumLI += '<li class="dropdown-submenu"><a data-toggle="dropdown" class="dropdown-toggle" href="#" role="button" name="aMediaType">';
            mediumLI += eventData.MediaTypeDesc + ' (' + eventData.RecordCountFormatted + ') </a>';
            mediumLI += '<ul aris-labelledby="drop2" role="menu" class="dropdown-menu sideMenu" id="ulSubMediaType">'
            $.each(eventData.SubMediaTypes, function (eventID2, eventData2) {
                mediumLI += '<li onclick="SetSubMediaType(\'' + eventData2.SubMediaType + '\', \'' + eventData2.SubMediaTypeDesc + '\');" role="presentation"><a href="#" tabindex="-1" role="menuitem">' + eventData2.SubMediaTypeDesc + ' (' + eventData2.RecordCountFormatted + ') </a></li>';                    
            });
            mediumLI += '</ul></li>';
        });
    }

    if (mediumLI == "") {
        $("#ulMediaType").html('<li role="presentation"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
    }
    else {
        $("#ulMediaType").html(mediumLI);
    }

     if (filter != null && filter.Categories != null) {

        var categoriesHTML = "";
        var strAndSelection ="";
        var strOrSelection =""
        if(_SelectionType == 'AND')
        {
            strAndSelection ="checked=\"checked\"";
        }
        else
        {
            strOrSelection ="checked=\"checked\"";
        }
        
        //var liCatSearchType ="<div class=\"fleft margintop5\"><input type=\"radio\" onclick=\"SetSelectionType()\" value=\"AND\" id=\"rdAnd\" style=\"margin-top: -3px\" name=\"rdSelectionType\" "+strAndSelection+" />AND &nbsp;<input type=\"radio\" onclick=\"SetSelectionType()\" value=\"OR\" id=\"rdOr\" style=\"margin-top: -3px\" name=\"rdSelectionType\" "+strOrSelection+" />OR</div>";
        //var liSearchBtn = "<div><input type=\"button\" value=\"Done\" id=\"btnSearchCategory\" class=\"button\" style=\"margin: 0px\" onclick=\"SearchCategory();\"></div>"
        //var liCatSearchDone ="<li role=\"presentation\" style=\"padding: 0px;\"><ul class=\"sideMenu sub-submenu\" aria-labelledby=\"drop2\" role=\"menu\"><li role=\"presentation\" style=\"text-align: right\"><div>" + liCatSearchType + liSearchBtn + "</div></li></ul></li>";
        $.each(filter.Categories, function (eventID, eventData) {
            var liStyle= "";
            if ($.inArray(eventData.CategoryGUID, _CategoryGUID) !== -1) 
            {
                liStyle ="style=\"background-color: rgb(233, 233, 233);\"";
            }
            categoriesHTML = categoriesHTML + '<li role=\"presentation\"><a '+liStyle+'  href=\"javascript:void(0)\" onclick="SetCategory(this,\'' + eventData.CategoryGUID + '\',\'' + eventData.CategoryName.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + '\');" tabindex=\"-1\" role=\"menuitem\">';
            categoriesHTML += EscapeHTML(eventData.CategoryName) + ' (' + eventData.RecordCountFormatted + ') </a></li>';
        });

        if (categoriesHTML == '') {
            $('#ulCategoryList').html('<li role="presentation"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
            $('#liCategorySearch').hide();
        }
        else {
            //var StrLiStart ="<li role=\"presentation\" style=\"padding: 0px;\"><ul id=\"ulCategoryList\" class=\"sideMenu sub-submenu\" aria-labelledby=\"drop2\" role=\"menu\">";
            //var StrLiEnd ="</ul></li>"
            $('#ulCategoryList').html(categoriesHTML);
            $('#liCategorySearch').show();
        }
    }

    if (filter != null && filter.Customers != null) {

        var customerHTML = "";
        $.each(filter.Customers , function (eventID, eventData) {
            customerHTML = customerHTML + '<li onclick="SetCustomer(\'' + eventData.CustomerKey + '\',\'' + eventData.CustomerName.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + '\');" role=\"presentation\"><a href=\"#\" tabindex=\"-1\" role=\"menuitem\">';
            customerHTML += eventData.CustomerName + ' (' + eventData.RecordCountFormatted + ') </a></li>';
        });

        if (customerHTML == "") {
            $('#ulCustomers').html('<li role="presentation"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
        }
        else {
            $('#ulCustomers').html(customerHTML);
        }
    }
}

function SetActiveFilter() {

    var isFilterEnable = false;
    $("#divActiveFilter").html("");
    

    if (_SearchTerm != "") {
        $('#divActiveFilter').append('<div id="divKeywordActiveFilter" class="filter-in">' + EscapeHTML(_SearchTerm) + '<span class="cancel" onclick="RemoveFilter(1);"></span></div>');
        isFilterEnable = true;
    }
    if ((_FromDate != null && _FromDate != "") && (_ToDate != null && _ToDate != "")) 
    {
        $('#divActiveFilter').append('<div id="divDateActiveFilter" class="filter-in">' + _FromDate + ' To ' + _ToDate + '<span class="cancel" onclick="RemoveFilter(2);"></span></div>');
        isFilterEnable = true;
    }
    if (_SubMediaType != "") {
        $('#divActiveFilter').append('<div id="divSubMediaTypeActiveFilter" class="filter-in">' + _SubMediaTypeDescription + '<span class="cancel" onclick="RemoveFilter(3);"></span></div>');
        isFilterEnable = true;
    }
    if (_Customer != "") {
        $('#divActiveFilter').append('<div id="divCustomerActiveFilter" class="filter-in">' + _CustomerName + '<span class="cancel" onclick="RemoveFilter(4);"></span></div>');
        isFilterEnable = true;
    }
     if (_CategoryGUID.length > 0) {
        $('#divActiveFilter').append('<div id="divCategoryActiveFilter" class="filter-in">' + EscapeHTML(_CategoryName) + '<span class="cancel" onclick="RemoveFilter(5);"></span></div>');
        isFilterEnable = true;
    }


    if (isFilterEnable) {
        $("#divActiveFilter").css({ 'border-bottom': '1px solid rgb(236, 236, 236)' });
    }
    else {
        $("#divActiveFilter").css({ 'border-bottom': '' });
    }
}

function RemoveFilter(filterType) {

    // Represent SearchKeyword
    if (filterType == 1) {

        $("#txtKeyword").val("");
        _SearchTerm = "";
        RefreshResults();
    }

    // Represent Date filter(From Date,To Date)
    if (filterType == 2) {

        $("#dpFrom").datepicker("setDate", null);
        $("#dpTo").datepicker("setDate", null);
        $("#divCalender").datepicker("setDate", null);

        $('#aDuration').html(_msgAll +'&nbsp;&nbsp;<span class="caret"></span>');

        _FromDate = null;
        _ToDate = null;
        RefreshResults();
    }

    // Represent SubMediaType Filter
    if (filterType == 3) {
        _SubMediaType = "";
        _SubMediaTypeDescription = "";
        RefreshResults();
    }

    // Represent Customer Filter
    if (filterType == 4) {
        _Customer = "";
        _CustomerName = "";
        RefreshResults();
    }

     // Represent Category Filter
    if (filterType == 5) {
        _CategoryGUID = [];
        _CategoryNameList = [];
        _OldCategoryGUID = [];
        _CategoryName = "";
        _SelectionType = $('#rdOr').val();
        RefreshResults();
    }

}

function SortDirection(p_SortColumn,isAsc) 
{
    if (isAsc != _IsAsc || _SortColumn != p_SortColumn) 
    {
        _IsAsc = isAsc;
        _SortColumn = p_SortColumn;

        if (_IsAsc && _SortColumn == "MediaDate") 
        {
            $('#aSortDirection').html(_msgOldestFirst + ' (Media Date)&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (!_IsAsc && _SortColumn == "MediaDate") 
        {
            $('#aSortDirection').html(_msgMostRecent+ ' (Media Date)&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (_IsAsc && _SortColumn == "CreatedDate") 
        {
            $('#aSortDirection').html(_msgOldestFirst + ' (Created Date)&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (!_IsAsc && _SortColumn == "CreatedDate") 
        {
            $('#aSortDirection').html(_msgMostRecent + ' (Created Date)&nbsp;&nbsp;<span class="caret"></span>');
        }
        RefreshResults();
    }
}

function ShowHideProgressNearButton(value) {
    if (value == true) {
        $("#imgMoreResultLoading").removeClass("visibilityHidden");
    }
    else {
        $("#imgMoreResultLoading").addClass("visibilityHidden");
    }
}

function ShowHideMoreResults(result) 
{
    if (result.hasMoreResults == true) {
        $('#btnShowMoreResults').attr('value', _msgShowMoreResults);
        $('#btnShowMoreResults').attr('onclick', 'GetMoreResults();');
    }
    else {
        $('#btnShowMoreResults').attr('value', _msgNoMoreResult);
        $('#btnShowMoreResults').removeAttr('onclick');
    }
}

function DateValidation() {
    $('#dpFrom').removeClass('warningInput');
    $('#dpTo').removeClass('warningInput');


    // if both empty
    if (($('#dpTo').val() == '') && ($('#dpFrom').val() == '')) {
        return true;

    }
    //if one empty not other
    else if (($('#dpFrom').val() != '') && ($('#dpTo').val() == '')
                    ||
                    ($('#dpFrom').val() == '') && ($('#dpTo').val() != '')
                    ) {
        if ($('#dpFrom').val() == '') {

            ShowNotification(_msgFromDateNotSelected);
            $('#dpFrom').addClass('warningInput');
        }

        if ($('#dpTo').val() == '') {

            ShowNotification(_msgToDateNotSelected);
            $('#dpTo').addClass('warningInput');

        }
        return false;
    }
    //if both not empty
    else {
        var isFromDateValid = isValidDate($('#dpFrom').val().toString());
        var isToDateValid = isValidDate($('#dpTo').val().toString());
        if (isFromDateValid && isToDateValid) {
            var fromDate = new Date($('#dpFrom').val());
            var toDate = new Date($('#dpTo').val());
            if (fromDate > toDate) {
                ShowNotification(_msgFromDateLessThanToDate);
                $('#dpFrom').addClass('warningInput');
                $('#dpTo').addClass('warningInput');
                return false;
            }
            else {
                return true;

            }
        }
        else {
            if (!isFromDateValid) {
                $('#dpFrom').addClass('warningInput');
            }
            if (!isToDateValid) {
                $('#dpTo').addClass('warningInput');
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

function GetResultOnDuration(duration) {

    $("#dpFrom").removeClass("warningInput");
    $("#dpTo").removeClass("warningInput");
    var dtcurrent = new Date();
    var fDate;
    _Duration = duration;

    // All
    if (duration == 0) 
    {
        $("#dpFrom").val("");
        $("#dpTo").val("");
        dtcurrent = "";
        RemoveFilter(2);
        $('#aDuration').html(_msgAll + '&nbsp;&nbsp;<span class="caret"></span>');
    }

    // 24 hours
    else if (duration == 1) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth(), dtcurrent.getDate() - 1);
        $('#aDuration').html(_msgLast24Hours + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    // Last week
    else if (duration == 2) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth(), dtcurrent.getDate() - 7);
        $('#aDuration').html(_msgLastWeek + '&nbsp;&nbsp;<span class="caret"></span>');
    }

    // Last Month
    else if (duration == 3) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth() - 1, dtcurrent.getDate());
        $('#aDuration').html(_msgLastMonth + '&nbsp;&nbsp;<span class="caret"></span>');
    }

    // Last 3 Months
    else if (duration == 4) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth() - 3, dtcurrent.getDate());
        $('#aDuration').html(_msgLast3Month + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    // Custom
    else if (duration == 5) {

        dtcurrent = null;
        if ($("#dpFrom").val() != "" && $("#dpTo").val() != "") {
            $('#aDuration').html(_msgCustom + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        else 
        {
            if ($("#dpFrom").val() == "") 
            {
                $("#dpFrom").addClass("warningInput");
            }
            if ($("#dpTo").val() == "") 
            {
                $("#dpTo").addClass("warningInput");
            }
            ShowNotification(_msgSelectDate);
        }
    }

    $("#dpFrom").datepicker("setDate", fDate);
    $("#dpTo").datepicker("setDate", dtcurrent);

    if ($("#dpFrom").val() != "" && $("#dpTo").val() != "") {

        if (_FromDate != $("#dpFrom").val() || _ToDate != $("#dpTo").val()) {
            _FromDate = $("#dpFrom").val();
            _ToDate = $("#dpTo").val();
            RefreshResults();
        }
    }
}

/*---------------------------------------------- Clip Player Scripts ---------------------------------------------------------------------*/

function LoadClipPlayerOld(clipID) {
    
    var jsonPostData = { ClipID: clipID }

    $.ajax({

        type: "post",
        dataType: "json",
        url: _urlLibraryLoadClipPlayer,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPostData),
        success: OnLoadClipPlayerComplete,
        error: OnClipLoadFail
    });

    if(IsShowTimeSync)
    {
         /* load chart for player */
        $.ajax({

            type: "post",
            dataType: "json",
            url: _urlLibraryGetChart,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(jsonPostData),
            success: OnGetChartComplete,
            error: OnGetChartFail
        });
    }
}





function OnLoadClipPlayerComplete(result) {

    CheckForSessionExpired(result);

    if (result != null && result.isSuccess) 
    {
        var clipPlayer = '<div id="diviframe" runat="server" class="modal fade hide resizable modalPopupDiv">'
                        + '<div class="closemodalpopup"><img id="img2" src="../Images/close-icon.png" class="popup-top-close" onclick="ClosePlayer(\'diviframe\');" /></div>'
                        + '<div id="divIFrameMain" class="modalPopupMediaDiv"><div style="position:relative;float:left;"><div id="divCapMain" class="CaptionMain">'
                    + '<div id="divCaption"></div></div>'
                    + '<div id="divShowCaption" class="divShowCaption" onclick="RegisterCCCallback();">'
                    + '<img src="../../../Images/right_arrow_cc.gif" id="imgCCDirection" alt="Show CC" /></div>';
                if(IsShowTimeSync)
                {
                    clipPlayer += 
                       '<div id= "divShowHideTimeSync" onclick="ShowHideTimeSync();" style="position: absolute; right: 0px; top: 325px;cursor:pointer;" title="Show Time Sync">'
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
        RegisterCCCallback();
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

function RegisterCCCallback() {

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

/*----------------------------------------- Download -------------------------------------------------*/

function DownloadNM(itemid)
{
    if(itemid != "")
    {
        window.open('/Download/Index/?type=NM&id='+ itemid,'_blank','width=600,height=625,scrollbars=1,location=0,menubar=0,toolbar=0');
    }
}

function DownloadSM(itemid)
{
   if(itemid != "")
   {
        window.open('/Download/Index/?type=SM&id='+ itemid,'_blank','width=600,height=625,scrollbars=1,location=0,menubar=0,toolbar=0');
   }
}

function DownloadTV(itemid)
{
    if(itemid != "")
    {
        window.open('/Download/Index/?type=TV&id='+ itemid,'_blank','width=600,height=625,scrollbars=1,location=0,menubar=0,toolbar=0');
    }
}

function DownloadTVEyes(itemid)
{
    if(itemid != "")
    {
        window.open('/Download/Index/?type=TVEyes&id='+ itemid,'_blank','width=600,height=625,scrollbars=1,location=0,menubar=0,toolbar=0');
    }
}

function DownloadRadio(itemid)
{
    if(itemid != "")
    {
        window.open('/Download/Index/?type=Radio&id='+ itemid,'_blank','width=600,height=625,scrollbars=1,location=0,menubar=0,toolbar=0');
    }
}

function OpenDownloadStatus()
{
    window.open('/Download/TV/','_blank','width=600,height=625,scrollbars=1,location=0,menubar=0,toolbar=0');
}


/*-------------------------------- Email send functionality -------------------------------------*/

function OpenEmailPopUp(emailtype)
{
    if(emailtype == "result" || emailtype == "report" || emailtype == "mcmedia")
    {
        $("#hdnEmailFunctionalityType").val(emailtype);

        var output;
        if (emailtype == "result") {
            output = $.map($("#ulIQArchieveResults input[type=checkbox]:checked"), function(n, i){ return n.value; }).join(',');
        }
        else if (emailtype == "mcmedia") {
            output = $.map($("input[id^='chkMCResults_']:checked"), function(n, i){ return n.value; }).join(',');
        }

        if(emailtype == "result" || emailtype == "mcmedia")
        {
            if(output == "")
            {
                ShowNotification(_msgAtleastOneRecordToEmail);
                return;
            }
            else
            {
                var selectedItemCount = output.toString().split(',').length;
                var maxItemEmailLimit = parseInt($("#hdnLibraryMaxLibraryEmailItems").val());
                if(selectedItemCount > maxItemEmailLimit)
                {
                   getConfirm("Max Limit Exceeded", _msgMaxLibraryEmailItemsMessage.replace(/@@MaxLibraryEmailItems@@/g, maxItemEmailLimit), "Confirm", "Cancel", function (res) {
                    if (res) {
                        ShowEmailPopup();
                        }
                    });
                }
                else
                {
                    ShowEmailPopup();
                }
            }
        }
        else if(emailtype == "report")
        {
            if($("#hdnReportID").val() == undefined || $("#hdnReportID").val() == "")
            {
                 ShowNotification(_msgNoReportAvailable);
                 return;
            }
            else {
                ShowEmailPopup();
            }
        }         
     }
}

function ShowEmailPopup()
{
         $("#spanFromEmail").html("").hide();
         $("#spanToEmail").html("").hide();
         $("#spanBCCEmail").html("").hide();
         $("#spanSubject").html("").hide();
         $("#spanMessage").html("").hide();

         if ($("#hdnEmailFunctionalityType").val() != "mcmedia")
         {
            $("#divEmailTemplate").hide();
         }
         else
         {
            $("#divEmailTemplate").show();
            $("#ddlTemplate>option[isdefault=true]").prop('selected', true);
         }
            
         $("#divEmailPopup input[type=text]").val("");
         $("#divEmailPopup button[type=button]").removeAttr("disabled");
         $("#txtMessage").val("");

          
                $('#divEmailPopup').modal({
                backdrop: 'static',
                keyboard: true,
                dynamic: true
            });
          

         $("#txtFromEmail").val($("#hdnDefaultSender").val());
}

function CancelEmailpopup()
{
    $("#divEmailPopup").css({ "display": "none" });
    $("#divEmailPopup").modal("hide");
    $("#chkInputAll").prop("checked", false);
}

function SendEmail()
{
    var emailFunctionalityType = $("#hdnEmailFunctionalityType").val();

    if(emailFunctionalityType == "result" || emailFunctionalityType == "report" || emailFunctionalityType == "mcmedia")
    {
        var requestURL = "";
        var jsonPostData = null;
        var output;

        if(emailFunctionalityType == "result")
        {
            output = $.map($("#ulIQArchieveResults input[type=checkbox]:checked"), function(n, i){ return n.value; }).join(',');
    
            if(output != "")
            {
                requestURL =  _urlLibrarySendEmail;
                var jsonPostData = { 
                    ArchiveIDs : output,
                    FromEmail: $("#txtFromEmail").val(),
                    ToEmail: $("#txtToEmail").val(),
                    BCCEmail: $("#txtBCCEmail").val(),
                    Subject: $("#txtSubject").val(),
                    UserBody: $("#txtMessage").val()
                }
            }
            else
            {
                alert(_msgAtleastOneRecordToEmail);
                return;
            }
        }
        else if (emailFunctionalityType == "mcmedia")
        {
            output = $.map($("input[id^='chkMCResults_']:checked"), function(n, i){ return n.value; }).join(',');
    
            if(output != "")
            {
                requestURL =  _urlLibrarySendMCMediaEmail;
                var jsonPostData = { 
                    ArchiveIDs : output,
                    FromEmail: $("#txtFromEmail").val(),
                    ToEmail: $("#txtToEmail").val(),
                    BCCEmail: $("#txtBCCEmail").val(),
                    Subject: $("#txtSubject").val(),
                    UserBody: $("#txtMessage").val(),
                    TemplateID: $("#ddlTemplate").val()
                }
            }
            else
            {
                alert(_msgAtleastOneRecordToEmail);
                return;
            }
        }
        else 
        {           
            if($("#hdnReportID").val() == undefined || $("#hdnReportID").val() == "")
            {
                ShowNotification(_msgNoReportAvailable);
                return;
            }

            requestURL = _urlLibraryReportEmail;
    
            var chartHTML = [];
            $('input:checkbox[name=chkShowHideDashboard]:checked').each(function () {
                var jsonChart = { medium: $(this).val(), html: $("#div" + $(this).val()).html() }
                chartHTML.push(jsonChart);
            });

            var jsonPostData = { 
                    p_ReportID :  $("#hdnReportID").val(),
                    FromEmail: $("#txtFromEmail").val(),
                    ToEmail: $("#txtToEmail").val(),
                    BCCEmail: $("#txtBCCEmail").val(),
                    Subject: $("#txtSubject").val(),
                    UserBody: $("#txtMessage").val(),
                    p_ChartHTML: chartHTML
                }
        }

        if(ValidateSendEmail())
        {
            $("#divEmailPopup button[type=button]").prop("disabled", "disabled");             

            $.ajax({
                type: "post",
                dataType: "json",
                url: requestURL,
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(jsonPostData),
                success:function(result){
                    CheckForSessionExpired(result);

                    CancelEmailpopup();

                    if (result.isSuccess) 
                    {
                        ShowNotification("Email sent successfully to " + result.emailSendCount + " person(s)");
                        if (emailFunctionalityType == "result" || emailFunctionalityType == "report") {
                            $(".media input[type=checkbox]").each( function()
                                { 
                                    $(this).prop("checked", false);
                                    $(this).closest(".media").css("background", "");
                                });
                        }
                        else {
                            $("input[id^='chkMCResults_']").each(function () {
                                $(this).prop("checked", false);
                                $(this).closest(".media").css("background", "");
                            });

                            $("#chkMCAll").prop("checked", false);                            
                        }
                    }
                    else 
                    {
                        ShowNotification(result.errorMessage);
                        $("#divEmailPopup button[type=button]").removeAttr("disabled");
                    }
                },
                error: function (a, b, c) {
                    CancelEmailpopup();
                    ShowNotification(_msgSomeErrorProcessing);
                    $("#divEmailPopup button[type=button]").removeAttr("disabled");
                }
            });
        }
    }
}

function ValidateSendEmail()
{
     var isValid = true;

    $("#spanFromEmail").html("").hide();
    $("#spanToEmail").html("").hide();
    $("#spanSubject").html("").hide();
    $("#spanMessage").html("").hide();

    if($("#txtFromEmail").val() == "")
    {
        $("#spanFromEmail").show().html(_msgFromEmailRequired);
        isValid = false;
    }
    if($("#txtToEmail").val() == "")
    {
        $("#spanToEmail").show().html(_msgToEmailRequired);
        isValid = false;
    }
   
    if($("#txtSubject").val() == "")
    {
        $("#spanSubject").show().html(_msgSubjectRequired);
        isValid = false;
    }
    if($("#txtMessage").val() == "")
    {
       $("#spanMessage").show().html(_msgMessageRequired);
        isValid = false;
    }

    if($("#txtFromEmail").val() != "" && !CheckEmailAddress($("#txtFromEmail").val()))
    {
        $("#spanFromEmail").show().html(_msgIncorrectEmail);
        isValid = false;
    }

    if($("#txtToEmail").val() != "")
    {
        var Toemail = $("#txtToEmail").val();
        if(Toemail.substr(Toemail.length - 1) == ";"){
            Toemail = Toemail.slice(0,-1);
        }
        $(Toemail.split(';')).each( function(index,value)
        {
            if(value.indexOf(',') > 0)
            {
                $("#spanToEmail").show().html(_msgEmailNotCommaSeprated);
                 isValid = false;              
            }
            else
            {
                if(!CheckEmailAddress($.trim(value)))
                {
                    $("#spanToEmail").show().html(_msgOneEmailAddressInCorrect);
                    isValid = false;
                }
            }            
         });         

        if (Toemail.split(';').length > _MaxEmailAdressAllowed) {
            $("#spanToEmail").show().html(_msgMaxEmailAdressLimitExceeds.replace(/@@MaxLimit@@/g, _MaxEmailAdressAllowed));
            isValid = false;
        }
     }

    if($("#txtBCCEmail").val() != "")
    {
        if ($("#txtToEmail").val() == "") {
            $("#spanBCCEmail").show().html(_msgBCCEmailMissingTo);
            isValid = false;
        }
        else {
            var bccEmail = $("#txtBCCEmail").val();
            if(bccEmail.substr(bccEmail.length - 1) == ";"){
                bccEmail = bccEmail.slice(0,-1);
            }
            $(bccEmail.split(';')).each( function(index,value)
            {
                if(value.indexOf(',') > 0)
                {
                    $("#spanBCCEmail").show().html(_msgEmailNotCommaSeprated);
                     isValid = false;              
                }
                else
                {
                    if(!CheckEmailAddress($.trim(value)))
                    {
                        $("#spanBCCEmail").show().html(_msgOneEmailAddressInCorrect);
                        isValid = false;
                    }
                }            
             });         

            if (bccEmail.split(';').length > _MaxEmailAdressAllowed) {
                $("#spanBCCEmail").show().html(_msgMaxEmailAdressLimitExceeds.replace(/@@MaxLimit@@/g, _MaxEmailAdressAllowed));
                isValid = false;
            }
        }
     }

     return isValid;
}

function LoadTMPopup(id) {

    var jsonPostData = {
        p_ID: id
    }

    $.ajax({

        url: _urlLibraryGetArchiveTVEyesLocation,
        type: "post",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPostData),
        success: function (result) {

            if (result.isSuccess) {
                $("#divtranscript").html(result.HTML);
                
                var htmlaudio  ='<audio autoplay="autoplay" controls="controls" class="width100p"><source id="vidsrc" src="'+result.AudiFileLocation+'" id="audiosource" /></audio>';
                $("#divAudio").html(htmlaudio);

                $('#divLoadTMIframePopup').modal({
                    backdrop: 'static',
                    keyboard: true,
                    dynamic: true
                });
            }
            else {
                ShowErrorMessage();
            }
        },
        error: function (a, b, c) {
            ShowErrorMessage();
        }
    });
}

function CloseTMIframePopup(){
    $("#divLoadTMIframePopup").css({ "display": "none" });
    $("#divLoadTMIframePopup").modal("hide");
    

     $("audio").each(function () { 
        this.pause();
        this.src = "";
     });

    $("#divAudio").html('');

    $("#divtranscript").html('');
}

function SelectPageSize(pageSize) {
    if (_PageSize != pageSize) {
        _PageSize = pageSize;
        $('#aPageSize').html(_PageSize + ' Items Per Page&nbsp;&nbsp;<span class="caret"></span>');
         $('#divPopover').remove();
        RefreshResults();
    }
}

function showPopupOver() {
    if ($('#divPopover').length <= 0) {
        var drphtml = $("#divPageSizeDropDown").html();
        $('#aPageSize').popover({
            trigger: 'manual',
            html: true,
            title: '',
            placement: 'top',
            template: '<div id="divPopover" class="popover" style="width:125px;"><div class="popover-inner"><div class="popover-content"><p></p></div></div></div>',
            content: drphtml
        }); 
        $('#aPageSize').popover('show');
    }
    else{
        $('#divPopover').remove();
    }
}

function ShowChild(parentid) {

    if ($('#divChildMedia_' + parentid).is(':visible')) {
        $('#divChildMedia_' + parentid).hide();
        $('#expand_' + parentid).attr('src', '../images/expand.png');
        
    }
    else {
        $('#divChildMedia_' + parentid).show();
        $('#expand_' + parentid).attr('src', '../images/collapse.png');
    }
}

function SetMediaClickEvent() {
    $("#divResultScrollContent .media").click(function (e) {
        if ($(e.target).closest("a").length <= 0 && e.target.type != "checkbox") {
            e.stopPropagation();
            
            // If a parent item was updated, only update child items if "Select All w/ Dupes" is set
            var isChildItem = $(e.target).closest("div[id^='divChildMedia_']").length == 1;
            var isCheckedSelector = isChildItem ? "input" : "input[id^='chkdivResults_']";
            var checkSelector = isChildItem || _IsSelectAll ? "input" : "input[id^='chkdivResults_']";

            if ($(e.target).closest('.media').find(isCheckedSelector).is(':checked')) {
                $(e.target).closest('.media').find(checkSelector).removeAttr('checked');
                $(this).css("background", "");
            }
            else {
                $(e.target).closest('.media').find(checkSelector).prop('checked', true);
                $(this).css("background", "#F4F4F4");
            }            

            if (_IsSelectAll) {
                if($("#divResultScrollContent .media input[type=checkbox]").length == $(".media input[type=checkbox]:checked").length) {
                    $("#chkInputAll").prop("checked", true);
                } else {
                    $("#chkInputAll").prop("checked", false);
                }
            }
            else{
                if($("#divResultScrollContent .media input[type=checkbox][id^='chkdivResults_']").length == $(".media input[type=checkbox][id^='chkdivResults_']:checked").length) {
                    $("#chkInputAll").prop("checked", true);
                } else {
                    $("#chkInputAll").prop("checked", false);
                }
            }
        }
    });
}



function GetPlayLogNSummary(ClipID)
{
    if (PopupDateValidation()) {
        _FromDate = $("#dpFromPopup").val();;
        _ToDate = $("#dpToPopup").val();    
        var jsonPostData = {
            p_AssetGuid: ClipID,
            p_FromDate: _FromDate,
            p_ToDate: _ToDate

        }
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlLibraryGetIQPlayLogNSummary,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            success: OnResultComplete,
            error: OnFailVideoMetrics
        });
    }
}

var OnFailVideoMetrics=function(XHR,Status,Error)
{
    ShowNotification(_msgErrorOccured);
}

function ShowPlayLogNSummaryPopup(ClipID, TVThumbUrl)
{
    $('#divPlayLogNSummaryPopup').modal({
        backdrop: 'static',
        keyboard: true,
        dynamic: true
    });

    $("#dpFromPopup").datepicker({
        //beforeShowDay: enableAllTheseDays,
        showOn: "button",
        buttonImage: "../images/calender.png",
        buttonImageOnly: true,
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            $('#dpFromPopup').val(dateText);
            SetDateVariable();
        },
        onClose: function (dateText) {
            //$('#dpFrom').focus();
            //SetDateVariable();
        }
    });

    $("#dpToPopup").datepicker({
        showOn: "button",
        buttonImage: "../images/calender.png",
        buttonImageOnly: true,
        changeMonth: true,
        changeYear: true,
        //beforeShowDay: enableAllTheseDays,
        onSelect: function (dateText, inst) {
            $('#dpToPopup').val(dateText);
            SetDateVariable();
        },
        onClose: function (dateText) {
            //$('#dpTo').focus();
            //SetDateVariable();
        }
    });

    if (_FromDate == null || _ToDate == null) {
        var currDate = new Date();
        var previousDate = new Date(currDate.getFullYear(), currDate.getMonth(), currDate.getDate() - 30);
        _FromDate = previousDate;
        _ToDate = currDate;

        $("#dpFromPopup").datepicker("setDate", previousDate);
        $("#dpToPopup").datepicker("setDate", currDate);
    }

    $('#hdnClipID').val(ClipID);
    $("#ClipPlayerLogo").attr("onclick","LoadClipPlayer('" + ClipID + "')")
    $("#imgClipThumbnails").attr("src",TVThumbUrl)

    GetPlayLogNSummary(ClipID) 
}

function ClosePlayLogNSummaryPopup(){
    $("#divPlayLogNSummaryPopup").css({ "display": "none" });
    $("#divPlayLogNSummaryPopup").modal("hide");
}

function GetTVViews(){
    var ClipID = $("#hdnClipID").val();
    GetPlayLogNSummary(ClipID)
}

function OnResultComplete(result) {
    if(result.jsonSparkChartRecords != null)
    {
        $('#TVTitle').html(result.clipTitle);
        $('#TVLifeTimeCount').html(result.lifeTimeCount);
        $('#TotalViews').html(result.totalViews);
        $('#divPlayLogNSummaryChart').html('');
        var JsonSparkChart = JSON.parse(result.jsonSparkChartRecords);
        $('#divPlayLogNSummaryChart').highcharts(JsonSparkChart);
        RenderDemographicsMapChart(result.jsonFusionMapRecords, "divUsaMap");
        $('#ulTopRegions').html(result.topRegionHTML);
        $('#divTopReferrerSites').html(result.topReferrerSitesHTML);
    }
}

function RenderDemographicsMapChart(jsonMapData, divMapChartID) {        
    var demographicsMap = new FusionCharts({
        type: 'maps/usa',
        renderAt: divMapChartID,
        width: '100%',
        height: '150px',
        dataFormat: 'json',
        dataSource:jsonMapData
    }).render();
    demographicsMap.addEventListener('entityRollOut',function(evt,data){
        hideToolTip();
    });
    demographicsMap.addEventListener('entityRollOver',function(evt,data){
        showToolTipOnChart("State : " + data.label + "<br/>" + "Plays : " + data.value);
    });
//    demographicsMap.addEventListener('chartRollOver',function(evt,data){
//        SetCurrentSelectedItemsFillColor(this);
//    });
//    demographicsMap.addEventListener('chartRollOut',function(evt,data){
//        SetCurrentSelectedItemsFillColor(this);
//    });
}

var x,y,zInterval;
var Interval=0;
document.onmousemove = setMouseCoords;

function setMouseCoords(e) {
    if(document.all) {
        tooltipx = window.event.clientX;
        tooltipy = window.event.clientY + 600;
       
    } else {
   
        tooltipx = e.pageX;
        tooltipy = e.pageY;
    }
}

function showToolTipOnChart(zText) {
    clearInterval(zInterval);
    zInterval = setTimeout("doShowToolTip('" + zText.trim() + "')",0);
    Interval=0;
}

function doShowToolTip(zText) {
    clearInterval(zInterval);

    document.getElementById("mapToolTip").style.top = (tooltipy+10) + "px";
    document.getElementById("mapToolTip").style.left = tooltipx + "px";
    document.getElementById("mapToolTip").innerHTML = zText.trim();
    document.getElementById("mapToolTip").style.display = "block";
    zInterval = setTimeout("hideToolTip()",500000);
}

function hideToolTip() {
    zInterval = setTimeout("hideToolTip1()",0);
    Interval=0;   
}

function hideToolTip1() {
    if(Interval!=1000)
    {
        document.getElementById("mapToolTip").style.display = "none";
        clearInterval(zInterval);
        Interval=0;
    }
}

function hideToolTipDiv() {
    zInterval = setTimeout("hideToolTip1()",100000);
    Interval=1000;
}

function PopupDateValidation() {
    $('#dpFromPopup').removeClass('warningInput');
    $('#dpToPopup').removeClass('warningInput');

    // if both empty
    if (($('#dpToPopup').val() == '') && ($('#dpFromPopup').val() == '')) {
        return true;

    }
    //if one empty not other
    else if (($('#dpFromPopup').val() != '') && ($('#dpToPopup').val() == '')
                    ||
                    ($('#dpFromPopup').val() == '') && ($('#dpToPopup').val() != '')
                    ) {
        if ($('#dpFromPopup').val() == '') {

            ShowNotification(_msgFromDateNotSelected); //'From Date not selected');
            $('#dpFromPopup').addClass('warningInput');
        }

        if ($('#dpToPopup').val() == '') {

            ShowNotification(_msgToDateNotSelected); //'To Date not selected');
            $('#dpToPopup').addClass('warningInput');

        }
        return false;
    }
    //if both not empty
    else {
        var isFromDateValid = isValidDate($('#dpFromPopup').val().toString());
        var isToDateValid = isValidDate($('#dpToPopup').val().toString());
        if (isFromDateValid && isToDateValid) {
            var fromDate = new Date($('#dpFromPopup').val());
            var toDate = new Date($('#dpToPopup').val());
            if (fromDate > toDate) {
                ShowNotification(_msgFromDateLessThanToDate); //'From Date should be less than To Date');
                $('#dpFromPopup').addClass('warningInput');
                $('#dpToPopup').addClass('warningInput');
                return false;
            }
            else {
                return true;

            }
        }
        else {
            if (!isFromDateValid) {
                $('#dpFromPopup').addClass('warningInput');
            }
            if (!isToDateValid) {
                $('#dpToPopup').addClass('warningInput');
            }
            ShowNotification(_msgInvalidDate);
            return false;
        }
    }
}

function DateValidation() {
    $('#dpFrom').removeClass('warningInput');
    $('#dpTo').removeClass('warningInput');

    // if both empty
    if (($('#dpTo').val() == '') && ($('#dpFrom').val() == '')) {
        return true;

    }
    //if one empty not other
    else if (($('#dpFrom').val() != '') && ($('#dpTo').val() == '')
                    ||
                    ($('#dpFrom').val() == '') && ($('#dpTo').val() != '')
                    ) {
        if ($('#dpFrom').val() == '') {

            ShowNotification(_msgFromDateNotSelected); //'From Date not selected');
            $('#dpFrom').addClass('warningInput');
        }

        if ($('#dpTo').val() == '') {

            ShowNotification(_msgToDateNotSelected); //'To Date not selected');
            $('#dpTo').addClass('warningInput');

        }
        return false;
    }
    //if both not empty
    else {
        var isFromDateValid = isValidDate($('#dpFrom').val().toString());
        var isToDateValid = isValidDate($('#dpTo').val().toString());
        if (isFromDateValid && isToDateValid) {
            var fromDate = new Date($('#dpFrom').val());
            var toDate = new Date($('#dpTo').val());
            if (fromDate > toDate) {
                ShowNotification(_msgFromDateLessThanToDate); //'From Date should be less than To Date');
                $('#dpFrom').addClass('warningInput');
                $('#dpTo').addClass('warningInput');
                return false;
            }
            else {
                return true;
                 
            }
        }
        else {
            if (!isFromDateValid) {
                $('#dpFrom').addClass('warningInput');
            }
            if (!isToDateValid) {
                $('#dpTo').addClass('warningInput');
            }
            ShowNotification(_msgInvalidDate);
            return false;
        }
    }
}

var SetThumbOffsetToStart = function () {

    _clipThumbOffset = _clipStart;
    $("#thumbOffset").html(_clipThumbOffset.toHHMMSS());
}

var UpdateLKnob = function () {

    var pos = (($(".video-dragger-l").width() * _clipStart) / _Stop);

    $(".video-dragger-l-lk").css("left", pos - 6);
};

var SetPreviewTimeToStart = function () {

    $(".video-preview-time").html(_clipStart.toHHMMSS());

}

var SeekThumb = function (seekPoint) {

    console.log("Seek...SeekThumb");
    _flash.setSeekPoint(seekPoint);
}

function BuildDashboard() {    
    if ($("#chkInputAll").is(":checked")) {
        var jsonPostData = {
            p_CustomerGuid: _Customer,
            p_FromDate: _FromDate,
            p_ToDate: _ToDate,
            p_SearchTerm: _SearchTerm,
            p_SubMediaType: _SubMediaType,
            p_CategoryGUID: _CategoryGUID,
            p_SelectionType: _SelectionType,
            p_IsOnlyParents: _IsSelectAllParent 
        }

        ShowLoading();

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlLibraryGetDashboardArchiveIDs,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            global: false,
            success: function (result) {
                if (result.isSuccess) {
                    HideLoading();

                    _MediaIDs = result.mediaIDs;                        
                    OpenDashboardPopup("Library");
                }
                else {
                    ShowErrorMessage();
                }
            },
            error: function (a, b, c) {
                HideLoading();
                ShowErrorMessage();
            }
        });
    }
    else {
        _MediaIDs = $.map($("#ulIQArchieveResults input[type=checkbox]:checked"), function (n, i) {
            return n.value;
        });

        if (_MediaIDs.length == 0) {
            ShowNotification(_msgAtleastOneRecordSelect);
        }
        else {
            OpenDashboardPopup("Library");
        }
    }
}

function SeekPoint(second)
{
    var _flash = document.getElementById('HUY');
    _flash.setSeekPoint(second);
}

function ShowErrorMessage(a) {
    ShowNotification(_msgErrorOccured, a);
}

function LoadGalleryResults() {
    $.ajax({
        type: "post",
        dataType: "json",
        url: _urlLibraryDisplayGallaryResults,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result.isSuccess) {
                $("#divGalleryResultsHTML").html(result.HTML);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgSomeErrorProcessing);
        }
    });
}

function ShowViewArticleLibrary(ID) {
    var jsonPostData = {
        ID: ID
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlLibraryGetProQuestRecordByID,
        data: JSON.stringify(jsonPostData),
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            if (result.isSuccess) {
                $("#divPQTitle").html(result.title);
                $("#divPQMediaDate").html(result.mediaDate);
                $("#divPQAuthor").html(result.authors);
                $("#divPQPublication").html(result.publication);
                $("#divPQContent").html(result.content);
                $("#divPQCopyright").html(result.copyright);

                $('#divViewArticlePopup').modal({
                    backdrop: 'static',
                    keyboard: true,
                    dynamic: true
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

function CloseViewArticlePopup()
{    
    $('#divViewArticlePopup').css({ "display": "none" });
    $('#divViewArticlePopup').unbind().modal();
    $('#divViewArticlePopup').modal('hide');
}

function AddToMCMedia() {    
    var mediaIDs = $.map($("#ulIQArchieveResults input[type=checkbox]:checked"), function (n, i) {
        return n.value;
    });

    if (mediaIDs.length > 0) {        
        var jsonPostData = { mediaIDs: mediaIDs }

        $.ajax({
            url: _urlLibraryAddToMCMedia,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                if (result.isSuccess) {
                    ShowNotification(result.numAdded + " item(s) added to Media Room successfully");
                    $(".media input[type=checkbox]").each(function () {
                        $(this).prop("checked", false);
                        $(this).closest(".media").css("background", "");
                    });

                    $("#chkInputAll").prop("checked", false);
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
        ShowNotification(_msgAtleastOneRecordSelect);
    }
}

function RemoveFromMCMedia(source) {
    var mediaIDs;
    if (source == "library") {  
        mediaIDs = $.map($("#ulIQArchieveResults input[type=checkbox]:checked"), function (n, i) {
            return n.value;
        });
    }
    else if (source == "mcmedia") {        
        mediaIDs = $.map($("input[id^='chkMCResults_']:checked"), function (n, i) {
            return n.value;
        });
    }

    if (mediaIDs.length > 0) {        
        var jsonPostData = { mediaIDs: mediaIDs }

        $.ajax({
            url: _urlLibraryRemoveFromMCMedia,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                if (result.isSuccess) {
                    ShowNotification(result.numRemoved + " item(s) removed from Media Room successfully");
                    if (source == "library") {
                        $("#ulIQArchieveResults .media input[type=checkbox]").each(function () {
                            $(this).prop("checked", false);
                            $(this).closest(".media").css("background", "");
                        });

                        $("#chkInputAll").prop("checked", false);
                    }
                    else if (source == "mcmedia") {                        
                        $("input[id^='chkMCResults_']").each(function () {
                            $(this).prop("checked", false);
                            $(this).closest(".media").css("background", "");
                        });

                        $("#chkMCAll").prop("checked", false);
                        LoadResults_MCMedia(false);
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
    else {
        ShowNotification(_msgAtleastOneRecordSelect);
    }
}


