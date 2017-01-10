using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using System.Xml.Linq;
using System.Data;
using IQMedia.Shared.Utility;
using IQMedia.Data.Base;

namespace IQMedia.Data.MCMediaTemplate
{
    // Base class. Contains functionality common to each MCMedia template class.
    public class MCMediaTemplateBaseDA
    {
        protected DataSet ExecuteSP(Guid reportGuid, MCMediaSearchModel searchSettings, string spName)
        {
            // Pass all of the search criteria as an xml structure, so that different templates can easily use different sets of criteria.
            string searchSettingsXml = null;
            if (searchSettings != null)
            {
                searchSettingsXml = CommonFunctions.SerializeToXml(searchSettings);
            }
            List<DataType> dataTypeList = new List<DataType>();
            dataTypeList.Add(new DataType("@ReportGUID", DbType.Guid, reportGuid, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@SearchSettings", DbType.String, searchSettingsXml, ParameterDirection.Input));
            return DataAccess.GetDataSet(spName, dataTypeList);
        }

        protected List<IQArchive_MediaModel> GetTVResults(DataTable dt, string currentUrl)
        {
            List<IQArchive_MediaModel> lstMediaResults = new List<IQArchive_MediaModel>();

            foreach (DataRow dr in dt.Rows)
            {
                IQArchive_MediaModel objIQArchive_MediaModel = new IQArchive_MediaModel();
                IQArchive_ArchiveClipModel objIQArchive_ArchiveClipModel = new IQArchive_ArchiveClipModel();
                if (!dr["ID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ID = Convert.ToInt64(dr["ID"]);
                }
                if (!dr["_ArchiveMediaID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ArchiveMediaID = Convert.ToInt64(dr["_ArchiveMediaID"]);
                }
                if (!dr["Title"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Title = Convert.ToString(dr["Title"]);
                }
                if (!dr["Content"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Content = GetClosedCaptionText(Convert.ToString(dr["Content"]));
                }
                if (!dr["MediaDate"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.MediaDate = Convert.ToDateTime(dr["MediaDate"]);
                }
                if (!dr["MediaType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.MediaType = Convert.ToString(dr["MediaType"]);
                }
                if (!dr["SubMediaType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.SubMediaType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), Convert.ToString(dr["SubMediaType"]));
                }
                if (!dr["TableType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.DataModelType = Convert.ToString(dr["TableType"]);
                }
                if (!dr["Description"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Description = Convert.ToString(dr["Description"]);
                }
                if (!dr["DisplayDescription"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.DisplayDescription = Convert.ToBoolean(dr["DisplayDescription"]);
                }
                if (dt.Columns.Contains("ClientGUID") && !dr["ClientGUID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ClientGUID = new Guid(Convert.ToString(dr["ClientGUID"]));
                }
                if (dt.Columns.Contains("ClientName") && !dr["ClientName"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ClientName = Convert.ToString(dr["ClientName"]);
                }
                if (dt.Columns.Contains("CreatedDate") && !dr["CreatedDate"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                }
                if (dt.Columns.Contains("CategoryGUID") && !dr["CategoryGUID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryGUID = new Guid(Convert.ToString(dr["CategoryGUID"]));
                }
                if (dt.Columns.Contains("CategoryName") && !dr["CategoryName"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                }
                if (dt.Columns.Contains("CategoryRanking") && !dr["CategoryRanking"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryRanking = Convert.ToInt32(dr["CategoryRanking"]);
                }
                if (!dr["ClipID"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveClipModel.ClipID = Convert.ToString(dr["ClipID"]);
                }
                if (!dr["Nielsen_Audience"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveClipModel.Nielsen_Audience = Convert.ToInt32(dr["Nielsen_Audience"]);
                    objIQArchive_MediaModel.Audience = objIQArchive_ArchiveClipModel.Nielsen_Audience.HasValue ? objIQArchive_ArchiveClipModel.Nielsen_Audience.Value : 0;
                }
                if (!dr["IQAdShareValue"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveClipModel.IQAdShareValue = Convert.ToDecimal(dr["IQAdShareValue"]);
                }
                if (!dr["Nielsen_Result"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveClipModel.Nielsen_Result = Convert.ToString(dr["Nielsen_Result"]);
                }
                if (!dr["HighlightingText"].Equals(DBNull.Value) && !string.IsNullOrWhiteSpace(Convert.ToString(dr["HighlightingText"])))
                {
                    HighlightedCCOutput highlightedCCOutput = new HighlightedCCOutput();
                    objIQArchive_ArchiveClipModel.HighlightedOutput = (HighlightedCCOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedCCOutput);
                }

                if (dt.Columns.Contains("Market") && !dr["Market"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveClipModel.Market = Convert.ToString(dr["Market"]);
                }
                if (dt.Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveClipModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                }
                if (dt.Columns.Contains("ClipDate") && !dr["ClipDate"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveClipModel.LocalDateTime = Convert.ToDateTime(dr["ClipDate"]);
                }
                if (dt.Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveClipModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                }
                if (dt.Columns.Contains("StationLogo") && !dr["StationLogo"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveClipModel.StationLogo = "http://" + currentUrl + "/StationLogoImages/" + Convert.ToString(dr["StationLogo"]) + ".jpg";
                }
                if (dt.Columns.Contains("TimeZone") && !dr["TimeZone"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveClipModel.TimeZone = Convert.ToString(dr["TimeZone"]);
                }
                if (dt.Columns.Contains("Dma_Num") && !dr["Dma_Num"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveClipModel.Dma_Num = Convert.ToString(dr["Dma_Num"]);
                }

                objIQArchive_MediaModel.MediaData = objIQArchive_ArchiveClipModel;
                lstMediaResults.Add(objIQArchive_MediaModel);
            }

            return lstMediaResults;
        }

        protected List<IQArchive_MediaModel> GetBLPMResults(DataTable dt)
        {
            List<IQArchive_MediaModel> lstMediaResults = new List<IQArchive_MediaModel>();

            foreach (DataRow dr in dt.Rows)
            {
                IQArchive_MediaModel objIQArchive_MediaModel = new IQArchive_MediaModel();
                IQArchive_ArchiveBLPMModel objIQArchive_ArchiveBLPMModel = new IQArchive_ArchiveBLPMModel();

                if (!dr["ID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ID = Convert.ToInt64(dr["ID"]);
                }
                if (!dr["_ArchiveMediaID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ArchiveMediaID = Convert.ToInt64(dr["_ArchiveMediaID"]);
                }
                if (!dr["Title"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Title = Convert.ToString(dr["Title"]);
                }
                if (!dr["Content"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Content = Convert.ToString(dr["Content"]);
                }
                if (!dr["MediaDate"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.MediaDate = Convert.ToDateTime(dr["MediaDate"]);
                }
                if (!dr["MediaType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.MediaType = Convert.ToString(dr["MediaType"]);
                }
                if (!dr["SubMediaType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.SubMediaType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), Convert.ToString(dr["SubMediaType"]));
                }
                if (!dr["TableType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.DataModelType = Convert.ToString(dr["TableType"]);
                }
                if (!dr["Description"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Description = Convert.ToString(dr["Description"]);
                }
                if (!dr["DisplayDescription"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.DisplayDescription = Convert.ToBoolean(dr["DisplayDescription"]);
                }
                if (dt.Columns.Contains("ClientGUID") && !dr["ClientGUID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ClientGUID = new Guid(Convert.ToString(dr["ClientGUID"]));
                }
                if (dt.Columns.Contains("ClientName") && !dr["ClientName"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ClientName = Convert.ToString(dr["ClientName"]);
                }
                if (dt.Columns.Contains("CreatedDate") && !dr["CreatedDate"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                }
                if (dt.Columns.Contains("CategoryGUID") && !dr["CategoryGUID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryGUID = new Guid(Convert.ToString(dr["CategoryGUID"]));
                }
                if (dt.Columns.Contains("CategoryName") && !dr["CategoryName"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                }
                if (dt.Columns.Contains("CategoryRanking") && !dr["CategoryRanking"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryRanking = Convert.ToInt32(dr["CategoryRanking"]);
                }
                if (!dr["Circulation"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveBLPMModel.Circulation = Convert.ToInt32(dr["Circulation"]);
                    objIQArchive_MediaModel.Audience = objIQArchive_ArchiveBLPMModel.Circulation;
                }
                if (!dr["FileLocation"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveBLPMModel.FileLocation = Convert.ToString(dr["FileLocation"]);
                }
                if (!dr["Pub_Name"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveBLPMModel.Pub_Name = Convert.ToString(dr["Pub_Name"]);
                }

                objIQArchive_MediaModel.MediaData = objIQArchive_ArchiveBLPMModel;
                lstMediaResults.Add(objIQArchive_MediaModel);
            }

            return lstMediaResults;
        }

        protected List<IQArchive_MediaModel> GetSMResults(DataTable dt)
        {
            List<IQArchive_MediaModel> lstMediaResults = new List<IQArchive_MediaModel>();

            foreach (DataRow dr in dt.Rows)
            {
                IQArchive_MediaModel objIQArchive_MediaModel = new IQArchive_MediaModel();
                IQArchive_ArchiveSMModel objIQArchive_ArchiveSMModel = new IQArchive_ArchiveSMModel();

                if (!dr["ID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ID = Convert.ToInt64(dr["ID"]);
                }
                if (!dr["_ArchiveMediaID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ArchiveMediaID = Convert.ToInt64(dr["_ArchiveMediaID"]);
                }
                if (!dr["Title"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Title = Convert.ToString(dr["Title"]);
                }
                if (!dr["Content"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Content = Convert.ToString(dr["Content"]);
                }
                if (!dr["MediaDate"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.MediaDate = Convert.ToDateTime(dr["MediaDate"]);
                }
                if (!dr["MediaType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.MediaType = Convert.ToString(dr["MediaType"]);
                }
                if (!dr["SubMediaType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.SubMediaType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), Convert.ToString(dr["SubMediaType"]));
                }
                if (!dr["TableType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.DataModelType = Convert.ToString(dr["TableType"]);
                }
                if (!dr["Description"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Description = Convert.ToString(dr["Description"]);
                }
                if (!dr["DisplayDescription"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.DisplayDescription = Convert.ToBoolean(dr["DisplayDescription"]);
                }
                if (dt.Columns.Contains("ClientGUID") && !dr["ClientGUID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ClientGUID = new Guid(Convert.ToString(dr["ClientGUID"]));
                }
                if (dt.Columns.Contains("ClientName") && !dr["ClientName"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ClientName = Convert.ToString(dr["ClientName"]);
                }
                if (dt.Columns.Contains("CreatedDate") && !dr["CreatedDate"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                }
                if (dt.Columns.Contains("CategoryGUID") && !dr["CategoryGUID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryGUID = new Guid(Convert.ToString(dr["CategoryGUID"]));
                }
                if (dt.Columns.Contains("CategoryName") && !dr["CategoryName"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                }
                if (dt.Columns.Contains("CategoryRanking") && !dr["CategoryRanking"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryRanking = Convert.ToInt32(dr["CategoryRanking"]);
                }
                if (!dr["HighlightingText"].Equals(DBNull.Value) && !string.IsNullOrWhiteSpace(Convert.ToString(dr["HighlightingText"])))
                {
                    HighlightedSMOutput highlightedSMOutput = new HighlightedSMOutput();
                    objIQArchive_ArchiveSMModel.HighlightedOutput = (HighlightedSMOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedSMOutput);
                }
                if (!dr["Url"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveSMModel.Url = Convert.ToString(dr["Url"]);
                }
                if (!dr["Compete_Audience"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveSMModel.Compete_Audience = Convert.ToInt32(dr["Compete_Audience"]);
                    objIQArchive_MediaModel.Audience = objIQArchive_ArchiveSMModel.Compete_Audience.HasValue ? objIQArchive_ArchiveSMModel.Compete_Audience.Value : 0;
                }
                if (dt.Columns.Contains("homelink") && !dr["homelink"].Equals(DBNull.Value))
                {
                    Uri aPublisherUri;
                    objIQArchive_ArchiveSMModel.Publication = Uri.TryCreate(Convert.ToString(dr["homelink"]), UriKind.Absolute, out aPublisherUri) ? aPublisherUri.Host.Replace("www.", string.Empty) : Convert.ToString(dr["homelink"]);
                }
                if (!dr["IQAdShareValue"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveSMModel.IQAdShareValue = Convert.ToDecimal(dr["IQAdShareValue"]);
                }
                if (!dr["Compete_Result"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveSMModel.Compete_Result = Convert.ToString(dr["Compete_Result"]);
                }
                if (dt.Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveSMModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                }
                if (dt.Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveSMModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                }
                if (dt.Columns.Contains("ThumbUrl") && !dr["ThumbUrl"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveSMModel.ThumbUrl = Convert.ToString(dr["ThumbUrl"]);
                }
                if (dt.Columns.Contains("ArticleStats") && !dr["ArticleStats"].Equals(DBNull.Value) && !String.IsNullOrWhiteSpace(Convert.ToString(dr["ArticleStats"])))
                {
                    ArticleStatsModel statsModel = new ArticleStatsModel();
                    objIQArchive_ArchiveSMModel.ArticleStats = (ArticleStatsModel)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["ArticleStats"]), statsModel);
                }

                objIQArchive_MediaModel.MediaData = objIQArchive_ArchiveSMModel;
                lstMediaResults.Add(objIQArchive_MediaModel);
            }

            return lstMediaResults;
        }

        protected List<IQArchive_MediaModel> GetNMResults(DataTable dt)
        {
            List<IQArchive_MediaModel> lstMediaResults = new List<IQArchive_MediaModel>();

            foreach (DataRow dr in dt.Rows)
            {
                IQArchive_MediaModel objIQArchive_MediaModel = new IQArchive_MediaModel();
                IQArchive_ArchiveNMModel objIQArchive_ArchiveNMModel = new IQArchive_ArchiveNMModel();

                if (!dr["ID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ID = Convert.ToInt64(dr["ID"]);
                }
                if (!dr["_ArchiveMediaID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ArchiveMediaID = Convert.ToInt64(dr["_ArchiveMediaID"]);
                }
                if (!dr["Title"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Title = Convert.ToString(dr["Title"]);
                }
                if (!dr["Content"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Content = Convert.ToString(dr["Content"]);
                }
                if (!dr["MediaDate"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.MediaDate = Convert.ToDateTime(dr["MediaDate"]);
                }
                if (!dr["MediaType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.MediaType = Convert.ToString(dr["MediaType"]);
                }
                if (!dr["SubMediaType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.SubMediaType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), Convert.ToString(dr["SubMediaType"]));
                }
                if (!dr["TableType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.DataModelType = Convert.ToString(dr["TableType"]);
                }
                if (!dr["Description"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Description = Convert.ToString(dr["Description"]);
                }
                if (!dr["DisplayDescription"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.DisplayDescription = Convert.ToBoolean(dr["DisplayDescription"]);
                }
                if (dt.Columns.Contains("ClientGUID") && !dr["ClientGUID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ClientGUID = new Guid(Convert.ToString(dr["ClientGUID"]));
                }
                if (dt.Columns.Contains("ClientName") && !dr["ClientName"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ClientName = Convert.ToString(dr["ClientName"]);
                }
                if (dt.Columns.Contains("CreatedDate") && !dr["CreatedDate"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                }
                if (dt.Columns.Contains("CategoryGUID") && !dr["CategoryGUID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryGUID = new Guid(Convert.ToString(dr["CategoryGUID"]));
                }
                if (dt.Columns.Contains("CategoryName") && !dr["CategoryName"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                }
                if (dt.Columns.Contains("CategoryRanking") && !dr["CategoryRanking"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryRanking = Convert.ToInt32(dr["CategoryRanking"]);
                }
                if (!dr["HighlightingText"].Equals(DBNull.Value) && !string.IsNullOrWhiteSpace(Convert.ToString(dr["HighlightingText"])))
                {
                    HighlightedNewsOutput highlightedNewsOutput = new HighlightedNewsOutput();
                    objIQArchive_ArchiveNMModel.HighlightedOutput = (HighlightedNewsOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedNewsOutput);
                }
                if (!dr["Url"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveNMModel.Url = Convert.ToString(dr["Url"]);
                }
                if (!dr["Compete_Audience"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveNMModel.Compete_Audience = Convert.ToInt32(dr["Compete_Audience"]);
                    objIQArchive_MediaModel.Audience = objIQArchive_ArchiveNMModel.Compete_Audience.HasValue ? objIQArchive_ArchiveNMModel.Compete_Audience.Value : 0;
                }
                if (!dr["IQAdShareValue"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveNMModel.IQAdShareValue = Convert.ToDecimal(dr["IQAdShareValue"]);
                }
                if (dt.Columns.Contains("Publication") && !dr["Publication"].Equals(DBNull.Value))
                {
                    Uri aPublisherUri;
                    objIQArchive_ArchiveNMModel.Publication = Uri.TryCreate(Convert.ToString(dr["Publication"]), UriKind.Absolute, out aPublisherUri) ? aPublisherUri.Host.Replace("www.", string.Empty) : Convert.ToString(dr["Publication"]);
                }
                if (!dr["Compete_Result"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveNMModel.Compete_Result = Convert.ToString(dr["Compete_Result"]);
                }
                if (dt.Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveNMModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                }
                if (dt.Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveNMModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                }
                if (dt.Columns.Contains("IQLicense") && !dr["IQLicense"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveNMModel.IQLicense = Convert.ToInt16(dr["IQLicense"]);
                }

                objIQArchive_MediaModel.MediaData = objIQArchive_ArchiveNMModel;
                lstMediaResults.Add(objIQArchive_MediaModel);
            }

            return lstMediaResults;
        }

        protected List<IQArchive_MediaModel> GetTWResults(DataTable dt)
        {
            List<IQArchive_MediaModel> lstMediaResults = new List<IQArchive_MediaModel>();

            foreach (DataRow dr in dt.Rows)
            {
                IQArchive_MediaModel objIQArchive_MediaModel = new IQArchive_MediaModel();
                IQArchive_ArchiveTweetsModel objIQArchive_ArchiveTweetsModel = new IQArchive_ArchiveTweetsModel();

                if (!dr["ID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ID = Convert.ToInt64(dr["ID"]);
                }
                if (!dr["_ArchiveMediaID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ArchiveMediaID = Convert.ToInt64(dr["_ArchiveMediaID"]);
                }
                if (!dr["Title"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Title = Convert.ToString(dr["Title"]);
                }
                if (!dr["Content"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Content = Convert.ToString(dr["Content"]);
                }
                if (!dr["MediaDate"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.MediaDate = Convert.ToDateTime(dr["MediaDate"]);
                }
                if (!dr["MediaType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.MediaType = Convert.ToString(dr["MediaType"]);
                }
                if (!dr["SubMediaType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.SubMediaType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), Convert.ToString(dr["SubMediaType"]));
                }
                if (!dr["TableType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.DataModelType = Convert.ToString(dr["TableType"]);
                }
                if (!dr["Description"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Description = Convert.ToString(dr["Description"]);
                }
                if (!dr["DisplayDescription"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.DisplayDescription = Convert.ToBoolean(dr["DisplayDescription"]);
                }
                if (dt.Columns.Contains("ClientGUID") && !dr["ClientGUID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ClientGUID = new Guid(Convert.ToString(dr["ClientGUID"]));
                }
                if (dt.Columns.Contains("ClientName") && !dr["ClientName"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ClientName = Convert.ToString(dr["ClientName"]);
                }
                if (dt.Columns.Contains("CreatedDate") && !dr["CreatedDate"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                }
                if (dt.Columns.Contains("CategoryGUID") && !dr["CategoryGUID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryGUID = new Guid(Convert.ToString(dr["CategoryGUID"]));
                }
                if (dt.Columns.Contains("CategoryName") && !dr["CategoryName"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                }
                if (dt.Columns.Contains("CategoryRanking") && !dr["CategoryRanking"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryRanking = Convert.ToInt32(dr["CategoryRanking"]);
                }
                if (!dr["HighlightingText"].Equals(DBNull.Value) && !string.IsNullOrWhiteSpace(Convert.ToString(dr["HighlightingText"])))
                {
                    HighlightedTWOutput highlightedTWOutput = new HighlightedTWOutput();
                    objIQArchive_ArchiveTweetsModel.HighlightedOutput = (HighlightedTWOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedTWOutput);
                }
                if (!dr["Actor_DisplayName"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveTweetsModel.ActorDisplayname = Convert.ToString(dr["Actor_DisplayName"]);
                }
                if (!dr["Actor_PreferredUserName"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveTweetsModel.PreferredUserName = Convert.ToString(dr["Actor_PreferredUserName"]);
                }
                if (!dr["Actor_FollowersCount"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveTweetsModel.FollowersCount = Convert.ToInt32(dr["Actor_FollowersCount"]);
                    objIQArchive_MediaModel.Audience = objIQArchive_ArchiveTweetsModel.FollowersCount;
                }
                if (!dr["Actor_FriendsCount"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveTweetsModel.FreiendsCount = Convert.ToInt32(dr["Actor_FriendsCount"]);
                }
                if (!dr["Actor_Image"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveTweetsModel.ActorImage = Convert.ToString(dr["Actor_Image"]);
                }
                if (!dr["Actor_link"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveTweetsModel.ActorLink = Convert.ToString(dr["Actor_link"]);
                }
                if (!dr["Tweet_ID"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveTweetsModel.TweetID = Convert.ToInt64(dr["Tweet_ID"]);
                }
                if (!dr["gnip_Klout_Score"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveTweetsModel.KloutScore = Convert.ToString(dr["gnip_Klout_Score"]);
                }
                if (dt.Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveTweetsModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                }
                if (dt.Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveTweetsModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                }

                objIQArchive_MediaModel.MediaData = objIQArchive_ArchiveTweetsModel;
                lstMediaResults.Add(objIQArchive_MediaModel);
            }

            return lstMediaResults;
        }

        protected List<IQArchive_MediaModel> GetTMResults(DataTable dt)
        {
            List<IQArchive_MediaModel> lstMediaResults = new List<IQArchive_MediaModel>();

            foreach (DataRow dr in dt.Rows)
            {
                IQArchive_MediaModel objIQArchive_MediaModel = new IQArchive_MediaModel();
                IQArchive_ArchiveTVEyesModel objIQArchive_ArchiveTVEyesModel = new IQArchive_ArchiveTVEyesModel();

                if (!dr["ID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ID = Convert.ToInt64(dr["ID"]);
                }
                if (!dr["_ArchiveMediaID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ArchiveMediaID = Convert.ToInt64(dr["_ArchiveMediaID"]);
                }
                if (!dr["Title"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Title = Convert.ToString(dr["Title"]);
                }
                if (!dr["Content"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Content = Convert.ToString(dr["Content"]);
                }
                if (!dr["MediaDate"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.MediaDate = Convert.ToDateTime(dr["MediaDate"]);
                }
                if (!dr["MediaType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.MediaType = Convert.ToString(dr["MediaType"]);
                }
                if (!dr["SubMediaType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.SubMediaType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), Convert.ToString(dr["SubMediaType"]));
                }
                if (!dr["TableType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.DataModelType = Convert.ToString(dr["TableType"]);
                }
                if (!dr["Description"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Description = Convert.ToString(dr["Description"]);
                }
                if (!dr["DisplayDescription"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.DisplayDescription = Convert.ToBoolean(dr["DisplayDescription"]);
                }
                if (dt.Columns.Contains("ClientGUID") && !dr["ClientGUID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ClientGUID = new Guid(Convert.ToString(dr["ClientGUID"]));
                }
                if (dt.Columns.Contains("ClientName") && !dr["ClientName"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ClientName = Convert.ToString(dr["ClientName"]);
                }
                if (dt.Columns.Contains("CreatedDate") && !dr["CreatedDate"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                }
                if (dt.Columns.Contains("CategoryGUID") && !dr["CategoryGUID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryGUID = new Guid(Convert.ToString(dr["CategoryGUID"]));
                }
                if (dt.Columns.Contains("CategoryName") && !dr["CategoryName"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                }
                if (dt.Columns.Contains("CategoryRanking") && !dr["CategoryRanking"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryRanking = Convert.ToInt32(dr["CategoryRanking"]);
                }
                if (dt.Columns.Contains("LocalDateTime") && !dr["LocalDateTime"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveTVEyesModel.LocalDateTime = Convert.ToDateTime(dr["LocalDateTime"]);
                }
                if (dt.Columns.Contains("TimeZone") && !dr["TimeZone"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveTVEyesModel.TimeZone = Convert.ToString(dr["TimeZone"]);
                }
                if (!dr["StationID"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveTVEyesModel.StationID = Convert.ToString(dr["StationID"]);
                }
                if (!dr["Market"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveTVEyesModel.Market = Convert.ToString(dr["Market"]);
                }
                if (!dr["DMARank"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveTVEyesModel.DMARank = Convert.ToString(dr["DMARank"]);
                }
                if (dt.Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveTVEyesModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                }
                if (dt.Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveTVEyesModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                }

                objIQArchive_MediaModel.MediaData = objIQArchive_ArchiveTVEyesModel;
                lstMediaResults.Add(objIQArchive_MediaModel);
            }

            return lstMediaResults;
        }

        protected List<IQArchive_MediaModel> GetMSResults(DataTable dt)
        {
            List<IQArchive_MediaModel> lstMediaResults = new List<IQArchive_MediaModel>();

            foreach (DataRow dr in dt.Rows)
            {
                IQArchive_MediaModel objIQArchive_MediaModel = new IQArchive_MediaModel();
                IQArchive_ArchiveMiscModel objIQArchive_ArchiveMiscModel = new IQArchive_ArchiveMiscModel();

                if (!dr["ID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ID = Convert.ToInt64(dr["ID"]);
                }
                if (!dr["_ArchiveMediaID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ArchiveMediaID = Convert.ToInt64(dr["_ArchiveMediaID"]);
                }
                if (!dr["Title"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Title = Convert.ToString(dr["Title"]);
                }
                if (!dr["Content"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Content = Convert.ToString(dr["Content"]);
                }
                if (!dr["MediaDate"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.MediaDate = Convert.ToDateTime(dr["MediaDate"]);
                }
                if (!dr["MediaType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.MediaType = Convert.ToString(dr["MediaType"]);
                }
                if (!dr["SubMediaType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.SubMediaType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), Convert.ToString(dr["SubMediaType"]));
                }
                if (!dr["TableType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.DataModelType = Convert.ToString(dr["TableType"]);
                }
                if (!dr["Description"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Description = Convert.ToString(dr["Description"]);
                }
                if (dt.Columns.Contains("ClientGUID") && !dr["ClientGUID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ClientGUID = new Guid(Convert.ToString(dr["ClientGUID"]));
                }
                if (dt.Columns.Contains("ClientName") && !dr["ClientName"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ClientName = Convert.ToString(dr["ClientName"]);
                }
                if (dt.Columns.Contains("CreatedDate") && !dr["CreatedDate"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                }
                if (dt.Columns.Contains("CategoryGUID") && !dr["CategoryGUID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryGUID = new Guid(Convert.ToString(dr["CategoryGUID"]));
                }
                if (dt.Columns.Contains("CategoryName") && !dr["CategoryName"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                }
                if (dt.Columns.Contains("CategoryRanking") && !dr["CategoryRanking"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryRanking = Convert.ToInt32(dr["CategoryRanking"]);
                }
                if (dt.Columns.Contains("CreateDT") && !dr["CreateDT"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveMiscModel.CreateDT = Convert.ToDateTime(dr["CreateDT"]);
                }
                if (dt.Columns.Contains("CreateDTTimeZone") && !dr["CreateDTTimeZone"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveMiscModel.TimeZone = Convert.ToString(dr["CreateDTTimeZone"]);
                }
                if (dt.Columns.Contains("FileType") && !dr["FileType"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveMiscModel.FileType = (CommonFunctions.IQUGCMediaTypes)Enum.Parse(typeof(CommonFunctions.IQUGCMediaTypes), Convert.ToString(dr["FileType"]));
                }
                if (dt.Columns.Contains("MediaUrl") && !dr["MediaUrl"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveMiscModel.MediaUrl = Convert.ToString(dr["MediaUrl"]);
                }

                objIQArchive_MediaModel.MediaData = objIQArchive_ArchiveMiscModel;
                lstMediaResults.Add(objIQArchive_MediaModel);
            }

            return lstMediaResults;
        }

        protected List<IQArchive_MediaModel> GetPQResults(DataTable dt)
        {
            List<IQArchive_MediaModel> lstMediaResults = new List<IQArchive_MediaModel>();

            foreach (DataRow dr in dt.Rows)
            {
                IQArchive_MediaModel objIQArchive_MediaModel = new IQArchive_MediaModel();
                IQArchive_ArchivePQModel objIQArchive_ArchivePQModel = new IQArchive_ArchivePQModel();

                if (!dr["ID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ID = Convert.ToInt64(dr["ID"]);
                }
                if (!dr["_ArchiveMediaID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ArchiveMediaID = Convert.ToInt64(dr["_ArchiveMediaID"]);
                }
                if (!dr["Title"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Title = Convert.ToString(dr["Title"]);
                }
                if (!dr["Content"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Content = Convert.ToString(dr["Content"]);
                }
                if (!dr["MediaDate"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.MediaDate = Convert.ToDateTime(dr["MediaDate"]);
                }
                if (!dr["MediaType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.MediaType = Convert.ToString(dr["MediaType"]);
                }
                if (!dr["SubMediaType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.SubMediaType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), Convert.ToString(dr["SubMediaType"]));
                }
                if (!dr["TableType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.DataModelType = Convert.ToString(dr["TableType"]);
                }
                if (!dr["Description"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Description = Convert.ToString(dr["Description"]);
                }
                if (!dr["DisplayDescription"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.DisplayDescription = Convert.ToBoolean(dr["DisplayDescription"]);
                }
                if (dt.Columns.Contains("ClientGUID") && !dr["ClientGUID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ClientGUID = new Guid(Convert.ToString(dr["ClientGUID"]));
                }
                if (dt.Columns.Contains("ClientName") && !dr["ClientName"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ClientName = Convert.ToString(dr["ClientName"]);
                }
                if (dt.Columns.Contains("CreatedDate") && !dr["CreatedDate"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                }
                if (dt.Columns.Contains("CategoryGUID") && !dr["CategoryGUID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryGUID = new Guid(Convert.ToString(dr["CategoryGUID"]));
                }
                if (dt.Columns.Contains("CategoryName") && !dr["CategoryName"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                }
                if (dt.Columns.Contains("CategoryRanking") && !dr["CategoryRanking"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryRanking = Convert.ToInt32(dr["CategoryRanking"]);
                }
                if (!dr["HighlightingText"].Equals(DBNull.Value) && !string.IsNullOrWhiteSpace(Convert.ToString(dr["HighlightingText"])))
                {
                    HighlightedPQOutput highlightedPQOutput = new HighlightedPQOutput();
                    objIQArchive_ArchivePQModel.HighlightedOutput = (HighlightedPQOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedPQOutput);
                }
                if (dt.Columns.Contains("Publication") && !dr["Publication"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchivePQModel.Publication = Convert.ToString(dr["Publication"]);
                }
                if (dt.Columns.Contains("Author") && !dr["Author"].Equals(DBNull.Value))
                {
                    if (!String.IsNullOrWhiteSpace(Convert.ToString(dr["Author"])))
                    {
                        XDocument xDoc = XDocument.Parse(Convert.ToString(dr["Author"]));
                        objIQArchive_ArchivePQModel.Authors = xDoc.Descendants("author").Select(x => x.Value).ToList();
                    }
                }
                if (dt.Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchivePQModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                }
                if (dt.Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchivePQModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                }

                objIQArchive_MediaModel.MediaData = objIQArchive_ArchivePQModel;
                lstMediaResults.Add(objIQArchive_MediaModel);
            }

            return lstMediaResults;
        }

        protected List<IQArchive_MediaModel> GetIQRadioResults(DataTable dt, string currentUrl)
        {
            List<IQArchive_MediaModel> lstMediaResults = new List<IQArchive_MediaModel>();

            foreach (DataRow dr in dt.Rows)
            {
                IQArchive_MediaModel objIQArchive_MediaModel = new IQArchive_MediaModel();
                IQArchive_ArchiveRadioModel objIQArchive_ArchiveRadioModel = new IQArchive_ArchiveRadioModel();
                if (!dr["ID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ID = Convert.ToInt64(dr["ID"]);
                }
                if (!dr["_ArchiveMediaID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ArchiveMediaID = Convert.ToInt64(dr["_ArchiveMediaID"]);
                }
                if (!dr["Title"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Title = Convert.ToString(dr["Title"]);
                }
                if (!dr["Content"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Content = GetClosedCaptionText(Convert.ToString(dr["Content"]));
                }
                if (!dr["MediaDate"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.MediaDate = Convert.ToDateTime(dr["MediaDate"]);
                }
                if (!dr["MediaType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.MediaType = Convert.ToString(dr["MediaType"]);
                }
                if (!dr["SubMediaType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.SubMediaType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), Convert.ToString(dr["SubMediaType"]));
                }
                if (!dr["TableType"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.DataModelType = Convert.ToString(dr["TableType"]);
                }
                if (!dr["Description"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.Description = Convert.ToString(dr["Description"]);
                }
                if (!dr["DisplayDescription"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.DisplayDescription = Convert.ToBoolean(dr["DisplayDescription"]);
                }
                if (!dr["ClipGuid"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveRadioModel.ClipGuid = Convert.ToString(dr["ClipGuid"]);
                }
                if (!dr["LocalDatetime"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveRadioModel.LocalDateTime = Convert.ToDateTime(dr["LocalDatetime"]);
                }
                if (!dr["Dma_Name"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveRadioModel.Market = Convert.ToString(dr["Dma_Name"]);
                }
                if (!dr["IQ_Station_ID"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveRadioModel.StationID = Convert.ToString(dr["IQ_Station_ID"]);
                    objIQArchive_ArchiveRadioModel.StationLogo = "https://" + currentUrl + "/StationLogoImages/" + Convert.ToString(dr["IQ_Station_ID"]) + ".jpg";
                }
                if (!dr["TimeZone"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveRadioModel.TimeZone = Convert.ToString(dr["TimeZone"]);
                }
                if (!dr["Dma_Num"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveRadioModel.DMARank = Convert.ToInt16(dr["Dma_Num"]);
                }
                if (!dr["HighlightingText"].Equals(DBNull.Value) && !string.IsNullOrWhiteSpace(Convert.ToString(dr["HighlightingText"])))
                {
                    HighlightedCCOutput highlightedCCOutput = new HighlightedCCOutput();
                    objIQArchive_ArchiveRadioModel.HighlightedOutput = (HighlightedCCOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedCCOutput);
                }

                if (dt.Columns.Contains("ClientGUID") && !dr["ClientGUID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ClientGUID = new Guid(Convert.ToString(dr["ClientGUID"]));
                }
                if (dt.Columns.Contains("ClientName") && !dr["ClientName"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.ClientName = Convert.ToString(dr["ClientName"]);
                }
                if (dt.Columns.Contains("CreatedDate") && !dr["CreatedDate"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                }
                if (dt.Columns.Contains("CategoryGUID") && !dr["CategoryGUID"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryGUID = new Guid(Convert.ToString(dr["CategoryGUID"]));
                }
                if (dt.Columns.Contains("CategoryName") && !dr["CategoryName"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                }
                if (dt.Columns.Contains("CategoryRanking") && !dr["CategoryRanking"].Equals(DBNull.Value))
                {
                    objIQArchive_MediaModel.CategoryRanking = Convert.ToInt32(dr["CategoryRanking"]);
                }
                if (dt.Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveRadioModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                }
                if (dt.Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                {
                    objIQArchive_ArchiveRadioModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                }

                objIQArchive_MediaModel.MediaData = objIQArchive_ArchiveRadioModel;
                lstMediaResults.Add(objIQArchive_MediaModel);
            }

            return lstMediaResults;
        }

        protected void GetFilterResults(DataTable dt, string tableType, MCMediaReport_FilterModel filterModel)
        {
            switch (tableType)
            {
                case "SubMediaTypeFilter":
                    filterModel.MediaTypes = new List<MCMediaReport_MediaTypeFilter>();
                    Dictionary<string, MCMediaReport_MediaTypeFilter> dictMediaTypes = new Dictionary<string, MCMediaReport_MediaTypeFilter>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        string mediaType = String.Empty;
                        string mediaTypeDesc = String.Empty;
                        bool hasSubMediaTypes = false;
                        string subMediaType = String.Empty;
                        string subMediaTypeDesc = String.Empty;
                        long subMediaTypeCount = 0;

                        if (!dr["MediaType"].Equals(DBNull.Value))
                        {
                            mediaType = Convert.ToString(dr["MediaType"]);
                        }
                        if (!dr["MediaTypeDesc"].Equals(DBNull.Value))
                        {
                            mediaTypeDesc = Convert.ToString(dr["MediaTypeDesc"]);
                        }
                        if (!dr["HasSubMediaTypes"].Equals(DBNull.Value))
                        {
                            hasSubMediaTypes = Convert.ToBoolean(dr["HasSubMediaTypes"]);
                        }
                        if (!dr["SubMediaType"].Equals(DBNull.Value))
                        {
                            subMediaType = Convert.ToString(dr["SubMediaType"]);
                        }
                        if (!dr["SubMediaTypeDesc"].Equals(DBNull.Value))
                        {
                            subMediaTypeDesc = Convert.ToString(dr["SubMediaTypeDesc"]);
                        }
                        if (!dr["SubMediaTypeCount"].Equals(DBNull.Value))
                        {
                            subMediaTypeCount = Convert.ToInt64(dr["SubMediaTypeCount"]);
                        }

                        if (!dictMediaTypes.ContainsKey(mediaType))
                        {
                            MCMediaReport_MediaTypeFilter newFilter = new MCMediaReport_MediaTypeFilter();
                            newFilter.MediaType = mediaType;
                            newFilter.MediaTypeDesc = mediaTypeDesc;
                            newFilter.SubMediaTypes = new List<MCMediaReport_SubMediaTypeFilter>();

                            dictMediaTypes.Add(mediaType, newFilter);
                        }

                        MCMediaReport_MediaTypeFilter mediaTypeFilter = dictMediaTypes[mediaType];
                        mediaTypeFilter.RecordCount += subMediaTypeCount;
                        mediaTypeFilter.SubMediaTypes.Add(new MCMediaReport_SubMediaTypeFilter()
                        {
                            SubMediaType = subMediaType,
                            SubMediaTypeDesc = subMediaTypeDesc,
                            RecordCount = subMediaTypeCount
                        });
                    }

                    filterModel.MediaTypes.AddRange(dictMediaTypes.Values);
                    break;
                case "ClientFilter":
                    filterModel.Clients = new List<MCMediaReport_Filter>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        MCMediaReport_Filter clientFilter = new MCMediaReport_Filter();

                        if (!dr["ClientName"].Equals(DBNull.Value))
                        {
                            clientFilter.ClientName = Convert.ToString(dr["ClientName"]);
                        }
                        if (!dr["ClientGUID"].Equals(DBNull.Value))
                        {
                            clientFilter.ClientGuid = Convert.ToString(dr["ClientGUID"]);
                        }
                        if (!dr["NumResults"].Equals(DBNull.Value))
                        {
                            clientFilter.RecordCount = Convert.ToInt64(dr["NumResults"]);
                        }

                        filterModel.Clients.Add(clientFilter);
                    }
                    break;
                case "CategoryFilter":
                    filterModel.Categories = new List<MCMediaReport_Filter>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        MCMediaReport_Filter categoryFilter = new MCMediaReport_Filter();

                        if (!dr["CategoryName"].Equals(DBNull.Value))
                        {
                            categoryFilter.CategoryName = Convert.ToString(dr["CategoryName"]);
                        }
                        if (!dr["NumResults"].Equals(DBNull.Value))
                        {
                            categoryFilter.RecordCount = Convert.ToInt64(dr["NumResults"]);
                        }

                        filterModel.Categories.Add(categoryFilter);
                    }
                    break;
            }
        }

        protected string GetClosedCaptionText(string string_XML)
        {
            if (!string.IsNullOrEmpty(string_XML))
            {
                XDocument xdoc = XDocument.Parse(string_XML);
                xdoc = RemoveNamespace(xdoc);
                string hilightedText = string.Join(" ", xdoc.Descendants("p").Select(e => e.Value));
                return hilightedText.Trim();
            }
            else
            {
                return string.Empty;
            }
        }

        private XDocument RemoveNamespace(XDocument xdoc)
        {
            foreach (XElement e in xdoc.Root.DescendantsAndSelf())
            {
                if (e.Name.Namespace != XNamespace.None)
                {
                    e.Name = XNamespace.None.GetName(e.Name.LocalName);
                }

                if (e.Attributes().Where(a => a.IsNamespaceDeclaration || a.Name.Namespace != XNamespace.None).Any())
                {
                    e.ReplaceAttributes(e.Attributes().Select(a => a.IsNamespaceDeclaration ? null : a.Name.Namespace != XNamespace.None ? new XAttribute(XNamespace.None.GetName(a.Name.LocalName), a.Value) : a));
                }
            }
            return xdoc;
        }
    }
}
