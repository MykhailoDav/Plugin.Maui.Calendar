using System.Collections.ObjectModel;

namespace SampleApp.ViewModels;

public partial class WeekSelectionCalendarViewModel : BasePageViewModel
{
    [ObservableProperty]
    DateTime shownDate = DateTime.Today;

    [ObservableProperty]
    ObservableCollection<DateTime> selectedDates = [];

    [RelayCommand]
    void DayTapped(object item)
    {
        if (item is not DateTime tappedDate)
		{
			return;
		}

		var a = tappedDate.DayOfWeek - DayOfWeek.Monday;
		int diffToMonday = (7 + a) % 7;
        var startOfWeek = tappedDate.AddDays(-diffToMonday);

		SelectedDates.Clear();

		ObservableCollection<DateTime> userSelectedDates = [];

		for (int i = 0; i < 7; i++)
		{
			userSelectedDates.Add(startOfWeek.AddDays(i));
		}

		SelectedDates = new ObservableCollection<DateTime>(userSelectedDates);
	}
}
