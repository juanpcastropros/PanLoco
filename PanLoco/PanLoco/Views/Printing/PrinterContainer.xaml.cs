using PanLoco.Helpers;
using PanLoco.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PanLoco.Views.Printing
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrinterContainer : ContentPage
    {
        public PrinterContainer()
        {
            try
            {
                InitializeComponent();
                Label header = new Label
                {
                    Text = "WebView",
                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                    HorizontalOptions = LayoutOptions.Center
                };

                WebView webView = new WebView
                {
                    VerticalOptions = LayoutOptions.FillAndExpand
                };
                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = createPrintDoc(null);

                DependencyService.Get<IBluetooth>().Imprimir("MTP-3", 0, null, null);
                webView.Source = htmlSource;
                //DependencyService.Get<IPrinter>().Print(webView);
                this.Content = new StackLayout
                {
                    Children =
                {
                    header,
                    webView
                }
                };
            }
            catch (Exception ex)
            {
                DisplayAlert("error", ex.Message + "||" + ex.StackTrace, "ok");
            }
        }
        public PrinterContainer(Entrega ent)
        {
            InitializeComponent();
            Label header = new Label
            {
                Text = "Print Vista Previa",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center
            };
            try
            {
                PanLoco.Models.Perfil perfil = App.PerfilDB.GetItemAsync(1).Result;
                DependencyService.Get<IBluetooth>().Imprimir("MTP-3", 0, ent, perfil);
            }
            catch (Exception ex)
            {
                DisplayAlert("Bluethooth ", "No encontramos la impresora", "Ok");
            }
            try
            {
                WebView webView = new WebView
                {
                    VerticalOptions = LayoutOptions.FillAndExpand
                };
                var htmlSource = new HtmlWebViewSource();
                if (ent.ClienteMayorista)
                {
                    htmlSource.Html = createPrintDocMayorista(ent);
                }
                else
                {
                    htmlSource.Html = createPrintDoc(ent);
                }

                webView.Source = htmlSource;
                //DependencyService.Get<IPrinter>().Print(webView);
                this.Content = new StackLayout
                {
                    Children =
                {
                    header,
                    webView
                }
                };
            }
            catch (Exception ex)
            {
                string messag = "";
                Exception er = ex;
                while (er != null)
                {
                    messag += er.Message + "!!!";
                    er = er.InnerException;
                }

                DisplayAlert("Error", messag, "OK");
            }
        }

        private string createPrintDocMayorista(Entrega ent)
        {
            IList<EntregaItemVendido> nor = new List<EntregaItemVendido>();
            IList<EntregaItemVendido> of = new List<EntregaItemVendido>();
            IList<EntregaItemVendido> dev = new List<EntregaItemVendido>();
            StringBuilder st = new StringBuilder();
            if (ent != null)
            {
                if (ent.ItemVendidos.Count > 0)
                {
                    foreach (EntregaItemVendido i in ent.ItemVendidos)
                    {
                        if (i.Oferta && i.CantidadNor > 0)
                            of.Add(i);
                        if (i.Devolucion || i.CantidadDev > 0)
                            dev.Add(i);
                        if (!i.Oferta && i.CantidadNor > 0)
                            nor.Add(i);
                    }
                }
            }

            st.Append("<html>");
            st.Append("<body>");
            st.Append("<table>");
            st.Append("<tr>");
            PanLoco.Models.Perfil perfile = App.PerfilDB.GetItemAsync(1).Result;
            st.Append("<tr><td  colspan=\"2\">" + perfile.FullName + "</td ></tr>");
            st.Append("<tr><td colspan=\"2\">" + perfile.Calle + "</td ></tr>");
            st.Append("<tr><td colspan=\"2\">" + perfile.Localidad + "</td ></tr>");

            if (ent != null)
            {
                st.Append("<tr><td colspan=\"2\">" + perfile.Cuil + "          " + ent.Fecha.ToString() + " </ td ></tr>");
                st.Append("<tr><td colspan=\"2\">" + ent.ClienteNombre + "</td></tr>");
            }
            else
            {
                st.Append("<tr><td>[Cliente]</td><td></td></tr>");
            }
            st.Append("<tr><td colspan=\"2\">Item    Descripción                       Neto</td ></tr>");
            st.Append("<tr><td colspan=\"2\">Cant    Precio                            Neto</td ></tr>");

            double subt = 0;
            if (nor.Count > 0)
            {
                st.Append("</tr><tr><td colspan=\"2\"></td></tr>");
                st.Append("</tr><tr><td colspan=\"2\">Productos</td></tr>");
                st.Append("</tr><tr><td colspan=\"2\"></td></tr>");
            }
            foreach (EntregaItemVendido i in nor)
            {
                st.Append("</tr><tr><td>" + i.Producto.Codigo + "   " + i.Producto.Nombre + "</td><td> $ " + (i.CantidadNor * i.PrecioMayorista) + "</td></tr>");
                st.Append("</tr><tr><td colspan=\"2\">" + i.CantidadNor + " x $ " + i.PrecioMayorista + "</td></tr>");
                subt += (i.CantidadNor * i.PrecioMayorista);
            }
            if (nor.Count() > 0)
            {
                st.Append("</tr><tr><td colspan=\"2\"></td></tr>");
                st.Append("</tr><tr><td>Sub Total Prod</td><td> $ " + subt.ToString("##.##") + "</td></tr>");
                if (ent.ClienteDescuento > 0)
                {
                    st.Append("</tr><tr><td>Descuento " + ent.ClienteDescuento.ToString() + "%</td><td> - $ " + (subt * (ent.ClienteDescuento / 100)).ToString("##.##") + "</td></tr>");
                    subt -= (subt * (ent.ClienteDescuento / 100));
                    st.Append("</tr><tr><td>Sub Total con Desc</td><td> $ " + subt.ToString("##.##") + "</td></tr>");
                }
            }
            if (of.Count > 0)
            {
                st.Append("</tr><tr><td colspan=\"2\"></td></tr>");
                st.Append("</tr><tr><td colspan=\"2\">Ofertas</td></tr>");
                st.Append("</tr><tr><td colspan=\"2\"></td></tr>");
            }
            subt = 0;
            foreach (EntregaItemVendido i in of)
            {
                st.Append("</tr><tr><td>" + i.Producto.Codigo + "   " + i.Producto.Nombre + "</td><td> $" + (i.CantidadNor * i.PrecioMayorista) + "</td></tr>");
                st.Append("</tr><tr><td colspan=\"2\">" + i.CantidadNor + " x $" + i.PrecioMayorista + "</td></tr>");
                subt += (i.CantidadNor * i.PrecioMayorista);
            }
            if (of.Count > 0)
                st.Append("</tr><tr><td>Sub Total Ofertas</td><td> $ " + subt.ToString("##.##") + "</td></tr>");

            if (dev.Count() > 0)
            {
                st.Append("</tr><tr><td colspan=\"2\"></td></tr>");
                st.Append("</tr><tr><td colspan=\"2\"  >Devoluciones</td></tr>");
                st.Append("</tr><tr><td colspan=\"2\"></td></tr>");
            }
            foreach (EntregaItemVendido i in dev)
            {
                st.Append("</tr><tr><td>" + i.Producto.Codigo + "   " + i.Producto.Nombre + "</td><td>$ 0</td></tr>");
                st.Append("</tr><tr><td colspan=\"2\">" + i.CantidadDev + " x $ 0</td></tr>");
            }
            if (ent != null)
            {
                st.Append("<tr><td>TOTAL</td><td>$ " + ent.Total.ToString("##.##") + "</td></tr>");
            }
            st.Append("</table>");
            st.Append("</body>");
            st.Append("</html>");
            return st.ToString();
        }

        string createPrintDoc(Entrega ent)
        {
            IList<EntregaItemVendido> nor = new List<EntregaItemVendido>();
            IList<EntregaItemVendido> of = new List<EntregaItemVendido>();
            IList<EntregaItemVendido> dev = new List<EntregaItemVendido>();
            StringBuilder st = new StringBuilder();
            if (ent != null)
            {
                if (ent.ItemVendidos.Count > 0)
                {
                    foreach (EntregaItemVendido i in ent.ItemVendidos)
                    {
                        if (i.Oferta && i.CantidadNor > 0)
                            of.Add(i);
                        if (i.Devolucion || i.CantidadDev > 0)
                            dev.Add(i);
                        if (!i.Oferta && i.CantidadNor > 0)
                            nor.Add(i);
                    }
                }
            }

            st.Append("<html>");
            st.Append("<body>");
            st.Append("<table>");
            st.Append("<tr>");

            PanLoco.Models.Perfil perfile = App.PerfilDB.GetItemAsync(1).Result;
            st.Append("<tr><td  colspan=\"2\">" + perfile.FullName + "</td ></tr>");
            st.Append("<tr><td colspan=\"2\">" + perfile.Calle + "</td ></tr>");
            st.Append("<tr><td colspan=\"2\">" + perfile.Localidad + "</td ></tr>");

            if (ent != null)
            {
                st.Append("<tr><td colspan=\"2\">" + perfile.Cuil + "          " + ent.Fecha.ToString() + " </ td ></tr>");
                st.Append("<tr><td colspan=\"2\">" + ent.ClienteNombre + "</td></tr>");
            }
            else
            {
                st.Append("<tr><td>[Cliente]</td><td></td></tr>");
            }
            st.Append("<tr><td colspan=\"2\">Item    Descripción                       Neto</td ></tr>");
            st.Append("<tr><td colspan=\"2\">Cant    Precio                          Neto</td ></tr>");

            double subt = 0;
            if (nor.Count > 0)
            {
                st.Append("</tr><tr><td colspan=\"2\"></td></tr>");
                st.Append("</tr><tr><td colspan=\"2\">Productos</td></tr>");
                st.Append("</tr><tr><td colspan=\"2\"></td></tr>");
            }
            foreach (EntregaItemVendido i in nor)
            {
                st.Append("</tr><tr><td>" + i.Producto.Codigo + "   " + i.Producto.Nombre + "</td><td> $ " + (i.CantidadNor * i.PrecioUnitario) + "</td></tr>");
                st.Append("</tr><tr><td colspan=\"2\">" + i.CantidadNor + " x $ " + i.PrecioUnitario + "</td></tr>");
                subt += (i.CantidadNor * i.PrecioUnitario);
            }
            if (nor.Count() > 0)
            {
                st.Append("</tr><tr><td colspan=\"2\"></td></tr>");
                st.Append("</tr><tr><td>Sub Total Prod</td><td> $ " + subt.ToString("##.##") + "</td></tr>");
                if (ent.ClienteDescuento > 0)
                {
                    st.Append("</tr><tr><td>Descuento " + ent.ClienteDescuento.ToString() + "%</td><td> - $ " + (subt * (ent.ClienteDescuento / 100)).ToString("##.##") + "</td></tr>");
                    subt -= (subt * (ent.ClienteDescuento / 100));
                    st.Append("</tr><tr><td>Sub Total con Desc</td><td> $ " + subt.ToString("##.##") + "</td></tr>");
                }
            }
            if (of.Count > 0)
            {
                st.Append("</tr><tr><td colspan=\"2\"></td></tr>");
                st.Append("</tr><tr><td colspan=\"2\">Ofertas</td></tr>");
                st.Append("</tr><tr><td colspan=\"2\"></td></tr>");
            }
            subt = 0;
            foreach (EntregaItemVendido i in of)
            {
                st.Append("</tr><tr><td>" + i.Producto.Codigo + "   " + i.Producto.Nombre + "</td><td> $" + (i.CantidadNor * i.PrecioOferta) + "</td></tr>");
                st.Append("</tr><tr><td colspan=\"2\">" + i.CantidadNor + " x $" + i.PrecioOferta + "</td></tr>");
                subt += (i.CantidadNor * i.PrecioOferta);
            }
            if (of.Count > 0)
                st.Append("</tr><tr><td>Sub Total Ofertas</td><td> $ " + subt.ToString("##.##") + "</td></tr>");

            if (dev.Count() > 0)
            {
                st.Append("</tr><tr><td colspan=\"2\"></td></tr>");
                st.Append("</tr><tr><td colspan=\"2\"  >Devoluciones</td></tr>");
                st.Append("</tr><tr><td colspan=\"2\"></td></tr>");
            }
            foreach (EntregaItemVendido i in dev)
            {
                st.Append("</tr><tr><td>" + i.Producto.Codigo + "   " + i.Producto.Nombre + "</td><td>$ 0</td></tr>");
                st.Append("</tr><tr><td colspan=\"2\">" + i.CantidadDev + " x $ 0</td></tr>");
            }
            if (ent != null)
            {
                st.Append("<tr><td>TOTAL</td><td>$ " + ent.Total.ToString("##.##") + "</td></tr>");
            }
            st.Append("</table>");
            st.Append("</body>");
            st.Append("</html>");
            return st.ToString();
        }
    }

    //class PrinterContainerViewModel : INotifyPropertyChanged
    //{

    //    public PrinterContainerViewModel()
    //    {
    //        IncreaseCountCommand = new Command(IncreaseCount);
    //    }

    //    int count;

    //    string countDisplay = "You clicked 0 times.";
    //    public string CountDisplay
    //    {
    //        get { return countDisplay; }
    //        set { countDisplay = value; OnPropertyChanged(); }
    //    }

    //    public ICommand IncreaseCountCommand { get; }

    //    void IncreaseCount() =>
    //        CountDisplay = $"You clicked {++count} times";


    //    public event PropertyChangedEventHandler PropertyChanged;
    //    void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    //}
}
