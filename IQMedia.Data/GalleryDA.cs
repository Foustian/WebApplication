using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using System.Data;
using IQMedia.Model;

namespace IQMedia.Data
{
    public class GalleryDA : IDataAccess
    {
        public string InsertGallery(Guid p_CustomerGUID, IQGalleryModel p_IQGalleryModel)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@CustomerGUID", DbType.Guid, p_CustomerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Name", DbType.String, p_IQGalleryModel.Name, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Title", DbType.String, p_IQGalleryModel.Title, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Description", DbType.String, p_IQGalleryModel.Description, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@xml", DbType.Xml, p_IQGalleryModel.xml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Output", DbType.Int32, 0, ParameterDirection.Output));

                string _Result = DataAccess.ExecuteNonQuery("usp_v4_Gallery_Insert", dataTypeList);

                return _Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string UpdateGallery(Int64 p_ID, Guid p_CustomerGUID, IQGalleryModel p_IQGalleryModel)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.Guid, p_CustomerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Name", DbType.String, p_IQGalleryModel.Name, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Title", DbType.String, p_IQGalleryModel.Title, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Description", DbType.String, p_IQGalleryModel.Description, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@xml", DbType.String, p_IQGalleryModel.xml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Output", DbType.Int32, 0, ParameterDirection.Output));

                string _Result = DataAccess.ExecuteNonQuery("usp_v4_Gallery_Update", dataTypeList);

                return _Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IQGalleryResult GetGalleryListByCustomerGUID(Guid p_CustomerGUID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@CustomerGUID", DbType.Guid, p_CustomerGUID, ParameterDirection.Input));

                DataSet dataSet = DataAccess.GetDataSet("usp_v4_Gallery_SelectByCustomerGUID", dataTypeList);

                IQGalleryResult _Result = FillIQGalleryResults(dataSet);

                return _Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IQGalleryModel GetGalleryByID(Int64 p_ID, Guid p_CustomerGUID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@CustomerGUID", DbType.Guid, p_CustomerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));

                DataSet dataSet = DataAccess.GetDataSet("usp_v4_Gallery_SelectByID", dataTypeList);

                IQGalleryModel _IQGalleryModel = FillIQGallery(dataSet);

                return _IQGalleryModel;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<IQGallery> GetClipIDByMediaIDs(string p_IDs)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();

                dataTypeList.Add(new DataType("@IDs", DbType.String, p_IDs, ParameterDirection.Input));

                DataSet dataSet = DataAccess.GetDataSet("usp_v4_Gallery_SelectClipID_ByMediaID", dataTypeList);

                List<IQGallery> lstIQGallery = FillIQGalleryList(dataSet);

                return lstIQGallery;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private IQGalleryResult FillIQGalleryResults(DataSet dataSet)
        {
            IQGalleryResult objIQGalleryResult = new IQGalleryResult();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                objIQGalleryResult.IQGalleryList = new List<IQGalleryModel>();

                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    IQGalleryModel objIQGalleryModel = new IQGalleryModel();

                    if (!dr["ID"].Equals(DBNull.Value))
                    {
                        objIQGalleryModel.ID = Convert.ToInt64(dr["ID"]);
                    }

                    if (!dr["Name"].Equals(DBNull.Value))
                    {
                        objIQGalleryModel.Name = Convert.ToString(dr["Name"]);
                    }

                    if (!dr["Description"].Equals(DBNull.Value))
                    {
                        objIQGalleryModel.Description = Convert.ToString(dr["Description"]);
                    }

                    if (!dr["CreatedDate"].Equals(DBNull.Value))
                    {
                        objIQGalleryModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    }

                    objIQGalleryResult.IQGalleryList.Add(objIQGalleryModel);
                }
            }
            return objIQGalleryResult;
        }

        private IQGalleryModel FillIQGallery(DataSet dataSet)
        {
            IQGalleryModel objIQGalleryModel = new IQGalleryModel();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    if (!dr["ID"].Equals(DBNull.Value))
                    {
                        objIQGalleryModel.ID = Convert.ToInt64(dr["ID"]);
                    }

                    if (!dr["Name"].Equals(DBNull.Value))
                    {
                        objIQGalleryModel.Name = Convert.ToString(dr["Name"]);
                    }

                    if (!dr["Title"].Equals(DBNull.Value))
                    {
                        objIQGalleryModel.Title = Convert.ToString(dr["Title"]);
                    }

                    if (!dr["Description"].Equals(DBNull.Value))
                    {
                        objIQGalleryModel.Description = Convert.ToString(dr["Description"]);
                    }
                }
            }

            if (dataSet != null && dataSet.Tables.Count > 1)
            {
                List<IQGallery> lstIQgallery = new List<IQGallery>();
                foreach (DataRow dr in dataSet.Tables[1].Rows)
                {
                    IQGallery objIQGallery = new IQGallery();
                    if (!dr["_ArchiveMediaID"].Equals(DBNull.Value))
                    {
                        objIQGallery.ID = Convert.ToInt64(dr["_ArchiveMediaID"]);
                    }

                    if (!dr["Col"].Equals(DBNull.Value))
                    {
                        objIQGallery.Col = Convert.ToInt32(dr["Col"]);
                    }

                    if (!dr["Row"].Equals(DBNull.Value))
                    {
                        objIQGallery.Row = Convert.ToInt32(dr["Row"]);
                    }

                    if (!dr["Name"].Equals(DBNull.Value))
                    {
                        objIQGallery.Type = Convert.ToString(dr["Name"]);
                    }

                    if (!dr["Width"].Equals(DBNull.Value))
                    {
                        objIQGallery.size_x = Convert.ToInt32(dr["Width"]);
                    }

                    if (!dr["Height"].Equals(DBNull.Value))
                    {
                        objIQGallery.size_y = Convert.ToInt32(dr["Height"]);
                    }

                    if (!dr["MetaData"].Equals(DBNull.Value))
                    {
                        objIQGallery.MetaData = Convert.ToString(dr["MetaData"]);
                    }

                    lstIQgallery.Add(objIQGallery);
                }
                objIQGalleryModel.Json = lstIQgallery.OrderBy(c => c.Col).OrderBy(r=>r.Row).ToList();
            }
            return objIQGalleryModel;
        }

        private List<IQGallery> FillIQGalleryList(DataSet dataSet)
        {
            List<IQGallery> lstIQGallery = new List<IQGallery>();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    IQGallery objIQGallery = new IQGallery();

                    if (!dr["ID"].Equals(DBNull.Value))
                    {
                        objIQGallery.ID = Convert.ToInt64(dr["ID"]);
                    }

                    if (!dr["ClipID"].Equals(DBNull.Value))
                    {
                        objIQGallery.ClipID = Convert.ToString(dr["ClipID"]);
                    }
                    lstIQGallery.Add(objIQGallery);
                }
            }
            return lstIQGallery;
        }

        public List<IQGalleryItemType> GetGalleryItemTypeList()
        {
            try
            {
                DataSet dataset = DataAccess.GetDataSetByProcedure("usp_v4_IQGalleryItemType_SelectAll");

                List<IQGalleryItemType> lstIQGalleryItemType = FillGalleryItemType(dataset);

                return lstIQGalleryItemType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<IQGalleryItemType> FillGalleryItemType(DataSet dataSet)
        {
            List<IQGalleryItemType> lstIQGalleryItemType = new List<IQGalleryItemType>();
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    IQGalleryItemType objIQGalleryItemType = new IQGalleryItemType();
                    if (dataSet.Tables[0].Columns.Contains("ID") && !dr["ID"].Equals(DBNull.Value))
                    {
                        objIQGalleryItemType.ID = Convert.ToInt32(dr["ID"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Description") && !dr["Description"].Equals(DBNull.Value))
                    {
                        objIQGalleryItemType.Description = Convert.ToString(dr["Description"]);
                    }

                    if (dataSet.Tables[0].Columns.Contains("Name") && !dr["Name"].Equals(DBNull.Value))
                    {
                        objIQGalleryItemType.Name = Convert.ToString(dr["Name"]);
                    }

                    lstIQGalleryItemType.Add(objIQGalleryItemType);
                }

            }
            return lstIQGalleryItemType;
        }
    }
}
