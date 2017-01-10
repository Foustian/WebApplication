using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQMedia.Web.Logic;
using IQMedia.Web.Logic.Base;
using System.IO;
using System.Text;
using IQMedia.Shared.Utility;

namespace IQMedia.WebApplication.Controllers
{
    public class ClipRadioPlayerController : Controller
    {
        //
        // GET: /ClipRadioPlayer/

        public ActionResult Index(string id)
        {
            Dictionary<string, string> discRadio = new Dictionary<string, string>();
            try
            {
                if (!string.IsNullOrEmpty(id) && id.Length > 8)
                {
                    Int64 mediaID;

                    UTF8Encoding encoding = new UTF8Encoding();
                    byte[] Key = encoding.GetBytes(CommonFunctions.AesKeyLibRadioPlayer);
                    byte[] IV = encoding.GetBytes(CommonFunctions.AesIVLibRadioPlayer);

                    string decryptedText = IQMedia.Shared.Utility.CommonFunctions.DecryptStringFromBytes_Aes(id.Substring(8), Key, IV);

                    string p_AudioFileLocation = string.Empty;
                    string p_TranscriptFileLocation = string.Empty;
                    if (!string.IsNullOrEmpty(decryptedText) && Int64.TryParse(decryptedText, out mediaID))
                    {
                        IQArchieveLogic iQArchieveLogic = (IQArchieveLogic)LogicFactory.GetLogic(LogicType.IQArchieve);
                        iQArchieveLogic.GetArchiveTVeyesLocationByMediaID(mediaID, out p_TranscriptFileLocation, out p_AudioFileLocation);
                        string transcriptHtml = string.Empty;
                        if (!string.IsNullOrEmpty(p_TranscriptFileLocation) && System.IO.File.Exists(p_TranscriptFileLocation))
                        {
                            StreamReader strmTranscriptHtml = new StreamReader(p_TranscriptFileLocation);
                            transcriptHtml = strmTranscriptHtml.ReadToEnd();
                            strmTranscriptHtml.Close();
                            strmTranscriptHtml.Dispose();
                        }

                        discRadio.Add("ClipRadioPlayer", p_AudioFileLocation);
                        discRadio.Add("Transcript", transcriptHtml);
                    }
                }
                ViewBag.IsErrorOccurred = false;
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
