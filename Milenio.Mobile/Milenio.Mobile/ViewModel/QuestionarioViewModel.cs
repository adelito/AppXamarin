using Mobile.Helpers;
using Mobile.Model;
using Mobile.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace Mobile.ViewModel
{
    public class QuestionarioViewModel : INotifyPropertyChanged
    {
    
      
        DatabaseHelper service = new DatabaseHelper();
        List<QuestionarioVisita> questionarioVisita = new List<QuestionarioVisita>();
        QuestionarioVisita questionario = new QuestionarioVisita();
        Instituto instituto = new Instituto();
        Municipio municipio = new Municipio();
        string protocolo;
        string day;
        string month;
        string year;
        string datavisita;

        public QuestionarioViewModel()
        {
            
            GetQuestionarioBaixado = new ObservableCollection<QuestionarioBaixadoModel>();
            
            FillData();
            ExcluirQuestionario = new Command(async (model) =>
            {
                DatabaseHelper service = new DatabaseHelper();
                try
                {
                    bool answer = await Application.Current.MainPage.DisplayAlert("", "Deseja realmente excluir o(s) registro(s) selecionado(s)?", "Cancelar", "OK");
                    if (!answer)
                    {
                        QuestionarioBaixadoModel questionarioexcluir = (QuestionarioBaixadoModel)model;
                        GetQuestionarioBaixado.Remove(questionarioexcluir);
                        var protocolo = Convert.ToInt32(questionarioexcluir.Protocolo.TrimStart('0'));
                        var idAspecto = service.GetAspectoQuestionarioID(protocolo);
                        foreach (var item in idAspecto)
                        {
                            var questoes = service.GetQuestoes(item.id);
                            foreach (var itemQuestao in questoes)
                            {
                                service.ExcluirQuestoesanuladas(itemQuestao.id);
                                
                            }
                            service.ExcluirQuesteos(item.id);

                        }
                        service.ExcluirAspectos(protocolo);
                        service.ExcluirQuestiorario(protocolo);
                        await Application.Current.MainPage.DisplayAlert("", "Operação efetuada com sucesso.", "OK");
                        await Application.Current.MainPage.Navigation.PushAsync(new BaixarQuestionario());
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });
        }
        public IList<QuestionarioBaixadoModel> GetQuestionarioBaixado { get; set; }
        public void FillData()
        {
            questionarioVisita = service.GetQuestionarioVisitaPreencherQuestionario();
            foreach (var item in questionarioVisita)
            {
                questionario.id = item.id;
                questionario.instituicaoId = item.instituicaoId;
                questionario.dataAplicacao = item.dataAplicacao;
                instituto = service.GetInstitutoIntQuestionarioBaixados(questionario.instituicaoId);
                municipio = service.GetMunicipioIntQuestionarioBaixados(instituto.CidadeId);
                protocolo = questionario.id.ToString().PadLeft(6, '0');
                day = questionario.dataAplicacao.Day.ToString();
                month = questionario.dataAplicacao.Month.ToString();
                year = questionario.dataAplicacao.Year.ToString();
                datavisita = day + "/" + month + "/" + year;
                GetQuestionarioBaixado.Add(new QuestionarioBaixadoModel()
                {
                    Protocolo = protocolo,
                    DataVisita = questionario.dataAplicacao.ToString("dd/MM/yyyy"),
                    //DataVisita = datavisita,
                    Cidade = municipio.NomeMunicipio,
                    Instituicao = instituto.NomeInstituto
                });
            }

        }
        /*public Command<QuestionarioBaixadoModel> RemoveCommand
        {
            get {
                return new Command<QuestionarioBaixadoModel>((questionariolist) => {
                    GetQuestionarioBaixado.Remove(questionariolist);
                });

            }
            set
            {
                OnPropertyChanged();
            }
        }*/
        public Command ExcluirQuestionario { get; set; }
        

        public Command PreencherQuestionario { get; set; } = new Command(async (model) =>
        {
            try
            {
                QuestionarioBaixadoModel questionariopreencher = (QuestionarioBaixadoModel)model;
                await Application.Current.MainPage.Navigation.PushAsync(new PreencherQuestionarioAspecto(questionariopreencher));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        });

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        #endregion
    }
}
