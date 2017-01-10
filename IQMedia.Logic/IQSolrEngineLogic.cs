using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Model;
using IQMedia.Data;

namespace IQMedia.Web.Logic
{
    public static class IQSolrEngineLogic
    {

        public static List<IQSolrEngineModel> GetSolrEngines()
        {
            return IQSolrEngineDA.GetSolrEngines();
        }
    }
}
