using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMedia.Model
{
    [Serializable]
    public class IQClient_CustomSettingsModel
    {
        public List<IQ_Industry> visibleLRIndustries { get; set; }

        public List<string> visibleLRBrands { get; set; }

        public int? v4MaxLibraryReportItems { get; set; }

        public int? v4MaxLibraryEmailReportItems { get; set; }

        public int? v4MaxFeedsExportItems { get; set; }

        public int? AutoClipDuration { get; set; }

        public int? TotalNoOfIQAgent { get; set; }

        public int? v4MaxDiscoveryReportItems { get; set; }

        public int? v4MaxDiscoveryExportItems { get; set; }

        public int v4MaxDiscoveryHistory { get; set; }

        public int? v4MaxFeedsReportItems { get; set; }

        public decimal? CompeteAudienceMultiplier { get; set; }

        public decimal? CompeteMultiplier { get; set; }

        public decimal? Multiplier { get; set; }

        public decimal? OnlineNewsAdRate { get; set; }

        public decimal? OtherOnlineAdRate { get; set; }

        public decimal? URLPercentRead { get; set; }

        public decimal? TVLowThreshold { get; set; }

        public decimal? TVHighThreshold { get; set; }

        public decimal? NMLowThreshold { get; set; }

        public decimal? NMHighThreshold { get; set; }

        public decimal? SMLowThreshold { get; set; }

        public decimal? SMHighThreshold { get; set; }

        public decimal? TwitterLowThreshold { get; set; }

        public decimal? TwitterHighThreshold { get; set; }

        public decimal? PQLowThreshold { get; set; }

        public decimal? PQHighThreshold { get; set; }

        public List<Int16> IQLicense { get; set; }

        public SearchSettingsModel SearchSettings { get; set; }

        public bool v4LibraryRollup { get; set; }

        public bool? UseProminence { get; set; }

        public bool? UseProminenceMediaValue { get; set; }

        public bool? ForceCategorySelection { get; set; }

        public int MCMediaPublishedTemplateID { get; set; }

        public string MCMediaPublishedTemplate { get; set; }

        public int MCMediaDefaultEmailTemplateID { get; set; }

        public string MCMediaDefaultEmailTemplate { get; set; }

        public MCMediaAvailableTemplatesModel MCMediaAvailableTemplates { get; set; }

        public int? IQRawMediaExpiration { get; set; }

        public Shared.Utility.CommonFunctions.LibraryTextTypes LibraryTextType { get; set; }

        public int DefaultFeedsPageSize { get; set; }

        public int DefaultDiscoveryPageSize { get; set; }

        public int DefaultArchivePageSize { get; set; }

        public bool? ClipEmbedAutoPlay { get; set; }

        public bool? DefaultFeedsShowUnread { get; set; }

        public bool? UseCustomerEmailDefault { get; set; }

        public string IQTVCountry { get; set; }

        public string IQTVRegion { get; set; }

        [Serializable]
        [XmlRoot("SearchSettings")]
        public class SearchSettingsModel
        {
            [XmlElement("IQ_Dma_Set")]
            public IQ_Dma_Set IQ_Dma_Set { get; set; }

            [XmlElement("Station_Affiliate_Set")]
            public Station_Affiliate_Set Station_Affiliate_Set { get; set; }

            [XmlElement("IQ_Class_Set")]
            public IQ_Class_Set IQ_Class_Set { get; set; }
        }

        [Serializable]
        public class IQ_Dma_Set
        {
            public IQ_Dma_Set()
            {
                IQ_Dma = new List<IQ_Dma>();
            }
            [XmlAttribute]
            public bool IsAllowAll { get; set; }

            [XmlElement("IQ_Dma")]
            public List<IQ_Dma> IQ_Dma { get; set; }
        }

        [Serializable]
        public class Station_Affiliate_Set
        {
            public Station_Affiliate_Set()
        {
            Station_Affiliate = new List<Station_Affil>();
        }

        [XmlAttribute]
        public bool IsAllowAll { get; set; }

        [XmlElement("Station_Affiliate")]
        public List<Station_Affil> Station_Affiliate { get; set; }
        }

        [Serializable]
        public class IQ_Class_Set
        {
            public IQ_Class_Set()
            {
                IQ_Class = new List<IQ_Class>();
            }

            [XmlAttribute]
            public bool IsAllowAll { get; set; }

            [XmlElement("IQ_Class")]
            public List<IQ_Class> IQ_Class { get; set; }
        }

        [Serializable]
        [XmlRoot("Templates")]
        public class MCMediaAvailableTemplatesModel
        {
            [XmlElement("Published")]
            public MCMediaTemplateSet Published { get; set; }

            [XmlElement("Email")]
            public MCMediaTemplateSet Email { get; set; }
        }

        [Serializable]
        public class MCMediaTemplateSet
        {
            public MCMediaTemplateSet()
            {
                Template = new List<MCMediaTemplate>();
            }
            [XmlAttribute]
            public bool IsAllowAll { get; set; }

            [XmlElement("Template")]
            public List<MCMediaTemplate> Template { get; set; }
        }

        [Serializable]
        public class MCMediaTemplate
        {
            [XmlElement("Name")]
            public string Name { get; set; }

            [XmlElement("ID")]
            public int ID { get; set; }
        }
    }
}
