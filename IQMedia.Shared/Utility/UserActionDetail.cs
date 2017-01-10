using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Shared.Utility
{

    public static class UserActionList
    {
        public static List<UserActionDetail> UserActions { get; set; }

        static UserActionList()
        {
            UserActions = new List<UserActionDetail> {
                new UserActionDetail{
                Controller = "Dashboard",
                PageName = "Dashboard",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "SummaryReportResults","Get Overall Summary" },
                    { "Index","Load" },
                    { "GetMediumData","Get Medium Chart" },
                    { "GenerateDashboardPDF","Get Medium Summary" },
                    { "SendDashBoardEmail","Send Summary Email" },
                    { "DownloadPDFFile","Download Summary" },
                    { "GetAdhocSummaryData","Get Adhoc Summary" },
                    { "SaveThirdPartyDataTypeSelections", "Save Third Party Data Types" }
                }
                },
                new UserActionDetail{
                Controller = "Discovery",
                PageName = "Discovery",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Load" },
                    { "MediaJsonChart","Load" },
                    { "MediaJsonResults","Search" },
                    { "MoreResult","Load More Results" },
                    { "SaveArticle","Save Article" },
                    { "SaveSearch","Save Search" },
                    { "UpdateSavedSearch","Update Saved Search" },
                    { "LoadSavedSearch","Search from Saved Search" },
                    { "DeleteDiscoverySavedSearchByID","Delete Saved Search" },
                    { "GenerateDiscoveryPDF","Export to PDF" },
                    { "SendEmail","Send Chart Email" },
                    { "DownloadPDFFile","Download Chart PDF File" },
                    { "SelectDiscoveryReport","Get Report List" },
                    { "Insert_DiscoveryReport","Create Report" },
                    { "AddToDiscoveryReport","Add to Report" },
                    { "AddToDiscoveryLibrary","Add to Library" },
                    { "GetProQuestResultByID","Get ProQuest Result" },
                    { "GetTopics","Get Topics" },
                    { "AddToFeeds","Add To Feeds" },
                    { "ExportCSV","Export to CSV" }
                }
                },
                new UserActionDetail{
                Controller = "DiscoveryLite",
                PageName = "Discovery Lite",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Load" },
                    { "MediaJsonChart","Load" },
                    { "GenerateDiscoveryPDF","Export to PDF" },
                    { "DownloadPDFFile","Download Chart PDF File" }
                }
                },
                new UserActionDetail{
                Controller = "Feeds",
                PageName = "Feeds",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Load" },
                    { "_MediaJsonResults","Search" },
                    { "_DeleteMediaResults","Delete Item(s)" },
                    { "_MoreMediaJsonResults","Load More Results" },
                    { "_GetChildResults","Load Child Results" },
                    { "LoadPlayer","Load TV Player" },
                    { "ExportCSV","Export to CSV" },
                    { "DownloadCSVFile","Download Exported CSV" },
                    { "ExcludeDomains","Exclude Domains" },
                    { "Insert_ArchiveData","Save Article" },
                    { "Insert_FeedReport","Create Report" },
                    { "InsertMissingArticle","Insert Missing Article" },
                    { "SelectFeedsReport","Get Report List" },
                    { "AddToFeedsReport","Add to Report" },
                    { "AddToFeedsLibrary","Add to Library" },
                    { "GetDashboardMediaIDs","Get Dashboard Media IDs" },
                    { "GetProQuestResultByID", "Get ProQuest Result" },
                    { "UpdateIsRead", "Mark as Read/Unread" }
                }
                },
                new UserActionDetail{
                Controller = "Timeshift",
                PageName = "Timeshift",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Load" },
                    { "TimeshiftSearchResults","Search TV" },
                    { "TimeshiftSearchResultsPaging","Load More Results" },
                    { "SelectRadioStationResults","Search Radio" },
                    { "SaveSearch","Save Search" },
                    { "UpdateSavedSearch","Update Saved Search" },
                    { "LoadSavedSearch","Search from Saved Search" },
                    { "DeleteSavedSearchByID","Delete Saved Search" },
                }
                },
                new UserActionDetail{
                Controller = "TAds",
                PageName = "TAds",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Load" },
                    { "TAdsSearchResults","Search TV" },
                    { "TAdsSearchResultsPaging","TV Results Change Page" }
                }
                },
                new UserActionDetail{
                Controller = "Library",
                PageName = "Library",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Load" },
                    { "GetIQArchieveResults","Search Library" },
                    { "GetMoreResults","Load More Library Results" },
                    { "FilterCategory","Filter Category" },
                    { "Delete","Delete Library Item(s)" },
                    { "LoadClipPlayer","Load Clip Player" },
                    { "SendEmail","Send Library Item(s) in Email" },
                    { "GetEditRecord","Edit Library Item" },
                    { "UpdateMediaRecord","Update Library Item" },
                    { "RefreshTVResults","Refresh Items" },
                    { "CreateReportFolder","Create Report Folder" },
                    { "MoveReportFolder","Move Report Folder" },
                    { "RenameReportFolder","Rename Report Folder" },
                    { "DeleteReportFolder","Delete Report Folder" },
                    { "InsertLibraryReport","Create Report" },
                    { "ModifyReport","Add to Report" },
                    { "RemoveReportItems","Remove Item(s) from Report" },
                    { "RemoveReportByID","Delete Report" },
                    { "GenerateLibraryReportByID","Load Report" },
                    { "GenerateReportPDF","Export to PDF" },
                    { "DownloadPDFFile","Download Report PDF" },
                    { "GenerateReportCSV","Export to CSV" },
                    { "DownloadCSVFile","Download Report CSV" },
                    { "ReportEmail","Send Report Email" },
                    { "SaveReportWithSettings","Update Report Settings" },
                    { "UGCUploadContent","Upload UGC Media" },
                    { "UGCUploadContentIE","Upload UGC Media" },
                    { "GetUGCUploadFTPContent","Open FTP" },
                    { "MoveUGCFTPFile","Update Report Settings" },
                    { "DisplayIQUGCArchiveResults","Search UGC" },
                    { "DeleteIQUGCArchiveResults","Delete UGC Item(s)" },
                    { "FilterIQUGCArchiveCategory","Filter UGC Category" },
                    { "GetIQUGCArchiveEdit","Edit UGC Item" },
                    { "UpdateIQUGCArchive","Update UGC Item" },
                    { "RefreshIQUGCArchiveResults","Refresh UGC" },
                    { "CheckForUGCFile","Download UGC File" },
                    { "GetArchiveTVEyesLocation","Play Radio" },
                    { "AddIQUGCToLibrary","Add UGC To Library" },
                    { "GetProQuestRecordByID","Get ProQuest Record" },
                    { "AddToMCMedia","Add to MC Media" },
                    { "RemoveFromMCMedia","Remove From MC Media" },
                    { "SearchMCMediaResults", "Search MC Media" },
                    { "GetMoreMCMediaResults", "Load More MC Media Results" },
                    { "FilterMCMediaCategory", "Filter MC Media Category" },
                    { "AddToMCMediaReport", "Add To MC Media Report" },
                    { "RemoveFromMCMediaReport", "Remove From MC Media Report" },
                    { "GetMCMediaReportGUID", "Get MC Media Report GUID" },
                    { "SendMCMediaEmail", "Send MC Media Email" },
                    { "MergeReports", "Merge Reports" },
                    { "SaveReportItemPositions", "Update Report Item Sorting" },
                    { "RevertReportItemPositions", "Revert Report Item Sorting" },
                    { "ReportSort", "Sort Report" },
                    {"ShareClip", "Share Clip"}
                }
                },
                new UserActionDetail{
                Controller = "Group",
                PageName = "Group",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Load or Switch" }
                }
                },
                new UserActionDetail{
                Controller = "Setup",
                PageName = "Setup",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Load" },
                    { "SelectCustomCategories","Load Categories" },
                    { "GetAddEditCustomCategory","Add / Edit Category" },
                    { "SaveCustomCategory","Create / Update Category" },
                    { "DeleteCustomCategory","Delete Category" },
                    { "DisplayIQAgentSetupContent","Load IQAgents" },
                    { "DisplayIQAgentSearchXMLByID","Show IQAgent XML" },
                    { "DeleteIQAgent","Delete IQAgent" },
                    { "GetIQAgentSetupAddEditForm","Add / Edit IQAgent" },
                    { "SubmitIQAgentSetupAddEdit","Create / Update IQAgent" },
                    { "DisplayIQAgentNotificationSettings","Load Notifications" },
                    { "SaveIQAgentNotificationSettings","Create / Update Notification" },
                    { "DeleteIQNotification","Delete Notification" },
                    { "LoadReportFolder","Load Report Folders" },
                    { "RenameReportFolder","Rename Report Folder" },
                    { "CreateReportFolder","Create Report Folder" },
                    { "MoveReportFolder","Move Report Folder" },
                    { "DeleteReportFolder","Delete Report Folder" },
                    { "DisplayCustomSettings","Display Custom Settings" },
                    { "DisplayJobStatusList","Display Job Status List" },
                    { "ResetJob","Reset Job" },
                    {"SuspendAgent","Suspend Agent"},
                    { "GetCustomCategoriesForRanking","Load Categories For Ranking" },
                    { "UpdateCustomCategoryRankings","Update Category Rankings" },
                    { "GetGoogleAnalytics", "Load Google Sign-In" },
                    { "UpdateAuthCode", "Update Google Authentication Code" },
                    { "GetTwitterRule", "View/Edit Agent Twitter Rule" },
                    { "SaveTwitterRule", "Save Agent Twitter Rule" },
                    { "GetTVEyesRule", "View/Edit Agent TVEyes Rule" },
                    { "SaveTVEyesRule", "Save Agent TVEyes Rule" },
                    { "DeleteExternalRules", "Delete Orphaned TVEyes/Twitter/BLPM Rules" }
                }
                },
                new UserActionDetail{
                Controller = "ClientCustomImage",
                PageName = "Setup",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "DisplayReportImages","Load Report Images" },
                    { "DeleteReportImage","Delete Report Images" },
                    { "InsertReportImage","Add Report Image" },
                    { "UpdateIsDefault","Update Report Image as Default Image" },
                    { "UpdateIsDefaultEmail","Update Report Image as Default Email Image" }
                }
                },
                new UserActionDetail{
                Controller = "GlobalAdmin",
                PageName = "Global Admin",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Load" },
                    { "RemoveUserFromCache","Reset User" },
                    { "DisplayClients","Load / Search Clients" },
                    { "GetClientRegistation","Add / Edit Client" },
                    { "ClientRegistration","Crete New / Update Client" },
                    { "DeleteClient","Delete Client" },
                    { "DisplayCustomers","Load / Search Customer" },
                    { "GetCustomerRegistration","Add / Edit Customer" },
                    { "CustomerRegistration","Create New / Update Customer" },
                    { "DeleteCustomer","Delete Customer" },
                    { "RemoveAllUsers", "Reset All Users"},
                    {"GetAllActiveClient", "Gropup Load"},
                    {"GroupAddSubClient", "Group  Add SubClient"},
                    {"GroupRemoveSubClient", "Group Remove SubClient"},
                    {"GroupGetCLCUST", "Group Get Customer of Client"},
                    {"GroupAddSubCustomer", "Group Add SubCustomer"},
                    {"GroupGetSubCustomer", "Group Get SubCustomer"},
                    {"GroupRemoveSubCustomer", "Group Remove SubCustomer"},
                    { "AddClientToAnewstip", "Add Client To Anewstip" },
                    { "AddCustomerToAnewstip", "Add Customer To Anewstip" },
                }
                },
                new UserActionDetail{
                Controller = "FliqCustomer",
                PageName = "Global Admin",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "DisplayFliq_Customers","Load / Search Fliq Customers" },
                    { "GetFliq_CustomerRegistration","Add / Edit Fliq Customer" },
                    { "Fliq_CustomerRegistration","Create New / Update Fliq Customer" },
                    { "DeleteFliq_Customer","Delete Fliq Customer" }
                }
                },
                new UserActionDetail{
                Controller = "SignIn",
                PageName = "Sign In",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Load / Sign In" },
                }
                },
                new UserActionDetail{
                Controller = "BasicPlayer",
                PageName = "Basic IAgent Player",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Load Player" }
                }
                },
                new UserActionDetail{
                Controller = "ClipPlayer",
                PageName = "Clip Player",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Load Player" }
                }
                },
                new UserActionDetail{
                Controller = "ClipPlayer",
                PageName = "Clip Player",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Load Player" }
                }
                },
                new UserActionDetail{
                Controller = "ClipRadioPlayer",
                PageName = "Clip Radio Player",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Load Player" }
                }
                },
                new UserActionDetail{
                Controller = "IFrameMicrosite",
                PageName = "IFrame Microsite",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Load / Search Microsite" },
                    { "DownloadClip","Download Clip" },
                    { "LoadClipPlayer","Load Clip Player" }
                }
                },
                new UserActionDetail{
                Controller = "IframeRawMedia",
                PageName = "Iframe RawMedia",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Load Player" }
                }
                },
                new UserActionDetail{
                Controller = "IframeServiceRawMedia",
                PageName = "Iframe Service RawMedia",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Load Player" }
                }
                },
                new UserActionDetail{
                Controller = "QVC",
                PageName = "QVC",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Load" },
                    { "SignIn","Sign In" },
                    { "QVCSearchResults","Search QVC" },
                    { "QVCSearchResultsPaging","Search QVC" },
                    { "GetChart","Load Chart" }
                }
                },
                new UserActionDetail{
                Controller = "SonyMedia",
                PageName = "Sony Media",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Load" },
                    { "SignIn","Sign In" },
                    { "SonyMediaSearchResults","Search Kantar" },
                    { "SonyMediaSearchResultsPaging","Search Kantar Page" },
                    { "GetChart","Load Chart" }
                }
                },
                new UserActionDetail{
                Controller = "LogOut",
                PageName = "Log Out",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Logout" },
                    { "LogOut","Log Out" }
                }
                },
                new UserActionDetail{
                Controller = "RawMediaPlayer",
                PageName = "RawMedia Player",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Load Player" }
                }
                },
                new UserActionDetail{
                Controller = "RawRadioPlayer",
                PageName = "Radio Player",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Load Player" }
                }
                },
                new UserActionDetail{
                Controller = "Report",
                PageName = "Report",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "IQAgent","Load Report" }
                }
                },
                new UserActionDetail{
                Controller = "Subscription",
                PageName = "Subscription",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Load" }
                }
                },
                new UserActionDetail{
                Controller = "Download",
                PageName = "Library Download",
                IsCheckControllerParameters = true,
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Load" },
                    { "TermsAndCondition","Load / Accept Terms" },
                    { "NM","Load / Refresh News Article Download List" },
                    { "SM","Load / Refresh Social Media Article Download List" },
                    { "DownloadNMFile","Download News Article" },
                    { "DownloadSMFile","Download Social Media Article" },
                    { "TV","Load / Refresh TV Clip Download List" },
                    { "ClipDelete","Remove Clip From Download List" },
                    { "DownloadClip","Delete Clip" },
                    { "TVEyes","Load / Refresh Radio Media Download List" },
                    { "DownloadTVEyesFile","Download Radio Media" },
                    { "DownloadClipCC", "Download Clip CC"}
                }
                },
                new UserActionDetail{
                Controller = "Common",
                PageName = "Common",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "LoadPlayerByGuidnSearchTerm","Load Player" },
                    { "LoadBasicPlayerByGuidnSearchTerm","Load Basic Player" },
                    { "GetMTChart","Load Player MarTech Chart" }
                }
                },
                new UserActionDetail{
                Controller = "DailyDigest",
                PageName = "Setup",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "DisplayClientIQAgentDailyDigests","Display IQAgent Email Digest" },
                    { "GetIQAgentDailyDigestsByID","Edit IQAgent Email Digest" },
                    { "SubmitDailyDigest","Save IQAgent Email Digest" },
                    { "DeleteIQAgentDailyDigest","Delete IQAgent Email Digest" }
                }
                },
                new UserActionDetail{
                Controller = "ImagiQ",
                PageName = "ImagiQ",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "Index","Load" },
                    { "ImagiQSearchResults","Search TV" },
                    { "ImagiQSearchResultsPaging","Load More Results" }
                }
                },
                new UserActionDetail{
                Controller = "MCMediaTemplate",
                PageName = "MCMediaTemplate",
                ActionFriendlyName = new Dictionary<string, string>(){
                    { "MCMediaTemplate1","Load MC Media Report Template 1" },
                    { "MCMediaTemplate2","Load MC Media Report Template 2" },
                    { "MCMediaTemplate3","Load MC Media Report Template 3" },
                    { "MCMediaTemplateDemo","Load MC Media Report Template Demo" },
                    { "MCMediaTemplate2_SearchResults","Load Results for MC Media Report Template 2" },
                    { "MCMediaTemplate3_SearchResults","Load Results for MC Media Report Template 3" },
                    { "MCMediaTemplateDemo_SearchResults","Load Results for MC Media Report Template Demo" },
                    { "MCMediaTemplate2_FilterCategory","Filter Categories for MC Media Report Template 2" },
                    { "MCMediaTemplate3_FilterCategory","Filter Categories for MC Media Report Template 3" }
                }
                },
                new UserActionDetail{
                    Controller = "Analytics",
                    PageName = "Analytics",
                    ActionFriendlyName = new Dictionary<string,string>(){
                        { "Index","Load" },
                        { "GetMainTable", "Load Agent/Campaign Table" },
                        { "GetTabSpecificTable", "Load Tab-Specific Table" },
                        { "GetNetworkShowTabTable", "Load Network/Show Tab Table" },
                        { "GetChart", "Load Chart Data" },
                        { "GetOverlay", "Load Overlay Data" },
                        { "OpenIFrame", "Open Feeds Drilldown" },
                        { "GetActiveElements", "Get Active Element Master List" },
                        { "GeneratePDF", "Create PDF" }
                    }
                },
                new UserActionDetail{
                    Controller = "ContactUs",
                    PageName = "ContactUs",
                    ActionFriendlyName = new Dictionary<string,string>(){
                        { "Index","Load" }
                    }
                },
                new UserActionDetail{
                    Controller = "Connect",
                    PageName = "Connect",
                    ActionFriendlyName = new Dictionary<string,string>(){
                        { "Index","Load" }
                    }
                }
            };
        }
    }

    public class UserActionDetail
    {
        public string Controller { get; set; }
        public string PageName { get; set; }
        public bool IsCheckControllerParameters { get; set; }
        public Dictionary<string, string> ActionFriendlyName { get; set; }
    }
}
