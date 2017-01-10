using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using System.Data;
using IQMedia.Model;

namespace IQMedia.Data
{
    public class IQCompeteAllDA : IDataAccess
    {
        public List<IQCompeteAll> GetArtileAdShareValueByClientGuidAndXml(Guid p_ClientGuid, string p_WebSiteURLXml, string p_MediaType)
        {
            try
            {
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PublicationXml", DbType.Xml, p_WebSiteURLXml, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@MediaType", DbType.String, p_MediaType, ParameterDirection.Input));
                DataSet _Result = DataAccess.GetDataSet("usp_IQ_CompeteAll_SelectArtileAdShareByClientGuidAndXml", _ListOfDataType);
                return FillListOfIQ_Compete(_Result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<IQCompeteAll> FillListOfIQ_Compete(DataSet p_DataSet)
        {
            try
            {
                List<IQCompeteAll> _ListOfIQ_CompeteAll = new List<IQCompeteAll>();

                if (p_DataSet != null && p_DataSet.Tables.Count > 0)
                {
                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        IQCompeteAll iqCompeteAll = new IQCompeteAll();

                        if (p_DataSet.Tables[0].Columns.Contains("CompeteURL") && !_DataRow["CompeteURL"].Equals(DBNull.Value))
                        {
                            iqCompeteAll.CompeteURL = Convert.ToString(_DataRow["CompeteURL"]);
                        }

                        if (p_DataSet.Tables[0].Columns.Contains("IQ_AdShare_Value") && !_DataRow["IQ_AdShare_Value"].Equals(DBNull.Value))
                        {
                            iqCompeteAll.IQ_AdShare_Value = Convert.ToDecimal(_DataRow["IQ_AdShare_Value"]);
                        }

                        if (p_DataSet.Tables[0].Columns.Contains("c_uniq_visitor") && !_DataRow["c_uniq_visitor"].Equals(DBNull.Value))
                        {
                            iqCompeteAll.c_uniq_visitor = Convert.ToInt32(_DataRow["c_uniq_visitor"]);
                        }

                        if (p_DataSet.Tables[0].Columns.Contains("IsCompeteAll") && !_DataRow["IsCompeteAll"].Equals(DBNull.Value))
                        {
                            iqCompeteAll.IsCompeteAll = Convert.ToBoolean(_DataRow["IsCompeteAll"]);
                        }

                        if (p_DataSet.Tables[0].Columns.Contains("IsUrlFound") && !_DataRow["IsUrlFound"].Equals(DBNull.Value))
                        {
                            iqCompeteAll.IsUrlFound = Convert.ToBoolean(_DataRow["IsUrlFound"]);
                        }

                        _ListOfIQ_CompeteAll.Add(iqCompeteAll);

                    }
                }

                return _ListOfIQ_CompeteAll;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
