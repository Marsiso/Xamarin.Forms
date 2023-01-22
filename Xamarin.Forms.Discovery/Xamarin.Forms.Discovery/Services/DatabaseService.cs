using Discovery.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discovery.Services;

public sealed class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public DatabaseService(string databasePath)
    {
        _database = new SQLiteAsyncConnection(databasePath);
        _database.CreateTableAsync<PhotoEntity>();
    }

    public async Task<int> CreatePhoto(PhotoEntity photo) => await _database.InsertAsync(photo);

    public async Task<List<PhotoEntity>> GetAllPhotos() => await _database.Table<PhotoEntity>().ToListAsync();

    public async Task<int> UpdatePhoto(PhotoEntity photo) => await _database.UpdateAsync(photo);

    public async Task<int> DeletePhoto(PhotoEntity photo) => await _database.DeleteAsync(photo);
}
