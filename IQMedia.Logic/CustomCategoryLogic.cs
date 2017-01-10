using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using IQMedia.Data;
using IQMedia.Logic.Base;
using System.Xml.Linq;

namespace IQMedia.Web.Logic
{
    public class CustomCategoryLogic : IQMedia.Web.Logic.Base.ILogic
    {
        public IEnumerable<CustomCategoryModel> GetCustomCategory(Guid clientGUID)
        {
            CustomCategoryDA categoryDA = (CustomCategoryDA)DataAccessFactory.GetDataAccess(DataAccessType.Category);
            IEnumerable<CustomCategoryModel> customCategoryModelList = categoryDA.GetCustomCategory(clientGUID);
            return customCategoryModelList;
        }

        public IEnumerable<CustomCategoryModel> GetCustomCategoryByClientID(Int64 p_ClientID)
        {
            CustomCategoryDA categoryDA = (CustomCategoryDA)DataAccessFactory.GetDataAccess(DataAccessType.Category);
            IEnumerable<CustomCategoryModel> customCategoryModelList = categoryDA.GetCustomCategoryByClientID(p_ClientID);
            return customCategoryModelList;
        }

        public List<CustomCategoryModel> SelectCustomCategoryForSetup(Guid clientGUID)
        {
            CustomCategoryDA categoryDA = (CustomCategoryDA)DataAccessFactory.GetDataAccess(DataAccessType.Category);
            return categoryDA.SelectCustomCategoryForSetup(clientGUID);
        }

        public string InsertCustomCategoryForSetup(CustomCategoryModel p_customerCategory)
        {
            CustomCategoryDA categoryDA = (CustomCategoryDA)DataAccessFactory.GetDataAccess(DataAccessType.Category);
            return categoryDA.InsertCustomCategoryForSetup(p_customerCategory);
        }

        public string UpdateCustomCategoryForSetup(CustomCategoryModel p_customerCategory)
        {
            CustomCategoryDA categoryDA = (CustomCategoryDA)DataAccessFactory.GetDataAccess(DataAccessType.Category);
            return categoryDA.UpdateCustomCategoryForSetup(p_customerCategory);
        }

        public CustomCategoryModel SelectCustomCategoryByCategoryKeyForSetup(Guid clientGUID, long CustomCategoryKey)
        {
            CustomCategoryDA categoryDA = (CustomCategoryDA)DataAccessFactory.GetDataAccess(DataAccessType.Category);
            return categoryDA.SelectCustomCategoryByCategoryKeyForSetup(clientGUID, CustomCategoryKey);
        }

        public string DeleteCustomCategoryForSetup(Guid clientGUID, long CustomCategoryKey)
        {
            CustomCategoryDA categoryDA = (CustomCategoryDA)DataAccessFactory.GetDataAccess(DataAccessType.Category);
            return categoryDA.DeleteCustomCategoryForSetup(clientGUID, CustomCategoryKey);
        }

        public int UpdateCustomCategoryRankings(List<string> categoryGUIDs)
        {
            XDocument xdoc = new XDocument(new XElement("list",
                                         from ele in categoryGUIDs
                                         select new XElement("item", new XAttribute("guid", ele), new XAttribute("ranking", categoryGUIDs.IndexOf(ele)))
                                                 ));

            CustomCategoryDA categoryDA = (CustomCategoryDA)DataAccessFactory.GetDataAccess(DataAccessType.Category);
            return categoryDA.UpdateCustomCategoryRankings(xdoc.ToString());
        }
    }
}
