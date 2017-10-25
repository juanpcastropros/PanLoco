using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PanLoco.Models;

using Xamarin.Forms;

[assembly: Dependency(typeof(PanLoco.Services.PProductosDataSource))]

namespace PanLoco.Services
{
    public class PProductosDataSource : IDataStore<Producto>
    {
        bool isInitialized;
        List<Producto> items;

        public async Task<bool> AddItemAsync(Producto item)
        {
            var newItem = item.Id == 0;
            await Initialize();

            App.ProductoDB.SaveItem(item);

            if(newItem)
                items.Add(item);
            else
                UpdateInternalCollection(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Producto item)
        {
            await Initialize();
            App.ProductoDB.SaveItem(item);
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
            await Initialize();
            App.ProductoDB.DeleteItem(item);

            var _item = items.Where((Producto arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public Producto GetItem(int id)
        {
            Initialize();
            return App.ProductoDB.GetItem(id);

            //return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }
        public async Task<Producto> GetItemAsync(string codigo)
        {
            await Initialize();

            return await Task.FromResult(items.FirstOrDefault(s => s.Codigo == codigo));
        }

        public async Task<IEnumerable<Producto>> GetItemsAsync(bool forceRefresh = false)
        {
            await Initialize();

            return await Task.FromResult(items);
        }

        public Task<bool> PullLatestAsync()
        {
            return Task.FromResult(true);
        }

        public void ForceRefreshCollection()
        {
            isInitialized = false;
            Initialize();
        }
        public Task<bool> SyncAsync()
        {
            return Task.FromResult(true);
        }

        public Task Initialize()
        {
            if (isInitialized)
                return null;
            items = App.ProductoDB.GetItems();
            
            isInitialized = true;
            return null;
        }

        Task<bool> IDataStore<Producto>.AddItemAsync(Producto item)
        {
            throw new NotImplementedException();
        }

        Task<bool> IDataStore<Producto>.UpdateItemAsync(Producto item)
        {
            throw new NotImplementedException();
        }

        Task<bool> IDataStore<Producto>.DeleteItemAsync(Producto item)
        {
            throw new NotImplementedException();
        }

        Task<Producto> IDataStore<Producto>.GetItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Producto>> IDataStore<Producto>.GetItemsAsync(bool forceRefresh)
        {
            throw new NotImplementedException();
        }

        Task IDataStore<Producto>.InitializeAsync()
        {
            throw new NotImplementedException();
        }

        Task<bool> IDataStore<Producto>.PullLatestAsync()
        {
            throw new NotImplementedException();
        }

        Task<bool> IDataStore<Producto>.SyncAsync()
        {
            throw new NotImplementedException();
        }
    }

 
}
