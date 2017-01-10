using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data;
using IQMedia.Logic.Base;

namespace IQMedia.Web.Logic
{
    public class IQDiscovery_ToFeedsLogic : IQMedia.Web.Logic.Base.ILogic
    {
        public string InsertIQDiscovery_ToFeeds(Guid p_ClientGuid, Guid p_CustomerGuid, string p_ArticleXml, Int64 p_SearchRequestID, Int64? p_ReportID)
        {
            IQDiscovery_ToFeedsDA IQDiscovery_ToFeedsDA = (IQDiscovery_ToFeedsDA)DataAccessFactory.GetDataAccess(DataAccessType.IQDiscovery_ToFeeds);
            return IQDiscovery_ToFeedsDA.InsertIQDiscovery_ToFeeds(p_ClientGuid, p_CustomerGuid, p_ArticleXml, p_SearchRequestID, p_ReportID);
        }
    }
}
