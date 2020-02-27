using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionFlexDBConnector
{
    class Customer
    {
        public int id { get; }
        public string company { get; }
        public string first { get; }
        public string middle { get; }
        public string last { get; }
        public string address { get; }
        public string city { get; }
        public string state { get; }
        public string zip { get; }
        public string phone { get; }
        public string fax { get; }
        public string email { get; }
        public string country { get; }
        public string buyercode { get; }
        public bool isSeller { get; }
        public string sellercode { get; }
        public string phone2 { get; }
        public string phone3 { get; }
        public int sellerid { get; }

        public Customer(int id, string company, string first, string middle, string last, string address, string city, string state, string zip, string phone, string fax, string email, string country, string buyercode, bool isSeller, string sellercode, string phone2, string phone3, int sellerid)
        {
            this.id = id;
            this.company = company;
            this.first = first;
            this.middle = middle;
            this.last = last;
            this.address = address;
            this.city = city;
            this.state = state;
            this.zip = zip;
            this.phone = phone;
            this.fax = fax;
            this.email = email;
            this.country = country;
            this.buyercode = buyercode;
            this.isSeller = isSeller;
            this.sellercode = sellercode;
            this.sellerid = sellerid;
            this.phone2 = phone2;
            this.phone3 = phone3;
        }
    }
}
