using IQMedia.Data.Base;
using System.Collections.Generic;
using IQMedia.Model;
using System.Data;
using System;

namespace IQMedia.Data
{
    public class UGCDA : IDataAccess
    {
        public string InsertUGCUploadLog(UGCUploadLogModel p_UGCUploadLogModel)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.Guid, p_UGCUploadLogModel.CustomerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@UGCContentXml", DbType.Xml, p_UGCUploadLogModel.UGCXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FileName", DbType.String, p_UGCUploadLogModel.FileName, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@UploadedDateTime", DbType.DateTime, p_UGCUploadLogModel.UploadedDateTime, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Output", DbType.Int64, 0, ParameterDirection.Output));

                string result = DataAccess.ExecuteNonQuery("usp_v4_UGC_Upload_Log_Insert", dataTypeList);

                return result;
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int InsertArchiveMS(string mediaIDXml, Guid clientGUID, Guid customerGUID)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@MediaIDs", DbType.Xml, mediaIDXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, clientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGUID", DbType.Guid, customerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Output", DbType.Int32, 0, ParameterDirection.Output));

                return Convert.ToInt32(DataAccess.ExecuteNonQuery("usp_v4_ArchiveMS_Insert", dataTypeList));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
