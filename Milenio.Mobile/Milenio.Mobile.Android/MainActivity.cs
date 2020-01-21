
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using CarouselView.FormsPlugin.Android;
using Milenio.Mobile.Droid;
using Plugin.CurrentActivity;
using Plugin.Media;
using Plugin.Permissions;
using System.IO;
using System.Threading.Tasks;
using Resource = Milenio.Mobile.Droid.Resource;

namespace Mobile.Droid
{
    [Activity(Label = "Milênio Mobile", Icon = "@drawable/logo_milenio", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            StrictMode.SetThreadPolicy(new StrictMode.ThreadPolicy.Builder().DetectAll().PenaltyLog().Build());
            StrictMode.SetVmPolicy(new StrictMode.VmPolicy.Builder().DetectLeakedSqlLiteObjects().DetectLeakedClosableObjects().PenaltyLog().PenaltyDeath().Build());
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            base.OnCreate(savedInstanceState);
            //await CrossMedia.Current.Initialize();
            
            Plugin.InputKit.Platforms.Droid.Config.Init(this, savedInstanceState);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);


            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CarouselViewRenderer.Init();
            UserDialogs.Init(this);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public static readonly int PickImageId = 1000;

        public TaskCompletionSource<Stream> PickImageTaskCompletionSource { set; get; }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            if (requestCode == PickImageId)
            {
                if ((resultCode == Result.Ok) && (intent != null))
                {
                    Android.Net.Uri uri = intent.Data;
                    Stream stream = ContentResolver.OpenInputStream(uri);

                    // Set the Stream as the completion of the Task
                    PickImageTaskCompletionSource.SetResult(stream);
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }
        }
    }
}