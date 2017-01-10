using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using System.Data;
using IQMedia.Model;

namespace IQMedia.Data
{
    public class TWDA : IDataAccess
    {
        public string InsertArchiveTW(IQAgent_TwitterResultsModel p_IQAgent_TwitterResultsModel, Guid p_CustomerGUID, Guid p_ClientGUID, Guid p_CategoryGUID, string p_Keywords, string p_Description, string p_MediaType, string p_SubMediaType, Int64? MediaID)
        {
            try
            {
                Int32 archiveKey = 0;
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@CustomerGuid", DbType.Guid, p_CustomerGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Actor_PreferredUserName", DbType.String, p_IQAgent_TwitterResultsModel.Actor_PreferredName, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Tweet_Body", DbType.String, p_IQAgent_TwitterResultsModel.Summary, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Actor_FollowersCount", DbType.Int32, p_IQAgent_TwitterResultsModel.Actor_FollowersCount, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Actor_FriendsCount", DbType.Int32, p_IQAgent_TwitterResultsModel.Actor_FriendsCount, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Actor_Image", DbType.String, p_IQAgent_TwitterResultsModel.Actor_Image, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Actor_link", DbType.String, p_IQAgent_TwitterResultsModel.Actor_Link, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@gnip_Klout_Score", DbType.Int32, p_IQAgent_TwitterResultsModel.KlOutScore, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Tweet_PostedDateTime", DbType.DateTime, p_IQAgent_TwitterResultsModel.Tweet_DateTime, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Tweet_ID", DbType.String, p_IQAgent_TwitterResultsModel.TweetID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Actor_DisplayName", DbType.String, p_IQAgent_TwitterResultsModel.Actor_DisplayName, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@CategoryGuid", DbType.Guid, p_CategoryGUID, ParameterDirection.Input));                
                dataTypeList.Add(new DataType("@PositiveSentiment", DbType.Int32, p_IQAgent_TwitterResultsModel.PositiveSentiment, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@NegativeSentiment", DbType.Int32, p_IQAgent_TwitterResultsModel.NegativeSentiment, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MediaID", DbType.Int64, MediaID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@HighLightText", DbType.String, p_IQAgent_TwitterResultsModel.HighlightedOutput != null ?  Shared.Utility.CommonFunctions.SerializeToXml(p_IQAgent_TwitterResultsModel.HighlightedOutput) : null, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchTerm", DbType.String, p_IQAgent_TwitterResultsModel.SearchTerm, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Number_Hits", DbType.Int32, p_IQAgent_TwitterResultsModel.Number_Hits, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Keywords", DbType.String, p_Keywords, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Description", DbType.String, p_Description, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@MediaType", DbType.String, p_MediaType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SubMediaType", DbType.String, p_SubMediaType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ArchiveKey", DbType.Int32, archiveKey, ParameterDirection.Output));

                string _Result = DataAccess.ExecuteNonQuery("usp_v5_ArchiveTW_Insert", dataTypeList);
                return _Result;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
