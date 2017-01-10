using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using IQMedia.Model;
using System.Data;
using System.Xml.Linq;

namespace IQMedia.Data
{
    public class PQDA : IDataAccess
    {
        public string InsertArchivePQ(IQAgent_PQResultsModel iQAgent_PQResultsModel, Guid customerGUID, Guid clientGUID, Guid categoryGUID, string p_Keywords, string p_Description, string p_MediaType, string p_SubMediaType, Int64? mediaID)
        {
            try
            {
                Int32 archiveKey = 0;
                string authorsXml = String.Empty;
                if (iQAgent_PQResultsModel.Authors != null && iQAgent_PQResultsModel.Authors.Count > 0)
                {
                    XDocument xdoc = new XDocument(new XElement("authors", from ele in iQAgent_PQResultsModel.Authors select new XElement("author", ele)));
                    authorsXml = xdoc.ToString();
                }

                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ProQuestID", DbType.String, iQAgent_PQResultsModel.ArticleID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Title", DbType.String, iQAgent_PQResultsModel.Title, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Publication", DbType.String, iQAgent_PQResultsModel.Publication, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Author", DbType.Xml, authorsXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Content", DbType.String, iQAgent_PQResultsModel.Content, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ContentHTML", DbType.String, iQAgent_PQResultsModel.ContentHTML, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@HighlightingText", DbType.String, iQAgent_PQResultsModel.HighlightedPQOutput != null ? Shared.Utility.CommonFunctions.SerializeToXml(iQAgent_PQResultsModel.HighlightedPQOutput) : null, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@AvailableDate", DbType.Date, iQAgent_PQResultsModel.AvailableDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MediaDate", DbType.Date, iQAgent_PQResultsModel.MediaDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@LanguageNum", DbType.Int16, iQAgent_PQResultsModel.LanguageNum, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MediaCategory", DbType.String, iQAgent_PQResultsModel.MediaCategory, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Copyright", DbType.String, iQAgent_PQResultsModel.Copyright, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, customerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, clientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryGuid", DbType.Guid, categoryGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PositiveSentiment", DbType.Int32, iQAgent_PQResultsModel.PositiveSentiment, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@NegativeSentiment", DbType.Int32, iQAgent_PQResultsModel.NegativeSentiment, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MediaID", DbType.Int64, mediaID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchTerm", DbType.String, iQAgent_PQResultsModel.SearchTerm, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Keywords", DbType.String, p_Keywords, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Description", DbType.String, p_Description, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MediaType", DbType.String, p_MediaType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubMediaType", DbType.String, p_SubMediaType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ArchiveKey", DbType.Int32, archiveKey, ParameterDirection.Output));

                string _Result = DataAccess.ExecuteNonQuery("usp_v5_ArchivePQ_Insert", dataTypeList);
                return _Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
