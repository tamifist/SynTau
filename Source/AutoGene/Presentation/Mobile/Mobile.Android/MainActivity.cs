using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace AutoGene.Mobile.Droid
{
    [Activity(Label = "Auto Gene", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle bundle)
        {
            Instance = this;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Telerik.XamarinForms.Common.Android.TelerikForms.Init();
            UserDialogs.Init(this);

            LoadApplication(new AutoGene.Mobile.App());
        }
    }
}

