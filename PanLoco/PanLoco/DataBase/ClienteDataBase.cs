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
                database.CreateTableAsync<Cliente>(CreateFlags.None).Wait();
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
                database.CreateTableAsync<Cliente>(CreateFlags.None).Wait();
            }
            catch (Exception ex)
            {

            }
        }

        public Task<List<Cliente>> GetItemsAsync()
        {
            return database.Table<Cliente>().ToListAsync();
        }

        public Task<List<Cliente>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<Cliente>("SELECT * FROM [Cliente]");
        }

        public Task<Cliente> GetItemAsync(int id)
        {
            return database.Table<Cliente>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Cliente item)
        {
            if (item.Id != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Cliente item)
        {
            return database.DeleteAsync(item);
        }
    }
}