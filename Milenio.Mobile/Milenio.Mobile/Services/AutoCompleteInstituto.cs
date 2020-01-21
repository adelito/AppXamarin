using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Mobile.Model;
using Xamarin.Forms;
using System.Text;


namespace Mobile.Services
{
    public class AutoCompleteInstituto
    {
        InstitutoService institutoservice = new InstitutoService();
        MunicipioService municipioservice = new MunicipioService();
        List<Instituto> _instituto;
        Municipio _municipio = new Municipio();
        public List<string> Instituto { get; set; }
       
        public List<string> ListInstituto(string cidade, int tipoinstituto)
        {
            int idcidade;
            _municipio = municipioservice.GetMunicipioData(cidade);
            idcidade = _municipio.Id;
            Instituto = new List<string>();
            _instituto = institutoservice.GetInstituto(idcidade, tipoinstituto);
            var count = _instituto.Count();
            for (int i = 0; i < count; i++)
            {
                Instituto.Add(_instituto[i].NomeInstituto);
            }
            return Instituto;
        }
    }
}