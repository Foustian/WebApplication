using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Web.Logic.Base;
using IQMedia.Logic.Base;
using IQMedia.Data;
using IQMedia.Model;

namespace IQMedia.Web.Logic
{
    public class GoogleLogic : ILogic
    {
        public void UpdateAuthCode(Guid clientGuid, string authCode)
        {
            GoogleDA googleDA = (GoogleDA)DataAccessFactory.GetDataAccess(DataAccessType.Google);
            googleDA.UpdateAuthCode(clientGuid, authCode);
        }

        public bool CheckClientAccess(Guid clientGuid)
        {
            GoogleDA googleDA = (GoogleDA)DataAccessFactory.GetDataAccess(DataAccessType.Google);
            return googleDA.CheckClientAccess(clientGuid);
        }

        public string GetClientID()
        {
            GoogleDA googleDA = (GoogleDA)DataAccessFactory.GetDataAccess(DataAccessType.Google);
            return googleDA.GetClientID();
        }

        public List<GoogleSummaryModel> GetGoogleDataByHour(Guid clientGuid, DateTime fromDate, DateTime toDate)
        {
            GoogleDA googleDA = new GoogleDA();
            return googleDA.GetGoogleDataByHour(clientGuid, fromDate, toDate);
        }

        public List<GoogleSummaryModel> GetGoogleDataByDay(Guid clientGuid, DateTime fromDate, DateTime toDate)
        {
            GoogleDA googleDA = new GoogleDA();
            return googleDA.GetGoogleDataByDay(clientGuid, fromDate, toDate);
        }

        public List<GoogleSummaryModel> GetGoogleDataByMonth(Guid clientGuid, DateTime fromDate, DateTime toDate)
        {
            GoogleDA googleDA = new GoogleDA();
            return googleDA.GetGoogleDataByMonth(clientGuid, fromDate, toDate);
        }
    }
}
