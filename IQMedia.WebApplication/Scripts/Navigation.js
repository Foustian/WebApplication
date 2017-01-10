function setActiveMenu(currentMenu) {
    $('#spnHome').removeClass("selected");
    $('#spnProducts').removeClass("selected");
    $('#spnIndustries').removeClass("selected");
    $('#spnResources').removeClass("selected");
    $('#spnAboutUs').removeClass("selected");
    $('#' + currentMenu).addClass("selected");
    //$('#spnAboutUs').addClass("selected");    
}