using System;

namespace IQMedia.Model
{
    public interface IEmailTemplate
    {
        EmailResultsModel GetArchiveResultsForEmail(int? masterClientID, Guid? clientGuid, string archiveIDXml, ReportTypeSettings settings, string currentUrl);
    }
}
