using Mobile.Helpers;
using Mobile.Model;
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
	public partial class PreencherQuestionarioAspecto : ContentPage
	{
        ListView listApecto;
        QuestionarioBaixadoModel questionarioBaixadoModel = new QuestionarioBaixadoModel();
        public PreencherQuestionarioAspecto(object item)
        {
            InitializeComponent();
            listView.ItemTapped += ListApecto_ItemTapped;
            BindingContext = item;
            ListAspecto();
        
        }
        public void ListAspecto()
        {
            DatabaseHelper service = new DatabaseHelper();
            var visitaId = Convert.ToInt32(lbProtocolo.Text.TrimStart('0'));
            var item = service.GetAspectoQuestionario(visitaId);
            listView.ItemsSource = item;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //ListView listApecto;
            //listApecto = null;
            listApecto = (ListView)sender;
            //listApecto.ItemTapped += ListApecto_ItemTapped;
        }

        private async void ListApecto_ItemTapped(object sender, ItemTappedEventArgs e)
        {
           
            questionarioBaixadoModel.Protocolo = lbProtocolo.Text;
            questionarioBaixadoModel.DataVisita = lbDataVisita.Text;
            questionarioBaixadoModel.Cidade = lbCidade.Text;
            questionarioBaixadoModel.Instituicao = lbInstituicao.Text;
            var item = e.Item;
            await Navigation.PushAsync(new PreencherQuestionarioQuestoes(item,questionarioBaixadoModel), true);
            
        }
    }
}