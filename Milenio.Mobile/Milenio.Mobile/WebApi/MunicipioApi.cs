using System;
using System.Collections.Generic;
using System.Text;

namespace Milenio.Mobile.WebApi
{
    class MunicipioApi
    {
        public bool success { get; set; }
        public List<object> informations { get; set; }
        public List<RelacaoMunicipio> data { get; set; }
    }
}
