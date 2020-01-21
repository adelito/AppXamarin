using Mobile.Helpers;
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
	public partial class PreencherQuestionario : ContentPage
	{
		public PreencherQuestionario ()
		{
			InitializeComponent ();
		}

        private async void BtVisita_Clicked(object sender, EventArgs e)
        {
            btVisita.IsEnabled = false;
            DatabaseHelper dataBase = new DatabaseHelper();
            var countVisita = dataBase.GetQuestionarioVisita();
            if(countVisita > 0)
            {
                await Navigation.PushAsync(new PreencherQuestionarioVisita(), true);
            }
            else
            {
                await DisplayAlert("Visita Instituição", " Não Possui questionário para realizar visita", "Ok");
            }
            
            this.IsBusy = true;
            btVisita.IsEnabled = true;
        }
    }
}