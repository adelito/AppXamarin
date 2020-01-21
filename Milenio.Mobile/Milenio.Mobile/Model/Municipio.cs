using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Mobile.Model
{
    [Table ("Municipio")]
    public class Municipio
    {
        //[PrimaryKey]
        public int Id { get; set; }
        public string NomeMunicipio { get; set; }
    }
}
