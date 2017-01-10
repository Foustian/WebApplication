using System;

namespace IQMedia.Model
{
    [Serializable]
    public class DataImportClientModel
    {
        public long ID { get; set; }
        public string ViewPath { get; set; }
        public string GetResultsMethod { get; set; }
    }
}
