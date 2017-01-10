using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using IQMedia.Data;
using IQMedia.Logic.Base;

namespace IQMedia.Web.Logic
{
    public class SessionLogic : Base.ILogic
    {
        public string InsertSession(SessionModel p_SessionModel)
        {
           return ((SessionDA)DataAccessFactory.GetDataAccess(DataAccessType.Session)).InsertSession(p_SessionModel);
        }

        public List<SessionModel> GetAll(string p_SearchTerm, string p_SortColumn, bool p_IsAsc)
        {
            return ((SessionDA)DataAccessFactory.GetDataAccess(DataAccessType.Session)).GetAll(p_SearchTerm, p_SortColumn, p_IsAsc);
        }

        public string DeleteBySessionID(string p_SessionID)
        {
            return ((SessionDA)DataAccessFactory.GetDataAccess(DataAccessType.Session)).DeleteBySessionID(p_SessionID);
        }

        public string DeleteByLoginID(string p_LoginID)
        {
            return ((SessionDA)DataAccessFactory.GetDataAccess(DataAccessType.Session)).DeleteByLoginID(p_LoginID);
        }

        public List<ActiveUser> GetActiveUsersFromDB()
        {
            List<ActiveUser> activeUsers = null;

            Dictionary<string,object> dicObj = ((SessionDA)DataAccessFactory.GetDataAccess(DataAccessType.Session)).GetActiveUsersNRoles();

            activeUsers = (List<ActiveUser>)dicObj["User"];

            List<CustomerRoleModel> roles= (List<CustomerRoleModel>)dicObj["Role"];

            foreach (ActiveUser user in activeUsers)
            {
                IEnumerable<CustomerRoleModel> customerRoles = roles.Where(r => r.CustomerID == user.CustomerKey);

                AssingRolesToUser(user, customerRoles);
            }

            return activeUsers;
        }

        public ActiveUser GetUserBySessionID(string p_SessionID)
        {
            ActiveUser user = null;

            Dictionary<string, object> dicObj = ((SessionDA)DataAccessFactory.GetDataAccess(DataAccessType.Session)).GetUserBySessionID(p_SessionID);

            user = (ActiveUser)dicObj["User"];

            List<CustomerRoleModel> roles = (List<CustomerRoleModel>)dicObj["Role"];

            if (user != null)
            {
                AssingRolesToUser(user, roles);
            }

            return user;
        }

        private void AssingRolesToUser(ActiveUser user, IEnumerable<CustomerRoleModel> customerRoles)
        {
            foreach (CustomerRoleModel customerrole in customerRoles)
            {
                IQMedia.Shared.Utility.CommonFunctions.Roles role = (IQMedia.Shared.Utility.CommonFunctions.Roles)Enum.Parse(typeof(IQMedia.Shared.Utility.CommonFunctions.Roles), customerrole.RoleName);

                switch (role)
                {
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.v4Feeds:
                        user.Isv4FeedsAccess = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.v4Discovery:
                        user.Isv4DiscoveryAccess = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.v4Library:
                        user.Isv4LibraryAccess = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.v4Timeshift:
                        user.Isv4TimeshiftAccess = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.v4TAds:
                        user.Isv4TAdsAccess = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.v5Ads:
                        user.Isv5AdsAccess = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.v4Dashboard:
                        user.Isv4DashboardAccess = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.v4LibraryDashboard:
                        user.Isv4LibraryDashboardAccess = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.v4Radio:
                        user.Isv4TimeshiftRadioAccess = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.v4Setup:
                        user.Isv4SetupAccess = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.v5LR:
                        user.isv5LRAccess = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.GlobalAdminAccess:
                        user.IsGlobalAdminAccess = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.v4UGC:
                        user.Isv4UGCAccess = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.v4IQAgentSetup:
                        user.Isv4IQAgentAccess = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.v4TV:
                        user.Isv4TV = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.v4NM:
                        user.Isv4NM = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.v4SM:
                        user.Isv4SM = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.v4TW:
                        user.Isv4TW = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.v4TM:
                        user.Isv4TM = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.UGCAutoClip:
                        user.IsUgcAutoClip = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.UGCDownload:
                        user.Isv4UGCDownload = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.UGCUploadEdit:
                        user.Isv4UGCUploadEdit = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.v4Group:
                        user.Isv4Group = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.v4CustomImage:
                        user.Isv4CustomImage = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.CompeteData:
                        user.IsCompeteData = true;
                        break;
                    case IQMedia.Shared.Utility.CommonFunctions.Roles.NielsenData:
                        user.IsNielsenData = true;
                        break;
                    case Shared.Utility.CommonFunctions.Roles.v4BLPM:
                        user.Isv4BLPM = true;
                        break;
                    case Shared.Utility.CommonFunctions.Roles.NewsRight:
                        user.IsNewsRights = true;
                        break;
                    case Shared.Utility.CommonFunctions.Roles.v4CustomSettings:
                        user.Isv4CustomSettings = true;
                        break;
                    case Shared.Utility.CommonFunctions.Roles.v4DiscoveryLite:
                        user.Isv4DiscoveryLiteAccess = true;
                        break;
                    case Shared.Utility.CommonFunctions.Roles.fliQAdmin:
                        user.IsfliQAdmin = true;
                        break;
                    case Shared.Utility.CommonFunctions.Roles.v4PQ:
                        user.Isv4PQ = true;
                        break;
                    case Shared.Utility.CommonFunctions.Roles.MediaRoomContributor:
                        user.IsMediaRoomContributor = true;
                        break;
                    case Shared.Utility.CommonFunctions.Roles.MediaRoomEditor:
                        user.IsMediaRoomEditor = true;
                        break;
                    case Shared.Utility.CommonFunctions.Roles.v4Google:
                        user.Isv4Google = true;
                        break;
                    case Shared.Utility.CommonFunctions.Roles.TimeshiftFacet:
                        user.IsTimeshiftFacet = true;
                        break;
                    case Shared.Utility.CommonFunctions.Roles.ShareTV:
                        user.IsShareTV = true;
                        break;
                    case Shared.Utility.CommonFunctions.Roles.ThirdPartyData:
                        user.IsThirdPartyData = true;
                        break;
                    case Shared.Utility.CommonFunctions.Roles.ClientSpecificData:
                        user.IsClientSpecificData = true;
                        break;
                    case Shared.Utility.CommonFunctions.Roles.SMOther:
                        user.IsSMOther = true;
                        break;
                    case Shared.Utility.CommonFunctions.Roles.FB:
                        user.IsFB = true;
                        break;
                    case Shared.Utility.CommonFunctions.Roles.IG:
                        user.IsIG = true;
                        break;
                    case Shared.Utility.CommonFunctions.Roles.BL:
                        user.IsBL = true;
                        break;
                    case Shared.Utility.CommonFunctions.Roles.FO:
                        user.IsFO = true;
                        break;
                    case Shared.Utility.CommonFunctions.Roles.PR:
                        user.IsPR = true;
                        break;
                    case Shared.Utility.CommonFunctions.Roles.LN:
                        user.IsLN = true;
                        break;
                    case Shared.Utility.CommonFunctions.Roles.ConnectAccess:
                        user.IsConnectAccess = true;
                        break;
                    case Shared.Utility.CommonFunctions.Roles.ExternalRuleEditor:
                        user.IsExternalRuleEditor = true;
                        break;
                    default:
                        break;
                }
            }
        }

        public void Update(SessionModel p_SessionModel)
        {
            ((SessionDA)DataAccessFactory.GetDataAccess(DataAccessType.Session)).Update(p_SessionModel);
        }

        public void DeleteAll()
        {
            ((SessionDA)DataAccessFactory.GetDataAccess(DataAccessType.Session)).DeleteAll();
        }
    }
}
