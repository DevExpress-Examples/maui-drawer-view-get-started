using System;
using Microsoft.Maui.Controls;
using DevExpress.Maui.Navigation;
using System.Globalization;

namespace DrawerViewExample {
	public partial class MainPage : ContentPage {
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
            drawer.IsDrawerOpened = true;
            SizeChanged += OnSizeChanged;
        }
        protected void OnSizeChanged(object sender, EventArgs args) {
            IsLandscapeOriented = this.Width > this.Height;
        }
    }

    class BoolToDrawerBehaviorConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (targetType != typeof(DrawerBehavior)) return null;
            bool boolValue = (bool)value;
            return boolValue ? DrawerBehavior.Split : DrawerBehavior.SlideOnTop;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
