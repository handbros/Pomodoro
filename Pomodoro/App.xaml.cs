using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using Pomodoro.Models;
using Pomodoro.Utilities;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Pomodoro
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InitializeSettings();
            InitializeRecords();
            InitializeTheme();
        }

        private void InitializeSettings()
        {
            Settings settings;
            // Initialize settings.
            if (File.Exists(Settings.SETTINGS_JSON)) // if 'settings.json' exists.
            {
                string json = FileUtility.ReadFile(Settings.SETTINGS_JSON, Encoding.UTF8);
                settings = JsonConvert.DeserializeObject<Settings>(json);
                Settings.SetInstance(settings);
            }
            else
            {
                settings = new Settings();
                settings.IsDarkTheme = false;
                settings.IsAutomatic = false;
                settings.IsNotify = true;
                settings.Pomodoro = new TimeSpan(0, Settings.DEFAULT_POMODORO, 0);
                settings.ShortBreak = new TimeSpan(0, Settings.DEFAULT_SHORT, 0);
                settings.LongBreak = new TimeSpan(0, Settings.DEFAULT_LONG, 0);
                settings.IsUseAlarm = false;
                settings.AlarmSongVolume = 100.0f;
                settings.AlarmDuration = 5000;
                Settings.SetInstance(settings);
            }
        }

        private void InitializeRecords()
        {
            Records records;
            // Initialize records.
            if (File.Exists(Settings.RECORDS_JSON)) // if 'records.json' exists.
            {
                string json = FileUtility.ReadFile(Settings.RECORDS_JSON, Encoding.UTF8);
                records = JsonConvert.DeserializeObject<Records>(json);
                Records.SetInstance(records);
            }
        }

        private void InitializeTheme()
        {
            var paletteHelper = new PaletteHelper();

            //Retrieve the app's existing theme.
            ITheme theme = paletteHelper.GetTheme();

            // Change theme.
            if (Settings.GetInstance().IsDarkTheme)
            {
                theme.SetBaseTheme(Theme.Dark);
            }
            else
            {
                theme.SetBaseTheme(Theme.Light);
            }

            // Change colors.
            PrimaryColor primary = PrimaryColor.Blue;
            Color primaryColor = SwatchHelper.Lookup[(MaterialDesignColor)primary];
            theme.SetPrimaryColor(primaryColor);

            PrimaryColor secondary = PrimaryColor.Blue;
            Color secondaryColor = SwatchHelper.Lookup[(MaterialDesignColor)secondary];
            theme.SetSecondaryColor(secondaryColor);

            // Change the app's current theme.
            paletteHelper.SetTheme(theme);
        }
    }
}
