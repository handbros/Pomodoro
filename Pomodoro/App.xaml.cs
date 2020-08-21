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

        public static void InitializeSettings()
        {
            // Initialize settings.
            if (File.Exists(Settings.SETTINGS_JSON)) // if 'settings.json' exists.
            {
                string json = FileUtility.ReadFile(Settings.SETTINGS_JSON, Encoding.UTF8);
                Settings.Instance = JsonConvert.DeserializeObject<Settings>(json);
            }
            else
            {
                // Set to default settings.
                Settings settings = new Settings();
                settings.IsDarkTheme = false;
                settings.IsAutomatic = false;
                settings.IsNotify = true;
                settings.Pomodoro = new TimeSpan(0, Settings.DEFAULT_POMODORO, 0);
                settings.ShortBreak = new TimeSpan(0, Settings.DEFAULT_SHORT, 0);
                settings.LongBreak = new TimeSpan(0, Settings.DEFAULT_LONG, 0);
                settings.IsUseAlarm = false;
                settings.AlarmSongVolume = 100.0f;
                Settings.Instance = settings;
            }
        }

        public static void InitializeRecords()
        {
            // Initialize records.
            if (File.Exists(Settings.RECORDS_JSON)) // if 'records.json' exists.
            {
                string json = FileUtility.ReadFile(Settings.RECORDS_JSON, Encoding.UTF8);
                Records.Instance = JsonConvert.DeserializeObject<Records>(json);
            }
        }

        public static void InitializeTheme()
        {
            var paletteHelper = new PaletteHelper();

            //Retrieve the app's existing theme.
            ITheme theme = paletteHelper.GetTheme();

            // Change theme.
            if (Settings.Instance.IsDarkTheme)
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
