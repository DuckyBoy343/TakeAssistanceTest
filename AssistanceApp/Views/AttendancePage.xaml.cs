using AssistanceApp.ViewModels;

namespace AssistanceApp.Views;

public partial class AttendancePage : ContentPage
{
    private readonly AttendanceViewModel _vm;
    public AttendancePage(AttendanceViewModel vm)
	{
		InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _vm.LoadListCommand.ExecuteAsync(null);
    }
}