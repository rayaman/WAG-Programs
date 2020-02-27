using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Data;
using System.Linq;
using System.Data.OleDb;
using System.Windows;
using System.Collections.Generic;

namespace AuctionFlexDBConnector
{
    class afdb
    {
        public DataTable pictures; // done
        public DataTable items; // done
        public DataTable Auction_items; // done
        public DataTable invoiced_items; // done
        public DataTable consignor_orders; // done
        public DataTable buyers; // done
        public DataTable auctions; // done
        public DataTable invoices; // done
        public DataTable invrecipt;
        public bool itemIsInvoiced(int id)
        {
            return invoiced_items.Select("ITEM_ID = " + id).Length > 0;
        }
        public bool InvoiceIsPaid(int id)
        {
            var rows = invrecipt.Select("INVNUM = "+id);
            return rows.Length > 0;
        }
        public List<InvoiceItem> GetInvoiceItems()
        {
            List<InvoiceItem> temp = new List<InvoiceItem>();
            DataRow[] rows = invoiced_items.Select();
            for (int i = 0; i < rows.Length; i++)
            {
                DataRow r = rows[i];
                temp.Add(new InvoiceItem((int)r["INVNUM"],(int)r["BIDNUM"],GetItemByID((int)r["ITEM_ID"]),(decimal)r["BASEPRICEEXT"]));
            }
            return temp;
        }
        public List<InvoiceItem> GetInvoiceItemsByInvoiceNum(int id)
        {
            List<InvoiceItem> temp = new List<InvoiceItem>();
            DataRow[] rows = invoiced_items.Select("INVNUM = "+id);
            for (int i = 0; i < rows.Length; i++)
            {
                DataRow r = rows[i];
                temp.Add(new InvoiceItem((int)r["INVNUM"], (int)r["BIDNUM"], GetItemByID((int)r["ITEM_ID"]), (decimal)r["BASEPRICEEXT"]));
            }
            return temp;
        }
        public Invoice GetInvoiceFromInvoiceNum(int id)
        {
            DataRow[] rows = invoices.Select("INVNUM = " + id);
            if (rows.Length > 0) {
                var r = rows[0];
                return new Invoice((int)r["INVNUM"], (DateTime)r["DATE"], (string)r["NOTES"],(int)r["EVENT_ID"],(int)r["BUYER_ID"],(decimal)r["BASEPRICEEXT"],(decimal)r["buyerpremiumrate"],(decimal)r["TAXRATE1"],(decimal)r["taxableamount2"],(decimal)r["TOTAL"],(int)r["BIDCARDNUM"],(decimal)r["TOTCHARGES"], GetInvoiceItemsByInvoiceNum((int)r["INVNUM"]));
            }
            return null;
        }
        public InvoiceItem GetInvoiceItemFromItemID(int id)
        {
            var inv = GetInvoiceItems();
            foreach (InvoiceItem i in inv)
            {
                if (i.item.id == id)
                    return i;
            }
            return null;
        }
        public List<InvoiceItem> GetInvoiceItemByInvNum(int id)
        {
            List<InvoiceItem> temp = new List<InvoiceItem>();
            DataRow[] rows = invoiced_items.Select("INVNUM = " + id);
            for (int i = 0; i < rows.Length; i++)
            {
                DataRow r = rows[i];
                temp.Add(new InvoiceItem((int)r["INVNUM"], (int)r["BIDNUM"], GetItemByID((int)r["ITEM_ID"]), (decimal)r["BASEPRICEEXT"]));
            }
            return temp;
        }
        public List<CO> GetCOs()
        {
            List<CO> temp = new List<CO>();
            DataRow[] rows = consignor_orders.Select();
            for (int i = 0; i < rows.Length; i++)
            {
                DataRow r = rows[i];
                var b = GetBuyerFromSellerID((int)r["SELLER_ID"]);
                temp.Add(new CO((int)r["CONUM"],(int)r["SELLER_ID"],(int)r["EVENT_ID"],b));
            }
            return temp;
        }
        public List<CO> GetCOBySeller_ID(int id)
        {
            List<CO> temp = new List<CO>();
            DataRow[] rows = consignor_orders.Select("SELLER_ID = " + id);
            for (int i = 0; i < rows.Length; i++)
            {
                DataRow r = rows[i];
                var b = GetBuyerFromSellerID(id);
                temp.Add(new CO((int)r["CONUM"], id, (int)r["EVENT_ID"], b));
            }
            return temp;
        }
        public List<Item> GetItemsByAuctionID(int id)
        {
            List<Item> temp = new List<Item>();
            DataRow[] rows = Auction_items.Select("EVENT_ID = " + id, "LOTNUM ASC");
            for (int i = 0; i < rows.Length; i++)
            {
                DataRow r = rows[i];
                Item item = GetItemByID((int)r["ITEM_ID"]);
                item.lotnum = (int)r["LOTNUM"];
                temp.Add(item);
            }
            return temp;
        }
        public List<Item> GetItemsByAuctionIDAndSeller(int id,Customer b)
        {
            List<Item> items = GetItemsByAuctionID(id);
            List<CO> cos = GetCOBySeller_ID(b.sellerid);
            List<Item> temp = new List<Item>();
            foreach(Item i in items)
            {
                foreach(CO c in cos)
                {
                    if ((i.conum == c.conum) && (c.seller_id==b.sellerid))
                    {
                        temp.Add(i);
                    }
                }
            }
            return temp;
        }
        public List<Item> GetItemListByCO(int id)
        {
            List<Item> temp = new List<Item>();
            DataRow[] rows = items.Select("SOURCENUM = "+id, "ITEM_ID ASC");
            for (int i = 0; i < rows.Length; i++)
            {
                DataRow r = rows[i];
                temp.Add(new Item((int)r["ITEM_ID"], (int)r["ITEMNUM"], (string)r["LEAD"],(int)r["SOURCENUM"],(string)r["description"],(decimal)r["PRESALEESTMIN"],(decimal)r["PRESALEESTMAX"],(decimal)r["RESERVE"],(decimal)r["QUANTITY"],(decimal)r["ONHAND"],(decimal)r["ALLOCATED"], GetPictureListByItemID((int)r["ITEM_ID"])));
            }
            return temp;
        }
        public Item GetItemByID(int id)
        {
            try
            {
                DataRow r = items.Select("ITEM_ID = " + id)[0];
                return new Item((int)r["ITEM_ID"], (int)r["ITEMNUM"], (string)r["LEAD"], (int)r["SOURCENUM"], (string)r["description"], (decimal)r["PRESALEESTMIN"], (decimal)r["PRESALEESTMAX"], (decimal)r["RESERVE"], (decimal)r["QUANTITY"], (decimal)r["ONHAND"], (decimal)r["ALLOCATED"], GetPictureListByItemID((int)r["ITEM_ID"]));
            }
            catch
            {
                return null;
            }
        }
        public Item GetItemByInvNum(int id)
        {
            try
            {
                DataRow r = items.Select("ITEMNUM = " + id)[0];
                return new Item((int)r["ITEM_ID"], (int)r["ITEMNUM"], (string)r["LEAD"], (int)r["SOURCENUM"], (string)r["description"], (decimal)r["PRESALEESTMIN"], (decimal)r["PRESALEESTMAX"], (decimal)r["RESERVE"], (decimal)r["QUANTITY"], (decimal)r["ONHAND"], (decimal)r["ALLOCATED"], GetPictureListByItemID((int)r["ITEM_ID"]));
            }
            catch
            {
                return null;
            }
        }
        public List<Picture> GetPictureListByItemID(int id)
        {
            List<Picture> temp = new List<Picture>();
            DataRow[] rows = pictures.Select("ITEM_ID = " + id);
            for (int i = 0; i < rows.Length; i++)
            {
                DataRow r = rows[i];
                temp.Add(new Picture((int)r["PICTURE_ID"],(string)r["WEBPICTURE_LOC"],(string)r["TNPICTURE_LOC"]));
            }
            return temp;
        }
        public List<Auction> GetAuctionList()
        {
            List<Auction> temp = new List<Auction>();
            DataRow[] rows = auctions.Select();
            for (int i = 0; i < rows.Length; i++)
            {
                temp.Add(new Auction((string)rows[i]["EVENTNAME"], (int)rows[i]["EVENT_ID"]));
            }
            return temp;
        }
        public Customer GetBuyerFromSellerID(int id)
        {
            try {
                List<Customer> temp = new List<Customer>();
                DataRow r = buyers.Select("SELLER_ID = " + id)[0];
                return new Customer((int)r["BUYER_ID"], (string)r["COMPANY"], (string)r["FIRST"], (string)r["MIDDLENAME"], (string)r["LAST"], (string)r["ADDRESS"], (string)r["CITY"], (string)r["STATE"], (string)r["ZIP"], (string)r["PHONE"], (string)r["FAX"], (string)r["EMAIL"], (string)r["COUNTRYNAME"], (string)r["BUYERCODE"], (bool)r["SELLER"], (string)r["SELLERCODE"], (string)r["PHONE2"], (string)r["PHONE3"], (int)r["SELLER_ID"]);
            } catch
            {
                return null;
            }
        }
        public Customer GetBuyerFromSellerCode(string id)
        {
            try {
                List<Customer> temp = new List<Customer>();
                DataRow r = buyers.Select("SELLERCODE = '" + id + "'")[0];
                return new Customer((int)r["BUYER_ID"], (string)r["COMPANY"], (string)r["FIRST"], (string)r["MIDDLENAME"], (string)r["LAST"], (string)r["ADDRESS"], (string)r["CITY"], (string)r["STATE"], (string)r["ZIP"], (string)r["PHONE"], (string)r["FAX"], (string)r["EMAIL"], (string)r["COUNTRYNAME"], (string)r["BUYERCODE"], (bool)r["SELLER"], (string)r["SELLERCODE"], (string)r["PHONE2"], (string)r["PHONE3"], (int)r["SELLER_ID"]);
            } catch {
                return null;
            }
        }
        public Customer GetBuyerFromCO(int id)
        {
            var list = GetCOs();
            foreach(CO c in list)
            {
                if (c.conum == id)
                {
                    return c.buyer;
                }
            }
            return null;
        }
        public List<Customer> GetBuyerList()
        {
            List<Customer> temp = new List<Customer>();
            DataRow[] rows = buyers.Select();
            for (int i = 0; i < rows.Length; i++)
            {
                DataRow r = rows[i];
                temp.Add(new Customer((int)r["BUYER_ID"], (string)r["COMPANY"], (string)r["FIRST"], (string)r["MIDDLENAME"], (string)r["LAST"], (string)r["ADDRESS"], (string)r["CITY"], (string)r["STATE"], (string)r["ZIP"], (string)r["PHONE"], (string)r["FAX"], (string)r["EMAIL"], (string)r["COUNTRYNAME"], (string)r["BUYERCODE"], (bool)r["SELLER"], (string)r["SELLERCODE"], (string)r["PHONE2"], (string)r["PHONE3"], (int)r["SELLER_ID"]));
            }
            return temp;
        }
        public Customer GetBuyerByID(int id)
        {
            DataRow[] rows = buyers.Select("BUYER_ID = " + id);
            if (rows.Length > 0)
            {
                DataRow r = rows[0];
                return new Customer((int)r["BUYER_ID"], (string)r["COMPANY"], (string)r["FIRST"], (string)r["MIDDLENAME"], (string)r["LAST"], (string)r["ADDRESS"], (string)r["CITY"], (string)r["STATE"], (string)r["ZIP"], (string)r["PHONE"], (string)r["FAX"], (string)r["EMAIL"], (string)r["COUNTRYNAME"], (string)r["BUYERCODE"], (bool)r["SELLER"], (string)r["SELLERCODE"], (string)r["PHONE2"], (string)r["PHONE3"], (int)r["SELLER_ID"]);
            }
            return null;
        }
        public bool initAFDB()
        {
            try
            {
                if (Directory.Exists(Directory.GetCurrentDirectory() + @"\Reference"))
                {
                    var dir = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\Reference");
                    dir.Delete(true);
                }
                DirectoryCopy(@"C:\AuctionFlex\Data", Directory.GetCurrentDirectory() + @"\Reference", true);
                pictures = DBFToDataTable("PICTURES.DBF");
                items = DBFToDataTable("items.DBF");
                Auction_items = DBFToDataTable("EVENTITEMS.DBF");
                invoiced_items = DBFToDataTable("invitems.DBF");
                invoices = DBFToDataTable("invhead.dbf");
                consignor_orders = DBFToDataTable("cohead.DBF");
                buyers = DBFToDataTable("BUYERMAST.DBF");
                auctions = DBFToDataTable("EVENTHEAD.DBF");
                invrecipt = DBFToDataTable("INVRECEIPTS.DBF");
                if (Directory.Exists(Directory.GetCurrentDirectory() + @"\Reference"))
                {
                    var dir = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\Reference");
                    dir.Delete(true);
                }
                return true;
            } catch
            {
                return false;
            }
        }
        public afdb()
        {
            initAFDB();
        }
        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
        private void DBFToCSV(string dbfPath, string csvPath)
        {
            string connectionString = "Provider=VFPOLEDB.1;Data Source=" + Directory.GetCurrentDirectory() + @"\Reference\" + dbfPath;
            string dbfToConvert = Directory.GetCurrentDirectory() + @"\Reference\" + dbfPath;
            ConvertDbf(connectionString, dbfToConvert, Directory.GetCurrentDirectory() + "\\" + csvPath);
        }
        private DataTable DBFToDataTable(string dbfPath)
        {
            string connectionString = "Provider=VFPOLEDB.1;Data Source=" + Directory.GetCurrentDirectory() + @"\Reference\" + dbfPath;
            string dbfToConvert = Directory.GetCurrentDirectory() + @"\Reference\" + dbfPath;
            return ConvertDbf(connectionString, dbfToConvert);
        }
        private void DataTableToCSV(DataTable dt, string csvFile)
        {
            StringBuilder sb = new StringBuilder();
            var columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();
            sb.AppendLine(string.Join(",", columnNames));
            foreach (DataRow row in dt.Rows)
            {
                var fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                for (int i = 0; i < fields.Length; i++)
                {
                    fields[i] = fields[i].Replace("\"", "\"\"");
                    sb.Append("\"" + fields[i].Trim());
                    sb.Append((i != fields.Length - 1) ? "\"," : "\"");
                }
                sb.Append("\r\n");
            }
            File.WriteAllText(csvFile, sb.ToString());
        }

        private string DataTableToCSV(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            var columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();
            sb.AppendLine(string.Join(",", columnNames));
            foreach (DataRow row in dt.Rows)
            {
                var fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                for (int i = 0; i < fields.Length; i++)
                {
                    fields[i] = fields[i].Replace("\"", "\"\"");
                    sb.Append("\"" + fields[i].Trim());
                    sb.Append((i != fields.Length - 1) ? "\"," : "\"");
                }
                sb.Append("\r\n");
            }
            return sb.ToString();
        }

        private void ConvertDbf(string connectionString, string dbfFile, string csvFile)
        {
            string sqlSelect = string.Format("SELECT * FROM {0}", dbfFile);
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbDataAdapter da = new OleDbDataAdapter(sqlSelect, connection))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    DataTableToCSV(ds.Tables[0], csvFile);
                }
            }
        }
        private DataTable ConvertDbf(string connectionString, string dbfFile)
        {
            string sqlSelect = string.Format("SELECT * FROM {0}", dbfFile);
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                using (OleDbDataAdapter da = new OleDbDataAdapter(sqlSelect, connection))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
            }
        }
    }
}
