using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using System.Data;
using IQMedia.Model;

namespace IQMedia.Data
{
    public class UtilityDA : IDataAccess
    {
        public void InsertException(IQMediaGroupExceptions ex)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ExceptionStackTrace", DbType.String, ex.ExceptionStackTrace, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ExceptionMessage", DbType.String, ex.ExceptionMessage, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CreatedBy", DbType.String, ex.CreatedBy, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CreatedDate", DbType.DateTime, ex.CreatedDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerGuid", DbType.Guid, ex.CustomerGuid, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@IQMediaGroupExceptionKey", DbType.Int64, ex.IQMediaGroupExceptionsKey, ParameterDirection.Output));

                _Result = DataAccess.ExecuteNonQuery("usp_IQMediaGroupExceptions_Insert", _ListOfDataType);

            }
            catch (Exception)
            {
            }
        }

        public string InsertActionLog(IQLog_UserActionsModel p_IQLog_UserActionsModel)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@SessionID", DbType.String, p_IQLog_UserActionsModel.SessionID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerID", DbType.Int64, p_IQLog_UserActionsModel.CustomerID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ActionName", DbType.String, p_IQLog_UserActionsModel.ActionName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageName", DbType.String, p_IQLog_UserActionsModel.PageName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RequestParameters", DbType.String, p_IQLog_UserActionsModel.RequestParameters, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RequestDateTime", DbType.DateTime, p_IQLog_UserActionsModel.RequestDateTime, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IPAddress", DbType.String, p_IQLog_UserActionsModel.IPAddress, ParameterDirection.Input));
                _Result = DataAccess.ExecuteNonQuery("usp_v4_IQLog_UserActions_Insert", _ListOfDataType);

                return _Result;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetFeedsReportLimit(Guid clientGuid)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));
                IDataReader reader = DataAccess.GetDataReader("usp_v4_IQClient_CustomSettings_SelectMaxFeedsLimit", _ListOfDataType);

                while (reader.Read())
                {
                    _Result = Convert.ToString(reader["MaxFeedsReportItems"]);
                }

                return _Result;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool GetForceCategorySelection(Guid clientGuid)
        {
            try
            {
                bool _Result = true;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));
                IDataReader reader = DataAccess.GetDataReader("usp_v4_IQClient_CustomSettings_SelectForceCategorySelection", _ListOfDataType);

                while (reader.Read())
                {
                    _Result = reader["ForceCategorySelection"].ToString() == "1";
                }

                return _Result;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQClient_CustomSettingsModel GetDiscoveryReportAndExportLimit(Guid clientGuid)
        {
            try
            {
                IQClient_CustomSettingsModel _Result = new IQClient_CustomSettingsModel();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));
                IDataReader reader = DataAccess.GetDataReader("usp_v4_IQClient_CustomSettings_SelectMaxDiscoveryLimit", _ListOfDataType);

                while (reader.Read())
                {
                    _Result.v4MaxDiscoveryReportItems = Convert.ToInt32(reader["v4MaxDiscoveryReportItems"]);
                    _Result.v4MaxDiscoveryExportItems = Convert.ToInt32(reader["v4MaxDiscoveryExportItems"]);
                    _Result.v4MaxDiscoveryHistory = Convert.ToInt32(reader["v4MaxDiscoveryHistory"]);
                }

                return _Result;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public string InsertPMGSearchLog(SearchLogModel p_SearchLog)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerID", DbType.Int32, p_SearchLog.CustomerID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchType", DbType.String, p_SearchLog.SearchType, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RequestXML", DbType.Xml, p_SearchLog.RequestXML, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ErrorResponseXML", DbType.String, p_SearchLog.ErrorResponseXML, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@PMGSearchLogKey", DbType.Int32, p_SearchLog.SearchLogKey, ParameterDirection.Output));

                _Result = DataAccess.ExecuteNonQuery("usp_PMGSearchLog_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string InsertFeedsSearchLog(SearchLogModel p_SearchLog)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerID", DbType.Int32, p_SearchLog.CustomerID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchType", DbType.String, p_SearchLog.SearchType, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RequestXML", DbType.Xml, p_SearchLog.RequestXML, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ErrorResponseXML", DbType.String, p_SearchLog.ErrorResponseXML, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@FeedsSearchLogKey", DbType.Int32, p_SearchLog.SearchLogKey, ParameterDirection.Output));

                _Result = DataAccess.ExecuteNonQuery("usp_FeedsSearchLog_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
