using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using IQMedia.Model;
using System.Text;
using System.Net;
using System.Xml;

namespace IQMedia.WebApplication.Controllers
{
    public class ISVCController : Controller
    {
        //
        // GET: /ISVC/

        [CheckAuthentication()]
        public ActionResult Index()
        {

            ActiveUser sessionInformation = Utility.ActiveUserMgr.GetActiveUser();

            if (sessionInformation.ClientGUID.Equals(new Guid("7722a116-c3bc-40ae-8070-8c59ee9e3d2a")))
            {

                StreamReader strmISVCServices = new StreamReader(Server.MapPath("~/content/ISVCServices.json"));
                string isvcServices = strmISVCServices.ReadToEnd();
                strmISVCServices.Close();
                strmISVCServices.Dispose();


                ISVCModel isvcModel = new ISVCModel();
                isvcModel = (ISVCModel)Newtonsoft.Json.JsonConvert.DeserializeObject(isvcServices, isvcModel.GetType());
                isvcModel.ISVCServices = isvcModel.ISVCServices.OrderBy(a => a.Name).ToList();


                foreach (ISVCServiceModel service in isvcModel.ISVCServices)
                {
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.LoadXml(service.XmlRequest);
                    XmlNode node = xdoc.SelectSingleNode("/");

                    service.XmlRequest = FormatXml(node).Trim();
                }

                return View(isvcModel);
            }
            else
            {
                return RedirectToAction("Unauthorized", "Error");
            }
        }

        public JsonResult GetResponse(string p_Url, string p_Input, string p_Format)
        {
            try
            {
                Uri _Uri = new Uri(p_Url);


                ASCIIEncoding _objEncodedData = new ASCIIEncoding();
                byte[] byteArray = _objEncodedData.GetBytes(p_Input);

                System.Net.ServicePointManager.Expect100Continue = false;

                HttpWebRequest _objWebRequest = (HttpWebRequest)WebRequest.Create(_Uri);

                _objWebRequest.Method = "POST";

                if (p_Format == "xml")
                {
                    _objWebRequest.ContentType = "application/xml";
                }
                else
                {
                    _objWebRequest.ContentType = "application/json";
                }
                _objWebRequest.ContentLength = byteArray.Length;

                Stream _objStream = _objWebRequest.GetRequestStream();
                _objStream.Write(byteArray, 0, byteArray.Length);
                _objStream.Close();

                HttpWebResponse _WebResponse = (HttpWebResponse)_objWebRequest.GetResponse();
                string _ResponseRawMedia = string.Empty;
                if ((_WebResponse.ContentLength > 0))
                {
                    StreamReader _StreamReader = new StreamReader(_WebResponse.GetResponseStream());
                    _ResponseRawMedia = _StreamReader.ReadToEnd();
                    if (p_Format == "xml")
                    {
                        XmlDocument xdoc = new XmlDocument();
                        xdoc.LoadXml(_ResponseRawMedia);
                        XmlNode node = xdoc.SelectSingleNode("/");

                        _ResponseRawMedia = FormatXml(node).Trim();
                    }
                    _StreamReader.Dispose();
                }

                return Json(new
                {
                    isSuccess = true,
                    response = _ResponseRawMedia
                });
            }
            catch (Exception ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(ex);
                return Json(new
                {
                    isSuccess = false,
                    errorMsg = ex.Message
                });
            }
        }

        protected string FormatXml(System.Xml.XmlNode xmlNode)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            // We will use stringWriter to push the formated xml into our StringBuilder bob.
            using (StringWriter stringWriter = new StringWriter(builder))
            {
                // We will use the Formatting of our xmlTextWriter to provide our indentation.
                using (System.Xml.XmlTextWriter xmlTextWriter = new System.Xml.XmlTextWriter(stringWriter))
                {
                    xmlTextWriter.Formatting = System.Xml.Formatting.Indented;
                    xmlNode.WriteTo(xmlTextWriter);
                }
            }

            return builder.ToString();
        }

    }


}
