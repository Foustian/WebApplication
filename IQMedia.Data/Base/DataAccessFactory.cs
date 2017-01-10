using System.Collections;
using System;
using IQMedia.Data.Base;
using IQMedia.Data;
using IQMedia.Data.MCMediaTemplate;

namespace IQMedia.Logic.Base
{
    public static class DataAccessFactory
    {
        private static readonly Hashtable _DAMap = new Hashtable();

        public static IDataAccess GetDataAccess(DataAccessType daType)
        {
            if (_DAMap[daType] == null)
                _DAMap[daType] = CreateDataAccess(daType);

            return (IDataAccess)_DAMap[daType];
        }

        private static IDataAccess CreateDataAccess(DataAccessType daType)
        {
            switch (daType)
            {
                case DataAccessType.Customer:
                    return new CustomerDA();
                case DataAccessType.Client:
                    return new ClientDA();
                case DataAccessType.IQAgent:
                    return new IQAgentDA();
                case DataAccessType.Utility:
                    return new UtilityDA();
                case DataAccessType.Category:
                    return new CustomCategoryDA();
                case DataAccessType.NM:
                    return new NMDA();
                case DataAccessType.IQArchieve:
                    return new IQArchieveDA();
                case DataAccessType.IQNielsen:
                    return new IQNielsenDA();
                case DataAccessType.IQCompeteAll:
                    return new IQCompeteAllDA();
                case DataAccessType.SM:
                    return new SMDA();
                case DataAccessType.TW:
                    return new TWDA();
                case DataAccessType.TV:
                    return new TVDA();
                case DataAccessType.SavedSearch:
                    return new IQDiscovery_SavedSearchDA();
                case DataAccessType.Report:
                    return new ReportDA();
                case DataAccessType.Radio:
                    return new RadioDA();
                case DataAccessType.IQUGCArchive:
                    return new IQUGCArchiveDA();
                case DataAccessType.UGC:
                    return new UGCDA();
                case DataAccessType.IQNews:
                    return new IQNewsDA();
                case DataAccessType.IQ_SMSCampaign:
                    return new IQ_SMSCampaignDA();
                case DataAccessType.Timeshift_SavedSearch :
                    return new IQTimeshift_SavedSearchDA();
                case DataAccessType.Tads_SavedSearch:
                    return new IQTads_SavedSearchDA();
                case DataAccessType.TVEyes:
                    return new TVEyesDA();
                case DataAccessType.IQClient_CustomImage:
                    return new IQClient_CustomImageDA();
                case DataAccessType.IQTrack_LicenseClick:
                    return new IQTrack_LicenseClickDA();
                case DataAccessType.KantorData:
                    return new IQ_QVCDataDA();
                case DataAccessType.IQReport_Folder:
                    return new IQReport_FolderDA();
                case DataAccessType.fliq_Customer:
                    return new fliq_CustomerDA();
                case DataAccessType.fliq_Application:
                    return new fliq_ApplicationDA();
                case DataAccessType.fliq_CustomerApplication:
                    return new fliq_CustomerApplicationDA();
                case DataAccessType.fliq_ClientApplication:
                    return new fliq_ClientApplicationDA();
                case DataAccessType.IQService_Discovery:
                    return new IQService_DiscoveryDA();
                case DataAccessType.IQNotification:
                    return new IQNotificationSettingsDA();
                case DataAccessType.IQDiscovery_ToFeeds:
                    return new IQDiscovery_ToFeedsDA();
                case DataAccessType.IQTimeSync_Data:
                    return new IQTimeSync_DataDA();
                case DataAccessType.Clip:
                    return new ClipDA();
                case DataAccessType.JobStatus:
                    return new JobStatusDA();
                case DataAccessType.Gallery:
                    return new GalleryDA();
                case DataAccessType.ImagiQ:
                    return new ImagiQDA();
                case DataAccessType.QVCData:
                    return new IQ_QVCDataDA();
                case DataAccessType.PQ:
                    return new PQDA();
                case DataAccessType.MCMediaTemplate2:
                    return new MCMediaTemplate2DA();
                case DataAccessType.MCMediaTemplate3:
                    return new MCMediaTemplate3DA();
                case DataAccessType.MCMediaTemplateDemo:
                    return new MCMediaTemplateDemoDA();
                case DataAccessType.Google:
                    return new GoogleDA();
                case DataAccessType.Facebook:
                    return new FacebookDA();
                case DataAccessType.Session:
                    return new SessionDA();
                case DataAccessType.MCMediaTemplateTrivago:
                    return new MCMediaTemplateTrivagoDA();
                case DataAccessType.Instagram:
                    return new InstagramDA();
                case DataAccessType.IQService_Feeds:
                    return new IQService_FeedsDA();
                case DataAccessType.Analytics:
                    return new AnalyticsDA();
                case DataAccessType.ThirdParty:
                    return new ThirdPartyDA();
                case DataAccessType.DataImport:
                    return new DataImportDA();
                default:
                    //If we get to this point, no logic has bee defined and the code 'SHOULD' fail...
                    throw new ArgumentException("No Logic defined for requested type: '" + daType + "'");
            }
        }
    }

    public enum DataAccessType
    {
        Customer,
        Client,
        IQAgent,
        Utility,
        Category,
        NM,
        IQArchieve,
        IQNielsen,
        IQCompeteAll,
        TW,
        TV,
        SM,
        SavedSearch,
        Report,
        Radio,
        IQUGCArchive,
        UGC,
        IQNews,
        IQ_SMSCampaign,
        Timeshift_SavedSearch,
        Tads_SavedSearch,
        TVEyes,
        IQClient_CustomImage,
        IQTrack_LicenseClick,
        KantorData,
        IQReport_Folder,
        fliq_Customer,
        fliq_Application,
        fliq_CustomerApplication,
        fliq_ClientApplication,
        IQService_Discovery,
        IQNotification,
        IQDiscovery_ToFeeds,
        IQTimeSync_Data,
        Clip,
        JobStatus,
        Gallery,
        ImagiQ,
        QVCData,
        PQ,
        MCMediaTemplate2,
        MCMediaTemplate3,
        MCMediaTemplateDemo,
        Google,
        Facebook,
        Session,
        MCMediaTemplateTrivago,
        Instagram,
        IQService_Feeds,
        Analytics,
        ThirdParty,
        DataImport
    }
}
