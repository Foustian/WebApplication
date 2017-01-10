var _SearchTerm = "";
var _FromDate = null;
var _ToDate = null;
var _Class = "";
var _ClassName = "";
var _Date = null;
var _IsAsc = false;
var _Duration = null;
var _IsNext = false;
var _SortColumn = '';
var _Title = '';
var _IsDefaultLoad = true;
var isFilterEnable = false;
var _CurrentStationFilter = new Array();
var _CurrentDmaFilter = new Array();
var _RequestDma = [];
var _OldRequestDma = [];
var _RequestAffiliate = [];
var _OldRequestAffiliate = [];
var _Country = null;
var _CountryName = '';
var _Region = null;
var _RegionName = '';
var _CurrentLogoIDFilter = new Array();
var _RequestStationID = [];
var _RequestStationIDDisplay = [];
var _OldRequestStationID = [];
var _CurrentStationIDFilter = [];
var _RequestLogoSearch = null;
var _RequestBrand = [];
var _RequestBrandName = [];
var _OldRequestBrand = [];
var _RequestIndustry = [];
var _RequestIndustryName = [];
var _OldRequestIndustry = [];
var _VisibleLRIndustries = [];
var _VisibleLRBrands = [];
var _RequestPE = '';
var _RequestLogoBrand = [];
var _SearchHasBeenMade = false;

var _IsManualHover = false;
var _HasRadioLoaded = false;

var _logoHits = [];
var _adHits = [];
var _searchLogoHits = [];
var filterToEarned = true;
var filterToPaid = true;
var brandOriginal = "";
$(document).ready(function () {

    //Chosen Select watchers:---------------------------------------:
    $(".chosen-select").chosen({
        display_disabled_options: true,
        default_item: "0",
        allow_single_deselect: true,
        disable_search_threshold: 5,
        search_contains: false,
        enable_split_word_search: false,
        width: "100%",
        height: "200%"
    });
    $('#tadsLogoSelect').on('change', function (evt, params) {
        if (params != null) {
            $("#divSearchLogoSideMenu").show();
            LoadSearchIDs(params.selected);
        } else {
            $("#divSearchLogoSideMenu").hide();
        }
    });
    brandOriginal = $("#tadsBrandSelect");
    $('#tadsBrandSelect').on('change', function (evt, params) {
        var brandArray = params.selected.split(":");
        SetBrand(brandArray[0], brandArray[1]);
    });
    $('#tadsIndustrySelect').on('change', function (evt, params) {
        if (params.deselected == null) {
            var industryArray = params.selected.split(":");
            SetIndustry(industryArray[0], industryArray[1], "select");
        } else {
            var deselected = params.deselected.split(":");
            SetIndustry(deselected[0], deselected[1], "deselect");
        }
    });
    $('#tadsStationSelect').on('change', function (evt, params) {
        if (params.deselected == null) {
            var stationArray = params.selected.split(":");
            SetStation(stationArray[0], stationArray[1], "select");
        } else {
            var deselected = params.deselected.split(":");
            SetStation(deselected[0], deselected[1], "deselect");
        }
    });
    $('#tadsAffiliateSelect').on('change', function (evt, params) {
        if (params.deselected == null) {
            SetAffiliate(params.selected, "select");
        } else {
            SetAffiliate(params.selected, "deselect");
        }
    });
    $('#tadsMarketSelect').on('change', function (evt, params) {
        if (params.deselected == null) {
            SetDma(params.selected, "select");
        } else {
            SetDma(params.deselected, "deselect");
        }
    });
    $('#radioMarketSelect').on('change', function (evt, params) {
        SetMarket(params.selected);
    });
    $('#radioStationSelect').on('change', function (evt, params) {
        SetRadioStation(params.selected);
    });
    //---------------------------------------------------------- end of chosen select

    $('#ulMainMenu li').removeAttr("class");
    $('#liMenuTAds').attr("class", "active");

    var documentHeight = $(window).height();
    $("#divTVScrollContent").css({ "height": documentHeight - 250 });
    $("#divRadioScrollContent").css({ "height": documentHeight - 250 });

    $('#mCSB_1').css({ 'max-height': '' });
    $('#mCSB_2').css({ 'max-height': '' });

    $('#divMessage').html('');
    $('#dpFrom').val('');
    $('#dpTo').val('');

    $("#divCalender").datepicker({
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
            $('#dpFrom').focus();
            SetDateVariable();
        }
    });

    $("#dpTo").datepicker({
        showOn: "button",
        buttonImage: "../images/calender.png",
        buttonImageOnly: true,
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            $('#dpTo').val(dateText);
            SetDateVariable();
        },
        onClose: function (dateText) {
            $('#dpTo').focus();
            SetDateVariable();
        }
    });

    $("#txtKeyword").keypress(function (e) {
        if (e.keyCode == 13) {
            SetKeyword();

        }
    });
    $("#txtKeyword").blur(function () {
        SetKeyword();
    });
    $("#imgKeyword").click(function (e) {
        SetKeyword();
    });

    $("#txtTitle").keypress(function (e) {
        if (e.keyCode == 13) {
            SetTitle();

        }
    });
    $("#txtTitle").blur(function () {
        SetTitle();
    });
    $("#imgKeyword").click(function (e) {
        SetTitle();
    });


    $("#divTVScrollContent").mCustomScrollbar({
        advanced: {
            updateOnContentResize: true,
            autoScrollOnFocus: false
        }
    });
    $("#divRadioScrollContent").mCustomScrollbar({
        advanced: {
            updateOnContentResize: true,
            autoScrollOnFocus: false
        }
    });
    $("#dpFrom").datepicker("setDate", new Date(new Date().setMonth(new Date().getMonth() - 3)));
    $("#dpTo").datepicker("setDate", new Date());
    _FromDate = $("#dpFrom").val();
    _ToDate = $("#dpTo").val();
    SetActiveFilter();
    GetSavedSearch(false, true)
});


//SIDE MENU TOGGLE:-------------------------------------------------------------------------------------------------
var closedAllToggleItems = false;
$(document).mouseup(function (e) {
    if (e.target.className != "search-choice-close" && e.target.className != "chosen-results") {
        closeOnClickOff();
        closedAllToggleItems = true;
    }
});
var toggleDiv = function (item) {
    var element = $("#" + item);
    closeOnClickOff();
    if (_SearchHasBeenMade == false && $(element)[0] != undefined) {
        element.show();
        //used to open jquery's chosen select full dropdown on click
        var selectDropdown = $(element)[0].firstElementChild.id
        $('#' + selectDropdown).trigger('chosen:open');
    } else { _SearchHasBeenMade = false; }
}
var closeOnClickOff = function () {
    //this finds all the individual/toggle-able options in your side menu and combines them into a list of children
    var narrowResultsUl = $('#narrowResultsMenu > li > ul');
    var narrowResultsDiv = $('#narrowResultsMenu > li > div');
    var logoFilterDiv = $('#logoFilterMenu > li > div');
    var adFilterUl = $('#adFilterMenu > li > ul');
    var radioMenu = $('#narrowResultsRadioMenu > li > div')
    var allChildren = narrowResultsUl.add(narrowResultsDiv.add(logoFilterDiv.add(adFilterUl.add(radioMenu))));
    //loop through your children and hide them all
    if (closedAllToggleItems == false) {
        allChildren.each(function (index) {
            $(this).hide();
        });
    } else {
        closedAllToggleItems = false;
    }
}
//----------------------------------------------------------------------------------------------------------------
function CorrectTargetName(e) {
    var e = window.event || e;
    var targ = e.target || e.srcElement;
    return targ.name == "brandLogoItem";
}

function PopulateDefaultLogos(result) {
    if (result != null) {
        result = result.substring(3);
        result = result.substring(0, result.length - 2);
        result = result.replace(/&quot;/g, '"');
        _logoHits = JSON.parse(result);
    }
}
function PopulateDefaultAds(result) {
    if (result != null) {
        result = result.substring(3);
        result = result.substring(0, result.length - 2);
        result = result.replace(/&quot;/g, '"');
        _adHits = JSON.parse(result);
    }
}

function LoadSearchIDs(brandID) {
    var iqLogolist =

            '<li role="presentation" style="padding: 0px;">' +
                '<ul role="menu" class="sideMenu sub-submenu" id="ulSearchLogo" style="text-align:center;">';

    var count = 0;
    var possibleLogos = [];

    possibleLogos = $.grep(_CurrentLogoIDFilter, function (e) { return e.BrandId == brandID; });
    $.each(possibleLogos, function (index, obj) {
        if (obj != null) {
            count++;
            iqLogolist += "<li role=\"presentation\" class=\"cursorPointer\" onclick=\"SetLogo(" + obj.ID + ");\"><img src='" + EscapeHTML(obj.URL) + "' title='" + EscapeHTML(obj.Name) + "'/></li>";
        }
    });

    if (count == 0) {
        iqLogolist += '<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">No Filter Available</a></li>';
    }
    $("#divSearchLogoSideMenu").html(iqLogolist + "</ul></li>");
    _RequestLogoBrand = [];
    _RequestLogoBrand.push(brandID);
}

$(window).resize(function () {
    if (screen.height >= 768) {
        $("#divTVScrollContent").css({ "height": documentHeight - 250 });
        $("#divRadioScrollContent").css({ "height": documentHeight - 250 });
    }
});

function CollapseExpandLeftSection(sectionid) {

    if (sectionid == 1) {

        $("#divTVSection").show("2000");
        $("#divTVContent").show("2000");

        $("#divRadioSection").hide("2000");
        $("#divRadioContent").hide("200");


        $("#h5TV").removeAttr("class");
        $("#h5TV").addClass("tvheader-active");

        $("#h5Radio").removeAttr("class");
        $("#h5Radio").addClass("radioheader-inactive");

        setTimeout(function () { $("#divTVScrollContent").mCustomScrollbar("scrollTo", "top"); }, 200);
    }
    else {

        $("#divTVSection").hide("2000");
        $("#divTVContent").hide("2000");

        $("#divRadioSection").show("2000");
        $("#divRadioContent").show("200");

        $("#h5Radio").removeAttr("class");
        $("#h5Radio").addClass("radioheader-active");

        $("#h5TV").removeAttr("class");
        $("#h5TV").addClass("tvheader-inactive");

        if (!_HasRadioLoaded) {
            RefreshRadioResults(false, false);
            _HasRadioLoaded = true;
        }
        else {
            setTimeout(function () { $("#divRadioScrollContent").mCustomScrollbar("scrollTo", "top"); }, 200);
        }
    }
}

function SetKeyword() {
    if ($("#txtKeyword").val() != "" && _SearchTerm != $("#txtKeyword").val()) {
        _SearchTerm = $("#txtKeyword").val();
        _IsDefaultLoad = false;
        SearchResult();
    }
}

function SetTitle() {
    if ($("#txtTitle").val() != "" && _Title != $("#txtTitle").val()) {
        _Title = $("#txtTitle").val();
        _IsDefaultLoad = false;
        SearchResult();
    }
}

function SetDateVariable() {
    //do not get results from before 1/1/16
    var arrDate = $("#dpFrom").val().split("/");
    var fromDateFormat = new Date(arrDate[2], arrDate[0] - 1, arrDate[1]);
    var arrDate2 = $("#dpTo").val().split("/");
    var toDateFormat = new Date(arrDate2[2], arrDate2[0] - 1, arrDate2[1]);

    if ($("#dpFrom").val() == "") {
        $("#dpFrom").datepicker("setDate", new Date("January 1, 2016 00:00:00"));
    }
    else if (fromDateFormat <= new Date("December 31, 2015 23:59:59")) {
        $("#dpFrom").datepicker("setDate", new Date("January 1, 2016 00:00:00"));
    }

    if ($("#dpTo").val() == "") {
        $("#dpTo").datepicker("setDate", new Date());
    }
    else if (toDateFormat <= new Date("December 31, 2015 23:59:59")) {
        $("#dpTo").datepicker("setDate", new Date("January 1, 2016 00:00:00"));
    }

    if ($("#dpFrom").val() && $("#dpTo").val()) {
        $('#dpFrom').removeClass('warningInput');
        $('#dpTo').removeClass('warningInput');
        if (_FromDate != $("#dpFrom").val() || _ToDate != $("#dpTo").val()) {
            _FromDate = $("#dpFrom").val();
            _ToDate = $("#dpTo").val();
            _IsDefaultLoad = false;
            SearchResult();
            $('#ulCalender').parent().removeClass('open');
            $('#aDuration').html('Custom&nbsp;&nbsp;<span class="caret"></span>');
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

function SetDma(dmaname, status) {
    if (status == "select") {
        _RequestDma.push(dmaname);
        $('#divMarketSideMenu').show();
    }
    else {
        _RequestDma.splice(dmaname, 1);
        if (_RequestDma.length == 0) {
            $('#divMarketSideMenu').hide();
        };
    };
}
function SearchDma() {
    if ($(_RequestDma).not(_OldRequestDma).length != 0 || $(_OldRequestDma).not(_RequestDma).length != 0) {
        _OldRequestDma = _RequestDma.slice(0);
        _IsDefaultLoad = false;
        SearchResult();
    }
}
function SetStation(stationID, stationName, status) {
    if (status == "select") {
        _RequestStationID.push(stationID);
        _RequestStationIDDisplay.push(stationName);
        $("#divStationSideMenu").show();
    }
    if (status == "deselect") {
        _RequestStationID.splice(_RequestStationID.indexOf(stationID), 1);
        _RequestStationIDDisplay.splice(_RequestStationIDDisplay.indexOf(stationName), 1);
        if (_RequestStationID.length == 0) {
            $("#divStationSideMenu").hide();
        };
    };
}
function SearchStation() {
    if ($(_RequestStationID).not(_OldRequestStationID).length != 0 || $(_OldRequestStationID).not(_RequestStationID).length != 0) {
        _OldRequestStationID = _RequestStationID.slice(0);
        _IsDefaultLoad = false;
        SearchResult();
    }
}
function SetAffiliate(affiliateName, status) {
    if (status == "select") {
        _RequestAffiliate.push(affiliateName);
        $("#divAffiliateSideMenu").show();
    }
    if (status == "deselect") {
        _RequestAffiliate.splice(_RequestAffiliate.indexOf(affiliateName), 1);
        if (_RequestAffiliate.length == 0) {
            $("#divAffiliateSideMenu").hide();
        };
    };
}
function SearchAffiliate() {
    if ($(_RequestAffiliate).not(_OldRequestAffiliate).length != 0 || $(_OldRequestAffiliate).not(_RequestAffiliate).length != 0) {
        _OldRequestAffiliate = _RequestAffiliate.slice(0);
        _IsDefaultLoad = false;
        SearchResult();
    }
}

function SetClass(classnum, classname) {
    _Class = classnum;
    _ClassName = classname;
    _IsDefaultLoad = false;
    SearchResult();
}

function SetCountry(countrynum, countryname) {
    _Country = countrynum;
    _CountryName = countryname;
    _IsDefaultLoad = false;
    SearchResult();
}

function SetRegion(regionnum, regionname) {
    _Region = regionnum;
    _RegionName = regionname;
    _IsDefaultLoad = false;
    SearchResult();
}

function SetLogo(id) {
    _RequestLogoSearch = id;
    _IsDefaultLoad = false;
    $('#ulSearchLogoMain').hide();
    SearchResult();
}

function SetBrand(id, eleName) {
    _RequestBrandName = [];
    _RequestBrand = [];
    _RequestBrandName.push(eleName);
    _RequestBrand.push(id.toString());
    SearchBrand();
}
function SearchBrand() {
    if ($(_RequestBrand).not(_OldRequestBrand).length != 0 || $(_OldRequestBrand).not(_RequestBrand).length != 0) {
        _OldRequestBrand = _RequestBrand.slice(0);
        _IsDefaultLoad = false;
        $('#ulBrandMain').hide();
        SearchResult();
    }
}
function SetIndustry(id, eleName, status) {
    if (status == "select") {
        _RequestIndustry.push(id.toString());
        _RequestIndustryName.push(eleName);
        $("#divIndustrySideMenu").show();
        }
    if (status == "deselect") {
        _RequestIndustry.splice(_RequestIndustry.indexOf(id.toString()), 1);
        _RequestIndustryName.splice(_RequestIndustryName.indexOf(eleName), 1);
        if (_RequestIndustryName.length == 0) {
            $("#divIndustrySideMenu").hide();
        };
    };
}
function SearchIndustry() {
    if ($(_RequestIndustry).not(_OldRequestIndustry).length != 0 || $(_OldRequestIndustry).not(_RequestIndustry).length != 0) {
        _OldRequestIndustry = _RequestIndustry.slice(0);
        _IsDefaultLoad = false;
        $('#ulIndustryMain').hide();
        SearchResult();
    }
}

function SetPE(type) {
    if (_RequestPE != type) {
        if (type == "Earned") {
            filterToEarned = true;
            filterToPaid = false;
        }
        else if (type == "Paid") {
            filterToEarned = false;
            filterToPaid = true;
        }
        _RequestPE = type;
        _IsDefaultLoad = false;
        SearchResult();
    }
}

function SearchResult() {
    var jsonPostData = {
        p_FromDate: _FromDate,
        p_ToDate: _ToDate,
        p_SearchTerm: _SearchTerm,
        p_Title: _Title,
        p_Dma: _RequestDma,
        p_Station: _RequestAffiliate,
        p_IQStationID: _RequestStationID,
        p_Class: _Class,
        p_Region: _Region,
        p_Country: _Country,
        p_IsAsc: _IsAsc,
        p_SortColumn: _SortColumn,
        p_IsDefaultLoad: _IsDefaultLoad,
        p_SearchLogo: _RequestLogoSearch,
        p_Brand: _RequestBrand,
        p_Industry: _RequestIndustry,
        p_PaidEarned: _RequestPE
    }

    // alert('searchcalled');
    if (DateValidation()) {
        SetActiveFilter();

        if (isFilterEnable) {
            $('#divTAdsClearAll').show();
        }
        else {
            $('#divTAdsClearAll').hide();
        }

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlTAdsSearchResults,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            success: OnResultSearchComplete,
            error: OnFail
        });
    }
    //needed for toggle function - close on search
    _SearchHasBeenMade = true;
    setTimeout(function () { toggleDiv() }, 1000);

}
function SearchResultPaging(isNextPage) {
    // alert(isNextPage);
    _IsNext = isNextPage;
    var jsonPostData = {
        p_FromDate: _FromDate,
        p_ToDate: _ToDate,
        p_SearchTerm: _SearchTerm,
        p_Title: _Title,
        p_Dma: _RequestDma,
        p_Station: _RequestAffiliate,
        p_IQStationID: _RequestStationID,
        p_Class: _Class,
        p_Region: _Region,
        p_Country: _Country,
        p_IsAsc: _IsAsc,
        p_IsNext: _IsNext,
        p_SortColumn: _SortColumn,
        p_SearchLogo: _RequestLogoSearch,
        p_Brand: _RequestBrand,
        p_Industry: _RequestIndustry,
        p_PaidEarned: _RequestPE
    }

    // alert('searchcalled');
    if (DateValidation()) {
        SetActiveFilter();
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlTAdsSearchResultsPaging,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            success: OnResultSearchComplete,
            error: OnFail
        });
    }


}
function OnResultSearchComplete(result) {
    if (result.isSuccess) {
        $("#divPreviousNext").show();
        $('#lblRecords').html('');
        $('#ulTAdsResults').html('');
        $('#ulTAdsResults').html(result.HTML);
        if (result.hasMoreResult) {
            $('#btnNextPage').show();
        }
        else {
            $('#btnNextPage').hide();
        }

        if (result.hasPreviouResult) {
            $('#btnPreviousPage').show();
        }
        else {
            $('#btnPreviousPage').hide();
        }
        if (result.recordNumber != '') {
            $('#lblRecords').html(result.recordNumber);
        }

        if (result.filters != null) {
            //dma/market
            var iqdmalist = "";
            if (result.filters.TadsDmas != null) {
                $.each(result.filters.TadsDmas, function (index, obj) {
                    var selected = "";
                    if (_RequestDma.includes(obj.Name)) { selected = "selected"; }
                    iqdmalist = iqdmalist + '<option ' + selected + ' value=\"' + obj.Name + '\">' + obj.Name + '(' + obj.CountFormatted + ')</option>';
                });
            } else {
                iqdmalist += "<option value=\"0\" disabled=\"true\">No Filter Available</option>";
                $("#tadsMarketSelect").html(iqdmalist);
            }
            updateChosenSelect(iqdmalist, "#tadsMarketSelect");
            $('#ulMarketMain').hide();
            _CurrentDmaFilter = result.filters.TadsDmas;
            //affiliates
            var iqstationaflist = "";
            if (result.filters.TadsAffiliates != null) {
                $.each(result.filters.TadsAffiliates, function (index, obj) {
                    var selected = "";
                    if (_RequestAffiliate.includes(obj.Name)) { selected = "selected"; }
                    iqstationaflist = iqstationaflist + '<option ' + selected + ' value=\"' + obj.Name + '\">' + obj.Name + '(' + obj.CountFormatted + ')</option>';
                });
            } else {
                iqstationaflist += "<option value=\"0\" disabled=\"true\">No Filter Available</option>";
                $("#tadsAffiliateSelect").html(iqstationaflist);
            }
            updateChosenSelect(iqstationaflist, "#tadsAffiliateSelect");
            $("#ulNetwork").hide();
            _CurrentStationFilter = result.filters.TadsAffiliates;
            //stations
            var iqstationlist = "";
            if (result.filters.TadsStations != null) {
                $.each(result.filters.TadsStations, function (index, obj) {
                    var selected = "";
                    if (_RequestStationID.includes(obj.ID)) { selected = "selected"; }
                    var liStyle = "";
                    iqstationlist = iqstationlist + '<option ' + selected + ' value=\"' + obj.ID + ":" + obj.Name + '\">' + obj.Name + '(' + obj.CountFormatted + ')</option>';
                });
            } else {
                iqstationlist = "<option value=\"0\" disabled=\"true\">No Filter Available</option>";
                $("#tadsStationSelect").html(iqstationlist);
            }
            updateChosenSelect(iqstationlist, "#tadsStationSelect");
            $("#ulStationMain").hide();
            _CurrentStationIDFilter = result.filters.TadsStations;
            //country
            var iqcountrylist = "";
            if (result.filters.TadsCountries != null) {
                $.each(result.filters.TadsCountries, function (index, obj) {
                    iqcountrylist = iqcountrylist + "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetCountry(" + obj.ID + ",'" + obj.Name.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + obj.Name + " (" + obj.CountFormatted + ") </a></li>";
                });
                $("#ulCountry").hide();
            } else {
                iqcountrylist = "<li role=\"presentation\" class=\"cursorPointer\"><a tabindex=\"-1\" role=\"menuitem\">No Filter Available</a></li>";
            }
            $("#ulCountry").html(iqcountrylist);
            //region
            var iqregionlist = "";
            if (result.filters.TadsRegions != null) {
                $.each(result.filters.TadsRegions, function (index, obj) {
                    iqregionlist = iqregionlist + "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetRegion(" + obj.ID + ",'" + obj.Name.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + obj.Name + " (" + obj.CountFormatted + ") </a></li>";
                });
                $("#ulRegion").hide();
            } else {
                iqregionlist = "<li role=\"presentation\" class=\"cursorPointer\"><a tabindex=\"-1\" role=\"menuitem\">No Filter Available</a></li>";
            }
            $("#ulRegion").html(iqregionlist);
            //logos
            if (_RequestLogoSearch == null || _RequestLogoSearch.length == "") { _RequestLogoBrand = []; }
            var iqlogolist = "";
            if (result.filters.AllLogos != null) {
                useSearchLogoHits = true;
                iqlogolist = "<option value=\"0\"></option>";
                $.each(result.filters.AllBrands, function (index, obj) {
                    var selected = "";
                    if (_RequestLogoBrand.indexOf(obj.ID.toString()) != -1) { selected = "selected"; }
                    iqlogolist = iqlogolist + '<option ' + selected + ' value=\"' + obj.ID + '\">' + obj.Name + '</option>';
                });
            } else {
                iqlogolist += "<option value=\"0\" disabled=\"true\">No Filter Available</option>"
                $("#tadsLogoSelect").html(iqlogolist);
            }
            updateChosenSelect(iqlogolist, "#tadsLogoSelect");
            $("#divSearchLogoSideMenu").hide();
            $("#ulSearchLogoMain").hide();
            //brands
            var iqbrandslist = '<option value=\"0\"></option>';
            if (result.filters.AllBrands != null) {
                $.each(result.filters.AllBrands, function (index, obj) {
                    var selected = "";
                    if (_RequestBrand.includes(obj.ID.toString())) { selected = "selected"; }
                    iqbrandslist = iqbrandslist + '<option ' + selected + ' value=\"' + obj.ID + ":" + obj.Name.replace(/"/g, '&quot;') + '\">' + obj.Name + '</option>'
                });
            } else {
                iqbrandslist += "<option value=\"0\" disabled=\"true\">No Filter Available</option>"
                $("#tadsBrandSelect").html(iqbrandslist);
            }
            updateChosenSelect(iqbrandslist, "#tadsBrandSelect")
            $("#ulBrandMain").hide();
            //industries
            var iqindlist = '';
            if (result.filters.AllIndustries != null) {
                $.each(result.filters.AllIndustries, function (index, obj) {
                    var selected = ""
                    if (_RequestIndustry.includes(obj.ID.toString())) { selected = "selected" }
                    iqindlist = iqindlist + '<option ' + selected + ' value=\"' + obj.ID + ":" + obj.Name + '\">' + obj.Name + '</option>';
                });
            } else {
                iqindlist = "<option value=\"0\" disabled=\"true\">No Filter Available</option>"
                $("#tadsIndustrySelect").html(iqindlist);
            }
            updateChosenSelect(iqindlist, "#tadsIndustrySelect");
            $("#ulIndustryMain").hide();
            //classes/category
            var iqclasslist = "";
            if (result.filters.TadsClasses != null) {
                $.each(result.filters.TadsClasses, function (index, obj) {
                    iqclasslist = iqclasslist + "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetClass( '" + obj.ID + "','" + obj.Name.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + obj.Name + " (" + obj.CountFormatted + ") </a></li>";
                });
                $("#ulCategory").hide();
            } else {
                iqclasslist = "<li role=\"presentation\" class=\"cursorPointer\"><a tabindex=\"-1\" role=\"menuitem\">No Filter Available</a></li>";
            }
            $("#ulCategory").html(iqclasslist);


            //PAID/EARNED
            if (result.filters.TadsPaidEarned != null) {
                var paidList = "";
                var earnedList = "";
                if (filterToPaid && result.filters.TadsPaidEarned.Paid != null) {
                    paidList += "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetPE('Paid');\" tabindex=\"-1\" role=\"menuitem\">Paid</a></li>";
                    $("#ulPaid").html(paidList);
                    $("#ulEarned").html(earnedList);
                }
                if (filterToEarned && result.filters.TadsPaidEarned.Earned != null) {
                    earnedList += "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetPE('Earned');\" tabindex=\"-1\" role=\"menuitem\">Earned</a></li>";
                    $("#ulEarned").html(earnedList);
                    $("#ulPaid").html(paidList);
                }
            } else {
                var iqPElist = '<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">No Filter Available</a></li>';
                $("#ulPaid").html(iqPElist);
                $("#ulEarned").html("");
            }
        }
        //Logos and Ads inside video player.
        if (result.logoHits != null && result.logoHits.length > 0) _logoHits = result.logoHits;
        if (result.adHits != null && result.adHits.length > 0) _adHits = result.adHits;
    }
    else {
        ClearResultsOnError('ulTAdsResults', 'divPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "SearchResult()"));
    }
}

var updateChosenSelect = function (htmlLogos, selector) {
    //updates chosen select plugin for updated list of options.
    $(selector).empty();
    iqlogolist = $(htmlLogos);
    $(selector).append(htmlLogos);
    $(selector).trigger("chosen:updated");
}

function OnFail(result) {
    _IsTabChange = false;

    ShowNotification(_msgErrorOccured);
    ClearResultsOnError('ulTAdsResults', 'divPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "SearchResult()"));
}

function SortByName(a, b) {
    var aName = a.Name.toLowerCase();
    var bName = b.Name.toLowerCase();
    return ((aName < bName) ? -1 : ((aName > bName) ? 1 : 0));
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
function SetActiveFilter() {

    isFilterEnable = false;
    $("#divActiveFilter").html("");


    if (_SearchTerm != null && _SearchTerm != "") {
        $('#divActiveFilter').append('<div id="divKeywordActiveFilter" class="filter-in">' + EscapeHTML(_SearchTerm) + '<span class="cancel" onclick="RemoveFilter(1);"></span></div>');
        isFilterEnable = true;
    }

    if ((_FromDate != null && _FromDate != "") && (_ToDate != null && _ToDate != "")) {
        $('#divActiveFilter').append('<div id="divDateActiveFilter" class="filter-in">' + _FromDate + ' To ' + _ToDate + '<span class="cancel" onclick="RemoveFilter(2);"></span></div>');
        isFilterEnable = true;
    }
    if (_RequestDma != null && _RequestDma.length > 0) {
        $('#divActiveFilter').append('<div id="divDmaActiveFilter" class="filter-in">' + _RequestDma.join() + '<span class="cancel" onclick="RemoveFilter(3);"></span></div>');
        isFilterEnable = true;
    }
    if (_RequestAffiliate != null && _RequestAffiliate.length > 0) {
        $('#divActiveFilter').append('<div id="divStationActiveFilter" class="filter-in">' + _RequestAffiliate.join() + '<span class="cancel" onclick="RemoveFilter(4);"></span></div>');
        isFilterEnable = true;
    }
    if (_RequestStationIDDisplay != null && _RequestStationIDDisplay.length > 0) {
        $('#divActiveFilter').append('<div id="divStationIDActiveFilter" class="filter-in">' + _RequestStationIDDisplay.join() + '<span class="cancel" onclick="RemoveFilter(7);"></span></div>');
        isFilterEnable = true;
    }
    if (_Class != null && _Class != "") {
        $('#divActiveFilter').append('<div id="divClassActiveFilter" class="filter-in">' + _ClassName + '<span class="cancel" onclick="RemoveFilter(5);"></span></div>');
        isFilterEnable = true;
    }
    if (_Title != null && _Title != "") {
        $('#divActiveFilter').append('<div id="divTitleActiveFilter" class="filter-in">' + EscapeHTML(_Title) + '<span class="cancel" onclick="RemoveFilter(6);"></span></div>');
        isFilterEnable = true;
    }

    if (_Region != null && _Region != "") {
        $('#divActiveFilter').append('<div id="divRegionActiveFilter" class="filter-in">' + _RegionName + '<span class="cancel" onclick="RemoveFilter(8);"></span></div>');
        isFilterEnable = true;
    }
    if (_Country != null && _Country != "") {
        $('#divActiveFilter').append('<div id="divCountryActiveFilter" class="filter-in">' + _CountryName + '<span class="cancel" onclick="RemoveFilter(9);"></span></div>');
        isFilterEnable = true;
    }

    if (_RequestLogoSearch != null && _RequestLogoSearch != "") {
        $('#divActiveFilter').append('<div id="divLogoSearchActiveFilter" class="filter-in">Search Images<span class="cancel" onclick="RemoveFilter(10);"></span></div>');
        isFilterEnable = true;
    }
    if (_RequestBrand != null && _RequestBrand.length > 0) {
        var brand = '';
        $.each(_RequestBrandName, function (index, value) {
            brand += value;
            if (_RequestBrandName.length > 1) {
                brand += ", "
            }
        });

        $('#divActiveFilter').append('<div id="divBrandActiveFilter" class="filter-in">' + brand.replace(/"/g, '&quot;') + '<span class="cancel" onclick="RemoveFilter(11);"></span></div>');
        isFilterEnable = true;
    }

    if (_RequestIndustry != null && _RequestIndustry.length > 0) {
        var industry = '';
        $.each(_RequestIndustryName, function (index, value) {
            industry += value;
            if (_RequestIndustryName.length > 1) {
                industry += ", "
            }
        });

        $('#divActiveFilter').append('<div id="divIndustryActiveFilter" class="filter-in">' + industry + '<span class="cancel" onclick="RemoveFilter(13);"></span></div>');
        isFilterEnable = true;
    }
    if (_RequestPE != null && _RequestPE.length > 0) {
        $('#divActiveFilter').append('<div id="divPEActiveFilter" class="filter-in">' + _RequestPE + '<span class="cancel" onclick="RemoveFilter(14);"></span></div>');
        isFilterEnable = true;
    }

    if (isFilterEnable) {
        $("#divActiveFilter").css({ 'border-bottom': '1px solid rgb(236, 236, 236)', 'margin-bottom': '5px' });
    }
    else {
        $("#divActiveFilter").css({ 'border-bottom': '' });
    }
}

function RemoveFilter(filterType) {

    var refreshResults = false;

    _IsDefaultLoad = false;
    // Represent SearchKeyword
    if (filterType == 1) {

        $("#txtKeyword").val("");
        _SearchTerm = "";
        refreshResults = true;
    }

    // Represent Date filter(From Date,To Date)
    if (filterType == 2) {

        $("#dpFrom").datepicker("setDate", null);
        $("#dpTo").datepicker("setDate", null);
        $("#divCalender").datepicker("setDate", null);

        $('#aDuration').html(_msgAll + '&nbsp;&nbsp;<span class="caret"></span>');

        _FromDate = null;
        _ToDate = null;
        refreshResults = true;
    }

    // Represent Dma Filter
    if (filterType == 3) {
        _RequestDma = [];
        _OldRequestDma = [];
        refreshResults = true;
    }

    // Represent Affil Filter
    if (filterType == 4) {
        _RequestAffiliate = [];
        _OldRequestAffiliate = [];
        refreshResults = true;
    }
    // Represent Class Filter
    if (filterType == 5) {
        _Class = "";
        _ClassName = "";
        refreshResults = true;
    }

    if (filterType == 6) {

        $("#txtTitle").val("");
        _Title = "";
        refreshResults = true;
    }

    // Represent Station Filter
    if (filterType == 7) {
        _RequestStationID = [];
        _RequestStationIDDisplay = [];
        _OldRequestStationID = [];
        refreshResults = true;
    }

    if (filterType == 8) {
        _Region = null;
        _RegionName = "";
        refreshResults = true;
    }

    if (filterType == 9) {
        _Country = null;
        _CountryName = "";
        refreshResults = true;
    }

    // Represent Logo Search Filter
    if (filterType == 10) {
        _RequestLogoBrand = [];
        _RequestLogoSearch = [];
        refreshResults = true;
    }

    // Represent Brand Filter
    if (filterType == 11) {
        _OldRequestBrand = [];
        _RequestBrandName = [];
        _RequestBrand = [];
        refreshResults = true;
    }

    // Represent Industry Filter
    if (filterType == 13) {
        _OldRequestIndustry = [];
        _RequestIndustry = [];
        _RequestIndustryName = []
        refreshResults = true;
    }

    // Represent PE Filter
    if (filterType == 14) {
        _RequestPE = "";
        filterToEarned = true;
        filterToPaid = true;
        refreshResults = true;
    }

    if (refreshResults) {
        SearchResult();
    }
}
function SortDirection(p_SortColumn, p_IsAsc) {

    if (p_IsAsc != _IsAsc || _SortColumn != p_SortColumn) {
        _IsAsc = p_IsAsc;
        _SortColumn = p_SortColumn;

        if (_SortColumn == 'Date' && _IsAsc) {
            $('#aSortDirection').html(_msgOldestFirst + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (_SortColumn == 'Date' && !_IsAsc) {
            $('#aSortDirection').html(_msgMostRecent + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (_SortColumn == 'Market' && _IsAsc) {
            $('#aSortDirection').html(_msgMarketAscending + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (_SortColumn == 'Market' && !_IsAsc) {
            $('#aSortDirection').html(_msgMarketDescending + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        _IsDefaultLoad = false;
        SearchResult();
    }
}
function GetResultOnDuration(duration) {

    $("#dpFrom").removeClass("warningInput");
    $("#dpTo").removeClass("warningInput");
    var dtcurrent = new Date();
    var fDate;
    _Duration = duration;

    // All
    if (duration == 0) {
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
        else {
            if ($("#dpFrom").val() == "") {
                $("#dpFrom").addClass("warningInput");
            }
            if ($("#dpTo").val() == "") {
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
            _IsDefaultLoad = false;
            SearchResult();
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

function ClosePopUp(divID) {
    $('#' + divID).css({ "display": "none" });
    $('#' + divID).modal('hide');
}

function ClearSearch() {
    _FromDate = null;
    _ToDate = null;
    _SearchTerm = "";
    _RequestDma = [];
    _OldRequestDma = [];
    _RequestAffiliate = [];
    _OldRequestAffiliate = [];
    _RequestStationID = [];
    _RequestStationIDDisplay = [];
    _OldRequestStationID = [];
    _Class = "";
    _ClassName = "";
    _Country = null;
    _Region = null;
    _Title = "";
    _IsAsc = false;
    _SortColumn = "";
    _IsDefaultLoad = true;
    _RequestLogoSearch = [];
    _RequestBrand = [];
    _RequestBrandName = [];
    _RequestIndustry = [];
    _RequestIndustryName = [];
    _RequestPE = "";
    filterToEarned = true;
    filterToPaid = true;


    $('#aDuration').html(_msgAll + '&nbsp;&nbsp;<span class="caret"></span>');
    $("#dpFrom").datepicker("setDate", null);
    $("#dpTo").datepicker("setDate", null);
    $("#divCalender").datepicker("setDate", null);
    $("#txtTitle").val("");
    $("#txtKeyword").val("");

    SearchResult();
}

var dfdPlayerInfoLoaded = new $.Deferred();

function GetRawData(iqcckey) {
    var jsonPostData = {
        iqcckey: iqcckey
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlTAdsGetRawData,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {
                window.open(_urlTAdsGetRawDataXML);
                window.open(_urlTAdsGetRawDataTGZ);
            }
            else {
                ShowNotification("There was an error getting the files.");
            }
        },
        error: function (result) {
            ShowNotification("There was an error getting the files.");
        }
    });
}

function LoadChartNPlayer(rawMediaGuid, iqcckey, title) {
    dfdPlayerInfoLoaded = new $.Deferred();
    LoadPlayerbyGuidTS(rawMediaGuid, title);
    var logoHits = ParseLogoStrings(iqcckey);
    var adHits = ParseAdStrings(iqcckey);

    $.when(dfdPlayerInfoLoaded).then(
        function () {
            var jsonPostData = {
                IQ_CC_KEY: iqcckey,
                RAW_MEDIA_GUID: rawMediaGuid,
                sortByHitStart: true,
                lstSearchTermHits: _searchTermHits,
                lstLogoHitStrings: logoHits,
                lstAdHitStrings: adHits
            }

            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: _urlCommonGetMTChart,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(jsonPostData),
                success: OnGetChartComplete,
                error: OnGetChartFail
            });
        }
    );
}
function OnGetChartComplete(result) {
    if (result.isSuccess) {
        if (result.hasTAdsResults && result.tAdsResultsJson.length > 0) {
            $('.ads-chart-content').html('');
            $('.ads-chart-content').append('<div class="float-left" id="ads-results" style="width:100%; padding-right:20px; padding-left:163px; background-color: #FFFFFF""></div>');
            numOfVisibleLogos = result.yAxisCompanies.length;
            yAxisInfoList = [];
            $.each(result.yAxisCompanies, function (index, value) {
                var yAxisInfo = {};
                yAxisInfo.ID = [];
                yAxisInfo.Position = [];
                yAxisInfo.ID.push(index + 1);
                yAxisInfo.Position.push(index + 1);
                yAxisInfo.yAxisCompany = result.yAxisCompanies[index];
                yAxisInfo.yAxisLogoPath = result.yAxisLogoPaths[index];

                yAxisInfoList.push(yAxisInfo);
            });
            tAdsResultsJson = result.tAdsResultsJson;

            RenderHighCharts(result.tAdsResultsJson, 'ads-results');
        }
        else {
            $('.ads-chart-content').append('<table style="width:100%; background-color:#c0c0c0"><tr><td>There is no available data.</td></tr></table>');

        }
        if (result.isTimeSync && result.lineChartJson.length > 0) {
            $('.video-chart').closest('li').show();
            $('.chart-tabs').html('');
            $('.chart-tab-content').html('');
            $.each(result.lineChartJson, function (index, obj) {
                $('.chart-tabs').append('<div class="chartTabHeader" id="video-parent-tab-' + index + '"><div class="padding5" id="video-tab-' + index + '" onclick="changeChartTab(' + index + ');">' + obj.Type + '</div></div>');
                $('.chart-tab-content').append('<div class="float-left" id="video-chart-' + index + '" style="width:1020px;"></div>');
                RenderHighCharts(obj.Data, 'video-chart-' + index);
            });

            changeChartTab(0);
            $(".chart-tab-content").on("mouseout", function () {
                _IsManualHover = false;
            });
        }
    }
    else {
        ShowNotification(_msgErrorOccured);
    }
}
function OnGetChartFail(result) {
    ShowNotification(_msgErrorOccured);
}
function ParseLogoStrings(iqcckey) {
    var returnLogos = [];
    if (_logoHits.length > 0) {
        var logos = $.grep(_logoHits, function (e) { return iqcckey in e; });
        $.each(logos, function (index, value) {
            var logo = value[iqcckey];
            returnLogos.push(logo);
        });
    }

    return returnLogos;
}
function ParseAdStrings(iqcckey) {
    var returnAds = [];
    if (_adHits.length > 0) {
        var ads = $.grep(_adHits, function (e) { return iqcckey in e; });
        $.each(ads, function (index, value) {
            var date = value[iqcckey];
            returnAds.push(date);
        });
    }

    return returnAds;
}

function RenderHighCharts(jsonLineChartData, chartID) {

    var JsonLineChart = JSON.parse(jsonLineChartData);
    JsonLineChart.plotOptions.series.point.events.click = LineChartClick;
    JsonLineChart.xAxis.labels.formatter = FormatTime
    JsonLineChart.tooltip.formatter = tooltipFormat
    JsonLineChart.plotOptions.series.events.show = HandleSeriesShowHide;
    JsonLineChart.plotOptions.series.events.hide = HandleSeriesShowHide;
    JsonLineChart.plotOptions.series.point.events.mouseOver = ChartHoverManage;
    JsonLineChart.plotOptions.series.point.events.mouseOut = ChartHoverOutManage;

    $('#' + chartID).highcharts(JsonLineChart);
}

function HandleSeriesShowHide() {

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
            chart.tooltip.hide();
        }
    }

}

function tooltipFormat() {
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

function ChartHoverOutManage() {
    _IsManualHover = false;
    console.log("chart hover out");
}

function ChartHoverManage() {
    _IsManualHover = true;
    console.log("chart hover");
}


function LineChartClick() {
    setSeekPoint(this.category);
}

function changeChartTab(tabNumber) {

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

//-----------------------------------------------------------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------------------------------------------------------
function GetSavedSearch(p_isNext, p_isInitialize) {
    //$('#divSavedSearch').html(_imgLoading);

    var jsonPostData = {
        isNext: p_isNext,
        isInitialize: p_isInitialize
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlTAdsGetSaveSearch,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),
        global: false,
        success: OnGetSaveSearchComplete,
        error: OnGetSaveSearchFail
    });

    return false;
}

function OnGetSaveSearchComplete(result) {

    if (result.isSuccess) {
        SetSavedSearchHTML(result);
        $('#spnSavedSearchRecordDetail').html(result.saveSearchRecordDetail);
    }
    else {

        //CheckForAuthentication(result, 'Some error occured, try again later');
        $('#divSavedSearch').html('An error occured,<a class="cursorPointer" onclick="GetSavedSearch(false, true);">try again</a>');
        //ShowNotification('Some error occured, try again later');
    }
}

function OnGetSaveSearchFail(result) {
    //alert('GetSaveSearchFail - Local');
    //ShowNotification('Some error occured, try again later');
    $('#divSavedSearch').html('An error occured, <a class="cursorPointer" onclick="GetSavedSearch(false, true);">try again</a>');
}
function SetSavedSearchHTML(result) {
    $('#divSavedSearch').removeClass('text-align-center');
    $('#divSavedSearch').html(result.HTML);

    if (result.isPreviousAvailable) {
        $('#aSavedSearchPrevious').attr("onclick", "GetSavedSearch(false,false);");
        $('#aSavedSearchPrevious').show(); //  removeClass("inactiveLink");
    }
    else {
        $('#aSavedSearchPrevious').removeAttr("onclick");
        $('#aSavedSearchPrevious').hide(); // addClass("inactiveLink");
    }


    if (result.HasMoreResult) {
        $('#aSavedSearchNext').attr("onclick", "GetSavedSearch(true,false);");
        $('#aSavedSearchNext').show(); //  removeClass("inactiveLink");
    }
    else {
        $('#aSavedSearchNext').removeAttr("onclick");
        $('#aSavedSearchNext').hide(); //  addClass("inactiveLink");
    }
}
function ShowSaveSearchTads() {

    $('#divPopover').remove();
    $('#aSaveSearch').popover({
        trigger: 'manual',
        html: true,
        title: '',
        placement: 'right',
        template: '<div id="divPopover" class="popover"><div class="arrow"></div><div class="popover-inner"><div class="popover-content"><p></p></div></div></div>',
        content: '<input type="text" placeholder="Save Search Title" id="txtSaveSearchPopup" /><div><input type="button"  class="cancelButton marginbottom0"  style="margin-top:0px !important;" value="Cancel"  onclick="$(\'#divPopover\').remove();" /><input type="button" id="btnSaveSearch" class="button marginbottom0" style="margin-left:10px !important;margin-top:0px !important;" value="Submit" onclick="SaveSearch();" /><img src="../../Images/Loading_1.gif" class="displayNone" id="imgSaveSearchLoading" /></div>'
    });

    $('#aSaveSearch').popover('show');
}

function ValidateSaveSearchInput() {
    var isValid = true;

    if ($('#txtSaveSearchPopup').val().trim() == '') {

        $('#txtSaveSearchPopup').css('border', '1px red solid');
        $('#txtSaveSearchPopup').addClass('boxshadow');

        setTimeout(function () {
            $('#txtSaveSearchPopup').removeClass('boxshadow');
        }, 2000);

        ShowNotification(_msgEnterSaveSearchTitle);

        isValid = false;
    }
    return isValid;
}

function SaveSearch() {
    if (ValidateSaveSearchInput()) {
        $('#imgSaveSearchLoading').show();

        var ListIQ_Station = new Array()
        $.each(_RequestStationID, function (index, obj) {
            ListIQ_Station.push({ Station_Call_Sign: _RequestStationIDDisplay[index], IQ_Station_ID: obj });
        });

        var jsonPostData = {
            p_Title: $('#txtSaveSearchPopup').val().trim(),
            p_SearchTerm: {
                SearchTerm: $('#txtKeyword').val().trim(),
                Title120: $('#txtTitle').val().trim(),
                FromDate: _FromDate,
                ToDate: _ToDate,
                Dma: _RequestDma,
                Affiliate: _RequestAffiliate,
                IQStationID: _RequestStationID,
                Category: _Class,
                CategoryName: _ClassName,
                Region: _Region,
                RegionName: _RegionName,
                Country: _Country,
                CountryName: _CountryName,
                IndustryID: _RequestIndustry,
                IndustryName: _RequestIndustryName,
                BrandID: _RequestBrand,
                BrandName: _RequestBrandName,
                Logo: _RequestLogoSearch,
                PaidEarned: _RequestPE
            }
        }

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlTadsSaveSearch,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),

            success: OnSaveSearchComplete,
            error: OnSaveSearchFail
        });
    }
}

function OnSaveSearchComplete(result) {
    $('#imgSaveSearchLoading').hide();
    $('#divPopover').remove();
    if (result.isSuccess) {
        ShowNotification(result.message);
        GetSavedSearch(false, true);
    }
    else {
        CheckForAuthentication(result, _msgErrorOccured);
        //ShowNotification('Some error occured, try again later');
    }
}

function OnSaveSearchFail(result) {
    $('#imgSaveSearchLoading').hide();
    $('#divPopover').remove();
    //alert('On Save Search Fail - Local');
    ShowNotification(_msgErrorOccured);
}



function DeleteSavedSearchByID(ID) {
    var jsonPostData = {
        p_ID: ID
    }

    getConfirm("Delete Saved Search", _msgConfirmSavedSearchDelete, "Confirm Delete", "Cancel", function (res) {
        if (res) {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: _urlTadsDeleteSavedSearchByID,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(jsonPostData),

                success: OnDeleteSaveSearchComplete,
                error: OnDeleteSaveSearchFail
            });
        }
    });
}

function OnDeleteSaveSearchComplete(result) {
    if (result.isSuccess) {
        ShowNotification(result.message);
        setTimeout(function () {
            GetSavedSearch(false, true);
        }, 1);
    }
    else {
        CheckForAuthentication(result, _msgErrorOccured);
        //ShowNotification('Some error occured, try again later');
    }
}

function OnDeleteSaveSearchFail(result) {
    //alert('OnDeleteSaveSearchFail - Local');
    ShowNotification(_msgErrorOccured);
}

function LoadSavedSearch(ID) {
    var jsonPostData = {
        p_ID: ID
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlTadsLoadSavedSearch,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),

        success: OnLoadSaveSearchComplete,
        error: OnLoadSaveSearchFail
    });
}

function OnLoadSaveSearchComplete(result) {
    if (result.isSuccess) {
        //date,brand,searchterm,title,market,station,affiliate
        _FromDate = result.tads_SavedSearch.SearchTerm.FromDate;
        _ToDate = result.tads_SavedSearch.SearchTerm.ToDate;
        _SearchTerm = result.tads_SavedSearch.SearchTerm.SearchTerm;
        _Title = result.tads_SavedSearch.SearchTerm.Title120;
        _RequestDma = result.tads_SavedSearch.SearchTerm.Dma;
        _RequestAffiliate = result.tads_SavedSearch.SearchTerm.Affiliate;
        _RequestStationIDDisplay = result.tads_SavedSearch.SearchTerm.IQStationID;

        //category,country,region,logo, brand, paid/earned, industry
        _Class = result.tads_SavedSearch.SearchTerm.Category;
        _ClassName = result.tads_SavedSearch.SearchTerm.CategoryName;
        _Country = result.tads_SavedSearch.SearchTerm.Country;
        _CountryName = result.tads_SavedSearch.SearchTerm.CountryName;
        _Region = result.tads_SavedSearch.SearchTerm.Region;
        _RegionName = result.tads_SavedSearch.SearchTerm.RegionName;
        _RequestLogoSearch = result.tads_SavedSearch.SearchTerm.Logo;
        _RequestBrand = result.tads_SavedSearch.SearchTerm.BrandID;
        _RequestBrandName = result.tads_SavedSearch.SearchTerm.BrandName;
        _RequestPE = result.tads_SavedSearch.SearchTerm.PaidEarned;
        _RequestIndustry = result.tads_SavedSearch.SearchTerm.IndustryID;
        _RequestIndustryName = result.tads_SavedSearch.SearchTerm.IndustryName;

        $('#txtKeyword').val(_SearchTerm);
        $('#txtTitle').val(_Title);
        $("#dpFrom").datepicker("setDate", _FromDate);
        $("#dpTo").datepicker("setDate", _ToDate);

        SetSavedSearchHTML(result);
        $('#spnSavedSearchRecordDetail').html(result.saveSearchRecordDetail);
        _IsDefaultLoad = false;
        SearchResult();
    }
    else {
        ShowNotification(_msgErrorOccured);
    }
}

function OnLoadSaveSearchFail(result) {
    ShowNotification(_msgErrorOccured);
}

function OpenSavedSearchPopup(p_id, p_title) {
    $('#txtSavedSearchTitle').removeClass('boxshadow');
    $('#txtSavedSearchTitle').css("border", "");
    $('#spnUpdateSavedSearchPopupNote').html('');
    $('#txtSavedSearchTitle').val(p_title);
    $('#hdnSavedSearchID').val(p_id);
    $('#divUpdateSavedSearchPopup').modal({
        backdrop: 'static',
        keyboard: true,
        dynamic: true
    });
}
function UpdateTimeshiftSavedSearch() {
    if ($('#txtSavedSearchTitle').val().trim() != '') {
        $('#imgUpdateSaveSearchLoading').show();

        var ListIQ_Station = new Array()
        $.each(_RequestStationID, function (index, obj) {
            ListIQ_Station.push({ Station_Call_Sign: _RequestStationIDDisplay[index], IQ_Station_ID: obj });
        });

        var jsonPostData = {
            p_ID: $('#hdnSavedSearchID').val(),
            p_Title: $('#txtSavedSearchTitle').val().trim(),
            p_SearchTerm: {
                SearchTerm: $('#txtKeyword').val().trim(),
                Title120: $('#txtTitle').val().trim(),            
                FromDate: _FromDate,
                ToDate: _ToDate,
                Dma: _RequestDma,
                Affiliate: _RequestAffiliate,
                IQStationID: _RequestStationID,
                Category: _Class,
                CategoryName: _ClassName,
                Region: _Region,
                RegionName: _RegionName,
                Country: _Country,
                CountryName: _CountryName,
                IndustryID: _RequestIndustry,
                IndustryName: _RequestIndustryName,
                BrandID: _RequestBrand,
                BrandName: _RequestBrandName,
                Logo: _RequestLogoSearch,
                PaidEarned: _RequestPE
            }
        }

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlTadsUpdateSavedSearch,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                $('#imgUpdateSaveSearchLoading').hide();
                if (result.isError) {
                    ClosePopUp('divUpdateSavedSearchPopup')
                    CheckForAuthentication(result, _msgErrorOccured);
                }
                else if (result.isSuccess) {
                    ClosePopUp('divUpdateSavedSearchPopup')
                    ShowNotification(result.message);
                    GetSavedSearch(false, true);
                }
                else {
                    $('#spnUpdateSavedSearchPopupNote').html(result.message)
                }
            },
            error: function (a, b, c) {
                $('#imgUpdateSaveSearchLoading').hide();
                ShowNotification(_msgErrorOccured);
            }
        });
    }
    else {
        $('#txtSavedSearchTitle').css('border', '1px red solid');
        $('#txtSavedSearchTitle').addClass('boxshadow');

        setTimeout(function () {
            $('#txtSavedSearchTitle').removeClass('boxshadow');
        }, 2000);
    }
}
