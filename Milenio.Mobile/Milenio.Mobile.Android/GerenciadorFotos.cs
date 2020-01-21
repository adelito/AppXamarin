using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Milenio.Mobile.Droid;
using Milenio.Mobile.Services;
using Mobile.Droid;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(GerenciadorFotos))]
namespace Milenio.Mobile.Droid
{
    public class GerenciadorFotos : IGerenciadorFotos
    {
        public Task<Stream> ObterFoto()
        {
            var intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);

            #pragma warning disable CS0618 // O tipo ou membro é obsoleto
                        MainActivity activity = Forms.Context as MainActivity;
            #pragma warning restore CS0618 // O tipo ou membro é obsoleto

                        activity.StartActivityForResult(
                            Intent.CreateChooser(intent, "Select Picture"),
                            MainActivity.PickImageId);

                        activity.PickImageTaskCompletionSource = new TaskCompletionSource<Stream>();

                        return activity.PickImageTaskCompletionSource.Task;

        }
        public async Task MidiaAsync()
        {
            MediaFile file;
            IMedia media = CrossMedia.Current;
            try
            {
               
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    System.Diagnostics.Debug.WriteLine("Erro.");
                    return;
                }
                file = await media.PickPhotoAsync();
                if (file == null)
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", "Mídia não selecionada.", "OK");
                    return;
                }
                else
                {
                    string caminho = file.Path;
                    string filename = null;
                    filename = Path.GetFileName(caminho);
                    file.Dispose();
                   

                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}