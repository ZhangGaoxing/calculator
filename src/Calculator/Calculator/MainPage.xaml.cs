using Android.Content.PM;
using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Calculator.Views;
using Calculator.Data;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Calculator
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();

            // ListView 点击事件
            masterPage.primaryListView.ItemSelected += MasterPageItemSelected;
            masterPage.secondaryListView.ItemSelected += MasterPageItemSelected;

            var detail = Detail as NavigationPage;
            detail.BarBackgroundColor = Color.FromHex("#D3D3D3");
            detail.BarTextColor = Color.White;               
        }

        public MainPage(PageName name)
        {
            InitializeComponent();

            switch (name)
            {
                case PageName.StandardPage:
                    {
                        foreach (MasterPageItem mpi in masterPage.primaryListView.ItemsSource)
                        {
                            mpi.Selected = false;
                            mpi.Color = Color.Black;

                            if (mpi.Title == "标准")
                            {
                                mpi.Selected = true;
                                mpi.Color = Color.FromHex("#009999");
                            }
                        }

                        var detail = new NavigationPage((Page)Activator.CreateInstance(typeof(StandardPage)));
                        detail.BarBackgroundColor = Color.FromHex("#D3D3D3");
                        detail.BarTextColor = Color.White;
                        Detail = detail;
                    }                   
                    break;
                case PageName.SciencePage:
                    {
                        foreach (MasterPageItem mpi in masterPage.primaryListView.ItemsSource)
                        {
                            mpi.Selected = false;
                            mpi.Color = Color.Black;

                            if (mpi.Title == "科学")
                            {
                                mpi.Selected = true;
                                mpi.Color = Color.FromHex("#009999");
                            }
                        }

                        var detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SciencePage)));
                        detail.BarBackgroundColor = Color.FromHex("#D3D3D3");
                        detail.BarTextColor = Color.White;
                        Detail = detail;
                    }
                    break;
                case PageName.ProgrammerPage:
                    {
                        foreach (MasterPageItem mpi in masterPage.primaryListView.ItemsSource)
                        {
                            mpi.Selected = false;
                            mpi.Color = Color.Black;

                            if (mpi.Title == "程序员")
                            {
                                mpi.Selected = true;
                                mpi.Color = Color.FromHex("#009999");
                            }
                        }

                        var detail = new NavigationPage((Page)Activator.CreateInstance(typeof(ProgrammerPage)));
                        detail.BarBackgroundColor = Color.FromHex("#D3D3D3");
                        detail.BarTextColor = Color.White;
                        Detail = detail;
                    }
                    break;
                case PageName.StatisticsPage:
                    {
                        foreach (MasterPageItem mpi in masterPage.primaryListView.ItemsSource)
                        {
                            mpi.Selected = false;
                            mpi.Color = Color.Black;

                            if (mpi.Title == "统计信息")
                            {
                                mpi.Selected = true;
                                mpi.Color = Color.FromHex("#009999");
                            }
                        }

                        var detail = new NavigationPage((Page)Activator.CreateInstance(typeof(StatisticsPage)));
                        detail.BarBackgroundColor = Color.FromHex("#D3D3D3");
                        detail.BarTextColor = Color.White;
                        Detail = detail;
                    }
                    break;
                case PageName.ColorPage:
                    {
                        foreach (MasterPageItem mpi in masterPage.primaryListView.ItemsSource)
                        {
                            mpi.Selected = false;
                            mpi.Color = Color.Black;

                            if (mpi.Title == "颜色")
                            {
                                mpi.Selected = true;
                                mpi.Color = Color.FromHex("#009999");
                            }
                        }

                        var detail = new NavigationPage((Page)Activator.CreateInstance(typeof(ColorPage)));
                        detail.BarBackgroundColor = Color.FromHex("#D3D3D3");
                        detail.BarTextColor = Color.White;
                        Detail = detail;
                    }
                    break;
                case PageName.DatePage:
                    {
                        foreach (MasterPageItem mpi in masterPage.primaryListView.ItemsSource)
                        {
                            mpi.Selected = false;
                            mpi.Color = Color.Black;

                            if (mpi.Title == "日期计算")
                            {
                                mpi.Selected = true;
                                mpi.Color = Color.FromHex("#009999");
                            }
                        }

                        var detail = new NavigationPage((Page)Activator.CreateInstance(typeof(DatePage)));
                        detail.BarBackgroundColor = Color.FromHex("#D3D3D3");
                        detail.BarTextColor = Color.White;
                        Detail = detail;
                    }
                    break;
                default:
                    break;
            }

            // ListView 点击事件
            masterPage.primaryListView.ItemSelected += MasterPageItemSelected;
            masterPage.secondaryListView.ItemSelected += MasterPageItemSelected;
        }

        private void MasterPageItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;

            if (item != null)
            {
                // 遍历 ListView 数据源，将选中项矩形显示，字体颜色设置成未选中
                foreach (MasterPageItem mpi in masterPage.primaryListView.ItemsSource)
                {
                    mpi.Selected = false;
                    mpi.Color = Color.Black;
                }
                foreach (MasterPageItem mpi in masterPage.secondaryListView.ItemsSource)
                {
                    mpi.Selected = false;
                    mpi.Color = Color.Black;
                }

                // 设置选中项
                item.Selected = true;
                item.Color = Color.FromHex("#009999");

                // 跳转
                var detail = new NavigationPage((Page)Activator.CreateInstance(item.DestPage));
                detail.BarBackgroundColor = Color.FromHex("#D3D3D3");
                detail.BarTextColor = Color.White;
                Detail = detail;              

                // 取消 ListView 默认选中样式
                masterPage.primaryListView.SelectedItem = null;
                masterPage.secondaryListView.SelectedItem = null;

                // 关闭“大纲”
                IsPresented = false;
            }
        }
    }
}
