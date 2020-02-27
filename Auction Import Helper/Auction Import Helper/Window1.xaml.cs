using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Auction_Import_Helper
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private List<BitmapImage> images = new List<BitmapImage>();
        private int pos = 0;
        private string[] assemblies = {
            "Invaluable Step 1",
            "Invaluable Step 2",
            "Invaluable Step 3",
            "Invaluable Step 4",
            "Live Auctioneers Step 1",
            "Live Auctioneers Step 2",
            "Importing Records Step 1",
            "Importing Records Step 2",
            "Importing Records Step 3",
            "Importing Records Step 4",
            "Importing Records Step 5",
            "AuctionFlex Import Customers Step 1",
            "AuctionFlex Import Customers Step 2",
            "AuctionFlex Import Customers Step 3",
            "AuctionFlex Import Customers Step 4",
            "AuctionFlex Import Customers Step 5",
            "AuctionFlex Import Customers Step 6",
            "AuctionFlex Import Customers Step 7",
            "BidInfo Records Step 1",
            "BidInfo Records Step 2",
            "BidInfo Records Step 3",
            "BidInfo Records Step 4",
            "BidInfo Records Step 5",
            "BidInfo Records Step 6",
            "Auto Clerking Bids Step 1",
            "Reserve And BidExt Step 1",
            "Reserve And BidExt Step 2",
            "Reserve And BidExt Step 3",
            "Reserve And BidExt Step 4"
        };
        private string[] helps =
        {
            @"Importing customers and bids help - Getting required files from Invaluable

Notice: Invaluables (Refer to the pictures on the right for assistance as you move through the images the steps on the dialogue will change)
Step 1: Log into Invaluable and go to 'Auctions/Sales'
Step 2: From there select 'Past'
Step 3: We see a list of auctions past, we want the most recent one. Select 'View sale results'
Step 4: On this menu we need to download 2 files 'Bidder Report' and 'EOA Report' Save these files to a location that is easy to get to. I suggest storing all of these files into a folder for easy access.",
            @"Importing customers and bids help - Getting required files from Liveauctioneers

Step 1: Log into Liveauctioneers and to to 'Post Auction' Select the most recent auction
Step 2: Go to 'EOA' and on that page select 'Full Report' and save it in the same location as the other files",
            @"Importing customers and bids help - Processing the files

Notice: Having all the files togeher will make this a much faster process
Step 1: Hit the browse button for the file you are trying to process. 
The first browse gets you the Invaluable Bidder Report.
The second browse gets you the Invaluable EOA Report
The Third browse gets you the Liveauctioneers Full Report
Step 2: Locate the files for each area, ensure the files you are selecting are for the correct auction!!!
Step 3: When a selection has been made you should see the field show the path to the file
Step 4: An example of all the fields being populated
Step 5: Once you have all of your files press 'Process Files' and when the program is done doing its thing it will open up a directory containing the generated files. Feel free to close this program at this time.",
            @"Importing customers and bids help - Importing Customers into AuctionFlex

Step 1: Goto 'Check-In' and insure that you are on the correct auction, then hit Import customers.
Step 2: You may want to do a back up of your data first!
Step 3: We need to select our generated files. The folder that opened up earlier should show where you should look to access the generated files. The file name we want is 'customerinfo.csv'
Step 4: This menu should already be set up on Auction Flex, but incase it isn't make your Auction Flex match what the image to the left shows
Step 5: On this menu make sure everything lines up and that  'Skip First Row When Importing' is selected
Step 6: Make sure your duplicate page looks the same as the image to the left
Step 7: Finally select 'Import Customers'",
            @"Importing customers and bids help - Importing Bid Info into AuctionFlex

Step 1: Goto 'Check-In' and insure that you are on the correct auction, then hit 'Import Alternate bids'. You may want to do a back up of your data first!
Step 2: Locate and select the 'BidInfo' file
Step 3: Ensure your meun fields look like the image to the left
Step 4: Ensure that 'Set bid start same as bid max...' is Selected
Step 5: Ensure the fields look correct
Step 6: Select Import Alt Bids and you are done",
            @"Importing customers and bids help - AutoClearking the bids

Step 1: Click that button and follow the steps provided by auctionflex",
            @"Importing customers and bids help - Reserve And BidExt
Note: Hitting Esc or clicking outside of the menu will close the menu and you will have to start over!
Step 1: If items were sold in the auction even though they didn't meet the reserve, we will see a menu pop up that will allow us to manually enter that data in.
Step 2: Locate each occurrence of 'Reserve not met by #...' This will be under the notes column.
Step 3: Set the 'Auto-Clerk BidCard#' the right most column to the value of the bidcard# in the notes section. Refer to the diagram and colors to see what should match with what.
Step 4: When the reserve is also not met we have to set the 'Auto-Clerk Bid Ext' to the value of 'Bidmax' the blue colored text in the diagram.
Step 5: Repeat previous steps until each occurrence has been resolved.
Step 6: Once done hit Esc to exit the menu"
        };
        public Window1()
        {
            InitializeComponent();
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            Stream myStream;
            foreach (string img in assemblies)
            {
                myStream = myAssembly.GetManifestResourceStream("Auction_Import_Helper.Properties." + img + ".PNG");
                images.Add(Convert(new Bitmap(myStream)));
            }
            ImageHolder.Source = images[pos];
            display.Content = assemblies[pos];
            HelpDisp.Text = helps[0];
        }
        public BitmapImage Convert(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }
        private void Checker()
        {
            if (assemblies[pos].Contains("Invaluable"))
            {
                HelpDisp.Text = helps[0];
            } else if (assemblies[pos].Contains("Live")) {
                HelpDisp.Text = helps[1];
            } else if (assemblies[pos].Contains("Importing")) {
                HelpDisp.Text = helps[2];
            } else if (assemblies[pos].Contains("Customers")) {
                HelpDisp.Text = helps[3];
            } else if (assemblies[pos].Contains("BidInfo")) {
                HelpDisp.Text = helps[4];
            } else if (assemblies[pos].Contains("Clerking")) {
                HelpDisp.Text = helps[5];
            } else if (assemblies[pos].Contains("Reserve")) {
                HelpDisp.Text = helps[6];
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pos--;
            if (pos < 0)
            {
                pos = 0;
            }
            ImageHolder.Source = images[pos];
            display.Content = assemblies[pos];
            Checker();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            pos++;
            if (pos > assemblies.Length-1)
            {
                pos = assemblies.Length - 1;
            }
            ImageHolder.Source = images[pos];
            display.Content = assemblies[pos];
            Checker();
        }
    }
}
