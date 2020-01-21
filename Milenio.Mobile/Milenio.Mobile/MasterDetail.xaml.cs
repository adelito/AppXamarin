using Mobile.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mobile.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mobile.Model;
using Mobile.Views;

namespace Mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MasterDetail : MasterDetailPage
	{
        public List<MasterPageItem> menuList { get; set; }
		public MasterDetail (Usuario usuario)
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            menuList = new List<MasterPageItem>();

            // incluindo items de menu e definindo : titulo ,page and icon
            menuList.Add(new MasterPageItem(){Titulo = "Inicio", Icone = "home.png", TargetType =typeof(Inicio)});
            //menuList.Add(new MasterPageItem(){Titulo = "Gerar Questionário", Icone = "list.png",TargetType =typeof(GerarQuestionario)});
            menuList.Add(new MasterPageItem(){Titulo = "Baixar Questionário",Icone = "syncing.png",TargetType =typeof(BaixarQuestionario) });
            menuList.Add(new MasterPageItem(){Titulo = "Preencher Questionário",Icone = "contract.png", TargetType =typeof(PreencherQuestionario) });
            menuList.Add(new MasterPageItem(){ Titulo = "Alterar Senha", Icone = "login.png", TargetType = typeof(RedefinirSenha) });
            menuList.Add(new MasterPageItem() { Titulo = "Sair", Icone = "door.png", TargetType = typeof(MainPage)});
            // Configurando o ItemSource fpara o ListView na MainPage.xaml
            paginaMestreList.ItemsSource = menuList;
            // Impririmindo o nome do usuario
            lbUsuario.Text = usuario.DsNome;
            // navegação inicial
            var Inicio = new Inicio();
            this.BindingContext = usuario;
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Inicio)));
        }
        // Evento para a seleção de item no menu
        // trata a seleção do usuário no ListView
        private void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (MasterPageItem)e.SelectedItem;
            Type page = item.TargetType;
            Detail = new NavigationPage((Page)Activator.CreateInstance(page));
            IsPresented = false;
        }

    }
}