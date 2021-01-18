using Calculator.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Calculator
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : ContentPage
    {
        // 向 MainPage 传递控件
        public ListView primaryListView { get { return PrimaryListView; } }
        public ListView secondaryListView { get { return SecondaryListView; } }

        public MasterPage()
        {
            InitializeComponent();

            // 设置不同平台的字体路径
            string fontFamily;
            switch (Device.RuntimePlatform)
            {
                case "Android":
                    fontFamily = "segmdl2.ttf#Segoe MDL2 Assets";
                    break;

                case "iOS":
                    fontFamily = "Segoe MDL2 Assets";
                    break;

                case "Windows":
                    fontFamily = "/Assets/segmdl2.ttf#Segoe MDL2 Assets";
                    break;

                case "WinPhone":
                    fontFamily = "/Assets/segmdl2.ttf#Segoe MDL2 Assets";
                    break;

                default:
                    fontFamily = "segmdl2.ttf#Segoe MDL2 Assets";
                    break;
            }

            // 列表项
            var primaryItems = new List<MasterPageItem>() {
                new MasterPageItem
                {
                    Title = "标准",
                    FontFamily = fontFamily,
                    Icon = "\xE75F",
                    Color = Color.FromHex("#009999"),
                    Selected = true,
                    DestPage = typeof(StandardPage)
                },
                new MasterPageItem
                {
                    Title = "科学",
                    FontFamily = fontFamily,
                    Icon = "\xE7BE",
                    Color = Color.Black,
                    Selected = false,
                    DestPage = typeof(SciencePage)
                },
                new MasterPageItem
                {
                    Title = "程序员",
                    FontFamily = fontFamily,
                    Icon = "\xE950",
                    Color = Color.Black,
                    Selected = false,
                    DestPage = typeof(ProgrammerPage)
                },
                new MasterPageItem
                {
                    Title = "统计信息",
                    FontFamily = fontFamily,
                    Icon = "\xE9D2",
                    Color = Color.Black,
                    Selected = false,
                    DestPage = typeof(StatisticsPage)
                },
                new MasterPageItem
                {
                    Title = "日期计算",
                    FontFamily = fontFamily,
                    Icon = "\xE787",
                    Color = Color.Black,
                    Selected = false,
                    DestPage = typeof(DatePage)
                },
                new MasterPageItem
                {
                    Title = "颜色",
                    FontFamily = fontFamily,
                    Icon = "\xE790",
                    Color = Color.Black,
                    Selected = false,
                    DestPage = typeof(ColorPage)
                },
            };

            var secondaryItems = new List<MasterPageItem>() {
                new MasterPageItem
                {
                    Title = "帮助",
                    FontFamily = fontFamily,
                    Icon = "\xEA80",
                    Color = Color.Black,
                    Selected = false,
                    DestPage = typeof(HelpPage)
                },
                new MasterPageItem
                {
                    Title = "关于",
                    FontFamily = fontFamily,
                    Icon = "\xE783",
                    Color = Color.Black,
                    Selected = false,
                    DestPage = typeof(AboutPage)
                }
            };

            // ListView 数据绑定
            PrimaryListView.ItemsSource = primaryItems;
            SecondaryListView.ItemsSource = secondaryItems;

            // 设置二级菜单高度
            SecondaryListView.HeightRequest = 48 * secondaryItems.Count;
        }
    }
}