using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using IQMedia.Data.Base;
using System.Data;

namespace IQMedia.Data
{
    public class SessionDA : IDataAccess
    {
        public string InsertSession(SessionModel p_SessionModel)
        {

            List<DataType> dataTypeList = new List<DataType>();

            dataTypeList.Add(new DataType("@SessionID", DbType.String, p_SessionModel.SessionID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@LoginID", DbType.String, p_SessionModel.LoginID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@SessionTimeout", DbType.DateTime, p_SessionModel.SessionTimeOut, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@LastAccessTime", DbType.DateTime, p_SessionModel.LastAccessTime, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@Server", DbType.String, p_SessionModel.Server, ParameterDirection.Input));


            string _Result = DataAccess.ExecuteNonQuery("usp_v4_IQSession_Insert", dataTypeList);
            return _Result;

        }

        public List<SessionModel> GetAll(string p_SearchTerm, string p_SortColumn, bool p_IsAsc)
        {
            List<DataType> dataTypeList = new List<DataType>();

            DataSet ds = DataAccess.GetDataSet("usp_v4_IQSession_SelectAll", dataTypeList);

            return FillSessionList(ds);
        }

        private List<SessionModel> FillSessionList(DataSet ds)
        {
            List<SessionModel> sessionList = new List<SessionModel>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                SessionModel session = new SessionModel();

                session.SessionID = Convert.ToString(row["SessionID"]);
                session.LoginID = Convert.ToString(row["LoginID"]);
                session.LastAccessTime = Convert.ToDateTime(row["LastAccessTime"]);
                session.SessionTimeOut = Convert.ToDateTime(row["SessionTimeOut"]);
                session.Server = Convert.ToString(row["Server"]);

                sessionList.Add(session);
            }

            return sessionList;
        }

        public string DeleteBySessionID(string p_SessionID)
        {
            List<DataType> dataTypeList = new List<DataType>();

            dataTypeList.Add(new DataType("@SessionID", DbType.String, p_SessionID, ParameterDirection.Input));

            string result = DataAccess.ExecuteNonQuery("usp_v4_IQSession_DeleteBySessionID", dataTypeList);

            return result;
        }

        public string DeleteByLoginID(string p_LoginID)
        {
            List<DataType> dataTypeList = new List<DataType>();

            dataTypeList.Add(new DataType("@LoginID", DbType.String, p_LoginID, ParameterDirection.Input));

            string result = DataAccess.ExecuteNonQuery("usp_v4_IQSession_DeleteByLoginID", dataTypeList);

            return result;
        }

        public Dictionary<string, object> GetActiveUsersNRoles()
        {
            Dictionary<string, object> dicObj = new Dictionary<string, object>();

            List<ActiveUser> activeUserList = new List<ActiveUser>();
            List<CustomerRoleModel> customerRoleList = new List<CustomerRoleModel>();            

            DataSet ds = DataAccess.GetDataSet("usp_v4_Customer_SelectBySession", new List<DataType>());

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ActiveUser user = new ActiveUser();

                FillActiveUser(dr, user);

                activeUserList.Add(user);
            }

            customerRoleList = FillUserRole(ds.Tables[1]);

            dicObj.Add("User", activeUserList);
            dicObj.Add("Role", customerRoleList);

            return dicObj;
        }

        public Dictionary<string, object> GetUserBySessionID(string p_SessionID)
        {
            Dictionary<string, object> dicObj = new Dictionary<string, object>();

            List<DataType> dataTypeList = new List<DataType>();

            dataTypeList.Add(new DataType("@SessionID", DbType.String, p_SessionID, ParameterDirection.Input));

            DataSet ds = DataAccess.GetDataSet("usp_v4_IQSession_SelectBySessionID", dataTypeList);

            ActiveUser user = null;
            List<CustomerRoleModel> userRoleList = null;

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                user = new ActiveUser();

                FillActiveUser(ds.Tables[0].Rows[0], user);

                userRoleList = FillUserRole(ds.Tables[1]);
            }

            dicObj.Add("User", user);
            dicObj.Add("Role", userRoleList);

            return dicObj;
        }

        private void FillActiveUser(DataRow dr, ActiveUser user)
        {
            user.CustomerKey = Convert.ToInt32(dr["CustomerKey"]);

            user.ClientID = Convert.ToInt32(dr["ClientID"]);

            user.ClientName = Convert.ToString(dr["ClientName"]);

            user.FirstName = Convert.ToString(dr["FirstName"]);

            user.LastName = Convert.ToString(dr["LastName"]);

            user.Email = Convert.ToString(dr["Email"]);

            user.LoginID = Convert.ToString(dr["LoginID"]);

            user.MultiLogin = Convert.ToBoolean(dr["MultiLogin"]);

            user.CustomerGUID = new Guid(Convert.ToString(dr["CustomerGUID"]));

            user.ClientGUID = new Guid(Convert.ToString(dr["ClientGUID"]));

            user.ClientPlayerLogoImage = Convert.ToString(dr["PlayerLogo"]);

            user.IsClientPlayerLogoActive = Convert.ToBoolean(dr["IsActivePlayerLogo"]);

            user.DefaultPage = Convert.ToString(dr["DefaultPage"]);

            user.AuthorizedVersion = !string.IsNullOrWhiteSpace(Convert.ToString(dr["AuthorizedVersion"])) ? Convert.ToInt16(dr["AuthorizedVersion"]) : (Int16?)null;

            user.TimeZone = Convert.ToString(dr["TimeZone"]);

            user.gmt = Convert.ToDecimal(dr["gmt"]);

            user.dst = Convert.ToDecimal(dr["dst"]);

            if (dr["MCID"] != DBNull.Value)
            {
                user.MCID = Convert.ToInt32(dr["MCID"]);
            }

            if (dr["MasterCustomerID"] != DBNull.Value)
            {
                user.MasterCustomerID = (Convert.ToInt32(dr["MasterCustomerID"]) == 0 ? user.CustomerKey : Convert.ToInt32(dr["MasterCustomerID"]));
            }

            user.SessionID = Convert.ToString(dr["SessionID"]);

            user.SessionTimeOut = Convert.ToDateTime(dr["SessionTimeOut"]);

            user.LastAccessTime = Convert.ToDateTime(dr["LastAccessTime"]);

            user.Server = Convert.ToString(dr["Server"]);
        }

        private List<CustomerRoleModel> FillUserRole(DataTable dt)
        {
            List<CustomerRoleModel> customerRoleList = new List<CustomerRoleModel>();

            foreach (DataRow dr in dt.Rows)
            {
                CustomerRoleModel customerRole = new CustomerRoleModel();

                if (!dr["RoleKey"].Equals(DBNull.Value))
                {
                    customerRole.RoleID = Convert.ToInt32(dr["RoleKey"]);
                }
                if (!dr["RoleName"].Equals(DBNull.Value))
                {
                    customerRole.RoleName = Convert.ToString(dr["RoleName"]);
                }
                if (!dr["CustomerKey"].Equals(DBNull.Value))
                {
                    customerRole.CustomerID = Convert.ToInt32(dr["CustomerKey"]);
                }

                customerRoleList.Add(customerRole);
            }

            return customerRoleList;
        }

        public void Update(SessionModel p_SessionModel)
        {
            List<DataType> dataTypeList = new List<DataType>();

            dataTypeList.Add(new DataType("@SessionID", DbType.String, p_SessionModel.SessionID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@LastAccessTime", DbType.DateTime, p_SessionModel.LastAccessTime, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@SessionTimeOut", DbType.DateTime, p_SessionModel.SessionTimeOut, ParameterDirection.Input));

            DataAccess.ExecuteNonQuery("usp_v4_IQSession_Update", dataTypeList);
        }

        public void DeleteAll()
        {
            List<DataType> dataTypeList = new List<DataType>();

            DataAccess.ExecuteNonQuery("usp_v4_IQSession_DeleteAll", dataTypeList);
        }
    }
}
