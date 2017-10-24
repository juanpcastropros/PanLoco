﻿using System.Collections.Generic;
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
            SaveItemAsync(prod);
            resutl = string.Concat(resutl, " to -> ", prod.Stock);
            return resutl;
            //var re = database.ExecuteAsync("UPDATE [Producto] SET Stock = ? where Codigo = '?'", stock, codigo);
            //database.Table<Producto>().ex.Where( new System.Linq.Expressions.Expression<Func<Producto, bool>>())
            //var resutl = database.QueryAsync<Producto>("UPDATE [Producto] SET Stock = ? where Codigo = '?'", stock, codigo));
        }

        public Task<List<Producto>> GetItemsAsync()
        {
            return Task.FromResult(items);
        }
        public Task<List<Producto>> getItemsAsync()
        {
            return database.Table<Producto>().ToListAsync();
        }

        public Task<List<Producto>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<Producto>("SELECT * FROM [Producto] ");
        }

        public Task<Producto> GetItemAsync(int id)
        {
            //return database.Table<Producto>().Where(i => i.Id == id).FirstOrDefaultAsync();
            return Task.FromResult(items.Where((Producto arg) => arg.Id == id).FirstOrDefault());
        }
        public Task<Producto> GetItemAsyncByCode(string code)
        {
            //return database.Table<Producto>().Where(i => i.Codigo == code).FirstOrDefaultAsync();
            return Task.FromResult(items.Where((Producto arg) => arg.Codigo == code).FirstOrDefault());
        }
        public Task<Producto> GetItemByCode(string code)
        {
            //return database.Table<Producto>().Where(i => i.Codigo == code).FirstOrDefaultAsync();
            return Task.FromResult(items.Where((Producto arg) => arg.Codigo == code).FirstOrDefault());
        }
        public Task<Producto> IsCodeExist(string code)
        {
            return database.Table<Producto>().Where(i => i.Codigo == code).FirstOrDefaultAsync();
            //return Task.FromResult(items.Where((Producto arg) => arg.Codigo == code).FirstOrDefault());
        }

        public Task<int> SaveItemAsync(Producto item)
        {
            Task<int> rt;
            if (item.Id != 0)
            {
                rt=database.UpdateAsync(item);
            }
            else
            {
                rt=database.InsertAsync(item);
            }
            UpdateInternalCollection(item);
            return rt;
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
                item.Id = rt;
            }
            UpdateInternalCollection(item);
            return rt;
        }

        public Task<int> DeleteItemAsync(Producto item)
        {
            Task<int> resul;
            try
            {

                database.GetConnection().BeginTransaction();
                resul =database.DeleteAsync(item);
                database.GetConnection().Commit();
                items.Remove(item);
                return resul;
            }
            catch(Exception ex)
            {
                database.GetConnection().Rollback();
                return Task.FromResult(-1);
            }
        }
        private void UpdateInternalCollection(Producto item)
        {
            var _item = items.Where((Producto arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);
        }
    }
}