using Milenio.Mobile.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace Mobile.Helpers
{
    public class RenovarToken
    {
        DatabaseHelper db = new DatabaseHelper();
        public async System.Threading.Tasks.Task GerarNovoTokenAsync(string Login)
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
            var endereco = Settings.EnderecoApi;
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
    }
}
