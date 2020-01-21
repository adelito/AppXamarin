using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Model
{
    [Table("QuestionarioVisita")]
    public class QuestionarioVisita
    {
        //[PrimaryKey]
        public int id { get; set; }
        public int instituicaoId { get; set; }
        public DateTime dataAplicacao { get; set; }
        public int colaboradorExternoId { get; set; }
        public int colaboradorInternoId { get; set; }
        public int formaColetaId { get; set; }
        public int revisitaId { get; set; }
        public DateTime dataEnvio { get; set; }
        public bool finalizado { get; set; }

        public string emissorExternoId { get; set; }
    }
}
