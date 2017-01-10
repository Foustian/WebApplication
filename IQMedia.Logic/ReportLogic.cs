using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data;
using IQMedia.Logic.Base;
using IQMedia.Model;
using System.Xml.Linq;

namespace IQMedia.Web.Logic
{
    public class ReportLogic : IQMedia.Web.Logic.Base.ILogic
    {
        public string InsertFeedsReport(IQFeeds_ReportModel iQFeeds_ReportModel)
        {
            ReportDA reportDA = (ReportDA)DataAccessFactory.GetDataAccess(DataAccessType.Report);
            return reportDA.InsertFeedsReport(iQFeeds_ReportModel);
        }

        public string InsertDiscoveryReport(IQDiscovery_ReportModel iQDiscovery_ReportModel)
        {
            ReportDA reportDA = (ReportDA)DataAccessFactory.GetDataAccess(DataAccessType.Report);
            return reportDA.InsertDiscoveryReport(iQDiscovery_ReportModel);
        }

        public List<IQFeeds_ReportModel> SelectFeedsReport(Guid p_ClientGUID)
        {
            ReportDA reportDA = (ReportDA)DataAccessFactory.GetDataAccess(DataAccessType.Report);
            return reportDA.SelectFeedsReport(p_ClientGUID);
        }

        public string SelectMediaIDByID(Guid p_ClientGUID, Int64 p_ReportID)
        {
            ReportDA reportDA = (ReportDA)DataAccessFactory.GetDataAccess(DataAccessType.Report);
            return reportDA.SelectMediaIDByID(p_ClientGUID, p_ReportID);
        }

        public string IQReportFeeds_Update(string p_MediaID, Int64 p_ReportID, Guid ClientGuid, Guid CustomerGuid)
        {
            ReportDA reportDA = (ReportDA)DataAccessFactory.GetDataAccess(DataAccessType.Report);
            return reportDA.IQReportFeeds_Update(p_MediaID, p_ReportID, ClientGuid, CustomerGuid);
        }


        public string SelectDiscoveryMediaIDByID(Guid p_ClientGUID, Int64 p_ReportID)
        {
            ReportDA reportDA = (ReportDA)DataAccessFactory.GetDataAccess(DataAccessType.Report);
            return reportDA.SelectDiscoveryMediaIDByID(p_ClientGUID, p_ReportID);
        }

        public string IQReportDiscovery_Update(string p_MediaID, Int64 p_ReportID, Guid ClientGuid, Guid CustomerGuid)
        {
            ReportDA reportDA = (ReportDA)DataAccessFactory.GetDataAccess(DataAccessType.Report);
            return reportDA.IQReportDiscovery_Update(p_MediaID, p_ReportID, ClientGuid, CustomerGuid);
        }

        public List<IQDiscovery_ReportModel> SelectDiscoveryReport(Guid p_ClientGUID)
        {
            ReportDA reportDA = (ReportDA)DataAccessFactory.GetDataAccess(DataAccessType.Report);
            return reportDA.SelectDiscoveryReport(p_ClientGUID);
        }

        public string InsertFeedsLibrary(IQFeeds_ReportModel iQFeeds_ReportModel)
        {
            ReportDA reportDA = (ReportDA)DataAccessFactory.GetDataAccess(DataAccessType.Report);
            return reportDA.InsertFeedsLibrary(iQFeeds_ReportModel);
        }

        public string InsertDiscoveryLibrary(IQDiscovery_ReportModel iQDiscovery_ReportModel)
        {
            ReportDA reportDA = (ReportDA)DataAccessFactory.GetDataAccess(DataAccessType.Report);
            return reportDA.InsertDiscoveryLibrary(iQDiscovery_ReportModel);
        }

        public IQReport_DetailModel IQReportCheckForLimitnData(Guid p_ClientGuid, Int64 p_ReportID)
        {
            ReportDA reportDA = (ReportDA)DataAccessFactory.GetDataAccess(DataAccessType.Report);
            return reportDA.IQReportCheckForLimitnData(p_ClientGuid, p_ReportID);
        }

        public string SaveReportWithSettings(Guid p_ClientGUID, Int64 p_ReportID, IQ_ReportSettingsModel p_ReportSettingXml,Int64? p_ReportImageID, bool p_IsSaveAs, string p_ReportTile, bool p_ResetSort)
        {
            string strReportSettingXml = IQMedia.Shared.Utility.CommonFunctions.SerializeToXml(p_ReportSettingXml);
            ReportDA reportDA = (ReportDA)DataAccessFactory.GetDataAccess(DataAccessType.Report);
            return reportDA.SaveReportWithSettings(p_ClientGUID, p_ReportID, strReportSettingXml, p_ReportImageID, p_IsSaveAs, p_ReportTile, p_ResetSort);
        }

        public List<IQ_ReportTypeModel> GetReportTypes(string masterReportType)
        {
            ReportDA reportDA = (ReportDA)DataAccessFactory.GetDataAccess(DataAccessType.Report);
            return reportDA.GetReportTypes(masterReportType);
        }

        public IQ_ReportTypeModel GetReportTypeByReportGuid(Guid reportGuid)
        {
            ReportDA reportDA = (ReportDA)DataAccessFactory.GetDataAccess(DataAccessType.Report);
            return reportDA.GetReportTypeByReportGuid(reportGuid);
        }

        public int SaveReportItemPositions(long reportID, string reportItemXml)
        {
            ReportDA reportDA = (ReportDA)DataAccessFactory.GetDataAccess(DataAccessType.Report);
            return reportDA.SaveReportItemPositions(reportID, reportItemXml);
        }

        public int RevertReportItemPositions(long reportID)
        {
            ReportDA reportDA = (ReportDA)DataAccessFactory.GetDataAccess(DataAccessType.Report);
            return reportDA.RevertReportItemPositions(reportID);
        }

        public string InsertReportPDFExport(long reportID, Guid customerGuid, string baseUrl, string filename)
        {
            ReportDA reportDA = (ReportDA)DataAccessFactory.GetDataAccess(DataAccessType.Report);
            return reportDA.InsertReportPDFExport(reportID, customerGuid, baseUrl, filename);
        }

        public bool CheckReportPDFExportExists(long reportID)
        {
            ReportDA reportDA = (ReportDA)DataAccessFactory.GetDataAccess(DataAccessType.Report);
            return reportDA.CheckReportPDFExportExists(reportID);
        }
    }

}
