using Mobile.WebApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SincronizarQuestionario : ContentPage, INotifyPropertyChanged
    {
        VisitaToken visitaToken = new VisitaToken();
        RevisitaToken revisitaToken = new RevisitaToken();
        private Int32 idInstituto;
        private string token;
        private string dataVisita;
        private int tipo;


        public SincronizarQuestionario(string idInstituto, string token, string dataVisita, int tipo)
        {
			InitializeComponent ();
            this.idInstituto = Convert.ToInt32(idInstituto);
            this.token = token;
            this.dataVisita = dataVisita;
            this.tipo = tipo;
            BaixarQuestionario();
        }
        public async void BaixarQuestionario()
        {
            switch (tipo)
            {
                case 1:
                    //await visitaToken.ConsumirVisita(token,idInstituto,dataVisita);
                    //await DisplayAlert("Internet", "Verifique sua conexão com a internet.", "OK");
                    break;
                case 2:
                    revisitaToken.ConsumirRevisita(token, idInstituto, dataVisita);
                    await DisplayAlert("Internet", "Verifique sua conexão com a internet.", "OK");
                    break;
                default:
                    break;

                    
            }
            txtBaixando.IsVisible =false;
            txtBaixado.IsVisible = true;
           // RetornarAsync();
        }
       
    }

}
