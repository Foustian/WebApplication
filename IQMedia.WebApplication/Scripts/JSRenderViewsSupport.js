function ReplaceNumberWithCommas1(num, makeAbs) {

    if (makeAbs) {
        num = Math.abs(num);
    }

    //Seperates the components of the number
    var components = num.toString().split(".");
    //Comma-fies the first part
    components[0] = components[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    //Combines the two sections
    return components.join(".");
}

function GetPercentageRelative(Value1, Value2) {
    if (Value1 == 0 || Value2 == 0) {
        return 100;
    }

    return Math.abs(100 - parseInt(Value2 * 100 / Value1));
}

$.views.converters({
    ReplaceNumberWithCommas: function (value) {

        var mkABS = this.tagCtx.props.mkABS;

        if (mkABS) {
            value = Math.abs(value);
        }

        //Seperates the components of the number
        var components = value.toString().split(".");
        //Comma-fies the first part
        components[0] = components[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        //Combines the two sections
        return components.join(".").toString();
    }
});

var helperPerRelative = { PerRelative: GetPercentageRelative };

$.views.helpers(helperPerRelative);