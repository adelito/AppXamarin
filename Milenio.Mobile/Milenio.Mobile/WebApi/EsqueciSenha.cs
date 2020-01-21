using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Milenio.Mobile.WebApi
{
    public class EsqueciSenha
    {
        /*
        public async void GerarNovaSenhaAsync(string usuario)
        {
            
            if (CrossConnectivity.Current.IsConnected)
            {


                string resp = string.Empty;
                try
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri("http://10.130.155.44");
                    var url = string.Format("/milenioapi/api/visita/obter-visita/{0}", usuario);
                    var result = await client.PutAsync(url);

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
   
            }
            else
            {

            }
        }

        private Task DisplayAlert(string v1, string message, string v2)
        {
            throw new NotImplementedException();
        }
        */
    }
}
