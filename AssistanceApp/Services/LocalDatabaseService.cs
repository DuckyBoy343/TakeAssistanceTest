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
        await _db.CreateTableAsync<Grade>();
        await _db.CreateTableAsync<School>();
        await _db.CreateTableAsync<Attendance>();
    }

    public async Task SyncStudentsAsync(List<Student> serverStudents)
    {
        await Init();

        if (serverStudents.Count == 0) return;

        await _db.RunInTransactionAsync(trans =>
        {
            foreach (var student in serverStudents)
            {
                trans.InsertOrReplace(student);
            }
        });
    }

    public async Task SyncGradesAsync(List<Grade> remoteGrades)
    {
        await Init();
        await _db.RunInTransactionAsync(trans =>
        {
            foreach (var g in remoteGrades) trans.InsertOrReplace(g);
        });
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

    public async Task<List<Grade>> GetGradesBySchoolAsync(int schoolId)
    {
        await Init();
        return await _db.Table<Grade>()
                        .Where(g => g.SchoolId == schoolId)
                        .ToListAsync();
    }

    public async Task<List<Student>> GetStudentsByGradeAsync(int gradeId)
    {
        await Init();
        return await _db.Table<Student>()
                        .Where(s => s.GradeId == gradeId)
                        .ToListAsync();
    }

    public async Task SaveAttendanceBatchAsync(List<Attendance> records)
    {
        await Init();
        await _db.RunInTransactionAsync(trans =>
        {
            foreach (var record in records)
            {
                // Logic: If we already took attendance for this student on this day, update it.
                // Otherwise, insert new.
                // Note: This requires a complex query or a composite unique key.
                // For MVP Demo: Just Insert (or InsertOrReplace if you set PK correctly)

                trans.Insert(record);
            }
        });
    }

    public async Task<List<Student>> GetStudentsForDemoAsync()
    {
        await Init();

        // Step A: Try to find ANY grade
        var firstGrade = await _db.Table<Grade>().FirstOrDefaultAsync();

        if (firstGrade != null)
        {
            // If we have grades, return students for that grade
            return await _db.Table<Student>()
                            .Where(s => s.GradeId == firstGrade.GradeId)
                            .ToListAsync();
        }

        // Step B: If no grades exist yet, just return ALL students (Fail-safe)
        return await _db.Table<Student>().ToListAsync();
    }
}
