using PanLoco.Helpers;
using PanLoco.Models;
using PanLoco.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PanLoco.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntregaAddProducto : ContentPage
    {
        bool esMayorista = false;
        EntregaItemVendido item = new EntregaItemVendido();
        EntregaCreacionViewModel vmParent;
        private string message = string.Empty;
        public EntregaAddProducto(EntregaCreacionViewModel viewModel, bool EsMayorista)
        {
            try
            {
                vmParent = viewModel;
                InitializeComponent();
                BindingContext = this;
                esMayorista = EsMayorista;
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
        public EntregaAddProducto(EntregaCreacionViewModel viewModel, bool EsMayorista, EntregaItemVendido _item)
        {
            try
            {
                vmParent = viewModel;

                item = _item;
                BindingContext = this;
                InitializeComponent();
                this.cta_eliminar.IsEnabled = true;
                this.cta_agregar.Text = "Guardar";
                esMayorista = EsMayorista;
                this.SC_Devolucion.On = item.Devolucion;
                this.SC_Oferta.On = item.Oferta;
                this.ProdCtaSrc.Text = item.Cantidad.ToString();
                this.SubTotal.Text = item.SubTotal.ToString();
                this.ProdcutoSrc.Text = item.Producto.Codigo;
                this.ProdcutoSrc.IsEnabled = false;
                vmParent.ProductoIncrementarStockPorEdicion(item.Producto.Codigo, item.Cantidad);
                ProductoSelected.Text = string.Concat(item.Producto.Codigo, " - ", item.Producto.Nombre, " - $ ", item.PrecioUnitario);
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

        private void SC_Devolucion_OnChanged(object sender, ToggledEventArgs e)
        {
            try
            {
                item.Devolucion = e.Value;
                if (e.Value)
                {
                    item.PrecioUnitario = vmParent.GetPrecio(item.Producto, SC_Devolucion.On, SC_Oferta.On);
                    ProductoSelected.Text = string.Concat(item.Producto.Codigo, " - ", item.Producto.Nombre, " - $ ", item.PrecioUnitario);
                    SC_Oferta.On = false;
                    this.SubTotal.Text = "";
                }
                else
                {
                    item.PrecioUnitario = vmParent.GetPrecio(item.Producto, SC_Devolucion.On, SC_Oferta.On);
                    ProductoSelected.Text = string.Concat(item.Producto.Codigo, " - ", item.Producto.Nombre, " - $ ", item.PrecioUnitario);
                    int cta = 0;
                    if (int.TryParse(ProdCtaSrc.Text, out cta))
                    {
                        item.Cantidad = cta;
                        this.SubTotal.Text = item.SubTotal.ToString();
                    }
                }
                SC_Oferta.IsEnabled = !e.Value;
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
                ((Entry)sender).TextColor = Const.TextInvalidColor;
                DisplayAlert("Error", messag, "OK");
            }
        }

        private void SC_Oferta_OnChanged(object sender, ToggledEventArgs e)
        {
            try
            {
                item.Oferta = e.Value;
                if (e.Value)
                {
                    item.PrecioUnitario = vmParent.GetPrecio(item.Producto, SC_Devolucion.On, SC_Oferta.On);
                    item.Devolucion = true;
                    ProductoSelected.Text = string.Concat(item.Producto.Codigo, " - ", item.Producto.Nombre, " - $ ", item.PrecioUnitario);
                    int cta = 0;
                    if (int.TryParse(ProdCtaSrc.Text, out cta))
                    {
                        item.Cantidad = cta;
                        this.SubTotal.Text = item.SubTotal.ToString();
                    }
                }
                else
                {
                    item.PrecioUnitario = vmParent.GetPrecio(item.Producto, SC_Devolucion.On, SC_Oferta.On);
                    ProductoSelected.Text = string.Concat(item.Producto.Codigo, " - ", item.Producto.Nombre, " - $ ", item.PrecioUnitario);
                    int cta = 0;
                    if (int.TryParse(ProdCtaSrc.Text, out cta))
                    {
                        item.Cantidad = cta;
                        this.SubTotal.Text = item.SubTotal.ToString();
                    }
                }
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
                ((Entry)sender).TextColor = Const.TextInvalidColor;
                DisplayAlert("Error", messag, "OK");
            }
        }

        async void Guardar_Clicked(object sender, EventArgs e)
        {
            try
            {
                item.Oferta = SC_Oferta.On;
                item.Devolucion = SC_Devolucion.On;
                MessagingCenter.Send(this, "AddItemVendido", item);
                await Navigation.PopModalAsync();
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
                ((Entry)sender).TextColor = Const.TextInvalidColor;
                DisplayAlert("Error", messag, "OK");
            }
        }

        async void Cerrar_Clicked_1(object sender, EventArgs e)
        {
            try
            {
                MessagingCenter.Send(this, "CerrarItemVendido", 0);
                await Navigation.PopModalAsync();
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
                ((Entry)sender).TextColor = Const.TextInvalidColor;
                await DisplayAlert("Error", messag, "OK");
            }
        }

        async void ProdCtaSrc_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName.ToLower().Equals("text"))
                {
                    if (item.Producto != null)
                    {
                        int cta = 0;
                        if (int.TryParse(ProdCtaSrc.Text, out cta))
                        {
                            if (vmParent.IsProductoSelectionValid(item.Producto, cta, out message))
                            {
                                item.Cantidad = cta;
                                this.SubTotal.Text = item.SubTotal.ToString();
                                this.cta_agregar.IsEnabled = true;
                            }
                            else
                            {
                                this.cta_agregar.IsEnabled = false;
                                this.SubTotal.Text = string.Empty;
                                await DisplayAlert(this.Title, message, "ok");
                            }
                        }
                    }
                }
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
                ((Entry)sender).TextColor = Const.TextInvalidColor;
                DisplayAlert("Error", messag, "OK");
            }

        }
        async void ProdcutoSrc_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName.ToLower().Equals("text"))
                {
                    var prod = vmParent.GetProducto(ProdcutoSrc.Text.Trim());
                    int cta = 0;
                    if (int.TryParse(ProdCtaSrc.Text, out cta))
                    {
                        item.Cantidad = cta;
                        this.SubTotal.Text = item.SubTotal.ToString();
                    }
                    if (prod != null)
                    {
                        item.Producto = prod;
                        item.PrecioUnitario = vmParent.GetPrecio(prod, SC_Devolucion.On, SC_Oferta.On);
                        ProductoSelected.Text = string.Concat(item.Producto.Codigo, " - ", item.Producto.Nombre, " - $ ", item.PrecioUnitario);
                        if (vmParent.IsProductoSelectionValid(prod, cta, out message))
                        {
                            this.cta_agregar.IsEnabled = true;
                        }
                        else
                        {
                            this.cta_agregar.IsEnabled = false;
                            if (message.Length > 0)
                                await DisplayAlert(this.Title, message, "ok");
                        }
                    }
                }
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
                ((Entry)sender).TextColor = Const.TextInvalidColor;
                DisplayAlert("Error", messag, "OK");
            }
        }

        async void Eliminar_Clicked(object sender, EventArgs e)
        {
            try
            {
                MessagingCenter.Send(this, "DeleteItemVendido", 0);
                await Navigation.PopModalAsync();
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
                ((Entry)sender).TextColor = Const.TextInvalidColor;
                DisplayAlert("Error", messag, "OK");
            }
        }
    }
}
