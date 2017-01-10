using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMedia.WebApplication.Config.Sections;
using System.Configuration;

namespace IQMedia.WebApplication.Config
{
    public sealed class ConfigSettings
    {
        private const string MESSAGE_SETTINGS = "MessageSettings";
        private const string SOLRURL_SETTINGS ="SolrUrlSettings";

        public static MessageSettings Settings
        {
            get { return ConfigurationManager.GetSection(MESSAGE_SETTINGS) as MessageSettings; }
        }

        public static SolrUrlSettings SolrSettings
        {
            get { return ConfigurationManager.GetSection(SOLRURL_SETTINGS) as SolrUrlSettings; }
        }
    }
}