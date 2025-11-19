using AssistanceApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AssistanceApp.ViewModels
{
    public partial class SchoolsViewModel : ObservableObject
    {
        private readonly LocalDatabaseService _db;
        private readonly SyncService _syncService;

        [ObservableProperty]
        private ObservableCollection<School> schools;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string statusMessage;

        public SchoolsViewModel(LocalDatabaseService db, SyncService syncService)
        {
            _db = db;
            _syncService = syncService;
            Schools = new ObservableCollection<School>();
        }

        [RelayCommand]
        public async Task InitDataAsync()
        {
            var cachedSchools = await _db.GetLocalSchoolsAsync();

            if (cachedSchools.Count > 0)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Schools.Clear();
                    foreach (var s in cachedSchools) Schools.Add(s);
                });
            }

            if (IsBusy) return;
            IsBusy = true;
            StatusMessage = "Checking for updates...";

            try
            {
                await _syncService.SyncAllAsync();

                var updatedSchools = await _db.GetLocalSchoolsAsync();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Schools.Clear();
                    foreach (var s in updatedSchools) Schools.Add(s);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Sync Error: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
