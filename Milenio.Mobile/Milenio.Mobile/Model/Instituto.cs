using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Mobile.Model
{
    [Table("Instituto")]
    public class Instituto
    {
        //[PrimaryKey]
        
        public int CidadeId { get; set; }
        public int Id { get; set; }
        public int TipoInstituicaoId { get; set; }
        public string NomeInstituto { get; set; }

    }
}
