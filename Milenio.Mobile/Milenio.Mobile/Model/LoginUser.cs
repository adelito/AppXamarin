using Mobile.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Milenio.Mobile.Model
{
    class LoginUser
    {
        public string login { get; set; }

        public string senha { get; set; }

        public bool success { get; set; }
        public List<string> errors { get; set; }
        public List<object> informations { get; set; }
        public LoginAcesso data { get; set; }


    }
}
