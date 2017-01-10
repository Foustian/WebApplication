using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class ProQuestModel
    {
        public long ID { get; set; }
        public string Title { get; set; }
        public DateTime MediaDate { get; set; }
        public List<string> Authors { get; set; }
        public string Publication { get; set; }
        public string Content { get; set; }
        public string Copyright { get; set; }
    }
}
