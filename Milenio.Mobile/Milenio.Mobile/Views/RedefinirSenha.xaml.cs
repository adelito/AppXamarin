using Milenio.Mobile.WebApi;
using Mobile.Helpers;
using Mobile.Model;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
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
    public partial class RedefinirSenha : ContentPage
    {
       
        public RedefinirSenha()
        {
            InitializeComponent();
            lbUsuario.Text = "Usuário: " + Settings.Nome;
          //  this.BindingContext = this;
          //  this.IsBusy = false;


        }
        
        private async void BtRedefinirSenha_Clicked(object sender, EventArgs e)
        {
            //string usuario = Settings.Login.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
            string usuario = Settings.Login;
            btRedefinirSenha.IsEnabled = false;
            var token = Settings.Token;
            DatabaseHelper ds = new DatabaseHelper();
            Usuario _usuario = new Usuario();
            if (CrossConnectivity.Current.IsConnected)
            {
                if (string.IsNullOrEmpty(txtSenhaAtual.Text))
                {
                    await DisplayAlert("Campo Obrigatório", "Senha Atual", "Ok");
                    txtSenhaAtual.Focus();
                }
                else if (string.IsNullOrEmpty(txtNovaSenha.Text))
                {
                    await DisplayAlert("Campo Obrigatório", "Nova Senha", "Ok");
                    txtNovaSenha.Focus();
                }
                else if (string.IsNullOrEmpty(txtConfirmaNovaSenha.Text))
                {
                    await DisplayAlert("Campo Obrigatório", "Confirmação de Senha", "Ok");
                    txtConfirmaNovaSenha.Focus();
                }
                else if (txtNovaSenha.Text != txtConfirmaNovaSenha.Text)
                {
                    await DisplayAlert("", "Senhas informadas não conferem.", "Ok");
                    txtNovaSenha.Focus();
                }
                else if (txtNovaSenha.Text.Length < 6)
                {
                    await DisplayAlert("", "A nova senha deve ter no mínimo 6 caracteres.", "Ok");
                    txtNovaSenha.Focus();
                }
                else
                {
                    string resp = string.Empty;
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var loginuser = new NovaSenhaUsuario
                    {
                        login = usuario,
                        senhaAtual = txtSenhaAtual.Text,
                        novaSenha = txtNovaSenha.Text

                    };
                    try
                    {
                        string jsonRequest = JsonConvert.SerializeObject(loginuser);
                        StringContent httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application / json");
                        client.BaseAddress = new Uri("http://milenio-app.tst.sistemas.intranet.mpba.mp.br/api/");
                        var url = string.Format("usuario/alterar-senha");
                        var result = await client.PutAsync(url, httpContent);

                        if (!result.IsSuccessStatusCode)
                        {
                            throw new Exception(result.RequestMessage.Content.ToString());
                        }
                        resp = await result.Content.ReadAsStringAsync();
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("", ex.Message, "Ok");
                    }
                    NovaSenhaUsuario user = JsonConvert.DeserializeObject<NovaSenhaUsuario>(resp);
                    if (user.success)
                    {
                        _usuario.login = usuario;
                        _usuario.senha = txtNovaSenha.Text;
                        ds.UpdateUsuario(_usuario);
                        await DisplayAlert("", "Operação efetuada com sucesso.", "Ok");
                        await Navigation.PushAsync(new MainPage(), true);
                    }
                    else
                    {
                        await DisplayAlert("", "Senha atual não confere.", "Ok");
                    }
                }
            }
            else
            {
                await DisplayAlert("", "Verifique sua conexão com a internet.", "OK");
            }
            btRedefinirSenha.IsEnabled = true;
        }
      
    }
}