using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class ISVCModel
    {
        public List<ISVCServiceModel> ISVCServices { get; set; }
    }

    public class ISVCServiceModel
    {

        public string Name { get; set; }

        public string Url { get; set; }

        public string JsonRequest { get; set; }

        public string XmlRequest { get; set; }

        public string Note { get; set; }

    }
}
