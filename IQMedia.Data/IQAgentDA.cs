using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using IQMedia.Model;
using System.Data;
using IQMedia.Shared.Utility;
using System.Xml.Linq;
using System.Configuration;
using System.Data.SqlClient;

namespace IQMedia.Data
{
    public class IQAgentDA : IDataAccess
    {

        public void GetSearchTermByIQAgentTVResultID(Guid clientGuid, Int64 iqagentTVResultID, out Guid rlVideoGUID, out string searchTerm, out string iqCCKey)
        {
            try
            {
                rlVideoGUID = new Guid();
                searchTerm = string.Empty;
                iqCCKey = string.Empty;
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@IQAgentTVResultID", DbType.Int64, iqagentTVResultID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));
                IDataReader reader = DataAccess.GetDataReader("usp_v5_IQAgent_SearchRequest_SelectSearchTermBy_IQAgent_TVResultsID", dataTypeList);

                while (reader.Read())
                {
                    searchTerm = Convert.ToString(reader["SearchTerm"]);
                    rlVideoGUID = new Guid(Convert.ToString(reader["RL_VideoGUID"]));
                    iqCCKey = Convert.ToString(reader["IQ_CC_Key"]);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQAgent_MediaResultsModel GetIQAgent_MediaResultByID(long ID, Guid ClientGuid, out bool p_IsMissingArticle)
        {
            try
            {
                p_IsMissingArticle = false;
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ID", DbType.Int64, ID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, ClientGuid, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v5_IQAgent_MediaResults_SelectByID", dataTypeList);

                IQAgent_MediaResultsModel objIQAgent_MediaResultsModel = new IQAgent_MediaResultsModel();
                IQAgent_NewsResultsModel objIQAgent_NewsResultsModel = new IQAgent_NewsResultsModel();
                IQAgent_SMResultsModel objIQAgent_SMResultsModel = new IQAgent_SMResultsModel();

                if (dataset != null && dataset.Tables.Count > 0)
                {
                    foreach (DataRow dr in dataset.Tables[0].Rows)
                    {
                        string dataModelType = string.Empty;
                        if (!dr["DataModelType"].Equals(DBNull.Value))
                        {
                            dataModelType = Convert.ToString(dr["DataModelType"]);
                        }

                        if (!dr["MediaType"].Equals(DBNull.Value))
                        {
                            objIQAgent_MediaResultsModel.MediaType = Convert.ToString(dr["MediaType"]);
                        }

                        if (!dr["SubMediaType"].Equals(DBNull.Value))
                        {
                            objIQAgent_MediaResultsModel.CategoryType = Convert.ToString(dr["SubMediaType"]);
                        }

                        if (!dr["ArticleID"].Equals(DBNull.Value))
                        {
                            objIQAgent_MediaResultsModel.ArticleID = Convert.ToString(dr["ArticleID"]);
                        }

                        if (!dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            objIQAgent_MediaResultsModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }

                        if (!dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            objIQAgent_MediaResultsModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }


                        if (dataModelType == "NM")
                        {
                            if (!dr["ArticleID"].Equals(DBNull.Value))
                            {
                                objIQAgent_NewsResultsModel.ArticleID = Convert.ToString(dr["ArticleID"]);
                            }

                            if (!dr["Url"].Equals(DBNull.Value))
                            {
                                objIQAgent_NewsResultsModel.ArticleUri = Convert.ToString(dr["Url"]);
                            }

                            if (!dr["harvest_time"].Equals(DBNull.Value))
                            {
                                objIQAgent_NewsResultsModel.Harvest_Time = Convert.ToDateTime(dr["harvest_time"]);
                                objIQAgent_MediaResultsModel.MediaDateTime = Convert.ToDateTime(dr["harvest_time"]);
                            }

                            if (!dr["Title"].Equals(DBNull.Value))
                            {
                                objIQAgent_NewsResultsModel.Title = Convert.ToString(dr["Title"]);
                            }

                            if (!dr["Content"].Equals(DBNull.Value))
                            {
                                objIQAgent_NewsResultsModel.HighlightingText = Convert.ToString(dr["Content"]);
                            }

                            if (!dr["Publication"].Equals(DBNull.Value))
                            {
                                objIQAgent_NewsResultsModel.Publication = Convert.ToString(dr["Publication"]);
                            }

                            if (!dr["CompeteUrl"].Equals(DBNull.Value))
                            {
                                objIQAgent_NewsResultsModel.CompeteUrl = Convert.ToString(dr["CompeteUrl"]);
                            }

                            if (!dr["IQProminence"].Equals(DBNull.Value))
                            {
                                objIQAgent_NewsResultsModel.IQProminence = Convert.ToDecimal(dr["IQProminence"]);
                            }
                            if (!dr["IQProminenceMultiplier"].Equals(DBNull.Value))
                            {
                                objIQAgent_NewsResultsModel.IQProminenceMultiplier = Convert.ToDecimal(dr["IQProminenceMultiplier"]);
                            }

                            objIQAgent_NewsResultsModel.PositiveSentiment = Convert.ToInt32(objIQAgent_MediaResultsModel.PositiveSentiment);
                            objIQAgent_NewsResultsModel.NegativeSentiment = Convert.ToInt32(objIQAgent_MediaResultsModel.NegativeSentiment);

                            if (!dr["IsMissingArticle"].Equals(DBNull.Value))
                            {
                                p_IsMissingArticle = Convert.ToBoolean(dr["IsMissingArticle"]);
                            }
                            objIQAgent_MediaResultsModel.MediaData = objIQAgent_NewsResultsModel;
                        }
                        else if (dataModelType == "SM")
                        {
                            if (!dr["ArticleID"].Equals(DBNull.Value))
                            {
                                objIQAgent_SMResultsModel.ArticleID = Convert.ToString(dr["ArticleID"]);
                            }

                            if (!dr["Url"].Equals(DBNull.Value))
                            {
                                objIQAgent_SMResultsModel.ArticleUri = Convert.ToString(dr["Url"]);
                            }

                            if (!dr["harvest_time"].Equals(DBNull.Value))
                            {
                                objIQAgent_SMResultsModel.ItemHarvestDate = Convert.ToDateTime(dr["harvest_time"]);
                                objIQAgent_MediaResultsModel.MediaDateTime = Convert.ToDateTime(dr["harvest_time"]);
                            }

                            if (!dr["Title"].Equals(DBNull.Value))
                            {
                                objIQAgent_SMResultsModel.Description = Convert.ToString(dr["Title"]);
                            }

                            if (!dr["Content"].Equals(DBNull.Value))
                            {
                                objIQAgent_SMResultsModel.HighlightingText = Convert.ToString(dr["Content"]);
                            }

                            if (!dr["Publication"].Equals(DBNull.Value))
                            {
                                objIQAgent_SMResultsModel.HomeLink = Convert.ToString(dr["Publication"]);
                            }

                            if (!dr["CompeteUrl"].Equals(DBNull.Value))
                            {
                                objIQAgent_SMResultsModel.CompeteURL = Convert.ToString(dr["CompeteUrl"]);
                            }

                            if (!dr["CompeteUrl"].Equals(DBNull.Value))
                            {
                                objIQAgent_SMResultsModel.CompeteURL = Convert.ToString(dr["CompeteUrl"]);
                            }

                            if (!dr["feedClass"].Equals(DBNull.Value))
                            {
                                objIQAgent_SMResultsModel.SourceCategory = Convert.ToString(dr["feedClass"]);
                            }

                            objIQAgent_SMResultsModel.PositiveSentiment = Convert.ToInt32(objIQAgent_MediaResultsModel.PositiveSentiment);
                            objIQAgent_SMResultsModel.NegativeSentiment = Convert.ToInt32(objIQAgent_MediaResultsModel.NegativeSentiment);

                            if (!dr["IsMissingArticle"].Equals(DBNull.Value))
                            {
                                p_IsMissingArticle = Convert.ToBoolean(dr["IsMissingArticle"]);
                            }

                            if (!dr["HighlightingText"].Equals(DBNull.Value))
                            {
                                HighlightedSMOutput highlightedSMOutput = new HighlightedSMOutput();
                                objIQAgent_SMResultsModel.HighlightedSMOutput = (HighlightedSMOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedSMOutput);
                            }

                            if (!dr["ThumbUrl"].Equals(DBNull.Value))
                            {
                                objIQAgent_SMResultsModel.ThumbUrl = Convert.ToString(dr["ThumbUrl"]);
                            }


                            if (!dr["IQProminence"].Equals(DBNull.Value))
                            {
                                objIQAgent_SMResultsModel.IQProminence = Convert.ToDecimal(dr["IQProminence"]);
                            }
                            if (!dr["IQProminenceMultiplier"].Equals(DBNull.Value))
                            {
                                objIQAgent_SMResultsModel.IQProminenceMultiplier = Convert.ToDecimal(dr["IQProminenceMultiplier"]);
                            }

                            if (!dr["ArticleStats"].Equals(DBNull.Value) && !String.IsNullOrWhiteSpace(Convert.ToString(dr["ArticleStats"])))
                            {
                                ArticleStatsModel statsModel = new ArticleStatsModel();
                                objIQAgent_SMResultsModel.ArticleStats = (ArticleStatsModel)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["ArticleStats"]), statsModel);
                            }
                            objIQAgent_MediaResultsModel.MediaData = objIQAgent_SMResultsModel;
                        }
                    }
                }

                return objIQAgent_MediaResultsModel;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IQAgentReport_WithoutAuthentication GetIQAgent_MediaResultReportByReportGuid(string ReportGuid, int IQAgentReportMaxRecordDisplay, bool IsSourceEmail)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ReportGUID", DbType.String, ReportGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MaxDisplayRecord", DbType.Int32, IQAgentReportMaxRecordDisplay, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsSourceEmail", DbType.Boolean, IsSourceEmail, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQ_Report_SelectForIQAgentReportByReportGUID", dataTypeList);

                IQAgentReport_WithoutAuthentication objIQAgentReport_WithoutAuthentication = FillIQAgent_MediaResultForReport(dataset);

                return objIQAgentReport_WithoutAuthentication;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IQAgentReport GetIQAgent_MediaResultReportByReportGuid(string ReportGuid, Int64? SearchRequestID, string MediaType, int IQAgentReportMaxRecordDisplay, bool IsSourceEmail)
        {
            try
            {
                MediaType = string.IsNullOrEmpty(MediaType) ? null : MediaType;
                string useRollupStr = "false";
                
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ReportGUID", DbType.String, ReportGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MaxDisplayRecord", DbType.Int32, IQAgentReportMaxRecordDisplay, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchRequestID", DbType.Int64, SearchRequestID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MediaType", DbType.String, MediaType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsSourceEmail", DbType.Boolean, IsSourceEmail, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@UseRollUp", DbType.Boolean, false, ParameterDirection.Output));
                
                DataSet dataset = DataAccess.GetDataSetWithOutParam("usp_v5_IQ_Report_SelectIQAgentReportByReportGUID", dataTypeList,out useRollupStr);               

                IQAgentReport objIQAgentReport_WithoutAuthentication = FillIQAgent_ResultForReport(dataset);

                bool useRollup = false;

                Boolean.TryParse(useRollupStr, out useRollup);
                objIQAgentReport_WithoutAuthentication.UseRollup = useRollup;

                return objIQAgentReport_WithoutAuthentication;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IQAgentReport_RawMediaPlayer GetIQAgentReport_DetailsToPlayRawMedia(string IQAgentResultUrl, out int Offset)
        {
            try
            {
                Offset = 0;
                bool IsFirstOffset = true;
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@IQAgentResultUrl", DbType.String, IQAgentResultUrl, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_IQ_Report_SelectToPlayRawMediaByIQAgentResultUrl", dataTypeList);
                IQAgentReport_RawMediaPlayer objIQAgentReport_RawMediaPlayer = new IQAgentReport_RawMediaPlayer();

                if (dataset != null && dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
                {
                    objIQAgentReport_RawMediaPlayer.RawMediaGuid = Convert.ToString(dataset.Tables[0].Rows[0]["RawMediaGuid"]);
                    //objIQAgentReport_RawMediaPlayer.CC_Highlight = string.Empty;
                    //if (dataset.Tables[0].Rows[0]["CC_Highlight"] != null)
                    //{
                    //    XElement CC_Highlight = XElement.Parse(Convert.ToString(dataset.Tables[0].Rows[0]["CC_Highlight"]));
                    //    StringBuilder strngCaption = new StringBuilder();
                    //    foreach (XElement ele in CC_Highlight.Descendants("ClosedCaption"))
                    //    {
                    //        XElement eleOffset = ele.Element("Offset");
                    //        XElement eleText = ele.Element("Text");
                    //        strngCaption.Append("<div class=\"hit\" onclick=\"setSeekPoint(" + (Convert.ToInt32(eleOffset.Value) - Convert.ToInt32(ConfigurationSettings.AppSettings["RawMediaCaptionDelay"].ToString())) + ");\">"
                    //                                    + "<div class=\"boldgray\">" + formatOffset(Convert.ToInt32(eleOffset.Value)) + "</div>"
                    //                                    + "<div class=\"caption\">" + eleText.Value + "</div>"
                    //                                + "</div>");

                    //        if (IsFirstOffset)
                    //        {
                    //            Offset = Convert.ToInt32(eleOffset.Value);
                    //            IsFirstOffset = false;
                    //        }
                    //    }
                    //    objIQAgentReport_RawMediaPlayer.CC_Highlight = strngCaption.ToString();
                    //}

                    if (!dataset.Tables[0].Rows[0]["Expiry_Date"].Equals(DBNull.Value))
                    {
                        objIQAgentReport_RawMediaPlayer.ExpiryDate = Convert.ToDateTime(dataset.Tables[0].Rows[0]["Expiry_Date"]);
                    }

                    if (!dataset.Tables[0].Rows[0]["SearchTerm"].Equals(DBNull.Value))
                    {
                        objIQAgentReport_RawMediaPlayer.SearchTerm = Convert.ToString(dataset.Tables[0].Rows[0]["SearchTerm"]);
                    }

                    if (dataset.Tables[0].Columns.Contains("DataModelType") && !dataset.Tables[0].Rows[0]["DataModelType"].Equals(DBNull.Value))
                    {
                        objIQAgentReport_RawMediaPlayer.DataModelType = Convert.ToString(dataset.Tables[0].Rows[0]["DataModelType"]);
                    }
                }

                return objIQAgentReport_RawMediaPlayer;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<IQAgent_SearchRequestModel> SelectIQAgentSearchRequestByClientGuid(string ClientGuid, bool includeDeleted)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGuid", DbType.String, ClientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IncludeDeleted", DbType.Boolean, includeDeleted, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQAgent_SearchRequest_SelectByClientGuid", dataTypeList);

                List<IQAgent_SearchRequestModel> lstIQAgentSearchRequest = new List<IQAgent_SearchRequestModel>();

                if (dataset != null && dataset.Tables.Count > 0)
                {
                    foreach (DataRow dr in dataset.Tables[0].Rows)
                    {
                        IQAgent_SearchRequestModel objIQAgent_SearchRequestModel = new IQAgent_SearchRequestModel();
                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objIQAgent_SearchRequestModel.ID = Convert.ToInt64(dr["ID"]);
                        }
                        if (!dr["Query_Name"].Equals(DBNull.Value))
                        {
                            objIQAgent_SearchRequestModel.QueryName = Convert.ToString(dr["Query_Name"]);
                        }
                        if (!dr["SearchTerm"].Equals(DBNull.Value))
                        {
                            objIQAgent_SearchRequestModel.SearchTerm = Convert.ToString(dr["SearchTerm"]);
                        }
                        if (!dr["Query_Version"].Equals(DBNull.Value))
                        {
                            objIQAgent_SearchRequestModel.Query_Version = Convert.ToInt32(dr["Query_Version"]);
                        }
                        if (!dr["ModifiedDate"].Equals(DBNull.Value))
                        {
                            objIQAgent_SearchRequestModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                        }
                        if (!dr["IsRestrictedMedia"].Equals(DBNull.Value))
                        {
                            objIQAgent_SearchRequestModel.IsRestrictedMedia = Convert.ToBoolean(dr["IsRestrictedMedia"]);
                        }

                        objIQAgent_SearchRequestModel.IsActive = Convert.ToInt16(dr["IsActive"]);

                        lstIQAgentSearchRequest.Add(objIQAgent_SearchRequestModel);
                    }
                }

                return lstIQAgentSearchRequest;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IQAgent_SearchRequestModel SelectIQAgentSearchRequestByID(string ClientGuid, long ID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGuid", DbType.String, ClientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ID", DbType.Int64, ID, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQAgent_SearchRequest_SelectByID", dataTypeList);

                IQAgent_SearchRequestModel objIQAgentSearchRequest = new IQAgent_SearchRequestModel();

                if (dataset != null && dataset.Tables.Count > 0)
                {
                    foreach (DataRow dr in dataset.Tables[0].Rows)
                    {
                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objIQAgentSearchRequest.ID = Convert.ToInt64(dr["ID"]);
                        }
                        if (!dr["Query_Name"].Equals(DBNull.Value))
                        {
                            objIQAgentSearchRequest.QueryName = Convert.ToString(dr["Query_Name"]);
                        }
                        if (!dr["Query_Version"].Equals(DBNull.Value))
                        {
                            objIQAgentSearchRequest.Query_Version = Convert.ToInt32(dr["Query_Version"]);
                        }
                        if (!dr["SearchTerm"].Equals(DBNull.Value))
                        {
                            objIQAgentSearchRequest.SearchTerm = Convert.ToString(dr["SearchTerm"]);
                        }
                        if (!dr["ModifiedDate"].Equals(DBNull.Value))
                        {
                            objIQAgentSearchRequest.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                        }
                    }
                }

                return objIQAgentSearchRequest;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string InsertIQAgentSearchRequest(string ClientGuid, string QueryName, string SearchXML)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGuid", DbType.String, ClientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Query_Name", DbType.String, QueryName, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchTerm", DbType.Xml, SearchXML, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Output", DbType.Int32, 0, ParameterDirection.Output));

                string result = DataAccess.ExecuteNonQuery("usp_v5_IQAgent_SearchRequest_Insert", dataTypeList);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string UpdateIQAgentSearchRequest(string ClientGuid, long IQAgentSearchRequestID, string QueryName, string SearchXML)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@IQAgent_SearchRequestID", DbType.Int64, IQAgentSearchRequestID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.String, ClientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Query_Name", DbType.String, QueryName, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchTerm", DbType.Xml, SearchXML, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Output", DbType.Int32, 0, ParameterDirection.Output));

                string result = DataAccess.ExecuteNonQuery("usp_v5_IQAgent_SearchRequest_Update", dataTypeList);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IQAgentSearchRequest_DropDown SelectAllDropdown(string ClientGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGuid", DbType.String, ClientGuid, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQAgent_SearchRequest_SelectAllDropDown", dataTypeList);

                IQAgentSearchRequest_DropDown objIQAgentDropDown = new IQAgentSearchRequest_DropDown();
                objIQAgentDropDown.TV_DMAList = new List<IQ_Dma>();
                objIQAgentDropDown.TV_ClassList = new List<IQ_Class>();
                objIQAgentDropDown.TV_StationList = new List<IQ_Station>();
                objIQAgentDropDown.TV_AffiliateList = new List<Station_Affil>();
                objIQAgentDropDown.NM_CategoryList = new List<IQAgentDropDown_NM_Category>();
                objIQAgentDropDown.NM_GenereList = new List<IQAgentDropDown_NM_Genere>();
                objIQAgentDropDown.NM_PublicationCategoryList = new List<IQAgentDropDown_NM_PublicationCategory>();
                objIQAgentDropDown.NM_RegionList = new List<IQAgentDropDown_NM_Region>();
                objIQAgentDropDown.SM_SourceCategoryList = new List<IQAgentDropDown_SM_SourceCategory>();
                objIQAgentDropDown.SM_SourceTypeList = new List<IQAgentDropDown_SM_SourceType>();
                objIQAgentDropDown.CountryList = new Dictionary<string, string>();
                objIQAgentDropDown.LanguageList = new List<string>();

                objIQAgentDropDown.TV_RegionList = new List<IQ_Region>();
                objIQAgentDropDown.TV_CountryList = new List<IQ_Country>();

                if (dataset != null && dataset.Tables.Count > 0)
                {
                    // Table[0] Represents TV_DMA
                    foreach (DataRow dr in dataset.Tables[0].Rows)
                    {
                        IQ_Dma objDMA = new IQ_Dma();
                        if (!dr["Dma_Num"].Equals(DBNull.Value))
                        {
                            objDMA.Num = Convert.ToString(dr["Dma_Num"]);
                        }
                        if (!dr["Dma_Name"].Equals(DBNull.Value))
                        {
                            objDMA.Name = Convert.ToString(dr["Dma_Name"]);
                        }
                        objIQAgentDropDown.TV_DMAList.Add(objDMA);
                    }

                    // Table[1] Represents TV_Class
                    foreach (DataRow dr in dataset.Tables[1].Rows)
                    {
                        IQ_Class objClass = new IQ_Class();
                        if (!dr["IQ_Class_Num"].Equals(DBNull.Value))
                        {
                            objClass.Num = Convert.ToString(dr["IQ_Class_Num"]);
                        }
                        if (!dr["IQ_Class"].Equals(DBNull.Value))
                        {
                            objClass.Name = Convert.ToString(dr["IQ_Class"]);
                        }
                        objIQAgentDropDown.TV_ClassList.Add(objClass);
                    }

                    // Table[2] Represents TV_Station
                    foreach (DataRow dr in dataset.Tables[2].Rows)
                    {
                        IQ_Station objStation = new IQ_Station();

                        if (!dr["IQ_Station_ID"].Equals(DBNull.Value))
                        {
                            objStation.IQ_Station_ID = Convert.ToString(dr["IQ_Station_ID"]);
                        }

                        if (!dr["Station_Call_Sign"].Equals(DBNull.Value))
                        {
                            objStation.Station_Call_Sign = Convert.ToString(dr["Station_Call_Sign"]);
                        }

                        if (!objIQAgentDropDown.TV_AffiliateList.Select(a => a.Name).Contains(dr["Station_Affil"]))
                        {
                            Station_Affil objAffiliate = new Station_Affil();
                            objAffiliate.Name = Convert.ToString(dr["Station_Affil"]);

                            objIQAgentDropDown.TV_AffiliateList.Add(objAffiliate);
                        }

                        objIQAgentDropDown.TV_StationList.Add(objStation);
                    }

                    objIQAgentDropDown.TV_AffiliateList = objIQAgentDropDown.TV_AffiliateList.OrderBy(a => a.Name).ToList();

                    // Table[3] Represents NM_Genere
                    foreach (DataRow dr in dataset.Tables[3].Rows)
                    {
                        IQAgentDropDown_NM_Genere objGenere = new IQAgentDropDown_NM_Genere();
                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objGenere.ID = Convert.ToInt32(dr["ID"]);
                        }
                        if (!dr["Label"].Equals(DBNull.Value))
                        {
                            objGenere.Label = Convert.ToString(dr["Label"]);
                        }
                        objIQAgentDropDown.NM_GenereList.Add(objGenere);
                    }

                    // Table[4] Represents NM_Category
                    foreach (DataRow dr in dataset.Tables[4].Rows)
                    {
                        IQAgentDropDown_NM_Category objCategory = new IQAgentDropDown_NM_Category();
                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objCategory.ID = Convert.ToInt32(dr["ID"]);
                        }
                        if (!dr["Label"].Equals(DBNull.Value))
                        {
                            objCategory.Label = Convert.ToString(dr["Label"]);
                        }
                        objIQAgentDropDown.NM_CategoryList.Add(objCategory);
                    }

                    // Table[5] Represents NM_PublicationCategory
                    foreach (DataRow dr in dataset.Tables[5].Rows)
                    {
                        IQAgentDropDown_NM_PublicationCategory objPublicationCategory = new IQAgentDropDown_NM_PublicationCategory();
                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objPublicationCategory.ID = Convert.ToInt32(dr["ID"]);
                        }
                        if (!dr["Label"].Equals(DBNull.Value))
                        {
                            objPublicationCategory.Label = Convert.ToString(dr["Label"]);
                        }
                        objIQAgentDropDown.NM_PublicationCategoryList.Add(objPublicationCategory);
                    }

                    // Table[6] Represents NM_Region
                    foreach (DataRow dr in dataset.Tables[6].Rows)
                    {
                        IQAgentDropDown_NM_Region objRegion = new IQAgentDropDown_NM_Region();
                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objRegion.ID = Convert.ToInt32(dr["ID"]);
                        }
                        if (!dr["Label"].Equals(DBNull.Value))
                        {
                            objRegion.Label = Convert.ToString(dr["Label"]);
                        }
                        objIQAgentDropDown.NM_RegionList.Add(objRegion);
                    }

                    // Table[7] Represents SM_SourceCategory
                    foreach (DataRow dr in dataset.Tables[7].Rows)
                    {
                        IQAgentDropDown_SM_SourceCategory objSourceCatgory = new IQAgentDropDown_SM_SourceCategory();
                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objSourceCatgory.ID = Convert.ToInt32(dr["ID"]);
                        }
                        if (!dr["Label"].Equals(DBNull.Value))
                        {
                            objSourceCatgory.Label = Convert.ToString(dr["Label"]);
                        }
                        if (!dr["Value"].Equals(DBNull.Value))
                        {
                            objSourceCatgory.Value = Convert.ToString(dr["Value"]);
                        }
                        objIQAgentDropDown.SM_SourceCategoryList.Add(objSourceCatgory);
                    }

                    // Table[8] Represents SM_SourceType
                    foreach (DataRow dr in dataset.Tables[8].Rows)
                    {
                        IQAgentDropDown_SM_SourceType objSourceType = new IQAgentDropDown_SM_SourceType();
                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objSourceType.ID = Convert.ToInt32(dr["ID"]);
                        }
                        if (!dr["Label"].Equals(DBNull.Value))
                        {
                            objSourceType.Label = Convert.ToString(dr["Label"]);
                        }
                        objIQAgentDropDown.SM_SourceTypeList.Add(objSourceType);
                    }

                    // Table[9] Represents MO Country                    
                    foreach (DataRow dr in dataset.Tables[9].Rows)
                    {
                        objIQAgentDropDown.CountryList.Add(Convert.ToString(dr["Name"]), Convert.ToString(dr["Code"]));
                    }

                    // Table[10] Represents MO Country                    
                    foreach (DataRow dr in dataset.Tables[10].Rows)
                    {
                        objIQAgentDropDown.LanguageList.Add(Convert.ToString(dr["Name"]));
                    }

                    // Table[11] Represents TV Region
                    foreach (DataRow dr in dataset.Tables[11].Rows)
                    {
                        IQ_Region objRegion = new IQ_Region();
                        if (!dr["Region_Num"].Equals(DBNull.Value))
                        {
                            objRegion.Num = Convert.ToInt32(dr["Region_Num"]);
                        }
                        if (!dr["Region"].Equals(DBNull.Value))
                        {
                            objRegion.Name = Convert.ToString(dr["Region"]);
                        }
                        objIQAgentDropDown.TV_RegionList.Add(objRegion);
                    }

                    // Table[12] Represents TV Country
                    foreach (DataRow dr in dataset.Tables[12].Rows)
                    {
                        IQ_Country objCountry = new IQ_Country();
                        if (!dr["Country_Num"].Equals(DBNull.Value))
                        {
                            objCountry.Num = Convert.ToInt32(dr["Country_Num"]);
                        }
                        if (!dr["Country"].Equals(DBNull.Value))
                        {
                            objCountry.Name = Convert.ToString(dr["Country"]);
                        }
                        objIQAgentDropDown.TV_CountryList.Add(objCountry);
                    }
                }

                return objIQAgentDropDown;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private IQAgentReport_WithoutAuthentication FillIQAgent_MediaResultForReport(DataSet dataSet)
        {
            IQAgentReport_WithoutAuthentication objIQAgentReport_WithoutAuthentication = new IQAgentReport_WithoutAuthentication();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {

                if (dataSet.Tables[0] != null)
                {
                    objIQAgentReport_WithoutAuthentication.ReportTitle = Convert.ToString(dataSet.Tables[0].Rows[0]["ReportTitle"]);

                    if (dataSet.Tables[0].Columns.Contains("ReportImage") && !dataSet.Tables[0].Rows[0]["ReportImage"].Equals(DBNull.Value))
                    {
                        objIQAgentReport_WithoutAuthentication.CustomHeader = Convert.ToString(dataSet.Tables[0].Rows[0]["ReportImage"]);
                    }
                    if (dataSet.Tables[0].Columns.Contains("ClientGuid") && !dataSet.Tables[0].Rows[0]["ClientGuid"].Equals(DBNull.Value))
                    {
                        objIQAgentReport_WithoutAuthentication.ClientGuid = new Guid(Convert.ToString(dataSet.Tables[0].Rows[0]["ClientGuid"]));
                    }
                }
                objIQAgentReport_WithoutAuthentication.MediaResults = new List<IQAgent_MediaResultsModel>();

                if (dataSet.Tables[1] != null)
                {
                    // Represents TV
                    foreach (DataRow dr in dataSet.Tables[1].Rows)
                    {
                        IQAgent_TVResultsModel iQAgent_TVResultsModel = new IQAgent_TVResultsModel();
                        IQAgent_MediaResultsModel iQAgent_MediaResultsModel = new IQAgent_MediaResultsModel();

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.ID = Convert.ToInt64(dr["ID"]);
                        }
                        if (!dr["_MediaID"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.ID = Convert.ToInt64(dr["_MediaID"]);
                        }

                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.Title120 = Convert.ToString(dr["Title"]);
                        }

                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.Date = Convert.ToDateTime(dr["MediaDate"]);
                            iQAgent_MediaResultsModel.MediaDateTime = Convert.ToDateTime(dr["MediaDate"]);
                        }

                        if (!dr["HighlightingText"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.higlightedCC = Convert.ToString(dr["HighlightingText"]);
                            if (!string.IsNullOrWhiteSpace(iQAgent_TVResultsModel.higlightedCC))
                            {
                                HighlightedCCOutput highlightedCCOutput = new HighlightedCCOutput();
                                iQAgent_TVResultsModel.highlightedCCOutput = (HighlightedCCOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedCCOutput);
                            }
                        }

                        if (!dr["Category"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.CategoryType = Convert.ToString(dr["Category"]);
                        }

                        if (!dr["IQAgentResultUrl"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.IQAgentResultUrl = Convert.ToString(dr["IQAgentResultUrl"]);
                        }

                        if (!dr["RawMediaThumbUrl"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.RawMediaThumbUrl = Convert.ToString(dr["RawMediaThumbUrl"]);
                        }

                        /*if (dataSet.Tables[1].Columns.Contains("RL_Station") && !dr["RL_Station"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.RL_Station = Convert.ToString(dr["RL_Station"]);

                            iQAgent_TVResultsModel.StationLogo = "http://" + currentUrl + "/StationLogoImages/" + iQAgent_TVResultsModel.RL_Station + ".jpg";
                        }*/

                        if (!dr["TimeZone"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.TimeZone = Convert.ToString(dr["TimeZone"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("RL_Market") && !dr["RL_Market"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.Market = Convert.ToString(dr["RL_Market"]);
                        }

                        if (!dr["Nielsen_Audience"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.Nielsen_Audience = Convert.ToInt32(dr["Nielsen_Audience"]);
                        }

                        if (!dr["IQAdShareValue"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.IQAdShareValue = Convert.ToDecimal(dr["IQAdShareValue"]);
                        }

                        if (!dr["Nielsen_Result"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.Nielsen_Result = Convert.ToString(dr["Nielsen_Result"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("RL_DateTime") && !dr["RL_DateTime"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.LocalDateTime = Convert.ToDateTime(dr["RL_DateTime"]);
                        }

                        iQAgent_MediaResultsModel.MediaType = "TV";
                        iQAgent_MediaResultsModel.MediaData = iQAgent_TVResultsModel;

                        if (iQAgent_MediaResultsModel.ID > 0)
                        {
                            objIQAgentReport_WithoutAuthentication.MediaResults.Add(iQAgent_MediaResultsModel);
                        }
                    }
                }

                if (dataSet.Tables[2] != null)
                {

                    // Represents NM

                    foreach (DataRow dr in dataSet.Tables[2].Rows)
                    {
                        IQAgent_NewsResultsModel iQAgent_NewsResultsModel = new IQAgent_NewsResultsModel();
                        IQAgent_MediaResultsModel iQAgent_MediaResultsModel = new IQAgent_MediaResultsModel();

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.ID = Convert.ToInt64(dr["ID"]);
                        }

                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel.Title = Convert.ToString(dr["Title"]);
                        }

                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel.Harvest_Time = Convert.ToDateTime(dr["MediaDate"]);
                            iQAgent_MediaResultsModel.MediaDateTime = Convert.ToDateTime(dr["MediaDate"]);
                        }

                        if (!dr["HighlightingText"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel.HighlightingText = Convert.ToString(dr["HighlightingText"]);
                            if (!string.IsNullOrWhiteSpace(iQAgent_NewsResultsModel.HighlightingText))
                            {
                                HighlightedNewsOutput highlightedNewsOutput = new HighlightedNewsOutput();
                                iQAgent_NewsResultsModel.HighlightedNewsOutput = (HighlightedNewsOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedNewsOutput);
                            }
                        }
                        if (!dr["Category"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.CategoryType = Convert.ToString(dr["Category"]);
                        }

                        if (!dr["Url"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel.Url = Convert.ToString(dr["Url"]);
                        }

                        if (dataSet.Tables[2].Columns.Contains("Publication") && !dr["Publication"].Equals(DBNull.Value))
                        {
                            Uri aPublisherUri;
                            iQAgent_NewsResultsModel.Publication = Uri.TryCreate(Convert.ToString(dr["Publication"]), UriKind.Absolute, out aPublisherUri) ? aPublisherUri.Host.Replace("www.", string.Empty) : Convert.ToString(dr["Publication"]);
                        }

                        if (!dr["Compete_Audience"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel.Compete_Audience = Convert.ToInt32(dr["Compete_Audience"]);
                        }

                        if (!dr["IQAdShareValue"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel.IQAdShareValue = Convert.ToDecimal(dr["IQAdShareValue"]);
                        }

                        if (!dr["Compete_Result"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel.Compete_Result = Convert.ToString(dr["Compete_Result"]);
                        }

                        if (dataSet.Tables[2].Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }

                        if (dataSet.Tables[2].Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }

                        if (dataSet.Tables[2].Columns.Contains("IQLicense") && !dr["IQLicense"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel.IQLicense = Convert.ToInt16(dr["IQLicense"]);
                        }

                        iQAgent_MediaResultsModel.MediaType = "NM";
                        iQAgent_MediaResultsModel.MediaData = iQAgent_NewsResultsModel;
                        if (iQAgent_MediaResultsModel.ID > 0)
                        {
                            objIQAgentReport_WithoutAuthentication.MediaResults.Add(iQAgent_MediaResultsModel);
                        }
                    }
                }

                if (dataSet.Tables[3] != null)
                {

                    // Represents SM

                    foreach (DataRow dr in dataSet.Tables[3].Rows)
                    {

                        IQAgent_SMResultsModel iQAgent_SMResultsModel = new IQAgent_SMResultsModel();
                        IQAgent_MediaResultsModel iQAgent_MediaResultsModel = new IQAgent_MediaResultsModel();

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.ID = Convert.ToInt64(dr["ID"]);
                        }

                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.Description = Convert.ToString(dr["Title"]);
                        }

                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.ItemHarvestDate = Convert.ToDateTime(dr["MediaDate"]);
                            iQAgent_MediaResultsModel.MediaDateTime = Convert.ToDateTime(dr["MediaDate"]);
                        }
                        if (!dr["Category"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.CategoryType = Convert.ToString(dr["Category"]);
                        }

                        if (!dr["HighlightingText"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.HighlightingText = Convert.ToString(dr["HighlightingText"]);
                            if (!string.IsNullOrWhiteSpace(iQAgent_SMResultsModel.HighlightingText))
                            {
                                HighlightedSMOutput highlightedSMOutput = new HighlightedSMOutput();
                                iQAgent_SMResultsModel.HighlightedSMOutput = (HighlightedSMOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedSMOutput);
                            }
                        }
                        if (!dr["Url"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.Link = Convert.ToString(dr["Url"]);
                        }

                        if (dataSet.Tables[3].Columns.Contains("homelink") && !dr["homelink"].Equals(DBNull.Value))
                        {
                            Uri aPublisherUri;
                            iQAgent_SMResultsModel.HomeLink = Uri.TryCreate(Convert.ToString(dr["homelink"]), UriKind.Absolute, out aPublisherUri) ? aPublisherUri.Host.Replace("www.", string.Empty) : Convert.ToString(dr["homelink"]);
                        }

                        if (!dr["Compete_Audience"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.Compete_Audience = Convert.ToInt32(dr["Compete_Audience"]);
                        }

                        if (!dr["IQAdShareValue"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.IQAdShareValue = Convert.ToDecimal(dr["IQAdShareValue"]);
                        }

                        if (!dr["Compete_Result"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.Compete_Result = Convert.ToString(dr["Compete_Result"]);
                        }

                        if (dataSet.Tables[3].Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }

                        if (dataSet.Tables[3].Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }

                        iQAgent_MediaResultsModel.MediaType = "SM";
                        iQAgent_MediaResultsModel.MediaData = iQAgent_SMResultsModel;
                        if (iQAgent_MediaResultsModel.ID > 0)
                        {
                            objIQAgentReport_WithoutAuthentication.MediaResults.Add(iQAgent_MediaResultsModel);
                        }
                    }
                }

                if (dataSet.Tables[4] != null)
                {
                    // Represents TW

                    foreach (DataRow dr in dataSet.Tables[4].Rows)
                    {
                        IQAgent_TwitterResultsModel iQAgent_TwitterResultsModel = new IQAgent_TwitterResultsModel();
                        IQAgent_MediaResultsModel iQAgent_MediaResultsModel = new IQAgent_MediaResultsModel();

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.ID = Convert.ToInt64(dr["ID"]);
                        }
                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            iQAgent_TwitterResultsModel.Tweet_DateTime = Convert.ToDateTime(dr["MediaDate"]);
                            iQAgent_MediaResultsModel.MediaDateTime = Convert.ToDateTime(dr["MediaDate"]);
                        }
                        if (!dr["HighlightingText"].Equals(DBNull.Value))
                        {
                            iQAgent_TwitterResultsModel.Summary = Convert.ToString(dr["HighlightingText"]);
                        }
                        if (!dr["Category"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.CategoryType = Convert.ToString(dr["Category"]);
                        }
                        if (!dr["actor_link"].Equals(DBNull.Value))
                        {
                            iQAgent_TwitterResultsModel.Actor_Link = Convert.ToString(dr["actor_link"]);
                        }
                        if (!dr["actor_image"].Equals(DBNull.Value))
                        {
                            iQAgent_TwitterResultsModel.Actor_Image = Convert.ToString(dr["actor_image"]);
                        }
                        if (!dr["actor_preferredname"].Equals(DBNull.Value))
                        {
                            iQAgent_TwitterResultsModel.Actor_PreferredName = Convert.ToString(dr["actor_preferredname"]);
                        }
                        if (!dr["actor_displayname"].Equals(DBNull.Value))
                        {
                            iQAgent_TwitterResultsModel.Actor_DisplayName = Convert.ToString(dr["actor_displayname"]);
                        }
                        if (!dr["gnip_klout_score"].Equals(DBNull.Value))
                        {
                            iQAgent_TwitterResultsModel.KlOutScore = Convert.ToInt64(dr["gnip_klout_score"]);
                        }
                        if (!dr["actor_followersCount"].Equals(DBNull.Value))
                        {
                            iQAgent_TwitterResultsModel.Actor_FollowersCount = Convert.ToInt64(dr["actor_followersCount"]);
                        }
                        if (!dr["actor_friendsCount"].Equals(DBNull.Value))
                        {
                            iQAgent_TwitterResultsModel.Actor_FriendsCount = Convert.ToInt64(dr["actor_friendsCount"]);
                        }

                        if (dataSet.Tables[4].Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }

                        if (dataSet.Tables[4].Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }

                        iQAgent_MediaResultsModel.MediaType = "TW";
                        iQAgent_MediaResultsModel.MediaData = iQAgent_TwitterResultsModel;
                        if (iQAgent_MediaResultsModel.ID > 0)
                        {
                            objIQAgentReport_WithoutAuthentication.MediaResults.Add(iQAgent_MediaResultsModel);
                        }
                    }
                }

                if (dataSet.Tables[5] != null)
                {

                    // Represents SM

                    foreach (DataRow dr in dataSet.Tables[5].Rows)
                    {

                        IQAgent_TVEyesResultsModel iQAgent_TVEyesResultsModel = new IQAgent_TVEyesResultsModel();
                        IQAgent_MediaResultsModel iQAgent_MediaResultsModel = new IQAgent_MediaResultsModel();

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.ID = Convert.ToInt64(dr["ID"]);
                        }

                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            iQAgent_TVEyesResultsModel.Title = Convert.ToString(dr["Title"]);
                        }

                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            iQAgent_TVEyesResultsModel.UTCDateTime = Convert.ToDateTime(dr["MediaDate"]);
                            iQAgent_MediaResultsModel.MediaDateTime = Convert.ToDateTime(dr["MediaDate"]);
                        }
                        if (!dr["Category"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.CategoryType = Convert.ToString(dr["Category"]);
                        }

                        if (!dr["HighlightingText"].Equals(DBNull.Value))
                        {
                            iQAgent_TVEyesResultsModel.HighlightingText = Convert.ToString(dr["HighlightingText"]);
                            iQAgent_TVEyesResultsModel.HighlightingText = Convert.ToString(dr["HighlightingText"]);
                        }

                        if (dataSet.Tables[5].Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }

                        if (dataSet.Tables[5].Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }


                        if (dataSet.Tables[5].Columns.Contains("StationID") && !dr["StationID"].Equals(DBNull.Value))
                        {
                            iQAgent_TVEyesResultsModel.StationID = Convert.ToString(dr["StationID"]);
                        }


                        if (dataSet.Tables[5].Columns.Contains("Market") && !dr["Market"].Equals(DBNull.Value))
                        {
                            iQAgent_TVEyesResultsModel.Market = Convert.ToString(dr["Market"]);
                        }

                        if (dataSet.Tables[5].Columns.Contains("DMARank") && !dr["DMARank"].Equals(DBNull.Value))
                        {
                            iQAgent_TVEyesResultsModel.DMARank = Convert.ToString(dr["DMARank"]);
                        }

                        iQAgent_MediaResultsModel.MediaType = "TM";
                        iQAgent_MediaResultsModel.MediaData = iQAgent_TVEyesResultsModel;
                        if (iQAgent_MediaResultsModel.ID > 0)
                        {
                            objIQAgentReport_WithoutAuthentication.MediaResults.Add(iQAgent_MediaResultsModel);
                        }
                    }
                }

                if (dataSet.Tables.Count > 6 && dataSet.Tables[6] != null)
                {

                    DataTable pmDataTable = dataSet.Tables[6];

                    foreach (DataRow dr in dataSet.Tables[6].Rows)
                    {

                        IQAgent_BLPMResultsModel iQAgent_BLPMResultsModel = new IQAgent_BLPMResultsModel();
                        IQAgent_MediaResultsModel iQAgent_MediaResultsModel = new IQAgent_MediaResultsModel();


                        if (pmDataTable.Columns.Contains("ID") && !dr["ID"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.ID = Convert.ToInt32(dr["ID"]);
                        }

                        if (pmDataTable.Columns.Contains("HighlightingText") && !dr["HighlightingText"].Equals(DBNull.Value))
                        {
                            iQAgent_BLPMResultsModel.HighlightingText = GetHilightedText_PM(Convert.ToString(dr["HighlightingText"]));
                        }

                        if (pmDataTable.Columns.Contains("PubDate") && !dr["PubDate"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.MediaDateTime = Convert.ToDateTime(dr["PubDate"]);
                        }

                        if (pmDataTable.Columns.Contains("Category") && !dr["Category"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.CategoryType = Convert.ToString(dr["Category"]);
                        }

                        if (pmDataTable.Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }

                        if (pmDataTable.Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }

                        if (pmDataTable.Columns.Contains("Title") && !dr["Title"].Equals(DBNull.Value))
                        {
                            iQAgent_BLPMResultsModel.Title = Convert.ToString(dr["Title"]);
                        }

                        if (pmDataTable.Columns.Contains("Circulation") && !dr["Circulation"].Equals(DBNull.Value))
                        {
                            iQAgent_BLPMResultsModel.Circulation = Convert.ToInt32(dr["Circulation"]);
                        }

                        if (pmDataTable.Columns.Contains("FileLocation") && !dr["FileLocation"].Equals(DBNull.Value))
                        {
                            iQAgent_BLPMResultsModel.FileLocation = Convert.ToString(dr["FileLocation"]);
                        }

                        if (pmDataTable.Columns.Contains("Pub_Name") && !dr["Pub_Name"].Equals(DBNull.Value))
                        {
                            iQAgent_BLPMResultsModel.Pub_Name = Convert.ToString(dr["Pub_Name"]);
                        }

                        iQAgent_MediaResultsModel.MediaType = "PM";
                        iQAgent_MediaResultsModel.MediaData = iQAgent_BLPMResultsModel;

                        if (iQAgent_MediaResultsModel.ID > 0)
                        {
                            objIQAgentReport_WithoutAuthentication.MediaResults.Add(iQAgent_MediaResultsModel);
                        }
                    }
                }
            }

            return objIQAgentReport_WithoutAuthentication;
        }

        private IQAgentReport FillIQAgent_ResultForReport(DataSet dataSet)
        {
            IQAgentReport objIQAgentReport_WithoutAuthentication = new IQAgentReport();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {

                if (dataSet.Tables[0] != null)
                {
                    objIQAgentReport_WithoutAuthentication.ReportTitle = Convert.ToString(dataSet.Tables[0].Rows[0]["ReportTitle"]);

                    if (dataSet.Tables[0].Columns.Contains("ReportImage") && !dataSet.Tables[0].Rows[0]["ReportImage"].Equals(DBNull.Value))
                    {
                        objIQAgentReport_WithoutAuthentication.CustomHeader = Convert.ToString(dataSet.Tables[0].Rows[0]["ReportImage"]);
                    }
                    if (dataSet.Tables[0].Columns.Contains("ClientGuid") && !dataSet.Tables[0].Rows[0]["ClientGuid"].Equals(DBNull.Value))
                    {
                        objIQAgentReport_WithoutAuthentication.ClientGuid = new Guid(Convert.ToString(dataSet.Tables[0].Rows[0]["ClientGuid"]));
                    }
                }
                objIQAgentReport_WithoutAuthentication.Results = new List<IQAgentReport_SearchRequestModel>();


                if (dataSet.Tables.Count > 1 && dataSet.Tables[1] != null)
                {
                    foreach (DataRow dr in dataSet.Tables[1].Rows)
                    {
                        IQAgentReport_SearchRequestModel iQAgentReport_SearchRequestModel = new IQAgentReport_SearchRequestModel();
                        iQAgentReport_SearchRequestModel.MediaResults = new List<IQAgent_MediaResultsModel>();

                        if (dataSet.Tables[1].Columns.Contains("SearchRequestID") && !dr["SearchRequestID"].Equals(DBNull.Value))
                        {
                            iQAgentReport_SearchRequestModel.SearchRequestID = Convert.ToInt64(dr["SearchRequestID"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("Query_Name") && !dr["Query_Name"].Equals(DBNull.Value))
                        {
                            iQAgentReport_SearchRequestModel.QueryName = Convert.ToString(dr["Query_Name"]);
                        }

                        objIQAgentReport_WithoutAuthentication.Results.Add(iQAgentReport_SearchRequestModel);
                    }

                }

                if (dataSet.Tables.Count > 2 && dataSet.Tables[2] != null)
                {
                    // Represents TV
                    foreach (DataRow dr in dataSet.Tables[2].Rows)
                    {
                        IQAgent_TVResultsModel iQAgent_TVResultsModel = new IQAgent_TVResultsModel();
                        IQAgent_MediaResultsModel iQAgent_MediaResultsModel = new IQAgent_MediaResultsModel();

                        Int64 searchRequestID = 0;

                        if (!dr["SearchRequestID"].Equals(DBNull.Value))
                        {
                            searchRequestID = Convert.ToInt64(dr["SearchRequestID"]);
                        }

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.ID = Convert.ToInt64(dr["ID"]);
                        }
                        if (!dr["_MediaID"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.ID = Convert.ToInt64(dr["_MediaID"]);
                        }

                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.Title120 = Convert.ToString(dr["Title"]);
                        }

                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.Date = Convert.ToDateTime(dr["MediaDate"]);
                            iQAgent_MediaResultsModel.MediaDateTime = Convert.ToDateTime(dr["MediaDate"]);
                        }

                        if (!dr["HighlightingText"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.higlightedCC = Convert.ToString(dr["HighlightingText"]);
                            if (!string.IsNullOrWhiteSpace(iQAgent_TVResultsModel.higlightedCC))
                            {
                                HighlightedCCOutput highlightedCCOutput = new HighlightedCCOutput();
                                iQAgent_TVResultsModel.highlightedCCOutput = (HighlightedCCOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedCCOutput);
                            }
                        }

                        if (!dr["Category"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.CategoryType = Convert.ToString(dr["Category"]);
                        }

                        if (!dr["IQAgentResultUrl"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.IQAgentResultUrl = Convert.ToString(dr["IQAgentResultUrl"]);
                        }

                        if (!dr["RawMediaThumbUrl"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.RawMediaThumbUrl = Convert.ToString(dr["RawMediaThumbUrl"]);
                        }

                        /*if (dataSet.Tables[1].Columns.Contains("RL_Station") && !dr["RL_Station"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.RL_Station = Convert.ToString(dr["RL_Station"]);

                            iQAgent_TVResultsModel.StationLogo = "http://" + currentUrl + "/StationLogoImages/" + iQAgent_TVResultsModel.RL_Station + ".jpg";
                        }*/

                        if (!dr["TimeZone"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.TimeZone = Convert.ToString(dr["TimeZone"]);
                        }

                        if (dataSet.Tables[2].Columns.Contains("Market") && !dr["Market"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.Market = Convert.ToString(dr["Market"]);
                        }

                        if (!dr["Nielsen_Audience"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.Nielsen_Audience = Convert.ToInt32(dr["Nielsen_Audience"]);
                        }

                        if (!dr["IQAdShareValue"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.IQAdShareValue = Convert.ToDecimal(dr["IQAdShareValue"]);
                        }

                        if (!dr["Nielsen_Result"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.Nielsen_Result = Convert.ToString(dr["Nielsen_Result"]);
                        }

                        if (dataSet.Tables[2].Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }

                        if (dataSet.Tables[2].Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }

                        if (dataSet.Tables[2].Columns.Contains("LocalDate") && !dr["LocalDate"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel.LocalDateTime = Convert.ToDateTime(dr["LocalDate"]).AddHours(Convert.ToInt16(dr["LocalTime"]) / 100);
                        }

                        if (dataSet.Tables[2].Columns.Contains("ParentID") && !dr["ParentID"].Equals(DBNull.Value))
                        {
                            iQAgent_TVResultsModel._ParentID = Convert.ToInt32(dr["ParentID"]);
                        }

                        iQAgent_MediaResultsModel.MediaType = Convert.ToString(dr["Category"]);
                        iQAgent_MediaResultsModel.MediaData = iQAgent_TVResultsModel;

                        if (iQAgent_MediaResultsModel.ID > 0)
                        {
                            objIQAgentReport_WithoutAuthentication.Results.FirstOrDefault(a => a.SearchRequestID == searchRequestID).MediaResults.Add(iQAgent_MediaResultsModel);
                        }
                    }
                }

                if (dataSet.Tables.Count > 3 && dataSet.Tables[3] != null)
                {

                    // Represents NM

                    foreach (DataRow dr in dataSet.Tables[3].Rows)
                    {
                        IQAgent_NewsResultsModel iQAgent_NewsResultsModel = new IQAgent_NewsResultsModel();
                        IQAgent_MediaResultsModel iQAgent_MediaResultsModel = new IQAgent_MediaResultsModel();

                        Int64 searchRequestID = 0;
                        if (!dr["SearchRequestID"].Equals(DBNull.Value))
                        {
                            searchRequestID = Convert.ToInt64(dr["SearchRequestID"]);
                        }

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.ID = Convert.ToInt64(dr["ID"]);
                        }

                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel.Title = Convert.ToString(dr["Title"]);
                        }

                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel.Harvest_Time = Convert.ToDateTime(dr["MediaDate"]);
                            iQAgent_MediaResultsModel.MediaDateTime = Convert.ToDateTime(dr["MediaDate"]);
                        }

                        if (!dr["HighlightingText"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel.HighlightingText = Convert.ToString(dr["HighlightingText"]);
                            if (!string.IsNullOrWhiteSpace(iQAgent_NewsResultsModel.HighlightingText))
                            {
                                HighlightedNewsOutput highlightedNewsOutput = new HighlightedNewsOutput();
                                iQAgent_NewsResultsModel.HighlightedNewsOutput = (HighlightedNewsOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedNewsOutput);
                            }
                        }
                        if (!dr["Category"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.CategoryType = Convert.ToString(dr["Category"]);
                        }

                        if (!dr["Url"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel.Url = Convert.ToString(dr["Url"]);
                        }

                        if (dataSet.Tables[3].Columns.Contains("Publication") && !dr["Publication"].Equals(DBNull.Value))
                        {
                            Uri aPublisherUri;
                            iQAgent_NewsResultsModel.Publication = Uri.TryCreate(Convert.ToString(dr["Publication"]), UriKind.Absolute, out aPublisherUri) ? aPublisherUri.Host.Replace("www.", string.Empty) : Convert.ToString(dr["Publication"]);
                        }

                        if (!dr["Compete_Audience"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel.Compete_Audience = Convert.ToInt32(dr["Compete_Audience"]);
                        }

                        if (!dr["IQAdShareValue"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel.IQAdShareValue = Convert.ToDecimal(dr["IQAdShareValue"]);
                        }

                        if (!dr["Compete_Result"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel.Compete_Result = Convert.ToString(dr["Compete_Result"]);
                        }

                        if (dataSet.Tables[3].Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }

                        if (dataSet.Tables[3].Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }

                        if (dataSet.Tables[3].Columns.Contains("IQLicense") && !dr["IQLicense"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel.IQLicense = Convert.ToInt16(dr["IQLicense"]);
                        }

                        if (dataSet.Tables[3].Columns.Contains("ParentID") && !dr["ParentID"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel._ParentID = Convert.ToInt32(dr["ParentID"]);
                        }

                        iQAgent_MediaResultsModel.MediaType = Convert.ToString(dr["Category"]);
                        iQAgent_MediaResultsModel.MediaData = iQAgent_NewsResultsModel;
                        if (iQAgent_MediaResultsModel.ID > 0)
                        {
                            objIQAgentReport_WithoutAuthentication.Results.FirstOrDefault(a => a.SearchRequestID == searchRequestID).MediaResults.Add(iQAgent_MediaResultsModel);
                        }
                    }
                }

                if (dataSet.Tables.Count > 4 && dataSet.Tables[4] != null)
                {

                    // Represents Social Media

                    foreach (DataRow dr in dataSet.Tables[4].Rows)
                    {

                        IQAgent_SMResultsModel iQAgent_SMResultsModel = new IQAgent_SMResultsModel();
                        IQAgent_MediaResultsModel iQAgent_MediaResultsModel = new IQAgent_MediaResultsModel();

                        Int64 searchRequestID = 0;
                        if (!dr["SearchRequestID"].Equals(DBNull.Value))
                        {
                            searchRequestID = Convert.ToInt64(dr["SearchRequestID"]);
                        }

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.ID = Convert.ToInt64(dr["ID"]);
                        }
                        if (!dr["Category"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.CategoryType = Convert.ToString(dr["Category"]);
                        }
                        if (dataSet.Tables[4].Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }
                        if (dataSet.Tables[4].Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }

                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.Description = Convert.ToString(dr["Title"]);
                        }
                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.ItemHarvestDate = Convert.ToDateTime(dr["MediaDate"]);
                            iQAgent_MediaResultsModel.MediaDateTime = Convert.ToDateTime(dr["MediaDate"]);
                        }
                        if (!dr["HighlightingText"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.HighlightingText = Convert.ToString(dr["HighlightingText"]);
                            if (!string.IsNullOrWhiteSpace(iQAgent_SMResultsModel.HighlightingText))
                            {
                                HighlightedSMOutput highlightedSMOutput = new HighlightedSMOutput();
                                iQAgent_SMResultsModel.HighlightedSMOutput = (HighlightedSMOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedSMOutput);
                            }
                        }
                        if (!dr["ThumbUrl"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.ThumbUrl = Convert.ToString(dr["ThumbUrl"]);
                        }
                        if (!dr["Url"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.Link = Convert.ToString(dr["Url"]);
                        }
                        if (dataSet.Tables[4].Columns.Contains("homelink") && !dr["homelink"].Equals(DBNull.Value))
                        {
                            Uri aPublisherUri;
                            iQAgent_SMResultsModel.HomeLink = Uri.TryCreate(Convert.ToString(dr["homelink"]), UriKind.Absolute, out aPublisherUri) ? aPublisherUri.Host.Replace("www.", string.Empty) : Convert.ToString(dr["homelink"]);
                        }
                        if (!dr["Compete_Audience"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.Compete_Audience = Convert.ToInt32(dr["Compete_Audience"]);
                        }
                        if (!dr["IQAdShareValue"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.IQAdShareValue = Convert.ToDecimal(dr["IQAdShareValue"]);
                        }
                        if (!dr["Compete_Result"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.Compete_Result = Convert.ToString(dr["Compete_Result"]);
                        }
                        if (!dr["ArticleStats"].Equals(DBNull.Value) && !String.IsNullOrWhiteSpace(Convert.ToString(dr["ArticleStats"])))
                        {
                            ArticleStatsModel statsModel = new ArticleStatsModel();
                            iQAgent_SMResultsModel.ArticleStats = (ArticleStatsModel)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["ArticleStats"]), statsModel);
                        }


                        iQAgent_MediaResultsModel.MediaType = Convert.ToString(dr["Category"]);
                        iQAgent_MediaResultsModel.MediaData = iQAgent_SMResultsModel;
                        if (iQAgent_MediaResultsModel.ID > 0)
                        {
                            objIQAgentReport_WithoutAuthentication.Results.FirstOrDefault(a => a.SearchRequestID == searchRequestID).MediaResults.Add(iQAgent_MediaResultsModel);
                        }
                    }
                }


                if (dataSet.Tables.Count > 5 && dataSet.Tables[5] != null)
                {

                    // Represents FB

                    foreach (DataRow dr in dataSet.Tables[5].Rows)
                    {

                        IQAgent_SMResultsModel iQAgent_SMResultsModel = new IQAgent_SMResultsModel();
                        IQAgent_MediaResultsModel iQAgent_MediaResultsModel = new IQAgent_MediaResultsModel();

                        Int64 searchRequestID = 0;
                        if (!dr["SearchRequestID"].Equals(DBNull.Value))
                        {
                            searchRequestID = Convert.ToInt64(dr["SearchRequestID"]);
                        }

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.ID = Convert.ToInt64(dr["ID"]);
                        }
                        if (!dr["Category"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.CategoryType = Convert.ToString(dr["Category"]);
                        }
                        if (dataSet.Tables[5].Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }
                        if (dataSet.Tables[5].Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }

                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.Description = Convert.ToString(dr["Title"]);
                        }
                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.ItemHarvestDate = Convert.ToDateTime(dr["MediaDate"]);
                            iQAgent_MediaResultsModel.MediaDateTime = Convert.ToDateTime(dr["MediaDate"]);
                        }
                        if (!dr["HighlightingText"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.HighlightingText = Convert.ToString(dr["HighlightingText"]);
                            if (!string.IsNullOrWhiteSpace(iQAgent_SMResultsModel.HighlightingText))
                            {
                                HighlightedSMOutput highlightedSMOutput = new HighlightedSMOutput();
                                iQAgent_SMResultsModel.HighlightedSMOutput = (HighlightedSMOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedSMOutput);
                            }
                        }
                        if (!dr["ThumbUrl"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.ThumbUrl = Convert.ToString(dr["ThumbUrl"]);
                        }
                        if (!dr["Url"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.Link = Convert.ToString(dr["Url"]);
                        }
                        if (dataSet.Tables[5].Columns.Contains("homelink") && !dr["homelink"].Equals(DBNull.Value))
                        {
                            Uri aPublisherUri;
                            iQAgent_SMResultsModel.HomeLink = Uri.TryCreate(Convert.ToString(dr["homelink"]), UriKind.Absolute, out aPublisherUri) ? aPublisherUri.Host.Replace("www.", string.Empty) : Convert.ToString(dr["homelink"]);
                        }
                        if (!dr["Compete_Audience"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.Compete_Audience = Convert.ToInt32(dr["Compete_Audience"]);
                        }
                        if (!dr["IQAdShareValue"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.IQAdShareValue = Convert.ToDecimal(dr["IQAdShareValue"]);
                        }
                        if (!dr["Compete_Result"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.Compete_Result = Convert.ToString(dr["Compete_Result"]);
                        }
                        if (!dr["ArticleStats"].Equals(DBNull.Value) && !String.IsNullOrWhiteSpace(Convert.ToString(dr["ArticleStats"])))
                        {
                            ArticleStatsModel statsModel = new ArticleStatsModel();
                            iQAgent_SMResultsModel.ArticleStats = (ArticleStatsModel)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["ArticleStats"]), statsModel);
                        }


                        iQAgent_MediaResultsModel.MediaType = Convert.ToString(dr["Category"]);
                        iQAgent_MediaResultsModel.MediaData = iQAgent_SMResultsModel;
                        if (iQAgent_MediaResultsModel.ID > 0)
                        {
                            objIQAgentReport_WithoutAuthentication.Results.FirstOrDefault(a => a.SearchRequestID == searchRequestID).MediaResults.Add(iQAgent_MediaResultsModel);
                        }
                    }
                }

                if (dataSet.Tables.Count > 6 && dataSet.Tables[6] != null)
                {

                    // Represents IG

                    foreach (DataRow dr in dataSet.Tables[6].Rows)
                    {

                        IQAgent_SMResultsModel iQAgent_SMResultsModel = new IQAgent_SMResultsModel();
                        IQAgent_MediaResultsModel iQAgent_MediaResultsModel = new IQAgent_MediaResultsModel();

                        Int64 searchRequestID = 0;
                        if (!dr["SearchRequestID"].Equals(DBNull.Value))
                        {
                            searchRequestID = Convert.ToInt64(dr["SearchRequestID"]);
                        }

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.ID = Convert.ToInt64(dr["ID"]);
                        }
                        if (!dr["Category"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.CategoryType = Convert.ToString(dr["Category"]);
                        }
                        if (dataSet.Tables[6].Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }
                        if (dataSet.Tables[6].Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }

                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.Description = Convert.ToString(dr["Title"]);
                        }
                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.ItemHarvestDate = Convert.ToDateTime(dr["MediaDate"]);
                            iQAgent_MediaResultsModel.MediaDateTime = Convert.ToDateTime(dr["MediaDate"]);
                        }
                        if (!dr["HighlightingText"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.HighlightingText = Convert.ToString(dr["HighlightingText"]);
                            if (!string.IsNullOrWhiteSpace(iQAgent_SMResultsModel.HighlightingText))
                            {
                                HighlightedSMOutput highlightedSMOutput = new HighlightedSMOutput();
                                iQAgent_SMResultsModel.HighlightedSMOutput = (HighlightedSMOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedSMOutput);
                            }
                        }
                        if (!dr["ThumbUrl"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.ThumbUrl = Convert.ToString(dr["ThumbUrl"]);
                        }
                        if (!dr["Url"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.Link = Convert.ToString(dr["Url"]);
                        }
                        if (dataSet.Tables[6].Columns.Contains("homelink") && !dr["homelink"].Equals(DBNull.Value))
                        {
                            Uri aPublisherUri;
                            iQAgent_SMResultsModel.HomeLink = Uri.TryCreate(Convert.ToString(dr["homelink"]), UriKind.Absolute, out aPublisherUri) ? aPublisherUri.Host.Replace("www.", string.Empty) : Convert.ToString(dr["homelink"]);
                        }
                        if (!dr["Compete_Audience"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.Compete_Audience = Convert.ToInt32(dr["Compete_Audience"]);
                        }
                        if (!dr["IQAdShareValue"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.IQAdShareValue = Convert.ToDecimal(dr["IQAdShareValue"]);
                        }
                        if (!dr["Compete_Result"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.Compete_Result = Convert.ToString(dr["Compete_Result"]);
                        }
                        if (!dr["ArticleStats"].Equals(DBNull.Value) && !String.IsNullOrWhiteSpace(Convert.ToString(dr["ArticleStats"])))
                        {
                            ArticleStatsModel statsModel = new ArticleStatsModel();
                            iQAgent_SMResultsModel.ArticleStats = (ArticleStatsModel)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["ArticleStats"]), statsModel);
                        }


                        iQAgent_MediaResultsModel.MediaType = Convert.ToString(dr["Category"]);
                        iQAgent_MediaResultsModel.MediaData = iQAgent_SMResultsModel;
                        if (iQAgent_MediaResultsModel.ID > 0)
                        {
                            objIQAgentReport_WithoutAuthentication.Results.FirstOrDefault(a => a.SearchRequestID == searchRequestID).MediaResults.Add(iQAgent_MediaResultsModel);
                        }
                    }
                }


                if (dataSet.Tables.Count > 7 && dataSet.Tables[7] != null)
                {

                    // Represents Blog

                    foreach (DataRow dr in dataSet.Tables[7].Rows)
                    {

                        IQAgent_SMResultsModel iQAgent_SMResultsModel = new IQAgent_SMResultsModel();
                        IQAgent_MediaResultsModel iQAgent_MediaResultsModel = new IQAgent_MediaResultsModel();

                        Int64 searchRequestID = 0;
                        if (!dr["SearchRequestID"].Equals(DBNull.Value))
                        {
                            searchRequestID = Convert.ToInt64(dr["SearchRequestID"]);
                        }

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.ID = Convert.ToInt64(dr["ID"]);
                        }

                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.Description = Convert.ToString(dr["Title"]);
                        }

                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.ItemHarvestDate = Convert.ToDateTime(dr["MediaDate"]);
                            iQAgent_MediaResultsModel.MediaDateTime = Convert.ToDateTime(dr["MediaDate"]);
                        }
                        if (!dr["Category"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.CategoryType = Convert.ToString(dr["Category"]);
                        }

                        if (!dr["HighlightingText"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.HighlightingText = Convert.ToString(dr["HighlightingText"]);
                            if (!string.IsNullOrWhiteSpace(iQAgent_SMResultsModel.HighlightingText))
                            {
                                HighlightedSMOutput highlightedSMOutput = new HighlightedSMOutput();
                                iQAgent_SMResultsModel.HighlightedSMOutput = (HighlightedSMOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedSMOutput);
                            }
                        }
                        if (!dr["Url"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.Link = Convert.ToString(dr["Url"]);
                        }

                        if (dataSet.Tables[7].Columns.Contains("homelink") && !dr["homelink"].Equals(DBNull.Value))
                        {
                            Uri aPublisherUri;
                            iQAgent_SMResultsModel.HomeLink = Uri.TryCreate(Convert.ToString(dr["homelink"]), UriKind.Absolute, out aPublisherUri) ? aPublisherUri.Host.Replace("www.", string.Empty) : Convert.ToString(dr["homelink"]);
                        }

                        if (!dr["Compete_Audience"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.Compete_Audience = Convert.ToInt32(dr["Compete_Audience"]);
                        }

                        if (!dr["IQAdShareValue"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.IQAdShareValue = Convert.ToDecimal(dr["IQAdShareValue"]);
                        }

                        if (!dr["Compete_Result"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.Compete_Result = Convert.ToString(dr["Compete_Result"]);
                        }

                        if (dataSet.Tables[7].Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }

                        if (dataSet.Tables[7].Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }

                        iQAgent_MediaResultsModel.MediaType = Convert.ToString(dr["Category"]);
                        iQAgent_MediaResultsModel.MediaData = iQAgent_SMResultsModel;
                        if (iQAgent_MediaResultsModel.ID > 0)
                        {
                            objIQAgentReport_WithoutAuthentication.Results.FirstOrDefault(a => a.SearchRequestID == searchRequestID).MediaResults.Add(iQAgent_MediaResultsModel);
                        }
                    }
                }

                if (dataSet.Tables.Count > 8 && dataSet.Tables[8] != null)
                {

                    // Represents Forum

                    foreach (DataRow dr in dataSet.Tables[8].Rows)
                    {

                        IQAgent_SMResultsModel iQAgent_SMResultsModel = new IQAgent_SMResultsModel();
                        IQAgent_MediaResultsModel iQAgent_MediaResultsModel = new IQAgent_MediaResultsModel();

                        Int64 searchRequestID = 0;
                        if (!dr["SearchRequestID"].Equals(DBNull.Value))
                        {
                            searchRequestID = Convert.ToInt64(dr["SearchRequestID"]);
                        }

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.ID = Convert.ToInt64(dr["ID"]);
                        }

                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.Description = Convert.ToString(dr["Title"]);
                        }

                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.ItemHarvestDate = Convert.ToDateTime(dr["MediaDate"]);
                            iQAgent_MediaResultsModel.MediaDateTime = Convert.ToDateTime(dr["MediaDate"]);
                        }
                        if (!dr["Category"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.CategoryType = Convert.ToString(dr["Category"]);
                        }

                        if (!dr["HighlightingText"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.HighlightingText = Convert.ToString(dr["HighlightingText"]);
                            if (!string.IsNullOrWhiteSpace(iQAgent_SMResultsModel.HighlightingText))
                            {
                                HighlightedSMOutput highlightedSMOutput = new HighlightedSMOutput();
                                iQAgent_SMResultsModel.HighlightedSMOutput = (HighlightedSMOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedSMOutput);
                            }
                        }
                        if (!dr["Url"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.Link = Convert.ToString(dr["Url"]);
                        }

                        if (dataSet.Tables[8].Columns.Contains("homelink") && !dr["homelink"].Equals(DBNull.Value))
                        {
                            Uri aPublisherUri;
                            iQAgent_SMResultsModel.HomeLink = Uri.TryCreate(Convert.ToString(dr["homelink"]), UriKind.Absolute, out aPublisherUri) ? aPublisherUri.Host.Replace("www.", string.Empty) : Convert.ToString(dr["homelink"]);
                        }

                        if (!dr["Compete_Audience"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.Compete_Audience = Convert.ToInt32(dr["Compete_Audience"]);
                        }

                        if (!dr["IQAdShareValue"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.IQAdShareValue = Convert.ToDecimal(dr["IQAdShareValue"]);
                        }

                        if (!dr["Compete_Result"].Equals(DBNull.Value))
                        {
                            iQAgent_SMResultsModel.Compete_Result = Convert.ToString(dr["Compete_Result"]);
                        }

                        if (dataSet.Tables[8].Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }

                        if (dataSet.Tables[8].Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }

                        iQAgent_MediaResultsModel.MediaType = Convert.ToString(dr["Category"]);
                        iQAgent_MediaResultsModel.MediaData = iQAgent_SMResultsModel;
                        if (iQAgent_MediaResultsModel.ID > 0)
                        {
                            objIQAgentReport_WithoutAuthentication.Results.FirstOrDefault(a => a.SearchRequestID == searchRequestID).MediaResults.Add(iQAgent_MediaResultsModel);
                        }
                    }
                }

                if (dataSet.Tables.Count > 9 && dataSet.Tables[9] != null)
                {
                    // Represents TW

                    foreach (DataRow dr in dataSet.Tables[9].Rows)
                    {
                        IQAgent_TwitterResultsModel iQAgent_TwitterResultsModel = new IQAgent_TwitterResultsModel();
                        IQAgent_MediaResultsModel iQAgent_MediaResultsModel = new IQAgent_MediaResultsModel();

                        Int64 searchRequestID = 0;
                        if (!dr["SearchRequestID"].Equals(DBNull.Value))
                        {
                            searchRequestID = Convert.ToInt64(dr["SearchRequestID"]);
                        }

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.ID = Convert.ToInt64(dr["ID"]);
                        }
                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            iQAgent_TwitterResultsModel.Tweet_DateTime = Convert.ToDateTime(dr["MediaDate"]);
                            iQAgent_MediaResultsModel.MediaDateTime = Convert.ToDateTime(dr["MediaDate"]);
                        }
                        if (!dr["HighlightingText"].Equals(DBNull.Value))
                        {
                            iQAgent_TwitterResultsModel.Summary = Convert.ToString(dr["HighlightingText"]);

                            if (!string.IsNullOrWhiteSpace(iQAgent_TwitterResultsModel.Summary))
                            {
                                HighlightedTWOutput highlightedTWOutput = new HighlightedTWOutput();
                                iQAgent_TwitterResultsModel.HighlightedOutput = (HighlightedTWOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedTWOutput);
                            }
                        }
                        if (!dr["Category"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.CategoryType = Convert.ToString(dr["Category"]);
                        }
                        if (!dr["TweetID"].Equals(DBNull.Value))
                        {
                            iQAgent_TwitterResultsModel.TweetID = Convert.ToString(dr["TweetID"]);
                        }
                        if (!dr["actor_link"].Equals(DBNull.Value))
                        {
                            iQAgent_TwitterResultsModel.Actor_Link = Convert.ToString(dr["actor_link"]);
                        }
                        if (!dr["actor_image"].Equals(DBNull.Value))
                        {
                            iQAgent_TwitterResultsModel.Actor_Image = Convert.ToString(dr["actor_image"]);
                        }
                        if (!dr["actor_preferredname"].Equals(DBNull.Value))
                        {
                            iQAgent_TwitterResultsModel.Actor_PreferredName = Convert.ToString(dr["actor_preferredname"]);
                        }
                        if (!dr["actor_displayname"].Equals(DBNull.Value))
                        {
                            iQAgent_TwitterResultsModel.Actor_DisplayName = Convert.ToString(dr["actor_displayname"]);
                        }
                        if (!dr["gnip_klout_score"].Equals(DBNull.Value))
                        {
                            iQAgent_TwitterResultsModel.KlOutScore = Convert.ToInt64(dr["gnip_klout_score"]);
                        }
                        if (!dr["actor_followersCount"].Equals(DBNull.Value))
                        {
                            iQAgent_TwitterResultsModel.Actor_FollowersCount = Convert.ToInt64(dr["actor_followersCount"]);
                        }
                        if (!dr["actor_friendsCount"].Equals(DBNull.Value))
                        {
                            iQAgent_TwitterResultsModel.Actor_FriendsCount = Convert.ToInt64(dr["actor_friendsCount"]);
                        }

                        if (dataSet.Tables[9].Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }

                        if (dataSet.Tables[9].Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }

                        iQAgent_MediaResultsModel.MediaType = Convert.ToString(dr["Category"]);
                        iQAgent_MediaResultsModel.MediaData = iQAgent_TwitterResultsModel;
                        if (iQAgent_MediaResultsModel.ID > 0)
                        {
                            objIQAgentReport_WithoutAuthentication.Results.FirstOrDefault(a => a.SearchRequestID == searchRequestID).MediaResults.Add(iQAgent_MediaResultsModel);
                        }
                    }
                }

                if (dataSet.Tables.Count > 10 && dataSet.Tables[10] != null)
                {

                    // Represents Radio

                    foreach (DataRow dr in dataSet.Tables[10].Rows)
                    {

                        IQAgent_TVEyesResultsModel iQAgent_TVEyesResultsModel = new IQAgent_TVEyesResultsModel();
                        IQAgent_MediaResultsModel iQAgent_MediaResultsModel = new IQAgent_MediaResultsModel();

                        Int64 searchRequestID = 0;
                        if (!dr["SearchRequestID"].Equals(DBNull.Value))
                        {
                            searchRequestID = Convert.ToInt64(dr["SearchRequestID"]);
                        }

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.ID = Convert.ToInt64(dr["ID"]);
                        }

                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            iQAgent_TVEyesResultsModel.Title = Convert.ToString(dr["Title"]);
                        }

                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            iQAgent_TVEyesResultsModel.UTCDateTime = Convert.ToDateTime(dr["MediaDate"]);
                            iQAgent_MediaResultsModel.MediaDateTime = Convert.ToDateTime(dr["MediaDate"]);
                        }
                        if (!dr["Category"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.CategoryType = Convert.ToString(dr["Category"]);
                        }

                        if (dataSet.Tables[10].Columns.Contains("HighlightingText") && !dr["HighlightingText"].Equals(DBNull.Value))
                        {
                            iQAgent_TVEyesResultsModel.HighlightingText = Convert.ToString(dr["HighlightingText"]);
                            iQAgent_TVEyesResultsModel.HighlightingText = Convert.ToString(dr["HighlightingText"]);
                        }

                        if (dataSet.Tables[10].Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }

                        if (dataSet.Tables[10].Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }


                        if (dataSet.Tables[10].Columns.Contains("StationID") && !dr["StationID"].Equals(DBNull.Value))
                        {
                            iQAgent_TVEyesResultsModel.StationID = Convert.ToString(dr["StationID"]);
                        }


                        if (dataSet.Tables[10].Columns.Contains("Market") && !dr["Market"].Equals(DBNull.Value))
                        {
                            iQAgent_TVEyesResultsModel.Market = Convert.ToString(dr["Market"]);
                        }

                        if (dataSet.Tables[10].Columns.Contains("DMARank") && !dr["DMARank"].Equals(DBNull.Value))
                        {
                            iQAgent_TVEyesResultsModel.DMARank = Convert.ToString(dr["DMARank"]);
                        }

                        iQAgent_MediaResultsModel.MediaType = Convert.ToString(dr["Category"]);
                        iQAgent_MediaResultsModel.MediaData = iQAgent_TVEyesResultsModel;
                        if (iQAgent_MediaResultsModel.ID > 0)
                        {
                            objIQAgentReport_WithoutAuthentication.Results.FirstOrDefault(a => a.SearchRequestID == searchRequestID).MediaResults.Add(iQAgent_MediaResultsModel);
                        }
                    }
                }

                if (dataSet.Tables.Count > 11 && dataSet.Tables[11] != null)
                {

                    DataTable pmDataTable = dataSet.Tables[11];

                    foreach (DataRow dr in dataSet.Tables[11].Rows)
                    {

                        IQAgent_BLPMResultsModel iQAgent_BLPMResultsModel = new IQAgent_BLPMResultsModel();
                        IQAgent_MediaResultsModel iQAgent_MediaResultsModel = new IQAgent_MediaResultsModel();

                        Int64 searchRequestID = 0;
                        if (!dr["SearchRequestID"].Equals(DBNull.Value))
                        {
                            searchRequestID = Convert.ToInt64(dr["SearchRequestID"]);
                        }

                        if (pmDataTable.Columns.Contains("ID") && !dr["ID"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.ID = Convert.ToInt32(dr["ID"]);
                        }

                        if (pmDataTable.Columns.Contains("HighlightingText") && !dr["HighlightingText"].Equals(DBNull.Value))
                        {
                            iQAgent_BLPMResultsModel.HighlightingText = GetHilightedText_PM(Convert.ToString(dr["HighlightingText"]));
                        }

                        if (pmDataTable.Columns.Contains("PubDate") && !dr["PubDate"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.MediaDateTime = Convert.ToDateTime(dr["PubDate"]);
                        }

                        if (pmDataTable.Columns.Contains("Category") && !dr["Category"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.CategoryType = Convert.ToString(dr["Category"]);
                        }

                        if (pmDataTable.Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }

                        if (pmDataTable.Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }

                        if (pmDataTable.Columns.Contains("Title") && !dr["Title"].Equals(DBNull.Value))
                        {
                            iQAgent_BLPMResultsModel.Title = Convert.ToString(dr["Title"]);
                        }

                        if (pmDataTable.Columns.Contains("Circulation") && !dr["Circulation"].Equals(DBNull.Value))
                        {
                            iQAgent_BLPMResultsModel.Circulation = Convert.ToInt32(dr["Circulation"]);
                        }

                        if (pmDataTable.Columns.Contains("FileLocation") && !dr["FileLocation"].Equals(DBNull.Value))
                        {
                            iQAgent_BLPMResultsModel.FileLocation = Convert.ToString(dr["FileLocation"]);
                        }

                        if (pmDataTable.Columns.Contains("Pub_Name") && !dr["Pub_Name"].Equals(DBNull.Value))
                        {
                            iQAgent_BLPMResultsModel.Pub_Name = Convert.ToString(dr["Pub_Name"]);
                        }

                        iQAgent_MediaResultsModel.MediaType = Convert.ToString(dr["Category"]);
                        iQAgent_MediaResultsModel.MediaData = iQAgent_BLPMResultsModel;

                        if (iQAgent_MediaResultsModel.ID > 0)
                        {
                            objIQAgentReport_WithoutAuthentication.Results.FirstOrDefault(a => a.SearchRequestID == searchRequestID).MediaResults.Add(iQAgent_MediaResultsModel);
                        }
                    }
                }

                if (dataSet.Tables.Count > 12 && dataSet.Tables[12] != null)
                {

                    DataTable pqDataTable = dataSet.Tables[12];

                    foreach (DataRow dr in dataSet.Tables[12].Rows)
                    {

                        IQAgent_PQResultsModel iQAgent_PQResultsModel = new IQAgent_PQResultsModel();
                        IQAgent_MediaResultsModel iQAgent_MediaResultsModel = new IQAgent_MediaResultsModel();

                        Int64 searchRequestID = 0;
                        if (!dr["SearchRequestID"].Equals(DBNull.Value))
                        {
                            searchRequestID = Convert.ToInt64(dr["SearchRequestID"]);
                        }

                        if (pqDataTable.Columns.Contains("ID") && !dr["ID"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.ID = Convert.ToInt32(dr["ID"]);
                        }

                        if (pqDataTable.Columns.Contains("HighlightingText") && !dr["HighlightingText"].Equals(DBNull.Value))
                        {
                            HighlightedPQOutput highlightedPQOutput = new HighlightedPQOutput();
                            iQAgent_PQResultsModel.HighlightedPQOutput = (HighlightedPQOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedPQOutput);
                        }

                        if (pqDataTable.Columns.Contains("MediaDate") && !dr["MediaDate"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.MediaDateTime = Convert.ToDateTime(dr["MediaDate"]);
                        }

                        if (pqDataTable.Columns.Contains("Category") && !dr["Category"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.CategoryType = Convert.ToString(dr["Category"]);
                        }

                        if (pqDataTable.Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }

                        if (pqDataTable.Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }

                        if (pqDataTable.Columns.Contains("Title") && !dr["Title"].Equals(DBNull.Value))
                        {
                            iQAgent_PQResultsModel.Title = Convert.ToString(dr["Title"]);
                        }

                        if (pqDataTable.Columns.Contains("Publication") && !dr["Publication"].Equals(DBNull.Value))
                        {
                            iQAgent_PQResultsModel.Publication = Convert.ToString(dr["Publication"]);
                        }

                        if (pqDataTable.Columns.Contains("Authors") && !dr["Authors"].Equals(DBNull.Value))
                        {
                            iQAgent_PQResultsModel.Authors = XDocument.Parse(Convert.ToString(dr["Authors"])).Descendants("author").Select(n => n.Value).ToList();
                        }

                        iQAgent_MediaResultsModel.MediaType = Convert.ToString(dr["Category"]);
                        iQAgent_MediaResultsModel.MediaData = iQAgent_PQResultsModel;

                        if (iQAgent_MediaResultsModel.ID > 0)
                        {
                            objIQAgentReport_WithoutAuthentication.Results.FirstOrDefault(a => a.SearchRequestID == searchRequestID).MediaResults.Add(iQAgent_MediaResultsModel);
                        }
                    }
                }

                if (dataSet.Tables.Count > 13 && dataSet.Tables[13] != null)
                {

                    // Represents LN

                    foreach (DataRow dr in dataSet.Tables[13].Rows)
                    {
                        IQAgent_NewsResultsModel iQAgent_NewsResultsModel = new IQAgent_NewsResultsModel();
                        IQAgent_MediaResultsModel iQAgent_MediaResultsModel = new IQAgent_MediaResultsModel();

                        Int64 searchRequestID = 0;
                        if (!dr["SearchRequestID"].Equals(DBNull.Value))
                        {
                            searchRequestID = Convert.ToInt64(dr["SearchRequestID"]);
                        }

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.ID = Convert.ToInt64(dr["ID"]);
                        }

                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel.Title = Convert.ToString(dr["Title"]);
                        }

                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel.Harvest_Time = Convert.ToDateTime(dr["MediaDate"]);
                            iQAgent_MediaResultsModel.MediaDateTime = Convert.ToDateTime(dr["MediaDate"]);
                        }

                        if (!dr["HighlightingText"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel.HighlightingText = Convert.ToString(dr["HighlightingText"]);
                            if (!string.IsNullOrWhiteSpace(iQAgent_NewsResultsModel.HighlightingText))
                            {
                                HighlightedNewsOutput highlightedNewsOutput = new HighlightedNewsOutput();
                                iQAgent_NewsResultsModel.HighlightedNewsOutput = (HighlightedNewsOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedNewsOutput);
                            }
                        }
                        if (!dr["Category"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.CategoryType = Convert.ToString(dr["Category"]);
                        }

                        if (!dr["Url"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel.Url = Convert.ToString(dr["Url"]);
                        }

                        if (dataSet.Tables[13].Columns.Contains("Publication") && !dr["Publication"].Equals(DBNull.Value))
                        {
                            Uri aPublisherUri;
                            iQAgent_NewsResultsModel.Publication = Uri.TryCreate(Convert.ToString(dr["Publication"]), UriKind.Absolute, out aPublisherUri) ? aPublisherUri.Host.Replace("www.", string.Empty) : Convert.ToString(dr["Publication"]);
                        }

                        if (!dr["Compete_Audience"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel.Compete_Audience = Convert.ToInt32(dr["Compete_Audience"]);
                        }

                        if (!dr["IQAdShareValue"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel.IQAdShareValue = Convert.ToDecimal(dr["IQAdShareValue"]);
                        }

                        if (!dr["Compete_Result"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel.Compete_Result = Convert.ToString(dr["Compete_Result"]);
                        }

                        if (dataSet.Tables[13].Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }

                        if (dataSet.Tables[13].Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            iQAgent_MediaResultsModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }

                        if (dataSet.Tables[13].Columns.Contains("IQLicense") && !dr["IQLicense"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel.IQLicense = Convert.ToInt16(dr["IQLicense"]);
                        }

                        if (dataSet.Tables[13].Columns.Contains("ParentID") && !dr["ParentID"].Equals(DBNull.Value))
                        {
                            iQAgent_NewsResultsModel._ParentID = Convert.ToInt32(dr["ParentID"]);
                        }

                        iQAgent_MediaResultsModel.MediaType = Convert.ToString(dr["Category"]);
                        iQAgent_MediaResultsModel.MediaData = iQAgent_NewsResultsModel;
                        if (iQAgent_MediaResultsModel.ID > 0)
                        {
                            objIQAgentReport_WithoutAuthentication.Results.FirstOrDefault(a => a.SearchRequestID == searchRequestID).MediaResults.Add(iQAgent_MediaResultsModel);
                        }
                    }
                }
            }

            return objIQAgentReport_WithoutAuthentication;
        }

        public Int64 DeleteIQAgentSearchRequest(Int64 ID, Guid ClientGuid)
        {
            try
            {

                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@SearchRequestKey", DbType.Int64, ID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, ClientGuid, ParameterDirection.Input));
                string result = DataAccess.ExecuteNonQuery("usp_v4_IQAgent_SearchRequest_Delete", dataTypeList);
                return Convert.ToInt64(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Int64 RequestDeleteIQAgentSearchRequest(Int64 ID, Guid ClientGuid, Guid CustomerGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@SearchRequestKey", DbType.Int64, ID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, CustomerGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, ClientGuid, ParameterDirection.Input));
                string result = DataAccess.ExecuteNonQuery("usp_v4_IQAgent_SearchRequest_DeleteRequest", dataTypeList);
                return Convert.ToInt64(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Int64 ExcludeDomainsBySearchRequest(Guid p_ClientGUID, string p_MediaXml, string p_SearchRequestXml)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ArticleXml", DbType.Xml, p_MediaXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchRequestXml", DbType.Xml, p_SearchRequestXml, ParameterDirection.Input));
                string result = DataAccess.ExecuteNonQuery("usp_v5_IQAgent_MediaResults_ExcludeDomains", dataTypeList);
                return Convert.ToInt64(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetHilightedText_PM(string string_XML)
        {
            if (!string.IsNullOrEmpty(string_XML))
            {
                XDocument xdoc = XDocument.Parse(string_XML);
                string hilightedText = string.Join(" ", xdoc.Descendants("text").Select(e => e.Value));
                return hilightedText.Trim();
            }
            else
            {
                return string.Empty;
            }
        }

        public string InsertMissingArticle(IQAgent_MissingArticlesModel p_IQAgent_MissingArticlesModel, Guid p_ClientGuid, Guid p_CustomerGuid)
        {
            try
            {
                string Status = string.Empty;
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Title", DbType.String, p_IQAgent_MissingArticlesModel.Title, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Content", DbType.String, p_IQAgent_MissingArticlesModel.Content, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchRequestID", DbType.Int64, p_IQAgent_MissingArticlesModel._SearchRequestID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Url", DbType.String, p_IQAgent_MissingArticlesModel.Url, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Harvest_Time", DbType.DateTime, p_IQAgent_MissingArticlesModel.harvest_time, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Category", DbType.String, p_IQAgent_MissingArticlesModel.Category, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, p_CustomerGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@AddToLibrary", DbType.Boolean, p_IQAgent_MissingArticlesModel.AddToLibrary, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@LibraryCategory", DbType.Guid, p_IQAgent_MissingArticlesModel.LibraryCategory, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MissingArticleID", DbType.Int64, Status, ParameterDirection.Output));

                Dictionary<string, string> _outputParams;

                string result = DataAccess.ExecuteNonQuery("usp_v4_IQAgent_MissingArticles_Insert", dataTypeList, out _outputParams);

                if (_outputParams != null && _outputParams.Count > 0 && _outputParams.ContainsKey("@MissingArticleID"))
                {
                    Status = Convert.ToString(_outputParams["@MissingArticleID"]);
                }

                return Status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<IQAgent_SearchRequestModel> SelectNewsAndSocialMediSearchRequestByClientGuid(string ClientGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGuid", DbType.String, ClientGuid, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQAgent_SearchRequest_SelectNMAndSMRequests", dataTypeList);

                List<IQAgent_SearchRequestModel> lstIQAgentSearchRequest = new List<IQAgent_SearchRequestModel>();

                if (dataset != null && dataset.Tables.Count > 0)
                {
                    foreach (DataRow dr in dataset.Tables[0].Rows)
                    {
                        IQAgent_SearchRequestModel objIQAgent_SearchRequestModel = new IQAgent_SearchRequestModel();
                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objIQAgent_SearchRequestModel.ID = Convert.ToInt64(dr["ID"]);
                        }
                        if (!dr["Query_Name"].Equals(DBNull.Value))
                        {
                            objIQAgent_SearchRequestModel.QueryName = Convert.ToString(dr["Query_Name"]);
                        }
                        lstIQAgentSearchRequest.Add(objIQAgent_SearchRequestModel);
                    }
                }

                return lstIQAgentSearchRequest;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GetTVEyesLocationMediaID(Int64 p_ID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@MediaID", DbType.Int64, p_ID, ParameterDirection.Input));

                string transcriptURL = string.Empty;
                DataSet result = DataAccess.GetDataSet("usp_v4_IQAgent_MediaResults_SelectTVEyesUrlByMediaID", dataTypeList);

                if (result != null && result.Tables.Count > 0 && result.Tables[0] != null && result.Tables[0].Rows.Count > 0)
                {
                    transcriptURL = Convert.ToString(result.Tables[0].Rows[0]["TranscriptURL"]);

                }

                return transcriptURL;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int QueueMediaResultsForDelete(Guid clientGUID, Guid customerGuid, string mediaIDXml)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                Dictionary<string, string> outParameter = null;
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, clientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.Guid, customerGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MediaIDXml", DbType.Xml, mediaIDXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Result", DbType.Int32, 0, ParameterDirection.Output));
                DataSet result = DataAccess.GetDataSetWithOutParam("usp_v4_IQAgent_MediaResults_QueueForDelete", dataTypeList, out outParameter);

                if (outParameter != null && outParameter.Count > 0)
                {
                    return !string.IsNullOrWhiteSpace(outParameter["@Result"]) ? Convert.ToInt32(outParameter["@Result"]) : 0;
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Int16 SuspendAgentSearchRequest(long p_ID, Guid p_ClientGUID, Guid p_CustomerGUID)
        {
            List<DataType> dataTypeList = new List<DataType>();

            dataTypeList.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@CustomerGUID", DbType.Guid, p_CustomerGUID, ParameterDirection.Input));

            string result = Convert.ToString(DataAccess.ExecuteScalar("usp_v4_IQAgent_SearchRequest_SuspendRequest", dataTypeList));

            return Convert.ToInt16(result);
        }

        public short ResumeSuspendedAgent(long p_ID, Guid p_ClientGUID, Guid p_CustomerGUID)
        {
            List<DataType> dataTypeList = new List<DataType>();

            dataTypeList.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@CustomerGUID", DbType.Guid, p_CustomerGUID, ParameterDirection.Input));

            string result = Convert.ToString(DataAccess.ExecuteScalar("usp_v4_IQAgent_SearchRequest_ResumeSuspendedRequest", dataTypeList));

            return Convert.ToInt16(result);
        }

        public List<IQAgent_CampaignModel> SelectIQAgentCampaignsByClientGuid(Guid clientGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, clientGuid, ParameterDirection.Input));
                DataSet dataSet = DataAccess.GetDataSet("usp_v5_IQAgent_Campaign_SelectByClientGuid", dataTypeList);

                List<IQAgent_CampaignModel> lstCampaigns = new List<IQAgent_CampaignModel>();
                if (dataSet != null && dataSet.Tables.Count == 1)
                {
                    DataTable dt = dataSet.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        IQAgent_CampaignModel campaign = new IQAgent_CampaignModel();

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            campaign.ID = Convert.ToInt64(dr["ID"]);
                        }
                        if (!dr["Name"].Equals(DBNull.Value))
                        {
                            campaign.Name = Convert.ToString(dr["Name"]);
                        }
                        if (!dr["SearchRequestID"].Equals(DBNull.Value))
                        {
                            campaign.SearchRequestID = Convert.ToInt64(dr["SearchRequestID"]);
                        }
                        if (!dr["Query_Name"].Equals(DBNull.Value))
                        {
                            campaign.QueryName = Convert.ToString(dr["Query_Name"]);
                        }
                        if (!dr["StartDateTime"].Equals(DBNull.Value))
                        {
                            campaign.StartDateTime = Convert.ToDateTime(dr["StartDateTime"]);
                        }
                        if (!dr["EndDateTime"].Equals(DBNull.Value))
                        {
                            campaign.EndDateTime = Convert.ToDateTime(dr["EndDateTime"]);
                        }
                        if (!dr["Query_Version"].Equals(DBNull.Value))
                        {
                            campaign.QueryVersion = Convert.ToInt32(dr["Query_Version"]);
                        }

                        lstCampaigns.Add(campaign);
                    }
                }

                return lstCampaigns;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw;
            }
        }

        #region Twitter Rule Settings

        public string GetTwitterRuleByTrackGUID(Guid userTrackGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@UserTrackGUID", DbType.Guid, userTrackGuid, ParameterDirection.Input));
                return Convert.ToString(DataAccess.ExecuteScalar("usp_v5_IQTwitterSettings_SelectByTrackGUID", dataTypeList));
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw;
            }   
        }

        public int InsertTwitterRule(Guid clientGuid, Guid userTrackGuid, string twitterRuleXml, string agentName, long searchRequestID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, clientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@UserTrackGUID", DbType.Guid, userTrackGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@TwitterRule", DbType.String, twitterRuleXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@AgentName", DbType.String, agentName, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchRequestID", DbType.Int64, searchRequestID, ParameterDirection.Input));
                return Convert.ToInt32(DataAccess.ExecuteScalar("usp_v5_IQTwitterSettings_Insert", dataTypeList));
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw;
            }
        }

        public int UpdateTwitterRule(Guid userTrackGuid, string twitterRuleXml, string agentName)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@UserTrackGUID", DbType.Guid, userTrackGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@TwitterRule", DbType.String, twitterRuleXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@AgentName", DbType.String, agentName, ParameterDirection.Input));
                return Convert.ToInt32(DataAccess.ExecuteScalar("usp_v5_IQTwitterSettings_Update", dataTypeList));
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw;
            }
        }

        public int DeleteTwitterRule(Guid userTrackGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@UserTrackGUID", DbType.Guid, userTrackGuid, ParameterDirection.Input));
                return Convert.ToInt32(DataAccess.ExecuteScalar("usp_v5_IQTwitterSettings_Delete", dataTypeList));
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw;
            }
        }

        public int InsertTwitterRuleJob(Guid clientGuid, Guid customerGuid, long searchRequestID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, clientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.Guid, customerGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchRequestID", DbType.Int64, searchRequestID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CreateJobRecord", DbType.Boolean, true, ParameterDirection.Input));
                return Convert.ToInt32(DataAccess.ExecuteScalar("usp_v5_IQService_TwitterSettings_Insert", dataTypeList));
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw;
            }
        }

        #endregion

        #region TVEyes Rule Settings

        public string GetTVEyesRuleByID(long tvEyesSettingsKey)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@TVESettingsKey", DbType.Int64, tvEyesSettingsKey, ParameterDirection.Input));
                return Convert.ToString(DataAccess.ExecuteScalar("usp_v5_IQTVEyesSettings_SelectByID", dataTypeList));
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw;
            }
        }

        public int InsertTVEyesRule(Guid clientGuid, long searchRequestID, string searchTerm, string agentName)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, clientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchRequestID", DbType.Int64, searchRequestID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@AgentName", DbType.String, agentName, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchTerm", DbType.String, searchTerm, ParameterDirection.Input));
                return Convert.ToInt32(DataAccess.ExecuteScalar("usp_v5_IQTVEyesSettings_Insert", dataTypeList));
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw;
            }
        }

        public int UpdateTVEyesRule(long TVESettingsKey, string searchTerm, string agentName)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@TVESettingsKey", DbType.Int64, TVESettingsKey, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchTerm", DbType.String, searchTerm, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@AgentName", DbType.String, agentName, ParameterDirection.Input));
                return Convert.ToInt32(DataAccess.ExecuteScalar("usp_v5_IQTVEyesSettings_Update", dataTypeList));
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw;
            }
        }

        public int DeleteTVEyesRule(long TVESettingsKey)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@TVESettingsKey", DbType.Int64, TVESettingsKey, ParameterDirection.Input));
                return Convert.ToInt32(DataAccess.ExecuteScalar("usp_v5_IQTVEyesSettings_Delete", dataTypeList));
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw;
            }
        }

        public int InsertTVEyesRuleJob(Guid clientGuid, Guid customerGuid, long searchRequestID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, clientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.Guid, customerGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchRequestID", DbType.Int64, searchRequestID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CreateJobRecord", DbType.Boolean, true, ParameterDirection.Input));
                return Convert.ToInt32(DataAccess.ExecuteScalar("usp_v5_IQService_TVEyesSettings_Insert", dataTypeList));
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw;
            }
        }

        #endregion
    }
}
