using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using IQMedia.Model;
using System.Data;

namespace IQMedia.Data
{
    public class ClipDA : IDataAccess
    {

        public List<IQTrackPlayLogModel> GetPlayLogNSummary(string p_AssetGuid, DateTime p_FromDate, DateTime p_ToDate)
        {
            try
            {

                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@AssetGuid", DbType.String, p_AssetGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_IQTrack_PlayLogNSummary_Select", dataTypeList);

                List<IQTrackPlayLogModel> lstIQTrackPlayLogModel = FillPlayLogNSummary(dataset);

                return lstIQTrackPlayLogModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<IQTrackPlayLogModel> FillPlayLogNSummary(DataSet dsTPL)
        {
            try
            {
                List<IQTrackPlayLogModel> lstIQTrackPlayLogModel = new List<IQTrackPlayLogModel>();
                if (dsTPL != null && dsTPL.Tables.Count > 1)
                {
                    foreach (DataRow datarow in dsTPL.Tables[1].Rows)
                    {
                        IQTrackPlayLogModel _IQTrackPlayLogModel = new IQTrackPlayLogModel();

                        if (dsTPL.Tables[1].Columns.Contains("Count"))
                        {
                            _IQTrackPlayLogModel.Count = Convert.ToInt64(datarow["Count"]);
                        }
                        
                        if (dsTPL.Tables[1].Columns.Contains("PlayDate"))
                        {
                            _IQTrackPlayLogModel.PlayDate = Convert.ToDateTime(datarow["PlayDate"]);
                        }

                        lstIQTrackPlayLogModel.Add(_IQTrackPlayLogModel);
                    }
                }
                if (dsTPL != null && dsTPL.Tables.Count > 0)
                {
                    if (dsTPL.Tables[0].Columns.Contains("ClipTitle") && dsTPL.Tables[0].Rows.Count > 0)
                    {
                        if (lstIQTrackPlayLogModel != null && lstIQTrackPlayLogModel.Count > 0)
                        {
                            lstIQTrackPlayLogModel.FirstOrDefault().ClipTitle = dsTPL.Tables[0].Rows[0]["ClipTitle"].ToString();
                        }
                        else
                        {
                            IQTrackPlayLogModel _IQTrackPlayLogModel = new IQTrackPlayLogModel();
                            _IQTrackPlayLogModel.ClipTitle = dsTPL.Tables[0].Rows[0]["ClipTitle"].ToString();
                            lstIQTrackPlayLogModel.Add(_IQTrackPlayLogModel);
                        }
                    }
                }
                if (dsTPL != null && dsTPL.Tables.Count > 2)
                {
                    if (dsTPL.Tables[2].Columns.Contains("Count") && dsTPL.Tables[2].Rows.Count > 0)
                    {
                        if (lstIQTrackPlayLogModel != null && lstIQTrackPlayLogModel.Count > 0)
                        {
                            lstIQTrackPlayLogModel.FirstOrDefault().LifeTimeCount = Convert.ToInt64(dsTPL.Tables[2].Rows[0]["Count"]);
                        }
                    }
                }
                if (dsTPL != null && dsTPL.Tables.Count > 3)
                {
                    Dictionary<string, long> regionPlayMapList = new Dictionary<string, long>();
                    foreach (DataRow datarow in dsTPL.Tables[3].Rows)
                    {
                        if (dsTPL.Tables[3].Columns.Contains("Region") && dsTPL.Tables[3].Columns.Contains("Count"))
                        {
                            regionPlayMapList.Add(datarow["Region"].ToString(), Convert.ToInt64(datarow["Count"]));
                        }
                    }

                    if (lstIQTrackPlayLogModel != null && lstIQTrackPlayLogModel.Count > 0)
                    {
                        lstIQTrackPlayLogModel.FirstOrDefault().RegionPlayMapList = regionPlayMapList;
                    }
                }
                if (dsTPL != null && dsTPL.Tables.Count > 4)
                {
                    List<PlayLogTopReferrersModel> topReferrersList = new List<PlayLogTopReferrersModel>();
                    foreach (DataRow datarow in dsTPL.Tables[4].Rows)
                    {
                        PlayLogTopReferrersModel topReferrersModel = new PlayLogTopReferrersModel();

                        if (dsTPL.Tables[4].Columns.Contains("Url"))
                        {
                            topReferrersModel.Url = datarow["Url"].ToString();
                        }

                        if (dsTPL.Tables[4].Columns.Contains("Count"))
                        {
                            topReferrersModel.ViewsCount = Convert.ToInt64(datarow["Count"]);

                            Int64 totalViews = lstIQTrackPlayLogModel.Sum(s => s.Count);
                            if (totalViews > 0)
                            {
                                topReferrersModel.ViewsPercent = (topReferrersModel.ViewsCount * 100) / totalViews;
                            }
                        }

                        topReferrersList.Add(topReferrersModel);
                    }

                    if (lstIQTrackPlayLogModel != null && lstIQTrackPlayLogModel.Count > 0)
                    {
                        lstIQTrackPlayLogModel.FirstOrDefault().TopReferrersList = topReferrersList;
                    }
                }
                return lstIQTrackPlayLogModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Dictionary<Guid, string> GetClipCCLocation(Guid p_ClientGUID, string p_ClipXml)
        {
            Dictionary<Guid, string> clipLocation = new Dictionary<Guid, string>();
            
            List<DataType> dataTypeList = new List<DataType>();

            dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@ClipXml", DbType.Xml, p_ClipXml, ParameterDirection.Input));

            DataSet dataset = DataAccess.GetDataSet("usp_v5_ClipMeta_SelectCCLocation", dataTypeList);

            foreach (DataRow dr in dataset.Tables[0].Rows)
            {
                clipLocation.Add(Guid.Parse(Convert.ToString(dr["ClipGUID"])), Convert.ToString(dr["FileLocation"]));
            }

            return clipLocation;
        }

        public int UpdateClipDownloadCCStatus(Int64 p_ClipDownloadKey, Guid p_ClipGUID)
        {
            List<DataType> dataTypeList = new List<DataType>();

            dataTypeList.Add(new DataType("@ClipDownloadKey", DbType.Int64, p_ClipDownloadKey, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@ClipGUID", DbType.Guid, p_ClipGUID, ParameterDirection.Input));

            return Convert.ToInt32(DataAccess.ExecuteScalar("usp_v5_ClipDownload_UpdateClipCC", dataTypeList)); 
        }

        public int UpdateRadioClipDownloadCCStatus(Int64 p_ClipDownloadKey, Guid p_ClipGUID)
        {
            List<DataType> dataTypeList = new List<DataType>();

            dataTypeList.Add(new DataType("@ClipDownloadKey", DbType.Int64, p_ClipDownloadKey, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@ClipGUID", DbType.Guid, p_ClipGUID, ParameterDirection.Input));

            return Convert.ToInt32(DataAccess.ExecuteScalar("usp_v5_ArticleRadioDownload_UpdateClipCC", dataTypeList));
        }
    }
}
