using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionFlexDBConnector
{
    class InvoiceItem
    {
        public int invoicenum { get; }
        public int bidnum { get; }
        public Item item { get; }
        public decimal baseprice { get; }

        public InvoiceItem(int invoicenum, int bidnum, Item item, decimal baseprice)
        {
            this.invoicenum = invoicenum;
            this.bidnum = bidnum;
            this.item = item;
            this.baseprice = baseprice;
        }
    }
}
