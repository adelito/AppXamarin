using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Model
{
    [Table("AspectQuestionario")]
    public class AspectoQuestionario
    {
        //[PrimaryKey]
        public int id { get; set; }
        public string descricao { get; set; }
        public int visitaId { get; set; }
        public DateTime dataFinalizacao { get; set; }
        public int usuarioFinalizacaoId { get; set; }
        public int formaColetaId { get; set; }
        public string observacao { get; set; }
    }
}
