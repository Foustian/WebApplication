using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class EmailResultsModel
    {
        public Guid? ReportGuid { get; set; }

        public string CustomHeader { get; set; }

        public Guid? ClientGuid { get; set; }

        public Guid? MasterClientGuid { get; set; }

        public string MasterClientName { get; set; }

        public bool HasResults { get; set; }

        public List<Email_GroupTier1Model> GroupTier1Results { get; set; }
    }

    public class Email_GroupTier1Model
    {
        public string GroupName { get; set; }

        public int GroupRank { get; set; }

        public bool IsEnabled { get; set; }

        public List<Email_GroupTier2Model> GroupTier2Results { get; set; }
    }

    public class Email_GroupTier2Model
    {
        public string GroupName { get; set; }

        public int GroupRank { get; set; }

        public bool IsEnabled { get; set; }

        public List<Email_GroupTier3Model> GroupTier3Results { get; set; }
    }

    public class Email_GroupTier3Model
    {
        public string GroupName { get; set; }

        public int GroupRank { get; set; }

        public bool IsEnabled { get; set; }

        public List<IQArchive_MediaModel> MediaResults { get; set; }
    }
}
