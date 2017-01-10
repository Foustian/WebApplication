using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using System.Data;

namespace IQMedia.Data
{
    public class IQDiscovery_ToFeedsDA : IDataAccess
    {
        public string InsertIQDiscovery_ToFeeds(Guid p_ClientGuid, Guid p_CustomerGuid, string p_ArticleXml, Int64 p_SearchRequestID, Int64? p_ReportID)
        {
            try
            {
                string _result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerGuid", DbType.Guid, p_CustomerGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchRequestID", DbType.Int64, p_SearchRequestID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ReportID", DbType.Int64, p_ReportID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@MediaXml", DbType.String, p_ArticleXml, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ID", DbType.Int64, 0, ParameterDirection.Output));

                _result = DataAccess.ExecuteNonQuery("usp_v4_IQDiscovery_ToFeeds_Insert", _ListOfDataType);

                return _result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
