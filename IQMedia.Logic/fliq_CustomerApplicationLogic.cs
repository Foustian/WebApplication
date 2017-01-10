using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using IQMedia.Data;
using IQMedia.Logic.Base;

namespace IQMedia.Web.Logic
{
    public class fliq_CustomerApplicationLogic : IQMedia.Web.Logic.Base.ILogic
    {
        public string Insertfliq_CustomerApplication(fliQ_CustomerApplicationModel p_CustomerApplication)
        {
            fliq_CustomerApplicationDA objfliq_CustomerApplicationDA = (fliq_CustomerApplicationDA)DataAccessFactory.GetDataAccess(DataAccessType.fliq_CustomerApplication);
            return objfliq_CustomerApplicationDA.Insertfliq_CustomerApplication(p_CustomerApplication);
        }

        public string Updatefliq_CustomerApplication(fliQ_CustomerApplicationModel p_CustomerApplication)
        {
            fliq_CustomerApplicationDA objfliq_CustomerApplicationDA = (fliq_CustomerApplicationDA)DataAccessFactory.GetDataAccess(DataAccessType.fliq_CustomerApplication);
            return objfliq_CustomerApplicationDA.Updatefliq_CustomerApplication(p_CustomerApplication);
        }

        public string Deletefliq_CustomerApplication(Int64 p_ID)
        {
            fliq_CustomerApplicationDA objfliq_CustomerApplicationDA = (fliq_CustomerApplicationDA)DataAccessFactory.GetDataAccess(DataAccessType.fliq_CustomerApplication);
            return objfliq_CustomerApplicationDA.Deletefliq_CustomerApplication(p_ID);
        }

        public fliQ_CustomerApplicationModel Getfliq_CustomerApplicationByID(Int64 p_ID)
        {
            fliq_CustomerApplicationDA objfliq_CustomerApplicationDA = (fliq_CustomerApplicationDA)DataAccessFactory.GetDataAccess(DataAccessType.fliq_CustomerApplication);
            return objfliq_CustomerApplicationDA.Getfliq_CustomerApplicationByID(p_ID);
        }

        public CustomerApplication_DropDown GetClientApplication_Dropdowns(bool p_isFetchClient, Int64? p_ClientID)
        {
            fliq_CustomerApplicationDA objfliq_CustomerApplicationDA = (fliq_CustomerApplicationDA)DataAccessFactory.GetDataAccess(DataAccessType.fliq_CustomerApplication);
            return objfliq_CustomerApplicationDA.GetClientApplication_Dropdowns(p_isFetchClient, p_ClientID);
        }

        public List<fliQ_CustomerApplicationModel> GetAllfliq_CustomerApplication(string p_ClientName, string p_CustomerName, int p_PageNumner, int p_PageSize, bool p_IsAsc, string p_SortColumn, out int p_TotalResults)
        {
            fliq_CustomerApplicationDA objfliq_CustomerApplicationDA = (fliq_CustomerApplicationDA)DataAccessFactory.GetDataAccess(DataAccessType.fliq_CustomerApplication);
            return objfliq_CustomerApplicationDA.GetAllfliq_CustomerApplication(p_ClientName, p_CustomerName, p_PageNumner, p_PageSize,p_IsAsc,p_SortColumn, out p_TotalResults);
        }

        public List<fliQ_UploadTrackingModel> Getfliq_UplaodsByClientGuid(Guid p_ClientGuid, int p_PageNumner, int p_PageSize,bool p_IsAsc,string p_SortColumn, out int p_TotalResults)
        {
            fliq_CustomerApplicationDA objfliq_CustomerApplicationDA = (fliq_CustomerApplicationDA)DataAccessFactory.GetDataAccess(DataAccessType.fliq_CustomerApplication);
            return objfliq_CustomerApplicationDA.Getfliq_UplaodsByClientGuid(p_ClientGuid, p_PageNumner, p_PageSize,p_IsAsc,p_SortColumn, out p_TotalResults);
        }
    }
}
