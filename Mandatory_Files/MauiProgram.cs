using Microsoft.Extensions.Logging;

namespace YTDL;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("fa-solid-900.ttf", "FontAwesomeSolid");
				fonts.AddFont("fa-regular-400.ttf", "FontAwesomeRegular");
				fonts.AddFont("fa-brands-400.ttf", "FontAwesomeBrands");
			});
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<UsageGuidePage>();
        builder.Services.AddTransient<AboutPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
