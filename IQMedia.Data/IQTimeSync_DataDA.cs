using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using System.Data;
using IQMedia.Model;

namespace IQMedia.Data
{
    public class IQTimeSync_DataDA : IDataAccess
    {
        public List<IQTimeSync_DataModel> GetTimeSyncDataByIQCCKeyAndCustomerGuid(string IQ_CC_Key, Guid p_CustomerGuid)
        {
            try
            {
                List<IQTimeSync_DataModel> lstIQTimeSync_DataModel = new List<IQTimeSync_DataModel>();
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@IQ_CC_Key", DbType.String, IQ_CC_Key, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerGuid", DbType.Guid, p_CustomerGuid, ParameterDirection.Input));

                DataSet _Result = DataAccess.GetDataSet("usp_v4_IQTimeSync_Data_SelectByIQCCKeyAndCustomerGuid", _ListOfDataType);
                if (_Result.Tables.Count > 0 && _Result.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in _Result.Tables[0].Rows)
                    {
                        IQTimeSync_DataModel objIQTimeSync_DataModel = new IQTimeSync_DataModel();

                        if (_Result.Tables[0].Columns.Contains("Data") && !dr["Data"].Equals(DBNull.Value))
                        {
                            objIQTimeSync_DataModel.Data = Convert.ToString(dr["Data"]);
                        }

                        if (_Result.Tables[0].Columns.Contains("Name") && !dr["Name"].Equals(DBNull.Value))
                        {
                            objIQTimeSync_DataModel.Type = Convert.ToString(dr["Name"]);
                        }

                        if (_Result.Tables[0].Columns.Contains("GraphStructure") && !dr["GraphStructure"].Equals(DBNull.Value))
                        {
                            GraphStructureModel graphStructureModel = new GraphStructureModel();
                            objIQTimeSync_DataModel.GraphStructure = (GraphStructureModel)Newtonsoft.Json.JsonConvert.DeserializeObject(Convert.ToString(dr["GraphStructure"]), graphStructureModel.GetType());
                        }

                        lstIQTimeSync_DataModel.Add(objIQTimeSync_DataModel);
                    }

                }

                return lstIQTimeSync_DataModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<IQTimeSync_DataModel> GetClipTimeSyncDataByClipGuidAndCustomerGuid(Guid p_ClipGuid, Guid p_CustomerGuid)
        {
            try
            {
                List<IQTimeSync_DataModel> lstIQTimeSync_DataModel = new List<IQTimeSync_DataModel>();
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClipGuid", DbType.Guid, p_ClipGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerGuid", DbType.Guid, p_CustomerGuid, ParameterDirection.Input));

                DataSet _Result = DataAccess.GetDataSet("usp_v4_IQTimeSync_ClipData_SelectByClipGuidAndCustomerGuid", _ListOfDataType);
                if (_Result.Tables.Count > 0 && _Result.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in _Result.Tables[0].Rows)
                    {
                        IQTimeSync_DataModel objIQTimeSync_DataModel = new IQTimeSync_DataModel();

                        if (_Result.Tables[0].Columns.Contains("Data") && !dr["Data"].Equals(DBNull.Value))
                        {
                            objIQTimeSync_DataModel.Data = Convert.ToString(dr["Data"]);
                        }

                        if (_Result.Tables[0].Columns.Contains("Name") && !dr["Name"].Equals(DBNull.Value))
                        {
                            objIQTimeSync_DataModel.Type = Convert.ToString(dr["Name"]);
                        }

                        if (_Result.Tables[0].Columns.Contains("GraphStructure") && !dr["GraphStructure"].Equals(DBNull.Value))
                        {
                            GraphStructureModel graphStructureModel = new GraphStructureModel();
                            objIQTimeSync_DataModel.GraphStructure = (GraphStructureModel)Newtonsoft.Json.JsonConvert.DeserializeObject(Convert.ToString(dr["GraphStructure"]), graphStructureModel.GetType());
                        }

                        lstIQTimeSync_DataModel.Add(objIQTimeSync_DataModel);
                    }

                }

                return lstIQTimeSync_DataModel;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
