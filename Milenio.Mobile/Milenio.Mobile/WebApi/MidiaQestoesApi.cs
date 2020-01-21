using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.WebApi
{
    public class MidiaQestoesApi
    {
        public int id { get; set; }
        public int questaoVisitaId { get; set; }
        public DateTime dataGravacao { get; set; }
        public string legenda { get; set; }
        public string caminho { get; set; }
    }
}
