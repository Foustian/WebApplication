using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using IQMedia.Model;
using System.Data;

namespace IQMedia.Data
{
    public class IQTrack_LicenseClickDA : IDataAccess
    {
        public string Insert(Int64 p_CustomerID, string p_Event, string p_MOUrl, Int16 p_IQLicense)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@CustomerID", DbType.Int64, p_CustomerID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MOURL", DbType.String, p_MOUrl, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Event", DbType.String, p_Event, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IQLicense", DbType.Int16, p_IQLicense, ParameterDirection.Input));

                string _Result = DataAccess.ExecuteNonQuery("usp_v4_IQTrack_LicenseClick_Insert", dataTypeList);
                return _Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
