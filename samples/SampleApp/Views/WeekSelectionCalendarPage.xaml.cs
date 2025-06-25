namespace SampleApp.Views;

public partial class WeekSelectionCalendarPage : ContentPage
{
	public WeekSelectionCalendarPage(WeekSelectionCalendarViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}