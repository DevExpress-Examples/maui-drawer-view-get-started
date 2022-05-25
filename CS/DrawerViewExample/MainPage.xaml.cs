using System;
using Microsoft.Maui.Controls;
using System.Globalization;

namespace DrawerViewExample {
	public partial class MainPage : FlyoutPage {
        const string IsLandscapeOrientedPropertyName = "IsLandscapeOriented";

        public static readonly BindableProperty IsLandscapeOrientedProperty = BindableProperty.Create(
            IsLandscapeOrientedPropertyName,
            typeof(bool),
            typeof(MainPage),
            defaultValue: false);

        public bool IsLandscapeOriented {
            get => (bool)GetValue(IsLandscapeOrientedProperty);
            set => SetValue(IsLandscapeOrientedProperty, value);
        }
        public MainPage() {
            InitializeComponent();
            SizeChanged += OnSizeChanged;
            carBrandList.SelectionChanged += OnSelectionChanged;
        }
        protected void OnSizeChanged(object sender, EventArgs args) {
            IsLandscapeOriented = this.Width > this.Height;
        }
        void OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            IsPresented = false;
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            carBrandList.SelectedItem = ((MainViewModel)BindingContext).CarModelsByBrand[0];
        }
    }

    class BoolToDrawerBehaviorConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (targetType != typeof(FlyoutLayoutBehavior)) return null;
            bool boolValue = (bool)value;
            return boolValue ? FlyoutLayoutBehavior.Split : FlyoutLayoutBehavior.Popover;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
