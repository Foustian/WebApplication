using System;

namespace IQMedia.Model
{
    public interface IMCMediaTemplate
    {
        MCMediaReportModel GetMCMediaResultsForReport(Guid reportGuid, MCMediaSearchModel searchSettings, ReportTypeSettings templateSettings, string currentUrl);
    }
}
