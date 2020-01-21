using Milenio.Mobile.Repositorio;
using Milenio.Mobile.ViewModel;
using Mobile.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PreencherQuestionarioVisita : ContentPage
	{
        
        public PreencherQuestionarioVisita ()
		{
			InitializeComponent ();
            QuestionariosBaixados.ItemTapped += Btn_ItemTapped;
            BindingContext = new PreencherQuestionarioVisitaViewModel();
            
        }

        private  void QuestionariosBaixados_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ListView btn = (ListView)sender;
            //btn.ItemTapped += Btn_ItemTapped;
            
        }

        private async void Btn_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item;
            await Navigation.PushAsync(new PreencherQuestionarioAspecto(item));
        }
    }
}