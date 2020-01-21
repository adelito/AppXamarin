using System;
using System.Collections.Generic;
using System.Text;
using Mobile.Model;
using Mobile.Helpers;

namespace Mobile.Services
{
    public class MunicipioService : IMunicipio
    {
        DatabaseHelper _databaseHelper;
        public MunicipioService()
        {
            _databaseHelper = new DatabaseHelper();
        }

        public List<Municipio> GetMunicipio()
        {
            return _databaseHelper.GetMunicipio();
        }

        public Municipio GetMunicipioData(string nomemunicipio)
        {
           return _databaseHelper.GetMunicipioData(nomemunicipio);
        }

        public void InsertMunicipio(Municipio municipio)
        {
            _databaseHelper.InsertMunicipio(municipio);
        }
    }
}

