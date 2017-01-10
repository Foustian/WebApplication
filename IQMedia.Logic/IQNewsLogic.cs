using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using IQMedia.Data;
using IQMedia.Logic.Base;

namespace IQMedia.Web.Logic
{
    public class IQNewsLogic : IQMedia.Web.Logic.Base.ILogic
    {
        public List<IQNews_Model> GetIQNews()
        {
            IQNewsDA iQNewsDA = (IQNewsDA)DataAccessFactory.GetDataAccess(DataAccessType.IQNews);
            List<IQNews_Model> result = iQNewsDA.GetIQNews();
            return result;

        }
    }
}
