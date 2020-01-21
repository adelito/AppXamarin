using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.WebApi
{
    public class EmissorExterno
    {
        public int id { get; set; }
        public string nome { get; set; }

        public Orgao orgao { get; set; }
    }
}
