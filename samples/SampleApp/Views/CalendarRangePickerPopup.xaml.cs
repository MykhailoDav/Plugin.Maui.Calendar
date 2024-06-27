﻿using Mopups.Pages;
using SampleApp.Model;
using SampleApp.ViewModels;

namespace SampleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarRangePickerPopup : PopupPage
    {
        private readonly Action<CalendarRangePickerResult> _onClosedPopup;

        public CalendarRangePickerPopup(Action<CalendarRangePickerResult> onClosedPopup)
        {
            _onClosedPopup = onClosedPopup;
            InitializeComponent();            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is CalendarRangePickerPopupViewModel vm)
                vm.Closed += _onClosedPopup;
        }

        protected override void OnDisappearing()
        {
            if (BindingContext is CalendarRangePickerPopupViewModel vm)
                vm.Closed -= _onClosedPopup;

            base.OnDisappearing();
        }

        void UnloadedHandler(object sender, EventArgs e)
        {
            calendar.Dispose();
        }
    }
}
