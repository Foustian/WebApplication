using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using IQMedia.Model;
using System.Data;

namespace IQMedia.Data
{
    public class IQTads_SavedSearchDA : IDataAccess
    {
        public string InsertTadsSavedSearch(Tads_SavedSearchModel tads_SavedSearchModel)
        {
            try
            {
                Int32 SavedSearchID = 0;
                List<DataType> dataTypeList = new List<DataType>();
                string SearchTermXml = IQMedia.Shared.Utility.CommonFunctions.SerializeToXml(tads_SavedSearchModel.SearchTerm);

                dataTypeList.Add(new DataType("@Title", DbType.String, tads_SavedSearchModel.Title, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchTerm", DbType.Xml, SearchTermXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, tads_SavedSearchModel.ClientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, tads_SavedSearchModel.CustomerGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ComponentType", DbType.String, "TADS", ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SavedSearchID", DbType.Int32, SavedSearchID, ParameterDirection.Output));

                string _Result = DataAccess.ExecuteNonQuery("usp_v4_IQTimeshift_SavedSearch_Insert", dataTypeList);
                return _Result;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public string UpdateTadsSavedSearch(Tads_SavedSearchModel tads_SavedSearchModel)
        {
            try
            {
                int RowUpdated = 0;
                List<DataType> dataTypeList = new List<DataType>();
                string SearchTermXml = IQMedia.Shared.Utility.CommonFunctions.SerializeToXml(tads_SavedSearchModel.SearchTerm);

                dataTypeList.Add(new DataType("@Title", DbType.String, tads_SavedSearchModel.Title, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SavedSearchID", DbType.Int32, tads_SavedSearchModel.ID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchTerm", DbType.Xml, SearchTermXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, tads_SavedSearchModel.CustomerGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@RowUpdated", DbType.Int32, RowUpdated, ParameterDirection.Output));


                string _Result = DataAccess.ExecuteNonQuery("usp_v4_IQTimeshift_SavedSearch_Update", dataTypeList);
                return _Result;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<Tads_SavedSearchModel> SelectTadsSavedSearch(Int32? p_PageNumber, Int32 p_Pagesize, Guid p_CustomerGUID, out Int64 totalRecords)
        {
            try
            {
                totalRecords = 0;
                Dictionary<string, string> p_outParameter;
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@PageNumber", DbType.String, p_PageNumber, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageSize", DbType.String, p_Pagesize, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, p_CustomerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ComponentType", DbType.String, "TADS", ParameterDirection.Input));
                dataTypeList.Add(new DataType("@TotalRecords", DbType.Int64, totalRecords, ParameterDirection.Output));
                DataSet dSet = DataAccess.GetDataSetWithOutParam("usp_v4_IQTimeshift_SavedSearch_Select", dataTypeList, out p_outParameter);

                if (p_outParameter != null && p_outParameter.Count > 0)
                {
                    totalRecords = !string.IsNullOrWhiteSpace(p_outParameter["@TotalRecords"]) ? Convert.ToInt32(p_outParameter["@TotalRecords"]) : 0;
                }

                return FillSavedSearch(dSet);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public List<Tads_SavedSearchModel> SelectTadsSavedSearchByID(Int64 p_ID, Guid p_CustomerGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, p_CustomerGuid, ParameterDirection.Input));
                DataSet dSet = DataAccess.GetDataSet("usp_v4_IQTimeshift_SavedSearch_SelectByID", dataTypeList);

                return FillSavedSearch(dSet);

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public string DeleteTadsSavedSearchByID(Int64 p_ID, Guid p_CustomerGUID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, p_CustomerGUID, ParameterDirection.Input));
                string result = DataAccess.ExecuteNonQuery("usp_v4_IQTimeshift_SavedSearch_Delete", dataTypeList);
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }


        protected List<Tads_SavedSearchModel> FillSavedSearch(DataSet dataSet)
        {

            List<Tads_SavedSearchModel> lstTads_SavedSearchModel = new List<Tads_SavedSearchModel>();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                if (dataSet.Tables[0] != null)
                {

                    DataTable dataTable = dataSet.Tables[0];

                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        Tads_SavedSearchModel tads_SavedSearchModel = new Tads_SavedSearchModel();
                        if (dataTable.Columns.Contains("SearchTerm") && !dr["SearchTerm"].Equals(DBNull.Value))
                        {
                            if (dataTable.Columns.Contains("ID") && !dr["ID"].Equals(DBNull.Value))
                            {
                            tads_SavedSearchModel.ID = Convert.ToInt32(dr["ID"]);
                            }

                            if (dataTable.Columns.Contains("Title") && !dr["Title"].Equals(DBNull.Value))
                            {
                            tads_SavedSearchModel.Title = Convert.ToString(dr["Title"]);
                            }

                            tads_SavedSearchModel.SearchTerm = new TadsSearchTerm();
                            tads_SavedSearchModel.SearchTerm = IQMedia.Shared.Utility.CommonFunctions.DeserialiazeXml(Convert.ToString(dr["SearchTerm"]), tads_SavedSearchModel.SearchTerm) as TadsSearchTerm;
                            lstTads_SavedSearchModel.Add(tads_SavedSearchModel);
                        }
                    }
                }
            }
            return lstTads_SavedSearchModel;
        }
    }
}
