using System;
using System.Collections.Generic;
using System.Text;

namespace Milenio.Mobile.WebApi
{
    class NovaSenhaUsuario
    {
        public Boolean success { get; set; }
        public string login { get; set; }
        public string senhaAtual { get; set; }

        public string novaSenha { get; set; }

        public string DSNome { get; set; }
    }
}
