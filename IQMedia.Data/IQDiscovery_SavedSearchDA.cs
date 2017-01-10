using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using System.Xml.Linq;
using System.Data;
using IQMedia.Model;
using System.Xml;

namespace IQMedia.Data
{
    public class IQDiscovery_SavedSearchDA : IDataAccess
    {
        public string InsertDiscoverySavedSearch(Discovery_SavedSearchModel discovery_SavedSearch)
        {
            try
            {
                Int32 SavedSearchID = 0;

                string advancedSearchIDs = discovery_SavedSearch.AdvanceSearchSettingIDsList == null ? "" : String.Join(";", discovery_SavedSearch.AdvanceSearchSettingIDsList);
                string mediums = discovery_SavedSearch.Mediums == null ? "" : String.Join(";", discovery_SavedSearch.Mediums);

                string advanceSearchSettings = "<AdvanceSearchSettingsRoot>";
                if (discovery_SavedSearch.AdvanceSearchSettingsList != null)
                {
                    foreach (DiscoveryAdvanceSearchModel search in discovery_SavedSearch.AdvanceSearchSettingsList)
                    {
                        // Don't save the advanced search settings if they are all empty
                        bool hasValues = false;
                        foreach (var property in search.GetType().GetProperties())
                        {
                            hasValues = hasValues || property.GetValue(search, null) != null;
                        }

                        if (hasValues)
                        {
                            advanceSearchSettings += Shared.Utility.CommonFunctions.SerializeToXml(search);
                        }
                    }
                }
                advanceSearchSettings += "</AdvanceSearchSettingsRoot>";


                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@Title", DbType.String, discovery_SavedSearch.Title, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchTerm", DbType.String, discovery_SavedSearch.SearchTerm, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchID", DbType.String, discovery_SavedSearch.SearchID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@AdvanceSearchSettings", DbType.Xml, advanceSearchSettings, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@AdvanceSearchSettingIDs", DbType.String, advancedSearchIDs, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Medium", DbType.String, mediums, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, discovery_SavedSearch.ClientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.Guid, discovery_SavedSearch.CustomerGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SavedSearchID", DbType.Int32, SavedSearchID, ParameterDirection.Output));

                string _Result = DataAccess.ExecuteNonQuery("usp_v4_IQDiscovery_SavedSearch_Insert", dataTypeList);
                return _Result;
            }
            catch (Exception ex)
            {
                Shared.Utility.Log4NetLogger.Error("InsertDiscoverySavedSearch: " + ex.ToString());
                throw;
            }
        }

        public string UpdateDiscoverySavedSearch(Discovery_SavedSearchModel discovery_SavedSearch)
        {
            try
            {
                string advancedSearchIDs = discovery_SavedSearch.AdvanceSearchSettingIDsList == null ? "" : String.Join(";", discovery_SavedSearch.AdvanceSearchSettingIDsList);
                string mediums = discovery_SavedSearch.Mediums == null ? "" : String.Join(";", discovery_SavedSearch.Mediums);

                string advanceSearchSettings = "<AdvanceSearchSettingsRoot>";
                if (discovery_SavedSearch.AdvanceSearchSettingsList != null)
                {
                    foreach (DiscoveryAdvanceSearchModel search in discovery_SavedSearch.AdvanceSearchSettingsList)
                    {
                        // Don't save the advanced search settings if they are all empty
                        bool hasValues = false;
                        foreach (var property in search.GetType().GetProperties())
                        {
                            hasValues = hasValues || property.GetValue(search, null) != null;
                        }

                        if (hasValues)
                        {
                            advanceSearchSettings += Shared.Utility.CommonFunctions.SerializeToXml(search);
                        }
                    }
                }
                advanceSearchSettings += "</AdvanceSearchSettingsRoot>";

                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@SavedSearchID", DbType.Int32, discovery_SavedSearch.ID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchTerm", DbType.String, discovery_SavedSearch.SearchTerm, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchID", DbType.String, discovery_SavedSearch.SearchID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@AdvanceSearchSettings", DbType.Xml, advanceSearchSettings, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@AdvanceSearchSettingIDs", DbType.String, advancedSearchIDs, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Medium", DbType.String, mediums, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.Guid, discovery_SavedSearch.CustomerGuid, ParameterDirection.Input));


                string _Result = DataAccess.ExecuteNonQuery("usp_v4_IQDiscovery_SavedSearch_Update", dataTypeList);
                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Discovery_SavedSearchModel> SelectDiscoverySavedSearch(Int32? p_PageNumber, Int32 p_Pagesize, Int32? p_ID, Guid p_CustomerGUID, out Int64 totalRecords)
        {
            try
            {
                totalRecords = 0;
                Dictionary<string, string> p_outParameter;
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@PageNumber", DbType.String, p_PageNumber, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageSize", DbType.String, p_Pagesize, ParameterDirection.Input));
                //dataTypeList.Add(new DataType("@ID", DbType.String, p_ID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.Guid, p_CustomerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@TotalRecords", DbType.Int64, totalRecords, ParameterDirection.Output));
                DataSet dSet = DataAccess.GetDataSetWithOutParam("usp_v4_IQDiscovery_SavedSearch_Select", dataTypeList, out p_outParameter);

                if (p_outParameter != null && p_outParameter.Count > 0)
                {
                    totalRecords = !string.IsNullOrWhiteSpace(p_outParameter["@TotalRecords"]) ? Convert.ToInt32(p_outParameter["@TotalRecords"]) : 0;
                }

                return FillSavedSearch(dSet, p_ID);

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public List<Discovery_SavedSearchModel> SelectDiscoverySavedSearchByID(Int64 p_ID, Guid p_CustomerGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, p_CustomerGuid, ParameterDirection.Input));
                DataSet dSet = DataAccess.GetDataSet("usp_v4_IQDiscovery_SavedSearch_SelectByID", dataTypeList);

                return FillSavedSearch(dSet, (Int32?)p_ID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string DeleteDiscoverySavedSearchByID(Int64 p_ID, Guid p_CustomerGUID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, p_CustomerGUID, ParameterDirection.Input));
                string result = DataAccess.ExecuteNonQuery("usp_v4_IQDiscovery_SavedSearch_DeleteByID", dataTypeList);
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }


        protected List<Discovery_SavedSearchModel> FillSavedSearch(DataSet dataSet, Int32? p_ID)
        {
            List<Discovery_SavedSearchModel> lstDiscovery_SavedSearchModel = new List<Discovery_SavedSearchModel>();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                if (dataSet.Tables[0] != null)
                {
                    DataTable dataTable = dataSet.Tables[0];

                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        Discovery_SavedSearchModel discovery_SavedSearchModel = new Discovery_SavedSearchModel();

                        if (dataTable.Columns.Contains("ID") && !dr["ID"].Equals(DBNull.Value))
                        {
                            if (p_ID == Convert.ToInt32(dr["ID"]))
                            {
                                discovery_SavedSearchModel.IsCurrent = true;
                            }
                            discovery_SavedSearchModel.ID = Convert.ToInt32(dr["ID"]);
                        }

                        if (dataTable.Columns.Contains("Title") && !dr["Title"].Equals(DBNull.Value))
                        {
                            discovery_SavedSearchModel.Title = Convert.ToString(dr["Title"]);
                        }

                        if (dataTable.Columns.Contains("SearchTerm") && !dr["SearchTerm"].Equals(DBNull.Value))
                        {
                            discovery_SavedSearchModel.SearchTerm = Convert.ToString(dr["SearchTerm"]);
                        }

                        if (dataTable.Columns.Contains("SearchID") && !dr["SearchID"].Equals(DBNull.Value))
                        {
                            discovery_SavedSearchModel.SearchID = Convert.ToString(dr["SearchID"]);
                        }

                        if (dataTable.Columns.Contains("AdvanceSearchSettingIDs") && !dr["AdvanceSearchSettingIDs"].Equals(DBNull.Value))
                        {
                            discovery_SavedSearchModel.AdvanceSearchSettingIDsList = new List<string>();
                            var stringIDs = Convert.ToString(dr["AdvanceSearchSettingIDs"]);
                            discovery_SavedSearchModel.AdvanceSearchSettingIDsList = stringIDs.Split(new char[] { ';' }).ToList();
                        }

                        if (dataTable.Columns.Contains("AdvanceSearchSettings") && !dr["AdvanceSearchSettings"].Equals(DBNull.Value))
                        {
                            discovery_SavedSearchModel.AdvanceSearchSettingsList = new List<DiscoveryAdvanceSearchModel>();
                            DiscoveryAdvanceSearchModel discSearchModel;

                            var savedSearchString = Convert.ToString(dr["AdvanceSearchSettings"]);
                            var xml = XDocument.Parse(savedSearchString);
                            var xmlList = xml.Root.Elements().ToArray();
                            var listSavedSearches = new List<string>();
                            foreach (var node in xmlList)
                            {
                                listSavedSearches.Add(node.ToString());
                            }

                            foreach(string search in listSavedSearches)
                            {
                                discSearchModel = new DiscoveryAdvanceSearchModel();
                                discovery_SavedSearchModel.AdvanceSearchSettingsList.Add((DiscoveryAdvanceSearchModel)Shared.Utility.CommonFunctions.DeserialiazeXml(search, discSearchModel));
                            }
                        }

                        if (dataTable.Columns.Contains("Medium") && !dr["Medium"].Equals(DBNull.Value) && !String.IsNullOrWhiteSpace(Convert.ToString(dr["Medium"])))
                        {
                            discovery_SavedSearchModel.Mediums = Convert.ToString(dr["Medium"]).Split(new char[] { ';' }).ToList();
                        }

                        lstDiscovery_SavedSearchModel.Add(discovery_SavedSearchModel);
                    }
                }
            }
            return lstDiscovery_SavedSearchModel;
        }
    }
}
