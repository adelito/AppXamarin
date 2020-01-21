using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.WebApi
{

    public  class RelacaoQuestionario
    {
        public int id { get; set; }
        public int instituicaoId { get; set; }
        public DateTime dataAplicacao { get; set; }
        public int? colaboradorExternoId { get; set; }
        public int? colaboradorInternoId { get; set; }
        public int formaColetaId { get; set; }
        public int? revisitaId { get; set; }
        public DateTime dataEnvio { get; set; }
        public bool finalizado { get; set; }
        public EmissorExterno emissorExterno { get; set; }
        public List<AspectoApi> aspectos { get; set; }
    }

}
