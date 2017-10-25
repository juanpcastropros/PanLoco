using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using PanLoco.Models;
using System;
using Xamarin.Forms;
using PanLoco.Helpers;

namespace PanLoco.DataBase
{
    public class EntregaDataBase
    {
        //readonly SQLiteAsyncConnection database;
        private SQLiteAsyncConnection database;
        List<Entrega> internallCollection = new List<Entrega>();

        public EntregaDataBase(string dbPath)
        {
            try
            {
                database = new SQLiteAsyncConnection(dbPath);
                database.GetConnection().CreateTable<Entrega>(CreateFlags.AutoIncPK);
                database.GetConnection().CreateTable<EntregaItemVendido>(CreateFlags.AutoIncPK);
            }
            catch (Exception ex)
            {

            }
        }

        public EntregaDataBase(SQLiteAsyncConnection dataBase)
        {
            try
            {
                database = dataBase;
                database.GetConnection().CreateTable<Entrega>(CreateFlags.AutoIncPK);
                database.GetConnection().CreateTable<EntregaItemVendido>(CreateFlags.AutoIncPK);
                internallCollection = getItems();
            }
            catch (Exception ex)
            {

            }
        }
        private void UpdateInternalCollection(Entrega item)
        {

            var _item = internallCollection.Where((Entrega arg) => arg.Id == item.Id).FirstOrDefault();
            internallCollection.Remove(_item);
            internallCollection.Add(item);
        }
        public Task<List<Entrega>> RefreshForce()
        {
            internallCollection = getItems();
            return Task.FromResult(internallCollection);
        }
        private List<Entrega> getItems()
        {
            return database.GetConnection().Table<Entrega>().OrderByDescending(s => s.Fecha).ToList();
        }
        public Task<List<Entrega>> GetItems()
        {
            return Task.FromResult(internallCollection);
        }
        
        public List<Entrega> GetItemsNotDoneSync()
        {
            return database.GetConnection().Query<Entrega>("SELECT * FROM [Entrega] order by [FECHA] desc");
        }

        public Entrega GetItem(int id)
        {
            try
            {
                
                var rt = database.GetConnection().Table<Entrega>().Where(i => i.Id == id).FirstOrDefault();
                
                rt.ItemVendidos = GetItemsVendidos(id);
                return rt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return null;
        }

        public Task<bool> SaveItem(Entrega item, Dictionary<string, int> _stock)
        {
            try
            {
                database.GetConnection().BeginTransaction();
                int result;
                
                if (item.Id != 0)
                {
                    result = database.GetConnection().Update(item);
                    UpdateInternalCollection(item);
                }
                else
                {
                    result = database.GetConnection().Insert(item);
                    //item.Id = result;
                    internallCollection.Add(item);
                }
                foreach (EntregaItemVendido eiv in item.ItemVendidos)
                {
                    eiv.EntregaId = item.Id;
                }
                SaveItemsVendidos(item.ItemVendidos, item.Id);
                string t = "";
                if (_stock != null)
                {
                    foreach (KeyValuePair<string, int> iv in _stock)
                    {
                        t = App.ProductoDB.StockUpdate(iv.Key, iv.Value);
                    }
                }
                database.GetConnection().Commit();
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                database.GetConnection().Rollback();
                return Task.FromResult(false);
            }

        }

        private void SaveItemsVendidos(List<EntregaItemVendido> itemVendidos, int id)
        {
            string f = DependencyService.Get<IFileHelper>().SaveEntrega(itemVendidos,id);
        }
        private List<EntregaItemVendido> GetItemsVendidos( int id)
        {
            return DependencyService.Get<IFileHelper>().ReadEntrega(id);
        }

        public int DeleteItemAsync(Entrega item)
        {
            try
            {
                database.GetConnection().BeginTransaction();

                var rt = database.GetConnection().Delete(item);
                internallCollection.Remove(item);
                database.GetConnection().Commit();
                return rt;
            }
            catch
            {
                database.GetConnection().Rollback();
                return -1;
            }

        }
        public bool DeleteAll()
        {
            try
            {
                //database.GetConnection().BeginTransaction();
                //database.GetConnection().DropTable<Entrega>();
                //database.GetConnection().DropTable<EntregaItemVendido>();
                //database.GetConnection().Commit();
                database.GetConnection().DropTable<Entrega>();
                database.GetConnection().DropTable<EntregaItemVendido>();
                database.GetConnection().CreateTable<Entrega>(CreateFlags.AutoIncPK);
                database.GetConnection().CreateTable<EntregaItemVendido>(CreateFlags.AutoIncPK);
                DependencyService.Get<IFileHelper>().DeleteFiles();
                return true;
            }
            catch
            {
                database.GetConnection().Rollback();
                return false;

            }

        }
    }
}