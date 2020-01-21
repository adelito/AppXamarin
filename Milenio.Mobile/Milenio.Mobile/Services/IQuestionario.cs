using System;
using System.Collections.Generic;
using System.Text;
using Mobile.Model;

namespace Milenio.Mobile.Services
{
    public interface IQuestionario
    {
        void InserirQuestionario(QuestionarioVisita questionarioVisita);

        List<QuestionarioVisita> GetQuestionarioVisita(int institutoId, DateTime dataVisita);

        QuestionarioVisita GetQuestionarioVisitainstitutoId(int institutoId);
        void InserirAspecto(AspectoQuestionario aspectoQuestionario);

        IEnumerable<AspectoQuestionario> GetAspectoQuestionario(int visitaId);

        void InsertMidiasAspectos(MidiaAspecto midiaAspecto);

        IEnumerable<MidiaAspecto> GetMidiaAspectos(int aspectoVisitaId);

        void InsertQuestoes(Questoes questoes);

        IEnumerable<Questoes> GetQuestoes(int aspectoVisitaId);

        void InsertMidiaQuestoes(MidiaQuestoes midiaQuestoes);

        IEnumerable<MidiaQuestoes> GetMiGetMidiaQuestoesdiaQuestoes(int questaoVisitaId);


    }
}
