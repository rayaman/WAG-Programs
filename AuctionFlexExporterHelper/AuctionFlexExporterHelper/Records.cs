using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace AuctionFlexExporterHelper
{
    public class LivauctioneersMap : ClassMap<Liveauctioneers>
    {
        public LivauctioneersMap()
        {
            Map(m => m.LotNum).Index(0).Name("LotNum");
            Map(m => m.lead).Index(1).Name("Title");
            Map(m => m.description1).Index(2).Name("Description");
            Map(m => m.presaleestmin).Index(3).Name("LowEst");
            Map(m => m.presaleestmax).Index(4).Name("HighEst");
            Map(m => m.start).Index(5).Name("StartPrice");
            Map(m => m.buynow).Index(6).Name("Buy Now Price");
            Map(m => m.exclude).Index(7).Name("Exclude From Buy Now");
            Map(m => m.condition).Index(8).Name("Condition");
            Map(m => m.category).Index(9).Name("Category");
            Map(m => m.origin).Index(10).Name("Origin");
            Map(m => m.style).Index(11).Name("Style & Period");
            Map(m => m.creator).Index(12).Name("Creator");
            Map(m => m.materials).Index(13).Name("Materials & Techniques");
            Map(m => m.reserve).Index(14).Name("Reserve Price");
            Map(m => m.shipping).Index(15).Name("Domestic Flat Shipping Price");
            Map(m => m.Height).Index(16).Name("Height");
            Map(m => m.Width).Index(17).Name("Width");
            Map(m => m.Depth).Index(18).Name("Depth");
            Map(m => m.dimensionU).Index(19).Name("Dimension Unit");
            Map(m => m.Weight).Index(20).Name("Weight");
            Map(m => m.WeightU).Index(21).Name("Weight Unit");
            Map(m => m.quantity).Index(22).Name("Quantity");
        }
    }
    public class Liveauctioneers
    {
        public string LotNum { get; set; }
        public string lead { get; set; }
        public string description1 { get; set; }
        public string presaleestmin { get; set; }
        public string presaleestmax { get; set; }
        public string inventorynum { get; set; }
        public string Length { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Weight { get; set; }
        public string start { get; set; }
        public string buynow { get; set; }
        public string exclude { get; set; }
        public string condition { get; set; }
        public string category { get; set; }
        public string origin { get; set; }
        public string style { get; set; }
        public string creator { get; set; }
        public string materials { get; set; }
        public string reserve { get; set; }
        public string shipping { get; set; }
        public string Depth { get; set; }
        public string dimensionU { get; set; }
        public string WeightU { get; set; }
        public string quantity { get; set; }
        public Liveauctioneers(string lotnum, string lead, string desc, string min, string max)
        {
            LotNum = lotnum;
            this.lead = lead;
            description1 = desc;
            presaleestmin = min;
            presaleestmax = max;
            start = "0";
        }
    }
    public class InvaluableMap : ClassMap<Invaluable>
    {
        public InvaluableMap()
        {
            Map(m => m.lotnum).Index(0).Name("lotnum");
            Map(m => m.lead).Index(1).Name("lead");
            Map(m => m.description1).Index(2).Name("description1");
            Map(m => m.presaleestmin).Index(3).Name("presaleestmin");
            Map(m => m.presaleestmax).Index(4).Name("presaleestmax");
            Map(m => m.inventorynum).Index(5).Name("inventorynum");
            Map(m => m.Length).Index(6).Name("Length");
            Map(m => m.Height).Index(7).Name("Height");
            Map(m => m.Width).Index(8).Name("Width");
            Map(m => m.Weight).Index(9).Name("Weight");
        }
    }
    public class Invaluable
    {
        public string lotnum { get; set; }
        public string lead { get; set; }
        public string description1 { get; set; }
        public string presaleestmin { get; set; }
        public string presaleestmax { get; set; }
        public string inventorynum { get; set; }
        public string Length { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Weight { get; set; }
        public Invaluable(string lotnum, string lead, string desc, string min, string max)
        {
            this.lotnum = lotnum;
            this.lead = lead;
            description1 = desc;
            presaleestmin = min;
            presaleestmax = max;
        }
    }
}
