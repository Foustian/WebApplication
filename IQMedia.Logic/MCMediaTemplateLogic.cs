using System;
using IQMedia.Data.MCMediaTemplate;
using IQMedia.Model;
using IQMedia.Shared.Utility;
using System.Collections.Generic;
using System.Xml.Linq;
using IQMedia.Logic.Base;
using System.Linq;

namespace IQMedia.Web.Logic.Base
{
    public class MCMediaTemplateLogic : ILogic, IMCMediaTemplate
    {
        public MCMediaReportModel GetMCMediaResultsForReport(Guid reportGuid, MCMediaSearchModel searchSettings, ReportTypeSettings templateSettings, string currentUrl)
        {
            // Dynamically determine which MCMediaTemplateDA class to call based on IQ_ReportType settings
            IMCMediaTemplate mcMediaTemplateDA = (IMCMediaTemplate)MCMediaTemplateFactory.GetMCMediaTemplate(CommonFunctions.StringToEnum<MCMediaTemplateType>(templateSettings.TemplateType));
            return mcMediaTemplateDA.GetMCMediaResultsForReport(reportGuid, searchSettings, templateSettings, currentUrl);
        }

        public List<IQArchive_Filter> MCMediaTemplate2_FilterCategory(string ClientGUID, DateTime? FromDate, DateTime? ToDate, string SubMediaType, string SearchTerm, List<string> CategoryNames, string reportGUID)
        {
            string strcategoryList = null;
            if (CategoryNames != null && CategoryNames.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in CategoryNames
                                             select new XElement("item", new XAttribute("name", ele))
                                                     ));
                strcategoryList = xdoc.ToString();
            }

            MCMediaTemplate2DA mcMediaTemplate2DA = (MCMediaTemplate2DA)DataAccessFactory.GetDataAccess(DataAccessType.MCMediaTemplate2);
            return mcMediaTemplate2DA.GetFilterCategory(ClientGUID, FromDate, ToDate, SubMediaType, SearchTerm, strcategoryList, reportGUID);
        }

        public List<IQArchive_Filter> MCMediaTemplate3_FilterCategory(string SubMediaType, string SearchTerm, List<string> CategoryNames, string reportGUID)
        {
            string strcategoryList = null;
            if (CategoryNames != null && CategoryNames.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in CategoryNames
                                             select new XElement("item", new XAttribute("name", ele))
                                                     ));
                strcategoryList = xdoc.ToString();
            }

            MCMediaTemplate3DA mcMediaTemplate3DA = (MCMediaTemplate3DA)DataAccessFactory.GetDataAccess(DataAccessType.MCMediaTemplate3);
            return mcMediaTemplate3DA.GetFilterCategory(SubMediaType, SearchTerm, strcategoryList, reportGUID);
        }

        public List<IQArchive_Filter> MCMediaTemplateTrivago_FilterCategory(DateTime? FromDate, DateTime? ToDate, string SubMediaType, string SearchTerm, int? SentimentFlag, List<string> CategoryNames, string reportGUID)
        {
            string strcategoryList = null;
            if (CategoryNames != null && CategoryNames.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in CategoryNames
                                             select new XElement("item", new XAttribute("name", ele))
                                                     ));
                strcategoryList = xdoc.ToString();
            }

            MCMediaTemplateTrivagoDA mcMediaTemplateTrivagoDA = (MCMediaTemplateTrivagoDA)DataAccessFactory.GetDataAccess(DataAccessType.MCMediaTemplateTrivago);
            return mcMediaTemplateTrivagoDA.GetFilterCategory(FromDate, ToDate, SubMediaType, SearchTerm, SentimentFlag, strcategoryList, reportGUID);
        }
    }
}
