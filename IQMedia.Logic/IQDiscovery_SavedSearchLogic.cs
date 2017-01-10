using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using IQMedia.Data;
using IQMedia.Logic.Base;
using IQMedia.Model;
using IQMedia.Shared.Utility;

namespace IQMedia.Web.Logic
{
    public class IQDiscovery_SavedSearchLogic : IQMedia.Web.Logic.Base.ILogic
    {
        public string InsertDiscoverySavedSearch(Discovery_SavedSearchModel discovery_SavedSearch)
        {
            try
            {
                XDocument xdoc = new XDocument(new XElement("SearchTerms"));
                XDocument xdoc2 = new XDocument(new XElement("SearchIDs"));
                foreach (String sTerm in discovery_SavedSearch.SearchTermArray)
                {
                    xdoc.Root.Add(new XElement("Term", sTerm));
                }                
                foreach (String sName in discovery_SavedSearch.SearchIDArray)
                {
                    xdoc2.Root.Add(new XElement("ID", sName));
                }

                discovery_SavedSearch.SearchTerm = Convert.ToString(xdoc);
                discovery_SavedSearch.SearchID = Convert.ToString(xdoc2);
                IQDiscovery_SavedSearchDA iQDiscovery_SavedSearchDA = (IQDiscovery_SavedSearchDA)DataAccessFactory.GetDataAccess(DataAccessType.SavedSearch);
                return iQDiscovery_SavedSearchDA.InsertDiscoverySavedSearch(discovery_SavedSearch);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string UpdateDiscoverySavedSearch(Discovery_SavedSearchModel discovery_SavedSearch)
        {
            try
            {
                XDocument xdoc = new XDocument(new XElement("SearchTerms"));
                XDocument xdoc2 = new XDocument(new XElement("SearchIDs"));
                if (discovery_SavedSearch.SearchTermArray != null)
                {
                    foreach (String sTerm in discovery_SavedSearch.SearchTermArray)
                    {
                        xdoc.Root.Add(new XElement("Term", sTerm));
                    }
                }
                else xdoc.Root.Add(new XElement("Term", ""));

                if (discovery_SavedSearch.SearchIDArray != null)
                {
                    foreach (String sName in discovery_SavedSearch.SearchIDArray)
                    {
                        xdoc2.Root.Add(new XElement("ID", sName));
                    }
                }
                else xdoc2.Root.Add(new XElement("ID", ""));

                discovery_SavedSearch.SearchTerm = Convert.ToString(xdoc);
                discovery_SavedSearch.SearchID = Convert.ToString(xdoc2);
                IQDiscovery_SavedSearchDA iQDiscovery_SavedSearchDA = (IQDiscovery_SavedSearchDA)DataAccessFactory.GetDataAccess(DataAccessType.SavedSearch);
                return iQDiscovery_SavedSearchDA.UpdateDiscoverySavedSearch(discovery_SavedSearch);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<Discovery_SavedSearchModel> SelectDiscoverySavedSearch(Int32? p_PageNumber, Int32 p_Pagesize, Int32? p_ID, Guid p_CustomerGUID, out Int64 p_TotalRecords)
        {
            try
            {
                IQDiscovery_SavedSearchDA iQDiscovery_SavedSearchDA = (IQDiscovery_SavedSearchDA)DataAccessFactory.GetDataAccess(DataAccessType.SavedSearch);
                return iQDiscovery_SavedSearchDA.SelectDiscoverySavedSearch(p_PageNumber, p_Pagesize, p_ID, p_CustomerGUID, out p_TotalRecords);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Discovery_SavedSearchModel SelectDiscoverySavedSearchByID(Int64 p_ID, Guid p_CustomerGuid)
        {
            try
            {
                IQDiscovery_SavedSearchDA iQDiscovery_SavedSearchDA = (IQDiscovery_SavedSearchDA)DataAccessFactory.GetDataAccess(DataAccessType.SavedSearch);
                List<Discovery_SavedSearchModel> lstDiscovery_SavedSearchModel = iQDiscovery_SavedSearchDA.SelectDiscoverySavedSearchByID(p_ID, p_CustomerGuid);
                Discovery_SavedSearchModel discovery_SavedSearchReturn = new Discovery_SavedSearchModel();

                List<string> lstofSearchTerm = new List<string>();
                List<string> lstofSearchID = new List<string>();
                if (lstDiscovery_SavedSearchModel != null && lstDiscovery_SavedSearchModel.Count > 0)
                {
                    foreach (Discovery_SavedSearchModel discovery_SavedSearchModel in lstDiscovery_SavedSearchModel)
                    {
                        if (!string.IsNullOrWhiteSpace(discovery_SavedSearchModel.SearchTerm))
                        {
                            XDocument xDocSearchTerm = XDocument.Parse(discovery_SavedSearchModel.SearchTerm);
                            foreach (XElement xelem in xDocSearchTerm.Descendants("SearchTerms").Descendants("Term"))
                            {
                                lstofSearchTerm.Add(xelem.Value);
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(discovery_SavedSearchModel.SearchID))
                        {
                            XDocument xDocSearchTerm = XDocument.Parse(discovery_SavedSearchModel.SearchID);
                            foreach (XElement xelem in xDocSearchTerm.Descendants("SearchIDs").Descendants("ID"))
                            {
                                lstofSearchID.Add(xelem.Value);
                            }
                        }
                        discovery_SavedSearchReturn.AdvanceSearchSettingsList = lstDiscovery_SavedSearchModel[0].AdvanceSearchSettingsList;
                        discovery_SavedSearchReturn.AdvanceSearchSettingIDsList = lstDiscovery_SavedSearchModel[0].AdvanceSearchSettingIDsList;
                        discovery_SavedSearchReturn.SearchTermArray = lstofSearchTerm.ToArray();
                        discovery_SavedSearchReturn.SearchIDArray = lstofSearchID.ToArray();
                        discovery_SavedSearchReturn.Mediums = lstDiscovery_SavedSearchModel[0].Mediums;
                        discovery_SavedSearchReturn.FromDate = lstDiscovery_SavedSearchModel[0].FromDate;
                        discovery_SavedSearchReturn.ToDate = lstDiscovery_SavedSearchModel[0].ToDate;
                        discovery_SavedSearchReturn.TVMarket = lstDiscovery_SavedSearchModel[0].TVMarket;
                        discovery_SavedSearchReturn.Title = lstDiscovery_SavedSearchModel[0].Title;
                        discovery_SavedSearchReturn.ID = lstDiscovery_SavedSearchModel[0].ID;
                    }
                }

                return discovery_SavedSearchReturn;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string DeleteDiscoverySavedSearchByID(Int64 p_ID, Guid p_CustomerGuid)
        {
            try
            {
                IQDiscovery_SavedSearchDA iQDiscovery_SavedSearchDA = (IQDiscovery_SavedSearchDA)DataAccessFactory.GetDataAccess(DataAccessType.SavedSearch);
                return iQDiscovery_SavedSearchDA.DeleteDiscoverySavedSearchByID(p_ID, p_CustomerGuid);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
