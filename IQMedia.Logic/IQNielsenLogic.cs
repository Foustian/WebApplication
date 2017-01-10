using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data;
using IQMedia.Logic.Base;
using System.Xml.Linq;
using IQMedia.Model;

namespace IQMedia.Web.Logic
{
    public class IQNielsenLogic
    {
        public List<DiscoveryMediaResult> GetNielsenDataByXML(XDocument xmldata, List<DiscoveryMediaResult> lstDiscoveryMediaResult, Guid clientGuid)
        {

            IQNielsenDA iqNielsenDA = (IQNielsenDA)DataAccessFactory.GetDataAccess(DataAccessType.IQNielsen);
            return iqNielsenDA.GetNielsenDataByXML(xmldata, lstDiscoveryMediaResult, clientGuid);

        }
    }
}
