using Milenio.Mobile.Repositorio;
using Mobile.Helpers;
using Mobile.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace Milenio.Mobile.ViewModel
{
    class PreencherQuestionarioVisitaViewModel : INotifyPropertyChanged
    {
        public IList<QuestionarioBaixadoModel> Items { get; private set; }

        public PreencherQuestionarioVisitaViewModel()
        {
            var repo = new PreencherQuestionarioVisitaRepositorio();
           
            Items = repo.GetQuestionarioBaixado.OrderBy(q => q.DataVisita).ToList();
            OnPropertyChanged();

        }
        public Command ButtonClickCommandRespostaNA { get; set; } = new Command(async (model) =>
        {
            try
            {
                Questoes questoesResultado = new Questoes();
                DatabaseHelper db = new DatabaseHelper();
                QuestionarioModelClass questoes = (QuestionarioModelClass)model;
               
               

            }
            catch (Exception ex)
            {
                throw ex;
            }
        });

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        #endregion
    }
}
