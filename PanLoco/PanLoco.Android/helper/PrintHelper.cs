using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PanLoco.Helpers;
using Xamarin.Forms;
using PanLoco.Droid.Helper;
using Xamarin.Forms.Platform.Android;
using DroidWebView = Android.Webkit.WebView;

[assembly: Dependency(typeof(PrintHelper))]
namespace PanLoco.Droid.Helper
{
    public class PrintHelper : Activity, Helpers.IPrinter
    {
        public void print(string content)
        {
            try
            {
                
                var printManager = (Android.Print.PrintManager)this.ApplicationContext.GetSystemService(Context.PrintService);
                //var browser = new WebView();
                Android.Webkit.WebView s = new Android.Webkit.WebView(this.ApplicationContext);
                //wv.CreatePrintDocumentAdapter()
                //var htmlSource = new HtmlWebViewSource();
                //htmlSource.Html = content;
                //          htmlSource.Html = @"<html><body>
                //<h1>Xamarin.Forms</h1>
                //<p>Welcome to WebView.</p>
                //</body></html>";
                //WebView s = new WebView(this);
                s.LoadData(content, "text/html", "");

                //browser.Source = htmlSource;
                printManager.Print("testm", s.CreatePrintDocumentAdapter("pipi"), null);
            }
            catch(Exception ex)
            {

            }
        }
        public void Print(WebView viewToPrint)
        {
            try
            {
                var renderer = Platform.CreateRenderer(viewToPrint);
                var webView = renderer.ViewGroup.GetChildAt(0) as DroidWebView;
                if (webView != null)
                {
                    var version = Build.VERSION.SdkInt;
                    if (version >= BuildVersionCodes.Kitkat)
                    {
                        var documentAdapter = webView.CreatePrintDocumentAdapter();
                        var printMgr = (Android.Print.PrintManager)Forms.Context.GetSystemService(Context.PrintService);
                        printMgr.Print("Forms-EZ-Print", documentAdapter, null);
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

    }
}