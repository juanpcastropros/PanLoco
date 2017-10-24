using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PanLoco.Models;

using Xamarin.Forms;

[assembly: Dependency(typeof(PanLoco.Services.ProductosDataSource))]

namespace PanLoco.Services
{
    public class ProductosDataSource : IDataStore<Producto>
    {
        bool isInitialized;
        List<Producto> items;

        public async Task<bool> AddItemAsync(Producto item)
        {
            var newItem = item.Id == 0;
            await InitializeAsync();

            await App.ProductoDB.SaveItemAsync(item);

            if(newItem)
                items.Add(item);
            else
                UpdateInternalCollection(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Producto item)
        {
            await InitializeAsync();
            await App.ProductoDB.SaveItemAsync(item);
            UpdateInternalCollection(item);
            return await Task.FromResult(true);
        }

        private void UpdateInternalCollection(Producto item)
        {
            
            var _item = items.Where((Producto arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);
        }

        public async Task<bool> DeleteItemAsync(Producto item)
        {
            await InitializeAsync();
            await App.ProductoDB.DeleteItemAsync(item);

            var _item = items.Where((Producto arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<Producto> GetItemAsync(int id)
        {
            await InitializeAsync();
            return await App.ProductoDB.GetItemAsync(id);

            //return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }
        public async Task<Producto> GetItemAsync(string codigo)
        {
            await InitializeAsync();

            return await Task.FromResult(items.FirstOrDefault(s => s.Codigo == codigo));
        }

        public async Task<IEnumerable<Producto>> GetItemsAsync(bool forceRefresh = false)
        {
            await InitializeAsync();

            return await Task.FromResult(items);
        }

        public Task<bool> PullLatestAsync()
        {
            return Task.FromResult(true);
        }

        public async void ForceRefreshCollection()
        {
            isInitialized = false;
            await InitializeAsync();
        }
        public Task<bool> SyncAsync()
        {
            return Task.FromResult(true);
        }

        public async Task InitializeAsync()
        {
            if (isInitialized)
                return;
            items = await App.ProductoDB.GetItemsAsync();

            isInitialized = true;
        }
    }

 
}
