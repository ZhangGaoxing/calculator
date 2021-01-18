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
	public partial class ColorPage : ContentPage
	{
		public ColorPage ()
		{
			InitializeComponent ();
		}

        private void HexConvert_Clicked(object sender, EventArgs e)
        {
            try
            {
                int red = Convert.ToInt32(RedVal.Text);
                int green = Convert.ToInt32(GreenVal.Text);
                int blue = Convert.ToInt32(BlueVal.Text);

                string rH = Convert.ToString(red, 16);
                if (rH.Length == 1)
                {
                    rH = "0" + rH;
                }
                string gH = Convert.ToString(green, 16);
                if (gH.Length == 1)
                {
                    gH = "0" + gH;
                }
                string bH = Convert.ToString(blue, 16);
                if (bH.Length == 1)
                {
                    bH = "0" + bH;
                }

                string hex = "#" + rH + gH + bH;
                HexResult.Text = hex.ToUpper();
                ToHexColor.BackgroundColor = Color.FromRgb(red, green, blue);
            }
            catch
            {
                HexResult.Text = "请输入正确的颜色值";
            }
        }

        private void RGBConvert_Clicked(object sender, EventArgs e)
        {
            try
            {
                string hex = HexVal.Text;

                string red = hex.Substring(1, 2);
                string green = hex.Substring(3, 2);
                string blue = hex.Substring(5, 2);

                RGBResult.Text = Convert.ToInt32(red, 16) + "," + Convert.ToInt32(green, 16) + "," + Convert.ToInt32(blue, 16);
                ToRGBColor.BackgroundColor = Color.FromHex(hex);
            }
            catch
            {
                RGBResult.Text = "请输入正确的颜色值";
            }
        }
    }
}