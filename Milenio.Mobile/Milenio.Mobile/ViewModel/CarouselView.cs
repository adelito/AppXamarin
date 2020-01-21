using Mobile.Helpers;
using Mobile.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xamarin.Forms;

namespace Mobile.ViewModel
{
    public class CarouselViewModel : INotifyPropertyChanged
    {
        int count = 0;
        public event PropertyChangedEventHandler PropertyChanged;
        DatabaseHelper service = new DatabaseHelper();
        static int idquestao;
        public CarouselViewModel(int _idquestao)
        {
            idquestao = _idquestao;
            Imagens();
            //Views = new ObservableCollection<View>()
            //{
               // _img
            //};
            PositionCommand = new Command(() =>
            {
                Debug.WriteLine("Posição selecionada.");
            });
            TotalImagens = new Command(() =>
            {
                Debug.WriteLine(count);
            });
        }

        //ObservableCollection<View> _views;
        ObservableCollection<View> _views = new ObservableCollection<View>();
        public void Imagens()
        {
          
            var midia = service.GetMidiaQuestoes(idquestao);
            foreach (var item in midia)
            { 
                var _view = new Image();
                _view.Source = item.caminho;
                
                if (count < 5)
                {
                    Views.Add(_view);
                }
                count++;
            }            
        }
        public ObservableCollection<View> Views
        {
            set
            {
                _views = value;
                OnPropertyChanged("Views");
            }
            get
            {
                return _views;
            }
        }
        public Command PositionCommand { protected set; get; }
        public Command TotalImagens { protected set; get; }
        public Command ButtonClickExcluirMidia { get; set; } = new Command(async (model) =>
        {
          
        });

        public Command ButtonClickExcluirMidiaCentral { get; set; } = new Command(async (model) =>
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Token);
            var endereco = Settings.EnderecoApi;
            client.BaseAddress = new Uri(endereco);
            var url = string.Format("visita/obter-midias-questao/{0}", idquestao);
            var resultVisita = await client.GetAsync(url);
            var respVisita = await resultVisita.Content.ReadAsStringAsync();
            var resposta = JsonConvert.DeserializeObject<listImagensCentral>(respVisita);
        });

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
