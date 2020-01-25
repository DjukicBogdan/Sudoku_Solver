using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string path = @"..\..\Data\TextFileRecord.txt";
        public static string NewName = "";
        public List<Record> listRecord = new List<Record>();
        public DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public DateTime dateTime = new DateTime();
        public Style styleStackPanel;
        public Style styleButton;
        public Style styleTextBox;
        public int buttonInput = 0;
        public string empty = "";
        public TextBox textBoxFocus = new TextBox();
        public string TextBoxName = "textBox1";
        public int[,] ArrayList = new int[9, 9];
        public int[,] ArrayListTask = new int[9, 9];
        public int[] TextBoxArrayListTask = new int[82];
        public List<int> NumbersList = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        public int[,] TableArrayList = new int[9, 9];
        public int[,] CollumnsArrayList = new int[9, 9];
        public int[,] RowArrayList = new int[9, 9];
        public int[,] CheckInputArrayList = new int[9, 9];
        public int[,] InputTableArrayList = new int[9, 9];
        public int[,] InputRowArrayList = new int[9, 9];
        public int[,] InputCollumnArrayList = new int[9, 9];
        public List<int> CountTextBoxR = new List<int>() { 0, 3, 6, 0, 3, 6, 0, 3, 6 };
        public List<int> CountTextBoxRR = new List<int>() { 0, 0, 0, 1, 1, 1, 2, 2, 2 };
        public List<int> CountTextBoxC = new List<int>() { 0, 0, 0, 3, 3, 3, 6, 6, 6 };
        public List<int> CountTextBoxCC = new List<int>() { 0, 1, 2, 0, 1, 2, 0, 1, 2 };
        public IEnumerable<int> randomList;
        public Random rnd = new Random();
        int focusNumber = 0;
        bool showSolution = false;
        public int[,,] FreeFounderTableArrayList = new int[9, 9, 9];
        public int[,,] FreeFounderRowArrayList = new int[9, 9, 9];
        public int[,,] FreeFounderCollumnArrayList = new int[9, 9, 9];
        public int[,] TryFounderTableArrayList = new int[9, 9];

        public class Record
        {
            public int Number { get; set; }
            public string Name { get; set; }
            public string Time { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();

            styleStackPanel = this.Resources["StyleStackPanel"] as Style;
            styleButton = this.Resources["StyleButton"] as Style;
            styleTextBox = this.Resources["StyleTextBox"] as Style;
            textBoxFocus.Name = TextBoxName;         
            TimerSet();
            CreateTable();
            StartGame();
        }

        private void TimeTextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {           
            //GetWin();
            int count = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    count++;
                    textBoxFocus = (TextBox)this.FindName("textBox" + (count).ToString());
                    textBoxFocus.Text = TableArrayList[i, j].ToString();
                }
            }
        }

        private void ButtonNewGame_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void ButtonSolve_Click(object sender, RoutedEventArgs e)
        {            
            if (!showSolution)
            {
                showSolution = true;
                ButtonSolve.Content = "Hide hints";
                ShowResult();
            }
            else
            {
                showSolution = false;
                ButtonSolve.Content = "Show hints";
                CheckInputAsync();
            }
        }

        private void TimerSet()
        {
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, object e)
        {
            dateTime = dateTime.AddSeconds(1);
            TimeTextBlock.Text = dateTime.ToString("mm:ss");
        }

        private void CreateTable()
        {
            int countTextBox = 1;
            //main field
            // 1 collumn table
            for (int i = 1; i <= 3; i++)
            {
                StackPanel stackPanelNew = new StackPanel();
                stackPanelNew.Name = "stackPanel1" + i.ToString();
                stackPanelNew.Style = styleStackPanel;
                StackPanelField1.Children.Add(stackPanelNew);
                // 1 row cells
                StackPanel stackPanelNew1 = new StackPanel();
                stackPanelNew1.Width = 180;
                stackPanelNew1.Height = 60;
                stackPanelNew.Orientation = Orientation.Vertical;
                stackPanelNew1.Orientation = Orientation.Horizontal;
                stackPanelNew.Children.Add(stackPanelNew1);
                for (int j = 1; j <= 3; j++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Name = "textBox" + countTextBox.ToString();
                    textBox.Tag = textBox.Name.ToString();                    
                    textBox.MouseLeftButtonUp += TextBox_MouseLeftButtonUp;
                    countTextBox++;
                    textBox.Style = styleTextBox;
                    stackPanelNew1.Children.Add(textBox);
                    RegisterTextBox(textBox.Name, textBox);
                    textBox.AddHandler(FrameworkElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(TextBox_MouseLeftButtonUp), true);
                    textBox.IsReadOnly = true;
                }
                // 2 row cells
                StackPanel stackPanelNew2 = new StackPanel();
                stackPanelNew2.Width = 180;
                stackPanelNew2.Height = 60;
                stackPanelNew.Orientation = Orientation.Vertical;
                stackPanelNew2.Orientation = Orientation.Horizontal;
                stackPanelNew.Children.Add(stackPanelNew2);
                for (int j = 1; j <= 3; j++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Name = "textBox" + (countTextBox).ToString();
                    textBox.MouseLeftButtonUp += TextBox_MouseLeftButtonUp;
                    textBox.Style = styleTextBox;
                    stackPanelNew2.Children.Add(textBox);
                    RegisterTextBox(textBox.Name, textBox);
                    textBox.AddHandler(FrameworkElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(TextBox_MouseLeftButtonUp), true);
                    countTextBox++;
                    textBox.IsReadOnly = true;
                }
                // 3 row cells
                StackPanel stackPanelNew3 = new StackPanel();
                stackPanelNew3.Width = 180;
                stackPanelNew3.Height = 60;
                stackPanelNew.Orientation = Orientation.Vertical;
                stackPanelNew3.Orientation = Orientation.Horizontal;
                stackPanelNew.Children.Add(stackPanelNew3);
                for (int j = 1; j <= 3; j++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Name = "textBox" + (countTextBox).ToString();
                    textBox.MouseLeftButtonUp += TextBox_MouseLeftButtonUp;
                    textBox.Style = styleTextBox;
                    stackPanelNew3.Children.Add(textBox);
                    RegisterTextBox(textBox.Name, textBox);
                    countTextBox++;
                    textBox.AddHandler(FrameworkElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(TextBox_MouseLeftButtonUp), true);
                    textBox.IsReadOnly = true;
                }
            }
            // 2 collumn table

            for (int i = 1; i <= 3; i++)
            {
                StackPanel stackPanelNew = new StackPanel();
                stackPanelNew.Name = "stackPanel2" + i.ToString();
                stackPanelNew.Style = styleStackPanel;
                StackPanelField2.Children.Add(stackPanelNew);

                // 1 row cells
                StackPanel stackPanelNew1 = new StackPanel();
                stackPanelNew1.Width = 180;
                stackPanelNew1.Height = 60;
                stackPanelNew.Orientation = Orientation.Vertical;
                stackPanelNew1.Orientation = Orientation.Horizontal;
                stackPanelNew.Children.Add(stackPanelNew1);
                for (int j = 1; j <= 3; j++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Name = "textBox" + (countTextBox).ToString();
                    textBox.MouseLeftButtonUp += TextBox_MouseLeftButtonUp;
                    countTextBox++;
                    textBox.Style = styleTextBox;
                    stackPanelNew1.Children.Add(textBox);
                    RegisterTextBox(textBox.Name, textBox);
                    textBox.AddHandler(FrameworkElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(TextBox_MouseLeftButtonUp), true);
                    textBox.IsReadOnly = true;
                }
                // 2 row cells
                StackPanel stackPanelNew2 = new StackPanel();
                stackPanelNew2.Width = 180;
                stackPanelNew2.Height = 60;
                stackPanelNew.Orientation = Orientation.Vertical;
                stackPanelNew2.Orientation = Orientation.Horizontal;
                stackPanelNew.Children.Add(stackPanelNew2);
                for (int j = 1; j <= 3; j++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Name = "textBox" + countTextBox.ToString();
                    textBox.MouseLeftButtonUp += TextBox_MouseLeftButtonUp;
                    countTextBox++;
                    textBox.Style = styleTextBox;
                    stackPanelNew2.Children.Add(textBox);
                    RegisterTextBox(textBox.Name, textBox);
                    textBox.AddHandler(FrameworkElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(TextBox_MouseLeftButtonUp), true);
                    textBox.IsReadOnly = true;
                }
                // 3 row cells
                StackPanel stackPanelNew3 = new StackPanel();
                stackPanelNew3.Width = 180;
                stackPanelNew3.Height = 60;
                stackPanelNew.Orientation = Orientation.Vertical;
                stackPanelNew3.Orientation = Orientation.Horizontal;
                stackPanelNew.Children.Add(stackPanelNew3);
                for (int j = 1; j <= 3; j++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Name = "textBox" + countTextBox.ToString();
                    textBox.MouseLeftButtonUp += TextBox_MouseLeftButtonUp;
                    countTextBox++;
                    textBox.Style = styleTextBox;
                    stackPanelNew3.Children.Add(textBox);
                    RegisterTextBox(textBox.Name, textBox);
                    textBox.AddHandler(FrameworkElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(TextBox_MouseLeftButtonUp), true);
                    textBox.IsReadOnly = true;
                }
            }
            // 3 collumn table

            for (int i = 1; i <= 3; i++)
            {
                StackPanel stackPanelNew = new StackPanel();
                stackPanelNew.Name = "stackPanel3" + i.ToString();
                stackPanelNew.Style = styleStackPanel;
                StackPanelField3.Children.Add(stackPanelNew);

                // 1 row cells
                StackPanel stackPanelNew1 = new StackPanel();
                stackPanelNew1.Width = 180;
                stackPanelNew1.Height = 60;
                stackPanelNew.Orientation = Orientation.Vertical;
                stackPanelNew1.Orientation = Orientation.Horizontal;
                stackPanelNew.Children.Add(stackPanelNew1);
                for (int j = 1; j <= 3; j++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Name = "textBox" + countTextBox.ToString();
                    textBox.MouseLeftButtonUp += TextBox_MouseLeftButtonUp;
                    countTextBox++;
                    textBox.Style = styleTextBox;
                    stackPanelNew1.Children.Add(textBox);
                    RegisterTextBox(textBox.Name, textBox);
                    textBox.AddHandler(FrameworkElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(TextBox_MouseLeftButtonUp), true);
                    textBox.IsReadOnly = true;
                }
                // 2 row cells
                StackPanel stackPanelNew2 = new StackPanel();
                stackPanelNew2.Width = 180;
                stackPanelNew2.Height = 60;
                stackPanelNew.Orientation = Orientation.Vertical;
                stackPanelNew2.Orientation = Orientation.Horizontal;
                stackPanelNew.Children.Add(stackPanelNew2);
                for (int j = 1; j <= 3; j++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Name = "textBox" + countTextBox.ToString();
                    textBox.MouseLeftButtonUp += TextBox_MouseLeftButtonUp;
                    countTextBox++;
                    textBox.Style = styleTextBox;
                    stackPanelNew2.Children.Add(textBox);
                    RegisterTextBox(textBox.Name, textBox);
                    textBox.AddHandler(FrameworkElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(TextBox_MouseLeftButtonUp), true);
                    textBox.IsReadOnly = true;
                }
                // 3 row cells
                StackPanel stackPanelNew3 = new StackPanel();
                stackPanelNew3.Width = 180;
                stackPanelNew3.Height = 60;
                stackPanelNew.Orientation = Orientation.Vertical;
                stackPanelNew3.Orientation = Orientation.Horizontal;
                stackPanelNew.Children.Add(stackPanelNew3);
                for (int j = 1; j <= 3; j++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Name = "textBox" + countTextBox.ToString();
                    textBox.MouseLeftButtonUp += TextBox_MouseLeftButtonUp;
                    countTextBox++;
                    textBox.Style = styleTextBox;
                    stackPanelNew3.Children.Add(textBox);
                    RegisterTextBox(textBox.Name, textBox);
                    textBox.AddHandler(FrameworkElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(TextBox_MouseLeftButtonUp), true);
                    textBox.IsReadOnly = true;
                }
            }
            textBoxFocus = (TextBox)this.FindName("textBox1");

            //keyboardfield
            for (int i = 1; i <= 3; i++)
            {
                Button button = new Button();
                button.Content = i.ToString();
                button.Name = "button" + i.ToString();
                button.Style = styleButton;
                StackPanelKey1.Children.Add(button);
                button.MouseLeftButtonUp += Button_MouseLeftButtonUp;
                RegisterButton(button.Name, button);
                button.AddHandler(FrameworkElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Button_MouseLeftButtonUp), true);

            }
            for (int i = 1; i <= 3; i++)
            {
                Button button = new Button();
                button.Content = (i + 3).ToString();
                button.Name = "button" + (i + 3).ToString().Trim();
                button.Style = styleButton;
                StackPanelKey2.Children.Add(button);
                button.MouseLeftButtonUp += Button_MouseLeftButtonUp;
                RegisterButton(button.Name, button);
                button.AddHandler(FrameworkElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Button_MouseLeftButtonUp), true);

                // button.AddHandler(PointerPressedEvent, new PointerEventHandler(Button_PointerPressed), true);
            }
            for (int i = 1; i <= 3; i++)
            {
                Button button = new Button();
                button.Content = (i + 6).ToString();
                button.Name = "button" + (i + 6).ToString().Trim();
                button.Style = styleButton;
                StackPanelKey3.Children.Add(button);
                button.MouseLeftButtonUp += Button_MouseLeftButtonUp;
                RegisterButton(button.Name, button);
                button.AddHandler(FrameworkElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Button_MouseLeftButtonUp), true);

                // button.AddHandler(PointerPressedEvent, new PointerEventHandler(Button_PointerPressed), true);
            }
            Button buttonDel = new Button();
            buttonDel.Content = "DELETE";
            buttonDel.Name = "buttonDelete";
            buttonDel.Style = styleButton;
            StackPanelKeyDel.Children.Add(buttonDel);
            buttonDel.Width = 274;
            buttonDel.MouseLeftButtonUp += Button_MouseLeftButtonUp;
            RegisterButton(buttonDel.Name, buttonDel);
            buttonDel.AddHandler(FrameworkElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Button_MouseLeftButtonUp), true);

            // buttonDel.AddHandler(PointerPressedEvent, new PointerEventHandler(Button_PointerPressed), true);
        }

        private void Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Button button1 = sender as Button;
            Button button = (Button)this.FindName(button1.Name);
            switch (button.Name.ToString())
            {
                case "button1": buttonInput = 1; break;
                case "button2": buttonInput = 2; break;
                case "button3": buttonInput = 3; break;
                case "button4": buttonInput = 4; break;
                case "button5": buttonInput = 5; break;
                case "button6": buttonInput = 6; break;
                case "button7": buttonInput = 7; break;
                case "button8": buttonInput = 8; break;
                case "button9": buttonInput = 9; break;
                case "buttonDelete": buttonInput = 0; break;
                default:
                    buttonInput = 0;
                    break;
            }
            TextBox textBoxFocus = (TextBox)this.FindName(TextBoxName);
            for (int i = 1; i < 82; i++)
            {
                if ("textBox" + TextBoxArrayListTask[i].ToString() == textBoxFocus.Name)
                {
                    break;
                }
            }
            textBoxFocus.Text = buttonInput.ToString();
            if (button.Name == "buttonDelete")
            {
                textBoxFocus.Text = "";
            }
            GetInput();
            CheckInputAsync();
        }

        void RegisterTextBox(string textBoxName, TextBox textBox)
        {
            if ((TextBox)this.FindName(textBoxName) != null)
                this.UnregisterName(textBoxName);
            this.RegisterName(textBoxName, textBox);
        }

        void RegisterButton(string buttonName, Button button)
        {
            if ((Button)this.FindName(buttonName) != null)
                this.UnregisterName(buttonName);
            this.RegisterName(buttonName, button);
        }

        private void TextBox_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {           
            textBoxFocus = (TextBox)this.FindName(TextBoxName);
            CheckInputAsync();
            TextBox textBox = sender as TextBox;
            textBoxFocus = (TextBox)this.FindName(textBox.Name);
            textBoxFocus.Background = new SolidColorBrush(Colors.LightBlue);
            TextBoxName = textBoxFocus.Name.ToString();
            GetInput();
        }

        private void GenerateRandom()
        {
            int[,] FreeRandomArrayList = new int[9, 9];
            int[] countFreeNumbersList = new int[9];
            int countFreeNumbers = 0;
            bool checkDouble = false;
            for (int i = 0; i < 9; i++)
            {
                //if (i == 2 || i == 5)
                //{
                //    continue;
                //}
                ArrayList[i, 0] = rnd.Next(1, 9);
                ArrayList[i, 1] = rnd.Next(1, 9);
                ArrayList[i, 2] = rnd.Next(1, 9);
                ArrayList[i, 3] = rnd.Next(1, 9);
                ArrayList[i, 4] = rnd.Next(1, 9);
                ArrayList[i, 5] = rnd.Next(1, 9);
                ArrayList[i, 6] = rnd.Next(1, 9);
                ArrayList[i, 7] = rnd.Next(1, 9);
                ArrayList[i, 8] = rnd.Next(1, 9);
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (ArrayList[i, j] == ArrayList[i, k] && j != k)
                        {
                            for (int m = 1; m < 10; m++)
                            {
                                for (int n = 0; n < 9; n++)
                                {
                                    checkDouble = FindDuplicateInRowColl(m, i, j);
                                    if (checkDouble)
                                    {
                                        break;
                                    }
                                    if (ArrayList[i, n] == m)
                                    {
                                        checkDouble = true;
                                        break;
                                    }
                                    if (ArrayList[i, n] != m)
                                    {
                                        checkDouble = false;
                                    }
                                }
                                if (!checkDouble)
                                {
                                    FreeRandomArrayList[i, countFreeNumbers] = m;
                                    ArrayList[i, j] = FreeRandomArrayList[i, countFreeNumbers];
                                    countFreeNumbers++;
                                }

                            }


                        }
                    }
                    countFreeNumbers = 0;
                    for (int ii = 0; ii < 9; ii++)
                    {
                        checkDouble = FindDuplicateInRowColl(ii, i, j);
                        if (checkDouble)
                        {
                            //repeat
                        }
                    }


                }


            }
        }

        private void MixNumbers()
        {
            // TableArrayList
            int[,] TableStartTrueCombinationArreyList = new int[,] {
               { 4, 7, 1, 2, 6, 9, 8, 3, 5 },
               { 7, 4, 3, 5, 2, 8, 1, 9, 6 },
               { 3, 5, 7, 6, 8, 2, 9, 1, 4 },

               { 9, 3, 6, 1, 5, 8, 4, 7, 2 },
               { 8, 2, 5, 6, 9, 1, 3, 4, 7 },
               { 2, 8, 4, 5, 1, 9, 7, 6, 3 },

               { 2, 5, 8, 7, 3, 4, 6, 9, 1 },
               { 1, 6, 9, 4, 7, 3, 8, 2, 5 },
               { 9, 1, 6, 3, 4, 7, 5, 8, 2 }
            };
            // RowArrayList
            int[,] StartTrueCombinationArreyList = new int[,] {
               { 4, 7, 1, 9, 3, 6, 2, 5, 8 },
               { 2, 6, 9, 1, 5, 8, 7, 3, 4 },
               { 8, 3, 5, 4, 7, 2, 6, 9, 1 },

               { 7, 4, 3, 8, 2, 5, 1, 6, 9 },
               { 5, 2, 8, 6, 9, 1, 4, 7, 3 },
               { 1, 9, 6, 3, 4, 7, 8, 2, 5 },

               { 3, 5, 7, 2, 8, 4, 9, 1, 6 },
               { 6, 8, 2, 5, 1, 9, 3, 4, 7 },
               { 9, 1, 4, 7, 6, 3, 5, 8, 2 }
            };

            // mix row

            for (int j = 0; j < 9; j += 3)
            {
                int randomRow = rnd.Next(1, 5);
                switch (randomRow)
                {
                    case 1:
                        for (int k = 0; k < 9; k++)
                        {
                            RowArrayList[1 + j, k] = StartTrueCombinationArreyList[0 + j, k];
                            RowArrayList[0 + j, k] = StartTrueCombinationArreyList[1 + j, k];
                            RowArrayList[2 + j, k] = StartTrueCombinationArreyList[2 + j, k];
                        }
                        break;

                    case 2:
                        for (int k = 0; k < 9; k++)
                        {
                            RowArrayList[1 + j, k] = StartTrueCombinationArreyList[0 + j, k];
                            RowArrayList[2 + j, k] = StartTrueCombinationArreyList[1 + j, k];
                            RowArrayList[0 + j, k] = StartTrueCombinationArreyList[2 + j, k];
                        }
                        break;

                    case 3:
                        for (int k = 0; k < 9; k++)
                        {
                            RowArrayList[2 + j, k] = StartTrueCombinationArreyList[0 + j, k];
                            RowArrayList[0 + j, k] = StartTrueCombinationArreyList[1 + j, k];
                            RowArrayList[1 + j, k] = StartTrueCombinationArreyList[2 + j, k];
                        }
                        break;
                    case 4:
                        for (int k = 0; k < 9; k++)
                        {
                            RowArrayList[2 + j, k] = StartTrueCombinationArreyList[0 + j, k];
                            RowArrayList[1 + j, k] = StartTrueCombinationArreyList[1 + j, k];
                            RowArrayList[0 + j, k] = StartTrueCombinationArreyList[2 + j, k];
                        }
                        break;
                    case 5:
                        for (int k = 0; k < 9; k++)
                        {
                            RowArrayList[0 + j, k] = StartTrueCombinationArreyList[0 + j, k];
                            RowArrayList[2 + j, k] = StartTrueCombinationArreyList[1 + j, k];
                            RowArrayList[1 + j, k] = StartTrueCombinationArreyList[2 + j, k];
                        }
                        break;


                    default:
                        for (int k = 0; k < 9; k++)
                        {
                            RowArrayList[0 + j, k] = StartTrueCombinationArreyList[0 + j, k];
                            RowArrayList[1 + j, k] = StartTrueCombinationArreyList[1 + j, k];
                            RowArrayList[2 + j, k] = StartTrueCombinationArreyList[2 + j, k];
                        }
                        break;
                }
            }
            GetTableFromRow();
            // mix collumn
            for (int j = 0; j < 8; j += 3)
            {
                int randomCollumn = rnd.Next(1, 5);
                switch (randomCollumn)
                {
                    case 1:
                        for (int k = 0; k < 9; k++)
                        {
                            CollumnsArrayList[1 + j, k] = RowArrayList[k, 0 + j];
                            CollumnsArrayList[0 + j, k] = RowArrayList[k, 1 + j];
                            CollumnsArrayList[2 + j, k] = RowArrayList[k, 2 + j];
                        }
                        break;

                    case 2:
                        for (int k = 0; k < 9; k++)
                        {
                            CollumnsArrayList[1 + j, k] = RowArrayList[k, 0 + j];
                            CollumnsArrayList[2 + j, k] = RowArrayList[k, 1 + j];
                            CollumnsArrayList[0 + j, k] = RowArrayList[k, 2 + j];
                        }
                        break;

                    case 3:
                        for (int k = 0; k < 9; k++)
                        {
                            CollumnsArrayList[2 + j, k] = RowArrayList[k, 0 + j];
                            CollumnsArrayList[0 + j, k] = RowArrayList[k, 1 + j];
                            CollumnsArrayList[1 + j, k] = RowArrayList[k, 2 + j];
                        }
                        break;
                    case 4:
                        for (int k = 0; k < 9; k++)
                        {
                            CollumnsArrayList[2 + j, k] = RowArrayList[k, 0 + j];
                            CollumnsArrayList[1 + j, k] = RowArrayList[k, 1 + j];
                            CollumnsArrayList[0 + j, k] = RowArrayList[k, 2 + j];
                        }
                        break;
                    case 5:
                        for (int k = 0; k < 9; k++)
                        {
                            CollumnsArrayList[0 + j, k] = RowArrayList[k, 0 + j];
                            CollumnsArrayList[2 + j, k] = RowArrayList[k, 1 + j];
                            CollumnsArrayList[1 + j, k] = RowArrayList[k, 2 + j];
                        }
                        break;


                    default:
                        for (int k = 0; k < 9; k++)
                        {
                            CollumnsArrayList[0 + j, k] = RowArrayList[k, 0 + j];
                            CollumnsArrayList[1 + j, k] = RowArrayList[k, 1 + j];
                            CollumnsArrayList[2 + j, k] = RowArrayList[k, 2 + j];
                        }
                        break;
                }
            }
            GetTableFromCollumn();

            // mix Table
            // MIX Table in rows
            int randomTable = rnd.Next(1, 5);
            for (int j = 0; j < 9; j += 3)
            {

                switch (randomTable)
                {
                    case 1:
                        for (int k = 0; k < 9; k++)
                        {
                            TableStartTrueCombinationArreyList[1 + j, k] = TableArrayList[0 + j, k];
                            TableStartTrueCombinationArreyList[0 + j, k] = TableArrayList[1 + j, k];
                            TableStartTrueCombinationArreyList[2 + j, k] = TableArrayList[2 + j, k];
                        }
                        break;

                    case 2:
                        for (int k = 0; k < 9; k++)
                        {
                            TableStartTrueCombinationArreyList[1 + j, k] = TableArrayList[0 + j, k];
                            TableStartTrueCombinationArreyList[2 + j, k] = TableArrayList[1 + j, k];
                            TableStartTrueCombinationArreyList[0 + j, k] = TableArrayList[2 + j, k];
                        }
                        break;

                    case 3:
                        for (int k = 0; k < 9; k++)
                        {
                            TableStartTrueCombinationArreyList[2 + j, k] = TableArrayList[0 + j, k];
                            TableStartTrueCombinationArreyList[0 + j, k] = TableArrayList[1 + j, k];
                            TableStartTrueCombinationArreyList[1 + j, k] = TableArrayList[2 + j, k];
                        }
                        break;
                    case 4:
                        for (int k = 0; k < 9; k++)
                        {
                            TableStartTrueCombinationArreyList[2 + j, k] = TableArrayList[0 + j, k];
                            TableStartTrueCombinationArreyList[1 + j, k] = TableArrayList[1 + j, k];
                            TableStartTrueCombinationArreyList[0 + j, k] = TableArrayList[2 + j, k];
                        }
                        break;
                    case 5:
                        for (int k = 0; k < 9; k++)
                        {
                            TableStartTrueCombinationArreyList[0 + j, k] = TableArrayList[0 + j, k];
                            TableStartTrueCombinationArreyList[2 + j, k] = TableArrayList[1 + j, k];
                            TableStartTrueCombinationArreyList[1 + j, k] = TableArrayList[2 + j, k];
                        }
                        break;


                    default:
                        for (int k = 0; k < 9; k++)
                        {
                            TableStartTrueCombinationArreyList[0 + j, k] = TableArrayList[0 + j, k];
                            TableStartTrueCombinationArreyList[1 + j, k] = TableArrayList[1 + j, k];
                            TableStartTrueCombinationArreyList[2 + j, k] = TableArrayList[2 + j, k];
                        }
                        break;
                }
            }

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    TableArrayList[i, j] = TableStartTrueCombinationArreyList[i, j];
                }
            }

            // MIX Table in collumns 
            int randomTableC = rnd.Next(1, 5);
            for (int j = 0; j < 3; j++)
            {
                switch (randomTableC)
                {
                    case 1:
                        for (int k = 0; k < 9; k++)
                        {
                            TableStartTrueCombinationArreyList[3 + j, k] = TableArrayList[0 + j, k];
                            TableStartTrueCombinationArreyList[0 + j, k] = TableArrayList[3 + j, k];
                            TableStartTrueCombinationArreyList[6 + j, k] = TableArrayList[6 + j, k];
                        }
                        break;

                    case 2:
                        for (int k = 0; k < 9; k++)
                        {
                            TableStartTrueCombinationArreyList[3 + j, k] = TableArrayList[0 + j, k];
                            TableStartTrueCombinationArreyList[6 + j, k] = TableArrayList[3 + j, k];
                            TableStartTrueCombinationArreyList[0 + j, k] = TableArrayList[6 + j, k];
                        }
                        break;

                    case 3:
                        for (int k = 0; k < 9; k++)
                        {
                            TableStartTrueCombinationArreyList[6 + j, k] = TableArrayList[0 + j, k];
                            TableStartTrueCombinationArreyList[0 + j, k] = TableArrayList[3 + j, k];
                            TableStartTrueCombinationArreyList[3 + j, k] = TableArrayList[6 + j, k];
                        }
                        break;
                    case 4:
                        for (int k = 0; k < 9; k++)
                        {
                            TableStartTrueCombinationArreyList[6 + j, k] = TableArrayList[0 + j, k];
                            TableStartTrueCombinationArreyList[3 + j, k] = TableArrayList[3 + j, k];
                            TableStartTrueCombinationArreyList[0 + j, k] = TableArrayList[6 + j, k];
                        }
                        break;
                    case 5:
                        for (int k = 0; k < 9; k++)
                        {
                            TableStartTrueCombinationArreyList[0 + j, k] = TableArrayList[0 + j, k];
                            TableStartTrueCombinationArreyList[6 + j, k] = TableArrayList[3 + j, k];
                            TableStartTrueCombinationArreyList[3 + j, k] = TableArrayList[6 + j, k];
                        }
                        break;


                    default:
                        for (int k = 0; k < 9; k++)
                        {
                            TableStartTrueCombinationArreyList[0 + j, k] = TableArrayList[0 + j, k];
                            TableStartTrueCombinationArreyList[3 + j, k] = TableArrayList[3 + j, k];
                            TableStartTrueCombinationArreyList[6 + j, k] = TableArrayList[6 + j, k];
                        }
                        break;
                }
            }

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    TableArrayList[i, j] = TableStartTrueCombinationArreyList[i, j];
                }
            }
            GetRowCollumn();
        }

        private void MakeTask()
        {          
            int rndcount = 0;
            Random rnd1 = new Random();
            Random rndCount = new Random();

            for (int i = 0; i < 9; i++)
            {
                rndcount = rndCount.Next(1, 8);
                for (int j = 0; j < rndcount; j++)
                {
                    ArrayListTask[i, j] = rnd1.Next(1, 10);
                    TextBoxArrayListTask[i * 9 + ArrayListTask[i, j]] = ArrayListTask[i, j];
                }
            }
        }

        private void FindNumbers()
        {           
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    InputTableArrayList[i, j] = 0;
                    TryFounderTableArrayList[i, j] = 0;
                    for (int k = 0; k < 9; k++)
                    {                       
                        FreeFounderTableArrayList[i, j, k] = 0;
                        //  table, index, number
                    }
                }
            }
            GetInput();
            GetRowCollumn();
            bool checkDouble = true;
            bool checkDoubleCollumn = true;
            int currentNumber = 0;
           //table
            for (int i = 0; i < 9; i++)
            {
                currentNumber = NumbersList[i];
                //index
                for (int j = 0; j < 9; j++)
                {
                   //number
                    for (int k = 0; k < 9; k++)
                    {
                        checkDouble = FindDuplicateInRowColl(NumbersList[k], i, j);
                        if (!checkDouble)
                        {
                            FreeFounderTableArrayList[i, j, k] = NumbersList[k];                          
                        }
                        if (checkDouble)
                        {
                           
                        }
                        //checkDoubleCollumn = FindDuplicateInRowCollForCollumn(NumbersList[k], i, j);
                        //if (!checkDoubleCollumn)
                        //{
                        //    FreeFounderCollumnArrayList[i, j, k] = NumbersList[k];
                        //}
                    }
                }              
            }
            GetRowCollumn();         
            PrintNumbers();
        }

        static Stack<int> GetStackIndex()
        {
            Stack<int> stackIndex = new Stack<int>();          
            return stackIndex;
        }

        static Stack<int> GetStackTable()
        {          
            Stack<int> stackTable = new Stack<int>();
            return stackTable;
        }    

        private void FindCombination()
        {
            for (int i = 0; i < 30; i++)
            {              
                PutOnlyChance();
            }
          
            bool checkDouble = true;
            bool repeat = false;
            bool missTable = false;         
            GetRowCollumn();
            Stack<int> stackIndex = GetStackIndex();
            Stack<int> stackTable = GetStackTable();
            stackIndex.Clear();
            stackTable.Clear();
            for (int number = 0; number < 9; number++)
            {             
                for (int table = 0; table < 9; table++)
                {
                    if (ValidateTableAndNumberExist(table, number) && !repeat)
                    {
                        continue;
                    }

                    for (int index = 0; index < 9; index++)
                    {                                              
                        if (repeat)
                        {
                            repeat = false;
                            if (stackTable.Count == 0)
                            {
                                break;
                            }
                            if (stackTable.Count > 0)
                            {
                                table = stackTable.Pop();
                                index = stackIndex.Pop();                             
                            }
                            number = InputTableArrayList[table, index];
                            number--;
                            InputTableArrayList[table, index] = 0;
                            missTable = true;
                            continue;
                        }
                        if (number < 0)
                        {
                            number = 0;
                        }
                        checkDouble = FindDuplicateInRowColl(NumbersList[number], table, index);
                        if (!checkDouble)
                        {
                            InputTableArrayList[table, index] = NumbersList[number];
                            GetRowCollumn();
                            stackIndex.Push(index);
                            stackTable.Push(table);
                            TextBox textBox = new TextBox();
                            textBox = (TextBox)this.FindName("textBox" + (table * 9 + index + 1).ToString());
                            textBox.IsReadOnly = false;
                            GetRowCollumn();
                            WriteAllTextBox();
                            break;
                        }
                        if (checkDouble)
                        {                           
                                                  
                        }
                    }

                    if (checkDouble)
                    {                        
                        repeat = true;                    
                    }
                    WriteAllTextBox();
                }               
            }
        }

        private void PutOnlyChance()
        {
            FindNumbers();
            GetRowCollumn();
           
            bool check = false;
            int count = 0;
            int getNumber = -1;
            int getIndex = 0;
            bool checkDouble = false;
            for (int table = 0; table < 9; table++)
            {
                for (int index = 0; index < 9; index++)
                {
                    for (int number = 0; number < 9; number++)
                        {
                        if (FreeFounderTableArrayList[table, index, number] != 0)
                        {
                            getNumber = FreeFounderTableArrayList[table, index, number];
                            getIndex = index;
                            check = true;
                            count++;
                        }
                        if (count > 1)
                        {
                            getNumber = -1;
                            check = false;
                            count = 0;
                            break;
                        }
                    }
                    if (check && getNumber != -1)
                    {
                        checkDouble = FindDuplicateInRowColl(FreeFounderTableArrayList[table, getIndex, getNumber - 1], table, getIndex);
                        if (!checkDouble)
                        {
                            InputTableArrayList[table, getIndex] = FreeFounderTableArrayList[table, getIndex, getNumber - 1];
                        }                                           
                    }
                }
            }
            GetRowCollumn();
            PrintNumbers();
          
        }

        private bool ValidateTableAndNumberExist(int table, int number)
        {
            if (table < 0 || number < 0)
            {
                return false;
            }
            for (int i = 0; i < 9; i++)
            {
                if (InputTableArrayList[table, i] == NumbersList[number])
                {
                    return true;
                }
            }
            return false;
        }

        private void WriteTextBox(TextBox textBox, int table, int index)
        {
            textBoxFocus = textBox;
            textBoxFocus.IsReadOnly = false;
            Dispatcher.Invoke(new Action(() =>
            {
                textBox.Text = InputTableArrayList[table, index].ToString();
            }), DispatcherPriority.Background);          
            System.Threading.Thread.Sleep(50);
        }

        public void WriteAllTextBox()
        {
            TextBox textBox = new TextBox();
            int count = 0;
            textBoxFocus.IsReadOnly = false;
            for (int table = 0; table < 9; table++)
            {
                for (int index = 0; index < 9; index++)
                {
                    count++;
                    textBox = (TextBox)this.FindName("textBox" + (count).ToString());
                    Dispatcher.Invoke(new Action(() =>
                    {
                        textBox.Text = InputTableArrayList[table, index].ToString() == "0" ? "" : InputTableArrayList[table, index].ToString();
                    }), DispatcherPriority.Background);
                }
            }
           // System.Threading.Thread.Sleep(1);
        }

        private bool CheckTrueCombination(int number)
        {
            int count = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (InputTableArrayList[i, j] == number)
                    {
                        count++;                     
                    }                   
                }
            }
            if (count == 9)
            {
                return true;
            }
            else
            {
                return false;
            }           
        }

        private bool FindDuplicateInRowColl(int findNumber, int FindTable, int FindIndex)
        {
            GetRowCollumn();
            if (InputTableArrayList[FindTable, FindIndex] != 0)
            {
                return true;
            }
            for (int i = 0; i < 9; i++)
            {
                if (InputTableArrayList[FindTable, i] == findNumber)
                {
                    return true;
                }
            }
            for (int collumn = 0; collumn < 9; collumn++)
            {
                if (InputRowArrayList[CountTextBoxR[FindTable] + CountTextBoxRR[FindIndex], collumn] == findNumber)
                {
                    return true;
                }               
            }
          
            for (int row = 0; row < 9; row++)
            {
                if (InputCollumnArrayList[CountTextBoxC[FindTable] + CountTextBoxCC[FindIndex], row] == findNumber)
                {
                    return true;
                }
            }
          
            return false;
        }
        
        private void GetRowCollumn()
        {
            //int[,] CollumnsArrayList = new int[9, 9];
            int row = 0;
            int collumn = 0;
            //get collumns
            for (int o = 0; o < 8; o += 3)
            {
                for (int k = 0; k < 3; k++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 8; j += 3)
                        {
                            CollumnsArrayList[row, collumn] = TableArrayList[i + o, j + k];
                            InputCollumnArrayList[row, collumn] = InputTableArrayList[i + o, j + k]; 
                            collumn++;
                        }
                        if (collumn >= 8)
                        {
                            row++;
                            collumn = 0;
                        }
                    }
                }
            }
            //int[,] RowArrayList = new int[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    RowArrayList[j, i] = CollumnsArrayList[i, j];
                    InputRowArrayList[j, i] = InputCollumnArrayList[i, j];
                  
                }
            }
        }

        private bool FindDuplicateInRowCollForCollumn(int findNumber, int FindTable, int FindIndex)
        {
            GetRowCollumn();
            if (InputTableArrayList[FindTable, FindIndex] != 0)
            {
                return true;
            }
            for (int i = 0; i < 9; i++)
            {
                if (InputTableArrayList[FindTable, i] == findNumber)
                {
                    return true;
                }
            }
            for (int collumn = 0; collumn < 9; collumn++)
            {
                if (InputRowArrayList[CountTextBoxR[FindTable] + CountTextBoxRR[FindIndex], collumn] == findNumber)
                {
                    return true;
                }
            }

            for (int row = 0; row < 9; row++)
            {
                if (InputCollumnArrayList[CountTextBoxC[FindTable] + CountTextBoxCC[FindIndex], row] == findNumber)
                {
                    return true;
                }
            }

            return false;
        }

        private void GetTableFromCollumn()
        {
            int row = 0;
            int collumn = 0;
            //get Table
            for (int o = 0; o < 8; o += 3)
            {
                for (int k = 0; k < 3; k++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 8; j += 3)
                        {
                            TableArrayList[i + o, j + k] = CollumnsArrayList[row, collumn];
                            InputTableArrayList[i + o, j + k] = InputCollumnArrayList[row, collumn]; collumn++;
                        }
                        if (collumn >= 8)
                        {
                            row++;
                            collumn = 0;
                        }
                    }
                }
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    RowArrayList[j, i] = CollumnsArrayList[i, j];
                    InputRowArrayList[j, i] = InputCollumnArrayList[i, j];
                }
            }
        }

        private void GetTableFromRow()
        {         
            int row = 0;
            int collumn = 0;
            //get Table
            for (int o = 0; o < 8; o += 3)
            {
                for (int k = 0; k < 3; k++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 8; j += 3)
                        {
                            TableArrayList[i + o, j + k] = RowArrayList[collumn, row];
                            InputTableArrayList[i + o, j + k] = InputRowArrayList[collumn, row]; collumn++;
                        }
                        if (collumn >= 8)
                        {
                            row++;
                            collumn = 0;
                        }
                    }
                }
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    CollumnsArrayList[i, j] = RowArrayList[j, i];
                    InputCollumnArrayList[i, j] = InputRowArrayList[j, i];
                }
            }
        }
    
        private void PrintNumbers()
        {
            textBoxFocus = (TextBox)this.FindName(TextBoxName);
            if (textBoxFocus.Text != "")
            {
                focusNumber = Convert.ToInt32(textBoxFocus.Text);
            }

            int count = 1;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    TextBox textBox = (TextBox)this.FindName("textBox" + count);
                    textBox.IsReadOnly = false;                  
                                       
                    if (InputTableArrayList[i, j].ToString() == "0")
                    {
                        textBox.Text = "";
                        textBox.Background = new SolidColorBrush(Colors.White);
                        if (textBox.Name == TextBoxName)
                        {
                            textBox.Background = new SolidColorBrush(Colors.LightBlue);
                        }
                    }
                    if (TextBoxArrayListTask[count] != 0)
                    {
                        textBox.Text = TableArrayList[i, j].ToString();
                        textBox.Background = new SolidColorBrush(Colors.LightGray);
                    }
                    if (InputTableArrayList[i, j].ToString() != "0")
                    {
                        textBox.Text = InputTableArrayList[i, j].ToString();
                    }
                    //if (j == ArrayListTask[i, ArrayListTask[i, j]] && ArrayListTask[i, ArrayListTask[i, j]] != 0)
                    //{                        
                    //    textBox.Text = TableArrayList[i, j].ToString();
                    //    textBox.Background = new SolidColorBrush(Colors.LightGray);
                    //}
                    if (TableArrayList[i, j] == focusNumber && focusNumber != 0)
                    {
                        textBox.Foreground = new SolidColorBrush(Colors.Blue);
                    }
                    if (InputTableArrayList[i, j] == focusNumber && focusNumber != 0)
                    {
                        textBox.Foreground = new SolidColorBrush(Colors.Blue);
                    }
                    if (InputTableArrayList[i, j] != focusNumber && focusNumber != 0)
                    {
                        textBox.Foreground = new SolidColorBrush(Colors.Black);
                    }
                    textBox.IsReadOnly = true;
                    count++;
                }
            }
        }

        private void DeleteNumbers()
        {
            TextBoxName = "textBox1";
            int count = 1;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    TextBox textBox = (TextBox)this.FindName("textBox" + count);
                    ArrayList[i, j] = 0;
                    ArrayListTask[i, j] = 0;
                    InputTableArrayList[i, j] = 0;
                    InputRowArrayList[i, j] = 0;
                    InputCollumnArrayList[i, j] = 0;
                    CheckInputArrayList[i, j] = 0;
                    TableArrayList[i, j] = 0;
                    RowArrayList[i, j] = 0;
                    CollumnsArrayList[i, j] = 0;                   
                    TextBoxArrayListTask[count] = 0;
                    count++;
                    textBox.Text = "";
                    textBox.Foreground = new SolidColorBrush(Colors.Black);
                    textBox.Background = new SolidColorBrush(Colors.White);
                }
            }
        }       

        private void GetInput()
        {
            int count = 1;
            int row = 0;
            int collumn = 0;
            focusNumber = 0;
            TextBox textBox = new TextBox();
            textBox = (TextBox)this.FindName(TextBoxName);
            string stringTest = textBox.Text;
            int intTest;
            try
            {
                if (stringTest == "")
                {
                    stringTest = "0";
                }
                intTest = Convert.ToInt32(stringTest);
            }
            catch (Exception)
            {
                textBox.Text = "";
                throw;
            }
            if (textBox.Text != "")
            {
                focusNumber = Convert.ToInt32(textBox.Text);
            }
            if (textBox.Text == "")
            {
                focusNumber = 0;
            }
            for (int i = 0; i < 81; i++)
            {
                textBoxFocus = (TextBox)this.FindName("textBox" + count); count++;
                if (textBoxFocus.Text == "")
                {
                    InputTableArrayList[collumn, row] = 0;
                    CheckInputArrayList[collumn, row] = 0; row++;
                    if (row == 9)
                    {
                        row = 0;
                        collumn++;
                    }
                }
                if (textBoxFocus.Text == focusNumber.ToString())
                {
                    textBoxFocus.Foreground = new SolidColorBrush(Colors.Blue);
                }
                if (textBoxFocus.Text != focusNumber.ToString())
                {
                    textBoxFocus.Foreground = new SolidColorBrush(Colors.Black);
                }
                if (textBoxFocus.Text != "")
                {
                    InputTableArrayList[collumn, row] = Convert.ToInt32(textBoxFocus.Text);
                    CheckInputArrayList[collumn, row] = Convert.ToInt32(textBoxFocus.Text); row++;
                    if (row == 9)
                    {
                        row = 0;
                        collumn++;
                    }
                }
            }
            row = 0;
            collumn = 0;
            //get collumns
            for (int o = 0; o < 8; o += 3)
            {
                for (int k = 0; k < 3; k++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 8; j += 3)
                        {
                            InputCollumnArrayList[row, collumn] = CheckInputArrayList[i + o, j + k]; collumn++;
                        }
                        if (collumn >= 8)
                        {
                            row++;
                            collumn = 0;
                        }
                    }
                }
            }
            //int[,] RowArrayList = new int[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    RowArrayList[j, i] = CollumnsArrayList[i, j];
                    InputRowArrayList[j, i] = InputCollumnArrayList[i, j];
                }
            }
            GetRowCollumn();
            PrintNumbers();
        }

        private void ResetTextBox()
        {
            for (int i = 1; i < 82; i++)
            {
                textBoxFocus = (TextBox)this.FindName("textBox" + (i).ToString());

                textBoxFocus.Background = new SolidColorBrush(Colors.White);
                //textBoxFocus.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void CheckInputAsync()
        {
           
            ResetTextBox();
            GetInput();
            bool GameRules = true;
            int count = 1;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (CheckInputArrayList[i, j] == 0)
                        {
                            GameRules = false;
                        }
                        if (InputTableArrayList[i, j].ToString() == InputTableArrayList[i, k].ToString() && j != k && InputTableArrayList[i, j] != 0)
                        {
                            textBoxFocus = (TextBox)this.FindName("textBox" + (count).ToString());
                           
                            if (textBoxFocus.Name == "textBox" + TextBoxArrayListTask[i * 9 + j].ToString())
                            {
                                textBoxFocus.Background = new SolidColorBrush(Colors.LightGray);
                            }
                            textBoxFocus.Background = new SolidColorBrush(Colors.Red);
                            GameRules = false;
                          
                           
                        }
                       

                        if (textBoxFocus.Text == "")
                        {
                            textBoxFocus.Background = new SolidColorBrush(Colors.White);
                        }
                    }
                    count++;

                }
            }
            count = 1;
            List<int> CountTextBox1 = new List<int>() { 1, 2, 3, 28, 29, 30, 55, 56, 57 };
            for (int i = 0; i < 9; i++)
            {
                count++;
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        if (InputRowArrayList[i, j].ToString() == InputRowArrayList[i, k].ToString() && k != j)
                        {
                            textBoxFocus = (TextBox)this.FindName("textBox" + (CountTextBox1[j] + i * 3).ToString());
                            textBoxFocus.Background = new SolidColorBrush(Colors.Red);
                            if (textBoxFocus.Name == "textBox" + TextBoxArrayListTask[i * 9 + j].ToString())
                            {
                                textBoxFocus.Background = new SolidColorBrush(Colors.LightGray);
                            }
                            GameRules = false;
                        }

                        if (textBoxFocus.Text == "")
                        {
                            textBoxFocus.Background = new SolidColorBrush(Colors.White);
                            GameRules = false;
                        }
                    }

                }
            }
            count = 0;
            List<int> CountTextBox2 = new List<int>() { 1, 2, 3, 28, 29, 30, 55, 56, 57 };

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    //count++;
                    for (int k = 0; k < 9; k++)
                    {

                        if (InputCollumnArrayList[i, j].ToString() == InputCollumnArrayList[i, k].ToString() && k != j)
                        {
                            textBoxFocus = (TextBox)this.FindName("textBox" + (count + CountTextBox2[i]).ToString());
                            textBoxFocus.Background = new SolidColorBrush(Colors.Red);
                            if (textBoxFocus.Name == "textBox" + TextBoxArrayListTask[i * 9 + j].ToString())
                            {
                                textBoxFocus.Background = new SolidColorBrush(Colors.LightGray);
                            }
                            GameRules = false;
                        }
                        if (textBoxFocus.Text == "")
                        {
                            textBoxFocus.Background = new SolidColorBrush(Colors.White);
                            GameRules = false;
                        }
                    }
                    count += 3;
                    if (count > 26)
                    {
                        count = 0;
                    }

                }
            }
            if (showSolution)
            {
                ShowResult();
            }

            if (GameRules)
            {
                dispatcherTimer.Stop();
                Win();
            }
            if (!GameRules)
            {
                //await new MessageDialog("Game Rules Fail", "Game Rules").ShowAsync();
            }
            //TextBox textBox2 = new TextBox();
            //textBox2 = (TextBox)this.FindName(TextBoxName);
            //for (int i = 1; i < 82; i++)
            //{
            //    TextBox textBox1 = new TextBox();
            //    textBox1 = (TextBox)this.FindName("textBox" + (i).ToString());
            //    if (textBox1.Text == textBox2.Text)
            //    {
            //        textBoxFocus.Background = new SolidColorBrush(Colors.GreenYellow);
            //    }
            //}

        }

        private void ShowResult()
        {
            int countBox = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    countBox++;
                    textBoxFocus = (TextBox)this.FindName("textBox" + (countBox).ToString());

                    if (textBoxFocus.Text == TableArrayList[i, j].ToString())
                    {
                        textBoxFocus.Foreground = new SolidColorBrush(Colors.Green);
                    }
                    if (textBoxFocus.Text != TableArrayList[i, j].ToString())
                    {
                        textBoxFocus.Foreground = new SolidColorBrush(Colors.DarkRed);
                    }
                }
            }           
        }

        private void GetWin()
        {

            int[,] TextBoxWin = new int[,] {
               { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
               { 2, 3, 4, 5, 6, 7, 8, 9, 1 },
               { 3, 4, 5, 6, 7, 8, 9, 1, 2 },
               { 4, 5, 6, 7, 8, 9, 1, 2, 3 },
               { 5, 6, 7, 8, 9, 1, 2, 3, 4 },
               { 6, 7, 8, 9, 1, 2, 3, 4, 5 },
               { 7, 8, 9, 1, 2, 3, 4, 5, 6 },
               { 8, 9, 1, 2, 3, 4, 5, 6, 7 },
               { 9, 1, 2, 3, 4, 5, 6, 7, 8 }
            };

            int[,] TextBoxTest = new int[,] {
               { 4, 7, 1, 2, 6, 9, 8, 3, 5 },
               { 7, 4, 3, 5, 2, 8, 1, 9, 6 },
               { 3, 5, 7, 6, 8, 2, 9, 1, 4 },
               { 9, 3, 6, 1, 5, 8, 4, 7, 2 },
               { 8, 2, 5, 6, 9, 1, 3, 4, 7 },
               { 2, 8, 4, 5, 1, 9, 7, 6, 3 },
               { 2, 5, 8, 7, 3, 4, 6, 9, 1 },
               { 1, 6, 9, 4, 7, 3, 8, 2, 5 },
               { 9, 1, 6, 3, 4, 7, 5, 8, 2 }
            };


            int count = 1;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    TextBox textBox = (TextBox)this.FindName("textBox" + count);
                    count++;
                    textBox.Text = TextBoxWin[i, j].ToString();
                    //textBox.Text = TextBoxTest[i, j].ToString();
                }
            }
        }

        private void Win()
        {
            MessageBox.Show("You Win !!!");

            WindowNewName windowNewName = new WindowNewName();
            windowNewName.ShowDialog();
            EditTextFile(dateTime.ToString("mm:ss"));
            GridRecord.Visibility = Visibility.Visible;
            ButtonClose.Visibility = Visibility.Visible;
           
        }

        private void StartGame()
        {
            dateTime = dateTime.AddMinutes(-dateTime.Minute).AddSeconds(-dateTime.Second);
            DeleteNumbers();
            //GenerateRandom();
           
            MixNumbers();
            MakeTask();
            PrintNumbers();
           
            dispatcherTimer.Start();
        }
      
        private void EditTextFile(string time)
        {

            string data = "";
            if (!File.Exists(path))
            {
                MessageBox.Show("File TextFileRecord.txt not found");
                try
                {
                    using (FileStream fs = File.Create(path))
                    {

                    }
                    MessageBox.Show("File TextFileRecord.txt is created");
                }
                catch (Exception exc)
                {

                    MessageBox.Show("File TextFileRecord.txt is not created, error: " + exc);
                }
            }
            data = ((NewName + "      " + time.ToString()) + Environment.NewLine);
            File.AppendAllText(path, data);
            string[] line = File.ReadAllLines(path);
            listRecord.Clear();

            foreach (var item in line)
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }
                Record record = new Record();
                record.Name = item.Substring(0, item.Length - 5);
                record.Time = (item.Substring(item.Length - 5, 5));
                listRecord.Add(record);

            }
            var NewlistRecord = listRecord.OrderBy(x => x.Time);
            int i = 1;
            foreach (var item in NewlistRecord)
            {
                item.Number = i; i++;
            }

            foreach (var item1 in NewlistRecord)
            {
                if (item1.Number > 10)
                {
                    listRecord.Remove(item1);
                }
            }
            NewlistRecord = listRecord.OrderBy(x => x.Number);
            File.WriteAllText(path, "");
            foreach (var item in NewlistRecord)
            {
                string Time = item.Time;
                data = (item.Name + " " + Time + Environment.NewLine);
                File.AppendAllText(path, data);
            }

            DataGridRecord.ItemsSource = null;
            DataGridRecord.Items.Clear();
            DataGridRecord.ItemsSource = NewlistRecord.ToList();
            List<Record> newList = new List<Record>();
            newList = NewlistRecord.ToList();
            for (int j = 0; j < newList.Count; j++)
            {
                if (newList[j].Name.Trim() == NewName && newList[j].Time.ToString() == time.ToString())
                {
                   // DataGridRecord.SelectedIndex = j;
                      DataGridRecord.SelectedItem = DataGridRecord.Items[j];
                  
                }
            }
        }      

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            ButtonClose.Visibility = Visibility.Hidden;
            GridRecord.Visibility = Visibility.Hidden;          
        }

        private void ButtonSolveAlgoritam_Click(object sender, RoutedEventArgs e)
        {
            FindCombination();
        }

        private void ButtonClearTable_Click(object sender, RoutedEventArgs e)
        {
            DeleteNumbers();           
        }


    }
    //  TableList 036  RowList 000 CollumnList 036
    //            147          333             036
    //            258          666             036
    //  [0] [3] [6]                    [012 345 678]
    //  012 012 012   [0] 012 345 678   000 000 000  T[0,0] R[0,0] C[0,0]    T[0,1] R[0,1] C[0,0]    T[0,2] R[0,2] C[0,0]
    //  345 345 345   [1] 012 345 678   111 111 111  T[0,3] R[1,0] C[0,1]    T[0,4] R[1,1] C[0,1]    T[0,5] R[1,2] C[0,1]
    //  678 678 678   [2] 012 345 678   222 222 222  T[0,6] R[2,0] C[0,2]    T[0,7] R[2,1] C[0,2]    T[0,8] R[2,2] C[0,2]
    //  [1] [4] [7]
    //  012 012 012   [3] 012 345 678   333 333 333  T[1,0] R[3,0] C[0,3]    T[1,1] R[3,1] C[0,3]    T[1,2] R[3,2] C[0,3]
    //  345 345 345   [4] 012 345 678   444 444 444  T[1,3] R[4,0] C[0,4]    T[1,4] R[4,1] C[0,4]    T[1,5] R[4,2] C[0,4]
    //  678 678 678   [5] 012 345 678   555 555 555  T[1,6] R[5,0] C[0,5]    T[1,7] R[5,1] C[0,5]    T[1,8] R[5,2] C[0,5]
    //  [2] [5] [8]
    //  012 012 012   [6] 012 345 678   666 666 666  T[2,0] R[6,0] C[0,6]    T[2,1] R[6,1] C[0,6]    T[2,2] R[6,2] C[0,6]
    //  345 345 345   [7] 012 345 678   777 777 777  T[2,3] R[7,0] C[0,7]    T[2,4] R[7,1] C[0,7]    T[2,5] R[7,2] C[0,7]
    //  678 678 678   [8] 012 345 678   888 888 888  T[2,6] R[8,0] C[0,8]    T[2,7] R[8,1] C[0,8]    T[2,8] R[8,2] C[0,8]
    //
    //
    //    T[0,0] R[0,0] C[0,0]   T[0,1] R[0,1] C[1,0]   T[0,2] R[0,2] C[2,0]  |  T[3,0] R[0,3] C[3,0]   T[3,1] R[0,4] C[4,0]   T[3,2] R[0,5] C[5,0]  |  T[6,0] R[0,6] C[6,0]   T[6,1] R[0,7] C[7,0]   T[6,2] R[0,8] C[8,0]   
    //    T[0,3] R[1,0] C[0,1]   T[0,4] R[1,1] C[1,1]   T[0,5] R[1,2] C[2,1]  |  T[3,3] R[1,3] C[3,1]   T[3,4] R[1,4] C[4,1]   T[3,5] R[1,5] C[5,1]  |  T[6,3] R[1,6] C[6,1]   T[6,4] R[1,7] C[7,1]   T[6,5] R[1,8] C[8,1]
    //    T[0,6] R[2,0] C[0,2]   T[0,7] R[2,1] C[1,2]   T[0,8] R[2,2] C[2,2]  |  T[3,6] R[2,3] C[3,2]   T[3,7] R[2,4] C[4,2]   T[3,8] R[2,5] C[5,2]  |  T[6,6] R[2,6] C[6,2]   T[6,7] R[2,7] C[7,2]   T[6,8] R[2,8] C[8,2]
    //    --------------------------------------------------------------------|----------------------------------------------------------------------|--------------------------------------------------------------------
    //    T[1,0] R[3,0] C[0,3]   T[1,1] R[3,1] C[1,3]   T[1,2] R[3,2] C[2,3]  |  T[4,0] R[3,3] C[3,3]   T[4,1] R[3,4] C[4,3]   T[4,2] R[3,5] C[5,3]  |  T[7,0] R[3,6] C[6,3]   T[7,1] R[3,7] C[7,3]   T[7,2] R[3,8] C[8,3]
    //    T[1,3] R[4,0] C[0,4]   T[1,4] R[4,1] C[1,4]   T[1,5] R[4,2] C[2,4]  |  T[4,3] R[4,3] C[3,4]   T[4,4] R[4,4] C[4,4]   T[4,5] R[4,5] C[5,4]  |  T[7,3] R[4,6] C[6,4]   T[7,4] R[4,7] C[7,4]   T[7,5] R[4,8] C[8,4]
    //    T[1,6] R[5,0] C[0,5]   T[1,7] R[5,1] C[1,5]   T[1,8] R[5,2] C[2,5]  |  T[4,6] R[5,3] C[3,5]   T[4,7] R[5,4] C[4,5]   T[4,8] R[5,5] C[5,5]  |  T[7,6] R[5,6] C[6,5]   T[7,7] R[5,7] C[7,5]   T[7,8] R[5,8] C[8,5]
    //    --------------------------------------------------------------------|----------------------------------------------------------------------|--------------------------------------------------------------------
    //    T[2,0] R[6,0] C[0,6]   T[2,1] R[6,1] C[1,6]   T[2,2] R[6,2] C[2,6]  |  T[5,0] R[6,3] C[3,6]   T[5,1] R[6,4] C[4,6]   T[5,2] R[6,5] C[5,6]  |  T[8,0] R[6,6] C[6,6]   T[8,1] R[6,7] C[7,6]   T[8,2] R[6,8] C[8,6]
    //    T[2,3] R[7,0] C[0,7]   T[2,4] R[7,1] C[1,7]   T[2,5] R[7,2] C[2,7]  |  T[5,3] R[7,3] C[3,7]   T[5,4] R[7,4] C[4,7]   T[5,5] R[7,5] C[5,7]  |  T[8,3] R[7,6] C[6,7]   T[8,4] R[7,7] C[7,7]   T[8,5] R[7,8] C[8,7]
    //    T[2,6] R[8,0] C[0,8]   T[2,7] R[8,1] C[1,8]   T[2,8] R[8,2] C[2,8]  |  T[5,6] R[8,3] C[3,8]   T[5,7] R[8,4] C[4,8]   T[5,8] R[8,5] C[5,8]  |  T[8,6] R[8,6] C[6,8]   T[8,7] R[8,7] C[7,8]   T[8,8] R[8,8] C[8,8]
    //
    // T findTable 7 findIndex 2    R 3,8   C 8,3
    // T findTable 5 findIndex 5    R 7,5   C 5,7 
}

