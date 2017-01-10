using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Web.Logic.Base;
using IQMedia.Logic.Base;
using IQMedia.Data;
using IQMedia.Model;
using System.Xml.Linq;
using IQCommon.Model;

namespace IQMedia.Web.Logic
{
    public class IQArchieveLogic : ILogic
    {
        /// <summary>
        /// Returns rows from table which satisfy criteria and filters associated
        /// </summary>
        /// <param name="ClientGUID"></param>
        /// <param name="CustomerGUID"></param>
        /// <param name="FromRecordID"></param>
        /// <param name="PageSize"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="SubMediaType"></param>
        /// <param name="SearchTerm"></param>
        /// <param name="IsAsc"></param>
        /// <param name="SinceID"></param>
        /// <param name="TotalResults"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetIQAgentMediaResults(string ClientGUID, string CustomerGUID, long FromRecordID, int PageSize, DateTime? FromDate, DateTime? ToDate, string SubMediaType, string SearchTerm,
                                                                List<string> CategoryGUID, string SelectionType, bool IsAsc, string SortColumn, bool IsEnableFilter, string currentUrl, ref long SinceID, out long TotalResults, out long TotalResultsDisplay, bool IsLibraryRollup)
        {
            string strcategoryList = null;
            if (CategoryGUID != null && CategoryGUID.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in CategoryGUID
                                             select new XElement("item", new XAttribute("guid", ele))
                                                     ));
                strcategoryList = xdoc.ToString();
            }

            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            Dictionary<string, object> dictResult = iQArchieveDA.GetIQArchieveResults(ClientGUID, CustomerGUID, FromRecordID, PageSize, FromDate, ToDate, SubMediaType,
                                                                                        SearchTerm, strcategoryList, SelectionType, IsAsc, SortColumn, IsEnableFilter, currentUrl, ref SinceID, out TotalResults, out TotalResultsDisplay, IsLibraryRollup);
            return dictResult;
        }

        public List<IQArchive_MediaModel> GetIQArchieveResultsForEmail(Guid ClientGuid,string ArchiveXML, string currentUrl)
        {
            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            List<IQArchive_MediaModel> lstIQArchive_Media = iQArchieveDA.GetIQArchieveResultsForEmail(ClientGuid,ArchiveXML, currentUrl);
            return lstIQArchive_Media;
        }

        public List<Int64> GetIQArchiveResultsForDashboard(string ClientGUID, string CustomerGUID, DateTime? FromDate, DateTime? ToDate, string SubMediaType, string SearchTerm, List<string> CategoryGUID, string SelectionType, long SinceID, bool IsRadioAccess, bool IsLibraryRollup, bool IsOnlyParents)
        {
            string strcategoryList = null;
            if (CategoryGUID != null && CategoryGUID.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in CategoryGUID
                                             select new XElement("item", new XAttribute("guid", ele))
                                                     ));
                strcategoryList = xdoc.ToString();
            }

            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchieveDA.GetIQArchiveResultsForDashboard(ClientGUID, CustomerGUID, FromDate, ToDate, SubMediaType, SearchTerm, strcategoryList, SelectionType, SinceID, IsRadioAccess, IsLibraryRollup, IsOnlyParents);
        }

        /// <summary>
        /// Returns list of ID which is deleted successfully
        /// </summary>
        /// <param name="ClientGUID">Represent Client who are logged in</param>
        /// <param name="ArchiveIDs">XML formatted multiple ArchiveID</param>
        /// <returns></returns>
        public List<IQArchive_MediaModel> Delete(string ClientGUID, string ArchiveIDs, string currentUrl, out List<long> lstArchiveID, bool IsLibraryRollup)
        {
            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchieveDA.Delete(ClientGUID, ArchiveIDs, currentUrl, out lstArchiveID, IsLibraryRollup);

        }

        public IQArchive_ArchiveClipModel GetArchiveClipByClipID(string ClipID)
        {
            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchieveDA.GetArchiveClipByClipID(ClipID);
        }

        public IQArchive_FilterModel GetIQArchieveFilters(string ClientGUID, string CustomerGUID, DateTime? FromDate, DateTime? ToDate, string SubMediaType, string SearchTerm, List<string> CategoryGUID,string SelectionType, long SinceID, bool IsRadioAccess)
        {
            string strcategoryList = null;
            if (CategoryGUID != null && CategoryGUID.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in CategoryGUID
                                             select new XElement("item", new XAttribute("guid", ele))
                                                     ));
                strcategoryList = xdoc.ToString();
            }

            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchieveDA.GetIQArchieveFilters(ClientGUID, CustomerGUID, FromDate, ToDate, SubMediaType, SearchTerm, strcategoryList, SelectionType, SinceID, IsRadioAccess);
        }

        public IQArchive_EditModel GetIQArchiveByIDForEdit(string ClientGuid, long ID)
        {
            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchieveDA.GetIQArchiveByIDForEdit(ClientGuid, ID);
        }

        public IQArchive_MediaModel GetIQArchiveByIDForView(long ID)
        {
            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchieveDA.GetIQArchiveByIDForView(ID);
        }

        public string Update_MediaRecord(long ID, string Title, Guid? CategoryGuid, Guid? SubCategory1Guid, Guid? SubCategory2Guid,
                                                    Guid? SubCategory3Guid, string Keywords, string Description, bool? DisplayDescription, short PositiveSentiment, short NegativeSentiment, Guid ClientGuid)
        {
            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchieveDA.Update_MediaRecord(ID, Title, CategoryGuid, SubCategory1Guid, SubCategory2Guid, SubCategory3Guid, Keywords, Description, DisplayDescription, PositiveSentiment, NegativeSentiment, ClientGuid);
        }

        public List<IQArchive_RefreshResultsForTV> GetRefreshResultsForTV(Guid ClientGuid)
        {
            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchieveDA.GetRefreshResultsForTV(ClientGuid);
        }

        public string Update_ArchiveClipClosedCaption(long ArchiveClipKey, string CC)
        {
            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchieveDA.Update_ArchiveClipClosedCaption(ArchiveClipKey, CC);
        }

        public string Insert_IQ_Report(string ReportTitle, string ReportRule, Int64? p_ReportImage, string ClientGuid, Int64 p_FolderID)
        {
            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchieveDA.Insert_IQ_Report(ReportTitle, ReportRule, p_ReportImage, ClientGuid, p_FolderID);
        }

        public List<IQ_ReportModel> GetLibraryIQ_ReportByClient(string ClientGuid)
        {
            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchieveDA.GetLibraryIQ_ReportByClient(ClientGuid);
        }

        public IQArchive_DisplayLibraryReport GetIQArchieveResultsForLibraryReport(long ReportID, string currentUrl, Guid ClientGuid, bool isNielsenData, bool isCompeteData, List<IQ_MediaTypeModel> lstSubMediaTypes)
        {
            int totalDisplayedRecords;

            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            IQArchive_DisplayLibraryReport objDisplayLibraryReport = iQArchieveDA.GetIQArchieveResultsForLibraryReport(ReportID, currentUrl, ClientGuid, isNielsenData, isCompeteData, lstSubMediaTypes, out totalDisplayedRecords);

            // A scenario exists where, after applying custom sorting to a report, some records may no longer be displayed. Log instances of this problem for debugging purposes.
            if (objDisplayLibraryReport != null && objDisplayLibraryReport.ArchiveResults != null && objDisplayLibraryReport.ArchiveResults.Count != totalDisplayedRecords)
            {
                UtilityLogic.WriteException(new Exception(String.Format("Encountered missing records in custom-sorted report. Report ID: {0}  Total Results: {1}  Displayed Results: {2}", ReportID, objDisplayLibraryReport.ArchiveResults.Count, totalDisplayedRecords)));
            }

            return objDisplayLibraryReport;
        }

        public IQClient_UGCMapModel GetUGCMapByClientGUID(string ClientGuid)
        {
            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            IQClient_UGCMapModel objIQClient_UGCMapModel = iQArchieveDA.GetUGCMapByClientGUID(ClientGuid);
            return objIQClient_UGCMapModel;
        }

        public int AppendItemsIQReport(Guid ClientGuid, long ReportID, string ReportXML)
        {
            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchieveDA.AppendItemsIQReport(ClientGuid, ReportID, ReportXML);
        }

        public int RemoveItemsIQReport(Guid ClientGuid, long ReportID, string ReportXML)
        {
            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchieveDA.RemoveItemsIQReport(ClientGuid, ReportID, ReportXML);
        }

        public int RemoveIQReportByReportID(long ReportID, string ClientGUID)
        {
            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchieveDA.RemoveIQReportByReportID(ReportID, ClientGUID);
        }

        public List<IFrameMicrositeModel> GetArchiveClipByParams(Guid p_ClientGUID, string p_ListCategoryGUID, string p_ListSubCategory1GUID, string p_ListSubCategory2GUID, string p_ListSubCategory3GUID, string p_ListCustomerGUID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, string p_SearchText, string p_ClipTitle, out Guid? p_ClipID, out Int64 p_TotalRecordsCount, string currentURLHost)
        {
            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchieveDA.GetArchiveClipByParams(p_ClientGUID, p_ListCategoryGUID, p_ListSubCategory1GUID, p_ListSubCategory2GUID, p_ListSubCategory3GUID, p_ListCustomerGUID, p_PageNumber, p_PageSize, p_SortField, p_IsAscending, p_SearchText, p_ClipTitle, out  p_ClipID, out p_TotalRecordsCount, currentURLHost);
        }

        public Dictionary<string, string> GetClipPathByClipGUID(string p_ClipGUID, Guid p_ClientGUID)
        {
            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchieveDA.GetClipPathByClipGUID(p_ClipGUID, p_ClientGUID);
        }

        public string UpdateDownloadCountByClipGUID(string p_ClipGUID)
        {
            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchieveDA.UpdateDownloadCountByClipGUID(p_ClipGUID);
        }

        public void GetArchiveTVeyesLocationByArchiveTVEyesKey(Int64 p_ID, out string p_TranscriptFileLocation, out string p_AudioFileLocation)
        {
            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            iQArchieveDA.GetArchiveTVeyesLocationByArchiveTVEyesKey(p_ID, out p_TranscriptFileLocation, out p_AudioFileLocation);
        }

        public List<IQArchive_Filter> GetCategoryFilter(string ClientGUID, string CustomerGUID, DateTime? FromDate, DateTime? ToDate, string SubMediaType, string SearchTerm, List<string> CategoryGUID, long SinceID)
        {
            string strcategoryList = null;
            if (CategoryGUID != null && CategoryGUID.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in CategoryGUID
                                             select new XElement("item", new XAttribute("guid", ele))
                                                     ));
                strcategoryList = xdoc.ToString();
            }

            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchieveDA.GetCategoryFilter(ClientGUID, CustomerGUID, FromDate, ToDate, SubMediaType, SearchTerm, strcategoryList, SinceID);
        }

        public void GetArchiveTVeyesLocationByMediaID(Int64 p_ID, out string p_TranscriptFileLocation, out string p_AudioFileLocation)
        {
            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            iQArchieveDA.GetArchiveTVeyesLocationByMediaID(p_ID, out p_TranscriptFileLocation, out p_AudioFileLocation);
        }

        public string MergeReports(string reportTitle, List<string> reportIDs, Int64? reportImage, string clientGuid, Int64 folderID)
        {
            string strReportIDs = null;
            if (reportIDs != null && reportIDs.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in reportIDs
                                             select new XElement("item", new XAttribute("id", ele))
                                                     ));
                strReportIDs = xdoc.ToString();
            }

            IQArchieveDA iQArchiveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchiveDA.MergeReports(reportTitle, strReportIDs, reportImage, clientGuid, folderID);
        }

        public string GetMergedReportItemCount(List<string> reportIDs)
        {
            string strReportIDs = null;
            if (reportIDs != null && reportIDs.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in reportIDs
                                             select new XElement("item", new XAttribute("id", ele))
                                                     ));
                strReportIDs = xdoc.ToString();
            }

            IQArchieveDA iQArchiveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchiveDA.GetMergedReportItemCount(strReportIDs);
        }

        public string GetClipTitleByClipID(string p_ClipID)
        {
            return ((IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve)).GetClipTitleByClipID(p_ClipID);
        }

        #region MCMedia

        public int AddToMCMedia(int masterClientID, List<string> mediaIDs, bool isMediaRoomEditor)
        {
            string strMediaIDs = null;
            if (mediaIDs != null && mediaIDs.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("MediaResults",
                                             from ele in mediaIDs
                                             select new XElement("ID", ele)
                                                     ));
                strMediaIDs = xdoc.ToString();
            }

            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchieveDA.AddToMCMedia(masterClientID, strMediaIDs, isMediaRoomEditor);
        }

        public int RemoveFromMCMedia(int masterClientID, List<string> mediaIDs)
        {
            string strMediaIDs = null;
            if (mediaIDs != null && mediaIDs.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in mediaIDs
                                             select new XElement("item", new XAttribute("id", ele))
                                                     ));
                strMediaIDs = xdoc.ToString();
            }

            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchieveDA.RemoveFromMCMedia(masterClientID, strMediaIDs);
        }

        public List<IQArchive_Filter> GetMCMediaCategoryFilter(string ClientGUID, string CustomerGUID, DateTime? FromDate, DateTime? ToDate, string SubMediaType, string SearchTerm, List<string> CategoryGUID, long SinceID, int? MasterClientID, string reportGUID)
        {
            string strcategoryList = null;
            if (CategoryGUID != null && CategoryGUID.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in CategoryGUID
                                             select new XElement("item", new XAttribute("guid", ele))
                                                     ));
                strcategoryList = xdoc.ToString();
            }

            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchieveDA.GetMCMediaCategoryFilter(ClientGUID, CustomerGUID, FromDate, ToDate, SubMediaType, SearchTerm, strcategoryList, SinceID, MasterClientID, reportGUID);
        }

        public Dictionary<string, object> GetMCMediaResults(string ClientGUID, string CustomerGUID, long FromRecordID, int PageSize, DateTime? FromDate, DateTime? ToDate, string SubMediaType, string SearchTerm,
                                                                List<string> CategoryGUID, string SelectionType, bool IsAsc, string SortColumn, bool IsEnableFilter, string currentUrl, ref long SinceID, out long TotalResults, int? MasterClientID)
        {
            string strcategoryList = null;
            if (CategoryGUID != null && CategoryGUID.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in CategoryGUID
                                             select new XElement("item", new XAttribute("guid", ele))
                                                     ));
                strcategoryList = xdoc.ToString();
            }

            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            Dictionary<string, object> dictResult = iQArchieveDA.GetMCMediaResults(ClientGUID, CustomerGUID, FromRecordID, PageSize, FromDate, ToDate, SubMediaType,
                                                                                        SearchTerm, strcategoryList, SelectionType, IsAsc, SortColumn, IsEnableFilter, currentUrl, ref SinceID, out TotalResults, MasterClientID);
            return dictResult;
        }

        public Guid? AddToMCMediaReport(Guid? reportGUID, int masterClientID, int reportTypeID, List<string> mediaIDs)
        {
            string strMediaIDs = null;
            if (mediaIDs != null && mediaIDs.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("MediaResults",
                                             from ele in mediaIDs
                                             select new XElement("ID", ele)
                                                     ));
                strMediaIDs = xdoc.ToString();
            }

            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchieveDA.AddToMCMediaReport(reportGUID, masterClientID, reportTypeID, strMediaIDs);
        }

        public void RemoveFromMCMediaReport(Guid reportGUID, List<string> mediaIDs)
        {
            string strMediaIDs = null;
            if (mediaIDs != null && mediaIDs.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("MediaResults",
                                             from ele in mediaIDs
                                             select new XElement("ID", ele)
                                                     ));
                strMediaIDs = xdoc.ToString();
            }

            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            iQArchieveDA.RemoveFromMCMediaReport(reportGUID, strMediaIDs);
        }

        public Guid? GetMCMediaReportGUID(int masterClientID)
        {
            IQArchieveDA iQArchieveDA = (IQArchieveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQArchieve);
            return iQArchieveDA.GetMCMediaReportGUID(masterClientID);
        }

        #endregion
    }
}
