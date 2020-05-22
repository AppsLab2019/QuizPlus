using QuizPlus.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace QuizPlus
{
    public class Database
    {
        private SQLiteAsyncConnection connection;

        public async Task Initialize()
        {
            var path = GetPath();

            await ExtractDatabase(path);

            connection = new SQLiteAsyncConnection(path);
            await connection.CreateTableAsync<Country>();
        }

        private string GetPath()
        {   
            // cesta do databazoveho suboru
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(path, "QuizPlus.db3");
        }

        private async Task ExtractDatabase(string path)
        {
            if (File.Exists(path))
                return;

            // ziskame udaje o projekte
            var assembly = Assembly.GetExecutingAssembly();
            using var dbStream = assembly.GetManifestResourceStream("QuizPlus.QuizPlus.db3");
            using var fileStream = File.Open(path, FileMode.Create, FileAccess.Write);

            await dbStream.CopyToAsync(fileStream);
        }

        public Task<List<Country>> GetAllCountries()
        {
            // z databázy zoberieme všetky štáty
            return connection
                .Table<Country>()
                .ToListAsync();
        }
    }
}
