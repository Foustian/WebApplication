using System;
using IQMedia.Data.EmailTemplate;
using IQMedia.Model;
using IQMedia.Shared.Utility;
using IQMedia.Web.Logic.Base;

namespace IQMedia.Web.Logic
{
    public class EmailTemplateLogic : ILogic, IEmailTemplate
    {
        public EmailResultsModel GetArchiveResultsForEmail(int? masterClientID, Guid? clientGuid, string archiveIDXml, ReportTypeSettings settings, string currentUrl)
        {
            // Dynamically determine which EmailTemplateDA class to call based on IQ_ReportType settings
            IEmailTemplate emailTemplateDA = (IEmailTemplate)EmailTemplateFactory.GetEmailTemplate(CommonFunctions.StringToEnum<EmailTemplateType>(settings.TemplateType));
            return emailTemplateDA.GetArchiveResultsForEmail(masterClientID, clientGuid, archiveIDXml, settings, currentUrl);
        }
    }
}
