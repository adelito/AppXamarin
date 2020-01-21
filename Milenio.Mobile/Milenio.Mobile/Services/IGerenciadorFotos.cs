using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Milenio.Mobile.Services
{
    public interface IGerenciadorFotos
    {
        Task<Stream> ObterFoto();
        Task MidiaAsync();
    }
}
