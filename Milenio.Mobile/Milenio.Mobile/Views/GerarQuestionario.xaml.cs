using Acr.UserDialogs;
using dotMorten.Xamarin.Forms;
using Mobile.Helpers;
using Mobile.Model;
using Mobile.Services;
using Mobile.WebApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GerarQuestionario : ContentPage
    {
        List<string> estados = new List<string>();
        public AutoCompleteCidade autoCompleteCidade => (AutoCompleteCidade)BindingContext;
        public AutoCompleteInstituto autoCompleteInstituto = new AutoCompleteInstituto();
        InstitutoService institutoService = new InstitutoService();
        DatabaseHelper databaseHelper = new DatabaseHelper();
        string nomeCidade;
        int tipoinstituto;
        string nomeinstituto = null;
        string dataVisita;
        DateTime _dataConsultaVisita;
        int op, countMunicipio = 0;
        public GerarQuestionario()
        {

            InitializeComponent();
            listaEstados.IsVisible = false;
            this.BindingContext = new AutoCompleteCidade();
            btConfirmar.IsEnabled = false;
            SearchConteudo.IsEnabled = false;
            pckTipoInstituicao.IsEnabled = false;
            this.IsBusy = false;

        }

        // Verificando conexão com internet

        // AUTOCOMPLETE CIDADE
        //Evento de AutoComplete Cidade
        private void AutoSuggestBox_TextChangedCidade(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs e)
        {
            //LimparTela();
            //RegraForm();
            AutoSuggestBox box = (AutoSuggestBox)sender;
            bool valido = box.Text.Length == 11 && box.Text.All(char.IsDigit);
            if (!Regex.IsMatch(box.Text, @"[a-zA-Z\sà-ùÀ-Ù]{0,}"))
            {
                box.Text = "";
            }

            if (e.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (string.IsNullOrWhiteSpace(box.Text))
                    box.ItemsSource = null;
                else
                {
                    var suggestions = autoCompleteCidade.ObterSugestoes(box.Text);
                    countMunicipio = suggestions.ToList().Count;
                    if (countMunicipio > 0)
                    {
                        box.ItemsSource = suggestions.ToList();
                        RegraForm();
                    }
                    else
                    {
                        box.Text = "";
                        RegraForm();
                    }

                }
            }
        }
        //Sugestao Selecionada Cidade
        private void AutoSuggestBox_SuggestionChosenCidade(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs e)
        {
            var itemselecionado = e.SelectedItem;
            autoCompleteCidade.Selecionado = $"Item Selecionado: {itemselecionado}";
            nomeCidade = e.SelectedItem.ToString();

            RegraForm();
            //LimparTela();
            //ValidacaoCampos();

        }
        //Evento para pegar se foi utilizada a opçao do usuario ou do auto complete Cidade
        private void AutoSuggestBox_QuerySubmittedCidade(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs e)
        {
            //LimparTela();
            RegraForm();
            if (e.ChosenSuggestion == null)
            {
                autoCompleteCidade.Pesquisa = $"Pesquisa: {e.QueryText} ";
                autoCompleteCidade.Selecionado = "Item Selecionado: Nenhum";
            }
            else
            {
                autoCompleteCidade.Pesquisa = $"Sugestao: {e.ChosenSuggestion}";
            }

        }
        // FIM AUTOCOMPLETE CIDADE
        // Validação dos Campos do Formulário
        public void ValidacaoCampos()
        {
            if ((cidade.Text != null) && (SearchConteudo.IsVisible == false) && (txtData.IsVisible == true))
            {
                SearchConteudo.IsVisible = true;
                pckTipoInstituicao.IsVisible = true;
                //}else if ((cidade.Text != null) && (pckTipoInstituicao.Focus().Equals(true)))
                //{
                //SearchConteudo.IsVisible = true;
            }
            else if ((cidade.Text != null) && (SearchConteudo.Text != null) && (txtData.IsVisible == true))
            {
                btConfirmar.IsVisible = true;
            }

        }
        // Tipo de Instituto
        private void pckTipoInstituicao_SelectedIndexChanged(object sender, EventArgs e)
        {
            var valor = pckTipoInstituicao.Items[pckTipoInstituicao.SelectedIndex];
            var itemSelecionado = valor;
            if (itemSelecionado == "Educação")
            {
                tipoinstituto = 1;

            }
            else if (itemSelecionado == "Saúde")
            {
                tipoinstituto = 2;
            }
            RegraForm();
            //ValidacaoCampos();
            if (tipoinstituto != 0)
            {
                estados = autoCompleteInstituto.ListInstituto(nomeCidade, tipoinstituto);
            }
        }
        // Fim Tipo Instituto

        // AUTOCOMPLETE INSTITUTO

        private void SearchConteudo_TextChanged(object sender, TextChangedEventArgs e)
        {
            RegraForm();
            var keyword = SearchConteudo.Text;
            if (keyword.Length >= 3)
            {
                var sugestao = estados.Where(c => c.ToLower().Contains(keyword.ToLower()));
                listaEstados.ItemsSource = sugestao;

            }
        }

        private void ListaEstado_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item as string == null)
            {
                return;
            }
            else
            {
                listaEstados.ItemsSource = estados.Where(c => c.Equals(e.Item as string));

                SearchConteudo.Text = e.Item as string;
                nomeinstituto = e.Item as string;
                RegraForm();
                //ValidacaoCampos();
            }
        }
        //FIM Instituto

        private void OnDateSelected()
        {
            var e = txtData.Date;
            dataVisita = e.Month + "-" + e.Day + "-" + e.Year;
            _dataConsultaVisita = txtData.Date;
        }

        private async void BtConfirmar_Clicked(object sender, EventArgs e)
        {
            btConfirmar.IsEnabled = false;
            if (cidade.Text == "")
            {
                await DisplayAlert("", "Campo Cidade Obrigatório.", "Ok");
                btConfirmar.IsEnabled = true;
            }
            else if (pckTipoInstituicao.SelectedIndex == 0)
            {
                await DisplayAlert("", "Campo Tipo Instituição Obrigatório.", "Ok");
                btConfirmar.IsEnabled = true;
            }
            else if (SearchConteudo.Text == "")
            {
                await DisplayAlert("", "Campo Instituição Obrigatório.", "Ok");
                btConfirmar.IsEnabled = true;
            }
            else if (txtData.IsEnabled == false)
            {
                if (op == 1)
                {
                    await DisplayAlert("", "Campo Data da Visita Obrigatório.", "Ok");
                }
                else if (op == 2)
                {
                    await DisplayAlert("", "Campo Data da Revisita Obrigatório.", "Ok");
                }
                else
                {
                    await DisplayAlert("", "Campo Data é Obrigatório.", "Ok");
                }
                txtData.IsEnabled = true;
                btConfirmar.IsEnabled = true;
            }
            else
            {
                QuestionarioApi questionarioApi = new QuestionarioApi();
                Instituto _instituto = institutoService.GetInstituto(nomeinstituto, 13); //Verificar a substituição do valor 13 pelo cidadeId
                DownloadQuestionario downloadQuestionario = new DownloadQuestionario();
                OnDateSelected();
                string idinstituto = _instituto.Id.ToString();
                string token = Settings.Token;
                dataVisita = Convert.ToString(dataVisita.ToString());
                string respVisita = string.Empty;
                try
                {
                    var QuestionarioExist = databaseHelper.GetQuestionarioVisita(Convert.ToInt32(idinstituto), txtData.Date);
                    var idUsuario =Convert.ToInt32(Settings.idUsuario);
                    if (QuestionarioExist.Count == 0)
                    {
                        using (var Dialog = UserDialogs.Instance.Loading("Gerando Questionário...", null, null, true, MaskType.Black))
                        {
                            var addQuestionario = new AddQuestionario
                            {
                                id = idUsuario,
                                instituicaoId = Convert.ToInt32(idinstituto),
                                dataAplicacao = dataVisita,
                                colaboradorExternoId = 0,
                                formaColetaId = 0

                            };

                            string jsonRequest = JsonConvert.SerializeObject(addQuestionario);
                            StringContent httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application / json");
                            var client = new HttpClient();
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                            var endereco = Settings.EnderecoApi;
                            //var orgaoEmissor = Settings.emissorExternoId;
                            client.BaseAddress = new Uri(endereco);
                            var url = string.Format("visita/adicionar-visita");
                            var resultVisita = await client.PostAsync(url, httpContent);
                            respVisita = await resultVisita.Content.ReadAsStringAsync();
                            if (!resultVisita.IsSuccessStatusCode)
                            {
                                throw new Exception(resultVisita.RequestMessage.Content.ToString());

                            }
                            await Task.Delay(10000);
                        }
                        QuestionarioApi resposta;
                        using (var Dialog = UserDialogs.Instance.Loading("Sincronizando questionário...", null, null, true, MaskType.Black))
                        {
                            resposta = JsonConvert.DeserializeObject<QuestionarioApi>(respVisita);
                            await Task.CompletedTask;
                        }
                        if (resposta.success)
                        {
                            VisitaToken visitaToken;
                            bool retorno;
                            using (var Dialog = UserDialogs.Instance.Loading("Sincronizando aspectos e questões...", null, null, true, MaskType.Black))
                            {
                                visitaToken = new VisitaToken();
                                retorno = visitaToken.ConsumirVisita(resposta);
                                await Task.Delay(30000);
                            }
                            if (retorno)
                            {
                                await DisplayAlert("", "Questionário baixado com sucesso.", "Ok");
                                await Navigation.PushAsync(new VisualizarQuestionariosBaixados(), true);

                            }

                        }
                        else
                        {
                            await DisplayAlert("Erro", "Não é permitido baixar um questionário com emissor associado a outro órgão.", "Ok");
                        }
                    }
                    else
                    {
                        await DisplayAlert("", "Já possui um registro deste questionário no aplicativo.", "Ok");
                    }

                }
                catch (Exception ex)
                {

                    await DisplayAlert("", "Não existem registros para o(s) critério(s) informado(s).", "Ok");
                }
                btConfirmar.IsEnabled = true;
            }

        }
        public void LimparTela()
        {
            if (cidade.IsFocused)
            {
                var picker = pckTipoInstituicao;
                picker.SelectedIndex = 0;
                pckTipoInstituicao.SelectedIndexChanged += (sender, e) => { if (picker.SelectedIndex == 0) { picker.SelectedIndex = 0; } };

                SearchConteudo.Text = "";
                listaEstados.IsVisible = false;


            }
        }

        public void RegraForm()
        {
            if ((cidade.IsFocused) && (cidade.Text != ""))
            {
                pckTipoInstituicao.IsEnabled = true;
                var picker = pckTipoInstituicao;
                picker.SelectedIndex = 0;
                pckTipoInstituicao.SelectedIndexChanged += (sender, e) => { if (picker.SelectedIndex == 0) { picker.SelectedIndex = 0; } };
                //estados.Clear();
                listaEstados.ItemsSource = estados;
                listaEstados.IsVisible = false;
                SearchConteudo.Text = "";
                SearchConteudo.IsEnabled = false;
            }
            else if ((cidade.Text != "") && (pckTipoInstituicao.SelectedIndex != 0) && ((SearchConteudo.Text == null) || (SearchConteudo.Text == "")))
            {

                SearchConteudo.IsEnabled = true;
                listaEstados.IsVisible = true;
                listaEstados.IsEnabled = true;
            }
            else if ((cidade.Text != "") && (pckTipoInstituicao.SelectedIndex != 0) &&
                (SearchConteudo.Text != "") && (SearchConteudo.IsFocused))
            {
                listaEstados.IsVisible = true;
                //btConfirmar.IsEnabled = true;
            }
            else if ((cidade.IsFocused) && (cidade.Text == ""))
            {
                pckTipoInstituicao.IsEnabled = false;
                var picker = pckTipoInstituicao;
                picker.SelectedIndex = 0;
                SearchConteudo.IsEnabled = false;
                estados.Clear();
                listaEstados.ItemsSource = estados;
                SearchConteudo.Text = "";
                pckTipoInstituicao.SelectedIndexChanged += (sender, e) => { if (picker.SelectedIndex == 0) { picker.SelectedIndex = 0; } };
                btConfirmar.IsEnabled = false;
            }
            else if (pckTipoInstituicao.IsFocused)
            {
                estados.Clear();
                listaEstados.ItemsSource = estados;
                SearchConteudo.IsEnabled = true;


                SearchConteudo.Text = "";
                btConfirmar.IsEnabled = false;
            }
            else if ((cidade.Text != "") && (pckTipoInstituicao.SelectedIndex != 0) && (SearchConteudo.Text != ""))
            {
                listaEstados.IsVisible = false;
                btConfirmar.IsEnabled = true;
            }
        }
    }
}