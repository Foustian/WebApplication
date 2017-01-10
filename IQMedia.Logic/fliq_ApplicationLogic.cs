using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using IQMedia.Data;
using IQMedia.Logic.Base;

namespace IQMedia.Web.Logic
{
    public class fliq_ApplicationLogic : IQMedia.Web.Logic.Base.ILogic
    {
        public string Insertfliq_Application(fliQ_ApplicationModel p_Application)
        {
            fliq_ApplicationDA objfliq_ApplicationDA = (fliq_ApplicationDA)DataAccessFactory.GetDataAccess(DataAccessType.fliq_Application);
            return objfliq_ApplicationDA.Insertfliq_Application(p_Application);
        }

        public string Updatefliq_Application(fliQ_ApplicationModel p_Application)
        {
            fliq_ApplicationDA objfliq_ApplicationDA = (fliq_ApplicationDA)DataAccessFactory.GetDataAccess(DataAccessType.fliq_Application);
            return objfliq_ApplicationDA.Updatefliq_Application(p_Application);
        }

        public string Deletefliq_Application(Int64 p_ApplicationID)
        {
            fliq_ApplicationDA objfliq_ApplicationDA = (fliq_ApplicationDA)DataAccessFactory.GetDataAccess(DataAccessType.fliq_Application);
            return objfliq_ApplicationDA.Deletefliq_Application(p_ApplicationID);
        }

        public fliQ_ApplicationModel Getliq_ApplicationByID(Int64 p_ApplicationID)
        {
            fliq_ApplicationDA objfliq_ApplicationDA = (fliq_ApplicationDA)DataAccessFactory.GetDataAccess(DataAccessType.fliq_Application);
            return objfliq_ApplicationDA.Getliq_ApplicationByID(p_ApplicationID);
        }


        public List<fliQ_ApplicationModel> GetAllfliq_Application(string p_Application, int p_PageNumner, int p_PageSize, out int p_TotalResults)
        {
            fliq_ApplicationDA objfliq_ApplicationDA = (fliq_ApplicationDA)DataAccessFactory.GetDataAccess(DataAccessType.fliq_Application);
            return objfliq_ApplicationDA.GetAllfliq_Application(p_Application, p_PageNumner, p_PageSize, out p_TotalResults);
        }
    }
}
