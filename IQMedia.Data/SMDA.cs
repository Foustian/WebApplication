using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using System.Data;
using IQMedia.Model;
using IQMedia.Shared.Utility;

namespace IQMedia.Data
{
    public class SMDA : IDataAccess
    {
        public string InsertArchiveSM(IQAgent_SMResultsModel p_IQAgent_SMResultsModel, Guid p_CustomerGUID, Guid p_ClientGUID, Guid p_CategoryGUID, string p_Keywords, string p_Description, string p_MediaType, string p_SubMediaType, Int64? MediaID, bool UseProminenceMultiplier = false)
        {
            try
            {
                Int32 archiveKey = 0;
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ArticleID", DbType.String, p_IQAgent_SMResultsModel.ArticleID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ArticleUri", DbType.String, p_IQAgent_SMResultsModel.ArticleUri, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Harvest_Time", DbType.DateTime, p_IQAgent_SMResultsModel.ItemHarvestDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Title", DbType.String, p_IQAgent_SMResultsModel.Description, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, p_CustomerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryGuid", DbType.Guid, p_CategoryGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Content", DbType.String, p_IQAgent_SMResultsModel.HighlightingText, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@HomeLink", DbType.String, p_IQAgent_SMResultsModel.HomeLink, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CompeteURL", DbType.String, p_IQAgent_SMResultsModel.CompeteURL, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PositiveSentiment", DbType.Int32, p_IQAgent_SMResultsModel.PositiveSentiment, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@NegativeSentiment", DbType.Int32, p_IQAgent_SMResultsModel.NegativeSentiment, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MediaID", DbType.Int64, MediaID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@HighLightText", DbType.String, p_IQAgent_SMResultsModel.HighlightedSMOutput != null ? Shared.Utility.CommonFunctions.SerializeToXml(p_IQAgent_SMResultsModel.HighlightedSMOutput) : null, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchTerm", DbType.String, p_IQAgent_SMResultsModel.SearchTerm, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Number_Hits", DbType.Int32, p_IQAgent_SMResultsModel.Number_Hits, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ThumbUrl", DbType.String, p_IQAgent_SMResultsModel.ThumbUrl, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ArticleStats", DbType.Xml, p_IQAgent_SMResultsModel.ArticleStats != null ? CommonFunctions.SerializeToXml(p_IQAgent_SMResultsModel.ArticleStats) : null, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Keywords", DbType.String, p_Keywords, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Description", DbType.String, p_Description, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ProminenceMultiplier", DbType.Decimal, UseProminenceMultiplier ? p_IQAgent_SMResultsModel.IQProminenceMultiplier : 1, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MediaType", DbType.String, p_MediaType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubMediaType", DbType.String, p_SubMediaType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ArchiveKey", DbType.Int32, archiveKey, ParameterDirection.Output));

                string _Result = DataAccess.ExecuteNonQuery("usp_v5_ArchiveSM_Insert", dataTypeList);
                return _Result;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public int SelectDownloadLimit(string CustomerGUID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.String, CustomerGUID, ParameterDirection.Input));
                int _Result = (int)DataAccess.ExecuteScalar("usp_v4_ArticleSMDownload_SelectDownloadLimit", dataTypeList);
                return _Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string Insert_ArticleSMDownload(string CustomerGUID, long ID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.String, CustomerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ID", DbType.Int64, ID, ParameterDirection.Input));

                string _Result = DataAccess.ExecuteNonQuery("usp_v4_ArticleSMDownload_Insert", dataTypeList);
                return _Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<ArticleSMDownload> SelectArticleSMDownloadByCustomer(string CustomerGUID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@CustomerGUID", DbType.String, CustomerGUID, ParameterDirection.Input));

                DataSet _Result = DataAccess.GetDataSet("usp_v4_ArticleSMDownload_SelectByCustomer", dataTypeList);

                List<ArticleSMDownload> lstArticleSMDownload = FillArticleSMDownload(_Result);

                return lstArticleSMDownload;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ArticleSMDownload SelectArticleSMByID(long ID,Guid CustomerGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ID", DbType.Int64, ID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, CustomerGuid, ParameterDirection.Input));

                DataSet _Result = DataAccess.GetDataSet("usp_v4_ArticleSMDownload_SelectByID", dataTypeList);

                ArticleSMDownload objArticleSMDownload = FillArticleSMDownloadByID(_Result);

                return objArticleSMDownload;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string UpdateDownloadStatusByID(long ID,Guid CustomerGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ID", DbType.Int64, ID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, CustomerGuid, ParameterDirection.Input));

                string _Result = DataAccess.ExecuteNonQuery("usp_v4_ArticleSMDownload_UpdateDownloadStatusByID", dataTypeList);
                return _Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private List<ArticleSMDownload> FillArticleSMDownload(DataSet dataSet)
        {
            List<ArticleSMDownload> lstArticleSMDownload = new List<ArticleSMDownload>();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    ArticleSMDownload objArticleSMDownload = new ArticleSMDownload();
                    if (!dr["ID"].Equals(DBNull.Value))
                    {
                        objArticleSMDownload.ID = Convert.ToInt64(dr["ID"]);
                    }
                    if (!dr["ArticleID"].Equals(DBNull.Value))
                    {
                        objArticleSMDownload.ArticleID = Convert.ToString(dr["ArticleID"]);
                    }
                    if (!dr["Title"].Equals(DBNull.Value))
                    {
                        objArticleSMDownload.ArticleTitle = Convert.ToString(dr["Title"]);
                    }
                    if (!dr["CustomerGuid"].Equals(DBNull.Value))
                    {
                        objArticleSMDownload.CustomerGuid = Convert.ToString(dr["CustomerGuid"]);
                    }
                    if (!dr["DownloadStatus"].Equals(DBNull.Value))
                    {
                        objArticleSMDownload.DownloadStatus = Convert.ToInt32(dr["DownloadStatus"]);
                    }
                    if (!dr["DownloadLocation"].Equals(DBNull.Value))
                    {
                        objArticleSMDownload.DownloadLocation = Convert.ToString(dr["DownloadLocation"]);
                    }
                    if (!dr["DLRequestDateTime"].Equals(DBNull.Value))
                    {
                        objArticleSMDownload.DLRequestDateTime = Convert.ToDateTime(dr["DLRequestDateTime"]);
                    }
                    if (!dr["DownLoadedDateTime"].Equals(DBNull.Value))
                    {
                        objArticleSMDownload.DownLoadedDateTime = Convert.ToDateTime(dr["DownLoadedDateTime"]);
                    }
                    if (!dr["IsActive"].Equals(DBNull.Value))
                    {
                        objArticleSMDownload.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    }
                    lstArticleSMDownload.Add(objArticleSMDownload);
                }
            }

            return lstArticleSMDownload;

        }

        private ArticleSMDownload FillArticleSMDownloadByID(DataSet dataSet)
        {
            ArticleSMDownload objArticleSMDownload = new ArticleSMDownload();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    if (!dr["ID"].Equals(DBNull.Value))
                    {
                        objArticleSMDownload.ID = Convert.ToInt64(dr["ID"]);
                    }
                    if (!dr["ArticleID"].Equals(DBNull.Value))
                    {
                        objArticleSMDownload.ArticleID = Convert.ToString(dr["ArticleID"]);
                    }
                    if (!dr["CustomerGuid"].Equals(DBNull.Value))
                    {
                        objArticleSMDownload.CustomerGuid = Convert.ToString(dr["CustomerGuid"]);
                    }
                    if (!dr["DownloadStatus"].Equals(DBNull.Value))
                    {
                        objArticleSMDownload.DownloadStatus = Convert.ToInt32(dr["DownloadStatus"]);
                    }
                    if (!dr["DownloadLocation"].Equals(DBNull.Value))
                    {
                        objArticleSMDownload.DownloadLocation = Convert.ToString(dr["DownloadLocation"]);
                    }
                    if (!dr["DLRequestDateTime"].Equals(DBNull.Value))
                    {
                        objArticleSMDownload.DLRequestDateTime = Convert.ToDateTime(dr["DLRequestDateTime"]);
                    }
                    if (!dr["DownLoadedDateTime"].Equals(DBNull.Value))
                    {
                        objArticleSMDownload.DownLoadedDateTime = Convert.ToDateTime(dr["DownLoadedDateTime"]);
                    }
                    if (!dr["IsActive"].Equals(DBNull.Value))
                    {
                        objArticleSMDownload.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    }
                }
            }

            return objArticleSMDownload;
        }
    }
}
