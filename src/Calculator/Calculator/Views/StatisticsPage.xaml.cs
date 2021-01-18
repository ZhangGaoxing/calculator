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
	public partial class StatisticsPage : ContentPage
	{
        private string numStr = "";

        private bool isEqual = false;

        private ObservableCollection<double> numData = new ObservableCollection<double>();
        private ObservableCollection<MemoryHistory> memory = new ObservableCollection<MemoryHistory>();
        private ObservableCollection<MemoryHistory> reverse = new ObservableCollection<MemoryHistory>();

        public StatisticsPage ()
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
                isEqual = false;
            }

            if (btn.Text != "0" && btn.Text != ".")
            {
                if (numStr == "0")
                {
                    numStr = btn.Text;
                }
                else
                {
                    numStr += btn.Text;
                }
            }
            else if (btn.Text == "0")
            {
                if (numStr != "0")
                {
                    numStr += "0";
                }
            }
            else if (btn.Text == ".")
            {
                if (!numStr.Contains('.'))
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

        private void OthersFunc_Clicked(object sender, EventArgs e)
        {
            isEqual = true;

            Button btn = sender as Button;

            double temp = 0;
            double sum = 0;
            double ave = 0;

            try
            {
                switch (btn.ClassId)
                {
                    case "Exp":
                        temp = Convert.ToDouble(numStr);
                        numStr = temp + "E+";
                        //Result.Text = numStr;
                        Result.Text = ResultTextSplit(numStr);
                        isEqual = false;
                        break;
                    case "Ave":                        
                        foreach (var item in numData)
                        {
                            sum += item;
                        }
                        temp = sum / numData.Count;
                        numStr = temp.ToString();
                        //Result.Text = numStr;
                        Result.Text = ResultTextSplit(numStr);
                        break;
                    case "SqAve":
                        foreach (var item in numData)
                        {
                            sum += Math.Pow(item, 2);
                        }
                        temp = sum / numData.Count;
                        numStr = temp.ToString();
                        //Result.Text = numStr;
                        Result.Text = ResultTextSplit(numStr);
                        break;
                    case "Sum":
                        foreach (var item in numData)
                        {
                            sum += item;
                        }
                        temp = sum;
                        numStr = temp.ToString();
                        //Result.Text = numStr;
                        Result.Text = ResultTextSplit(numStr);
                        break;
                    case "SqSum":
                        foreach (var item in numData)
                        {
                            sum += Math.Pow(item, 2);
                        }
                        temp = sum;
                        numStr = temp.ToString();
                        //Result.Text = numStr;
                        Result.Text = ResultTextSplit(numStr);
                        break;
                    case "StdDev":
                        foreach (var item in numData)
                        {
                            sum += item;
                        }
                        ave = sum / numData.Count;
                        foreach (var item in numData)
                        {
                            temp += Math.Pow(item - ave, 2);
                        }
                        temp = temp / numData.Count;
                        temp = Math.Sqrt(temp);
                        numStr = temp.ToString();
                        //Result.Text = numStr;
                        Result.Text = ResultTextSplit(numStr);
                        break;
                    case "SampltStdDev":
                        foreach (var item in numData)
                        {
                            sum += item;
                        }
                        ave = sum / numData.Count;
                        foreach (var item in numData)
                        {
                            temp += Math.Pow(item - ave, 2);
                        }
                        temp = temp / (numData.Count - 1.0);
                        temp = Math.Sqrt(temp);
                        numStr = temp.ToString();
                        //Result.Text = numStr;
                        Result.Text = ResultTextSplit(numStr);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                //Result.Text = ex.Message;
            }

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

        private void Opposite_Clicked(object sender, EventArgs e)
        {
            if (numStr.Length == 0)
            {
                return;
            }
            else
            {
                if (numStr[0] == '-')
                {
                    numStr = numStr.Substring(1, numStr.Length - 1);
                }
                else
                {
                    numStr = "-" + numStr;
                }

                //Result.Text = numStr;
                Result.Text = ResultTextSplit(numStr);
            }

            ResultTextChecked();
        }

        private void DataFunc_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            double temp = 0;

            switch (btn.ClassId)
            {
                case "Add":
                    bool isNum = double.TryParse(numStr, out temp);
                    if (!isNum)
                    {
                        return;
                    }
                    else
                    {
                        numData.Add(temp);
                        DataListView.ItemsSource = numData;
                        numStr = "";
                        Result.Text = numStr;
                    }                   
                    break;
                case "Del":
                    if (numData.Count == 0)
                    {
                        return;
                    }
                    else
                    {
                        numData.RemoveAt(numData.Count - 1);
                        DataListView.ItemsSource = numData;
                    }                  
                    break;
                default:
                    break;
            }
        }

        private void DataListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            DataListView.SelectedItem = null;
        }

        private void Func_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            switch (btn.ClassId)
            {
                case "CAD":
                    numData = new ObservableCollection<double>();
                    DataListView.ItemsSource = numData;
                    break;
                case "C":
                    numStr = "";
                    Result.Text = numStr;
                    break;
                case "Backspace":
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
                    break;
                default:
                    break;
            }

            ResultTextChecked();
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
                    MemoryHistory.ItemsSource = reverse;
                    MC.IsEnabled = true;
                    MR.IsEnabled = true;
                    M.IsEnabled = true;
                    break;
                case "MAdd":
                    if (memory.Count == 0)
                    {
                        memory.Add(new MemoryHistory { Memory = result });
                        reverse = new ObservableCollection<MemoryHistory>(memory.Reverse());
                        MemoryHistory.ItemsSource = reverse;
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
                        MemoryHistory.ItemsSource = reverse;
                    }
                    break;
                case "MMinus":
                    if (memory.Count == 0)
                    {
                        memory.Add(new MemoryHistory { Memory = result });
                        reverse = new ObservableCollection<MemoryHistory>(memory.Reverse());
                        MemoryHistory.ItemsSource = reverse;
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
                        MemoryHistory.ItemsSource = reverse;
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
                    MemoryGrid.IsVisible = false;
                    ButtonGrid.IsVisible = true;
                    break;
                case "M":
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

        private void ResultTextChecked()
        {
            if (Result.Text.Length > 24)
            {
                Result.FontSize = 21;
            }
            else
            {
                Result.FontSize = 28;
            }  
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