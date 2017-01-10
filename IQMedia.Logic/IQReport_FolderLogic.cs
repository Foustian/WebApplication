using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using IQMedia.Data;
using IQMedia.Logic.Base;

namespace IQMedia.Web.Logic
{
    public class IQReport_FolderLogic : IQMedia.Web.Logic.Base.ILogic
    {
        public List<IQReport_FolderModel> GetFolderData(Guid clientGuid)
        {
            IQReport_FolderDA iQReport_FolderDA = (IQReport_FolderDA)DataAccessFactory.GetDataAccess(DataAccessType.IQReport_Folder);
            return iQReport_FolderDA.GetFolderData(clientGuid);
        }

        public string CreateFolder(string p_Name, string p_ParentID, Guid clientGuid)
        {
            IQReport_FolderDA iQReport_FolderDA = (IQReport_FolderDA)DataAccessFactory.GetDataAccess(DataAccessType.IQReport_Folder);
            return iQReport_FolderDA.CreateFolder(p_Name, p_ParentID, clientGuid);
        }

        public string RenameFolder(string p_ID, string p_Name,  Guid clientGuid)
        {
            IQReport_FolderDA iQReport_FolderDA = (IQReport_FolderDA)DataAccessFactory.GetDataAccess(DataAccessType.IQReport_Folder);
            return iQReport_FolderDA.RenameFolder(p_Name, p_ID, clientGuid);
        }

        public string MoveFolder(string p_ParentID, string p_ID, Guid clientGuid)
        {
            IQReport_FolderDA iQReport_FolderDA = (IQReport_FolderDA)DataAccessFactory.GetDataAccess(DataAccessType.IQReport_Folder);
            return iQReport_FolderDA.MoveFolder(p_ParentID, p_ID, clientGuid);
        }

        public string DeleteFolder(string p_ID, Guid clientGuid)
        {
            IQReport_FolderDA iQReport_FolderDA = (IQReport_FolderDA)DataAccessFactory.GetDataAccess(DataAccessType.IQReport_Folder);
            return iQReport_FolderDA.DeleteFolder(p_ID,clientGuid);
        }

        public string PasteFolder(string p_CopyID, string p_PasteID, Guid clientGuid)
        {
            IQReport_FolderDA iQReport_FolderDA = (IQReport_FolderDA)DataAccessFactory.GetDataAccess(DataAccessType.IQReport_Folder);
            return iQReport_FolderDA.PasteFolder(p_CopyID, p_PasteID, clientGuid);
        }

        public List<IQReport_FolderModel> GetReportAndReportFolderData(Guid clientGuid)
        {
            IQReport_FolderDA iQReport_FolderDA = (IQReport_FolderDA)DataAccessFactory.GetDataAccess(DataAccessType.IQReport_Folder);
            return iQReport_FolderDA.GetReportAndReportFolderData(clientGuid);
        }

        public string MoveReport(string p_ParentID, string p_ID, Guid clientGuid)
        {
            IQReport_FolderDA iQReport_FolderDA = (IQReport_FolderDA)DataAccessFactory.GetDataAccess(DataAccessType.IQReport_Folder);
            return iQReport_FolderDA.MoveReport(p_ParentID, p_ID, clientGuid);
        }

        public List<IQReport_FolderModel> GetReportFolderByClientGuid(Guid clientGuid)
        {
            IQReport_FolderDA iQReport_FolderDA = (IQReport_FolderDA)DataAccessFactory.GetDataAccess(DataAccessType.IQReport_Folder);
            return iQReport_FolderDA.GetReportFolderByClientGuid(clientGuid);
        }
    }
}


