if (_UserHostAddress == 'True') {
    $.ajax({
        url: _urlIOSAndroidClipPlayer,
        contentType: "application/json; charset=utf-8",
        type: "GET",
        dataType: 'jsonp',
        success: function (result) {
        },
        error: function (a, b, c) {
        }
    });
}