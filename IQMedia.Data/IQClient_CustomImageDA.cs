using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using IQMedia.Data.Base;
using System.Data;

namespace IQMedia.Data
{
    public class IQClient_CustomImageDA : IDataAccess
    {
        public string InsertIQClient_CustomImage(IQClient_CustomImageModel p_IQClient_CustomImageModel, bool p_IsReplace)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@Output", DbType.Int64, p_IQClient_CustomImageModel.ID, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_IQClient_CustomImageModel.ClientGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Image", DbType.String, p_IQClient_CustomImageModel.Location, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsDefault", DbType.Boolean, p_IQClient_CustomImageModel.IsDefault, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsDefaultEmail", DbType.Boolean, p_IQClient_CustomImageModel.IsDefaultEmail, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsReplace", DbType.Boolean, p_IsReplace, ParameterDirection.Input));

                _Result = DataAccess.ExecuteNonQuery("usp_v4_IQClient_CustomImage_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string DeleteIQClient_CustomImage(Int64 p_ID, Guid p_ClientGuid)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Output", DbType.Int32, string.Empty, ParameterDirection.Output));

                _Result = DataAccess.ExecuteNonQuery("usp_v4_IQClient_CustomImage_Delete", _ListOfDataType);

                return _Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<IQClient_CustomImageModel> GetAllIQClient_CustomImageByClientGuid(Guid p_ClientGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGuid, ParameterDirection.Input));

                DataSet dataSet = DataAccess.GetDataSet("usp_v4_IQClient_CustomImage_Select", dataTypeList);

                List<IQClient_CustomImageModel> lstIQClient_CustomImageModel = new List<IQClient_CustomImageModel>();

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        IQClient_CustomImageModel iQClient_CustomImageModel = new IQClient_CustomImageModel();

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            iQClient_CustomImageModel.ID = Convert.ToInt64(dr["ID"]);
                        }
                        if (!dr["Location"].Equals(DBNull.Value))
                        {
                            iQClient_CustomImageModel.Location = Convert.ToString(dr["Location"]);
                        }
                        if (!dr["ModifiedDate"].Equals(DBNull.Value))
                        {
                            iQClient_CustomImageModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                        }
                        if (!dr["IsDefault"].Equals(DBNull.Value))
                        {
                            iQClient_CustomImageModel.IsDefault = Convert.ToBoolean(dr["IsDefault"]);
                        }
                        if (!dr["IsDefaultEmail"].Equals(DBNull.Value))
                        {
                            iQClient_CustomImageModel.IsDefaultEmail = Convert.ToBoolean(dr["IsDefaultEmail"]);
                        }
                        lstIQClient_CustomImageModel.Add(iQClient_CustomImageModel);
                    }
                }

                return lstIQClient_CustomImageModel;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public int? CheckForImageCopy(string p_Image, Guid p_ClientGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@Image", DbType.String, p_Image, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGuid, ParameterDirection.Input));

                DataSet dataSet = DataAccess.GetDataSet("usp_v4_IQClient_CustomImage_CheckForImageCopy", dataTypeList);

                int? Result = null;

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[0].Columns.Contains("MaxCount") && !dataSet.Tables[0].Rows[0]["MaxCount"].Equals(DBNull.Value))
                    {
                        Result = Convert.ToInt32(dataSet.Tables[0].Rows[0]["MaxCount"]);
                    }
                }

                return Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string UpdateIsDefaultIQClient_CustomImage(Int64 p_ID, Guid p_ClientGuid)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Output", DbType.Int32, string.Empty, ParameterDirection.Output));

                _Result = DataAccess.ExecuteNonQuery("usp_v4_IQClient_CustomImage_UpdateIsDefault", _ListOfDataType);

                return _Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string UpdateIsDefaultEmailIQClient_CustomImage(Int64 p_ID, Guid p_ClientGuid)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Output", DbType.Int32, string.Empty, ParameterDirection.Output));

                _Result = DataAccess.ExecuteNonQuery("usp_v4_IQClient_CustomImage_UpdateIsDefaultEmail", _ListOfDataType);

                return _Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
