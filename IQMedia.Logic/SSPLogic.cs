using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data;
using System.Xml.Linq;
using System.Configuration;
using System.Web;

namespace IQMedia.Web.Logic
{
    public class SSPLogic : IQMedia.Web.Logic.Base.ILogic
    {
        public Dictionary<string, object> GetSSPDataByClientGUID(Guid p_ClientGUID, out bool p_IsAllDmaAllowed, out bool p_IsAllClassAllowed, out bool p_IsAllStationAllowed, List<int> p_TVRegions, bool isTAds = false)
        {
            SSPDA ssPDA = new SSPDA();

            List<string> listOfTAdsIQStationID = new List<string>();
            if (isTAds)
            {
                TVLogic tvlogic = new TVLogic();
                listOfTAdsIQStationID = tvlogic.GetTAdsStations();
            }

            Dictionary<string, object> dicSSP = ssPDA.GetSSPDataByClientGUID(p_ClientGUID, out p_IsAllDmaAllowed, out  p_IsAllClassAllowed, out p_IsAllStationAllowed, p_TVRegions, isTAds, listOfTAdsIQStationID);
            dicSSP.Add("stations", listOfTAdsIQStationID);

            return dicSSP;
        }

        public Dictionary<string, object> GetSSPDataByClientGUIDAndFilter(Guid p_ClientGUID, string p_Dma, string p_Station, string p_StationID, int? p_Region, int? p_Country, List<int> p_TVRegions)
        {
            SSPDA ssPDA = new SSPDA();

            Dictionary<string, object> dicSSP = ssPDA.GetSSPDataByClientGUIDAndFilter(p_ClientGUID, p_Dma, p_Station, p_StationID, p_Region, p_Country, p_TVRegions);

            return dicSSP;
        }

        public Dictionary<string, object> GetSSPDataWithStationByClientGUIDOld(Guid p_ClientGUID, out bool p_IsAllDmaAllowed, out bool p_IsAllClassAllowed, out bool p_IsAllStationAllowed, List<int> p_TVRegions)
        {
            SSPDA ssPDA = new SSPDA();

            Dictionary<string, object> dicSSP = ssPDA.GetSSPDataWithStationByClientGUIDOld(p_ClientGUID, out p_IsAllDmaAllowed, out  p_IsAllClassAllowed, out p_IsAllStationAllowed, p_TVRegions);

            return dicSSP;
        }

        public Dictionary<string, object> GetDMAsByZipCode(List<string> zipCodes)
        {
            SSPDA ssPDA = new SSPDA();

            XDocument xdoc = new XDocument(new XElement("list",
                                            from ele in zipCodes
                                            select new XElement("item", new XAttribute("zipcode", ele))
                                                    ));
            string zipCodeList = xdoc.ToString();

            Dictionary<string, object> dictResult = ssPDA.GetDMAsByZipCode(zipCodeList);
            return dictResult;
        }

        public Boolean GetSharing(string p_ClipID, HttpCookie p_AuthCookie = null)
        {
            try
            {
                Boolean IsSharing = false;
                string url = ConfigurationManager.AppSettings["GetStationSharing"] + p_ClipID;

                string respone = string.Empty;
                if (p_AuthCookie != null)
                {
                    respone = IQMedia.Shared.Utility.CommonFunctions.DoHttpGetRequest(url, authCookie: new System.Net.Cookie(p_AuthCookie.Name, p_AuthCookie.Value, "/", ".iqmediacorp.com"));
                }

                Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();
                jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(respone.Replace(@"\", string.Empty).Replace("([", string.Empty).Replace("])", string.Empty).Replace("\"", "'"));
                if (jsonData != null)
                {
                    if (Convert.ToBoolean(jsonData["IsSharing"]))
                    {
                        IsSharing = true;
                    }
                }
                return IsSharing;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Boolean GetEmailSharing(string p_ClipID, HttpCookie p_AuthCookie = null)
        {
            try
            {
                Boolean IsEmailSharing = false;
                string url = ConfigurationManager.AppSettings["GetEmailSharing"];
                string respone = string.Empty;
                if (p_AuthCookie != null)
                {
                    respone = IQMedia.Shared.Utility.CommonFunctions.DoHttpGetRequest(url, authCookie: new System.Net.Cookie(p_AuthCookie.Name, p_AuthCookie.Value, "/", ".iqmediacorp.com"));
                }
                Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();
                jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(respone.Replace(@"\", string.Empty).Replace("([", string.Empty).Replace("])", string.Empty).Replace("\"", "'"));
                if (jsonData != null)
                {
                    if (Convert.ToBoolean(jsonData["IsEmailSharing"]))
                    {
                        IsEmailSharing = true;
                    }
                }
                return IsEmailSharing;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
