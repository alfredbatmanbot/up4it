using Microsoft.Extensions.Logging;
using Up4It.Services;
using Up4It.ViewModels;
using Up4It.Pages;

namespace Up4It;

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
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		// Register services
		builder.Services.AddSingleton<SupabaseService>();

		// Register ViewModels
		builder.Services.AddTransient<EventFeedViewModel>();
		builder.Services.AddTransient<CreateEventViewModel>();

		// Register Pages
		builder.Services.AddTransient<EventFeedPage>();
		builder.Services.AddTransient<CreateEventPage>();

		return builder.Build();
	}
}
