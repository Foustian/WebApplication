using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data;
using IQMedia.Logic.Base;
using IQMedia.Model;

namespace IQMedia.Web.Logic
{
    public class IQTads_SavedSearchLogic : IQMedia.Web.Logic.Base.ILogic
    {
        public string InsertTadsSavedSearch(Tads_SavedSearchModel tads_SavedSearchModel)
        {
            IQTads_SavedSearchDA iQTads_SavedSearchDA = (IQTads_SavedSearchDA)DataAccessFactory.GetDataAccess(DataAccessType.Tads_SavedSearch);
            return iQTads_SavedSearchDA.InsertTadsSavedSearch(tads_SavedSearchModel);
        }

        public List<Tads_SavedSearchModel> SelectTadsSavedSearch(Int32? p_PageNumber, Int32 p_Pagesize, Guid p_CustomerGUID, out Int64 totalRecords)
        {
            IQTads_SavedSearchDA iQTads_SavedSearchDA = (IQTads_SavedSearchDA)DataAccessFactory.GetDataAccess(DataAccessType.Tads_SavedSearch);
            return iQTads_SavedSearchDA.SelectTadsSavedSearch(p_PageNumber, p_Pagesize, p_CustomerGUID, out totalRecords);
        }

        public List<Tads_SavedSearchModel> SelectTadsSavedSearchByID(Int64 p_ID, Guid p_CustomerGuid)
        {
            IQTads_SavedSearchDA iQTads_SavedSearchDA = (IQTads_SavedSearchDA)DataAccessFactory.GetDataAccess(DataAccessType.Tads_SavedSearch);
            return iQTads_SavedSearchDA.SelectTadsSavedSearchByID(p_ID, p_CustomerGuid);
        }

        public string DeleteTadsSavedSearchByID(Int64 p_ID, Guid p_CustomerGUID)
        {
            IQTads_SavedSearchDA iQTads_SavedSearchDA = (IQTads_SavedSearchDA)DataAccessFactory.GetDataAccess(DataAccessType.Tads_SavedSearch);
            return iQTads_SavedSearchDA.DeleteTadsSavedSearchByID(p_ID, p_CustomerGUID);
        }

        public string UpdateTadsSavedSearch(Tads_SavedSearchModel tads_SavedSearchModel)
        {
            IQTads_SavedSearchDA iQTads_SavedSearchDA = (IQTads_SavedSearchDA)DataAccessFactory.GetDataAccess(DataAccessType.Tads_SavedSearch);
            return iQTads_SavedSearchDA.UpdateTadsSavedSearch(tads_SavedSearchModel);
        }
    }
}
