using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Mobile.Services;
using Mobile.Model;
using Milenio.Mobile.Views;
using Milenio.Mobile.Validacao;
using Milenio.Mobile.WebApi;
using Milenio.Mobile.Model;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Mobile.Helpers;
using Acr.UserDialogs;


namespace Mobile
{
    public partial class MainPage : ContentPage
    {
        Validacao validacaocpf = new Validacao();
        LoginToken getLogin = new LoginToken();
        Usuario usuario = new Usuario();
        UsuarioService service = new UsuarioService();
        MunicipioToken municipio = new MunicipioToken();
        DatabaseHelper db = new DatabaseHelper();
        // InstitutoToken institutos = new InstitutoToken();
        HttpResponseMessage result;
        string token;

        public MainPage()
        {

            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            lbEsqueciSenha.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => OnLabelClicked()),
            });
            txtUsuario.Completed += TxtUsuario_CompletedAsync;
            this.BindingContext = this;
            this.IsBusy = false;
            btClikedAutenticacao.Clicked += ClikedAutenticacao;
            



        }

        public async void TxtUsuario_CompletedAsync(object sender, EventArgs e)
        {
            string user = txtUsuario.Text;
            if (user is null)
            {
                await DisplayAlert("Campo Obrigatório", "Usuário e Senha", "OK");
            }
            else
            {
                bool valido = user.Length == 11 && user.All(char.IsDigit);
                if (valido)
                {
                    var valicaoCpf = validacaocpf.IsCpf(Convert.ToUInt64(user).ToString(@"000\.000\.000\-00"));
                    if (valicaoCpf)
                    {
                        txtUsuario.Text = Convert.ToUInt64(user).ToString(@"000\.000\.000\-00");
                    }
                    else
                    {
                        await DisplayAlert("Validação CPF", "CPF Invalido", "OK");
                    }

                }
            }
        }

        private void OnLabelClicked()
        {
            Navigation.PushAsync(new Milenio.Mobile.Views.EsqueciSenha(), true);
        }
        
        public async void ClikedAutenticacao(object sender, EventArgs e)
        {
           
            if (string.IsNullOrEmpty(txtUsuario.Text))
            {
                await DisplayAlert("Campo Obrigatório", "Usuário", "Ok");
                txtUsuario.Focus();
                btClikedAutenticacao.IsEnabled = true;
                return;
            }

            if (string.IsNullOrEmpty(txtSenha.Text))
            {
                await DisplayAlert("Campo Obrigatório", "Senha", "Ok");
                txtSenha.Focus();
                btClikedAutenticacao.IsEnabled = true;
                return;
            }
            if (txtSenha.Text.Length<6)
            {
                await DisplayAlert("Senha", "A senha deve ter no mínimo 6 caracteres.", "Ok");
                txtSenha.Focus();
                btClikedAutenticacao.IsEnabled = true;
                return;
            }

            if (CrossConnectivity.Current.IsConnected)
            {
                //Settings.EnderecoApi = "http://milenio-app.tst.sistemas.intranet.mpba.mp.br/api/"; 
                Settings.EnderecoApi = "http://10.130.155.44/milenioapi/api/";
                string usuarioLogin = txtUsuario.Text;
                bool valido = usuarioLogin.Length == 14 && usuarioLogin.All(char.IsDigit);
                if(valido)
                {
                    usuarioLogin = usuarioLogin.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
                }
                this.IsBusy = true;
               
                var loginuser = new LoginUser
                {
                    login = usuarioLogin,
                    senha = txtSenha.Text
                };

                string jsonRequest = JsonConvert.SerializeObject(loginuser);
                StringContent httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application / json");
                string resp = string.Empty;
                var client = new HttpClient();
                var endereco = Settings.EnderecoApi;
                client.BaseAddress = new Uri(endereco);
                var url = "usuario/autenticar";
                try {
                    result = await client.PostAsync(url, httpContent);
                    if (!result.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Erro", "Erro Servidor Central", "OK");
                    }
                    else
                    {
                        resp = await result.Content.ReadAsStringAsync();

                        LoginUser user = JsonConvert.DeserializeObject<LoginUser>(resp);
                        if (user.success)
                        {
                            usuario.id = user.data.id;
                            usuario.login = txtUsuario.Text;
                            usuario.senha = txtSenha.Text;
                            usuario.DsNome = user.data.nome;
                            usuario.orgaoExternoId = user.data.orgaoExternoId; 
                            token = user.data.token;
                            //Consumindo Lista de Institutos - Ba
                            //institutos.Institutos(user.data.token.ToString());
                            //institutos.Institutos(user.data.token.ToString());
                            Settings.Login = txtUsuario.Text;
                            Settings.Token = token;
                            Settings.Nome = usuario.DsNome.ToString();
                            Settings.idUsuario = usuario.id.ToString();
                            Settings.emissorExternoId = usuario.orgaoExternoId.ToString();
                            //Veiricando se o usuario já possui cadastro no milenio.db - tbUsuario
                            Usuario usuarioexist = service.GetUsuarioDataLogin(txtUsuario.Text);
                            await QtdMidiaAsync();
                            if (usuarioexist != null)
                            {
                                using (var Dialog = UserDialogs.Instance.Loading("Aguarde...", null, null, true, MaskType.Black))
                                {

                                    await Navigation.PushAsync(new MasterDetail(usuarioexist), true);
                                    await Task.Delay(5000);
                                }
                            }
                            else
                            {
                                using (var Dialog = UserDialogs.Instance.Loading("Sincronizando...", null, null, true, MaskType.Black))
                                {
                                    btClikedAutenticacao.IsEnabled = false;
                                    service.InsertUsuario(usuario);
                                    var dbExist = db.DBExists();
                                    //if (!dbExist) --> Verificar se existe usuário cadastrado
                                    //{
                                    //Consumindo Lista de Institutos - Ba
                                    await Navigation.PushAsync(new MasterDetail(usuario), true);
                                    Institutos(user.data.token.ToString());
                                        //Consumindo Lista de Cidades - Ba
                                        municipio.ApiMunicipio(user.data.token.ToString());
                                    
                                    //} 
                                    await Task.Delay(60000);
                                   

                                }
                            }
                        }
                        else
                        {
                            await DisplayAlert("Erro", user.errors[0].ToString(), "OK");

                        }

                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro", ex.Message, "OK");
                }
                
                  
            }
            else
            {
                using (var Dialog = UserDialogs.Instance.Loading("Aguarde..", null, null, true, MaskType.Black))
                {
                    
                    btClikedAutenticacao.IsEnabled = false;
                    usuario = service.GetUsuarioData(txtUsuario.Text, txtSenha.Text);
                    if (usuario != null)
                    {
                        await Navigation.PushAsync(new MasterDetail(usuario), true);
                        await Task.Delay(3000);
                    }
                    else
                    {
                        await DisplayAlert("Internet", "Verifique sua conexão com a internet.", "OK");
                    }
                }
            }
            btClikedAutenticacao.IsEnabled = true;
        }

        public async void Institutos(string token)
        {
            InstitutoApi _Institutos = new InstitutoApi();
            InstitutoService service = new InstitutoService();
            List<Instituto> listInstituto = new List<Instituto>();
            Instituto instituto = new Instituto();
            string respOrgao = string.Empty;
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var endereco = Settings.EnderecoApi;
                client.BaseAddress = new Uri(endereco);
                var url = "instituicao/obter-instituicoes";
                var resultOrgao = await client.GetAsync(url);

                if (!resultOrgao.IsSuccessStatusCode)
                {
                    throw new Exception(resultOrgao.RequestMessage.Content.ToString());
                }

                respOrgao = await resultOrgao.Content.ReadAsStringAsync();


                _Institutos = JsonConvert.DeserializeObject<InstitutoApi>(respOrgao);

                foreach (var obj in _Institutos.data)
                {

                    listInstituto.Add(new Instituto()
                    {
                        Id = obj.Id,
                        CidadeId = obj.CidadeId,
                        TipoInstituicaoId = obj.TipoInstituicaoId,
                        NomeInstituto = obj.nome
                    });
                }
                service.InserirInstituto(listInstituto);

            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }
        private async void TxtUsuario_TextChanged(object sender, TextChangedEventArgs e)
        {
            string user = txtUsuario.Text;
            if (user is null)
            {
                await DisplayAlert("Campo Obrigatório", "Usuário e Senha", "OK");
            }
            else
            {
                bool valido = user.Length == 11 && user.All(char.IsDigit);
                if (valido)
                {
                    var valicaoCpf = validacaocpf.IsCpf(Convert.ToUInt64(user).ToString(@"000\.000\.000\-00"));
                    if (valicaoCpf)
                    {
                        txtUsuario.Text = Convert.ToUInt64(user).ToString(@"000\.000\.000\-00");
                    }
                    else
                    {
                        await DisplayAlert("Validação CPF", "CPF Invalido", "OK");
                    }

                }
            }
        }

        private async Task QtdMidiaAsync()
        {
           
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var endereco = Settings.EnderecoApi;
                client.BaseAddress = new Uri(endereco);
                var url = "visita/obter-quantidade-de-midias-por-questao";
                var resultOrgao = await client.GetAsync(url);

                if (!resultOrgao.IsSuccessStatusCode)
                {
                    throw new Exception(resultOrgao.RequestMessage.Content.ToString());
                }
                db.ExcluirqtdMidia();
                var respOrgao = await resultOrgao.Content.ReadAsStringAsync();
                var qtdMidias = JsonConvert.DeserializeObject<QuantidadeMidia>(respOrgao);
                db.InsertQtdMidia(qtdMidias);
            }
            catch (Exception ex)
            {
                await DisplayAlert("", ex.Message, "OK");
            }
        }
        }
    
}
