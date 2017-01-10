(function ($) {
    $.fn.ActiveNav = function () {
        if ($(this).is("li")) {
            var $this = $(this);
            $(this).siblings().removeClass('active');
            $this.addClass('active');
        }

        return this;
    }
}(jQuery));
