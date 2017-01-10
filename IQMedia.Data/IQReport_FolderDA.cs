using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using IQMedia.Model;
using System.Data;

namespace IQMedia.Data
{
    public class IQReport_FolderDA : IDataAccess
    {
        public List<IQReport_FolderModel> GetFolderData(Guid clientGuid)
        {
            try
            {
               List<IQReport_FolderModel> lstIQ_ReportFolderModel = new List<IQReport_FolderModel>();
                
                List<DataType> _ListOfDataType = new List<DataType>();

                DataSet _DataSet = new DataSet();

                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQReport_Folder_SelectbyClientGuid", _ListOfDataType);

                if (dataset.Tables != null && dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dataset.Tables[0].Rows)
                    {
                        IQReport_FolderModel iQ_ReportFolderModel = new IQReport_FolderModel();

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            iQ_ReportFolderModel.id = Convert.ToString(dr["ID"]);
                        }

                        if (!dr["text"].Equals(DBNull.Value))
                        {
                            iQ_ReportFolderModel.text = Convert.ToString(dr["text"]);
                        }

                        if (!dr["parent"].Equals(DBNull.Value))
                        {
                            iQ_ReportFolderModel.parent = Convert.ToString(dr["parent"]);
                        }
                        else
                        {
                            iQ_ReportFolderModel.parent = "#";
                        }


                        lstIQ_ReportFolderModel.Add(iQ_ReportFolderModel);
                        
                    }
                }

                return lstIQ_ReportFolderModel;
                
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<IQReport_FolderModel> GetReportAndReportFolderData(Guid clientGuid)
        {
            try
            {
                List<IQReport_FolderModel> lstIQ_ReportFolderModel = new List<IQReport_FolderModel>();

                List<DataType> _ListOfDataType = new List<DataType>();

                DataSet _DataSet = new DataSet();

                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQ_Report_SelectFolderByClientGuid", _ListOfDataType);

                if (dataset.Tables != null && dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dataset.Tables[0].Rows)
                    {
                        IQReport_FolderModel iQ_ReportFolderModel = new IQReport_FolderModel();

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            iQ_ReportFolderModel.id = Convert.ToString(dr["ID"]);
                        }

                        if (!dr["text"].Equals(DBNull.Value))
                        {
                            iQ_ReportFolderModel.text = Convert.ToString(dr["text"]);
                        }

                        if (!dr["Type"].Equals(DBNull.Value))
                        {
                            iQ_ReportFolderModel.type = Convert.ToString(dr["Type"]);
                        }

                        if (!dr["parent"].Equals(DBNull.Value))
                        {
                            iQ_ReportFolderModel.parent = Convert.ToString(dr["parent"]);
                        }
                        else
                        {
                            iQ_ReportFolderModel.parent = "#";
                        }


                        lstIQ_ReportFolderModel.Add(iQ_ReportFolderModel);

                    }
                }

                return lstIQ_ReportFolderModel;

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string CreateFolder(string p_Name, string p_ParentID, Guid clientGuid)
        {
            try
            {
                string _result = string.Empty;
                string _id = string.Empty;
                
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@Name", DbType.String, p_Name, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ParentID", DbType.Int64, p_ParentID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Output", DbType.Int64, 0, ParameterDirection.Output));

                Dictionary<string, string> _outputParams;

                _result = DataAccess.ExecuteNonQuery("usp_v4_IQReport_Folder_CreateFolder", _ListOfDataType, out _outputParams);

                if (_outputParams != null && _outputParams.Count > 0 && _outputParams.ContainsKey("@Output"))
                {
                    _id = Convert.ToString(_outputParams["@Output"]);
                }

                return _id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string RenameFolder(string p_Name, string p_ID, Guid clientGuid)
        {
            try
            {
                string _result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@Name", DbType.String, p_Name, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));

                _result = DataAccess.ExecuteNonQuery("usp_v4_IQReport_Folder_RenameFolder", _ListOfDataType);

                return _result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string MoveFolder(string p_ParentID, string p_ID, Guid clientGuid)
        {
            try
            {
                string _result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@NewParentID", DbType.Int64, p_ParentID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));

                _result = DataAccess.ExecuteNonQuery("usp_v4_IQReport_Folder_MoveFolder", _ListOfDataType);

                return _result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string MoveReport(string p_ParentID, string p_ID, Guid clientGuid)
        {
            try
            {
                string _result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@NewParentID", DbType.Int64, p_ParentID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));

                _result = DataAccess.ExecuteNonQuery("usp_v4_IQ_Report_MoveReport", _ListOfDataType);

                return _result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string DeleteFolder(string p_ID, Guid clientGuid)
        {
            try
            {
                string _result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ID", DbType.Int64, p_ID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Output", DbType.Int64, 0, ParameterDirection.Output));

                _result = DataAccess.ExecuteNonQuery("usp_v4_IQReport_Folder_DeleteFolder", _ListOfDataType);

                return _result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string PasteFolder(string p_CopyID, string p_PasteID, Guid clientGuid)
        {
            try
            {
                string _result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@IDtoCopy", DbType.Int64, p_CopyID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IDtoPaste", DbType.Int64, p_PasteID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));

                _result = DataAccess.ExecuteNonQuery("usp_v4_IQReport_Folder_CopyFolder", _ListOfDataType);

                return _result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<IQReport_FolderModel> GetReportFolderByClientGuid(Guid clientGuid)
        {
            try
            {
                List<IQReport_FolderModel> lstIQ_ReportFolderModel = new List<IQReport_FolderModel>();

                List<DataType> _ListOfDataType = new List<DataType>();

                DataSet _DataSet = new DataSet();

                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));

                DataSet dataset = DataAccess.GetDataSet("usp_v4_IQReport_Folder_SelectFoldersByClientGuid", _ListOfDataType);

                if (dataset.Tables != null && dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dataset.Tables[0].Rows)
                    {
                        IQReport_FolderModel iQ_ReportFolderModel = new IQReport_FolderModel();

                        if (!dr["ID"].Equals(DBNull.Value))
                        {
                            iQ_ReportFolderModel.id = Convert.ToString(dr["ID"]);
                        }

                        if (!dr["Name"].Equals(DBNull.Value))
                        {
                            iQ_ReportFolderModel.text = Convert.ToString(dr["Name"]);
                        }

                        if (!dr["_ParentID"].Equals(DBNull.Value))
                        {
                            iQ_ReportFolderModel.parent = Convert.ToString(dr["_ParentID"]);
                        }

                        lstIQ_ReportFolderModel.Add(iQ_ReportFolderModel);

                    }
                }

                return lstIQ_ReportFolderModel;

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
