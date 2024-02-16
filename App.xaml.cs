namespace shareapp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

	public void HandleApp(byte[] p_dado)
	{
		MessagingCenter.Send(this, shareapp.MainPage.NewImageMessage, p_dado);
	}

}
