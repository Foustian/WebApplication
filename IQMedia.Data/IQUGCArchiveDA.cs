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
    public class IQUGCArchiveDA :  IDataAccess
    {

        public IQUGCArchiveResult GetIQArchieveResults(string ClientGUID,DateTime? FromDate,DateTime? ToDate,string SearchTerm,string CategoryGuid,string SelectionType,string CustomerGuid,string FileType,
                                                        long FromRecordID, int PageSize, long SinceID, string Sortcolumn, bool IsAsc, bool IsEnableFilter)
        {
            try
            {

                SearchTerm = string.IsNullOrWhiteSpace(SearchTerm) ? null : SearchTerm;
                CategoryGuid = string.IsNullOrWhiteSpace(CategoryGuid) ? null : CategoryGuid;
                CustomerGuid = string.IsNullOrWhiteSpace(CustomerGuid) ? null : CustomerGuid;
                FileType = string.IsNullOrWhiteSpace(FileType) ? null : FileType;
                     
                List<DataType> dataTypeList = new List<DataType>();
                Dictionary<string, string> p_outParameter;
                dataTypeList.Add(new DataType("@ClientGUID", DbType.String, ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromRecordID", DbType.Int64, FromRecordID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.Date, FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.Date, ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchTerm", DbType.String, SearchTerm, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryGUID", DbType.Xml, CategoryGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.String, CustomerGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SelectionType", DbType.String, SelectionType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsAsc", DbType.Boolean, IsAsc, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SortColumn", DbType.String, Sortcolumn, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsEnableFilter", DbType.Boolean, IsEnableFilter, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SinceID", DbType.Int64, SinceID, ParameterDirection.Output));
                dataTypeList.Add(new DataType("@FileType", DbType.String, FileType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@TotalResults", DbType.Int64, 0, ParameterDirection.Output));

                DataSet dataset = DataAccess.GetDataSetWithOutParam("usp_v4_IQUGCArchive_SelectByClientGuid", dataTypeList, out p_outParameter);

                IQUGCArchiveResult result = FillIQUGCArchieveResults(dataset);

                if (IsEnableFilter)
                {
                    result.Filter = FillIQUGCArchieveFilter(dataset.Tables[1], dataset.Tables[2], dataset.Tables[3], dataset.Tables[4]);
                }

                if (p_outParameter != null && p_outParameter.Count > 0)
                {
                    result.SinceID = !string.IsNullOrWhiteSpace(p_outParameter["@SinceID"]) ? Convert.ToInt64(p_outParameter["@SinceID"]) : 0;
                    result.TotalResults = !string.IsNullOrWhiteSpace(p_outParameter["@TotalResults"]) ? Convert.ToInt32(p_outParameter["@TotalResults"]) : 0;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQUGCArchiveResult_FilterModel GetIQArchieveFilter(string ClientGUID, DateTime? FromDate, DateTime? ToDate, string SearchTerm, string CategoryGuid, string SelectionType, string CustomerGuid, long SinceID,string FileType)
        {
            try
            {
                IQUGCArchiveResult_FilterModel objIQUGCArchiveResult_FilterModel = new IQUGCArchiveResult_FilterModel();

                SearchTerm = string.IsNullOrWhiteSpace(SearchTerm) ? null : SearchTerm;
                CategoryGuid = string.IsNullOrWhiteSpace(CategoryGuid) ? null : CategoryGuid;
                CustomerGuid = string.IsNullOrWhiteSpace(CustomerGuid) ? null : CustomerGuid;
                FileType = string.IsNullOrWhiteSpace(FileType) ? null : FileType;

                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGUID", DbType.String, ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.Date, FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.Date, ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchTerm", DbType.String, SearchTerm, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryGUID", DbType.Xml, CategoryGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.String, CustomerGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SelectionType", DbType.String, SelectionType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FileType", DbType.String, FileType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SinceID", DbType.Int64, SinceID, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQUGCArchive_SelectFilter", dataTypeList);

                objIQUGCArchiveResult_FilterModel = FillIQUGCArchieveFilter(dataset.Tables[0], dataset.Tables[1], dataset.Tables[2], dataset.Tables[3]);

                return objIQUGCArchiveResult_FilterModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<long> Delete(string ClientGUID, string IQUGCArchiveIDs)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGuid", DbType.String, ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IQUGCArchiveXML", DbType.Xml, IQUGCArchiveIDs, ParameterDirection.Input));

                DataSet result = DataAccess.GetDataSet("usp_v4_IQUGCArchive_Delete", dataTypeList);

                List<long> lstArchiveID = new List<long>();

                foreach (DataRow dr in result.Tables[0].Rows)
                {
                    if (!dr["UGCArchiveID"].Equals(DBNull.Value))
                    {
                        lstArchiveID.Add(Convert.ToInt64(dr["UGCArchiveID"]));
                    }
                }

                return lstArchiveID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQUGCArchiveEditModel SelectForEdit(string ClientGUID, long IQUGCArchiveKey)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGuid", DbType.String, ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IQUGCArchiveKey", DbType.Int64, IQUGCArchiveKey, ParameterDirection.Input));

                DataSet result = DataAccess.GetDataSet("usp_v4_IQUGCArchive_SelectForEdit", dataTypeList);

                IQUGCArchiveEditModel objIQUGCArchiveEditModel = new IQUGCArchiveEditModel();

                if (result != null && result.Tables.Count > 0)
                {
                    objIQUGCArchiveEditModel.CustomCategories = new List<CustomCategoryModel>();
                    objIQUGCArchiveEditModel.Customers = new List<CustomerModel>();

                    // Table[0] represents Customers List

                    foreach (DataRow dr in result.Tables[0].Rows)
                    {
                        CustomerModel customer = new CustomerModel();

                        if (!dr["CustomerKey"].Equals(DBNull.Value))
                        {
                            customer.CustomerKey = Convert.ToInt32(dr["CustomerKey"]);
                        }
                        if (!dr["CustomerGuid"].Equals(DBNull.Value))
                        {
                            customer.CustomerGUID = new Guid(Convert.ToString(dr["CustomerGuid"]));
                        }
                        if (!dr["FirstName"].Equals(DBNull.Value))
                        {
                            customer.FirstName = Convert.ToString(dr["FirstName"]);
                        }
                        if (!dr["LastName"].Equals(DBNull.Value))
                        {
                            customer.LastName = Convert.ToString(dr["LastName"]);
                        }
                        if (!dr["Email"].Equals(DBNull.Value))
                        {
                            customer.Email = Convert.ToString(dr["Email"]);
                        }
                        customer.FullName = customer.FirstName + " " + customer.LastName;

                        objIQUGCArchiveEditModel.Customers.Add(customer);
                    }

                    // Table[1] represents Custom Categories List

                    foreach (DataRow dr in result.Tables[1].Rows)
                    {
                        CustomCategoryModel customercategory = new CustomCategoryModel();

                        if (!dr["CategoryKey"].Equals(DBNull.Value))
                        {
                            customercategory.CategoryKey = Convert.ToInt32(dr["CategoryKey"]);
                        }
                        if (!dr["CategoryName"].Equals(DBNull.Value))
                        {
                            customercategory.CategoryName = Convert.ToString(dr["CategoryName"]);
                        }
                        if (!dr["CategoryGUID"].Equals(DBNull.Value))
                        {
                            customercategory.CategoryGUID = new Guid(Convert.ToString(dr["CategoryGUID"]));
                        }

                        objIQUGCArchiveEditModel.CustomCategories.Add(customercategory);
                    }

                    // Table[2] represents IQUGCArchive record to be edit

                    foreach (DataRow dr in result.Tables[2].Rows)
                    {
                        if (!dr["IQUGCArchiveKey"].Equals(DBNull.Value))
                        {
                            objIQUGCArchiveEditModel.IQUGCArchiveKey = Convert.ToInt64(dr["IQUGCArchiveKey"]);
                        }
                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            objIQUGCArchiveEditModel.Title = Convert.ToString(dr["Title"]);
                        }
                        if (!dr["CustomerGUID"].Equals(DBNull.Value))
                        {
                            objIQUGCArchiveEditModel.CustomerGuid = Convert.ToString(dr["CustomerGUID"]);
                        }
                        if (!dr["CategoryGUID"].Equals(DBNull.Value))
                        {
                            objIQUGCArchiveEditModel.CategoryGuid = Convert.ToString(dr["CategoryGUID"]);
                        }
                        if (!dr["SubCategory1GUID"].Equals(DBNull.Value))
                        {
                            objIQUGCArchiveEditModel.SubCategory1Guid = Convert.ToString(dr["SubCategory1GUID"]);
                        }
                        if (!dr["SubCategory2GUID"].Equals(DBNull.Value))
                        {
                            objIQUGCArchiveEditModel.SubCategory2Guid = Convert.ToString(dr["SubCategory2GUID"]);
                        }
                        if (!dr["SubCategory3GUID"].Equals(DBNull.Value))
                        {
                            objIQUGCArchiveEditModel.SubCategory3Guid = Convert.ToString(dr["SubCategory3GUID"]);
                        }
                        if (!dr["Keywords"].Equals(DBNull.Value))
                        {
                            objIQUGCArchiveEditModel.Keywords = Convert.ToString(dr["Keywords"]);
                        }
                        if (!dr["Description"].Equals(DBNull.Value))
                        {
                            objIQUGCArchiveEditModel.Description = Convert.ToString(dr["Description"]);
                        }
                    }
                }
                return objIQUGCArchiveEditModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string UpdateIQUGCArchive(string ClientGUID, long IQUGCArchiveKey, string p_Title, string p_Keywords, Guid p_Customer, Guid? p_Category, Guid? p_Subcategory1, Guid? p_Subcategory2, Guid? p_Subcategory3, string p_Description)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGuid", DbType.String, ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IQUGCArchiveKey", DbType.Int64, IQUGCArchiveKey, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Title", DbType.String, p_Title, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Keywords", DbType.String, p_Keywords, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, p_Customer, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryGuid", DbType.Guid, p_Category, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubCategory1Guid", DbType.Guid, p_Subcategory1, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubCategory2Guid", DbType.Guid, p_Subcategory2, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubCategory3Guid", DbType.Guid, p_Subcategory3, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Description", DbType.String, p_Description, ParameterDirection.Input));

                string result = DataAccess.ExecuteNonQuery("usp_v4_IQUGCArchive_Update", dataTypeList);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string RefreshResults(string ClientGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGuid", DbType.String, ClientGuid, ParameterDirection.Input));

                string result = DataAccess.ExecuteNonQuery("usp_v4_IQUGCArchive_RefreshResults", dataTypeList);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQUGCArchiveModel SelectUGCFileLocationAndName(string ClientGuid,long IQUGCArchiveKey)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@IQUGCArchiveKey", DbType.Int64, IQUGCArchiveKey, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.String, ClientGuid, ParameterDirection.Input));
                DataSet dataSet = DataAccess.GetDataSet("usp_v4_IQUGCArchive_SelectFileLocationForDownload", dataTypeList);

                IQUGCArchiveModel result = new IQUGCArchiveModel();

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        if (!dr["UGCFileLocation"].Equals(DBNull.Value))
                        {
                            result.UGCFileLocation = Convert.ToString(dr["UGCFileLocation"]);
                        }
                        if (!dr["UGCFileName"].Equals(DBNull.Value))
                        {
                            result.UGCFileName = Convert.ToString(dr["UGCFileName"]);
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IQUGCArchiveResult FillIQUGCArchieveResults(DataSet dataSet)
        {
            IQUGCArchiveResult objIQUGCArchiveResult = new IQUGCArchiveResult();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                objIQUGCArchiveResult.IQUGCArchiveList = new List<IQUGCArchiveModel>();

                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    IQUGCArchiveModel objIQUGCArchiveModel = new IQUGCArchiveModel();

                    if (!dr["IQUGCArchiveKey"].Equals(DBNull.Value))
                    {
                        objIQUGCArchiveModel.IQUGCArchiveKey = Convert.ToInt64(dr["IQUGCArchiveKey"]);
                    }

                    if (!dr["Title"].Equals(DBNull.Value))
                    {
                        objIQUGCArchiveModel.Title = Convert.ToString(dr["Title"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Filetype") && !dr["Filetype"].Equals(DBNull.Value))
                    {
                        objIQUGCArchiveModel.FileType = (CommonFunctions.IQUGCMediaTypes)Enum.Parse(typeof(CommonFunctions.IQUGCMediaTypes), Convert.ToString(dr["Filetype"]));
                    }

                    if (dataSet.Tables[0].Columns.Contains("MediaUrl") && !dr["MediaUrl"].Equals(DBNull.Value))
                    {
                        objIQUGCArchiveModel.MediaUrl = Convert.ToString(dr["MediaUrl"]);
                    }

                    if (!dr["UGCGUID"].Equals(DBNull.Value))
                    {
                        objIQUGCArchiveModel.UGCGuid = Convert.ToString(dr["UGCGUID"]);
                    }

                    if (!dr["DateUploaded"].Equals(DBNull.Value))
                    {
                        objIQUGCArchiveModel.DateUploaded = Convert.ToDateTime(dr["DateUploaded"]);
                    }

                    if (!dr["CreateDTTimeZone"].Equals(DBNull.Value))
                    {
                        objIQUGCArchiveModel.CreateDTTimeZone = (CommonFunctions.Timezone)Enum.Parse(typeof(CommonFunctions.Timezone), Convert.ToString(dr["CreateDTTimeZone"]));
                    }

                    if (!dr["AirDate"].Equals(DBNull.Value))
                    {
                        objIQUGCArchiveModel.AirDate = Convert.ToDateTime(dr["AirDate"]);
                    }

                    if (!dr["Description"].Equals(DBNull.Value))
                    {
                        objIQUGCArchiveModel.Description = Convert.ToString(dr["Description"]);
                    }

                    if (!dr["ThumbnailImage"].Equals(DBNull.Value))
                    {
                        objIQUGCArchiveModel.ThumbnailImage = Convert.ToString(dr["ThumbnailImage"]);
                    }

                    if (!dr["CategoryGUID"].Equals(DBNull.Value))
                    {
                        objIQUGCArchiveModel.CategoryGUID = Convert.ToString(dr["CategoryGUID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("CategoryName") && !dr["CategoryName"].Equals(DBNull.Value))
                    {
                        objIQUGCArchiveModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                    }
                    objIQUGCArchiveResult.IQUGCArchiveList.Add(objIQUGCArchiveModel);
                }
            }
            return objIQUGCArchiveResult;
        }

        private IQUGCArchiveResult_FilterModel FillIQUGCArchieveFilter(DataTable dtCreatedDT, DataTable dtCategory, DataTable dtCustomer, DataTable dtFileType)
        {
            IQUGCArchiveResult_FilterModel filter = new IQUGCArchiveResult_FilterModel();
            filter.CreateDTList = new List<string>();
            filter.Categories = new List<IQUGCArchiveResult_Filter>();
            filter.Customers = new List<IQUGCArchiveResult_Filter>();
            filter.MediaTypes = new List<IQUGCArchiveResult_Filter>();

            if (dtCreatedDT != null)
            {
                foreach (DataRow dr in dtCreatedDT.Rows)
                {
                    IQArchive_Filter objCustomer = new IQArchive_Filter();
                    if (!dr["DateUploaded"].Equals(DBNull.Value))
                    {
                        filter.CreateDTList.Add(Convert.ToDateTime(dr["DateUploaded"]).ToString("MM/dd/yyyy"));
                    }
                }
            }
           
            if (dtCustomer != null)
            {
                foreach (DataRow dr in dtCustomer.Rows)
                {
                    IQUGCArchiveResult_Filter objCustomer = new IQUGCArchiveResult_Filter();

                    if (!dr["CustomerGUID"].Equals(DBNull.Value))
                    {
                        objCustomer.CustomerKey = Convert.ToString(dr["CustomerGUID"]);
                    }
                    if (!dr["CustomerName"].Equals(DBNull.Value))
                    {
                        objCustomer.CustomerName = Convert.ToString(dr["CustomerName"]);
                    }
                    if (!dr["CustomerCount"].Equals(DBNull.Value))
                    {
                        objCustomer.RecordCount = Convert.ToInt64(dr["CustomerCount"]);
                    }
                    if (!string.IsNullOrWhiteSpace(objCustomer.CustomerKey) && !string.IsNullOrWhiteSpace(objCustomer.CustomerName))
                    {
                        filter.Customers.Add(objCustomer);
                    }
                }
            }
            if (dtCategory != null)
            {
                foreach (DataRow dr in dtCategory.Rows)
                {
                    IQUGCArchiveResult_Filter objCategory = new IQUGCArchiveResult_Filter();
                    if (!dr["CategoryGUID"].Equals(DBNull.Value))
                    {
                        objCategory.CategoryGUID = Convert.ToString(dr["CategoryGUID"]);
                    }
                    if (!dr["CategoryName"].Equals(DBNull.Value))
                    {
                        objCategory.CategoryName = Convert.ToString(dr["CategoryName"]);
                    }
                    if (!dr["CategoryCount"].Equals(DBNull.Value))
                    {
                        objCategory.RecordCount = Convert.ToInt64(dr["CategoryCount"]);
                    }
                    if (!string.IsNullOrWhiteSpace(objCategory.CategoryGUID) && !string.IsNullOrWhiteSpace(objCategory.CategoryName))
                    {
                        filter.Categories.Add(objCategory);
                    }
                }
            }

            if (dtFileType != null)
            {
                foreach (DataRow dr in dtFileType.Rows)
                {
                    IQUGCArchiveResult_Filter objFileType = new IQUGCArchiveResult_Filter();
                    if (!dr["FileType"].Equals(DBNull.Value))
                    {
                        objFileType.MediaType = Enum.Parse(typeof(CommonFunctions.IQUGCMediaTypes), Convert.ToString(dr["FileType"])).ToString();
                    }

                    if (!dr["FileType"].Equals(DBNull.Value))
                    {
                        objFileType.MediaTypeDesc = Shared.Utility.CommonFunctions.GetEnumDescription((CommonFunctions.IQUGCMediaTypes)Enum.Parse(typeof(CommonFunctions.IQUGCMediaTypes), Convert.ToString(dr["FileType"])));
                    }

                    if (!dr["FileTypeCount"].Equals(DBNull.Value))
                    {
                        objFileType.RecordCount = Convert.ToInt64(dr["FileTypeCount"]);
                    }
                    filter.MediaTypes.Add(objFileType);
                    
                }
            }

            return filter;
        }

        public List<IQUGCArchiveResult_Filter> GetCategoryFilter(string ClientGUID, DateTime? FromDate, DateTime? ToDate, string SearchTerm, string CategoryGuid, string CustomerGuid, long SinceID, string FileType)
        {


            try
            {

                try
                {
                    IQUGCArchiveResult_FilterModel objIQUGCArchiveResult_FilterModel = new IQUGCArchiveResult_FilterModel();

                    SearchTerm = string.IsNullOrWhiteSpace(SearchTerm) ? null : SearchTerm;
                    CategoryGuid = string.IsNullOrWhiteSpace(CategoryGuid) ? null : CategoryGuid;
                    CustomerGuid = string.IsNullOrWhiteSpace(CustomerGuid) ? null : CustomerGuid;
                    FileType = string.IsNullOrWhiteSpace(FileType) ? null : FileType;

                    List<DataType> dataTypeList = new List<DataType>();

                    dataTypeList.Add(new DataType("@ClientGuid", DbType.String, ClientGUID, ParameterDirection.Input));
                    dataTypeList.Add(new DataType("@FromDate", DbType.Date, FromDate, ParameterDirection.Input));
                    dataTypeList.Add(new DataType("@ToDate", DbType.Date, ToDate, ParameterDirection.Input));
                    dataTypeList.Add(new DataType("@SearchTerm", DbType.String, SearchTerm, ParameterDirection.Input));
                    dataTypeList.Add(new DataType("@CategoryGUID", DbType.Xml, CategoryGuid, ParameterDirection.Input));
                    dataTypeList.Add(new DataType("@CustomerGUID", DbType.String, CustomerGuid, ParameterDirection.Input));
                    dataTypeList.Add(new DataType("@SinceID", DbType.Int64, SinceID, ParameterDirection.Input));
                    dataTypeList.Add(new DataType("@FileType", DbType.String, FileType, ParameterDirection.Input));

                    DataSet dataset = DataAccess.GetDataSet("usp_v4_IQUGCArchive_SelectCategoryFilter", dataTypeList);

                    objIQUGCArchiveResult_FilterModel = FillIQUGCArchieveFilter(null, dataset.Tables[0],null ,null);

                    return objIQUGCArchiveResult_FilterModel.Categories;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string InsertIQUGCArchiveDocument(Guid CategoryGUID, Guid? SubCategory1GUID, Guid? SubCategory2GUID, Guid? SubCategory3GUID, string Title, string Keywords, string Description, string DocumentDate, string DocumentTimeZone, Guid CustomerGUID, Guid ClientGUID,string FileType, int _RootPathID, string Location)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@CategoryGUID", DbType.Guid, CategoryGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubCategory1GUID", DbType.Guid, SubCategory1GUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubCategory2GUID", DbType.Guid, SubCategory2GUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubCategory3GUID", DbType.Guid, SubCategory3GUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Title", DbType.String, Title, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Keywords", DbType.String, Keywords, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Description", DbType.String, Description, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@DocumentDate", DbType.DateTime, DocumentDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@DocumentTimeZone", DbType.String, DocumentTimeZone, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.Guid, CustomerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FileType", DbType.String, FileType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@_RootPathID", DbType.Int32, _RootPathID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Location", DbType.String, Location, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ID", DbType.Int64, string.Empty, ParameterDirection.Output));


                string result = DataAccess.ExecuteNonQuery("usp_v4_IQUGCArchive_Insert", dataTypeList);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GetUGCDocumentStoragePath(out int p_RootPathID,out string StoragePath)
        {
            try
            {

                p_RootPathID = 0;
                StoragePath = string.Empty;
                
                List<DataType> dataTypeList = new List<DataType>();

                IDataReader reader = DataAccess.GetDataReader("usp_v4_IQCore_RootPath_SelectUGCDocumentRootPath", dataTypeList);

                while (reader.Read())
                {
                    p_RootPathID = Convert.ToInt32(reader["ID"]);
                    StoragePath = Convert.ToString(reader["StoragePath"]);
                }

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dictionary<string,string> GetUGCFileTypes()
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                Dictionary<string, string> _fileTypes = new Dictionary<string, string>();

                DataSet dataSet = DataAccess.GetDataSet("usp_v4_IQUGC_FileTypes_SelectFileTypes", dataTypeList);

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        string _filetype = string.Empty;
                        string _filetypeext = string.Empty;
                        if (dataSet.Tables[0].Columns.Contains("FileTypeExt") && !dr["FileTypeExt"].Equals(DBNull.Value))
                        {
                            _filetypeext = Convert.ToString(dr["FileTypeExt"]);
                        }

                        if (dataSet.Tables[0].Columns.Contains("FileType") && !dr["FileType"].Equals(DBNull.Value))
                        {
                            _filetype = Convert.ToString(dr["FileType"]);
                        }

                        if (!string.IsNullOrEmpty(_filetypeext))
                        {
                            _fileTypes.Add(_filetypeext, _filetype);
                        }
                    }
                }
                return _fileTypes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
