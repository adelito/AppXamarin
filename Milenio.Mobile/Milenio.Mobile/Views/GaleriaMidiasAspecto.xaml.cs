using Mobile.Helpers;
using Mobile.Model;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GaleriaMidiasAspecto : ContentPage
	{
        DatabaseHelper service = new DatabaseHelper();
        List<MidiaAspecto> MidiaAspecto = new List<MidiaAspecto>();
        QuestionarioBaixadoModel QuestionarioBaixadomodel = new QuestionarioBaixadoModel();
        AspectoQuestionario aspecto;
        int idAspecto;
        string nomecaminho;
        public GaleriaMidiasAspecto(QuestionarioBaixadoModel questionarioBaixadoModel, int id, string nomeAspecto, AspectoQuestionario aspecto)
        {
            InitializeComponent();
            this.aspecto = aspecto;
            lbProtocolo.Text = questionarioBaixadoModel.Protocolo;
            lbDataVisita.Text = questionarioBaixadoModel.DataVisita;
            lbCidade.Text = questionarioBaixadoModel.Cidade;
            lbInstituicao.Text = questionarioBaixadoModel.Instituicao;
            lbAspecto.Text =nomeAspecto;
            idAspecto = id;
            QuestionarioBaixadomodel = questionarioBaixadoModel;
            EscolherFoto();

           
        }
        private async void EscolherFoto()
        {
            MidiaAspecto = service.GetMidiaAspecto(idAspecto);
            var qtdgaleria = MidiaAspecto.Count();
            if (qtdgaleria==0)
            {
                
                await Navigation.PushAsync(new PreencherQuestionarioQuestoes(aspecto, QuestionarioBaixadomodel), true);

            }
            else
            {
                MainCarouselView.ItemsSource = MidiaAspecto;
            }
            
        }

        private void MainCarouselView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            MidiaAspecto file = e.SelectedItem as MidiaAspecto;
            nomecaminho = file.caminho.ToString();
        }

        private async void BtExcluirAspecto_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("", "Deseja remover a imagem da galeria?", "Sim", "Não");
            if (answer)
            {
                service.ExcluirMidiaAspecto(nomecaminho);
                await DisplayAlert("", "Imagem Removida com sucesso.", "ok");
                EscolherFoto();
            }
            
        }
    }
}