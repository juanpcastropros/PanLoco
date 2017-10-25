using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using PanLoco.Models;
using System;

namespace PanLoco.DataBase
{
    public class ClienteDatabase
    {
        //readonly SQLiteAsyncConnection database;
        private SQLiteAsyncConnection database;
        public ClienteDatabase(string dbPath)
        {
            try
            {
                database = new SQLiteAsyncConnection(dbPath);
                database.GetConnection().CreateTable<Cliente>(CreateFlags.None);
            }
            catch(Exception ex)
            {

            }
        }
        public ClienteDatabase(SQLiteAsyncConnection dataBase)
        {
            try
            {
                database = dataBase;
                database.GetConnection().CreateTable<Cliente>(CreateFlags.None);
            }
            catch (Exception ex)
            {

            }
        }

        public Task<List<Cliente>> GetItemsAsync()
        {
            return database.Table<Cliente>().ToListAsync();
        }
        public List<Cliente> GetItems()
        {
            //return database.GetConnection().Table<Cliente>.ToList();
            return GetItemsNotDoneSync();
        }
        
        public List<Cliente> GetItemsNotDoneSync()
        {
            return database.GetConnection().Query<Cliente>("SELECT * FROM [Cliente]");
        }

        public Cliente GetItem(int id)
        {
            return database.GetConnection().Table<Cliente>().Where(i => i.Id == id).FirstOrDefault();
        }

        public int SaveItem(Cliente item)
        {
            if (item.Id != 0)
            {
                return database.GetConnection().Update(item);
            }
            else
            {
                int i = database.GetConnection().Insert(item);
                //item.Id = i;
                return i;
            }
        }

        public int DeleteItem(Cliente item)
        {
            return database.GetConnection().Delete(item);
        }
    }
}