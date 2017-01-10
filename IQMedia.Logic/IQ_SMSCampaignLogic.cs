using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Logic.Base;
using IQMedia.Data;

namespace IQMedia.Web.Logic
{
    public class IQ_SMSCampaignLogic : IQMedia.Web.Logic.Base.ILogic
    {
        public string UpdateIQ_SMSCampaignIsActive(Int64 p_SearchRequestID, Int64 p_HubSpotID, bool p_IsActivated)
        {
            IQ_SMSCampaignDA iQ_SMSCampaignDA = (IQ_SMSCampaignDA)DataAccessFactory.GetDataAccess(DataAccessType.IQ_SMSCampaign);
            return iQ_SMSCampaignDA.UpdateIQ_SMSCampaignIsActive(p_SearchRequestID, p_HubSpotID, p_IsActivated);

        }
    }
}
