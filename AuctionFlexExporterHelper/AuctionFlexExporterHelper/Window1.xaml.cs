using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using AuctionFlexDBConnector;
using CsvHelper;

namespace AuctionFlexExporterHelper
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private int count;
        private Auction SelectedAuction;
        private List<Item> items;
        private afdb auction;
        private bool cc;
        public Window1()
        {
            if (!Directory.Exists("C:\\AuctionFlex"))
            {
                System.Windows.Forms.MessageBox.Show("This program only works on the main Auction Flex Computer!");
                Environment.Exit(0);
            }
            InitializeComponent();
            invbox.KeyDown += new System.Windows.Input.KeyEventHandler(invbox_keyDown);
            progress.Minimum = 0;
            progress.Maximum = 100;
            selectedAuction.IsEnabled = false;
            auction = new afdb();
            var list = auction.GetAuctionList();
            for(int i = list.Count-1;i>=0;i--)
                listbox.Items.Add(list[i]);
        }
        public void UpdateProgress()
        {
            Dispatcher.Invoke(() =>
            {
                progress.Value = ((double)count / (double)items.Count) * 100;
                if (count == items.Count)
                {
                    progress.Value = 0;
                    new Thread(() =>
                    {
                        Thread.Sleep(2000);
                        if (cc)
                            Process.Start(Directory.GetCurrentDirectory() + "\\Auctions\\" + SelectedAuction.name.Trim());
                        this.count = 0;
                    }).Start();
                    export.IsEnabled = true;
                    Refresh.IsEnabled = true;
                    listbox.IsEnabled = true;
                    upload.IsEnabled = true;
                }
            });
        }
        public void UpdateProg()
        {
            Dispatcher.Invoke(() =>
            {
                progress.Value = ((double)counter / (double)max) * 100;
            });
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedAuction = (Auction)e.AddedItems[0];
            selectedAuction.Text = SelectedAuction.name;
            items = auction.GetItemsByAuctionID(SelectedAuction.id);
            auctionlots.Items.Clear();
            lotcount.Content = "Total: "+items.Count;
            foreach(Item i in items)
            {
                auctionlots.Items.Add(i);
            }
        }
        static System.Drawing.Image FixedSize(System.Drawing.Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height,
                              PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WriteFiles();
        }
        private void CopyImages(Item i,string path)
        {
            int c = 1;
            foreach (Picture p in i.pic)
            {
                if (File.Exists(path + @"\Images\" + i.lotnum + "_" + c + ".jpg"))
                {
                    var img = System.Drawing.Image.FromFile(@"C:\AuctionFlex\Images\" + p.webpath);
                    if (img.Width<1000 && img.Height>1000)
                        FixedSize(img, 1500, 1500).Save(path + @"\Images\" + i.lotnum + "a_" + c + ".jpg");
                    else if (img.Height<1000 && img.Width > 1000)
                        FixedSize(img, 1500, 1500).Save(path + @"\Images\" + i.lotnum + "a_" + c + ".jpg");
                    else if (img.Height < 1000 && img.Width < 1000)
                        FixedSize(img, 1500, 1500).Save(path + @"\Images\" + i.lotnum + "a_" + c + ".jpg");
                    else
                        File.Copy(@"C:\AuctionFlex\Images\" + p.webpath, path + @"\Images\" + i.lotnum + "a_" + c + ".jpg");
                    img.Dispose();
                }
                else
                {
                    try
                    {
                        var img = System.Drawing.Image.FromFile(@"C:\AuctionFlex\Images\" + p.webpath);
                        if (img.Width < 1000 && img.Height > 1000)
                            FixedSize(img, 1500, 1500).Save(path + @"\Images\" + i.lotnum + "_" + c + ".jpg");
                        else if (img.Height < 1000 && img.Width > 1000)
                            FixedSize(img, 1500, 1500).Save(path + @"\Images\" + i.lotnum + "_" + c + ".jpg");
                        else if (img.Height < 1000 && img.Width < 1000)
                            FixedSize(img, 1500, 1500).Save(path + @"\Images\" + i.lotnum + "_" + c + ".jpg");
                        else
                            File.Copy(@"C:\AuctionFlex\Images\" + p.webpath, path + @"\Images\" + i.lotnum + "_" + c + ".jpg");
                        img.Dispose();
                    }
                    catch(Exception e)
                    {
                        System.Windows.MessageBox.Show(e.Message);
                        //Do Nothing
                    }
                }
                c++;
                Thread.Sleep(200);
            }
            Interlocked.Increment(ref count);
            UpdateProgress();
        }
        private class StringBuffer
        {
            private List<string> str = new List<string>();
            public void Append(string s)
            {
                lock(str)
                {
                    str.Add(s);
                }
            }
            public override string ToString()
            {
                string s = "";
                foreach(string ss in str)
                {
                    s += ss;
                }
                return s;
            }
        }
        private int counter = 0;
        private int max;
        private void ManageString(StringBuffer buf,string str)
        {
            Debug.WriteLine(str);
            buf.Append(str);
            Interlocked.Increment(ref counter);
            UpdateProg();
        }
        private void SendData(string data)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.worldauctiongallery.com/sendResults");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }
        private void DoInvoiceWork(StringBuffer str, Item inv)
        {
            var s = new StringBuilder();
            var invoice = auction.GetInvoiceItemFromItemID(inv.id);
            if (auction.itemIsInvoiced(inv.id))
            {
                // Sold
                s.Append("[");
                s.Append("\"Sold\"");
                s.Append(",");
                s.Append(inv.lotnum);
                s.Append(",");
                s.Append(invoice.baseprice);
                s.Append("],");
            }
            else
            {
                // Unsold
                s.Append("[");
                s.Append("\"Unsold\"");
                s.Append(",");
                s.Append(inv.lotnum);
                s.Append("],");
            }
            if (inv.lotnum == max)
            {
                s.Remove(s.Length-1, 1);
            }
            ManageString(str, s.ToString());
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            DialogResult result;
            if (SelectedAuction == null)
            {
                System.Windows.Forms.MessageBox.Show("Please Select an auction to upload");
            }
            else
            {
                result = System.Windows.Forms.MessageBox.Show("Has \"" + SelectedAuction.name.Trim() + "\" been completely clerked Online and In house? And have all Invoices been checked out?", "Auction Complete?", MessageBoxButtons.YesNo);
                if (result.ToString() == "Yes")
                {
                    export.IsEnabled = false;
                    Refresh.IsEnabled = false;
                    listbox.IsEnabled = false;
                    upload.IsEnabled = false;
                    new Thread(() =>
                    {
                        var str = new StringBuffer();
                        str.Append("{\"Auction\":\"");
                        str.Append(SelectedAuction.name.Trim());
                        str.Append("\",\"lots\":[");
                        counter = 0;
                        max = items.Count;
                        foreach (Item inv in items)
                        {
                            Thread.Sleep(50);
                            Thread temp = new Thread(() => DoInvoiceWork(str, inv));
                            temp.Start();
                        }
                        while (counter != max)
                        {
                            Thread.Sleep(200);
                        }
                        str.Append("]}");
                        //File.WriteAllText("test.json", str.ToString());
                        try
                        {
                            SendData(str.ToString());
                        } catch
                        {
                            //
                        }
                        Dispatcher.Invoke(() =>
                        {
                            export.IsEnabled = true;
                            Refresh.IsEnabled = true;
                            listbox.IsEnabled = true;
                            upload.IsEnabled = true;
                        });
                    }).Start();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Once the invoicing is completed for this auction please upload the sales so I can update the website with the results!");
                }
            }
        }
        private void WriteFiles()
        {
            if (SelectedAuction == null)
            {
                System.Windows.Forms.MessageBox.Show("Please Select an Auction before Trying to Export!");
                return;
            }
            export.IsEnabled = false;
            Refresh.IsEnabled = false;
            listbox.IsEnabled = false;
            upload.IsEnabled = false;
            string path = Directory.GetCurrentDirectory() + "\\Auctions\\" + SelectedAuction.name.Trim();
            if (Directory.Exists(path))
            {
                var dir = new DirectoryInfo(path);
                dir.Delete(true);
                Directory.CreateDirectory(path + @"\Images");
            }
            cc = (bool)check.IsChecked;
            Directory.CreateDirectory(path + @"\Images");
            List<Invaluable> InvRecords = new List<Invaluable>();
            List<Liveauctioneers> LiveRecords = new List<Liveauctioneers>();
            string disclaimer = "Please note the absence of a condition report does not imply that there are no condition issues with this lot. Please contact us for a detailed condition report.";
            if (File.Exists("disclaimer.txt"))
            {
                File.ReadAllText("disclaimer.txt");
            } else
            {
                File.WriteAllText("disclaimer.txt", disclaimer);
            }
            new Thread(() =>
            {
                foreach (Item i in items)
                {
                    Thread.Sleep(300);
                    Thread myNewThread = new Thread(() => CopyImages(i, path));
                    myNewThread.Start();
                    InvRecords.Add(new Invaluable(i.lotnum.ToString(), i.lead, i.desc + "\n\n" + disclaimer, i.min.ToString(), i.max.ToString()));
                    LiveRecords.Add(new Liveauctioneers(i.lotnum.ToString(), i.lead, i.desc + "\n\n" + disclaimer, i.min.ToString(), i.max.ToString()));
                }
                using (var writer = new StreamWriter(path + @"\liveauctioneersCatalog.csv"))
                using (var csvw = new CsvWriter(writer))
                {
                    csvw.Configuration.RegisterClassMap<LivauctioneersMap>();
                    csvw.WriteRecords(LiveRecords);
                }
                using (var writer = new StreamWriter(path + @"\InvaluablesCatalog.csv"))
                using (var csvw = new CsvWriter(writer))
                {
                    csvw.Configuration.RegisterClassMap<InvaluableMap>();
                    csvw.WriteRecords(InvRecords);
                }
            }).Start();
        }
        private List<Picture> pics;
        private List<BitmapImage> imgs;
        private int pos = 0;
        private Item item;
        private void Work(Item inv)
        {
            if (inv == null)
                return;
            info.Text = "";
            if(inv.lotnum!=0)
                invdetail.Text = inv.invnum + ": "+inv.lead.Trim() + " Lot: "+inv.lotnum;
            else
                invdetail.Text = inv.invnum + ": " + inv.lead.Trim();
            item = inv;
            imgs = new List<BitmapImage>();
            desc.Text = inv.desc;
            pics = inv.pic;
            foreach (Picture p in pics)
            {
                imgs.Add(new BitmapImage(new Uri(@"C:\AuctionFlex\Images\" + p.webpath, UriKind.Absolute)));
            }
            if (imgs.Count > 0)
                pic.Source = imgs[0];
            else
                pic.Source = null;
            var items = auction.GetItemListByCO(inv.conum);
            var b = auction.GetBuyerFromCO(inv.conum);
            consignor.Text = b.first.Trim() + " " + b.last.Trim();
            otheritems.Items.Clear();
            reserve.Text = "Reserve: " + (int)inv.reserve;
            estmax.Text = ((int)inv.max).ToString();
            estmin.Text = ((int)inv.min).ToString();
            foreach (Item i in items)
                otheritems.Items.Add(i);
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (item == null)
                return;
            Item inv = item;
            StringBuilder str = new StringBuilder();
            var invoice = auction.GetInvoiceItemFromItemID(inv.id);
            if (inv.onhand == 0 && auction.itemIsInvoiced(inv.id))
                str.Append("Sold ");
            else if (inv.onhand != 0 && auction.itemIsInvoiced(inv.id))
                str.Append("Sold ");
            else
                str.Append("Unsold ");
            if (invoice != null)
            {
                var i = auction.GetInvoiceFromInvoiceNum(invoice.invoicenum);
                if (!auction.InvoiceIsPaid(invoice.invoicenum))
                    str.Append("(Unpaid): ");
                else
                    str.Append("(Paid): ");
                str.Append("$" + invoice.baseprice);
                var b = auction.GetBuyerByID(i.buyer_id);
                str.Append(" Buyer: "+b.first + " " +b.last);
            }
            info.Text = str.ToString();
        }
        private void auctionlots_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count>0)
                Work((Item)e.AddedItems[0]);
        }
        private void select_Click(object sender, RoutedEventArgs e)
        {
            if (otheritems.SelectedItem == null)
                return;
            Work((Item)otheritems.SelectedItem);
        }

        private void pic_left_Click(object sender, RoutedEventArgs e)
        {
            if (imgs == null)
                return;
            if (imgs.Count == 0)
                return;
            pos--;
            if (pos < 0)
                pos = imgs.Count - 1;
            if (imgs.Count == 1)
                pos = 0;
            pic.Source = imgs[pos];
        }

        private void pic_right_Click(object sender, RoutedEventArgs e)
        {
            if (imgs == null)
                return;
            if (imgs.Count == 0)
                return;
            pos++;
            if (pos > imgs.Count - 1)
                pos = 0;
            if (imgs.Count == 1)
                pos = 0;
            pic.Source = imgs[pos];
        }
        private void invbox_keyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int id;
                if (int.TryParse(invbox.Text, out id))
                {
                    var item = auction.GetItemByInvNum(id);
                    if (item == null)
                        System.Windows.Forms.MessageBox.Show("Inventory: " + invbox.Text + " does not exist!");
                    invbox.Text = "";
                    Work(item);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(invbox.Text + " is not a valid inventory item!");
                    invbox.Text = "";
                }
            }
        }
        private void gotoinv_Click(object sender, RoutedEventArgs e)
        {
            int id;
            if(int.TryParse(invbox.Text, out id))
            {
                var item = auction.GetItemByInvNum(id);
                if (item == null)
                    System.Windows.Forms.MessageBox.Show("Inventory: " + invbox.Text + " does not exist!");
                invbox.Text = "";
                Work(item);
            } else
            {
                System.Windows.Forms.MessageBox.Show(invbox.Text+" is not a valid inventory item!");
                invbox.Text = "";
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            auction = new afdb();
        }
    }
}
