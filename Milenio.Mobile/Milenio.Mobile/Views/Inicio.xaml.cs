using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mobile.Views;

namespace Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Inicio : ContentPage
	{
		public Inicio ()
		{
			InitializeComponent ();
            this.BindingContext = this;
            this.IsBusy = false;


        }

        private async void btGerarQuestionario_Clicked(object sender, EventArgs e)
        {
            btBaixarQuestionario.IsEnabled = false;
            await Navigation.PushAsync(new GerarQuestionario(), true);
            this.IsBusy = true;
            btBaixarQuestionario.IsEnabled = true;
        }
        private async void btBaixarQuestionario_Clicked(object sender, EventArgs e)
        {
            btBaixarQuestionario.IsEnabled = false;
            await Navigation.PushAsync(new BaixarQuestionario(), true);
            this.IsBusy = true;
            btBaixarQuestionario.IsEnabled = true;
        }

        private async void BtPreencherQuestionario_Clicked(object sender, EventArgs e)
        {
            btPreencherQuestionario.IsEnabled = false;
            await Navigation.PushAsync(new PreencherQuestionario(), true);
            this.IsBusy = true;
            btPreencherQuestionario.IsEnabled = true;
        }
    }
}