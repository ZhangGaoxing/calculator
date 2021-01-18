using Calculator.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Calculator.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SciencePage : ContentPage
	{
        private Stack<double> num = new Stack<double>();
        private Stack<string> op = new Stack<string>();

        private bool isEqual = false;
        private bool isOthers = false;
        private string radianFlag = "DEG";

        private List<string> tipList = new List<string>();
        private string numStr = "";
        private string othersStr = "";

        private int leftCount = 0;
        private int rightCount = 0;

        private bool shiftFlag = false;
        private bool hypFlag = false;

        private ObservableCollection<CalculatingHistory> history = new ObservableCollection<CalculatingHistory>();
        private ObservableCollection<MemoryHistory> memory = new ObservableCollection<MemoryHistory>();
        private ObservableCollection<MemoryHistory> reverse = new ObservableCollection<MemoryHistory>();

        public SciencePage ()
		{
			InitializeComponent ();
		}

        private void Number_Clicked(object sender, EventArgs e)
        {
            if (numStr.Length == 26)
            {
                return;
            }

            Button btn = sender as Button;

            if (isEqual)
            {              
                numStr = "";
                if (isOthers)
                {
                    othersStr = "";
                    
                    isOthers = false;
                }
                isEqual = false;
            }
            
            string btnStr = "";
            if (btn.Text == "π")
            {
                btnStr = Math.PI.ToString();
                numStr = "";
            }
            else if (btn.Text == "e")
            {
                btnStr = Math.E.ToString();
                numStr = "";
            }
            else
            {
                btnStr = btn.Text;
            }

            if (btnStr != "0" && btnStr != ".")
            {
                if (numStr == "0")
                {
                    numStr = btnStr;
                }
                else
                {
                    numStr += btnStr;
                }
            }
            else if (btnStr == "0")
            {
                if (numStr != "0")
                {
                    numStr += "0";
                }
            }
            else if (btnStr == ".")
            {
                if (!numStr.Contains('.') && !numStr.Contains('E'))
                {
                    if (numStr.Length == 0)
                    {
                        numStr = "0.";
                    }
                    else
                    {
                        numStr += ".";
                    }
                }
            }

            //Result.Text = numStr;
            Result.Text = ResultTextSplit(numStr);

            ResultTextChecked();
        }

        private void Func_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            switch (btn.ClassId)
            {
                case "CE":
                    ClearError();
                    break;
                case "C":
                    Clear();
                    break;
                case "Backspace":
                    Backspace();
                    break;
                default:
                    break;
            }

            ResultTextChecked();
        }

        private void Backspace()
        {
            if (numStr.Length > 1)
            {
                numStr = numStr.Substring(0, numStr.Length - 1);
                //Result.Text = numStr;
                Result.Text = ResultTextSplit(numStr);
            }
            else
            {
                numStr = "";
                Result.Text = numStr;
            }
        }

        private void ClearError()
        {
            numStr = "";
            Result.Text = numStr;
        }

        private void Clear()
        {
            num = new Stack<double>();
            op = new Stack<string>();
            numStr = "";
            tipList = new List<string>();
            Tip.Text = "";
            Result.Text = "";
            leftCount = 0;
            rightCount = 0;
        }

        private void History_Clicked(object sender, EventArgs e)
        {
            History.ItemsSource = history;

            HistoryGrid.IsVisible = !HistoryGrid.IsVisible;
            ButtonGrid.IsVisible = !ButtonGrid.IsVisible;
            if (memory.Count != 0)
            {
                M.IsEnabled = !M.IsEnabled;
            }

            (sender as Button).TextColor = HistoryGrid.IsVisible ? Color.FromHex("#009999") : Color.Black;
        }

        private void History_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            History.SelectedItem = null;
            var item = e.Item as CalculatingHistory;

            Tip.Text = item.Tip;
            //Result.Text = item.Result;
            Result.Text = ResultTextSplit(item.Result);
            numStr = item.Result;

            ResultTextChecked();
        }

        private void OthersFunc_Clicked(object sender, EventArgs e)
        {
            isEqual = true;
            isOthers = true;

            Button btn = sender as Button;

            double temp = 0;
            double x = 0;

            try
            {
                switch (btn.ClassId)
                {
                    case "Square":
                        temp = Math.Pow(Convert.ToDouble(numStr), 2);
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"({numStr})²";
                        }
                        else
                        {
                            othersStr = $"({othersStr})²";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "Cube":
                        temp = Math.Pow(Convert.ToDouble(numStr), 3);
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"({numStr})³";
                        }
                        else
                        {
                            othersStr = $"({othersStr})³";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "Sin":                       
                        if (radianFlag == "DEG")
                        {
                            temp = Math.Sin(Convert.ToDouble(numStr) * Math.PI / 180.0);
                        }
                        else
                        {
                            temp = Math.Sin(Convert.ToDouble(numStr));
                        }
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"sin({numStr})";
                        }
                        else
                        {
                            othersStr = $"sin({othersStr})";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "ArcSin":
                        temp = Math.Asin(Convert.ToDouble(numStr));
                        if (radianFlag == "DEG")
                        {
                            temp = temp * 180 / Math.PI;
                        }
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"arcsin({numStr})";
                        }
                        else
                        {
                            othersStr = $"arcsin({othersStr})";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "Sinh":
                        temp = Math.Sinh(Convert.ToDouble(numStr));
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"sinh({numStr})";
                        }
                        else
                        {
                            othersStr = $"sinh({othersStr})";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "ArcSinh":
                        x = Convert.ToDouble(numStr);
                        temp = Math.Log(x + Math.Sqrt(x * x + 1));
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"asinh({numStr})";
                        }
                        else
                        {
                            othersStr = $"asinh({othersStr})";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "Cos":
                        if (radianFlag == "DEG")
                        {
                            temp = Math.Cos(Convert.ToDouble(numStr) * Math.PI / 180.0);
                        }
                        else
                        {
                            temp = Math.Cos(Convert.ToDouble(numStr));
                        }                       
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"cos({numStr})";
                        }
                        else
                        {
                            othersStr = $"cos({othersStr})";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "ArcCos":
                        temp = Math.Acos(Convert.ToDouble(numStr));
                        if (radianFlag == "DEG")
                        {
                            temp = temp * 180 / Math.PI;
                        }
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"arccos({numStr})";
                        }
                        else
                        {
                            othersStr = $"arccos({othersStr})";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "Cosh":
                        temp = Math.Cosh(Convert.ToDouble(numStr));
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"cosh({numStr})";
                        }
                        else
                        {
                            othersStr = $"cosh({othersStr})";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "ArcCosh":
                        x = Convert.ToDouble(numStr);
                        temp = Math.Log(x + Math.Sqrt(x * x - 1));
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"acosh({numStr})";
                        }
                        else
                        {
                            othersStr = $"acosh({othersStr})";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "Tan":
                        if (radianFlag == "DEG")
                        {
                            temp = Math.Tan(Convert.ToDouble(numStr) * Math.PI / 180.0);
                        }
                        else
                        {
                            temp = Math.Tan(Convert.ToDouble(numStr));
                        }                       
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"tan({numStr})";
                        }
                        else
                        {
                            othersStr = $"tan({othersStr})";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "ArcTan":
                        temp = Math.Atan(Convert.ToDouble(numStr));
                        if (radianFlag == "DEG")
                        {
                            temp = temp * 180 / Math.PI;
                        }
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"arctan({numStr})";
                        }
                        else
                        {
                            othersStr = $"arctan({othersStr})";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "Tanh":
                        temp = Math.Tanh(Convert.ToDouble(numStr));
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"tanh({numStr})";
                        }
                        else
                        {
                            othersStr = $"tanh({othersStr})";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "ArcTanh":
                        x = Convert.ToDouble(numStr);
                        temp = Math.Log((1 + x) / (1 - x)) / 2.0;
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"atanh({numStr})";
                        }
                        else
                        {
                            othersStr = $"atanh({othersStr})";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "Sqrt":
                        temp = Math.Sqrt(Convert.ToDouble(numStr));
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"√({numStr})";
                        }
                        else
                        {
                            othersStr = $"√({othersStr})";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "Reciprocal":
                        temp = 1 / Convert.ToDouble(numStr);
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"1/({numStr})";
                        }
                        else
                        {
                            othersStr = $"1/({othersStr})";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "Exponent":
                        temp = Math.Pow(10, Convert.ToDouble(numStr));
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"10^({numStr})";
                        }
                        else
                        {
                            othersStr = $"10^({othersStr})";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "EExponent":
                        temp = Math.Pow(Math.E, Convert.ToDouble(numStr));
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"e^({numStr})";
                        }
                        else
                        {
                            othersStr = $"e^({othersStr})";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "Log":
                        temp = Math.Log10(Convert.ToDouble(numStr));
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"log({numStr})";
                        }
                        else
                        {
                            othersStr = $"log({othersStr})";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "Ln":
                        temp = Math.Log(Math.E, Convert.ToDouble(numStr));
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"ln({numStr})";
                        }
                        else
                        {
                            othersStr = $"ln({othersStr})";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "Opposite":
                        temp = -Convert.ToDouble(numStr);
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"-({numStr})";
                        }
                        else
                        {
                            othersStr = $"-({othersStr})";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "Factorial":
                        uint n = (uint)Convert.ToDouble(numStr);
                        temp = 1;
                        for (int i = 1; i <= n; i++)
                        {
                            temp = temp * i;
                        }
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"({numStr})!";
                        }
                        else
                        {
                            othersStr = $"({othersStr})!";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "Exp":
                        temp = Convert.ToDouble(numStr);
                        numStr = temp + "E+";
                        //Result.Text = numStr;
                        Result.Text = ResultTextSplit(numStr);
                        isEqual = false;
                        isOthers = false;
                        return;
                        break;
                    case "Dms":
                        temp = Convert.ToDouble(numStr);
                        numStr = DEG2DMS(temp);
                        //Result.Text = numStr;
                        Result.Text = ResultTextSplit(numStr);
                        isEqual = true;
                        isOthers = false;
                        return;
                        break;
                    case "Deg":
                        temp = Convert.ToDouble(numStr);
                        numStr = DMS2DEG(temp);
                        //Result.Text = numStr;
                        Result.Text = ResultTextSplit(numStr);
                        isEqual = true;
                        isOthers = false;
                        return;
                        break;
                    default:
                        break;
                }

                tipList.Add($"{othersStr} ");
                numStr = temp.ToString();
                //Result.Text = temp.ToString();
                Result.Text = ResultTextSplit(numStr);
                string tipStr = "";
                foreach (var item in tipList)
                {
                    tipStr += item;
                }
                Tip.Text = tipStr;
            }
            catch (Exception ex)
            {
                //Result.Text = ex.Message;
            }

            ResultTextChecked();
        }

        private void Shift_Clicked(object sender, EventArgs e)
        {
            shiftFlag = !shiftFlag;

            if (shiftFlag)
            {              
                if (hypFlag)
                {
                    Sin.IsVisible = false;
                    Cos.IsVisible = false;
                    Tan.IsVisible = false;

                    ArcSin.IsVisible = false;
                    ArcCos.IsVisible = false;
                    ArcTan.IsVisible = false;

                    Sinh.IsVisible = false;
                    Cosh.IsVisible = false;
                    Tanh.IsVisible = false;

                    ArcSinh.IsVisible = true;
                    ArcCosh.IsVisible = true;
                    ArcTanh.IsVisible = true;
                }
                else
                {
                    Sin.IsVisible = false;
                    Cos.IsVisible = false;
                    Tan.IsVisible = false;

                    ArcSin.IsVisible = true;
                    ArcCos.IsVisible = true;
                    ArcTan.IsVisible = true;

                    Sinh.IsVisible = false;
                    Cosh.IsVisible = false;
                    Tanh.IsVisible = false;

                    ArcSinh.IsVisible = false;
                    ArcCosh.IsVisible = false;
                    ArcTanh.IsVisible = false;
                }
            }
            else
            {
                if (hypFlag)
                {
                    Sin.IsVisible = false;
                    Cos.IsVisible = false;
                    Tan.IsVisible = false;

                    ArcSin.IsVisible = false;
                    ArcCos.IsVisible = false;
                    ArcTan.IsVisible = false;

                    Sinh.IsVisible = true;
                    Cosh.IsVisible = true;
                    Tanh.IsVisible = true;

                    ArcSinh.IsVisible = false;
                    ArcCosh.IsVisible = false;
                    ArcTanh.IsVisible = false;
                }
                else
                {
                    Sin.IsVisible = true;
                    Cos.IsVisible = true;
                    Tan.IsVisible = true;

                    ArcSin.IsVisible = false;
                    ArcCos.IsVisible = false;
                    ArcTan.IsVisible = false;

                    Sinh.IsVisible = false;
                    Cosh.IsVisible = false;
                    Tanh.IsVisible = false;

                    ArcSinh.IsVisible = false;
                    ArcCosh.IsVisible = false;
                    ArcTanh.IsVisible = false;
                }
            }

            Square.IsVisible = !Square.IsVisible;
            Cube.IsVisible = !Cube.IsVisible;
            Pow.IsVisible = !Pow.IsVisible;
            RePow.IsVisible = !RePow.IsVisible;
            //Sin.IsVisible = !Sin.IsVisible;
            //ArcSin.IsVisible = !ArcSin.IsVisible;
            //Cos.IsVisible = !Cos.IsVisible;
            //ArcCos.IsVisible = !ArcCos.IsVisible;
            //Tan.IsVisible = !Tan.IsVisible;
            //ArcTan.IsVisible = !ArcTan.IsVisible;
            Sqrt.IsVisible = !Sqrt.IsVisible;
            Reciprocal.IsVisible = !Reciprocal.IsVisible;
            Exponent.IsVisible = !Exponent.IsVisible;
            EExponent.IsVisible = !EExponent.IsVisible;
            Log.IsVisible = !Log.IsVisible;
            Ln.IsVisible = !Ln.IsVisible;
            Exp.IsVisible = !Exp.IsVisible;
            Dms.IsVisible = !Dms.IsVisible;
            Mod.IsVisible = !Mod.IsVisible;
            Deg.IsVisible = !Deg.IsVisible;
            E.IsVisible = !E.IsVisible;

            (sender as Button).TextColor = Square.IsVisible ? Color.Black : Color.FromHex("#009999");
        }

        private void Memory_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            string classID = btn.ClassId;
            //bool isNum = double.TryParse(Result.Text, out double result);
            bool isNum = double.TryParse(ResultTextJoin(Result.Text), out double result);
            if (!isNum)
            {
                result = 0;
            }

            switch (classID)
            {
                case "MS":
                    memory.Add(new MemoryHistory { Memory = result });
                    reverse = new ObservableCollection<MemoryHistory>(memory.Reverse());
                    MC.IsEnabled = true;
                    MR.IsEnabled = true;
                    M.IsEnabled = true;
                    break;
                case "MAdd":
                    if (memory.Count == 0)
                    {
                        memory.Add(new MemoryHistory { Memory = result });
                        reverse = new ObservableCollection<MemoryHistory>(memory.Reverse());
                        MC.IsEnabled = true;
                        MR.IsEnabled = true;
                        M.IsEnabled = true;
                    }
                    else
                    {
                        MemoryHistory temp = new MemoryHistory
                        {
                            Memory = memory[memory.Count - 1].Memory + result
                        };
                        memory[memory.Count - 1] = temp;
                        reverse = new ObservableCollection<MemoryHistory>(memory.Reverse());
                    }
                    break;
                case "MMinus":
                    if (memory.Count == 0)
                    {
                        memory.Add(new MemoryHistory { Memory = result });
                        reverse = new ObservableCollection<MemoryHistory>(memory.Reverse());
                        MC.IsEnabled = true;
                        MR.IsEnabled = true;
                        M.IsEnabled = true;
                    }
                    else
                    {
                        MemoryHistory temp = new MemoryHistory
                        {
                            Memory = memory[memory.Count - 1].Memory - result
                        };
                        memory[memory.Count - 1] = temp;
                        reverse = new ObservableCollection<MemoryHistory>(memory.Reverse());
                    }
                    break;
                case "MR":
                    numStr = memory[memory.Count - 1].Memory.ToString();
                    //Result.Text = memory[memory.Count - 1].Memory.ToString();
                    Result.Text = ResultTextSplit(memory[memory.Count - 1].Memory.ToString());
                    break;
                case "MC":
                    MC.IsEnabled = false;
                    MR.IsEnabled = false;
                    M.IsEnabled = false;
                    memory = new ObservableCollection<MemoryHistory>();
                    reverse = new ObservableCollection<MemoryHistory>();
                    MemoryHistory.ItemsSource = reverse;
                    HistoryBtn.IsEnabled = true;
                    MemoryGrid.IsVisible = false;
                    ButtonGrid.IsVisible = true;
                    break;
                case "M":
                    HistoryBtn.IsEnabled = !HistoryBtn.IsEnabled;
                    reverse = new ObservableCollection<MemoryHistory>(memory.Reverse());
                    MemoryHistory.ItemsSource = reverse;
                    MemoryGrid.IsVisible = !MemoryGrid.IsVisible;
                    ButtonGrid.IsVisible = !ButtonGrid.IsVisible;
                    (sender as Button).TextColor = MemoryGrid.IsVisible ? Color.FromHex("#009999") : Color.Black;
                    break;
                default:
                    break;
            }

            ResultTextChecked();
        }

        private void MemoryHistory_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            MemoryHistory.SelectedItem = null;
            MemoryHistory item = e.Item as MemoryHistory;

            numStr = item.Memory.ToString();
            //Result.Text = item.Memory.ToString();
            Result.Text = ResultTextSplit(item.Memory.ToString());

            ResultTextChecked();
        }

        private void FE_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (Result.Text.Contains("E"))
                {
                    //double temp = Convert.ToDouble(Result.Text);
                    //Result.Text = temp.ToString();
                    double temp = Convert.ToDouble(ResultTextJoin(Result.Text));
                    numStr = temp.ToString();
                    Result.Text = ResultTextSplit(numStr);
                }
                else
                {
                    //bool isNum = double.TryParse(Result.Text, out double temp);
                    bool isNum = double.TryParse(ResultTextJoin(Result.Text), out double temp);

                    if (!isNum)
                    {
                        return;
                    }

                    Regex reg = new Regex(@"^\d+\.\d+$");
                    if (reg.IsMatch(Math.Abs(temp).ToString()))
                    {
                        if (temp > 0)
                        {
                            if (temp > 1)
                            {
                                Result.Text = temp.ToString($"E{Result.Text.Length - 2}");
                            }
                            else
                            {
                                Result.Text = temp.ToString($"E{Result.Text.Length - 3}");
                            }
                        }
                        else
                        {
                            if (temp < -1)
                            {
                                Result.Text = temp.ToString($"E{Result.Text.Length - 3}");
                            }
                            else
                            {
                                Result.Text = temp.ToString($"E{Result.Text.Length - 4}");
                            }

                        }
                    }
                    else
                    {
                        if (temp >= 0)
                        {
                            Result.Text = temp.ToString($"E{Result.Text.Length - 1}");
                        }
                        else
                        {
                            Result.Text = temp.ToString($"E{Result.Text.Length - 2}");
                        }
                    }

                    numStr = temp.ToString();
                }

                ResultTextChecked();
            }
            catch
            {
            }
        }

        private void Radian_Clicked(object sender, EventArgs e)
        {
            if (Radian.Text == "DEG")
            {
                radianFlag = "RAD";
                Radian.Text = "RAD";
            }
            else
            {
                radianFlag = "DEG";
                Radian.Text = "DEG";
            }
        }

        private void HYP_Clicked(object sender, EventArgs e)
        {
            hypFlag = !hypFlag;

            if (hypFlag)
            {
                HYP.TextColor = Color.FromHex("#009999");

                if (shiftFlag)
                {
                    Sin.IsVisible = false;
                    Cos.IsVisible = false;
                    Tan.IsVisible = false;

                    ArcSin.IsVisible = false;
                    ArcCos.IsVisible = false;
                    ArcTan.IsVisible = false;

                    Sinh.IsVisible = false;
                    Cosh.IsVisible = false;
                    Tanh.IsVisible = false;

                    ArcSinh.IsVisible = true;
                    ArcCosh.IsVisible = true;
                    ArcTanh.IsVisible = true;
                }
                else
                {
                    Sin.IsVisible = false;
                    Cos.IsVisible = false;
                    Tan.IsVisible = false;

                    ArcSin.IsVisible = false;
                    ArcCos.IsVisible = false;
                    ArcTan.IsVisible = false;

                    Sinh.IsVisible = true;
                    Cosh.IsVisible = true;
                    Tanh.IsVisible = true;

                    ArcSinh.IsVisible = false;
                    ArcCosh.IsVisible = false;
                    ArcTanh.IsVisible = false;
                }
            }
            else
            {
                HYP.TextColor = Color.Black;

                if (shiftFlag)
                {
                    Sin.IsVisible = false;
                    Cos.IsVisible = false;
                    Tan.IsVisible = false;

                    ArcSin.IsVisible = true;
                    ArcCos.IsVisible = true;
                    ArcTan.IsVisible = true;

                    Sinh.IsVisible = false;
                    Cosh.IsVisible = false;
                    Tanh.IsVisible = false;

                    ArcSinh.IsVisible = false;
                    ArcCosh.IsVisible = false;
                    ArcTanh.IsVisible = false;
                }
                else
                {
                    Sin.IsVisible = true;
                    Cos.IsVisible = true;
                    Tan.IsVisible = true;

                    ArcSin.IsVisible = false;
                    ArcCos.IsVisible = false;
                    ArcTan.IsVisible = false;

                    Sinh.IsVisible = false;
                    Cosh.IsVisible = false;
                    Tanh.IsVisible = false;

                    ArcSinh.IsVisible = false;
                    ArcCosh.IsVisible = false;
                    ArcTanh.IsVisible = false;
                }
            }
        }

        public static string DEG2DMS(double d)
        {
            int Degree = Convert.ToInt16(Math.Truncate(d));//度

            d = d - Degree;
            int M = Convert.ToInt16(Math.Truncate((d) * 60));//分

            int S = Convert.ToInt16(Math.Round((d * 60 - M) * 60));

            if (S == 60)
            {

                M = M + 1;
                S = 0;
            }
            if (M == 60)
            {

                M = 0;
                Degree = Degree + 1;
            }
            string rstr = Degree.ToString() + ".";
            if (M < 10)
            {
                rstr = rstr + "0" + M.ToString();
            }
            else
            {
                rstr = rstr + M.ToString();
            }

            if (S < 10)
            {
                rstr = rstr + "0" + S.ToString();
            }
            else
            {
                rstr = rstr + S.ToString();
            }
            return rstr;
        }

        public string DMS2DEG(double DMS)
        {
            return (DMS / 0.6).ToString();
        }

        private void ResultTextChecked()
        {
            if (Result.Text.Length > 15 && Result.Text.Length <= 21)
            {
                Result.FontSize = 33;
            }
            else if (Result.Text.Length > 21)
            {
                Result.FontSize = 21;
            }
            else if (Result.Text.Length <= 15)
            {
                Result.FontSize = 44;
            }
        }

        private void Operator_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            // 未输入数字按下操作键
            if (numStr == "" && double.TryParse(ResultTextJoin(Result.Text), out double v) == false) //double.TryParse(Result.Text, out double v) == false
            {
                if (btn.Text != "(")
                {
                    return;
                }
            }
            // 判断括号个数
            if (btn.Text == ")")
            {
                string tipTemp = "";
                foreach (var item in tipList)
                {
                    tipTemp += item;
                }
                leftCount = tipTemp.Count(x => x == '(');
                rightCount = tipTemp.Count(x => x == ')');

                if (leftCount == rightCount)
                {
                    return;
                }
            }
            // 操作栈底置#
            if (op.Count == 0)
            {
                op.Push("#");
            }           

            othersStr = "";
            // 操作符转换
            string opStr = "";
            switch (btn.ClassId)
            {
                case "Pow":
                    opStr = "^";
                    break;
                case "RePow":
                    opStr = "√";
                    break;
                case "Mod":
                    opStr = "%";
                    break;
                case "Equal":
                    opStr = "#";
                    leftCount = 0;
                    rightCount = 0;
                    break;
                default:
                    opStr = btn.Text;
                    break;
            }
            // 当输入完右括号直接输入左括号时
            try
            {
                if (tipList.Last() == ") " && opStr == "(")
                {
                    return;
                }
            }
            catch
            {

            }
            // 操作数入栈
            bool isNum = double.TryParse(numStr, out double numStrC);
            if (isNum)
            {
                num.Push(numStrC);
            }
            // 计算
            string peekOp = op.Peek();
            if (op.Count == 1 && opStr == "(" && num.Count == 1)
            {
                op.Push("×");
                peekOp = "×";
                tipList.Add($"{numStr} ");
                tipList.Add($"{peekOp} ");
                isOthers = true;
            }
            double temp = CycleJudgment(peekOp, opStr);

            if (!isOthers)
            {
                tipList.Add($"{numStr} ");
            }
            isOthers = false;
            if (opStr != "#")
            {
                // 不是=按键
                tipList.Add($"{opStr} ");

                numStr = "";
                string tipStr = "";
                foreach (var item in tipList)
                {
                    tipStr += item;
                }
                Tip.Text = tipStr;
            }
            else
            {
                // 结束运算
                isEqual = true;

                tipList.Add("= ");
                othersStr = "";
                numStr = temp.ToString();
                string tipStr = "";
                foreach (var item in tipList)
                {
                    tipStr += item;
                }
                history.Add(new CalculatingHistory
                {
                    Tip = tipStr,
                    Result = temp.ToString()
                });
                tipList = new List<string>();
                Tip.Text = "";
            }

            //Result.Text = temp.ToString();
            Result.Text = ResultTextSplit(temp.ToString());

            ResultTextChecked();
        }

        /// <summary>
        /// 循环判断计算
        /// </summary>
        private double CycleJudgment(string peekOp, string opStr)
        {
            double temp = 0;

            double num1 = 0, num2 = 0;
            try
            {
                switch (Precede(peekOp[0], opStr[0]))
                {
                    case '<':
                        op.Push(opStr);
                        break;
                    case '=':
                        op.Pop();
                        break;
                    case '>':
                        num2 = num.Pop();
                        num1 = num.Pop();
                        temp = Operate(num1, op.Pop(), num2);
                        num.Push(temp);

                        temp = CycleJudgment(op.Peek(), opStr);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
            }

            bool isNum = num.TryPeek(out double result);

            if (!isNum)
            {
                result = 0;
            }
            return result;
        }

        /// <summary>
        /// 操作符优先级
        /// </summary>
        private char Precede(char first, char last)
        {
            string operate = "+-×^√%÷()#";
            char[,] level = new char[10, 10]{
                            {'>','>','<','<','<','<','<','<','>','>'},
                            {'>','>','<','<','<','<','<','<','>','>'},
                            {'>','>','>','>','>','>','>','<','>','>'},
                            {'>','>','>','>','>','>','>','<','>','>'},
                            {'>','>','>','>','>','>','>','<','>','>'},
                            {'>','>','>','>','>','>','>','<','>','>'},
                            {'>','>','<','<','<','<','<','<','>','>'},
                            {'<','<','<','<','<','<','<','<','=',' '},
                            {'>','>','>','>','>','>','>',' ','>','>'},
                            {'<','<','<','<','<','<','<','<',' ','='}};

            int rows = operate.IndexOf(first);
            int cols = operate.IndexOf(last);
            return level[rows, cols];
        }

        /// <summary>
        /// 计算
        /// </summary>
        private double Operate(double num1, string op, double num2)
        {
            double temp = 0;

            switch (op)
            {
                case "+":
                    temp = num1 + num2;
                    break;
                case "-":
                    temp = num1 - num2;
                    break;
                case "×":
                    temp = num1 * num2;
                    break;
                case "÷":
                    temp = num1 / num2;
                    break;
                case "^":
                    temp = Math.Pow(num1, num2);
                    break;
                case "√":
                    temp = Math.Pow(num1, 1 / num2);
                    break;
                case "%":
                    temp = num1 % num2;
                    break; 
                default:
                    break;
            }

            return temp;
        }

        private string ResultTextSplit(string text)
        {
            string leftNum = text;
            string rightNum = "";
            bool isOpposite = false;

            if (text.Contains("E"))
            {
                return text;
            }

            if (text.Contains("-"))
            {
                isOpposite = true;
                text = text.Substring(1);
                leftNum = text;
            }

            if (text.Contains("."))
            {
                leftNum = text.Split('.')[0];
                try
                {
                    rightNum = "." + text.Split('.')[1];
                }
                catch
                {
                    rightNum = ".";
                }
            }

            List<char> temp = new List<char>();

            int flag = 0;
            for (int i = leftNum.Length - 1; i >= 0; i--)
            {
                temp.Add(leftNum[i]);
                flag++;
                if (flag == 3 && i != 0)
                {
                    temp.Add(',');
                    flag = 0;
                }
            }
            temp.Reverse();

            string res = "";
            foreach (var item in temp)
            {
                res += item;
            }
            res += rightNum;

            if (isOpposite)
            {
                res = "-" + res;
            }

            return res;
        }

        private string ResultTextJoin(string text)
        {
            var strs = text.Split(',');
            var str = "";

            foreach (var item in strs)
            {
                str += item;
            }

            return str;
        }
    }
}