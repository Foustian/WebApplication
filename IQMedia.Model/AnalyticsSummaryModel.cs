using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    [Serializable]
    public class AnalyticsSummaryModel
    {
        public Int64 Number_Docs { get; set; }
        public DateTime SummaryDateTime { get; set; }
        public DateTime LocalDateTime { get; set; }
        public DateTime GMTDateTime { get; set; }
        public string DayTimeDisplay { get; set; }
        public string DayTimeID { get; set; }
        public string DayPartDisplay { get; set; }
        public string DayPartID { get; set; }
        public string MediaType { get; set; }
        public Int64 Number_Of_Hits { get; set; }
        public Int64 Audience { get; set; }
        public decimal IQMediaValue { get; set; }
        public string SubMediaType { get; set; }
        public string SMTDisplayName { get; set; }
        public string Query_Name { get; set; }
        public Int64 SearchRequestID { get; set; }
        public Int64 PositiveSentiment { get; set; }
        public Int64 NegativeSentiment { get; set; }
        public Int64 ReadEarned { get; set; }
        public Int64 SeenEarned { get; set; }
        public Int64 SeenPaid { get; set; }
        public Int64 HeardEarned { get; set; }
        public Int64 HeardPaid { get; set; }
        public Int64 MaleAudience { get; set; }
        public Int64 FemaleAudience { get; set; }
        public Int64 AM18_20 { get; set; }
        public Int64 AM21_24 { get; set; }
        public Int64 AM25_34 { get; set; }
        public Int64 AM35_49 { get; set; }
        public Int64 AM50_54 { get; set; }
        public Int64 AM55_64 { get; set; }
        public Int64 AM65_Plus { get; set; }
        public Int64 AF18_20 { get; set; }
        public Int64 AF21_24 { get; set; }
        public Int64 AF25_34 { get; set; }
        public Int64 AF35_49 { get; set; }
        public Int64 AF50_54 { get; set; }
        public Int64 AF55_64 { get; set; }
        public Int64 AF65_Plus { get; set; }
        public string CampaignName { get; set; }
        public Int64 CampaignID { get; set; }
        public Int64 CampaignOffset { get; set; }
        public string Market { get; set; }
        public Int64? MarketID { get; set; }
        public string Networks { get; set; }
        public string Shows { get; set; }
        public decimal AdSpend
        {
            get { return this._SeenAdSpend + this._HeardAdSpend; }
        }

        decimal _SeenAdSpend
        {
            get
            {
                if (this.SeenPaid == 0)
                {
                    return 0;
                }
                else
                {
                    return ((this.SeenPaid / 30) * this.IQMediaValue * (decimal)1.15);
                }
            }
        }
        decimal _HeardAdSpend
        {
            get { return (this.IQMediaValue * (decimal)0.85); }
        }
    }
}
