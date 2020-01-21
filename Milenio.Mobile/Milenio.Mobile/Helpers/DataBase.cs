using System.Collections.Generic;
using System.Text;
using Mobile.Model;
using SQLite;
using System.Linq;
using PCLExt.FileStorage;
using PCLExt.FileStorage.Folders;
using Milenio.Mobile.Model;
using System;

namespace Mobile.Helpers
{
    class DatabaseHelper
    {
        //defina uma conexao e o  nome do banco de dados
        static SQLiteConnection sqliteconnection;
        public const string DbFileName = "Milenio.db";
        public DatabaseHelper()
        {
            //Cria uma pasta base local para o dispositivo
            var pasta = new LocalRootFolder();
            //Cria o arquivo
            var arquivo = pasta.CreateFile(DbFileName, CreationCollisionOption.OpenIfExists);
            //Abre o BD
            sqliteconnection = new SQLiteConnection(arquivo.Path);
            //Cria a tabela no BD
            sqliteconnection.CreateTable<Usuario>();
            sqliteconnection.CreateTable<Municipio>();
            sqliteconnection.CreateTable<Instituto>();
            sqliteconnection.CreateTable<QuestionarioVisita>();
            sqliteconnection.CreateTable<AspectoQuestionario>();
            sqliteconnection.CreateTable<MidiaAspecto>();
            sqliteconnection.CreateTable<Questoes>();
            sqliteconnection.CreateTable<MidiaQuestoes>();
            sqliteconnection.CreateTable<QuestoesAnuladas>(); 
            sqliteconnection.CreateTable<QuantidadeMidia>();
        }
        public Boolean DBExists()
        {
            var pasta = new LocalRootFolder();
            var arquivo = pasta.CreateFile(DbFileName, CreationCollisionOption.OpenIfExists);
            if (arquivo.Exists)
            {
                return true;
            }
            else
            {
                return false;
            }
             
        }
        //Pegar dados especifico por Usuario e Senha
        public Usuario GetUsuarioData(string usuario, string senha)
        {
            return sqliteconnection.Table<Usuario>().FirstOrDefault(t => t.login == usuario && t.senha==senha);
        }
        public Usuario GetUsuarioDataLogin(string usuario)
        {
            return sqliteconnection.Table<Usuario>().FirstOrDefault(t => t.login == usuario);
        }
        // Inserir dados
        public void InsertUsuario(Usuario usuario)
        {
            sqliteconnection.Insert(usuario);
        }
        // Atualizar dados
        public void UpdateUsuario(Usuario usuario)
        {
            sqliteconnection.Update(usuario);
        }


        //Inserir Municipio
        public void InsertMunicipio(Municipio municipio)
        {
            //sqliteconnection.Insert(municipio);
            List<Municipio> municipioList = new List<Municipio>();
            municipioList.Add(municipio);
            sqliteconnection.InsertAll(municipioList);
            //sqliteconnection.InsertAll(municipio);
        }

        //Pegar Municipio pelo nome
        public Municipio GetMunicipioData(string nomemunicipio)
        {
            return sqliteconnection.Table<Municipio>().FirstOrDefault(x => x.NomeMunicipio == nomemunicipio);
        }
        public List<Municipio> GetMunicipio()
        {
            List<Municipio> _municipio = new List<Municipio>();
             _municipio = sqliteconnection.Table<Municipio>().ToList();
            return _municipio;
           
        }
        public List<Municipio> GetMunicipioInt(int id)
        {
            List<Municipio> municipio = new List<Municipio>();
            municipio = sqliteconnection.Table<Municipio>().Where(x => x.Id == id).ToList();
            return municipio;
        }

        public Municipio GetMunicipioIntQuestionarioBaixados(int id)
        {
           return sqliteconnection.Table<Municipio>().FirstOrDefault(x => x.Id == id);
        }
        //Institutos
        public void InserirInstituto(List<Instituto> instituto)
        {
            sqliteconnection.InsertAll(instituto);
        }
        public Instituto GetInstitutoData(string nomeinstituto, int CidadeId)
        {
            return sqliteconnection.Table<Instituto>().FirstOrDefault(x => x.NomeInstituto == nomeinstituto && x.CidadeId == CidadeId);
        }

        public List<Instituto> GetInstitutoInt(int id)
        {
           
            List<Instituto> instituto = new List<Instituto>();
            instituto = sqliteconnection.Table<Instituto>().Where(x => x.Id == id).ToList();
            return instituto;
        }
        public Instituto GetInstitutoIntQuestionarioBaixados(int id) // Institutos Questionarios Baixados
        {
           
            return  sqliteconnection.Table<Instituto>().FirstOrDefault(x => x.Id == id);
        }

        public List<Instituto> GetInstituto(int idcidade, int tipoinstituto)
        {
            List<Instituto> institutos = new List<Instituto>();
            institutos = sqliteconnection.Table<Instituto>().Where(x => x.CidadeId == idcidade && x.TipoInstituicaoId==tipoinstituto).ToList();
            return institutos;
        }

        // Questionário Visita

        public void InserirQuestionario(QuestionarioVisita questionario)
        {
                sqliteconnection.Insert(questionario);
                sqliteconnection.Commit();
                var query = sqliteconnection.Table<QuestionarioVisita>().ToList();
        }

        public List<QuestionarioVisita> GetQuestionarioVisita(int institutoId, DateTime DataVisita)
        {
            string day = DataVisita.Day.ToString();
            string month = DataVisita.Month.ToString();
            string year = DataVisita.Year.ToString();
            string datavisita = day + "/" + month + "/" + year;
            DateTime dataAplicação = Convert.ToDateTime(datavisita);
           var query = sqliteconnection.Table<QuestionarioVisita>().Where(x => x.instituicaoId == institutoId && x.dataAplicacao== dataAplicação).ToList();
           return query; 
        }

        public QuestionarioVisita GetQuestionarioVisitainstitutoId(int institutoId)
        {
            return sqliteconnection.Table<QuestionarioVisita>().FirstOrDefault(x => x.instituicaoId == institutoId);
        }

        public List<QuestionarioVisita> GetQuestionarioVisitaPreencherQuestionario() //Visualizar Questionarios Preencher Questionarios
        {

            //var query =  sqliteconnection.Table<QuestionarioVisita>().Where(x => x.finalizado==false && x.revisitaId==0).ToList();
            var query = sqliteconnection.Table<QuestionarioVisita>().Where(x => x.finalizado == false).OrderBy(q => q.dataAplicacao).ToList();
            
            return query; 
        }
        public int GetQuestionarioVisita() //Visualizar Questionarios Preencher Questionarios
        {

            //var query =  sqliteconnection.Table<QuestionarioVisita>().Where(x => x.finalizado==false && x.revisitaId==0).ToList();
           return sqliteconnection.Table<QuestionarioVisita>().Count(x => x.finalizado == false);
            
        }
         public void ExcluirQuestiorario(int id)
        {
           
            sqliteconnection.Table<QuestionarioVisita>().Delete(x => x.id == id);
        }
        // Aspectos Questionario

        public void InserirAspecto(AspectoQuestionario aspecto)
        {
            sqliteconnection.Insert(aspecto);
        }
        public IEnumerable<AspectoQuestionario> GetAspectoQuestionario(int visitaId)
        {
            return sqliteconnection.Table<AspectoQuestionario>().Where(x => x.visitaId == visitaId);
        }

        public AspectoQuestionario GetAspecto(int AspectoId)
        {
            return sqliteconnection.Table<AspectoQuestionario>().FirstOrDefault(x => x.id == AspectoId);
        }
        public List<AspectoQuestionario> GetAspectoQuestionarioID(int visitaId)
        {
            List<AspectoQuestionario> IdAspecto = new List<AspectoQuestionario>();
            IdAspecto = sqliteconnection.Table<AspectoQuestionario>().Where(x => x.visitaId == visitaId).ToList();
            return IdAspecto;
        } 
        public List<AspectoQuestionario> GetAspectoQuestionariosIdVisitaNomeAspecto(int Id)
        {
            List<AspectoQuestionario> query = new List<AspectoQuestionario>();
            query = sqliteconnection.Table<AspectoQuestionario>().Where(x => x.id == Id).ToList();
            return query;
        }

        public void ExcluirAspectos(int id)
        {

            sqliteconnection.Table<AspectoQuestionario>().Delete(x => x.visitaId == id);
        }
        // Midias Aspectos
        public void InsertMidiasAspectos(MidiaAspecto midiaAspecto)
        {
            sqliteconnection.Insert(midiaAspecto);
        }

        public List<MidiaAspecto> GetMidiaAspecto(int aspectoVisitaId)
        {
            return sqliteconnection.Table<MidiaAspecto>().Where(x => x.aspectoVisitaId == aspectoVisitaId).ToList();
        }
        public void ExcluirMidiaAspecto(string caminho)
        {
            sqliteconnection.Table<MidiaAspecto>().Delete(x=>x.caminho == caminho);
        }
        public void UpdateAspecto(int id,string observacao)
        {
            sqliteconnection.Query<AspectoQuestionario>("UPDATE AspectQuestionario set observacao=? Where id=?", observacao, id);

        }
        // Questões 
        public void InsertQuestoes(Questoes questoes)
        {
            sqliteconnection.Insert(questoes);
        }

        public List<Questoes> GetQuestoes(int aspectoVisitaId)
        {
            List<Questoes> query = new List<Questoes>();
            query =sqliteconnection.Table<Questoes>().Where(x => x.aspectoId == aspectoVisitaId).OrderBy(x => x.ordem).ToList();
            //query = sqliteconnection.Table<Questoes>().ToList();
            return query;
        }
        public void UpdateQuestoesFechada(int id, string resposta, string observacao)
        {
            sqliteconnection.Query<Questoes>("UPDATE Questoes set resposta=?, observacao=?, respondida=true Where Id=?", resposta, observacao, id);
            
            }

        public void UpdateQuestoesAberta(int id, string observacao)
        {
            sqliteconnection.Query<Questoes>("UPDATE Questoes set observacao=?, respondida=true Where Id=?", observacao, id);

        }
        public void UpdatePergunta(int id)
        {
            sqliteconnection.Query<Questoes>("UPDATE Questoes set respondida=true Where Id=?",id);

        }
        public void UpdatequestaoAnulada(int id)
        {
            sqliteconnection.Query<Questoes>("UPDATE Questoes set questaoAnulada=true Where Id=?", id);

        }
        public void ExcluirQuesteos(int id)
        {

            sqliteconnection.Table<Questoes>().Delete(x => x.aspectoId == id);
        }

        // Midias Questões
        public void InsertMidiaQuestoes(MidiaQuestoes midiaQuestoes)
        {
            sqliteconnection.Insert(midiaQuestoes);
        }

        
        public List<MidiaQuestoes> GetMidiaQuestoes(int questaoVisitaId)
        {
            List<MidiaQuestoes> query = new List<MidiaQuestoes>();
            query = sqliteconnection.Table<MidiaQuestoes>().Where(x => x.questaoVisitaId == questaoVisitaId).ToList();
            return query;
        }
        public List<MidiaQuestoes> ConfirmMidiaQuestoes(int questaoVisitaId)
        {
            List<MidiaQuestoes> query = new List<MidiaQuestoes>();
            query = sqliteconnection.Table<MidiaQuestoes>().Where(x => x.questaoVisitaId == questaoVisitaId && x.flagEnvio==false).ToList();
            return query;
        }
        public void ExcluirMidiaQuestao(string caminho)
        {
            sqliteconnection.Table<MidiaQuestoes>().Delete(x => x.caminho == caminho);
            
        }
        
        public void ConfirmUploadImage(int questaoVisitaId, string legenda)
        {
            sqliteconnection.Query<MidiaQuestoes>("UPDATE MidiaQuestoes set flagEnvio=true Where questaoVisitaId=? and legenda=?", questaoVisitaId, legenda);

        }
        public MidiaQuestoes MidiaQuestoes(string legenda, int questaoVisitaId)
        {
            return sqliteconnection.Table<MidiaQuestoes>().FirstOrDefault(x => x.legenda == legenda && x.questaoVisitaId==questaoVisitaId);
        }
        public int QuantidadeMidiaQuestoes(int id)
        {
            return sqliteconnection.Table<MidiaQuestoes>().Count(x =>x.questaoVisitaId == id);
        }

        //Cabecalho Questões
        public Questoes GetQuestoesCabecalho(int id)
        {
            return sqliteconnection.Table<Questoes>().FirstOrDefault(x => x.id == id);
        }
        public AspectoQuestionario GetAspectoCabecalho(int id)
        {
            return sqliteconnection.Table<AspectoQuestionario>().FirstOrDefault(x => x.id == id);
        }
        public QuestionarioVisita GetQuestionarioCabecalho(int id)
        {
            return sqliteconnection.Table<QuestionarioVisita>().FirstOrDefault(x => x.id == id);
        }
        public Instituto GetInstitutoCabecalho(int id)
        {
            return sqliteconnection.Table<Instituto>().FirstOrDefault(x => x.Id == id);
        }
        public Municipio GetMunicipioCabecalho(int id)
        {
            return sqliteconnection.Table<Municipio>().FirstOrDefault(x => x.Id == id);
        }
        public Cabecalho Montarcabecalho(int id)
        {
            Cabecalho cabecalho = new Cabecalho();
            var getquestoes = GetQuestoesCabecalho(id);
            var getaspecto = GetAspectoCabecalho(getquestoes.aspectoId);
            var getquestionario = GetQuestionarioCabecalho(getaspecto.visitaId);
            var getinstituto = GetInstitutoCabecalho(getquestionario.instituicaoId);
            var getcidade = GetMunicipioCabecalho(getinstituto.CidadeId);


            cabecalho.Aspecto = getaspecto.descricao;
            cabecalho.DataVisita = getquestionario.dataAplicacao;
            cabecalho.protocolo = getquestionario.id;
            cabecalho.Instituicao = getinstituto.NomeInstituto;
            cabecalho.Cidade = getcidade.NomeMunicipio;
            return cabecalho;
        }

        //Envio Questionário

        public List<Questoes> TotalQuestoesAspecto(int id)
        {
            List<Questoes> questoes = new List<Questoes>();
            questoes=sqliteconnection.Table<Questoes>().Where(x => x.aspectoId == id).ToList();
            return questoes;
        }

        public int QuantidadeQuestoesAspectoNaoRespondida(int id)
        {
            return sqliteconnection.Table<Questoes>().Count(x => x.respondida == false && x.aspectoId == id);
        }

        // Questões Anuladas

        public void InsertQuestoesAnuladas(QuestoesAnuladas questoesanuladas)
        {
            sqliteconnection.Insert(questoesanuladas);
        }
        public List<QuestoesAnuladas> GetQuestoesAnuladas(int id)
        {
            List<QuestoesAnuladas> questoes = new List<QuestoesAnuladas>();
           // questoes = sqliteconnection.Table<QuestoesAnuladas>().Where(x => x.questaoVisitaId == id && x.respostaId == resposta).ToList();
            questoes = sqliteconnection.Table<QuestoesAnuladas>().Where(x => x.questaoVisitaId == id).ToList();
            return questoes;
        }
        public void UpdateQuestoesAnulada(int id, int resposta)
        {
            sqliteconnection.Query<Questoes>("UPDATE Questoes set resposta=? Where Id=?", resposta,id);
            
        }
        public void ExcluirQuestoesanuladas(int id)
        {

            sqliteconnection.Table<QuestoesAnuladas>().Delete(x => x.questaoVisitaId == id);
        }
        public void InsertQtdMidia(QuantidadeMidia qtdmidia)
        {
            sqliteconnection.Insert(qtdmidia);
        }
        public void  ExcluirqtdMidia()
        {
            sqliteconnection.Query<QuantidadeMidia>("Delete from QuantidadeMidia");
        }
        public QuantidadeMidia GetqtdMidia() // Institutos Questionarios Baixados
        {

            return sqliteconnection.Table<QuantidadeMidia>().FirstOrDefault();
        }
    }
}
