using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls.Xaml;
using DevExpress.Maui.Navigation;


[assembly: XamlCompilationAttribute(XamlCompilationOptions.Skip)]

namespace DrawerViewExample
{
	public class Startup : IStartup
	{
		public void Configure(IAppHostBuilder appBuilder)
		{
			appBuilder
				.ConfigureMauiHandlers((_, handlers) =>
						handlers.AddHandler<DrawerView, DrawerViewHandler>())
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});
		}
	}
}