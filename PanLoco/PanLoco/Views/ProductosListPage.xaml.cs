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
    public partial class ProductosListPage : ContentPage
    {
        ProductosLPViewModel viewModel;
        public ProductosListPage()
        {
            InitializeComponent();
            BindingContext = viewModel =new ProductosLPViewModel();
        }
        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Producto;
            if (item == null)
                return;

            await Navigation.PushAsync(new ProductoNuevoPage(item));

            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductoNuevoPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }

    
}
