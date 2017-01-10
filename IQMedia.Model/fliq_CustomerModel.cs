using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public class fliq_CustomerModel
    {
        /// <summary> 
        /// Represents Primary Key
        /// </summary>
        public int CustomerKey { get; set; }

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

        public string LoginID { get; set; }

        /// <summary>
        /// Represents Password of Customer
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Represents ContactNo of Customer
        /// </summary>
        public string ContactNo { get; set; }

        /// <summary>
        /// Represents Comment given by Customer
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Represents Flag for particular record is active or not.
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Represents Unique Key associated with client
        /// </summary>
        public int ClientID { get; set; }

        public Guid ClientGUID { get; set; }

        /// Represents CreatedBy
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Represents ModifiedDate
        /// </summary>
        public DateTime? ModifiedDate { get; set; }
    }

    public class fliq_CustomerPostModel
    {
        public fliq_CustomerModel customer { get; set; }
        public Customer_DropDown Customer_DropDown { get; set; }
    }
}
