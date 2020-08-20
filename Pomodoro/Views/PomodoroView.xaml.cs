using Newtonsoft.Json;
using Pomodoro.Models;
using Pomodoro.Utilities;
using Pomodoro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;

namespace Pomodoro.Views
{
    /// <summary>
    /// PomodoroView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PomodoroView : Window
    {
        #region ::Fields::

        public static NotifyIcon NotifyIcon = new NotifyIcon();

        #endregion

        public PomodoroView()
        {
            InitializeComponent();
            InitializeNotifyIcon();
            this.DataContext = new PomodoroViewModel();
        }

        private void InitializeNotifyIcon()
        {
            NotifyIcon.Text = "Pomodoro";
            NotifyIcon.Icon = Properties.Resources.icon; // Register application icon.
            NotifyIcon.Click += delegate (object senders, EventArgs args) // Subscribe notify icon double click event.
            {
                NotifyIcon.Visible = false;
                this.Show();
            };
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to switch application to the system tray?", "Pomodoro", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                NotifyIcon.Visible = true;
                this.Hide();

                e.Cancel = true; // Cancel window closing.
            }
            else
            {
                // Save settings.
                Settings settings = Settings.GetInstance();
                string settingsJson = JsonConvert.SerializeObject(settings);
                FileUtility.WriteFile(Settings.SETTINGS_JSON, settingsJson, Encoding.UTF8);

                // Save records.
                Records records = Records.GetInstance();
                string recordsJson = JsonConvert.SerializeObject(records);
                FileUtility.WriteFile(Settings.RECORDS_JSON, recordsJson, Encoding.UTF8);
            }
        }
    }
}
