using System;
using System.Collections.Generic;
using System.Text;
using Mobile.Model;
using Mobile.Helpers;

namespace Milenio.Mobile.Services
{
    public class QuestionarioService : IQuestionario
    {
        DatabaseHelper _databaseHelper;
        public QuestionarioService()
        {
            _databaseHelper = new DatabaseHelper();
        }

        public void InserirQuestionario(QuestionarioVisita questionarioVisita)
        {
            _databaseHelper.InserirQuestionario(questionarioVisita);
        }

        public List<QuestionarioVisita> GetQuestionarioVisita(int institutoId, DateTime dataVisita)
        {
            return _databaseHelper.GetQuestionarioVisita(institutoId, dataVisita);
        }

        public QuestionarioVisita GetQuestionarioVisitainstitutoId(int institutoId)
        {
            return _databaseHelper.GetQuestionarioVisitainstitutoId(institutoId);
        }

        public void InserirAspecto(AspectoQuestionario aspectoQuestionario)
        {
            _databaseHelper.InserirAspecto(aspectoQuestionario);
        }

        public IEnumerable<AspectoQuestionario> GetAspectoQuestionario(int visitaId)
        {
            return _databaseHelper.GetAspectoQuestionario(visitaId);
        }

        public void InsertMidiasAspectos(MidiaAspecto midiaAspecto)
        {
            _databaseHelper.InsertMidiasAspectos(midiaAspecto);
        }

        public IEnumerable<MidiaAspecto> GetMidiaAspectos(int aspectoVisitaId)
        {
            return _databaseHelper.GetMidiaAspecto(aspectoVisitaId);
        }
        
        public void InsertQuestoes(Questoes questoes)
        {
            _databaseHelper.InsertQuestoes(questoes);
        }

        public IEnumerable<Questoes> GetQuestoes(int aspectoVisitaId)
        {
            return _databaseHelper.GetQuestoes(aspectoVisitaId);
        }

        public void InsertMidiaQuestoes(MidiaQuestoes midiaQuestoes)
        {
            _databaseHelper.InsertMidiaQuestoes(midiaQuestoes);
        }

        public IEnumerable<MidiaQuestoes> GetMiGetMidiaQuestoesdiaQuestoes(int questaoVisitaId)
        {
            return _databaseHelper.GetMidiaQuestoes(questaoVisitaId);
        }


    }
}
