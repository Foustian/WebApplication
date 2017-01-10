using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using IQMedia.Model;
using System.Data;

namespace IQMedia.Data
{
    public class FacebookDA : IDataAccess
    {
        public void InsertFBPages(string FBPageXml)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@FBPageXml", DbType.Xml, FBPageXml, ParameterDirection.Input));
                DataAccess.ExecuteNonQuery("usp_v4_IQ_FacebookProfile_Insert", dataTypeList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<FBPageModel> GetFBPages()
        {
            try
            {
                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQ_FacebookProfile_Select", new List<DataType>());

                List<FBPageModel> lstFBPageModel = new List<FBPageModel>();
                if (dataset != null && dataset.Tables.Count > 0)
                {
                    if (dataset.Tables[0] != null)
                    {
                        DataTable dataTable = dataset.Tables[0];
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            FBPageModel FBPageModel = new FBPageModel();

                            if (!dr["ID"].Equals(DBNull.Value))
                            {
                                FBPageModel.ID = Convert.ToInt64(dr["ID"]);
                            }
                            if (!dr["FBPageID"].Equals(DBNull.Value))
                            {
                                FBPageModel.FBPageID = Convert.ToInt64(dr["FBPageID"]);
                            }
                            if (!dr["FBPageName"].Equals(DBNull.Value))
                            {
                                FBPageModel.FBPageName = Convert.ToString(dr["FBPageName"]);
                            }
                            if (!dr["FBLikes"].Equals(DBNull.Value))
                            {
                                FBPageModel.Likes = Convert.ToInt64(dr["FBLikes"]);
                            }
                            if (!dr["FBLink"].Equals(DBNull.Value))
                            {
                                FBPageModel.FBPageUrl = Convert.ToString(dr["FBLink"]);
                            }
                            if (!dr["FBCategory"].Equals(DBNull.Value))
                            {
                                FBPageModel.Category = Convert.ToString(dr["FBCategory"]);
                            }
                            if (!dr["FBPicture"].Equals(DBNull.Value))
                            {
                                FBPageModel.PictureUrl = Convert.ToString(dr["FBPicture"]);
                            }
                            if (!dr["FBIsVerified"].Equals(DBNull.Value))
                            {
                                FBPageModel.IsVerified = Convert.ToBoolean(dr["FBIsVerified"]);
                            }

                            lstFBPageModel.Add(FBPageModel);
                        }
                    }
                }

                return lstFBPageModel;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
