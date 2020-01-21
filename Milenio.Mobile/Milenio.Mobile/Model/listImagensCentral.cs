using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Model
{
    public class listImagensCentral
    {
        public bool success { get; set; }
        public List<object> informations { get; set; }
        public List<uploadImageCentral> data { get; set; }
    }
}
