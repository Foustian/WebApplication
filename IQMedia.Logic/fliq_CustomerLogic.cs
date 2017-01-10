using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using IQMedia.Data;
using IQMedia.Logic.Base;

namespace IQMedia.Web.Logic
{
    public class fliq_CustomerLogic : IQMedia.Web.Logic.Base.ILogic
    {
        public string Insertfliq_Customer(fliq_CustomerModel p_Customer, string p_DefaultCategory)
        {
            fliq_CustomerDA customerDA = (fliq_CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.fliq_Customer);
            return customerDA.Insertfliq_Customer(p_Customer, p_DefaultCategory);
        }

        public string Updatefliq_Customer(fliq_CustomerModel p_Customer)
        {
            fliq_CustomerDA customerDA = (fliq_CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.fliq_Customer);
            return customerDA.Updatefliq_Customer(p_Customer);
        }

        public List<fliq_CustomerModel> GetAllfliq_Customer(string p_ClientName, string p_CustomerName, int p_PageNumner, int p_PageSize, out int p_TotalResults)
        {
            fliq_CustomerDA customerDA = (fliq_CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.fliq_Customer);
            return customerDA.GetAllfliq_Customer(p_ClientName,p_CustomerName, p_PageNumner, p_PageSize, out p_TotalResults);
        }

        public Customer_DropDown GetAllDropDown()
        {
            fliq_CustomerDA customerDA = (fliq_CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.fliq_Customer);
            return customerDA.GetAllDropDown();
        }

        public string Deletefliq_Customer(Int64 p_CustomerKey)
        {
            fliq_CustomerDA customerDA = (fliq_CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.fliq_Customer);
            return customerDA.Deletefliq_Customer(p_CustomerKey);
        }

        public fliq_CustomerModel Gefliq_CustomerByCustomerID(Int64 p_CustomerID)
        {
            fliq_CustomerDA customerDA = (fliq_CustomerDA)DataAccessFactory.GetDataAccess(DataAccessType.fliq_Customer);
            return customerDA.Gefliq_CustomerByCustomerID(p_CustomerID);
        }
    }
}
