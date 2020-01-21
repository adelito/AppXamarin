using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Model
{
    public class uploadImageCentral
    {
        public int id { get; set; }
        public int questaoVisitaId { get; set; }
        public DateTime dataGravacao { get; set; }
        public string legenda { get; set; }
        public string caminho { get; set; }
        public object file { get; set; }
        public string base64 { get; set; }
    }
}
