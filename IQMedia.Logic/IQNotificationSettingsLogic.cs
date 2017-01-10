using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using IQMedia.Data;
using IQMedia.Logic.Base;
using System.Xml.Linq;

namespace IQMedia.Web.Logic
{
    public class IQNotificationSettingsLogic : IQMedia.Web.Logic.Base.ILogic
    {
        public List<IQNotifationSettingsModel> SelectIQNotifcationsBySearchRequestID(string ClientGuid, long SearchRequestID)
        {
            IQNotificationSettingsDA iQNotificationSettingsDA = (IQNotificationSettingsDA)DataAccessFactory.GetDataAccess(DataAccessType.IQNotification);
            return iQNotificationSettingsDA.SelectIQNotifcationsBySearchRequestID(ClientGuid, SearchRequestID);
        }

        public IQNotifationSettingsModel SelectIQNotifcationsByID(Int64 p_ID)
        {
            IQNotificationSettingsDA iQNotificationSettingsDA = (IQNotificationSettingsDA)DataAccessFactory.GetDataAccess(DataAccessType.IQNotification);
            return iQNotificationSettingsDA.SelectIQNotifcationsByID(p_ID);
        }

        public IQNotifationSettings_DropDown GetIQNotificationDropDown(Guid p_Clientguid)
        {
            IQNotificationSettingsDA iQNotificationSettingsDA = (IQNotificationSettingsDA)DataAccessFactory.GetDataAccess(DataAccessType.IQNotification);
            return iQNotificationSettingsDA.GetIQNotificationDropDown(p_Clientguid);
        }

        public List<IQNotifationSettingsModel> SelectIQNotifcationsByClientGuid(Guid p_Clientguid, int p_PageNumner, int p_PageSize, out int p_TotalResults)
        {
            IQNotificationSettingsDA iQNotificationSettingsDA = (IQNotificationSettingsDA)DataAccessFactory.GetDataAccess(DataAccessType.IQNotification);
            return iQNotificationSettingsDA.SelectIQNotifcationsByClientGuid(p_Clientguid, p_PageNumner, p_PageSize, out p_TotalResults);
        }

        public string InsertIQNotificationSettings(IQNotifationSettingsModel p_IQNotifationSettingsModel, Guid p_ClientGuid)
        {
            string strMediumTypeList = null;

            SearchRequestIDList searchRequestList = new SearchRequestIDList();
            searchRequestList.SearchRequestID = p_IQNotifationSettingsModel.SearchRequestList;
            string IQAgentXml = Shared.Utility.CommonFunctions.SerializeToXml(searchRequestList);


            EmailAddressList emailAddressList = new EmailAddressList();
            emailAddressList.EmailAddress = p_IQNotifationSettingsModel.Notification_Address;
            string emailAddressXml = Shared.Utility.CommonFunctions.SerializeToXml(emailAddressList);


            if (p_IQNotifationSettingsModel != null && p_IQNotifationSettingsModel.MediaTypeList != null && p_IQNotifationSettingsModel.MediaTypeList.Count > 0)
            {
                MediaTypeList mediaTypeList = new MediaTypeList();
                mediaTypeList.MediaType = p_IQNotifationSettingsModel.MediaTypeList;
                strMediumTypeList = Shared.Utility.CommonFunctions.SerializeToXml(mediaTypeList);
            }
            else
            {
                throw new ArgumentException("MediaTypeList cannot be empty.");
            }

            IQNotificationSettingsDA iQNotificationSettingsDA = (IQNotificationSettingsDA)DataAccessFactory.GetDataAccess(DataAccessType.IQNotification);
            return iQNotificationSettingsDA.InsertIQNotificationSettings(p_IQNotifationSettingsModel, strMediumTypeList, IQAgentXml, emailAddressXml, p_ClientGuid);
        }

        public string UdateIQNotificationSettings(IQNotifationSettingsModel p_IQNotifationSettingsModel, Guid p_ClientGuid)
        {
            string strMediumTypeList = null;

            SearchRequestIDList searchRequestList = new SearchRequestIDList();
            searchRequestList.SearchRequestID = p_IQNotifationSettingsModel.SearchRequestList;
            string IQAgentXml = Shared.Utility.CommonFunctions.SerializeToXml(searchRequestList);

            EmailAddressList emailAddressList = new EmailAddressList();
            emailAddressList.EmailAddress = p_IQNotifationSettingsModel.Notification_Address;
            string emailAddressXml = Shared.Utility.CommonFunctions.SerializeToXml(emailAddressList);

            if (p_IQNotifationSettingsModel != null && p_IQNotifationSettingsModel.MediaTypeList != null && p_IQNotifationSettingsModel.MediaTypeList.Count > 0)
            {
                MediaTypeList mediaTypeList = new MediaTypeList();
                mediaTypeList.MediaType = p_IQNotifationSettingsModel.MediaTypeList;
                strMediumTypeList = Shared.Utility.CommonFunctions.SerializeToXml(mediaTypeList);
            }
            else
            {
                throw new ArgumentException("MediaTypeList cannot be empty.");
            }

            IQNotificationSettingsDA iQNotificationSettingsDA = (IQNotificationSettingsDA)DataAccessFactory.GetDataAccess(DataAccessType.IQNotification);
            return iQNotificationSettingsDA.UdateIQNotificationSettings(p_IQNotifationSettingsModel, strMediumTypeList, IQAgentXml, emailAddressXml, p_ClientGuid);
        }

        public int DeleteIQNotification(int p_IQAgentNotificationKey, Guid p_ClientGuid)
        {
            IQNotificationSettingsDA iQNotificationSettingsDA = (IQNotificationSettingsDA)DataAccessFactory.GetDataAccess(DataAccessType.IQNotification);
            return iQNotificationSettingsDA.DeleteIQNotification(p_IQAgentNotificationKey, p_ClientGuid);
        }

    }
}
