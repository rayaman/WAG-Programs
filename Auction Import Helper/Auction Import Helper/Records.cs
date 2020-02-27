using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Auction_Import_Helper
{
    public static class utils {
        public static bool isEnglish(string[] str)
        {
            bool test = true;
            foreach (string s in str)
            {
                if (!Regex.IsMatch(s, "^['/a-zA-Z0-9\\s\\.,\\-#\\(\\)]*$"))
                    test = false;
            }
            return test;
        }
    }
    public class LAFRecord
    {
        [Index(0)]
        public string LotNum { get; set; }
        [Index(3)]
        public string Hammer { get; set; }
        [Index(5)]
        public string First { get; set; }
        [Index(6)]
        public string Last { get; set; }
        [Index(8)]
        public string Email { get; set; }
        [Index(9)]
        public string Phone1 { get; set; }
        [Index(10)]
        public string Phone2 { get; set; }
        [Index(11)]
        public string Mobile { get; set; }
        [Index(12)]
        public string Address { get; set; }
        [Index(13)]
        public string City { get; set; }
        [Index(14)]
        public string State { get; set; }
        [Index(15)]
        public string Country { get; set; }
        [Index(16)]
        public string Zip { get; set; }
        [Index(17)]
        public string BidNum { get; set; }
        public static List<LAFRecord> problematic = new List<LAFRecord>();
        public CustomersRecord convertCustomers()
        {
            try
            {
                State = GetStateByName(State);
            } catch
            {
                // Do nothing to State!
            }
            if (!utils.isEnglish(new string[] { Address, First, Last, City, State, Country })) { 
                problematic.Add(this);
            }
            string p1, p2, p3;
            string[] numbers = new string[] { Mobile, Phone1, Phone2 };
            Array.Sort(numbers);
            p1 = numbers[2];
            p2 = numbers[1];
            p3 = numbers[0];
            p1 = p1.Replace(" ", string.Empty);
            p2 = p2.Replace(" ", string.Empty);
            p3 = p3.Replace(" ", string.Empty);
            p1 = p1.Replace("-", string.Empty);
            p2 = p2.Replace("-", string.Empty);
            p3 = p3.Replace("-", string.Empty);
            p1 = p1.Replace("(", string.Empty);
            p2 = p2.Replace("(", string.Empty);
            p3 = p3.Replace("(", string.Empty);
            p1 = p1.Replace(")", string.Empty);
            p2 = p2.Replace(")", string.Empty);
            p3 = p3.Replace(")", string.Empty);
            p1 = p1.Replace("+", string.Empty);
            p2 = p2.Replace("+", string.Empty);
            p3 = p3.Replace("+", string.Empty);
            CustomersRecord.Reference.Add(Email,BidNum);
            return new CustomersRecord(First, Last, Email, p1, p2, p3, Address, City, State, Country, Zip, BidNum);
        }
        public BidInfo convertBidInfo()
        {
            Hammer = Hammer.Substring(1);
            return new BidInfo(LotNum, Hammer.Replace(",",""), Hammer.Replace(",",""), BidNum);
        }
        public string GetStateByName(string name)
        {
            switch (name.ToUpper())
            {
                case "ALABAMA":
                    return "AL";

                case "ALASKA":
                    return "AK";

                case "AMERICAN SAMOA":
                    return "AS";

                case "ARIZONA":
                    return "AZ";

                case "ARKANSAS":
                    return "AR";

                case "CALIFORNIA":
                    return "CA";

                case "COLORADO":
                    return "CO";

                case "CONNECTICUT":
                    return "CT";

                case "DELAWARE":
                    return "DE";

                case "DISTRICT OF COLUMBIA":
                    return "DC";

                case "FEDERATED STATES OF MICRONESIA":
                    return "FM";

                case "FLORIDA":
                    return "FL";

                case "GEORGIA":
                    return "GA";

                case "GUAM":
                    return "GU";

                case "HAWAII":
                    return "HI";

                case "IDAHO":
                    return "ID";

                case "ILLINOIS":
                    return "IL";

                case "INDIANA":
                    return "IN";

                case "IOWA":
                    return "IA";

                case "KANSAS":
                    return "KS";

                case "KENTUCKY":
                    return "KY";

                case "LOUISIANA":
                    return "LA";

                case "MAINE":
                    return "ME";

                case "MARSHALL ISLANDS":
                    return "MH";

                case "MARYLAND":
                    return "MD";

                case "MASSACHUSETTS":
                    return "MA";

                case "MICHIGAN":
                    return "MI";

                case "MINNESOTA":
                    return "MN";

                case "MISSISSIPPI":
                    return "MS";

                case "MISSOURI":
                    return "MO";

                case "MONTANA":
                    return "MT";

                case "NEBRASKA":
                    return "NE";

                case "NEVADA":
                    return "NV";

                case "NEW HAMPSHIRE":
                    return "NH";

                case "NEW JERSEY":
                    return "NJ";

                case "NEW MEXICO":
                    return "NM";

                case "NEW YORK":
                    return "NY";

                case "NORTH CAROLINA":
                    return "NC";

                case "NORTH DAKOTA":
                    return "ND";

                case "NORTHERN MARIANA ISLANDS":
                    return "MP";

                case "OHIO":
                    return "OH";

                case "OKLAHOMA":
                    return "OK";

                case "OREGON":
                    return "OR";

                case "PALAU":
                    return "PW";

                case "PENNSYLVANIA":
                    return "PA";

                case "PUERTO RICO":
                    return "PR";

                case "RHODE ISLAND":
                    return "RI";

                case "SOUTH CAROLINA":
                    return "SC";

                case "SOUTH DAKOTA":
                    return "SD";

                case "TENNESSEE":
                    return "TN";

                case "TEXAS":
                    return "TX";

                case "UTAH":
                    return "UT";

                case "VERMONT":
                    return "VT";

                case "VIRGIN ISLANDS":
                    return "VI";

                case "VIRGINIA":
                    return "VA";

                case "WASHINGTON":
                    return "WA";

                case "WEST VIRGINIA":
                    return "WV";

                case "WISCONSIN":
                    return "WI";

                case "WYOMING":
                    return "WY";
            }
            throw new Exception("Not Available");
        }
    }
    
    public class IEOARecord
    {
        [Index(0)]
        public string Lotnum { get; set; }
        [Index(1)]
        public string Hammer { get; set; }
        [Index(4)]
        public string Email { get; set; }
        public string BidNum { get; set; }
        public BidInfo convertBidInfo()
        {
            return new BidInfo(Lotnum.Remove(Lotnum.Length - 1), Hammer, Hammer, CustomersRecord.Reference[Email]);
        }
    }
    public class IBRecord
    {
        [Index(0)]
        public string Name { get; set; }
        [Index(2)]
        public string Email { get; set; }
        [Index(3)]
        public string Phone { get; set; }
        [Index(4)]
        public string Address { get; set; }
        [Index(5)]
        public string City { get; set; }
        [Index(6)]
        public string State { get; set; }
        [Index(7)]
        public string Country { get; set; }
        [Index(8)]
        public string Zip { get; set; }
        public static List<IBRecord> problematic = new List<IBRecord>();
        public int BidNum;
        public static int BN = 400;
        public IBRecord()
        {
            BidNum = BN++;
        }
        public CustomersRecord convertCustomers()
        {
            if (!utils.isEnglish(new string[] { Address, Name, City, State, Country }))
            {
                problematic.Add(this);
            }
            var temp = Name.Split(' ');
            string first = temp[0];
            string last = temp[1];
            Phone = Phone.Replace(" ", string.Empty);
            Phone = Phone.Replace("-", string.Empty);
            Phone = Phone.Replace("(", string.Empty);
            Phone = Phone.Replace(")", string.Empty);
            Phone = Phone.Replace("+", string.Empty);
            CustomersRecord.Reference.Add(Email, BidNum.ToString()); // Allows us to look up some bid info later on. IV only!
            return new CustomersRecord(first, last, Email, Phone, "", "", Address, City, State, Country, Zip, BidNum.ToString());
        }
    }
    public class CustomersRecord
    {
        public string First { get; set; }
        public string Last { get; set; }
        public string Email { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Phone3 { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string BidNum { get; set; }
        public static Dictionary<string, string> Reference =
    new Dictionary<string, string>();
        public CustomersRecord(string first, string last, string email, string phone1, string phone2, string phone3, string address, string city, string state, string country, string zip, string bidNum)
        {
            First = first;
            Last = last;
            Email = email;
            Phone1 = phone1;
            Phone2 = phone2;
            Phone3 = phone3;
            Address = address;
            City = city;
            State = state;
            Country = country;
            Zip = zip;
            BidNum = bidNum;
        }
    }
    public class BidInfo
    {
        public string Lot { get; set; }
        public string BidMaxU { get; set; }
        public string BidMaxE { get; set; }
        public string BidNum { get; set; }
        public string Quantity { get; set; }

        public BidInfo(string lot, string bidMaxU, string bidMaxE, string bidNum)
        {
            Lot = lot;
            BidMaxU = bidMaxU;
            BidMaxE = bidMaxE;
            BidNum = bidNum;
            Quantity = "1";
        }
    }
    public class BidInfoMap : ClassMap<BidInfo>
    {
        public BidInfoMap()
        {
            Map(m => m.Lot).Index(0).Name("LotNum");
            Map(m => m.BidMaxU).Index(1).Name("BidUnit");
            Map(m => m.BidMaxE).Index(2).Name("BidExt");
            Map(m => m.BidNum).Index(3).Name("BidNum");
            Map(m => m.Quantity).Index(4).Name("Quantity");
        }
    }
    public class CustomersRecordMap : ClassMap<CustomersRecord>
    {
        public CustomersRecordMap()
        {
            Map(m => m.First).Index(0).Name("First");
            Map(m => m.Last).Index(1).Name("Last");
            Map(m => m.Email).Index(2).Name("Email");
            Map(m => m.Phone1).Index(3).Name("Phone1");
            Map(m => m.Phone2).Index(4).Name("Phone2");
            Map(m => m.Phone3).Index(5).Name("Phone3");
            Map(m => m.Address).Index(6).Name("Address");
            Map(m => m.City).Index(7).Name("City");
            Map(m => m.State).Index(8).Name("State");
            Map(m => m.Country).Index(9).Name("Country");
            Map(m => m.Zip).Index(10).Name("Zip");
            Map(m => m.BidNum).Index(11).Name("BidNum");
        }
    }
}
