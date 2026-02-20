using Up4It.Pages;

namespace Up4It;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		
		Routing.RegisterRoute("createevent", typeof(CreateEventPage));
	}
}
