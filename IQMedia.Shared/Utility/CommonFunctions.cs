using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Net.Mail;
using System.Configuration;
using System.Security.Cryptography;
using System.Net;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Globalization;

namespace IQMedia.Shared.Utility
{
    public static class CommonFunctions
    {
        private static string AesKeyLicense = "6D372F5167584155694672674D486B67";
        private static string AesIVLicense = "516341644D4A3373";

        public static string AesKeyFeedsRadioPlayer = "632B783177697669622F333041426133";
        public static string AesIVFeedsRadioPlayer = "3955456236643867";

        public static string AesKeyLibRadioPlayer = "69414A69742B7538744D436170537539";
        public static string AesIVLibRadioPlayer = "4C37626234324A6A";

        public static string UGCMediaType = "VMedia";

        public static Random random = new Random();

        public static int KantorChartWidth = 8000;

        public static string SerializeToXml(object p_SerializationObject)
        {
            try
            {
                string _XMLString = string.Empty;

                System.Text.UTF8Encoding _Encoding = new System.Text.UTF8Encoding();
                XmlWriterSettings _XmlWriterSettings = new XmlWriterSettings();
                // _XmlWriterSettings.Encoding=_Encoding;
                _XmlWriterSettings.OmitXmlDeclaration = true;

                XmlSerializer _XmlSerializer = new XmlSerializer(p_SerializationObject.GetType(), "");

                try
                {
                    StringWriter _StringWriter = new StringWriter();
                    using (XmlWriter _XmlWriter = XmlWriter.Create(_StringWriter,
                    _XmlWriterSettings))
                    {
                        XmlSerializerNamespaces _XmlSerializerNamespaces = new XmlSerializerNamespaces();
                        _XmlSerializerNamespaces.Add("", "");

                        _XmlSerializer.Serialize(_XmlWriter, p_SerializationObject, _XmlSerializerNamespaces);
                    }

                    _XMLString = _StringWriter.ToString();
                }
                catch (Exception _Exception)
                {
                    throw _Exception;
                }

                return _XMLString;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static object DeserialiazeXml(string p_XMLString, object p_Deserialization)
        {
            StringReader _StringReader;
            XmlTextReader _XmlTextReader;

            try
            {
                XmlSerializer _XmlSerializer = new XmlSerializer(p_Deserialization.GetType());
                _StringReader = new StringReader(p_XMLString);
                _XmlTextReader = new XmlTextReader(_StringReader);
                p_Deserialization = (object)_XmlSerializer.Deserialize(_XmlTextReader);
                _StringReader.Close();
                _XmlTextReader.Close();
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return p_Deserialization;
        }

        public static string GetPropertyValueAsString(object src, string propertyName)
        {
            try
            {
                PropertyInfo propInfo = src.GetType().GetProperty(propertyName);
                if (propInfo != null)
                {
                    return propInfo.GetValue(src, null) != null ? propInfo.GetValue(src, null).ToString() : String.Empty;
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public enum MissingArticleTypes
        {
            [Description("Social Media")]
            SocialMedia,
            [Description("Online News")]
            NM,
            [Description("Forum")]
            Forum,
            [Description("Blog")]
            Blog,
            [Description("Comment")]
            Comment,
            [Description("Microblog")]
            Microblog,
            [Description("Q&A")]
            QNA,
            [Description("Social Photo")]
            SocialPhoto,
            [Description("Review")]
            Review,
            [Description("Podcast")]
            Podcast,
            [Description("Social Network")]
            SocialNetwork,
            [Description("Social Video")]
            SocialVideo,
            [Description("Wiki")]
            Wiki,
            [Description("Classified")]
            Classified,
        }

        public enum MediaType
        {

            [Description("Social Media")]
            SM,
            [Description("Online News")]
            NM,
            [Description("Publications (scanned)")]
            PM,
            [Description("TV")]
            TV,
            [Description("Twitter")]
            TW,
            [Description("Radio")]
            TM,
            [Description("Publications (text)")]
            PQ,
            [Description("Logo Recognition")]
            LR,
            [Description("Print Media")]
            PR,
            [Description("Blog")]
            BL,
            [Description("Forum")]
            FO
        }

        public enum SearchRequestMediaType
        {

            [Description("TV")]
            TV,
            [Description("Online News")]
            News,
            [Description("Social Media")]
            SocialMedia,
            [Description("Twitter")]
            Twitter,
            [Description("Radio")]
            TM,
            [Description("Publications (scanned)")]
            PM,
            [Description("Publications (text)")]
            PQ
        }

        public enum CategoryType
        {

            [Description("Social Media")]
            SocialMedia = 4,
            [Description("Online News")]
            NM = 1,
            [Description("Publications (scanned)")]
            PM = 101,
            [Description("TV")]
            TV = 102,
            [Description("Twitter")]
            TW = 103,
            [Description("Forum")]
            Forum = 3,
            [Description("Blog")]
            Blog = 2,
            [Description("Radio")]
            Radio = 104,
            [Description("Publications (text)")]
            PQ,
            [Description("Miscellaneous")]
            MS,
            [Description("Facebook")]
            FB,
            [Description("Instagram")]
            IG,
            [Description("Publications (LexisNexis)")]
            LN
        }

        public enum DashBoardMediumType
        {
            [Description("Social Media")]
            SocialMedia,
            [Description("Online News")]
            NM,
            [Description("Publications")]
            PM,
            [Description("TV")]
            TV,
            [Description("Twitter")]
            TW,
            [Description("Forum")]
            Forum,
            [Description("Blog")]
            Blog,
            [Description("Overview")]
            Overview,
            [Description("Radio")]
            Radio,
            [Description("Miscellaneous")]
            MS,
            [Description("Google Analytics")]
            Google,
            [Description("Client Specific Data")]
            ClientSpecific
        }

        public enum Roles
        {
            [Description("Feeds")]
            v4Feeds,
            [Description("Discovery")]
            v4Discovery,
            [Description("Library")]
            v4Library,
            [Description("Timeshift")]
            v4Timeshift,
            [Description("Dashboard")]
            v4Dashboard,
            [Description("Timeshift-Radio")]
            v4Radio,
            [Description("Setup")]
            v4Setup,
            [Description("GlobalAdminAccess")]
            GlobalAdminAccess,
            [Description("UGC")]
            v4UGC,
            [Description("IframeMicrosite")]
            IframeMicrosite,
            [Description("MicrositeDownload")]
            MicrositeDownload,
            [Description("IQAgent")]
            v4IQAgentSetup,
            [Description("TV")]
            v4TV,
            [Description("Online News")]
            v4NM,
            [Description("Social Media")]
            v4SM,
            [Description("Twitter")]
            v4TW,
            [Description("Radio")]
            v4TM,
            [Description("UGCAutoClip")]
            UGCAutoClip,
            [Description("UGCDownload")]
            UGCDownload,
            [Description("UGCUploadEdit")]
            UGCUploadEdit,
            [Description("v4Group")]
            v4Group,
            [Description("v4CustomImage")]
            v4CustomImage,
            [Description("CompeteData")]
            CompeteData,
            [Description("NielsenData")]
            NielsenData,
            [Description("BurrellesLuce")]
            v4BLPM,
            [Description("News Right")]
            NewsRight,
            v4CustomSettings,
            [Description("Discovery Lite")]
            v4DiscoveryLite,
            [Description("fliQ Admin")]
            fliQAdmin,
            [Description("ProQuest")]
            v4PQ,
            [Description("MediaRoomContributor")]
            MediaRoomContributor,
            [Description("MediaRoomEditor")]
            MediaRoomEditor,
            [Description("Google")]
            v4Google,
            [Description("Library Dashboard")]
            v4LibraryDashboard,
            [Description("Timeshift Facet")]
            TimeshiftFacet,
            [Description("Share TV")]
            ShareTV,
            [Description("TAds")]
            v4TAds,
            [Description("Social Media - Other")]
            SMOther,
            [Description("Facebook")]
            FB,
            [Description("Instagram")]
            IG,
            [Description("Blog")]
            BL,
            [Description("Forum")]
            FO,
            [Description("Print Media")]
            PR,
            [Description("LexisNexis")]
            LN,
            [Description("Analytics")]
            v5Analytics,
            [Description("LR")]
            v5LR,
            [Description("Ads")]
            v5Ads,
            [Description("Third Party Data")]
            ThirdPartyData,
            [Description("Imported Sony Data")]
            ClientSpecificData,
            [Description("ANewsTip Integration Access")]
            ConnectAccess,
            [Description("Twitter, TVEyes, BLPM Rule Edit Access")]
            ExternalRuleEditor
        }

        public enum Timezone
        {
            [Description("CST")]
            CST,
            [Description("EST")]
            EST,
            [Description("PST")]
            PST,
            [Description("MST")]
            MST
        }

        public enum SearchType
        {
            TimeShift,
            Discovery_TV,
            Discovery_NM,
            Discovery_BL,
            Discovery_FO,
            Discovery_TW,
            Discovery_PQ,
            Discovery_LN,
            Feeds,
            Feeds_Dashboard,
            Feeds_Children,
            TAds
        }

        public enum DefaultPage
        {
            v4Dashboard,
            v4Feeds,
            v4Discovery,
            v4Timeshift,
            v5LR,
            v4Library,
            v4Setup,
            v4GlobalAdmin,
            v4DiscoveryLite,
            v4TAds,
            v5Analytics
        }

        public enum IQNotificationFrequency
        {
            [Description("Immediate")]
            Immediate,
            [Description("Hourly")]
            Hourly,
            [Description("OnceDay")]
            OnceDay,
            [Description("OnceWeek")]
            OnceWeek
        }

        public enum LibraryReportSettings
        {
            TotalAudience,
            TotalMediaValue,
            Audience,
            MediaValue,
            Sentiment,
            NationalValue,
            TotalNationalAudience,
            TotalNationalMediaValue,
            CoverageSources
        }

        public enum FullTextLogicalOperator
        {
            [StringValue("AND")]
            AND = 1,
            [StringValue("&")]
            AND1 = 2,
            [StringValue("OR")]
            OR = 3,
            [StringValue("|")]
            OR1 = 4,
            [StringValue("NEAR")]
            NEAR = 5,
            [StringValue("NOT")]
            NOT = 6,
            [StringValue("AND NOT")]
            ANDNOT = 7,
            [StringValue("~")]
            TILT = 8

        }

        public enum IQTVRegions
        {
            [Description("Canada")]
            Canada = 600,
            [Description("Latin America")]
            LatinAmerica = 650
        }

        public enum KantarMediaTypes
        {
            Kantar = 1,
            Sony = 2,
            QVC = 3
        }

        public enum IQUGCMediaTypes
        {
            [Description("Image")]
            Image,
            [Description("Pdf")]
            Pdf,
            [Description("Excel")]
            Excel,
            [Description("Doc")]
            Doc,
            [Description("Media")]
            VMedia
        }

        public enum TemplateTypes
        {
            EmailTemplate,
            MCMediaTemplate,
            ReportTemplate
        }

        public enum LibraryTextTypes
        {
            [Description("Content")]
            Content = 1,
            [Description("Highlighting Text")]
            HighlightingText = 2,
            [Description("Description")]
            Description = 3
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static string GetValueFromDescription<T>(string description)
        {

            string returnValue = string.Empty;
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                    {
                        //returnValue = true;
                        returnValue = Convert.ToString((T)field.GetValue(null));
                    }
                    //return (T)field.GetValue(null);
                }

            }
            return returnValue;
            //throw new ArgumentException("Not found.", "description");
            // or return default(T); 
        }

        public static T StringToEnum<T>(string name)
        {
            return (T)Enum.Parse(typeof(T), name);
        }

        public static string SearializeJson(object _object)
        {
            try
            {
                DataContractJsonSerializer Serializer = new DataContractJsonSerializer(_object.GetType());

                MemoryStream Stream = new MemoryStream();

                Serializer.WriteObject(Stream, _object);

                Stream.Position = 0;

                StreamReader StreamReader = new StreamReader(Stream);

                return StreamReader.ReadToEnd();

                /*JavaScriptSerializer _JavaScriptSerializer = new JavaScriptSerializer();

                return _JavaScriptSerializer.Serialize(_object);*/
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static bool SendMail(string sTo, string sCC, string[] sBCCs, string sFrom, string sSubject, string sMsgBody, bool bHtml, string[] sAttachments, string[] alternetViewsName = null)
        {

            string sSMTPServer = ConfigurationManager.AppSettings["SMTPServer"];
            string sSMTPPort = ConfigurationManager.AppSettings["Port"];
            bool sSmtpSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEnableSSL"]);
            string sSMTPPassword = ConfigurationManager.AppSettings["LoginPassword"];

            if (sFrom.Length == 0 || sTo.Length == 0 || sSMTPServer.Length == 0)
                return false;
            try
            {
                System.Net.Mail.MailMessage objMail = new System.Net.Mail.MailMessage();

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["AddSender"]) && string.Compare(sFrom, ConfigurationManager.AppSettings["Sender"], true) != 0)
                {
                    objMail.Sender = new MailAddress(ConfigurationManager.AppSettings["Sender"]); 
                }
                objMail.From = new MailAddress(sFrom);
                objMail.Subject = sSubject;
                objMail.Body = sMsgBody;

                if (sTo.Length > 0)
                    objMail.To.Add(sTo);
                if (sCC.Length > 0)
                    objMail.CC.Add(sCC);
                if (sBCCs!=null && sBCCs.Count() > 0)
                {
                    foreach (string sBCC in sBCCs)
                    {
                        objMail.Bcc.Add(sBCC);
                    }
                }

                if (bHtml)
                    objMail.IsBodyHtml = true;
                else
                    objMail.IsBodyHtml = false;


                if (alternetViewsName != null && alternetViewsName.Length > 0)
                {
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(sMsgBody, null, "text/html");
                    foreach (string altrView in alternetViewsName)
                    {


                        LinkedResource imagelink = new LinkedResource(altrView);
                        imagelink.ContentId = Path.GetFileName(altrView);

                        imagelink.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                        htmlView.LinkedResources.Add(imagelink);


                    }
                    objMail.AlternateViews.Add(htmlView);
                    //imagelink.Dispose();
                    //htmlView.Dispose();
                }


                if (sAttachments != null && sAttachments.Length > 0)
                {
                    for (int iCount = 0; iCount < sAttachments.Length; iCount++)
                    {
                        objMail.Attachments.Add(new Attachment(sAttachments[iCount]));
                    }
                }

                SmtpClient smtp = new SmtpClient();
                smtp.Host = sSMTPServer;
                System.Net.NetworkCredential objCredential = new System.Net.NetworkCredential(sFrom, sSMTPPassword);
                smtp.EnableSsl = sSmtpSSL;
                smtp.Credentials = objCredential;
                smtp.Send(objMail);

                if (objMail.AlternateViews != null)
                {
                    foreach (AlternateView altView in objMail.AlternateViews)
                    {
                        foreach (LinkedResource lnkdResource in altView.LinkedResources)
                        {
                            lnkdResource.Dispose();
                        }

                    }

                    objMail.AlternateViews.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error("Send Mail error", ex);

                return false;
            }
        }

        /// <summary>
        /// Return file content type based on file extension passed as parameter
        /// </summary>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public static string GetFileContentTypeByExtension(string fileExtension)
        {
            if (!string.IsNullOrEmpty(fileExtension))
            {
                if (!fileExtension.StartsWith("."))
                {
                    fileExtension = "." + fileExtension;
                }
                switch (fileExtension.ToLower())
                {
                    case ".asf":
                        return "video/x-ms-asf";
                    case ".avi":
                        return "video/avi";
                    case ".wav":
                        return "audio/wav";
                    case ".mp3":
                        return "audio/mpeg3";
                    case ".mpg":
                    case "mpeg":
                        return "video/mpeg";
                    case ".rtf":
                        return "application/rtf";
                    case ".asp":
                        return "text/asp";
                    case ".dwg":
                        return "image/vnd.dwg";
                    case ".wmv":
                        return "video/wmv";
                    case ".mp4":
                        return "video/mp4";
                    case ".pdf" :
                        return "application/pdf";
                    case ".jpg":
                    case ".jpeg":
                        return "image/jpeg";
                    case ".png":
                        return "image/png";
                    case ".doc":
                        return "application/msword";
                    case ".docx":
                        return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    case ".xls":
                        return "application/vnd.ms-excel";
                    case ".xlsx":
                        return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    case ".xml":
                        return "application/xml";
                    default:
                        return "application/octet-stream";
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public static bool Validate_url(string internet_url)
        {
            if (!string.IsNullOrWhiteSpace(internet_url))
            {
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
                return reg.IsMatch(internet_url);
            }
            return false;
        }

        /// <summary>
        /// Use this method to complete word while performing Substring function, remove incomplete "html" tags from string that cause problem while rendering
        /// </summary>
        /// <param name="p_HighlightingText"></param>
        /// <returns></returns>
        public static string ProcessHighlightingText(string p_HighlightingText, string p_SubHighlightingText)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(p_HighlightingText) && !string.IsNullOrWhiteSpace(p_SubHighlightingText))
                {
                    // Step - 1. Try to complete word if it cuts while performing Substring operation
                    int WordCompletedIndex = -1;
                    if (p_HighlightingText.Length > p_SubHighlightingText.Length)
                    {
                        WordCompletedIndex = p_HighlightingText.IndexOf(" ", p_SubHighlightingText.Length);
                    }

                    if (!p_SubHighlightingText.EndsWith(" ") && WordCompletedIndex > 0)
                    {
                        p_SubHighlightingText = p_HighlightingText.Substring(0, WordCompletedIndex);
                    }

                    // Step - 2. Load Substring into HTMLAgilityPack. If any unclosed or invalid HTML tags occurs, just trim Substring from that position.

                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(p_SubHighlightingText);

                    if (doc.ParseErrors.Count() > 0)
                    {
                        foreach (HtmlAgilityPack.HtmlParseError error in doc.ParseErrors)
                        {
                            if (error.StreamPosition > 0)
                            {
                                p_SubHighlightingText = p_SubHighlightingText.Substring(0, error.StreamPosition);
                                break;
                            }
                        }
                    }

                    return p_SubHighlightingText;
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error("error occured while processing text", ex);
                throw;
            }
            return string.Empty;
        }

        public static string DecryptStringFromBytes_Aes(string encrypteString, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (string.IsNullOrWhiteSpace(encrypteString))
                throw new ArgumentNullException("encrypted string is null");

            //byte[] cipherText = Convert.FromBase64String(encrypteString.Replace(" ", "+")); 
            byte[] cipherText = Convert.FromBase64String(encrypteString);

            //byte[] cipherText = StringToUTF8ByteArray(encrypteString); 
            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an AesManaged object 
            // with the specified key and IV. 
            using (AesManaged aesAlg = new AesManaged())
            {
                UTF8Encoding encoding = new UTF8Encoding();

                aesAlg.Key = Key;// aesAlg.Key; 
                aesAlg.IV = IV;// aesAlg.IV; 

                // Create a decrytor to perform the stream transform. 
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string. 
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        public static string DecryptAESHex(string p_encrypteString, byte[] p_Key, byte[] p_IV)
        {
            // Check arguments. 
            if (string.IsNullOrWhiteSpace(p_encrypteString))
            {
                throw new ArgumentNullException("encrypted string is null");
            }

            byte[] cipherText = StringToByteArray(p_encrypteString);

            // Declare the string used to hold 
            // the decrypted text. 
            string plainText = null;

            // Create an AesManaged object 
            // with the specified key and IV. 
            using (AesManaged aesAlg = new AesManaged())
            {
                UTF8Encoding encoding = new UTF8Encoding();

                aesAlg.Key = p_Key;
                aesAlg.IV = p_IV;

                // Create a decrytor to perform the stream transform. 
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string. 
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plainText;
        }

        public static string EncryptStringAES(string rawString, string key, string iv)
        {
            byte[] encrypted;


            UTF8Encoding encoding = new UTF8Encoding();

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesManaged = new AesManaged())
            {
                aesManaged.Key = encoding.GetBytes(key);
                aesManaged.IV = encoding.GetBytes(iv);

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesManaged.CreateEncryptor(aesManaged.Key, aesManaged.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(rawString);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return Convert.ToBase64String(encrypted);
        }

        public static string EncryptStringAES(string p_Key, string p_Data, out string p_AutoGenIV, bool p_IsAutoGenIV = true, string p_IV = "")
        {
            byte[] encrypted;
            p_AutoGenIV = p_IV;

            UTF8Encoding encoding = new UTF8Encoding();

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesManaged = new AesManaged())
            {
                aesManaged.Mode = CipherMode.CBC;
                aesManaged.Padding = PaddingMode.PKCS7;
                aesManaged.Key = encoding.GetBytes(p_Key);

                if (p_IsAutoGenIV)
                {
                    aesManaged.GenerateIV();
                }
                else
                {
                    aesManaged.IV = StringToByteArray(p_IV);
                }

                p_AutoGenIV = ByteArrayToString(aesManaged.IV);
                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesManaged.CreateEncryptor(aesManaged.Key, aesManaged.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(p_Data);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return (ByteArrayToString(encrypted));
        }

        public static string EncryptLicenseStringAES(string rawString)
        {
            byte[] encrypted;


            UTF8Encoding encoding = new UTF8Encoding();

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesManaged = new AesManaged())
            {
                aesManaged.Key = encoding.GetBytes(AesKeyLicense);
                aesManaged.IV = encoding.GetBytes(AesIVLicense);

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesManaged.CreateEncryptor(aesManaged.Key, aesManaged.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(rawString);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return Convert.ToBase64String(encrypted);
        }       

        /// <summary>
        /// Byte Array to Hex String
        /// </summary>
        /// <param name="p_BA">Byte Array</param>
        /// <returns>Hex String</returns>
        public static string ByteArrayToString(byte[] p_BA)
        {
            string hex = BitConverter.ToString(p_BA);
            return hex.Replace("-", "");
        }

        /// <summary>
        /// Hex String to Byte Array
        /// </summary>
        /// <param name="p_Hex">Hex String</param>
        /// <returns>Byte Array</returns>
        public static byte[] StringToByteArray(String p_Hex)
        {
            int numberChars = p_Hex.Length;
            byte[] bytes = new byte[numberChars / 2];

            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(p_Hex.Substring(i, 2), 16);
            }

            return bytes;
        }

        public static string DecryptLicenseStringAes(string encrypteString)
        {
            // Check arguments. 
            if (string.IsNullOrWhiteSpace(encrypteString))
                throw new ArgumentNullException("encrypted string is null");

            //byte[] cipherText = Convert.FromBase64String(encrypteString.Replace(" ", "+")); 
            byte[] cipherText = Convert.FromBase64String(encrypteString);

            //byte[] cipherText = StringToUTF8ByteArray(encrypteString); 
            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an AesManaged object 
            // with the specified key and IV. 
            using (AesManaged aesAlg = new AesManaged())
            {
                UTF8Encoding encoding = new UTF8Encoding();

                aesAlg.Key = encoding.GetBytes(AesKeyLicense);
                aesAlg.IV = encoding.GetBytes(AesIVLicense);

                // Create a decrytor to perform the stream transform. 
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string. 
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        public static string GetDateFormatFromMinutes(double p_value)
        {
            TimeSpan ts = TimeSpan.FromMinutes(p_value);
            return ts.Hours.ToString().PadLeft(2, '0') + ":" + ts.Minutes.ToString().PadLeft(2, '0') + ":" + ts.Seconds.ToString().PadLeft(2, '0');
        }

        public static string DoHttpGetRequest(string p_URL, string userAgent = null,Cookie authCookie=null, Dictionary<string, string> headers = null)
        {
            try
            {
                Uri _Uri = new Uri(p_URL);
                HttpWebRequest _objWebRequest = (HttpWebRequest)WebRequest.Create(_Uri);
              
                if (authCookie!=null)
                {
                    Log4NetLogger.Debug("Cookie.." + authCookie.Name + " value:" + authCookie.Value);

                    _objWebRequest.CookieContainer=new CookieContainer();
                    authCookie.Domain = ".iqmediacorp.com";
                    _objWebRequest.CookieContainer.Add(authCookie);
                }
                else
                {
                    Log4NetLogger.Debug("No cookie");
                }
              
                _objWebRequest.Timeout = 100000000;
                _objWebRequest.Method = "GET";
                if (!string.IsNullOrEmpty(userAgent))
                {
                    _objWebRequest.UserAgent = userAgent;
                }
                _objWebRequest.Headers.Add("origin", "http://qav4.iqmediacorp.com");
                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        _objWebRequest.Headers.Add(header.Key, header.Value);
                    }
                }
                StreamReader _StreamReader = null;

                string _ResponseRawMedia = string.Empty;                

                if ((_objWebRequest.GetResponse().ContentLength > 0))
                {
                    _StreamReader = new StreamReader(_objWebRequest.GetResponse().GetResponseStream());
                    _ResponseRawMedia = _StreamReader.ReadToEnd();
                    _StreamReader.Dispose();
                }

                Log4NetLogger.Debug("Url: "+p_URL+" response: " + _ResponseRawMedia);

                return _ResponseRawMedia;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error("WebRequest failed: ", ex);
                throw;
            }
        }

        public static string DoHttpPostRequest(string p_URL, string p_Data, Cookie authCookie = null, string p_ContentType = null, bool p_IgnoreResponseLength = false)
        {
            Uri uri = new Uri(p_URL);

            ASCIIEncoding encodedData = new ASCIIEncoding();
            byte[] byteArray = encodedData.GetBytes(p_Data);

           // System.Net.ServicePointManager.Expect100Continue = false;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);

            webRequest.Method = "POST";
            
            webRequest.ContentLength = byteArray.Length;
            webRequest.KeepAlive = true;
            webRequest.Timeout = 300000;
            webRequest.Headers.Add("origin", "https://www.iqmediacorp.com");

            if (!String.IsNullOrWhiteSpace(p_ContentType))
            {
                webRequest.ContentType = p_ContentType;
            }

            if (authCookie != null)
            {
                webRequest.CookieContainer = new CookieContainer();
                authCookie.Domain = ".iqmediacorp.com";
                webRequest.CookieContainer.Add(authCookie);
            }   

            Stream istream = webRequest.GetRequestStream();
            istream.Write(byteArray, 0, byteArray.Length);
            istream.Close();                   

            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
            string response = string.Empty;

            // There are situations where the ContentLength value is -1, but there is content.
            // I didn't want to remove the >0 check in case something depends on it. 
            if (webResponse.ContentLength > 0 || p_IgnoreResponseLength)
            {
                using (StreamReader sr = new StreamReader(webResponse.GetResponseStream()))
                {
                    response = sr.ReadToEnd();
                }
            }

            return response;
        }

        public static Int64 GetPercentageRelative(long Value1, long Value2)
        {
            if (Value1 == 0 || Value2 == 0)
            {
                return 100;
            }


            return Math.Abs(100 - Convert.ToInt64(Value2 * 100 / Value1));
        }

        public static Int64 GetPercentageRelative(decimal Value1, decimal Value2)
        {
           return Math.Abs(100 - Convert.ToInt64(Value2 * 100 / Value1));
        }

        public static string GetWordsAround(string InputString,string Keyword, int BeforeWords, int AfterWords,string Seprator,string WordPrefexSpecialChar = "<")
        {
            string returnStr = string.Empty;
            InputString = Regex.Replace(InputString, "\\s+", " ");
            //string regex = "(?:[a-zA-Z'-<>]+[^a-zA-Z'-]+){0," + BeforeWords + "}\\b" + Keyword + "\\b(?:[^a-zA-Z'-]+[a-zA-Z'-<>]+){0," + AfterWords + "}";
            //string regex = "((?:[\\s<>]\\S*){0," + BeforeWords + "})" + WordPrefexSpecialChar + "\\b" + Keyword + "\\b((?:\\S*\\s+){0," + AfterWords + "})";
            string regex = "((?:[\\s<>]\\S*){0," + BeforeWords + "})" + WordPrefexSpecialChar + "\\b" + Keyword + "\\b((?:(<(([^>]*)?)>[^<].*?(</span>)|\\S*\\s+)){0," + AfterWords + "})";
            MatchCollection collection = Regex.Matches(InputString, regex);
            int i = 1;
            foreach (Match m in collection)
            {
                if (i == collection.Count)
                {
                    returnStr = returnStr + m.Value;
                }
                else
                {
                    returnStr = returnStr + m.Value + Seprator;
                }
                i = i + 1;
            }
            return returnStr;
        }

        public static string GenerateRandomString(int length =8)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string strRandom =  new string(
                Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return strRandom;
        }

        public static bool IsValidEmail(string strIn)
        {
            var invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper);
            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn,
                   @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                   RegexOptions.IgnoreCase);
        }

        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            var invalid = false;
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }

        /// <summary>
        /// Checks Audience/MediaValue access
        /// </summary>
        /// <param name="p_UseAM">Use Audience/MediaValue of IQ_MediaTypeModel</param>
        /// <param name="p_RequireNielsenAccess">Nielsen Access role required or not</param>
        /// <param name="p_NielsenAccess">Has Nielsen Access</param>
        /// <param name="p_RequireCompeteAccess">Compete Access role required or not</param>
        /// <param name="p_CompeteAccess">Has Compete Access</param>
        /// <returns>true/false has access or not</returns>
        public static bool CheckNielsenCompeteAccess(bool p_UseAM, bool p_RequireNielsenAccess, bool p_NielsenAccess, bool p_RequireCompeteAccess, bool p_CompeteAccess)
        {
            return (p_UseAM && ((p_RequireNielsenAccess && p_NielsenAccess) || (p_RequireCompeteAccess && p_CompeteAccess) || (!p_RequireNielsenAccess && !p_RequireCompeteAccess)));
        }
    }

    public class StringValue : System.Attribute
    {
        private string _value;

        public StringValue(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

    }



}
