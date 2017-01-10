function SetHighligtedLi(ulID, liID) {
    $('#' + ulID + ' li').each(function () {        
        $(this).removeClass('highlightedli');
    });
    $('#' + liID).addClass('highlightedli');
}


