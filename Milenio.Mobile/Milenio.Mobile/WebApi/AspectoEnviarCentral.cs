using Mobile.WebApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.WebApi
{
    public class AspectoEnviarCentral
    {
        public int id { get; set; }
        public string descricao { get; set; }
        public int visitaId { get; set; }
        public object dataFinalizacao { get; set; }
        public object usuarioFinalizacaoId { get; set; }
        public int formaColetaId { get; set; }
        public string observacao { get; set; }
        public List<MidiaAspectoApi> midias { get; set; }
        public List<QuestoesEnviarCentral> questoes { get; set; }
    }
}
