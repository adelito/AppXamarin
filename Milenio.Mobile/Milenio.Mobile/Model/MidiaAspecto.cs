using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Model
{
    [Table("MidiaAspecto")]
    public class MidiaAspecto
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int aspectoVisitaId { get; set; }
        public DateTime dataGravacao { get; set; }
        public string legenda { get; set; }
        public string caminho { get; set; }
    }
}
