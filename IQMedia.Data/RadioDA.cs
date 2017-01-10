using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using IQMedia.Model;
using System.Data;

namespace IQMedia.Data
{
    public class RadioDA : IDataAccess
    {
        public List<RadioStation> SelectRadioStations()
        {
            List<DataType> dataTypeList = new List<DataType>();
            DataSet dataSet = DataAccess.GetDataSet("usp_v4_IQ_Station_SelectRadioStations", dataTypeList);

            List<RadioStation> radioStationList = new List<RadioStation>();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {

                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    RadioStation rs = new RadioStation();

                    rs.DMA = Convert.ToString(dr["Market"]);
                    rs.StationID = Convert.ToString(dr["StationID"]);

                    radioStationList.Add(rs);
                }
            }

            return radioStationList;
        }

        public Dictionary<string,object> SelectRadioStationFilters()
        {
            List<DataType> dataTypeList = new List<DataType>();
            DataSet dataSet = DataAccess.GetDataSet("usp_v4_IQ_Station_SelectRadioFilters", dataTypeList);

            Dictionary<string,object> radioStationFilters = new Dictionary<string,object>();

            if (dataSet != null && dataSet.Tables.Count > 1)
            {
                List<TadsDma> marketList = new List<TadsDma>();
                List<TadsStation> stationList = new List<TadsStation>();
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    TadsDma dma = new TadsDma();

                    dma.Name = Convert.ToString(dr["MarketName"]);
                    dma.ID = Convert.ToString(dr["MarketId"]);
                    marketList.Add(dma);
                }
                radioStationFilters.Add("Market", marketList);
            
                foreach (DataRow dr in dataSet.Tables[1].Rows)
                {
                     TadsStation stat = new TadsStation();

                    stat.ID = Convert.ToString(dr["StationId"]);
                    stat.Name = Convert.ToString(dr["StationName"]);
                    stationList.Add(stat);
                }
                radioStationFilters.Add("Station", stationList);
            }
            return radioStationFilters;
        }

        public List<RadioModel> SelectRadioResults(DateTime? FromDate, DateTime? ToDate, string Market, bool IsAsc, int PageNo, int PageSize, ref long SinceID, out long TotalResults)
        {
            try
            {
                TotalResults = 0;

                Market = !string.IsNullOrWhiteSpace(Market) ? Market : null;

                List<DataType> dataTypeList = new List<DataType>();
                Dictionary<string, string> p_outParameter;

                dataTypeList.Add(new DataType("@FromDate", DbType.DateTime, FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.DateTime, ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Market", DbType.String, Market, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsAsc", DbType.Boolean, IsAsc, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageNo", DbType.Int32, PageNo, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SinceID", DbType.Int64, SinceID, ParameterDirection.Output));
                dataTypeList.Add(new DataType("@TotalResults", DbType.Int64, TotalResults, ParameterDirection.Output));

                DataSet dataSet = DataAccess.GetDataSetWithOutParam("usp_v4_Radio_SelectRadioResults", dataTypeList, out p_outParameter);

                if (p_outParameter != null && p_outParameter.Count > 0)
                {
                    SinceID = !string.IsNullOrWhiteSpace(p_outParameter["@SinceID"]) ? Convert.ToInt64(p_outParameter["@SinceID"]) : 0;
                    TotalResults = !string.IsNullOrWhiteSpace(p_outParameter["@TotalResults"]) ? Convert.ToInt32(p_outParameter["@TotalResults"]) : 0;
                }

                List<RadioModel> lstRadioModels = new List<RadioModel>();

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        RadioModel objRadioModel = new RadioModel();

                        if (!dr["RL_GUIDSKey"].Equals(DBNull.Value))
                        {
                            objRadioModel.RL_GUIDSKey = Convert.ToInt64(dr["RL_GUIDSKey"]);
                        }
                        if (!dr["RL_Station_ID"].Equals(DBNull.Value))
                        {
                            objRadioModel.IQ_Station_ID = Convert.ToString(dr["RL_Station_ID"]);
                        }
                        if (!dr["Dma_Name"].Equals(DBNull.Value))
                        {
                            objRadioModel.Market = Convert.ToString(dr["Dma_Name"]);
                        }
                        if (!dr["RL_StationDateTime"].Equals(DBNull.Value))
                        {
                            objRadioModel.RL_StationDateTime = Convert.ToDateTime(dr["RL_StationDateTime"]);
                        }
                        if (!dr["RL_GUID"].Equals(DBNull.Value))
                        {
                            objRadioModel.RL_GUID = Convert.ToString(dr["RL_GUID"]);
                        }

                        lstRadioModels.Add(objRadioModel);
                    }
                }

                return lstRadioModels;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region Download

        public int SelectDownloadLimit(string CustomerGUID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.String, CustomerGUID, ParameterDirection.Input));

                return (int)DataAccess.ExecuteScalar("usp_v5_ArticleRadioDownload_SelectDownloadLimit", dataTypeList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Insert_ArticleRadioDownload(string CustomerGUID, long ID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.String, CustomerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ID", DbType.Int64, ID, ParameterDirection.Input));

                return DataAccess.ExecuteNonQuery("usp_v5_ArticleRadioDownload_Insert", dataTypeList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Update_ArticleRadioDownload(long ClipdownloadKey, string FileLocation, string FileExtension, int DownloadStatus, Guid CustomerGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClipDownloadKey", DbType.Int64, ClipdownloadKey, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FileLocation", DbType.String, FileLocation, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FileExtension", DbType.String, FileExtension, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@DownloadStatus", DbType.Int32, DownloadStatus, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, CustomerGuid, ParameterDirection.Input));

                return DataAccess.ExecuteNonQuery("usp_v5_ArticleRadioDownload_Update", dataTypeList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Delete_ArticleRadioDownload(string CustomerGUID, long ClipdownloadKey)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.String, CustomerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClipDownloadKey", DbType.Int64, ClipdownloadKey, ParameterDirection.Input));

                return DataAccess.ExecuteNonQuery("usp_v5_ArticleRadioDownload_Delete", dataTypeList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ArticleRadioDownload> SelectArticleRadioDownloadByCustomer(string CustomerGUID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.String, CustomerGUID, ParameterDirection.Input));

                DataSet dataSet = DataAccess.GetDataSet("usp_v5_ArticleRadioDownload_SelectByCustomer", dataTypeList);
                return FillArticleRadioDownloadByCustomer(dataSet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ArticleRadioDownload SelectArticleRadioDownloadByID(long ClipDownloadKey, Guid CustomerGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClipDownloadKey ", DbType.Int64, ClipDownloadKey, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid ", DbType.Guid, CustomerGuid, ParameterDirection.Input));

                DataSet dataSet = DataAccess.GetDataSet("usp_v5_ArticleRadioDownload_SelectByID", dataTypeList);
                return FillArticleRadioDownloadByID(dataSet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string SelectClipLocationFromIQCore_Meta(long ID, Guid CustomerGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ID", DbType.Int64, ID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, CustomerGuid, ParameterDirection.Input));

                string _Result = (string)DataAccess.ExecuteScalar("usp_v5_IQCore_ClipMeta_SelectRadioFileLocationByID", dataTypeList);

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<ArticleRadioDownload> FillArticleRadioDownloadByCustomer(DataSet dataSet)
        {
            List<ArticleRadioDownload> lstDownload = new List<ArticleRadioDownload>();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    ArticleRadioDownload objDownload = new ArticleRadioDownload();

                    if (!dr["ID"].Equals(DBNull.Value))
                    {
                        objDownload.ID = Convert.ToInt64(dr["ID"]);
                    }
                    if (!dr["ClipGuid"].Equals(DBNull.Value))
                    {
                        objDownload.ClipGUID = Convert.ToString(dr["ClipGuid"]);
                    }
                    if (!dr["Title"].Equals(DBNull.Value))
                    {
                        objDownload.ClipTitle = Convert.ToString(dr["Title"]);
                    }
                    if (!dr["ClipDownloadStatus"].Equals(DBNull.Value))
                    {
                        objDownload.ClipDownloadStatus = Convert.ToInt32(dr["ClipDownloadStatus"]);
                    }
                    if (!dr["ClipFileLocation"].Equals(DBNull.Value))
                    {
                        objDownload.ClipFileLocation = Convert.ToString(dr["ClipFileLocation"]);
                    }
                    if (!dr["ClipDLFormat"].Equals(DBNull.Value))
                    {
                        objDownload.ClipFormat = Convert.ToString(dr["ClipDLFormat"]);
                    }
                    if (!dr["ClipDLRequestDateTime"].Equals(DBNull.Value))
                    {
                        objDownload.ClipDLRequestDateTime = Convert.ToDateTime(dr["ClipDLRequestDateTime"]);
                    }
                    if (!dr["ClipDownLoadedDateTime"].Equals(DBNull.Value))
                    {
                        objDownload.ClipDownLoadedDateTime = Convert.ToDateTime(dr["ClipDownLoadedDateTime"]);
                    }
                    if (!dr["IsActive"].Equals(DBNull.Value))
                    {
                        objDownload.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    }
                    lstDownload.Add(objDownload);
                }
            }

            return lstDownload;
        }

        private ArticleRadioDownload FillArticleRadioDownloadByID(DataSet dataSet)
        {
            ArticleRadioDownload objDownload = new ArticleRadioDownload();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    if (!dr["ID"].Equals(DBNull.Value))
                    {
                        objDownload.ID = Convert.ToInt64(dr["ID"]);
                    }
                    if (!dr["ClipGuid"].Equals(DBNull.Value))
                    {
                        objDownload.ClipGUID = Convert.ToString(dr["ClipGuid"]);
                    }
                    if (!dr["ClipDownloadStatus"].Equals(DBNull.Value))
                    {
                        objDownload.ClipDownloadStatus = Convert.ToInt32(dr["ClipDownloadStatus"]);
                    }
                    if (!dr["ClipFileLocation"].Equals(DBNull.Value))
                    {
                        objDownload.ClipFileLocation = Convert.ToString(dr["ClipFileLocation"]);
                    }
                    if (!dr["ClipDLFormat"].Equals(DBNull.Value))
                    {
                        objDownload.ClipFormat = Convert.ToString(dr["ClipDLFormat"]);
                    }
                    if (!dr["ClipDLRequestDateTime"].Equals(DBNull.Value))
                    {
                        objDownload.ClipDLRequestDateTime = Convert.ToDateTime(dr["ClipDLRequestDateTime"]);
                    }
                    if (!dr["ClipDownLoadedDateTime"].Equals(DBNull.Value))
                    {
                        objDownload.ClipDownLoadedDateTime = Convert.ToDateTime(dr["ClipDownLoadedDateTime"]);
                    }
                    if (!dr["IsActive"].Equals(DBNull.Value))
                    {
                        objDownload.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    }
                }
            }

            return objDownload;
        }

        #endregion
    }
}
