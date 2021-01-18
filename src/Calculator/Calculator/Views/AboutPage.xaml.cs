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
	public partial class AboutPage : ContentPage
	{
        public AboutPage()
        {
            InitializeComponent();

            Version v = new Version(1, 2, 0);
            VersionLabel.Text = $"{v.Major}.{v.Minor}.{v.Build}";

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                if (s is Label)
                {
                    Device.OpenUri(new Uri("mailto:zhangyuexin121@live.cn"));
                }
                else if (s is Image)
                {
                    Device.OpenUri(new Uri("http://www.weibo.com/279639933"));
                }
            };
            Email.GestureRecognizers.Add(tapGestureRecognizer);
            Weibo.GestureRecognizers.Add(tapGestureRecognizer);
        }

        private async void AboutMe_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutMePage());
        }
    }
}