using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Content;
using Android.Graphics.Drawables;
using System.Collections.Generic;
using Calculator.Views;
using Calculator.Data;

namespace Calculator.Droid
{
    [Activity(Label = "高兴计算", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            try
            {
                this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;

                SetShortcut();
            }
            catch
            {

            }

            StartActivity(typeof(FormsActivity));

            Finish();
        }

        private void SetShortcut()
        {
            List<ShortcutInfo> shortcutInfoList = new List<ShortcutInfo>();

            Intent science = new Intent();
            science.SetClass(this, typeof(ShortcutContainerActivity));
            science.SetAction(Intent.ActionMain);
            science.PutExtra("PageName", (PageName.SciencePage).ToString());
            ShortcutInfo scienceInfo = new ShortcutInfo.Builder(this, "science")
                .SetRank(0)
                .SetIcon(Icon.CreateWithResource(this, Resource.Drawable.ScienceIcon))
                .SetShortLabel("科学")
                .SetLongLabel("科学")
                .SetIntent(science)
                .Build();
            shortcutInfoList.Add(scienceInfo);

            Intent programmer = new Intent();
            programmer.SetClass(this, typeof(ShortcutContainerActivity));
            programmer.SetAction(Intent.ActionMain);
            programmer.PutExtra("PageName", (PageName.ProgrammerPage).ToString());
            ShortcutInfo programmerInfo = new ShortcutInfo.Builder(this, "programmer")
                .SetRank(1)
                .SetIcon(Icon.CreateWithResource(this, Resource.Drawable.ProgrammerIcon))
                .SetShortLabel("程序员")
                .SetLongLabel("程序员")
                .SetIntent(programmer)
                .Build();
            shortcutInfoList.Add(programmerInfo);

            Intent statistics = new Intent();
            statistics.SetClass(this, typeof(ShortcutContainerActivity));
            statistics.SetAction(Intent.ActionMain);
            statistics.PutExtra("PageName", (PageName.StatisticsPage).ToString());
            ShortcutInfo statisticsInfo = new ShortcutInfo.Builder(this, "statistics")
                .SetRank(2)
                .SetIcon(Icon.CreateWithResource(this, Resource.Drawable.StatisticsIcon))
                .SetShortLabel("统计信息")
                .SetLongLabel("统计信息")
                .SetIntent(statistics)
                .Build();
            shortcutInfoList.Add(statisticsInfo);

            Intent color = new Intent();
            color.SetClass(this, typeof(ShortcutContainerActivity));
            color.SetAction(Intent.ActionMain);
            color.PutExtra("PageName", (PageName.ColorPage).ToString());
            ShortcutInfo colorInfo = new ShortcutInfo.Builder(this, "color")
                .SetRank(3)
                .SetIcon(Icon.CreateWithResource(this, Resource.Drawable.ColorIcon))
                .SetShortLabel("颜色")
                .SetLongLabel("颜色")
                .SetIntent(color)
                .Build();
            shortcutInfoList.Add(colorInfo);

            Intent date = new Intent();
            date.SetClass(this, typeof(ShortcutContainerActivity));
            date.SetAction(Intent.ActionMain);
            date.PutExtra("PageName", (PageName.DatePage).ToString());
            ShortcutInfo dateInfo = new ShortcutInfo.Builder(this, "date")
                .SetRank(4)
                .SetIcon(Icon.CreateWithResource(this, Resource.Drawable.DateIcon))
                .SetShortLabel("日期计算")
                .SetLongLabel("日期计算")
                .SetIntent(date)
                .Build();
            shortcutInfoList.Add(dateInfo);

            ShortcutManager shortcutManager = (ShortcutManager)GetSystemService(Context.ShortcutService);
            shortcutManager.SetDynamicShortcuts(shortcutInfoList);
        }
    }
}

