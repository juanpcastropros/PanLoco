using System;

using PanLoco.Models;

using Xamarin.Forms;
using PanLoco.Helpers;
using System.Collections.Generic;
using PanLoco.ViewModels;

namespace PanLoco.Views.Entregas
{
    public partial class EntregaNuevoPageSavana : ContentPage
    {

        bool _isValid = true;
        EntregaCreacionViewModel viewModel;
        //Cliente cliente = null;
        //public EntregaMock ItemMaster { get; set; }
        //public Entrega Item { get; set; }

        public List<EntregaItemVendido> Productos { get; set; }
        public EntregaNuevoPageSavana()
        {
            try
            {
                InitializeComponent();

                BindingContext = viewModel = new EntregaCreacionViewModel(true);
                MessagingCenter.Subscribe<EntregaCreacionViewModel, Entrega>(this, "EntregatotalNuevo", (obj, item) =>
               {
                   var entre = item as Entrega;
                   this.SubTotalAmount.Text = "$ " + entre.SubTotal.ToString();
                   this.TotalAmount.Text = "$ " + item.Total.ToString();
               });
                //InitPage();
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

        private void InitPage()
        {

        }

        public EntregaNuevoPageSavana(Entrega entrega)
        {
            try
            {
                InitializeComponent();
                BindingContext = viewModel = new EntregaCreacionViewModel(true);
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

        async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                //viewModel.Item.ItemVendidos.AddRange(viewModel.iVendidos);
                if (viewModel.IsEntregaValida())
                {
                    viewModel.Save();
                    await Navigation.PushModalAsync(new Printing.PrinterContainer(viewModel.Item));
                    await Navigation.PopToRootAsync();
                }
                else
                {
                    await DisplayAlert("Entrega no valida", "Seleccionar cliente y/o validar cantidades de productos", "ok");
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

                await DisplayAlert("Error", messag, "OK");
            }
        }



        async void Close_Clicked(object sender, EventArgs e)
        {
            try
            {
                MessagingCenter.Send(this, "Close");
                await Navigation.PopToRootAsync();
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

                await DisplayAlert("Error", messag, "OK");
            }
        }

        public void TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Trim().Length > 3)
            {
                var prod = App.ProductoDB.GetItemByCode(e.NewTextValue.Trim()).Result;
            }
        }

        private async void lstItemVendidos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                EntregaItemVendido param = e.SelectedItem as EntregaItemVendido;
                viewModel.EditingPosition = viewModel.iVendidos.IndexOf(param);
                await Navigation.PushModalAsync(new EntregaAddProducto(viewModel, viewModel.Item.EsMayorista, param));
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

                await DisplayAlert("Error", messag, "OK");
            }
        }
        

        private void ClienteSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClienteChoiced.Text = this.ClienteSelected.SelectedItem.ToString();
                viewModel.setCliente(this.ClienteSelected.SelectedItem.ToString());
                this.Discount.Text = viewModel.GetClienteDescuento().ToString() + "%";

                if (_isValid)
                {
                    this.viewModel.TotalRefresh();
                    this.TotalAmount.Text = viewModel.Item.Total.ToString("#.##");
                    this.SubTotalAmount.Text = viewModel.Item.SubTotal.ToString("#.##");
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

                DisplayAlert("Error", messag, "OK");
            }
        }
        private void PickerLabel_OnTapped(object sender, EventArgs e)
        {
            ClienteSelected.Focus();
        }

        void cta_TextChanged(object sender, TextChangedEventArgs e)
        {
            Entry ety;
            try
            {
                ety = ((Entry)sender);
                _isValid = Helpers.Const.Validation.IsNumeric(ety.Text, false);

                if (!_isValid)
                {
                    ((Entry)sender).TextColor = Helpers.Const.TextInvalidColor;
                    return;
                }
                ety.Text = int.Parse(ety.Text).ToString();
                if (!ety.Placeholder.ToLower().Equals("desc"))
                {
                    _isValid = Helpers.Const.Validation.IsStockValid(ety.BindingContext as EntregaItemVendido);
                }
                ((Entry)sender).TextColor = _isValid ? PanLoco.Helpers.Const.TextValidColor : PanLoco.Helpers.Const.TextInvalidColor;
                ((EntregaItemVendido)ety.BindingContext).IsValid = _isValid;
                if (_isValid)
                {
                    this.viewModel.TotalRefresh();
                    this.TotalAmount.Text = viewModel.Item.Total.ToString("#.##");
                    this.SubTotalAmount.Text = viewModel.Item.SubTotal.ToString("#.##");
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

        private bool Validate(Entry ety)
        {
            EntregaItemVendido iv = ety.BindingContext as EntregaItemVendido;
            if (iv != null)
            {
                if (!(iv.Producto.Stock > iv.CantidadDev + iv.CantidadNor))
                {
                    DisplayAlert("Stock Faltante", "Tenes " + iv.Producto.Stock + " de " + iv.Producto.Nombre, "OK");
                    ety.BackgroundColor = Color.Red;
                    return false;
                }
            }
            else
            {
                DisplayAlert("Erro", "algo salió mal. Cerra la aplicación y volve a abrirla", "OK");
                ety.BackgroundColor = Color.Red;
                return false;
            }
            ety.BackgroundColor = Color.Transparent;
            return true;
        }

        private void ctaOferta_Clicked(object sender, EventArgs e)
        {
            try
            {
                EntregaItemVendido iv = ((Button)sender).BindingContext as EntregaItemVendido;
                if (iv.Oferta)
                {
                    iv.Oferta = false;
                    ((Button)sender).Text = "NO";
                }
                else
                {
                    iv.Oferta = true;
                    ((Button)sender).Text = "OF";
                }
                this.viewModel.TotalRefresh();
                this.TotalAmount.Text = viewModel.Item.Total.ToString("#.##");
                this.SubTotalAmount.Text = viewModel.Item.SubTotal.ToString("#.##");
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

        private void CtaNormal_Unfocused_1(object sender, FocusEventArgs e)
        {
            try
            {
                if (((Entry)sender).Text.Length.Equals(0))
                    ((Entry)sender).Text = "0";

            }
            catch
            { }
        }
    }
}