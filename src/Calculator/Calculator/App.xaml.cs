using Calculator.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Calculator
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                var localize = DependencyService.Get<ILocalize>();
                var ci = localize.GetCurrentCultureInfo();
                //Resource.Resources.Culture = ci;// set the RESX for resource localization
                localize.SetLocale(ci); // set the Thread for locale-aware methods
            }

            MainPage = new Calculator.MainPage();           
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
