using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using System.Data;

namespace IQMedia.Data
{
    public class IQ_QVCDataDA : IDataAccess
    {
        public List<string> GetQVCStations(Int16 p_DataType)
        {
            try
            {
                List<string> lstOfStations = new List<string>();
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@DataType", DbType.Int16, p_DataType, ParameterDirection.Input));
                DataSet _Result = DataAccess.GetDataSet("usp_v4_IQ_KantorData_SelectStations", _ListOfDataType);
                if (_Result.Tables.Count > 0 && _Result.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in _Result.Tables[0].Rows)
                    {
                        if (_Result.Tables[0].Columns.Contains("Station_ID") && !dr["Station_ID"].Equals(DBNull.Value))
                        {
                            lstOfStations.Add(dr["Station_ID"].ToString());
                        }
                    }
                }

                return lstOfStations;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<string> GetQVCIQ_CC_Keys(string p_StationID, DateTime p_DataTime, Int16 p_DataType)
        {
            try
            {
                List<string> lstOfIQ_CC_Keys = new List<string>();
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@StationID", DbType.String, p_StationID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DateTime", DbType.DateTime, p_DataTime, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DataType", DbType.Int16, p_DataType, ParameterDirection.Input));
                DataSet _Result = DataAccess.GetDataSet("usp_v4_IQ_KantorData_SelectIQCCKeyByStationIDAndDateTime", _ListOfDataType);
                if (_Result.Tables.Count > 0 && _Result.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in _Result.Tables[0].Rows)
                    {
                        if (_Result.Tables[0].Columns.Contains("IQ_CC_Key") && !dr["IQ_CC_Key"].Equals(DBNull.Value))
                        {
                            lstOfIQ_CC_Keys.Add(dr["IQ_CC_Key"].ToString());
                        }
                    }
                }

                return lstOfIQ_CC_Keys;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetQVCAudienceDataByIQ_CC_Key(string p_IQ_CC_Key, Int16 p_DataType)
        {
            try
            {
                string audienceData = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@IQCCKEY", DbType.String, p_IQ_CC_Key, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DataType", DbType.Int16, p_DataType, ParameterDirection.Input));
                DataSet _Result = DataAccess.GetDataSet("usp_v4_IQ_KantorData_SelectAudienceByIQCCKey", _ListOfDataType);
                if (_Result.Tables.Count > 0 && _Result.Tables[0].Rows.Count > 0)
                {
                    audienceData = Convert.ToString(_Result.Tables[0].Rows[0][0]);
                }

                return audienceData;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
