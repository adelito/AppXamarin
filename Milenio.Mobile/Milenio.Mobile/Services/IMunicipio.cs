using System.Collections.Generic;
using Mobile.Model;

namespace Mobile.Services
{
    public interface IMunicipio
    {
        void InsertMunicipio(Municipio municipio);
        Municipio GetMunicipioData(string nomemunicipio);

        List<Municipio> GetMunicipio();
    }
}
