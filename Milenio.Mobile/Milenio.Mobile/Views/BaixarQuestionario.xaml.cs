using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BaixarQuestionario : ContentPage
	{
      
        public BaixarQuestionario ()
		{
			InitializeComponent();
            this.BindingContext = this;
            this.IsBusy = false;

        }
        int op=0;
        private async void BtVisita_Clicked(object sender, EventArgs e)
        {
            btVisita.IsEnabled = false;
            if (CrossConnectivity.Current.IsConnected)
            {
                op = 1;
                await Navigation.PushAsync(new FormBaixarQuestionario(op));
            }
            else
            {
                await DisplayAlert("Internet", "Verifique sua conexão com a internet.", "OK");
            }
            this.IsBusy = true;
            btVisita.IsEnabled = true;
        }

       /*private async void BtRevisita_Clicked(object sender, EventArgs e)
        {
            btRevisita.IsEnabled = false;
            if (CrossConnectivity.Current.IsConnected)
            {
                op = 2;
                await Navigation.PushAsync(new FormBaixarQuestionario(op));
            }
            else
            {
                await DisplayAlert("Internet", "Verifique sua conexão com a internet.", "OK");
            }
            this.IsBusy = true;
            btRevisita.IsEnabled = true;
        }*/

        private async void BtQuestionariosBaixados_Clicked(object sender, EventArgs e)
        {
            btQuestionariosBaixados.IsEnabled = false;
            op = 3;
            await Navigation.PushAsync(new VisualizarQuestionariosBaixados(), true);
            this.IsBusy = true;
            btQuestionariosBaixados.IsEnabled = true;
        }

    }
}