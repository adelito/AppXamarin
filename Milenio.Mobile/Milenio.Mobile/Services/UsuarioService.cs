using System;
using System.Collections.Generic;
using System.Text;
using Mobile.Model;
using Mobile.Helpers;

namespace Mobile.Services
{
    public class UsuarioService : IUsuario
    {
        DatabaseHelper _databaseHelper;
        public UsuarioService()
        {
            _databaseHelper = new DatabaseHelper();
        }
        public Usuario GetUsuarioData(string login, string senha)
        {
           return  _databaseHelper.GetUsuarioData(login, senha);
        }
        public Usuario GetUsuarioDataLogin(string login)
        {
            return _databaseHelper.GetUsuarioDataLogin(login);
        }
        public void InsertUsuario(Usuario usuario)
        {
            _databaseHelper.InsertUsuario(usuario);
        }

        public void UpdateUsuario(Usuario usuario)
        {
            throw new NotImplementedException();
        }
    }
}
