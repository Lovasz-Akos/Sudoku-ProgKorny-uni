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
using System.Windows.Shapes;

namespace SudokuProgKorny
{
    /// <summary>
    /// Interaction logic for PickWindow.xaml
    /// </summary>
    public partial class PickWindow : Window
    {
        public PickWindow()
        {
            InitializeComponent();
        }

        private TaskCompletionSource<String> _tcs = new TaskCompletionSource<String>();

        public Task<String> Fetch()
        {
            return _tcs.Task;
        }


        private void one_Click(object sender, RoutedEventArgs e)
        {
            _tcs.SetResult("1");
        }

        private void two_Click(object sender, RoutedEventArgs e)
        {
            _tcs.SetResult("2");
        }

        private void three_Click(object sender, RoutedEventArgs e)
        {
            _tcs.SetResult("3");
        }

        private void four_Click(object sender, RoutedEventArgs e)
        {
            _tcs.SetResult("4");
        }

        private void five_Click(object sender, RoutedEventArgs e)
        {
            _tcs.SetResult("5");
        }

        private void six_Click(object sender, RoutedEventArgs e)
        {
            _tcs.SetResult("6");
        }

        private void seven_Click(object sender, RoutedEventArgs e)
        {
            _tcs.SetResult("7");
        }

        private void eight_Click(object sender, RoutedEventArgs e)
        {
            _tcs.SetResult("8");
        }

        private void nine_Click(object sender, RoutedEventArgs e)
        {
            _tcs.SetResult("9");
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            _tcs.SetResult("0");
        }
    }
}
