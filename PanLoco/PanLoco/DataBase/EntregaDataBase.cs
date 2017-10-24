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
                database.CreateTableAsync<Entrega>(CreateFlags.AutoIncPK).Wait();
                database.CreateTableAsync<EntregaItemVendido>(CreateFlags.AutoIncPK).Wait();
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
                database.CreateTableAsync<Entrega>(CreateFlags.AutoIncPK).Wait();
                database.CreateTableAsync<EntregaItemVendido>(CreateFlags.AutoIncPK).Wait();
                internallCollection = getItemsAsync().Result;
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
            internallCollection = getItemsAsync().Result;
            return Task.FromResult(internallCollection);
        }
        private Task<List<Entrega>> getItemsAsync()
        {
            return database.Table<Entrega>().OrderByDescending(s => s.Fecha).ToListAsync();
        }
        public Task<List<Entrega>> GetItemsAsync()
        {
            return Task.FromResult(internallCollection);
        }

        public Task<List<Entrega>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<Entrega>("SELECT * FROM [Entrega] order by [FECHA] desc");
        }

        public Task<Entrega> GetItemAsync(int id)
        {
            try
            {
                //var o = database.Table<EntregaItemVendido>().Where(t => t.EntregaId==id).ToListAsync();
                var rt = database.Table<Entrega>().Where(i => i.Id == id).FirstOrDefaultAsync();
                //= database.Table<EntregaItemVendido>().ToListAsync();

                //var p = database.Table<EntregaItemVendido>().("SELECT * FROM [EntregaItemVendido] Where [EntregaID]= " + id).Result;

                //rt.ItemVendidos = p;//database.Table<EntregaItemVendido>().Where(s => s.EntregaId.Equals(id)).ToListAsync().Result;// ("SELECT * FROM [EntregaItemVendido] where [EntregaID] = "+id.ToString()).Result;
                rt.Result.ItemVendidos = GetItemsVendidos(id);
                return Task.FromResult(rt.Result);
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
        public Task<Entrega> GetItem(int id)
        {
            try
            {
                //var tq = database.GetConnection().Table<Entrega>().Single(i => i.Id == id);
                //var o = database.GetConnection().Table<EntregaItemVendido>().Where(i => i.EntregaId==id).ToList();
                var tq = database.Table<Entrega>().Where(e => e.Id == id).FirstAsync().Result;
                if (tq != null)
                {
                    throw new Exception("No se encuentra la Entrega");
                }
                var o = database.Table<EntregaItemVendido>().Where(eiv => eiv.EntregaId == id).ToListAsync().Result;
                tq.ItemVendidos = o;
                return Task.FromResult(tq);
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

        public Task<bool> SaveItemAsync(Entrega item, Dictionary<string, int> _stock)
        {
            try
            {
                database.GetConnection().BeginTransaction();
                Task<int> result;
                
                if (item.Id != 0)
                {
                    result = database.UpdateAsync(item);
                    UpdateInternalCollection(item);
                }
                else
                {
                    result = database.InsertAsync(item);
                    internallCollection.Add(item);
                }
                while (!result.Status.Equals(TaskStatus.RanToCompletion))
                {
                    if (result.Status.Equals(TaskStatus.Faulted))
                    {
                        string msg = string.Empty;
                        foreach (var ex in result.Exception.InnerExceptions)
                        {
                            msg += ex.Message;
                        }
                        throw new Exception(msg);
                    }
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

        //private void SaveItemsVendidos(List<EntregaItemVendido> itemVendidos, int EntregaID)
        //{
        //    //var listin = database.QueryAsync<EntregaItemVendido>("DELETE from [EntregaItemVendido] WHERE [EntregaID] = " + EntregaID.ToString()).Result;
        //    foreach (EntregaItemVendido eiv in itemVendidos)
        //    {
        //        eiv.EntregaId = EntregaID;
        //    }
        //    var result = database.InsertAllAsync(itemVendidos);
        //    //throw new NotImplementedException();
        //    ///TODO: guardar los item vendidos.
        //}

        public Task<int> DeleteItemAsync(Entrega item)
        {
            try
            {
                database.GetConnection().BeginTransaction();

                var rt = database.DeleteAsync(item);
                internallCollection.Remove(item);
                database.GetConnection().Commit();
                return rt;
            }
            catch
            {
                database.GetConnection().Rollback();
                return Task.FromResult(-1);
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
                database.DropTableAsync<Entrega>().Wait();
                database.DropTableAsync<EntregaItemVendido>().Wait();
                database.CreateTableAsync<Entrega>(CreateFlags.AutoIncPK).Wait();
                database.CreateTableAsync<EntregaItemVendido>(CreateFlags.AutoIncPK).Wait();
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