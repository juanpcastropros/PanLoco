using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PanLoco.Models;

using Xamarin.Forms;

[assembly: Dependency(typeof(PanLoco.Services.EEntregasDataSource))]

namespace PanLoco.Services
{
    public class EEntregasDataSource : IDataStore<Entrega>
    {
        bool isInitialized;
        List<Entrega> items;

        public async Task<bool> AddItemAsync(Entrega item, Dictionary<string, int> _stock)
        {
            var newItem = item.Id == 0;
            //await InitializeAsync();

            await App.EntregaDB.SaveItemAsync(item, _stock);

            if(newItem)
                items.Add(item);
            else
                UpdateInternalCollection(item);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Entrega item)
        {
            await InitializeAsync();
            await App.EntregaDB.SaveItemAsync(item, null);
            UpdateInternalCollection(item);
            return await Task.FromResult(true);
        }

        private void UpdateInternalCollection(Entrega item)
        {
            
            var _item = items.Where((Entrega arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);
        }

        public async Task<bool> DeleteItemAsync(Entrega item)
        {
            await InitializeAsync();
            await App.EntregaDB.DeleteItemAsync(item);

            var _item = items.Where((Entrega arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<Entrega> GetItemAsync(int id)
        {
            await InitializeAsync();
            return await App.EntregaDB.GetItemAsync(id);

            //return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }
        //public async Task<Entrega> GetItemAsync(string codigo)
        //{
        //    await InitializeAsync();

        //    return await Task.FromResult(items.FirstOrDefault(s => s.Codigo == codigo));
        //}

        public async Task<IEnumerable<Entrega>> GetItemsAsync(bool forceRefresh = false)
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
            items = await App.EntregaDB.GetItemsAsync();

            isInitialized = true;
        }

        public Task<bool> AddItemAsync(Entrega item)
        {
            throw new NotImplementedException();
        }
    }

 
}
