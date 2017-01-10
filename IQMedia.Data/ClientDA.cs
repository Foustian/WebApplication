using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using IQMedia.Model;
using System.Data;
using System.Globalization;
using IQMedia.Shared.Utility;
using System.Xml.Linq;

namespace IQMedia.Data
{
    public class ClientDA : IDataAccess
    {
        public string InsertClient(ClientModel p_Client, string p_Roles, out int Status, string p_RootFolder)
        {
            try
            {
                Status = 0;
                string _Result = string.Empty;

                string license = string.Join<Int16>(",", p_Client.IQLicense);

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientName", DbType.String, p_Client.ClientName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_Client.ClientGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DefaultCategory", DbType.String, p_Client.DefaultCategory, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PricingCodeID", DbType.Int64, p_Client.PricingCodeID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@BillFrequencyID", DbType.Int64, p_Client.@BillFrequencyID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@BillTypeID", DbType.Int64, p_Client.BillTypeID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IndustryID", DbType.Int64, p_Client.IndustryID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@StateID", DbType.Int64, p_Client.StateID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Address1", DbType.String, p_Client.Address1, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Address2", DbType.String, p_Client.Address2, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@City", DbType.String, p_Client.City, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Zip", DbType.String, p_Client.Zip, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Attention", DbType.String, p_Client.Attention, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Phone", DbType.String, p_Client.Phone, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@MasterClient", DbType.String, p_Client.MasterClient, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@MCID", DbType.Int64, p_Client.MCID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@NoOfUser", DbType.Int32, p_Client.NoOfUser, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomHeader", DbType.String, p_Client.CustomHeaderImage, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PlayerLogo", DbType.String, p_Client.PlayerLogo, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActivePlayerLogo", DbType.Boolean, p_Client.IsActivePlayerLogo, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@NoOfIQNotification", DbType.Int16, p_Client.NoOfIQNotification, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@NoOfIQAgnet", DbType.Int16, p_Client.NoOfIQAgent, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CompeteMultiplier", DbType.Decimal, p_Client.CompeteMultiplier, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@OnlineNewsAdRate", DbType.Decimal, p_Client.OnlineNewsAdRate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@OtherOnlineAdRate", DbType.Decimal, p_Client.OtherOnlineAdRate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@UrlPercentRead", DbType.Decimal, p_Client.URLPercentRead, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientKey", DbType.Int32, p_Client.ClientKey, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@ClientRoles", DbType.Xml, p_Roles, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQLicense", DbType.String, license, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Status", DbType.Int32, p_Client.ClientKey, ParameterDirection.Output));

                _ListOfDataType.Add(new DataType("@IsCDNUpload", DbType.Boolean, p_Client.IsCDNUpload, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@NotificationHeaderImage", DbType.String, p_Client.NotificationHeaderImage, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@TimeZone", DbType.String, p_Client.TimeZone, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@MultiPlier", DbType.Decimal, p_Client.Multiplier, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CompeteAudienceMultiplier", DbType.Decimal, p_Client.CompeteAudienceMultiplier, ParameterDirection.Input));
                //check if all industries are selected. If so insert specified string
                string visibleIndustriesXML = "";
                if (p_Client.visibleLRIndustries.Industries.Any(industry => industry.ID == "0")) { visibleIndustriesXML = "<VisibleLRIndustries IsAllowAll='true'></VisibleLRIndustries>"; }
                else { visibleIndustriesXML = CommonFunctions.SerializeToXml(p_Client.visibleLRIndustries); }

                _ListOfDataType.Add(new DataType("@visibleLRIndustries", DbType.String, visibleIndustriesXML, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@v4MaxDiscoveryReportItems", DbType.Int32, p_Client.v4MaxDiscoveryReportItems, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@v4MaxDiscoveryExportItems", DbType.Int32, p_Client.v4MaxDiscoveryExportItems, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@v4MaxDiscoveryHistory", DbType.Int32, p_Client.v4MaxDiscoveryHistory, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@v4MaxFeedsExportItems", DbType.Int32, p_Client.v4MaxFeedsExportItems, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@v4MaxFeedsReportItems", DbType.Int32, p_Client.v4MaxFeedsReportItems, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@v4MaxLibraryEmailReportItems", DbType.Int32, p_Client.v4MaxLibraryEmailReportItems, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@v4MaxLibraryReportItems", DbType.Int32, p_Client.v4MaxLibraryReportItems, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@TVHighThreshold", DbType.Decimal, p_Client.TVHighThreshold, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@TVLowThreshold", DbType.Decimal, p_Client.TVLowThreshold, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@NMHighThreshold", DbType.Decimal, p_Client.NMHighThreshold, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@NMLowThreshold", DbType.Decimal, p_Client.NMLowThreshold, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SMHighThreshold", DbType.Decimal, p_Client.SMHighThreshold, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SMLowThreshold", DbType.Decimal, p_Client.SMLowThreshold, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@TwitterHighThreshold", DbType.Decimal, p_Client.TwitterHighThreshold, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@TwitterLowThreshold", DbType.Decimal, p_Client.TwitterLowThreshold, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PQHighThreshold", DbType.Decimal, p_Client.PQHighThreshold, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PQLowThreshold", DbType.Decimal, p_Client.PQLowThreshold, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActive", DbType.Boolean, p_Client.IsActive, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RootFolder", DbType.String, p_RootFolder, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsFliq", DbType.Boolean, p_Client.IsFliq, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@UseProminence", DbType.Boolean, p_Client.UseProminence, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@UseProminenceMediaValue", DbType.Boolean, p_Client.UseProminenceMediaValue, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ForceCategorySelection", DbType.Boolean, p_Client.ForceCategorySelection, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@MCMediaPublishedTemplateID", DbType.Int32, p_Client.MCMediaPublishedTemplateID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@MCMediaDefaultEmailTemplateID", DbType.Int32, p_Client.MCMediaDefaultEmailTemplateID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQRawMediaExpiration", DbType.Int32, p_Client.IQRawMediaExpiration, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@LibraryTextType", DbType.String, p_Client.LibraryTextType, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DefaultFeedsPageSize", DbType.Int32, p_Client.DefaultFeedsPageSize, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DefaultDiscoveryPageSize", DbType.Int32, p_Client.DefaultDiscoveryPageSize, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DefaultArchivePageSize", DbType.Int32, p_Client.DefaultArchivePageSize, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClipEmbedAutoPlay", DbType.Boolean, p_Client.ClipEmbedAutoPlay, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DefaultFeedsShowUnread", DbType.Boolean, p_Client.DefaultFeedsShowUnread, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@UseCustomerEmailDefault", DbType.Boolean, p_Client.UseCustomerEmailDefault, ParameterDirection.Input));

                Dictionary<string, string> _outputParams;

                _Result = DataAccess.ExecuteNonQuery("usp_v4_Client_Insert", _ListOfDataType, out _outputParams);

                if (_outputParams != null && _outputParams.Count > 0)
                {
                    Status = Convert.ToInt32(_outputParams["@Status"]);
                    _Result = _outputParams["@ClientKey"].ToString();
                }

                return _Result;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string UpdateClient(ClientModel p_Client, string p_Roles, out int Status, out int p_NotificationStatus, out int p_IQAgentStatus)
        {
            try
            {
                Status = 0;
                p_NotificationStatus = 0;
                p_IQAgentStatus = 0;
                string _Result = string.Empty;

                string license = string.Join<Int16>(",", p_Client.IQLicense);

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientName", DbType.String, p_Client.ClientName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Active", DbType.Boolean, p_Client.IsActive, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PricingCodeID", DbType.Int64, p_Client.PricingCodeID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@BillFrequencyID", DbType.Int64, p_Client.@BillFrequencyID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@BillTypeID", DbType.Int64, p_Client.BillTypeID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IndustryID", DbType.Int64, p_Client.IndustryID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@StateID", DbType.Int64, p_Client.StateID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Address1", DbType.String, p_Client.Address1, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Address2", DbType.String, p_Client.Address2, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@City", DbType.String, p_Client.City, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Zip", DbType.String, p_Client.Zip, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Attention", DbType.String, p_Client.Attention, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Phone", DbType.String, p_Client.Phone, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@MasterClient", DbType.String, p_Client.MasterClient, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@MCID", DbType.Int64, p_Client.MCID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@NoOfUser", DbType.Int32, p_Client.NoOfUser, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientKey", DbType.Int64, p_Client.ClientKey, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ModifiedDate", DbType.DateTime, DateTime.Now, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PlayerLogo", DbType.String, p_Client.PlayerLogo, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActivePlayerLogo", DbType.Boolean, p_Client.IsActivePlayerLogo, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@NoOfIQNotification", DbType.Int16, p_Client.NoOfIQNotification, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@NoOfIQAgnet", DbType.Int16, p_Client.NoOfIQAgent, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CompeteMultiplier", DbType.Decimal, p_Client.CompeteMultiplier, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@OnlineNewsAdRate", DbType.Decimal, p_Client.OnlineNewsAdRate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@OtherOnlineAdRate", DbType.Decimal, p_Client.OtherOnlineAdRate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@UrlPercentRead", DbType.Decimal, p_Client.URLPercentRead, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientRoles", DbType.Xml, p_Roles, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQLicense", DbType.String, license, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Status", DbType.Boolean, Status, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@NotificationStatus", DbType.Int32, 0, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@IQAgentStatus", DbType.Int32, 0, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@IsClientExist", DbType.Int32, 0, ParameterDirection.Output));

                _ListOfDataType.Add(new DataType("@IsCDNUpload", DbType.Boolean, p_Client.IsCDNUpload, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@TimeZone", DbType.String, p_Client.TimeZone, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@MultiPlier", DbType.Decimal, p_Client.Multiplier, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CompeteAudienceMultiplier", DbType.Decimal, p_Client.CompeteAudienceMultiplier, ParameterDirection.Input));

                //check if all industries are selected. If so insert specified string
                string visibleIndustriesXML = "";
                if (p_Client.visibleLRIndustries.Industries.Any(industry => industry.ID == "0")) { visibleIndustriesXML = "<VisibleLRIndustries IsAllowAll='true'></VisibleLRIndustries>"; }
                else { visibleIndustriesXML = CommonFunctions.SerializeToXml(p_Client.visibleLRIndustries); }

                _ListOfDataType.Add(new DataType("@visibleLRIndustries", DbType.String, visibleIndustriesXML, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@v4MaxDiscoveryReportItems", DbType.Int32, p_Client.v4MaxDiscoveryReportItems, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@v4MaxDiscoveryExportItems", DbType.Int32, p_Client.v4MaxDiscoveryExportItems, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@v4MaxDiscoveryHistory", DbType.Int32, p_Client.v4MaxDiscoveryHistory, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@v4MaxFeedsExportItems", DbType.Int32, p_Client.v4MaxFeedsExportItems, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@v4MaxFeedsReportItems", DbType.Int32, p_Client.v4MaxFeedsReportItems, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@v4MaxLibraryEmailReportItems", DbType.Int32, p_Client.v4MaxLibraryEmailReportItems, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@v4MaxLibraryReportItems", DbType.Int32, p_Client.v4MaxLibraryReportItems, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@TVHighThreshold", DbType.Decimal, p_Client.TVHighThreshold, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@TVLowThreshold", DbType.Decimal, p_Client.TVLowThreshold, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@NMHighThreshold", DbType.Decimal, p_Client.NMHighThreshold, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@NMLowThreshold", DbType.Decimal, p_Client.NMLowThreshold, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SMHighThreshold", DbType.Decimal, p_Client.SMHighThreshold, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SMLowThreshold", DbType.Decimal, p_Client.SMLowThreshold, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@TwitterHighThreshold", DbType.Decimal, p_Client.TwitterHighThreshold, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@TwitterLowThreshold", DbType.Decimal, p_Client.TwitterLowThreshold, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PQHighThreshold", DbType.Decimal, p_Client.PQHighThreshold, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PQLowThreshold", DbType.Decimal, p_Client.PQLowThreshold, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActive", DbType.Boolean, p_Client.IsActive, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsFliq", DbType.Boolean, p_Client.IsFliq, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@UseProminence", DbType.Boolean, p_Client.UseProminence, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@UseProminenceMediaValue", DbType.Boolean, p_Client.UseProminenceMediaValue, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ForceCategorySelection", DbType.Boolean, p_Client.ForceCategorySelection, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@MCMediaPublishedTemplateID", DbType.Int32, p_Client.MCMediaPublishedTemplateID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@MCMediaDefaultEmailTemplateID", DbType.Int32, p_Client.MCMediaDefaultEmailTemplateID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQRawMediaExpiration", DbType.Int32, p_Client.IQRawMediaExpiration, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@LibraryTextType", DbType.String, p_Client.LibraryTextType, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DefaultFeedsPageSize", DbType.Int32, p_Client.DefaultFeedsPageSize, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DefaultDiscoveryPageSize", DbType.Int32, p_Client.DefaultDiscoveryPageSize, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DefaultArchivePageSize", DbType.Int32, p_Client.DefaultArchivePageSize, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClipEmbedAutoPlay", DbType.Boolean, p_Client.ClipEmbedAutoPlay, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DefaultFeedsShowUnread", DbType.Boolean, p_Client.DefaultFeedsShowUnread, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@UseCustomerEmailDefault", DbType.Boolean, p_Client.UseCustomerEmailDefault, ParameterDirection.Input));

                Dictionary<string, string> _outputParams;
                _Result = DataAccess.ExecuteNonQuery("usp_v4_Client_Update", _ListOfDataType, out _outputParams);
                if (Convert.ToInt32(_outputParams["@IsClientExist"].ToString()) == 1)
                {
                    _Result = "-1";
                }
                else
                {
                    _Result = "1";
                }

                if (_outputParams != null && _outputParams.Count > 0)
                {
                    Status = Convert.ToInt32(Convert.ToBoolean(_outputParams["@Status"].ToString()));
                    p_NotificationStatus = Convert.ToInt32(_outputParams["@NotificationStatus"].ToString());
                    p_IQAgentStatus = Convert.ToInt32(_outputParams["@IQAgentStatus"].ToString());
                }

                return _Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DeleteClient(Int64 p_ClientKey)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientKey", DbType.Int64, p_ClientKey, ParameterDirection.Input));

                _Result = DataAccess.ExecuteNonQuery("usp_v4_Client_Delete", _ListOfDataType);

                return _Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ClientModel GetClientInfoByClientGuid(string ClientGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGuid", DbType.String, ClientGuid, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_Client_SelectByClientGUID", dataTypeList);

                ClientModel obClientModel = new ClientModel();

                if (dataset != null && dataset.Tables.Count > 0)
                {
                    foreach (DataRow dr in dataset.Tables[0].Rows)
                    {
                        if (!dr["ClientKey"].Equals(DBNull.Value))
                        {
                            obClientModel.ClientKey = Convert.ToInt64(dr["ClientKey"]);
                        }
                        if (!dr["ClientName"].Equals(DBNull.Value))
                        {
                            obClientModel.ClientName = Convert.ToString(dr["ClientName"]);
                        }
                        if (!dr["UGCFtpUploadLocation"].Equals(DBNull.Value))
                        {
                            obClientModel.UGCFtpUploadLocation = Convert.ToString(dr["UGCFtpUploadLocation"]);
                        }
                        if (!dr["StateID"].Equals(DBNull.Value))
                        {
                            obClientModel.StateID = Convert.ToInt32(dr["StateID"]);
                        }
                        if (!dr["Address1"].Equals(DBNull.Value))
                        {
                            obClientModel.Address1 = Convert.ToString(dr["Address1"]);
                        }
                        if (!dr["Address2"].Equals(DBNull.Value))
                        {
                            obClientModel.Address2 = Convert.ToString(dr["Address2"]);
                        }
                        if (!dr["City"].Equals(DBNull.Value))
                        {
                            obClientModel.City = Convert.ToString(dr["City"]);
                        }
                        if (!dr["Zip"].Equals(DBNull.Value))
                        {
                            obClientModel.Zip = Convert.ToString(dr["Zip"]);
                        }
                        if (!dr["Phone"].Equals(DBNull.Value))
                        {
                            obClientModel.Phone = Convert.ToString(dr["Phone"]);
                        }
                    }
                }

                return obClientModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQClient_CustomSettingsModel GetClientCustomSettings(string ClientGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGUID", DbType.String, ClientGuid, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQClient_CustomSettings_SelectByClientGUID", dataTypeList);

                IQClient_CustomSettingsModel objClientSettings = new IQClient_CustomSettingsModel();

                if (dataset != null && dataset.Tables.Count > 0)
                {
                    foreach (DataRow dr in dataset.Tables[0].Rows)
                    {
                        if (!dr["MaxLibraryReportItems"].Equals(DBNull.Value))
                        {
                            objClientSettings.v4MaxLibraryReportItems = Convert.ToInt32(dr["MaxLibraryReportItems"]);
                        }
                        if (!dr["MaxLibraryEmailReportItems"].Equals(DBNull.Value))
                        {
                            objClientSettings.v4MaxLibraryEmailReportItems = Convert.ToInt32(dr["MaxLibraryEmailReportItems"]);
                        }
                        if (!dr["v4LibraryRollup"].Equals(DBNull.Value))
                        {
                            objClientSettings.v4LibraryRollup = Convert.ToBoolean(dr["v4LibraryRollup"]);
                        }
                        if (!dr["MCMediaPublishedTemplateID"].Equals(DBNull.Value))
                        {
                            objClientSettings.MCMediaPublishedTemplateID = Convert.ToInt32(dr["MCMediaPublishedTemplateID"]);
                        }
                        if (!dr["MCMediaDefaultEmailTemplateID"].Equals(DBNull.Value))
                        {
                            objClientSettings.MCMediaDefaultEmailTemplateID = Convert.ToInt32(dr["MCMediaDefaultEmailTemplateID"]);
                        }
                        if (!dr["LibraryTextType"].Equals(DBNull.Value))
                        {
                            objClientSettings.LibraryTextType = CommonFunctions.StringToEnum<CommonFunctions.LibraryTextTypes>(Convert.ToString(dr["LibraryTextType"]));
                        }
                        if (!dr["DefaultArchivePageSize"].Equals(DBNull.Value))
                        {
                            objClientSettings.DefaultArchivePageSize = Convert.ToInt32(dr["DefaultArchivePageSize"]);
                        }
                        if (!dr["DefaultFeedsPageSize"].Equals(DBNull.Value))
                        {
                            objClientSettings.DefaultFeedsPageSize = Convert.ToInt32(dr["DefaultFeedsPageSize"]);
                        }
                        if (!dr["DefaultDiscoveryPageSize"].Equals(DBNull.Value))
                        {
                            objClientSettings.DefaultDiscoveryPageSize = Convert.ToInt32(dr["DefaultDiscoveryPageSize"]);
                        }
                        if (!dr["UseProminence"].Equals(DBNull.Value))
                        {
                            objClientSettings.UseProminence = Convert.ToBoolean(dr["UseProminence"]);
                        }
                        if (!dr["UseProminenceMediaValue"].Equals(DBNull.Value))
                        {
                            objClientSettings.UseProminenceMediaValue = Convert.ToBoolean(dr["UseProminenceMediaValue"]);
                        }
                        if (!dr["ClipEmbedAutoPlay"].Equals(DBNull.Value))
                        {
                            objClientSettings.ClipEmbedAutoPlay = Convert.ToBoolean(dr["ClipEmbedAutoPlay"]);
                        }
                        if (!dr["IQTVCountry"].Equals(DBNull.Value))
                        {
                            objClientSettings.IQTVCountry = Convert.ToString(dr["IQTVCountry"]);
                        }
                        if (!dr["IQTVRegion"].Equals(DBNull.Value))
                        {
                            objClientSettings.IQTVRegion = Convert.ToString(dr["IQTVRegion"]);
                        }
                        if (!dr["DefaultFeedsShowUnread"].Equals(DBNull.Value))
                        {
                            objClientSettings.DefaultFeedsShowUnread = Convert.ToBoolean(dr["DefaultFeedsShowUnread"]);
                        }
                        if (!dr["UseCustomerEmailDefault"].Equals(DBNull.Value))
                        {
                            objClientSettings.UseCustomerEmailDefault = Convert.ToBoolean(dr["UseCustomerEmailDefault"]);
                        }
                    }
                    if (dataset.Tables.Count > 1)
                    {
                        objClientSettings.visibleLRIndustries = new List<IQ_Industry>();
                        objClientSettings.visibleLRBrands = new List<string>();
                        foreach (DataRow dr in dataset.Tables[1].Rows)
                        {
                            IQ_Industry industry = new IQ_Industry();
                            industry.ID = Convert.ToString(dr["ID"]);
                            objClientSettings.visibleLRIndustries.Add(industry);
                        }
                        foreach (DataRow dr in dataset.Tables[2].Rows)
                        {
                            objClientSettings.visibleLRBrands.Add(Convert.ToString(dr["ID"]));
                        }
                    }
                }

                return objClientSettings;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Int16> GetClientLicenseSettings(Guid ClientGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, ClientGuid, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQClient_CustomSettings_SelectIQLicenseSettingsByClientGUID", dataTypeList);

                List<Int16> lstClientLicense = new List<Int16>();

                if (dataset != null && dataset.Tables[0].Rows.Count > 0)
                {
                    if (!dataset.Tables[0].Rows[0]["IQLicense"].Equals(DBNull.Value))
                    {
                        lstClientLicense = dataset.Tables[0].Rows[0]["IQLicense"].ToString().Split(',').Select(Int16.Parse).ToList();
                    }
                    else
                    {
                        lstClientLicense.Add(0);
                    }
                }

                return lstClientLicense;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int16 GetClientRawMediaPauseSecs(Guid ClientGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, ClientGuid, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQClient_CustomSettings_SelectRawMediaPauseSecsByClientGUID", dataTypeList);

                Int16 _RawMediPlaySeconds = 0;

                if (dataset != null && dataset.Tables[0].Rows.Count > 0)
                {
                    if (!dataset.Tables[0].Rows[0]["RawMediaPauseSecs"].Equals(DBNull.Value))
                    {
                        _RawMediPlaySeconds = Convert.ToInt16(dataset.Tables[0].Rows[0]["RawMediaPauseSecs"]);
                    }
                }

                return _RawMediPlaySeconds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<object> GetClientTVRegionSettings(Guid ClientGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, ClientGuid, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQClient_CustomSettings_SelectTVRegionNCountryByClientGUID", dataTypeList);

                List<object> objRegionCountryList = new List<object>();
                List<int> IIQTVRegion = new List<int>();
                List<int> IQTVCountry = new List<int>();

                if (dataset != null && dataset.Tables[0].Rows.Count > 0)
                {
                    if (!dataset.Tables[0].Rows[0]["IQTVRegion"].Equals(DBNull.Value))
                    {
                        IIQTVRegion = dataset.Tables[0].Rows[0]["IQTVRegion"].ToString().Split(',').ToList().ConvertAll(r => Int32.Parse(r));
                    }
                }

                if (dataset != null && dataset.Tables[1].Rows.Count > 0)
                {
                    if (!dataset.Tables[1].Rows[0]["IQTVCountry"].Equals(DBNull.Value))
                    {
                        IQTVCountry = dataset.Tables[1].Rows[0]["IQTVCountry"].ToString().Split(',').ToList().ConvertAll(c => Int32.Parse(c));
                    }
                }
                objRegionCountryList.Add(IIQTVRegion);
                objRegionCountryList.Add(IQTVCountry);
                return objRegionCountryList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Int16> GetClientSettingsIQLicenseByCustomerID(Int64 p_CustomerID, out bool p_IsDefaultSettings)
        {
            try
            {
                p_IsDefaultSettings = false;

                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@CustomerID", DbType.Int64, p_CustomerID, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQClient_CustomSettings_SelectIQLicenseByCustomerID", dataTypeList);

                List<Int16> iqlicesneList = new List<Int16>();

                if (dataset != null && dataset.Tables[0].Rows.Count > 0)
                {
                    iqlicesneList = dataset.Tables[0].Rows[0]["IQLicense"].ToString().Split(',').Select(Int16.Parse).ToList();
                    p_IsDefaultSettings = Convert.ToBoolean(dataset.Tables[0].Rows[0]["IsDefaultSettings"]);
                }

                return iqlicesneList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetClientRoleByClientGUIDRoleName(Guid ClientGUID, string RoleName)
        {
            try
            {
                int IsAccess = 0;
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@RoleName", DbType.String, RoleName, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_ClientRole_SelectRoleByClientGUIDRoleName", dataTypeList);

                if (dataset != null && dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
                {
                    IsAccess = Convert.ToInt32(dataset.Tables[0].Rows[0]["IsAccess"]);
                }

                return IsAccess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQClient_ThresholdValueModel GetClientThresholdValue(Guid ClientGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, ClientGuid, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQClient_CustomSettings_SelectSentimentSettingsByClientGuid", dataTypeList);

                IQClient_ThresholdValueModel iQClient_ThresholdValueModel = new IQClient_ThresholdValueModel();

                if (dataset != null && dataset.Tables.Count > 0)
                {
                    foreach (DataRow dr in dataset.Tables[0].Rows)
                    {
                        if (dataset.Tables[0].Columns.Contains("TVLowThreshold") && !dr["TVLowThreshold"].Equals(DBNull.Value))
                        {
                            iQClient_ThresholdValueModel.TVLowThreshold = float.Parse(Convert.ToString(dr["TVLowThreshold"]), CultureInfo.InvariantCulture.NumberFormat);
                            //iQClient_ThresholdValueModel.TVLowThreshold = (float)(dr["TVLowThreshold"]);
                        }

                        if (dataset.Tables[0].Columns.Contains("TVHighThreshold") && !dr["TVHighThreshold"].Equals(DBNull.Value))
                        {
                            iQClient_ThresholdValueModel.TVHighThreshold = float.Parse(Convert.ToString(dr["TVHighThreshold"]), CultureInfo.InvariantCulture.NumberFormat);

                        }

                        if (dataset.Tables[0].Columns.Contains("NMLowThreshold") && !dr["NMLowThreshold"].Equals(DBNull.Value))
                        {
                            iQClient_ThresholdValueModel.NMLowThreshold = float.Parse(Convert.ToString(dr["NMLowThreshold"]), CultureInfo.InvariantCulture.NumberFormat);
                        }

                        if (dataset.Tables[0].Columns.Contains("NMHighThreshold") && !dr["NMHighThreshold"].Equals(DBNull.Value))
                        {
                            iQClient_ThresholdValueModel.NMHighThreshold = float.Parse(Convert.ToString(dr["NMHighThreshold"]), CultureInfo.InvariantCulture.NumberFormat);
                            //(float)(dr["NMHighThreshold"]);
                        }

                        if (dataset.Tables[0].Columns.Contains("SMLowThreshold") && !dr["SMLowThreshold"].Equals(DBNull.Value))
                        {
                            iQClient_ThresholdValueModel.SMLowThreshold = float.Parse(Convert.ToString(dr["SMLowThreshold"]), CultureInfo.InvariantCulture.NumberFormat);
                            //(float)(dr["SMLowThreshold"]);
                        }

                        if (dataset.Tables[0].Columns.Contains("SMHighThreshold") && !dr["SMHighThreshold"].Equals(DBNull.Value))
                        {
                            iQClient_ThresholdValueModel.SMHighThreshold = float.Parse(Convert.ToString(dr["SMHighThreshold"]), CultureInfo.InvariantCulture.NumberFormat);
                            //(float)(dr["SMHighThreshold"]);
                        }

                        if (dataset.Tables[0].Columns.Contains("TwitterLowThreshold") && !dr["TwitterLowThreshold"].Equals(DBNull.Value))
                        {
                            iQClient_ThresholdValueModel.TwitterLowThreshold = float.Parse(Convert.ToString(dr["TwitterLowThreshold"]), CultureInfo.InvariantCulture.NumberFormat);
                            //(float)(dr["TwitterLowThreshold"]);
                        }

                        if (dataset.Tables[0].Columns.Contains("TwitterHighThreshold") && !dr["TwitterHighThreshold"].Equals(DBNull.Value))
                        {
                            iQClient_ThresholdValueModel.TwitterHighThreshold = float.Parse(Convert.ToString(dr["TwitterHighThreshold"]), CultureInfo.InvariantCulture.NumberFormat);
                            //(float)(dr["TwitterHighThreshold"]);
                        }

                        if (dataset.Tables[0].Columns.Contains("PQLowThreshold") && !dr["PQLowThreshold"].Equals(DBNull.Value))
                        {
                            iQClient_ThresholdValueModel.PQLowThreshold = float.Parse(Convert.ToString(dr["PQLowThreshold"]), CultureInfo.InvariantCulture.NumberFormat);
                        }

                        if (dataset.Tables[0].Columns.Contains("PQHighThreshold") && !dr["PQHighThreshold"].Equals(DBNull.Value))
                        {
                            iQClient_ThresholdValueModel.PQHighThreshold = float.Parse(Convert.ToString(dr["PQHighThreshold"]), CultureInfo.InvariantCulture.NumberFormat);
                        }
                    }
                }

                return iQClient_ThresholdValueModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQClient_CustomSettingsModel GetClientFeedsExportSettings(Guid ClientGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, ClientGuid, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQClient_CustomSettings_SelectFeedsExportSettingsByClientGUID", dataTypeList);

                IQClient_CustomSettingsModel objClientSettings = new IQClient_CustomSettingsModel();

                if (dataset != null && dataset.Tables[0].Rows.Count > 0)
                {
                    if (!dataset.Tables[0].Rows[0]["MaxFeedsExportItems"].Equals(DBNull.Value))
                    {
                        objClientSettings.v4MaxFeedsExportItems = Convert.ToInt32(dataset.Tables[0].Rows[0]["MaxFeedsExportItems"]);
                    }
                    if (!dataset.Tables[0].Rows[0]["IQRawMediaExpiration"].Equals(DBNull.Value))
                    {
                        objClientSettings.IQRawMediaExpiration = Convert.ToInt32(dataset.Tables[0].Rows[0]["IQRawMediaExpiration"]);
                    }
                }

                return objClientSettings;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ClientModel> GetAllClientWithRole(string p_ClientName, int p_PageNumner, int p_PageSize, out int p_TotalResults)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                p_TotalResults = 0;
                dataTypeList.Add(new DataType("@ClientName", DbType.String, p_ClientName, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageNumner", DbType.Int16, p_PageNumner, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageSize", DbType.Int16, p_PageSize, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@TotalResults", DbType.Int64, p_TotalResults, ParameterDirection.Output));

                Dictionary<string, string> _Output;
                DataSet dataset = DataAccess.GetDataSetWithOutParam("usp_v4_Client_SelectAllClientWithRole", dataTypeList, out  _Output);

                if (_Output != null && _Output.Count > 0)
                {
                    p_TotalResults = !string.IsNullOrWhiteSpace(_Output["@TotalResults"]) ? Convert.ToInt32(_Output["@TotalResults"]) : 0;
                }

                List<ClientModel> lstClientModel = FillClientWithRole(dataset);

                return lstClientModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ClientModel> FillClientWithRole(DataSet dataSet)
        {
            List<ClientModel> lstClientModel = new List<ClientModel>();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    ClientModel objClientModel = new ClientModel();
                    if (dataSet.Tables[0].Columns.Contains("ClientKey") && !dr["ClientKey"].Equals(DBNull.Value))
                    {
                        objClientModel.ClientKey = Convert.ToInt64(dr["ClientKey"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("AnewstipClientID") && !dr["AnewstipClientID"].Equals(DBNull.Value))
                    {
                        objClientModel.AnewstipClientID = Convert.ToInt64(dr["AnewstipClientID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("ClientName") && !dr["ClientName"].Equals(DBNull.Value))
                    {
                        objClientModel.ClientName = Convert.ToString(dr["ClientName"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Address1") && !dr["Address1"].Equals(DBNull.Value))
                    {
                        objClientModel.Address1 = Convert.ToString(dr["Address1"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Address2") && !dr["Address2"].Equals(DBNull.Value))
                    {
                        objClientModel.Address2 = Convert.ToString(dr["Address2"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Attention") && !dr["Attention"].Equals(DBNull.Value))
                    {
                        objClientModel.Attention = Convert.ToString(dr["Attention"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("City") && !dr["City"].Equals(DBNull.Value))
                    {
                        objClientModel.City = Convert.ToString(dr["City"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("MasterClient") && !dr["MasterClient"].Equals(DBNull.Value))
                    {
                        objClientModel.MasterClient = Convert.ToString(dr["MasterClient"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("MCID") && !dr["MCID"].Equals(DBNull.Value))
                    {
                        objClientModel.MCID = Convert.ToInt64(dr["MCID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("NoOfUser") && !dr["NoOfUser"].Equals(DBNull.Value))
                    {
                        objClientModel.NoOfUser = Convert.ToInt32(dr["NoOfUser"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Phone") && !dr["Phone"].Equals(DBNull.Value))
                    {
                        objClientModel.Phone = Convert.ToString(dr["Phone"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Zip") && !dr["Zip"].Equals(DBNull.Value))
                    {
                        objClientModel.Zip = Convert.ToString(dr["Zip"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("BillFrequencyID") && !dr["BillFrequencyID"].Equals(DBNull.Value))
                    {
                        objClientModel.BillFrequencyID = Convert.ToInt32(dr["BillFrequencyID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("BillTypeID") && !dr["BillTypeID"].Equals(DBNull.Value))
                    {
                        objClientModel.BillTypeID = Convert.ToInt32(dr["BillTypeID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("IndustryID") && !dr["IndustryID"].Equals(DBNull.Value))
                    {
                        objClientModel.IndustryID = Convert.ToInt32(dr["IndustryID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("PricingCodeID") && !dr["PricingCodeID"].Equals(DBNull.Value))
                    {
                        objClientModel.PricingCodeID = Convert.ToInt32(dr["PricingCodeID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("StateID") && !dr["StateID"].Equals(DBNull.Value))
                    {
                        objClientModel.StateID = Convert.ToInt32(dr["StateID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("playerlogo") && !dr["playerlogo"].Equals(DBNull.Value))
                    {
                        objClientModel.PlayerLogo = Convert.ToString(dr["playerlogo"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("IsActivePlayerLogo") && !dr["IsActivePlayerLogo"].Equals(DBNull.Value))
                    {
                        objClientModel.IsActivePlayerLogo = Convert.ToBoolean(dr["IsActivePlayerLogo"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("NoOfIQNotification") && !dr["NoOfIQNotification"].Equals(DBNull.Value))
                    {
                        objClientModel.NoOfIQNotification = Convert.ToInt16(dr["NoOfIQNotification"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("NoOfIQAgent") && !dr["NoOfIQAgent"].Equals(DBNull.Value))
                    {
                        objClientModel.NoOfIQAgent = Convert.ToInt16(dr["NoOfIQAgent"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("OtherOnlineAdRate") && !dr["OtherOnlineAdRate"].Equals(DBNull.Value))
                    {
                        objClientModel.OtherOnlineAdRate = Convert.ToDecimal(dr["OtherOnlineAdRate"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("OnlineNewsAdRate") && !dr["OnlineNewsAdRate"].Equals(DBNull.Value))
                    {
                        objClientModel.OnlineNewsAdRate = Convert.ToDecimal(dr["OnlineNewsAdRate"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("CompeteMultiplier") && !dr["CompeteMultiplier"].Equals(DBNull.Value))
                    {
                        objClientModel.CompeteMultiplier = Convert.ToDecimal(dr["CompeteMultiplier"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("URLPercentRead") && !dr["URLPercentRead"].Equals(DBNull.Value))
                    {
                        objClientModel.URLPercentRead = Convert.ToDecimal(dr["URLPercentRead"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Multiplier") && !dr["Multiplier"].Equals(DBNull.Value))
                    {
                        objClientModel.Multiplier = Convert.ToDecimal(dr["Multiplier"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("CompeteAudienceMultiplier") && !dr["CompeteAudienceMultiplier"].Equals(DBNull.Value))
                    {
                        objClientModel.CompeteAudienceMultiplier = Convert.ToDecimal(dr["CompeteAudienceMultiplier"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("v4MaxDiscoveryReportItems") && !dr["v4MaxDiscoveryReportItems"].Equals(DBNull.Value))
                    {
                        objClientModel.v4MaxDiscoveryReportItems = Convert.ToInt32(dr["v4MaxDiscoveryReportItems"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("v4MaxDiscoveryExportItems") && !dr["v4MaxDiscoveryExportItems"].Equals(DBNull.Value))
                    {
                        objClientModel.v4MaxDiscoveryExportItems = Convert.ToInt32(dr["v4MaxDiscoveryExportItems"]);
                    }
                    if (dataSet.Tables[0].Columns.Contains("visibleLRIndustries") && !dr["visibleLRIndustries"].Equals(DBNull.Value))
                    {
                        objClientModel.visibleLRIndustries = new VisibleLRIndustries();
                        objClientModel.visibleLRIndustries.Industries = new List<IQ_Industry>();
                        XDocument xDoc = XDocument.Parse((string)dr["visibleLRIndustries"]);
                        //check for attribute IsAllowAll and insert new industry of 'All'
                        if (xDoc.Element("VisibleLRIndustries").Attribute("IsAllowAll") != null && xDoc.Element("VisibleLRIndustries").Attribute("IsAllowAll").Value == "true")
                        {
                            IQ_Industry allIndustries = new IQ_Industry();
                            allIndustries.Name = "All";
                            allIndustries.ID = "0";
                            objClientModel.visibleLRIndustries.Industries.Add(allIndustries);
                        }
                        else
                        {
                            objClientModel.visibleLRIndustries = (VisibleLRIndustries)CommonFunctions.DeserialiazeXml((string)dr["visibleLRIndustries"], objClientModel.visibleLRIndustries);
                        }
                    }
                    if (dataSet.Tables[0].Columns.Contains("v4MaxDiscoveryHistory") && !dr["v4MaxDiscoveryHistory"].Equals(DBNull.Value))
                    {
                        objClientModel.v4MaxDiscoveryHistory = Convert.ToInt32(dr["v4MaxDiscoveryHistory"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("v4MaxFeedsExportItems") && !dr["v4MaxFeedsExportItems"].Equals(DBNull.Value))
                    {
                        objClientModel.v4MaxFeedsExportItems = Convert.ToInt32(dr["v4MaxFeedsExportItems"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("v4MaxFeedsReportItems") && !dr["v4MaxFeedsReportItems"].Equals(DBNull.Value))
                    {
                        objClientModel.v4MaxFeedsReportItems = Convert.ToInt32(dr["v4MaxFeedsReportItems"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("v4MaxLibraryEmailReportItems") && !dr["v4MaxLibraryEmailReportItems"].Equals(DBNull.Value))
                    {
                        objClientModel.v4MaxLibraryEmailReportItems = Convert.ToInt32(dr["v4MaxLibraryEmailReportItems"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("v4MaxLibraryReportItems") && !dr["v4MaxLibraryReportItems"].Equals(DBNull.Value))
                    {
                        objClientModel.v4MaxLibraryReportItems = Convert.ToInt32(dr["v4MaxLibraryReportItems"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("TVHighThreshold") && !dr["TVHighThreshold"].Equals(DBNull.Value))
                    {
                        objClientModel.TVHighThreshold = Convert.ToDecimal(dr["TVHighThreshold"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("TVLowThreshold") && !dr["TVLowThreshold"].Equals(DBNull.Value))
                    {
                        objClientModel.TVLowThreshold = Convert.ToDecimal(dr["TVLowThreshold"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("NMHighThreshold") && !dr["NMHighThreshold"].Equals(DBNull.Value))
                    {
                        objClientModel.NMHighThreshold = Convert.ToDecimal(dr["NMHighThreshold"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("NMLowThreshold") && !dr["NMLowThreshold"].Equals(DBNull.Value))
                    {
                        objClientModel.NMLowThreshold = Convert.ToDecimal(dr["NMLowThreshold"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("SMHighThreshold") && !dr["SMHighThreshold"].Equals(DBNull.Value))
                    {
                        objClientModel.SMHighThreshold = Convert.ToDecimal(dr["SMHighThreshold"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("SMLowThreshold") && !dr["SMLowThreshold"].Equals(DBNull.Value))
                    {
                        objClientModel.SMLowThreshold = Convert.ToDecimal(dr["SMLowThreshold"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("TwitterHighThreshold") && !dr["TwitterHighThreshold"].Equals(DBNull.Value))
                    {
                        objClientModel.TwitterHighThreshold = Convert.ToDecimal(dr["TwitterHighThreshold"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("TwitterLowThreshold") && !dr["TwitterLowThreshold"].Equals(DBNull.Value))
                    {
                        objClientModel.TwitterLowThreshold = Convert.ToDecimal(dr["TwitterLowThreshold"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("PQHighThreshold") && !dr["PQHighThreshold"].Equals(DBNull.Value))
                    {
                        objClientModel.PQHighThreshold = Convert.ToDecimal(dr["PQHighThreshold"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("PQLowThreshold") && !dr["PQLowThreshold"].Equals(DBNull.Value))
                    {
                        objClientModel.PQLowThreshold = Convert.ToDecimal(dr["PQLowThreshold"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("TimeZone") && !dr["TimeZone"].Equals(DBNull.Value))
                    {
                        objClientModel.TimeZone = Convert.ToString(dr["TimeZone"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("CDNUpload") && !dr["CDNUpload"].Equals(DBNull.Value))
                    {
                        objClientModel.IsCDNUpload = Convert.ToBoolean(dr["CDNUpload"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("IsActive") && !dr["IsActive"].Equals(DBNull.Value))
                    {
                        objClientModel.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("IsFliq") && !dr["IsFliq"].Equals(DBNull.Value))
                    {
                        objClientModel.IsFliq = Convert.ToBoolean(dr["IsFliq"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("UseProminence") && !dr["UseProminence"].Equals(DBNull.Value))
                    {
                        objClientModel.UseProminence = Convert.ToBoolean(dr["UseProminence"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("UseProminenceMediaValue") && !dr["UseProminenceMediaValue"].Equals(DBNull.Value))
                    {
                        objClientModel.UseProminenceMediaValue = Convert.ToBoolean(dr["UseProminenceMediaValue"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("ForceCategorySelection") && !dr["ForceCategorySelection"].Equals(DBNull.Value))
                    {
                        objClientModel.ForceCategorySelection = Convert.ToBoolean(dr["ForceCategorySelection"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("MCMediaPublishedTemplateID") && !dr["MCMediaPublishedTemplateID"].Equals(DBNull.Value))
                    {
                        objClientModel.MCMediaPublishedTemplateID = Convert.ToInt32(dr["MCMediaPublishedTemplateID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("MCMediaDefaultEmailTemplateID") && !dr["MCMediaDefaultEmailTemplateID"].Equals(DBNull.Value))
                    {
                        objClientModel.MCMediaDefaultEmailTemplateID = Convert.ToInt32(dr["MCMediaDefaultEmailTemplateID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("IQRawMediaExpiration") && !dr["IQRawMediaExpiration"].Equals(DBNull.Value))
                    {
                        objClientModel.IQRawMediaExpiration = Convert.ToInt32(dr["IQRawMediaExpiration"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("LibraryTextType") && !dr["LibraryTextType"].Equals(DBNull.Value))
                    {
                        objClientModel.LibraryTextType = CommonFunctions.StringToEnum<CommonFunctions.LibraryTextTypes>(Convert.ToString(dr["LibraryTextType"]));
                    }

                    if (dataSet.Tables[0].Columns.Contains("DefaultFeedsPageSize") && !dr["DefaultFeedsPageSize"].Equals(DBNull.Value))
                    {
                        objClientModel.DefaultFeedsPageSize = Convert.ToInt32(dr["DefaultFeedsPageSize"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("DefaultDiscoveryPageSize") && !dr["DefaultDiscoveryPageSize"].Equals(DBNull.Value))
                    {
                        objClientModel.DefaultDiscoveryPageSize = Convert.ToInt32(dr["DefaultDiscoveryPageSize"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("DefaultArchivePageSize") && !dr["DefaultArchivePageSize"].Equals(DBNull.Value))
                    {
                        objClientModel.DefaultArchivePageSize = Convert.ToInt32(dr["DefaultArchivePageSize"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("IQLicense") && !dr["IQLicense"].Equals(DBNull.Value))
                    {
                        try
                        {
                            objClientModel.IQLicense = dr["IQLicense"].ToString().Split(',').Select(Int16.Parse).ToList();
                        }
                        catch (FormatException)
                        {
                            objClientModel.IQLicense = new List<Int16>();
                        }
                    }
                    else
                    {
                        objClientModel.IQLicense = new List<Int16>();
                    }

                    if (dataSet.Tables[0].Columns.Contains("ClipEmbedAutoPlay") && !dr["ClipEmbedAutoPlay"].Equals(DBNull.Value))
                    {
                        objClientModel.ClipEmbedAutoPlay = Convert.ToBoolean(dr["ClipEmbedAutoPlay"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("DefaultFeedsShowUnread") && !dr["DefaultFeedsShowUnread"].Equals(DBNull.Value))
                    {
                        objClientModel.DefaultFeedsShowUnread = Convert.ToBoolean(dr["DefaultFeedsShowUnread"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("UseCustomerEmailDefault") && !dr["UseCustomerEmailDefault"].Equals(DBNull.Value))
                    {
                        objClientModel.UseCustomerEmailDefault = Convert.ToBoolean(dr["UseCustomerEmailDefault"]);
                    }

                    objClientModel.ClientRoles = new Dictionary<string, bool>();
                    foreach (DataColumn dc in dataSet.Tables[0].Columns)
                    {
                        if (dc.ColumnName != "ClientKey" && dc.ColumnName != "AnewstipClientID" && dc.ColumnName != "ClientName" && dc.ColumnName != "IsActive"
                            && dc.ColumnName != "Address1" && dc.ColumnName != "Address2" && dc.ColumnName != "Attention"
                            && dc.ColumnName != "City" && dc.ColumnName != "MasterClient" && dc.ColumnName != "MCID"
                            && dc.ColumnName != "NoOfUser" && dc.ColumnName != "Phone" && dc.ColumnName != "Zip"
                            && dc.ColumnName != "BillFrequencyID" && dc.ColumnName != "BillTypeID" && dc.ColumnName != "IndustryID"
                            && dc.ColumnName != "PricingCodeID" && dc.ColumnName != "StateID"
                            && dc.ColumnName != "playerlogo" && dc.ColumnName != "IsActivePlayerLogo"
                            && dc.ColumnName != "NoOfIQNotification" && dc.ColumnName != "NoOfIQAgent" && dc.ColumnName != "OtherOnlineAdRate"
                            && dc.ColumnName != "OnlineNewsAdRate" && dc.ColumnName != "CompeteMultiplier" && dc.ColumnName != "URLPercentRead"
                            && dc.ColumnName != "CDNUpload" && dc.ColumnName != "TimeZone"
                            && dc.ColumnName != "Multiplier" && dc.ColumnName != "CompeteAudienceMultiplier" && dc.ColumnName != "v4MaxDiscoveryReportItems"
                            && dc.ColumnName != "v4MaxFeedsExportItems" && dc.ColumnName != "v4MaxFeedsReportItems" && dc.ColumnName != "v4MaxLibraryEmailReportItems"
                            && dc.ColumnName != "v4MaxLibraryReportItems" && dc.ColumnName != "TVHighThreshold" && dc.ColumnName != "TVLowThreshold"
                            && dc.ColumnName != "NMHighThreshold" && dc.ColumnName != "NMLowThreshold" && dc.ColumnName != "SMHighThreshold"
                            && dc.ColumnName != "SMLowThreshold" && dc.ColumnName != "TwitterHighThreshold" && dc.ColumnName != "TwitterLowThreshold"
                            && dc.ColumnName != "PQHighThreshold" && dc.ColumnName != "PQLowThreshold"
                            && dc.ColumnName != "IsFliq" && dc.ColumnName != "IQLicense" && dc.ColumnName != "v4MaxDiscoveryExportItems" && dc.ColumnName != "ForceCategorySelection"
                            && dc.ColumnName != "UseProminence" && dc.ColumnName != "MCMediaPublishedTemplateID" && dc.ColumnName != "MCMediaDefaultEmailTemplateID"
                            && dc.ColumnName != "IQRawMediaExpiration" && dc.ColumnName != "LibraryTextType" && dc.ColumnName != "DefaultFeedsPageSize"
                            && dc.ColumnName != "DefaultDiscoveryPageSize" && dc.ColumnName != "DefaultArchivePageSize" && dc.ColumnName != "ClipEmbedAutoPlay"
                            && dc.ColumnName != "DefaultFeedsShowUnread" && dc.ColumnName != "UseCustomerEmailDefault" && dc.ColumnName != "visibleLRIndustries" && dc.ColumnName != "v4MaxDiscoveryHistory"
                            )
                        {
                            objClientModel.ClientRoles.Add(dc.ColumnName, Convert.ToBoolean(dr[dc.ColumnName]));
                        }
                    }

                    lstClientModel.Add(objClientModel);
                }

            }
            return lstClientModel;
        }

        public string GetClientHeaderByReportGuid(Guid p_ReportGuid)
        {
            try
            {
                string CustomHeader = string.Empty;
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ReportGuid", DbType.Guid, p_ReportGuid, ParameterDirection.Input));


                DataSet dataset = DataAccess.GetDataSet("usp_v4_Client_SelectCustomHeaderByReportGuid", dataTypeList);

                if (dataset != null && dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
                {
                    CustomHeader = Convert.ToString(dataset.Tables[0].Rows[0]["CustomHeaderImage"]);
                }

                return CustomHeader;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ClientModel> SelectAllClient()
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                DataSet dataset = DataAccess.GetDataSet("usp_v4_Client_SelectAll", dataTypeList);

                List<ClientModel> lstClientModel = FillClientWithRole(dataset);

                return lstClientModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ClientModel> SelectAllFliqClient()
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                DataSet dataset = DataAccess.GetDataSet("usp_v4_Client_SelectAllFliqClientList", dataTypeList);

                List<ClientModel> lstClientModel = FillClientWithRole(dataset);

                return lstClientModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ClientModel> GetAllClientByCustomerAndMasterClient(Int64 customerId, int mcid, string clientName, bool isAsc)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@CustomerID", DbType.String, customerId, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MCID", DbType.String, mcid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientName", DbType.String, clientName, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsAsc", DbType.String, isAsc, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_Client_SelectByCustomerAndMasterClient", dataTypeList);

                List<ClientModel> lstClientModel = FillClientWithRole(dataset);

                return lstClientModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ClientModel GetClientWithRoleByClientID(Int64 clientID)
        {
            try
            {
                ClientModel objClientModel = new ClientModel();

                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientID", DbType.String, clientID, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_Client_SelectClientWithRoleByClientID", dataTypeList);

                List<ClientModel> lstClientModel = FillClientWithRole(dataset);

                if (lstClientModel.Count > 0)
                {
                    objClientModel = lstClientModel[0];
                }
                return objClientModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Client_DropDown GetAllClientDropDown()
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                DataSet dataset = DataAccess.GetDataSet("usp_v4_Client_SelectAllDropDown", dataTypeList);

                Client_DropDown objClient_DropDown = FillClientDropDown(dataset);

                return objClient_DropDown;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Client_DropDown GetClientDropDownByClient(Int64 clientID, Client_DropDown objClient_DropDown)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@clientID", DbType.Int64, clientID, ParameterDirection.Input));
                DataSet dataset = DataAccess.GetDataSet("usp_v4_Client_SelectDropDownByClient", dataTypeList);

                if (objClient_DropDown == null)
                {
                    objClient_DropDown = new Client_DropDown();
                }

                // Insert the client-specific data into the generic object
                if (dataset != null && dataset.Tables.Count > 0)
                {
                    List<MCMediaTemplateModel> lstPubTemplateModel = new List<MCMediaTemplateModel>();
                    foreach (DataRow dr in dataset.Tables[0].Rows)
                    {
                        MCMediaTemplateModel objPubTemplateModel = new MCMediaTemplateModel();
                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objPubTemplateModel.TemplateKey = Convert.ToInt32(dr["ID"]);
                        }

                        if (!dr["Name"].Equals(DBNull.Value))
                        {
                            objPubTemplateModel.TemplateName = Convert.ToString(dr["Name"]);
                        }
                        lstPubTemplateModel.Add(objPubTemplateModel);
                    }
                    objClient_DropDown.Client_MCMediaPublishedTemplateList = lstPubTemplateModel;
                }

                if (dataset != null && dataset.Tables.Count > 1)
                {
                    List<MCMediaTemplateModel> lstEmailTemplateModel = new List<MCMediaTemplateModel>();
                    foreach (DataRow dr in dataset.Tables[1].Rows)
                    {
                        MCMediaTemplateModel objEmailTemplateModel = new MCMediaTemplateModel();
                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objEmailTemplateModel.TemplateKey = Convert.ToInt32(dr["ID"]);
                        }

                        if (!dr["Name"].Equals(DBNull.Value))
                        {
                            objEmailTemplateModel.TemplateName = Convert.ToString(dr["Name"]);
                        }
                        lstEmailTemplateModel.Add(objEmailTemplateModel);
                    }
                    objClient_DropDown.Client_MCMediaEmailTemplateList = lstEmailTemplateModel;
                }

                return objClient_DropDown;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Client_DropDown FillClientDropDown(DataSet dataSet)
        {
            try
            {
                Client_DropDown objClient_DropDown = new Client_DropDown();
                objClient_DropDown.Client_RoleList = new List<RoleModel>();
                objClient_DropDown.Client_BillFrequencyList = new List<BillFrequencyModel>();
                objClient_DropDown.Client_BillTypeList = new List<BillTypeModel>();
                objClient_DropDown.Client_IndustryList = new List<IndustryModel>();
                objClient_DropDown.Client_MasterClientList = new List<string>();
                objClient_DropDown.Client_PricingCodeList = new List<PricingCodeModel>();
                objClient_DropDown.Client_StateList = new List<StateModel>();
                objClient_DropDown.Client_MasterList = new List<ClientModel>();
                objClient_DropDown.Client_MCMediaPublishedTemplateList = new List<MCMediaTemplateModel>();
                objClient_DropDown.Client_MCMediaEmailTemplateList = new List<MCMediaTemplateModel>();

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    List<string> lstMasterClient = new List<string>();
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        if (!dr["MasterClient"].Equals(DBNull.Value))
                        {
                            lstMasterClient.Add(Convert.ToString(dr["MasterClient"]));
                        }
                    }
                    objClient_DropDown.Client_MasterClientList = lstMasterClient;
                }

                if (dataSet != null && dataSet.Tables.Count > 1)
                {
                    List<StateModel> lstState = new List<StateModel>();
                    foreach (DataRow dr in dataSet.Tables[1].Rows)
                    {
                        StateModel objStateModel = new StateModel();
                        if (!dr["StateKey"].Equals(DBNull.Value))
                        {
                            objStateModel.StateKey = Convert.ToInt32(dr["StateKey"]);
                        }

                        if (!dr["StateName"].Equals(DBNull.Value))
                        {
                            objStateModel.StateName = Convert.ToString(dr["StateName"]);
                        }
                        lstState.Add(objStateModel);
                    }
                    objClient_DropDown.Client_StateList = lstState;
                }

                if (dataSet != null && dataSet.Tables.Count > 2)
                {
                    List<IndustryModel> lstIndustryModel = new List<IndustryModel>();
                    foreach (DataRow dr in dataSet.Tables[2].Rows)
                    {
                        IndustryModel objIndustryModel = new IndustryModel();
                        if (!dr["IndustryKey"].Equals(DBNull.Value))
                        {
                            objIndustryModel.IndustryKey = Convert.ToInt32(dr["IndustryKey"]);
                        }

                        if (!dr["IndustryCode"].Equals(DBNull.Value))
                        {
                            objIndustryModel.IndustryCode = Convert.ToString(dr["IndustryCode"]);
                        }
                        lstIndustryModel.Add(objIndustryModel);
                    }
                    objClient_DropDown.Client_IndustryList = lstIndustryModel;
                }

                if (dataSet != null && dataSet.Tables.Count > 3)
                {
                    List<BillTypeModel> lstBillTypeModel = new List<BillTypeModel>();
                    foreach (DataRow dr in dataSet.Tables[3].Rows)
                    {
                        BillTypeModel objBillTypeModel = new BillTypeModel();
                        if (!dr["BillTypeKey"].Equals(DBNull.Value))
                        {
                            objBillTypeModel.BillTypeKey = Convert.ToInt32(dr["BillTypeKey"]);
                        }

                        if (!dr["Bill_Type"].Equals(DBNull.Value))
                        {
                            objBillTypeModel.Bill_Type = Convert.ToString(dr["Bill_Type"]);
                        }
                        lstBillTypeModel.Add(objBillTypeModel);
                    }
                    objClient_DropDown.Client_BillTypeList = lstBillTypeModel;
                }

                if (dataSet != null && dataSet.Tables.Count > 4)
                {
                    List<BillFrequencyModel> lstBillFrequencyModel = new List<BillFrequencyModel>();
                    foreach (DataRow dr in dataSet.Tables[4].Rows)
                    {
                        BillFrequencyModel objBillFrequencyModel = new BillFrequencyModel();
                        if (!dr["BillFrequencyKey"].Equals(DBNull.Value))
                        {
                            objBillFrequencyModel.BillFrequencyKey = Convert.ToInt32(dr["BillFrequencyKey"]);
                        }

                        if (!dr["Bill_Frequency"].Equals(DBNull.Value))
                        {
                            objBillFrequencyModel.Bill_Frequency = Convert.ToString(dr["Bill_Frequency"]);
                        }
                        lstBillFrequencyModel.Add(objBillFrequencyModel);
                    }
                    objClient_DropDown.Client_BillFrequencyList = lstBillFrequencyModel;
                }

                if (dataSet != null && dataSet.Tables.Count > 5)
                {
                    List<PricingCodeModel> lstPricingCodeModel = new List<PricingCodeModel>();
                    foreach (DataRow dr in dataSet.Tables[5].Rows)
                    {
                        PricingCodeModel objPricingCodeModel = new PricingCodeModel();
                        if (!dr["PricingCodeKey"].Equals(DBNull.Value))
                        {
                            objPricingCodeModel.PricingCodeKey = Convert.ToInt32(dr["PricingCodeKey"]);
                        }

                        if (!dr["Pricing_Code"].Equals(DBNull.Value))
                        {
                            objPricingCodeModel.Pricing_Code = Convert.ToString(dr["Pricing_Code"]);
                        }
                        lstPricingCodeModel.Add(objPricingCodeModel);
                    }
                    objClient_DropDown.Client_PricingCodeList = lstPricingCodeModel;
                }

                if (dataSet != null && dataSet.Tables.Count > 6)
                {
                    List<RoleModel> lstRoleModel = new List<RoleModel>();
                    foreach (DataRow dr in dataSet.Tables[6].Rows)
                    {
                        RoleModel objRoleModel = new RoleModel();
                        if (!dr["RoleKey"].Equals(DBNull.Value))
                        {
                            objRoleModel.RoleKey = Convert.ToInt32(dr["RoleKey"]);
                        }

                        if (!dr["RoleName"].Equals(DBNull.Value))
                        {
                            objRoleModel.RoleName = Convert.ToString(dr["RoleName"]);
                        }

                        if (!dr["UIName"].Equals(DBNull.Value))
                        {
                            objRoleModel.DisplayName = Convert.ToString(dr["UIName"]);
                        }

                        if (!dr["Description"].Equals(DBNull.Value))
                        {
                            objRoleModel.Description = Convert.ToString(dr["Description"]);
                        }

                        if (!dr["IsEnabledInSetup"].Equals(DBNull.Value))
                        {
                            objRoleModel.IsEnabledInSetup = Convert.ToBoolean(dr["IsEnabledInSetup"]);
                        }

                        if (!dr["EnabledCustomerIDs"].Equals(DBNull.Value))
                        {
                            objRoleModel.EnabledCustomerIDs = Convert.ToString(dr["EnabledCustomerIDs"]).Split(',').ToList();
                        }

                        if (!dr["GroupName"].Equals(DBNull.Value))
                        {
                            objRoleModel.GroupName = Convert.ToString(dr["GroupName"]);
                        }

                        if (!dr["HasDefaultAccess"].Equals(DBNull.Value))
                        {
                            objRoleModel.HasDefaultAccess = Convert.ToBoolean(dr["HasDefaultAccess"]);
                        }
                        lstRoleModel.Add(objRoleModel);
                    }
                    objClient_DropDown.Client_RoleList = lstRoleModel;
                }

                if (dataSet != null && dataSet.Tables.Count > 7)
                {
                    List<ClientModel> lstClientModel = new List<ClientModel>();
                    foreach (DataRow dr in dataSet.Tables[7].Rows)
                    {
                        ClientModel objClientModel = new ClientModel();
                        if (!dr["ClientKey"].Equals(DBNull.Value))
                        {
                            objClientModel.ClientKey = Convert.ToInt32(dr["ClientKey"]);
                        }

                        if (!dr["ClientName"].Equals(DBNull.Value))
                        {
                            objClientModel.ClientName = Convert.ToString(dr["ClientName"]);
                        }
                        lstClientModel.Add(objClientModel);
                    }
                    objClient_DropDown.Client_MasterList = lstClientModel;
                }

                if (dataSet != null && dataSet.Tables.Count > 8)
                {
                    List<MCMediaTemplateModel> lstPubTemplateModel = new List<MCMediaTemplateModel>();
                    foreach (DataRow dr in dataSet.Tables[8].Rows)
                    {
                        MCMediaTemplateModel objPubTemplateModel = new MCMediaTemplateModel();
                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objPubTemplateModel.TemplateKey = Convert.ToInt32(dr["ID"]);
                        }

                        if (!dr["Name"].Equals(DBNull.Value))
                        {
                            objPubTemplateModel.TemplateName = Convert.ToString(dr["Name"]);
                        }
                        lstPubTemplateModel.Add(objPubTemplateModel);
                    }
                    objClient_DropDown.Client_MCMediaPublishedTemplateList = lstPubTemplateModel;
                }

                if (dataSet != null && dataSet.Tables.Count > 9)
                {
                    List<MCMediaTemplateModel> lstEmailTemplateModel = new List<MCMediaTemplateModel>();
                    foreach (DataRow dr in dataSet.Tables[9].Rows)
                    {
                        MCMediaTemplateModel objEmailTemplateModel = new MCMediaTemplateModel();
                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objEmailTemplateModel.TemplateKey = Convert.ToInt32(dr["ID"]);
                        }

                        if (!dr["Name"].Equals(DBNull.Value))
                        {
                            objEmailTemplateModel.TemplateName = Convert.ToString(dr["Name"]);
                        }
                        lstEmailTemplateModel.Add(objEmailTemplateModel);
                    }
                    objClient_DropDown.Client_MCMediaEmailTemplateList = lstEmailTemplateModel;
                }
                if (dataSet != null && dataSet.Tables.Count > 10)
                {
                    List<IQ_Industry> lstIQIndustry = new List<IQ_Industry>();
                    foreach (DataRow dr in dataSet.Tables[10].Rows)
                    {
                        IQ_Industry industry = new IQ_Industry();
                        if (!dr["Name"].Equals(DBNull.Value))
                        {
                            industry.Name = Convert.ToString(dr["Name"]);
                        }
                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            industry.ID = Convert.ToString(dr["ID"]);
                        }
                        lstIQIndustry.Add(industry);
                    }
                    objClient_DropDown.Client_LRIndustryList = lstIQIndustry;
                }

                return objClient_DropDown;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public Dictionary<string, IQClient_CustomSettingsModel> GetClientAllCustomSettings(Guid ClientGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, ClientGuid, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQClient_CustomSettings_SelectAllSettingsByClientGuid", dataTypeList);

                IQClient_CustomSettingsModel objClientSettings = new IQClient_CustomSettingsModel();
                IQClient_CustomSettingsModel objDefaultSettings = new IQClient_CustomSettingsModel();
                Dictionary<string, IQClient_CustomSettingsModel> discClientSettings = new Dictionary<string, IQClient_CustomSettingsModel>();

                if (dataset != null && dataset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dataset.Tables[0].Rows)
                    {
                        string value = Convert.ToString(dr["Value"]);
                        string strValue = Convert.ToString(dr["StringValue"]);
                        bool IsDefault = Convert.ToBoolean(dr["IsDefault"]);
                        switch (Convert.ToString(dr["Field"]))
                        {
                            case "AutoClipDuration":
                                if (!IsDefault)
                                {
                                    objClientSettings.AutoClipDuration = Convert.ToInt32(value);
                                }
                                else
                                {
                                    objDefaultSettings.AutoClipDuration = Convert.ToInt32(value);
                                }
                                break;
                            case "TotalNoOfIQAgent":
                                if (!IsDefault)
                                {
                                    objClientSettings.TotalNoOfIQAgent = Convert.ToInt32(value);
                                }
                                else
                                {
                                    objDefaultSettings.TotalNoOfIQAgent = Convert.ToInt32(value);
                                }
                                break;
                            case "v4MaxDiscoveryReportItems":
                                if (!IsDefault)
                                {
                                    objClientSettings.v4MaxDiscoveryReportItems = Convert.ToInt32(value);
                                }
                                else
                                {
                                    objDefaultSettings.v4MaxDiscoveryReportItems = Convert.ToInt32(value);
                                }
                                break;

                            case "visibleLRIndustries":

                                List<IQ_Industry> visibleLRIndustries = new List<IQ_Industry>();
                                XDocument xDoc = XDocument.Parse(value);
                                //check for attribute IsAllowAll and insert new industry of 'All'
                                if (xDoc.Element("VisibleLRIndustries").Attribute("IsAllowAll") != null && xDoc.Element("VisibleLRIndustries").Attribute("IsAllowAll").Value == "true")
                                {
                                    IQ_Industry allIndustries = new IQ_Industry();
                                    allIndustries.Name = "All";
                                    allIndustries.ID = "0";
                                    visibleLRIndustries.Add(allIndustries);
                                }
                                else
                                {
                                    VisibleLRIndustries VLRIndustries = new VisibleLRIndustries();
                                    VLRIndustries = (VisibleLRIndustries)CommonFunctions.DeserialiazeXml(value, VLRIndustries);
                                    visibleLRIndustries = VLRIndustries.Industries;
                                }
                                if (!IsDefault)
                                {
                                    objClientSettings.visibleLRIndustries = visibleLRIndustries;
                                }
                                else
                                {
                                    objDefaultSettings.visibleLRIndustries = visibleLRIndustries;
                                }
                                break;

                            case "v4MaxDiscoveryHistory":
                                if (!IsDefault)
                                {
                                    objClientSettings.v4MaxDiscoveryHistory = Convert.ToInt32(value);
                                }
                                else
                                {
                                    objDefaultSettings.v4MaxDiscoveryHistory = Convert.ToInt32(value);
                                }
                                break;
                            case "v4MaxFeedsExportItems":
                                if (!IsDefault)
                                {
                                    objClientSettings.v4MaxFeedsExportItems = Convert.ToInt32(value);
                                }
                                else
                                {
                                    objDefaultSettings.v4MaxFeedsExportItems = Convert.ToInt32(value);
                                }
                                break;
                            case "v4MaxDiscoveryExportItems":
                                if (!IsDefault)
                                {
                                    objClientSettings.v4MaxDiscoveryExportItems = Convert.ToInt32(value);
                                }
                                else
                                {
                                    objDefaultSettings.v4MaxDiscoveryExportItems = Convert.ToInt32(value);
                                }
                                break;
                            case "v4MaxFeedsReportItems":
                                if (!IsDefault)
                                {
                                    objClientSettings.v4MaxFeedsReportItems = Convert.ToInt32(value);
                                }
                                else
                                {
                                    objDefaultSettings.v4MaxFeedsReportItems = Convert.ToInt32(value);
                                }
                                break;
                            case "v4MaxLibraryEmailReportItems":
                                if (!IsDefault)
                                {
                                    objClientSettings.v4MaxLibraryEmailReportItems = Convert.ToInt32(value);
                                }
                                else
                                {
                                    objDefaultSettings.v4MaxLibraryEmailReportItems = Convert.ToInt32(value);
                                }
                                break;
                            case "v4MaxLibraryReportItems":
                                if (!IsDefault)
                                {
                                    objClientSettings.v4MaxLibraryReportItems = Convert.ToInt32(value);
                                }
                                else
                                {
                                    objDefaultSettings.v4MaxLibraryReportItems = Convert.ToInt32(value);
                                }
                                break;
                            case "CompeteAudienceMultiplier":
                                if (!IsDefault)
                                {
                                    objClientSettings.CompeteAudienceMultiplier = Convert.ToDecimal(value);
                                }
                                else
                                {
                                    objDefaultSettings.CompeteAudienceMultiplier = Convert.ToDecimal(value);
                                }
                                break;
                            case "CompeteMultiplier":
                                if (!IsDefault)
                                {
                                    objClientSettings.CompeteMultiplier = Convert.ToDecimal(value);
                                }
                                else
                                {
                                    objDefaultSettings.CompeteMultiplier = Convert.ToDecimal(value);
                                }
                                break;
                            case "Multiplier":
                                if (!IsDefault)
                                {
                                    objClientSettings.Multiplier = Convert.ToDecimal(value);
                                }
                                else
                                {
                                    objDefaultSettings.Multiplier = Convert.ToDecimal(value);
                                }
                                break;
                            case "OnlineNewsAdRate":
                                if (!IsDefault)
                                {
                                    objClientSettings.OnlineNewsAdRate = Convert.ToDecimal(value);
                                }
                                else
                                {
                                    objDefaultSettings.OnlineNewsAdRate = Convert.ToDecimal(value);
                                }
                                break;
                            case "OtherOnlineAdRate":
                                if (!IsDefault)
                                {
                                    objClientSettings.OtherOnlineAdRate = Convert.ToDecimal(value);
                                }
                                else
                                {
                                    objDefaultSettings.OtherOnlineAdRate = Convert.ToDecimal(value);
                                }
                                break;
                            case "URLPercentRead":
                                if (!IsDefault)
                                {
                                    objClientSettings.URLPercentRead = Convert.ToDecimal(value);
                                }
                                else
                                {
                                    objDefaultSettings.URLPercentRead = Convert.ToDecimal(value);
                                }
                                break;
                            case "TVLowThreshold":
                                if (!IsDefault)
                                {
                                    objClientSettings.TVLowThreshold = Convert.ToDecimal(value);
                                }
                                else
                                {
                                    objDefaultSettings.TVLowThreshold = Convert.ToDecimal(value);
                                }
                                break;
                            case "TVHighThreshold":
                                if (!IsDefault)
                                {
                                    objClientSettings.TVHighThreshold = Convert.ToDecimal(value);
                                }
                                else
                                {
                                    objDefaultSettings.TVHighThreshold = Convert.ToDecimal(value);
                                }
                                break;
                            case "NMLowThreshold":
                                if (!IsDefault)
                                {
                                    objClientSettings.NMLowThreshold = Convert.ToDecimal(value);
                                }
                                else
                                {
                                    objDefaultSettings.NMLowThreshold = Convert.ToDecimal(value);
                                }
                                break;
                            case "NMHighThreshold":
                                if (!IsDefault)
                                {
                                    objClientSettings.NMHighThreshold = Convert.ToDecimal(value);
                                }
                                else
                                {
                                    objDefaultSettings.NMHighThreshold = Convert.ToDecimal(value);
                                }
                                break;
                            case "SMLowThreshold":
                                if (!IsDefault)
                                {
                                    objClientSettings.SMLowThreshold = Convert.ToDecimal(value);
                                }
                                else
                                {
                                    objDefaultSettings.SMLowThreshold = Convert.ToDecimal(value);
                                }
                                break;
                            case "SMHighThreshold":
                                if (!IsDefault)
                                {
                                    objClientSettings.SMHighThreshold = Convert.ToDecimal(value);
                                }
                                else
                                {
                                    objDefaultSettings.SMHighThreshold = Convert.ToDecimal(value);
                                }
                                break;
                            case "TwitterLowThreshold":
                                if (!IsDefault)
                                {
                                    objClientSettings.TwitterLowThreshold = Convert.ToDecimal(value);
                                }
                                else
                                {
                                    objDefaultSettings.TwitterLowThreshold = Convert.ToDecimal(value);
                                }
                                break;
                            case "TwitterHighThreshold":
                                if (!IsDefault)
                                {
                                    objClientSettings.TwitterHighThreshold = Convert.ToDecimal(value);
                                }
                                else
                                {
                                    objDefaultSettings.TwitterHighThreshold = Convert.ToDecimal(value);
                                }
                                break;
                            case "PQLowThreshold":
                                if (!IsDefault)
                                {
                                    objClientSettings.PQLowThreshold = Convert.ToDecimal(value);
                                }
                                else
                                {
                                    objDefaultSettings.PQLowThreshold = Convert.ToDecimal(value);
                                }
                                break;
                            case "PQHighThreshold":
                                if (!IsDefault)
                                {
                                    objClientSettings.PQHighThreshold = Convert.ToDecimal(value);
                                }
                                else
                                {
                                    objDefaultSettings.PQHighThreshold = Convert.ToDecimal(value);
                                }
                                break;
                            case "SearchSettings":
                                if (!IsDefault)
                                {
                                    IQMedia.Model.IQClient_CustomSettingsModel.SearchSettingsModel searchSettings = new IQMedia.Model.IQClient_CustomSettingsModel.SearchSettingsModel();
                                    objClientSettings.SearchSettings = (IQMedia.Model.IQClient_CustomSettingsModel.SearchSettingsModel)Shared.Utility.CommonFunctions.DeserialiazeXml(value, searchSettings);
                                }
                                else
                                {
                                    IQMedia.Model.IQClient_CustomSettingsModel.SearchSettingsModel searchSettings = new IQMedia.Model.IQClient_CustomSettingsModel.SearchSettingsModel();
                                    objDefaultSettings.SearchSettings = (IQMedia.Model.IQClient_CustomSettingsModel.SearchSettingsModel)Shared.Utility.CommonFunctions.DeserialiazeXml(value, searchSettings);
                                }
                                break;
                            case "IQLicense":
                                List<Int16> license;
                                try
                                {
                                    license = value.Split(',').Select(Int16.Parse).ToList();
                                }
                                catch (FormatException)
                                {
                                    license = new List<Int16>();
                                }
                                if (!IsDefault)
                                {
                                    objClientSettings.IQLicense = license;
                                }
                                else
                                {
                                    objDefaultSettings.IQLicense = license;
                                }
                                break;
                            case "UseProminence":
                                if (!IsDefault)
                                {
                                    objClientSettings.UseProminence = value == "1" ? true : false;
                                }
                                else
                                {
                                    objDefaultSettings.UseProminence = value == "1" ? true : false;
                                }
                                break;
                            case "ForceCategorySelection":
                                if (!IsDefault)
                                {
                                    objClientSettings.ForceCategorySelection = value == "1" ? true : false;
                                }
                                else
                                {
                                    objDefaultSettings.ForceCategorySelection = value == "1" ? true : false;
                                }
                                break;
                            case "MCMediaPublishedTemplateID":
                                if (!IsDefault)
                                {
                                    objClientSettings.MCMediaPublishedTemplateID = Convert.ToInt32(value);
                                    objClientSettings.MCMediaPublishedTemplate = strValue;
                                }
                                else
                                {
                                    objDefaultSettings.MCMediaPublishedTemplateID = Convert.ToInt32(value);
                                    objDefaultSettings.MCMediaPublishedTemplate = strValue;
                                }
                                break;
                            case "MCMediaDefaultEmailTemplateID":
                                if (!IsDefault)
                                {
                                    objClientSettings.MCMediaDefaultEmailTemplateID = Convert.ToInt32(value);
                                    objClientSettings.MCMediaDefaultEmailTemplate = strValue;
                                }
                                else
                                {
                                    objDefaultSettings.MCMediaDefaultEmailTemplateID = Convert.ToInt32(value);
                                    objDefaultSettings.MCMediaDefaultEmailTemplate = strValue;
                                }
                                break;
                            case "MCMediaAvailableTemplates":
                                if (!IsDefault)
                                {
                                    IQMedia.Model.IQClient_CustomSettingsModel.MCMediaAvailableTemplatesModel availableTemplates = new IQMedia.Model.IQClient_CustomSettingsModel.MCMediaAvailableTemplatesModel();
                                    objClientSettings.MCMediaAvailableTemplates = (IQMedia.Model.IQClient_CustomSettingsModel.MCMediaAvailableTemplatesModel)Shared.Utility.CommonFunctions.DeserialiazeXml(value, availableTemplates);
                                }
                                else
                                {
                                    IQMedia.Model.IQClient_CustomSettingsModel.MCMediaAvailableTemplatesModel availableTemplates = new IQMedia.Model.IQClient_CustomSettingsModel.MCMediaAvailableTemplatesModel();
                                    objDefaultSettings.MCMediaAvailableTemplates = (IQMedia.Model.IQClient_CustomSettingsModel.MCMediaAvailableTemplatesModel)Shared.Utility.CommonFunctions.DeserialiazeXml(value, availableTemplates);
                                }
                                break;
                            case "IQRawMediaExpiration":
                                if (!IsDefault)
                                {
                                    objClientSettings.IQRawMediaExpiration = Convert.ToInt32(value);
                                }
                                else
                                {
                                    objDefaultSettings.IQRawMediaExpiration = Convert.ToInt32(value);
                                }
                                break;
                            case "LibraryTextType":
                                if (!IsDefault)
                                {
                                    objClientSettings.LibraryTextType = CommonFunctions.StringToEnum<CommonFunctions.LibraryTextTypes>(value);
                                }
                                else
                                {
                                    objDefaultSettings.LibraryTextType = CommonFunctions.StringToEnum<CommonFunctions.LibraryTextTypes>(value);
                                }
                                break;
                            case "DefaultFeedsPageSize":
                                if (!IsDefault)
                                {
                                    objClientSettings.DefaultFeedsPageSize = Convert.ToInt32(value);
                                }
                                else
                                {
                                    objDefaultSettings.DefaultFeedsPageSize = Convert.ToInt32(value);
                                }
                                break;
                            case "DefaultDiscoveryPageSize":
                                if (!IsDefault)
                                {
                                    objClientSettings.DefaultDiscoveryPageSize = Convert.ToInt32(value);
                                }
                                else
                                {
                                    objDefaultSettings.DefaultDiscoveryPageSize = Convert.ToInt32(value);
                                }
                                break;
                            case "DefaultArchivePageSize":
                                if (!IsDefault)
                                {
                                    objClientSettings.DefaultArchivePageSize = Convert.ToInt32(value);
                                }
                                else
                                {
                                    objDefaultSettings.DefaultArchivePageSize = Convert.ToInt32(value);
                                }
                                break;
                            case "UseProminenceMediaValue":
                                if (!IsDefault)
                                {
                                    objClientSettings.UseProminenceMediaValue = value == "1";
                                }
                                else
                                {
                                    objDefaultSettings.UseProminenceMediaValue = value == "1";
                                }
                                break;
                            case "ClipEmbedAutoPlay":
                                if (!IsDefault)
                                {
                                    objClientSettings.ClipEmbedAutoPlay = value == "1";
                                }
                                else
                                {
                                    objDefaultSettings.ClipEmbedAutoPlay = value == "1";
                                }
                                break;
                            case "DefaultFeedsShowUnread":
                                if (!IsDefault)
                                {
                                    objClientSettings.DefaultFeedsShowUnread = value == "1";
                                }
                                else
                                {
                                    objDefaultSettings.DefaultFeedsShowUnread = value == "1";
                                }
                                break;
                            case "UseCustomerEmailDefault":
                                if (!IsDefault)
                                {
                                    objClientSettings.UseCustomerEmailDefault = value == "1";
                                }
                                else
                                {
                                    objDefaultSettings.UseCustomerEmailDefault = value == "1";
                                }
                                break;
                        }
                    }
                }

                discClientSettings.Add("ClientSettings", objClientSettings);
                discClientSettings.Add("DefaultSettings", objDefaultSettings);
                return discClientSettings;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetClientManualClipDurationSettings(Guid p_ClientGUID)
        {
            List<DataType> dataTypeList = new List<DataType>();

            dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGUID, ParameterDirection.Input));

            var i = DataAccess.ExecuteScalar("usp_v4_IQClient_CustomSettings_SelectManualClipDuration", dataTypeList);

            return Convert.ToInt32(i);
        }

        public List<IQClient_UGCMapModel> GetAllClientUGCSettings(string p_ClientName, string p_SearchTerm, int p_PageNumner, int p_PageSize, out Int64 p_TotalResults)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                p_TotalResults = 0;
                dataTypeList.Add(new DataType("@ClientName", DbType.String, p_ClientName, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchTerm", DbType.String, p_SearchTerm, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageNumner", DbType.Int16, p_PageNumner, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageSize", DbType.Int16, p_PageSize, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@TotalResults", DbType.Int64, p_TotalResults, ParameterDirection.Output));

                Dictionary<string, string> _Output;
                DataSet dataset = DataAccess.GetDataSetWithOutParam("usp_v4_IQClient_UGCMap_SelectAll", dataTypeList, out  _Output);

                if (_Output != null && _Output.Count > 0)
                {
                    p_TotalResults = !string.IsNullOrWhiteSpace(_Output["@TotalResults"]) ? Convert.ToInt32(_Output["@TotalResults"]) : 0;
                }

                List<IQClient_UGCMapModel> lstIQClient_UGCMapModel = FillIQClient_UGCMap(dataset);

                return lstIQClient_UGCMapModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQClient_UGCMapDropDowns GetUGCSettingsDropdown()
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                DataSet dataSet = DataAccess.GetDataSet("usp_v4_IQClient_UGCMap_SelectAllDropdown", dataTypeList);

                IQClient_UGCMapDropDowns objIQClient_UGCMapDropDowns = new IQClient_UGCMapDropDowns();
                objIQClient_UGCMapDropDowns.Client_DropDown = new List<ClientModel>();
                objIQClient_UGCMapDropDowns.TimeZone_DropDown = new List<IQTimeZone>();

                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        ClientModel objClientModel = new ClientModel();
                        if (dataSet.Tables[0].Columns.Contains("ClientName") && !dr["ClientName"].Equals(DBNull.Value))
                        {
                            objClientModel.ClientName = Convert.ToString(dr["ClientName"]);
                        }

                        if (dataSet.Tables[0].Columns.Contains("ClientGUID") && !dr["ClientGUID"].Equals(DBNull.Value))
                        {
                            objClientModel.ClientGuid = new Guid(Convert.ToString(dr["ClientGUID"]));
                        }

                        objIQClient_UGCMapDropDowns.Client_DropDown.Add(objClientModel);
                    }
                    foreach (DataRow dr in dataSet.Tables[1].Rows)
                    {
                        IQTimeZone timezone = new IQTimeZone();
                        if(dataSet.Tables[1].Columns.Contains("ID") && !dr["ID"].Equals(DBNull.Value))
                        {
                            timezone.ID = Convert.ToInt32(dr["ID"]);
                        }
                        if (dataSet.Tables[1].Columns.Contains("Code") && !dr["Code"].Equals(DBNull.Value))
                        {
                            timezone.Code = Convert.ToString(dr["Code"]);
                        }
                        if (dataSet.Tables[1].Columns.Contains("Name") && !dr["Name"].Equals(DBNull.Value))
                        {
                            timezone.Name = Convert.ToString(dr["Name"]);
                        }
                        objIQClient_UGCMapDropDowns.TimeZone_DropDown.Add(timezone);
                    }
                }
                return objIQClient_UGCMapDropDowns;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQClient_UGCMapModel GetUGCSettingsByUGCMapKey(Int64 p_IQClient_UGCMapKey)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@IQClient_UGCMapKey", DbType.Int64, p_IQClient_UGCMapKey, ParameterDirection.Input));

                DataSet dataSet = DataAccess.GetDataSet("usp_v4_IQClient_UGCMap_SelectByUGCMapKey", dataTypeList);

                IQClient_UGCMapModel objIQClient_UGCMapModel = new IQClient_UGCMapModel();

                List<IQClient_UGCMapModel> lstIQClient_UGCMapModel = FillIQClient_UGCMap(dataSet);

                if (lstIQClient_UGCMapModel.Count > 0)
                {
                    objIQClient_UGCMapModel = lstIQClient_UGCMapModel[0];
                }

                return objIQClient_UGCMapModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string InsertIQClient_UGCMap(IQClient_UGCMapModel p_IQClient_UGCMapModel)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, p_IQClient_UGCMapModel.ClientGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@TimeZoneID", DbType.Int32, p_IQClient_UGCMapModel.TimeZoneID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@AutoClip_Status", DbType.Boolean, p_IQClient_UGCMapModel.AutoClip_Status, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SourceID", DbType.String, p_IQClient_UGCMapModel.SourceID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@BroadcastType", DbType.String, p_IQClient_UGCMapModel.BroadcastType, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Logo", DbType.String, p_IQClient_UGCMapModel.Logo, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@URL", DbType.String, p_IQClient_UGCMapModel.URL, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Title", DbType.String, p_IQClient_UGCMapModel.Title, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQClient_UGCMapKey", DbType.Int64, 0, ParameterDirection.Output));


                _Result = DataAccess.ExecuteNonQuery("usp_v4_IQClient_UGCMap_Insert", _ListOfDataType);

                return _Result;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string UpdateIQClient_UGCMap(IQClient_UGCMapModel p_IQClient_UGCMapModel)
        {
            try
            {

                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, p_IQClient_UGCMapModel.ClientGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@TimeZoneID", DbType.Int32, p_IQClient_UGCMapModel.TimeZoneID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@AutoClip_Status", DbType.Boolean, p_IQClient_UGCMapModel.AutoClip_Status, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SourceID", DbType.String, p_IQClient_UGCMapModel.SourceID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SourceGuid", DbType.Guid, p_IQClient_UGCMapModel.SourceGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@BroadcastType", DbType.String, p_IQClient_UGCMapModel.BroadcastType, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Logo", DbType.String, p_IQClient_UGCMapModel.Logo, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Title", DbType.String, p_IQClient_UGCMapModel.Title, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@URL", DbType.String, p_IQClient_UGCMapModel.URL, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQClient_UGCMapKey", DbType.Int64, p_IQClient_UGCMapModel.IQClient_UGCMapKey, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Output", DbType.Int32, 0, ParameterDirection.Output));


                _Result = DataAccess.ExecuteNonQuery("usp_v4_IQClient_UGCMap_Update", _ListOfDataType);


                return _Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<IQClient_UGCMapModel> FillIQClient_UGCMap(DataSet dataSet)
        {
            List<IQClient_UGCMapModel> lstIQClient_UGCMapModel = new List<IQClient_UGCMapModel>();
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    IQClient_UGCMapModel objIQClient_UGCMapModel = new IQClient_UGCMapModel();
                    if (dataSet.Tables[0].Columns.Contains("IQClient_UGCMapKey") && !dr["IQClient_UGCMapKey"].Equals(DBNull.Value))
                    {
                        objIQClient_UGCMapModel.IQClient_UGCMapKey = Convert.ToInt64(dr["IQClient_UGCMapKey"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("_TimezoneID") && !dr["_TimezoneID"].Equals(DBNull.Value))
                    {
                        objIQClient_UGCMapModel.TimeZoneID = Convert.ToInt32(dr["_TimezoneID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("AutoClip_Status") && !dr["AutoClip_Status"].Equals(DBNull.Value))
                    {
                        objIQClient_UGCMapModel.AutoClip_Status = Convert.ToBoolean(dr["AutoClip_Status"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("SourceGUID") && !dr["SourceGUID"].Equals(DBNull.Value))
                    {
                        objIQClient_UGCMapModel.SourceGUID = new Guid(Convert.ToString(dr["SourceGUID"]));
                    }

                    if (dataSet.Tables[0].Columns.Contains("BroadcastLocation") && !dr["BroadcastLocation"].Equals(DBNull.Value))
                    {
                        objIQClient_UGCMapModel.BroadcastLocation = Convert.ToString(dr["BroadcastLocation"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("BroadcastType") && !dr["BroadcastType"].Equals(DBNull.Value))
                    {
                        objIQClient_UGCMapModel.BroadcastType = Convert.ToString(dr["BroadcastType"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Logo") && !dr["Logo"].Equals(DBNull.Value))
                    {
                        objIQClient_UGCMapModel.Logo = Convert.ToString(dr["Logo"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("RetentionDays") && !dr["RetentionDays"].Equals(DBNull.Value))
                    {
                        objIQClient_UGCMapModel.RetentionDays = Convert.ToInt32(dr["RetentionDays"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("SourceID") && !dr["SourceID"].Equals(DBNull.Value))
                    {
                        objIQClient_UGCMapModel.SourceID = Convert.ToString(dr["SourceID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Title") && !dr["Title"].Equals(DBNull.Value))
                    {
                        objIQClient_UGCMapModel.Title = Convert.ToString(dr["Title"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("URL") && !dr["URL"].Equals(DBNull.Value))
                    {
                        objIQClient_UGCMapModel.URL = Convert.ToString(dr["URL"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("ClientName") && !dr["ClientName"].Equals(DBNull.Value))
                    {
                        objIQClient_UGCMapModel.ClientName = Convert.ToString(dr["ClientName"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("IsActive") && !dr["IsActive"].Equals(DBNull.Value))
                    {
                        objIQClient_UGCMapModel.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("ClientGuid") && !dr["ClientGuid"].Equals(DBNull.Value))
                    {
                        objIQClient_UGCMapModel.ClientGuid = new Guid(Convert.ToString(dr["ClientGuid"]));
                    }

                    lstIQClient_UGCMapModel.Add(objIQClient_UGCMapModel);
                }
            }
            return lstIQClient_UGCMapModel;
        }

        public short GetAPIIframeCSSOverrideSettings(Guid p_ClientGUID)
        {
            short result = 0;

            List<DataType> dataTypeList = new List<DataType>();

            dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGUID, ParameterDirection.Input));

            result = Convert.ToInt16(DataAccess.ExecuteScalar("usp_v4_IQClient_CustomSettings_SelectAPIIframeCSSOrideByClientGUID", dataTypeList));

            return result;
        }

        public short GetClipCCExportSettings(Guid p_ClientGUID)
        {
            short result = 0;

            List<DataType> dataTypeList = new List<DataType>();

            dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGUID, ParameterDirection.Input));

            result = Convert.ToInt16(DataAccess.ExecuteScalar("usp_v5_IQClient_CustomSettings_SelectClipCCExport", dataTypeList));

            return result;
        }

        public List<ClientModel> SelectAllActive()
        {
            List<DataType> dataTypeList = new List<DataType>();

            DataSet dataset = DataAccess.GetDataSet("usp_v5_Client_SelectAllActive", dataTypeList);

            List<ClientModel> lstClientModel = FillClientWithRole(dataset);

            return lstClientModel;
        }

        public bool GroupAddSubClient(Int64 p_MCID, string p_SCXml, Guid p_CustomerGUID)
        {
            List<DataType> dtL = new List<DataType>();

            dtL.Add(new DataType("@MCID", DbType.Int64, p_MCID, ParameterDirection.Input));
            dtL.Add(new DataType("@SCXml", DbType.String, p_SCXml, ParameterDirection.Input));
            dtL.Add(new DataType("@CustomerGUID", DbType.Guid, p_CustomerGUID, ParameterDirection.Input));

            var output = DataAccess.ExecuteNonQuery("usp_V5_Group_Client_AddSubClient", dtL);            

            return true;
        }

        public bool GroupRemoveSubClient(Int64 p_MCID, Int64 p_SCID, Guid p_CustomerGUID)
        {
            List<DataType> dtL = new List<DataType>();

            dtL.Add(new DataType("@MCID", DbType.Int64, p_MCID, ParameterDirection.Input));
            dtL.Add(new DataType("@SCID", DbType.Int64, p_SCID, ParameterDirection.Input));
            dtL.Add(new DataType("@CustomerGUID", DbType.Guid, p_CustomerGUID, ParameterDirection.Input));

            var output = DataAccess.ExecuteNonQuery("usp_V5_Group_Client_RemoveSubClient", dtL);

            return true;
        }

        public List<CustomerModel> GroupGetCustomerByClient(Int64 p_ClientID, Int64? p_CustomerID)
        {
            List<DataType> dtL = new List<DataType>();

            dtL.Add(new DataType("@CID", DbType.Int64, p_ClientID, ParameterDirection.Input));
            dtL.Add(new DataType("@CustID", DbType.Int64, p_CustomerID, ParameterDirection.Input));

            var ds = DataAccess.GetDataSet("usp_V5_Group_Customer_SelectByClient", dtL);

            List<CustomerModel> custList = new List<CustomerModel>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CustomerModel cust = new CustomerModel();

                    cust.CustomerKey = Convert.ToInt32(dr["CustomerKey"]);
                    cust.FirstName = Convert.ToString(dr["FirstName"]);
                    cust.LastName = Convert.ToString(dr["LastName"]);

                    custList.Add(cust);
                }
            }

            return custList;
        }

        public void AddClientToAnewstip(long clientKey, long AnewstipClientID)
        {
            List<DataType> dataTypeList = new List<DataType>();
            dataTypeList.Add(new DataType("@ClientKey", DbType.Int64, clientKey, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@AnewstipClientID", DbType.Int64, AnewstipClientID, ParameterDirection.Input));
            DataAccess.ExecuteNonQuery("usp_v5_Client_AddToAnewstip", dataTypeList);
        }
    }
}
