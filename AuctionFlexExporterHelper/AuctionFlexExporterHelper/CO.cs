using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionFlexDBConnector
{
    class CO
    {
        public int conum { get; }
        public int seller_id { get; }
        public int event_id { get; }
        public Customer buyer { get; }

        public CO(int conum, int seller_id, int event_id, Customer buyer)
        {
            this.conum = conum;
            this.seller_id = seller_id;
            this.event_id = event_id;
            this.buyer = buyer;
        }
    }
}
