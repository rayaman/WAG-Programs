using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionFlexDBConnector
{
    class Invoice
    {
        public int id { get; }
        public DateTime date { get; }
        public string notes { get; }
        public int auction_id { get; }
        public int buyer_id { get; }
        public decimal baseprice { get; }
        public decimal buyerPrem { get; }
        public decimal taxrate { get; }
        public decimal taxableamount { get; }
        public decimal total { get; }
        public int bidcardnum { get; }
        public decimal amountdue { get; }
        public List<InvoiceItem> items { get; }

        public Invoice(int id, DateTime date, string notes, int auction_id, int buyer_id, decimal baseprice, decimal buyerPrem, decimal taxrate, decimal taxableamount, decimal total, int bidcardnum, decimal amountdue, List<InvoiceItem> items)
        {
            this.id = id;
            this.date = date;
            this.notes = notes;
            this.auction_id = auction_id;
            this.buyer_id = buyer_id;
            this.baseprice = baseprice;
            this.buyerPrem = buyerPrem;
            this.taxrate = taxrate;
            this.taxableamount = taxableamount;
            this.total = total;
            this.bidcardnum = bidcardnum;
            this.amountdue = amountdue;
            this.items = items;
        }
    }
}
