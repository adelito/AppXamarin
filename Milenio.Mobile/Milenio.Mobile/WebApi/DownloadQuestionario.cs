using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.WebApi
{
    class DownloadQuestionario
    {
        VisitaToken visitaToken = new VisitaToken();
        RevisitaToken revisitaToken = new RevisitaToken();
        private int institutoId;
        private string token;
        private string dataVisita;
        private int tipo;

        public async void BaixarQuestionario(string idInstituto, string token, string dataVisita, int tipo)
        {
            QuestionarioApi questionarioApi = new QuestionarioApi();
            this.institutoId = Convert.ToInt32(idInstituto);
            this.token = token;
            this.dataVisita = dataVisita;
            this.tipo = tipo;
                switch (tipo)
                {
                    case 1:
                    
                        //await visitaToken.ConsumirVisita(token, institutoId, dataVisita);
                        break;
                    case 2:
                       // revisitaToken.ConsumirRevisita(token, institutoId, dataVisita);

                        break;
                    default:
                        break;
                }
            
        }

        private Task DisplayAlert(string v1, string message, string v2)
        {
            throw new NotImplementedException();
        }
    }
}
