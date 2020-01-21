using Mobile.Helpers;
using Mobile.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Milenio.Mobile.Repositorio
{
    class PreencherQuestionarioVisitaRepositorio
    {
        DatabaseHelper service = new DatabaseHelper();
        List<QuestionarioVisita> questionarioVisita = new List<QuestionarioVisita>();
        QuestionarioVisita questionario = new QuestionarioVisita();
        Instituto instituto = new Instituto();
        Municipio municipio = new Municipio();
        string protocolo;
        string day;
        string month;
        string year;
        string datavisita; 

        public IList<QuestionarioBaixadoModel> GetQuestionarioBaixado { get; private set; }

        public PreencherQuestionarioVisitaRepositorio()
        {
            
            questionarioVisita = service.GetQuestionarioVisitaPreencherQuestionario();
            GetQuestionarioBaixado = new List<QuestionarioBaixadoModel>();


            foreach (var item in questionarioVisita)
            {
                questionario.id = item.id;
                questionario.instituicaoId = item.instituicaoId;
                questionario.dataAplicacao = item.dataAplicacao;
                instituto = service.GetInstitutoIntQuestionarioBaixados(questionario.instituicaoId);
                municipio = service.GetMunicipioIntQuestionarioBaixados(instituto.CidadeId);
                protocolo = questionario.id.ToString().PadLeft(6, '0');
                day = questionario.dataAplicacao.Day.ToString();
                month = questionario.dataAplicacao.Month.ToString();
                year = questionario.dataAplicacao.Year.ToString();
                datavisita = day + "/" + month + "/" + year;
                GetQuestionarioBaixado.Add(new QuestionarioBaixadoModel()
                {
                    Protocolo = protocolo,
                    DataVisita = questionario.dataAplicacao.ToString("dd/MM/yyyy"),
                    //DataVisita = datavisita,
                    Cidade = municipio.NomeMunicipio,
                    Instituicao = instituto.NomeInstituto
                });
            }
        }
    }
}
