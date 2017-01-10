using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using IQMedia.Data;
using IQMedia.Logic.Base;
using IQMedia.Web.Logic.Base;

namespace IQMedia.Web.Logic
{
    public class InstagramLogic : ILogic
    {
        public void InsertSources(Guid clientGuid, List<string> tags, List<string> users)
        {
            string sourceXml = null;
            XDocument xdoc = new XDocument(new XElement("Instagram"));

            if (tags != null && tags.Count > 0)
            {
                xdoc.Root.Add(new XElement("Tags",
                    from ele in tags
                    select new XElement("Tag", ele)
                            ));
            }

            if (users != null && users.Count > 0)
            {
                xdoc.Root.Add(new XElement("Users",
                    from ele in users
                    select new XElement("User", ele)
                            ));
            }
            sourceXml = xdoc.ToString();

            InstagramDA instagramDA = (InstagramDA)DataAccessFactory.GetDataAccess(DataAccessType.Instagram);
            instagramDA.InsertSources(clientGuid, sourceXml);
        }

        public bool CheckClientAccess(Guid clientGuid)
        {
            InstagramDA instagramDA = (InstagramDA)DataAccessFactory.GetDataAccess(DataAccessType.Instagram);
            return instagramDA.CheckClientAccess(clientGuid);
        }

        public string GetClientID()
        {
            InstagramDA instagramDA = (InstagramDA)DataAccessFactory.GetDataAccess(DataAccessType.Instagram);
            return instagramDA.GetClientID();
        }
    }
}
