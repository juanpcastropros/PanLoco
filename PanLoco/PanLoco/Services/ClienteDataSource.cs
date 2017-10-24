using PanLoco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(PanLoco.Services.ClienteDataSource))]
namespace PanLoco.Services
{
    public class ClienteDataSource : IDataStore<Cliente>
    {
        bool isInitialized;
        List<Cliente> items;

        public async Task<bool> AddItemAsync(Cliente item)
        {
            var newItem = item.Id == 0;
            await App.ClienteDB.SaveItemAsync(item);
            if(newItem)
                items.Add(item);
            else
                UpdateInternalCollection(item);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Cliente item)
        {
            await App.ClienteDB.SaveItemAsync(item);// InitializeAsync();

            UpdateInternalCollection(item);

            return await Task.FromResult(true);
        }

        private void UpdateInternalCollection(Cliente item)
        {
            var _item = items.Where((Cliente arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);
        }

        public async Task<bool> DeleteItemAsync(Cliente item)
        {
            await App.ClienteDB.DeleteItemAsync(item);// InitializeAsync();

            var _item = items.Where((Cliente arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<Cliente> GetItemAsync(int id)
        {
            return await App.ClienteDB.GetItemAsync(id);//.Result InitializeAsync();

            //return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Cliente>> GetItemsAsync(bool forceRefresh = false)
        {
            await InitializeAsync();

            return await Task.FromResult(items);
        }

        public Task<bool> PullLatestAsync()
        {
            return Task.FromResult(true);
        }


        public Task<bool> SyncAsync()
        {
            return Task.FromResult(true);
        }

        public async Task InitializeAsync()
        {
            if (isInitialized)
                return;
            items = await App.ClienteDB.GetItemsAsync();

            isInitialized = true;
        }
    }
}
