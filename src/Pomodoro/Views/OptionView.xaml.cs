using Newtonsoft.Json;
using Pomodoro.Models;
using Pomodoro.Utilities;
using Pomodoro.ViewModels;
using System.Text;
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
            Regex regex = new Regex("[^0-9]+"); // Check that entered character is a number.
            e.Handled = regex.IsMatch(e.Text);
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Save settings.
            string settingsJson = JsonConvert.SerializeObject(Settings.Instance);
            FileUtility.WriteFile(Settings.SETTINGS_JSON, settingsJson, Encoding.UTF8);
        }
    }
}
