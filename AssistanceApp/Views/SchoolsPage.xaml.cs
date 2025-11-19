using AndroidX.Lifecycle;
using AssistanceApp.ViewModels;

namespace AssistanceApp.Views;

public partial class SchoolsPage : ContentPage
{
    private readonly SchoolsViewModel _viewModel;
    public SchoolsPage(SchoolsViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.InitDataCommand.ExecuteAsync(null);
    }
}