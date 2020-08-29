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
using System.IO;

namespace SudokuProgKorny
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static int[,] sudoku = new int[9, 9];
        static int size = 9;
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Sudoku()
        {
            #region statikusok
            //replace pls with dynamic
            int[] startingIndexes = new int[]   { 1,  11,  21,  31,  41,  51,  61,  71,  81,  91,
                                                101, 111, 121, 131, 141, 151, 161, 171, 181, 191,
                                                201, 211, 221, 231, 241, 251, 261, 271, 281, 291,
                                                301, 311, 321, 331, 341, 351, 361, 371, 381, 391,
                                                401, 411, 421, 431, 441, 451, 461, 471, 481, 491};
            Random rng = new Random();

            //
            var lines = System.IO.File.ReadLines("starter.txt").Skip(startingIndexes[rng.Next(0, startingIndexes.Length)]).Take(9).ToArray();
            #endregion

            #region letezo szamok tombje
            for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        sudoku[i, j] = int.Parse(lines[i][j].ToString());
                    }
                }
            #endregion

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Biztosan ki akarsz lépni?", "Kilépés", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            #region grafika osszerakasa
            gameGrid.Children.Clear();
            gameGrid.ColumnDefinitions.Clear();
            gameGrid.RowDefinitions.Clear();

            ColumnDefinition col;
            RowDefinition row;

            gameGrid.ShowGridLines = false;
            for (int i = 0; i < size; i++)
            {
                col = new ColumnDefinition();

                row = new RowDefinition();
                gameGrid.RowDefinitions.Add(row);
                gameGrid.ColumnDefinitions.Add(col);
            }

            Sudoku();
            FillGrid();
            #endregion
        }

        private void FillGrid()
        {
            #region jatszoter feltoltese
            Random rng = new Random();
            int fill = rng.Next(0,3);

            Button btn;
            Label lbl;


            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    
                    
                    int setNum = rng.Next(1, 10);

                    lbl = new Label();
                    lbl.FontWeight = FontWeights.Bold;
                    lbl.FontFamily = new FontFamily("Impact");
                    lbl.Foreground = Brushes.White;
                    lbl.FontSize = 28;

                    lbl.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    lbl.VerticalAlignment = System.Windows.VerticalAlignment.Center;

                    btn = new Button();

                    btn.Content = "";
                   
                    btn.FontSize = 28;
                    btn.FontFamily = new FontFamily("Impact");
                    btn.Background = Brushes.Black;
                    btn.Foreground = Brushes.White;
                    btn.BorderThickness = new Thickness(0.5);
                    btn.Name = "btn" + i + j;



                    lbl.Content = sudoku[i,j];


                    Grid.SetRow(lbl, i);
                    Grid.SetColumn(lbl, j);


                    if (sudoku[i,j] != 0)
                    {
                        gameGrid.Children.Add(lbl);
                    }
                    else
                    {
                        Grid.SetRow(btn, i);
                        Grid.SetColumn(btn, j);
                        gameGrid.Children.Add(btn);
                    }

                    btn.Click += Btn_Click;
                    

                }
            }
            #endregion

        }

        private async void Btn_Click(object sender, RoutedEventArgs e)
        {
            //megjelenit egy uj ablakot amin kivalasztod h mit akarsz beszurni
            //todo: numpad-ra reagalni?
            (sender as Button).Content = "";
            (sender as Button).Background = Brushes.Green;
            var picker = new PickWindow();

            picker.Show();
           
            var pickedNum = await picker.Fetch();

            int y = Grid.GetColumn((UIElement)sender);
            int x = Grid.GetRow((UIElement)sender);
           

            (sender as Button).Content = "";

            bool passedRow = false;
            bool passedColumn = false;
            bool passedSquare = false;
            bool illegalMove = false;

            int squareIndex = 0;
            #region GridIndex kereso
            if (x <= 2)
            {
                if (y <= 2)
                {
                  
                    squareIndex = 1;
                }
                else if(y > 5)
                {
                  
                    squareIndex = 3;
                }
                else
                {
                  
                    squareIndex = 2;
                }

            }
            else if (x > 5)
            {
                if (y <= 2)
                {
                  
                    squareIndex = 7;
                }
                else if (y > 5)
                {
                   
                    squareIndex = 9;
                }
                else
                {
                  
                    squareIndex = 8;
                }
            }
            else
            {
                if (y <= 2)
                {
                  
                    squareIndex = 4;
                }
                else if (y > 5)
                {
                 
                    squareIndex = 6;
                }
                else
                {
                 
                    squareIndex = 5;
                }
            }
            #endregion
            
            int errorCount = 0;


            #region valasztott szam legalitas ellenorzo
            if (int.Parse(pickedNum) == 0)
            {
                (sender as Button).Content = "";
                (sender as Button).Background = Brushes.Black;
                picker.Close();
            }
            else
            {
                (sender as Button).Content = "";
                if (CheckWithinBox(squareIndex, int.Parse(pickedNum)))
                {
                    passedSquare = true;
                    (sender as Button).Content = "";
                }
                else
                {
                    illegalMove = true;
                    (sender as Button).Content = "";
                    (sender as Button).Background = Brushes.Red;
                    passedRow = false;
                    picker.Close();
                }

                for (int rows = 0; rows < 9; rows++)
                {
                    (sender as Button).Content = "";
                    if (sudoku[rows, y] == int.Parse(pickedNum))
                    {

                        illegalMove = true;
                        (sender as Button).Content = "";
                        passedRow = false;
                        (sender as Button).Background = Brushes.Red;
                        picker.Close();
                        break;
                        
                    }
                    else
                    {
                        (sender as Button).Content = "";
                        passedRow = true;
                        (sender as Button).Background = Brushes.Black;

                    }
                }
                for (int columns = 0; columns < 9; columns++)
                {
                    (sender as Button).Content = "";
                    if ((sudoku[x, columns] == int.Parse(pickedNum)) && passedRow)
                    {
                        illegalMove = true;
                        (sender as Button).Content = "";
                        passedColumn = false;
                        (sender as Button).Background = Brushes.Red;
                        picker.Close();
                        break;
                    }
                    else
                    {
                        (sender as Button).Content = "";
                        passedColumn = true;
                        (sender as Button).Background = Brushes.Black;
                    }
                }

                if (illegalMove)
                {
                    errorCount++;
                    MessageBox.Show("Illegal move");
                    (sender as Button).Background = Brushes.Red;
                }
                if (passedRow && passedColumn && passedSquare)
                {
                    (sender as Button).Content = pickedNum;
                    sudoku[x, y] = int.Parse(pickedNum); 
                    (sender as Button).Background = Brushes.Black;
                    picker.Close();
                }
                else
                {
                    (sender as Button).Content = "";
                    (sender as Button).Background = Brushes.Black;
                    Btn_Click(sender, e);
                }
                
            }
            #endregion

            #region gyozelmi allapot ellenorzo
            int sum = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    sum += sudoku[i, j];
                }
            }

            
            if (sum == 405)
            {
                MessageBox.Show("Siker!\nHibák száma: " + errorCount.ToString());
                gameGrid.Children.Clear();
                gameGrid.ColumnDefinitions.Clear();
                gameGrid.RowDefinitions.Clear();
            }
            #endregion
        }

        private bool CheckWithinBox(int squareIndex, int pickedNum)
        {
            //I : SOR, J : OSZLOP
            int i, j;
            switch (squareIndex)
            {
                case 1:
                    i = 0;
                    j = 0;
                    if (CheckMiniSquare(i, j, pickedNum))
                    {
                        return true;
                    }
                    else { break; }

                case 2:
                    i = 0;
                    j = 3;
                    if (CheckMiniSquare(i, j, pickedNum))
                    {
                        return true;
                    }
                    else { break; }

                case 3:
                    i = 0;
                    j = 6;
                    if (CheckMiniSquare(i, j, pickedNum))
                    {
                        return true;
                    }
                    else { break; }

                case 4:
                    i = 3;
                    j = 0;
                    if (CheckMiniSquare(i, j, pickedNum))
                    {
                        return true;
                    }
                    else { break; }

                case 5:
                    i = 3;
                    j = 3;
                    if (CheckMiniSquare(i, j, pickedNum))
                    {
                        return true;
                    }
                    else { break; }

                case 6:
                    i = 3;
                    j = 6;
                    if (CheckMiniSquare(i, j, pickedNum))
                    {
                        return true;
                    }
                    else { break; }

                case 7:
                    i = 6;
                    j = 0;
                    if (CheckMiniSquare(i, j, pickedNum))
                    {
                        return true;
                    }
                    else { break; }

                case 8:
                    i = 6;
                    j = 3;
                    if (CheckMiniSquare(i, j, pickedNum))
                    {
                        return true;
                    }
                    else { break; }

                case 9:
                    i = 6;
                    j = 6;
                    if (CheckMiniSquare(i, j, pickedNum))
                    {
                        return true;
                    }
                    else { break; }
                    
                    

                default:
                    break;
            }
            return false;
        }

        private bool CheckMiniSquare(int i, int j, int pickedNum)
        { 
                for (int k = i; k <= i + 2; k++)
                {
                    for (int l = j; l <= j + 2; l++)
                    {
                        if (sudoku[k,l] == pickedNum)
                        {
                           
                            return false;
                        
                        }
                    }
                }        
            return true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var about = new AboutPage();
            about.Show();
        }
    }
}