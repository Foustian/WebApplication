using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using IQMedia.Model;

namespace IQMedia.Data.MCMediaTemplate
{
    class MCMediaTemplate1DA : MCMediaTemplateBaseDA, IMCMediaTemplate
    {
        public MCMediaReportModel GetMCMediaResultsForReport(Guid reportGuid, MCMediaSearchModel searchSettings, ReportTypeSettings templateSettings, string currentUrl)
        {
            try
            {
                DataSet dataset = ExecuteSP(reportGuid, searchSettings, templateSettings.SPName);

                return FillReportResults(dataset, currentUrl);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private MCMediaReportModel FillReportResults(DataSet dataSet, string currentUrl)
        {
            MCMediaReportModel mcMediaReportModel = new MCMediaReportModel();
            List<IQArchive_MediaModel> lstMediaResults = new List<IQArchive_MediaModel>();

            if (dataSet != null)
            {
                foreach (DataTable dt in dataSet.Tables)
                {
                    if (dt.Rows.Count > 0 && dt.Columns.Contains("TableType"))
                    {
                        switch (Convert.ToString(dt.Rows[0]["TableType"]))
                        {
                            case "HeaderInfo":
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dt.Columns.Contains("ReportImage") && !dr["ReportImage"].Equals(DBNull.Value))
                                    {
                                        mcMediaReportModel.CustomHeader = Convert.ToString(dataSet.Tables[0].Rows[0]["ReportImage"]);
                                    }
                                    if (dt.Columns.Contains("MasterClientGuid") && !dr["MasterClientGuid"].Equals(DBNull.Value))
                                    {
                                        mcMediaReportModel.MasterClientGuid = new Guid(Convert.ToString(dataSet.Tables[0].Rows[0]["MasterClientGuid"]));
                                    }
                                }
                                break;
                            case "TV":
                                lstMediaResults.AddRange(GetTVResults(dt, currentUrl));
                                break;
                            case "NM":
                                lstMediaResults.AddRange(GetNMResults(dt));
                                break;
                            case "SM":
                                lstMediaResults.AddRange(GetSMResults(dt));
                                break;
                            case "TW":
                                lstMediaResults.AddRange(GetTWResults(dt));
                                break;
                            case "PM":
                                lstMediaResults.AddRange(GetBLPMResults(dt));
                                break;
                            case "PQ":
                                lstMediaResults.AddRange(GetPQResults(dt));
                                break;
                            case "TM":
                                lstMediaResults.AddRange(GetTMResults(dt));
                                break;
                            case "MS":
                                lstMediaResults.AddRange(GetMSResults(dt));
                                break;
                            case "IQR":
                                lstMediaResults.AddRange(GetIQRadioResults(dt, currentUrl));
                                break;
                        }
                    }
                }

                // Group results
                mcMediaReportModel.GroupTier1Results = new List<MCMediaReport_GroupTier1Model>();
                foreach (string clientName in lstMediaResults.Select(s => s.ClientName).Distinct().OrderBy(o => o))
                {
                    MCMediaReport_GroupTier1Model groupTier1Model = new MCMediaReport_GroupTier1Model()
                    {
                        GroupName = clientName,
                        IsEnabled = true
                    };

                    groupTier1Model.GroupTier2Results = new List<MCMediaReport_GroupTier2Model>();

                    MCMediaReport_GroupTier2Model groupTier2Model = new MCMediaReport_GroupTier2Model()
                    {
                        IsEnabled = false,
                        GroupTier3Results = new List<MCMediaReport_GroupTier3Model>()
                            {
                                new MCMediaReport_GroupTier3Model()
                                {
                                    IsEnabled = false,
                                    MediaResults = lstMediaResults.Where(w => w.ClientName == clientName)
                                                        .OrderByDescending(o => new DateTime(o.MediaDate.Ticks - o.MediaDate.Ticks % TimeSpan.TicksPerMinute)) // Round to the minute
                                                        .ThenByDescending(o => o.Audience)
                                                        .ThenBy(o => o.MediaType)
                                                        .ToList()
                                }
                            }
                    };

                    groupTier1Model.GroupTier2Results.Add(groupTier2Model);

                    mcMediaReportModel.GroupTier1Results.Add(groupTier1Model);
                }
            }

            mcMediaReportModel.HasResults = lstMediaResults.Count > 0;
            return mcMediaReportModel;
        }
    }
}
