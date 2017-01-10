using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Web.Logic.Base;
using IQMedia.Model;
using IQMedia.Data;
using IQMedia.Logic.Base;

namespace IQMedia.Web.Logic
{
    public class IQClient_CustomImageLogic : ILogic
    {
        public string InsertIQClient_CustomImage(IQClient_CustomImageModel p_IQClient_CustomImageModel, bool p_IsReplace)
        {
            IQClient_CustomImageDA iQClient_CustomImageDA = (IQClient_CustomImageDA)DataAccessFactory.GetDataAccess(DataAccessType.IQClient_CustomImage);
            return iQClient_CustomImageDA.InsertIQClient_CustomImage(p_IQClient_CustomImageModel, p_IsReplace);

        }

        public string DeleteIQClient_CustomImage(Int64 p_ID, Guid p_ClientGuid)
        {
            IQClient_CustomImageDA iQClient_CustomImageDA = (IQClient_CustomImageDA)DataAccessFactory.GetDataAccess(DataAccessType.IQClient_CustomImage);
            return iQClient_CustomImageDA.DeleteIQClient_CustomImage(p_ID, p_ClientGuid);
        }

        public List<IQClient_CustomImageModel> GetAllIQClient_CustomImageByClientGuid(Guid p_ClientGuid)
        {
            IQClient_CustomImageDA iQClient_CustomImageDA = (IQClient_CustomImageDA)DataAccessFactory.GetDataAccess(DataAccessType.IQClient_CustomImage);
            return iQClient_CustomImageDA.GetAllIQClient_CustomImageByClientGuid(p_ClientGuid);
        }

        public int? CheckForImageCopy(string p_Image, Guid p_ClientGuid)
        {
            IQClient_CustomImageDA iQClient_CustomImageDA = (IQClient_CustomImageDA)DataAccessFactory.GetDataAccess(DataAccessType.IQClient_CustomImage);
            return iQClient_CustomImageDA.CheckForImageCopy(p_Image, p_ClientGuid);
        }

        public string UpdateIsDefaultEmailIQClient_CustomImage(Int64 p_ID, Guid p_ClientGuid)
        {
            IQClient_CustomImageDA iQClient_CustomImageDA = (IQClient_CustomImageDA)DataAccessFactory.GetDataAccess(DataAccessType.IQClient_CustomImage);
            return iQClient_CustomImageDA.UpdateIsDefaultEmailIQClient_CustomImage(p_ID, p_ClientGuid);
        }

        public string UpdateIsDefaultIQClient_CustomImage(Int64 p_ID, Guid p_ClientGuid)
        {
            IQClient_CustomImageDA iQClient_CustomImageDA = (IQClient_CustomImageDA)DataAccessFactory.GetDataAccess(DataAccessType.IQClient_CustomImage);
            return iQClient_CustomImageDA.UpdateIsDefaultIQClient_CustomImage(p_ID, p_ClientGuid);
        }
    }
}
