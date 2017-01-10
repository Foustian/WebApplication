using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using IQMedia.Model;
using System.Data;

namespace IQMedia.Data
{
    public class IQNewsDA : IDataAccess
    {
        public List<IQNews_Model> GetIQNews()
        {
            try
            {

                List<DataType> dataTypeList = new List<DataType>();
                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQ_News_Search", dataTypeList);
                List<IQNews_Model> lstIQNews_Model = FillIQNewsList(dataset);

                return lstIQNews_Model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<IQNews_Model> FillIQNewsList(DataSet p_Dataset)
        {
            List<IQNews_Model> listIQNews_Model = new List<IQNews_Model>();

            if (p_Dataset != null && p_Dataset.Tables.Count > 0 && p_Dataset.Tables[0] != null)
            {
                foreach (DataRow dr in p_Dataset.Tables[0].Rows)
                {
                    IQNews_Model iQNews_Model = new IQNews_Model();
                    if (!dr["Headline"].Equals(DBNull.Value))
                    {
                        iQNews_Model.Headline = Convert.ToString(dr["Headline"]);
                    }

                    if (!dr["ReleaseDate"].Equals(DBNull.Value))
                    {
                        iQNews_Model.ReleaseDate = Convert.ToDateTime(dr["ReleaseDate"]);
                    }

                    if (!dr["SubHead"].Equals(DBNull.Value))
                    {
                        iQNews_Model.SubHead = Convert.ToString(dr["SubHead"]);
                    }


                    if (!dr["Detail"].Equals(DBNull.Value))
                    {
                        iQNews_Model.Detail = Convert.ToString(dr["Detail"]);
                    }

                    if (!dr["Url"].Equals(DBNull.Value))
                    {
                        iQNews_Model.Url = Convert.ToString(dr["Url"]);
                    }

                    listIQNews_Model.Add(iQNews_Model);
                }
            }

            return listIQNews_Model;
        }
    }
}
