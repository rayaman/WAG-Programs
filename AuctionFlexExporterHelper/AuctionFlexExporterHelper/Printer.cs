using System;
using System.Windows.Forms; // In order to reference the WebBrowser class.
using System.Threading;  // In order to perform threading.
using System.IO;

// This console project needs to reference 3 assemblies :
// 1. System.Windows.Forms.dll
// 2. SHDocVw.dll (the "Microsoft Internet Controls" interop assembly)
// 3. System.Web.dll

namespace AuctionFlexDBConnector
{
    class Printer
    {
        public static void WB_OnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Console.WriteLine("Now printing document.");

            WebBrowser wb = sender as WebBrowser;

            SHDocVw.InternetExplorer ie = (SHDocVw.InternetExplorer)(wb.ActiveXInstance);

            ie.PrintTemplateInstantiation +=
              new SHDocVw.DWebBrowserEvents2_PrintTemplateInstantiationEventHandler(IE_OnPrintTemplateInstantiation);

            ie.PrintTemplateTeardown +=
              new SHDocVw.DWebBrowserEvents2_PrintTemplateTeardownEventHandler(IE_OnPrintTemplateTeardown);

            ie.PutProperty("WebBrowserControl", (object)wb);

            wb.Print();
        }

        public static void IE_OnPrintTemplateInstantiation(object pDisp)
        {
            Console.WriteLine("Printing started.");
        }

        public static void IE_OnPrintTemplateTeardown(object pDisp)
        {
            Console.WriteLine("Printing ended.");

            SHDocVw.IWebBrowser2 iwb2 = pDisp as SHDocVw.IWebBrowser2;
            WebBrowser wb = (WebBrowser)(iwb2.GetProperty("WebBrowserControl"));

            wb.Dispose();

            Application.ExitThread();
        }

        public static void Print(string path, bool file = true)
        {
            string strHTML;
            Thread th = new Thread(() =>
            {
                WebBrowser wb = new WebBrowser();

                wb.DocumentCompleted += WB_OnDocumentCompleted;

                if (!file)
                {
                    strHTML = path;
                }
                else
                {
                    strHTML = File.ReadAllText(path);
                }

                Console.WriteLine(strHTML);

                File.WriteAllText(Directory.GetCurrentDirectory() + "\\output.html", strHTML);

                wb.Navigate(Directory.GetCurrentDirectory() + "\\output.html");

                Application.Run();
            });

            th.SetApartmentState(ApartmentState.STA);
            th.Name = "WebBrowserPrint";
            th.Start();
        }
    }
}