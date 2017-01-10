using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace IQMedia.Model
{
    public class CustomerModel
    {
        /// <summary> 
        /// Represents Primary Key
        /// </summary>
        public int CustomerKey { get; set; }

        /// <summary>
        /// Represents the ID of the corresponding user in the Anewstip system
        /// </summary>
        public string AnewstipUserID { get; set; }

        public Guid CustomerGUID { get; set; }
        /// <summary>
        /// Represents First Name of Customer
        /// </summary>
        public string FirstName { get; set; }

        /// <summary> 
        /// Represents LastName of Customer
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Represents Full Name Of The Customer
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Represents Email of Customer
        /// </summary>
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        public string LoginID { get; set; }

        /// <summary>
        /// Represents Password of Customer
        /// </summary>
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        /// <summary>
        /// Represents Password of Customer
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// Represents ContactNo of Customer
        /// </summary>
        public string ContactNo { get; set; }

        /// <summary>
        /// Represents Comment given by Customer
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Represents CreatedBy
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Represents ModifiedBy
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Represents CreatedDate
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Represents ModifiedDate
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Represents Flag for particular record is active or not.
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Represents Unique Key associated with client
        /// </summary>
        public int ClientID { get; set; }

        public Guid ClientGUID { get; set; }

        /// <summary>
        /// Represents Unique Key associated with client
        /// </summary>
        public long RoleID { get; set; }

        /// <summary>
        /// Represents Client Name Associated with the customer
        /// </summary>
        public string ClientName { get; set; }

        public string MasterClient { get; set; }

        /// <summary>
        ///  Represents Role Name Associated with the customer
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Represents Client Name Associated with the customer
        /// </summary>
        public string CustomHeaderImage { get; set; }

        public bool IsCustomHeader { get; set; }

        public bool? MultiLogin { get; set; }

        public bool IQBasic { get; set; }

        public bool AdvancedSearchAccess { get; set; }

        public bool GlobalAdminAccess { get; set; }

        public bool IQAgentUser { get; set; }

        public bool IQAgentAdminAccess { get; set; }

        public bool IQCustomAccess { get; set; }

        public bool myIQAccess { get; set; }

        public bool IQAgentWebsiteAccess { get; set; }

        public bool DownloadClips { get; set; }

        public string DefaultPage { get; set; }

        public bool UGCDownload { get; set; }

        public bool? IsClientPlayerLogoActive { get; set; }

        public string ClientPlayerLogoImage { get; set; }

        public bool UGCUploadEdit { get; set; }

        public Int16? AuthorizedVersion { get; set; }

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

        public bool IsUGCAutoClip { get; set; }

        public bool IsUGCDownload { get; set; }

        public bool IsUGCUploadEdit { get; set; }

        public bool Isv4Group { get; set; }

        public bool Isv4TV { get; set; }
        public bool Isv4NM { get; set; }
        public bool Isv4SM { get; set; }
        public bool Isv4TW { get; set; }
        public bool Isv4TM { get; set; }
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
        public bool IsSMOther { get; set; }
        public bool IsFB { get; set; }
        public bool IsIG { get; set; }
        public bool IsBL { get; set; }
        public bool IsFO { get; set; }
        public bool IsPR { get; set; }
        public bool IsLN { get; set; }
        public bool IsThirdPartyData { get; set; }
        public bool IsClientSpecificData { get; set; }
        public bool IsConnectAccess { get; set; }
        public bool IsExternalRuleEditor { get; set; }

        public String TimeZone { get; set; }
        public decimal gmt { get; set; }
        public decimal dst { get; set; }

        public Dictionary<string, bool> CustomerRoles { get; set; }

        public Int64? MasterCustomerID { get; set; }
        public int? MCID { get; set; }

        public bool? IsFliqCustomer { get; set; }

        public int PasswordAttempts { get; set; }

    }

    public class CustomerPostModel
    {
        public CustomerModel customer { get; set; }
        public string[] chkRoles { get; set; }
        public Customer_DropDown Customer_DropDown { get; set; }
    }

    public class Customer_DropDown
    {
        public List<RoleModel> Customer_RoleList { get; set; }
        public List<CustomerModel> Customer_MasterList { get; set; }
        public List<ClientModel> ClientList { get; set; }
    }
}
