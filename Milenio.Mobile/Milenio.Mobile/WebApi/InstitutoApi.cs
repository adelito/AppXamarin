using System;
using System.Collections.Generic;
using System.Text;

namespace Milenio.Mobile.WebApi
{
    class InstitutoApi
    {
        public bool success { get; set; }
        public List<object> informations { get; set; }
        public List<RelacaoInstituto> data { get; set; }
    }
}
