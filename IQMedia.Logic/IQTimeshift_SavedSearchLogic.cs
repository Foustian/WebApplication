using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data;
using IQMedia.Logic.Base;
using IQMedia.Model;

namespace IQMedia.Web.Logic
{
    public class IQTimeshift_SavedSearchLogic : IQMedia.Web.Logic.Base.ILogic
    {
        public string InsertTimeshiftSavedSearch(Timeshift_SavedSearchModel timeshift_SavedSearchModel)
        {
            IQTimeshift_SavedSearchDA iQTimeshift_SavedSearchDA = (IQTimeshift_SavedSearchDA)DataAccessFactory.GetDataAccess(DataAccessType.Timeshift_SavedSearch);
            return iQTimeshift_SavedSearchDA.InsertTimeshiftSavedSearch(timeshift_SavedSearchModel);
        }

        public List<Timeshift_SavedSearchModel> SelectTimeshiftSavedSearch(Int32? p_PageNumber, Int32 p_Pagesize, Guid p_CustomerGUID, out Int64 totalRecords)
        {
            IQTimeshift_SavedSearchDA iQTimeshift_SavedSearchDA = (IQTimeshift_SavedSearchDA)DataAccessFactory.GetDataAccess(DataAccessType.Timeshift_SavedSearch);
            return iQTimeshift_SavedSearchDA.SelectTimeshiftSavedSearch(p_PageNumber, p_Pagesize, p_CustomerGUID, out totalRecords);
        }

        public List<Timeshift_SavedSearchModel> SelectTimeshiftSavedSearchByID(Int64 p_ID, Guid p_CustomerGuid)
        {
            IQTimeshift_SavedSearchDA iQTimeshift_SavedSearchDA = (IQTimeshift_SavedSearchDA)DataAccessFactory.GetDataAccess(DataAccessType.Timeshift_SavedSearch);
            return iQTimeshift_SavedSearchDA.SelectTimeshiftSavedSearchByID(p_ID, p_CustomerGuid);
        }

        public string DeleteTimeshiftSavedSearchByID(Int64 p_ID, Guid p_CustomerGUID)
        {
            IQTimeshift_SavedSearchDA iQTimeshift_SavedSearchDA = (IQTimeshift_SavedSearchDA)DataAccessFactory.GetDataAccess(DataAccessType.Timeshift_SavedSearch);
            return iQTimeshift_SavedSearchDA.DeleteTimeshiftSavedSearchByID(p_ID, p_CustomerGUID);
        }

        public string UpdateTimeshiftSavedSearch(Timeshift_SavedSearchModel timeshift_SavedSearchModel)
        {
            IQTimeshift_SavedSearchDA iQTimeshift_SavedSearchDA = (IQTimeshift_SavedSearchDA)DataAccessFactory.GetDataAccess(DataAccessType.Timeshift_SavedSearch);
            return iQTimeshift_SavedSearchDA.UpdateTimeshiftSavedSearch(timeshift_SavedSearchModel);
        }
    }
}
