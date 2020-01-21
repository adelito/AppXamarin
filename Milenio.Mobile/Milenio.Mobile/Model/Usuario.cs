using SQLite;

namespace Mobile.Model
{

    [Table("Usuario")]
    public class Usuario
    {    
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string login { get; set; }
        public string senha { get; set; }
        public string DsNome { get; set; }

        public int orgaoExternoId { get; set; }
    }
}
