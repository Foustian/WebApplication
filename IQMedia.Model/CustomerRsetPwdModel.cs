using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class CustomerRsetPwdModel
    {
        public Int64 ID { get; set; }

        public Guid CustomerGUID { get; set; }

        public string Token { get; set; }

        public DateTime DateExpired { get; set; }

        public bool IsActive { get; set; }

        public bool IsUsed { get; set; }
    }
}
