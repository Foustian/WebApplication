using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class IQ_QVCDataModel
    {
        public List<Data> Data { get; set; }
        public List<Appendix> Appendix { get; set; }
    }
    public class Data
    {
        public Int32 M { get; set; }
        public double A { get; set; }
        public Int32 CL { get; set; }
        public Int32 T { get; set; }
        public List<LabelData> labelData { get; set; }
    }
    public class LabelData
    {
        public Int32 I { get; set; }
        public double SA { get; set; }
        public Int32 SQ { get; set; }
    }
    public class Appendix
    {
        public string Label { get; set; }
        public Int32 I { get; set; }
    }
}
