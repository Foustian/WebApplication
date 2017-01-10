using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data;
using IQMedia.Logic.Base;
using IQMedia.Model;

namespace IQMedia.Web.Logic
{
    public class IQService_DiscoveryLogic : IQMedia.Web.Logic.Base.ILogic
    {
        public string InsertExportDiscovery(Guid p_CustomerGuid, Boolean p_IsSelectAll, string p_SearchCriteria, string p_ArticleXml)
        {
            IQService_DiscoveryDA iQService_DiscoveryDA = (IQService_DiscoveryDA)DataAccessFactory.GetDataAccess(DataAccessType.IQService_Discovery);
            return iQService_DiscoveryDA.InsertExportDiscovery(p_CustomerGuid, p_IsSelectAll, p_SearchCriteria, p_ArticleXml);
        }

        public List<IQService_DiscoveryExportModel> GetLastDiscoveryExportDetails(Guid p_CustomerGuid)
        {
            IQService_DiscoveryDA iQService_DiscoveryDA = (IQService_DiscoveryDA)DataAccessFactory.GetDataAccess(DataAccessType.IQService_Discovery);
            return iQService_DiscoveryDA.GetLastDiscoveryExportDetails(p_CustomerGuid);
        }
    }
}
