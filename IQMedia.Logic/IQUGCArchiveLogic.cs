using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Web.Logic.Base;
using IQMedia.Model;
using IQMedia.Data;
using IQMedia.Logic.Base;
using System.Xml.Linq;

namespace IQMedia.Web.Logic
{
    public class IQUGCArchiveLogic : ILogic
    {
        public IQUGCArchiveResult GetIQArchieveResults(string ClientGUID, DateTime? FromDate, DateTime? ToDate, string SearchTerm, List<string> CategoryGuid, string SelectionType, string CustomerGuid,
                                                        string FileType,long FromRecordID, int PageSize, long SinceID,string Sortcolumn, bool IsAsc, bool IsEnableFilter)
        {
            string strcategoryList = null;
            if (CategoryGuid != null && CategoryGuid.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in CategoryGuid
                                             select new XElement("item", new XAttribute("guid", ele))
                                                     ));
                strcategoryList = xdoc.ToString();
            }

            IQUGCArchiveDA iQUGCArchiveDA = (IQUGCArchiveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQUGCArchive);
            return iQUGCArchiveDA.GetIQArchieveResults(ClientGUID, FromDate, ToDate, SearchTerm, strcategoryList, SelectionType, CustomerGuid, FileType, FromRecordID, PageSize, SinceID, Sortcolumn, IsAsc, IsEnableFilter);
        }

        public IQUGCArchiveResult_FilterModel GetIQArchieveFilter(string ClientGUID, DateTime? FromDate, DateTime? ToDate, string SearchTerm, List<string> CategoryGuid, string SelectionType, string CustomerGuid, long SinceID,string FileType)
        {
            string strcategoryList = null;
            if (CategoryGuid != null && CategoryGuid.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in CategoryGuid
                                             select new XElement("item", new XAttribute("guid", ele))
                                                     ));
                strcategoryList = xdoc.ToString();
            }

            IQUGCArchiveDA iQUGCArchiveDA = (IQUGCArchiveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQUGCArchive);
            return iQUGCArchiveDA.GetIQArchieveFilter(ClientGUID, FromDate, ToDate, SearchTerm, strcategoryList, SelectionType, CustomerGuid, SinceID,FileType);
        }

        public List<long> Delete(string ClientGUID, string IQUGCArchiveIDs)
        {
            IQUGCArchiveDA iQUGCArchiveDA = (IQUGCArchiveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQUGCArchive);
            return iQUGCArchiveDA.Delete(ClientGUID, IQUGCArchiveIDs);
        }

        public IQUGCArchiveEditModel SelectForEdit(string ClientGUID, long IQUGCArchiveKey)
        {
            IQUGCArchiveDA iQUGCArchiveDA = (IQUGCArchiveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQUGCArchive);
            return iQUGCArchiveDA.SelectForEdit(ClientGUID, IQUGCArchiveKey);
        }

        public string UpdateIQUGCArchive(string ClientGUID, long IQUGCArchiveKey, string p_Title, string p_Keywords, Guid p_Customer, Guid? p_Category, Guid? p_Subcategory1, Guid? p_Subcategory2, Guid? p_Subcategory3, string p_Description)
        {
            IQUGCArchiveDA iQUGCArchiveDA = (IQUGCArchiveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQUGCArchive);
            return iQUGCArchiveDA.UpdateIQUGCArchive(ClientGUID, IQUGCArchiveKey, p_Title, p_Keywords, p_Customer, p_Category, p_Subcategory1, p_Subcategory2, p_Subcategory3,p_Description);
        }

        public string RefreshResults(string ClientGuid)
        {
            IQUGCArchiveDA iQUGCArchiveDA = (IQUGCArchiveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQUGCArchive);
            return iQUGCArchiveDA.RefreshResults(ClientGuid);
        }

        public IQUGCArchiveModel SelectUGCFileLocationAndName(string ClientGuid,long IQUGCArchiveKey)
        {
            IQUGCArchiveDA iQUGCArchiveDA = (IQUGCArchiveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQUGCArchive);
            return iQUGCArchiveDA.SelectUGCFileLocationAndName(ClientGuid,IQUGCArchiveKey);
        }

        public List<IQUGCArchiveResult_Filter> GetCategoryFilter(string ClientGUID, DateTime? FromDate, DateTime? ToDate, string SearchTerm, List<string> CategoryGuid, string CustomerGuid, long SinceID, string FileType)
        {
            string strcategoryList = null;
            if (CategoryGuid != null && CategoryGuid.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("list",
                                             from ele in CategoryGuid
                                             select new XElement("item", new XAttribute("guid", ele))
                                                     ));
                strcategoryList = xdoc.ToString();
            }

            IQUGCArchiveDA iQUGCArchiveDA = (IQUGCArchiveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQUGCArchive);
            return iQUGCArchiveDA.GetCategoryFilter(ClientGUID, FromDate, ToDate, SearchTerm, strcategoryList,  CustomerGuid, SinceID,FileType);
        }

        public string InsertIQUGCArchiveDocument(Guid CategoryGUID, Guid? SubCategory1GUID, Guid? SubCategory2GUID, Guid? SubCategory3GUID, string Title, string Keywords, string Description, string DocumentDate, string DocumentTimeZone, Guid CustomerGUID, Guid ClientGUID, string FileType, int _RootPathID, string Location)
        {
            IQUGCArchiveDA iQUGCArchiveDA = (IQUGCArchiveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQUGCArchive);
            return iQUGCArchiveDA.InsertIQUGCArchiveDocument(CategoryGUID, SubCategory1GUID, SubCategory2GUID, SubCategory3GUID, Title, Keywords, Description, DocumentDate, DocumentTimeZone, CustomerGUID, ClientGUID, FileType, _RootPathID, Location);
        }

        public void GetUGCDocumentStoragePath(out int p_RootPathID, out string StoragePath)
        {
            IQUGCArchiveDA iQUGCArchiveDA = (IQUGCArchiveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQUGCArchive);
            iQUGCArchiveDA.GetUGCDocumentStoragePath(out p_RootPathID,out StoragePath);
        }

        public Dictionary<string, string> GetUGCFileTypes()
        {
            IQUGCArchiveDA iQUGCArchiveDA = (IQUGCArchiveDA)DataAccessFactory.GetDataAccess(DataAccessType.IQUGCArchive);
            return iQUGCArchiveDA.GetUGCFileTypes();
        }
    }
}
