using Acr.UserDialogs;
using Milenio.Mobile.Model;
using Milenio.Mobile.Services;
using Mobile.Helpers;
using Mobile.Model;
using Mobile.Views;
using Mobile.WebApi;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;



namespace Mobile.ViewModel
{
    public class QuestoesViewModel : INotifyPropertyChanged
    {
        public IGerenciadorFotos GerenciadorFotos { get; }
        static int _idAspecto;
        MidiaAspecto MidiaAspecto = new MidiaAspecto();
        static AspectoEnviarCentral aspectoApi;
        DatabaseHelper db = new DatabaseHelper();
        int idQuestao;
        int idResposta;

        public QuestoesViewModel(int idAspecto)
        {
            GerenciadorFotos = DependencyService.Get<IGerenciadorFotos>();
            MyList = new ObservableCollection<QuestionarioModelClass>();
            _idAspecto = idAspecto;
            FillData(idAspecto);
            DesabilitarQuestoesAnulaveis(idQuestao, idResposta);
            RadionButtonCommandRespostaQuestao = new Command(async (model) =>
            {
                QuestionarioModelClass questaoSelecionada = (QuestionarioModelClass)model;
                var idQuestao = Convert.ToInt32(questaoSelecionada.id);
                var respostaQuestao = Convert.ToInt32(questaoSelecionada.resposta);
                DesabilitarQuestoesAnulaveis(idQuestao, respostaQuestao);
            });
            ButtonClickCommandEnviarQuestionario = new Command(async (model) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    IList<QuestionarioModelClass> relacaoQuestoes = (IList<QuestionarioModelClass>)model;
                    using (UserDialogs.Instance.Loading("Sincronizando Questionário.", null, null, true, MaskType.Black))
                    {
                        await EnviarQuestionario(relacaoQuestoes, idAspecto);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            UserDialogs.Instance.HideLoading();

                        });

                    }
                });
            });
            ButtonClickCommandFoto = new Command(async (model) =>
            {
                try
                {
                    
                    MediaFile file;
                    IMedia media = CrossMedia.Current;
                    QuestionarioModelClass questoes = (QuestionarioModelClass)model;
                    MidiaQuestoes midiaQuestoes;
                    DatabaseHelper service = new DatabaseHelper();
                    var qtdMidia = service.GetqtdMidia();
                    var qtdQuestoes = service.QuantidadeMidiaQuestoes(questoes.id);
                    if (qtdQuestoes < qtdMidia.data)
                    {
                        await CrossMedia.Current.Initialize();
                        bool answer = await Application.Current.MainPage.DisplayAlert("", "Deseja Obter imagem através ", "Galeria", "Câmera");
                        if (answer)
                        {

                            //Procedimento para capturar foto da galeria
                            try
                            {
                                /*MediaFile _mediaFile;
                                if (!CrossMedia.Current.IsPickPhotoSupported)
                                {
                                    System.Diagnostics.Debug.WriteLine("Erro.");
                                    return;
                                }
                                file = await media.PickPhotoAsync();
                                if (file == null)
                                {
                                    await Application.Current.MainPage.DisplayAlert("", "Mídia não selecionada.", "OK");
                                    return;
                                }
                                else
                                {
                                    string caminho = file.Path;
                                    string filename = null;
                                    filename = Path.GetFileName(caminho);
                                    file.Dispose();
                                    midiaQuestoes = new MidiaQuestoes();
                                    midiaQuestoes.questaoVisitaId = questoes.id;
                                    midiaQuestoes.dataGravacao = DateTime.Now;
                                    midiaQuestoes.legenda = filename;
                                    midiaQuestoes.caminho = caminho;
                                    service.InsertMidiaQuestoes(midiaQuestoes);

                                }*/
                                 await GerenciadorFotos.MidiaAsync();
                                //ImageSource imagem = ImageSource.FromStream(() => imageStream);


                            }
                            catch (Exception ex)
                            {
                                await Application.Current.MainPage.DisplayAlert("", ex.Message, "OK");
                            }
                            //Fim Procedimento para capturar foto da galeria

                        }
                        else
                        {
                            //Procedimento para Tirar Foto para questão.
                            string _datahora;
                            var _data = DateTime.Now;
                            var day = _data.Day.ToString();
                            var month = _data.Month.ToString();
                            var year = _data.Year.ToString();
                            var hora = _data.Hour.ToString();
                            var minuto = _data.Minute.ToString();
                            var segundo = _data.Second.ToString();
                            _datahora = day + "_" + month + "_" + year + "_" + hora + "_" + minuto + "_" + segundo;
                            if (!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
                            {
                                await Application.Current.MainPage.DisplayAlert("Ops", "Nenhuma câmera detectada.", "OK");

                                return;
                            }
                            try
                            {
                                file = await CrossMedia.Current.TakePhotoAsync(
                                new StoreCameraMediaOptions
                                {
                                    PhotoSize = PhotoSize.Small,
                                    CompressionQuality = 92,
                                    SaveToAlbum = true,
                                    Name = questoes.id.ToString() + "_" + _datahora,
                                    Directory = "Milenio"


                                });
                                string caminho = file.AlbumPath;

                                if (file == null)
                                {

                                    return;
                                }
                                else
                                {
                                    midiaQuestoes = new MidiaQuestoes();
                                    midiaQuestoes.questaoVisitaId = questoes.id;
                                    midiaQuestoes.dataGravacao = DateTime.Now;
                                    midiaQuestoes.legenda = questoes.id.ToString() + "_" + _datahora + ".jpg";
                                    midiaQuestoes.caminho = caminho;
                                    service.InsertMidiaQuestoes(midiaQuestoes);
                                }

                            }
                            catch (Exception ex)
                            {

                                throw;
                            }

                            // Fim Procedimento para tirar Foto para questão
                        }
                    }
                    else
                    {

                        string msg = "Limite atingido de " + qtdMidia.data + " mídias por pergunta. ";
                        await Application.Current.MainPage.DisplayAlert("", msg, "OK");
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("", "Mídia não selecionada.", "OK");
                }
            });

        }

        private void DesabilitarQuestoesAnulaveis(int idQuestao,int respostaQuestao)
        {
            var questoesAnuladas = db.GetQuestoesAnuladas(idQuestao);
            if (questoesAnuladas != null && questoesAnuladas.Any())
            {
                var anuladas = MyList.Where(x => questoesAnuladas.Any(a => a.id == x.id));
                if (anuladas.Any())
                {
                    foreach (var anulada in anuladas)
                    {
                        if (questoesAnuladas.Any(x => x.respostaId == respostaQuestao && x.id == anulada.id))
                        {
                            anulada.habilitado = false;
                            anulada.questaoAnulada = true;
                            //Altera as respostas das questões anuladas no questionário
                            if ((anulada.resposta == 1)||(anulada.resposta == 2))
                            {
                                anulada.resposta = 2;
                            }
                            else
                            {
                                anulada.resposta = 2;
                            }
                            // Conclusão desta alteração
                            DesabilitarQuestoesAnulaveis(anulada.id, anulada.resposta);
                        }
                        else
                        {
                            anulada.habilitado = true;
                            anulada.questaoAnulada = false;
                            
                        }
                    }
                }
            }
        }

        public async Task EnviarQuestionario(IList<QuestionarioModelClass> relacaoQuestoes, int idAspecto)
        {
           if (CrossConnectivity.Current.IsConnected)
           {
                    GerarNovoTokenAsync(Settings.Login);
                    List<MidiaQestoesApi> _MidiaQuestao = new List<MidiaQestoesApi>();
                    List<QuestoesEnviarCentral> questoesEnviar = new List<QuestoesEnviarCentral>();
                    aspectoApi = new AspectoEnviarCentral();
                    List<MidiaAspectoApi> _MidiaAspecto = new List<MidiaAspectoApi>();
                    // IList<QuestionarioModelClass> relacaoQuestoes = (IList<QuestionarioModelClass>)model;
                    validaQuestionario(relacaoQuestoes);
                    var pendencia = db.QuantidadeQuestoesAspectoNaoRespondida(idAspecto);
                    if (pendencia == 0)
                    {

                        //1 - Veriicar se todoas as questões foram preenchidas
                        var questoes = db.TotalQuestoesAspecto(idAspecto);
                        var infoAspecto = db.GetAspectoCabecalho(idAspecto);
                        foreach (var item in questoes)
                        {
                            aspectoApi.observacao = item.respostaAspecto;
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
                                pergunta = item.pergunta,
                                midias = _MidiaQuestao,
                                questaoAnulada = item.questaoAnulada

                            });
                            //midiaQuestoesAsync(item.id); - ENVIAR MIDIA POR QUESTÃO
                         }
                        aspectoApi.id = idAspecto;
                        aspectoApi.descricao = infoAspecto.descricao;
                        aspectoApi.visitaId = infoAspecto.visitaId;
                        aspectoApi.dataFinalizacao = DateTime.Now;
                        aspectoApi.usuarioFinalizacaoId = Convert.ToInt32(Settings.idUsuario);
                        aspectoApi.formaColetaId = infoAspecto.formaColetaId;
                        aspectoApi.observacao = infoAspecto.observacao;
                        aspectoApi.midias = _MidiaAspecto;
                        aspectoApi.questoes = questoesEnviar;
                        await EnviarQuestionarioCentral();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("", "Questão(ões) não preenchida.", "OK");
                        
                    }
           }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Internet", "Verifique sua conexão com a internet.", "OK");
            }              
        }
        public void AnularQuestoesAbertas(List<QuestoesAnuladas> questoesAnuladas)
        {
            foreach (var item in questoesAnuladas)
            {
                for (int i = 0; i < MyList.Count; i++)
                {
                    if ((item.id == MyList[i].id) && (!MyList[i].perguntaAberta))
                    {
                        MyList[i].questaoAnulada = true;
                    }
                }
            }
        }
        /*public QuestoesViewModel(int idAspecto)
        {
            _idAspecto = idAspecto;
            MyList = new ObservableCollection<QuestionarioModelClass>();
            FillData(idAspecto);

        }*/
        public Command RadionButtonCommandRespostaQuestao { get; set; }
        public ObservableCollection<QuestionarioModelClass> MyList { get; set; }
        
        private ObservableCollection<QuestionarioModelClass> _selectedItem;
        public ObservableCollection<QuestionarioModelClass> SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value; OnPropertyChanged();

            }
        }
        public void FillData(int idAspecto)
        {
            List<Questoes> questoes = new List<Questoes>();
            bool quantidadeMidia=true;
            _idAspecto = idAspecto;
            var respostaAspecto = db.GetAspecto(idAspecto);
            questoes = db.GetQuestoes(idAspecto);
            foreach (var item in questoes)
            {
                //var qtdMidia = db.QuantidadeMidiaQuestoes(item.id);
                
                MyList.Add(new QuestionarioModelClass
                {
                    id = item.id,
                    numeracao = item.numeracao,
                    descricao = item.descricao,
                    resposta = Convert.ToInt32(item.resposta) - 1,
                    observacao = item.observacao,
                    questao = Convert.ToString(item.numeracao) + " " + item.descricao,
                    pergunta = item.pergunta,
                    perguntaAberta = !item.perguntaAberta ? true : false,
                    questaoAnulada = false,
                    respostaAspecto = respostaAspecto.observacao,
                    radioButtonResposta = true,
                    habilitado = true,
                    textObervacao = true,
                    IdAspecto = idAspecto,
                    quantidadeMidia = quantidadeMidia
                });
            }
           var listquestao = MyList;
            if (listquestao.Count > 0)
            {
                idQuestao=listquestao[0].id;
                idResposta = listquestao[0].resposta;
            }
        }
        public Command ButtonClickCommandFoto { get; set; } 
       
        public Command ButtonClickCommandGaleria { get; set; } = new Command(async (model) =>
        {
            try
            {
                QuestionarioModelClass questoes = (QuestionarioModelClass)model;
                await Application.Current.MainPage.Navigation.PushAsync(new GaleriaMidiasQuestoes(questoes.id, questoes.questao), true);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("", "Operação inválida..", "OK");
            }
        });

        // Rotina Perguntas Fechadas
        public Command ButtonClickCommandRespostaSalvar { get; set; } = new Command(async (model) =>
       {
           try
           {
               using (var Dialog = UserDialogs.Instance.Loading("Aguarde..", null, null, true, MaskType.Black))
               {

                   Questoes questoesResultado = new Questoes();
                   DatabaseHelper db = new DatabaseHelper();

                   IList<QuestionarioModelClass> questoes = (IList<QuestionarioModelClass>)model;
                   foreach (var item in questoes)
                   {   
                       int _resposta = -1;
                       questoesResultado.id = item.id;
                       questoesResultado.observacao = item.observacao;
                       questoesResultado.perguntaAberta = item.perguntaAberta;
                       questoesResultado.resposta = item.resposta.ToString();
      
                       //db.UpdateAspecto(item.IdAspecto, item.respostaAspecto);
                       _resposta = item.resposta;
                       if (_resposta == 0)
                       {
                           questoesResultado.resposta = "1";
                       }
                       else if (_resposta == 1)
                       {
                           questoesResultado.resposta = "2";
                       }
                       else if (_resposta == 2)
                       {
                           questoesResultado.resposta = "3";
                       }
                       if (questoesResultado.perguntaAberta)
                       {
                           if (questoesResultado.resposta != "-1")
                           {
                               db.UpdateQuestoesFechada(questoesResultado.id, questoesResultado.resposta, questoesResultado.observacao);
                           }

                       }
                       else if (questoesResultado.observacao != null)
                       {
                           db.UpdateQuestoesAberta(questoesResultado.id, questoesResultado.observacao);
                       }
                       if (!item.pergunta)
                       {
                           db.UpdatePergunta(questoesResultado.id);
                       }
                   }
                   await Task.Delay(3000);
                   await Application.Current.MainPage.DisplayAlert("", "Operação efetuada com sucesso.", "OK");
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       });
        public Command ButtonClickCommandEnviarQuestionario { get; set; }
        public async Task EnviarQuestionarioCentral()
        {
            try
            {
                string jsonRequest = JsonConvert.SerializeObject(aspectoApi);
                string resp = string.Empty;
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);
                StringContent httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application / json");
                var endereco = "http://10.130.155.44/milenioapi/api/";
                //var endereco = "http://milenio-app.tst.sistemas.intranet.mpba.mp.br/api/";
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
                   await midiaQuestoes(_idAspecto);
                   await Application.Current.MainPage.DisplayAlert("", "Operação efetuada com sucesso.", "Ok");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("", "Questão não preenchida.", "Ok");
                }
                
            }
            catch (Exception ex)
            {

                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
            }

        }
        // Fim Rotina Perguntas Fechadas

        // O método midiaQuestoes, recebe o Id do aspecto e verifica as questões que possui mídias associadas
        public List<EnviarMidiaQuestao> enviarFileQuestao = new List<EnviarMidiaQuestao>(); 
        public async Task midiaQuestoes(int IdAspecto)
        {
            int i = 0;
            var questoesAspecto = db.GetQuestoes(IdAspecto);
            foreach (var questaoMidia in questoesAspecto)
            {
                var relacaoMidias = db.ConfirmMidiaQuestoes(questaoMidia.id);
                foreach (var midia in relacaoMidias)
                {
                    i--;
                    enviarFileQuestao.Add(new EnviarMidiaQuestao()
                    {
                        id = i,
                        questaoVisitaId = midia.questaoVisitaId,
                        dataGravacao = midia.dataGravacao.ToString("yyyy-MM-dd"),
                        legenda = midia.legenda,
                        fileImage = new List<FileInfo>(){
                           new FileInfo(midia.caminho)
                        }
                    });
                    await EnviarMidiaQuestao(enviarFileQuestao);
                    enviarFileQuestao.Clear();


                }
               
            }              
        }
        public async Task EnviarMidiaQuestao(List<EnviarMidiaQuestao> enviarFileQuestao)
        {
            DatabaseHelper db = new DatabaseHelper();
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);
                var multipartFormDataContent = new MultipartFormDataContent();
                var endereco = "http://10.130.155.44/milenioapi/api/";
                //var endereco = "http://milenio-app.tst.sistemas.intranet.mpba.mp.br/api/";

                CabecalhoImagemUpload cabecalhoImagemUpload = new CabecalhoImagemUpload();
                var url = string.Format("visita/adicionar-midias-a-questao");
                foreach (var item in enviarFileQuestao)
                {
                    httpClient.BaseAddress = new Uri(endereco);
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
                    db.ConfirmUploadImage(item.questaoVisitaId, item.legenda);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("", ex.Message, "Ok");
            }

        }
        public void validaQuestionario(IList<QuestionarioModelClass> resposta)
        {
            Questoes questoesResultado = new Questoes();
           
            foreach (var item in resposta)
            {
                int _resposta = -1;
                questoesResultado.id = item.id;
                questoesResultado.observacao = item.observacao;
                questoesResultado.perguntaAberta = item.perguntaAberta;
                questoesResultado.resposta = item.resposta.ToString();
                _resposta = item.resposta;
                if (_resposta == 0)
                {
                    questoesResultado.resposta = "1";
                }
                else if (_resposta == 1)
                {
                    questoesResultado.resposta = "2";
                }
                else if (_resposta == 2)
                {
                    questoesResultado.resposta = "3";
                }
                if (questoesResultado.perguntaAberta)
                {
                    if (questoesResultado.resposta != "-1")
                    {
                        db.UpdateQuestoesFechada(questoesResultado.id, questoesResultado.resposta, questoesResultado.observacao);
                    }


                }
                else if (questoesResultado.observacao != null)
                {
                    db.UpdateQuestoesAberta(questoesResultado.id, questoesResultado.observacao);
                }
                if (item.questaoAnulada)
                {
                    db.UpdatequestaoAnulada(questoesResultado.id);
                }
                if (!item.pergunta)
                {
                    db.UpdatePergunta(questoesResultado.id);
                }

            }
        }
        public async void GerarNovoTokenAsync(string Login)
        {
          
            var Usuario = db.GetUsuarioDataLogin(Login);
            string usuarioLogin = Settings.Login;
            bool valido = usuarioLogin.Length == 14 && usuarioLogin.All(char.IsDigit);
            if (valido)
            {
                usuarioLogin = usuarioLogin.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
            }

            var loginuser = new LoginUser
            {
                login = Usuario.login,
                senha = Usuario.senha
            };
            string jsonRequest = JsonConvert.SerializeObject(loginuser);
            StringContent httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application / json");
            string resp = string.Empty;
            var client = new HttpClient();
            var endereco = "http://10.130.155.44/milenioapi/api/";
            //var endereco = "http://milenio-app.tst.sistemas.intranet.mpba.mp.br/api/";
            client.BaseAddress = new Uri(endereco);
            var url = "usuario/autenticar";
            try
            {
                var result = await client.PostAsync(url, httpContent);
                if (!result.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", "Erro Servidor Central", "OK");
                }
                else
                {
                    resp = await result.Content.ReadAsStringAsync();
                    LoginUser user = JsonConvert.DeserializeObject<LoginUser>(resp);
                    Settings.Token = user.data.token;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
        }
        public  async Task OpenMygallery()
        {
            try
            {
                await CrossMedia.Current.Initialize();
               

               
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception:>" + ex);
            }
        }
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        #endregion
    }

}
