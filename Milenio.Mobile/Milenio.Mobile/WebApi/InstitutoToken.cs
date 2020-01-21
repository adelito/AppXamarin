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
    class InstitutoToken
    {
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
                client.BaseAddress = new Uri("http://milenio-app.tst.sistemas.intranet.mpba.mp.br/api/");
                var url = "instituicao/obter-instituicoes";
                var resultOrgao = await client.GetAsync(url);

                if (!resultOrgao.IsSuccessStatusCode)
                {
                    throw new Exception(resultOrgao.RequestMessage.Content.ToString());
                }

                respOrgao = await resultOrgao.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {

                await DisplayAlert("Erro", ex.Message, "Ok");
            }
            _Institutos = JsonConvert.DeserializeObject<InstitutoApi>(respOrgao);
            for (int i = 0; i < _Institutos.data.Count; i++)
            {
                instituto.Id = _Institutos.data[i].Id;
                instituto.CidadeId = _Institutos.data[i].CidadeId;
                instituto.TipoInstituicaoId = _Institutos.data[i].TipoInstituicaoId;
                instituto.NomeInstituto = _Institutos.data[i].nome;
                listInstituto.Add(instituto);

            }
            service.InserirInstituto(listInstituto);
        }

        private Task DisplayAlert(string v1, string message, string v2)
        {
            throw new NotImplementedException();
        }
    }
}
