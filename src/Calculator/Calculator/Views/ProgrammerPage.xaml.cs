using Calculator.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Calculator.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProgrammerPage : ContentPage
	{
        private Stack<long> num = new Stack<long>();
        private Stack<string> op = new Stack<string>();

        private List<string> tipList = new List<string>();

        private bool isEqual = false;
        private bool isOthers = false;
        private string numStr = "";
        private string othersStr = "";
        private long numVal = 0;

        private int leftCount = 0;
        private int rightCount = 0;
        
        private readonly string[] byteLabel = new string[64]
        {
            "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p",
            "q","r","s","t","u","v","w","x","y","z","aa","bb","cc","dd","ee","ff",
            "gg","hh","ii","jj","kk","ll","mm","nn","oo","pp","qq","rr","ss","tt","uu","vv",
            "ww","xx","yy","zz","aaa","bbb","ccc","ddd","eee","fff","ggg","hhh","iii","jjj","kkk","lll",
        };
        private string[] byteStr = new string[64]
        {
            "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0",
            "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0",
            "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0",
            "0","0","0","0","0","0","0","0","0","0","0","0","0","0","0","0",
        };

        private ObservableCollection<CalculatingHistory> history = new ObservableCollection<CalculatingHistory>();
        private ObservableCollection<MemoryHistory> memory = new ObservableCollection<MemoryHistory>();
        private ObservableCollection<MemoryHistory> reverse = new ObservableCollection<MemoryHistory>();

        private SystemSwitch sysFlag = SystemSwitch.Dec;
        private WordSwitch wordFlag = WordSwitch.QWORD;

        public static SystemSwitchStyle hexStyle = new SystemSwitchStyle()
        {
            Background = "#808080",
            IsEnabled = false
        };
        public static SystemSwitchStyle decStyle = new SystemSwitchStyle()
        {
            Background = "#FFFFFF",
            IsEnabled = true
        };
        public static SystemSwitchStyle otcStyle = new SystemSwitchStyle()
        {
            Background = "#FFFFFF",
            IsEnabled = true
        };

        //public static SystemSwitchStyle qwordStyle = new SystemSwitchStyle()
        //{
        //    Background = "#000000",
        //    IsEnabled = true
        //};
        //public static SystemSwitchStyle dwordStyle = new SystemSwitchStyle()
        //{
        //    Background = "#000000",
        //    IsEnabled = true
        //};
        //public static SystemSwitchStyle wordStyle = new SystemSwitchStyle()
        //{
        //    Background = "#000000",
        //    IsEnabled = true
        //};
        //public static SystemSwitchStyle byteStyle = new SystemSwitchStyle()
        //{
        //    Background = "#000000",
        //    IsEnabled = true
        //};

        public ProgrammerPage ()
		{
			InitializeComponent ();
		}

        // 进制切换
        private void SystemSwitch_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            
            if (btn.ClassId == "Hex")
            {
                hexStyle.Background = "#FFFFFF";
                hexStyle.IsEnabled = true;
                decStyle.Background = "#FFFFFF";
                decStyle.IsEnabled = true;
                otcStyle.Background = "#FFFFFF";
                otcStyle.IsEnabled = true;

                Hex.TextColor = Color.FromHex("#009999");
                Dec.TextColor = Color.Black;
                Otc.TextColor = Color.Black;
                Bin.TextColor = Color.Black;

                if (numStr != "")
                {
                    long numVal;
                    try
                    {
                        numVal = Convert.ToInt64(numStr, (int)sysFlag);
                    }
                    catch (ArgumentException)
                    {
                        numVal = -Convert.ToInt64(numStr.Substring(1), (int)sysFlag);
                    }
                    numStr = Convert.ToString(numVal, 16).ToUpper();
                    //Result.Text = numStr;
                    Result.Text = ResultTextSplit(numStr, SystemSwitch.Hex);
                }

                sysFlag = SystemSwitch.Hex;
            }
            else if (btn.ClassId == "Dec")
            {
                hexStyle.Background = "#808080";
                hexStyle.IsEnabled = false;
                decStyle.Background = "#FFFFFF";
                decStyle.IsEnabled = true;
                otcStyle.Background = "#FFFFFF";
                otcStyle.IsEnabled = true;

                Hex.TextColor = Color.Black;
                Dec.TextColor = Color.FromHex("#009999");
                Otc.TextColor = Color.Black;
                Bin.TextColor = Color.Black;

                if (numStr != "")
                {
                    long numVal;
                    try
                    {
                        numVal = Convert.ToInt64(numStr, (int)sysFlag);
                    }
                    catch (ArgumentException)
                    {
                        numVal = -Convert.ToInt64(numStr.Substring(1), (int)sysFlag);
                    }
                    numStr = Convert.ToString(numVal, 10);
                    //Result.Text = numStr;
                    Result.Text = ResultTextSplit(numStr, SystemSwitch.Dec);
                }

                sysFlag = SystemSwitch.Dec;
            }
            else if (btn.ClassId == "Otc")
            {
                hexStyle.Background = "#808080";
                hexStyle.IsEnabled = false;
                decStyle.Background = "#808080";
                decStyle.IsEnabled = false;
                otcStyle.Background = "#FFFFFF";
                otcStyle.IsEnabled = true;

                Hex.TextColor = Color.Black;
                Dec.TextColor = Color.Black;
                Otc.TextColor = Color.FromHex("#009999");
                Bin.TextColor = Color.Black;

                if (numStr != "")
                {
                    long numVal;
                    try
                    {
                        numVal = Convert.ToInt64(numStr, (int)sysFlag);
                    }
                    catch (ArgumentException)
                    {
                        numVal = -Convert.ToInt64(numStr.Substring(1), (int)sysFlag);
                    }
                    numStr = Convert.ToString(numVal, 8);
                    //Result.Text = numStr;
                    Result.Text = ResultTextSplit(numStr, SystemSwitch.Otc);
                }

                sysFlag = SystemSwitch.Otc;
            }
            else
            {
                hexStyle.Background = "#808080";
                hexStyle.IsEnabled = false;
                decStyle.Background = "#808080";
                decStyle.IsEnabled = false;
                otcStyle.Background = "#808080";
                otcStyle.IsEnabled = false;

                Hex.TextColor = Color.Black;
                Dec.TextColor = Color.Black;
                Otc.TextColor = Color.Black;
                Bin.TextColor = Color.FromHex("#009999");

                if (numStr != "")
                {
                    long numVal;
                    try
                    {
                        numVal = Convert.ToInt64(numStr, (int)sysFlag);
                    }
                    catch (ArgumentException)
                    {
                        numVal = -Convert.ToInt64(numStr.Substring(1), (int)sysFlag);
                    }
                    numStr = Convert.ToString(numVal, 2);
                    //Result.Text = numStr;
                    Result.Text = ResultTextSplit(numStr, SystemSwitch.Bin);
                }

                sysFlag = SystemSwitch.Bin;
            }

            ResultTextChecked();
        }

        // 数字点击
        private void Number_Clicked(object sender, EventArgs e)
        {
            if (sysFlag == SystemSwitch.Hex)
            {
                if (numStr.Length == 16)
                {
                    return;
                }
            }
            else if (sysFlag == SystemSwitch.Dec)
            {
                if (numStr.Length == 18)
                {
                    return;
                }
            }
            else if (sysFlag == SystemSwitch.Otc)
            {
                if (numStr.Length == 21)
                {
                    return;
                }
            }
            else
            {
                if (numStr.Length == 64)
                {
                    return;
                }
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

            numVal = Convert.ToInt64(numStr, (int)sysFlag);

            numVal = CutNum(numVal);
            CutShowString(numVal);
            numStr = Convert.ToString(numVal, (int)sysFlag).ToUpper();

            ResultTextChecked();
        }

        private void Func_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            if (btn.ClassId == "CE")
            {
                numStr = "";

                Result.Text = numStr;

                HexResult.Text = "";
                DecResult.Text = "";
                OtcResult.Text = "";
                BinResult.Text = "";
            }
            else if (btn.ClassId == "C")
            {
                num = new Stack<long>();
                op = new Stack<string>();
                numStr = "";
                tipList = new List<string>();
                Tip.Text = "";
                Result.Text = numStr;

                HexResult.Text = "";
                DecResult.Text = "";
                OtcResult.Text = "";
                BinResult.Text = "";
            }
            else
            {
                if (numStr.Length > 1)
                {
                    numStr = numStr.Substring(0, numStr.Length - 1);

                    //Result.Text = numStr;
                    Result.Text = ResultTextSplit(numStr, sysFlag);

                    // 进制转换
                    numVal = Convert.ToInt64(numStr, (int)sysFlag);

                    //HexResult.Text = Convert.ToString(numVal, 16).ToUpper();
                    //DecResult.Text = Convert.ToString(numVal, 10);
                    //OtcResult.Text = Convert.ToString(numVal, 8);
                    //BinResult.Text = Convert.ToString(numVal, 2);

                    HexResult.Text = ResultTextSplit(Convert.ToString(numVal, 16), SystemSwitch.Hex).ToUpper();
                    DecResult.Text = ResultTextSplit(Convert.ToString(numVal, 10), SystemSwitch.Dec);
                    OtcResult.Text = ResultTextSplit(Convert.ToString(numVal, 8), SystemSwitch.Otc);
                    BinResult.Text = ResultTextSplit(Convert.ToString(numVal, 2), SystemSwitch.Bin);
                }
                else
                {
                    numStr = "";

                    Result.Text = numStr;

                    // 进制转换
                    HexResult.Text = "";
                    DecResult.Text = "";
                    OtcResult.Text = "";
                    BinResult.Text = "";
                }
            }

            ResultTextChecked();
        }

        private void OthersFunc_Clicked(object sender, EventArgs e)
        {
            isEqual = true;
            isOthers = true;

            Button btn = sender as Button;

            long temp = 0;

            try
            {
                switch (btn.ClassId)
                {
                    case "Not":
                        numVal = ~numVal;
                        temp = numVal;
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"Not({numStr.ToUpper()})";
                        }
                        else
                        {
                            othersStr = $"Not({othersStr})";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "RoL":
                        numVal = numVal << 1;
                        temp = numVal;
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"RoL({numStr.ToUpper()})";
                        }
                        else
                        {
                            othersStr = $"RoL({othersStr})";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "RoR":
                        numVal = numVal >> 1;
                        temp = numVal;
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"RoR({numStr.ToUpper()})";
                        }
                        else
                        {
                            othersStr = $"RoR({othersStr})";
                            tipList.RemoveAt(tipList.Count - 1);
                        }
                        break;
                    case "Opposite":
                        numVal = -numVal;
                        temp = numVal;
                        if (othersStr.Length == 0)
                        {
                            othersStr = $"-({numStr.ToUpper()})";
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

                temp = CutNum(temp);

                numStr = Convert.ToString(temp, (int)sysFlag).ToUpper();

                CutShowString(temp);

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

        private void CutShowString(long val)
        {
            string hexText = Convert.ToString(val, 16).ToUpper();
            string decText = Convert.ToString(val, 10);
            string otcText = Convert.ToString(val, 8);
            string binText = Convert.ToString(val, 2);

            switch (wordFlag)
            {
                case WordSwitch.QWORD:
                    break;
                case WordSwitch.DWORD:
                    if (hexText.Length > 8)
                    {
                        hexText = hexText.Substring(hexText.Length - 8);
                    }
                    if (otcText.Length > 11)
                    {
                        otcText = otcText.Substring(otcText.Length - 11);
                    }
                    if (binText.Length > 32)
                    {
                        binText = binText.Substring(binText.Length - 32);
                    }
                    break;
                case WordSwitch.WORD:
                    if (hexText.Length > 4)
                    {
                        hexText = hexText.Substring(hexText.Length - 4);
                    }
                    if (otcText.Length > 6)
                    {
                        otcText = otcText.Substring(otcText.Length - 6);
                    }
                    if (binText.Length > 16)
                    {
                        binText = binText.Substring(binText.Length - 16);
                    }
                    break;
                case WordSwitch.BYTE:
                    if (hexText.Length > 2)
                    {
                        hexText = hexText.Substring(hexText.Length - 2);
                    }
                    if (otcText.Length > 3)
                    {
                        otcText = otcText.Substring(otcText.Length - 3);
                    }
                    if (binText.Length > 8)
                    {
                        binText = binText.Substring(binText.Length - 8);
                    }
                    break;
                default:
                    break;
            }

            switch (sysFlag)
            {
                case SystemSwitch.Hex:
                    //Result.Text = hexText;
                    Result.Text = ResultTextSplit(hexText, SystemSwitch.Hex);
                    break;
                case SystemSwitch.Dec:
                    //Result.Text = decText;
                    Result.Text = ResultTextSplit(decText, SystemSwitch.Dec);
                    break;
                case SystemSwitch.Otc:
                    //Result.Text = otcText;
                    Result.Text = ResultTextSplit(otcText, SystemSwitch.Otc);
                    break;
                case SystemSwitch.Bin:
                    //Result.Text = binText;
                    Result.Text = ResultTextSplit(binText, SystemSwitch.Bin);
                    break;
                default:
                    break;
            }

            //HexResult.Text = hexText;
            //DecResult.Text = decText;
            //OtcResult.Text = otcText;
            //BinResult.Text = binText;

            HexResult.Text = ResultTextSplit(hexText, SystemSwitch.Hex);
            DecResult.Text = ResultTextSplit(decText, SystemSwitch.Dec);
            OtcResult.Text = ResultTextSplit(otcText, SystemSwitch.Otc);
            BinResult.Text = ResultTextSplit(binText, SystemSwitch.Bin);
        }

        private void Shift_Clicked(object sender, EventArgs e)
        {
            Lsh.IsVisible = !Lsh.IsVisible;
            RoL.IsVisible = !RoL.IsVisible;
            Rsh.IsVisible = !Rsh.IsVisible;
            RoR.IsVisible = !RoR.IsVisible;

            (sender as Button).TextColor = Lsh.IsVisible ? Color.Black : Color.FromHex("#009999");
        }

        private void Memory_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            string classID = btn.ClassId;
            //bool isNum = double.TryParse(Result.Text, out double result);
            long result = 0;
            try
            {
                result = Convert.ToInt64(ResultTextJoin(Result.Text, sysFlag), (int)sysFlag);
            }
            catch
            {

            }

            switch (classID)
            {
                case "MS":
                    memory.Add(new MemoryHistory { Memory = result });
                    reverse = new ObservableCollection<MemoryHistory>(memory.Reverse());
                    MemoryHistory.ItemsSource = reverse;
                    M.IsEnabled = true;
                    break;
                case "M":
                    reverse = new ObservableCollection<MemoryHistory>(memory.Reverse());
                    MemoryHistory.ItemsSource = reverse;
                    if (MemoryGrid.IsVisible)
                    {
                        MemoryGrid.IsVisible = false;
                        if (NumKeybord.TextColor == Color.Black)
                        {
                            ButtonGrid.IsVisible = false;
                            ByteGrid.IsVisible = true;
                        }
                        else
                        {
                            ButtonGrid.IsVisible = true;
                            ByteGrid.IsVisible = false;
                        }
                    }
                    else
                    {
                        MemoryGrid.IsVisible = true;
                        ButtonGrid.IsVisible = false;
                        ByteGrid.IsVisible = false;
                    }
                    
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

            numStr = Convert.ToString((long)item.Memory, (int)sysFlag).ToUpper();
            //Result.Text = item.Memory.ToString();
            Result.Text = ResultTextSplit(numStr, sysFlag);

            string hexText = Convert.ToString((long)item.Memory, 16).ToUpper();
            string decText = Convert.ToString((long)item.Memory, 10);
            string otcText = Convert.ToString((long)item.Memory, 8);
            string binText = Convert.ToString((long)item.Memory, 2);

            HexResult.Text = ResultTextSplit(hexText, SystemSwitch.Hex);
            DecResult.Text = ResultTextSplit(decText, SystemSwitch.Dec);
            OtcResult.Text = ResultTextSplit(otcText, SystemSwitch.Otc);
            BinResult.Text = ResultTextSplit(binText, SystemSwitch.Bin);

            ResultTextChecked();
        }

        private void WordChange_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            switch (btn.Text)
            {
                case "QWORD":
                    btn.Text = "DWORD";
                    wordFlag = WordSwitch.DWORD;
                    if (numStr != "")
                    {
                        long numVal = Convert.ToInt64(numStr, (int)sysFlag);                        
                        numVal = CutNum(numVal);
                        numStr = Convert.ToString(numVal, (int)sysFlag).ToUpper();
                        CutShowString(numVal);
                    }

                    for (int i = 0; i < 32; i++)
                    {
                        Label l = this.FindByName<Label>(byteLabel[63 - i]);

                        l.Text = "0";
                        l.TextColor = Color.FromHex("#C6C6C6");
                        l.IsEnabled = false;
                    }

                    //qwordStyle.Background = "#C6C6C6";
                    //qwordStyle.IsEnabled = false;
                    //dwordStyle.Background = "#000000";
                    //dwordStyle.IsEnabled = true;
                    //wordStyle.Background = "#000000";
                    //wordStyle.IsEnabled = true;
                    //byteStyle.Background = "#000000";
                    //byteStyle.IsEnabled = true;
                    break;
                case "DWORD":
                    btn.Text = "WORD";
                    wordFlag = WordSwitch.WORD;
                    if (numStr != "")
                    {
                        long numVal = Convert.ToInt64(numStr, (int)sysFlag);                       
                        numVal = CutNum(numVal);
                        numStr = Convert.ToString(numVal, (int)sysFlag).ToUpper();
                        CutShowString(numVal);
                    }

                    for (int i = 0; i < 16; i++)
                    {
                        Label l = this.FindByName<Label>(byteLabel[31 - i]);

                        l.Text = "0";
                        l.TextColor = Color.FromHex("#C6C6C6");
                        l.IsEnabled = false;
                    }

                    //qwordStyle.Background = "#C6C6C6";
                    //qwordStyle.IsEnabled = false;
                    //dwordStyle.Background = "#C6C6C6";
                    //dwordStyle.IsEnabled = false;
                    //wordStyle.Background = "#000000";
                    //wordStyle.IsEnabled = true;
                    //byteStyle.Background = "#000000";
                    //byteStyle.IsEnabled = true;
                    break;
                case "WORD":
                    btn.Text = "BYTE";
                    wordFlag = WordSwitch.BYTE;
                    if (numStr != "")
                    {
                        long numVal = Convert.ToInt64(numStr, (int)sysFlag);                     
                        numVal = CutNum(numVal);
                        numStr = Convert.ToString(numVal, (int)sysFlag).ToUpper();
                        CutShowString(numVal);
                    }

                    for (int i = 0; i < 8; i++)
                    {
                        Label l = this.FindByName<Label>(byteLabel[15 - i]);

                        l.Text = "0";
                        l.TextColor = Color.FromHex("#C6C6C6");
                        l.IsEnabled = false;
                    }

                    //qwordStyle.Background = "#C6C6C6";
                    //qwordStyle.IsEnabled = false;
                    //dwordStyle.Background = "#C6C6C6";
                    //dwordStyle.IsEnabled = false;
                    //wordStyle.Background = "#C6C6C6";
                    //wordStyle.IsEnabled = false;
                    //byteStyle.Background = "#000000";
                    //byteStyle.IsEnabled = true;
                    break;
                case "BYTE":
                    btn.Text = "QWORD";
                    wordFlag = WordSwitch.QWORD;
                    if (numStr != "")
                    {
                        long numVal = Convert.ToInt64(numStr, (int)sysFlag);                       
                        numVal = CutNum(numVal);
                        numStr = Convert.ToString(numVal, (int)sysFlag).ToUpper();
                        CutShowString(numVal);
                    }

                    for (int i = 0; i < 56; i++)
                    {
                        Label l = this.FindByName<Label>(byteLabel[63 - i]);

                        l.Text = "0";
                        l.TextColor = Color.FromHex("#000000");
                        l.IsEnabled = true;
                    }

                    //qwordStyle.Background = "#000000";
                    //qwordStyle.IsEnabled = true;
                    //dwordStyle.Background = "#000000";
                    //dwordStyle.IsEnabled = true;
                    //wordStyle.Background = "#000000";
                    //wordStyle.IsEnabled = true;
                    //byteStyle.Background = "#000000";
                    //byteStyle.IsEnabled = true;
                    break;
                default:
                    break;
            }

            ResultTextChecked();
        }

        private void ResultTextChecked()
        {
            if (Result.Text.Length > 21 && Result.Text.Length <= 26)
            {
                Result.FontSize = 26;
            }
            else if (Result.Text.Length > 26)
            {
                Result.FontSize = 19;
            }           
            else if (Result.Text.Length <= 21)
            {
                Result.FontSize = 32;
            }
        }

        private void Operator_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            // 未输入数字按下操作键
            if (numStr == "" && double.TryParse(ResultTextJoin(Result.Text, sysFlag), out double v) == false) //double.TryParse(Result.Text, out double v) == false
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
                case "Lsh":
                    opStr = "<<";
                    break;
                case "Rsh":
                    opStr = ">>";
                    break;
                case "Or":
                    opStr = "|";
                    break;
                case "Xor":
                    opStr = "^";
                    break;
                case "And":
                    opStr = "&";
                    break;
                case "Mod":
                    opStr = "%";
                    break;
                case "Equal":
                    opStr = "#";
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
            long str2num;
            try
            {
                str2num = Convert.ToInt64(numStr, (int)sysFlag);
                str2num = CutNum(str2num);
                num.Push(str2num);
            }
            catch (ArgumentException ex)
            {
                if (ex.Message == "String cannot contain a minus sign if the base is not 10.")
                {
                    str2num = -Convert.ToInt64(numStr.Substring(1), (int)sysFlag);
                    str2num = CutNum(str2num);
                    num.Push(str2num);
                }
            }
            catch
            {
                
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
            long temp = CycleJudgment(peekOp, opStr);

            temp = CutNum(temp);

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
                // 是=按键
                isEqual = true;

                tipList.Add("= ");
                othersStr = "";
                numStr = Convert.ToString(temp, (int)sysFlag).ToUpper();
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
            numVal = temp;
            CutShowString(temp);

            ResultTextChecked();
        }

        private long CutNum(long val)
        {
            byte[] byteBuff = BitConverter.GetBytes(val);
            long temp = 0;
            switch (wordFlag)
            {
                case WordSwitch.QWORD:
                    temp = val;
                    break;
                case WordSwitch.DWORD:
                    temp = BitConverter.ToInt32(byteBuff, 0);
                    break;
                case WordSwitch.WORD:
                    temp = BitConverter.ToInt16(byteBuff, 0);
                    break;
                case WordSwitch.BYTE:
                    temp = (sbyte)byteBuff[0];
                    break;
                default:
                    break;
            }

            return temp;
        }

        private long CycleJudgment(string peekOp, string opStr)
        {
            long temp = 0;

            long num1 = 0, num2 = 0;
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

            bool isNum = num.TryPeek(out long result);

            if (!isNum)
            {
                result = 0;
            }
            return result;
        }

        private char Precede(char first, char last)
        {
            string operate = "+-×^√%÷<>&^|~()#";
            char[,] level = new char[16, 16]{
                            {'>','>','<','<','<','<','<','>','>','>','>','>','>','<','>','>'},
                            {'>','>','<','<','<','<','<','>','>','>','>','>','>','<','>','>'},
                            {'>','>','>','>','>','>','>','>','>','>','>','>','>','<','>','>'},
                            {'>','>','>','>','>','>','>','>','>','>','>','>','>','<','>','>'},
                            {'>','>','>','>','>','>','>','>','>','>','>','>','>','<','>','>'},
                            {'>','>','>','>','>','>','>','>','>','>','>','>','>','<','>','>'},
                            {'>','>','<','<','<','<','<','>','>','>','>','>','>','<','>','>'},
                            {'<','<','<','<','<','<','<','<','<','<','<','<','<','<','>','>'},
                            {'<','<','<','<','<','<','<','<','<','<','<','<','<','<','>','>'},
                            {'<','<','<','<','<','<','<','<','<','<','<','<','<','<','>','>'},
                            {'<','<','<','<','<','<','<','<','<','<','<','<','<','<','>','>'},
                            {'<','<','<','<','<','<','<','<','<','<','<','<','<','<','>','>'},
                            {'<','<','<','<','<','<','<','<','<','<','<','<','<','<','>','>'},
                            {'<','<','<','<','<','<','<','<','<','<','<','<','<','<','=',' '},
                            {'>','>','>','>','>','>','>','>','>','>','>','>','>',' ','>','>'},
                            {'<','<','<','<','<','<','<','<','<','<','<','<','<','<',' ','='}};

            int rows = operate.IndexOf(first);
            int cols = operate.IndexOf(last);
            return level[rows, cols];
        }

        private long Operate(long num1, string op, long num2)
        {
            long temp = 0;

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
                case "%":
                    temp = num1 % num2;
                    break;
                case "<<":
                    temp = num1 << (int)num2;
                    break;
                case ">>":
                    temp = num1 >> (int)num2;
                    break;
                case "&":
                    temp = num1 & num2;
                    break;
                case "^":
                    temp = num1 ^ num2;
                    break;
                case "|":
                    temp = num1 | num2;
                    break;
                default:
                    break;
            }

            return temp;
        }

        private void ByteTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Label label = sender as Label;

            int id = Convert.ToInt32(label.ClassId);
            string val = label.Text;

            switch (val)
            {
                case "0":
                    label.Text = "1";
                    label.TextColor = Color.FromHex("#009999");
                    byteStr[id] = "1";
                    break;
                case "1":
                    label.Text = "0";
                    label.TextColor = Color.Black;
                    byteStr[id] = "0";
                    break;
                default:
                    break;
            }

            string byteTemp = "";
            for (int i = 0; i < 64; i++)
            {
                Label l = this.FindByName<Label>(byteLabel[i]);
                byteTemp = l.Text + byteTemp;
            }

            numVal = Convert.ToInt64(byteTemp, 2);
            numStr = Convert.ToString(numVal, (int)sysFlag);
            numVal = CutNum(numVal);
            CutShowString(numVal);

            ResultTextChecked();
        }

        private void Keybord_Clicked(object sender, EventArgs e)
        {
            MemoryGrid.IsVisible = false;
            M.TextColor = Color.Black;

            if (!ButtonGrid.IsVisible)
            {
                NumKeybord.TextColor = Color.FromHex("#009999");
                ByteKeybord.TextColor = Color.Black;

                ButtonGrid.IsVisible = true;
                ByteGrid.IsVisible = false;
            }
            else
            {
                long numTemp;
                string binStr;
                try
                {
                    //numTemp = Convert.ToInt64(Result.Text, (int)sysFlag);
                    numTemp = Convert.ToInt64(ResultTextJoin(Result.Text, sysFlag), (int)sysFlag);
                    binStr = Convert.ToString(numTemp, 2);
                }
                catch
                {
                    numTemp = 0;
                    binStr = "0";
                }
                string binTemp = "";
                for (int i = 0; i < 64 - binStr.Length; i++)
                {
                    binTemp += "0";
                }
                binStr = binTemp + binStr;
                for (int i = 0; i < 64; i++)
                {
                    Label l = this.FindByName<Label>(byteLabel[i]);
                    string binByte = binStr[63 - i].ToString();
                    l.Text = binByte;
                    if (binByte == "1")
                    {
                        l.TextColor = Color.FromHex("#009999");
                    }
                    else
                    {
                        l.TextColor = Color.FromHex("#000000");
                    }
                }

                NumKeybord.TextColor = Color.Black;
                ByteKeybord.TextColor = Color.FromHex("#009999");

                ButtonGrid.IsVisible = false;
                ByteGrid.IsVisible = true;

                switch (wordFlag)
                {
                    case WordSwitch.QWORD:
                        
                        break;
                    case WordSwitch.DWORD:
                        for (int i = 0; i < 32; i++)
                        {
                            Label l = this.FindByName<Label>(byteLabel[63 - i]);

                            l.Text = "0";
                            l.TextColor = Color.FromHex("#C6C6C6");
                            l.IsEnabled = false;
                        }
                        break;
                    case WordSwitch.WORD:
                        for (int i = 0; i < 48; i++)
                        {
                            Label l = this.FindByName<Label>(byteLabel[63 - i]);

                            l.Text = "0";
                            l.TextColor = Color.FromHex("#C6C6C6");
                            l.IsEnabled = false;
                        }
                        break;
                    case WordSwitch.BYTE:
                        for (int i = 0; i < 56; i++)
                        {
                            Label l = this.FindByName<Label>(byteLabel[63 - i]);

                            l.Text = "0";
                            l.TextColor = Color.FromHex("#C6C6C6");
                            l.IsEnabled = false;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private string ResultTextSplit(string text, SystemSwitch system)
        {
            string num = text;
            bool isOpposite = false;

            if (text.Contains("-"))
            {
                isOpposite = true;
                text = text.Substring(1);
                num = text;
            }

            List<char> temp = new List<char>();

            int flag = 0;
            for (int i = num.Length - 1; i >= 0; i--)
            {
                temp.Add(num[i]);
                flag++;
                switch (system)
                {
                    case SystemSwitch.Hex:
                        if (flag == 4 && i != 0)
                        {
                            temp.Add(' ');
                            flag = 0;
                        }
                        break;
                    case SystemSwitch.Dec:
                        if (flag == 3 && i != 0)
                        {
                            temp.Add(',');
                            flag = 0;
                        }
                        break;
                    case SystemSwitch.Otc:
                        if (flag == 3 && i != 0)
                        {
                            temp.Add(' ');
                            flag = 0;
                        }
                        break;
                    case SystemSwitch.Bin:
                        if (flag == 4 && i != 0)
                        {
                            temp.Add(' ');
                            flag = 0;
                        }
                        break;
                    default:
                        break;
                }
            }
            temp.Reverse();

            string res = "";
            foreach (var item in temp)
            {
                res += item;
            }

            if (isOpposite)
            {
                res = "-" + res;
            }

            return res;
        }

        private string ResultTextJoin(string text, SystemSwitch system)
        {
            string[] strs;
            var str = "";

            switch (system)
            {
                case SystemSwitch.Hex:
                    strs = text.Split(' ');
                    break;
                case SystemSwitch.Dec:
                    strs = text.Split(',');
                    break;
                case SystemSwitch.Otc:
                    strs = text.Split(' ');
                    break;
                case SystemSwitch.Bin:
                    strs = text.Split(' ');
                    break;
                default:
                    strs = text.Split(',');
                    break;
            }

            foreach (var item in strs)
            {
                str += item;
            }

            return str;
        }
    }
}