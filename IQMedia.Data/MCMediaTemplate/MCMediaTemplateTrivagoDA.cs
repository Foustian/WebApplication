using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using IQMedia.Data.Base;
using IQMedia.Model;

namespace IQMedia.Data.MCMediaTemplate
{
    public class MCMediaTemplateTrivagoDA : MCMediaTemplateBaseDA, IMCMediaTemplate, IDataAccess
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

        public List<IQArchive_Filter> GetFilterCategory(DateTime? FromDate, DateTime? ToDate, string SubMediaType, string SearchTerm, int? SentimentFlag, string CategoryXml, string reportGUID)
        {
            try
            {
                SubMediaType = !string.IsNullOrWhiteSpace(SubMediaType) ? SubMediaType : null;
                SearchTerm = !string.IsNullOrWhiteSpace(SearchTerm) ? SearchTerm : null;
                CategoryXml = !string.IsNullOrWhiteSpace(CategoryXml) ? CategoryXml : null;

                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@FromDate", DbType.DateTime, FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.DateTime, ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubMediaType", DbType.String, SubMediaType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchTerm", DbType.String, SearchTerm, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SentimentFlag", DbType.Int32, SentimentFlag, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryXml", DbType.Xml, CategoryXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReportGUID", DbType.String, reportGUID, ParameterDirection.Input));
                DataSet dataset = DataAccess.GetDataSet("usp_v5_IQ_Report_MCMediaTemplateTrivago_SelectCategoryFilter", dataTypeList);

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
                            case "CategoryFilter":
                                GetFilterResults(dt, "CategoryFilter", mcMediaReportModel.FilterResults);
                                break;
                            case "DateFilter":
                                mcMediaReportModel.FilterResults.Dates = new List<string>();
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (!dr["MediaDate"].Equals(DBNull.Value))
                                    {
                                        mcMediaReportModel.FilterResults.Dates.Add(Convert.ToDateTime(dr["MediaDate"]).ToString("MM/dd/yyyy"));
                                    }
                                }
                                break;
                            case "PosSentimentFilter":
                                long posSentCount = 0;
                                if (Int64.TryParse(Convert.ToString(dt.Rows[0]["NumResults"]), out posSentCount))
                                {
                                    mcMediaReportModel.FilterResults.PositiveSentiment = posSentCount;
                                }
                                break;
                            case "NegSentimentFilter":
                                long negSentCount = 0;
                                if (Int64.TryParse(Convert.ToString(dt.Rows[0]["NumResults"]), out negSentCount))
                                {
                                    mcMediaReportModel.FilterResults.NegativeSentiment = negSentCount;
                                }
                                break;
                            case "NullSentimentFilter":
                                long nullSentCount = 0;
                                if (Int64.TryParse(Convert.ToString(dt.Rows[0]["NumResults"]), out nullSentCount))
                                {
                                    mcMediaReportModel.FilterResults.NullSentiment = nullSentCount;
                                }
                                break;
                        }
                    }
                }

                // Group results
                mcMediaReportModel.GroupTier1Results = new List<MCMediaReport_GroupTier1Model>()
                {
                    new MCMediaReport_GroupTier1Model()
                    {
                        IsEnabled = false,
                        GroupTier2Results = new List<MCMediaReport_GroupTier2Model>()
                        {
                            new MCMediaReport_GroupTier2Model()
                            {
                                IsEnabled = false,
                                GroupTier3Results = new List<MCMediaReport_GroupTier3Model>()
                                {
                                    new MCMediaReport_GroupTier3Model()
                                    {
                                        IsEnabled = false,
                                        MediaResults = lstMediaResults.OrderByDescending(o => o.MediaDate).ToList()
                                    }
                                }
                            }
                        }
                    }
                };
            }

            mcMediaReportModel.HasResults = lstMediaResults.Count > 0;
            return mcMediaReportModel;
        }
    }
}
