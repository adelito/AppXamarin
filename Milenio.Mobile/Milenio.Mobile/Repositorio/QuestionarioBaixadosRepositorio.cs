using System;
using System.Collections.Generic;
using System.Text;
using Mobile.Helpers;
using Mobile.Model;

namespace Mobile.Repositorio
{
    class QuestionarioBaixadosRepositorio
    {
        DatabaseHelper service = new DatabaseHelper();
        List<QuestionarioVisita> questionarioVisita = new List<QuestionarioVisita>();
        QuestionarioVisita questionario = new QuestionarioVisita();
        Instituto instituto = new Instituto();
        Municipio municipio = new Municipio();

        public IList<QuestionarioBaixadoModel> GetQuestionarioBaixado { get; private set; }

        public QuestionarioBaixadosRepositorio(string institutoId, DateTime dataVisita)
        {
            int idInstituto = Convert.ToInt32(institutoId);
            questionarioVisita = service.GetQuestionarioVisita(idInstituto, dataVisita);
            foreach (var item in questionarioVisita)
            {
                questionario.id = item.id;
                questionario.instituicaoId = item.instituicaoId;
                questionario.dataAplicacao = item.dataAplicacao;
            }
            instituto = service.GetInstitutoIntQuestionarioBaixados(questionario.instituicaoId);
            municipio = service.GetMunicipioIntQuestionarioBaixados(instituto.CidadeId);
            var protocolo = questionario.id.ToString().PadLeft(6, '0');
            string day = questionario.dataAplicacao.Day.ToString();
            string month = questionario.dataAplicacao.Month.ToString();
            string year =  questionario.dataAplicacao.Year.ToString();
            string datavisita = day + "/" + month + "/" + year;
            GetQuestionarioBaixado = new List<QuestionarioBaixadoModel> {
                new QuestionarioBaixadoModel
                {
                    Protocolo = protocolo,
                    DataVisita = datavisita,
                    Cidade = municipio.NomeMunicipio,
                    Instituicao = instituto.NomeInstituto
                }
            };  
        }
            
    }
}
