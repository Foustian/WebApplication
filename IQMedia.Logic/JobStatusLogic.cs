using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using IQMedia.Data;
using IQMedia.Logic.Base;

namespace IQMedia.Web.Logic
{
    public class JobStatusLogic : IQMedia.Web.Logic.Base.ILogic
    {
        public List<JobStatusModel> GetJobStatusByClientGuid(Guid p_ClientGuid, int p_PageNumner, int p_PageSize, bool p_IsAsc, string p_SortColumn, int? p_JobTypeID, out int p_TotalResults)
        {
            JobStatusDA objJobStatusDA = (JobStatusDA)DataAccessFactory.GetDataAccess(DataAccessType.JobStatus);
            return objJobStatusDA.GetJobStatusByClientGuid(p_ClientGuid, p_PageNumner, p_PageSize, p_IsAsc, p_SortColumn,p_JobTypeID,  out p_TotalResults);
        }

        public string SelectJobStatusDownloadByID(long p_ID)
        {
            JobStatusDA objJobStatusDA = (JobStatusDA)DataAccessFactory.GetDataAccess(DataAccessType.JobStatus);
            string result = objJobStatusDA.SelectJobStatusDownload_SelectByID(p_ID);
            return result;
        }

        public List<JobType> GetJobTypeList()
        {
            JobStatusDA objJobStatusDA = (JobStatusDA)DataAccessFactory.GetDataAccess(DataAccessType.JobStatus);
            return objJobStatusDA.GetJobTypeList();
        }

        public bool ResetJob(long ID, long requestID, string resetProcedureName)
        {
            JobStatusDA objJobStatusDA = (JobStatusDA)DataAccessFactory.GetDataAccess(DataAccessType.JobStatus);
            return objJobStatusDA.ResetJob(ID, requestID, resetProcedureName);
        }
    }
}
