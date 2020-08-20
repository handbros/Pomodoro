using Microsoft.Win32;
using NAudio.Wave;
using Pomodoro.Commands;
using Pomodoro.Models;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;

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

        #endregion

        #region ::Constructor::

        public OptionViewModel()
        {
            _settings = Settings.GetInstance();
        }

        #endregion

        #region ::Methods::

        private void AlarmBrowse()
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Title = "Selects a audio file...";
            
            dialog.Filter = "Audio Files|*.aac;*.aif;*.aiff;*.aifc;*.caf;*.mp3;*.m4a;*.wav;*.wma;*.mp4";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (dialog.ShowDialog() == true)
            {
                if (File.Exists(dialog.FileName))
                {
                    AlarmSongPath = dialog.FileName;
                }
            }
        }

        private void AlarmTest()
        {
            // Check null reference.
            if (_settings.AlarmSongPath == null)
            {
                MessageBox.Show("Please specify the audio file to be used for the alarm.", "Pomodoro", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            // Check audio file.
            if (!File.Exists(_settings.AlarmSongPath))
            {
                MessageBox.Show("The audio file does not exist.", "Pomodoro", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            // Play audio file in a new thread.
            Thread alarm = new Thread(() =>
            {
                using (var audioFile = new AudioFileReader(_settings.AlarmSongPath))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Volume = (float)_settings.AlarmSongVolume / 100f;
                    outputDevice.Play();
                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        Thread.Sleep(1000);
                    }
                }
            });

            alarm.Start();
        }

        #endregion

        #region ::Commands::

        private ICommand _alarmBrowseCommand;

        public ICommand AlarmBrowseCommand
        {
            get
            {
                return (_alarmBrowseCommand) ?? (_alarmBrowseCommand = new DelegateCommand(AlarmBrowse));
            }
        }

        private ICommand _alarmTestCommand;

        public ICommand AlarmTestCommand
        {
            get
            {
                return (_alarmTestCommand) ?? (_alarmTestCommand = new DelegateCommand(AlarmTest));
            }
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
