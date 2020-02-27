using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionFlexDBConnector
{
    class Picture
    {
        public int id { get; }
        public string webpath { get; }
        public string tinypath { get; }
        public Picture(int id, string webpath, string tinypath)
        {
            this.id = id;
            this.webpath = webpath;
            this.tinypath = tinypath;
        }
    }
}
