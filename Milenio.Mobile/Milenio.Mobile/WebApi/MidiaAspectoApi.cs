using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.WebApi
{
    public class MidiaAspectoApi
    {
        public int id { get; set; }
        public int aspectoVisitaId { get; set; }
        public DateTime dataGravacao { get; set; }
        public string legenda { get; set; }
        public string caminho { get; set; }
    }
}
