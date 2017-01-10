using System;
using System.Collections.Generic;
using System.Data;
using IQMedia.Data.Base;
using IQMedia.Model;

namespace IQMedia.Data
{
    public class ImagiQDA : IDataAccess
    {
        public List<ImagiQLogoModel> GetLRResultsByGuid(Guid recordfileGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@RecordfileGuid", DbType.Guid, recordfileGuid, ParameterDirection.Input));
                DataSet dataset = DataAccess.GetDataSet("usp_v4_OptiQ_LRResults_SelectByGuid", dataTypeList);

                List<ImagiQLogoModel> lstLogoModel = new List<ImagiQLogoModel>();
                if (dataset != null && dataset.Tables.Count > 0)
                {
                    if (dataset.Tables[0] != null)
                    {
                        DataTable dataTable = dataset.Tables[0];
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            ImagiQLogoModel logoModel = new ImagiQLogoModel();

                            if (dataTable.Columns.Contains("LogoID") && !dr["LogoID"].Equals(DBNull.Value))
                            {
                                logoModel.ID = Convert.ToInt64(dr["LogoID"]);
                            }
                            if (dataTable.Columns.Contains("CompanyName") && !dr["CompanyName"].Equals(DBNull.Value))
                            {
                                logoModel.CompanyName = Convert.ToString(dr["CompanyName"]);
                            }
                            if (dataTable.Columns.Contains("ThumbnailPath") && !dr["ThumbnailPath"].Equals(DBNull.Value))
                            {
                                logoModel.ThumbnailPath = Convert.ToString(dr["ThumbnailPath"]);
                            }
                            if (dataTable.Columns.Contains("Offset") && !dr["Offset"].Equals(DBNull.Value))
                            {
                                logoModel.Offset = Convert.ToInt32(dr["Offset"]);
                            }
                            if (dataTable.Columns.Contains("HitLogoPath") && !dr["HitLogoPath"].Equals(DBNull.Value))
                            {
                                logoModel.HitLogoPath = Convert.ToString(dr["HitLogoPath"]);
                            }

                            lstLogoModel.Add(logoModel);
                        }
                    }
                }

                return lstLogoModel;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Dictionary<string, object> GetLRResults(Guid clientGuid, DateTime? fromDate, DateTime? toDate, string logoIDList, string dmaList, string stationAffilList, string stationIDList, string classNum, 
                                                            int? regionNum, int? countryNum, bool isAsc, bool isMarketSort, long? fromRecordID, int pageSize, string industryList, string brandList, ref long? sinceID, out int totalResults)
        {
            try
            {
                totalResults = 0;
                List<DataType> dataTypeList = new List<DataType>();
                Dictionary<string, string> outParameter;

                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.Date, fromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.Date, toDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@LogoIDList", DbType.Xml, logoIDList, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@DmaList", DbType.Xml, dmaList, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@StationAffilList", DbType.Xml, stationAffilList, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@StationIDList", DbType.Xml, stationIDList, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClassNum", DbType.String, classNum, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@RegionNum", DbType.String, regionNum, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CountryNum", DbType.String, countryNum, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsAsc", DbType.Boolean, isAsc, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsMarketSort", DbType.Boolean, isMarketSort, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromRecordID", DbType.Int64, fromRecordID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageSize", DbType.Int64, pageSize, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IndustryList", DbType.Xml, industryList, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@BrandList", DbType.Xml, brandList, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SinceID", DbType.Int64, sinceID, ParameterDirection.Output));
                dataTypeList.Add(new DataType("@TotalResults", DbType.Int64, totalResults, ParameterDirection.Output));
                DataSet dataset = DataAccess.GetDataSetWithOutParam("usp_v4_OptiQ_LRResults_Select", dataTypeList, out outParameter);

                if (outParameter != null && outParameter.Count > 0)
                {
                    sinceID = !string.IsNullOrWhiteSpace(outParameter["@SinceID"]) ? Convert.ToInt64(outParameter["@SinceID"]) : 0;
                    totalResults = !string.IsNullOrWhiteSpace(outParameter["@TotalResults"]) ? Convert.ToInt32(outParameter["@TotalResults"]) : 0;
                }

                return FillLRResults(dataset);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private Dictionary<string, object> FillLRResults(DataSet dataSet)
        {
            Dictionary<string, object> dictResults = new Dictionary<string, object>();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                List<IQAgent_TVResultsModel> lstIQAgent_TVResultsModel = new List<IQAgent_TVResultsModel>();
                if (dataSet.Tables[0] != null)
                {
                    DataTable dataTable = dataSet.Tables[0];
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        IQAgent_TVResultsModel tvResult = new IQAgent_TVResultsModel();

                        if (dataTable.Columns.Contains("IQ_CC_Key") && !dr["IQ_CC_Key"].Equals(DBNull.Value))
                        {
                            tvResult.IQ_CC_Key = Convert.ToString(dr["IQ_CC_Key"]);
                        }
                        if (dataTable.Columns.Contains("StationID") && !dr["StationID"].Equals(DBNull.Value))
                        {
                            tvResult.StationID = Convert.ToString(dr["StationID"]);
                        }
                        if (dataTable.Columns.Contains("Title120") && !dr["Title120"].Equals(DBNull.Value))
                        {
                            tvResult.Title120 = Convert.ToString(dr["Title120"]);
                        }
                        if (dataTable.Columns.Contains("Dma_Name") && !dr["Dma_Name"].Equals(DBNull.Value))
                        {
                            tvResult.Market = Convert.ToString(dr["Dma_Name"]);
                        }
                        if (dataTable.Columns.Contains("HitCount") && !dr["HitCount"].Equals(DBNull.Value))
                        {
                            tvResult.Hits = Convert.ToInt32(dr["HitCount"]);
                        }
                        if (dataTable.Columns.Contains("StationDT") && !dr["StationDT"].Equals(DBNull.Value))
                        {
                            tvResult.Date = Convert.ToDateTime(dr["StationDT"]);
                        }
                        if (dataTable.Columns.Contains("RecordfileGUID") && !dr["RecordfileGUID"].Equals(DBNull.Value))
                        {
                            tvResult.RL_VideoGUID = new Guid(dr["RecordfileGUID"].ToString());
                        }
                        if (dataTable.Columns.Contains("TimeZone") && !dr["TimeZone"].Equals(DBNull.Value))
                        {
                            tvResult.TimeZone = Convert.ToString(dr["TimeZone"]);
                        }

                        lstIQAgent_TVResultsModel.Add(tvResult);
                    }
                }
                dictResults.Add("Results", lstIQAgent_TVResultsModel);               
            }

            if (dataSet != null && dataSet.Tables.Count > 1)
            {
                List<ImagiQLogoModel> lstLogo = new List<ImagiQLogoModel>();
                if (dataSet.Tables[1] != null)
                {
                    DataTable dataTable = dataSet.Tables[1];
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        ImagiQLogoModel logo = new ImagiQLogoModel();

                        if (dataTable.Columns.Contains("LogoID") && !dr["LogoID"].Equals(DBNull.Value))
                        {
                            logo.ID = Convert.ToInt64(dr["LogoID"]);
                        }
                        if (dataTable.Columns.Contains("CompanyName") && !dr["CompanyName"].Equals(DBNull.Value))
                        {
                            logo.CompanyName = Convert.ToString(dr["CompanyName"]);
                        }
                        if (dataTable.Columns.Contains("ThumbnailPath") && !dr["ThumbnailPath"].Equals(DBNull.Value))
                        {
                            logo.ThumbnailPath = Convert.ToString(dr["ThumbnailPath"]);
                        }
                        if (dataTable.Columns.Contains("Offset") && !dr["Offset"].Equals(DBNull.Value))
                        {
                            logo.Offset = Convert.ToInt32(dr["Offset"]);
                        }

                        lstLogo.Add(logo);
                    }
                }
                dictResults.Add("Logo", lstLogo);
            }

            if (dataSet != null && dataSet.Tables.Count > 2)
            {
                List<IQ_Dma> lstDma = new List<IQ_Dma>();
                if (dataSet.Tables[2] != null)
                {
                    DataTable dataTable = dataSet.Tables[2];
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        IQ_Dma dma = new IQ_Dma();

                        if (dataTable.Columns.Contains("Dma_Name") && !dr["Dma_Name"].Equals(DBNull.Value))
                        {
                            dma.Name = Convert.ToString(dr["Dma_Name"]);
                        }

                        lstDma.Add(dma);
                    }
                }
                dictResults.Add("IQ_Dma", lstDma);
            }

            if (dataSet != null && dataSet.Tables.Count > 3)
            {
                List<Station_Affil> lstStationAffil = new List<Station_Affil>();
                if (dataSet.Tables[3] != null)
                {
                    DataTable dataTable = dataSet.Tables[3];
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        Station_Affil stationAffil = new Station_Affil();

                        if (dataTable.Columns.Contains("Station_Affil") && !dr["Station_Affil"].Equals(DBNull.Value))
                        {
                            stationAffil.Name = Convert.ToString(dr["Station_Affil"]);
                        }

                        lstStationAffil.Add(stationAffil);
                    }
                }
                dictResults.Add("Station_Affil", lstStationAffil);
            }

            if (dataSet != null && dataSet.Tables.Count > 4)
            {
                List<IQ_Station> lstStation = new List<IQ_Station>();
                if (dataSet.Tables[4] != null)
                {
                    DataTable dataTable = dataSet.Tables[4];
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        IQ_Station station = new IQ_Station();

                        if (dataTable.Columns.Contains("IQ_Station_ID") && !dr["IQ_Station_ID"].Equals(DBNull.Value))
                        {
                            station.IQ_Station_ID = Convert.ToString(dr["IQ_Station_ID"]);
                        }
                        if (dataTable.Columns.Contains("Station_Call_Sign") && !dr["Station_Call_Sign"].Equals(DBNull.Value))
                        {
                            station.Station_Call_Sign = Convert.ToString(dr["Station_Call_Sign"]);
                        }

                        lstStation.Add(station);
                    }
                }
                dictResults.Add("IQ_Station", lstStation);
            }

            if (dataSet != null && dataSet.Tables.Count > 5)
            {
                List<IQ_Region> lstRegion = new List<IQ_Region>();
                if (dataSet.Tables[5] != null)
                {
                    DataTable dataTable = dataSet.Tables[5];
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        IQ_Region region = new IQ_Region();

                        if (dataTable.Columns.Contains("Region") && !dr["Region"].Equals(DBNull.Value))
                        {
                            region.Name = Convert.ToString(dr["Region"]);
                        }
                        if (dataTable.Columns.Contains("Region_Num") && !dr["Region_Num"].Equals(DBNull.Value))
                        {
                            region.Num = Convert.ToInt32(dr["Region_Num"]);
                        }

                        lstRegion.Add(region);
                    }
                }
                dictResults.Add("IQ_Region", lstRegion);
            }

            if (dataSet != null && dataSet.Tables.Count > 6)
            {
                List<IQ_Country> lstCountry = new List<IQ_Country>();
                if (dataSet.Tables[6] != null)
                {
                    DataTable dataTable = dataSet.Tables[6];
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        IQ_Country country = new IQ_Country();

                        if (dataTable.Columns.Contains("Country") && !dr["Country"].Equals(DBNull.Value))
                        {
                            country.Name = Convert.ToString(dr["Country"]);
                        }
                        if (dataTable.Columns.Contains("Country_Num") && !dr["Country_Num"].Equals(DBNull.Value))
                        {
                            country.Num = Convert.ToInt32(dr["Country_Num"]);
                        }

                        lstCountry.Add(country);
                    }
                }
                dictResults.Add("IQ_Country", lstCountry);
            }

            if (dataSet != null && dataSet.Tables.Count > 7)
            {
                List<IQ_Class> lstClass = new List<IQ_Class>();
                if (dataSet.Tables[7] != null)
                {
                    DataTable dataTable = dataSet.Tables[7];
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        IQ_Class iQClass = new IQ_Class();

                        if (dataTable.Columns.Contains("iQClass") && !dr["iQClass"].Equals(DBNull.Value))
                        {
                            iQClass.Name = Convert.ToString(dr["iQClass"]);
                        }

                        lstClass.Add(iQClass);
                    }
                }
                dictResults.Add("IQ_Class", lstClass);
            }

            if (dataSet != null && dataSet.Tables.Count > 8)
            {
                List<string> lstIndustry = new List<string>();
                if (dataSet.Tables[8] != null)
                {
                    DataTable dataTable = dataSet.Tables[8];
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        if (dataTable.Columns.Contains("Industry") && !dr["Industry"].Equals(DBNull.Value))
                        {
                            lstIndustry.Add(Convert.ToString(dr["Industry"]));
                        }
                    }
                }
                dictResults.Add("Industry", lstIndustry);
            }

            if (dataSet != null && dataSet.Tables.Count > 9)
            {
                List<string> lstBrand = new List<string>();
                if (dataSet.Tables[9] != null)
                {
                    DataTable dataTable = dataSet.Tables[9];
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        if (dataTable.Columns.Contains("Brand") && !dr["Brand"].Equals(DBNull.Value))
                        {
                            lstBrand.Add(Convert.ToString(dr["Brand"]));
                        }
                    }
                }
                dictResults.Add("Brand", lstBrand);
            }

            return dictResults;
        }


        public List<long> GetSearchImageIDs(long BrandID)
        {
            try
            {
                List<long> lstSearchImageIDs = new List<long>();

                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@BrandID", DbType.Int64, BrandID, ParameterDirection.Input));
                DataSet dataSet = DataAccess.GetDataSet("usp_v4_OptiQ_GetSearchImageIDs", dataTypeList);

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        lstSearchImageIDs.Add(Convert.ToInt64(dr[0]));
                    }
                }

                return lstSearchImageIDs;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<long> GetSearchImagesById(List<long> LRSearchIDs,  out List<string> lstSearchImages)
        {
            try
            {
                var lstSearchImageIDs = new List<long>();
                lstSearchImages = new List<string>();

                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@LRSearchIDs", DbType.String, String.Join(",", LRSearchIDs), ParameterDirection.Input));
                DataSet dataSet = DataAccess.GetDataSet("usp_v4_OptiQ_GetSearchImagesById", dataTypeList);

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        lstSearchImageIDs.Add(Convert.ToInt64(dr[0]));
                        lstSearchImages.Add(Convert.ToString(dr[1]));
                    }
                }

                return lstSearchImageIDs;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    
    }
}
