using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using IQMedia.Model;
using System.Data;

namespace IQMedia.Data
{
    public class CustomCategoryDA : IDataAccess
    {

        public IEnumerable<CustomCategoryModel> GetCustomCategory(Guid clientGUID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, clientGUID, ParameterDirection.Input));
                IDataReader reader = DataAccess.GetDataReader("usp_V4_CustomCategory_SelectByClientGUID", dataTypeList);
                List<CustomCategoryModel> customCategoryModelList = new List<CustomCategoryModel>();

                while (reader.Read())
                {
                    CustomCategoryModel customCategoryModel = new CustomCategoryModel();

                    customCategoryModel.CategoryKey = Convert.ToInt64(reader["CategoryKey"]);
                    customCategoryModel.CategoryName = Convert.ToString(reader["CategoryName"]);
                    customCategoryModel.CategoryDescription = Convert.ToString(reader["CategoryDescription"]);
                    customCategoryModel.CategoryGUID = new Guid(Convert.ToString(reader["CategoryGUID"]));
                    customCategoryModel.ClientGUID = new Guid(Convert.ToString(reader["ClientGUID"]));

                    customCategoryModelList.Add(customCategoryModel);
                }

                return customCategoryModelList.AsEnumerable();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<CustomCategoryModel> GetCustomCategoryByClientID(Int64 p_ClientID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientID", DbType.Int64, p_ClientID, ParameterDirection.Input));
                IDataReader reader = DataAccess.GetDataReader("usp_v4_CustomCategory_SelectByClientID", dataTypeList);
                List<CustomCategoryModel> customCategoryModelList = new List<CustomCategoryModel>();

                while (reader.Read())
                {
                    CustomCategoryModel customCategoryModel = new CustomCategoryModel();

                    customCategoryModel.CategoryKey = Convert.ToInt64(reader["CategoryKey"]);
                    customCategoryModel.CategoryName = Convert.ToString(reader["CategoryName"]);
                    customCategoryModel.CategoryDescription = Convert.ToString(reader["CategoryDescription"]);
                    customCategoryModel.CategoryGUID = new Guid(Convert.ToString(reader["CategoryGUID"]));
                    customCategoryModel.ClientGUID = new Guid(Convert.ToString(reader["ClientGUID"]));

                    customCategoryModelList.Add(customCategoryModel);
                }

                return customCategoryModelList.AsEnumerable();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<CustomCategoryModel> SelectCustomCategoryForSetup(Guid clientGUID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, clientGUID, ParameterDirection.Input));

                List<CustomCategoryModel> customCategoryModelList = new List<CustomCategoryModel>();

                DataSet dataSet = DataAccess.GetDataSet("usp_v4_CustomCategory_SelectByCustomer", dataTypeList);

                if(dataSet != null && dataSet.Tables.Count > 0)
                {
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        CustomCategoryModel customCategoryModel = new CustomCategoryModel();
                        if (!dr["CategoryKey"].Equals(DBNull.Value))
                        {
                            customCategoryModel.CategoryKey = Convert.ToInt64(dr["CategoryKey"]);
                        }
                        if (!dr["CategoryName"].Equals(DBNull.Value))
                        {
                            customCategoryModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                        }
                        if (!dr["CategoryDescription"].Equals(DBNull.Value))
                        {
                            customCategoryModel.CategoryDescription = Convert.ToString(dr["CategoryDescription"]);
                        }
                        if (!dr["CategoryGUID"].Equals(DBNull.Value))
                        {
                            customCategoryModel.CategoryGUID = new Guid(Convert.ToString(dr["CategoryGUID"]));
                        }
                        if (!dr["CategoryRanking"].Equals(DBNull.Value))
                        {
                            customCategoryModel.CategoryRanking = Convert.ToInt32(dr["CategoryRanking"]);
                        }
                        customCategoryModelList.Add(customCategoryModel);
                    }
                }

                return customCategoryModelList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string InsertCustomCategoryForSetup(CustomCategoryModel p_customerCategory)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, p_customerCategory.ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryName", DbType.String, p_customerCategory.CategoryName, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryDescription", DbType.String, p_customerCategory.CategoryDescription, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryKey", DbType.Int64, null, ParameterDirection.Output));

                string _Result = DataAccess.ExecuteNonQuery("usp_v4_CustomCategory_Insert", dataTypeList);

                return _Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string UpdateCustomCategoryForSetup(CustomCategoryModel p_customerCategory)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@CategoryKey", DbType.Int64, p_customerCategory.CategoryKey, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, p_customerCategory.ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryName", DbType.String, p_customerCategory.CategoryName, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryDescription", DbType.String, p_customerCategory.CategoryDescription, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Flag", DbType.Int32, 0, ParameterDirection.Output));

                string _Result = DataAccess.ExecuteNonQuery("usp_v4_CustomCategory_Update", dataTypeList);

                return _Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public CustomCategoryModel SelectCustomCategoryByCategoryKeyForSetup(Guid clientGUID, long CustomCategoryKey)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, clientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomCategoryKey", DbType.Int64, CustomCategoryKey, ParameterDirection.Input));

                List<CustomCategoryModel> customCategoryModelList = new List<CustomCategoryModel>();

                DataSet dataSet = DataAccess.GetDataSet("usp_v4_CustomCategory_SelectByCustomCategoryKey", dataTypeList);

                CustomCategoryModel customCategoryModel = new CustomCategoryModel();

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        if (!dr["CategoryKey"].Equals(DBNull.Value))
                        {
                            customCategoryModel.CategoryKey = Convert.ToInt64(dr["CategoryKey"]);
                        }
                        if (!dr["CategoryName"].Equals(DBNull.Value))
                        {
                            customCategoryModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                        }
                        if (!dr["CategoryDescription"].Equals(DBNull.Value))
                        {
                            customCategoryModel.CategoryDescription = Convert.ToString(dr["CategoryDescription"]);
                        }
                    }
                }

                return customCategoryModel;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string DeleteCustomCategoryForSetup(Guid clientGUID, long CustomCategoryKey)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@CustomCategoryKey", DbType.Int64, CustomCategoryKey, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, clientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Status", DbType.Int32, 0, ParameterDirection.Output));

                string _Result = DataAccess.ExecuteNonQuery("usp_v4_CustomCategory_DeleteByCategoryKey", dataTypeList);

                return _Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int UpdateCustomCategoryRankings(string categoryRankingXml)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@CategoryRankingXml", DbType.Xml, categoryRankingXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@RowCount", DbType.Int32, 0, ParameterDirection.Output));

                int result = Convert.ToInt32(DataAccess.ExecuteNonQuery("usp_v4_CustomCategory_UpdateRankings", dataTypeList));
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
