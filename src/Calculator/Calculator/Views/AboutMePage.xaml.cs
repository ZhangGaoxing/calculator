using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Calculator.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AboutMePage : ContentPage
	{
		public AboutMePage ()
		{
			InitializeComponent ();
		}

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private async void CloseWindow_Clicked(object sender, EventArgs e)
        {
            var page = await Navigation.PopAsync();
            Navigation.RemovePage(page);
        }
    }
}