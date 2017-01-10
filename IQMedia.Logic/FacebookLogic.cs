using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Web.Logic.Base;
using IQMedia.Model;
using IQMedia.Data;
using IQMedia.Logic.Base;

namespace IQMedia.Web.Logic
{
    public class FacebookLogic : ILogic
    {
        public void InsertFBPages(string FBPageXml)
        {
            FacebookDA facebookDA = (FacebookDA)DataAccessFactory.GetDataAccess(DataAccessType.Facebook);
            facebookDA.InsertFBPages(FBPageXml);
        }

        public List<FBPageModel> GetFBPages()
        {
            FacebookDA facebookDA = (FacebookDA)DataAccessFactory.GetDataAccess(DataAccessType.Facebook);
            return facebookDA.GetFBPages();
        }
    }
}
