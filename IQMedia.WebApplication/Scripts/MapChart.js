var _SelectedDmas = [];
var MaxVal = 0;
var MinVal = 1000000000;


function drawmap(jsonMap, clean) {
    _SelectedDmas = [];
    if (clean != false) {
        $("#legend").empty();
    }
    //var width = 450,
    //height = 330;

    var width = 960,
    height = 700;

   
    // sets the type of view
    var projection = d3.geo.albersUsa()
    .scale(1070) // size, bigger is bigger
    .translate([width / 2, height / 2]);

    //creates a new geographic path generator
    var path = d3.geo.path().projection(projection);
    var xScale = d3.scale.linear()
    .domain([0, 7])
    .range([0, 500]);

    var xAxis = d3.svg.axis()
    //.scale(xScale)
    .orient("bottom")
    .tickSize(13)
    .tickFormat(d3.format("0.0f"));

    var tip = d3.tip()
      .offset([-10, 0])
      .html(function (d) {
          var prop = d.properties;
          return "<div class='d3-tip'><strong>Market Area Name</strong>: " + prop.dma1 + " </br> <strong style='padding: 10px 0 0 0;'>Mention</strong>:" + numberWithCommas(prop["Value"]) + "</div>";
      })

    var CustomHover = function(key,val)
    {
        d = new Object();
        d.properties = new Object();
        d.properties.dma1 = key;
        d.properties.Value = val;
        tip.show(d);
    }

    //set svg window

    var svg = d3.select("#legend")
      .append("svg")
      .attr("width", width)
      .attr("height", height)
      .attr("id", "mySVG")

    svg.call(tip);

    var graticule = d3.geo.graticule()
    .extent([[-98 - 45, 38 - 45], [-98 + 45, 38 + 45]])
    .step([5, 5]);

    // adding a blank background
    svg.append("rect")
    .datum(graticule)
    .attr("class", "legend")
    .attr("class", "background")
    .attr("width", width)
    .attr("height", height)
    // .on("click", clicked);

    //declare g as our appended svg
    var g = svg.append("g");

    var defaultFill = "#aaa";
    var percentage = 100;
    d3.json("/scripts/map/nielsentopo.json", function (error, dma) {

        var nielsen = dma.objects.nielsen_dma.geometries;

        // adding data from tv json (number of TVs, etc) to map json
        //d3.json(jsonMap, function (error, tv) {
        for (var i = 0; i < nielsen.length; i++) {
            var dma_code = nielsen[i].id;
            if (jsonMap[dma_code] != undefined) {
                for (key in jsonMap[dma_code]) {
                    nielsen[i].properties[key] = jsonMap[dma_code][key];
                }
            }
            else {
                nielsen[i].properties["Key"] = dma_code;
                nielsen[i].properties["Value"] = "0";
            }


            if (parseInt(nielsen[i].properties["Value"]) > MaxVal) {
                MaxVal = parseInt(nielsen[i].properties["Value"]);
            }

            if (parseInt(nielsen[i].properties["Value"]) < MinVal) {
                MinVal = parseInt(nielsen[i].properties["Value"]);
            }
        }
        dma.objects.nielsen_dma.geometries = nielsen;
        GenerateLegendMap();

        g.append("g")
    .attr("id", "dmas")
    .selectAll("path")
    .data(topojson.feature(dma, dma.objects.nielsen_dma).features)
    .enter()
    .append("path")
    .attr("d", path)
    .on("click", function (d) {
        CheckUncheckDMA(d.properties.Key,d.properties.Value, d3.select(this));
    })
    .on('mouseover', tip.show)
    .on('mouseout', tip.hide)
    .attr("opacity", 0.9)
    .attr("fill", function (d) {
        var prop = d.properties;
        return CalclulateFillColor(prop.Value);
    })

        // add dma borders
        g.append("path", ".graticule")
      .datum(topojson.mesh(dma, dma.objects.nielsen_dma, function (a, b) {
          return true;
      }))
      .attr("id", "dma-borders")
      .attr("d", path);
        //})
    })

    $(function () {
        

        setTimeout(function () {
            var lineDataF = [{ "x": 175, "y": 528 },
        { "x": 170, "y": 523 }, { "x": 171, "y": 521 }, { "x": 179, "y": 518 },
        { "x": 185, "y": 518 }, { "x": 190, "y": 521 }, { "x": 190, "y": 523 }, { "x": 185, "y": 527 }, { "x": 174, "y": 528}];

            var lineFunctionF = d3.svg.line()
                          .x(function (d) { return d.x; })
                          .y(function (d) { return d.y; })
                         .interpolate("linear");

            //The SVG Container
            var svgContainerF = svg.append("svg")
                                    .attr("width", 429)
                                    .attr("height", 732);

            var lineGraphF = svgContainerF.append("path")
    .attr("d", lineFunctionF(lineDataF))
    .attr("stroke", "blue")
    .attr("stroke-width", 1)
            //.attr("fill", "none")
        .attr("fill", function () {
            if (jsonMap['Fairbanks'] != undefined) {
                return CalclulateFillColor(jsonMap['Fairbanks'].Value);
            }
            else {
                return "rgb(245,245,255)";
            }
        })
        .on('mouseover', function(){
            if (jsonMap['Fairbanks'] != undefined)
            {
                CustomHover(jsonMap['Fairbanks'].Key, jsonMap['Fairbanks'].Value);
            }
            else{
                CustomHover('Fairbanks', '0');
            }
        })
        .on('mouseout', tip.hide)
        .on("click", function (d) {
            //d3.select(this).attr("fill", "rgb(200,255,25)");
            if (jsonMap['Fairbanks'] != undefined) {
                CheckUncheckDMA(jsonMap['Fairbanks'].Key, jsonMap['Fairbanks'].Value, d3.select(this));
            }
            else {
                CheckUncheckDMA('Fairbanks', "0", d3.select(this));
            }
        });
            //        .on("click", function (d) {
            //            d3.select(this).attr("fill", "rgb(255,200,25)");
            //        });

            var lineDataA = [{ "x": 162, "y": 562 },
        { "x": 152, "y": 561 }, { "x": 164, "y": 551 }, { "x": 169, "y": 543 },
        { "x": 171, "y": 541 }, { "x": 179, "y": 538 }, { "x": 186, "y": 541 }, { "x": 184, "y": 549 }, { "x": 179, "y": 554 }, { "x": 162, "y": 562}];

            var lineFunctionA = d3.svg.line()
                          .x(function (d) { return d.x; })
                          .y(function (d) { return d.y; })
                         .interpolate("linear");

            //The SVG Container
            var svgContainerA = svg.append("svg")
                                    .attr("width", 429)
                                    .attr("height", 732);

            var lineGraphA = svgContainerA.append("path")
    .attr("d", lineFunctionA(lineDataA))
    .attr("stroke", "blue")
    .attr("stroke-width", 1)
            //.attr("fill", "none")
     .attr("fill", function () {
         if (jsonMap['Anchorage'] != undefined) {
             return CalclulateFillColor(jsonMap['Anchorage'].Value);
         }
         else {
             return "rgb(245,245,255)";
         }
     })
     .on('mouseover', function(){
         if (jsonMap['Anchorage'] != undefined)
            {
                CustomHover(jsonMap['Anchorage'].Key, jsonMap['Anchorage'].Value);
            }
            else{
                CustomHover('Anchorage', '0');
            }
        })
     .on('mouseout', tip.hide)
     .on("click", function (d) {
         //d3.select(this).attr("fill", "rgb(160,200,10)");
         if (jsonMap['Anchorage'] != undefined) {
             CheckUncheckDMA(jsonMap['Anchorage'].Key, jsonMap['Anchorage'].Value, d3.select(this));
         }
         else {
             CheckUncheckDMA('Anchorage', "0", d3.select(this));
         }
         
     });
            //     .on("click", function (d) {
            //         d3.select(this).attr("fill", "rgb(200,160,10)");
            //     });

            var lineDataJ = [{ "x": 224, "y": 606 },
        { "x": 218, "y": 591 }, { "x": 218, "y": 578 }, { "x": 222, "y": 576 },
        { "x": 223, "y": 576 }, { "x": 227, "y": 581 }, { "x": 231, "y": 597 }, { "x": 233, "y": 602 }, { "x": 223, "y": 605}];

            var lineFunctionJ = d3.svg.line()
                          .x(function (d) { return d.x; })
                          .y(function (d) { return d.y; })
                         .interpolate("linear");

            //The SVG Container
            var svgContainerJ = svg.append("svg")
                                    .attr("width", 429)
                                    .attr("height", 732);

            var lineGraphJ = svgContainerJ.append("path")
    .attr("d", lineFunctionJ(lineDataJ))
    .attr("stroke", "blue")
    .attr("stroke-width", 1)
            //.attr("fill", "none")
     .attr("fill", function () {
         if (jsonMap['Juneau'] != undefined) {
             return CalclulateFillColor(jsonMap['Juneau'].Value);
         }
         else {
             return "rgb(245,245,255)";
         }
     })
     .on('mouseover', function(){
         if (jsonMap['Juneau'] != undefined)
            {
                CustomHover(jsonMap['Juneau'].Key, jsonMap['Juneau'].Value);
            }
            else{
                CustomHover('Juneau', '0');
            }
        })
     .on('mouseout', tip.hide)
     .on("click", function (d) {
         //d3.select(this).attr("fill", "rgb(120,200,50)");
         if (jsonMap['Juneau'] != undefined) {
             CheckUncheckDMA(jsonMap['Juneau'].Key, jsonMap['Juneau'].Value, d3.select(this));
         }
         else {
             CheckUncheckDMA('Juneau', "0", d3.select(this));
         }
     });
            //     .on("click", function (d) {
            //         d3.select(this).attr("fill", "rgb(200,120,50)");
            //     });

            var lineDataH = [{ "x": 351, "y": 578 },
        { "x": 350, "y": 573 }, { "x": 353, "y": 570 }, { "x": 355, "y": 567 },
        { "x": 362, "y": 563 }, { "x": 371, "y": 557 }, { "x": 376, "y": 558 }, { "x": 380, "y": 562 }, { "x": 373, "y": 572 }, { "x": 363, "y": 575 }, { "x": 351, "y": 578}];

            var lineFunctionH = d3.svg.line()
                          .x(function (d) { return d.x; })
                          .y(function (d) { return d.y; })
                         .interpolate("linear");

            //The SVG Container
            var svgContainerH = svg.append("svg")
                                    .attr("width", 429)
                                    .attr("height", 732);

            var lineGraphH = svgContainerH.append("path")
    .attr("d", lineFunctionH(lineDataH))
    .attr("stroke", "blue")
    .attr("stroke-width", 1)
            //.attr("fill", "none")
    .attr("fill", function () {
        if (jsonMap['Honolulu'] != undefined) {
            return CalclulateFillColor(jsonMap['Honolulu'].Value);
        }
        else {
            return "rgb(245,245,255)";
        }
    })
    .on('mouseover', function(){

        if (jsonMap['Honolulu'] != undefined)
            {
                CustomHover(jsonMap['Honolulu'].Key, jsonMap['Honolulu'].Value);
            }
            else{
                CustomHover('Honolulu', '0');
            }
        })
    .on('mouseout', tip.hide)
    .on("click", function (d) {
        //d3.select(this).attr("fill", "rgb(80,200,100)");
        if (jsonMap['Honolulu'] != undefined) {
            CheckUncheckDMA(jsonMap['Honolulu'].Key, jsonMap['Honolulu'].Value, d3.select(this));
        }
        else {
            CheckUncheckDMA('Honolulu', "0", d3.select(this));
        }
    });
            //    .on("click", function (d) {
            //        d3.select(this).attr("fill", "rgb(200,80,100)");
            //    });
        }, 1000);
    });


}

function CalclulateFillColor(value) {
    if (MaxVal != 0) {
        var NewValue = (value * 255) / MaxVal;
    }
    else {
        var NewValue = value;
    }

    var Rank = parseInt(NewValue);

    if (Rank >= 250) {
        Rank = 1;
    }
    else if (Rank <= 10) {
        Rank = 245
    }
    else {
        Rank = 255 - Rank;
    }

    if (Number(Rank) > 0) {

        return "rgb(" + Rank + "," + Rank + ",255)"
    }
    else {
        return "rgb(" + Number(1) + ", " + Number(1) + ", 255)"
    }
}


function CheckUncheckDMA(key,value,eleChart) {
    if (eleChart.attr("changed") == null) {
        if (_SelectedDmas.length < 2) {

            var rgb = eleChart.attr("fill");
            rgb = rgb.substr(Number(rgb.indexOf("(")) + Number(1), (Number(rgb.indexOf(")")) - Number(rgb.indexOf("(") + Number(1))));
            var array = rgb.split(",");
            var r = array[0];
            var g = array[1];
            var b = array[2];
            //r = 255;
            b = 1;

            if (g > 0 && g < 50) {
                g = Number(g) + Number(30);
            }
            else if (g >= 100 && g < 150) {
                g = Number(g) - Number(20);
            }
            else if (g >= 150 && g < 200) {
                g = Number(g) - Number(50);
            }
            else if (g >= 200 && g < 255) {
                g = Number(g) - Number(10);
            }
            if (r > 0 && r < 50) {
                r = Number(r) + Number(10);
            }
            else if (r >= 100 && r < 150) {
                r = Number(r) - Number(80);
            }
            else if (r >= 150 && r < 200) {
                r = Number(r) - Number(100);
            }
            else if (r >= 200 && r < 255) {
                r = Number(r) - Number(100);
            }

            var newRGB = "rgb(" + r + ", " + g + ", " + b + ")"
            eleChart.attr("fill", newRGB);
            eleChart.attr("changed", "true");

            _SelectedDmas.push(key);
            CompareDma(_SelectedDmas);
        }
        else {
            ShowNotification(_msgMaxDmaLimitExceed);
        }
    }
    else {

        eleChart.attr("fill", CalclulateFillColor(value));
        eleChart.attr("changed", null);

        var catIndex = _SelectedDmas.indexOf(key);
        if (catIndex > -1) {
            _SelectedDmas.splice(catIndex, 1);
            CompareDma(_SelectedDmas);
        }
    }   
}

function GenerateLegendMap()
{
    var colorbrewer = { Blues: {
            1: ["rgb(245,245,255)", "rgb(225,225,255)", "rgb(130,140,255)", "rgb(100,100,255)", "rgb(50,50,255)", "rgb(1,1,255)"]
        }
        };

        var colors = d3.scale.quantize()
    .range(colorbrewer.Blues[1]);
        var i = 1;
        var legend = d3.select('#legendMAP')
  .append('ul').attr('class', 'list-inline');

        var keys = legend.selectAll('li.key')
    .data(colors.range());
        var avg = 0;
        var legendArray = [];
        avg = (Number(MaxVal) - Number(MinVal)) / 5;
        keys.enter().append('li')
            .attr('class', 'key')
            .style('border-top-color', String)
            .text(function (data, count) {
                if (count == 0) {
                    return numberWithCommas(MinVal);
                }
                else if (count == 5) {
                    return numberWithCommas(MaxVal);
                }
                else {
                    return numberWithCommas(Math.round((Number(avg) * count)));
                }
            });
}

// via http://stackoverflow.com/a/2901298
    function numberWithCommas(x) {
        return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }