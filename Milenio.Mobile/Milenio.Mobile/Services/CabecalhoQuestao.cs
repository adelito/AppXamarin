using Mobile.Helpers;
using Mobile.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Services
{
    class CabecalhoQuestao
    {
        int Protocolo;
        string DataVisita, Cidade, Instituicao;
        Cabecalho cabecalho = new Cabecalho();
        DatabaseHelper service = new DatabaseHelper();
        public Cabecalho GetCabecalho(int id)
        {
            cabecalho = service.Montarcabecalho(id);
            return cabecalho;
        }
    }
}
