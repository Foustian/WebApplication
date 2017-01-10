using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using IQMedia.Model;
using System.Data;
using System.Xml.Linq;

namespace IQMedia.Data
{
    public class ReportDA : IDataAccess
    {
        public string InsertFeedsReport(IQFeeds_ReportModel iQFeeds_ReportModel)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@Title", DbType.String, iQFeeds_ReportModel.Title, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Keywords", DbType.String, iQFeeds_ReportModel.Keywords, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Description", DbType.String, iQFeeds_ReportModel.Description, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryGuid", DbType.Guid, iQFeeds_ReportModel.CategoryGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MediaID", DbType.String, iQFeeds_ReportModel.MediaID.ToString(), ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, iQFeeds_ReportModel.CustomerGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, iQFeeds_ReportModel.ClientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReportImageID", DbType.Int64, iQFeeds_ReportModel.ReportImageID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@_FolderID", DbType.Int64, iQFeeds_ReportModel.FolderID, ParameterDirection.Input));
                //dataTypeList.Add(new DataType("@ReturnValue", DbType.Guid, iQFeeds_ReportModel.ClientGuid, ParameterDirection.Output));

                //string _Result = DataAccess.ExecuteNonQuery("usp_v4_IQFeeds_Report_Insert", dataTypeList);

                object _Result = DataAccess.ExecuteScalar("usp_v4_IQFeeds_Report_Insert", dataTypeList);
                return Convert.ToString(_Result);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public string InsertDiscoveryReport(IQDiscovery_ReportModel iQDiscovery_ReportModel)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@Title", DbType.String, iQDiscovery_ReportModel.Title, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Keywords", DbType.String, iQDiscovery_ReportModel.Keywords, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Description", DbType.String, iQDiscovery_ReportModel.Description, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryGuid", DbType.Guid, iQDiscovery_ReportModel.CategoryGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MediaID", DbType.String, iQDiscovery_ReportModel.MediaID.ToString(), ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, iQDiscovery_ReportModel.CustomerGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, iQDiscovery_ReportModel.ClientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@_FolderID", DbType.Int64, iQDiscovery_ReportModel.FolderID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReportImageID", DbType.Int64, iQDiscovery_ReportModel.ReportImageID, ParameterDirection.Input));

                object _Result = DataAccess.ExecuteScalar("usp_v4_IQReport_Discovery_Insert", dataTypeList);
                return Convert.ToString(_Result);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public string InsertFeedsLibrary(IQFeeds_ReportModel iQFeeds_ReportModel)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@Keywords", DbType.String, iQFeeds_ReportModel.Keywords, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Description", DbType.String, iQFeeds_ReportModel.Description, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryGuid", DbType.Guid, iQFeeds_ReportModel.CategoryGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubCategory1Guid", DbType.Guid, iQFeeds_ReportModel.SubCategory1Guid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubCategory2Guid", DbType.Guid, iQFeeds_ReportModel.SubCategory2Guid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubCategory3Guid", DbType.Guid, iQFeeds_ReportModel.SubCategory3Guid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MediaID", DbType.String, iQFeeds_ReportModel.MediaID.ToString(), ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, iQFeeds_ReportModel.CustomerGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, iQFeeds_ReportModel.ClientGuid, ParameterDirection.Input));

                object _Result = DataAccess.ExecuteScalar("usp_v4_IQReport_Feeds_InsertLibrary", dataTypeList);
                return Convert.ToString(_Result);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string InsertDiscoveryLibrary(IQDiscovery_ReportModel iQDiscovery_ReportModel)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@Keywords", DbType.String, iQDiscovery_ReportModel.Keywords, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Description", DbType.String, iQDiscovery_ReportModel.Description, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryGuid", DbType.Guid, iQDiscovery_ReportModel.CategoryGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MediaID", DbType.String, iQDiscovery_ReportModel.MediaID.ToString(), ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, iQDiscovery_ReportModel.CustomerGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, iQDiscovery_ReportModel.ClientGuid, ParameterDirection.Input));

                object _Result = DataAccess.ExecuteScalar("usp_v4_IQReport_Discovery_InsertLibrary", dataTypeList);
                return Convert.ToString(_Result);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<IQFeeds_ReportModel> SelectFeedsReport(Guid p_ClientGUID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));

                DataSet _Result = DataAccess.GetDataSet("usp_v4_IQReport_Feeds_SelectByClientGUID", dataTypeList);
                List<IQFeeds_ReportModel> lstIQFeeds_ReportModel = FillIQFeeds_ReportModelList(_Result);
                return lstIQFeeds_ReportModel;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public string SelectMediaIDByID(Guid p_ClientGUID, Int64 p_ReportID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReportID", DbType.Int64, p_ReportID, ParameterDirection.Input));

                string _Result = Convert.ToString(DataAccess.ExecuteScalar("usp_v4_IQReport_Feeds_SelectMediaIDByID", dataTypeList));

                return _Result;
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public string IQReportFeeds_Update(string p_MediaID, Int64 p_ReportID, Guid p_ClientGuid, Guid p_CustomerGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@MediaID", DbType.String, p_MediaID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReportID", DbType.Int64, p_ReportID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, p_CustomerGuid, ParameterDirection.Input));

                string _Result = DataAccess.ExecuteNonQuery("usp_v4_IQReport_Feeds_Update", dataTypeList);

                return _Result;
            }
            catch (Exception ex)
            {

                throw;
            }


        }


        private List<IQFeeds_ReportModel> FillIQFeeds_ReportModelList(DataSet p_Dataset)
        {
            List<IQFeeds_ReportModel> listIQFeeds_ReportModel = new List<IQFeeds_ReportModel>();

            if (p_Dataset != null && p_Dataset.Tables.Count > 0 && p_Dataset.Tables[0] != null)
            {
                foreach (DataRow dr in p_Dataset.Tables[0].Rows)
                {
                    IQFeeds_ReportModel iQFeeds_ReportModel = new IQFeeds_ReportModel();
                    if (!dr["ID"].Equals(DBNull.Value))
                    {
                        iQFeeds_ReportModel.ID = Convert.ToInt64(dr["ID"]);
                    }

                    if (!dr["Title"].Equals(DBNull.Value))
                    {
                        iQFeeds_ReportModel.Title = Convert.ToString(dr["Title"]);
                    }

                    if (!dr["Status"].Equals(DBNull.Value))
                    {
                        iQFeeds_ReportModel.Status = Convert.ToString(dr["Status"]);
                    }

                    listIQFeeds_ReportModel.Add(iQFeeds_ReportModel);
                }
            }

            return listIQFeeds_ReportModel;
        }

        public List<IQDiscovery_ReportModel> SelectDiscoveryReport(Guid p_ClientGUID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));

                DataSet _Result = DataAccess.GetDataSet("usp_v4_IQReport_Discovery_SelectByClientGUID", dataTypeList);
                List<IQDiscovery_ReportModel> lstIQDiscovery_ReportModel = FillIQDiscovery_ReportModelList(_Result);
                return lstIQDiscovery_ReportModel;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public string SelectDiscoveryMediaIDByID(Guid p_ClientGUID, Int64 p_ReportID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReportID", DbType.Int64, p_ReportID, ParameterDirection.Input));

                string _Result = Convert.ToString(DataAccess.ExecuteScalar("usp_v4_IQReport_Discovery_SelectMediaIDByID", dataTypeList));

                return _Result;
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public string IQReportDiscovery_Update(string p_MediaID, Int64 p_ReportID, Guid p_ClientGuid, Guid p_CustomerGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@MediaID", DbType.String, p_MediaID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReportID", DbType.Int64, p_ReportID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, p_CustomerGuid, ParameterDirection.Input));

                string _Result = DataAccess.ExecuteNonQuery("usp_v4_IQReport_Discovery_Update", dataTypeList);

                return _Result;
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        private List<IQDiscovery_ReportModel> FillIQDiscovery_ReportModelList(DataSet p_Dataset)
        {
            List<IQDiscovery_ReportModel> listIQDiscovery_ReportModel = new List<IQDiscovery_ReportModel>();

            if (p_Dataset != null && p_Dataset.Tables.Count > 0 && p_Dataset.Tables[0] != null)
            {
                foreach (DataRow dr in p_Dataset.Tables[0].Rows)
                {
                    IQDiscovery_ReportModel iQDiscovery_ReportModel = new IQDiscovery_ReportModel();

                    if (!dr["ID"].Equals(DBNull.Value))
                    {
                        iQDiscovery_ReportModel.ID = Convert.ToInt64(dr["ID"]);
                    }

                    if (!dr["Title"].Equals(DBNull.Value))
                    {
                        iQDiscovery_ReportModel.Title = Convert.ToString(dr["Title"]);
                    }

                    if (!dr["Status"].Equals(DBNull.Value))
                    {
                        iQDiscovery_ReportModel.Status = Convert.ToString(dr["Status"]);
                    }

                    listIQDiscovery_ReportModel.Add(iQDiscovery_ReportModel);
                }
            }

            return listIQDiscovery_ReportModel;
        }

        public IQReport_DetailModel IQReportCheckForLimitnData(Guid p_ClientGuid, Int64 p_ReportID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ReportID", DbType.Int64, p_ReportID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGuid, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQ_Report_CheckForLimitnData", dataTypeList);

                IQReport_DetailModel iQReport_DetailModel = FillIQReport_DetailModel(dataset);
                return iQReport_DetailModel;
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        private IQReport_DetailModel FillIQReport_DetailModel(DataSet p_Dataset)
        {
            IQReport_DetailModel iQReport_DetailModel = new IQReport_DetailModel();

            if (p_Dataset != null && p_Dataset.Tables.Count > 0 && p_Dataset.Tables[0] != null)
            {
                foreach (DataRow dr in p_Dataset.Tables[0].Rows)
                {


                    if (!dr["IsFeedsReport"].Equals(DBNull.Value))
                    {
                        iQReport_DetailModel.IsFeedsReport = Convert.ToBoolean(dr["IsFeedsReport"]);
                    }

                    if (!dr["MaxFeedsReportItems"].Equals(DBNull.Value))
                    {
                        iQReport_DetailModel.MaxFeedsReportItems = Convert.ToInt32(dr["MaxFeedsReportItems"]);
                    }

                    if (!dr["IsDiscoveryReport"].Equals(DBNull.Value))
                    {
                        iQReport_DetailModel.IsDiscoveryReport = Convert.ToBoolean(dr["IsDiscoveryReport"]);
                    }

                    if (!dr["MaxDiscoveryReportItems"].Equals(DBNull.Value))
                    {
                        iQReport_DetailModel.MaxDiscoveryReportItems = Convert.ToInt32(dr["MaxDiscoveryReportItems"]);
                    }

                    if (!dr["CurrentReportTotal"].Equals(DBNull.Value))
                    {
                        iQReport_DetailModel.CurrentReportTotal = Convert.ToInt32(dr["CurrentReportTotal"]);
                    }
                }
            }

            return iQReport_DetailModel;
        }

        public string SaveReportWithSettings(Guid p_ClientGUID, Int64 p_ReportID,string p_ReportSettingXml,Int64? p_ReportImageID,bool p_IsSaveAs, string p_ReportTile, bool p_ResetSort)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ReportID", DbType.Int64, p_ReportID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReportSettingsRule", DbType.Xml, p_ReportSettingXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReportImageID", DbType.Int64, p_ReportImageID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsSaveAs", DbType.Boolean, p_IsSaveAs, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReportTitle", DbType.String, p_ReportTile, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ResetSort", DbType.Boolean, p_ResetSort, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Output", DbType.Int64, p_ReportTile, ParameterDirection.Output));
                
                

                string result = DataAccess.ExecuteNonQuery("usp_v4_IQ_Report_SaveReportWithSettings", dataTypeList);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<IQ_ReportTypeModel> GetReportTypes(string masterReportType)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@MasterReportType", DbType.String, masterReportType, ParameterDirection.Input));
                DataSet dataSet = DataAccess.GetDataSet("usp_v4_IQ_ReportType_Select", dataTypeList);

                List<IQ_ReportTypeModel> lstReportTypes = new List<IQ_ReportTypeModel>();
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    lstReportTypes = FillReportTypes(dataSet.Tables[0]);
                }

                return lstReportTypes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQ_ReportTypeModel GetReportTypeByReportGuid(Guid reportGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ReportGuid", DbType.Guid, reportGuid, ParameterDirection.Input));
                DataSet dataSet = DataAccess.GetDataSet("usp_v4_IQ_ReportType_SelectByReportGuid", dataTypeList);

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    List<IQ_ReportTypeModel> lstReportTypes = FillReportTypes(dataSet.Tables[0]);
                    if (lstReportTypes.Count > 0)
                    {
                        return lstReportTypes[0];
                    }
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<IQ_ReportTypeModel> FillReportTypes(DataTable dt)
        {
            List<IQ_ReportTypeModel> lstReportTypes = new List<IQ_ReportTypeModel>();
            foreach (DataRow dr in dt.Rows)
            {
                IQ_ReportTypeModel reportType = new IQ_ReportTypeModel();

                if (!dr["ID"].Equals(DBNull.Value))
                {
                    reportType.ID = Convert.ToInt32(dr["ID"]);
                }
                if (!dr["Name"].Equals(DBNull.Value))
                {
                    reportType.Name = Convert.ToString(dr["Name"]);
                }
                if (!dr["Identity"].Equals(DBNull.Value))
                {
                    reportType.Identity = Convert.ToString(dr["Identity"]);
                }
                if (!dr["MasterReportType"].Equals(DBNull.Value))
                {
                    reportType.MasterReportType = Convert.ToString(dr["MasterReportType"]);
                }
                if (!dr["Description"].Equals(DBNull.Value))
                {
                    reportType.Description = Convert.ToString(dr["Description"]);
                }
                if (!dr["Settings"].Equals(DBNull.Value))
                {
                    IQMedia.Model.ReportTypeSettings settings = new IQMedia.Model.ReportTypeSettings();
                    reportType.Settings = (IQMedia.Model.ReportTypeSettings)Shared.Utility.CommonFunctions.DeserialiazeXml(Convert.ToString(dr["Settings"]), settings);
                }
                if (!dr["IsDefault"].Equals(DBNull.Value))
                {
                    reportType.IsDefault = Convert.ToBoolean(dr["IsDefault"]);
                }

                lstReportTypes.Add(reportType);
            }

            return lstReportTypes;
        }

        public int SaveReportItemPositions(long reportID, string reportItemXml)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ReportID", DbType.Int64, reportID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReportItemXml", DbType.Xml, reportItemXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReturnVal", DbType.Int16, 0, ParameterDirection.Output));

                string result = DataAccess.ExecuteNonQuery("usp_v4_IQ_Report_SaveItemPositions", dataTypeList);
                short intResult;

                if (Int16.TryParse(result, out intResult))
                {
                    return intResult;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RevertReportItemPositions(long reportID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ReportID", DbType.Int64, reportID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReturnVal", DbType.Int16, 0, ParameterDirection.Output));

                string result = DataAccess.ExecuteNonQuery("usp_v4_IQ_Report_RevertItemPositions", dataTypeList);
                short intResult;

                if (Int16.TryParse(result, out intResult))
                {
                    return intResult;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string InsertReportPDFExport(long reportID, Guid customerGuid, string baseUrl, string filename)
        {
            try
            {
                string _result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ReportID", DbType.Int64, reportID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerGuid", DbType.Guid, customerGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@BaseUrl", DbType.String, baseUrl, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@HTMLFilename", DbType.String, filename, ParameterDirection.Input));

                _result = Convert.ToString(DataAccess.ExecuteScalar("usp_v4_IQService_ReportPDFExport_Insert", _ListOfDataType));

                return _result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckReportPDFExportExists(long reportID)
        {
            try
            {
                int _result = 0;

                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ReportID", DbType.Int64, reportID, ParameterDirection.Input));

                if (!Int32.TryParse(Convert.ToString(DataAccess.ExecuteScalar("usp_v4_IQService_ReportPDFExport_CheckExists", _ListOfDataType)), out _result) || _result == 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
