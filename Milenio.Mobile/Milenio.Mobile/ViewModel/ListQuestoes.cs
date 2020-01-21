using Mobile.Helpers;
using Mobile.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mobile.ViewModel
{
    class ListQuestoes : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        DatabaseHelper service = new DatabaseHelper();
       
        public ICommand idQuestaoComand { get; private set; }

        public ListQuestoes(int idAspecto)
        {

            List<Questoes> questoes = service.GetQuestoes(idAspecto);
            List<Questoes> _questoes = new List<Questoes>();
            foreach (var item in questoes)
            {
                item.numeracao = item.numeracao + "" + item.descricao;
                _questoes.Add(new Questoes()
                {
                    id = item.id,
                    aspectoId = item.aspectoId,
                    descricao = item.descricao,
                    respostaNegativaId = item.respostaNegativaId,
                    ordem = item.respostaNegativaId,
                    numeracao = item.numeracao,
                    descricaoDevolutiva = item.descricaoDevolutiva,
                    resposta = item.resposta,
                    observacao = item.observacao
                });
            }

        }
        void ListarQuestoes()
        {
            idQuestaoComand = new Command(IdQuestao);
            OnPropertyChanged("Questoes");
        }
        void IdQuestao()
        {

        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
