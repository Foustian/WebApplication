using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using IQMedia.Logic.Base;
using IQMedia.Data;

namespace IQMedia.Web.Logic
{
    public class fliq_ClientApplicationLogic : IQMedia.Web.Logic.Base.ILogic
    {
        public string Insertfliq_ClientApplication(fliQ_ClientApplicationModel p_ClientApplication)
        {
            fliq_ClientApplicationDA objfliq_ApplicationDA = (fliq_ClientApplicationDA)DataAccessFactory.GetDataAccess(DataAccessType.fliq_ClientApplication);
            return objfliq_ApplicationDA.Insertfliq_ClientApplication(p_ClientApplication);
        }

        public string Updatefliq_ClientApplication(fliQ_ClientApplicationModel p_ClientApplication)
        {
            fliq_ClientApplicationDA objfliq_ApplicationDA = (fliq_ClientApplicationDA)DataAccessFactory.GetDataAccess(DataAccessType.fliq_ClientApplication);
            return objfliq_ApplicationDA.Updatefliq_ClientApplication(p_ClientApplication);
        }

        public string Deletefliq_ClientApplication(Int64 p_ID)
        {
            fliq_ClientApplicationDA objfliq_ApplicationDA = (fliq_ClientApplicationDA)DataAccessFactory.GetDataAccess(DataAccessType.fliq_ClientApplication);
            return objfliq_ApplicationDA.Deletefliq_ClientApplication(p_ID);
        }

        public fliQ_ClientApplicationModel Getliq_ClientApplicationByID(Int64 p_ID)
        {
            fliq_ClientApplicationDA objfliq_ApplicationDA = (fliq_ClientApplicationDA)DataAccessFactory.GetDataAccess(DataAccessType.fliq_ClientApplication);
            return objfliq_ApplicationDA.Getliq_ClientApplicationByID(p_ID);
        }

        public ClientApplication_DropDown GetClientApplication_Dropdowns()
        {
            fliq_ClientApplicationDA objfliq_ApplicationDA = (fliq_ClientApplicationDA)DataAccessFactory.GetDataAccess(DataAccessType.fliq_ClientApplication);
            return objfliq_ApplicationDA.GetClientApplication_Dropdowns();
        }

        public List<fliQ_ClientApplicationModel> GetAllfliq_ClientApplication(string p_ClientName, string p_ApplicationName, int p_PageNumner, int p_PageSize, out int p_TotalResults)
        {
            fliq_ClientApplicationDA objfliq_ApplicationDA = (fliq_ClientApplicationDA)DataAccessFactory.GetDataAccess(DataAccessType.fliq_ClientApplication);
            return objfliq_ApplicationDA.GetAllfliq_ClientApplication(p_ClientName, p_ApplicationName, p_PageNumner, p_PageSize, out p_TotalResults);
        }
    }
}
