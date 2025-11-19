using AssistanceApp.Models;
using SQLite;

public class LocalDatabaseService
{
    private SQLiteAsyncConnection _db;

    public LocalDatabaseService()
    {
        
    }

    private async Task Init()
    {
        if (_db is not null) return;

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "LocalDb.db3");

        _db = new SQLiteAsyncConnection(dbPath);

        await _db.CreateTableAsync<Student>();
        await _db.CreateTableAsync<School>();
    }

    public async Task SyncStudentsAsync(List<Student> serverStudents)
    {
        await Init();

        if (serverStudents.Count == 0) return;

        await _db.InsertOrReplaceAsync(serverStudents);
    }

    public async Task SyncSchoolsAsync(List<School> serverSchools)
    {
        await Init();

        if (serverSchools.Count == 0) return;

        await _db.RunInTransactionAsync(trans =>
        {
            foreach (var school in serverSchools)
            {
                trans.InsertOrReplace(school);
            }
        });
    }

    public async Task<List<Student>> GetLocalStudentsAsync()
    {
        await Init();
        return await _db.Table<Student>().ToListAsync();
    }

    public async Task<List<School>> GetLocalSchoolsAsync()
    {
        await Init();
        return await _db.Table<School>().ToListAsync();
    }
}
