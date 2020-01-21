using Milenio.Mobile.ViewModel;
using Mobile.Helpers;
using Mobile.Model;
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
	public partial class VisualizarQuestionariosBaixados : ContentPage
	{
        DatabaseHelper service = new DatabaseHelper();
        public VisualizarQuestionariosBaixados ()
		{
            try
            {
                InitializeComponent();
                //QuestionariosBaixados.ItemTapped += Btn_ItemTapped;
                BindingContext = new QuestionarioViewModel();
            }
            catch (Exception ex)
            {

                throw ex;
            }
			
        }

       /* private void BtExcluirQuestionario_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var questionario = button.BindingContext as QuestionarioViewModel;

            var vm = BindingContext as QuestionarioViewModel;
            vm.RemoveCommand.Execute(questionario);
        }*/
        /* private void QuestionariosBaixados_ItemSelected(object sender, SelectedItemChangedEventArgs e)
{
    ListView btn = (ListView)sender;
    //btn.ItemTapped += Btn_ItemTapped;

}

private async void Btn_ItemTapped(object sender, ItemTappedEventArgs e)
{
    var item = e.Item;

        await Navigation.PushAsync(new PreencherQuestionarioAspecto(item));

}*/
    }
}