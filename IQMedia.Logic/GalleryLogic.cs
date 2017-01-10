using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data;
using IQMedia.Logic.Base;
using IQMedia.Model;

namespace IQMedia.Web.Logic
{
    public class GalleryLogic : IQMedia.Web.Logic.Base.ILogic
    {
        public string InsertGallery(Guid p_CustomerGUID, IQGalleryModel p_IQGalleryModel)
        {
            GalleryDA gallaryDA = (GalleryDA)DataAccessFactory.GetDataAccess(DataAccessType.Gallery);
            return gallaryDA.InsertGallery(p_CustomerGUID, p_IQGalleryModel);
        }

        public string UpdateGallery(Int64 p_ID, Guid p_CustomerGUID, IQGalleryModel p_IQGalleryModel)
        {
            GalleryDA gallaryDA = (GalleryDA)DataAccessFactory.GetDataAccess(DataAccessType.Gallery);
            return gallaryDA.UpdateGallery(p_ID, p_CustomerGUID, p_IQGalleryModel);
        }

        public IQGalleryResult GetGalleryListByCustomerGUID(Guid p_CustomerGUID)
        {
            GalleryDA gallaryDA = (GalleryDA)DataAccessFactory.GetDataAccess(DataAccessType.Gallery);
            return gallaryDA.GetGalleryListByCustomerGUID(p_CustomerGUID);
        }

        public IQGalleryModel GetGalleryByID(Int64 p_ID, Guid p_CustomerGUID)
        {
            GalleryDA gallaryDA = (GalleryDA)DataAccessFactory.GetDataAccess(DataAccessType.Gallery);

            IQGalleryModel _IQGalleryModel = gallaryDA.GetGalleryByID(p_ID, p_CustomerGUID);

            string _MediaIDs = string.Empty;
            foreach (IQGallery _IQGallery in _IQGalleryModel.Json)
            {
                if (_IQGallery.ID > 0)
                {
                    if (!string.IsNullOrEmpty(_MediaIDs))
                    {
                        _MediaIDs = _MediaIDs + "," + _IQGallery.ID;
                    }
                    else
                    {
                        _MediaIDs = Convert.ToString(_IQGallery.ID);
                    }
                }
            }
            List<IQGallery> lstIQGallery = new List<IQGallery>();

            lstIQGallery = gallaryDA.GetClipIDByMediaIDs(_MediaIDs);

            string TVThumbUrl = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["IQArchieve_TVThumbUrl"]);
            foreach (IQGallery _IQGallery in _IQGalleryModel.Json)
            {
                if (_IQGallery.ID > 0)
                {
                    _IQGallery.ClipID = lstIQGallery.Where(g => g.ID == _IQGallery.ID).FirstOrDefault().ClipID;
                    _IQGallery.TVThumbUrl = TVThumbUrl + _IQGallery.ClipID;
                }
            }

            return _IQGalleryModel;
        }

        public List<IQGallery> GetClipIDByMediaIDs(string p_IDs)
        {
            GalleryDA gallaryDA = (GalleryDA)DataAccessFactory.GetDataAccess(DataAccessType.Gallery);
            return gallaryDA.GetClipIDByMediaIDs(p_IDs);
        }

        public List<IQGalleryItemType> GetGalleryItemTypeList()
        {
            GalleryDA gallaryDA = (GalleryDA)DataAccessFactory.GetDataAccess(DataAccessType.Gallery);
            return gallaryDA.GetGalleryItemTypeList();
        }
    }
}
