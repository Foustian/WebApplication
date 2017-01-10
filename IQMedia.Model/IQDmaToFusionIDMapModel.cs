using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Model
{
    public static class IQDmaToFusionIDMapModel
    {
        public static Dictionary<string, Int16> IQDmaToFusionIDMap { get; set; }

        static IQDmaToFusionIDMapModel()
        {
            IQDmaToFusionIDMap = new Dictionary<string, short>();
            IQDmaToFusionIDMap.Add("Abilene-Sweetwater", 662);
            IQDmaToFusionIDMap.Add("Albany, GA", 525);
            IQDmaToFusionIDMap.Add("Albany-Schenectay-Troy", 790);
            IQDmaToFusionIDMap.Add("Albuquerque-Santa Fe", 644);
            IQDmaToFusionIDMap.Add("Alexandria, LA", 583);
            IQDmaToFusionIDMap.Add("Alpena", 634);
            IQDmaToFusionIDMap.Add("Amarillo", 743);
            IQDmaToFusionIDMap.Add("Anchorage", 524);
            IQDmaToFusionIDMap.Add("Atlanta", 500);
            IQDmaToFusionIDMap.Add("Augusta-Aiken", 520);
            IQDmaToFusionIDMap.Add("Austin", 635);
            IQDmaToFusionIDMap.Add("Bakersfield", 800);
            IQDmaToFusionIDMap.Add("Baltimore", 512);
            IQDmaToFusionIDMap.Add("Bangor", 537);
            IQDmaToFusionIDMap.Add("Baton Rouge", 716);
            IQDmaToFusionIDMap.Add("Beaumont-Port Arthur", 692);
            IQDmaToFusionIDMap.Add("Bend, OR", 821);
            IQDmaToFusionIDMap.Add("Billings", 756);
            IQDmaToFusionIDMap.Add("Biloxi-Gulfport", 746);
            IQDmaToFusionIDMap.Add("Binghamton", 502);
            IQDmaToFusionIDMap.Add("Birmingham (Ann and Tusc)", 630);
            IQDmaToFusionIDMap.Add("Bluefield-Beckley-Oak Hill", 687);
            IQDmaToFusionIDMap.Add("Boise", 559);
            IQDmaToFusionIDMap.Add("Boston (Manchester)", 757);
            IQDmaToFusionIDMap.Add("Bowling Green", 506);
            IQDmaToFusionIDMap.Add("Buffalo", 736);
            IQDmaToFusionIDMap.Add("Burlington-Plattsburgh", 514);
            IQDmaToFusionIDMap.Add("Butte-Bozeman", 523);
            IQDmaToFusionIDMap.Add("Casper-Riverton", 754);
            IQDmaToFusionIDMap.Add("Cedar Rapids-Wtrlo-IWC&Dub", 767);
            IQDmaToFusionIDMap.Add("Champaign&Sprngfld-Decatur", 637);
            IQDmaToFusionIDMap.Add("Charleston, SC", 648);
            IQDmaToFusionIDMap.Add("Charleston-Huntington", 519);
            IQDmaToFusionIDMap.Add("Charlotte", 517);
            IQDmaToFusionIDMap.Add("Charlottesville", 584);
            IQDmaToFusionIDMap.Add("Chattanooga", 575);
            IQDmaToFusionIDMap.Add("Cheyenne-Scottsbluf", 759);
            IQDmaToFusionIDMap.Add("Chicago", 602);
            IQDmaToFusionIDMap.Add("Chico-Redding", 868);
            IQDmaToFusionIDMap.Add("Cincinnati", 515);
            IQDmaToFusionIDMap.Add("Clarksburg-Weston", 598);
            IQDmaToFusionIDMap.Add("Cleveland-Akron (Canton)", 510);
            IQDmaToFusionIDMap.Add("Colorado Springs-Pueblo", 752);
            IQDmaToFusionIDMap.Add("Columbia, SC", 546);
            IQDmaToFusionIDMap.Add("Columbia-Jefferson City", 522);
            IQDmaToFusionIDMap.Add("Columbus, GA (Opelika, AL)", 535);
            IQDmaToFusionIDMap.Add("Columbus, OH", 600);
            IQDmaToFusionIDMap.Add("Columbus-Tupelo-W Pnt-Hstn", 623);
            IQDmaToFusionIDMap.Add("Corpus Christi", 682);
            IQDmaToFusionIDMap.Add("Dallas-Ft. Worth", 542);
            IQDmaToFusionIDMap.Add("Davenport-R.Island-Moline", 691);
            IQDmaToFusionIDMap.Add("Dayton", 751);
            IQDmaToFusionIDMap.Add("Denver", 679);
            IQDmaToFusionIDMap.Add("Des Moines-Ames", 505);
            IQDmaToFusionIDMap.Add("Detroit", 606);
            IQDmaToFusionIDMap.Add("Dothan", 676);
            IQDmaToFusionIDMap.Add("Duluth-Superior", 765);
            IQDmaToFusionIDMap.Add("El Paso (Las Cruces)", 565);
            IQDmaToFusionIDMap.Add("Elmira (Corning)", 516);
            IQDmaToFusionIDMap.Add("Erie", 801);
            IQDmaToFusionIDMap.Add("Eugene", 802);
            IQDmaToFusionIDMap.Add("EUREKA", 649);
            IQDmaToFusionIDMap.Add("Evansville", 745);
            IQDmaToFusionIDMap.Add("Fairbanks", 724);
            IQDmaToFusionIDMap.Add("Fargo-Valley City", 513);
            IQDmaToFusionIDMap.Add("Flint-Saginaw-Bay City", 570);
            IQDmaToFusionIDMap.Add("Fresno-Visalia", 670);
            IQDmaToFusionIDMap.Add("Ft. Myers-Naples", 509);
            IQDmaToFusionIDMap.Add("Ft. Smith-Fay-Sprngdl-Rgrs", 866);
            IQDmaToFusionIDMap.Add("Ft. Wayne", 592);
            IQDmaToFusionIDMap.Add("Gainesville", 798);
            IQDmaToFusionIDMap.Add("Glendive", 773);
            IQDmaToFusionIDMap.Add("Grand Junction-Montrose", 563);
            IQDmaToFusionIDMap.Add("Grand Rapids-Kalmzoo-B.Crk", 755);
            IQDmaToFusionIDMap.Add("Great Falls", 658);
            IQDmaToFusionIDMap.Add("Green Bay-Appleton", 518);
            IQDmaToFusionIDMap.Add("Greensboro-H.Point-W.Salem", 545);
            IQDmaToFusionIDMap.Add("Greenville-N.Bern-Washngtn", 647);
            IQDmaToFusionIDMap.Add("GREENVLL-SPART-ASHEVLL-AND", 636);
            IQDmaToFusionIDMap.Add("Greenwood-Greenville", 566);
            IQDmaToFusionIDMap.Add("Harlingen-Wslco-Brnsvl-McA", 533);
            IQDmaToFusionIDMap.Add("Harrisburg-Lncstr-Leb-York", 710);
            IQDmaToFusionIDMap.Add("Harrisonburg", 766);
            IQDmaToFusionIDMap.Add("Hartford & New Haven", 744);
            IQDmaToFusionIDMap.Add("Hattiesburg-Laurel", 618);
            IQDmaToFusionIDMap.Add("Helena", 564);
            IQDmaToFusionIDMap.Add("Honolulu", 678);
            IQDmaToFusionIDMap.Add("Houston", 758);
            IQDmaToFusionIDMap.Add("Huntsville-Decatur (Flor)", 527);
            IQDmaToFusionIDMap.Add("Idaho Fals-Pocatllo(Jcksn)", 639);
            IQDmaToFusionIDMap.Add("Indianapolis", 718);
            IQDmaToFusionIDMap.Add("Jackson, MS", 561);
            IQDmaToFusionIDMap.Add("JACKSON, TN", 604);
            IQDmaToFusionIDMap.Add("Jacksonville", 574);
            IQDmaToFusionIDMap.Add("Johnstown-Altoona-St Colge", 734);
            IQDmaToFusionIDMap.Add("Jonesboro", 603);
            IQDmaToFusionIDMap.Add("Joplin-Pittsburg", 747);
            IQDmaToFusionIDMap.Add("Juneau", 616);
            IQDmaToFusionIDMap.Add("Kansas City", 557);
            IQDmaToFusionIDMap.Add("KNOXVILLE", 702);
            IQDmaToFusionIDMap.Add("La Crosse-Eau Claire", 582);
            IQDmaToFusionIDMap.Add("Lafayette, IN", 642);
            IQDmaToFusionIDMap.Add("Lafayette, LA", 643);
            IQDmaToFusionIDMap.Add("Lake Charles", 569);
            IQDmaToFusionIDMap.Add("Lansing", 551);
            IQDmaToFusionIDMap.Add("Laredo", 749);
            IQDmaToFusionIDMap.Add("Las Vegas", 839);
            IQDmaToFusionIDMap.Add("Lexington", 541);
            IQDmaToFusionIDMap.Add("Lima", 558);
            IQDmaToFusionIDMap.Add("Lincoln & Hastings-Krny", 722);
            IQDmaToFusionIDMap.Add("Little Rock-Pine Bluff", 693);
            IQDmaToFusionIDMap.Add("Los Angeles", 803);
            IQDmaToFusionIDMap.Add("Louisville", 529);
            IQDmaToFusionIDMap.Add("Lubbock", 651);
            IQDmaToFusionIDMap.Add("Macon", 573);
            IQDmaToFusionIDMap.Add("Madison", 503);
            IQDmaToFusionIDMap.Add("Mankato", 669);
            IQDmaToFusionIDMap.Add("Marquette", 737);
            IQDmaToFusionIDMap.Add("Medford-Klamath Falls", 553);
            IQDmaToFusionIDMap.Add("Memphis", 611);
            IQDmaToFusionIDMap.Add("Meridian", 813);
            IQDmaToFusionIDMap.Add("Miami-Ft. Lauderdale", 640);
            IQDmaToFusionIDMap.Add("Milwaukee", 711);
            IQDmaToFusionIDMap.Add("Minneapolis-St. Paul", 528);
            IQDmaToFusionIDMap.Add("Minot-Bsmrck-Dcknsn(Wlstn)", 617);
            IQDmaToFusionIDMap.Add("Missoula", 613);
            IQDmaToFusionIDMap.Add("Mobile-Pensacola (Ft Walt)", 762);
            IQDmaToFusionIDMap.Add("Monroe-El Dorado", 628);
            IQDmaToFusionIDMap.Add("Monterey-Salinas", 828);
            IQDmaToFusionIDMap.Add("Montgomery-Selma", 698);
            IQDmaToFusionIDMap.Add("Myrtle Beach-Florence", 571);
            IQDmaToFusionIDMap.Add("Nashville", 659);
            IQDmaToFusionIDMap.Add("New Orleans", 622);
            IQDmaToFusionIDMap.Add("New York", 501);
            IQDmaToFusionIDMap.Add("Norfolk-Portsmth-Newpt New", 544);
            IQDmaToFusionIDMap.Add("North Platte", 740);
            IQDmaToFusionIDMap.Add("Odessa-Midland", 633);
            IQDmaToFusionIDMap.Add("Oklahoma City", 650);
            IQDmaToFusionIDMap.Add("Omaha", 652);
            IQDmaToFusionIDMap.Add("Orlando-Daytona Bch-Melbrn", 534);
            IQDmaToFusionIDMap.Add("Ottumwa-Kirksville", 631);
            IQDmaToFusionIDMap.Add("Paducah-Cape Girard-Harsbg", 632);
            IQDmaToFusionIDMap.Add("Palm Springs", 548);
            IQDmaToFusionIDMap.Add("Panama City", 804);
            IQDmaToFusionIDMap.Add("Parkersburg", 656);
            IQDmaToFusionIDMap.Add("Peoria-Bloomington", 597);
            IQDmaToFusionIDMap.Add("Philadelphia", 686);
            IQDmaToFusionIDMap.Add("Phoenix (Prescott)", 675);
            IQDmaToFusionIDMap.Add("Pittsburgh", 504);
            IQDmaToFusionIDMap.Add("Portland, OR", 753);
            IQDmaToFusionIDMap.Add("Portland-Auburn", 508);
            IQDmaToFusionIDMap.Add("PRESQUE ISLE", 820);
            IQDmaToFusionIDMap.Add("Providence-New Bedford", 552);
            IQDmaToFusionIDMap.Add("Quincy-Hannibal-Keokuk", 521);
            IQDmaToFusionIDMap.Add("Raleigh-Durham (Fayetvlle)", 717);
            IQDmaToFusionIDMap.Add("Rapid City", 560);
            IQDmaToFusionIDMap.Add("Reno", 764);
            IQDmaToFusionIDMap.Add("Richmond-Petersburg", 811);
            IQDmaToFusionIDMap.Add("Roanoke-Lynchburg", 556);
            IQDmaToFusionIDMap.Add("Rochester, NY", 538);
            IQDmaToFusionIDMap.Add("Rochestr-Mason City-Austin", 610);
            IQDmaToFusionIDMap.Add("Rockford", 862);
            IQDmaToFusionIDMap.Add("Sacramnto-Stkton-Modesto", 638);
            IQDmaToFusionIDMap.Add("Salisbury", 609);
            IQDmaToFusionIDMap.Add("Salt Lake City", 576);
            IQDmaToFusionIDMap.Add("San Angelo", 770);
            IQDmaToFusionIDMap.Add("San Antonio", 661);
            IQDmaToFusionIDMap.Add("San Diego", 641);
            IQDmaToFusionIDMap.Add("San Francisco-Oak-San Jose", 825);
            IQDmaToFusionIDMap.Add("SantaBarbra-SanMar-SanLuOb", 807);
            IQDmaToFusionIDMap.Add("Savannah", 855);
            IQDmaToFusionIDMap.Add("Seattle-Tacoma", 507);
            IQDmaToFusionIDMap.Add("Sherman-Ada", 819);
            IQDmaToFusionIDMap.Add("Shreveport", 657);
            IQDmaToFusionIDMap.Add("Sioux City", 612);
            IQDmaToFusionIDMap.Add("Sioux Falls(Mitchell)", 624);
            IQDmaToFusionIDMap.Add("South Bend-Elkhart", 725);
            IQDmaToFusionIDMap.Add("Spokane", 588);
            IQDmaToFusionIDMap.Add("Springfield, MO", 567);
            IQDmaToFusionIDMap.Add("Springfield-Holyoke", 881);
            IQDmaToFusionIDMap.Add("St. Joseph", 543);
            IQDmaToFusionIDMap.Add("St. Louis", 619);
            IQDmaToFusionIDMap.Add("Syracuse", 555);
            IQDmaToFusionIDMap.Add("Tallahassee-Thomasville", 530);
            IQDmaToFusionIDMap.Add("Tampa-St. Pete (Sarasota)", 539);
            IQDmaToFusionIDMap.Add("Terre Haute", 581);
            IQDmaToFusionIDMap.Add("Toledo", 547);
            IQDmaToFusionIDMap.Add("Topeka", 605);
            IQDmaToFusionIDMap.Add("Traverse City-Cadillac", 540);
            IQDmaToFusionIDMap.Add("Tri-Cities, TN-VA", 531);
            IQDmaToFusionIDMap.Add("Tucson (Sierra Vista)", 532);
            IQDmaToFusionIDMap.Add("Tulsa", 789);
            IQDmaToFusionIDMap.Add("Twin Falls", 671);
            IQDmaToFusionIDMap.Add("Tyler-Longview(Lfkn&Ncgd)", 673);
            IQDmaToFusionIDMap.Add("Utica", 760);
            IQDmaToFusionIDMap.Add("Victoria", 709);
            IQDmaToFusionIDMap.Add("Waco-Temple-Bryan", 526);
            IQDmaToFusionIDMap.Add("Washington, DC  (Hagrstwn)", 626);
            IQDmaToFusionIDMap.Add("Watertown", 625);
            IQDmaToFusionIDMap.Add("Wausau-Rhinelander", 511);
            IQDmaToFusionIDMap.Add("West Palm Beach-Ft. Pierce", 549);
            IQDmaToFusionIDMap.Add("Wheeling-Steubenville", 705);
            IQDmaToFusionIDMap.Add("Wichita Falls & Lawton", 554);
            IQDmaToFusionIDMap.Add("Wichita-Hutchinson Plus", 627);
            IQDmaToFusionIDMap.Add("Wilkes Barre-Scranton-Hztn", 577);
            IQDmaToFusionIDMap.Add("Wilmington", 550);
            IQDmaToFusionIDMap.Add("Yakima-Pasco-Rchlnd-KNnwck", 810);
            IQDmaToFusionIDMap.Add("Youngstown", 536);
            IQDmaToFusionIDMap.Add("Yuma-El Centro", 771);
            IQDmaToFusionIDMap.Add("Zanesville", 596);
        }

    }

    public static class IQStateToFusionIDMapModel
    {
        public static Dictionary<string, string> IQStateToFusionIDMap { get; set; }

        static IQStateToFusionIDMapModel()
        {
            IQStateToFusionIDMap = new Dictionary<string, string>();
            IQStateToFusionIDMap.Add("ALABAMA",	"AL");
            IQStateToFusionIDMap.Add("ALASKA", "AK");
            IQStateToFusionIDMap.Add("ARIZONA", "AZ");
            IQStateToFusionIDMap.Add("ARKANSAS", "AR");
            IQStateToFusionIDMap.Add("CALIFORNIA", "CA");
            IQStateToFusionIDMap.Add("COLORADO", "CO");
            IQStateToFusionIDMap.Add("CONNECTICUT", "CT");
            IQStateToFusionIDMap.Add("DELAWARE", "DE");
            IQStateToFusionIDMap.Add("DISTRICT OF COLUMBIA", "DC");
            IQStateToFusionIDMap.Add("FLORIDA", "FL");
            IQStateToFusionIDMap.Add("GEORGIA", "GA");
            IQStateToFusionIDMap.Add("HAWAII", "HI");
            IQStateToFusionIDMap.Add("IDAHO", "ID");
            IQStateToFusionIDMap.Add("ILLINOIS", "IL");
            IQStateToFusionIDMap.Add("INDIANA", "IN");
            IQStateToFusionIDMap.Add("IOWA", "IA");
            IQStateToFusionIDMap.Add("KANSAS", "KS");
            IQStateToFusionIDMap.Add("KENTUCKY", "KY");
            IQStateToFusionIDMap.Add("LOUISIANA", "LA");
            IQStateToFusionIDMap.Add("MAINE", "ME");
            IQStateToFusionIDMap.Add("MARYLAND", "MD");
            IQStateToFusionIDMap.Add("MASSACHUSETTS", "MA");
            IQStateToFusionIDMap.Add("MICHIGAN", "MI");
            IQStateToFusionIDMap.Add("MINNESOTA", "MN");
            IQStateToFusionIDMap.Add("MISSISSIPPI", "MS");
            IQStateToFusionIDMap.Add("MISSOURI", "MO");
            IQStateToFusionIDMap.Add("MONTANA", "MT");
            IQStateToFusionIDMap.Add("NEBRASKA", "NE");
            IQStateToFusionIDMap.Add("NEVADA", "NV");
            IQStateToFusionIDMap.Add("NEW HAMPSHIRE", "NH");
            IQStateToFusionIDMap.Add("NEW JERSEY", "NJ");
            IQStateToFusionIDMap.Add("NEW MEXICO", "NM");
            IQStateToFusionIDMap.Add("NEW YORK", "NY");
            IQStateToFusionIDMap.Add("NORTH CAROLINA", "NC");
            IQStateToFusionIDMap.Add("NORTH DAKOTA", "ND");
            IQStateToFusionIDMap.Add("OHIO", "OH");
            IQStateToFusionIDMap.Add("OKLAHOMA", "OK");
            IQStateToFusionIDMap.Add("OREGON", "OR");
            IQStateToFusionIDMap.Add("PENNSYLVANIA", "PA");
            IQStateToFusionIDMap.Add("RHODE ISLAND", "RI");
            IQStateToFusionIDMap.Add("SOUTH CAROLINA", "SC");
            IQStateToFusionIDMap.Add("SOUTH DAKOTA", "SD");
            IQStateToFusionIDMap.Add("TENNESSEE", "TN");
            IQStateToFusionIDMap.Add("TEXAS", "TX");
            IQStateToFusionIDMap.Add("UTAH", "UT");
            IQStateToFusionIDMap.Add("VERMONT", "VT");
            IQStateToFusionIDMap.Add("VIRGINIA", "VA");
            IQStateToFusionIDMap.Add("WASHINGTON", "WA");
            IQStateToFusionIDMap.Add("WEST VIRGINIA", "WV");
            IQStateToFusionIDMap.Add("WISCONSIN", "WI");
            IQStateToFusionIDMap.Add("WYOMING", "WY");
        }
    }

    public static class IQProvinceToFusionIDMapModel
    {
        public static Dictionary<string, string> IQProvinceToFusionIDMap { get; set; }

        static IQProvinceToFusionIDMapModel()
        {
            IQProvinceToFusionIDMap = new Dictionary<string, string>();
            IQProvinceToFusionIDMap.Add("Alberta", "01");
            IQProvinceToFusionIDMap.Add("British Columbia", "02");
            IQProvinceToFusionIDMap.Add("Manitoba", "03");
            IQProvinceToFusionIDMap.Add("New Brunswick", "04");
            IQProvinceToFusionIDMap.Add("Newfoundland & Labrador", "05");
            IQProvinceToFusionIDMap.Add("Northwest Territories", "13");
            IQProvinceToFusionIDMap.Add("Nova Scotia", "07");
            IQProvinceToFusionIDMap.Add("Nunavut", "14");
            IQProvinceToFusionIDMap.Add("Ontario", "08");
            IQProvinceToFusionIDMap.Add("Prince Edward Island", "09");
            IQProvinceToFusionIDMap.Add("Quebec", "10");
            IQProvinceToFusionIDMap.Add("Saskatchewan", "11");
            IQProvinceToFusionIDMap.Add("Yukon Territory", "12");
        }
    }
}
