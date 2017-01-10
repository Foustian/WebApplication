using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Web.Logic.Base;
using IQMedia.Model;
using IQMedia.Logic.Base;
using IQMedia.Data;
using System.Xml.Linq;

namespace IQMedia.Web.Logic
{
    public class ClientLogic : ILogic
    {
        public ClientModel GetArchiveClipByClipID(string ClientGuid)
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.GetClientInfoByClientGuid(ClientGuid);
        }

        public IQClient_CustomSettingsModel GetClientCustomSettings(string ClientGuid)
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.GetClientCustomSettings(ClientGuid);
        }

        public int GetClientRoleByClientGUIDRoleName(Guid ClientGUID, string RoleName)
        {
            ClientDA clientDA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDA.GetClientRoleByClientGUIDRoleName(ClientGUID, RoleName);
        }

        public IQClient_ThresholdValueModel GetClientThresholdValue(Guid ClientGuid)
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.GetClientThresholdValue(ClientGuid);
        }

        public IQClient_CustomSettingsModel GetClientFeedsExportSettings(Guid ClientGuid)
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.GetClientFeedsExportSettings(ClientGuid);
        }

        public List<ClientModel> GetAllClientWithRole(string p_ClientName, int p_PageNumner, int p_PageSize, out int p_TotalResults)
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.GetAllClientWithRole(p_ClientName, p_PageNumner, p_PageSize, out p_TotalResults);
        }

        public string GetClientHeaderByReportGuid(Guid p_ReportGuid)
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.GetClientHeaderByReportGuid(p_ReportGuid);
        }

        public List<ClientModel> SelectAllClient()
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.SelectAllClient();
        }

        public List<ClientModel> GetAllClientByCustomerAndMasterClient(Int64 customerId, int mcid, string clientName, bool isAsc)
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.GetAllClientByCustomerAndMasterClient(customerId, mcid, clientName, isAsc);
        }

        public Client_DropDown GetAllClientDropDown()
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.GetAllClientDropDown();
        }

        public Client_DropDown GetClientDropDownByClient(Int64 clientID, Client_DropDown objClient_DropDown)
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.GetClientDropDownByClient(clientID, objClient_DropDown);
        }

        public ClientModel GetClientWithRoleByClientID(Int64 clientID)
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.GetClientWithRoleByClientID(clientID);
        }

        public string InsertClient(ClientModel p_Client, string p_Roles, out int Status, string p_RootFolder)
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.InsertClient(p_Client, p_Roles, out Status, p_RootFolder);
        }

        public string UpdateClient(ClientModel p_Client, string p_Roles, out int Status, out int p_NotificationStatus, out int p_IQAgentStatus)
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.UpdateClient(p_Client, p_Roles, out Status, out p_NotificationStatus, out p_IQAgentStatus);
        }

        public string DeleteClient(Int64 p_ClientKey)
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.DeleteClient(p_ClientKey);
        }

        public ClientPostModel GetClientPostModel( ClientModel clientModel)
        {
            ClientPostModel clientPostModel = new ClientPostModel();
            clientPostModel.txtClientName = clientModel.ClientName;
            clientPostModel.hdnClientKey = clientModel.ClientKey;
            clientPostModel.hdnAnewstipClientID = clientModel.AnewstipClientID;

            clientPostModel.ddlPricingCode = clientModel.PricingCodeID;
            clientPostModel.ddlBillFrequency = clientModel.BillFrequencyID;
            clientPostModel.ddlBillType = clientModel.BillTypeID;
            clientPostModel.ddlIndustry = clientModel.IndustryID;
            clientPostModel.ddlState = clientModel.StateID;
            clientPostModel.txtAddress1 = clientModel.Address1;
            clientPostModel.txtAddress2 = clientModel.Address2;
            clientPostModel.txtCity = clientModel.City;
            clientPostModel.txtZip = clientModel.Zip;
            clientPostModel.txtAttention = clientModel.Attention;
            clientPostModel.txtPhone = clientModel.Phone;
            clientPostModel.ddlMCID = clientModel.MCID;
            clientPostModel.txtMasterClient = clientModel.MasterClient;

            clientPostModel.chkIsPlayerLogo = clientModel.IsActivePlayerLogo;

            clientPostModel.hfPlayerLogoImage = clientModel.PlayerLogo;

          
            


            //_Client.MasterClient = "test1";
            clientPostModel.txtNoOfUsers = clientModel.NoOfUser;

            clientPostModel.txtNoOfNotification = clientModel.NoOfIQNotification;

            clientPostModel.txtNoOfIQAgent = clientModel.NoOfIQAgent;
            clientPostModel.txtCompeteMultiplier = clientModel.CompeteMultiplier;
            clientPostModel.txtOnlineNewsAdRate = clientModel.OnlineNewsAdRate;
            clientPostModel.txtOtherOnlineAdRate = clientModel.OtherOnlineAdRate;
            clientPostModel.txtURLPercentRead = clientModel.URLPercentRead;

            clientPostModel.txtMultiplier = clientModel.Multiplier;
            clientPostModel.txtCompeteAudienceMultiplier = clientModel.CompeteAudienceMultiplier;
            clientPostModel.txtv4MaxDiscoveryReportItems = clientModel.v4MaxDiscoveryReportItems;
            clientPostModel.txtv4MaxDiscoveryExportItems = clientModel.v4MaxDiscoveryExportItems;

            clientPostModel.selectedVisibleLRIndustries = new List<string>();
            foreach(IQ_Industry i in clientModel.visibleLRIndustries.Industries){
                clientPostModel.selectedVisibleLRIndustries.Add(i.ID);
            };

            clientPostModel.txtv4MaxDiscoveryHistory = clientModel.v4MaxDiscoveryHistory;
            clientPostModel.txtv4MaxFeedsExportItems = clientModel.v4MaxFeedsExportItems;
            clientPostModel.txtv4MaxFeedsReportItems = clientModel.v4MaxFeedsReportItems;
            clientPostModel.txtv4MaxLibraryEmailReportItems = clientModel.v4MaxLibraryEmailReportItems;
            clientPostModel.txtv4MaxLibraryReportItems = clientModel.v4MaxLibraryReportItems;
            clientPostModel.txtTVHighThreshold = clientModel.TVHighThreshold;
            clientPostModel.txtTVLowThreshold = clientModel.TVLowThreshold;
            clientPostModel.txtNMHighThreshold = clientModel.NMHighThreshold;
            clientPostModel.txtNMLowThreshold = clientModel.NMLowThreshold;
            clientPostModel.txtSMHighThreshold = clientModel.SMHighThreshold;
            clientPostModel.txtSMLowThreshold = clientModel.SMLowThreshold;
            clientPostModel.txtTwitterHighThreshold = clientModel.TwitterHighThreshold;
            clientPostModel.txtTwitterLowThreshold = clientModel.TwitterLowThreshold;
            clientPostModel.txtPQHighThreshold = clientModel.PQHighThreshold;
            clientPostModel.txtPQLowThreshold = clientModel.PQLowThreshold;
            clientPostModel.ddlTimeZone = clientModel.TimeZone;
            clientPostModel.chkIsCDNUpload = clientModel.IsCDNUpload;
            clientPostModel.chkIsActive = clientModel.IsActive;
            clientPostModel.chkIsFliq = clientModel.IsFliq;
            clientPostModel.chkUseProminence = clientModel.UseProminence;
            clientPostModel.chkUseProminenceMediaValue = clientModel.UseProminenceMediaValue;
            clientPostModel.chkForceCategorySelection = clientModel.ForceCategorySelection;
            clientPostModel.ddlMCMediaPubTemplate = clientModel.MCMediaPublishedTemplateID;
            clientPostModel.ddlMCMediaEmailTemplate = clientModel.MCMediaDefaultEmailTemplateID;
            clientPostModel.txtIQRawMediaExpiration = clientModel.IQRawMediaExpiration;
            clientPostModel.ddlLibraryTextType = clientModel.LibraryTextType;
            clientPostModel.ddlDefaultFeedsPageSize = clientModel.DefaultFeedsPageSize;
            clientPostModel.ddlDefaultDiscoveryPageSize = clientModel.DefaultDiscoveryPageSize;
            clientPostModel.ddlDefaultArchivePageSize = clientModel.DefaultArchivePageSize;
            clientPostModel.chkClipEmbedAutoPlay = clientModel.ClipEmbedAutoPlay;
            clientPostModel.chkDefaultFeedsShowUnread = clientModel.DefaultFeedsShowUnread;
            clientPostModel.chkUseCustomerEmailDefault = clientModel.UseCustomerEmailDefault;

            clientPostModel.chkHasPremium = clientModel.IQLicense != null && clientModel.IQLicense.Contains(2) && clientModel.IQLicense.Contains(3);

            clientPostModel.chkRoles = clientModel.ClientRoles.Where(a => a.Value == true).Select(a => a.Key).ToArray();

            return clientPostModel;
        }

        public List<Int16> GetClientLicenseSettings(Guid ClientGuid)
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.GetClientLicenseSettings(ClientGuid);
        }

        public Int16 GetClientRawMediaPauseSecs(Guid ClientGuid)
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.GetClientRawMediaPauseSecs(ClientGuid);
        }
        
        [Obsolete] // TODO: Determine if this can be removed
        public List<Int16> GetClientSettingsIQLicenseByCustomerID(Int64 CustomerID, out bool p_IsDefaultSettings)
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.GetClientSettingsIQLicenseByCustomerID(CustomerID, out p_IsDefaultSettings);
        }

        public Dictionary<string, IQClient_CustomSettingsModel> GetClientAllCustomSettings(Guid ClientGuid)
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.GetClientAllCustomSettings(ClientGuid);
        }

        public List<object> GetClientTVRegionSettings(Guid ClientGuid)
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.GetClientTVRegionSettings(ClientGuid);
        }

        public int GetClientManualClipDurationSettings(Guid p_ClientGUID)
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.GetClientManualClipDurationSettings(p_ClientGUID);
        }

        public List<ClientModel> SelectAllFliqClient()
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.SelectAllFliqClient();
        }

        public List<IQClient_UGCMapModel> GetAllClientUGCSettings(string p_ClientName, string p_SearchTerm, int p_PageNumner, int p_PageSize, out Int64 p_TotalResults)
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.GetAllClientUGCSettings(p_ClientName, p_SearchTerm, p_PageNumner, p_PageSize, out p_TotalResults);
        }

        public IQClient_UGCMapDropDowns GetUGCSettingsDropdown()
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.GetUGCSettingsDropdown();
        }

        public IQClient_UGCMapModel GetUGCSettingsByUGCMapKey(Int64 p_IQClient_UGCMapKey)
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.GetUGCSettingsByUGCMapKey(p_IQClient_UGCMapKey);
        }

        public string InsertIQClient_UGCMap(IQClient_UGCMapModel p_IQClient_UGCMapModel)
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.InsertIQClient_UGCMap(p_IQClient_UGCMapModel);
        }

        public string UpdateIQClient_UGCMap(IQClient_UGCMapModel p_IQClient_UGCMapModel)
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDADA.UpdateIQClient_UGCMap(p_IQClient_UGCMapModel);
        }

        public bool GetAPIIframeCSSOverrideSettings(Guid p_ClientGUID)
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            short result= clientDADA.GetAPIIframeCSSOverrideSettings(p_ClientGUID);

            return (result == 1);
        }

        public bool GetClipCCExportSettings(Guid p_ClientGUID)
        {
            ClientDA clientDADA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return (clientDADA.GetClipCCExportSettings(p_ClientGUID) == 1);
        }

        public List<ClientModel> SelectAllActive()
        {
            ClientDA clientDA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDA.SelectAllActive();
        }

        public bool GroupAddSubClient(Int64 p_MCID, List<Int64> p_SCIDList, List<ClientModel> p_ClientList, Guid p_CustomerGUID)
        {
            var valid = false;

            valid = p_ClientList.Where(c => c.ClientKey == p_MCID && (c.MCID == 0 || c.MCID == c.ClientKey)).Count() == 1;

            if (valid)
            {
                foreach (var SCID in p_SCIDList)
                {
                    valid = p_ClientList.Where(c => c.ClientKey == SCID && (c.MCID == 0 || c.MCID == c.ClientKey)).Count() == 1;

                    valid = p_ClientList.Where(c => c.MCID == SCID && c.ClientKey != SCID).Count() == 0;

                    if (!valid)
                    {
                        break;
                    }
                }
            }

            if (valid)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ID in p_SCIDList
                                             select new XElement("item", new XAttribute("ID", ID))
                                                     ));

                ClientDA clientDA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
                valid = clientDA.GroupAddSubClient(p_MCID, xdoc.ToString(), p_CustomerGUID);
            }

            return valid;
        }

        public bool GroupRemoveSubClient(Int64 p_MCID, Int64 p_SCID, Guid p_CustomerGUID)
        {
            ClientDA clientDA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDA.GroupRemoveSubClient(p_MCID, p_SCID, p_CustomerGUID);
        }

        public List<CustomerModel> GroupGetCustomerByClient(Int64 p_ClientID, Int64? p_CustomerID)
        {
            ClientDA clientDA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            return clientDA.GroupGetCustomerByClient(p_ClientID, p_CustomerID);
        }

        public void AddClientToAnewstip(long clientKey, long AnewstipClientID)
        {
            ClientDA clientDA = (ClientDA)DataAccessFactory.GetDataAccess(DataAccessType.Client);
            clientDA.AddClientToAnewstip(clientKey, AnewstipClientID);
        }
    }
}
