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
	public partial class DatePage : ContentPage
	{
		public DatePage ()
		{
			InitializeComponent ();
		}

        private void Calculate_Clicked(object sender, EventArgs e)
        {
            int year = Convert.ToInt32(Year.Text);
            int month = Convert.ToInt32(Month.Text);
            int day = Convert.ToInt32(Day.Text);

            DateTime from = From.Date;
            DateTime addYears = from.AddYears(year);
            DateTime addMonths = addYears.AddMonths(month);
            DateTime addDays = addMonths.AddDays(day);

            ResultDt.Text = addDays.ToString("yyyy/MM/dd");
        }

        private void IntervalDt_DateSelected(object sender, DateChangedEventArgs e)
        {
            DateTime from = FromDt.Date;
            DateTime to = ToDt.Date;

            TimeSpan interval = from - to;

            int totalDays = (int)Math.Abs(interval.TotalDays);
            int week = totalDays / 7;
            int modDay = totalDays % 7;

            DayInterval.Text = $"{totalDays} 天";
            WeekInterval.Text = $"{week} 周，{modDay} 天";          
        }
    }
}