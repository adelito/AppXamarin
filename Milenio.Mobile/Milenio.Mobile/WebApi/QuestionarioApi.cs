using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.WebApi
{
    class QuestionarioApi
    {
        public bool success { get; set; }
        
        public List<object> informations { get; set; }
        public RelacaoQuestionario data { get; set; }
    }
}
