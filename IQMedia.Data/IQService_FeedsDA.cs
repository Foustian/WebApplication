using System;
using System.Collections.Generic;
using System.Data;
using IQMedia.Data.Base;

namespace IQMedia.Data
{
    public class IQService_FeedsDA : IDataAccess
    {
        public int InsertFeedsExport(Guid p_CustomerGuid, Boolean p_IsSelectAll, string p_SortType, string p_SearchCriteria, string p_ArticleXml, string p_Title, bool p_GetTVUrl)
        {
            try
            {
                string _result = string.Empty;
                int _intResult = 0;

                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@CustomerGuid", DbType.Guid, p_CustomerGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsSelectAll", DbType.Boolean, p_IsSelectAll, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchCriteria", DbType.String, p_SearchCriteria, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ArticleXml", DbType.String, p_ArticleXml, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SortType", DbType.String, p_SortType, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Title", DbType.String, p_Title, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@GetTVUrl", DbType.Boolean, p_GetTVUrl, ParameterDirection.Input));

                _result = Convert.ToString(DataAccess.ExecuteScalar("usp_v5_IQService_FeedsExport_Insert", _ListOfDataType));

                return Int32.TryParse(_result, out _intResult) ? _intResult : 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
