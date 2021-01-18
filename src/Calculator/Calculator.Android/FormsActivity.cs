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
    public class FormsActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private long exitTime = 0;

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
            LoadApplication(new App());
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