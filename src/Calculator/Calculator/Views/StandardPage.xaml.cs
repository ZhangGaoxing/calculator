using Calculator.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Calculator.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StandardPage : ContentPage
	{
        private Stack<double> num = new Stack<double>();
        private Stack<string> op = new Stack<string>();

        private bool isEqual = false;
        private bool isOthers = false;

        private List<string> tipList = new List<string>();
        private string numStr = "";
        private string othersStr = "";

        private ObservableCollection<CalculatingHistory> history = new ObservableCollection<CalculatingHistory>();
        private ObservableCollection<MemoryHistory> memory = new ObservableCollection<MemoryHistory>();
        private ObservableCollection<MemoryHistory> reverse = new ObservableCollection<MemoryHistory>();

        public StandardPage ()
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
                    tipList = new List<string>();
                    Tip.Text = "";
                    isOthers = false;
                }
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

            Result.Text = ResultTextSplit(numStr);
            //Result.Text = numStr;
            ResultTextChecked();
        }

        private void Func_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            switch (btn.ClassId)
            {
                case "CE":
                    numStr = "";
                    Result.Text = numStr;
                    break;
                case "C":
                    num = new Stack<double>();
                    op = new Stack<string>();
                    numStr = "";
                    othersStr = "";
                    tipList = new List<string>();
                    Tip.Text = "";
                    Result.Text = "";
                    break;
                case "Backspace":
                    if (numStr.Length > 1)
                    {
                        numStr = numStr.Substring(0, numStr.Length - 1);
                        Result.Text = ResultTextSplit(numStr);
                        //Result.Text = numStr;
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

        private void Sign_Clicked(object sender, EventArgs e)
        {
            if (numStr != "" && numStr != "0")
            {
                if (numStr[0] != '-')
                {
                    numStr = "-" + numStr;

                    Result.Text = ResultTextSplit(numStr);
                    //Result.Text = numStr;
                }
                else
                {
                    numStr = numStr.Substring(1, numStr.Length - 1);

                    Result.Text = ResultTextSplit(numStr);
                    //Result.Text = numStr;
                }
            }

            ResultTextChecked();
        }

        private void Operator_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            othersStr = "";

            if (numStr == "")
            {
                return;
            }

            if (tipList.Count != 0)
            {
                if (num.Count == 0)
                {
                    isOthers = false;

                    num.Push(Convert.ToDouble(numStr));
                    op.Push(btn.Text);

                    tipList.Add($"{op.Peek()} ");

                    Result.Text = "";
                }
                else
                {
                    double temp = 0;

                    switch (op.Pop())
                    {
                        case "+":
                            temp = num.Pop() + Convert.ToDouble(numStr);
                            break;
                        case "-":
                            temp = num.Pop() - Convert.ToDouble(numStr);
                            break;
                        case "×":
                            temp = num.Pop() * Convert.ToDouble(numStr);
                            break;
                        case "÷":
                            temp = num.Pop() / Convert.ToDouble(numStr);
                            break;
                        default:
                            break;
                    }

                    num.Push(temp);
                    op.Push(btn.Text);

                    if (!isOthers)
                    {
                        tipList.Add($"{Convert.ToDouble(numStr)} ");
                        tipList.Add($"{op.Peek()} ");
                    }
                    else
                    {
                        tipList.Add($"{op.Peek()} ");
                        isOthers = false;
                    }

                    Result.Text = ResultTextSplit(temp.ToString());
                    //Result.Text = temp.ToString();
                }
            }
            else
            {
                if (num.Count == 0)
                {
                    isOthers = false;

                    num.Push(Convert.ToDouble(numStr));
                    op.Push(btn.Text);

                    tipList = new List<string>
                    {
                        $"{num.Peek()} ",
                        $"{op.Peek()} "
                    };

                    Result.Text = "";
                }
                else
                {
                    double temp = 0;

                    switch (op.Pop())
                    {
                        case "+":
                            temp = num.Pop() + Convert.ToDouble(numStr);
                            break;
                        case "-":
                            temp = num.Pop() - Convert.ToDouble(numStr);
                            break;
                        case "×":
                            temp = num.Pop() * Convert.ToDouble(numStr);
                            break;
                        case "÷":
                            temp = num.Pop() / Convert.ToDouble(numStr);
                            break;
                        default:
                            break;
                    }

                    num.Push(temp);
                    op.Push(btn.Text);

                    if (!isOthers)
                    {
                        tipList.Add($"{Convert.ToDouble(numStr)} ");
                        tipList.Add($"{op.Peek()} ");
                    }
                    else
                    {
                        tipList.Add($"{op.Peek()} ");
                        isOthers = false;
                    }

                    Result.Text = ResultTextSplit(temp.ToString());
                    //Result.Text = temp.ToString();
                }
            }
            
            numStr = "";
            string tipStr = "";
            foreach (var item in tipList)
            {
                tipStr += item;
            }
            Tip.Text = tipStr;

            ResultTextChecked();
        }

        private void Equal_Clicked(object sender, EventArgs e)
        {
            isEqual = true;

            double temp = 0;
            string tempOp;

            try
            {
                tempOp = op.Pop();

                switch (tempOp)
                {
                    case "+":
                        temp = num.Pop() + Convert.ToDouble(numStr);
                        break;
                    case "-":
                        temp = num.Pop() - Convert.ToDouble(numStr);
                        break;
                    case "×":
                        temp = num.Pop() * Convert.ToDouble(numStr);
                        break;
                    case "÷":
                        temp = num.Pop() / Convert.ToDouble(numStr);
                        break;
                    default:
                        break;
                }

                tipList.Add($"{Convert.ToDouble(numStr)} =");
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
                Result.Text = ResultTextSplit(temp.ToString());
                //Result.Text = temp.ToString();
            }
            catch(InvalidOperationException)
            {
                Result.Text = ResultTextSplit(numStr);
                //Result.Text = numStr;
            }
            catch(Exception ex)
            {
                //Result.Text = ex.Message;
            }

            ResultTextChecked();
        }

        private void OthersFunc_Clicked(object sender, EventArgs e)
        {
            isEqual = true;
            isOthers = true;

            Button btn = sender as Button;

            double temp = 0;

            try
            {
                switch (btn.ClassId)
                {
                    case "Percent":
                        temp = Convert.ToDouble(numStr) / 100;
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"{numStr} %";
                        }
                        else
                        {
                            othersStr = $"({othersStr})%";
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
                    default:
                        break;
                }

                tipList.Add($"{othersStr} ");
                numStr = temp.ToString();
                Result.Text = ResultTextSplit(numStr);
                //Result.Text = temp.ToString();
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

        private void History_Clicked(object sender, EventArgs e)
        {
            History.ItemsSource = history;

            HistoryGrid.IsVisible = !HistoryGrid.IsVisible;
            ButtonGrid.IsVisible = !ButtonGrid.IsVisible;
            if (memory.Count != 0)
            {
                M.IsEnabled = !M.IsEnabled;
            }

            (sender as Button).TextColor = HistoryGrid.IsVisible ? Color.FromHex("#009999"): Color.Black;
        }

        private void History_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            History.SelectedItem = null;
            var item = e.Item as CalculatingHistory;

            Tip.Text = item.Tip;
            //Result.Text = item.Result;
            numStr = item.Result;
            Result.Text = ResultTextSplit(numStr);

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