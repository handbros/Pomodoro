using Pomodoro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Pomodoro.Views
{
    /// <summary>
    /// OptionView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OptionView : Window
    {
        public OptionView()
        {
            InitializeComponent();
            this.DataContext = new OptionViewModel();
        }

        private void NumberTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+"); // Check that entered variable is a number.
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
