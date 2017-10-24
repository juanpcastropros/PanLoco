using System;

using PanLoco.Models;
using PanLoco.ViewModels;

using Xamarin.Forms;
using PanLoco.Helpers;

namespace PanLoco.Views
{
    public partial class ClientesListPage : ContentPage
    {
        ClientesLPViewModel viewModel;

        public ClientesListPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ClientesLPViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            try
            {
                Cliente pasItem = new Cliente();
                var item = (args.SelectedItem as Cliente);
                if (item == null)
                    return;
                pasItem.Descuento = item.Descuento;
                pasItem.Horario = item.Horario;
                pasItem.Id = item.Id;
                pasItem.Mayorista = item.Mayorista;
                pasItem.NombreCompleto = item.NombreCompleto;
                pasItem.NombreDeFantasia = item.NombreDeFantasia;
                pasItem.Telefono = item.Telefono;

                await Navigation.PushAsync(new ClienteNuevoPage(pasItem));

                // Manually deselect item
                ItemsListView.SelectedItem = null;
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

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new ClienteNuevoPage());
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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
