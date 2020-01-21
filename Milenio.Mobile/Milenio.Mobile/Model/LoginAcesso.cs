using System;
using System.Collections.Generic;
using System.Text;

namespace Milenio.Mobile.Model
{
    class LoginAcesso
    {
        public int id { get; set; }
        public string nome { get; set; }
        public int orgaoExternoId { get; set; }
        public string token { get; set; }
        public DateTime dataExpiracao { get; set; }
    }
}
