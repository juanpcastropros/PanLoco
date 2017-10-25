using System;
using System.Diagnostics;
using System.Threading.Tasks;

using PanLoco.Helpers;
using PanLoco.Models;
using PanLoco.Views;

using Xamarin.Forms;
using PanLoco.Services;

namespace PanLoco.ViewModels
{
    public class ClientesLPViewModel : BaseViewModel
    {

        public ObservableRangeCollection<Cliente> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ClientesLPViewModel()
        {
            Title = "Browse";
            Items = new ObservableRangeCollection<Cliente>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<ClienteNuevoPage, Cliente>(this, "AddItem", async (obj, item) =>
            {
                var _item = item as Cliente;
                //Items.Add(_item);
                App.ClienteDB.SaveItem(_item);//DataStore.AddItemAsync(_item);
                Items.ReplaceRange(App.ClienteDB.GetItems());
                //LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            });
            MessagingCenter.Subscribe<ClienteNuevoPage, Cliente>(this, "DeleteItem", async (obj, item) =>
            {
                var _item = item as Cliente;
                //Items.Remove(_item);
                 App.ClienteDB.DeleteItem(_item);//DataStore.DeleteItemAsync(_item);
                Items.ReplaceRange(App.ClienteDB.GetItems());
                //LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = App.ClienteDB.GetItems();//DataStore.GetItemsAsync(true);
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
    }
}