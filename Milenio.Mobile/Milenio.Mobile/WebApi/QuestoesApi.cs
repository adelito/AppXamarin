using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.WebApi
{
    public class QuestoesApi
    {
        public int id { get; set; }
        public int aspectoId { get; set; }
        public string descricao { get; set; }
        public int respostaNegativaId { get; set; }
        public int ordem { get; set; }
        public string numeracao { get; set; }
        public string descricaoDevolutiva { get; set; }
        public string respostaId { get; set; }
        public string observacao { get; set; }
        public bool perguntaAberta { get; set; }
        public string questaoPaiId { get; set; }
        public bool pergunta { get; set; }
        public List<MidiaQestoesApi> midias { get; set; }
        public List<questoesAnuladasAPI> questoesAnuladas { get; set; }
    }
}
