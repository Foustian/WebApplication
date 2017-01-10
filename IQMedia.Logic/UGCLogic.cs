using IQMedia.Web.Logic.Base;
using IQMedia.Model;
using IQMedia.Data;
using IQMedia.Logic.Base;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace IQMedia.Web.Logic
{
    public class UGCLogic : ILogic
    {
        public string InsertUGCUploadLog(UGCUploadLogModel p_UGCUploadLogModel)
        {
            try
            {
                var objUGCDA = (UGCDA)DataAccessFactory.GetDataAccess(DataAccessType.UGC);
                return objUGCDA.InsertUGCUploadLog(p_UGCUploadLogModel);
            }
            catch (Exception)
            {                
                throw;
            }
        }

        public int InsertArchiveMS(List<string> mediaIDs, Guid clientGUID, Guid customerGUID)
        {
            try
            {
                string mediaIDXml = null;
                if (mediaIDs != null && mediaIDs.Count > 0)
                {
                    XDocument xdoc = new XDocument(new XElement("list",
                                                 from ele in mediaIDs
                                                 select new XElement("item", new XAttribute("id", ele))
                                                         ));
                    mediaIDXml = xdoc.ToString();
                }

                var objUGCDA = (UGCDA)DataAccessFactory.GetDataAccess(DataAccessType.UGC);
                return objUGCDA.InsertArchiveMS(mediaIDXml, clientGUID, customerGUID);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
