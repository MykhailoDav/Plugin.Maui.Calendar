using System.Collections.ObjectModel;

namespace SampleApp.ViewModels;

public partial class WeekSelectionCalendarViewModel : BasePageViewModel
{
    [ObservableProperty]
    DateTime shownDate = DateTime.Today;

    [ObservableProperty]
    ObservableCollection<DateTime> selectedDates = [];

	[ObservableProperty]
    DateTime? selectedStartDate = DateTime.Today.AddDays(-2);

    [ObservableProperty]
    DateTime? selectedEndDate = DateTime.Today.AddDays(2);

    [RelayCommand]
    void DayTapped(object item)
    {
        if (item is not DateTime tappedDate)
		{
			return;
		}

		var a = tappedDate.DayOfWeek - DayOfWeek.Monday;
		// Calculate Monday (start of the week)
		int diffToMonday = (7 + a) % 7;
        var startOfWeek = tappedDate.AddDays(-diffToMonday);

        // Sunday (end of the week)
        var endOfWeek = startOfWeek.AddDays(6);

        SelectedStartDate = startOfWeek;
        SelectedEndDate = endOfWeek;
    }
}
