using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class IQAgent_DaySummaryModel
    {
        public Int64 ID { get; set; }
        public Guid ClientGuid { get; set; }
        public DateTime DayDate { get; set; }
        public string MediaType { get; set; }
        public Int64 NoOfDocs { get; set; }
        public Int64 NoOfHits { get; set; }
        public Int64 Audience { get; set; }
        public Decimal IQMediaValue { get; set; }
        public string SubMediaType { get; set; }
        public Int64 PositiveSentiment { get; set; }
        public Int64 NegativeSentiment { get; set; }
        public string Query_Name { get; set; }
        public Int64 SearchRequestID { get; set; }
    }

    public class IQAgent_DashBoardModel
    {
        public List<IQAgent_DaySummaryModel> ListOfIQAgentSummary { get; set; }

        public List<DashboardTopResultsModel> ListOfTopDMABroadCast { get; set; }

        public List<DashboardTopResultsModel> ListOfTopStationBroadCast { get; set; }

        public List<DashboardTopResultsModel> ListOfTopCountryBroadCast { get; set; }

        public IQAgent_DashBoardPrevSummaryModel PrevIQAgentSummary { get; set; }

        public Dictionary<string, long> DmaMentionMapList { get; set; }

        public Dictionary<string, long> CanadaMentionMapList { get; set; }
    }

    public class IQAgent_DashboardDmaMapModel
    {
        public string Dma_Num { get; set; }

        public string DMA_Name { get; set; }

        public string Mentions { get; set; }
    }

    public class IQAgent_DashBoardPrevSummaryModel
    {
        public bool IsEnoughData { get; set; }
        public List<IQAgent_ComparisionValues> ListOfIQAgentPrevSummary { get; set; }
    }

    public class IQAgent_ComparisionValues
    {
        public Int64 NoOfDocs { get; set; }
        public Int64 TotalAirSeconds { get; set; }
        public Int64 NoOfHits { get; set; }
        public Int64 Audience { get; set; }
        public Decimal IQMediaValue { get; set; }
        public Int64 PositiveSentiment { get; set; }
        public Int64 NegativeSentiment { get; set; }
        public string MediaType { get; set; }
        public string SubMediaType { get; set; }
    }


}
