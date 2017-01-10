using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using System.Data;
using IQMedia.Model;
using IQMedia.Shared.Utility;
using System.Xml.Linq;
using System.Globalization;
using System.Web;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IQMedia.Data
{
    public class CohortDA : IDataAccess
    {
        public Dictionary<long, string> GetAllCohorts()
        {
            try
            {
                DataSet dsSSP = DataAccess.GetDataSet("usp_v5_IQCohort_GetAllCohorts", new List<DataType>());
                return FillCohorts(dsSSP);
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
            }
            return new Dictionary<long, string>();
        }

        public Dictionary<long, string> GetCohortAgents(string cohortID)
        {
            try
            {
                List<DataType> dataTypes = new List<DataType>();
                dataTypes.Add(new DataType("@CohortID", DbType.Int64, Convert.ToInt64(cohortID), ParameterDirection.Input));
                DataSet dsSSP = DataAccess.GetDataSet("usp_v5_IQCohort_GetCohortAgents", dataTypes);
                Dictionary<long, string> dictCohortAgents = new Dictionary<long, string>();

                if (dsSSP != null && dsSSP.Tables.Count > 0)
                {
                    foreach (DataTable dt in dsSSP.Tables)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                dictCohortAgents.Add(Convert.ToInt32(row["_SearchRequestID"]), Convert.ToString(row["Query_Name"]));
                            }
                        }
                    }
                }

                return dictCohortAgents;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
            }
            return new Dictionary<long, string>();
        }

        public List<string> GetHiddenElements(string report)
        {
            try
            {
                List<DataType> dataTypes = new List<DataType>();
                dataTypes.Add(new DataType("@Report", DbType.String, report, ParameterDirection.Input));
                DataSet dsSSP = DataAccess.GetDataSet("usp_v5_IQCohort_GetHiddenElements", dataTypes);
                List<string> hiddenElements = new List<string>();

                if (dsSSP != null && dsSSP.Tables.Count > 0)
                {
                    foreach (DataTable dt in dsSSP.Tables)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                hiddenElements.AddRange(Convert.ToString(row["HiddenElements"]).Split(','));
                            }
                        }
                    }
                }

                return hiddenElements;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
            }
            return new List<string>();
        }

        public Dictionary<long, string> FillCohorts(DataSet dsSSP)
        {
            try
            {
                Dictionary<long, string> dictCohorts = new Dictionary<long, string>();
                if (dsSSP != null && dsSSP.Tables.Count > 0)
                {
                    foreach (DataTable dt in dsSSP.Tables)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                dictCohorts.Add(Convert.ToInt32(row["ID"]), Convert.ToString(row["Name"]));
                            }
                        }
                    }
                }

                return dictCohorts;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
            }
            return new Dictionary<long, string>();
        }

        #region SolrREST

        private static string getResponse(string URL, List<KeyValuePair<string, string>> parameters)
        {
            try
            {
                //Log4NetLogger.Debug(string.Format("URL: {0}", URL));
                string hardcoded = "http://10.100.1.41:8080/solr/cfe-2016-4/select?";
                Uri address = new Uri(URL);
                string ret = string.Empty;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = 210000;

                StringBuilder data = new StringBuilder();
                int count = 0;
                foreach (KeyValuePair<string, string> kvp in parameters)
                {
                    if (count > 0)
                    {
                        data.Append("&");
                    }

                    data.Append(kvp.Key + "=" + HttpUtility.UrlEncode(kvp.Value));
                    count++;
                }

                byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());
                request.ContentLength = byteData.Length;

                string _URL = URL + data.ToString();
                Log4NetLogger.Debug(string.Format("_URL: {0}", _URL));

                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    ret = reader.ReadToEnd();
                    return ret;
                }
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return string.Empty;
            }
        }

        #endregion

        #region SolrEngine

        public List<CohortSolrFacet> Search(CohortSolrFacetRequest request, string Url)
        {
            try
            {
                UTF8Encoding enc = new UTF8Encoding();
                List<CohortSolrFacet> response = new List<CohortSolrFacet>();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                List<Task> tasks = new List<Task>();

                // query passes in q paramter
                string query = "*:*";
                parameters.Add(new KeyValuePair<string, string>("q", query));

                // fQuery passes in fq parameter
                StringBuilder fQuery = new StringBuilder();

                if (request.SearchRequestIDs != null && request.SearchRequestIDs.Count > 0)
                {
                    fQuery.Append("requestid:(");

                    int i = 0;
                    foreach (string SRID in request.SearchRequestIDs)
                    {
                        if (i == 0)
                        {
                            fQuery.Append(SRID);
                        }
                        else
                        {
                            fQuery.Append(" OR " + SRID);
                        }
                        i++;
                    }

                    fQuery.Append(")");
                }

                if (!string.IsNullOrEmpty(fQuery.ToString()))
                {
                    fQuery.Append(" AND ");
                }

                if (request.StartDate != null && request.EndDate != null)
                {
                    fQuery.Append("mediadatedt:[");
                    fQuery.Append(request.StartDate.Value.ToString("s", CultureInfo.CurrentCulture));
                    fQuery.Append("Z TO ");
                    fQuery.Append(request.EndDate.Value.ToString("s", CultureInfo.CurrentCulture));
                    fQuery.Append("Z]");
                }

                fQuery.Append(" AND (heardpaidhits:[1 TO *] OR seenpaidhits:[1 TO *]) AND isdeleted:0");

                if (request.ExtraParameters != null && request.ExtraParameters.Count > 0)
                {
                    fQuery.Append(" AND ");

                    int i = 0;
                    foreach (KeyValuePair<string, string> KvP in request.ExtraParameters)
                    {
                        if (i == 0)
                        {
                            fQuery.AppendFormat("{0}:{1}", KvP.Key, KvP.Value);
                        }
                        else
                        {
                            fQuery.AppendFormat(" AND {0}:{1}", KvP.Key, KvP.Value);
                        }
                    }
                }

                parameters.Add(new KeyValuePair<string, string>("fq", fQuery.ToString()));
                parameters.Add(new KeyValuePair<string, string>("wt", "json"));
                parameters.Add(new KeyValuePair<string, string>("indent", "true"));

                if (request.Facet != null)
                {
                    parameters.Add(new KeyValuePair<string, string>("json.facet", request.Facet));
                }

                parameters.Add(new KeyValuePair<string, string>("rows", "0"));

                tasks.Add(Task.Factory.StartNew((object obj) => ExecuteSearch(parameters, new Uri(Url)), "Results", TaskCreationOptions.AttachedToParent));

                try
                {
                    Task.WaitAll(tasks.ToArray(), 90000);
                }
                catch (Exception exc)
                {
                    Log4NetLogger.Error(exc);
                }

                string result = string.Empty;
                foreach (var tsk in tasks)
                {
                    result = ((Task<string>)tsk).Result;
                }

                response = ParseJSONResponse(result);

                return response;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return new List<CohortSolrFacet>();
            }
        }

        private string ExecuteSearch(List<KeyValuePair<string, string>> vars, Uri Url)
        {
            try
            {
                string response = getResponse(Url.AbsoluteUri, vars);

                return response;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return string.Empty;
            }
        }

        private List<CohortSolrFacet> ParseJSONResponse(string json)
        {
            try
            {
                List<CohortSolrFacet> networks = new List<CohortSolrFacet>();
                Dictionary<string, object> facets = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                Dictionary<string, object> facets2 = JsonConvert.DeserializeObject<Dictionary<string, object>>(facets["facets"].ToString());
                // If just one key then no facets are present
                if (facets2.Keys.Count > 1)
                {
                    Dictionary<string, List<RawCohortFacet>> dictParams = JsonConvert.DeserializeObject<Dictionary<string, List<RawCohortFacet>>>(facets2["buckets"].ToString());
                    List<RawCohortFacet> rawNetworks = dictParams["buckets"];

                    foreach (RawCohortFacet ntwkFacet in rawNetworks)
                    {
                        networks.Add(new CohortSolrFacet() {
                            Name = ntwkFacet.val,
                            Count = ntwkFacet.count,
                            AdValue = ntwkFacet.totaladvalue,
                            PaidHits = ntwkFacet.totalpaidhits
                        });
                    }
                }

                return networks;
            }
            catch (Exception exc)
            {
                Log4NetLogger.Error(exc);
                return new List<CohortSolrFacet>();
            }
        }

        #endregion
    }
}
