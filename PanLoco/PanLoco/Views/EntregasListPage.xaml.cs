using PanLoco.Helpers;
using PanLoco.Models;
using PanLoco.ViewModels;
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

namespace PanLoco.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntregasListPage : ContentPage
    {
        EntregasLPViewModel viewModel;
        public EntregasListPage()
        {
            try
            {
                InitializeComponent();
                BindingContext = viewModel = new EntregasLPViewModel();
                MessagingCenter.Subscribe<EntregasLPViewModel, string>(this, "Entrega_Creada", (obj, item) =>
                {
                    DisplayAlert("message", item.ToString(), "ok");
                    //Item.ItemVendidos.Add(_item);
                    //    //lstItemVendidos.ItemsSource = Item.ItemVendidos;
                });
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
        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            try
            { 
            var item = args.SelectedItem as Producto;
            if (item == null)
                return;

            await Navigation.PushAsync(new ProductoNuevoPage(item));

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

                await DisplayAlert("Error", messag, "OK");
            }
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            try
            { 
            await Navigation.PushAsync(new Entregas.EntregaNuevoPageSavana()); //EntregaNuevoPage());
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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private void NuevoDia_Clicked(object sender, EventArgs e)
        {
            if (App.EntregaDB.DeleteAll())
                viewModel.Delete();
        }

        async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (this.ItemsListView.SelectedItem != null)
                {
                    Entrega ent = (Entrega)this.ItemsListView.SelectedItem;
                    if (ent != null)
                    {
                        ent = await App.EntregaDB.GetItemAsync(ent.Id);
                        await Navigation.PushModalAsync(new Printing.PrinterContainer(ent));
                    }
                }
            }
            catch(Exception ex)
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
    }


}
