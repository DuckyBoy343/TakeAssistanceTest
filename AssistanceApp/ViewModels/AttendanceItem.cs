using CommunityToolkit.Mvvm.ComponentModel;

namespace AssistanceApp.ViewModels
{
    public partial class AttendanceItem : ObservableObject
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }

        [ObservableProperty]
        private bool isPresent;
    }
}
