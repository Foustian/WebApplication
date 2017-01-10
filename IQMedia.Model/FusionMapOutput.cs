using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IQMedia.Model
{
    [DataContract]
    public class FusionMapOutput
    {
        [DataMember(Name = "map")]
        public FusionMap map { get; set; }

        [DataMember(Name = "colorrange")]        
        public FusionMapColorRange colorrange { get; set; }

        [DataMember(Name = "data")]
        public List<FusionMapData> data { get; set; }
    }

    [DataContract]
    public class FusionMap
    {
        [DataMember(EmitDefaultValue=false)]
        public string animation{ get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string showbevel { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string showLabels { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string usehovercolor { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string canvasbordercolor{ get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string bordercolor{ get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string showlegend { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string showshadow { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string legendposition{ get; set; }

        [DataMember(EmitDefaultValue = false)]
        public float legendborderalpha{ get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string legendbordercolor{ get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string legendallowdrag { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string legendshadow { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string caption{ get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string subcaption{ get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string connectorcolor{ get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string fillalpha{ get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string hovercolor { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string showborder { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string showEntityToolTip { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string showToolTip { get; set; }
    }

    [DataContract]
    public class FusionMapColorRange
    {
        [DataMember]
        public string gradient = "1";

        [DataMember]
        public string code = "C3EBFD";

        [DataMember(Name = "color")]     
        public List<FusionMapColor> color { get; set; }
    }

    [DataContract]
    public class FusionMapColor
    {
        [DataMember()]
        public string minvalue { get; set; }

        [DataMember()]
        public string maxvalue { get; set; }

        [DataMember()]
        public string displayvalue { get; set; }

        [DataMember()]
        public string code { get; set; }
    }

    [DataContract]
    public class FusionMapData
    {
        [DataMember()]
        public string id { get; set; }

        [DataMember()]
        public string value { get; set; }

        [DataMember(EmitDefaultValue=false)]
        public string link { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string tooltext { get; set; }
        
        [DataMember(EmitDefaultValue = false)]
        public string showlabel { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string showEntityToolTip { get; set; }
    }
}
