using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using System.Data;
using System.Xml.Linq;
using IQMedia.Model;

namespace IQMedia.Data
{
    public class IQNielsenDA : IDataAccess
    {
        public List<DiscoveryMediaResult> GetNielsenDataByXML(XDocument xmldata, List<DiscoveryMediaResult> lstDiscoveryMediaResult, Guid clientGuid)
        {
            DataSet _DataSet = new DataSet();
            try
            {
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@IQCCKeyList", DbType.Xml, xmldata.ToString(), ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQ_NIELSEN_SQAD_SelectByIQCCKeyList", _ListOfDataType);

                return GetAudienceAndIQAdShareValue(xmldata, dataset, lstDiscoveryMediaResult);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<NielsenDataModel> GetTimeshiftNielsenDataByXML(XDocument xmldata, Guid clientGuid)
        {
            DataSet _DataSet = new DataSet();
            try
            {
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@IQCCKeyList", DbType.Xml, xmldata.ToString(), ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQ_NIELSEN_SQAD_SelectByIQCCKeyList", _ListOfDataType);

                return FillNeilsenDataModel(xmldata, dataset);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<NielsenDataModel> GetTAdsNielsenDataByXML(XDocument xmldata, Guid clientGuid)
        {
            DataSet _DataSet = new DataSet();
            try
            {
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@IQCCKeyList", DbType.Xml, xmldata.ToString(), ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQ_NIELSEN_SQAD_SelectByIQCCKeyList", _ListOfDataType);

                return FillNeilsenDataModel(xmldata, dataset);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private List<DiscoveryMediaResult> GetAudienceAndIQAdShareValue(XDocument xmldata, DataSet result, List<DiscoveryMediaResult> lstDiscoveryMediaResult)
        {
            try
            {
                if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in result.Tables[0].Rows)
                    {
                        DiscoveryMediaResult discoveryMediaResult = lstDiscoveryMediaResult.Find(p => p.IQ_CC_Key.Equals(dr["IQ_CC_KEY"]));
                        if (discoveryMediaResult != null)
                        {
                            if (!dr["AUDIENCE"].Equals(DBNull.Value))
                            {
                                discoveryMediaResult.Audience = Convert.ToInt32(dr["AUDIENCE"]);
                            }
                            
                            if (!dr["SQAD_SHAREVALUE"].Equals(DBNull.Value))
                            {
                                discoveryMediaResult.IQAdsharevalue = Convert.ToDecimal(dr["SQAD_SHAREVALUE"]);
                                discoveryMediaResult.Nielsen_Result = Convert.ToBoolean(dr["IsActualNielsen"]) == true ? " (A)" : " (E)";
                            }
                        }
                    }
                }
                return lstDiscoveryMediaResult;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        private List<NielsenDataModel> FillNeilsenDataModel(XDocument xmldata, DataSet result)
        {
            try
            {
                List<NielsenDataModel> listOfNelsenData = new List<NielsenDataModel>();
                if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in result.Tables[0].Rows)
                    {
                        NielsenDataModel discoveryMediaResult = new NielsenDataModel();
                        if (!string.IsNullOrEmpty(Convert.ToString(dr["AUDIENCE"])))
                        {
                            discoveryMediaResult.Audience = Convert.ToDecimal(dr["AUDIENCE"]);
                        }
                        if (!string.IsNullOrEmpty(Convert.ToString(dr["SQAD_SHAREVALUE"])))
                        {
                            discoveryMediaResult.IQAdsharevalue = Convert.ToDecimal(dr["SQAD_SHAREVALUE"]);
                        }
                        discoveryMediaResult.IsActualNielsen = Convert.ToBoolean(dr["IsActualNielsen"]);
                        discoveryMediaResult.IQ_CC_Key = Convert.ToString(dr["IQ_CC_Key"]);
                        listOfNelsenData.Add(discoveryMediaResult);
                    }
                }
                return listOfNelsenData;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
