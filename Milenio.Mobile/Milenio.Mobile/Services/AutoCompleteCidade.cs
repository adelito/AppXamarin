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
    public class AutoCompleteCidade : INotifyPropertyChanged
    {
        MunicipioService service = new MunicipioService();
        List<Municipio> _municipio = new List<Municipio>();
        #region Property

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion

        bool isBusy;

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (SetProperty(ref isBusy, value))
                    IsNotBusy = !isBusy;
            }
        }

        bool isNotBusy = true;

        public bool IsNotBusy
        {
            get => isNotBusy;
            set
            {
                if (SetProperty(ref isNotBusy, value))
                    IsBusy = !isNotBusy;
            }
        }


        public List<string> Cidade { get; }

        private string _pesquisa;
        public string Pesquisa
        {
            get { return _pesquisa; }
            set => SetProperty(ref _pesquisa, value);
        }

        private string _selecionado;
        public string Selecionado
        {
            get { return _selecionado; }
            set => SetProperty(ref _selecionado, value);
        }

        public AutoCompleteCidade()
        {
            Cidade = new List<string>();
            _municipio = service.GetMunicipio();
            for(int i=0; i<_municipio.Count;i++)
            {
                Cidade.Add(_municipio[i].NomeMunicipio);
            }
        }

        public IEnumerable<string> ObterSugestoes(string text)
        {
            return Cidade.Where(x => x.StartsWith(text, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}