using System;
using System.Collections.Generic;
using IQMedia.Model;
using IQMedia.Data;
using IQMedia.Logic.Base;
using System.Xml.Linq;
using System.Security.Authentication;

namespace IQMedia.Web.Logic
{
    public class CustomerLogic : IQMedia.Web.Logic.Base.ILogic
    {
        public IEnumerable<_CustomerModel> GetCustomerList()
        {
            CustomerDA customerDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            IEnumerable<_CustomerModel> customerList = customerDA.GetAllCustomers();

            return customerList;
        }

        public CustomerModel CheckAuthentication(string email, string password)
        {
            CustomerDA customerDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            CustomerModel customer = customerDA.CheckAuthentication(email, password);

            return customer;
        }

        public CustomerModel GetClientGUIDByCustomerGUID(System.Guid p_CustomerGUID)
        {
            CustomerDA customerDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            CustomerModel customer = customerDA.GetClientGUIDByCustomerGUID(p_CustomerGUID);

            return customer;
        }

        public List<CustomerRoleModel> GetCustomerRoles(Guid CustomerGuid)
        {
            CustomerDA customerDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            return customerDA.GetCustomerRoles(CustomerGuid);
        }

        public List<CustomerModel> GetCustomerDetailByEmailList(XDocument p_EmailXML)
        {
            CustomerDA customerDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            return customerDA.GetCustomerDetailByEmailList(p_EmailXML);
        }

        public Boolean GetDownloadRoleByCustomerGuid(Guid p_CustomerGuid)
        {
            CustomerDA customerDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            return customerDA.GetDownloadRoleByCustomerGuid(p_CustomerGuid);
        }

        public List<CustomerModel> GetAllCustomerWithRole(string p_ClientName, string p_CustomerName, int p_PageNumner, int p_PageSize, out int p_TotalResults)
        {
            CustomerDA customerDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            return customerDA.GetAllCustomerWithRole(p_ClientName,p_CustomerName, p_PageNumner, p_PageSize, out p_TotalResults);
        }

        public CustomerModel CheckAuthenticationByClient(Int64 masterCustomerId, Int64 clientId)
        {
            CustomerDA customerDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            return customerDA.CheckAuthenticationByClient(masterCustomerId, clientId);
        }

        public string InsertCustomer(CustomerModel p_Customer, string p_Roles,string p_DefaultCategory)
        {
            CustomerDA customerDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            return customerDA.InsertCustomer(p_Customer, p_Roles, p_DefaultCategory);
        }

        public string UpdateCustomer(CustomerModel p_Customer, string p_Roles, string p_DefaultCategory)
        {
            CustomerDA customerDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            return customerDA.UpdateCustomer(p_Customer, p_Roles, p_DefaultCategory);
        }

        public CustomerModel GetCustomerWithRoleByCustomerID(Int64 p_CustomerID)
        {
            CustomerDA customerDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            return customerDA.GetCustomerWithRoleByCustomerID(p_CustomerID);
        }

        public Customer_DropDown GetAllDropDown()
        {
            CustomerDA customerDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            return customerDA.GetAllDropDown();
        }

        public string DeleteCustomer(Int64 p_CustomerKey)
        {
            CustomerDA customerDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            return customerDA.DeleteCustomer(p_CustomerKey);
        }

        public bool CheckAuthentication(string p_LoginID,string p_Password,int p_MaxPasswordAttempts)
        {
            var isAuthenticated = false;

            CustomerDA customerDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);

            var customer = customerDA.GetCustomerDetailsForAuthentication(p_LoginID);

            if (customer!=null)
            {
                var maxPasswordAttempts = p_MaxPasswordAttempts<=0 ? 5 : p_MaxPasswordAttempts;

                if(customer.PasswordAttempts>=p_MaxPasswordAttempts)
                {
                    throw new AuthenticationException("You have exceeded the maximum number of attempts to authenticate with your credentials. contact: support@iqmediacorp.com");
                }

                isAuthenticated= IQMedia.Security.Authentication.VerifyPassword(p_Password, customer.Password);

                if (!isAuthenticated || customer.PasswordAttempts>0)
                {
                    customerDA.UpdatePasswordAttempts(p_LoginID, isAuthenticated);        
                }
                
            }

            return isAuthenticated;
        }

        public CustomerModel GetCustomerDetailsByLoginID(string p_LoginID)
        {
            var custDA = (CustomerDA) DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            return custDA.GetCustomerDetailsByLoginID(p_LoginID);
        }

        public void UpdatePasswordByLoginID(string p_LoginID, string p_Pwd)
        {
            var custDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            custDA.UpdatePasswordByLoginID(p_LoginID,p_Pwd);
        }

        public bool ValidateLoginIDForRsetPwd(string p_LoginID, out Int16 rsetPwdEmailCount, out string email)
        {
            var custDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            return custDA.ValidateLoginIDForRsetPwd(p_LoginID, out rsetPwdEmailCount, out email);
        }

        public Int64 InsertRsetPwd(string p_LoginID, short p_LinkTimeout,string p_Token)
        {
            var dateExpired = DateTime.Now.AddMinutes(p_LinkTimeout);

            var custDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            return custDA.InsertRsetPwd(p_LoginID, dateExpired, p_Token);
        }

        public void UpdateRsetPwdEmailCount(string p_LoginID)
        {
            var custDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            custDA.UpdateRsetPwdEmailCount(p_LoginID);
        }

        public CustomerRsetPwdModel GetRsetPwd(string p_LoginID)
        {
            var custDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            return custDA.GetRsetPwd(p_LoginID);
        }

        public void UpdateRsetPwdNPassword(long p_ID, Guid p_CustomerGUID, string p_Pwd)
        {
            var pwd = IQMedia.Security.Authentication.GetHashPassword(p_Pwd);

            var custDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            custDA.UpdateRsetPwdNPassword(p_ID, p_CustomerGUID, pwd);
        }

        public int ResetPasswordAttempts(long p_CustomerKey)
        {
            var custDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            return custDA.ResetPasswordAttempts(p_CustomerKey);
        }

        public bool GroupAddSubCustomer(Int64 p_GrpID, Int64 p_MCID, Int64 p_SCID, Int64 p_MasterCustomerID, Int64 p_SubCustomerID, Guid p_CustomerGUID)
        {
            var custDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            return custDA.GroupAddSubCustomer(p_GrpID, p_MCID, p_SCID, p_MasterCustomerID, p_SubCustomerID, p_CustomerGUID);
        }

        public List<CustomerModel> GroupGetSubCustomerByCustomer(Int64 p_MasterCustomerID)
        {
            var custDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            return custDA.GroupGetSubCustomerByCustomer(p_MasterCustomerID);
        }

        public bool GroupRemoveSubCustomer(Int64 p_MasterCustomerID, Int64 p_SubCustomerID, Guid p_CustomerGUID)
        {
            var custDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            return custDA.GroupRemoveSubCustomer(p_MasterCustomerID,p_SubCustomerID,p_CustomerGUID);
        }

        public void AddCustomerToAnewstip(long customerKey, string AnewstipUserID)
        {
            var custDA = (CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.Customer);
            custDA.AddCustomerToAnewstip(customerKey, AnewstipUserID);
        }
    }
}