using AssistanceApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AssistanceApp.ViewModels
{
    [QueryProperty(nameof(SchoolId), "id")]
    public partial class GradesViewModel : ObservableObject
    {
        private readonly LocalDatabaseService _db;
        private readonly SyncService _syncService;

        [ObservableProperty]
        private int schoolId;

        [ObservableProperty]
        private ObservableCollection<Grade> grades;

        [ObservableProperty]
        private bool isBusy;

        public GradesViewModel(LocalDatabaseService db, SyncService syncService)
        {
            _db = db;
            _syncService = syncService;
            Grades = new ObservableCollection<Grade>();
        }

        partial void OnSchoolIdChanged(int value)
        {
            // Load immediately when ID arrives
            LoadGradesCommand.ExecuteAsync(null);
        }

        [RelayCommand]
        public async Task LoadGradesAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            // 1. Instant Load (Offline)
            var localGrades = await _db.GetGradesBySchoolAsync(SchoolId);
            UpdateList(localGrades);

            try
            {
                // 2. Sync Background (If you have an API endpoint for this)
                // await _syncService.SyncGradesAsync(); // Uncomment if you implement this in SyncService

                // 3. Reload
                // var updated = await _db.GetGradesBySchoolAsync(SchoolId);
                // UpdateList(updated);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task GradeTappedAsync(Grade grade)
        {
            if (grade == null) return;
            // Navigate to StudentsPage, passing the GRADE ID
            await Shell.Current.GoToAsync($"StudentsPage?id={grade.GradeId}");
        }

        private void UpdateList(List<Grade> list)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Grades.Clear();
                foreach (var g in list) Grades.Add(g);
            });
        }
    }
}
