# Sharing images with MAUI

In this repository you will see how to share an image from your Android phone to your app.

* 1: Configure the main activity class to receive images. Here we have to set LaunchMode to SingleTask, so OnCreate won't be called again when you navigate back to the app with the image and also set the Intent tag with datatype = "image/*"
```Javascript
[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTask, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
[IntentFilter([Intent.ActionSend], Categories = [Intent.CategoryBrowsable, Intent.CategoryDefault, Intent.ActionView], DataMimeType = "image/*")]
```
After that we will override the OnNewIntent method, it will be called whenever you navigate back to the application, we will also create a method called GetContentFromIntent to take the image and turn it into a byte[] (within the application you can manipulate it this way you want to find better). After calling this method inside OnNewIntent, we will call the HandleApp that will be there in App.cs
```Javascript
protected override void OnNewIntent(Intent? intent)
{
    base.OnNewIntent(intent);
    byte[] m_data = GetContentFromIntent(intent);
    if (m_data is not null)
        (App.Current as App).HandleApp(m_data);
}

public byte[] GetContentFromIntent(Intent p_intent)
{
    byte[] data = null;
    if (p_intent?.Action == Intent.ActionSend)
    {
        Stream? inputStream = null;
        var filePath = p_intent?.ClipData?.GetItemAt(0);
        if (filePath?.Uri != null)
        {
            inputStream = ContentResolver!.OpenInputStream(filePath.Uri)!;
        }

        if (inputStream != null)
        {
            using (var reader = new StreamReader(inputStream))
            {
                using (var memstream = new MemoryStream())
                {
                    var buffer = new byte[512];
                    var bytesRead = default(int);
                    while ((bytesRead = reader.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
                        memstream.Write(buffer, 0, bytesRead);
                    data = memstream.ToArray();
                    return data;
                }
            }
        }
    }

    return data;
}
```
* 2: Configuring the App.xaml.cs. In the App we will create the HandleApp method, in it we will use the MessagingCenter to send this data directly to our MainPage (this property called NewImageMessage is a static property that we will create in the MainPage)
```Javascript
public void HandleApp(byte[] p_dado)
{
	MessagingCenter.Send(this, shareapp.MainPage.NewImageMessage, p_dado);
}
```

* 3: On MainPage we will create these three properties.
```Javascript
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
```
Next we will register on the MainPage to receive the byte[] of the image and also unsubscribe to not receive it again (don't forget that)
```Javascript
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
```

## Demo

https://github.com/Leozin777/ShareImageMaui/assets/73667534/5c3859de-68e7-47e2-9678-039b91b4f5cd


## Credits

I found out how to do some data manipulations and how to use OnNewIntent in a comment by u/reloded_diper on reddit.

- [Leo](https://github.com/leozin777)
- [reloded_diper](https://www.reddit.com/user/reloded_diper/)
