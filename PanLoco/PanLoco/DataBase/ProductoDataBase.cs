using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using PanLoco.Models;
using System;
using System.Linq;

namespace PanLoco.DataBase
{
    public class ProductoDataBase
    {
        //readonly SQLiteAsyncConnection database;
        private SQLiteAsyncConnection database;
        List<Producto> items;

        public ProductoDataBase(string dbPath)
        {
            try
            {
                database = new SQLiteAsyncConnection(dbPath);
                //database.CreateTableAsync<Producto>(CreateFlags.None).Wait();
                database.GetConnection().CreateTable<Producto>(CreateFlags.None);
            }
            catch(Exception ex)
            {

            }
        }
        public ProductoDataBase(SQLiteAsyncConnection dataBase)
        {
            try
            {
                database = dataBase;
                //database.CreateTableAsync<Producto>(CreateFlags.None).Wait();
                database.GetConnection().CreateTable<Producto>(CreateFlags.None);
                //items = getItemsAsync().Result;
                items = getItems();
            }
            catch (Exception ex)
            {

            }
        }

        private List<Producto> getItems()
        {
            return database.GetConnection().Table<Producto>().ToList();//.ToListAsync();
        }

        public string StockUpdate(string codigo, int stock)
        {
            string resutl = "from ";
            var prod = GetItemByCode(codigo).Result;
            resutl = string.Concat(resutl, prod.Stock);
            prod.Stock = stock;
            SaveItem(prod);
            resutl = string.Concat(resutl, " to -> ", prod.Stock);
            return resutl;
            //var re = database.ExecuteAsync("UPDATE [Producto] SET Stock = ? where Codigo = '?'", stock, codigo);
            //database.Table<Producto>().ex.Where( new System.Linq.Expressions.Expression<Func<Producto, bool>>())
            //var resutl = database.QueryAsync<Producto>("UPDATE [Producto] SET Stock = ? where Codigo = '?'", stock, codigo));
        }

        public List<Producto> GetItems()
        {
            return items;
        }
       
        public List<Producto> GetItemsSync()
        {
            return database.GetConnection().Table<Producto>().ToList();
        }

        public Producto GetItem(int id)
        {
            //return database.Table<Producto>().Where(i => i.Id == id).FirstOrDefaultAsync();
            return items.Where((Producto arg) => arg.Id == id).FirstOrDefault();
        }

        public Task<Producto> GetItemByCode(string code)
        {
            //return database.Table<Producto>().Where(i => i.Codigo == code).FirstOrDefaultAsync();
            return Task.FromResult(items.Where((Producto arg) => arg.Codigo == code).FirstOrDefault());
        }
        public Producto IsCodeExist(string code)
        {
            return database.GetConnection().Table<Producto>().Where(i => i.Codigo == code).FirstOrDefault();
            //return Task.FromResult(items.Where((Producto arg) => arg.Codigo == code).FirstOrDefault());
        }

        public int SaveItem(Producto item)
        {
            int rt=0;
            
            if (item.Id != 0)
            {
                //rt = database.UpdateAsync(item);
                rt = database.GetConnection().Update(item);
            }
            else
            {
                //rt = database.InsertAsync(item);
                rt = database.GetConnection().Insert(item);
                //item.Id = rt;
            }
            UpdateInternalCollection(item);
            return rt;
        }

        public int DeleteItem(Producto item)
        {
            int resul=-1;
            try
            {

                database.GetConnection().BeginTransaction();
                resul =database.GetConnection().Delete(item);
                database.GetConnection().Commit();
                items.Remove(item);
                return resul;
            }
            catch(Exception ex)
            {
                database.GetConnection().Rollback();
                return -1;
            }
        }
        private void UpdateInternalCollection(Producto item)
        {
            var _item = items.Where(predicate: (Producto arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);
        }
    }
}