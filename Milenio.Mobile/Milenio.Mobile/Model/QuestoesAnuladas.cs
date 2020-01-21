using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Model
{
    [Table("QuestoesAnuladas")]
    public class QuestoesAnuladas
    {
        //[PrimaryKey]
        public int id { get; set; }
        public int respostaId { get; set; }
        public int questaoVisitaId { get; set; }
    }
}
