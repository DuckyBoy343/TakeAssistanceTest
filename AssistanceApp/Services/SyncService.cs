using System;
using System.Collections.Generic;
using System.Text;


public class SyncService
{
    private readonly RestService _api;
    private readonly LocalDatabaseService _db;

    public SyncService(RestService api, LocalDatabaseService db)
    {
        _api = api;
        _db = db;
    }

    public async Task SyncAllAsync()
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet) return;

        try
        {
            var schools = await _api.GetAllSchoolsAsync();
            if (schools.Any()) await _db.SyncSchoolsAsync(schools);

            var students = await _api.GetAllStudentsAsync();
            if (students.Any()) await _db.SyncStudentsAsync(students);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Sync Error: {ex.Message}");
        }
    }
}
