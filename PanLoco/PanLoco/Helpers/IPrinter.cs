using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PanLoco.Helpers
{
    public interface IPrinter
    {
        void print(string content);
        void Print(WebView viewToPrint);
    }
}
