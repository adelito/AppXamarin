using Acr.UserDialogs;
using Milenio.Mobile.ViewModel;
using Mobile.Helpers;
using Mobile.Model;
using Mobile.ViewModel;
using Mobile.WebApi;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PreencherQuestionarioQuestoes : ContentPage
	{

        DatabaseHelper service = new DatabaseHelper();
        MidiaAspecto MidiaAspecto = new MidiaAspecto();
        List<MidiaAspectoApi> _MidiaAspecto = new List<MidiaAspectoApi>();
        List<MidiaQestoesApi> _MidiaQuestao = new List<MidiaQestoesApi>();
        AspectoQuestionario aspecto;
        int idAspecto;
        string nomeAspecto;
        int idquestao;
        int _idquestao;
        List<QuestoesEnviarCentral> questoesEnviar = new List<QuestoesEnviarCentral>();
        AspectoEnviarCentral aspectoApi = new AspectoEnviarCentral();
       
        List<FileInfo> fileImage;
        public ObservableCollection<QuestoesViewModel> ListaItens { get; }

        public PreencherQuestionarioQuestoes (object item, QuestionarioBaixadoModel Cabecalho)
		{
			InitializeComponent ();
            //BindingContext = item;
            var questoesViewModel = new QuestoesViewModel(idAspecto);
            aspecto = (AspectoQuestionario) item;
            idAspecto = aspecto.id;
            nomeAspecto = aspecto.descricao;
            lbProtocolo.Text = Cabecalho.Protocolo;
            lbDataVisita.Text = Cabecalho.DataVisita;
            lbCidade.Text = Cabecalho.Cidade;
            lbInstituicao.Text = Cabecalho.Instituicao;
            lbAspecto.Text = nomeAspecto;
            //var observacaoAspecto = service.GetAspecto(idAspecto);
            UpAspecto(idAspecto);
            //txtObeservacaoAspect.Text = aspecto.observacao;
           
            this.BindingContext = new QuestoesViewModel(idAspecto);
          
        }
        public void UpAspecto(int idAspecto)
        {
            var aspecto=service.GetAspecto(idAspecto);
            txtObeservacaoAspect.Text = aspecto.observacao;

        }

        // Rotina de Foto e Galeria do Aspecto 
        private async void BtCamera_Aspecto()
        {
           
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
            {
                await DisplayAlert("Ops", "Nenhuma câmera detectada.", "OK");

                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear,
                    SaveToAlbum = true,
                    Name = idAspecto + "_" + nomeAspecto,
                    Directory = "Milenio",
                   
                });
            string caminho = file.AlbumPath;
            
            if (file == null)
            {
                return;
            }
            else
            {
                MidiaAspecto.aspectoVisitaId = idAspecto;
                MidiaAspecto.dataGravacao = DateTime.Now;
                MidiaAspecto.legenda = idAspecto + "_" + nomeAspecto;
                MidiaAspecto.caminho = caminho;
                service.InsertMidiasAspectos(MidiaAspecto);
            }
               
        }
       
        
      
        private async void EscolherFotoAspecto()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            { 
               await DisplayAlert("Error", "Galeria de fotos não suportada.", "OK");

                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();
            string caminho = file.Path;

            if (file == null)
            {
                return;
            }
            else
            {
                MidiaAspecto.aspectoVisitaId = idAspecto;
                MidiaAspecto.dataGravacao = DateTime.Now;
                MidiaAspecto.legenda = idAspecto + "_" + nomeAspecto;
                MidiaAspecto.caminho = caminho;
                service.InsertMidiasAspectos(MidiaAspecto);
            }
                

        }
        private async void BtGaleriaAspecto_Clicked(object sender, EventArgs e)
        {

            QuestionarioBaixadoModel questionarioBaixadoModel = new QuestionarioBaixadoModel();
            questionarioBaixadoModel.Protocolo = lbProtocolo.Text;
            questionarioBaixadoModel.DataVisita = lbDataVisita.Text;
            questionarioBaixadoModel.Cidade = lbCidade.Text;
            questionarioBaixadoModel.Instituicao = lbInstituicao.Text;
            await Navigation.PushAsync(new GaleriaMidiasAspecto(questionarioBaixadoModel, idAspecto, nomeAspecto,aspecto), true);
        }

        public async void TipoFotoAspecto(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("", "Deseja Obter imagem através ", "Galeria", "Câmera");
            if (answer)
            {
                EscolherFotoAspecto();
            }
            else
            {
                BtCamera_Aspecto();
            }
        }
        // Fim Rotina Galeria e Foto Aspecto

        private async void GravarVideo(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsTakeVideoSupported || !CrossMedia.Current.IsCameraAvailable)
            {
                await DisplayAlert("Ops", "Nenhuma câmera detectada.", "OK");

                return;
            }

            var file = await CrossMedia.Current.TakeVideoAsync(
                new StoreVideoOptions
                {
                    SaveToAlbum = true,
                    Directory = "Demo",
                    Quality = VideoQuality.Medium
                });

            if (file == null)
                return;

        }

        private async void EscolherVideo(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickVideoSupported)
            {
                await DisplayAlert("Ops", "Galeria de videos não suportada.", "OK");

                return;
            }

            var file = await CrossMedia.Current.PickVideoAsync();

            if (file == null)
                return;
            /*
            MinhaImagem.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;

            });*/
        }

        private async void BtSalvarAspecto_Clicked(object sender, EventArgs e)
        {
            service.UpdateAspecto(idAspecto, txtObeservacaoAspect.Text);
            await DisplayAlert("Preencher Questionário", "Operação efetuada com sucesso.", "Ok");
        }


        //METODOS DAS QUESTÕES

        public async void BtTipoFotoQuestao(object sender, SelectedItemChangedEventArgs e)
        {
            Questoes listquestao =  e.SelectedItem as Questoes;
            await DisplayAlert("",Convert.ToString(idquestao), "OK");

        }
        private async void lvQuestoes_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ListView listQuestao = (ListView)sender;
            await DisplayAlert("", "IDQuestão", "OK");
        }
        /*
        private void LvQuestoes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Questoes listquestao = e.SelectedItem as Questoes;
            idquestao =  listquestao.id;
            int resposta = listquestao.resposta;
        }
        */


        private void MainCarouselView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            MidiaAspecto file = e.SelectedItem as MidiaAspecto;
            var nomecaminho = file.caminho.ToString();

        }

        public async void BtEnviarAspecto_Clicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var pendencia = service.QuantidadeQuestoesAspectoNaoRespondida(idAspecto);
                if (pendencia == 0)
                {
                    using (var Dialog = UserDialogs.Instance.Loading("Aguarde..", null, null, true, MaskType.Black))
                    {
                        //1 - Veriicar se todoas as questões foram preenchidas
                        var questoes = service.TotalQuestoesAspecto(idAspecto);
                        foreach (var item in questoes)
                        {
                            questoesEnviar.Add(new QuestoesEnviarCentral()
                            {
                                id = item.id,
                                aspectoId = item.aspectoId,
                                descricao = item.descricao,
                                respostaNegativaId = item.respostaNegativaId,
                                ordem = item.ordem,
                                numeracao = item.numeracao,
                                descricaoDevolutiva = item.descricaoDevolutiva,
                                respostaId = (item.resposta == "-1") ? null : item.resposta,
                                observacao = item.observacao,
                                perguntaAberta = item.perguntaAberta,
                                midias = _MidiaQuestao
                            });
                           await midiaQuestoesAsync(item.id);
                            
                        }
                        aspectoApi.id = idAspecto;
                        aspectoApi.descricao = aspecto.descricao;
                        aspectoApi.visitaId = aspecto.visitaId;
                        aspectoApi.dataFinalizacao = DateTime.Now;
                        aspectoApi.usuarioFinalizacaoId = Convert.ToInt32(Settings.idUsuario);
                        aspectoApi.formaColetaId = aspecto.formaColetaId;
                        aspectoApi.observacao = aspecto.observacao;
                        aspectoApi.midias = _MidiaAspecto;
                        aspectoApi.questoes = questoesEnviar;
                        EnviarQuestionarioCentral();
                        await Task.Delay(3000);
                    }
                }
                else
                {
                    await DisplayAlert("", "Aspecto incompleto.", "OK");
                }
                
            }
            else
            {
                await DisplayAlert("Internet", "Verifique sua conexão com a internet.", "OK");
            }
        }
        
        public async void EnviarQuestionarioCentral()
        {
            try
            {
                using (var Dialog = UserDialogs.Instance.Loading("Sincronizando..", null, null, true, MaskType.Black))
                {
                    string jsonRequest = JsonConvert.SerializeObject(aspectoApi);
                    string resp = string.Empty;
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);
                    StringContent httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application / json");
                    var endereco = Settings.EnderecoApi;
                    client.BaseAddress = new Uri(endereco);
                    var url = string.Format("visita/finalizar-aspecto-questionario");
                    var result = await client.PutAsync(url, httpContent);
                    if (!result.IsSuccessStatusCode)
                    {
                        throw new Exception(result.RequestMessage.Content.ToString());
                    }
                    resp = await result.Content.ReadAsStringAsync();
                    var resposta = JsonConvert.DeserializeObject<AspectoApi>(resp);
                    if (resposta.success)
                    {
                        await Task.Delay(3000);
                        await DisplayAlert("", "Operação efetuada com sucesso.", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("", "Questão não preenchida.", "Ok");
                    }
                    //resposta.
                    
                }
            }
            catch (Exception ex)
            {

                await DisplayAlert("Erro", ex.Message, "Ok");
            }
            
        }
         public async Task EnviarMidiaQuestao(List<EnviarMidiaQuestao> enviarFileQuestao)
        {
            try
            {
               
                foreach (var item in enviarFileQuestao)
                {
                    HttpClient httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);
                    var multipartFormDataContent = new MultipartFormDataContent();
                    var endereco = Settings.EnderecoApi;
                    httpClient.BaseAddress = new Uri(endereco);
                    CabecalhoImagemUpload cabecalhoImagemUpload = new CabecalhoImagemUpload();
                    var url = string.Format("visita/adicionar-midias-a-questao");
                    cabecalhoImagemUpload.questaoVisitaId = item.questaoVisitaId;
                    cabecalhoImagemUpload.dataGravacao = item.dataGravacao;
                    cabecalhoImagemUpload.legenda = item.legenda;
                    foreach (var fileInfo in item.fileImage)
                    {
                        string jsonRequest = JsonConvert.SerializeObject(cabecalhoImagemUpload);
                        StringContent httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/x-www-form-urlencoded");
                        var fileContent = new ByteArrayContent(File.ReadAllBytes(fileInfo.FullName));
                        multipartFormDataContent.Add(fileContent, "files", fileInfo.Name);
                        multipartFormDataContent.Add(httpContent, Convert.ToString(-1));
                        HttpResponseMessage result = httpClient.PostAsync(url, multipartFormDataContent).Result;
                    }
                  
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("", ex.Message, "Ok");
            }
               
        }

        private List<EnviarMidiaQuestao> enviarFileQuestao = new List<EnviarMidiaQuestao>();
        public async Task midiaQuestoesAsync(int idQuestao)
        {
           
            int i = 0;
            var midias = service.GetMidiaQuestoes(idQuestao);
            foreach (var item in midias)
            {
              
                i--;
                enviarFileQuestao.Add(new EnviarMidiaQuestao()
                {
                    id = i,
                    questaoVisitaId = item.questaoVisitaId,
                    dataGravacao = item.dataGravacao.ToString("yyyy-MM-dd"),
                    legenda = item.legenda,
                    fileImage = new List<FileInfo>(){
                    new FileInfo(item.caminho)
                    }
                });
                
            }
            await EnviarMidiaQuestao(enviarFileQuestao);
            enviarFileQuestao.Clear(); 
        }

        private void TxtObeservacaoAspect_Unfocused(object sender, FocusEventArgs e)
        {
            service.UpdateAspecto(idAspecto, txtObeservacaoAspect.Text);
           
        }
    }
}