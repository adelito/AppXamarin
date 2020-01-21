using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Model
{
    [Table("MidiaQuestoes")]
    public class MidiaQuestoes
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int questaoVisitaId { get; set; }
        public DateTime dataGravacao { get; set; }
        public string legenda { get; set; }
        public string caminho { get; set; }
        public bool flagEnvio { get; set; }
    }
}
