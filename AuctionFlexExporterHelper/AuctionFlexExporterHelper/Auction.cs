using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionFlexDBConnector
{
    class Auction
    {
        public string name { get; }
        public int id { get; }
        public Auction(string name, int id)
        {
            this.name = name;
            this.id = id;
        }
        public override string ToString()
        {
            return name;
        }
    }
}
