using PanLoco.Helpers;
using PanLoco.Models;
using PanLoco.Services;
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
    class EntregasLPViewModel : BaseViewModel
    {
        //public new IDataStore<Entrega> DataStore => DependencyService.Get<IDataStore<Entrega>>();
        public ObservableRangeCollection<Entrega> Items { get; set; }
        public EntregasLPViewModel()
        {
            Title = "Ventas";
            Items = new ObservableRangeCollection<Entrega>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            //MessagingCenter.Subscribe<EntregaNuevoPage, Entrega>(this, "Entrega_Crear", async (obj, item) =>
            MessagingCenter.Subscribe<EntregaCreacionViewModel, Entrega>(this, "Entrega_Crear", async (obj, item) =>
            {
                try
                {
                    var _item = item as Entrega;
                    //if(await (DataStore as EntregasDataSource).AddItemAsync(_item,obj._stock ))
                    if ( await App.EntregaDB.SaveItem(_item, obj._stock))
                    {
                        Items.Add(_item);
                        //Items = Items.OrderByDescending(i => i.Fecha);
                        //IDataStore<Producto> ProducDS = DependencyService.Get<IDataStore<Producto>>();
                        //((PProductosDataSource)ProducDS).ForceRefreshCollection();
                        //MessagingCenter.Send(this, "Entrega_Creada", resu);
                    }
                    
                }
                catch (Exception e)
                {

                }
            });
        }
        public void Delete()
        {
            Items.Clear();
        }
        public void Refresh()
        {
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
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
                var items = await App.EntregaDB.RefreshForce();
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
