using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using IQMedia.Data.Base;
using System.Data;
using IQMedia.Model;
using System.Collections.ObjectModel;

namespace IQMedia.Data
{
    public class SSPDA
    {
        public Dictionary<string, object> GetSSPDataByClientGUID(Guid p_ClientGUID, out bool p_IsAllDmaAllowed, out bool p_IsAllClassAllowed, out bool p_IsAllStationAllowed, List<int> p_TVRegions, bool isTAds = false, List<string> listOfTAdsIQStationID = null)
        {
            p_IsAllDmaAllowed = false;
            p_IsAllClassAllowed = false;
            p_IsAllStationAllowed = false;

            List<DataType> dataTypeList = new List<DataType>();
            dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));

            DataSet dsSSP = DataAccess.GetDataSet("usp_v4_IQ_Station_SelectSSPDataByClientGUID", dataTypeList);

            List<IQ_Dma> listOfIQDma = new List<IQ_Dma>();

            if (dsSSP != null && dsSSP.Tables.Count > 0)
            {
                //List<Shared.Utility.CommonFunctions.IQTVRegions> enums = Enum.GetValues(typeof(Shared.Utility.CommonFunctions.IQTVRegions)).Cast<Shared.Utility.CommonFunctions.IQTVRegions>().Where(e => p_TVRegions.Contains(e.ToString()) == false).Select(e => e).ToList();

                foreach (DataRow datarow in dsSSP.Tables[0].Rows)
                {
                    //if (!enums.Exists(a => a == (Shared.Utility.CommonFunctions.IQTVRegions)Convert.ToInt32(datarow["Dma_Num"])))
                    //{
                    IQ_Dma iqDma = new IQ_Dma();
                    iqDma.Name = Convert.ToString(datarow["Dma_Name"]);
                    iqDma.Num = Convert.ToString(datarow["Dma_Num"]);
                    listOfIQDma.Add(iqDma);
                    //}

                }

                int index = 0;

                IQ_Dma tmpDma = listOfIQDma.Find(d => string.Compare(d.Name, "National", true) == 0);

                if (tmpDma != null)
                {
                    listOfIQDma.Remove(tmpDma);
                    listOfIQDma.Insert(index, tmpDma);

                    index = index + 1;
                }

                tmpDma = listOfIQDma.Find(d => string.Compare(d.Name, "International", true) == 0);

                if (tmpDma != null)
                {
                    listOfIQDma.Remove(tmpDma);
                    listOfIQDma.Insert(index, tmpDma);

                    index = index + 1;
                }

                tmpDma = listOfIQDma.Find(d => string.Compare(d.Name, "Canada", true) == 0);

                if (tmpDma != null)
                {
                    listOfIQDma.Remove(tmpDma);
                    listOfIQDma.Insert(index, tmpDma);

                    index = index + 1;
                }
            }

            List<IQ_Class> listOfIQClass = new List<IQ_Class>();

            if (dsSSP != null && dsSSP.Tables.Count > 1)
            {
                foreach (DataRow datarow in dsSSP.Tables[1].Rows)
                {
                    IQ_Class iqClass = new IQ_Class();
                    iqClass.Name = Convert.ToString(datarow["IQ_Class"]);
                    iqClass.Num = Convert.ToString(datarow["IQ_Class_Num"]);

                    listOfIQClass.Add(iqClass);
                }
            }

            List<Station_Affil> listOfIQStation = new List<Station_Affil>();
            List<IQ_Station> listOfIQStationID = new List<IQ_Station>();

            if (dsSSP != null && dsSSP.Tables.Count > 2)
            {
                foreach (DataRow datarow in dsSSP.Tables[2].Rows)
                {
                    if (!isTAds || listOfTAdsIQStationID == null || listOfTAdsIQStationID.Contains(datarow["IQ_Station_ID"]))
                    {
                        if (!listOfIQStation.Select(a => a.Name).Contains(datarow["Station_Affil"]))
                        {
                            Station_Affil iqStation = new Station_Affil();
                            iqStation.Name = Convert.ToString(datarow["Station_Affil"]);

                            listOfIQStation.Add(iqStation);
                        }

                        IQ_Station iqStationId = new IQ_Station();
                        iqStationId.IQ_Station_ID = Convert.ToString(datarow["IQ_Station_ID"]);
                        iqStationId.Station_Call_Sign = Convert.ToString(datarow["Station_Call_Sign"]);

                        listOfIQStationID.Add(iqStationId);
                    }
                }
            }

            if (dsSSP != null && dsSSP.Tables.Count > 3)
            {
                foreach (DataRow datarow in dsSSP.Tables[3].Rows)
                {
                    p_IsAllDmaAllowed = Convert.ToBoolean(datarow["IsAllDmaAllowed"]);
                    p_IsAllClassAllowed = Convert.ToBoolean(datarow["IsAllClassAllowed"]);
                    p_IsAllStationAllowed = Convert.ToBoolean(datarow["IsAllStationAllowed"]);
                }
            }

            List<IQ_Region> listOfRegion = new List<IQ_Region>();

            if (dsSSP != null && dsSSP.Tables.Count > 4)
            {
                foreach (DataRow datarow in dsSSP.Tables[4].Rows)
                {
                    IQ_Region region = new IQ_Region();
                    region.Name = Convert.ToString(datarow["Region"]);
                    region.Num = Convert.ToInt32(datarow["Region_Num"]);

                    listOfRegion.Add(region);
                }
            }

            List<IQ_Country> listOfCountry = new List<IQ_Country>();

            if (dsSSP != null && dsSSP.Tables.Count > 5)
            {
                foreach (DataRow datarow in dsSSP.Tables[5].Rows)
                {
                    IQ_Country country = new IQ_Country();
                    country.Name = Convert.ToString(datarow["Country"]);
                    country.Num = Convert.ToInt32(datarow["Country_Num"]);

                    listOfCountry.Add(country);
                }

                IQ_Country tmpCountry = listOfCountry.Find(c => c.Num == 1);

                if (tmpCountry != null)
                {
                    listOfCountry.Remove(tmpCountry);
                    listOfCountry.Insert(0, tmpCountry);
                }
            }

            Dictionary<string, object> dicSSP = new Dictionary<string, object>();

            dicSSP.Add("IQ_Dma", listOfIQDma);
            dicSSP.Add("Station_Affil", listOfIQStation.OrderBy(a => a.Name).ToList());
            dicSSP.Add("IQ_Class", listOfIQClass);
            dicSSP.Add("IQ_Station", listOfIQStationID.Distinct().OrderBy(a => a.Station_Call_Sign).ToList());
            dicSSP.Add("IQ_Country", listOfCountry);
            dicSSP.Add("IQ_Region", listOfRegion);

            return dicSSP;
        }

        public Dictionary<string, object> GetSSPDataByClientGUIDAndFilter(Guid p_ClientGUID, string p_Dma, string p_Station, string p_StationID, int? p_Region, int? p_Country, List<int> p_TVRegions)
        {


            List<DataType> dataTypeList = new List<DataType>();
            dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@SelectedMarket", DbType.Xml, p_Dma, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@SelectedAffil", DbType.Xml, p_Station, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@SelectedStation", DbType.Xml, p_StationID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@Region", DbType.Int32, p_Region, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@Country", DbType.Int32, p_Country, ParameterDirection.Input));

            DataSet dsSSP = DataAccess.GetDataSet("usp_v4_IQ_Station_SelectSSPDataByClientGUIDAndFilter", dataTypeList);

            List<IQ_Dma> listOfIQDma = new List<IQ_Dma>();

            if (dsSSP != null && dsSSP.Tables.Count > 0)
            {
                //List<Shared.Utility.CommonFunctions.IQTVRegions> enums = Enum.GetValues(typeof(Shared.Utility.CommonFunctions.IQTVRegions)).Cast<Shared.Utility.CommonFunctions.IQTVRegions>().Where(e => p_TVRegions.Contains(e.ToString()) == false).Select(e => e).ToList();

                foreach (DataRow datarow in dsSSP.Tables[0].Rows)
                {
                    //if (!enums.Exists(a => a == (Shared.Utility.CommonFunctions.IQTVRegions)Convert.ToInt32(datarow["Dma_Num"])))
                    //{
                    IQ_Dma iqDma = new IQ_Dma();
                    iqDma.Name = Convert.ToString(datarow["Dma_Name"]);
                    iqDma.Num = Convert.ToString(datarow["Dma_Num"]);

                    listOfIQDma.Add(iqDma);
                    //}
                }

                int index = 0;

                IQ_Dma tmpDma = listOfIQDma.Find(d => string.Compare(d.Name, "National", true) == 0);

                if (tmpDma != null)
                {
                    listOfIQDma.Remove(tmpDma);
                    listOfIQDma.Insert(index, tmpDma);

                    index = index + 1;
                }

                tmpDma = listOfIQDma.Find(d => string.Compare(d.Name, "International", true) == 0);

                if (tmpDma != null)
                {
                    listOfIQDma.Remove(tmpDma);
                    listOfIQDma.Insert(index, tmpDma);

                    index = index + 1;
                }

                tmpDma = listOfIQDma.Find(d => string.Compare(d.Name, "Canada", true) == 0);

                if (tmpDma != null)
                {
                    listOfIQDma.Remove(tmpDma);
                    listOfIQDma.Insert(index, tmpDma);

                    index = index + 1;
                }
            }

            List<Station_Affil> listOfIQStation = new List<Station_Affil>();
            List<IQ_Station> listOfIQStationID = new List<IQ_Station>();

            if (dsSSP != null && dsSSP.Tables.Count > 1)
            {
                foreach (DataRow datarow in dsSSP.Tables[1].Rows)
                {
                    if (!listOfIQStation.Select(a => a.Name).Contains(datarow["Station_Affil"]))
                    {
                        Station_Affil iqStation = new Station_Affil();
                        iqStation.Name = Convert.ToString(datarow["Station_Affil"]);
                        listOfIQStation.Add(iqStation);
                    }


                    IQ_Station iqStationId = new IQ_Station();
                    iqStationId.IQ_Station_ID = Convert.ToString(datarow["IQ_Station_ID"]);
                    iqStationId.Station_Call_Sign = Convert.ToString(datarow["Station_Call_Sign"]);

                    listOfIQStationID.Add(iqStationId);
                }
            }

            List<IQ_Region> listOfRegion = new List<IQ_Region>();

            if (dsSSP != null && dsSSP.Tables.Count > 2)
            {
                foreach (DataRow datarow in dsSSP.Tables[2].Rows)
                {
                    IQ_Region region = new IQ_Region();
                    region.Name = Convert.ToString(datarow["Region"]);
                    region.Num = Convert.ToInt32(datarow["Region_Num"]);

                    listOfRegion.Add(region);
                }
            }

            List<IQ_Country> listOfCountry = new List<IQ_Country>();

            if (dsSSP != null && dsSSP.Tables.Count > 3)
            {
                foreach (DataRow datarow in dsSSP.Tables[3].Rows)
                {
                    IQ_Country country = new IQ_Country();
                    country.Name = Convert.ToString(datarow["Country"]);
                    country.Num = Convert.ToInt32(datarow["Country_Num"]);

                    listOfCountry.Add(country);
                }
            }

            Dictionary<string, object> dicSSP = new Dictionary<string, object>();

            dicSSP.Add("IQ_Dma", listOfIQDma);
            dicSSP.Add("Station_Affil", listOfIQStation.OrderBy(a => a.Name).ToList());
            dicSSP.Add("IQ_Station", listOfIQStationID.Distinct().OrderBy(a => a.Station_Call_Sign).ToList());
            dicSSP.Add("IQ_Country", listOfCountry);
            dicSSP.Add("IQ_Region", listOfRegion);

            return dicSSP;
        }

        public Dictionary<string, object> GetSSPDataWithStationByClientGUIDOld(Guid p_ClientGUID, out bool p_IsAllDmaAllowed, out bool p_IsAllClassAllowed, out bool p_IsAllStationAllowed, List<int> p_TVRegions)
        {
            p_IsAllDmaAllowed = false;
            p_IsAllClassAllowed = false;
            p_IsAllStationAllowed = false;

            List<DataType> dataTypeList = new List<DataType>();
            dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));

            DataSet dsSSP = DataAccess.GetDataSet("usp_v4_IQ_Station_SelectSSPDataWithStationByClientGUID", dataTypeList);

            List<IQ_Dma> listOfIQDma = new List<IQ_Dma>();

            if (dsSSP != null && dsSSP.Tables.Count > 0)
            {
                //List<Shared.Utility.CommonFunctions.IQTVRegions> enums = Enum.GetValues(typeof(Shared.Utility.CommonFunctions.IQTVRegions)).Cast<Shared.Utility.CommonFunctions.IQTVRegions>().Where(e => p_TVRegions.Contains(e.ToString()) == false).Select(e => e).ToList();

                foreach (DataRow datarow in dsSSP.Tables[0].Rows)
                {
                    /*if (!enums.Exists(a => a == (Shared.Utility.CommonFunctions.IQTVRegions)Convert.ToInt32(datarow["Dma_Num"])))
                    { */
                    IQ_Dma iqDma = new IQ_Dma();
                    iqDma.Name = Convert.ToString(datarow["Dma_Name"]);
                    iqDma.Num = Convert.ToString(datarow["Dma_Num"]);
                    listOfIQDma.Add(iqDma);
                    /*}*/
                }

                int index = 0;

                IQ_Dma tmpDma = listOfIQDma.Find(d => string.Compare(d.Name, "National", true) == 0);

                if (tmpDma != null)
                {
                    listOfIQDma.Remove(tmpDma);
                    listOfIQDma.Insert(index, tmpDma);

                    index = index + 1;
                }

                tmpDma = listOfIQDma.Find(d => string.Compare(d.Name, "International", true) == 0);

                if (tmpDma != null)
                {
                    listOfIQDma.Remove(tmpDma);
                    listOfIQDma.Insert(index, tmpDma);

                    index = index + 1;
                }

                tmpDma = listOfIQDma.Find(d => string.Compare(d.Name, "Canada", true) == 0);

                if (tmpDma != null)
                {
                    listOfIQDma.Remove(tmpDma);
                    listOfIQDma.Insert(index, tmpDma);

                    index = index + 1;
                }
            }

            List<IQ_Class> listOfIQClass = new List<IQ_Class>();

            if (dsSSP != null && dsSSP.Tables.Count > 1)
            {
                foreach (DataRow datarow in dsSSP.Tables[1].Rows)
                {
                    IQ_Class iqClass = new IQ_Class();
                    iqClass.Name = Convert.ToString(datarow["IQ_Class"]);
                    iqClass.Num = Convert.ToString(datarow["IQ_Class_Num"]);

                    listOfIQClass.Add(iqClass);
                }
            }

            List<IQ_Station> listOfIQStation = new List<IQ_Station>();
            List<Station_Affil> listOfAffiliate = new List<Station_Affil>();

            if (dsSSP != null && dsSSP.Tables.Count > 2)
            {
                foreach (DataRow datarow in dsSSP.Tables[2].Rows)
                {

                    if (!listOfAffiliate.Select(a => a.Name).Contains(datarow["Station_Affil"]))
                    {
                        Station_Affil iqStation = new Station_Affil();
                        iqStation.Name = Convert.ToString(datarow["Station_Affil"]);

                        listOfAffiliate.Add(iqStation);
                    }

                    IQ_Station iqStationId = new IQ_Station();
                    iqStationId.IQ_Station_ID = Convert.ToString(datarow["IQ_Station_ID"]);
                    iqStationId.Station_Call_Sign = Convert.ToString(datarow["Station_Call_Sign"]);

                    listOfIQStation.Add(iqStationId);
                }
            }

            if (dsSSP != null && dsSSP.Tables.Count > 3)
            {
                foreach (DataRow datarow in dsSSP.Tables[3].Rows)
                {
                    p_IsAllDmaAllowed = Convert.ToBoolean(datarow["IsAllDmaAllowed"]);
                    p_IsAllClassAllowed = Convert.ToBoolean(datarow["IsAllClassAllowed"]);
                    p_IsAllStationAllowed = Convert.ToBoolean(datarow["IsAllStationAllowed"]);
                }
            }

            List<IQ_Region> listOfRegion = new List<IQ_Region>();

            if (dsSSP != null && dsSSP.Tables.Count > 4)
            {
                foreach (DataRow datarow in dsSSP.Tables[4].Rows)
                {
                    IQ_Region region = new IQ_Region();
                    region.Name = Convert.ToString(datarow["Region"]);
                    region.Num = Convert.ToInt32(datarow["Region_Num"]);

                    listOfRegion.Add(region);
                }
            }

            List<IQ_Country> listOfCountry = new List<IQ_Country>();

            if (dsSSP != null && dsSSP.Tables.Count > 5)
            {
                foreach (DataRow datarow in dsSSP.Tables[5].Rows)
                {
                    IQ_Country country = new IQ_Country();
                    country.Name = Convert.ToString(datarow["Country"]);
                    country.Num = Convert.ToInt32(datarow["Country_Num"]);

                    listOfCountry.Add(country);
                }

                IQ_Country tmpCountry = listOfCountry.Find(c => c.Num == 1);

                if (tmpCountry != null)
                {
                    listOfCountry.Remove(tmpCountry);
                    listOfCountry.Insert(0, tmpCountry);
                }
            }

            Dictionary<string, object> dicSSP = new Dictionary<string, object>();

            dicSSP.Add("IQ_Dma", listOfIQDma);
            dicSSP.Add("IQ_Station", listOfIQStation);
            dicSSP.Add("IQ_Class", listOfIQClass);
            dicSSP.Add("Station_Affil", listOfAffiliate);
            dicSSP.Add("IQ_Country", listOfCountry);
            dicSSP.Add("IQ_Region", listOfRegion);


            return dicSSP;
        }

        public Dictionary<string, object> GetDMAsByZipCode(string zipCodeList)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ZipCodes", DbType.Xml, zipCodeList, ParameterDirection.Input));
                DataSet dataSet = DataAccess.GetDataSet("usp_v4_IQAgent_SearchRequest_GetDMAsByZipCode", dataTypeList);

                List<IQ_Zip_Code> listOfIQDmas = new List<IQ_Zip_Code>();
                if (dataSet.Tables[0] != null)
                {
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        IQ_Zip_Code zipCode = new IQ_Zip_Code();
                        zipCode.ZipCode = Int32.Parse(dr["zip_code"].ToString());
                        zipCode.IQ_DMA_Name = dr["iq_dma_name"].ToString();

                        listOfIQDmas.Add(zipCode);
                    }
                }

                List<int> listOfInvalidZipCodes = new List<int>();
                if (dataSet.Tables[1] != null)
                {
                    listOfInvalidZipCodes = dataSet.Tables[1].Rows.OfType<DataRow>().Select(dr => dr.Field<int>("zip_code")).ToList();
                }

                Dictionary<string, object> dictResults = new Dictionary<string, object>();
                dictResults.Add("DMAs", listOfIQDmas);
                dictResults.Add("InvalidZipCodes", listOfInvalidZipCodes);
                return dictResults;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
