using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using System.Data;
using IQMedia.Model;

namespace IQMedia.Data
{
    public class NMDA : IDataAccess
    {
        public int SelectDownloadLimit(string CustomerGUID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.String, CustomerGUID, ParameterDirection.Input));
                int _Result = (int)DataAccess.ExecuteScalar("usp_v4_ArticleNMDownload_SelectDownloadLimit", dataTypeList);
                return _Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string Insert_ArticleNMDownload(string CustomerGUID, long ID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.String, CustomerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ID", DbType.Int64, ID, ParameterDirection.Input));

                string _Result = DataAccess.ExecuteNonQuery("usp_v4_ArticleNMDownload_Insert", dataTypeList);
                return _Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<ArticleNMDownload> SelectArticleNMDownloadByCustomer(string CustomerGUID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@CustomerGUID", DbType.String, CustomerGUID, ParameterDirection.Input));

                DataSet _Result = DataAccess.GetDataSet("usp_v4_ArticleNMDownload_SelectByCustomer", dataTypeList);

                List<ArticleNMDownload> lstArticleNMDownload = FillArticleNMDownload(_Result);

                return lstArticleNMDownload;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ArticleNMDownload SelectArticleNMByID(long ID,Guid CustomerGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ID", DbType.Int64, ID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, CustomerGuid, ParameterDirection.Input));

                DataSet _Result = DataAccess.GetDataSet("usp_v4_ArticleNMDownload_SelectByID", dataTypeList);

                ArticleNMDownload objArticleNMDownload = FillArticleNMDownloadByID(_Result);

                return objArticleNMDownload;
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
                string _Result = DataAccess.ExecuteNonQuery("usp_v4_ArticleNMDownload_UpdateDownloadStatusByID", dataTypeList);
                return _Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private List<ArticleNMDownload> FillArticleNMDownload(DataSet dataSet)
        {
            List<ArticleNMDownload> lstArticleNMDownload = new List<ArticleNMDownload>();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    ArticleNMDownload objArticleNMDownload = new ArticleNMDownload();
                    if (!dr["ID"].Equals(DBNull.Value))
                    {
                        objArticleNMDownload.ID = Convert.ToInt64(dr["ID"]);
                    }
                    if (!dr["ArticleID"].Equals(DBNull.Value))
                    {
                        objArticleNMDownload.ArticleID = Convert.ToString(dr["ArticleID"]);
                    }
                    if (!dr["Title"].Equals(DBNull.Value))
                    {
                        objArticleNMDownload.ArticleTitle = Convert.ToString(dr["Title"]);
                    }
                    if (!dr["CustomerGuid"].Equals(DBNull.Value))
                    {
                        objArticleNMDownload.CustomerGuid = Convert.ToString(dr["CustomerGuid"]);
                    }
                    if (!dr["DownloadStatus"].Equals(DBNull.Value))
                    {
                        objArticleNMDownload.DownloadStatus = Convert.ToInt32(dr["DownloadStatus"]);
                    }
                    if (!dr["DownloadLocation"].Equals(DBNull.Value))
                    {
                        objArticleNMDownload.DownloadLocation = Convert.ToString(dr["DownloadLocation"]);
                    }
                    if (!dr["DLRequestDateTime"].Equals(DBNull.Value))
                    {
                        objArticleNMDownload.DLRequestDateTime = Convert.ToDateTime(dr["DLRequestDateTime"]);
                    }
                    if (!dr["DownLoadedDateTime"].Equals(DBNull.Value))
                    {
                        objArticleNMDownload.DownLoadedDateTime = Convert.ToDateTime(dr["DownLoadedDateTime"]);
                    }
                    if (!dr["IsActive"].Equals(DBNull.Value))
                    {
                        objArticleNMDownload.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    }
                    lstArticleNMDownload.Add(objArticleNMDownload);
                }
            }

            return lstArticleNMDownload;

        }

        private ArticleNMDownload FillArticleNMDownloadByID(DataSet dataSet)
        {
            ArticleNMDownload objArticleNMDownload = new ArticleNMDownload();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    if (!dr["ID"].Equals(DBNull.Value))
                    {
                        objArticleNMDownload.ID = Convert.ToInt64(dr["ID"]);
                    }
                    if (!dr["ArticleID"].Equals(DBNull.Value))
                    {
                        objArticleNMDownload.ArticleID = Convert.ToString(dr["ArticleID"]);
                    }
                    if (!dr["CustomerGuid"].Equals(DBNull.Value))
                    {
                        objArticleNMDownload.CustomerGuid = Convert.ToString(dr["CustomerGuid"]);
                    }
                    if (!dr["DownloadStatus"].Equals(DBNull.Value))
                    {
                        objArticleNMDownload.DownloadStatus = Convert.ToInt32(dr["DownloadStatus"]);
                    }
                    if (!dr["DownloadLocation"].Equals(DBNull.Value))
                    {
                        objArticleNMDownload.DownloadLocation = Convert.ToString(dr["DownloadLocation"]);
                    }
                    if (!dr["DLRequestDateTime"].Equals(DBNull.Value))
                    {
                        objArticleNMDownload.DLRequestDateTime = Convert.ToDateTime(dr["DLRequestDateTime"]);
                    }
                    if (!dr["DownLoadedDateTime"].Equals(DBNull.Value))
                    {
                        objArticleNMDownload.DownLoadedDateTime = Convert.ToDateTime(dr["DownLoadedDateTime"]);
                    }
                    if (!dr["IsActive"].Equals(DBNull.Value))
                    {
                        objArticleNMDownload.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    }
                }
            }

            return objArticleNMDownload;
        }

        public string InsertArchiveNM(IQAgent_NewsResultsModel p_IQAgent_NewsResultsModel, Guid p_CustomerGUID, Guid p_ClientGUID, Guid p_CategoryGUID,string p_Event, string p_Keywords, string p_Description, string p_MediaType, string p_SubMediaType, Int64? MediaID, bool UseProminenceMultiplier = false)
        {
            try
            {
                Int32 archiveKey = 0;
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ArticleID", DbType.String, p_IQAgent_NewsResultsModel.ArticleID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ArticleUri", DbType.String, p_IQAgent_NewsResultsModel.ArticleUri, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Harvest_Time", DbType.DateTime, p_IQAgent_NewsResultsModel.Harvest_Time, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Title", DbType.String, p_IQAgent_NewsResultsModel.Title, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, p_CustomerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryGuid", DbType.Guid, p_CategoryGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Content", DbType.String, p_IQAgent_NewsResultsModel.HighlightingText, ParameterDirection.Input));                
                dataTypeList.Add(new DataType("@Publication", DbType.String, p_IQAgent_NewsResultsModel.Publication, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CompeteUrl", DbType.String, p_IQAgent_NewsResultsModel.CompeteUrl, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PositiveSentiment", DbType.Int32, p_IQAgent_NewsResultsModel.PositiveSentiment, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@NegativeSentiment", DbType.Int32, p_IQAgent_NewsResultsModel.NegativeSentiment, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IQLicense", DbType.Int16, p_IQAgent_NewsResultsModel.IQLicense, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Event", DbType.String, p_Event, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MediaID", DbType.Int64, MediaID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@HighLightText", DbType.String, p_IQAgent_NewsResultsModel.HighlightedNewsOutput != null ? Shared.Utility.CommonFunctions.SerializeToXml(p_IQAgent_NewsResultsModel.HighlightedNewsOutput) : null, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchTerm", DbType.String, p_IQAgent_NewsResultsModel.SearchTerm, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Number_Hits", DbType.Int32, p_IQAgent_NewsResultsModel.Number_Hits, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Keywords", DbType.String, p_Keywords, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Description", DbType.String, p_Description, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ProminenceMultiplier", DbType.Decimal, UseProminenceMultiplier ? p_IQAgent_NewsResultsModel.IQProminenceMultiplier : 1, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MediaType", DbType.String, p_MediaType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubMediaType", DbType.String, p_SubMediaType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ArchiveKey", DbType.Int32, archiveKey, ParameterDirection.Output));

                string _Result = DataAccess.ExecuteNonQuery("usp_v5_ArchiveNM_Insert", dataTypeList);
                return _Result;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
