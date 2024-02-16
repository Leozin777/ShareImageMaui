namespace shareapp;

public partial class MainPage : ContentPage
{
	private byte[] _image;
	public byte[] C_Image
	{
		get => _image;
		set
		{
			_image = value;
			OnPropertyChanged();
		}
	}

	public const string NewImageMessage = "MainPage.new_image";
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
		BindingContext = this;
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);
		MessagingCenter.Subscribe<App, byte[]>(this, NewImageMessage, (_, dado) =>
		{
			C_Image = dado;
        });
	}

	protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
	{
		base.OnNavigatedFrom(args);
		MessagingCenter.Unsubscribe<App, string>(this, NewImageMessage);
	}
}

