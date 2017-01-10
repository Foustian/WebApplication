using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMedia.Model
{

    public class DiscoveryAdvanceSearch_DropDown
    {
        public List<IQ_Dma> TV_DMAList { get; set; }

        public List<IQ_Class> TV_ClassList { get; set; }

        public List<IQ_Station> TV_StationList { get; set; }

        public List<Station_Affil> TV_AffiliateList { get; set; }

        public List<IQ_Region> TV_RegionList { get; set; }

        public List<IQ_Country> TV_CountryList { get; set; }

        public List<IQAgentDropDown_NM_Genere> NM_GenreList { get; set; }

        public List<IQAgentDropDown_NM_Category> NM_CategoryList { get; set; }

        public List<IQAgentDropDown_NM_PublicationCategory> NM_PublicationCategoryList { get; set; }

        public List<IQAgentDropDown_NM_Market> NM_MarketList { get; set; }

        public List<IQAgentDropDown_NM_Region> NM_RegionList { get; set; }

        public List<IQAgentDropDown_SM_SourceType> SM_SourceTypeList { get; set; }

        public Dictionary<string, string> CountryList { get; set; }

        public List<string> LanguageList { get; set; }

        public bool IsAllDmaAllowed { get; set; }

        public bool IsAllClassAllowed { get; set; }

        public bool IsAllStationAllowed { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "AdvanceSearchSettings")]
    public class DiscoveryAdvanceSearchModel
    {
        public TVAdvanceSearchSettings TVSettings { get; set; }

        public NewsAdvanceSearchSettings NewsSettings { get; set; }

        public LexisNexisAdvanceSearchSettings LexisNexisSettings { get; set; }

        public BlogAdvanceSearchSettings BlogSettings { get; set; }

        public ForumAdvanceSearchSettings ForumSettings { get; set; }

        public ProQuestAdvanceSearchSettings ProQuestSettings { get; set; }
    }

    [Serializable]
    public class TVAdvanceSearchSettings
    {
        public string SearchTerm { get; set; }

        public string ProgramTitle { get; set; }

        public string Appearing { get; set; }

        [XmlArrayItem(ElementName = "Category")]
        public List<string> CategoryList { get; set; }

        [XmlArrayItem(ElementName = "IQDma")]
        public List<string> IQDmaList { get; set; }

        [XmlArrayItem(ElementName = "Station")]
        public List<string> StationList { get; set; }

        [XmlArrayItem(ElementName = "Affiliate")]
        public List<string> AffiliateList { get; set; }

        [XmlArrayItem(ElementName = "Region")]
        public List<string> RegionList { get; set; }

        [XmlArrayItem(ElementName = "Country")]
        public List<string> CountryList { get; set; }
    }

    [Serializable]
    public class NewsAdvanceSearchSettings
    {
        public string SearchTerm { get; set; }

        [XmlArrayItem(ElementName = "Publication")]
        public List<string> PublicationList { get; set; }

        [XmlArrayItem(ElementName = "Category")]
        public List<string> CategoryList { get; set; }

        [XmlArrayItem(ElementName = "PublicationCategory")]
        public List<int> PublicationCategoryList { get; set; }

        [XmlArrayItem(ElementName = "Market")]
        public List<string> MarketList { get; set;} 

        [XmlArrayItem(ElementName = "Genre")]
        public List<string> GenreList { get; set; }

        [XmlArrayItem(ElementName = "Region")]
        public List<string> RegionList { get; set; }

        [XmlArrayItem(ElementName = "Country")]
        public List<string> CountryList { get; set; }

        [XmlArrayItem(ElementName = "Language")]
        public List<string> LanguageList { get; set; }

        [XmlArrayItem(ElementName = "ExcludeDomain")]
        public List<string> ExcludeDomainList { get; set; }
    }

    [Serializable]
    public class LexisNexisAdvanceSearchSettings
    {
        public string SearchTerm { get; set; }

        [XmlArrayItem(ElementName = "Publication")]
        public List<string> PublicationList { get; set; }

        [XmlArrayItem(ElementName = "Category")]
        public List<string> CategoryList { get; set; }

        [XmlArrayItem(ElementName = "PublicationCategory")]
        public List<int> PublicationCategoryList { get; set; }

        [XmlArrayItem(ElementName = "Genre")]
        public List<string> GenreList { get; set; }

        [XmlArrayItem(ElementName = "Region")]
        public List<string> RegionList { get; set; }

        [XmlArrayItem(ElementName = "Country")]
        public List<string> CountryList { get; set; }

        [XmlArrayItem(ElementName = "Language")]
        public List<string> LanguageList { get; set; }

        [XmlArrayItem(ElementName = "ExcludeDomain")]
        public List<string> ExcludeDomainList { get; set; }
    }

    [Serializable]
    public class BlogAdvanceSearchSettings
    {
        public string SearchTerm { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        [XmlArrayItem(ElementName = "Source")]
        public List<string> SourceList { get; set; }

        [XmlArrayItem(ElementName = "ExcludeDomain")]
        public List<string> ExcludeDomainList { get; set; }
    }

    [Serializable]
    public class ForumAdvanceSearchSettings
    {
        public string SearchTerm { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        [XmlArrayItem(ElementName = "Source")]
        public List<string> SourceList { get; set; }

        [XmlArrayItem(ElementName = "SourceType")]
        public List<string> SourceTypeList { get; set; }

        [XmlArrayItem(ElementName = "ExcludeDomain")]
        public List<string> ExcludeDomainList { get; set; }
    }

    [Serializable]
    public class ProQuestAdvanceSearchSettings
    {
        public string SearchTerm { get; set; }

        [XmlArrayItem(ElementName = "Publication")]
        public List<string> PublicationList { get; set; }

        [XmlArrayItem(ElementName = "Author")]
        public List<string> AuthorList { get; set; }

        [XmlArrayItem(ElementName = "Language")]
        public List<string> LanguageList { get; set; }
    }
}
