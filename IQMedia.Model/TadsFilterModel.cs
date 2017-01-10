using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace IQMedia.Model
{
   public class TadsFilterModel
    {
       public List<TadsDma> TadsDmas { get; set; }
       public List<TadsAffiliate> TadsAffiliates { get; set; }
       public List<TadsCountry> TadsCountries { get; set; }
       public List<TadsRegion> TadsRegions { get; set; }
       public List<TadsStation> TadsStations { get; set; }
       public List<TadsClass> TadsClasses { get; set; }
       public List<TadsIndustry> IndustriesPaid { get; set; }
       public List<TadsIndustry> IndustriesEarned { get; set; }
       public List<TadsIndustry> AllIndustries { get; set; }
       public List<TadsLogo> AllLogos { get; set; }
       public List<TadsLogo> LogosPaid {get;set; }
       public List<TadsLogo> LogosEarned { get; set; }
       public List<TadsBrand> AllBrands { get; set; }
       public List<TadsBrand> BrandsPaid { get; set; }
       public List<TadsBrand> BrandsEarned { get; set; }
       public PaidEarnedContainer TadsPaidEarned { get; set; }
       public List<TadsDma> RadioMarket { get; set; }
       public List<TadsStation> RadioStation { get; set; }
    }

    public class TadsDma 
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public int Counts { get; set; }
        public string CountFormatted
        {
            set { CountFormatted = value; }
            get
            {
                return string.Format("{0:n0}", Counts);
            }
        }
    }
    public class TadsAffiliate
    {
        public string Name { get; set; }
        public int Counts { get; set; }
        public string CountFormatted
        {
            set { CountFormatted = value; }
            get
            {
                return string.Format("{0:n0}", Counts);
            }
        }
    }
    public class TadsCountry
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Counts { get; set; }
        public string CountFormatted
        {
            set { CountFormatted = value; }
            get
            {
                return string.Format("{0:n0}", Counts);
            }
        }
    }
    public class TadsRegion
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Counts { get; set; }
        public string CountFormatted
        {
            set { CountFormatted = value; }
            get
            {
                return string.Format("{0:n0}", Counts);
            }
        }
    }
    public class TadsStation
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int Counts { get; set; }
        public string CountFormatted
        {
            set { CountFormatted = value; }
            get
            {
                return string.Format("{0:n0}", Counts);
            }
        }
    }
    public class TadsClass
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int Counts { get; set; }
        public string CountFormatted
        {
            get{ return string.Format("{0:n0}",Counts);}
            set { CountFormatted = value; }
        }
    }

    public class TadsIndustry
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Counts { get; set; }
        public string CountFormatted
        {
            get { return string.Format("{0:n0}", Counts); }
            set { CountFormatted = value; }
        }
    }

    public class TadsBrand
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Counts { get; set; }
        public string CountFormatted
        {
            get{return string.Format("{0:n0}",Counts);}
            set { CountFormatted = value; }
        }
    }

    public class TadsLogo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Counts { get; set; }
        public int BrandId { get; set; }
        public string URL { get; set; }
        public string CountFormatted
        {
            get { return string.Format("{0:n0}", Counts); }
            set { CountFormatted = value; }
        }
    }

    public class PaidEarnedContainer
    {
        public PaidEarnedObject Paid { get; set; }
        public PaidEarnedObject Earned { get; set; }
    }

    public class PaidEarnedObject
    {
        public int Counts { get; set; }
        public string CountFormatted 
        {
            get { return string.Format("{0:n0}", Counts); }
            set { CountFormatted = value; }
        }
    }
}
