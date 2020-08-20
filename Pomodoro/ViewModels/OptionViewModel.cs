using Pomodoro.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.ViewModels
{
    public class OptionViewModel : INotifyPropertyChanged
    {
        #region ::Fields::

        private Settings _settings;

        #endregion

        #region ::Properties::

        public bool IsDarkTheme
        {
            get
            {
                return _settings.IsDarkTheme;
            }
            set
            {
                _settings.IsDarkTheme = value;
                RaisePropertyChanged("IsDarkTheme");
            }
        }

        public bool IsAutomatic
        {
            get
            {
                return _settings.IsAutomatic;
            }
            set
            {
                _settings.IsAutomatic = value;
                RaisePropertyChanged("IsAutomatic");
            }
        }

        public bool IsAutomaticUseLongBreak
        {
            get
            {
                return _settings.IsAutomaticUseLongBreak;
            }
            set
            {
                _settings.IsAutomaticUseLongBreak = value;
                RaisePropertyChanged("IsAutomaticUseLongBreak");
            }
        }

        public bool IsNotify
        {
            get
            {
                return _settings.IsNotify;
            }
            set
            {
                _settings.IsNotify = value;
                RaisePropertyChanged("IsNotify");
            }
        }

        public TimeSpan Pomodoro
        {
            get
            {
                return _settings.Pomodoro;
            }
            set
            {
                _settings.Pomodoro = value;
                RaisePropertyChanged("Pomodoro");
            }
        }

        public TimeSpan ShortBreak
        {
            get
            {
                return _settings.ShortBreak;
            }
            set
            {
                _settings.ShortBreak = value;
                RaisePropertyChanged("ShortBreak");
            }
        }

        public TimeSpan LongBreak
        {
            get
            {
                return _settings.LongBreak;
            }
            set
            {
                _settings.LongBreak = value;
                RaisePropertyChanged("LongBreak");
            }
        }

        public bool IsUseAlarm
        {
            get
            {
                return _settings.IsUseAlarm;
            }
            set
            {
                _settings.IsUseAlarm = value;
                RaisePropertyChanged("IsUseAlarm");
            }
        }

        public string AlarmSongPath
        {
            get
            {
                return _settings.AlarmSongPath;
            }
            set
            {
                _settings.AlarmSongPath = value;
                RaisePropertyChanged("AlarmSongPath");
            }
        }

        public double AlarmSongVolume
        {
            get
            {
                return _settings.AlarmSongVolume;
            }
            set
            {
                _settings.AlarmSongVolume = value;
                RaisePropertyChanged("AlarmSongVolume");
            }
        }

        public int AlarmDuration
        {
            get
            {
                return _settings.AlarmDuration;
            }
            set
            {
                _settings.AlarmDuration = value;
                RaisePropertyChanged("AlarmDuration");
            }
        }

        #endregion

        #region ::Constructor::

        public OptionViewModel()
        {
            _settings = Settings.GetInstance();
        }

        #endregion

        #region ::INotifyPropertyChanged Members::

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
