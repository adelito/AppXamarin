using System;
using System.Collections.Generic;
using System.Text;
using Mobile.Model;
using Mobile.Helpers;


namespace Mobile.Services
{
    public class InstitutoService : IInstituto
    {
        DatabaseHelper _databaseHelper;
        public InstitutoService()
        {
            _databaseHelper = new DatabaseHelper();
        }
        public Instituto GetInstituto(string instituto, int CidadeId)
        {
           return _databaseHelper.GetInstitutoData(instituto, CidadeId);
        }

        public List<Instituto> GetInstituto(int idcidade, int tipoinstituto)
        {
            return _databaseHelper.GetInstituto(idcidade, tipoinstituto);
            
        }

        public void InserirInstituto(List<Instituto> instituto)
        {
            _databaseHelper.InserirInstituto(instituto);
        }

       
    }
}
