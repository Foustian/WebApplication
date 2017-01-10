using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using IQMedia.Data;
using IQMedia.Logic.Base;
using IQMedia.Model;
using IQMedia.Web.Logic.Base;

namespace IQMedia.Web.Logic
{
    public class ThirdPartyLogic : ILogic
    {
        public List<ThirdPartyDataTypeModel> GetThirdPartyDataTypesWithCustomerSelection(Guid customerGuid)
        {
            ThirdPartyDA thirdPartyDA = (ThirdPartyDA)DataAccessFactory.GetDataAccess(DataAccessType.ThirdParty);
            return thirdPartyDA.GetThirdPartyDataTypesWithCustomerSelection(customerGuid);
        }

        public int SaveThirdPartyDataTypeSelections(Guid customerGuid, List<string> dataTypeIDs)
        {
            string dataTypeIDXml = null;
            if (dataTypeIDs != null && dataTypeIDs.Count > 0)
            {
                XDocument doc = new XDocument(new XElement(
                    "list",
                    from i in dataTypeIDs
                    select new XElement(
                        "item",
                        new XAttribute("id", i)
                    )
                ));
                dataTypeIDXml = doc.ToString();
            }

            ThirdPartyDA thirdPartyDA = (ThirdPartyDA)DataAccessFactory.GetDataAccess(DataAccessType.ThirdParty);
            return thirdPartyDA.SaveThirdPartyDataTypeSelections(customerGuid, dataTypeIDXml);
        }

        public List<SummaryReportModel> GetThirdPartySummaryData(Guid clientGuid, ThirdPartyDataTypeModel dataTypeModel, DateTime fromDate, DateTime toDate, int dateIntervalType, List<string> searchRequestIDs)
        {            
            string searchRequestIDXml = null;
            if (searchRequestIDs != null && searchRequestIDs.Count > 0)
            {
                XDocument doc = new XDocument(new XElement(
                    "list",
                    from i in searchRequestIDs
                    select new XElement(
                        "item",
                        new XAttribute("id", i)
                    )
                ));
                searchRequestIDXml = doc.ToString();
            }

            ThirdPartyDA thirdPartyDA = (ThirdPartyDA)DataAccessFactory.GetDataAccess(DataAccessType.ThirdParty);
            return thirdPartyDA.GetThirdPartySummaryData(clientGuid, dataTypeModel, fromDate, toDate, dateIntervalType, searchRequestIDXml);
        }
    }
}
