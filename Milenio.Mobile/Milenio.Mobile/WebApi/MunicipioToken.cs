using Mobile.Helpers;
using Mobile.Model;
using Mobile.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Milenio.Mobile.WebApi
{
    class MunicipioToken
    {
        public async void ApiMunicipio(string token)
        {
            MunicipioApi municipioapi = new MunicipioApi();
            MunicipioService service = new MunicipioService();
            Municipio municipio = new Municipio();
            string respm = string.Empty;
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var endereco = Settings.EnderecoApi;
                client.BaseAddress = new Uri(endereco);
                var url = "cidade/obter-cidades-da-bahia";
                var resultm = await client.GetAsync(url);

                if (!resultm.IsSuccessStatusCode)
                {
                    throw new Exception(resultm.RequestMessage.Content.ToString());
                }

                respm = await resultm.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
        

                   await DisplayAlert("Erro", ex.Message, "Ok");
            }
            municipioapi = JsonConvert.DeserializeObject<MunicipioApi>(respm);
            /*foreach (var obj in municipioapi.data)
            {
                Console.WriteLine(obj.nome);
            }*/
            for (int i=0; i<municipioapi.data.Count;i++)
            {
                municipio.Id = municipioapi.data[i].id;
                municipio.NomeMunicipio = municipioapi.data[i].nome;
                service.InsertMunicipio(municipio);
            }
            
        }

        private Task DisplayAlert(string v1, string message, string v2)
        {
            throw new NotImplementedException();
        }
       
    }
}
  
