using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using IQMedia.Model;

namespace IQMedia.Data.EmailTemplate
{
    public class EmailTemplateDevryDA : EmailTemplateBaseDA, IEmailTemplate
    {
        public EmailResultsModel GetArchiveResultsForEmail(int? masterClientID, Guid? clientGuid, string archiveIDXml, ReportTypeSettings settings, string currentUrl)
        {
            try
            {
                DataSet dataset = ExecuteSP(masterClientID, clientGuid, archiveIDXml, settings.SPName);

                return FillEmailResults(dataset, currentUrl);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private EmailResultsModel FillEmailResults(DataSet dataSet, string currentUrl)
        {
            EmailResultsModel emailResultsModel = new EmailResultsModel();
            List<IQArchive_MediaModel> lstMediaResults = new List<IQArchive_MediaModel>();

            if (dataSet != null)
            {
                foreach (DataTable dt in dataSet.Tables)
                {
                    if (dt.Rows.Count > 0 && dt.Columns.Contains("TableType"))
                    {
                        switch (Convert.ToString(dt.Rows[0]["TableType"]))
                        {
                            case "HeaderInfo":
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dt.Columns.Contains("ReportGuid") && !dr["ReportGuid"].Equals(DBNull.Value))
                                    {
                                        emailResultsModel.ReportGuid = new Guid(Convert.ToString(dataSet.Tables[0].Rows[0]["ReportGuid"]));
                                    }
                                    if (dt.Columns.Contains("HeaderImage") && !dr["HeaderImage"].Equals(DBNull.Value))
                                    {
                                        emailResultsModel.CustomHeader = Convert.ToString(dataSet.Tables[0].Rows[0]["HeaderImage"]);
                                    }
                                    if (dt.Columns.Contains("MasterClientGuid") && !dr["MasterClientGuid"].Equals(DBNull.Value))
                                    {
                                        emailResultsModel.MasterClientGuid = new Guid(Convert.ToString(dataSet.Tables[0].Rows[0]["MasterClientGuid"]));
                                    }
                                    if (dt.Columns.Contains("MasterClientName") && !dr["MasterClientName"].Equals(DBNull.Value))
                                    {
                                        emailResultsModel.MasterClientName = Convert.ToString(dataSet.Tables[0].Rows[0]["MasterClientName"]);
                                    }
                                }
                                break;
                            case "TV":
                                lstMediaResults.AddRange(GetTVResults(dt, currentUrl));
                                break;
                            case "NM":
                                lstMediaResults.AddRange(GetNMResults(dt));
                                break;
                            case "SM":
                                lstMediaResults.AddRange(GetSMResults(dt));
                                break;
                            case "TW":
                                lstMediaResults.AddRange(GetTWResults(dt));
                                break;
                            case "PM":
                                lstMediaResults.AddRange(GetBLPMResults(dt));
                                break;
                            case "PQ":
                                lstMediaResults.AddRange(GetPQResults(dt));
                                break;
                            case "TM":
                                lstMediaResults.AddRange(GetTMResults(dt));
                                break;
                            case "MS":
                                lstMediaResults.AddRange(GetMSResults(dt));
                                break;
                            case "IQR":
                                lstMediaResults.AddRange(GetIQRadioResults(dt, currentUrl));
                                break;
                        }
                    }
                }

                // Group results
                emailResultsModel.GroupTier1Results = new List<Email_GroupTier1Model>();
                foreach (string clientName in lstMediaResults.Select(s => s.ClientName).Distinct().OrderBy(o => o))
                {
                    Email_GroupTier1Model groupTier1Model = new Email_GroupTier1Model()
                    {
                        GroupName = clientName,
                        IsEnabled = true
                    };

                    groupTier1Model.GroupTier2Results = new List<Email_GroupTier2Model>();

                    // Group results by category name, and order the categories by rank
                    foreach (KeyValuePair<string, int> category in lstMediaResults.Where(w => w.ClientName == clientName).Select(s => new KeyValuePair<string, int>(s.CategoryName, s.CategoryRanking)).Distinct().OrderBy(o => o.Value))
                    {
                        Email_GroupTier2Model groupTier2Model = new Email_GroupTier2Model()
                        {
                            GroupName = category.Key,
                            GroupRank = category.Value,
                            IsEnabled = true,
                            GroupTier3Results = new List<Email_GroupTier3Model>()
                            {
                                new Email_GroupTier3Model()
                                {
                                    IsEnabled = false,
                                    MediaResults = lstMediaResults.Where(w => w.ClientName == clientName && w.CategoryName == category.Key)
                                                        .OrderByDescending(o => new DateTime(o.MediaDate.Ticks - o.MediaDate.Ticks % TimeSpan.TicksPerMinute)) // Round to the minute
                                                        .ThenByDescending(o => o.Audience)
                                                        .ThenBy(o => o.MediaType)
                                                        .ToList()
                                }
                            }
                        };

                        groupTier1Model.GroupTier2Results.Add(groupTier2Model);
                    }

                    emailResultsModel.GroupTier1Results.Add(groupTier1Model);
                }
            }

            emailResultsModel.HasResults = lstMediaResults.Count > 0;
            return emailResultsModel;
        }
    }
}
