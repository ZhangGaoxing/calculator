using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Calculator.Droid
{
    [Activity(Label = "高兴计算", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class ShortcutContainerActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private long exitTime = 0;

        public ShortcutContainerActivity()
        {

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            }
            catch
            {

            }

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Intent intent = Intent;
            string pageName = intent.GetStringExtra("PageName");

            var app = new App();
            switch (pageName)
            {
                case "StandardPage":
                    app.MainPage = new Calculator.MainPage(Data.PageName.StandardPage);
                    break;
                case "SciencePage":
                    app.MainPage = new Calculator.MainPage(Data.PageName.SciencePage);
                    break;
                case "ProgrammerPage":
                    app.MainPage = new Calculator.MainPage(Data.PageName.ProgrammerPage);
                    break;
                case "StatisticsPage":
                    app.MainPage = new Calculator.MainPage(Data.PageName.StatisticsPage);
                    break;
                case "ColorPage":
                    app.MainPage = new Calculator.MainPage(Data.PageName.ColorPage);
                    break;
                case "DatePage":
                    app.MainPage = new Calculator.MainPage(Data.PageName.DatePage);
                    break;
                default:
                    break;
            }
            
            LoadApplication(app);
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                if ((DateTime.Now.Ticks / 10000 - exitTime) > 2000)
                {
                    Toast.MakeText(this, "再按一次退出程序", ToastLength.Short).Show();
                    exitTime = DateTime.Now.Ticks / 10000;
                }
                else
                {
                    Finish();
                    Process.KillProcess(Process.MyPid());
                }

                return true;
            }

            return base.OnKeyDown(keyCode, e);
        }
    }
}