using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQMedia.WebApplication.Config.Sections
{
    public class MessageSettings
    {
        public string DiscoveryMessage { get; set; }
        public string DiscoveryNoDataAvailable { get; set; }
        public string FileNotAvailableForDownload { get; set; }
        public string ErrorOnDownloadFile { get; set; }
        public string ClipNotAvailableForDownload { get; set; }
        public string ErrorOnDownloadClip { get; set; }
        public string ExportClipMsg { get; set; }
        public string ArticleNotSaved { get; set; }
        public string ReportNotSaved { get; set; }
        public string ArticleAlreadySaved { get; set; }
        public string ErrorOccurred { get; set; }
        public string ArticleSaved { get; set; }
        public string MissingArticleSaved { get; set; }
        public string SearchSaved { get; set; }
        public string SearchUpdated { get; set; }
        public string ReportWithSameNameExists { get; set; }
        public string SearchWithSameNameExists { get; set; }
        public string SearchWithSameFilterExists { get; set; }
        public string ReportSaved { get; set; }
        public string RecordDeleted { get; set; }
        public string RecordNotDeleted { get; set; }
        public string DescriptionPage { get; set; }
        public string ContactPage { get; set; }
        public string MissingParameter { get; set; }
        public string FileNotAvailable { get; set; }
        public string ErrorWhileMovingFtp { get; set; }
        public string Success { get; set; }
        public string ErrorSavingFile { get; set; }
        public string UnauthorizedAccess { get; set; }
        public string Error { get; set; }
        public string FileNotFound { get; set; }
        public string AlreadyLoggedIn { get; set; }
        public string CredentialNotCorrect { get; set; }
        public string NoResultFound { get; set; }

        public string MaxLibraryReportItemsMsg { get; set; }
        public string MaxLibraryEmailReportItemsMsg { get; set; }
        public string UGCFileNotAvailableForDownload { get; set; }
        public string UGCFileDownloadError { get; set; }
        public string UGCInvalidFileUploadError { get; set; }

        public string IAgentQueryNameExist { get; set; }
        public string IAgentQueryNameInserted { get; set; }
        public string IAgentQueryNameUpdted { get; set; }

        public string IQNotificationExist { get; set; }
        public string IQNotificationSaved { get; set; }
        public string IQNotificationMaxLimitExceeds { get; set; }
        public string IQAgentMaxLimitExceeds { get; set; }
        public string IQAgentDeleteMsg { get; set; }
        public string AgentSuspendMsg { get; set; }
        public string IQAgentDeleteErrorMsg { get; set; }
        public string SubscriptionSuccess { get; set; }
        public string UnSubscriptionSuccess { get; set; }
        public string SubscriptionFailed { get; set; }
        public string UnSubscriptionFailed { get; set; }
        public string SubscriptionInvalidID { get; set; }
        public string SubscriptionInvalidAction { get; set; }

        public string ReportGenerationProgress { get; set; }
        public string ClipUnableToEnqueForDownload { get; set; }

        public string MicrositeError { get; set; }

        public string LoginFailed { get; set; }
        public string DeleteFailed { get; set; }
        public string SetDefaultFailed { get; set; }
        public string SetDefaultEmailFailed { get; set; }
        public string MaintenanceMessage { get; set; }
        public string PageLoadErrorMessage { get; set; }
        public string InvalidSearchRequestForArticle { get; set; }
        public string NRTrackingInsertFailed { get; set; }
        public string NRTrackingExceptionMessage { get; set; }
        public string SelectOneMoreItemMessage { get; set; }
        public string MaxEmailAdressLimitExceeds { get; set; }
        public string SourceAlreadyExist { get; set; }
        public string ClientUGCMapSettingsAlreadyExist { get; set; }
        public string ContentShareMessage { get; set; }
        public string InfoAlertMessage { get; set; }
        public string InfoAlertHeader { get; set; }

        public string ChPwdRequired { get; set; }
        public string ChPwdCnfmPwdNotMtch { get; set; }
        public string ChPwdCriteria { get; set; }
        public string ChPwdSmOldPwd { get; set; }
        public string ChPwdSuccess { get; set; }
        public string ChPwdWrongPwd { get; set; }

        public string RsetPwdLinkExpired { get; set; }
        public string RsetPwdRequired { get; set; }
        public string RsetPwdInvalidLoginID { get; set; }
        public string RsetPwdCnfmPwdNotMtch { get; set; }
        public string RsetPwdCriteria { get; set; }
        public string RsetPwdSuccess { get; set; }
        public string RsetPwdInvalidDetail { get; set; }
        public string RsetPwdEmailSent { get; set; }
        public string RsetPwdEmailError { get; set; }
        public string RsetPwdEmailLimit { get; set; }
        public string RsetPwdCaptchaEmpty { get; set; }
        public string RsetPwdInvalidCaptcha { get; set; }
        public string RsetPwdError { get; set; }

        public string ErrorUnhandled { get; set; }
    }
}