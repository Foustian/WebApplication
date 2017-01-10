using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using System.Data;
using IQMedia.Model;

namespace IQMedia.Data
{
    public class IQService_DiscoveryDA : IDataAccess
    {
        public string InsertExportDiscovery(Guid p_CustomerGuid, Boolean p_IsSelectAll, string p_SearchCriteria, string p_ArticleXml)
        {
            try
            {
                string _result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@CustomerGuid", DbType.Guid, p_CustomerGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsSelectAll", DbType.Boolean, p_IsSelectAll, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchCriteria", DbType.String, p_SearchCriteria, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ArticleXml", DbType.String, p_ArticleXml, ParameterDirection.Input));

                _result = Convert.ToString(DataAccess.ExecuteScalar("usp_v4_IQService_DiscoveryExport_Insert", _ListOfDataType));

                return _result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<IQService_DiscoveryExportModel> GetLastDiscoveryExportDetails(Guid p_CustomerGuid)
        {
            try
            {
                
                List<IQService_DiscoveryExportModel> discoveryExportList = new List<IQService_DiscoveryExportModel>();
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@CustomerGuid", DbType.Guid, p_CustomerGuid, ParameterDirection.Input));
                
                DataSet dataSet = DataAccess.GetDataSet("usp_v4_IQService_DiscoveryExport_GetLastExportStatusByCustomerGuid", _ListOfDataType);
                if(dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        IQService_DiscoveryExportModel discoveryExport = new IQService_DiscoveryExportModel();

                        if (!row["Status"].Equals(DBNull.Value))
                        {
                            discoveryExport.Status = Convert.ToString(row["Status"]);
                        }

                        if (!row["DownloadPath"].Equals(DBNull.Value))
                        {
                            discoveryExport.DownloadPath = Convert.ToString(row["DownloadPath"]);
                        }
                        else
                        {
                            discoveryExport.DownloadPath = "--";
                        }

                        if (!row["SearchTerm"].Equals(DBNull.Value))
                        {
                            discoveryExport.SearchTerm = Convert.ToString(row["SearchTerm"]);
                        }

                        if (!row["CreatedDate"].Equals(DBNull.Value))
                        {
                            discoveryExport.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                        }

                        discoveryExportList.Add(discoveryExport);
                    }

                }

                return discoveryExportList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DiscoveryAdvanceSearch_DropDown GetSSPDataWithStationByClientGUID(string ClientGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ClientGuid", DbType.String, ClientGuid, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_Discovery_AdvanceSearch_SelectAllDropdown", dataTypeList);

                DiscoveryAdvanceSearch_DropDown objDiscoveryDropDown = new DiscoveryAdvanceSearch_DropDown();
                objDiscoveryDropDown.TV_DMAList = new List<IQ_Dma>();
                objDiscoveryDropDown.TV_ClassList = new List<IQ_Class>();
                objDiscoveryDropDown.TV_StationList = new List<IQ_Station>();
                objDiscoveryDropDown.TV_AffiliateList = new List<Station_Affil>();
                objDiscoveryDropDown.TV_RegionList = new List<IQ_Region>();
                objDiscoveryDropDown.TV_CountryList = new List<IQ_Country>();

                objDiscoveryDropDown.NM_GenreList = new List<IQAgentDropDown_NM_Genere>();
                objDiscoveryDropDown.NM_CategoryList = new List<IQAgentDropDown_NM_Category>();
                objDiscoveryDropDown.NM_PublicationCategoryList = new List<IQAgentDropDown_NM_PublicationCategory>();
                objDiscoveryDropDown.NM_MarketList = new List<IQAgentDropDown_NM_Market>();
                objDiscoveryDropDown.NM_RegionList = new List<IQAgentDropDown_NM_Region>();
                
                objDiscoveryDropDown.SM_SourceTypeList = new List<IQAgentDropDown_SM_SourceType>();
                
                objDiscoveryDropDown.CountryList = new Dictionary<string, string>();
                objDiscoveryDropDown.LanguageList = new List<string>();

                objDiscoveryDropDown.IsAllDmaAllowed = false;
                objDiscoveryDropDown.IsAllStationAllowed = false;
                objDiscoveryDropDown.IsAllClassAllowed = false;

                if (dataset != null && dataset.Tables.Count > 0)
                {
                    // Table[0] Represents TV_DMA
                    foreach (DataRow dr in dataset.Tables[0].Rows)
                    {
                        IQ_Dma objDMA = new IQ_Dma();
                        if (!dr["Dma_Num"].Equals(DBNull.Value))
                        {
                            objDMA.Num = Convert.ToString(dr["Dma_Num"]);
                        }
                        if (!dr["Dma_Name"].Equals(DBNull.Value))
                        {
                            objDMA.Name = Convert.ToString(dr["Dma_Name"]);
                        }
                        objDiscoveryDropDown.TV_DMAList.Add(objDMA);
                    }
                    if (objDiscoveryDropDown.TV_DMAList != null && objDiscoveryDropDown.TV_DMAList.Count() > 0)
                    {
                        int index = 0;

                        IQ_Dma tmpDma = objDiscoveryDropDown.TV_DMAList.Find(d => string.Compare(d.Name, "National", true) == 0);

                        if (tmpDma != null)
                        {
                            objDiscoveryDropDown.TV_DMAList.Remove(tmpDma);
                            objDiscoveryDropDown.TV_DMAList.Insert(index, tmpDma);

                            index = index + 1;
                        }

                        tmpDma = objDiscoveryDropDown.TV_DMAList.Find(d => string.Compare(d.Name, "International", true) == 0);

                        if (tmpDma != null)
                        {
                            objDiscoveryDropDown.TV_DMAList.Remove(tmpDma);
                            objDiscoveryDropDown.TV_DMAList.Insert(index, tmpDma);

                            index = index + 1;
                        }

                        tmpDma = objDiscoveryDropDown.TV_DMAList.Find(d => string.Compare(d.Name, "Canada", true) == 0);

                        if (tmpDma != null)
                        {
                            objDiscoveryDropDown.TV_DMAList.Remove(tmpDma);
                            objDiscoveryDropDown.TV_DMAList.Insert(index, tmpDma);

                            index = index + 1;
                        }
                    }

                    // Table[1] Represents TV_Station
                    foreach (DataRow dr in dataset.Tables[1].Rows)
                    {
                        IQ_Station objStation = new IQ_Station();
                        if (!dr["IQ_Station_ID"].Equals(DBNull.Value))
                        {
                            objStation.IQ_Station_ID = Convert.ToString(dr["IQ_Station_ID"]);
                        }

                        if (!dr["Station_Call_Sign"].Equals(DBNull.Value))
                        {
                            objStation.Station_Call_Sign = Convert.ToString(dr["Station_Call_Sign"]);
                        }


                        if (!objDiscoveryDropDown.TV_AffiliateList.Select(a => a.Name).Contains(dr["Station_Affil"]))
                        {
                            Station_Affil objAffiliate = new Station_Affil();
                            objAffiliate.Name = Convert.ToString(dr["Station_Affil"]);

                            objDiscoveryDropDown.TV_AffiliateList.Add(objAffiliate);
                        }

                        objDiscoveryDropDown.TV_StationList.Add(objStation);
                    }

                    // Table[2] Represents NM_Region
                    foreach (DataRow dr in dataset.Tables[2].Rows)
                    {
                        IQAgentDropDown_NM_Region objRegion = new IQAgentDropDown_NM_Region();
                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objRegion.ID = Convert.ToInt32(dr["ID"]);
                        }
                        if (!dr["Label"].Equals(DBNull.Value))
                        {
                            objRegion.Label = Convert.ToString(dr["Label"]);
                        }
                        objDiscoveryDropDown.NM_RegionList.Add(objRegion);
                    }

                    // Table[3] Represents SM_SourceType
                    foreach (DataRow dr in dataset.Tables[3].Rows)
                    {
                        IQAgentDropDown_SM_SourceType objSourceType = new IQAgentDropDown_SM_SourceType();
                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objSourceType.ID = Convert.ToInt32(dr["ID"]);
                        }
                        if (!dr["Label"].Equals(DBNull.Value))
                        {
                            objSourceType.Label = Convert.ToString(dr["Label"]);
                        }
                        objDiscoveryDropDown.SM_SourceTypeList.Add(objSourceType);
                    }

                    // Table[4] Represents MO Country                    
                    foreach (DataRow dr in dataset.Tables[4].Rows)
                    {
                        objDiscoveryDropDown.CountryList.Add(Convert.ToString(dr["Name"]), Convert.ToString(dr["Code"]));
                    }

                    // Table[5] Represents MO Language                    
                    foreach (DataRow dr in dataset.Tables[5].Rows)
                    {
                        objDiscoveryDropDown.LanguageList.Add(Convert.ToString(dr["Name"]));
                    }

                    // Table[6] Represents TV Region
                    foreach (DataRow dr in dataset.Tables[6].Rows)
                    {
                        IQ_Region objRegion = new IQ_Region();
                        if (!dr["Region_Num"].Equals(DBNull.Value))
                        {
                            objRegion.Num = Convert.ToInt32(dr["Region_Num"]);
                        }
                        if (!dr["Region"].Equals(DBNull.Value))
                        {
                            objRegion.Name = Convert.ToString(dr["Region"]);
                        }
                        objDiscoveryDropDown.TV_RegionList.Add(objRegion);
                    }

                    // Table[7] Represents TV Country
                    foreach (DataRow dr in dataset.Tables[7].Rows)
                    {
                        IQ_Country objCountry = new IQ_Country();
                        if (!dr["Country_Num"].Equals(DBNull.Value))
                        {
                            objCountry.Num = Convert.ToInt32(dr["Country_Num"]);
                        }
                        if (!dr["Country"].Equals(DBNull.Value))
                        {
                            objCountry.Name = Convert.ToString(dr["Country"]);
                        }
                        objDiscoveryDropDown.TV_CountryList.Add(objCountry);
                    }

                    // Table[8] Represents TV_Class
                    List<IQ_Class> listOfIQClass = new List<IQ_Class>();
                    if (dataset != null && dataset.Tables.Count > 8)
                    {
                        foreach (DataRow dr in dataset.Tables[8].Rows)
                        {

                            IQ_Class objClass = new IQ_Class();
                            if (!dr["IQ_Class_Num"].Equals(DBNull.Value))
                            {
                                objClass.Num = Convert.ToString(dr["IQ_Class_Num"]);
                            }
                            if (!dr["IQ_Class"].Equals(DBNull.Value))
                            {
                                objClass.Name = Convert.ToString(dr["IQ_Class"]);
                            }
                            objDiscoveryDropDown.TV_ClassList.Add(objClass);
                        }
                    }

                    //Table[9] Represents NM_Genere
                    if (dataset != null && dataset.Tables.Count > 9)
                    {
                        foreach (DataRow dr in dataset.Tables[9].Rows)
                        {
                            IQAgentDropDown_NM_Genere objGenere = new IQAgentDropDown_NM_Genere();
                            if (!dr["ID"].Equals(DBNull.Value))
                            {
                                objGenere.ID = Convert.ToInt32(dr["ID"]);
                            }
                            if (!dr["Label"].Equals(DBNull.Value))
                            {
                                objGenere.Label = Convert.ToString(dr["Label"]);
                            }
                            objDiscoveryDropDown.NM_GenreList.Add(objGenere);
                        }
                    }

                    // Table[10] Represents NM_Category
                    if (dataset != null && dataset.Tables.Count > 10)
                    {
                        foreach (DataRow dr in dataset.Tables[10].Rows)
                        {
                            IQAgentDropDown_NM_Category objCategory = new IQAgentDropDown_NM_Category();
                            if (!dr["ID"].Equals(DBNull.Value))
                            {
                                objCategory.ID = Convert.ToInt32(dr["ID"]);
                            }
                            if (!dr["Label"].Equals(DBNull.Value))
                            {
                                objCategory.Label = Convert.ToString(dr["Label"]);
                            }
                            objDiscoveryDropDown.NM_CategoryList.Add(objCategory);
                        }
                    }

                    // Table[11] Represents NM_PublicationCategory
                    if (dataset != null && dataset.Tables.Count > 11)
                    {
                        foreach (DataRow dr in dataset.Tables[11].Rows)
                        {
                            IQAgentDropDown_NM_PublicationCategory objPublicationCategory = new IQAgentDropDown_NM_PublicationCategory();
                            if (!dr["ID"].Equals(DBNull.Value))
                            {
                                objPublicationCategory.ID = Convert.ToInt32(dr["ID"]);
                            }
                            if (!dr["Label"].Equals(DBNull.Value))
                            {
                                objPublicationCategory.Label = Convert.ToString(dr["Label"]);
                            }
                            objDiscoveryDropDown.NM_PublicationCategoryList.Add(objPublicationCategory);
                        }
                    }

                    // Table[12] Represents the Search Settings
                    if (dataset != null && dataset.Tables.Count > 12)
                    {
                        foreach (DataRow datarow in dataset.Tables[12].Rows)
                        {
                            objDiscoveryDropDown.IsAllDmaAllowed = Convert.ToBoolean(datarow["IsAllDmaAllowed"]);
                            objDiscoveryDropDown.IsAllClassAllowed = Convert.ToBoolean(datarow["IsAllClassAllowed"]);
                            objDiscoveryDropDown.IsAllStationAllowed = Convert.ToBoolean(datarow["IsAllStationAllowed"]);
                        }
                    }
                    // Table[13] Represents NM_DMA 
                    foreach (DataRow dr in dataset.Tables[13].Rows)
                    {
                        IQAgentDropDown_NM_Market objDMA = new IQAgentDropDown_NM_Market();
                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            objDMA.ID = Convert.ToInt32(dr["ID"]);
                        }
                        if (!dr["Label"].Equals(DBNull.Value))
                        {
                            objDMA.Label = Convert.ToString(dr["Label"]);
                        }
                        objDiscoveryDropDown.NM_MarketList.Add(objDMA);
                    }
                    if (objDiscoveryDropDown.NM_MarketList != null && objDiscoveryDropDown.NM_MarketList.Count() > 0)
                    {
                        int index = 0;

                        IQAgentDropDown_NM_Market tmpDma = objDiscoveryDropDown.NM_MarketList.Find(d => string.Compare(d.Label, "National", true) == 0);

                        if (tmpDma != null)
                        {
                            objDiscoveryDropDown.NM_MarketList.Remove(tmpDma);
                            objDiscoveryDropDown.NM_MarketList.Insert(index, tmpDma);

                            index = index + 1;
                        }

                        tmpDma = objDiscoveryDropDown.NM_MarketList.Find(d => string.Compare(d.Label, "International", true) == 0);

                        if (tmpDma != null)
                        {
                            objDiscoveryDropDown.NM_MarketList.Remove(tmpDma);
                            objDiscoveryDropDown.NM_MarketList.Insert(index, tmpDma);

                            index = index + 1;
                        }

                        tmpDma = objDiscoveryDropDown.NM_MarketList.Find(d => string.Compare(d.Label, "Canada", true) == 0);

                        if (tmpDma != null)
                        {
                            objDiscoveryDropDown.NM_MarketList.Remove(tmpDma);
                            objDiscoveryDropDown.NM_MarketList.Insert(index, tmpDma);

                            index = index + 1;
                        }
                    }
                }

                return objDiscoveryDropDown;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
