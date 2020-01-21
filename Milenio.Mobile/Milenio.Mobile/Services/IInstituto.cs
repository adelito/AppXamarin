using System;
using System.Collections.Generic;
using System.Text;
using Mobile.Model;

namespace Mobile.Services
{
    public interface IInstituto
    {
        void InserirInstituto(List<Instituto> instituto);
        Instituto GetInstituto(string instituto, int CidadeId);

        List<Instituto> GetInstituto(int idcidade, int tipoinstituto);
    }
}
