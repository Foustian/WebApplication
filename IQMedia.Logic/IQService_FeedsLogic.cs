using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using IQMedia.Data;
using IQMedia.Logic.Base;
using IQMedia.Web.Logic.Base;

namespace IQMedia.Web.Logic
{
    public class IQService_FeedsLogic : ILogic
    {
        public int InsertFeedsExport(Guid p_CustomerGuid, Boolean p_IsSelectAll, string p_SortType, string p_SearchCriteria, List<string> p_lstMediaIDs, string p_Title, bool p_GetTVUrl)
        {
            string strMediaIDs = null;
            if (p_lstMediaIDs != null && p_lstMediaIDs.Count > 0)
            {
                XDocument xdoc = new XDocument(new XElement("MediaIDs",
                                             from ele in p_lstMediaIDs
                                             select new XElement("ID", ele)
                                                     ));
                strMediaIDs = xdoc.ToString();
            }

            IQService_FeedsDA iQService_FeedsDA = (IQService_FeedsDA)DataAccessFactory.GetDataAccess(DataAccessType.IQService_Feeds);
            return iQService_FeedsDA.InsertFeedsExport(p_CustomerGuid, p_IsSelectAll, p_SortType, p_SearchCriteria, strMediaIDs, p_Title, p_GetTVUrl);
        }
    }
}
