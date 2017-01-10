using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class IQAgent_TVFullResultsModel
    {
        public Int64 ID { get; set; }
        public string Title120 { get; set; }
        public string CC { get; set; }
        public string RL_Station { get; set; }
        public string RawMediaThumbUrl { get; set; }
        public DateTime Date { get; set; }
        public DateTime LocalDateTime { get; set; }
        //public string MonthName { get; set; }
        public string StationLogo { get; set; }
        public string higlightedCC { get; set; }
        public HighlightedCCOutput highlightedCCOutput { get; set; }
        public Int64? SinceID { get; set; }
        public Guid RL_VideoGUID { get; set; }
        public Boolean IsUrlFound { get; set; }
        public string Market { get; set; }
        public int Hits { get; set; }
        public string StationID { get; set; }
        public string IQ_CC_Key { get; set; }
        public decimal? AUDIENCE { get; set; }
        public decimal? SQAD_SHAREVALUE { get; set; }
        public bool IsActualNielsen { get; set; }
        public decimal IQProminence { get; set; }
        public decimal IQProminenceMultiplier { get; set; }

        public string IQAgentResultUrl { get; set; }
        public int? Nielsen_Audience { get; set; }

        public decimal? IQAdShareValue { get; set; }

        public string Nielsen_Result { get; set; }
        public string TimeZone { get; set; }
        public string Station_Call_Sign { get; set; }

        public Int32 _ParentID { get; set; }
        public string Dma_Num { get; set; }
        public List<IQAgent_MediaResultsModel> ChildResults { get; set; }

        public Int64? National_Nielsen_Audience { get; set; }
        public decimal? National_IQAdShareValue { get; set; }
        public string National_Nielsen_Result { get; set; }

        public bool LogoStatus { get; set; } //Logo data has been processed
        public bool AdStatus { get; set; } //Ad data has been processed
        public bool PEStatus { get; set; } //Paid/Earned data has been processed
        public List<long> BrandIDs { get; set; }
        public List<long> CompanyIDs { get; set; }
        public List<long> IndustryIDs { get; set; }
        public List<long> SearchLogoIDs { get; set; }
        public List<string> Ads { get; set; }
        public List<string> Logos { get; set; }

        public IQAgent_TVResultsModel ShallowCopy()
        {
            return (IQAgent_TVResultsModel)this.MemberwiseClone();
        }
    }
}
