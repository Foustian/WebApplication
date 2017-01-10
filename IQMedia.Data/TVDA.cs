using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using System.Data;
using IQMedia.Model;
using System.Xml.Linq;

namespace IQMedia.Data
{
    public class TVDA : IDataAccess
    {
        public int SelectDownloadLimit(string CustomerGUID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.String, CustomerGUID, ParameterDirection.Input));
                int _Result = (int)DataAccess.ExecuteScalar("usp_v4_ClipDownload_SelectDownloadLimit", dataTypeList);
                return _Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string Insert_ClipDownload(string CustomerGUID, long ID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.String, CustomerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ID", DbType.Int64, ID, ParameterDirection.Input));

                string _Result = DataAccess.ExecuteNonQuery("usp_v4_ClipDownload_Insert", dataTypeList);
                return _Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<ClipDownload> SelectClipDownloadByCustomer(string CustomerGUID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.String, CustomerGUID, ParameterDirection.Input));

                DataSet dataSet = DataAccess.GetDataSet("usp_v4_ClipDownload_SelectByCustomer", dataTypeList);
                List<ClipDownload> lstClipDownload = FillClipDownloadByCustomer(dataSet);
                return lstClipDownload;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string SelectClipLocationFromIQCore_Meta(long ID,Guid CustomerGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ID", DbType.Int64, ID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, CustomerGuid, ParameterDirection.Input));

                string _Result = (string)DataAccess.ExecuteScalar("usp_v4_IQCore_ClipMeta_SelectFileLocationByID", dataTypeList);

                return _Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool CheckIntoIQServiceAndIQRemoetService_Export(string ClipGUID, string Extension)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClipID", DbType.String, ClipGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClipExtension", DbType.String, Extension, ParameterDirection.Input));

                bool IsClipLocationAvailable = (bool)DataAccess.ExecuteScalar("usp_v4_IQServiceAndIQRemoteService_Export_SelectByClipID", dataTypeList);

                return IsClipLocationAvailable;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string Update_ClipDownload(long ClipdownloadKey,string FileLocation,string FileExtension, int DownloadStatus,Guid CustomerGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClipDownloadKey", DbType.Int64, ClipdownloadKey, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FileLocation", DbType.String, FileLocation, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FileExtension", DbType.String, FileExtension, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@DownloadStatus", DbType.Int32, DownloadStatus, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, CustomerGuid, ParameterDirection.Input));

                string _Result = DataAccess.ExecuteNonQuery("usp_v4_ClipDownload_Update", dataTypeList);

                return _Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string Delete_ClipDownload(string CustomerGUID,long ClipdownloadKey)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.String, CustomerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClipDownloadKey", DbType.Int64, ClipdownloadKey, ParameterDirection.Input));

                string _Result = DataAccess.ExecuteNonQuery("usp_v4_ClipDownload_Delete", dataTypeList);

                return _Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ClipDownload SelectByClipDownloadKey(long ClipDownloadKey,Guid CustomerGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClipDownloadKey ", DbType.Int64, ClipDownloadKey, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid ", DbType.Guid, CustomerGuid, ParameterDirection.Input));

                DataSet dataSet = DataAccess.GetDataSet("usp_v4_ClipDownload_SelectByClipDownloadKey", dataTypeList);
                ClipDownload objClipDownload = FillClipDownloadByClipDownloadKey(dataSet);
                return objClipDownload;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private List<ClipDownload> FillClipDownloadByCustomer(DataSet dataSet)
        {
            List<ClipDownload> lstClipDownload = new List<ClipDownload>();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    ClipDownload objClipDownload = new ClipDownload();

                    if (!dr["IQ_ClipDownload_Key"].Equals(DBNull.Value))
                    {
                        objClipDownload.ID = Convert.ToInt64(dr["IQ_ClipDownload_Key"]);
                    }
                    if (!dr["ClipID"].Equals(DBNull.Value))
                    {
                        objClipDownload.ClipGUID = Convert.ToString(dr["ClipID"]);
                    }
                    if (!dr["ClipTitle"].Equals(DBNull.Value))
                    {
                        objClipDownload.ClipTitle = Convert.ToString(dr["ClipTitle"]);
                    }
                    if (!dr["ClipDownloadStatus"].Equals(DBNull.Value))
                    {
                        objClipDownload.ClipDownloadStatus = Convert.ToInt32(dr["ClipDownloadStatus"]);
                    }
                    if (!dr["ClipFileLocation"].Equals(DBNull.Value))
                    {
                        objClipDownload.ClipFileLocation = Convert.ToString(dr["ClipFileLocation"]);
                    }
                    if (!dr["ClipDLFormat"].Equals(DBNull.Value))
                    {
                        objClipDownload.ClipFormat = Convert.ToString(dr["ClipDLFormat"]);
                    }
                    if (!dr["ClipDLRequestDateTime"].Equals(DBNull.Value))
                    {
                        objClipDownload.ClipDLRequestDateTime = Convert.ToDateTime(dr["ClipDLRequestDateTime"]);
                    }
                    if (!dr["ClipDownLoadedDateTime"].Equals(DBNull.Value))
                    {
                        objClipDownload.ClipDownLoadedDateTime = Convert.ToDateTime(dr["ClipDownLoadedDateTime"]);
                    }
                    if (!dr["IsActive"].Equals(DBNull.Value))
                    {
                        objClipDownload.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    }
                    lstClipDownload.Add(objClipDownload);
                }
            }

            return lstClipDownload;
        }

        private ClipDownload FillClipDownloadByClipDownloadKey(DataSet dataSet)
        {
            ClipDownload objClipDownload = new ClipDownload();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    if (!dr["IQ_ClipDownload_Key"].Equals(DBNull.Value))
                    {
                        objClipDownload.ID = Convert.ToInt64(dr["IQ_ClipDownload_Key"]);
                    }
                    if (!dr["ClipID"].Equals(DBNull.Value))
                    {
                        objClipDownload.ClipGUID = Convert.ToString(dr["ClipID"]);
                    }
                    if (!dr["ClipDownloadStatus"].Equals(DBNull.Value))
                    {
                        objClipDownload.ClipDownloadStatus = Convert.ToInt32(dr["ClipDownloadStatus"]);
                    }
                    if (!dr["ClipFileLocation"].Equals(DBNull.Value))
                    {
                        objClipDownload.ClipFileLocation = Convert.ToString(dr["ClipFileLocation"]);
                    }
                    if (!dr["ClipDLFormat"].Equals(DBNull.Value))
                    {
                        objClipDownload.ClipFormat = Convert.ToString(dr["ClipDLFormat"]);
                    }
                    if (!dr["ClipDLRequestDateTime"].Equals(DBNull.Value))
                    {
                        objClipDownload.ClipDLRequestDateTime = Convert.ToDateTime(dr["ClipDLRequestDateTime"]);
                    }
                    if (!dr["ClipDownLoadedDateTime"].Equals(DBNull.Value))
                    {
                        objClipDownload.ClipDownLoadedDateTime = Convert.ToDateTime(dr["ClipDownLoadedDateTime"]);
                    }
                    if (!dr["IsActive"].Equals(DBNull.Value))
                    {
                        objClipDownload.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    }
                }
            }

            return objClipDownload;
        }

        #region TAds
        public List<string> GetTAdsStations()
        {
            try
            {
                List<string> tAdsStations = new List<string>();
                DataSet dataSet = DataAccess.GetDataSet("usp_v4_ADS_GetStations", new List<DataType>());

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        tAdsStations.Add(Convert.ToString(dr[0]));
                    }
                }

                return tAdsStations;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<IQTAdsResultModel> GetTadsResultByIQCCKey(string IQ_CC_Key)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                Dictionary<string, string> outParameter;

                dataTypeList.Add(new DataType("@IQ_CC_Key", DbType.String, IQ_CC_Key, ParameterDirection.Input));
                DataSet dataset = DataAccess.GetDataSetWithOutParam("usp_v4_TAdsResult_SelectByIQCCKey", dataTypeList, out outParameter);

                return FillTadsResults(dataset);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private List<IQTAdsResultModel> FillTadsResults(DataSet dataSet)
        {
            List<IQTAdsResultModel> lstIQTAdsResultModel = new List<IQTAdsResultModel>();

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0] != null)
            {
                DataTable dataTable = dataSet.Tables[0];
                foreach (DataRow dr in dataTable.Rows)
                {
                    IQTAdsResultModel tvAdsResult = new IQTAdsResultModel();

                    if (dataTable.Columns.Contains("ID") && !dr["ID"].Equals(DBNull.Value))
                        if (dataTable.Columns.Contains("IQ_CC_Key") && !dr["IQ_CC_Key"].Equals(DBNull.Value))
                        {
                            tvAdsResult.IQ_CC_Key = Convert.ToString(dr["IQ_CC_Key"]);
                        }
                    if (dataTable.Columns.Contains("StationID") && !dr["StationID"].Equals(DBNull.Value))
                    {
                        tvAdsResult.StationID = Convert.ToString(dr["StationID"]);
                    }
                    if (dataTable.Columns.Contains("Hits") && !dr["Hits"].Equals(DBNull.Value))
                    {
                        string hitString = Convert.ToString(dr["Hits"]);
                        XDocument xdoc = XDocument.Parse(hitString);
                        tvAdsResult.Hits = new List<IQTAdsHit>();

                        foreach (XElement hitNode in xdoc.Root.Descendants())
                        {
                            IQTAdsHit hit = new IQTAdsHit();
                            foreach (XElement content in hitNode.Elements())
                            {
                                string offsetString = content.Value.Replace("s", "");
                                if (!String.IsNullOrWhiteSpace(offsetString))
                                {
                                    double offsetSeconds = Convert.ToDouble(offsetString);
                                    //double offsetSeconds = TimeSpan.FromMilliseconds(Convert.ToDouble(offsetDouble)).TotalSeconds;

                                    if (content.Name == "Begin")
                                    {
                                        double secondsFloor = Math.Floor(offsetSeconds);
                                        int finalSeconds = Convert.ToInt32(secondsFloor);

                                        hit.startOffset = finalSeconds;
                                    }
                                    else if (content.Name == "End")
                                    {
                                        double secondsCeiling = Math.Ceiling(offsetSeconds);
                                        int finalSeconds = Convert.ToInt32(secondsCeiling);

                                        hit.endOffset = finalSeconds;
                                    }
                                }
                            }

                            if (hit.startOffset != hit.endOffset)
                            {
                                if (tvAdsResult.Hits.Any(x => x.startOffset == hit.startOffset || x.endOffset == hit.startOffset))
                                {
                                    tvAdsResult.Hits.Where(x => x.startOffset == hit.startOffset || x.endOffset == hit.startOffset).ToList().ForEach(x => x.endOffset = ((hit.endOffset > x.endOffset) ? hit.endOffset : x.endOffset));
                                }
                                else if (!tvAdsResult.Hits.Any(x => x.startOffset < hit.startOffset && x.endOffset >= hit.endOffset))
                                {
                                    tvAdsResult.Hits.Add(hit);
                                }
                            }
                        }
                    }
                    if (dataTable.Columns.Contains("HitCount") && !dr["HitCount"].Equals(DBNull.Value))
                    {
                        tvAdsResult.HitCount = Convert.ToInt32(dr["HitCount"]);
                    }
                    if (dataTable.Columns.Contains("CreatedDate") && !dr["CreatedDate"].Equals(DBNull.Value))
                    {
                        tvAdsResult.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    }
                    if (dataTable.Columns.Contains("ISactive") && !dr["ISactive"].Equals(DBNull.Value))
                    {
                        tvAdsResult.IsActive = Convert.ToInt32(dr["ISactive"]);
                    }

                    lstIQTAdsResultModel.Add(tvAdsResult);
                }
            }

            return lstIQTAdsResultModel;
        }

        public Dictionary<string, object> GetFilters()
        {
            List<DataType> dataTypeList = new List<DataType>();
            DataSet dataSet = DataAccess.GetDataSet("usp_v5_TAdsResult_GetFilters", dataTypeList);

            List<IQ_Logo> listOfLogosP = new List<IQ_Logo>();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow datarow in dataSet.Tables[0].Rows)
                {
                    IQ_Logo logo = new IQ_Logo();
                    logo.BrandID = Convert.ToString(datarow["BrandID"]);
                    logo.ID = Convert.ToString(datarow["ID"]);
                    logo.Name = Convert.ToString(datarow["Name"]);
                    logo.URL = Convert.ToString(datarow["URL"]);

                    listOfLogosP.Add(logo);
                }
            }

            List<IQ_Brand> listOfBrandsP = new List<IQ_Brand>();
            if (dataSet != null && dataSet.Tables.Count > 1)
            {
                foreach (DataRow datarow in dataSet.Tables[1].Rows)
                {
                    IQ_Brand brand = new IQ_Brand();
                    brand.ID = Convert.ToString(datarow["ID"]);
                    brand.Name = Convert.ToString(datarow["Name"]);
                    brand.URL = Convert.ToString(datarow["URL"]);

                    listOfBrandsP.Add(brand);
                }
            }

            List<IQ_Industry> listOfIndustriesP = new List<IQ_Industry>();
            if (dataSet != null && dataSet.Tables.Count > 2)
            {
                foreach (DataRow datarow in dataSet.Tables[2].Rows)
                {
                    IQ_Industry industry = new IQ_Industry();
                    industry.ID = Convert.ToString(datarow["ID"]);
                    industry.Name = Convert.ToString(datarow["Name"]);

                    listOfIndustriesP.Add(industry);
                }
            }

            List<IQ_Company> listOfCompaniesP = new List<IQ_Company>();
            if (dataSet != null && dataSet.Tables.Count > 3)
            {
                foreach (DataRow datarow in dataSet.Tables[3].Rows)
                {
                    IQ_Company comapny = new IQ_Company();
                    comapny.ID = Convert.ToString(datarow["ID"]);
                    comapny.Name = Convert.ToString(datarow["Name"]);

                    listOfCompaniesP.Add(comapny);
                }
            }

            Dictionary<string, object> filters = new Dictionary<string, object>();
            filters.Add("IQ_Logo", listOfLogosP);
            filters.Add("IQ_Brand", listOfBrandsP);
            filters.Add("IQ_Industry", listOfIndustriesP);
            filters.Add("IQ_Company", listOfCompaniesP);

            return filters;
        }

        public List<string> GetRawData(string iqcckey)
        {
            List<DataType> dataTypeList = new List<DataType>();
            dataTypeList.Add(new DataType("@IQ_CC_Key", DbType.String, iqcckey, ParameterDirection.Input));
            DataSet dataSet = DataAccess.GetDataSet("usp_v5_TAdsResult_GetRawData", dataTypeList);

            List<string> fileLocations = new List<string>();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow datarow in dataSet.Tables[0].Rows)
                {
                    fileLocations.Add(Convert.ToString(datarow["xmlFile"]));
                    fileLocations.Add(Convert.ToString(datarow["tgzFile"]));
                }
            }

            return fileLocations;
        }
        #endregion
    }
}
