using System.Collections;
using System;

namespace IQMedia.Web.Logic.Base
{
    public static class LogicFactory
    {
        private static readonly Hashtable _LogicMap = new Hashtable();

        public static ILogic GetLogic(LogicType logicType)
        {
            if (_LogicMap[logicType] == null)
                _LogicMap[logicType] = CreateLogic(logicType);

            return (ILogic)_LogicMap[logicType];
        }

        private static ILogic CreateLogic(LogicType logicType)
        {
            switch (logicType)
            {
                case LogicType.Customer:
                    return new CustomerLogic();
                case LogicType.Client:
                    return new ClientLogic();
                case LogicType.IQAgent:
                    return new IQAgentLogic();
                case LogicType.Category:
                    return new CustomCategoryLogic();
                case LogicType.NM:
                    return new NMLogic();
                case LogicType.Discovery:
                    return new DiscoveryLogic();
                case LogicType.IQArchieve:
                    return new IQArchieveLogic();
                case LogicType.SM:
                    return new SMLogic();
                case LogicType.TW:
                    return new TWLogic();
                case LogicType.TV:
                    return new TVLogic();
                case LogicType.SavedSearch:
                    return new IQDiscovery_SavedSearchLogic();
                case LogicType.SSP:
                    return new SSPLogic();
                case LogicType.Dashboard:
                    return new DashboardLogic();
                case LogicType.Report:
                    return new ReportLogic();
                //case LogicType.Utility:
                //    return new UtilityLogic();
                case LogicType.Radio:
                    return new RadioLogic();
                case LogicType.IQUGCArchive:
                    return new IQUGCArchiveLogic();
                case LogicType.UGC:
                    return new UGCLogic();
                case LogicType.IQNews:
                    return new IQNewsLogic();
                case LogicType.IQ_SMSCampaign :
                    return new IQ_SMSCampaignLogic();
                case LogicType.Timeshift_SavedSearch :
                    return new IQTimeshift_SavedSearchLogic();
                case LogicType.Tads_SavedSearch:
                    return new IQTads_SavedSearchLogic();
                case LogicType.Player:
                    return new PlayerLogic();
                case LogicType.TVEyes:
                    return new TVEyesLogic();
                case LogicType.IQClient_CustomImage:
                    return new IQClient_CustomImageLogic();
                case LogicType.IQTrack_LicenseClick:
                    return new IQTrack_LicenseClickLogic();
                case LogicType.DiscoveryLite:
                    return new DiscoveryLiteLogic();
                case LogicType.KantorData:
                    return new IQ_QVCDataLogic();
                case LogicType.IQReport_Folder:
                    return new IQReport_FolderLogic();
                case LogicType.fliq_Customer:
                    return new fliq_CustomerLogic();
                case LogicType.fliq_Application:
                    return new fliq_ApplicationLogic();
                case LogicType.fliq_ClientApplication:
                    return new fliq_ClientApplicationLogic();
                case LogicType.fliq_CustomerApplication:
                    return new fliq_CustomerApplicationLogic();
                case LogicType.IQService_Discovery:
                    return new IQService_DiscoveryLogic();
                case LogicType.IQNotification:
                    return new IQNotificationSettingsLogic();
                case LogicType.IQDiscovery_ToFeeds:
                    return new IQDiscovery_ToFeedsLogic();
                case LogicType.IQTimeSync_Data:
                    return new IQTimeSync_DataLogic();
                case LogicType.Clip:
                    return new ClipLogic();
                case LogicType.JobStatus:
                    return new JobStatusLogic();
                case LogicType.Gallery:
                    return new GalleryLogic();
                case LogicType.ImagiQ:
                    return new ImagiQLogic();
                case LogicType.QVCData:
                    return new IQ_QVCDataLogic();
                case LogicType.PQ:
                    return new PQLogic();
                case LogicType.EmailTemplate:
                    return new EmailTemplateLogic();
                case LogicType.MCMediaTemplate:
                    return new MCMediaTemplateLogic();
                case LogicType.Google:
                    return new GoogleLogic();
                case LogicType.Facebook:
                    return new FacebookLogic();
                case LogicType.Instagram:
                    return new InstagramLogic();
                case LogicType.Session:
                    return new SessionLogic();
                case LogicType.Analytics:
                    return new AnalyticsLogic();
                case LogicType.IQService_Feeds:
                    return new IQService_FeedsLogic();
                case LogicType.ThirdParty:
                    return new ThirdPartyLogic();
                case LogicType.DataImport:
                    return new DataImportLogic();
                case LogicType.Cohort:
                    return new CohortLogic();
                default:
                    //If we get to this point, no logic has bee defined and the code 'SHOULD' fail...
                    throw new ArgumentException("No Logic defined for requested type: '" + logicType + "'");
            }
        }
    }

    public enum LogicType
    {
        Customer,
        Client,
        IQAgent,
        Utility,
        Category,
        NM,
        Discovery,
        Discovery2,
        IQArchieve,
        SM,
        TW,
        TV,
        SavedSearch,
        SSP,
        Dashboard,
        Report,
        Radio,
        IQUGCArchive,
        UGC,
        IQNews,
        IQ_SMSCampaign,
        Timeshift_SavedSearch,
        Tads_SavedSearch,
        Player,
        TVEyes,
        IQClient_CustomImage,
        IQTrack_LicenseClick,
        DiscoveryLite,
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
        EmailTemplate,
        MCMediaTemplate,
        Google,
        Facebook,
        Session,       
        Instagram,
        Analytics,
        ThirdParty,
        DataImport,
        IQService_Feeds,
        Cohort
    }
}
