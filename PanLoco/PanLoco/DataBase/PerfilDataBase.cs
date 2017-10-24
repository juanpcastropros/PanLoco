using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using PanLoco.Models;
using System;

namespace PanLoco.DataBase
{
    public class PerfilDataBase
    {
        //readonly SQLiteAsyncConnection database;
        private SQLiteAsyncConnection database;
        public PerfilDataBase(string dbPath)
        {
            try
            {
                database = new SQLiteAsyncConnection(dbPath);
                database.CreateTableAsync<Perfil>(CreateFlags.None).Wait();
            }
            catch (Exception ex)
            {

            }
        }
        public PerfilDataBase(SQLiteAsyncConnection dataBase)
        {
            try
            {
                database = dataBase;
                database.CreateTableAsync<Perfil>(CreateFlags.None).Wait();
            }
            catch (Exception ex)
            {

            }
        }

        public Task<List<Perfil>> GetItemsAsync()
        {
            return database.Table<Perfil>().ToListAsync();
        }

        public Task<List<Perfil>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<Perfil>("SELECT * FROM [Perfil]");
        }

        public Task<Perfil> GetItemAsync(int id)
        {
            return database.Table<Perfil>().FirstOrDefaultAsync();//.Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Perfil item)
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

        public Task<int> DeleteItemAsync(Perfil item)
        {
            return database.DeleteAsync(item);
        }
    }
}
