using Mobile.Model;
using Mobile.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Milenio.Mobile.ViewModel
{
    class QuestionarioBaixadoViewModel
    {
        public IList<QuestionarioBaixadoModel> Items { get; private set; }
       
        public QuestionarioBaixadoViewModel(string idInstituto, DateTime dataVisita)
        {
            var repo = new QuestionarioBaixadosRepositorio(idInstituto, dataVisita);
            Items = repo.GetQuestionarioBaixado.OrderBy(q => q.DataVisita).ToList();
           
        }
    }
}
