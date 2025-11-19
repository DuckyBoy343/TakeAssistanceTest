using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AssistanceApp.Models;

namespace AssistanceApp.ViewModels;

public partial class AttendanceViewModel : ObservableObject
{
    private readonly LocalDatabaseService _db;

    [ObservableProperty]
    private ObservableCollection<AttendanceItem> studentsList;

    [ObservableProperty]
    private DateTime selectedDate;

    public AttendanceViewModel(LocalDatabaseService db)
    {
        _db = db;
        StudentsList = new ObservableCollection<AttendanceItem>();
        SelectedDate = DateTime.Now;
    }

    [RelayCommand]
    public async Task LoadListAsync()
    {
        Console.WriteLine("DEBUG: LoadListAsync STARTED"); // Look for this in Output

        // 1. Get data (Background)
        var dbStudents = await _db.GetStudentsForDemoAsync();
        Console.WriteLine($"DEBUG: DB returned {dbStudents.Count} students");

        // 2. Generate Fakes if needed
        if (dbStudents.Count == 0)
        {
            Console.WriteLine("DEBUG: Generating FAKE data...");
            dbStudents.Add(new Student { StudentId = 1, FullName = "Juan Perez (Demo)" });
            dbStudents.Add(new Student { StudentId = 2, FullName = "Maria Lopez (Demo)" });
            dbStudents.Add(new Student { StudentId = 3, FullName = "Carlos Ruiz (Demo)" });
        }

        // 3. UPDATE UI (CRITICAL: Must be on Main Thread)
        MainThread.BeginInvokeOnMainThread(() =>
        {
            StudentsList.Clear();
            foreach (var s in dbStudents)
            {
                StudentsList.Add(new AttendanceItem
                {
                    StudentId = s.StudentId,
                    StudentName = s.FullName,
                    IsPresent = true
                });
            }
            Console.WriteLine($"DEBUG: Added {StudentsList.Count} items to UI List");
        });
    }

    [RelayCommand]
    public async Task SaveAttendanceAsync()
    {
        // Convert UI items back to Database records
        var records = new List<Attendance>();

        foreach (var item in StudentsList)
        {
            records.Add(new Attendance
            {
                StudentId = item.StudentId,
                ClassDate = SelectedDate,
                IsPresent = item.IsPresent
            });
        }

        // Save to DB
        // (Make sure you added SaveAttendanceBatchAsync to LocalDatabaseService!)
        await _db.SaveAttendanceBatchAsync(records);

        await Shell.Current.DisplayAlert("Saved", "Attendance taken successfully!", "OK");
    }
}