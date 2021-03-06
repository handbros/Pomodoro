﻿using System;

namespace Pomodoro.Models
{
    public class Settings
    {
        #region ::Singleton Pattern::

        // Singleton pattern for saving settings.
        [NonSerialized]
        private static Settings _instance;

        public static Settings Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Settings();

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        #endregion

        #region ::Consts::

        public const string SETTINGS_JSON = @".\settings.json";

        public const string RECORDS_JSON = @".\records.json";

        public const int DEFAULT_POMODORO = 25;

        public const int DEFAULT_SHORT = 5;

        public const int DEFAULT_LONG = 10;

        #endregion

        #region ::Properties::

        public bool IsDarkTheme { get; set; }

        public bool IsAutomatic { get; set; }

        public bool IsAutomaticUseLongBreak { get; set; }

        public bool IsNotify { get; set; }

        public TimeSpan Pomodoro { get; set; }

        public TimeSpan ShortBreak { get; set; }

        public TimeSpan LongBreak { get; set; }

        public bool IsUseAlarm { get; set; }

        public string AlarmSongPath { get; set; }

        public double AlarmSongVolume { get; set; }

        #endregion
    }
}
