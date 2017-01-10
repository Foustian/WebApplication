using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using IQCommon.Model;
using IQMedia.Data;
using IQMedia.Logic.Base;
using IQMedia.Model;
using IQMedia.Shared.Utility;
//using AnalyticsSearch;

namespace IQMedia.Web.Logic
{
    public class CohortLogic: IQMedia.Web.Logic.Base.ILogic
    {
        #region DataAccess

        public Dictionary<long, string> GetAllCohorts()
        {
            CohortDA cohortDA = new CohortDA();
            Dictionary<long, string> cohorts = cohortDA.GetAllCohorts();

            return cohorts;
        }

        public Dictionary<long, string> GetCohortAgents(string cohortID)
        {
            CohortDA cohortDA = new CohortDA();
            Dictionary<long, string> cohortAgents = cohortDA.GetCohortAgents(cohortID);

            return cohortAgents;
        }

        public List<string> GetHiddenElements(string report)
        {
            CohortDA cohortDA = new CohortDA();
            List<string> hiddenElements = cohortDA.GetHiddenElements(report);

            return hiddenElements;
        }

        #endregion

        #region Solr

        public List<CohortSolrFacet> GetTopNetworks(string url, DateTime startDate, DateTime endDate, List<string> searchRequestIDs, string marketID, string program, string station, bool getAll)
        {
            string limit = getAll ? "-1" : "250";
            CohortSolrFacetRequest request = new CohortSolrFacetRequest() {
                StartDate = startDate,
                EndDate = endDate,
                SearchRequestIDs = searchRequestIDs,
                Facet = "{buckets:{terms:{field:stationaffil,limit:" + limit + ",sort:{totalpaidhits:desc},facet:{totaladvalue:'sum(mediavalue)',totalpaidhits:'sum(add(heardpaidhits,seenpaidhits))'}}}}",
                ExtraParameters = new List<KeyValuePair<string,string>>()
            };
            if (!string.IsNullOrEmpty(marketID))
            {
                request.ExtraParameters.Add(new KeyValuePair<string, string>("dmaid", marketID));
            }
            if (!string.IsNullOrEmpty(program))
            {
                request.ExtraParameters.Add(new KeyValuePair<string, string>("titlestr", program));
            }
            if (!string.IsNullOrEmpty(station))
            {
                request.ExtraParameters.Add(new KeyValuePair<string, string>("stationid", station));
            }

            CohortDA cohortDA = new CohortDA();
            List<CohortSolrFacet> networkFacets = cohortDA.Search(request, url);

            return networkFacets;
        }

        public List<CohortSolrFacet> GetTopPrograms(string url, DateTime startDate, DateTime endDate, List<string> searchRequestIDs, string marketID, string network, string station, bool getAll)
        {
            string limit = getAll ? "-1" : "250";
            CohortSolrFacetRequest request = new CohortSolrFacetRequest() {
                StartDate = startDate,
                EndDate = endDate,
                SearchRequestIDs = searchRequestIDs,
                Facet = "{buckets:{terms:{field:titlestr,limit:" + limit + ",sort:{totalpaidhits:desc},facet:{totaladvalue:'sum(mediavalue)',totalpaidhits:'sum(add(heardpaidhits,seenpaidhits))'}}}}",
                ExtraParameters = new List<KeyValuePair<string, string>>()
            };
            if (!string.IsNullOrEmpty(marketID))
            {
                request.ExtraParameters.Add(new KeyValuePair<string, string>("dmaid", marketID));
            }
            if (!string.IsNullOrEmpty(network))
            {
                request.ExtraParameters.Add(new KeyValuePair<string, string>("stationaffil", network));
            }
            if (!string.IsNullOrEmpty(station))
            {
                request.ExtraParameters.Add(new KeyValuePair<string, string>("stationid", station));
            }

            CohortDA cohortDA = new CohortDA();
            List<CohortSolrFacet> programFacets = cohortDA.Search(request, url);

            return programFacets;
        }

        public List<CohortSolrFacet> GetTopStations(string url, DateTime startDate, DateTime endDate, List<string> searchRequestIDs, string marketID, string network, string program, bool getAll)
        {
            string limit = getAll ? "-1" : "250";
            CohortSolrFacetRequest request = new CohortSolrFacetRequest() {
                StartDate = startDate,
                EndDate = endDate,
                SearchRequestIDs = searchRequestIDs,
                Facet = "{buckets:{terms:{field:stationid,limit:" + limit + ",sort:{totalpaidhits:desc},facet:{totaladvalue:'sum(mediavalue)',totalpaidhits:'sum(add(heardpaidhits,seenpaidhits))'}}}}",
                ExtraParameters = new List<KeyValuePair<string, string>>()
            };
            if (!string.IsNullOrEmpty(marketID))
            {
                request.ExtraParameters.Add(new KeyValuePair<string, string>("dmaid", marketID));
            }
            if (!string.IsNullOrEmpty(network))
            {
                request.ExtraParameters.Add(new KeyValuePair<string, string>("stationaffil", network));
            }
            if (!string.IsNullOrEmpty(program))
            {
                request.ExtraParameters.Add(new KeyValuePair<string, string>("titlestr", program));
            }

            CohortDA cohortDA = new CohortDA();
            List<CohortSolrFacet> stationFacets = cohortDA.Search(request, url);

            return stationFacets;
        }

        #endregion
    }
}

