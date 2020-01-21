using Milenio.Mobile.Services;
using Mobile.Helpers;
using Mobile.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.WebApi
{
    class RevisitaToken
    {

        public async void ConsumirRevisita(string token, int id, string data)
        {
            QuestionarioApi questionarioApi = new QuestionarioApi();
            QuestionarioService questionarioService = new QuestionarioService();
            QuestionarioVisita questionarioVisita = new QuestionarioVisita();
            AspectoQuestionario aspectoQuestionario = new AspectoQuestionario();
            MidiaAspecto midiaAspecto = new MidiaAspecto();
            Questoes questoes = new Questoes();
            MidiaQuestoes midiaQuestoes = new MidiaQuestoes();
            AspectoApi aspectoApi = new AspectoApi();
            MidiaAspectoApi midiaAspectoApi = new MidiaAspectoApi();
            QuestoesApi questoesApi = new QuestoesApi();
            MidiaQestoesApi midiaQestoesApi = new MidiaQestoesApi();
            string respVisita = string.Empty;
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var endereco = Settings.EnderecoApi;
                client.BaseAddress = new Uri(endereco);
                var url = string.Format("visita/obter-revisita/{0}/{1}", id, data);
                var resultVisita = await client.GetAsync(url);
                if (!resultVisita.IsSuccessStatusCode)
                {
                    throw new Exception(resultVisita.RequestMessage.Content.ToString());

                }
                respVisita = await resultVisita.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {

                await DisplayAlert("Erro", ex.Message, "Ok");
            }
            questionarioApi = JsonConvert.DeserializeObject<QuestionarioApi>(respVisita);
            questionarioVisita.id = questionarioApi.data.id;
            questionarioVisita.instituicaoId = questionarioApi.data.instituicaoId;
            questionarioVisita.dataAplicacao = questionarioApi.data.dataAplicacao;
            if (questionarioApi.data.colaboradorExternoId.HasValue)
            {
                questionarioVisita.colaboradorExternoId = questionarioApi.data.colaboradorExternoId.Value;
            }
            else
            {
                questionarioVisita.colaboradorExternoId = 0;
            }
            questionarioVisita.colaboradorInternoId = Convert.ToInt32(questionarioApi.data.colaboradorInternoId);
            questionarioVisita.formaColetaId = questionarioApi.data.formaColetaId;
            //questionarioVisita.revisitaId = questionarioApi.data.revisitaId;
            if (questionarioApi.data.revisitaId.HasValue)
            {
                questionarioVisita.revisitaId = questionarioApi.data.revisitaId.Value;
            }
            else
            {
                questionarioVisita.revisitaId = 0;
            }
            
            questionarioVisita.dataEnvio = questionarioApi.data.dataEnvio;
            questionarioVisita.finalizado = questionarioApi.data.finalizado;
            //insert Questionario
            questionarioService.InserirQuestionario(questionarioVisita);
            for (int i = 0; i < questionarioApi.data.aspectos.Count; i++)
            {
                aspectoApi.id = questionarioApi.data.aspectos[i].id;
                aspectoApi.descricao = questionarioApi.data.aspectos[i].descricao;
                aspectoApi.visitaId = questionarioApi.data.aspectos[i].visitaId;
                aspectoApi.dataFinalizacao = questionarioApi.data.aspectos[i].dataFinalizacao;
                aspectoApi.usuarioFinalizacaoId = questionarioApi.data.aspectos[i].usuarioFinalizacaoId;
                aspectoApi.formaColetaId = questionarioApi.data.aspectos[i].formaColetaId;
                aspectoApi.observacao = questionarioApi.data.aspectos[i].observacao;


                aspectoQuestionario.id = aspectoApi.id;
                aspectoQuestionario.descricao = aspectoApi.descricao;
                aspectoQuestionario.visitaId = aspectoApi.visitaId;
                aspectoQuestionario.dataFinalizacao = Convert.ToDateTime(aspectoApi.dataFinalizacao);
                aspectoQuestionario.usuarioFinalizacaoId = Convert.ToInt32(aspectoApi.usuarioFinalizacaoId);
                aspectoQuestionario.formaColetaId = aspectoApi.formaColetaId;
                aspectoQuestionario.observacao = aspectoApi.observacao;

                //Insert Aspecto Questionario
                questionarioService.InserirAspecto(aspectoQuestionario);
                aspectoApi.midias = questionarioApi.data.aspectos[i].midias;
                for (int j = 0; j < aspectoApi.midias.Count; j++)
                {
                    midiaAspectoApi.id = aspectoApi.midias[j].id;
                    midiaAspectoApi.aspectoVisitaId = aspectoApi.midias[j].aspectoVisitaId;
                    midiaAspectoApi.dataGravacao = aspectoApi.midias[j].dataGravacao;
                    midiaAspectoApi.legenda = aspectoApi.midias[j].legenda;
                    midiaAspectoApi.caminho = aspectoApi.midias[j].caminho;


                    midiaAspecto.id = midiaAspectoApi.id;
                    midiaAspecto.aspectoVisitaId = midiaAspectoApi.aspectoVisitaId;
                    midiaAspecto.dataGravacao = midiaAspectoApi.dataGravacao;
                    midiaAspecto.legenda = midiaAspectoApi.legenda;
                    midiaAspecto.caminho = midiaAspectoApi.caminho;

                    //Insert Midias Aspectos
                    questionarioService.InsertMidiasAspectos(midiaAspecto);
                }
                aspectoApi.questoes = questionarioApi.data.aspectos[i].questoes;
                for (int q = 0; q < aspectoApi.questoes.Count; q++)
                {
                    questoesApi.id = aspectoApi.questoes[q].id;
                    questoesApi.aspectoId = aspectoApi.questoes[q].aspectoId;
                    questoesApi.descricao = aspectoApi.questoes[q].descricao;
                    questoesApi.respostaNegativaId = aspectoApi.questoes[q].respostaNegativaId;
                    questoesApi.ordem = aspectoApi.questoes[q].ordem;
                    questoesApi.numeracao = aspectoApi.questoes[q].numeracao;
                    questoesApi.descricaoDevolutiva = aspectoApi.questoes[q].descricaoDevolutiva;
                    //if (aspectoApi.questoes[q].respostaId.HasValue)
                    //{
                        questoesApi.respostaId = aspectoApi.questoes[q].respostaId;
                    //}
                    //else
                   // {
                     //   questoesApi.respostaId = 0;
                    //}
                    questoesApi.observacao = aspectoApi.questoes[q].observacao;

                    questoes.id = questoesApi.id;
                    questoes.aspectoId = questoesApi.aspectoId;
                    questoes.descricao = questoesApi.descricao;
                    questoes.respostaNegativaId = questoesApi.respostaNegativaId;
                    questoes.ordem = questoesApi.ordem;
                    questoes.numeracao = questoesApi.numeracao;
                    questoes.descricaoDevolutiva = questoesApi.descricaoDevolutiva;
                    questoes.resposta = questoesApi.respostaId;
                    questoes.observacao = questoesApi.observacao;

                    // Insert Questões
                    questionarioService.InsertQuestoes(questoes);

                    questoesApi.midias = aspectoApi.questoes[q].midias;
                    for (int m = 0; m < questoesApi.midias.Count; m++)
                    {
                        midiaQestoesApi.id = questoesApi.midias[m].id;
                        midiaQestoesApi.questaoVisitaId = questoesApi.midias[m].questaoVisitaId;
                        midiaQestoesApi.dataGravacao = questoesApi.midias[m].dataGravacao;
                        midiaQestoesApi.legenda = questoesApi.midias[m].legenda;
                        midiaQestoesApi.caminho = questoesApi.midias[m].caminho;
                        midiaQuestoes.id = midiaQestoesApi.id;
                        midiaQuestoes.questaoVisitaId = midiaQestoesApi.id;
                        midiaQuestoes.dataGravacao = midiaQestoesApi.dataGravacao;
                        midiaQuestoes.legenda = midiaQestoesApi.legenda;
                        midiaQuestoes.caminho = midiaQestoesApi.caminho;

                        // Insert Midia Questões
                        questionarioService.InsertMidiaQuestoes(midiaQuestoes);
                    }

                }
            }
        }
        private Task DisplayAlert(string v1, object message, string v2)
        {
            throw new NotImplementedException();
        }
    }
}
