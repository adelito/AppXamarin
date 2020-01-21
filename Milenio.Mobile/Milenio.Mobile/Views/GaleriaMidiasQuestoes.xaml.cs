using Mobile.Helpers;
using Mobile.Model;
using Mobile.Services;
using Mobile.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GaleriaMidiasQuestoes : ContentPage
	{
        DatabaseHelper service = new DatabaseHelper();
        CabecalhoQuestao cabecalhoQuestao = new CabecalhoQuestao();
        string nomecaminho;
        int idquestao;
        int posicaoimagem;
        int quantidadeImagem;
        CarouselViewModel cv;
        public GaleriaMidiasQuestoes (int id, string questao)
		{
			InitializeComponent ();
            lbQuestao.Text = questao;
           
            this.idquestao = id;
            var cabecalho=cabecalhoQuestao.GetCabecalho(id);
            lbProtocolo.Text = cabecalho.protocolo.ToString().PadLeft(6, '0');
            lbDataVisita.Text = cabecalho.DataVisita.ToString("dd/MM/yyyy");
            lbCidade.Text = cabecalho.Cidade.ToString();
            lbInstituicao.Text = cabecalho.Instituicao.ToString();
            lbAspecto.Text = cabecalho.Aspecto.ToString();
            //EscolherFoto();
            BindingContext = cv = new CarouselViewModel(idquestao);
            //left.Text = quantidadeImagem.ToString();
        }
        void Handle_PositionSelected(object sender, CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs e)
        {
            int inicio;
            quantidadeImagem = service.QuantidadeMidiaQuestoes(idquestao);
            Debug.WriteLine("Posição " + e.NewValue + " Selecionada.");
            posicaoimagem = e.NewValue;
            if (quantidadeImagem > 0)
            {
                inicio = Convert.ToInt32(posicaoimagem + 1);
            }
            else
            {
                left.IsVisible = false;
                de.IsVisible = false;
                right.IsVisible = false;
                inicio = Convert.ToInt32(posicaoimagem);
            }
            
            right.Text = inicio.ToString();
            left.Text = quantidadeImagem.ToString();

        }

        void Handle_Scrolled(object sender, CarouselView.FormsPlugin.Abstractions.ScrolledEventArgs e)
        {
            Debug.WriteLine("Scrolled to " + e.NewValue + " percent.");
            Debug.WriteLine("Direction = " + e.Direction);
        }


        private async void BtExcluirQuestao_Clicked(object sender, CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs e)
        {
            var midia = service.GetMidiaQuestoes(idquestao);
            if (midia.Count() > 0)
            {
                bool answer = await DisplayAlert("", "Deseja remover a imagem da galeria?", "Sim", "Não");
                if (answer)
                {
                        for (int i = 0; i <= midia.Count; i++)
                        {
                            if (i == posicaoimagem)
                            {

                                ExcluirImagemAsyncLocal(midia[i].caminho);
                                quantidadeImagem--;
                                await DisplayAlert("", "Operação efetuada com sucesso.", "Ok");
                                BindingContext = cv = new CarouselViewModel(idquestao);
                            }
                        }
                }
            }

        }
        public async Task<bool> ExcluirImagemAsync(int id, string caminho)
        {
            
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);
            var endereco = Settings.EnderecoApi;
            client.BaseAddress = new Uri(endereco);
            var url = string.Format("visita/excluir-midias-questao?ids={0}", id);
            var resultVisita = await client.DeleteAsync(url);
            var respVisita = await resultVisita.Content.ReadAsStringAsync();
            ///////////////////////////////////////////////////////////////
            service.ExcluirMidiaQuestao(caminho);
            return true;
        }
        public bool ExcluirImagemAsyncLocal(string caminho)
        {

            service.ExcluirMidiaQuestao(caminho);
            return true;
        }

    }
   
}