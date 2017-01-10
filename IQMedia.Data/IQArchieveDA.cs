using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using IQMedia.Model;
using System.Data;
using IQMedia.Shared.Utility;
using System.Xml.Linq;
using System.Data.SqlTypes;
using System.Xml;
using System.IO;
using IQCommon.Model;

namespace IQMedia.Data
{
    public class IQArchieveDA : IDataAccess
    {
        /// <summary>
        /// Returns rows from table which satisfy criteria and filters associated
        /// </summary>
        /// <param name="ClientGUID"></param>
        /// <param name="CustomerGUID"></param>
        /// <param name="FromRecordID"></param>
        /// <param name="PageSize"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="SubMediaType"></param>
        /// <param name="SearchTerm"></param>
        /// <param name="IsAsc"></param>
        /// <param name="SinceID"></param>
        /// <param name="TotalResults"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetIQArchieveResults(string ClientGUID, string CustomerGUID, long FromRecordID, int PageSize, DateTime? FromDate, DateTime? ToDate, string SubMediaType, string SearchTerm,
                                                                string CategoryGUID, string SelectionType, bool IsAsc, string SortColumn, bool IsEnableFilter, string currentUrl, ref long SinceID, out long TotalResults, out long TotalResultsDisplay, bool IsLibraryRollup)
        {
            try
            {

                CustomerGUID = !string.IsNullOrWhiteSpace(CustomerGUID) ? CustomerGUID : null;
                SubMediaType = !string.IsNullOrWhiteSpace(SubMediaType) ? SubMediaType : null;
                SearchTerm = !string.IsNullOrWhiteSpace(SearchTerm) ? SearchTerm : null;
                CategoryGUID = !string.IsNullOrWhiteSpace(CategoryGUID) ? CategoryGUID : null;

                TotalResults = 0;
                TotalResultsDisplay = 0;

                List<DataType> dataTypeList = new List<DataType>();
                Dictionary<string, string> p_outParameter;
                dataTypeList.Add(new DataType("@ClientGUID", DbType.String, ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.String, CustomerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromRecordID", DbType.Int64, FromRecordID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.DateTime, FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.DateTime, ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubMediaType", DbType.String, SubMediaType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchTerm", DbType.String, SearchTerm, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryGUID", DbType.Xml, CategoryGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsAsc", DbType.Boolean, IsAsc, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SortColumn", DbType.String, SortColumn, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsEnableFilter", DbType.Boolean, IsEnableFilter, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SelectionType", DbType.String, SelectionType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@v4LibraryRollup", DbType.Boolean, IsLibraryRollup, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SinceID", DbType.Int64, SinceID, ParameterDirection.Output));
                dataTypeList.Add(new DataType("@TotalResults", DbType.Int64, TotalResults, ParameterDirection.Output));
                dataTypeList.Add(new DataType("@TotalResultsDisplay", DbType.Int64, TotalResultsDisplay, ParameterDirection.Output));

                DataSet dataset = DataAccess.GetDataSetWithOutParam("usp_v5_IQArchive_Media_Select", dataTypeList, out p_outParameter);

                Dictionary<string, object> dictresult = FillIQArchieveResults(dataset, currentUrl, IsEnableFilter, SortColumn, IsAsc);


                if (p_outParameter != null && p_outParameter.Count > 0)
                {
                    SinceID = !string.IsNullOrWhiteSpace(p_outParameter["@SinceID"]) ? Convert.ToInt64(p_outParameter["@SinceID"]) : 0;
                    TotalResults = !string.IsNullOrWhiteSpace(p_outParameter["@TotalResults"]) ? Convert.ToInt64(p_outParameter["@TotalResults"]) : 0;
                    TotalResultsDisplay = !string.IsNullOrWhiteSpace(p_outParameter["@TotalResultsDisplay"]) ? Convert.ToInt64(p_outParameter["@TotalResultsDisplay"]) : 0;
                }

                return dictresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Returns list of ID which is deleted successfully
        /// </summary>
        /// <param name="ClientGUID">Represent Client who are logged in</param>
        /// <param name="ArchiveIDs">XML formatted multiple ArchiveID</param>
        /// <returns></returns>
        public List<IQArchive_MediaModel> Delete(string ClientGUID, string ArchiveIDs, string currentUrl, out List<long> lstArchiveID, bool IsLibraryRollup)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGUID", DbType.String, ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ArchiveXML", DbType.Xml, ArchiveIDs, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@v4LibraryRollup", DbType.Boolean, IsLibraryRollup, ParameterDirection.Input));

                DataSet result = DataAccess.GetDataSet("usp_v5_IQArchive_Media_Delete", dataTypeList);

                lstArchiveID = new List<long>();

                foreach (DataRow dr in result.Tables[0].Rows)
                {
                    if (!dr["ArchiveID"].Equals(DBNull.Value))
                    {
                        lstArchiveID.Add(Convert.ToInt64(dr["ArchiveID"]));
                    }
                }

                List<IQArchive_MediaModel> lstOfIQArchive_MediaModel = new List<IQArchive_MediaModel>();
                if (result.Tables.Count > 2)
                {
                    lstOfIQArchive_MediaModel = FillTVParentChild(result.Tables[1], result.Tables[2], currentUrl);
                }

                return lstOfIQArchive_MediaModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQArchive_FilterModel GetIQArchieveFilters(string ClientGUID, string CustomerGUID, DateTime? FromDate, DateTime? ToDate, string SubMediaType, string SearchTerm, string CategoryGUID, string SelectionType, long SinceID, bool IsRadioAccess)
        {
            try
            {

                CustomerGUID = !string.IsNullOrWhiteSpace(CustomerGUID) ? CustomerGUID : null;
                SubMediaType = !string.IsNullOrWhiteSpace(SubMediaType) ? SubMediaType : null;
                SearchTerm = !string.IsNullOrWhiteSpace(SearchTerm) ? SearchTerm : null;
                CategoryGUID = !string.IsNullOrWhiteSpace(CategoryGUID) ? CategoryGUID : null;

                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGUID", DbType.String, ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.String, CustomerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.DateTime, FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.DateTime, ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubMediaType", DbType.String, SubMediaType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchTerm", DbType.String, SearchTerm, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryGUID", DbType.Xml, CategoryGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SelectionType", DbType.String, SelectionType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SinceID", DbType.Int64, SinceID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsRadioAccess", DbType.Boolean, IsRadioAccess, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v5_IQArchive_Media_SelectFilter", dataTypeList);
                IQArchive_FilterModel filter = FillFilterFromDataSet(dataset.Tables[0], dataset.Tables[1], dataset.Tables[2], dataset.Tables[3]);

                return filter;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQArchive_ArchiveClipModel GetArchiveClipByClipID(string ClipID)
        {
            try
            {

                ClipID = !string.IsNullOrWhiteSpace(ClipID) ? ClipID : null;

                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClipID", DbType.String, ClipID, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_ArchiveClip_SelectByClipID", dataTypeList);

                IQArchive_ArchiveClipModel clip = FillClipFromDataSet(dataset);

                return clip;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<IQArchive_MediaModel> GetIQArchieveResultsForEmail(Guid ClientGuid, string ArchiveXML, string currentUrl)
        {
            try
            {

                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ArchiveXML", DbType.Xml, ArchiveXML, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, ClientGuid, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v5_IQArchive_Media_SelectForEmail", dataTypeList);

                Dictionary<string, object> dictresult = FillIQArchieveResults(dataset, currentUrl, false);

                return dictresult["Result"] as List<IQArchive_MediaModel>;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Int64> GetIQArchiveResultsForDashboard(string ClientGUID, string CustomerGUID, DateTime? FromDate, DateTime? ToDate, string SubMediaType, string SearchTerm, string CategoryGUID, string SelectionType, long SinceID, bool IsRadioAccess, bool IsLibraryRollup, bool IsOnlyParents)
        {
            try
            {
                CustomerGUID = !string.IsNullOrWhiteSpace(CustomerGUID) ? CustomerGUID : null;
                SubMediaType = !string.IsNullOrWhiteSpace(SubMediaType) ? SubMediaType : null;
                SearchTerm = !string.IsNullOrWhiteSpace(SearchTerm) ? SearchTerm : null;
                CategoryGUID = !string.IsNullOrWhiteSpace(CategoryGUID) ? CategoryGUID : null;

                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.String, ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.String, CustomerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.DateTime, FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.DateTime, ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubMediaType", DbType.String, SubMediaType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchTerm", DbType.String, SearchTerm, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryGUID", DbType.Xml, CategoryGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsRadioAccess", DbType.Boolean, IsRadioAccess, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SelectionType", DbType.String, SelectionType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SinceID", DbType.Int64, SinceID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@v4LibraryRollup", DbType.Boolean, IsLibraryRollup, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsOnlyParents", DbType.Boolean, IsOnlyParents, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQArchive_Media_SelectForDashboard", dataTypeList);

                return dataset.Tables[0].Rows.OfType<DataRow>().Select(dr => dr.Field<Int64>("ID")).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQArchive_EditModel GetIQArchiveByIDForEdit(string ClientGuid, long ID)
        {
            try
            {

                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGUID", DbType.String, ClientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ID", DbType.Int64, ID, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v5_IQArchive_Media_SelectByIDForEdit", dataTypeList);

                IQArchive_EditModel objIQArchive_EditModel = FillIQArchieveForEdit(dataset);
                if (objIQArchive_EditModel != null)
                {
                    objIQArchive_EditModel.ID = ID;
                }
                return objIQArchive_EditModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQArchive_MediaModel GetIQArchiveByIDForView(long ID)
        {
            try
            {

                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ID", DbType.Int64, ID, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQArchive_Media_SelectByIDForView", dataTypeList);

                IQArchive_MediaModel iQArchive_MediaModel = FillIQArchiveForView(dataset);
                if (iQArchive_MediaModel != null)
                {
                    iQArchive_MediaModel.ID = ID;
                }
                return iQArchive_MediaModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Update_MediaRecord(long ID, string Title, Guid? CategoryGuid, Guid? SubCategory1Guid, Guid? SubCategory2Guid,
                                                    Guid? SubCategory3Guid, string Keywords, string Description, bool? DisplayDescription, short PositiveSentiment, short NegativeSentiment, Guid ClientGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ID", DbType.Int64, ID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Title", DbType.String, Title, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryGuid", DbType.Guid, CategoryGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubCategory1Guid", DbType.Guid, SubCategory1Guid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubCategory2Guid", DbType.Guid, SubCategory2Guid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubCategory3Guid", DbType.Guid, SubCategory3Guid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Keywords", DbType.String, Keywords, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Description", DbType.String, Description, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@DisplayDescription", DbType.Boolean, DisplayDescription, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PositiveSentiment", DbType.Int16, PositiveSentiment, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@NegativeSentiment", DbType.Int16, NegativeSentiment, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, ClientGuid, ParameterDirection.Input));

                string result = DataAccess.ExecuteNonQuery("usp_v5_IQArchive_Media_UpdateMediaRecord", dataTypeList);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<IQArchive_RefreshResultsForTV> GetRefreshResultsForTV(Guid ClientGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, ClientGuid, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQArvhive_Media_RefreshResultsForTV", dataTypeList);

                List<IQArchive_RefreshResultsForTV> lstRefreshResults = FillRefreshResultsForTV(dataset);

                return lstRefreshResults;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Update_ArchiveClipClosedCaption(long ArchiveClipKey, string CC)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ArchiveClipKey", DbType.Int64, ArchiveClipKey, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CC", DbType.Xml, new SqlXml(XmlReader.Create(new StringReader(CC))), ParameterDirection.Input));

                string result = DataAccess.ExecuteNonQuery("usp_v4_IQArchive_Media_UpdateCC_ArchiveClip", dataTypeList);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert_IQ_Report(string ReportTitle, string ReportRule, Int64? p_ReportImage, string ClientGuid, Int64 p_FolderID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ReportTitle", DbType.String, ReportTitle, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReportRule", DbType.String, ReportRule, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReportImageID", DbType.Int64, p_ReportImage, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.String, ClientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@_FolderID", DbType.Int64, p_FolderID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReportID", DbType.Int64, string.Empty, ParameterDirection.Output));


                string result = DataAccess.ExecuteNonQuery("usp_v4_IQ_Report_InsertForIQArchive_Media", dataTypeList);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<IQ_ReportModel> GetLibraryIQ_ReportByClient(string ClientGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGuid", DbType.String, ClientGuid, ParameterDirection.Input));

                DataSet dataSet = DataAccess.GetDataSet("usp_v4_IQ_Report_SelectLibraryReportsByClient", dataTypeList);

                List<IQ_ReportModel> lstIQ_Report = new List<IQ_ReportModel>();

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    lstIQ_Report = FillLibraryIQ_Report(dataSet.Tables[0]);
                }

                return lstIQ_Report;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQArchive_DisplayLibraryReport GetIQArchieveResultsForLibraryReport(long ReportID, string currentUrl, Guid ClientGuid, bool isNielsenData, bool isCompeteData, List<IQ_MediaTypeModel> lstSubMediaTypes, out int totalDisplayedRecords)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ReportID", DbType.Int64, ReportID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, ClientGuid, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v5_IQ_Report_SelectAllMediaForReportByReportID", dataTypeList);

                IQArchive_DisplayLibraryReport objDisplayLibraryReport = new IQArchive_DisplayLibraryReport();
                List<IQ_ReportModel> reports = new List<IQ_ReportModel>();
                string sortField = null;
                bool isAsc = false;
                totalDisplayedRecords = 0;

                if (dataset != null && dataset.Tables.Count > 9)
                {
                    reports = FillLibraryIQ_Report(dataset.Tables[9]);
                }

                if (reports.Count > 0)
                {
                    objDisplayLibraryReport.ReportDetails = reports[0];
                    if (objDisplayLibraryReport.ReportDetails.Settings != null && !String.IsNullOrEmpty(objDisplayLibraryReport.ReportDetails.Settings.Sort))
                    {
                        switch (objDisplayLibraryReport.ReportDetails.Settings.Sort.ToLower())
                        {
                            case "date-":
                                sortField = "MediaDate";
                                isAsc = false;
                                break;
                            case "date+":
                                sortField = "MediaDate";
                                isAsc = true;
                                break;
                            case "audience-":
                                sortField = "Audience";
                                isAsc = false;
                                break;
                            case "audience+":
                                sortField = "Audience";
                                isAsc = true;
                                break;
                        }
                    }
                }
                else
                {
                    objDisplayLibraryReport.ReportDetails = null;
                }

                Dictionary<string, object> dictResults = FillIQArchieveResults(dataset, currentUrl, false, IsReport: true);

                if (dictResults.Count > 0 && dictResults["Result"] != null)
                {
                    List<string> lstNielsenAudienceTypes = lstSubMediaTypes.Where(w => w.RequireNielsenAccess && w.UseAudience && w.TypeLevel == 2).Select(s => s.SubMediaType).ToList();
                    List<string> lstNielsenMediaValueTypes = lstSubMediaTypes.Where(w => w.RequireNielsenAccess && w.UseMediaValue && w.TypeLevel == 2).Select(s => s.SubMediaType).ToList();
                    List<string> lstCompeteAudienceTypes = lstSubMediaTypes.Where(w => w.RequireCompeteAccess && w.UseAudience && w.TypeLevel == 2).Select(s => s.SubMediaType).ToList();
                    List<string> lstCompeteMediaValueTypes = lstSubMediaTypes.Where(w => w.RequireCompeteAccess && w.UseMediaValue && w.TypeLevel == 2).Select(s => s.SubMediaType).ToList();
                    List<string> lstOtherAudienceTypes = lstSubMediaTypes.Where(w => !w.RequireCompeteAccess && !w.RequireNielsenAccess && w.UseAudience && w.TypeLevel == 2).Select(s => s.SubMediaType).ToList();
                    List<string> lstOtherMediaValueTypes = lstSubMediaTypes.Where(w => !w.RequireCompeteAccess && !w.RequireNielsenAccess && w.UseMediaValue && w.TypeLevel == 2).Select(s => s.SubMediaType).ToList();

                    List<IQArchive_MediaModel> archiveResults = dictResults["Result"] as List<IQArchive_MediaModel>;
                    objDisplayLibraryReport.ArchiveResults = archiveResults;

                    string primaryGroup = ReportGroupType.SubMediaType.ToString();
                    string secondaryGroup = ReportGroupType.CategoryName.ToString();
                    if (objDisplayLibraryReport.ReportDetails != null && objDisplayLibraryReport.ReportDetails.Settings != null)
                    {
                        IQ_ReportSettingsModel settings = objDisplayLibraryReport.ReportDetails.Settings;

                        if (!String.IsNullOrEmpty(settings.PrimaryGroup))
                        {
                            primaryGroup = settings.PrimaryGroup;
                        }
                        if (!String.IsNullOrEmpty(settings.SecondaryGroup))
                        {
                            secondaryGroup = settings.PrimaryGroup != settings.SecondaryGroup && settings.SecondaryGroup != ReportGroupType.None.ToString() ? settings.SecondaryGroup : String.Empty;
                        }
                    }

                    List<Tuple<string, string>> group1Elements = new List<Tuple<string, string>>();
                    switch (primaryGroup)
                    {
                        case "SubMediaType":
                            group1Elements = archiveResults.Select(s => new Tuple<string, string>(s.SubMediaType.ToString(), s.SubMediaTypeDesc)).Distinct().OrderBy(o => lstSubMediaTypes.First(f => f.SubMediaType == o.Item1.ToString()).SortOrder).ToList();
                            break;
                        case "CategoryName":
                            group1Elements = archiveResults.Select(s => String.IsNullOrEmpty(s.CategoryName) ? new Tuple<string, string>("-1", "Other") : new Tuple<string, string>(s.CategoryGUID.ToString(), s.CategoryName)).Distinct().OrderBy(o => o.Item2).ToList();
                            break;
                        case "AgentName":
                            group1Elements = archiveResults.Select(s => String.IsNullOrEmpty(s.AgentName) ? new Tuple<string, string>("-1", "Other") : new Tuple<string, string>(s.AgentID.ToString(), s.AgentName)).Distinct().OrderBy(o => o.Item2).ToList();
                            break;
                    }

                    objDisplayLibraryReport.GroupTier1Results = new List<IQArchive_GroupTier1Model>();
                    objDisplayLibraryReport.GroupTier1Counts = new Dictionary<string, int>();
                    foreach (Tuple<string, string> element in group1Elements)
                    {
                        string elementID = element.Item1;
                        string elementName = element.Item2;
                        string elementValue = primaryGroup == "SubMediaType" ? element.Item1 : element.Item2;

                        IQArchive_GroupTier1Model groupTier1Model = new IQArchive_GroupTier1Model()
                        {
                            GroupValue = elementID,
                            GroupName = elementName,
                            IsEnabled = true,
                            TotalAudience = 0,
                            TotalMediaValue = 0
                        };

                        // Get a list of results as dictated by the items' actual values, to ensure that the correct groups and subgroups are displayed
                        List<IQArchive_MediaModel> group1ResultsBase = archiveResults.Where(w => CommonFunctions.GetPropertyValueAsString(w, primaryGroup) == elementValue || (elementValue == "Other" && String.IsNullOrEmpty(CommonFunctions.GetPropertyValueAsString(w, primaryGroup)))).ToList();

                        // Get a list of results as dictated by the custom sorting values for display.
                        // Check custom values against the group ID, since the group name may change. 
                        List<IQArchive_MediaModel> group1Results = archiveResults.Where(w => (!String.IsNullOrEmpty(w.GroupTier1Value) && w.GroupTier1Value == elementID) ||
                                                                                                (String.IsNullOrEmpty(w.GroupTier1Value) &&
                                                                                                    (CommonFunctions.GetPropertyValueAsString(w, primaryGroup) == elementValue ||
                                                                                                    (elementValue == "Other" && String.IsNullOrEmpty(CommonFunctions.GetPropertyValueAsString(w, primaryGroup))))
                                                                                             )
                                                                                        ).ToList();

                        if (String.IsNullOrEmpty(secondaryGroup))
                        {
                            IQArchive_GroupTier2Model groupTier2Model = new IQArchive_GroupTier2Model()
                            {
                                IsEnabled = false,
                                ArchiveResults = group1Results
                            };

                            totalDisplayedRecords += groupTier2Model.ArchiveResults.Count;
                            groupTier1Model.GroupTier2Results = new List<IQArchive_GroupTier2Model>();
                            groupTier1Model.GroupTier2Results.Add(groupTier2Model);
                        }
                        else
                        {
                            List<Tuple<string, string>> group2Elements = new List<Tuple<string, string>>();
                            switch (secondaryGroup)
                            {
                                case "SubMediaType":
                                    group2Elements = group1ResultsBase.Select(s => new Tuple<string, string>(s.SubMediaType.ToString(), s.SubMediaTypeDesc)).Distinct().OrderBy(o => lstSubMediaTypes.First(f => f.SubMediaType == o.Item1.ToString()).SortOrder).ToList();
                                    break;
                                case "CategoryName":
                                    group2Elements = group1ResultsBase.Select(s => String.IsNullOrEmpty(s.CategoryName) ? new Tuple<string, string>("-1", "Other") : new Tuple<string, string>(s.CategoryGUID.ToString(), s.CategoryName)).Distinct().OrderBy(o => o.Item2).ToList();
                                    break;
                                case "AgentName":
                                    group2Elements = group1ResultsBase.Select(s => String.IsNullOrEmpty(s.AgentName) ? new Tuple<string, string>("-1", "Other") : new Tuple<string, string>(s.AgentID.ToString(), s.AgentName)).Distinct().OrderBy(o => o.Item2).ToList();
                                    break;
                            }

                            groupTier1Model.GroupTier2Results = new List<IQArchive_GroupTier2Model>();
                            groupTier1Model.GroupTier2Counts = new Dictionary<string, int>();
                            foreach (Tuple<string, string> element2 in group2Elements)
                            {
                                string element2ID = element2.Item1;
                                string element2Name = element2.Item2;
                                string element2Value = secondaryGroup == "SubMediaType" ? element2.Item1 : element2.Item2;

                                IQArchive_GroupTier2Model groupTier2Model = new IQArchive_GroupTier2Model()
                                {
                                    GroupValue = element2ID,
                                    GroupName = element2Name,
                                    IsEnabled = true,
                                    // Check custom values against the group ID, since the group name may change. 
                                    ArchiveResults = group1Results.Where(w => (!String.IsNullOrEmpty(w.GroupTier2Value) && w.GroupTier2Value == element2ID) ||
                                                                                (String.IsNullOrEmpty(w.GroupTier2Value) &&
                                                                                    (CommonFunctions.GetPropertyValueAsString(w, secondaryGroup) == element2Value ||
                                                                                    (element2Value == "Other" && String.IsNullOrEmpty(CommonFunctions.GetPropertyValueAsString(w, secondaryGroup))))
                                                                                )
                                                                        ).ToList()
                                };

                                totalDisplayedRecords += groupTier2Model.ArchiveResults.Count;
                                groupTier1Model.GroupTier2Counts.Add(element2Name, groupTier2Model.ArchiveResults.Count);
                                groupTier1Model.GroupTier2Results.Add(groupTier2Model);
                            }
                        }

                        foreach (IQArchive_GroupTier2Model groupTier2Model in groupTier1Model.GroupTier2Results)
                        {
                            // Set total audience and media values
                            List<IQArchive_MediaModel> lstMediaResults;
                            if (isNielsenData)
                            {
                                lstMediaResults = groupTier2Model.ArchiveResults.Where(w => lstNielsenAudienceTypes.Contains(w.SubMediaType.ToString())).ToList();
                                if (lstMediaResults.Count > 0)
                                {
                                    groupTier1Model.TotalAudience += lstMediaResults.Sum(s => s.Audience < 0 ? 0 : s.Audience);
                                }

                                lstMediaResults = groupTier2Model.ArchiveResults.Where(w => lstNielsenMediaValueTypes.Contains(w.SubMediaType.ToString())).ToList();
                                if (lstMediaResults.Count > 0)
                                {
                                    groupTier1Model.TotalMediaValue += lstMediaResults.Sum(s => !s.MediaValue.HasValue || s.MediaValue.Value < 0 ? 0 : s.MediaValue.Value);
                                }
                            }
                            if (isCompeteData)
                            {
                                lstMediaResults = groupTier2Model.ArchiveResults.Where(w => lstCompeteAudienceTypes.Contains(w.SubMediaType.ToString())).ToList();
                                if (lstMediaResults.Count > 0)
                                {
                                    groupTier1Model.TotalAudience += lstMediaResults.Sum(s => s.Audience < 0 ? 0 : s.Audience);
                                }

                                lstMediaResults = groupTier2Model.ArchiveResults.Where(w => lstCompeteMediaValueTypes.Contains(w.SubMediaType.ToString())).ToList();
                                if (lstMediaResults.Count > 0)
                                {
                                    groupTier1Model.TotalMediaValue += lstMediaResults.Sum(s => !s.MediaValue.HasValue || s.MediaValue.Value < 0 ? 0 : s.MediaValue.Value);
                                }
                            }

                            lstMediaResults = groupTier2Model.ArchiveResults.Where(w => lstOtherAudienceTypes.Contains(w.SubMediaType.ToString())).ToList();
                            if (lstMediaResults.Count > 0)
                            {
                                groupTier1Model.TotalAudience += lstMediaResults.Sum(s => s.Audience < 0 ? 0 : s.Audience);
                            }

                            lstMediaResults = groupTier2Model.ArchiveResults.Where(w => lstOtherMediaValueTypes.Contains(w.SubMediaType.ToString())).ToList();
                            if (lstMediaResults.Count > 0)
                            {
                                groupTier1Model.TotalMediaValue += lstMediaResults.Sum(s => !s.MediaValue.HasValue || s.MediaValue.Value < 0 ? 0 : s.MediaValue.Value);
                            }

                            switch (sortField)
                            {
                                case "MediaDate":
                                    if (isAsc)
                                    {
                                        groupTier2Model.ArchiveResults = groupTier2Model.ArchiveResults.OrderBy(a => a.Position).ThenBy(a => a.MediaDate).ToList();
                                    }
                                    else
                                    {
                                        groupTier2Model.ArchiveResults = groupTier2Model.ArchiveResults.OrderBy(a => a.Position).ThenByDescending(a => a.MediaDate).ToList();
                                    }
                                    break;
                                case "Audience":
                                    if (isAsc)
                                    {
                                        groupTier2Model.ArchiveResults = groupTier2Model.ArchiveResults.OrderBy(a => a.Position).ThenBy(a => a.Audience).ThenBy(a => a.SecondarySortField).ThenByDescending(a => a.MediaDate).ToList();
                                    }
                                    else
                                    {
                                        groupTier2Model.ArchiveResults = groupTier2Model.ArchiveResults.OrderBy(a => a.Position).ThenByDescending(a => a.Audience).ThenBy(a => a.SecondarySortField).ThenByDescending(a => a.MediaDate).ToList();
                                    }
                                    break;
                                default:
                                    groupTier2Model.ArchiveResults = groupTier2Model.ArchiveResults.OrderBy(a => a.Position).ThenByDescending(a => a.MediaDate).ToList();
                                    break;
                            }
                        }

                        objDisplayLibraryReport.GroupTier1Counts.Add(elementName, group1Results.Count);
                        objDisplayLibraryReport.GroupTier1Results.Add(groupTier1Model);
                    }
                }

                return objDisplayLibraryReport;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQClient_UGCMapModel GetUGCMapByClientGUID(string ClientGuid)
        {
            try
            {

                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGuid", DbType.String, ClientGuid, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQClient_UGCMap_SelectByClientGUID", dataTypeList);

                IQClient_UGCMapModel objIQClient_UGCMapModel = new IQClient_UGCMapModel();

                if (dataset != null && dataset.Tables.Count > 0)
                {
                    foreach (DataRow dr in dataset.Tables[0].Rows)
                    {
                        if (!dr["AutoClip_Status"].Equals(DBNull.Value))
                        {
                            objIQClient_UGCMapModel.AutoClip_Status = Convert.ToBoolean(dr["AutoClip_Status"]);
                        }
                        if (!dr["SourceID"].Equals(DBNull.Value))
                        {
                            objIQClient_UGCMapModel.SourceID = Convert.ToString(dr["SourceID"]);
                        }
                    }
                }

                return objIQClient_UGCMapModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AppendItemsIQReport(Guid ClientGuid, long ReportID, string ReportXML)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, ClientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReportID", DbType.Int64, ReportID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@AppendItems", DbType.Xml, ReportXML, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SuccessCount", DbType.Int32, 0, ParameterDirection.Output));

                string _reuslt = DataAccess.ExecuteNonQuery("usp_v4_IQ_Report_AppendItemsByReportID", dataTypeList);

                return !string.IsNullOrWhiteSpace(_reuslt) ? Convert.ToInt32(_reuslt) : 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RemoveItemsIQReport(Guid ClientGuid, long ReportID, string ReportXML)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, ClientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReportID", DbType.Int64, ReportID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@AppendItems", DbType.Xml, ReportXML, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SuccessCount", DbType.Int32, 0, ParameterDirection.Output));

                string _reuslt = DataAccess.ExecuteNonQuery("usp_v4_IQ_Report_RemoveItemsByReportID", dataTypeList);

                return !string.IsNullOrWhiteSpace(_reuslt) ? Convert.ToInt32(_reuslt) : 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RemoveIQReportByReportID(long ReportID, string ClientGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ReportID", DbType.Int64, ReportID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.String, ClientGuid, ParameterDirection.Input));

                string _reuslt = DataAccess.ExecuteNonQuery("usp_v4_IQ_Report_RemoveReportByReportID", dataTypeList);

                return !string.IsNullOrWhiteSpace(_reuslt) ? Convert.ToInt32(_reuslt) : 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private List<IQ_ReportModel> FillLibraryIQ_Report(DataTable dtReport)
        {
            List<IQ_ReportModel> lstIQ_Report = new List<IQ_ReportModel>();

            if (dtReport != null)
            {
                foreach (DataRow dr in dtReport.Rows)
                {
                    IQ_ReportModel objIQ_ReportModel = new IQ_ReportModel();

                    if (!dr["ID"].Equals(DBNull.Value))
                    {
                        objIQ_ReportModel.ID = Convert.ToInt64(dr["ID"]);
                    }
                    if (!dr["ReportGUID"].Equals(DBNull.Value))
                    {
                        objIQ_ReportModel.ReportGUID = Convert.ToString(dr["ReportGUID"]);
                    }
                    if (!dr["Title"].Equals(DBNull.Value))
                    {
                        objIQ_ReportModel.Title = Convert.ToString(dr["Title"]);
                    }
                    if (!dr["ReportRule"].Equals(DBNull.Value))
                    {
                        objIQ_ReportModel.ReportRule = Convert.ToString(dr["ReportRule"]);
                    }
                    if (!dr["ReportDate"].Equals(DBNull.Value))
                    {
                        objIQ_ReportModel.ReportDate = Convert.ToDateTime(dr["ReportDate"]);
                    }

                    objIQ_ReportModel.Settings = new IQ_ReportSettingsModel();
                    if (dtReport.Columns.Contains("ReportSettings") && !dr["ReportSettings"].Equals(DBNull.Value) && !string.IsNullOrEmpty(Convert.ToString(dr["ReportSettings"])))
                    {
                        objIQ_ReportModel.Settings = (IQ_ReportSettingsModel)IQMedia.Shared.Utility.CommonFunctions.DeserialiazeXml(Convert.ToString(dr["ReportSettings"]), objIQ_ReportModel.Settings);
                    }

                    if (dtReport.Columns.Contains("_ReportImage") && !dr["_ReportImage"].Equals(DBNull.Value))
                    {
                        objIQ_ReportModel._ReportImage = Convert.ToString(dr["_ReportImage"]);
                    }

                    if (dtReport.Columns.Contains("_ReportImageID") && !dr["_ReportImageID"].Equals(DBNull.Value))
                    {
                        objIQ_ReportModel._ReportImageID = Convert.ToInt64(dr["_ReportImageID"]);
                    }

                    if (dtReport.Columns.Contains("RecordCount") && !dr["RecordCount"].Equals(DBNull.Value))
                    {
                        objIQ_ReportModel.RecordCount = Convert.ToInt64(dr["RecordCount"]);
                    }

                    if (dtReport.Columns.Contains("NumSorted") && !dr["NumSorted"].Equals(DBNull.Value))
                    {
                        objIQ_ReportModel.HasCustomSort = Convert.ToInt64(dr["NumSorted"]) > 0;
                    }
                    lstIQ_Report.Add(objIQ_ReportModel);
                }
            }

            return lstIQ_Report;
        }

        private List<IQArchive_RefreshResultsForTV> FillRefreshResultsForTV(DataSet dataSet)
        {
            List<IQArchive_RefreshResultsForTV> lstRefreshResults = new List<IQArchive_RefreshResultsForTV>();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {

                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    IQArchive_RefreshResultsForTV objIQArchive_RefreshResultsForTV = new IQArchive_RefreshResultsForTV();

                    if (!dr["ArchiveClipKey"].Equals(DBNull.Value))
                    {
                        objIQArchive_RefreshResultsForTV.ArchiveClipKey = Convert.ToInt64(dr["ArchiveClipKey"]);
                    }
                    if (!dr["ClipID"].Equals(DBNull.Value))
                    {
                        objIQArchive_RefreshResultsForTV.ClipGuid = Convert.ToString(dr["ClipID"]);
                    }

                    lstRefreshResults.Add(objIQArchive_RefreshResultsForTV);
                }

            }
            return lstRefreshResults;
        }

        private IQArchive_EditModel FillIQArchieveForEdit(DataSet dataSet)
        {
            IQArchive_EditModel objIQArchive_EditModel = new IQArchive_EditModel();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                // dataSet.Tables[0] represents all the CustomCategory

                if (dataSet.Tables[0] != null)
                {
                    objIQArchive_EditModel.Categories = new List<CustomCategoryModel>();

                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        CustomCategoryModel objCustomCategoryModel = new CustomCategoryModel();
                        if (!dr["CategoryGUID"].Equals(DBNull.Value))
                        {
                            objCustomCategoryModel.CategoryGUID = new Guid(Convert.ToString(dr["CategoryGUID"]));
                        }
                        if (!dr["CategoryName"].Equals(DBNull.Value))
                        {
                            objCustomCategoryModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                        }

                        objIQArchive_EditModel.Categories.Add(objCustomCategoryModel);
                    }
                }

                if (dataSet.Tables[1] != null)
                {
                    foreach (DataRow dr in dataSet.Tables[1].Rows)
                    {
                        if (!dr["MediaType"].Equals(DBNull.Value))
                        {
                            objIQArchive_EditModel.MediaType = Convert.ToString(dr["MediaType"]);
                        }
                        if (!dr["SubMediaType"].Equals(DBNull.Value))
                        {
                            objIQArchive_EditModel.SubMediaType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), Convert.ToString(dr["SubMediaType"]));
                        }

                        if (!dr["ArchiveMediaKey"].Equals(DBNull.Value))
                        {
                            objIQArchive_EditModel.ArchiveMediaKey = Convert.ToInt64(dr["ArchiveMediaKey"]);
                        }
                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            objIQArchive_EditModel.Title = Convert.ToString(dr["Title"]);
                        }
                        if (!dr["CategoryGuid"].Equals(DBNull.Value))
                        {
                            objIQArchive_EditModel.CategoryGuid = Convert.ToString(dr["CategoryGuid"]);
                        }
                        if (!dr["SubCategory1Guid"].Equals(DBNull.Value))
                        {
                            objIQArchive_EditModel.SubCategory1Guid = Convert.ToString(dr["SubCategory1Guid"]);
                        }
                        if (!dr["SubCategory2Guid"].Equals(DBNull.Value))
                        {
                            objIQArchive_EditModel.SubCategory2Guid = Convert.ToString(dr["SubCategory2Guid"]);
                        }
                        if (!dr["SubCategory3Guid"].Equals(DBNull.Value))
                        {
                            objIQArchive_EditModel.SubCategory3Guid = Convert.ToString(dr["SubCategory3Guid"]);
                        }
                        if (!dr["Keywords"].Equals(DBNull.Value))
                        {
                            objIQArchive_EditModel.Keywords = Convert.ToString(dr["Keywords"]);
                        }
                        if (!dr["Description"].Equals(DBNull.Value))
                        {
                            objIQArchive_EditModel.Description = Convert.ToString(dr["Description"]);
                        }
                        if (!dr["DisplayDescription"].Equals(DBNull.Value))
                        {
                            objIQArchive_EditModel.DisplayDescription = Convert.ToBoolean(dr["DisplayDescription"]);
                        }
                        if (!dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            objIQArchive_EditModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }
                        if (!dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            objIQArchive_EditModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }
                    }
                }
            }
            return objIQArchive_EditModel;
        }

        private IQArchive_MediaModel FillIQArchiveForView(DataSet dataSet)
        {
            IQArchive_MediaModel iqArchive_MediaModel = null;
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                DataTable dt = dataSet.Tables[0];
                if (dt.Rows.Count == 1)
                {
                    DataRow dr = dataSet.Tables[0].Rows[0];
                    iqArchive_MediaModel = new IQArchive_MediaModel();
                    iqArchive_MediaModel.MediaType = Convert.ToString(dr["MediaType"]);

                    switch (iqArchive_MediaModel.MediaType)
                    {
                        case "PQ":
                            IQArchive_ArchivePQModel iqArchive_ArchivePQModel = new IQArchive_ArchivePQModel();
                            iqArchive_MediaModel.MediaData = iqArchive_ArchivePQModel;

                            if (dt.Columns.Contains("Title") && !dr["Title"].Equals(DBNull.Value))
                            {
                                iqArchive_MediaModel.Title = Convert.ToString(dr["Title"]);
                            }
                            if (dt.Columns.Contains("MediaDate") && !dr["MediaDate"].Equals(DBNull.Value))
                            {
                                iqArchive_MediaModel.MediaDate = Convert.ToDateTime(dr["MediaDate"]);
                            }
                            if (dt.Columns.Contains("Content") && !dr["Content"].Equals(DBNull.Value))
                            {
                                iqArchive_ArchivePQModel.Content = Convert.ToString(dr["Content"]);
                            }
                            if (dt.Columns.Contains("ContentHTML") && !dr["ContentHTML"].Equals(DBNull.Value))
                            {
                                iqArchive_ArchivePQModel.ContentHTML = Convert.ToString(dr["ContentHTML"]);
                            }
                            if (dt.Columns.Contains("Publication") && !dr["Publication"].Equals(DBNull.Value))
                            {
                                iqArchive_ArchivePQModel.Publication = Convert.ToString(dr["Publication"]);
                            }
                            if (dt.Columns.Contains("Author") && !dr["Author"].Equals(DBNull.Value))
                            {
                                if (!String.IsNullOrWhiteSpace(Convert.ToString(dr["Author"])))
                                {
                                    XDocument xDoc = XDocument.Parse(Convert.ToString(dr["Author"]));
                                    iqArchive_ArchivePQModel.Authors = xDoc.Descendants("author").Select(x => x.Value).ToList();
                                }
                            }
                            if (dt.Columns.Contains("Copyright") && !dr["Copyright"].Equals(DBNull.Value))
                            {
                                iqArchive_ArchivePQModel.Copyright = Convert.ToString(dr["Copyright"]);
                            }
                            break;
                    }
                }
            }
            return iqArchive_MediaModel;
        }

        private IQArchive_FilterModel FillFilterFromDataSet(DataTable dtMediaDate, DataTable dtSubMediaType, DataTable dtCustomer, DataTable dtCategory)
        {
            IQArchive_FilterModel filter = new IQArchive_FilterModel();
            filter.MediaTypes = new List<IQArchive_MediaTypeFilter>();
            filter.Customers = new List<IQArchive_Filter>();
            filter.Categories = new List<IQArchive_Filter>();
            filter.Dates = new List<string>();


            if (dtMediaDate != null)
            {
                foreach (DataRow dr in dtMediaDate.Rows)
                {
                    IQArchive_Filter objCustomer = new IQArchive_Filter();
                    if (!dr["MediaDate"].Equals(DBNull.Value))
                    {
                        filter.Dates.Add(Convert.ToDateTime(dr["MediaDate"]).ToString("MM/dd/yyyy"));
                    }
                }
            }

            if (dtSubMediaType != null)
            {
                Dictionary<string, IQArchive_MediaTypeFilter> dictMediaTypes = new Dictionary<string, IQArchive_MediaTypeFilter>();
                foreach (DataRow dr in dtSubMediaType.Rows)
                {
                    string mediaType = String.Empty;
                    string mediaTypeDesc = String.Empty;
                    bool hasSubMediaTypes = false;
                    string subMediaType = String.Empty;
                    string subMediaTypeDesc = String.Empty;
                    long subMediaTypeCount = 0;

                    if (!dr["MediaType"].Equals(DBNull.Value))
                    {
                        mediaType = Convert.ToString(dr["MediaType"]);
                    }
                    if (!dr["MediaTypeDesc"].Equals(DBNull.Value))
                    {
                        mediaTypeDesc = Convert.ToString(dr["MediaTypeDesc"]);
                    }
                    if (!dr["HasSubMediaTypes"].Equals(DBNull.Value))
                    {
                        hasSubMediaTypes = Convert.ToBoolean(dr["HasSubMediaTypes"]);
                    }
                    if (!dr["SubMediaType"].Equals(DBNull.Value))
                    {
                        subMediaType = Convert.ToString(dr["SubMediaType"]);
                    }
                    if (!dr["SubMediaTypeDesc"].Equals(DBNull.Value))
                    {
                        subMediaTypeDesc = Convert.ToString(dr["SubMediaTypeDesc"]);
                    }
                    if (!dr["SubMediaTypeCount"].Equals(DBNull.Value))
                    {
                        subMediaTypeCount = Convert.ToInt64(dr["SubMediaTypeCount"]);
                    }

                    if (!dictMediaTypes.ContainsKey(mediaType))
                    {
                        IQArchive_MediaTypeFilter newFilter = new IQArchive_MediaTypeFilter();
                        newFilter.MediaType = mediaType;
                        newFilter.MediaTypeDesc = mediaTypeDesc;
                        newFilter.SubMediaTypes = new List<IQArchive_SubMediaTypeFilter>();

                        dictMediaTypes.Add(mediaType, newFilter);
                    }

                    IQArchive_MediaTypeFilter mediaTypeFilter = dictMediaTypes[mediaType];
                    mediaTypeFilter.RecordCount += subMediaTypeCount;
                    mediaTypeFilter.SubMediaTypes.Add(new IQArchive_SubMediaTypeFilter()
                    {
                        SubMediaType = subMediaType,
                        SubMediaTypeDesc = subMediaTypeDesc,
                        RecordCount = subMediaTypeCount
                    });
                }

                filter.MediaTypes.AddRange(dictMediaTypes.Values);
            }
            if (dtCustomer != null)
            {
                foreach (DataRow dr in dtCustomer.Rows)
                {
                    IQArchive_Filter objCustomer = new IQArchive_Filter();
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
                    IQArchive_Filter objCategory = new IQArchive_Filter();
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
            return filter;
        }

        private Dictionary<string, object> FillIQArchieveResults(DataSet dataSet, string currentUrl, bool IsEnableFilter, string SortColumn = "", bool IsAsc = false, bool IsReport = false)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            List<IQArchive_MediaModel> listIQArchive_Media = new List<IQArchive_MediaModel>();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                // Represents dbo.ArchiveBLPM table

                if (dataSet.Tables[0] != null)
                {
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        IQArchive_MediaModel objIQArchive_MediaModel = new IQArchive_MediaModel();
                        IQArchive_ArchiveBLPMModel objIQArchive_ArchiveBLPMModel = new IQArchive_ArchiveBLPMModel();

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.ID = Convert.ToInt64(dr["ID"]);
                        }
                        if (!dr["_ArchiveMediaID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.ArchiveMediaID = Convert.ToInt64(dr["_ArchiveMediaID"]);
                        }
                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Title = Convert.ToString(dr["Title"]);
                        }
                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.MediaDate = Convert.ToDateTime(dr["MediaDate"]);
                        }
                        if (!dr["MediaType"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.MediaType = Convert.ToString(dr["MediaType"]);
                        }
                        if (!dr["SubMediaType"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubMediaType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), Convert.ToString(dr["SubMediaType"]));
                        }
                        if (!dr["Content"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Content = Convert.ToString(dr["Content"]);
                        }
                        if (!dr["Description"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Description = Convert.ToString(dr["Description"]);
                        }
                        if (!dr["DisplayDescription"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.DisplayDescription = Convert.ToBoolean(dr["DisplayDescription"]);
                        }

                        if (!dr["Circulation"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveBLPMModel.Circulation = Convert.ToInt32(dr["Circulation"]);
                            objIQArchive_MediaModel.Audience = objIQArchive_ArchiveBLPMModel.Circulation;
                        }
                        if (!dr["FileLocation"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveBLPMModel.FileLocation = Convert.ToString(dr["FileLocation"]);
                        }

                        if (!dr["Pub_Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveBLPMModel.Pub_Name = Convert.ToString(dr["Pub_Name"]);
                        }

                        if (dataSet.Tables[0].Columns.Contains("CategoryGUID") && !dr["CategoryGUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["CategoryGUID"])))
                            {
                                objIQArchive_MediaModel.CategoryGUID = new Guid(Convert.ToString(dr["CategoryGUID"]));
                            }
                        }
                        if (dataSet.Tables[0].Columns.Contains("CategoryName") && !dr["CategoryName"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                        }

                        if (dataSet.Tables[0].Columns.Contains("SubCategory1GUID") && !dr["SubCategory1GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory1GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory1GUID = new Guid(Convert.ToString(dr["SubCategory1GUID"]));
                            }
                        }
                        if (dataSet.Tables[0].Columns.Contains("SubCategory1Name") && !dr["SubCategory1Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory1Name = Convert.ToString(dr["SubCategory1Name"]);
                        }

                        if (dataSet.Tables[0].Columns.Contains("SubCategory2GUID") && !dr["SubCategory2GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory2GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory2GUID = new Guid(Convert.ToString(dr["SubCategory2GUID"]));
                            }
                        }
                        if (dataSet.Tables[0].Columns.Contains("SubCategory2Name") && !dr["SubCategory2Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory2Name = Convert.ToString(dr["SubCategory2Name"]);
                        }

                        if (dataSet.Tables[0].Columns.Contains("SubCategory3GUID") && !dr["SubCategory3GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory3GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory3GUID = new Guid(Convert.ToString(dr["SubCategory3GUID"]));
                            }
                        }
                        if (dataSet.Tables[0].Columns.Contains("SubCategory3Name") && !dr["SubCategory3Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory3Name = Convert.ToString(dr["SubCategory3Name"]);
                        }

                        if (dataSet.Tables[0].Columns.Contains("CreatedDate") && !dr["CreatedDate"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        }

                        if (dataSet.Tables[0].Columns.Contains("IsPublished") && !dr["IsPublished"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.IsPublished = Convert.ToBoolean(dr["IsPublished"]);
                        }

                        if (dataSet.Tables[0].Columns.Contains("AgentID") && !dr["AgentID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.AgentID = Convert.ToInt64(dr["AgentID"]);
                        }

                        if (dataSet.Tables[0].Columns.Contains("AgentName") && !dr["AgentName"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.AgentName = Convert.ToString(dr["AgentName"]);
                        }

                        if (dataSet.Tables[0].Columns.Contains("Position") && !dr["Position"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Position = Convert.ToInt32(dr["Position"]);
                        }

                        if (dataSet.Tables[0].Columns.Contains("GroupTier1Value") && !dr["GroupTier1Value"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.GroupTier1Value = Convert.ToString(dr["GroupTier1Value"]);
                        }

                        if (dataSet.Tables[0].Columns.Contains("GroupTier2Value") && !dr["GroupTier2Value"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.GroupTier2Value = Convert.ToString(dr["GroupTier2Value"]);
                        }

                        if (dataSet.Tables[0].Columns.Contains("SubMediaTypeDesc") && !dr["SubMediaTypeDesc"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubMediaTypeDesc = Convert.ToString(dr["SubMediaTypeDesc"]);
                        }

                        objIQArchive_MediaModel.MediaData = objIQArchive_ArchiveBLPMModel;
                        listIQArchive_Media.Add(objIQArchive_MediaModel);
                    }
                }

                // Represents dbo.ArchiveClip table
                #region TV Fill

                List<IQArchive_MediaModel> listIQArchiveChild_Media = new List<IQArchive_MediaModel>();
                #region TV Child Fill
                if (!IsReport && dataSet.Tables.Count > 9 && dataSet.Tables[9] != null)
                {
                    foreach (DataRow drChild in dataSet.Tables[9].Rows)
                    {
                        IQArchive_ArchiveClipModel objIQArchive_ArchiveClipModel = new IQArchive_ArchiveClipModel();
                        IQArchive_MediaModel objIQArchiveChild_MediaModel = new IQArchive_MediaModel();
                        if (dataSet.Tables[9].Columns.Contains("_ParentID") && !drChild["_ParentID"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel._ParentID = Convert.ToInt64(drChild["_ParentID"]);
                        }

                        if (!drChild["ID"].Equals(DBNull.Value))
                        {
                            objIQArchiveChild_MediaModel.ID = Convert.ToInt64(drChild["ID"]);
                        }

                        if (!drChild["_ArchiveMediaID"].Equals(DBNull.Value))
                        {
                            objIQArchiveChild_MediaModel.ArchiveMediaID = Convert.ToInt64(drChild["_ArchiveMediaID"]);
                        }
                        if (!drChild["Title"].Equals(DBNull.Value))
                        {
                            objIQArchiveChild_MediaModel.Title = Convert.ToString(drChild["Title"]);
                        }
                        if (!drChild["Content"].Equals(DBNull.Value))
                        {
                            objIQArchiveChild_MediaModel.Content = GetClosedCaptionText(Convert.ToString(drChild["Content"]));
                        }
                        if (!drChild["MediaDate"].Equals(DBNull.Value))
                        {
                            objIQArchiveChild_MediaModel.MediaDate = Convert.ToDateTime(drChild["MediaDate"]);
                        }
                        if (!drChild["MediaType"].Equals(DBNull.Value))
                        {
                            objIQArchiveChild_MediaModel.MediaType = Convert.ToString(drChild["MediaType"]);
                        }
                        if (!drChild["SubMediaType"].Equals(DBNull.Value))
                        {
                            objIQArchiveChild_MediaModel.SubMediaType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), Convert.ToString(drChild["SubMediaType"]));
                        }

                        if (!drChild["ClipID"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.ClipID = Convert.ToString(drChild["ClipID"]);
                        }
                        if (!drChild["Nielsen_Audience"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.Nielsen_Audience = Convert.ToInt32(drChild["Nielsen_Audience"]);
                        }
                        if (!drChild["IQAdShareValue"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.IQAdShareValue = Convert.ToDecimal(drChild["IQAdShareValue"]);
                        }
                        if (!drChild["Nielsen_Result"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.Nielsen_Result = Convert.ToString(drChild["Nielsen_Result"]);
                        }
                        if (!drChild["HighlightingText"].Equals(DBNull.Value) && !String.IsNullOrWhiteSpace(Convert.ToString(drChild["HighlightingText"])))
                        {
                            HighlightedCCOutput highlightedCCOutput = new HighlightedCCOutput();
                            objIQArchive_ArchiveClipModel.HighlightedOutput = (HighlightedCCOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(drChild["HighlightingText"]), highlightedCCOutput);
                        }

                        if (dataSet.Tables[9].Columns.Contains("Market") && !drChild["Market"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.Market = Convert.ToString(drChild["Market"]);
                        }

                        if (dataSet.Tables[9].Columns.Contains("Station_Call_Sign") && !drChild["Station_Call_Sign"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.Station_Call_Sign = Convert.ToString(drChild["Station_Call_Sign"]);
                        }

                        if (dataSet.Tables[9].Columns.Contains("PositiveSentiment") && !drChild["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.PositiveSentiment = Convert.ToInt16(drChild["PositiveSentiment"]);
                        }

                        if (dataSet.Tables[9].Columns.Contains("ClipDate") && !drChild["ClipDate"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.LocalDateTime = Convert.ToDateTime(drChild["ClipDate"]);
                        }

                        if (dataSet.Tables[9].Columns.Contains("NegativeSentiment") && !drChild["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.NegativeSentiment = Convert.ToInt16(drChild["NegativeSentiment"]);
                        }

                        if (dataSet.Tables[9].Columns.Contains("StationLogo") && !drChild["StationLogo"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.StationLogo = "https://" + currentUrl + "/StationLogoImages/" + Convert.ToString(drChild["StationLogo"]) + ".jpg";
                            //objIQArchive_ArchiveClipModel.StationLogo = objIQArchive_ArchiveClipModel.StationLogo;
                            //objIQArchive_ArchiveClipModel.StationLogo = Convert.ToString(dr["StationLogo"]);
                        }

                        if (dataSet.Tables[9].Columns.Contains("TimeZone") && !drChild["TimeZone"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.TimeZone = Convert.ToString(drChild["TimeZone"]);
                        }

                        if (dataSet.Tables[9].Columns.Contains("Dma_Num") && !drChild["Dma_Num"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.Dma_Num = Convert.ToString(drChild["Dma_Num"]);
                        }

                        if (dataSet.Tables[9].Columns.Contains("CategoryGUID") && !drChild["CategoryGUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(drChild["CategoryGUID"])))
                            {
                                objIQArchiveChild_MediaModel.CategoryGUID = new Guid(Convert.ToString(drChild["CategoryGUID"]));
                            }
                        }
                        if (dataSet.Tables[9].Columns.Contains("CategoryName") && !drChild["CategoryName"].Equals(DBNull.Value))
                        {
                            objIQArchiveChild_MediaModel.CategoryName = Convert.ToString(drChild["CategoryName"]);
                        }

                        if (dataSet.Tables[9].Columns.Contains("CreatedDate") && !drChild["CreatedDate"].Equals(DBNull.Value))
                        {
                            objIQArchiveChild_MediaModel.CreatedDate = Convert.ToDateTime(drChild["CreatedDate"]);
                        }

                        objIQArchiveChild_MediaModel.MediaData = objIQArchive_ArchiveClipModel;
                        listIQArchiveChild_Media.Add(objIQArchiveChild_MediaModel);
                    }
                }

                #endregion

                #region TV Parent Fill
                if (dataSet.Tables[1] != null)
                {
                    foreach (DataRow dr in dataSet.Tables[1].Rows)
                    {
                        IQArchive_MediaModel objIQArchive_MediaModel = new IQArchive_MediaModel();
                        IQArchive_ArchiveClipModel objIQArchive_ArchiveClipModel = new IQArchive_ArchiveClipModel();
                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.ID = Convert.ToInt64(dr["ID"]);
                        }
                        if (!dr["_ArchiveMediaID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.ArchiveMediaID = Convert.ToInt64(dr["_ArchiveMediaID"]);
                        }
                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Title = Convert.ToString(dr["Title"]);
                        }
                        if (!dr["Content"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Content = GetClosedCaptionText(Convert.ToString(dr["Content"]));
                        }
                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.MediaDate = Convert.ToDateTime(dr["MediaDate"]);
                        }
                        if (!dr["MediaType"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.MediaType = Convert.ToString(dr["MediaType"]);
                        }
                        if (!dr["SubMediaType"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubMediaType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), Convert.ToString(dr["SubMediaType"]));
                        }
                        if (!dr["Description"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Description = Convert.ToString(dr["Description"]);
                        }
                        if (!dr["DisplayDescription"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.DisplayDescription = Convert.ToBoolean(dr["DisplayDescription"]);
                        }

                        if (!dr["ClipID"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.ClipID = Convert.ToString(dr["ClipID"]);
                        }
                        if (!dr["Nielsen_Audience"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.Nielsen_Audience = Convert.ToInt32(dr["Nielsen_Audience"]);
                            objIQArchive_MediaModel.Audience = objIQArchive_ArchiveClipModel.Nielsen_Audience.Value;
                        }
                        if (!dr["IQAdShareValue"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.IQAdShareValue = Convert.ToDecimal(dr["IQAdShareValue"]);
                            objIQArchive_MediaModel.MediaValue = objIQArchive_ArchiveClipModel.IQAdShareValue;
                        }
                        if (!dr["Nielsen_Result"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.Nielsen_Result = Convert.ToString(dr["Nielsen_Result"]);
                        }
                        if (!dr["HighlightingText"].Equals(DBNull.Value) && !String.IsNullOrWhiteSpace(Convert.ToString(dr["HighlightingText"])))
                        {
                            HighlightedCCOutput highlightedCCOutput = new HighlightedCCOutput();
                            objIQArchive_ArchiveClipModel.HighlightedOutput = (HighlightedCCOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedCCOutput);
                        }

                        if (dataSet.Tables[1].Columns.Contains("Market") && !dr["Market"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.Market = Convert.ToString(dr["Market"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("Station_Call_Sign") && !dr["Station_Call_Sign"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.Station_Call_Sign = Convert.ToString(dr["Station_Call_Sign"]);
                            objIQArchive_MediaModel.SecondarySortField = objIQArchive_ArchiveClipModel.Station_Call_Sign;
                        }

                        if (dataSet.Tables[1].Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("ClipDate") && !dr["ClipDate"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.LocalDateTime = Convert.ToDateTime(dr["ClipDate"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("StationLogo") && !dr["StationLogo"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.StationLogo = "https://" + currentUrl + "/StationLogoImages/" + Convert.ToString(dr["StationLogo"]) + ".jpg";
                            //objIQArchive_ArchiveClipModel.StationLogo = objIQArchive_ArchiveClipModel.StationLogo;
                            //objIQArchive_ArchiveClipModel.StationLogo = Convert.ToString(dr["StationLogo"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("TimeZone") && !dr["TimeZone"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.TimeZone = Convert.ToString(dr["TimeZone"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("Dma_Num") && !dr["Dma_Num"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.Dma_Num = Convert.ToString(dr["Dma_Num"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("National_Nielsen_Audience") && !dr["National_Nielsen_Audience"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.National_Nielsen_Audience = Convert.ToInt64(dr["National_Nielsen_Audience"]);
                            objIQArchive_ArchiveClipModel.National_Nielsen_Audience = Convert.ToInt64(dr["National_Nielsen_Audience"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("National_IQAdShareValue") && !dr["National_IQAdShareValue"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.National_IQAdShareValue = Convert.ToDecimal(dr["National_IQAdShareValue"]);
                            objIQArchive_ArchiveClipModel.National_IQAdShareValue = Convert.ToDecimal(dr["National_IQAdShareValue"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("National_Nielsen_Result") && !dr["National_Nielsen_Result"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveClipModel.National_Nielsen_Result = Convert.ToString(dr["National_Nielsen_Result"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("CategoryGUID") && !dr["CategoryGUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["CategoryGUID"])))
                            {
                                objIQArchive_MediaModel.CategoryGUID = new Guid(Convert.ToString(dr["CategoryGUID"]));
                            }
                        }
                        if (dataSet.Tables[1].Columns.Contains("CategoryName") && !dr["CategoryName"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("SubCategory1GUID") && !dr["SubCategory1GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory1GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory1GUID = new Guid(Convert.ToString(dr["SubCategory1GUID"]));
                            }
                        }
                        if (dataSet.Tables[1].Columns.Contains("SubCategory1Name") && !dr["SubCategory1Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory1Name = Convert.ToString(dr["SubCategory1Name"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("SubCategory2GUID") && !dr["SubCategory2GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory2GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory2GUID = new Guid(Convert.ToString(dr["SubCategory2GUID"]));
                            }
                        }
                        if (dataSet.Tables[1].Columns.Contains("SubCategory2Name") && !dr["SubCategory2Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory2Name = Convert.ToString(dr["SubCategory2Name"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("SubCategory3GUID") && !dr["SubCategory3GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory3GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory3GUID = new Guid(Convert.ToString(dr["SubCategory3GUID"]));
                            }
                        }
                        if (dataSet.Tables[1].Columns.Contains("SubCategory3Name") && !dr["SubCategory3Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory3Name = Convert.ToString(dr["SubCategory3Name"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("CreatedDate") && !dr["CreatedDate"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("IsPublished") && !dr["IsPublished"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.IsPublished = Convert.ToBoolean(dr["IsPublished"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("AgentID") && !dr["AgentID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.AgentID = Convert.ToInt64(dr["AgentID"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("AgentName") && !dr["AgentName"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.AgentName = Convert.ToString(dr["AgentName"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("Position") && !dr["Position"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Position = Convert.ToInt32(dr["Position"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("GroupTier1Value") && !dr["GroupTier1Value"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.GroupTier1Value = Convert.ToString(dr["GroupTier1Value"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("GroupTier2Value") && !dr["GroupTier2Value"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.GroupTier2Value = Convert.ToString(dr["GroupTier2Value"]);
                        }

                        if (dataSet.Tables[1].Columns.Contains("SubMediaTypeDesc") && !dr["SubMediaTypeDesc"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubMediaTypeDesc = Convert.ToString(dr["SubMediaTypeDesc"]);
                        }

                        var childs = listIQArchiveChild_Media.Where(a => (a.MediaData as IQArchive_ArchiveClipModel)._ParentID == objIQArchive_MediaModel.ID).OrderByDescending(a => (a.MediaData as IQArchive_ArchiveClipModel).LocalDateTime).ToList();

                        objIQArchive_ArchiveClipModel.ChildResults = childs;

                        objIQArchive_MediaModel.MediaData = objIQArchive_ArchiveClipModel;
                        listIQArchive_Media.Add(objIQArchive_MediaModel);
                    }
                }
                #endregion
                #endregion

                // Represents dbo.ArchiveNM table

                #region NM Fill

                List<IQArchive_MediaModel> listIQArchiveNMChild_Media = new List<IQArchive_MediaModel>();
                #region NM Child Fill
                if (!IsReport && dataSet.Tables.Count > 10 && dataSet.Tables[10] != null)
                {
                    foreach (DataRow dr in dataSet.Tables[10].Rows)
                    {
                        IQArchive_MediaModel objIQArchive_MediaModel = new IQArchive_MediaModel();
                        IQArchive_ArchiveNMModel objIQArchive_ArchiveNMModel = new IQArchive_ArchiveNMModel();

                        if (dataSet.Tables[10].Columns.Contains("_ParentID") && !dr["_ParentID"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveNMModel._ParentID = Convert.ToInt64(dr["_ParentID"]);
                        }

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.ID = Convert.ToInt64(dr["ID"]);
                        }
                        if (!dr["_ArchiveMediaID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.ArchiveMediaID = Convert.ToInt64(dr["_ArchiveMediaID"]);
                        }
                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Title = Convert.ToString(dr["Title"]);
                        }
                        if (!dr["Content"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Content = Convert.ToString(dr["Content"]);
                        }
                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.MediaDate = Convert.ToDateTime(dr["MediaDate"]);
                        }
                        if (!dr["MediaType"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.MediaType = Convert.ToString(dr["MediaType"]);
                        }
                        if (!dr["SubMediaType"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubMediaType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), Convert.ToString(dr["SubMediaType"]));
                        }

                        if (!dr["Url"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveNMModel.Url = Convert.ToString(dr["Url"]);
                        }
                        if (!dr["Compete_Audience"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveNMModel.Compete_Audience = Convert.ToInt32(dr["Compete_Audience"]);
                        }
                        if (!dr["IQAdShareValue"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveNMModel.IQAdShareValue = Convert.ToDecimal(dr["IQAdShareValue"]);
                        }
                        if (!dr["HighlightingText"].Equals(DBNull.Value) && !string.IsNullOrWhiteSpace(Convert.ToString(dr["HighlightingText"])))
                        {
                            HighlightedNewsOutput highlightedNewsOutput = new HighlightedNewsOutput();
                            objIQArchive_ArchiveNMModel.HighlightedOutput = (HighlightedNewsOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedNewsOutput);
                        }

                        if (dataSet.Tables[10].Columns.Contains("Publication") && !dr["Publication"].Equals(DBNull.Value))
                        {
                            Uri aPublisherUri;
                            objIQArchive_ArchiveNMModel.Publication = Uri.TryCreate(Convert.ToString(dr["Publication"]), UriKind.Absolute, out aPublisherUri) ? aPublisherUri.Host.Replace("www.", string.Empty) : Convert.ToString(dr["Publication"]);
                        }

                        if (!dr["Compete_Result"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveNMModel.Compete_Result = Convert.ToString(dr["Compete_Result"]);
                        }

                        if (dataSet.Tables[10].Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveNMModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }

                        if (dataSet.Tables[10].Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveNMModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }

                        if (dataSet.Tables[10].Columns.Contains("CategoryGUID") && !dr["CategoryGUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["CategoryGUID"])))
                            {
                                objIQArchive_MediaModel.CategoryGUID = new Guid(Convert.ToString(dr["CategoryGUID"]));
                            }
                        }
                        if (dataSet.Tables[10].Columns.Contains("CategoryName") && !dr["CategoryName"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                        }

                        if (dataSet.Tables[10].Columns.Contains("CreatedDate") && !dr["CreatedDate"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        }

                        if (dataSet.Tables[10].Columns.Contains("IQLicense") && !dr["IQLicense"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveNMModel.IQLicense = Convert.ToInt16(dr["IQLicense"]);
                        }

                        objIQArchive_MediaModel.MediaData = objIQArchive_ArchiveNMModel;
                        listIQArchiveNMChild_Media.Add(objIQArchive_MediaModel);

                    }
                }
                #endregion

                #region NM Parent Fill
                if (dataSet.Tables[2] != null)
                {
                    foreach (DataRow dr in dataSet.Tables[2].Rows)
                    {
                        IQArchive_MediaModel objIQArchive_MediaModel = new IQArchive_MediaModel();
                        IQArchive_ArchiveNMModel objIQArchive_ArchiveNMModel = new IQArchive_ArchiveNMModel();

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.ID = Convert.ToInt64(dr["ID"]);
                        }
                        if (!dr["_ArchiveMediaID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.ArchiveMediaID = Convert.ToInt64(dr["_ArchiveMediaID"]);
                        }
                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Title = Convert.ToString(dr["Title"]);
                        }
                        if (!dr["Content"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Content = Convert.ToString(dr["Content"]);
                        }
                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.MediaDate = Convert.ToDateTime(dr["MediaDate"]);
                        }
                        if (!dr["MediaType"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.MediaType = Convert.ToString(dr["MediaType"]);
                        }
                        if (!dr["SubMediaType"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubMediaType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), Convert.ToString(dr["SubMediaType"]));
                        }
                        if (!dr["Description"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Description = Convert.ToString(dr["Description"]);
                        }
                        if (!dr["DisplayDescription"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.DisplayDescription = Convert.ToBoolean(dr["DisplayDescription"]);
                        }

                        if (!dr["Url"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveNMModel.Url = Convert.ToString(dr["Url"]);
                        }
                        if (!dr["Compete_Audience"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveNMModel.Compete_Audience = Convert.ToInt32(dr["Compete_Audience"]);
                            objIQArchive_MediaModel.Audience = objIQArchive_ArchiveNMModel.Compete_Audience.Value;
                        }
                        if (!dr["IQAdShareValue"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveNMModel.IQAdShareValue = Convert.ToDecimal(dr["IQAdShareValue"]);
                            objIQArchive_MediaModel.MediaValue = objIQArchive_ArchiveNMModel.IQAdShareValue;
                        }
                        if (!dr["HighlightingText"].Equals(DBNull.Value) && !string.IsNullOrWhiteSpace(Convert.ToString(dr["HighlightingText"])))
                        {
                            HighlightedNewsOutput highlightedNewsOutput = new HighlightedNewsOutput();
                            objIQArchive_ArchiveNMModel.HighlightedOutput = (HighlightedNewsOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedNewsOutput);
                        }

                        if (dataSet.Tables[2].Columns.Contains("Publication") && !dr["Publication"].Equals(DBNull.Value))
                        {
                            Uri aPublisherUri;
                            objIQArchive_ArchiveNMModel.Publication = Uri.TryCreate(Convert.ToString(dr["Publication"]), UriKind.Absolute, out aPublisherUri) ? aPublisherUri.Host.Replace("www.", string.Empty) : Convert.ToString(dr["Publication"]);
                        }

                        if (!dr["Compete_Result"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveNMModel.Compete_Result = Convert.ToString(dr["Compete_Result"]);
                        }

                        if (dataSet.Tables[2].Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveNMModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }

                        if (dataSet.Tables[2].Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveNMModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }

                        if (dataSet.Tables[2].Columns.Contains("CategoryGUID") && !dr["CategoryGUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["CategoryGUID"])))
                            {
                                objIQArchive_MediaModel.CategoryGUID = new Guid(Convert.ToString(dr["CategoryGUID"]));
                            }
                        }
                        if (dataSet.Tables[2].Columns.Contains("CategoryName") && !dr["CategoryName"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                        }

                        if (dataSet.Tables[2].Columns.Contains("SubCategory1GUID") && !dr["SubCategory1GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory1GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory1GUID = new Guid(Convert.ToString(dr["SubCategory1GUID"]));
                            }
                        }
                        if (dataSet.Tables[2].Columns.Contains("SubCategory1Name") && !dr["SubCategory1Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory1Name = Convert.ToString(dr["SubCategory1Name"]);
                        }

                        if (dataSet.Tables[2].Columns.Contains("SubCategory2GUID") && !dr["SubCategory2GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory2GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory2GUID = new Guid(Convert.ToString(dr["SubCategory2GUID"]));
                            }
                        }
                        if (dataSet.Tables[2].Columns.Contains("SubCategory2Name") && !dr["SubCategory2Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory2Name = Convert.ToString(dr["SubCategory2Name"]);
                        }

                        if (dataSet.Tables[2].Columns.Contains("SubCategory3GUID") && !dr["SubCategory3GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory3GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory3GUID = new Guid(Convert.ToString(dr["SubCategory3GUID"]));
                            }
                        }
                        if (dataSet.Tables[2].Columns.Contains("SubCategory3Name") && !dr["SubCategory3Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory3Name = Convert.ToString(dr["SubCategory3Name"]);
                        }

                        if (dataSet.Tables[2].Columns.Contains("CreatedDate") && !dr["CreatedDate"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        }

                        if (dataSet.Tables[2].Columns.Contains("IQLicense") && !dr["IQLicense"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveNMModel.IQLicense = Convert.ToInt16(dr["IQLicense"]);
                        }

                        if (dataSet.Tables[2].Columns.Contains("IsPublished") && !dr["IsPublished"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.IsPublished = Convert.ToBoolean(dr["IsPublished"]);
                        }

                        if (dataSet.Tables[2].Columns.Contains("CompeteURL") && !dr["CompeteURL"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SecondarySortField = Convert.ToString(dr["CompeteURL"]);
                        }

                        if (dataSet.Tables[2].Columns.Contains("AgentID") && !dr["AgentID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.AgentID = Convert.ToInt64(dr["AgentID"]);
                        }

                        if (dataSet.Tables[2].Columns.Contains("AgentName") && !dr["AgentName"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.AgentName = Convert.ToString(dr["AgentName"]);
                        }

                        if (dataSet.Tables[2].Columns.Contains("Position") && !dr["Position"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Position = Convert.ToInt32(dr["Position"]);
                        }

                        if (dataSet.Tables[2].Columns.Contains("GroupTier1Value") && !dr["GroupTier1Value"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.GroupTier1Value = Convert.ToString(dr["GroupTier1Value"]);
                        }

                        if (dataSet.Tables[2].Columns.Contains("GroupTier2Value") && !dr["GroupTier2Value"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.GroupTier2Value = Convert.ToString(dr["GroupTier2Value"]);
                        }

                        if (dataSet.Tables[2].Columns.Contains("SubMediaTypeDesc") && !dr["SubMediaTypeDesc"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubMediaTypeDesc = Convert.ToString(dr["SubMediaTypeDesc"]);
                        }

                        var childs = listIQArchiveNMChild_Media.Where(a => (a.MediaData as IQArchive_ArchiveNMModel)._ParentID == objIQArchive_MediaModel.ID).OrderByDescending(a => a.MediaDate).ToList();

                        objIQArchive_ArchiveNMModel.ChildResults = childs;

                        objIQArchive_MediaModel.MediaData = objIQArchive_ArchiveNMModel;
                        listIQArchive_Media.Add(objIQArchive_MediaModel);
                    }
                }

                #endregion

                #endregion

                // Represents dbo.ArchiveSM table

                if (dataSet.Tables[3] != null)
                {
                    foreach (DataRow dr in dataSet.Tables[3].Rows)
                    {
                        IQArchive_MediaModel objIQArchive_MediaModel = new IQArchive_MediaModel();
                        IQArchive_ArchiveSMModel objIQArchive_ArchiveSMModel = new IQArchive_ArchiveSMModel();

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.ID = Convert.ToInt64(dr["ID"]);
                        }
                        if (!dr["_ArchiveMediaID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.ArchiveMediaID = Convert.ToInt64(dr["_ArchiveMediaID"]);
                        }
                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Title = Convert.ToString(dr["Title"]);
                        }
                        if (!dr["Content"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Content = Convert.ToString(dr["Content"]);
                        }
                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.MediaDate = Convert.ToDateTime(dr["MediaDate"]);
                        }
                        if (!dr["MediaType"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.MediaType = Convert.ToString(dr["MediaType"]);
                        }
                        if (!dr["SubMediaType"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubMediaType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), Convert.ToString(dr["SubMediaType"]));
                        }
                        if (!dr["Description"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Description = Convert.ToString(dr["Description"]);
                        }
                        if (!dr["DisplayDescription"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.DisplayDescription = Convert.ToBoolean(dr["DisplayDescription"]);
                        }


                        if (!dr["Url"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveSMModel.Url = Convert.ToString(dr["Url"]);
                        }
                        if (!dr["Compete_Audience"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveSMModel.Compete_Audience = Convert.ToInt32(dr["Compete_Audience"]);
                            objIQArchive_MediaModel.Audience = objIQArchive_ArchiveSMModel.Compete_Audience.Value;
                        }
                        if (!dr["HighlightingText"].Equals(DBNull.Value) && !string.IsNullOrWhiteSpace(Convert.ToString(dr["HighlightingText"])))
                        {
                            HighlightedSMOutput highlightedSMOutput = new HighlightedSMOutput();
                            objIQArchive_ArchiveSMModel.HighlightedOutput = (HighlightedSMOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedSMOutput);
                        }

                        if (dataSet.Tables[3].Columns.Contains("homelink") && !dr["homelink"].Equals(DBNull.Value))
                        {
                            Uri aPublisherUri;
                            objIQArchive_ArchiveSMModel.Publication = Uri.TryCreate(Convert.ToString(dr["homelink"]), UriKind.Absolute, out aPublisherUri) ? aPublisherUri.Host.Replace("www.", string.Empty) : Convert.ToString(dr["homelink"]);
                        }

                        if (!dr["IQAdShareValue"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveSMModel.IQAdShareValue = Convert.ToDecimal(dr["IQAdShareValue"]);
                            objIQArchive_MediaModel.MediaValue = objIQArchive_ArchiveSMModel.IQAdShareValue;
                        }
                        if (!dr["Compete_Result"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveSMModel.Compete_Result = Convert.ToString(dr["Compete_Result"]);
                        }

                        if (dataSet.Tables[3].Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveSMModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }

                        if (dataSet.Tables[3].Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveSMModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }

                        if (dataSet.Tables[3].Columns.Contains("CategoryGUID") && !dr["CategoryGUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["CategoryGUID"])))
                            {
                                objIQArchive_MediaModel.CategoryGUID = new Guid(Convert.ToString(dr["CategoryGUID"]));
                            }
                        }
                        if (dataSet.Tables[3].Columns.Contains("CategoryName") && !dr["CategoryName"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                        }

                        if (dataSet.Tables[3].Columns.Contains("SubCategory1GUID") && !dr["SubCategory1GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory1GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory1GUID = new Guid(Convert.ToString(dr["SubCategory1GUID"]));
                            }
                        }
                        if (dataSet.Tables[3].Columns.Contains("SubCategory1Name") && !dr["SubCategory1Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory1Name = Convert.ToString(dr["SubCategory1Name"]);
                        }

                        if (dataSet.Tables[3].Columns.Contains("SubCategory2GUID") && !dr["SubCategory2GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory2GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory2GUID = new Guid(Convert.ToString(dr["SubCategory2GUID"]));
                            }
                        }
                        if (dataSet.Tables[3].Columns.Contains("SubCategory2Name") && !dr["SubCategory2Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory2Name = Convert.ToString(dr["SubCategory2Name"]);
                        }

                        if (dataSet.Tables[3].Columns.Contains("SubCategory3GUID") && !dr["SubCategory3GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory3GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory3GUID = new Guid(Convert.ToString(dr["SubCategory3GUID"]));
                            }
                        }
                        if (dataSet.Tables[3].Columns.Contains("SubCategory3Name") && !dr["SubCategory3Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory3Name = Convert.ToString(dr["SubCategory3Name"]);
                        }

                        if (dataSet.Tables[3].Columns.Contains("CreatedDate") && !dr["CreatedDate"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        }

                        if (dataSet.Tables[3].Columns.Contains("IsPublished") && !dr["IsPublished"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.IsPublished = Convert.ToBoolean(dr["IsPublished"]);
                        }

                        if (dataSet.Tables[3].Columns.Contains("ThumbUrl") && !dr["ThumbUrl"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveSMModel.ThumbUrl = Convert.ToString(dr["ThumbUrl"]);
                        }

                        if (dataSet.Tables[3].Columns.Contains("ArticleStats") && !dr["ArticleStats"].Equals(DBNull.Value) && !String.IsNullOrWhiteSpace(Convert.ToString(dr["ArticleStats"])))
                        {
                            ArticleStatsModel statsModel = new ArticleStatsModel();
                            objIQArchive_ArchiveSMModel.ArticleStats = (ArticleStatsModel)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["ArticleStats"]), statsModel);
                        }

                        if (dataSet.Tables[3].Columns.Contains("CompeteURL") && !dr["CompeteURL"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SecondarySortField = Convert.ToString(dr["CompeteURL"]);
                        }

                        if (dataSet.Tables[3].Columns.Contains("AgentID") && !dr["AgentID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.AgentID = Convert.ToInt64(dr["AgentID"]);
                        }

                        if (dataSet.Tables[3].Columns.Contains("AgentName") && !dr["AgentName"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.AgentName = Convert.ToString(dr["AgentName"]);
                        }

                        if (dataSet.Tables[3].Columns.Contains("Position") && !dr["Position"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Position = Convert.ToInt32(dr["Position"]);
                        }

                        if (dataSet.Tables[3].Columns.Contains("GroupTier1Value") && !dr["GroupTier1Value"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.GroupTier1Value = Convert.ToString(dr["GroupTier1Value"]);
                        }

                        if (dataSet.Tables[3].Columns.Contains("GroupTier2Value") && !dr["GroupTier2Value"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.GroupTier2Value = Convert.ToString(dr["GroupTier2Value"]);
                        }

                        if (dataSet.Tables[3].Columns.Contains("SubMediaTypeDesc") && !dr["SubMediaTypeDesc"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubMediaTypeDesc = Convert.ToString(dr["SubMediaTypeDesc"]);
                        }

                        objIQArchive_MediaModel.MediaData = objIQArchive_ArchiveSMModel;
                        listIQArchive_Media.Add(objIQArchive_MediaModel);
                    }
                }

                // Represents dbo.ArchiveTweets table

                if (dataSet.Tables[4] != null)
                {
                    foreach (DataRow dr in dataSet.Tables[4].Rows)
                    {
                        IQArchive_MediaModel objIQArchive_MediaModel = new IQArchive_MediaModel();
                        IQArchive_ArchiveTweetsModel objIQArchive_ArchiveTweetsModel = new IQArchive_ArchiveTweetsModel();

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.ID = Convert.ToInt64(dr["ID"]);
                        }
                        if (!dr["_ArchiveMediaID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.ArchiveMediaID = Convert.ToInt64(dr["_ArchiveMediaID"]);
                        }
                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Title = Convert.ToString(dr["Title"]);
                        }
                        if (!dr["Content"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Content = Convert.ToString(dr["Content"]);
                        }
                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.MediaDate = Convert.ToDateTime(dr["MediaDate"]);
                        }
                        if (!dr["MediaType"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.MediaType = Convert.ToString(dr["MediaType"]);
                        }
                        if (!dr["SubMediaType"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubMediaType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), Convert.ToString(dr["SubMediaType"]));
                        }
                        if (!dr["Description"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Description = Convert.ToString(dr["Description"]);
                        }
                        if (!dr["DisplayDescription"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.DisplayDescription = Convert.ToBoolean(dr["DisplayDescription"]);
                        }
                        if (!dr["Actor_DisplayName"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveTweetsModel.ActorDisplayname = Convert.ToString(dr["Actor_DisplayName"]);
                            objIQArchive_MediaModel.SecondarySortField = objIQArchive_ArchiveTweetsModel.ActorDisplayname;
                        }
                        if (!dr["Actor_PreferredUserName"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveTweetsModel.PreferredUserName = Convert.ToString(dr["Actor_PreferredUserName"]);
                        }
                        if (!dr["Actor_FollowersCount"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveTweetsModel.FollowersCount = Convert.ToInt32(dr["Actor_FollowersCount"]);
                            objIQArchive_MediaModel.Audience = objIQArchive_ArchiveTweetsModel.FollowersCount;
                        }
                        if (!dr["Actor_FriendsCount"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveTweetsModel.FreiendsCount = Convert.ToInt32(dr["Actor_FriendsCount"]);
                        }
                        if (!dr["Actor_Image"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveTweetsModel.ActorImage = Convert.ToString(dr["Actor_Image"]);
                        }
                        if (!dr["Actor_link"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveTweetsModel.ActorLink = Convert.ToString(dr["Actor_link"]);
                        }
                        if (!dr["Tweet_ID"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveTweetsModel.TweetID = Convert.ToInt64(dr["Tweet_ID"]);
                        }
                        if (!dr["gnip_Klout_Score"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveTweetsModel.KloutScore = Convert.ToString(dr["gnip_Klout_Score"]);
                        }
                        if (!dr["HighlightingText"].Equals(DBNull.Value) && !String.IsNullOrWhiteSpace(Convert.ToString(dr["HighlightingText"])))
                        {
                            HighlightedTWOutput highlightedTWOutput = new HighlightedTWOutput();
                            objIQArchive_ArchiveTweetsModel.HighlightedOutput = (HighlightedTWOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedTWOutput);
                        }

                        if (dataSet.Tables[4].Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveTweetsModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }

                        if (dataSet.Tables[4].Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveTweetsModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }


                        if (dataSet.Tables[4].Columns.Contains("CategoryGUID") && !dr["CategoryGUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["CategoryGUID"])))
                            {
                                objIQArchive_MediaModel.CategoryGUID = new Guid(Convert.ToString(dr["CategoryGUID"]));
                            }
                        }
                        if (dataSet.Tables[4].Columns.Contains("CategoryName") && !dr["CategoryName"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                        }

                        if (dataSet.Tables[4].Columns.Contains("SubCategory1GUID") && !dr["SubCategory1GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory1GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory1GUID = new Guid(Convert.ToString(dr["SubCategory1GUID"]));
                            }
                        }
                        if (dataSet.Tables[4].Columns.Contains("SubCategory1Name") && !dr["SubCategory1Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory1Name = Convert.ToString(dr["SubCategory1Name"]);
                        }

                        if (dataSet.Tables[4].Columns.Contains("SubCategory2GUID") && !dr["SubCategory2GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory2GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory2GUID = new Guid(Convert.ToString(dr["SubCategory2GUID"]));
                            }
                        }
                        if (dataSet.Tables[4].Columns.Contains("SubCategory2Name") && !dr["SubCategory2Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory2Name = Convert.ToString(dr["SubCategory2Name"]);
                        }

                        if (dataSet.Tables[4].Columns.Contains("SubCategory3GUID") && !dr["SubCategory3GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory3GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory3GUID = new Guid(Convert.ToString(dr["SubCategory3GUID"]));
                            }
                        }
                        if (dataSet.Tables[4].Columns.Contains("SubCategory3Name") && !dr["SubCategory3Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory3Name = Convert.ToString(dr["SubCategory3Name"]);
                        }

                        if (dataSet.Tables[4].Columns.Contains("CreatedDate") && !dr["CreatedDate"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        }

                        if (dataSet.Tables[4].Columns.Contains("IsPublished") && !dr["IsPublished"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.IsPublished = Convert.ToBoolean(dr["IsPublished"]);
                        }

                        if (dataSet.Tables[4].Columns.Contains("AgentID") && !dr["AgentID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.AgentID = Convert.ToInt64(dr["AgentID"]);
                        }

                        if (dataSet.Tables[4].Columns.Contains("AgentName") && !dr["AgentName"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.AgentName = Convert.ToString(dr["AgentName"]);
                        }

                        if (dataSet.Tables[4].Columns.Contains("Position") && !dr["Position"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Position = Convert.ToInt32(dr["Position"]);
                        }

                        if (dataSet.Tables[4].Columns.Contains("GroupTier1Value") && !dr["GroupTier1Value"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.GroupTier1Value = Convert.ToString(dr["GroupTier1Value"]);
                        }

                        if (dataSet.Tables[4].Columns.Contains("GroupTier2Value") && !dr["GroupTier2Value"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.GroupTier2Value = Convert.ToString(dr["GroupTier2Value"]);
                        }

                        if (dataSet.Tables[4].Columns.Contains("SubMediaTypeDesc") && !dr["SubMediaTypeDesc"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubMediaTypeDesc = Convert.ToString(dr["SubMediaTypeDesc"]);
                        }

                        objIQArchive_MediaModel.MediaData = objIQArchive_ArchiveTweetsModel;
                        listIQArchive_Media.Add(objIQArchive_MediaModel);
                    }
                }


                // Represents dbo.ArchiveTVEyes table

                if (dataSet.Tables[5] != null)
                {
                    foreach (DataRow dr in dataSet.Tables[5].Rows)
                    {
                        IQArchive_MediaModel objIQArchive_MediaModel = new IQArchive_MediaModel();
                        IQArchive_ArchiveTVEyesModel objIQArchive_ArchiveTVEyesModel = new IQArchive_ArchiveTVEyesModel();

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.ID = Convert.ToInt64(dr["ID"]);
                        }
                        if (!dr["_ArchiveMediaID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.ArchiveMediaID = Convert.ToInt64(dr["_ArchiveMediaID"]);
                        }
                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Title = Convert.ToString(dr["Title"]);
                        }
                        if (!dr["Content"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Content = Convert.ToString(dr["Content"]);
                        }
                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.MediaDate = Convert.ToDateTime(dr["MediaDate"]);
                        }
                        if (!dr["Description"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Description = Convert.ToString(dr["Description"]);
                        }
                        if (!dr["DisplayDescription"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.DisplayDescription = Convert.ToBoolean(dr["DisplayDescription"]);
                        }

                        if (dataSet.Tables[5].Columns.Contains("LocalDateTime") && !dr["LocalDateTime"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveTVEyesModel.LocalDateTime = Convert.ToDateTime(dr["LocalDateTime"]);
                        }

                        if (dataSet.Tables[5].Columns.Contains("TimeZone") && !dr["TimeZone"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveTVEyesModel.TimeZone = Convert.ToString(dr["TimeZone"]);
                        }

                        if (!dr["MediaType"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.MediaType = Convert.ToString(dr["MediaType"]);
                        }

                        if (!dr["SubMediaType"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubMediaType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), Convert.ToString(dr["SubMediaType"]));
                        }

                        if (dataSet.Tables[5].Columns.Contains("CategoryGUID") && !dr["CategoryGUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["CategoryGUID"])))
                            {
                                objIQArchive_MediaModel.CategoryGUID = new Guid(Convert.ToString(dr["CategoryGUID"]));
                            }
                        }
                        if (dataSet.Tables[5].Columns.Contains("CategoryName") && !dr["CategoryName"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                        }

                        if (dataSet.Tables[5].Columns.Contains("SubCategory1GUID") && !dr["SubCategory1GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory1GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory1GUID = new Guid(Convert.ToString(dr["SubCategory1GUID"]));
                            }
                        }
                        if (dataSet.Tables[5].Columns.Contains("SubCategory1Name") && !dr["SubCategory1Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory1Name = Convert.ToString(dr["SubCategory1Name"]);
                        }

                        if (dataSet.Tables[5].Columns.Contains("SubCategory2GUID") && !dr["SubCategory2GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory2GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory2GUID = new Guid(Convert.ToString(dr["SubCategory2GUID"]));
                            }
                        }
                        if (dataSet.Tables[5].Columns.Contains("SubCategory2Name") && !dr["SubCategory2Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory2Name = Convert.ToString(dr["SubCategory2Name"]);
                        }

                        if (dataSet.Tables[5].Columns.Contains("SubCategory3GUID") && !dr["SubCategory3GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory3GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory3GUID = new Guid(Convert.ToString(dr["SubCategory3GUID"]));
                            }
                        }
                        if (dataSet.Tables[5].Columns.Contains("SubCategory3Name") && !dr["SubCategory3Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory3Name = Convert.ToString(dr["SubCategory3Name"]);
                        }

                        if (dataSet.Tables[5].Columns.Contains("CreatedDate") && !dr["CreatedDate"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        }

                        if (!dr["StationID"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveTVEyesModel.StationID = Convert.ToString(dr["StationID"]);
                        }

                        if (!dr["Market"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveTVEyesModel.Market = Convert.ToString(dr["Market"]);
                        }

                        if (!dr["DMARank"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveTVEyesModel.DMARank = Convert.ToString(dr["DMARank"]);
                        }

                        if (dataSet.Tables[5].Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveTVEyesModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }

                        if (dataSet.Tables[5].Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveTVEyesModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }

                        if (dataSet.Tables[5].Columns.Contains("IsPublished") && !dr["IsPublished"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.IsPublished = Convert.ToBoolean(dr["IsPublished"]);
                        }

                        if (dataSet.Tables[5].Columns.Contains("AgentID") && !dr["AgentID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.AgentID = Convert.ToInt64(dr["AgentID"]);
                        }

                        if (dataSet.Tables[5].Columns.Contains("AgentName") && !dr["AgentName"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.AgentName = Convert.ToString(dr["AgentName"]);
                        }

                        if (dataSet.Tables[5].Columns.Contains("Position") && !dr["Position"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Position = Convert.ToInt32(dr["Position"]);
                        }

                        if (dataSet.Tables[5].Columns.Contains("GroupTier1Value") && !dr["GroupTier1Value"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.GroupTier1Value = Convert.ToString(dr["GroupTier1Value"]);
                        }

                        if (dataSet.Tables[5].Columns.Contains("GroupTier2Value") && !dr["GroupTier2Value"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.GroupTier2Value = Convert.ToString(dr["GroupTier2Value"]);
                        }

                        if (dataSet.Tables[5].Columns.Contains("SubMediaTypeDesc") && !dr["SubMediaTypeDesc"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubMediaTypeDesc = Convert.ToString(dr["SubMediaTypeDesc"]);
                        }

                        objIQArchive_MediaModel.MediaData = objIQArchive_ArchiveTVEyesModel;
                        listIQArchive_Media.Add(objIQArchive_MediaModel);
                    }
                }

                // Represents dbo.ArchiveMisc table

                if (dataSet.Tables[6] != null)
                {
                    foreach (DataRow dr in dataSet.Tables[6].Rows)
                    {
                        IQArchive_MediaModel objIQArchive_MediaModel = new IQArchive_MediaModel();
                        IQArchive_ArchiveMiscModel objIQArchive_ArchiveMiscModel = new IQArchive_ArchiveMiscModel();

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.ID = Convert.ToInt64(dr["ID"]);
                        }
                        if (!dr["_ArchiveMediaID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.ArchiveMediaID = Convert.ToInt64(dr["_ArchiveMediaID"]);
                        }
                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Title = Convert.ToString(dr["Title"]);
                        }
                        if (!dr["Content"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Content = Convert.ToString(dr["Content"]);
                        }
                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.MediaDate = Convert.ToDateTime(dr["MediaDate"]);
                        }
                        if (!dr["Description"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Description = Convert.ToString(dr["Description"]);
                        }
                        if (dataSet.Tables[6].Columns.Contains("CreateDT") && !dr["CreateDT"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveMiscModel.CreateDT = Convert.ToDateTime(dr["CreateDT"]);
                        }
                        if (dataSet.Tables[6].Columns.Contains("CreateDTTimeZone") && !dr["CreateDTTimeZone"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveMiscModel.TimeZone = Convert.ToString(dr["CreateDTTimeZone"]);
                        }
                        if (dataSet.Tables[6].Columns.Contains("FileType") && !dr["FileType"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveMiscModel.FileType = (CommonFunctions.IQUGCMediaTypes)Enum.Parse(typeof(CommonFunctions.IQUGCMediaTypes), Convert.ToString(dr["FileType"]));
                        }
                        if (dataSet.Tables[6].Columns.Contains("FileTypeExt") && !dr["FileTypeExt"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveMiscModel.FileTypeExt = Convert.ToString(dr["FileTypeExt"]);
                        }
                        if (dataSet.Tables[6].Columns.Contains("MediaUrl") && !dr["MediaUrl"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveMiscModel.MediaUrl = Convert.ToString(dr["MediaUrl"]);
                        }
                        if (!dr["MediaType"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.MediaType = Convert.ToString(dr["MediaType"]);
                        }
                        if (!dr["SubMediaType"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubMediaType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), Convert.ToString(dr["SubMediaType"]));
                        }
                        if (dataSet.Tables[6].Columns.Contains("CategoryGUID") && !dr["CategoryGUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["CategoryGUID"])))
                            {
                                objIQArchive_MediaModel.CategoryGUID = new Guid(Convert.ToString(dr["CategoryGUID"]));
                            }
                        }
                        if (dataSet.Tables[6].Columns.Contains("CategoryName") && !dr["CategoryName"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                        }

                        if (dataSet.Tables[6].Columns.Contains("SubCategory1GUID") && !dr["SubCategory1GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory1GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory1GUID = new Guid(Convert.ToString(dr["SubCategory1GUID"]));
                            }
                        }
                        if (dataSet.Tables[6].Columns.Contains("SubCategory1Name") && !dr["SubCategory1Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory1Name = Convert.ToString(dr["SubCategory1Name"]);
                        }

                        if (dataSet.Tables[6].Columns.Contains("SubCategory2GUID") && !dr["SubCategory2GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory2GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory2GUID = new Guid(Convert.ToString(dr["SubCategory2GUID"]));
                            }
                        }
                        if (dataSet.Tables[6].Columns.Contains("SubCategory2Name") && !dr["SubCategory2Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory2Name = Convert.ToString(dr["SubCategory2Name"]);
                        }

                        if (dataSet.Tables[6].Columns.Contains("SubCategory3GUID") && !dr["SubCategory3GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory3GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory3GUID = new Guid(Convert.ToString(dr["SubCategory3GUID"]));
                            }
                        }
                        if (dataSet.Tables[6].Columns.Contains("SubCategory3Name") && !dr["SubCategory3Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory3Name = Convert.ToString(dr["SubCategory3Name"]);
                        }

                        if (dataSet.Tables[6].Columns.Contains("CreatedDate") && !dr["CreatedDate"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        }

                        if (dataSet.Tables[6].Columns.Contains("IsPublished") && !dr["IsPublished"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.IsPublished = Convert.ToBoolean(dr["IsPublished"]);
                        }

                        if (dataSet.Tables[6].Columns.Contains("Position") && !dr["Position"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Position = Convert.ToInt32(dr["Position"]);
                        }

                        if (dataSet.Tables[6].Columns.Contains("GroupTier1Value") && !dr["GroupTier1Value"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.GroupTier1Value = Convert.ToString(dr["GroupTier1Value"]);
                        }

                        if (dataSet.Tables[6].Columns.Contains("GroupTier2Value") && !dr["GroupTier2Value"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.GroupTier2Value = Convert.ToString(dr["GroupTier2Value"]);
                        }

                        if (dataSet.Tables[6].Columns.Contains("SubMediaTypeDesc") && !dr["SubMediaTypeDesc"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubMediaTypeDesc = Convert.ToString(dr["SubMediaTypeDesc"]);
                        }

                        objIQArchive_MediaModel.MediaData = objIQArchive_ArchiveMiscModel;
                        listIQArchive_Media.Add(objIQArchive_MediaModel);
                    }
                }

                // Represents dbo.ArchivePQ table

                if (dataSet.Tables[7] != null)
                {
                    DataTable dt = dataSet.Tables[7];
                    foreach (DataRow dr in dt.Rows)
                    {
                        IQArchive_MediaModel objIQArchive_MediaModel = new IQArchive_MediaModel();
                        IQArchive_ArchivePQModel objIQArchive_ArchivePQModel = new IQArchive_ArchivePQModel();

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.ID = Convert.ToInt64(dr["ID"]);
                        }
                        if (!dr["_ArchiveMediaID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.ArchiveMediaID = Convert.ToInt64(dr["_ArchiveMediaID"]);
                        }
                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Title = Convert.ToString(dr["Title"]);
                        }
                        if (!dr["Content"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Content = Convert.ToString(dr["Content"]);
                        }
                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.MediaDate = Convert.ToDateTime(dr["MediaDate"]);
                        }
                        if (!dr["MediaType"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.MediaType = Convert.ToString(dr["MediaType"]);
                        }
                        if (!dr["SubMediaType"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubMediaType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), Convert.ToString(dr["SubMediaType"]));
                        }
                        if (!dr["Description"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Description = Convert.ToString(dr["Description"]);
                        }
                        if (!dr["DisplayDescription"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.DisplayDescription = Convert.ToBoolean(dr["DisplayDescription"]);
                        }
                        if (!dr["HighlightingText"].Equals(DBNull.Value) && !string.IsNullOrWhiteSpace(Convert.ToString(dr["HighlightingText"])))
                        {
                            HighlightedPQOutput highlightedPQOutput = new HighlightedPQOutput();
                            objIQArchive_ArchivePQModel.HighlightedOutput = (HighlightedPQOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedPQOutput);
                        }

                        if (dt.Columns.Contains("CategoryGUID") && !dr["CategoryGUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["CategoryGUID"])))
                            {
                                objIQArchive_MediaModel.CategoryGUID = new Guid(Convert.ToString(dr["CategoryGUID"]));
                            }
                        }
                        if (dt.Columns.Contains("CategoryName") && !dr["CategoryName"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                        }

                        if (dt.Columns.Contains("SubCategory1GUID") && !dr["SubCategory1GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory1GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory1GUID = new Guid(Convert.ToString(dr["SubCategory1GUID"]));
                            }
                        }
                        if (dt.Columns.Contains("SubCategory1Name") && !dr["SubCategory1Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory1Name = Convert.ToString(dr["SubCategory1Name"]);
                        }

                        if (dt.Columns.Contains("SubCategory2GUID") && !dr["SubCategory2GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory2GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory2GUID = new Guid(Convert.ToString(dr["SubCategory2GUID"]));
                            }
                        }
                        if (dt.Columns.Contains("SubCategory2Name") && !dr["SubCategory2Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory2Name = Convert.ToString(dr["SubCategory2Name"]);
                        }

                        if (dt.Columns.Contains("SubCategory3GUID") && !dr["SubCategory3GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory3GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory3GUID = new Guid(Convert.ToString(dr["SubCategory3GUID"]));
                            }
                        }
                        if (dt.Columns.Contains("SubCategory3Name") && !dr["SubCategory3Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory3Name = Convert.ToString(dr["SubCategory3Name"]);
                        }

                        if (dt.Columns.Contains("CreatedDate") && !dr["CreatedDate"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        }

                        if (dt.Columns.Contains("Publication") && !dr["Publication"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchivePQModel.Publication = Convert.ToString(dr["Publication"]);
                        }

                        if (dt.Columns.Contains("Author") && !dr["Author"].Equals(DBNull.Value))
                        {
                            if (!String.IsNullOrWhiteSpace(Convert.ToString(dr["Author"])))
                            {
                                XDocument xDoc = XDocument.Parse(Convert.ToString(dr["Author"]));
                                objIQArchive_ArchivePQModel.Authors = xDoc.Descendants("author").Select(x => x.Value).ToList();
                            }
                        }

                        if (dt.Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchivePQModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }

                        if (dt.Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchivePQModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }

                        if (dt.Columns.Contains("IsPublished") && !dr["IsPublished"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.IsPublished = Convert.ToBoolean(dr["IsPublished"]);
                        }

                        if (dt.Columns.Contains("AgentID") && !dr["AgentID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.AgentID = Convert.ToInt64(dr["AgentID"]);
                        }

                        if (dt.Columns.Contains("AgentName") && !dr["AgentName"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.AgentName = Convert.ToString(dr["AgentName"]);
                        }

                        if (dt.Columns.Contains("Position") && !dr["Position"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Position = Convert.ToInt32(dr["Position"]);
                        }

                        if (dt.Columns.Contains("GroupTier1Value") && !dr["GroupTier1Value"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.GroupTier1Value = Convert.ToString(dr["GroupTier1Value"]);
                        }

                        if (dt.Columns.Contains("GroupTier2Value") && !dr["GroupTier2Value"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.GroupTier2Value = Convert.ToString(dr["GroupTier2Value"]);
                        }

                        if (dt.Columns.Contains("SubMediaTypeDesc") && !dr["SubMediaTypeDesc"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubMediaTypeDesc = Convert.ToString(dr["SubMediaTypeDesc"]);
                        }

                        objIQArchive_MediaModel.MediaData = objIQArchive_ArchivePQModel;
                        listIQArchive_Media.Add(objIQArchive_MediaModel);
                    }
                }

                // Represents dbo.ArchiveRadio

                if (dataSet.Tables[8] != null)
                {
                    DataTable dt = dataSet.Tables[8];
                    foreach (DataRow dr in dt.Rows)
                    {
                        IQArchive_MediaModel objIQArchive_MediaModel = new IQArchive_MediaModel();
                        IQArchive_ArchiveRadioModel objIQArchive_ArchiveRadioModel = new IQArchive_ArchiveRadioModel();

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.ID = Convert.ToInt64(dr["ID"]);
                        }
                        if (!dr["_ArchiveMediaID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.ArchiveMediaID = Convert.ToInt64(dr["_ArchiveMediaID"]);
                        }
                        if (!dr["Title"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Title = Convert.ToString(dr["Title"]);
                        }
                        if (!dr["Content"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Content = Convert.ToString(dr["Content"]);
                        }
                        if (!dr["MediaDate"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.MediaDate = Convert.ToDateTime(dr["MediaDate"]);
                        }
                        if (!dr["MediaType"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.MediaType = Convert.ToString(dr["MediaType"]);
                        }
                        if (!dr["SubMediaType"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubMediaType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), Convert.ToString(dr["SubMediaType"]));
                        }
                        if (!dr["Description"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Description = Convert.ToString(dr["Description"]);
                        }
                        if (!dr["DisplayDescription"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.DisplayDescription = Convert.ToBoolean(dr["DisplayDescription"]);
                        }
                        if (!dr["HighlightingText"].Equals(DBNull.Value) && !string.IsNullOrWhiteSpace(Convert.ToString(dr["HighlightingText"])))
                        {
                            HighlightedCCOutput highlightedOutput = new HighlightedCCOutput();
                            objIQArchive_ArchiveRadioModel.HighlightedOutput = (HighlightedCCOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedOutput);
                        }
                        if (!dr["ClipGuid"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveRadioModel.ClipGuid = Convert.ToString(dr["ClipGuid"]);
                        }
                        if (!dr["LocalDatetime"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveRadioModel.LocalDateTime = Convert.ToDateTime(dr["LocalDatetime"]);
                        }
                        if (!dr["Dma_Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveRadioModel.Market = Convert.ToString(dr["Dma_Name"]);
                        }
                        if (!dr["IQ_Station_ID"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveRadioModel.StationID = Convert.ToString(dr["IQ_Station_ID"]);
                            objIQArchive_ArchiveRadioModel.StationLogo = "https://" + currentUrl + "/StationLogoImages/" + Convert.ToString(dr["IQ_Station_ID"]) + ".jpg";
                        }
                        if (!dr["TimeZone"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveRadioModel.TimeZone = Convert.ToString(dr["TimeZone"]);
                        }
                        if (!dr["Dma_Num"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveRadioModel.DMARank = Convert.ToInt16(dr["Dma_Num"]);
                        }

                        if (dt.Columns.Contains("CategoryGUID") && !dr["CategoryGUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["CategoryGUID"])))
                            {
                                objIQArchive_MediaModel.CategoryGUID = new Guid(Convert.ToString(dr["CategoryGUID"]));
                            }
                        }
                        if (dt.Columns.Contains("CategoryName") && !dr["CategoryName"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                        }

                        if (dt.Columns.Contains("SubCategory1GUID") && !dr["SubCategory1GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory1GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory1GUID = new Guid(Convert.ToString(dr["SubCategory1GUID"]));
                            }
                        }
                        if (dt.Columns.Contains("SubCategory1Name") && !dr["SubCategory1Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory1Name = Convert.ToString(dr["SubCategory1Name"]);
                        }

                        if (dt.Columns.Contains("SubCategory2GUID") && !dr["SubCategory2GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory2GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory2GUID = new Guid(Convert.ToString(dr["SubCategory2GUID"]));
                            }
                        }
                        if (dt.Columns.Contains("SubCategory2Name") && !dr["SubCategory2Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory2Name = Convert.ToString(dr["SubCategory2Name"]);
                        }

                        if (dt.Columns.Contains("SubCategory3GUID") && !dr["SubCategory3GUID"].Equals(DBNull.Value))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["SubCategory3GUID"])))
                            {
                                objIQArchive_MediaModel.SubCategory3GUID = new Guid(Convert.ToString(dr["SubCategory3GUID"]));
                            }
                        }
                        if (dt.Columns.Contains("SubCategory3Name") && !dr["SubCategory3Name"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubCategory3Name = Convert.ToString(dr["SubCategory3Name"]);
                        }

                        if (dt.Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveRadioModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                        }

                        if (dt.Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            objIQArchive_ArchiveRadioModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                        }

                        if (dt.Columns.Contains("CreatedDate") && !dr["CreatedDate"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        }

                        if (dt.Columns.Contains("IsPublished") && !dr["IsPublished"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.IsPublished = Convert.ToBoolean(dr["IsPublished"]);
                        }

                        if (dt.Columns.Contains("AgentID") && !dr["AgentID"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.AgentID = Convert.ToInt64(dr["AgentID"]);
                        }

                        if (dt.Columns.Contains("AgentName") && !dr["AgentName"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.AgentName = Convert.ToString(dr["AgentName"]);
                        }

                        if (dt.Columns.Contains("Position") && !dr["Position"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.Position = Convert.ToInt32(dr["Position"]);
                        }

                        if (dt.Columns.Contains("GroupTier1Value") && !dr["GroupTier1Value"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.GroupTier1Value = Convert.ToString(dr["GroupTier1Value"]);
                        }

                        if (dt.Columns.Contains("GroupTier2Value") && !dr["GroupTier2Value"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.GroupTier2Value = Convert.ToString(dr["GroupTier2Value"]);
                        }

                        if (dt.Columns.Contains("SubMediaTypeDesc") && !dr["SubMediaTypeDesc"].Equals(DBNull.Value))
                        {
                            objIQArchive_MediaModel.SubMediaTypeDesc = Convert.ToString(dr["SubMediaTypeDesc"]);
                        }

                        objIQArchive_MediaModel.MediaData = objIQArchive_ArchiveRadioModel;
                        listIQArchive_Media.Add(objIQArchive_MediaModel);
                    }
                }

                IQArchive_FilterModel filter = null;

                if (IsEnableFilter)
                {
                    /* dataSet.Tables[11] represents MediaDates table
                     * dataSet.Tables[12] represents SubMediaType table
                     * dataSet.Tables[13] represents Customer table 
                     * dataSet.Tables[14] represents CustomCategory table */
                    filter = FillFilterFromDataSet(dataSet.Tables[11], dataSet.Tables[12], dataSet.Tables[13], dataSet.Tables[14]);
                }

                if (!string.IsNullOrEmpty(SortColumn))
                {
                    switch (SortColumn)
                    {
                        case "MediaDate":
                            if (IsAsc)
                            {
                                listIQArchive_Media = listIQArchive_Media.OrderBy(a => a.MediaDate).ToList();
                            }
                            else
                            {
                                listIQArchive_Media = listIQArchive_Media.OrderByDescending(a => a.MediaDate).ToList();
                            }
                            break;
                        case "CreatedDate":
                            if (IsAsc)
                            {
                                listIQArchive_Media = listIQArchive_Media.OrderBy(a => a.CreatedDate).ToList();
                            }
                            else
                            {
                                listIQArchive_Media = listIQArchive_Media.OrderByDescending(a => a.CreatedDate).ToList();
                            }
                            break;
                        case "Audience":
                            if (IsAsc)
                            {
                                listIQArchive_Media = listIQArchive_Media.OrderBy(a => a.Audience).ThenBy(a => a.SecondarySortField).ThenByDescending(a => a.MediaDate).ToList();
                            }
                            else
                            {
                                listIQArchive_Media = listIQArchive_Media.OrderByDescending(a => a.Audience).ThenBy(a => a.SecondarySortField).ThenByDescending(a => a.MediaDate).ToList();
                            }
                            break;
                        default:
                            listIQArchive_Media = listIQArchive_Media.OrderByDescending(a => a.MediaDate).ToList();
                            break;
                    }
                }

                dictionary.Add("Filter", filter);
                dictionary.Add("Result", listIQArchive_Media);

            }
            return dictionary;
        }

        private IQArchive_ArchiveClipModel FillClipFromDataSet(DataSet dataset)
        {
            IQArchive_ArchiveClipModel objIQArchive_ArchiveClipModel = new IQArchive_ArchiveClipModel();
            if (dataset != null && dataset.Tables.Count > 0)
            {
                foreach (DataRow dr in dataset.Tables[0].Rows)
                {
                    if (!dr["ClipID"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.ClipID = Convert.ToString(dr["ClipID"]);
                    }

                    if (!dr["ClipTitle"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.ClipTitle = Convert.ToString(dr["ClipTitle"]);
                    }

                    if (!dr["ClosedCaption"].Equals(DBNull.Value))
                    {
                        XmlDocument CC_Node = new XmlDocument();
                        CC_Node.LoadXml(Convert.ToString(dr["ClosedCaption"]));
                        StringBuilder strngCaption = new StringBuilder();
                        XmlNodeList lstTitle = CC_Node.GetElementsByTagName("ttm:title");

                        if (lstTitle != null && lstTitle.Count > 0)
                        {
                            strngCaption.Append("<div class=\"hit\">"
                                                            + "<div class=\"caption\">" + lstTitle[0].InnerText + "</div>"
                                                        + "</div>");
                        }
                        foreach (XmlNode ele in CC_Node.GetElementsByTagName("p"))
                        {

                            int seekPoint = 0;

                            if (ele.Attributes["begin"] != null)
                            {
                                try
                                {
                                    seekPoint = Convert.ToInt32(ele.Attributes["begin"].Value.ToLower().Replace("s", "").Trim());
                                }
                                catch (FormatException)
                                {                                    
                                    seekPoint = Convert.ToInt32(Convert.ToDouble(ele.Attributes["begin"].Value.ToLower().Replace("s", "").Trim()));
                                }
                            }

                            if (ele.InnerText.Trim().ToUpper() != "NULL")
                            {
                                strngCaption.Append("<div class=\"hit\" onclick=\"SeekPoint(" + seekPoint + ");\" style=\"cursor: pointer;\">"
                                                                                + "<div class=\"caption\">" + ele.InnerText + "</div>"
                                                                            + "</div>"); 
                            }
                        }
                        objIQArchive_ArchiveClipModel.ClosedCaption = strngCaption.ToString();
                    }
                }
            }
            return objIQArchive_ArchiveClipModel;
        }

        private string GetClosedCaptionText(string string_XML)
        {
            if (!string.IsNullOrEmpty(string_XML))
            {
                XDocument xdoc = XDocument.Parse(string_XML);
                xdoc = RemoveNamespace(xdoc);
                string hilightedText = string.Join(" ", xdoc.Descendants("p").Where(e=>e.Value.Trim().ToUpper()!="NULL").Select(e => e.Value));
                return hilightedText.Trim();
            }
            else
            {
                return string.Empty;
            }
        }

        private XDocument RemoveNamespace(XDocument xdoc)
        {
            foreach (XElement e in xdoc.Root.DescendantsAndSelf())
            {
                if (e.Name.Namespace != XNamespace.None)
                {
                    e.Name = XNamespace.None.GetName(e.Name.LocalName);
                }

                if (e.Attributes().Where(a => a.IsNamespaceDeclaration || a.Name.Namespace != XNamespace.None).Any())
                {
                    e.ReplaceAttributes(e.Attributes().Select(a => a.IsNamespaceDeclaration ? null : a.Name.Namespace != XNamespace.None ? new XAttribute(XNamespace.None.GetName(a.Name.LocalName), a.Value) : a));
                }
            }
            return xdoc;
        }

        public List<IFrameMicrositeModel> GetArchiveClipByParams(Guid p_ClientGUID, string p_ListCategoryGUID, string p_ListSubCategory1GUID, string p_ListSubCategory2GUID, string p_ListSubCategory3GUID, string p_ListCustomerGUID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, string p_SearchText, string p_ClipTitle, out Guid? p_ClipID, out Int64 p_TotalRecordsCount, string currentURLHost)
        {
            try
            {
                p_TotalRecordsCount = 0;
                p_ClipID = null;

                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageNumber", DbType.Int32, p_PageNumber, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageSize", DbType.Int32, p_PageSize, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SortField", DbType.String, p_SortField, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsAscending", DbType.Boolean, p_IsAscending, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Category", DbType.String, p_ListCategoryGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory1", DbType.String, p_ListSubCategory1GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory2", DbType.String, p_ListSubCategory2GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory3", DbType.String, p_ListSubCategory3GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerGUID", DbType.String, p_ListCustomerGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchTerm", DbType.String, p_SearchText, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClipTitle", DbType.String, p_ClipTitle, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClipID", DbType.Guid, p_ClipID, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@TotalRecordsClipCount", DbType.Int32, p_TotalRecordsCount, ParameterDirection.Output));

                Dictionary<string, string> _OutputParams = null;

                _DataSet = DataAccess.GetDataSetWithOutParam("usp_ArchiveClip_SelectByParams", _ListOfDataType, out _OutputParams);

                if (_OutputParams != null && _OutputParams.Count > 0)
                {
                    if (string.IsNullOrEmpty(_OutputParams["@ClipID"]))
                    {
                        p_ClipID = null;
                    }
                    else
                    {
                        p_ClipID = new Guid(_OutputParams["@ClipID"]);
                    }
                    p_TotalRecordsCount = Convert.ToInt32(_OutputParams["@TotalRecordsClipCount"]);
                }

                return FillIFrameMicrositeResults(_DataSet, currentURLHost);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<IFrameMicrositeModel> FillIFrameMicrositeResults(DataSet p_DataSet, string currentURLHost)
        {
            List<IFrameMicrositeModel> lstIFrameMicrositeModel = new List<IFrameMicrositeModel>();

            foreach (DataRow dr in p_DataSet.Tables[0].Rows)
            {
                IFrameMicrositeModel iFrameMicrositeModel = new IFrameMicrositeModel();

                if (!dr["ClipDate"].Equals(DBNull.Value))
                {
                    iFrameMicrositeModel.ClipDate = Convert.ToDateTime(dr["ClipDate"]);
                }

                if (!dr["ClipID"].Equals(DBNull.Value))
                {
                    iFrameMicrositeModel.ClipID = new Guid(dr["ClipID"].ToString());
                }

                if (!dr["ClipTitle"].Equals(DBNull.Value))
                {
                    iFrameMicrositeModel.ClipTitle = Convert.ToString(dr["ClipTitle"]);
                }

                if (!dr["Keywords"].Equals(DBNull.Value))
                {
                    iFrameMicrositeModel.Keywords = Convert.ToString(dr["Keywords"]);
                }

                if (!dr["Description"].Equals(DBNull.Value))
                {
                    iFrameMicrositeModel.Description = Convert.ToString(dr["Description"]);
                }

                if (!dr["ClipCreationDate"].Equals(DBNull.Value))
                {
                    iFrameMicrositeModel.ClipCreationDate = Convert.ToDateTime(dr["ClipCreationDate"]);
                }

                if (!dr["ClipCreationDate"].Equals(DBNull.Value))
                {
                    iFrameMicrositeModel.ClipCreationDate = Convert.ToDateTime(dr["ClipCreationDate"]);
                }

                if (p_DataSet.Tables[0].Columns.Contains("ThumbnailImagePath"))
                {
                    if (!dr["ThumbnailImagePath"].Equals(DBNull.Value))
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(dr["ThumbnailImagePath"])))
                        {
                            iFrameMicrositeModel.ThumbnailImagePath = "http://" + currentURLHost + "/ThumbnailImage/noimage.jpg";
                            //iFrameMicrositeModel.ThumbnailImagePath = "http://localhost:65409/ThumbnailImage/noimage.jpg";

                        }
                        else
                        {
                            iFrameMicrositeModel.ThumbnailImagePath = Convert.ToString(dr["ThumbnailImagePath"]);
                        }
                    }
                    else
                    {
                        iFrameMicrositeModel.ThumbnailImagePath = "http://" + currentURLHost + "/ThumbnailImage/noimage.jpg";
                        //iFrameMicrositeModel.ThumbnailImagePath = "http://localhost:65409/ThumbnailImage/noimage.jpg";
                    }
                }

                lstIFrameMicrositeModel.Add(iFrameMicrositeModel);
            }

            return lstIFrameMicrositeModel;


        }

        public Dictionary<string, string> GetClipPathByClipGUID(string p_ClipGUID, Guid p_ClientGuid)
        {
            try
            {
                Dictionary<string, string> p_OutputPath = new Dictionary<string, string>();
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClipGUID", DbType.String, p_ClipGUID, ParameterDirection.Input));


                DataSet result = DataAccess.GetDataSet("usp_IQCore_ClipMeta_SelectFilePathByClipGUID", dataTypeList);

                if (result != null && result.Tables.Count > 0 && result.Tables[0] != null && result.Tables[0].Rows.Count > 0)
                {
                    p_OutputPath.Add("FilePath", Convert.ToString(result.Tables[0].Rows[0]["FilePath"]));
                    p_OutputPath.Add("FTPFileLocation", Convert.ToString(result.Tables[0].Rows[0]["FTPFileLocation"]));
                }
                return p_OutputPath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public string UpdateDownloadCountByClipGUID(string p_ClipGUID)
        {
            try
            {
                Dictionary<string, string> p_OutputPath = new Dictionary<string, string>();
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClipGUID", DbType.String, p_ClipGUID, ParameterDirection.Input));

                string result = DataAccess.ExecuteNonQuery("usp_v4_IQCore_ClipMeta_UpdateNoOfTimesDownloadByClipGUID", dataTypeList);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GetArchiveTVeyesLocationByArchiveTVEyesKey(Int64 p_ID, out string p_TranscriptFileLocation, out string p_AudioFileLocation)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));

                p_TranscriptFileLocation = string.Empty;
                p_AudioFileLocation = string.Empty;

                DataSet result = DataAccess.GetDataSet("usp_v4_ArchiveTVEyes_SelectLocationByArchiveTVEyesKey", dataTypeList);

                if (result != null && result.Tables.Count > 0 && result.Tables[0] != null && result.Tables[0].Rows.Count > 0)
                {
                    p_TranscriptFileLocation = Convert.ToString(result.Tables[0].Rows[0]["TranscriptFileLocation"]);
                    p_AudioFileLocation = Convert.ToString(result.Tables[0].Rows[0]["AudioFileLocation"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<IQArchive_Filter> GetCategoryFilter(string ClientGUID, string CustomerGUID, DateTime? FromDate, DateTime? ToDate, string SubMediaType, string SearchTerm, string CategoryGUID, long SinceID)
        {


            try
            {

                CustomerGUID = !string.IsNullOrWhiteSpace(CustomerGUID) ? CustomerGUID : null;
                SubMediaType = !string.IsNullOrWhiteSpace(SubMediaType) ? SubMediaType : null;
                SearchTerm = !string.IsNullOrWhiteSpace(SearchTerm) ? SearchTerm : null;
                CategoryGUID = !string.IsNullOrWhiteSpace(CategoryGUID) ? CategoryGUID : null;

                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGUID", DbType.String, ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.String, CustomerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.DateTime, FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.DateTime, ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubMediaType", DbType.String, SubMediaType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchTerm", DbType.String, SearchTerm, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryGUID", DbType.Xml, CategoryGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SinceID", DbType.Int64, SinceID, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v5_IQArchive_Media_SelectCategoryFilter", dataTypeList);
                IQArchive_FilterModel filter = FillFilterFromDataSet(null, null, null, dataset.Tables[0]);

                return filter.Categories;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<IQArchive_MediaModel> FillTVParentChild(DataTable tvDataTable, DataTable tvChildDataTable, string currentUrl)
        {
            List<IQArchive_MediaModel> listIQArchive_Media = new List<IQArchive_MediaModel>();

            #region TV Fill
            List<IQArchive_MediaModel> listIQArchiveChild_Media = new List<IQArchive_MediaModel>();
            #region TV Child Fill
            if (tvChildDataTable != null)
            {
                foreach (DataRow drChild in tvChildDataTable.Rows)
                {
                    IQArchive_ArchiveClipModel objIQArchive_ArchiveClipModel = new IQArchive_ArchiveClipModel();
                    IQArchive_MediaModel objIQArchiveChild_MediaModel = new IQArchive_MediaModel();
                    if (tvChildDataTable.Columns.Contains("_ParentID") && !drChild["_ParentID"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel._ParentID = Convert.ToInt64(drChild["_ParentID"]);
                    }

                    if (!drChild["ID"].Equals(DBNull.Value))
                    {
                        objIQArchiveChild_MediaModel.ID = Convert.ToInt64(drChild["ID"]);
                    }

                    if (!drChild["_ArchiveMediaID"].Equals(DBNull.Value))
                    {
                        objIQArchiveChild_MediaModel.ArchiveMediaID = Convert.ToInt64(drChild["_ArchiveMediaID"]);
                    }
                    if (!drChild["Title"].Equals(DBNull.Value))
                    {
                        objIQArchiveChild_MediaModel.Title = Convert.ToString(drChild["Title"]);
                    }
                    if (!drChild["Content"].Equals(DBNull.Value))
                    {
                        objIQArchiveChild_MediaModel.Content = GetClosedCaptionText(Convert.ToString(drChild["Content"]));
                    }
                    if (!drChild["MediaDate"].Equals(DBNull.Value))
                    {
                        objIQArchiveChild_MediaModel.MediaDate = Convert.ToDateTime(drChild["MediaDate"]);
                    }
                    if (!drChild["SubMediaType"].Equals(DBNull.Value))
                    {
                        objIQArchiveChild_MediaModel.SubMediaType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), Convert.ToString(drChild["SubMediaType"]));
                    }

                    if (!drChild["ClipID"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.ClipID = Convert.ToString(drChild["ClipID"]);
                    }
                    if (!drChild["Nielsen_Audience"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.Nielsen_Audience = Convert.ToInt32(drChild["Nielsen_Audience"]);
                    }
                    if (!drChild["IQAdShareValue"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.IQAdShareValue = Convert.ToDecimal(drChild["IQAdShareValue"]);
                    }
                    if (!drChild["Nielsen_Result"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.Nielsen_Result = Convert.ToString(drChild["Nielsen_Result"]);
                    }
                    if (!drChild["HighlightingText"].Equals(DBNull.Value) && !string.IsNullOrWhiteSpace(Convert.ToString(drChild["HighlightingText"])))
                    {
                        HighlightedCCOutput highlightedCCOutput = new HighlightedCCOutput();
                        objIQArchive_ArchiveClipModel.HighlightedOutput = (HighlightedCCOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(drChild["HighlightingText"]), highlightedCCOutput);
                    }

                    if (tvChildDataTable.Columns.Contains("Market") && !drChild["Market"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.Market = Convert.ToString(drChild["Market"]);
                    }

                    if (tvChildDataTable.Columns.Contains("Station_Call_Sign") && !drChild["Station_Call_Sign"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.Station_Call_Sign = Convert.ToString(drChild["Station_Call_Sign"]);
                    }

                    if (tvChildDataTable.Columns.Contains("PositiveSentiment") && !drChild["PositiveSentiment"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.PositiveSentiment = Convert.ToInt16(drChild["PositiveSentiment"]);
                    }

                    if (tvChildDataTable.Columns.Contains("ClipDate") && !drChild["ClipDate"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.LocalDateTime = Convert.ToDateTime(drChild["ClipDate"]);
                    }

                    if (tvChildDataTable.Columns.Contains("NegativeSentiment") && !drChild["NegativeSentiment"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.NegativeSentiment = Convert.ToInt16(drChild["NegativeSentiment"]);
                    }

                    if (tvChildDataTable.Columns.Contains("StationLogo") && !drChild["StationLogo"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.StationLogo = "https://" + currentUrl + "/StationLogoImages/" + Convert.ToString(drChild["StationLogo"]) + ".jpg";
                        //objIQArchive_ArchiveClipModel.StationLogo = objIQArchive_ArchiveClipModel.StationLogo;
                        //objIQArchive_ArchiveClipModel.StationLogo = Convert.ToString(dr["StationLogo"]);
                    }

                    if (tvChildDataTable.Columns.Contains("TimeZone") && !drChild["TimeZone"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.TimeZone = Convert.ToString(drChild["TimeZone"]);
                    }

                    if (tvChildDataTable.Columns.Contains("Dma_Num") && !drChild["Dma_Num"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.Dma_Num = Convert.ToString(drChild["Dma_Num"]);
                    }

                    if (tvChildDataTable.Columns.Contains("CategoryGUID") && !drChild["CategoryGUID"].Equals(DBNull.Value))
                    {
                        if (!string.IsNullOrWhiteSpace(Convert.ToString(drChild["CategoryGUID"])))
                        {
                            objIQArchiveChild_MediaModel.CategoryGUID = new Guid(Convert.ToString(drChild["CategoryGUID"]));
                        }
                    }
                    if (tvChildDataTable.Columns.Contains("CategoryName") && !drChild["CategoryName"].Equals(DBNull.Value))
                    {
                        objIQArchiveChild_MediaModel.CategoryName = Convert.ToString(drChild["CategoryName"]);
                    }

                    if (tvChildDataTable.Columns.Contains("CreatedDate") && !drChild["CreatedDate"].Equals(DBNull.Value))
                    {
                        objIQArchiveChild_MediaModel.CreatedDate = Convert.ToDateTime(drChild["CreatedDate"]);
                    }

                    objIQArchiveChild_MediaModel.MediaType = "TV";
                    objIQArchiveChild_MediaModel.MediaData = objIQArchive_ArchiveClipModel;
                    listIQArchiveChild_Media.Add(objIQArchiveChild_MediaModel);
                }
            }

            #endregion

            #region TV Parent Fill
            if (tvDataTable != null)
            {
                foreach (DataRow dr in tvDataTable.Rows)
                {
                    IQArchive_MediaModel objIQArchive_MediaModel = new IQArchive_MediaModel();
                    IQArchive_ArchiveClipModel objIQArchive_ArchiveClipModel = new IQArchive_ArchiveClipModel();
                    if (!dr["ID"].Equals(DBNull.Value))
                    {
                        objIQArchive_MediaModel.ID = Convert.ToInt64(dr["ID"]);
                    }
                    if (!dr["_ArchiveMediaID"].Equals(DBNull.Value))
                    {
                        objIQArchive_MediaModel.ArchiveMediaID = Convert.ToInt64(dr["_ArchiveMediaID"]);
                    }
                    if (!dr["Title"].Equals(DBNull.Value))
                    {
                        objIQArchive_MediaModel.Title = Convert.ToString(dr["Title"]);
                    }
                    if (!dr["Content"].Equals(DBNull.Value))
                    {
                        objIQArchive_MediaModel.Content = GetClosedCaptionText(Convert.ToString(dr["Content"]));
                    }
                    if (!dr["MediaDate"].Equals(DBNull.Value))
                    {
                        objIQArchive_MediaModel.MediaDate = Convert.ToDateTime(dr["MediaDate"]);
                    }
                    if (!dr["SubMediaType"].Equals(DBNull.Value))
                    {
                        objIQArchive_MediaModel.SubMediaType = (CommonFunctions.CategoryType)Enum.Parse(typeof(CommonFunctions.CategoryType), Convert.ToString(dr["SubMediaType"]));
                    }
                    if (!dr["Description"].Equals(DBNull.Value))
                    {
                        objIQArchive_MediaModel.Description = Convert.ToString(dr["Description"]);
                    }
                    if (!dr["DisplayDescription"].Equals(DBNull.Value))
                    {
                        objIQArchive_MediaModel.DisplayDescription = Convert.ToBoolean(dr["DisplayDescription"]);
                    }

                    if (!dr["ClipID"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.ClipID = Convert.ToString(dr["ClipID"]);
                    }
                    if (!dr["Nielsen_Audience"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.Nielsen_Audience = Convert.ToInt32(dr["Nielsen_Audience"]);
                    }
                    if (!dr["IQAdShareValue"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.IQAdShareValue = Convert.ToDecimal(dr["IQAdShareValue"]);
                    }
                    if (!dr["Nielsen_Result"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.Nielsen_Result = Convert.ToString(dr["Nielsen_Result"]);
                    }
                    if (!dr["HighlightingText"].Equals(DBNull.Value) && !string.IsNullOrWhiteSpace(Convert.ToString(dr["HighlightingText"])))
                    {
                        HighlightedCCOutput highlightedCCOutput = new HighlightedCCOutput();
                        objIQArchive_ArchiveClipModel.HighlightedOutput = (HighlightedCCOutput)CommonFunctions.DeserialiazeXml(Convert.ToString(dr["HighlightingText"]), highlightedCCOutput);
                    }

                    if (tvDataTable.Columns.Contains("Market") && !dr["Market"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.Market = Convert.ToString(dr["Market"]);
                    }

                    if (tvDataTable.Columns.Contains("Station_Call_Sign") && !dr["Station_Call_Sign"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.Station_Call_Sign = Convert.ToString(dr["Station_Call_Sign"]);
                    }

                    if (tvDataTable.Columns.Contains("PositiveSentiment") && !dr["PositiveSentiment"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.PositiveSentiment = Convert.ToInt16(dr["PositiveSentiment"]);
                    }

                    if (tvDataTable.Columns.Contains("ClipDate") && !dr["ClipDate"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.LocalDateTime = Convert.ToDateTime(dr["ClipDate"]);
                    }

                    if (tvDataTable.Columns.Contains("NegativeSentiment") && !dr["NegativeSentiment"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.NegativeSentiment = Convert.ToInt16(dr["NegativeSentiment"]);
                    }

                    if (tvDataTable.Columns.Contains("StationLogo") && !dr["StationLogo"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.StationLogo = "https://" + currentUrl + "/StationLogoImages/" + Convert.ToString(dr["StationLogo"]) + ".jpg";
                        //objIQArchive_ArchiveClipModel.StationLogo = objIQArchive_ArchiveClipModel.StationLogo;
                        //objIQArchive_ArchiveClipModel.StationLogo = Convert.ToString(dr["StationLogo"]);
                    }

                    if (tvDataTable.Columns.Contains("TimeZone") && !dr["TimeZone"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.TimeZone = Convert.ToString(dr["TimeZone"]);
                    }

                    if (tvDataTable.Columns.Contains("Dma_Num") && !dr["Dma_Num"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.Dma_Num = Convert.ToString(dr["Dma_Num"]);
                    }

                    if (tvDataTable.Columns.Contains("National_Nielsen_Audience") && !dr["National_Nielsen_Audience"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.National_Nielsen_Audience = Convert.ToInt64(dr["National_Nielsen_Audience"]);
                    }

                    if (tvDataTable.Columns.Contains("National_IQAdShareValue") && !dr["National_IQAdShareValue"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.National_IQAdShareValue = Convert.ToDecimal(dr["National_IQAdShareValue"]);
                    }

                    if (tvDataTable.Columns.Contains("National_Nielsen_Result") && !dr["National_Nielsen_Result"].Equals(DBNull.Value))
                    {
                        objIQArchive_ArchiveClipModel.National_Nielsen_Result = Convert.ToString(dr["National_Nielsen_Result"]);
                    }

                    if (tvDataTable.Columns.Contains("CategoryGUID") && !dr["CategoryGUID"].Equals(DBNull.Value))
                    {
                        if (!string.IsNullOrWhiteSpace(Convert.ToString(dr["CategoryGUID"])))
                        {
                            objIQArchive_MediaModel.CategoryGUID = new Guid(Convert.ToString(dr["CategoryGUID"]));
                        }
                    }
                    if (tvDataTable.Columns.Contains("CategoryName") && !dr["CategoryName"].Equals(DBNull.Value))
                    {
                        objIQArchive_MediaModel.CategoryName = Convert.ToString(dr["CategoryName"]);
                    }

                    if (tvDataTable.Columns.Contains("CreatedDate") && !dr["CreatedDate"].Equals(DBNull.Value))
                    {
                        objIQArchive_MediaModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    }

                    var childs = listIQArchiveChild_Media.Where(a => (a.MediaData as IQArchive_ArchiveClipModel)._ParentID == objIQArchive_MediaModel.ID).OrderBy(a => (a.MediaData as IQArchive_ArchiveClipModel).Dma_Num).OrderByDescending(a => (a.MediaData as IQArchive_ArchiveClipModel).LocalDateTime).ToList();
                    if (childs != null && childs.Count > 0)
                    {
                        if (string.Compare((childs[0].MediaData as IQArchive_ArchiveClipModel).Dma_Num, objIQArchive_ArchiveClipModel.Dma_Num) < 0
                                || (string.Compare((childs[0].MediaData as IQArchive_ArchiveClipModel).Dma_Num, objIQArchive_ArchiveClipModel.Dma_Num) == 0 && (childs[0].MediaData as IQArchive_ArchiveClipModel).LocalDateTime > objIQArchive_ArchiveClipModel.LocalDateTime))
                        {
                            var childIQArchive_MediaModel = objIQArchive_MediaModel.ShallowCopy();
                            childIQArchive_MediaModel.MediaData = objIQArchive_ArchiveClipModel.ShallowCopy();

                            objIQArchive_MediaModel = childs[0].ShallowCopy();
                            objIQArchive_ArchiveClipModel = (childs[0].MediaData as IQArchive_ArchiveClipModel).ShallowCopy();
                            childs.RemoveAt(0);
                            childs.Add(childIQArchive_MediaModel);
                            childs = childs.OrderBy(a => (a.MediaData as IQArchive_ArchiveClipModel).Dma_Num).OrderByDescending(a => (a.MediaData as IQArchive_ArchiveClipModel).LocalDateTime).ToList();
                        }
                    }

                    objIQArchive_ArchiveClipModel.ChildResults = childs;

                    objIQArchive_MediaModel.MediaType = "TV";
                    objIQArchive_MediaModel.MediaData = objIQArchive_ArchiveClipModel;
                    listIQArchive_Media.Add(objIQArchive_MediaModel);
                }
            }
            #endregion

            #endregion

            return listIQArchive_Media;
        }

        public void GetArchiveTVeyesLocationByMediaID(Int64 p_ID, out string p_TranscriptFileLocation, out string p_AudioFileLocation)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@MediaID", DbType.Int64, p_ID, ParameterDirection.Input));

                p_TranscriptFileLocation = string.Empty;
                p_AudioFileLocation = string.Empty;

                DataSet result = DataAccess.GetDataSet("usp_v4_IQArchive_Media_SelectTVEyesLocationByMediaID", dataTypeList);

                if (result != null && result.Tables.Count > 0 && result.Tables[0] != null && result.Tables[0].Rows.Count > 0)
                {
                    p_TranscriptFileLocation = Convert.ToString(result.Tables[0].Rows[0]["TranscriptFileLocation"]);
                    p_AudioFileLocation = Convert.ToString(result.Tables[0].Rows[0]["AudioFileLocation"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string MergeReports(string reportTitle, string reportIDs, Int64? reportImage, string clientGuid, Int64 folderID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ReportTitle", DbType.String, reportTitle, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReportIDs", DbType.String, reportIDs, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReportImageID", DbType.Int64, reportImage, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.String, clientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FolderID", DbType.Int64, folderID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReportID", DbType.Int64, string.Empty, ParameterDirection.Output));

                string result = DataAccess.ExecuteNonQuery("usp_v4_IQ_Report_MergeReports", dataTypeList);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetMergedReportItemCount(string reportIDs)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ReportIDs", DbType.String, reportIDs, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ItemCount", DbType.Int64, string.Empty, ParameterDirection.Output));

                string result = DataAccess.ExecuteNonQuery("usp_v4_IQ_Report_GetMergedReportItemCount", dataTypeList);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetClipTitleByClipID(string p_ClipID)
        {
            List<DataType> dataTypeList = new List<DataType>();
            dataTypeList.Add(new DataType("@ClipID", DbType.String, p_ClipID, ParameterDirection.Input));

            var clipTitle = Convert.ToString(DataAccess.ExecuteScalar("usp_v4_ArchiveClip_SelectClipTitleByClipID", dataTypeList));

            return clipTitle;
        }



        #region MCMedia

        public int AddToMCMedia(int masterClientID, string mediaIDs, bool isMediaRoomEditor)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@MasterClientID", DbType.Int32, masterClientID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsMediaRoomEditor", DbType.Boolean, isMediaRoomEditor, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MediaIDs", DbType.Xml, mediaIDs, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReturnValue", DbType.Int32, 0, ParameterDirection.Output));

                string sRetVal = DataAccess.ExecuteNonQuery("usp_v4_IQArchive_MCMedia_Insert", dataTypeList);
                int retVal;

                return Int32.TryParse(sRetVal, out retVal) ? retVal : -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RemoveFromMCMedia(int masterClientID, string mediaIDs)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@MasterClientID", DbType.Int32, masterClientID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MediaIDs", DbType.Xml, mediaIDs, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReturnValue", DbType.Int32, 0, ParameterDirection.Output));

                string sRetVal = DataAccess.ExecuteNonQuery("usp_v4_IQArchive_MCMedia_Delete", dataTypeList);
                int retVal;

                return Int32.TryParse(sRetVal, out retVal) ? retVal : -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<IQArchive_Filter> GetMCMediaCategoryFilter(string ClientGUID, string CustomerGUID, DateTime? FromDate, DateTime? ToDate, string SubMediaType, string SearchTerm, string CategoryGUID, long SinceID, int? MasterClientID, string reportGUID)
        {
            try
            {
                ClientGUID = !string.IsNullOrWhiteSpace(ClientGUID) ? ClientGUID : null;
                CustomerGUID = !string.IsNullOrWhiteSpace(CustomerGUID) ? CustomerGUID : null;
                SubMediaType = !string.IsNullOrWhiteSpace(SubMediaType) ? SubMediaType : null;
                SearchTerm = !string.IsNullOrWhiteSpace(SearchTerm) ? SearchTerm : null;
                CategoryGUID = !string.IsNullOrWhiteSpace(CategoryGUID) ? CategoryGUID : null;

                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGUID", DbType.String, ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.String, CustomerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.DateTime, FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.DateTime, ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubMediaType", DbType.String, SubMediaType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchTerm", DbType.String, SearchTerm, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryGUID", DbType.Xml, CategoryGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SinceID", DbType.Int64, SinceID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MasterClientID", DbType.Int32, MasterClientID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReportGUID", DbType.String, reportGUID, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v5_IQArchive_MCMedia_SelectCategoryFilter", dataTypeList);
                IQArchive_FilterModel filter = FillFilterFromDataSet(null, null, null, dataset.Tables[0]);

                return filter.Categories;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Dictionary<string, object> GetMCMediaResults(string ClientGUID, string CustomerGUID, long FromRecordID, int PageSize, DateTime? FromDate, DateTime? ToDate, string SubMediaType, string SearchTerm,
                                                                string CategoryGUID, string SelectionType, bool IsAsc, string SortColumn, bool IsEnableFilter, string currentUrl, ref long SinceID, out long TotalResults, int? MasterClientID)
        {
            try
            {
                CustomerGUID = !string.IsNullOrWhiteSpace(CustomerGUID) ? CustomerGUID : null;
                SubMediaType = !string.IsNullOrWhiteSpace(SubMediaType) ? SubMediaType : null;
                SearchTerm = !string.IsNullOrWhiteSpace(SearchTerm) ? SearchTerm : null;
                CategoryGUID = !string.IsNullOrWhiteSpace(CategoryGUID) ? CategoryGUID : null;

                TotalResults = 0;

                List<DataType> dataTypeList = new List<DataType>();
                Dictionary<string, string> p_outParameter;
                dataTypeList.Add(new DataType("@ClientGUID", DbType.String, ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.String, CustomerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromRecordID", DbType.Int64, FromRecordID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.DateTime, FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.DateTime, ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubMediaType", DbType.String, SubMediaType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchTerm", DbType.String, SearchTerm, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryGUID", DbType.Xml, CategoryGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsAsc", DbType.Boolean, IsAsc, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SortColumn", DbType.String, SortColumn, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsEnableFilter", DbType.Boolean, IsEnableFilter, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SelectionType", DbType.String, SelectionType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MasterClientID", DbType.Int32, MasterClientID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SinceID", DbType.Int64, SinceID, ParameterDirection.Output));
                dataTypeList.Add(new DataType("@TotalResults", DbType.Int64, TotalResults, ParameterDirection.Output));

                DataSet dataset = DataAccess.GetDataSetWithOutParam("usp_v5_IQArchive_MCMedia_Select", dataTypeList, out p_outParameter);

                Dictionary<string, object> dictresult = FillIQArchieveResults(dataset, currentUrl, IsEnableFilter, SortColumn, IsAsc);

                if (p_outParameter != null && p_outParameter.Count > 0)
                {
                    SinceID = !string.IsNullOrWhiteSpace(p_outParameter["@SinceID"]) ? Convert.ToInt64(p_outParameter["@SinceID"]) : 0;
                    TotalResults = !string.IsNullOrWhiteSpace(p_outParameter["@TotalResults"]) ? Convert.ToInt64(p_outParameter["@TotalResults"]) : 0;
                }

                return dictresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Guid? AddToMCMediaReport(Guid? reportGUID, int masterClientID, int reportTypeID, string mediaIDXml)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ReportGUID", DbType.Guid, reportGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MasterClientID", DbType.Int64, masterClientID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MediaIDXml", DbType.Xml, mediaIDXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReportTypeID", DbType.Int32, reportTypeID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReturnValue", DbType.Guid, null, ParameterDirection.Output));

                string sRetVal = DataAccess.ExecuteNonQuery("usp_v4_IQArchive_MCMedia_AddToReport", dataTypeList);
                Guid retVal;

                return Guid.TryParse(sRetVal, out retVal) ? retVal : (Guid?)null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RemoveFromMCMediaReport(Guid reportGUID, string mediaIDXml)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ReportGUID", DbType.Guid, reportGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MediaIDXml", DbType.Xml, mediaIDXml, ParameterDirection.Input));
                DataAccess.ExecuteNonQuery("usp_v4_IQArchive_MCMedia_RemoveFromReport", dataTypeList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Guid? GetMCMediaReportGUID(int masterClientID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@MasterClientID", DbType.Int64, masterClientID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ReportGUID", DbType.Guid, null, ParameterDirection.Output));

                string sRetVal = DataAccess.ExecuteNonQuery("usp_v4_IQArchive_MCMedia_GetReportGUID", dataTypeList);
                Guid retVal;

                return Guid.TryParse(sRetVal, out retVal) ? retVal : (Guid?)null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
