using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQCommon.Model;

namespace IQMedia.Model
{
    [Serializable]
    public class ActiveUser
    {
        public bool IsLogIn { get; set; }
        public bool? IsUgcAutoClip { get; set; }
        public int ClientID { get; set; }
        public int CustomerKey { get; set; }
        public string ClientName { get; set; }
        public Guid ClientGUID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public Guid CustomerGUID { get; set; }
        public bool? MultiLogin { get; set; }
        public bool? IsClientPlayerLogoActive { get; set; }
        public string ClientPlayerLogoImage { get; set; }

        public Int64 MaxTVResultID { get; set; }

        public Int16? AuthorizedVersion { get; set; }

        public string DefaultPage { get; set; }

        public bool Isv4FeedsAccess { get; set; }

        public bool Isv4DiscoveryAccess { get; set; }

        public bool Isv4LibraryAccess { get; set; }

        public bool Isv4TimeshiftAccess { get; set; }

        public bool Isv4TAdsAccess { get; set; }

        public bool Isv5AdsAccess { get; set; }

        public bool Isv5AnalyticsAccess { get; set; }

        public bool Isv4DashboardAccess { get; set; }

        public bool Isv4LibraryDashboardAccess { get; set; }

        public bool Isv4TimeshiftRadioAccess { get; set; }

        public bool Isv4SetupAccess { get; set; }

        public bool isv5LRAccess { get; set; }

        public bool IsGlobalAdminAccess { get; set; }

        public bool Isv4UGCAccess { get; set; }

        public bool Isv4IQAgentAccess { get; set; }

        public bool Isv4UGCUploadEdit { get; set; }

        public bool Isv4UGCDownload { get; set; }

        public bool Isv4Group { get; set; }

        public bool Isv4TV { get; set; }
        public bool Isv4NM { get; set; }
        public bool Isv4SM { get; set; }
        public bool Isv4TW { get; set; }
        public bool Isv4TM { get; set; }
        public bool IsSMOther { get; set; }
        public bool IsFB { get; set; }
        public bool IsIG { get; set; }
        public bool IsBL { get; set; }
        public bool IsFO { get; set; }
        public bool IsPR { get; set; }
        public bool IsLN { get; set; }
        public bool Isv4CustomImage { get; set; }
        public bool IsNielsenData { get; set; }
        public bool IsCompeteData { get; set; }
        public bool Isv4BLPM { get; set; }
        public bool IsNewsRights { get; set; }
        public bool Isv4CustomSettings { get; set; }
        public bool Isv4DiscoveryLiteAccess { get; set; }
        public bool IsfliQAdmin { get; set; }
        public bool Isv4PQ { get; set; }
        public bool IsMediaRoomContributor { get; set; }
        public bool IsMediaRoomEditor { get; set; }
        public bool Isv4Google { get; set; }
        public bool IsTimeshiftFacet { get; set; }
        public bool IsShareTV { get; set; }
        public bool IsThirdPartyData { get; set; }
        public bool IsClientSpecificData { get; set; }
        public bool IsConnectAccess { get; set; }
        public bool IsExternalRuleEditor { get; set; }

        public String TimeZone { get; set; }
        public decimal gmt { get; set; }
        public decimal dst { get; set; }

        public int? MCID { get; set; }
        public Int64 MasterCustomerID { get; set; }
        public string LoginID { get; set; }
        public string SessionID { get; set; }
        public DateTime SessionTimeOut { get; set; }
        public DateTime LastAccessTime { get; set; }
        public string Server { get; set; }

        public List<IQ_MediaTypeModel> MediaTypes { get; set; }
    }
}

