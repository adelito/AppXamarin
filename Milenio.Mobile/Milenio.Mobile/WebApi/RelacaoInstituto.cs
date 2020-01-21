using System;
using System.Collections.Generic;
using System.Text;

namespace Milenio.Mobile.WebApi
{
    class RelacaoInstituto
    {
        public int Id { get; set; }
        public int CidadeId { get; set; }
        public int TipoInstituicaoId { get; set; }
        public string nome { get; set; }
    }
}
