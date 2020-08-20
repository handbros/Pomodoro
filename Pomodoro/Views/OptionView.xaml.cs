using Pomodoro.ViewModels;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

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
