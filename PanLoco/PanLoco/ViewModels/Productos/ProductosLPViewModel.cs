using PanLoco.Helpers;
using PanLoco.Models;
using PanLoco.Services;
using PanLoco.ViewModels.Productos;
using PanLoco.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PanLoco.ViewModels
{
    class ProductosLPViewModel : BaseViewModel
    {
        //public new IDataStore<Producto> DataStore => DependencyService.Get<IDataStore<Producto>>();
        public ObservableRangeCollection<Producto> Items { get; set; }
        public ProductosLPViewModel()
        {
            Title = "Productos";
            Items = new ObservableRangeCollection<Producto>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<ProductoNuevoPage, Producto>(this, "AddItem", async (obj, item) =>
            {
                var _item = item as Producto;
                await App.ProductoDB.SaveItemAsync(_item);
                Items.ReplaceRange(App.ProductoDB.GetItemsAsync().Result);
            });
            MessagingCenter.Subscribe<ProductoViewModelCRUD, Producto>(this, "Producto Eliminado", async (obj, item) =>
            {
                var _item = item as Producto;
                await App.ProductoDB.DeleteItemAsync(_item);
                //Items.Remove(_item);
                Items.ReplaceRange(App.ProductoDB.GetItemsAsync().Result);
            });
        }

        public Command LoadItemsCommand { get; set; }
        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                //var items = await DataStore.GetItemsAsync(true);
                var items = await App.ProductoDB.GetItemsAsync(); 
                Items.ReplaceRange(items);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load items.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBusy = false;
            }
        }
        int count;

        string countDisplay = "You clicked 0 times.";
        public string CountDisplay
        {
            get { return countDisplay; }
            set { countDisplay = value; OnPropertyChanged(); }
        }

        public ICommand IncreaseCountCommand { get; }

        void IncreaseCount() =>
            CountDisplay = $"You clicked {++count} times";


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
