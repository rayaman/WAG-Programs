using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using CsvHelper;
using System.Globalization;
using System.Diagnostics;
using System.Text;

namespace Auction_Import_Helper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Directory.CreateDirectory(localDir+@"\temp");
            File.Delete(localDir + @"\temp\IBR.csv"); 
            File.Delete(localDir + @"\temp\IEOAR.csv");
            File.Delete(localDir + @"\temp\LAFR.csv");
        }
        private string localDir = Directory.GetCurrentDirectory();
        private string path_IBR = "";
        private string path_IEOAR = "";
        private string path_LAFR = "";
        private OpenFileDialog openFileDialog = new OpenFileDialog();
        private string SelectFile(string title)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;
            openFileDialog.Title = title;
            openFileDialog.Filter = "Excel Files (*.xls)|*.xls";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ShowDialog();
            return openFileDialog.FileName;
        }
        public int ConvertExcelToCsv(string excelFilePath, string csvOutputFile, int worksheetNumber = 1)
        {
            var cnnStr = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;IMEX=1;HDR=NO\"", excelFilePath);
            var cnn = new OleDbConnection(cnnStr);
            var dt = new DataTable();
            try
            {
                cnn.Open();
                var schemaTable = cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (schemaTable.Rows.Count < worksheetNumber) throw new ArgumentException("The worksheet number provided cannot be found in the spreadsheet");
                string worksheet = schemaTable.Rows[worksheetNumber - 1]["table_name"].ToString().Replace("'", "");
                string sql = String.Format("select * from [{0}]", worksheet);
                var da = new OleDbDataAdapter(sql, cnn);
                da.Fill(dt);
            }
            catch (Exception e)
            {
                return 1;
            }
            finally
            {
                cnn.Close();
            }

            // write out CSV data
            using (var wtr = new StreamWriter(csvOutputFile))
            {
                foreach (DataRow row in dt.Rows)
                {
                    bool firstLine = true;
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (!firstLine) { wtr.Write(","); } else { firstLine = false; }
                        var data = row[col.ColumnName].ToString().Replace("\"", "\"\"");
                        wtr.Write(string.Format("\"{0}\"", data));
                    }
                    wtr.WriteLine();
                }
            }
            return 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //IBR
            string file = SelectFile("Please select the Invaluble Bidder Reoprt file.");
            int err = ConvertExcelToCsv(file, localDir + @"\temp\IBR.csv");
            if (err==1)
            {
                System.Windows.MessageBox.Show("Not a valid excel file!");
                return;
            }
            IBR.Text = file;
            path_IBR = file;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //IEOAR
            string file = SelectFile("Please select the Invaluble EOA Reoprt file.");
            int err = ConvertExcelToCsv(file, localDir + @"\temp\IEOAR.csv");
            if (err == 1)
            {
                System.Windows.MessageBox.Show("Not a valid excel file!");
                return;
            }
            IEOAR.Text = file;
            path_IEOAR = file;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //LAFR
            string file = SelectFile("Please select the Liveauctioneers Full Report File.");
            int err = ConvertExcelToCsv(file, localDir + @"\temp\LAFR.csv");
            if (err == 1)
            {
                System.Windows.MessageBox.Show("Not a valid excel file!");
                return;
            }
            LAFR.Text = file;
            path_LAFR = file;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (Disable_Inv.IsChecked == true && Disable_Live.IsChecked == true)
            {
                System.Windows.MessageBox.Show("You cannot disable both Liveauctioneers and Invaluable!");
                return;
            }
            if ((path_IBR == "" && Disable_Inv.IsChecked == false) || (path_IEOAR == "" && Disable_Inv.IsChecked == false) || (path_LAFR == "" && Disable_Live.IsChecked == false))
            {
                System.Windows.MessageBox.Show("Select each file before trying to process data!");
                return;
            }
            string saveDir = DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture) + " " + DateTime.Now.Year + " AuctionData";
            Directory.CreateDirectory(localDir + "\\" + saveDir);
            var CustomerList = new List<CustomersRecord> { };
            var BidInfoList = new List<BidInfo> { };
            if (Disable_Inv.IsChecked == false)
            {
                using (var reader = new StreamReader(localDir + @"\temp\IBR.csv"))
                using (var csv = new CsvReader(reader))
                {
                    var records = csv.GetRecords<IBRecord>();
                    foreach (IBRecord temp in records)
                    {
                        CustomerList.Add(temp.convertCustomers());
                    }
                }
                using (var reader = new StreamReader(localDir + @"\temp\IEOAR.csv"))
                using (var csv = new CsvReader(reader))
                {
                    csv.Configuration.HasHeaderRecord = false;
                    var records = csv.GetRecords<IEOARecord>();
                    foreach (IEOARecord temp in records)
                    {
                        BidInfoList.Add(temp.convertBidInfo());
                    }
                }
            }
            if (Disable_Live.IsChecked==false)
            {
                using (var reader = new StreamReader(localDir + @"\temp\LAFR.csv"))
                using (var csv = new CsvReader(reader))
                {
                    var records = csv.GetRecords<LAFRecord>();
                    foreach (LAFRecord temp in records)
                    {
                        if (!CustomersRecord.Reference.ContainsKey(temp.Email))
                            CustomerList.Add(temp.convertCustomers());
                        if (!(temp.First == ""))
                        {
                            BidInfoList.Add(temp.convertBidInfo());
                        }
                    }
                }
            }
            using (var writer = new StreamWriter(localDir + "\\" + saveDir + @"\CustomerInfo.csv"))
            using (var csvw = new CsvWriter(writer))
            {
                csvw.Configuration.RegisterClassMap<CustomersRecordMap>();
                csvw.WriteRecords(CustomerList);
            }
            using (var writer = new StreamWriter(localDir + "\\" + saveDir + @"\BidInfo.csv"))
            using (var csvw = new CsvWriter(writer))
            {
                csvw.Configuration.RegisterClassMap<BidInfoMap>();
                csvw.WriteRecords(BidInfoList);
            }
            StringBuilder str = new StringBuilder();
            str.AppendLine("The Following List of people may have an issue being entered into the system! This will be written to a file named: problematic.txt");
            foreach(LAFRecord l in LAFRecord.problematic)
            {
                str.AppendLine("BidCard: "+l.BidNum+" "+l.First+" "+l.Last);
            }
            foreach (IBRecord l in IBRecord.problematic)
            {
                str.AppendLine("BidCard: " + l.BidNum + " " + l.Name);
            }
            System.Windows.MessageBox.Show(str.ToString());
            File.WriteAllText(localDir + "\\" + saveDir + @"\problematic.txt", str.ToString());
            Process.Start("explorer.exe", saveDir);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Window1 help = new Window1();
            help.Show();
        }

        private void Disable_Live_Checked(object sender, RoutedEventArgs e)
        {
            Browse_LAFR.IsEnabled = false;
        }

        private void Disable_Inv_Checked(object sender, RoutedEventArgs e)
        {
            Browse_IBR.IsEnabled = false;
            Browse_IEOAR.IsEnabled = false;
        }
        private void Disable_Live_UnChecked(object sender, RoutedEventArgs e)
        {
            Browse_LAFR.IsEnabled = true;
        }

        private void Disable_Inv_UnChecked(object sender, RoutedEventArgs e)
        {
            Browse_IBR.IsEnabled = true;
            Browse_IEOAR.IsEnabled = true;
        }
    }
}
