var _FromDate = null;
var _ToDate = null;
var _Class = "";
var _Date = null;
var _IsAsc = false;
var _Duration = null;
var _IsNext = false;
var _IsMarketSort = false;
var isFilterEnable = false;
var _CurrentStationFilter = new Array();
var _CurrentDmaFilter = new Array();
var _CurrentLogoFilter = new Array();
var _RequestDma = [];
var _OldRequestDma = [];
var _RequestStation = [];
var _OldRequestStation = [];
var _RequestLogo = [];
var _OldRequestLogo = [];
var _RequestLogoDisplay = [];
var _Country = null;
var _CountryName = '';
var _Region = null;
var _RegionName = '';
var _Title = '';
var _SearchTerm = '';

var _RequestStationID = [];
var _RequestStationIDDisplay = [];
var _OldRequestStationID = [];
var _CurrentStationIDFilter = [];
var _RequestIndustry = [];
var _CurrentIndustryFilter = [];
var _OldRequestIndustry = [];
var _RequestBrand = [];
var _CurrentBrandFilter = [];
var _OldRequestBrand = [];

var _IsManualHover = false;

$(document).ready(function () {

    $('#ulMainMenu li').removeAttr("class");
    $('#liMenuImagiQ').attr("class", "active");

    //SearchResult();  

    var documentHeight = $(window).height();
    $("#divTVScrollContent").css({ "height": documentHeight - 250 });

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

    $("#divTVScrollContent").mCustomScrollbar({
        advanced: {
            updateOnContentResize: true,
            autoScrollOnFocus: false
        }
    });

    $("body").click(function (e) {
        if (e.target.id == "liDmaFilter" || $(e.target).parents("#liDmaFilter").size() > 0) {
            if ($('#ulMarket').is(':visible')) {
                $('#ulMarket').hide();
            }
            else {
                $('#ulMarket').show();
            }
        }
        else if ((e.target.id !== "liDmaFilter" && e.target.id !== "ulMarket" && $(e.target).parents("#ulMarket").size() <= 0) || e.target.id == "btnSearchDma") {
            $('#ulMarket').hide();
            if (e.target.id != "btnSearchDma") {
                var iqdmalist = "";
                $.each(_CurrentDmaFilter, function (eventID, eventData) {
                    if (_OldRequestDma.length > 0 && $.inArray(eventData.Name, _OldRequestDma) !== -1 && $.inArray(eventData.Name, _RequestDma) == -1) {
                        _RequestDma.push(eventData.Name);
                    }
                    var liStyle = "";
                    if ($.inArray(eventData.Name, _RequestDma) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    iqdmalist = iqdmalist + "<li role=\"presentation\" class=\"cursorPointer\"><a " + liStyle + " onclick=\"SetDma(this,'" + eventData.Name.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + eventData.Name + "</a></li>";
                });

                if (iqdmalist != "") {
                    $('#ulMarketList').html(iqdmalist);
                    $('#liDMASearch').show();
                }
                else {
                    $('#ulMarketList').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
                    $('#liDMASearch').hide();
                }
            }
        }

        if (e.target.id == "liStationFilter" || $(e.target).parents("#liStationFilter").size() > 0) {
            if ($('#ulNetwork').is(':visible')) {
                $('#ulNetwork').hide();
            }
            else {
                $('#ulNetwork').show();
            }
        }
        else if ((e.target.id !== "liStationFilter" && e.target.id !== "ulNetwork" && $(e.target).parents("#ulNetwork").size() <= 0) || e.target.id == "btnSearchStation") {
            $('#ulNetwork').hide();
            if (e.target.id != "btnSearchStation") {
                var iqstationlist = "";
                $.each(_CurrentStationFilter, function (eventID, eventData) {
                    if (_OldRequestStation.length > 0 && $.inArray(eventData.Name, _OldRequestStation) !== -1 && $.inArray(eventData.Name, _RequestStation) == -1) {
                        _RequestStation.push(eventData.Name);
                    }
                    var liStyle = "";
                    if ($.inArray(eventData.Name, _RequestStation) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    iqstationlist = iqstationlist + "<li role=\"presentation\" class=\"cursorPointer\"><a " + liStyle + " onclick=\"SetStation(this,'" + eventData.Name.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + eventData.Name + "</a></li>";
                });

                if (iqstationlist != "") {
                    $('#ulNetworkList').html(iqstationlist);
                    $('#liStationSearch').show();
                }
                else {
                    $('#ulNetworkList').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
                    $('#liStationSearch').hide();
                }
            }
        }


        if (e.target.id == "liStationIDFilter" || $(e.target).parents("#liStationIDFilter").size() > 0) {
            if ($('#ulStation').is(':visible')) {
                $('#ulStation').hide();
            }
            else {
                $('#ulStation').show();
            }
        }
        else if ((e.target.id !== "liStationIDFilter" && e.target.id !== "ulStation" && $(e.target).parents("#ulStation").size() <= 0) || e.target.id == "btnSearchStationID") {
            $('#ulStation').hide();
            if (e.target.id != "btnSearchStationID") {
                var iqstationlist = "";
                $.each(_CurrentStationIDFilter, function (eventID, eventData) {
                    if (_OldRequestStationID.length > 0 && $.inArray(eventData.IQ_Station_ID, _OldRequestStationID) !== -1 && $.inArray(eventData.IQ_Station_ID, _RequestStationID) == -1) {
                        _RequestStationID.push(eventData.IQ_Station_ID);
                        _RequestStationIDDisplay.push(eventData.Station_Call_Sign);
                    }
                    var liStyle = "";
                    if ($.inArray(eventData.IQ_Station_ID, _RequestStationID) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    iqstationlist = iqstationlist + "<li role=\"presentation\" class=\"cursorPointer\"><a " + liStyle + " onclick=\"SetStationID(this,'" + eventData.IQ_Station_ID.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "','" + eventData.Station_Call_Sign.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + eventData.Station_Call_Sign + "</a></li>";
                });

                if (iqstationlist != "") {
                    $('#ulStationList').html(iqstationlist);
                    $('#liStationIDSearch').show();
                }
                else {
                    $('#ulStationList').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
                    $('#liStationIDSearch').hide();
                }
            }
        }

        if (e.target.id == "liLogoFilter" || $(e.target).parents("#liLogoFilter").size() > 0) {
            if ($('#ulLogo').is(':visible')) {
                $('#ulLogo').hide();
            }
            else {
                $('#ulLogo').show();
            }
        }
        else if ((e.target.id !== "liLogoFilter" && e.target.id !== "ulLogo" && $(e.target).parents("#ulLogo").size() <= 0) || e.target.id == "btnSearchLogo") {
            $('#ulLogo').hide();
            if (e.target.id != "btnSearchLogo") {
                var logolist = "";
                $.each(_CurrentLogoFilter, function (eventID, eventData) {
                    if (_OldRequestLogo.length > 0 && $.inArray(eventData.ID, _OldRequestLogo) !== -1 && $.inArray(eventData.ID, _RequestLogo) == -1) {
                        _RequestLogo.push(eventData.ID);
                        _RequestLogoDisplay.push(eventData.ThumbnailPath);
                    }
                    var liStyle = "";
                    if ($.inArray(eventData.ID, _RequestLogo) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    logolist = logolist + "<li role=\"presentation\" class=\"cursorPointer\" " + liStyle + " onclick=\"SetLogo(this," + eventData.ID + ",'" + eventData.ThumbnailPath.replace(/\\/g, '/') + "');\"><img src=\"" + eventData.ThumbnailPath + "\" title=\"" + eventData.CompanyName + "\" /></li>";
                });

                if (logolist != "") {
                    $('#ulLogoList').html(logolist);
                    $('#liLogoSearch').show();
                }
                else {
                    $('#ulLogoList').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
                    $('#liLogoSearch').hide();
                }
            }
        }

        if (e.target.id == "liIndustryFilter" || $(e.target).parent("#liIndustryFilter").size() > 0) {
            if ($('#ulIndustry').is(':visible')) {
                $('#ulIndustry').hide();
            }
            else {
                $('#ulIndustry').show();
            }
        }
        else if ((e.target.id !== "liIndustryFilter" && e.target.id !== "ulIndustry" && $(e.target).parents("#ulIndustry").size() <= 0) || e.target.id == "btnSearchIndustry") {
            $('#ulIndustry').hide();
            if (e.target.id != "btnSearchIndustry") {
                var industrylist = "";
                $.each(_CurrentIndustryFilter, function (eventID, eventData) {
                    if (_OldRequestIndustry.length > 0 && $.inArray(eventData, _OldRequestIndustry) !== -1 && $.inArray(eventData, _RequestIndustry) == -1) {
                        _RequestIndustry.push(eventData);
                    }
                    var liStyle = "";
                    if ($.inArray(eventData, _RequestIndustry) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    industrylist = industrylist + "<li role=\"presentation\" class=\"cursorPointer\"><a " + liStyle + " onclick=\"SetIndustry(this,'" + eventData.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + eventData + "</a></li>";
                });

                if (industrylist != "") {
                    $('#ulIndustryList').html(industrylist);
                    $('#liIndustrySearch').show();
                }
                else {
                    $('#ulIndustryList').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
                    $('#liIndustrySearch').hide();
                }
            }
        }

        if (e.target.id == "liBrandFilter" || $(e.target).parent("#liBrandFilter").size() > 0) {
            if ($('#ulBrand').is(':visible')) {
                $('#ulBrand').hide();
            }
            else {
                $('#ulBrand').show();
            }
        }
        else if ((e.target.id !== "liBrandFilter" && e.target.id !== "ulBrand" && $(e.target).parents("#ulBrand").size() <= 0) || e.target.id == "btnSearchBrand") {
            $('#ulBrand').hide();
            if (e.target.id != "btnSearchBrand") {
                var brandlist = "";
                $.each(_CurrentBrandFilter, function (eventID, eventData) {
                    if (_OldRequestBrand.length > 0 && $.inArray(eventData, _OldRequestBrand) !== -1 && $.inArray(eventData, _RequestBrand) == -1) {
                        _RequestBrand.push(eventData);
                    }
                    var liStyle = "";
                    if ($.inArray(eventData, _RequestBrand) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    brandlist = brandlist + "<li role=\"presentation\" class=\"cursorPointer\"><a " + liStyle + " onclick=\"SetBrand(this,'" + eventData.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + eventData + "</a></li>";
                });

                if (brandlist != "") {
                    $('#ulBrandList').html(brandlist);
                    $('#liBrandSearch').show();
                }
                else {
                    $('#ulBrandList').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
                    $('#liBrandSearch').hide();
                }
            }
        }
    });
});

$(window).resize(function () {
    if (screen.height >= 768) {
        $("#divTVScrollContent").css({ "height": documentHeight - 250 });
    }
});

function SetDateVariable() {

    if ($("#dpFrom").val() && $("#dpTo").val()) {
        $('#dpFrom').removeClass('warningInput');
        $('#dpTo').removeClass('warningInput');
        if (_FromDate != $("#dpFrom").val() || _ToDate != $("#dpTo").val()) {
            _FromDate = $("#dpFrom").val();
            _ToDate = $("#dpTo").val();
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
function SetDma(eleDma, dmaname) {
    if ($.inArray(dmaname, _RequestDma) == -1) {
        _RequestDma.push(dmaname);
        $(eleDma).css("background-color", "#E9E9E9");
    }
    else {
        var catIndex = _RequestDma.indexOf(dmaname);
        if (catIndex > -1) {
            _RequestDma.splice(catIndex, 1);
            $(eleDma).css("background-color", "#ffffff");
        }
    }
}

function SearchDma() {
    if ($(_RequestDma).not(_OldRequestDma).length != 0 || $(_OldRequestDma).not(_RequestDma).length != 0) {
        _OldRequestDma = _RequestDma.slice(0);
        SearchResult();
    }
}

function SetLogo(eleLogo, logoID, thumbnailPath) {
    if ($.inArray(logoID, _RequestLogo) == -1) {
        _RequestLogo.push(logoID);
        _RequestLogoDisplay.push(thumbnailPath);
        $(eleLogo).css("background-color", "#E9E9E9");
    }
    else {
        var catIndex = _RequestLogo.indexOf(logoID);
        if (catIndex > -1) {
            _RequestLogo.splice(catIndex, 1);
            _RequestLogoDisplay.splice(catIndex, 1);
            $(eleLogo).css("background-color", "#ffffff");
        }
    }
}

function SearchLogo() {
    if ($(_RequestLogo).not(_OldRequestLogo).length != 0 || $(_OldRequestLogo).not(_RequestLogo).length != 0) {
        _OldRequestLogo = _RequestLogo.slice(0);
        SearchResult();
    }
}

function SetStation(eleStation, stationname) {
    if ($.inArray(stationname, _RequestStation) == -1) {
        _RequestStation.push(stationname);
        $(eleStation).css("background-color", "#E9E9E9");
    }
    else {
        var catIndex = _RequestStation.indexOf(stationname);
        if (catIndex > -1) {
            _RequestStation.splice(catIndex, 1);
            $(eleStation).css("background-color", "#ffffff");
        }
    }
}

function SearchStation() {
    if ($(_RequestStation).not(_OldRequestStation).length != 0 || $(_OldRequestStation).not(_RequestStation).length != 0) {
        _OldRequestStation = _RequestStation.slice(0);
        SearchResult();
    }
}


function SetStationID(eleStation, stationid, stationiddisplay) {
    if ($.inArray(stationid, _RequestStationID) == -1) {
        _RequestStationID.push(stationid);
        _RequestStationIDDisplay.push(stationiddisplay);
        $(eleStation).css("background-color", "#E9E9E9");
    }
    else {
        var catIndex = _RequestStationID.indexOf(stationid);
        if (catIndex > -1) {
            _RequestStationID.splice(catIndex, 1);
            _RequestStationIDDisplay.splice(catIndex, 1);
            $(eleStation).css("background-color", "#ffffff");
        }
    }
}

function SearchStationID() {
    if ($(_RequestStationID).not(_OldRequestStationID).length != 0 || $(_OldRequestStationID).not(_RequestStationID).length != 0) {
        _OldRequestStationID = _RequestStationID.slice(0);
        SearchResult();
    }
}

function SetIndustry(eleIndustry, industry) {
    if ($.inArray(industry, _RequestIndustry) == -1) {
        _RequestIndustry.push(industry);
        $(eleIndustry).css("background-color", "#E9E9E9");
    }
    else {
        var catIndex = _RequestIndustry.indexOf(industry);
        if (catIndex > -1) {
            _RequestIndustry.splice(catIndex, 1);
            $(eleIndustry).css("background-color", "#ffffff");
        }
    }
}

function SearchIndustry() {
    if ($(_RequestIndustry).not(_OldRequestIndustry).length != 0 || $(_OldRequestIndustry).not(_RequestIndustry).length != 0) {
        _OldRequestIndustry = _RequestIndustry.slice(0);
        SearchResult();
    }
}

function SetBrand(eleBrand, brand) {
    if ($.inArray(brand, _RequestBrand) == -1) {
        _RequestBrand.push(brand);
        $(eleBrand).css("background-color", "#E9E9E9");
    }
    else {
        var catIndex = _RequestBrand.indexOf(brand);
        if (catIndex > -1) {
            _RequestBrand.splice(catIndex, 1);
            $(eleBrand).css("background-color", "#ffffff");
        }
    }
}

function SearchBrand() {
    if ($(_RequestBrand).not(_OldRequestBrand).length != 0 || $(_OldRequestBrand).not(_RequestBrand).length != 0) {
        _OldRequestBrand = _RequestBrand.slice(0);
        SearchResult();
    }
}

function SetClass(classname) {
    _Class = classname;
    SearchResult();
}

function SetCountry(countrynum, countryname) {
    _Country = countrynum;
    _CountryName = countryname;
    SearchResult();
}

function SetRegion(regionnum, regionname) {
    _Region = regionnum;
    _RegionName = regionname;
    SearchResult();
}

function SearchResult() {
    var jsonPostData = {
        p_FromDate: _FromDate,
        p_ToDate: _ToDate,
        p_Logo: _RequestLogo,
        p_Dma: _RequestDma,
        p_Station: _RequestStation,
        p_IQStationID: _RequestStationID,
        p_Class: _Class,
        p_Region: _Region,
        p_Country: _Country,
        p_IsAsc: _IsAsc,
        p_IsMarketSort: _IsMarketSort,
        p_Industry: _RequestIndustry,
        p_Brand: _RequestBrand
    }

    if (DateValidation()) {
        SetActiveFilter();

        if (isFilterEnable) {
            $('#divImagiQClearAll').show();
        }
        else {
            $('#divImagiQClearAll').hide();
        }

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlImagiQSearchResults,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            success: OnResultSearchComplete,
            error: OnFail
        });
    }


}
function SearchResultPaging(isNextPage) {
    _IsNext = isNextPage;
    var jsonPostData = {
        p_FromDate: _FromDate,
        p_ToDate: _ToDate,
        p_Logo: _RequestLogo,
        p_Dma: _RequestDma,
        p_Station: _RequestStation,
        p_IQStationID: _RequestStationID,
        p_Class: _Class,
        p_Region: _Region,
        p_Country: _Country,
        p_IsAsc: _IsAsc,
        p_IsNext: _IsNext,
        p_IsMarketSort: _IsMarketSort,
        p_Industry: _RequestIndustry,
        p_Brand: _RequestBrand
    }

    if (DateValidation()) {
        SetActiveFilter();
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlImagiQSearchResultsPaging,
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
        $('#ulImagiQResults').html('');
        $('#ulImagiQResults').html(result.HTML);
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
            if (result.filters["IQ_Dma"] != null) {
                var iqdmalist = "";
                $.each(result.filters["IQ_Dma"], function (index, obj) {
                    var liStyle = "";
                    if ($.inArray(obj.Name, _RequestDma) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    iqdmalist = iqdmalist + "<li role=\"presentation\" class=\"cursorPointer\"><a " + liStyle + " onclick=\"SetDma(this,'" + obj.Name.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + obj.Name + "</a></li>";
                });
                $("#ulMarketList").html(iqdmalist);
                _CurrentDmaFilter = result.filters["IQ_Dma"].slice(0);
            }
            if (result.filters["Station_Affil"] != null) {
                var iqstationlist = "";
                $.each(result.filters["Station_Affil"], function (index, obj) {
                    var liStyle = "";
                    if ($.inArray(obj.Name, _RequestStation) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    iqstationlist = iqstationlist + "<li role=\"presentation\" class=\"cursorPointer\"><a " + liStyle + " onclick=\"SetStation(this,'" + obj.Name.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + obj.Name + "</a></li>";
                });
                $("#ulNetworkList").html(iqstationlist);
                _CurrentStationFilter = result.filters["Station_Affil"].slice(0);
            }


            if (result.filters["IQ_Station"] != null) {
                var iqstationlist = "";
                $.each(result.filters["IQ_Station"], function (index, obj) {
                    var liStyle = "";
                    if ($.inArray(obj.IQ_Station_ID, _RequestStationID) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    iqstationlist = iqstationlist + "<li role=\"presentation\" class=\"cursorPointer\"><a " + liStyle + " onclick=\"SetStationID(this,'" + obj.IQ_Station_ID.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "','" + obj.Station_Call_Sign.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + obj.Station_Call_Sign + "</a></li>";
                });
                $("#ulStationList").html(iqstationlist);
                _CurrentStationIDFilter = result.filters["IQ_Station"].slice(0);

                if (result.filters["IQ_Country"] != null) {
                    var iqcountrylist = "";
                    $.each(result.filters["IQ_Country"], function (index, obj) {
                        iqcountrylist = iqcountrylist + "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetCountry(" + obj.Num + ",'" + obj.Name.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + obj.Name + "</a></li>";
                    });
                    $("#ulCountry").html(iqcountrylist);
                }

                if (result.filters["IQ_Region"] != null) {
                    var iqregionlist = "";
                    $.each(result.filters["IQ_Region"], function (index, obj) {
                        iqregionlist = iqregionlist + "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetRegion(" + obj.Num + ",'" + obj.Name.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + obj.Name + "</a></li>";
                    });
                    $("#ulRegion").html(iqregionlist);
                }
            }

            if (result.filters["Logo"] != null) {
                var logolist = "";
                $.each(result.filters["Logo"], function (index, obj) {
                    var liStyle = "";
                    if ($.inArray(obj.ID, _RequestLogo) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    logolist = logolist + "<li role=\"presentation\" class=\"cursorPointer\" " + liStyle + " onclick=\"SetLogo(this," + obj.ID + ",'" + obj.ThumbnailPath.replace(/\\/g, '/') + "');\"><img src=\"" + obj.ThumbnailPath + "\" title=\"" + obj.CompanyName + "\" /></li>";
                });
                $("#ulLogoList").html(logolist);
                _CurrentLogoFilter = result.filters["Logo"].slice(0);
            }

            if (result.filters["IQ_Class"] != null) {
                var iqclasslist = "";
                $.each(result.filters["IQ_Class"], function (index, obj) {
                    iqclasslist = iqclasslist + "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetClass('" + obj.Name.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + obj.Name + "</a></li>";
                });
                $("#ulCategory").html(iqclasslist);
            }

            if (result.filters["Industry"] != null) {
                var industrylist = "";
                $.each(result.filters["Industry"], function (index, obj) {
                    var liStyle = "";
                    if ($.inArray(obj.ID, _RequestIndustry) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    industrylist = industrylist + "<li role=\"presentation\" class=\"cursorPointer\"><a " + liStyle + " onclick=\"SetIndustry(this,'" + obj.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + obj + "</a></li>";
                });
                $("#ulIndustryList").html(industrylist);
                _CurrentIndustryFilter = result.filters["Industry"].slice(0);
            }

            if (result.filters["Brand"] != null) {
                var brandlist = "";
                $.each(result.filters["Brand"], function (index, obj) {
                    var liStyle = "";
                    if ($.inArray(obj.ID, _RequestBrand) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    brandlist = brandlist + "<li role=\"presentation\" class=\"cursorPointer\"><a " + liStyle + " onclick=\"SetBrand(this,'" + obj.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + obj + "</a></li>";
                });
                $("#ulBrandList").html(brandlist);
                _CurrentBrandFilter = result.filters["Brand"].slice(0);
            }
        }
    }
    else {
        ClearResultsOnError('ulImagiQResults', 'divPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "SearchResult()"));
    }



}
function OnFail(result) {
    _IsTabChange = false;

    ShowNotification(_msgErrorOccured);
    ClearResultsOnError('ulImagiQResults', 'divPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "SearchResult()"));
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
    
    if ((_FromDate != null && _FromDate != "") && (_ToDate != null && _ToDate != "")) {
        $('#divActiveFilter').append('<div id="divDateActiveFilter" class="filter-in">' + _FromDate + ' To ' + _ToDate + '<span class="cancel" onclick="RemoveFilter(1);"></span></div>');
        isFilterEnable = true;
    }
    if (_RequestDma != null && _RequestDma.length > 0) {
        $('#divActiveFilter').append('<div id="divDmaActiveFilter" class="filter-in">' + _RequestDma.join() + '<span class="cancel" onclick="RemoveFilter(2);"></span></div>');
        isFilterEnable = true;
    }
    if (_RequestStation != null && _RequestStation.length > 0) {
        $('#divActiveFilter').append('<div id="divStationActiveFilter" class="filter-in">' + _RequestStation.join() + '<span class="cancel" onclick="RemoveFilter(3);"></span></div>');
        isFilterEnable = true;
    }
    if (_Class != null && _Class != "") {
        $('#divActiveFilter').append('<div id="divClassActiveFilter" class="filter-in">' + _Class + '<span class="cancel" onclick="RemoveFilter(4);"></span></div>');
        isFilterEnable = true;
    }
    if (_RequestStationIDDisplay != null && _RequestStationIDDisplay.length > 0) {
        $('#divActiveFilter').append('<div id="divStationIDActiveFilter" class="filter-in">' + _RequestStationIDDisplay.join() + '<span class="cancel" onclick="RemoveFilter(5);"></span></div>');
        isFilterEnable = true;
    }
    if (_Region != null && _Region != "") {
        $('#divActiveFilter').append('<div id="divRegionActiveFilter" class="filter-in">' + _RegionName + '<span class="cancel" onclick="RemoveFilter(6);"></span></div>');
        isFilterEnable = true;
    }
    if (_Country != null && _Country != "") {
        $('#divActiveFilter').append('<div id="divCountryActiveFilter" class="filter-in">' + _CountryName + '<span class="cancel" onclick="RemoveFilter(7);"></span></div>');
        isFilterEnable = true;
    }
    if (_RequestLogoDisplay != null && _RequestLogoDisplay.length > 0) {
        var logoFilter = '<div id="divLogoActiveFilter" class="filter-in">';
        $.each(_RequestLogoDisplay, function (eventID, eventData) {
            logoFilter = logoFilter + '<img src="' + eventData + '" style="padding-right:8px;"/>';
        });
        logoFilter = logoFilter + '<span class="cancel" onclick="RemoveFilter(8);"></span></div>';
        $('#divActiveFilter').append(logoFilter);
        isFilterEnable = true;
    }
    if (_RequestIndustry != null && _RequestIndustry.length > 0) {
        $('#divActiveFilter').append('<div id="divIndustryActiveFilter" class="filter-in">' + _RequestIndustry.join() + '<span class="cancel" onclick="RemoveFilter(9);"></span></div>');
        isFilterEnable = true;
    }
    if (_RequestBrand != null && _RequestBrand.length > 0) {
        $('#divActiveFilter').append('<div id="divBrandActiveFilter" class="filter-in">' + _RequestBrand.join() + '<span class="cancel" onclick="RemoveFilter(10);"></span></div>');
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

    // Represent Date filter(From Date,To Date)
    if (filterType == 1) {

        $("#dpFrom").datepicker("setDate", null);
        $("#dpTo").datepicker("setDate", null);
        $("#divCalender").datepicker("setDate", null);

        $('#aDuration').html(_msgAll + '&nbsp;&nbsp;<span class="caret"></span>');

        _FromDate = null;
        _ToDate = null;
        SearchResult();
    }

    // Represent Dma Filter
    if (filterType == 2) {
        _RequestDma = [];
        _OldRequestDma = [];
        SearchResult();
    }

    // Represent Station Filter
    if (filterType == 3) {
        _RequestStation = [];
        _OldRequestStation = [];
        SearchResult();
    }
    // Represent Class Filter
    if (filterType == 4) {
        _Class = "";
        SearchResult();
    }

    // Represent Station Filter
    if (filterType == 5) {
        _RequestStationID = [];
        _RequestStationIDDisplay = [];
        _OldRequestStationID = [];
        SearchResult();
    }

    if (filterType == 6) {
        _Region = null;
        _RegionName = "";
        SearchResult();
    }

    if (filterType == 7) {
        _Country = null;
        _CountryName = "";
        SearchResult();
    }

    if (filterType == 8) {
        _RequestLogo = [];
        _RequestLogoDisplay = [];
        _OldRequestLogo = [];
        SearchResult();
    }

    if (filterType == 9) {
        _RequestIndustry = [];
        _OldRequestIndustry = [];
        SearchResult();
    }

    if (filterType == 10) {
        _RequestBrand = [];
        _OldRequestBrand = [];
        SearchResult();
    }
}
function SortDirection(p_IsMarketSort, p_IsAsc) {

    if (p_IsAsc != _IsAsc || _IsMarketSort != p_IsMarketSort) {
        _IsAsc = p_IsAsc;
        _IsMarketSort = p_IsMarketSort;

        if (!_IsMarketSort && _IsAsc) {
            $('#aSortDirection').html(_msgOldestFirst + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (!_IsMarketSort && !_IsAsc) {
            $('#aSortDirection').html(_msgMostRecent + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (_IsMarketSort && _IsAsc) {
            $('#aSortDirection').html(_msgMarketAscending + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (_IsMarketSort && !_IsAsc) {
            $('#aSortDirection').html(_msgMarketDescending + '&nbsp;&nbsp;<span class="caret"></span>');
        }
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

function ClearSearch() {
    _FromDate = null;
    _ToDate = null;
    _RequestDma = [];
    _OldRequestDma = [];
    _RequestStation = [];
    _OldRequestStation = [];
    _RequestStationID = [];
    _RequestStationIDDisplay = [];
    _OldRequestStationID = [];
    _RequestLogo = [];
    _RequestLogoDisplay = [];
    _OldRequestLogo = [];
    _RequestIndustry = [];
    _OldRequestIndustry = [];
    _RequestBrand = [];
    _OldRequestBrand = [];
    _Class = "";
    _Country = null;
    _Region = null;
    _IsAsc = false;
    _IsMarketSort = false;

    $('#aDuration').html(_msgAll + '&nbsp;&nbsp;<span class="caret"></span>');
    $("#dpFrom").datepicker("setDate", null);
    $("#dpTo").datepicker("setDate", null);
    $("#divCalender").datepicker("setDate", null);

    SearchResult();
}




function LoadChartNPlayer(rawMediaGuid, iqcckey, title) {
    LoadPlayerbyGuidTS(rawMediaGuid, title);
    var jsonPostData = {
        IQ_CC_KEY: iqcckey,
        RAW_MEDIA_GUID: rawMediaGuid
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlImagiQGetChart,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),
        success: OnGetChartComplete,
        error: OnGetChartFail
    });


}

function OnGetChartComplete(result) {
    if (result.isSuccess) {
        if (result.hasLRResults && result.LRResultsJson.length > 0) {
            $('.logo-chart-content').html('');
            $('.logo-chart-content').append('<div class="float-left" id="logo-results" style="width:100%; padding-right:20px; padding-left:20px; background-color: #FFFFFF""></div>');
            numOfVisibleLogos = result.yAxisCompanies.length;
            yAxisInfo = {};
            yAxisInfo.IDs = [];
            $.each(result.yAxisCompanies, function (index, value) {
                yAxisInfo.IDs.push(index + 1);
            });
            yAxisInfo.yAxisCompanies = result.yAxisCompanies;
            yAxisInfo.yAxisLogoPaths = result.yAxisLogoPaths;
            RenderHighCharts(result.LRResultsJson, 'logo-results');
        }
        else {
            $('.logo-chart-content').append('<table style="width:100%; background-color:#c0c0c0"><tr><td>There is no available Logo data.</td></tr></table>');
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

function OnFail(result) {
    ShowNotification(_msgErrorOccured);
}

function OnGetChartFail(result) {
    ShowNotification(_msgErrorOccured);
}

function RenderHighCharts(jsonLineChartData, chartID) {

    var JsonLineChart = JSON.parse(jsonLineChartData);
    JsonLineChart.plotOptions.series.point.events.click = LineChartClick;
    JsonLineChart.xAxis.labels.formatter = FormatTime
    //JsonLineChart.tooltip.positioner = TooltipSetY
    JsonLineChart.tooltip.formatter = tooltipFormat
    JsonLineChart.plotOptions.series.events.show = HandleSeriesShowHide;
    JsonLineChart.plotOptions.series.events.hide = HandleSeriesShowHide;
    JsonLineChart.plotOptions.series.point.events.mouseOver = ChartHoverManage;
    JsonLineChart.plotOptions.series.point.events.mouseOut = ChartHoverOutManage;

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
            //chart.tooltip.refresh([chart.series[0].data[xIndex], chart.series[1].data[xIndex]]);
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
    //$('#divKantorChart').scrollLeft(this.plotX);
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