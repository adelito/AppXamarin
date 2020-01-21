using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
//using PropertyChanged;
namespace Mobile.Model
{
   
    public class QuestionarioModelClass : BaseDomain
    {
        public int id { get; set; }
        public string numeracao { get; set; }
        public string descricao { get; set; }
        public string questao { get; set; }
        public int resposta { get; set; }
       
        public int Resposta
        {
            get { return resposta; }
            set { resposta = value; OnPropertyChanged(nameof(Resposta)); }
        }
        
        public string observacao { get; set; }
        public string Observacao
        {
            get { return observacao; }
            set { observacao = value; OnPropertyChanged(nameof(Observacao)); }
        }
        public bool perguntaAberta { get; set; }
        //public string questaoPaiId { get; set; }
        public bool pergunta { get; set; }
        public bool questaoAnulada { get; set; }
        public string respostaAspecto { get; set; }
        public bool habilitado { get; set; }
        public bool radioButtonResposta { get; set; }
        public bool textObervacao { get; set; }

        public string tipoPergunta { get; set; }
        public int IdAspecto { get; set; }

        public bool quantidadeMidia { get; set; }

    }
}
