using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data;
using IQMedia.Logic.Base;
using IQMedia.Web.Logic.Base;

namespace IQMedia.Web.Logic
{
    public class IQTrack_LicenseClickLogic : ILogic
    {
        public string Insert(Int64 p_CustomerID, string p_Event, string p_MOUrl, Int16 p_IQLicense)
        {
            IQTrack_LicenseClickDA iQTrack_LicenseClickDA = (IQTrack_LicenseClickDA)DataAccessFactory.GetDataAccess(DataAccessType.IQTrack_LicenseClick);
            return iQTrack_LicenseClickDA.Insert(p_CustomerID, p_Event, p_MOUrl, p_IQLicense);
        }
    }
}
