using System;

using PanLoco.Models;

using Xamarin.Forms;
using PanLoco.Helpers;
using System.Collections.Generic;
using PanLoco.ViewModels;

namespace PanLoco.Views
{
    public partial class EntregaNuevoPage : ContentPage
    {

        EntregaCreacionViewModel viewModel;
        //Cliente cliente = null;
        //public EntregaMock ItemMaster { get; set; }
        //public Entrega Item { get; set; }

        public List<EntregaItemVendido> Productos { get; set; }
        public EntregaNuevoPage()
        {

            InitializeComponent();
            
            BindingContext = viewModel = new EntregaCreacionViewModel();
            MessagingCenter.Subscribe<EntregaCreacionViewModel , Entrega>(this, "EntregatotalNuevo", (obj, item) =>
            {
                var entre = item as Entrega;
                this.SubTotalAmount.Text = "$ " + entre.SubTotal.ToString();
                this.TotalAmount.Text= "$ " + item.Total.ToString();
            });
            //InitPage();
        }

        private void InitPage()
        {
            //ItemMaster = new EntregaMock();
            //listaProductos.ItemsSource = viewModel.iVendidos;
            //MessagingCenter.Subscribe<EntregaAddProducto, EntregaItemVendido>(this, "AddItemVendido",  (obj, item) =>
            //{
            //    var _item = item as EntregaItemVendido;
            //    viewModel.iVendidos.Add(_item);
            //    Item.Total = Item.Total + _item.SubTotal;
            //    this.TotalAmount.Text = "$ " + Item.Total.ToString();
            //});

        }

        public EntregaNuevoPage(Entrega entrega)
        {
            InitializeComponent();
            //InitPage();
            //Item = entrega;
            BindingContext = viewModel = new EntregaCreacionViewModel();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            //viewModel.Item.Fecha = DateTime.Now;
            //viewModel.Item.ClienteID = GetClienteID(this.ClienteSelected.SelectedItem);
            //viewModel.Item.ClienteNombre = GetClienteName(this.ClienteSelected.SelectedItem);
            //viewModel.Item.ClienteDescuento = GetClienteDescuento(Item.ClienteID);
            viewModel.Item.ItemVendidos.AddRange(viewModel.iVendidos);
            viewModel.Save();
            //MessagingCenter.Send(this, "Entrega_Crear", viewModel.Item);
            await Navigation.PopToRootAsync();
        }

        

        async void Close_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "Close");
            await Navigation.PopToRootAsync();
        }

        public void TextChanged(object sender, TextChangedEventArgs e)
        {
            if(e.NewTextValue.Trim().Length>3)
            {
                var prod = App.ProductoDB.GetItemAsyncByCode(e.NewTextValue.Trim()).Result;
            }
        }

        async void AddProducto_click(object sender, EventArgs e)
        {
            if (viewModel.Item.ClienteID.Equals(0))
            {
                await DisplayAlert("Seleccionar Cliente", "Debe seleccionar un Cliente", "ok");
            }
            else
            {
                //await Navigation.PushModalAsync(new AddProduct());
                //await Navigation.PushModalAsync(new EntregaAddProducto());
                await Navigation.PushModalAsync(new EntregaAddProducto(viewModel, viewModel.Item.EsMayorista));
            }
        }

        private async void lstItemVendidos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            EntregaItemVendido param = e.SelectedItem as EntregaItemVendido;
            viewModel.EditingPosition = viewModel.iVendidos.IndexOf(param);
            await Navigation.PushModalAsync(new EntregaAddProducto(viewModel, viewModel.Item.EsMayorista, param)  );
        }

        private void Borrar_Clicked(object sender, EventArgs e)
        {

        }

        private void Editar_Clicked(object sender, EventArgs e)
        {

        }

        private void ClienteSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClienteChoiced.Text = this.ClienteSelected.SelectedItem.ToString();
            viewModel.setCliente(this.ClienteSelected.SelectedItem.ToString());
            this.Discount.Text = viewModel.GetClienteDescuento().ToString()+"%";

        }
        private void PickerLabel_OnTapped(object sender, EventArgs e)
        {
            ClienteSelected.Focus();
        }
        
    }
}