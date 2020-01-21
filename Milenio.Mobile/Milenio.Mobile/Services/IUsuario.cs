
using System.Collections.Generic;

using Mobile.Model;


namespace Mobile.Services
{
   public interface IUsuario
    {
        void InsertUsuario(Usuario usuario);

        void UpdateUsuario(Usuario usuario);

        Usuario GetUsuarioData(string login, string senha);

        Usuario GetUsuarioDataLogin(string login);

    }
}
