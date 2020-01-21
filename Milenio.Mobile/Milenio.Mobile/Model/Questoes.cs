using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Model
{
    [Table("Questoes")]
    public class Questoes
    {
        //[PrimaryKey]
        public int id { get; set; }
        public int aspectoId { get; set; }
        public string descricao { get; set; }
        public int respostaNegativaId { get; set; }
        public int ordem { get; set; }
        public string numeracao { get; set; }
        public string descricaoDevolutiva { get; set; }
        public string resposta { get; set; }
        public string observacao { get; set; }
        public bool perguntaAberta { get; set; }
        public bool respondida { get; set; }
        public string questaoPaiId { get; set; }
        public bool pergunta { get; set; }

        public bool questaoAnulada { get; set; }

        public string respostaAspecto{ get; set; }
    }
}
