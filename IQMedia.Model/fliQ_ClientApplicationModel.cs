using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class fliQ_ClientApplicationModel
    {
        public Int64 ID { get; set; }

        public Guid ClientGUID { get; set; }

        public Int64 ClientID { get; set; }

        public string ClientName { get; set; }

        public Int64 FliqApplicationID { get; set; }

        public string Application { get; set; }

        public string FTPHost { get; set; }

        public string FTPPath { get; set; }

        public string FTPLoginID { get; set; }

        public string FTPPwd { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsLandscapeOnly { get; set; }

        public bool? IsCategoryEnable { get; set; }

        public Guid? DefaultCategory { get; set; }

        public string CategoryName { get; set; }

        public int MaxVideoDuration { get; set; }

        public string ApplicationDescription { get; set; }

        public string DownloadPath { get; set; }
    }

    public class fliQ_ClientApplicationPostModel
    {
        public fliQ_ClientApplicationModel clientApplication { get; set; }
        public ClientApplication_DropDown ClientApplication_DropDown { get; set; }
    }

    public class ClientApplication_DropDown
    {
        public List<CustomCategoryModel> CategoryList { get; set; }
        public List<ClientModel> ClientList { get; set; }
        public List<fliQ_ApplicationModel> ApplicationList { get; set; }
    }

}
