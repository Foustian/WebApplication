using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using IQMedia.Data.Base;
using IQMedia.Model;

namespace IQMedia.Data.MCMediaTemplate
{
    public class MCMediaTemplate2DA : MCMediaTemplateBaseDA, IMCMediaTemplate, IDataAccess
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

        public List<IQArchive_Filter> GetFilterCategory(string ClientGUID, DateTime? FromDate, DateTime? ToDate, string SubMediaType, string SearchTerm, string CategoryXml, string reportGUID)
        {
            try
            {
                ClientGUID = !string.IsNullOrWhiteSpace(ClientGUID) ? ClientGUID : null;
                SubMediaType = !string.IsNullOrWhiteSpace(SubMediaType) ? SubMediaType : null;
                SearchTerm = !string.IsNullOrWhiteSpace(SearchTerm) ? SearchTerm : null;
                CategoryXml = !string.IsNullOrWhiteSpace(CategoryXml) ? CategoryXml : null;

                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGUID", DbType.String, ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.DateTime, FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.DateTime, ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubMediaType", DbType.String, SubMediaType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchTerm", DbType.String, SearchTerm, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryXml", DbType.Xml, CategoryXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReportGUID", DbType.String, reportGUID, ParameterDirection.Input));
                DataSet dataset = DataAccess.GetDataSet("usp_v5_IQ_Report_MCMediaTemplate2_SelectCategoryFilter", dataTypeList);

                List<IQArchive_Filter> lstCategories = new List<IQArchive_Filter>();
                if (dataset != null && dataset.Tables.Count > 0)
                {
                    foreach (DataRow dr in dataset.Tables[0].Rows)
                    {
                        IQArchive_Filter categoryFilter = new IQArchive_Filter();

                        if (!dr["CategoryName"].Equals(DBNull.Value))
                        {
                            categoryFilter.CategoryName = Convert.ToString(dr["CategoryName"]);
                        }
                        if (!dr["CategoryCount"].Equals(DBNull.Value))
                        {
                            categoryFilter.RecordCount = Convert.ToInt64(dr["CategoryCount"]);
                        }

                        lstCategories.Add(categoryFilter);
                    }
                }

                return lstCategories;
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

            mcMediaReportModel.FilterResults = new MCMediaReport_FilterModel()
            {
                MediaTypes = new List<MCMediaReport_MediaTypeFilter>(),
                Clients = new List<MCMediaReport_Filter>(),
                Categories = new List<MCMediaReport_Filter>(),
                Dates = new List<string>()
            };

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
                            case "SubMediaTypeFilter":
                                GetFilterResults(dt, "SubMediaTypeFilter", mcMediaReportModel.FilterResults);
                                break;
                            case "ClientFilter":
                                GetFilterResults(dt, "ClientFilter", mcMediaReportModel.FilterResults);
                                break;
                            case "CategoryFilter":
                                GetFilterResults(dt, "CategoryFilter", mcMediaReportModel.FilterResults);
                                break;
                            case "DateFilter":
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (!dr["MediaDate"].Equals(DBNull.Value))
                                    {
                                        mcMediaReportModel.FilterResults.Dates.Add(Convert.ToDateTime(dr["MediaDate"]).ToString("MM/dd/yyyy"));
                                    }
                                }
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
