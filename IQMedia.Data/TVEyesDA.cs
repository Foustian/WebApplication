using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using System.Data;
using IQMedia.Model;

namespace IQMedia.Data
{
    public class TVEyesDA : IDataAccess 
    {
        public int SelectDownloadLimit(Guid CustomerGUID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.Guid, CustomerGUID, ParameterDirection.Input));
                int _Result = (int)DataAccess.ExecuteScalar("usp_v4_ArticleTVEyesDownload_SelectDownloadLimit", dataTypeList);
                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Insert_ArticleTVEyesDownload(Guid CustomerGUID, long ID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.Guid, CustomerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ID", DbType.Int64, ID, ParameterDirection.Input));

                string _Result = DataAccess.ExecuteNonQuery("usp_v4_ArticleTVEyesDownload_Insert", dataTypeList);
                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ArticleTVEyesDownload> SelectArticleTVEyesDownloadByCustomer(Guid CustomerGUID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@CustomerGUID", DbType.Guid, CustomerGUID, ParameterDirection.Input));

                DataSet _Result = DataAccess.GetDataSet("usp_v4_ArticleTVEyesDownload_SelectByCustomer", dataTypeList);

                List<ArticleTVEyesDownload> lstArticleTVEyesDownload = FillArticleTVEyesDownload(_Result);

                return lstArticleTVEyesDownload;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ArticleTVEyesDownload SelectArticleTVEyesByID(long ID, Guid CustomerGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ID", DbType.Int64, ID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, CustomerGuid, ParameterDirection.Input));

                DataSet _Result = DataAccess.GetDataSet("usp_v4_ArticleTVEyesDownload_SelectByID", dataTypeList);

                ArticleTVEyesDownload objArticleTVEyesDownload = FillArticleTVEyesDownloadByID(_Result);

                return objArticleTVEyesDownload;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string UpdateDownloadStatusByID(long ID, Guid CustomerGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ID", DbType.Int64, ID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, CustomerGuid, ParameterDirection.Input));
                string _Result = DataAccess.ExecuteNonQuery("usp_v4_ArticleTVEyesDownload_UpdateDownloadStatusByID", dataTypeList);
                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string InsertArchiveTVEyes(long mediaID, Guid customerGUID, Guid clientGUID, Guid categoryGUID, IQAgent_TVEyesResultsModel iqAgent_TVEyesResultsModel, string keywords, string description, string p_MediaType, string p_SubMediaType)
        {
            try
            {
                Int32 archiveKey = 0;
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@MediaID", DbType.Int64, mediaID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, customerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, clientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryGuid", DbType.Guid, categoryGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Title", DbType.String, iqAgent_TVEyesResultsModel.Title, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@StationID", DbType.String, iqAgent_TVEyesResultsModel.StationID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Market", DbType.String, iqAgent_TVEyesResultsModel.Market, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@DMARank", DbType.String, iqAgent_TVEyesResultsModel.DMARank, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@StationIDNum", DbType.String, iqAgent_TVEyesResultsModel.StationIDNum, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Duration", DbType.Int32, iqAgent_TVEyesResultsModel.Duration, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Transcript", DbType.String, iqAgent_TVEyesResultsModel.HighlightingText, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@UTCDateTime", DbType.DateTime, iqAgent_TVEyesResultsModel.UTCDateTime, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@LocalDateTime", DbType.DateTime, iqAgent_TVEyesResultsModel.LocalDateTime, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PositiveSentiment", DbType.Int16, iqAgent_TVEyesResultsModel.PositiveSentiment, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@NegativeSentiment", DbType.Int16, iqAgent_TVEyesResultsModel.NegativeSentiment, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@TimeZone", DbType.String, iqAgent_TVEyesResultsModel.TimeZone, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Keywords", DbType.String, keywords, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Description", DbType.String, description, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MediaType", DbType.String, p_MediaType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubMediaType", DbType.String, p_SubMediaType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ArchiveKey", DbType.Int32, archiveKey, ParameterDirection.Output));

                string _Result = DataAccess.ExecuteNonQuery("usp_v5_ArchiveTVEyes_Insert", dataTypeList);
                return _Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private List<ArticleTVEyesDownload> FillArticleTVEyesDownload(DataSet dataSet)
        {
            List<ArticleTVEyesDownload> lstArticleTVEyesDownload = new List<ArticleTVEyesDownload>();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    ArticleTVEyesDownload objArticleTVEyesDownload = new ArticleTVEyesDownload();
                    if (!dr["ID"].Equals(DBNull.Value))
                    {
                        objArticleTVEyesDownload.ID = Convert.ToInt64(dr["ID"]);
                    }
                    if (!dr["ArchiveTVEyesKey"].Equals(DBNull.Value))
                    {
                        objArticleTVEyesDownload.ArticleID = Convert.ToString(dr["ArchiveTVEyesKey"]);
                    }
                    if (!dr["Title"].Equals(DBNull.Value))
                    {
                        objArticleTVEyesDownload.ArticleTitle = Convert.ToString(dr["Title"]);
                    }
                    if (!dr["CustomerGuid"].Equals(DBNull.Value))
                    {
                        objArticleTVEyesDownload.CustomerGuid = Convert.ToString(dr["CustomerGuid"]);
                    }
                    if (!dr["DownloadStatus"].Equals(DBNull.Value))
                    {
                        objArticleTVEyesDownload.DownloadStatus = Convert.ToInt32(dr["DownloadStatus"]);
                    }
                    if (!dr["DownloadLocation"].Equals(DBNull.Value))
                    {
                        objArticleTVEyesDownload.DownloadLocation = Convert.ToString(dr["DownloadLocation"]);
                    }
                    if (!dr["DLRequestDateTime"].Equals(DBNull.Value))
                    {
                        objArticleTVEyesDownload.DLRequestDateTime = Convert.ToDateTime(dr["DLRequestDateTime"]);
                    }
                    if (!dr["DownLoadedDateTime"].Equals(DBNull.Value))
                    {
                        objArticleTVEyesDownload.DownLoadedDateTime = Convert.ToDateTime(dr["DownLoadedDateTime"]);
                    }
                    if (!dr["IsActive"].Equals(DBNull.Value))
                    {
                        objArticleTVEyesDownload.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    }
                    lstArticleTVEyesDownload.Add(objArticleTVEyesDownload);
                }
            }

            return lstArticleTVEyesDownload;

        }

        private ArticleTVEyesDownload FillArticleTVEyesDownloadByID(DataSet dataSet)
        {
            ArticleTVEyesDownload objArticleTVEyesDownload = new ArticleTVEyesDownload();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    if (!dr["ID"].Equals(DBNull.Value))
                    {
                        objArticleTVEyesDownload.ID = Convert.ToInt64(dr["ID"]);
                    }
                    if (!dr["ArticleID"].Equals(DBNull.Value))
                    {
                        objArticleTVEyesDownload.ArticleID = Convert.ToString(dr["ArticleID"]);
                    }
                    if (!dr["CustomerGuid"].Equals(DBNull.Value))
                    {
                        objArticleTVEyesDownload.CustomerGuid = Convert.ToString(dr["CustomerGuid"]);
                    }
                    if (!dr["DownloadStatus"].Equals(DBNull.Value))
                    {
                        objArticleTVEyesDownload.DownloadStatus = Convert.ToInt32(dr["DownloadStatus"]);
                    }
                    if (!dr["DownloadLocation"].Equals(DBNull.Value))
                    {
                        objArticleTVEyesDownload.DownloadLocation = Convert.ToString(dr["DownloadLocation"]);
                    }
                    if (!dr["DLRequestDateTime"].Equals(DBNull.Value))
                    {
                        objArticleTVEyesDownload.DLRequestDateTime = Convert.ToDateTime(dr["DLRequestDateTime"]);
                    }
                    if (!dr["DownLoadedDateTime"].Equals(DBNull.Value))
                    {
                        objArticleTVEyesDownload.DownLoadedDateTime = Convert.ToDateTime(dr["DownLoadedDateTime"]);
                    }
                    if (!dr["IsActive"].Equals(DBNull.Value))
                    {
                        objArticleTVEyesDownload.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    }
                }
            }

            return objArticleTVEyesDownload;
        }
    }
}
