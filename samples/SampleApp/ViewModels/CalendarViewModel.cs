using System.Collections.ObjectModel;
using System.Globalization;

namespace SampleApp.ViewModels;

public partial class CalendarViewModel : BasePageViewModel
{
	public ObservableCollection<string> Languages { get; } =
	[
		"English",
		"Украї☺нська",
		"Deutsch",
		"Français",
		"Español",
		"Polski",
		"Italiano",
		"中文",
		"日本語",
		"한국어"
	];

	[ObservableProperty]
	string selectedLanguage;

	[ObservableProperty]
	CultureInfo calendarCulture;

	partial void OnSelectedLanguageChanged(string value)
	{
		string cultureCode = value switch
		{
			"English" => "en",
			"Українська" => "uk",
			"Deutsch" => "de",
			"Français" => "fr",
			"Español" => "es",
			"Polski" => "pl",
			"Italiano" => "it",
			"中文" => "zh",
			"日本語" => "ja",
			"한국어" => "ko",
			_ => "en"
		};

		SetCulture(cultureCode);
	}

	void SetCulture(string cultureCode)
	{
		var culture = CultureInfo.CreateSpecificCulture(cultureCode);
		Thread.CurrentThread.CurrentCulture = culture;
		Thread.CurrentThread.CurrentUICulture = culture;

		CalendarCulture = culture;

		Preferences.Set("AppLanguage", cultureCode);

		Application.Current.MainPage!.DisplayAlert(
			"Мова змінена",
			$"Поточна мова: {culture.NativeName}",
			"OK"
		);
	}
}
