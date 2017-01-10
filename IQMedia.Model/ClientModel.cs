using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    [Serializable]
    public class ClientModel
    {
        public long ClientKey { get; set; }

        public long AnewstipClientID { get; set; }

        public Guid ClientGuid { get; set; }

        public string DefaultCategory { get; set; }

        public string ClientName { get; set; }

        public string UGCFtpUploadLocation { get; set; }

        public int StateID { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string Zip { get; set; }

        public string Phone { get; set; }

        public bool IsActive { get; set; }

        public Dictionary<string, bool> ClientRoles { get; set; }

        public List<Int16> IQLicense { get; set; }

        public string CustomHeaderImage { get; set; }

        public int PricingCodeID { get; set; }

        public int BillFrequencyID { get; set; }

        public int BillTypeID { get; set; }

        public int IndustryID { get; set; }

        public string Attention { get; set; }

        public string MasterClient { get; set; }

        public int NoOfUser { get; set; }

        public string PlayerLogo { get; set; }

        public bool IsActivePlayerLogo { get; set; }

        public long MCID { get; set; }

        public short NoOfIQNotification { get; set; }

        public short NoOfIQAgent { get; set; }

        public decimal OtherOnlineAdRate { get; set; }

        public decimal OnlineNewsAdRate { get; set; }

        public decimal CompeteMultiplier { get; set; }

        public decimal URLPercentRead { get; set; }

        public string TimeZone { get; set; }

        public string NotificationHeaderImage { get; set; }

        public bool IsCDNUpload { get; set; }

        public int AutoClipDuration { get; set; }

        public VisibleLRIndustries visibleLRIndustries { get; set; }

        public int? v4MaxDiscoveryReportItems { get; set; }

        public int? v4MaxDiscoveryExportItems { get; set; }

        public int? v4MaxDiscoveryHistory { get; set; }

        public int? v4MaxFeedsExportItems { get; set; }

        public int? v4MaxFeedsReportItems { get; set; }

        public int? v4MaxLibraryEmailReportItems { get; set; }

        public int? v4MaxLibraryReportItems { get; set; }

        public decimal CompeteAudienceMultiplier { get; set; }

        public decimal Multiplier { get; set; }

        public decimal? NMHighThreshold { get; set; }
        public decimal? NMLowThreshold { get; set; }
        public decimal? SMHighThreshold { get; set; }
        public decimal? SMLowThreshold { get; set; }
        public decimal? TVHighThreshold { get; set; }
        public decimal? TVLowThreshold { get; set; }
        public decimal? TwitterHighThreshold { get; set; }
        public decimal? TwitterLowThreshold { get; set; }
        public decimal? PQHighThreshold { get; set; }
        public decimal? PQLowThreshold { get; set; }

        public bool IsFliq { get; set; }

        public bool UseProminence { get; set; }

        public bool UseProminenceMediaValue { get; set; }

        public bool ForceCategorySelection { get; set; }

        public int MCMediaPublishedTemplateID { get; set; }

        public int MCMediaDefaultEmailTemplateID { get; set; }

        public int? IQRawMediaExpiration { get; set; }

        public Shared.Utility.CommonFunctions.LibraryTextTypes LibraryTextType { get; set; }

        public int DefaultFeedsPageSize { get; set; }

        public int DefaultDiscoveryPageSize { get; set; }

        public int DefaultArchivePageSize { get; set; }

        public bool ClipEmbedAutoPlay { get; set; }

        public bool DefaultFeedsShowUnread { get; set; }

        public bool UseCustomerEmailDefault { get; set; }
    }

    [Serializable]
    public class RoleModel
    {
        public int RoleKey { get; set; }
        public string RoleName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool IsEnabledInSetup { get; set; }
        public List<string> EnabledCustomerIDs { get; set; }
        public string GroupName { get; set; }
        public bool HasAccess { get; set; }
        public bool HasDefaultAccess { get; set; }
    }

    [Serializable]
    public class StateModel
    {
        public int StateKey { get; set; }
        public string StateName { get; set; }
    }

    [Serializable]
    public class IndustryModel
    {
        public int IndustryKey { get; set; }
        public string IndustryCode { get; set; }
    }

    [Serializable]
    public class BillTypeModel
    {
        public int BillTypeKey { get; set; }
        public string Bill_Type { get; set; }
    }

    [Serializable]
    public class BillFrequencyModel
    {
        public int BillFrequencyKey { get; set; }
        public string Bill_Frequency { get; set; }
    }

    [Serializable]
    public class PricingCodeModel
    {
        public int PricingCodeKey { get; set; }
        public string Pricing_Code { get; set; }
    }

    [Serializable]
    public class MCMediaTemplateModel
    {
        public int TemplateKey { get; set; }
        public string TemplateName { get; set; }
    }

    [Serializable]
    public class Client_DropDown
    {
        public List<IQ_Industry> Client_LRIndustryList { get; set; }

        public List<RoleModel> Client_RoleList { get; set; }

        public List<string> Client_MasterClientList { get; set; }

        public List<StateModel> Client_StateList { get; set; }

        public List<IndustryModel> Client_IndustryList { get; set; }

        public List<BillTypeModel> Client_BillTypeList { get; set; }

        public List<BillFrequencyModel> Client_BillFrequencyList { get; set; }

        public List<PricingCodeModel> Client_PricingCodeList { get; set; }

        public List<ClientModel> Client_MasterList { get; set; }

        public List<MCMediaTemplateModel> Client_MCMediaPublishedTemplateList { get; set; }

        public List<MCMediaTemplateModel> Client_MCMediaEmailTemplateList { get; set; }
    }

    public class ClientPostModel
    {

        public long hdnClientKey { get; set; }

        public long hdnAnewstipClientID { get; set; }

        public string txtClientName { get; set; }

        public int ddlState { get; set; }

        public string txtAddress1 { get; set; }

        public string txtAddress2 { get; set; }

        public string txtCity { get; set; }

        public string txtZip { get; set; }

        public string txtPhone { get; set; }

        public int ddlPricingCode { get; set; }

        public int ddlBillFrequency { get; set; }

        public int ddlBillType { get; set; }

        public int ddlIndustry { get; set; }

        public string txtAttention { get; set; }

        public string txtMasterClient { get; set; }

        public int txtNoOfUsers { get; set; }

        public bool chkIsPlayerLogo { get; set; }

        public long ddlMCID { get; set; }

        public short txtNoOfNotification { get; set; }

        public short txtNoOfIQAgent { get; set; }

        public decimal? txtOtherOnlineAdRate { get; set; }

        public decimal? txtOnlineNewsAdRate { get; set; }

        public decimal? txtCompeteMultiplier { get; set; }

        public decimal? txtURLPercentRead { get; set; }

        public string hfCustomHeaderImage { get; set; }

        public string hfPlayerLogoImage { get; set; }

        public Client_DropDown Client_DropDown { get; set; } 

        public string[] chkRoles { get; set; }

        public string ddlTimeZone { get; set; }

        public string hfNotificationHeaderImage { get; set; }

        public bool chkIsCDNUpload { get; set; }

        public int txtAutoClipDuration { get; set; }

        public List<string> selectedVisibleLRIndustries { get; set; }

        public int? txtv4MaxDiscoveryHistory { get; set; }

        public int? txtv4MaxDiscoveryReportItems { get; set; }

        public int? txtv4MaxDiscoveryExportItems { get; set; }

        public int? txtv4MaxFeedsExportItems { get; set; }

        public int? txtv4MaxFeedsReportItems { get; set; }

        public int? txtv4MaxLibraryEmailReportItems { get; set; }

        public int? txtv4MaxLibraryReportItems { get; set; }

        public decimal txtCompeteAudienceMultiplier { get; set; }

        public decimal txtMultiplier { get; set; }

        public bool? chkIsFliq { get; set; }

        public bool? chkHasPremium { get; set; }

        public decimal? txtNMHighThreshold { get; set; }
        public decimal? txtNMLowThreshold { get; set; }
        public decimal? txtSMHighThreshold { get; set; }
        public decimal? txtSMLowThreshold { get; set; }
        public decimal? txtTVHighThreshold { get; set; }
        public decimal? txtTVLowThreshold { get; set; }
        public decimal? txtTwitterHighThreshold { get; set; }
        public decimal? txtTwitterLowThreshold { get; set; }
        public decimal? txtPQHighThreshold { get; set; }
        public decimal? txtPQLowThreshold { get; set; }

        public bool? chkIsActive { get; set; }

        public bool? chkUseProminence { get; set; }

        public bool? chkUseProminenceMediaValue { get; set; }

        public bool? chkForceCategorySelection { get; set; }

        public int ddlMCMediaPubTemplate { get; set; }

        public int ddlMCMediaEmailTemplate { get; set; }

        public int? txtIQRawMediaExpiration { get; set; }

        public Shared.Utility.CommonFunctions.LibraryTextTypes ddlLibraryTextType { get; set; }

        public int ddlDefaultFeedsPageSize { get; set; }

        public int ddlDefaultDiscoveryPageSize { get; set; }

        public int ddlDefaultArchivePageSize { get; set; }

        public bool chkClipEmbedAutoPlay { get; set; }

        public bool chkDefaultFeedsShowUnread { get; set; }

        public bool chkUseCustomerEmailDefault { get; set; }
    }
}


