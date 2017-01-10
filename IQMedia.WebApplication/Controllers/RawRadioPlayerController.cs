using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using System.Text;
using IQMedia.Shared.Utility;

namespace IQMedia.WebApplication.Controllers
{
    [LogAction()]
    public class RawRadioPlayerController : Controller
    {
        //
        // GET: /RawRadioPlayer/
        [HttpGet]
        public ActionResult Index(string id)
        {
            Dictionary<string, string> discRadio = new Dictionary<string, string>();
            try
            {
                if (!string.IsNullOrEmpty(id) && id.Length > 8)
                {
                    int mediaID;
                    UTF8Encoding encoding = new UTF8Encoding();
                    byte[] Key = encoding.GetBytes(CommonFunctions.AesKeyFeedsRadioPlayer);
                    byte[] IV = encoding.GetBytes(CommonFunctions.AesIVFeedsRadioPlayer);

                    string decryptedText = IQMedia.Shared.Utility.CommonFunctions.DecryptStringFromBytes_Aes(id.Substring(8), Key, IV);

                    if (!string.IsNullOrEmpty(decryptedText) && int.TryParse(decryptedText, out mediaID))
                    {
                        IQAgentLogic iqAgentLogic = (IQAgentLogic)LogicFactory.GetLogic(LogicType.IQAgent);
                        string playerUrl = iqAgentLogic.GetTVEyesLocationMediaID(mediaID);

                        discRadio.Add("PlayerUrl", playerUrl);
                        ViewBag.IsErrorOccurred = false;
                    }
                }
            }
            catch (Exception _ex)
            {
                IQMedia.WebApplication.Utility.CommonFunctions.WriteException(_ex);
                ViewBag.IsErrorOccurred = true;
            }

            return View(discRadio);
        }

    }
}
