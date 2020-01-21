using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milenio.Mobile.Validacao;
using Mobile;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mobile.WebApi;
using System.Net.Http;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Mobile.Helpers;

namespace Milenio.Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EsqueciSenha : ContentPage
	{
        Validacao.Validacao validacaocpf = new Validacao.Validacao();
        public EsqueciSenha()
        {
            InitializeComponent();
            this.BindingContext = this;
            this.IsBusy = false;
            txtUsuario.Completed += TxtUsuario_CompletedAsync;

        }

        private async void TxtUsuario_CompletedAsync(object sender, EventArgs e)
        {
            string user = txtUsuario.Text;
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
            else
            {
                txtUsuario.Text = user;
            }
        }

        private async void BtConfirmarEsqueciSenha_Clicked(object sender, EventArgs e)
        {
            btConfirmarEsqueciSenha.IsEnabled = false;
            if (CrossConnectivity.Current.IsConnected)
            {
                if (string.IsNullOrEmpty(txtUsuario.Text))
                {
                    await DisplayAlert("Campo Obrigatório", "Usuário", "Ok");
                    txtUsuario.Focus();

                }
                else
                {
                    string usuario = txtUsuario.Text;
                  
                    bool valido = usuario.Length == 11 && usuario.All(char.IsDigit);
                    if (valido)
                    {
                        usuario = usuario.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
                    }
                    string resp = string.Empty;
                    HttpClient client = new HttpClient();
                    var loginuser = new UsuarioEsqueciSenha
                    {
                        login = usuario,

                    };
                    try
                    {
                        string jsonRequest = JsonConvert.SerializeObject(loginuser);
                        StringContent httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application / json");
                        var endereco = Settings.EnderecoApi;
                        //client.BaseAddress = new Uri(endereco);
                        client.BaseAddress = new Uri("http://milenio-app.tst.sistemas.intranet.mpba.mp.br/api/");
                        var url = string.Format("usuario/redefinir-senha");
                        var result = await client.PutAsync(url, httpContent);

                        if (!result.IsSuccessStatusCode)
                        {
                            throw new Exception(result.RequestMessage.Content.ToString());
                        }

                        resp = await result.Content.ReadAsStringAsync();


                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Erro", ex.Message, "Ok");
                    }
                    UsuarioEsqueciSenha user = JsonConvert.DeserializeObject<UsuarioEsqueciSenha>(resp);
                    if (user.success)
                    {
                        await DisplayAlert("Redefinir Senha", user.informations[0].ToString(), "Ok");
                        await Navigation.PushAsync(new MainPage(), true);
                    }
                    else
                    {
                        await DisplayAlert("Redefinir Senha", "Usuário não encontrado" , "Ok");
                    }    
                }
            }
            else
            {
                await DisplayAlert("Internet", "Verifique sua conexão com a internet.", "OK");
            }
            btConfirmarEsqueciSenha.IsEnabled = true; ;
        }

        private async void BtCancelar_Clicked(object sender, EventArgs e)
        {
            btCancelar.IsEnabled = false;
            await Navigation.PushAsync(new MainPage(), true);
            btCancelar.IsEnabled = true;
        }

        private async void TxtUsuario_TextChanged(object sender, TextChangedEventArgs e)
        {
            string user = txtUsuario.Text;
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
            else
            {
                txtUsuario.Text = user;
            }
        }
    }
}