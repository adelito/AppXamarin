using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.WebApi
{
    public class RelacaoQuestoes
    {
            public int id { get; set; }
            public int aspectoVisitaId { get; set; }
            public string descricao { get; set; }
            public int respostaNegativaId { get; set; }
            public int ordem { get; set; }
            public string numeracao { get; set; }
            public string descricaoDevolutiva { get; set; }
            public int? respostaId { get; set; }
            public string observacao { get; set; }
    }
}
