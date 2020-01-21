using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Model
{
    class AddQuestionario
    {
        public int id { get; set; }
        public int instituicaoId { get; set; }
        public string dataAplicacao { get; set; }
        public int colaboradorExternoId { get; set; }
        public int colaboradorInternoId { get; set; }
        public int formaColetaId { get; set; }
    }
}
