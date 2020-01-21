using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Model
{
    public class Cabecalho
    {
        public int protocolo { get; set; }
        public DateTime DataVisita { get; set; }
        public string Cidade { get; set; }
        public string Instituicao { get; set; }
        public string Aspecto { get; set; }
    }
}
