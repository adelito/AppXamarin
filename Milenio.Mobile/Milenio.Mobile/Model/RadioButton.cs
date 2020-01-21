using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Model
{
    public class RadioButton
    {
        public int id { get; set; }
        public string Name { get; set; }

        public override string ToString() => Name;
    }
}
