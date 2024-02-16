using Android.App;
using Android.Content.PM;
using Android.Content;
using Android.OS;

namespace shareapp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTask, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
[IntentFilter([Intent.ActionSend], Categories = [Intent.CategoryBrowsable, Intent.CategoryDefault, Intent.ActionView], DataMimeType = "image/*")]

public class MainActivity : MauiAppCompatActivity
{

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
    }

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
}
