using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Collections.Generic;

namespace IQMedia.Data.Base
{
    internal class DataType
    {
        public string ParameterName { get; set; }

        public DbType DBType { get; set; }

        public object Value { get; set; }

        public ParameterDirection Direction { get; set; }

        public DataType()
        {

        }

        public DataType(string ParameterName, DbType DBType, object Value, ParameterDirection Direction)
        {
            this.ParameterName = ParameterName;
            this.DBType = DBType;
            this.Value = Value;
            this.Direction = Direction;
        }
    }
}