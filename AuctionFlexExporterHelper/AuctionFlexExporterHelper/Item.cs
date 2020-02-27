using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionFlexDBConnector
{
    class Item
    {
        public int id { get; }
        public string lead { get; }
        public int conum { get; }
        public string desc { get; }
        public decimal min { get; }
        public decimal max { get; }
        public decimal reserve { get; }
        public decimal amount { get; }
        public decimal onhand { get; }
        public decimal allocated { get; }
        public int invnum { get; }
        public int lotnum { get; set; }
        public List<Picture> pic { get; }

        public Item(int id, int invnum, string lead, int conum, string desc, decimal min, decimal max, decimal reserve, decimal amount, decimal onhand, decimal allocated, List<Picture> pic)
        {
            this.id = id;
            this.lead = lead;
            this.conum = conum;
            this.desc = desc;
            this.min = min;
            this.max = max;
            this.reserve = reserve;
            this.amount = amount;
            this.onhand = onhand;
            this.allocated = allocated;
            this.invnum = invnum;
            this.pic = pic;
        }
        public override string ToString()
        {
            if(lotnum!=0)
                return lotnum + ": "+lead;
            return invnum + ": " + lead;
        }
    }
}
