using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mobile.Model
{
    public class EnviarMidiaQuestao
    {
        public int id { get; set; }
        public int questaoVisitaId { get; set; }
        public string dataGravacao { get; set; }
        public string legenda { get; set; }

        public List<FileInfo> fileImage { get; set; }
    }
}
