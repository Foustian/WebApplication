using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using IQMedia.Model;
using System.Data;

namespace IQMedia.Data
{
    public class IQNotificationSettingsDA : IDataAccess
    {
        public List<IQNotifationSettingsModel> SelectIQNotifcationsBySearchRequestID(string ClientGuid, long SearchRequestID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGuid", DbType.String, ClientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchRequestID", DbType.Int64, SearchRequestID, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQNotificationSettings_SelectBySearchRequestIDClientGuid", dataTypeList);

                return FillIQNotification(dataset);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<IQNotifationSettingsModel> SelectIQNotifcationsByClientGuid(Guid p_Clientguid, int p_PageNumner, int p_PageSize, out int p_TotalResults)
        {
            try
            {
                p_TotalResults = 0;
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, p_Clientguid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageNumner", DbType.Int16, p_PageNumner, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageSize", DbType.Int16, p_PageSize, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@TotalResults", DbType.Int64, p_TotalResults, ParameterDirection.Output));

                Dictionary<string, string> _Output;
                DataSet dataset = DataAccess.GetDataSetWithOutParam("usp_v4_IQNotificationSettings_SelectByClientGuid", dataTypeList, out _Output);

                if (_Output != null && _Output.Count > 0)
                {
                    p_TotalResults = !string.IsNullOrWhiteSpace(_Output["@TotalResults"]) ? Convert.ToInt32(_Output["@TotalResults"]) : 0;
                }

                List<IQNotifationSettingsModel> lstIQNotifationSettings = new List<IQNotifationSettingsModel>();

                return FillIQNotification(dataset);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IQNotifationSettingsModel SelectIQNotifcationsByID(Int64 p_ID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQNotificationSettings_SelectByID", dataTypeList);

                List<IQNotifationSettingsModel> lstIQNotifationSettings = new List<IQNotifationSettingsModel>();

                lstIQNotifationSettings = FillIQNotification(dataset);

                if (lstIQNotifationSettings != null && lstIQNotifationSettings.Count > 0)
                {
                    return lstIQNotifationSettings[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IQNotifationSettings_DropDown GetIQNotificationDropDown(Guid p_Clientguid)
        {
            try
            {
                IQNotifationSettings_DropDown objIQAgent_DailyDigest_DropDown = new IQNotifationSettings_DropDown();
                objIQAgent_DailyDigest_DropDown.ReportImageList = new List<IQClient_CustomImageModel>();
                objIQAgent_DailyDigest_DropDown.SearchRequestList = new List<IQAgent_SearchRequestModel>();

                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, p_Clientguid, ParameterDirection.Input));
                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQNotification_SelectAllDropDown", dataTypeList);

                if (dataset != null && dataset.Tables.Count > 0)
                {
                    List<IQAgent_SearchRequestModel> lstIQAgent_SearchRequestModel = new List<IQAgent_SearchRequestModel>();
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
                        lstIQAgent_SearchRequestModel.Add(objIQAgent_SearchRequestModel);
                    }
                    objIQAgent_DailyDigest_DropDown.SearchRequestList = lstIQAgent_SearchRequestModel;
                }

                if (dataset != null && dataset.Tables.Count > 1)
                {
                    List<IQClient_CustomImageModel> lstIQClient_CustomImageModel = new List<IQClient_CustomImageModel>();
                    foreach (DataRow dr in dataset.Tables[1].Rows)
                    {
                        IQClient_CustomImageModel objIQClient_CustomImageModel = new IQClient_CustomImageModel();
                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objIQClient_CustomImageModel.ID = Convert.ToInt64(dr["ID"]);
                        }
                        if (!dr["Location"].Equals(DBNull.Value))
                        {
                            objIQClient_CustomImageModel.Location = Convert.ToString(dr["Location"]);
                        }
                        lstIQClient_CustomImageModel.Add(objIQClient_CustomImageModel);
                    }
                    objIQAgent_DailyDigest_DropDown.ReportImageList = lstIQClient_CustomImageModel;
                }

                return objIQAgent_DailyDigest_DropDown;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string InsertIQNotificationSettings(IQNotifationSettingsModel p_IQNotifationSettingsModel, string p_MediaTypes, string p_SearchRequests, string p_EmailAddress, Guid p_ClientGuid)
        {
            try
            {

                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@SearchRequestList", DbType.Xml, p_SearchRequests, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Notification_Address", DbType.String, p_EmailAddress, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Frequency", DbType.String, p_IQNotifationSettingsModel.Frequency, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MediumType", DbType.Xml, p_MediaTypes, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@DayOfWeek", DbType.Int16, p_IQNotifationSettingsModel.Notification_Day, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Time", DbType.String, p_IQNotifationSettingsModel.Notification_Time, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@_ReportImageID", DbType.Int64, p_IQNotifationSettingsModel.ReportImageID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@UseRollup", DbType.Boolean, p_IQNotifationSettingsModel.UseRollup, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Output", DbType.Int32, 0, ParameterDirection.Output));

                string result = DataAccess.ExecuteNonQuery("usp_v4_IQNotificationSettings_Insert", dataTypeList);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string UdateIQNotificationSettings(IQNotifationSettingsModel p_IQNotifationSettingsModel, string p_MediaTypes, string p_SearchRequests, string p_EmailAddress, Guid p_ClientGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchRequestList", DbType.Xml, p_SearchRequests, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IQNotificationKey", DbType.Int64, p_IQNotifationSettingsModel.IQNotificationKey, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Notification_Address", DbType.String, p_EmailAddress, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Frequency", DbType.String, p_IQNotifationSettingsModel.Frequency, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MediumType", DbType.Xml, p_MediaTypes, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@DayOfWeek", DbType.Int16, p_IQNotifationSettingsModel.Notification_Day, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Time", DbType.String, p_IQNotifationSettingsModel.Notification_Time, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@_ReportImageID", DbType.Int64, p_IQNotifationSettingsModel.ReportImageID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@UseRollup", DbType.Boolean, p_IQNotifationSettingsModel.UseRollup, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Output", DbType.Int32, 0, ParameterDirection.Output));

                string result = DataAccess.ExecuteNonQuery("usp_v4_IQNotificationSettings_Update", dataTypeList);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int DeleteIQNotification(int p_IQAgentNotificationKey, Guid p_ClientGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@IQNotificationKey", DbType.Int64, p_IQAgentNotificationKey, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGuid, ParameterDirection.Input));

                Dictionary<string, string> _output = new Dictionary<string, string>();
                string result = DataAccess.ExecuteNonQuery("usp_v4_IQNotificationSettings_Delete", dataTypeList);

                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<IQNotifationSettingsModel> FillIQNotification(DataSet dataset)
        {
            List<IQNotifationSettingsModel> lstIQNotifationSettings = new List<IQNotifationSettingsModel>();
            if (dataset != null && dataset.Tables.Count > 0)
            {
                foreach (DataRow dr in dataset.Tables[0].Rows)
                {
                    IQNotifationSettingsModel objIQNotifationSettingsModel = new IQNotifationSettingsModel();

                    if (!dr["IQNotificationKey"].Equals(DBNull.Value))
                    {
                        objIQNotifationSettingsModel.IQNotificationKey = Convert.ToInt64(dr["IQNotificationKey"]);
                    }
                    if (!dr["Frequency"].Equals(DBNull.Value) && !string.IsNullOrWhiteSpace(Convert.ToString(dr["Frequency"])))
                    {
                        objIQNotifationSettingsModel.Frequency = (IQMedia.Shared.Utility.CommonFunctions.IQNotificationFrequency)Enum.Parse(typeof(IQMedia.Shared.Utility.CommonFunctions.IQNotificationFrequency), Convert.ToString(dr["Frequency"]));
                    }
                    if (!dr["Notification_Address"].Equals(DBNull.Value))
                    {
                        EmailAddressList emailAddressList = new EmailAddressList();
                        emailAddressList = (EmailAddressList)Shared.Utility.CommonFunctions.DeserialiazeXml(Convert.ToString(dr["Notification_Address"]), emailAddressList);
                        objIQNotifationSettingsModel.Notification_Address = emailAddressList.EmailAddress;
                    }

                    if (dataset.Tables[0].Columns.Contains("Time") && !dr["Time"].Equals(DBNull.Value))
                    {
                        objIQNotifationSettingsModel.Notification_Time = Convert.ToString(dr["Time"]);
                    }

                    if (dataset.Tables[0].Columns.Contains("DayOfWeek") && !dr["DayOfWeek"].Equals(DBNull.Value))
                    {
                        objIQNotifationSettingsModel.Notification_Day = Convert.ToInt16(dr["DayOfWeek"]);
                    }

                    if (dataset.Tables[0].Columns.Contains("MediaType") && !dr["MediaType"].Equals(DBNull.Value) && !string.IsNullOrWhiteSpace(Convert.ToString(dr["MediaType"])))
                    {
                        MediaTypeList lstOfMediaTypes = new MediaTypeList();
                        lstOfMediaTypes = (MediaTypeList)Shared.Utility.CommonFunctions.DeserialiazeXml(Convert.ToString(dr["MediaType"]), lstOfMediaTypes);
                        objIQNotifationSettingsModel.MediaTypeList = lstOfMediaTypes.MediaType;                        
                    }

                    if (dataset.Tables[0].Columns.Contains("IQAgentNames") && !dr["IQAgentNames"].Equals(DBNull.Value) && !string.IsNullOrWhiteSpace(Convert.ToString(dr["IQAgentNames"])))
                    {
                        objIQNotifationSettingsModel.SearchRequestNames = Convert.ToString(dr["IQAgentNames"]);
                    }

                    if (dataset.Tables[0].Columns.Contains("_ReportImageID") && !dr["_ReportImageID"].Equals(DBNull.Value))
                    {
                        objIQNotifationSettingsModel.ReportImageID = Convert.ToInt64(dr["_ReportImageID"]);
                    }

                    if (dataset.Tables[0].Columns.Contains("SearchRequestList") && !dr["SearchRequestList"].Equals(DBNull.Value) && !string.IsNullOrWhiteSpace(Convert.ToString(dr["SearchRequestList"])))
                    {
                        SearchRequestIDList searchRequestList = new SearchRequestIDList();
                        searchRequestList = (SearchRequestIDList)Shared.Utility.CommonFunctions.DeserialiazeXml(Convert.ToString(dr["SearchRequestList"]), searchRequestList);
                        objIQNotifationSettingsModel.SearchRequestList = searchRequestList.SearchRequestID;

                    }

                    if (dataset.Tables[0].Columns.Contains("UseRollup") && !dr["UseRollup"].Equals(DBNull.Value))
                    {
                        objIQNotifationSettingsModel.UseRollup = Convert.ToBoolean(dr["UseRollup"]);
                    }

                    lstIQNotifationSettings.Add(objIQNotifationSettingsModel);
                }
            }
            return lstIQNotifationSettings;
        }
    }
}
