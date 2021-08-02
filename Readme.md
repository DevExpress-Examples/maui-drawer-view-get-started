# DevExpress Drawer View for .NET MAUI

This example allows you to get started with the DrawerView component - use it to add a [navigation drawer﻿](https://material.io/design/components/navigation-drawer.html) to your .NET MAUI application.

1. Install a [.NET MAUI development](https://docs.microsoft.com/en-gb/dotnet/maui/get-started/installation) environment and open the solution in Visual Studio 22 Preview.
2. Download a free copy of DevExpress Xamarin.Forms and MAUI Controls from https://www.devexpress.com/xamarin-free.
3. [Obtain your NuGet feed URL](http://docs.devexpress.com/GeneralInformation/116042/installation/install-devexpress-controls-using-nuget-packages/obtain-your-nuget-feed-url).
4. Register the DevExpress NuGet feed as a package source.
5. Restore all NuGet packages for the solution.  
6. Run the application on an Android device or emulator.  

<img src="./img/devexpress-maui-drawer-view.png"/>

The following step-by-step instructions describe how to create the same application.

## Create a New MAUI Application and Add a Drawer View

Create a new .NET MAUI solution in Visual Studio 22 Preview.  
Refer to the following Microsoft documentation for more information on how to get started with .NET MAUI: [.NET Multi-platform App UI](https://docs.microsoft.com/en-gb/dotnet/maui/).

Add the DevExpress Drawer View component to your solution as follows: 
1. Download a free copy of DevExpress Xamarin.Forms and MAUI Controls from https://www.devexpress.com/xamarin-free.
2. [Obtain your NuGet feed URL](http://docs.devexpress.com/GeneralInformation/116042/installation/install-devexpress-controls-using-nuget-packages/obtain-your-nuget-feed-url).
3. Register the DevExpress NuGet feed as a package source. 
4. Install the **DevExpress.Maui.Navigation** package from the DevExpress NuGet feed.

In the *Startup.cs* file, register a handler for the DevExpress DrawerView :

```cs
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Hosting;
using DevExpress.Maui.Navigation;

namespace DrawerViewExample {
	public class Startup : IStartup {
		public void Configure(IAppHostBuilder appBuilder) {
			appBuilder
				.ConfigureMauiHandlers((_, handlers) => 
                                        handlers.AddHandler<DrawerView, DrawerViewHandler>())
				.UseMauiApp<App>()
				.ConfigureFonts(fonts => {
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});
		}
	}
}
```

In the *MainPage.xaml* file, use the *dxn* prefix to declare the **DevExpress.Maui.Navigation** namespace and add a **DrawerView** object to the main page:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:dxn="clr-namespace:DevExpress.Maui.Navigation;assembly=DevExpress.Maui.Navigation"
             x:Class="DrawerViewExample.MainPage">
     <dxn:DrawerView/>
</ContentPage>
```

## Create Models and View Models
Add a **CarModel** class that specifies a data object in the application:

```cs
namespace DrawerViewExample {
    public class CarModel {
        public string BrandName { get; }
        public string ModelName { get; }
        public string FullName => $"{BrandName} {ModelName}";

        public CarModel(string brand, string model) {
            this.BrandName = brand;
            this.ModelName = model;
        }
    }
}
```

Create a **CarBrandViewModel** class that defines content for the drawer view: car make and corresponding models. The application will display brands in the drawer and matching models in the main content area:

```cs
using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DrawerViewExample {
    public class CarBrandViewModel : INotifyPropertyChanged  {
        public string BrandName { get; }
        public IReadOnlyList<CarModel> CarModels { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public CarBrandViewModel(string brandName, IEnumerable<CarModel> carModels) {
            if (String.IsNullOrEmpty(brandName)) {
                this.BrandName = String.Empty;
            }
            else {
                this.BrandName = brandName;
            }
            if (carModels == null) {
                this.CarModels = new List<CarModel>();
            }
            else {
                this.CarModels = carModels.ToList();
            }
        }
        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler.Invoke(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}
```

Create a **MainViewModel** class that defines content for the MainPage (models grouped by make/brand):

```cs
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DrawerViewExample {
    public class MainViewModel : INotifyPropertyChanged {
        private static readonly IReadOnlyList<CarModel> allCarModels = new List<CarModel> {
            new CarModel("Mercedes-Benz", "SL500 Roadster"),
            new CarModel("Mercedes-Benz", "CLK55 AMG Cabriolet"),
            new CarModel("Mercedes-Benz", "C230 Kompressor Sport Coupe"),
            new CarModel("BMW", "530i"),
            new CarModel("Rolls-Royce", "Corniche"),
            new CarModel("Jaguar", "S-Type 3.0"),
            new CarModel("Cadillac", "Seville"),
            new CarModel("Cadillac", "DeVille"),
            new CarModel("Lexus", "LS430"),
            new CarModel("Lexus", "GS430"),
            new CarModel("Ford", "Ranger FX-4"),
            new CarModel("Dodge", "RAM 1500"),
            new CarModel("GMC", "Siera Quadrasteer"),
            new CarModel("Nissan", "Crew Cab SE"),
            new CarModel("Toyota", "Tacoma S-Runner"),
        };

        public IReadOnlyList<CarBrandViewModel> CarModelsByBrand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel() {
            List<CarBrandViewModel> carBrandViewModels = new List<CarBrandViewModel>();
            carBrandViewModels.Add(new CarBrandViewModel("All", allCarModels));

            IEnumerable<IGrouping<string, CarModel>> groupedCarModels = 
                                                        allCarModels.GroupBy(v => v.BrandName);
            foreach (IGrouping<string, CarModel> carModelGroup in groupedCarModels) {
                carBrandViewModels.Add(new CarBrandViewModel(carModelGroup.Key, carModelGroup));
            }
            CarModelsByBrand = carBrandViewModels;
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler.Invoke(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}
```

## Specify the Drawer View Content
In the *MainPage.xaml* file:
1. Set the **ContentPage.BindingContext** property to a **MainViewModel** object.
2. Set the **DrawerView.DrawerContent** property to a **ListView** object. Bind the list’s **ItemsSource** property to the **CarModelsByBrand** property of the view model, and set up list items to display brand names.
3. Set the **DrawerView.MainContent** property to a **ListView** object. Specify the list’s **ItemsSource** binding. The bound list should contain car models corresponding to the selected brand.

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="DrawerViewExample.MainPage"
             xmlns:local="clr-namespace:DrawerViewExample"
             xmlns:dxn="clr-namespace:DevExpress.Maui.Navigation;assembly=DevExpress.Maui.Navigation">
    <ContentPage.BindingContext>
        <local:MainViewModel/>
    </ContentPage.BindingContext>
    <dxn:DrawerView>
        <dxn:DrawerView.DrawerContent>
            <ListView x:Name="carBrandList" 
                      ItemsSource="{Binding CarModelsByBrand}">
                <ListView.ItemTemplate>
                    <DataTemplate>
                        <TextCell Text="{Binding BrandName}" />
                    </DataTemplate>
                </ListView.ItemTemplate>
            </ListView>
        </dxn:DrawerView.DrawerContent>
        <dxn:DrawerView.MainContent>
            <ListView BindingContext="{x:Reference carBrandList}"
                      ItemsSource="{Binding SelectedItem.CarModels}">
                <ListView.ItemTemplate>
                    <DataTemplate>
                        <TextCell Text="{Binding FullName}"/>
                    </DataTemplate>
                </ListView.ItemTemplate>
            </ListView>
        </dxn:DrawerView.MainContent>
    </dxn:DrawerView>
</ContentPage>
```

## Define the Drawer Behavior

Define drawer behavior depending on device or emulator orientation:  
- Landscape orientation – the drawer is always visible.
- Portrait orientation – the drawer moves out over the main content area when a user swipes a screen from the left edge.

Add the **IsLandscapeOriented** dependency property to the MainPage class:

```cs
using System;
using Microsoft.Maui.Controls;
using DevExpress.Maui.Navigation;

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
            SizeChanged += OnSizeChanged;
        }
        protected void OnSizeChanged(object sender, EventArgs args) {
            IsLandscapeOriented = this.Width > this.Height;
        }
    }
}
```

Implement a value converter that converts a Boolean value to a value of the **DrawerBehavior** enumeration:

```cs
using System.Globalization;
// ...

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
```

Bind the **DrawerView.DrawerBehavior** property to **IsLandscapeOriented**:

```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
            xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
            x:Class="DrawerViewExample.MainPage"
            xmlns:local="clr-namespace:DrawerViewExample"
            xmlns:dxn="clr-namespace:DevExpress.Maui.Navigation;assembly=DevExpress.Maui.Navigation"
            x:Name="page">
    <!-- ... -->
    <ContentPage.Resources>
        <local:BoolToDrawerBehaviorConverter x:Key="boolToDrawerBehaviorConverter"/>
    </ContentPage.Resources>
    <dxn:DrawerView DrawerBehavior="{Binding IsLandscapeOriented, Source={x:Reference page}, 
                                    Converter={StaticResource boolToDrawerBehaviorConverter}}">
        <!-- Other properteis of the drawer view are here. -->
    </dxn:DrawerView>
</ContentPage>
```

## Customize the Drawer Appearance
Use the following properties to customize the drawer size, shadow, and scrim:

```xaml
<dxn:DrawerView DrawerBehavior="{Binding IsLandscapeOriented, Source={x:Reference page}, 
                                Converter={StaticResource boolToDrawerBehaviorConverter}}"
                DrawerWidth="180"
                DrawerShadowHeight="10"
                DrawerShadowRadius="40"
                DrawerShadowColor="#808080"
                ScrimColor="#80000000">
    <!-- Other properties of the drawer view are here. -->
</dxn:DrawerView>
```

The scrim does not affect a drawer view when drawer behavior is set to **Split**.
