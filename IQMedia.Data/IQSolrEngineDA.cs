using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using IQMedia.Data.Base;
using System.Data;

namespace IQMedia.Data
{
    public static class IQSolrEngineDA
    {
        public static List<IQSolrEngineModel> GetSolrEngines()
        {
            try
            {
                List<IQSolrEngineModel> lstIQSolrEngineModel = new List<IQSolrEngineModel>();
                List<DataType> _ListOfDataType = new List<DataType>();


                DataSet _Result = DataAccess.GetDataSet("usp_v4_IQSolrEngines_SelectAll", _ListOfDataType);
                if (_Result.Tables.Count > 0 && _Result.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in _Result.Tables[0].Rows)
                    {
                        IQSolrEngineModel objIQSolrEngineModel = new IQSolrEngineModel();

                        if (_Result.Tables[0].Columns.Contains("MediaType") && !dr["MediaType"].Equals(DBNull.Value))
                        {
                            objIQSolrEngineModel.Type = Convert.ToString(dr["MediaType"]);
                        }

                        if (_Result.Tables[0].Columns.Contains("BaseUrl") && !dr["BaseUrl"].Equals(DBNull.Value))
                        {
                            objIQSolrEngineModel.BaseUrl = Convert.ToString(dr["BaseUrl"]);
                        }

                        if (_Result.Tables[0].Columns.Contains("FromDate") && !dr["FromDate"].Equals(DBNull.Value))
                        {
                            objIQSolrEngineModel.FromDate = Convert.ToDateTime(dr["FromDate"]);
                        }

                        if (_Result.Tables[0].Columns.Contains("ToDate") && !dr["ToDate"].Equals(DBNull.Value))
                        {
                            objIQSolrEngineModel.ToDate = Convert.ToDateTime(dr["ToDate"]);
                        }

                        lstIQSolrEngineModel.Add(objIQSolrEngineModel);
                    }

                }

                return lstIQSolrEngineModel;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
