using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            btnStandard.Background = new SolidColorBrush(Color.FromArgb(100, 0, 128, 0));
        }

        #region NumberButtonProcess
        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            int pressedNumber = Convert.ToInt32((sender as Button).Name.Substring(9));
            appropriateNumOnDisplay(pressedNumber);
            (sender as Button).Focusable = false;
        }

        public void appropriateNumOnDisplay(int num)
        {
            if (lblOperation.Content.ToString() == "")
            {
                lblOperation.Content = num.ToString();
            }
            else if(lblOperation.Content.ToString().Length<17)
            {
                lblOperation.Content = $"{lblOperation.Content.ToString()}{num.ToString()}";
            }
        }
        #endregion NumberButtonProcess
        #region otherButtonAndOperatorProcess
        public void appropriateOperatorOnDisplay(string num)
        {
           

            if(lblResult.Content.ToString()!="0")
            {
                lblOperation.Content = lblResult.Content;
                lblResult.Content = "0";
            }

            string content = lblOperation.Content.ToString();
            char lastChar;
            if (content!="")
            {
                if (content.Length == 1 && (num == "/" || num == "x"))
                    return;

                lastChar = content[content.Length - 1];
                if (lastChar == '+' || lastChar == '-' || lastChar == 'x' || lastChar == '/') { lblOperation.Content = content.Substring(0, content.Length - 1); }
            }else if(num=="/" || num=="x")
            {
                return;
            }        
                       

            if (lblOperation.Content.ToString() == "")
            {
                lblOperation.Content = num.ToString();
            }
            else
            {
                lblOperation.Content = $"{lblOperation.Content.ToString()}{num}";
            }
        }

        private void btnCE_Click(object sender, RoutedEventArgs e)
        {
            if (lblOperation.Content.ToString().Length == 0)
                return;

            lblOperation.Content = lblOperation.Content.ToString().Substring(0, lblOperation.Content.ToString().Length - 1);                

        }

        private void btnAC_Click(object sender, RoutedEventArgs e)
        {
            lblResult.Content = "0";
            lblOperation.Content = "";
        }

        //Need to improve this method to know that already decimal is placed or not in the last entry no
        private void btnDecimalPoint_Click(object sender, RoutedEventArgs e)
        {
            DecimalPoint_Insert();
        }

        private void DecimalPoint_Insert()
        {
            string firstPart = lblOperation.Content.ToString();
            int indexOfLastOperator = firstPart.LastIndexOf('+');
            if (indexOfLastOperator < firstPart.LastIndexOf('-'))
            {
                indexOfLastOperator = firstPart.LastIndexOf('-');
            }
            if (indexOfLastOperator < firstPart.LastIndexOf('x'))
            {
                indexOfLastOperator = firstPart.LastIndexOf('x');
            }
            if (indexOfLastOperator < firstPart.LastIndexOf('/'))
            {
                indexOfLastOperator = firstPart.LastIndexOf('/');
            }

            if (indexOfLastOperator == -1)
                indexOfLastOperator = 0;

            string checkContent = firstPart.Substring(indexOfLastOperator);

            if (checkContent.Contains(".") == false)
            {
                lblOperation.Content = $"{lblOperation.Content.ToString()}.";
            }
        }

        private void btnDevide_Click(object sender, RoutedEventArgs e)
        {
            appropriateOperatorOnDisplay("/");
        }

        private void btnMultiplication_Click(object sender, RoutedEventArgs e)
        {
            appropriateOperatorOnDisplay("x");
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            appropriateOperatorOnDisplay("-");
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            appropriateOperatorOnDisplay("+");
        }
        #endregion otherButtonAndOperatorProcess
        #region equationSolveAndEqualProcess
        private void btnEqual_Click(object sender, RoutedEventArgs e)
        {
            EqualProcess();
        }
        private void EqualProcess()
        {
            double fontSizeBackup = lblResult.FontSize;

            string equationToevaluate = lblOperation.Content.ToString();
            if(equationToevaluate=="")
            {
                lblResult.Content = "0";
                return;
            }

            if (equationToevaluate[equationToevaluate.Length - 1] == '+'
                || equationToevaluate[equationToevaluate.Length - 1] == '-' ||
                equationToevaluate[equationToevaluate.Length - 1] == 'x' ||
                equationToevaluate[equationToevaluate.Length - 1] == '/')
            {
                equationToevaluate = equationToevaluate.Substring(0, equationToevaluate.Length - 1);
                lblOperation.Content = equationToevaluate;
            }

            if (equationToevaluate[0] == '+'
                || equationToevaluate[0] == '-'
                )
            {
                equationToevaluate = "0" + equationToevaluate;
            }
            else if (equationToevaluate[0] == 'x'
               || equationToevaluate[0] == '/')
            {
                lblResult.Content = "Error";
                return;
            }
            //First devide
            //Then Multiplication
            //Then addition
            //Then Subtraction

            //Get the operands to devide  and take their initial position and end position on string
            while (equationToevaluate.Contains("/"))
            {
                equationToevaluate = processOperator("/", equationToevaluate);
                if (equationToevaluate.Contains("∞"))
                {
                    lblResult.Content = "Error";
                    return;
                }

            }
            while (equationToevaluate.Contains("x"))
            {
                equationToevaluate = processOperator("x", equationToevaluate);
                if (equationToevaluate.Contains("∞"))
                {
                    lblResult.Content = "Error";
                    return;
                }

            }
            while (equationToevaluate.Contains("+"))
            {
                equationToevaluate = processOperator("+", equationToevaluate);
                if (equationToevaluate.Contains("∞"))
                {
                    lblResult.Content = "Error";
                    return;
                }

            }
            while (equationToevaluate.Contains("-") && equationToevaluate.StartsWith("-") == false)
            {
                equationToevaluate = processOperator("-", equationToevaluate);
                if (equationToevaluate.Contains("∞"))
                {
                    lblResult.Content = "Error";
                    return;
                }

            }

            //If still minus sign on the string
            if (equationToevaluate.Contains("-"))
            {
                string[] numbers = equationToevaluate.Split('-');
                double sum = 0;
                foreach (string ch in numbers)
                {
                    double result;
                    if (double.TryParse(ch, out result)) { sum = sum + result; }

                }
                equationToevaluate = $"-{sum}";
            }

            if (equationToevaluate.Length > 17) { lblResult.FontSize = lblResult.FontSize / 2; lblResult.Content = "Out of Memory"; return; }

            lblResult.FontSize = fontSizeBackup;
            lblResult.Content = equationToevaluate;
        }

        private string processOperator(string operand,string equation) 
        {
           if(equation.Contains("E"))
            {
                lblResult.FontSize = lblResult.FontSize / 2; lblResult.Content = "Out of Memory";
                return "Out of Memory";
            }

            int startIndex = equation.IndexOf(operand);
            string firstPart = equation.Substring(0, startIndex);
            string secondPart = equation.Substring(startIndex + 1);

            double firsOperand;

            int indexOfLastOperator = firstPart.LastIndexOf('+');
            if (indexOfLastOperator < firstPart.LastIndexOf('-'))
            {
                indexOfLastOperator = firstPart.LastIndexOf('-');
            }
            if (indexOfLastOperator < firstPart.LastIndexOf('x'))
            {
                indexOfLastOperator = firstPart.LastIndexOf('x');
            }
            if (indexOfLastOperator < firstPart.LastIndexOf('/'))
            {
                indexOfLastOperator = firstPart.LastIndexOf('/');
            }



            firsOperand = Convert.ToDouble(firstPart.Substring(indexOfLastOperator + 1)); 

            double secondOperand;

            int indexOfFirstOperator = secondPart.IndexOf('+');
            if ((indexOfFirstOperator > secondPart.IndexOf('-') && secondPart.Contains('-')) || indexOfFirstOperator == -1)
            {
                indexOfFirstOperator = secondPart.IndexOf('-');
            }
            if ((indexOfFirstOperator > secondPart.IndexOf('x') && secondPart.Contains('x')) || indexOfFirstOperator == -1)
            {
                indexOfFirstOperator = secondPart.IndexOf('x');
            }
            if ((indexOfFirstOperator > secondPart.IndexOf('/') && secondPart.Contains('/')) || indexOfFirstOperator == -1)
            {
                indexOfFirstOperator = secondPart.IndexOf('/');
            }

            if (indexOfFirstOperator == -1)
            {
                indexOfFirstOperator = secondPart.Length;
            }

            secondOperand = Convert.ToDouble(secondPart.Substring(0, indexOfFirstOperator));

            //need to show error here if divide by 0
            double result=0;

            try
            {
                switch(operand)
                {
                    case "+":
                        result = (double)firsOperand + (double)secondOperand;
                        break;
                    case "-":
                        result = (double)firsOperand - (double)secondOperand;
                        break;
                    case "x":
                        result = (double)firsOperand * (double)secondOperand;
                        break;
                    case "/":
                        result = (double)firsOperand / (double)secondOperand;
                        break;
                }

            }
            catch
            {
                lblResult.Content = "Error";
                return "Error";
            }

            string returnString="";

            if (indexOfLastOperator == -1)
                returnString= $"{result}{secondPart.Substring(indexOfFirstOperator)}";
            else
                returnString = $"{firstPart.Substring(0, indexOfLastOperator+1)}{result}{secondPart.Substring(indexOfFirstOperator)}";

            return returnString;


        }

        public static decimal TruncateToDecimalPlace(decimal numberToTruncate, int decimalPlaces)
        {
            decimal power = (decimal)(Math.Pow(10.0, (double)decimalPlaces));

            return Math.Truncate((power * numberToTruncate)) / power;
        }

        #endregion equationSolveAndEqualProcess

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                Application.Current.MainWindow.DragMove();
        }

        private void btnPercent_Click(object sender, RoutedEventArgs e)
        {
            string lblOperationString = lblOperation.Content.ToString();
            if (lblOperation.Content.ToString()!=""
                && lblOperation.Content.ToString()[lblOperation.Content.ToString().Length-1] != '+'
                && lblOperation.Content.ToString()[lblOperation.Content.ToString().Length - 1] != '-'
                && lblOperation.Content.ToString()[lblOperation.Content.ToString().Length - 1] != '/'
                && lblOperation.Content.ToString()[lblOperation.Content.ToString().Length - 1] != 'x'
                && lblOperation.Content.ToString().Contains("%")==false)
            {
                int lastOperatorIndex = lblOperationString.LastIndexOf('+');
                if(lastOperatorIndex< lblOperationString.LastIndexOf('-'))
                {
                    lastOperatorIndex = lblOperationString.LastIndexOf('-');
                }
                if (lastOperatorIndex < lblOperationString.LastIndexOf('x'))
                {
                    lastOperatorIndex = lblOperationString.LastIndexOf('x');
                }
                if (lastOperatorIndex < lblOperationString.LastIndexOf('/'))
                {
                    lastOperatorIndex = lblOperationString.LastIndexOf('/');
                }

                double lastNumber;
                if(lastOperatorIndex!=-1)
                {
                    double.TryParse( lblOperationString.Substring(lastOperatorIndex + 1), out lastNumber);
                    lblOperation.Content = $"{lblOperationString.Substring(0, lastOperatorIndex+1)}{lastNumber/100}";
                }else
                {
                    double.TryParse(lblOperationString.Substring(0), out lastNumber);
                    lblOperation.Content = $"{lblOperationString.Substring(0, lastOperatorIndex + 1)}{lastNumber / 100}";
                }

            }

            
            
        }
        #region headerButtons

        private void btnTopMost_Click(object sender, RoutedEventArgs e)
        {
            if(defaultWindow.Topmost == false)
            {
                defaultWindow.Topmost = true;
                btnTopMost.Background = new SolidColorBrush(Color.FromArgb(100, 0, 128, 0));
            }else
            {
                defaultWindow.Topmost = false;
                btnTopMost.Background = lblTitle.Background;
            }
            
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            defaultWindow.WindowState=WindowState.Minimized;
        }

        private void btnStandard_Click(object sender, RoutedEventArgs e)
        {   
            btnStandard.Background= new SolidColorBrush(Color.FromArgb(100, 0, 128, 0));
            btnSmall.Background = lblTitle.Background;

            Style numberButtonStyle = this.FindResource("NumberButtonStyle") as Style;
            Style operatorButtonStyle = this.FindResource("OperatorButtonStyle") as Style;
            Style otherButtonStyle = this.FindResource("OtherButtonStyle") as Style;

            //applying appropriate style for number button
            foreach (Control contrl in gridButtons.Children)
            {
                Button calcButton = contrl as Button;
                if (calcButton != null && calcButton.Name.Contains("Number"))
                {
                    calcButton.Style = numberButtonStyle;
                }
                else if (calcButton != null && calcButton.Name.Contains("Operator"))
                {
                    calcButton.Style = operatorButtonStyle;
                }
                else if (calcButton != null && calcButton.Name.Contains("Other"))
                {
                    calcButton.Style = otherButtonStyle;
                }

            }
            defaultWindow.Height = 450; defaultWindow.Width = 400;
            lblOperation.FontSize = 40;
            lblResult.FontSize = 40;
        }

        private void btnSmall_Click(object sender, RoutedEventArgs e)
        {
            btnStandard.Background = lblTitle.Background;
            btnSmall.Background = new SolidColorBrush(Color.FromArgb(100, 0, 128, 0));

            defaultWindow.Height = 300; defaultWindow.Width = 266;

            Style numberButtonStyle = this.FindResource("smallWidowNumberButton") as Style;
            Style operatorButtonStyle = this.FindResource("smallWidowOperatorButton") as Style;
            Style otherButtonStyle= this.FindResource("smallWidowOtherButton") as Style;

            //applying appropriate style for number button
            foreach (Control contrl in gridButtons.Children)
            {
                Button calcButton = contrl as Button;
                if(calcButton!=null && calcButton.Name.Contains("Number"))
                {
                    calcButton.Style = numberButtonStyle;
                }else if(calcButton != null && calcButton.Name.Contains("Operator"))
                {
                    calcButton.Style = operatorButtonStyle;
                }
                else if (calcButton != null && calcButton.Name.Contains("Other"))
                {
                    calcButton.Style = otherButtonStyle;
                }
                
            }

            lblOperation.FontSize = 27;
            lblResult.FontSize = 27;

        }
        #endregion headerButtons

        private void btnNumber_KeyDown(object sender, KeyEventArgs e)
        {
            //int pressedNumber = Convert.ToInt32((sender as Button).Name.Substring(9));
            //appropriateNumOnDisplay(pressedNumber);
            //int PressedNumber = Convert.ToInt32(e.Key.ToString().Substring(6));
            if(this.defaultWindow.IsActive==true)
            {
                switch (e.Key)
                {
                    case (Key.NumPad0):
                        appropriateNumOnDisplay(0);
                        break;
                    case (Key.NumPad1):
                        appropriateNumOnDisplay(1);
                        break;
                    case (Key.NumPad2):
                        appropriateNumOnDisplay(2);
                        break;
                    case (Key.NumPad3):
                        appropriateNumOnDisplay(3);
                        break;
                    case (Key.NumPad4):
                        appropriateNumOnDisplay(4);
                        break;
                    case (Key.NumPad5):
                        appropriateNumOnDisplay(5);
                        break;
                    case (Key.NumPad6):
                        appropriateNumOnDisplay(6);
                        break;
                    case (Key.NumPad7):
                        appropriateNumOnDisplay(7);
                        break;
                    case (Key.NumPad8):
                        appropriateNumOnDisplay(8);
                        break;
                    case (Key.NumPad9):
                        appropriateNumOnDisplay(9);
                        break;
                    case (Key.Add):
                        appropriateOperatorOnDisplay("+");
                        break;
                    case (Key.Subtract):
                        appropriateOperatorOnDisplay("-");
                        break;
                    case (Key.Multiply):
                        appropriateOperatorOnDisplay("x");
                        break;
                    case (Key.Divide):
                        appropriateOperatorOnDisplay("/");
                        break;
                    case (Key.Decimal):
                        DecimalPoint_Insert();
                        break;
                    case (Key.Enter):
                        btnEqual_Click(sender, e);
                        //EqualProcess();
                        break;
                    case (Key.Escape):
                        lblResult.Content = "0";
                        lblOperation.Content = "";
                        break;
                    case (Key.Back):
                        if (lblOperation.Content.ToString().Length == 0)
                            return;
                        lblOperation.Content = lblOperation.Content.ToString().Substring(0, lblOperation.Content.ToString().Length - 1);
                        break;
                    case (Key.D0):
                        appropriateNumOnDisplay(0);
                        break;
                    case (Key.D1):
                        appropriateNumOnDisplay(1);
                        break;
                    case (Key.D2):
                        appropriateNumOnDisplay(2);
                        break;
                    case (Key.D3):
                        appropriateNumOnDisplay(3);
                        break;
                    case (Key.D4):
                        appropriateNumOnDisplay(4);
                        break;
                    case (Key.D5):
                        appropriateNumOnDisplay(5);
                        break;
                    case (Key.D6):
                        appropriateNumOnDisplay(6);
                        break;
                    case (Key.D7):
                        appropriateNumOnDisplay(7);
                        break;
                    case (Key.D8):
                        if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                        {
                            appropriateOperatorOnDisplay("x");
                        }
                        else
                        {
                            appropriateNumOnDisplay(8);
                        }
                            
                        break;
                    case (Key.D9):
                        appropriateNumOnDisplay(9);
                        break;
                    case (Key.OemPlus):
                        if(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                        {
                            btnEqual_Click(sender, e);
                        }
                        else
                        {
                            appropriateOperatorOnDisplay("+");
                        }
                        break;
                    case (Key.OemMinus):
                        appropriateOperatorOnDisplay("-");
                        break;
                    case (Key.Oem2):
                        appropriateOperatorOnDisplay("/");
                        break;
                    case (Key.OemPeriod):
                        DecimalPoint_Insert();
                        break;                        

                }
            }
           
            
        }

        private void btnOtherCE_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        
    }

    
}
