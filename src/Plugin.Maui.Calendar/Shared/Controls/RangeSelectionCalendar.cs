﻿using System.Runtime.CompilerServices;
using Plugin.Maui.Calendar.Controls.SelectionEngines;
using Plugin.Maui.Calendar.Models;

namespace Plugin.Maui.Calendar.Controls;

[XamlCompilation(XamlCompilationOptions.Compile)]
public class RangeSelectionCalendar : Calendar
{
    /// <summary> Bindable property for StartDate </summary>
    public static readonly BindableProperty SelectedStartDateProperty = BindableProperty.Create(
        nameof(SelectedStartDate),
        typeof(DateTime?),
        typeof(RangeSelectionCalendar),
        null,
        BindingMode.TwoWay,
        propertyChanged: OnSelectedStartDateChanged
    );

    /// <summary>
    /// Beggining of selected interval
    /// </summary>
    public DateTime? SelectedStartDate
    {
        get => (DateTime?)GetValue(SelectedStartDateProperty);
        set => SetValue(SelectedStartDateProperty, value);
    }

    /// <summary> Bindable property for EndDate </summary>
    public static readonly BindableProperty SelectedEndDateProperty = BindableProperty.Create(
        nameof(SelectedEndDate),
        typeof(DateTime?),
        typeof(RangeSelectionCalendar),
        null,
        BindingMode.TwoWay,
        propertyChanged: OnSelectedEndDateChanged
    );

    /// <summary> End of selected interval </summary>
    public DateTime? SelectedEndDate
    {
        get => (DateTime?)GetValue(SelectedEndDateProperty);
        set => SetValue(SelectedEndDateProperty, value);
    }

    /// <summary>
    /// Background color for the range between SelectedStartDate and SelectedEndDate.
    /// </summary>
    public static readonly BindableProperty SelectedDatesRangeBackgroundColorProperty = BindableProperty.Create(
        nameof(SelectedDatesRangeBackgroundColor),
        typeof(Color),
        typeof(RangeSelectionCalendar),
        Colors.LightGray
    );

    /// <summary>
    /// Background color for the range between SelectedStartDate and SelectedEndDate.
    /// </summary>
    public Color SelectedDatesRangeBackgroundColor
    {
        get => (Color)GetValue(SelectedDatesRangeBackgroundColorProperty);
        set => SetValue(SelectedDatesRangeBackgroundColorProperty, value);
    }
    private bool _isSelectionDatesChanging = false;
    private readonly RangedSelectionEngine _selectionEngine;

    /// <summary>
    /// Constructor
    /// </summary>
    public RangeSelectionCalendar() : base()
    {
        monthDaysView.CurrentSelectionEngine = new RangedSelectionEngine();
        _selectionEngine = monthDaysView.CurrentSelectionEngine as RangedSelectionEngine;
    }

    /// <summary>
    /// Method that is called when a bound property is changed.
    /// </summary>
    /// <param name="propertyName">The name of the bound property that changed.</param>
    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName is nameof(SelectedDates) && !_isSelectionDatesChanging)
        {
            var first = _selectionEngine.GetDateRange(DisabledDates);

            if (first.Count > 0)
            {
                _isSelectionDatesChanging = true;
                SetValue(SelectedStartDateProperty, first.First());
                SetValue(SelectedEndDateProperty, first.Last());
            }
            UpdateDateColors();
        }
    }

    private static void OnSelectedStartDateChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var rangeSelectionCalendar = (RangeSelectionCalendar)bindable;
        if (!rangeSelectionCalendar._isSelectionDatesChanging)
        {
            rangeSelectionCalendar._isSelectionDatesChanging = true;
            rangeSelectionCalendar._selectionEngine.SelectDateRange(
                (DateTime?)newValue,
                rangeSelectionCalendar.DisabledDates
            );
            rangeSelectionCalendar.SelectedDates =
                rangeSelectionCalendar._selectionEngine.GetDateRange(
                    rangeSelectionCalendar.DisabledDates
                );
            rangeSelectionCalendar._isSelectionDatesChanging = false;
        }
        rangeSelectionCalendar.UpdateDateColors();
    }

    private static void OnSelectedEndDateChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var rangeSelectionCalendar = (RangeSelectionCalendar)bindable;
        if (!rangeSelectionCalendar._isSelectionDatesChanging)
        {
            rangeSelectionCalendar._isSelectionDatesChanging = true;
            rangeSelectionCalendar._selectionEngine.SelectDateRange(
                (DateTime?)newValue,
                rangeSelectionCalendar.DisabledDates
            );
            rangeSelectionCalendar.SelectedDates =
                rangeSelectionCalendar._selectionEngine.GetDateRange();
        }
        rangeSelectionCalendar._isSelectionDatesChanging = false;

        rangeSelectionCalendar.UpdateDateColors();
    }

    private void UpdateDateColors()
    {
        foreach (var dayView in monthDaysView.DayViews)
        {
            if (dayView.BindingContext is DayModel dayModel)
            {
                if (dayModel.Date == SelectedStartDate || dayModel.Date == SelectedEndDate)
                {
                    dayModel.SelectedBackgroundColor = monthDaysView.SelectedDayBackgroundColor;
                }
                else if (SelectedStartDate.HasValue && SelectedEndDate.HasValue &&
                dayModel.Date > SelectedStartDate.Value && dayModel.Date < SelectedEndDate.Value)
                {
                    dayModel.SelectedBackgroundColor = SelectedDatesRangeBackgroundColor;
                }
            }
        }
    }
}
