using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using IQMedia.Model;
using System.Data;
using IQMedia.Shared.Utility;


namespace IQMedia.Data
{
    public class IQ_SMSCampaignDA : IDataAccess
    {
        public string UpdateIQ_SMSCampaignIsActive(Int64 p_SearchRequestID, Int64 p_HubSpotID, bool p_IsActivated)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@SearchRequestID", DbType.Int64, p_SearchRequestID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@HubSpotID", DbType.Int32, p_HubSpotID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActivated", DbType.Boolean, p_IsActivated, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@AffectedResults", DbType.Int32, 0, ParameterDirection.Output));

                _Result = DataAccess.ExecuteNonQuery("usp_v4_IQ_SMSCampaign_UpdateIsActive", _ListOfDataType);

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
