using NAudio.Wave;
using Newtonsoft.Json;
using Pomodoro.Commands;
using Pomodoro.Utilities;
using Pomodoro.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace Pomodoro.ViewModels
{
    public class PomodoroViewModel : INotifyPropertyChanged
    {
        #region ::Enums::

        public enum TimerType
        {
            None,
            Pomodoro,
            ShortBreak,
            LongBreak
        }

        #endregion

        #region ::Fields::

        private Settings _settings;

        private Timer _timer;

        #endregion

        #region ::Properties::

        private TimeSpan _residueTimeSpan;

        public TimeSpan ResidueTimeSpan
        {
            get
            {
                return _residueTimeSpan;
            }
            set
            {
                _residueTimeSpan = value;
                RaisePropertyChanged("ResidueTimeSpan");
            }
        }

        private TimerType _currentTimerType;

        public TimerType CurrentTimerType
        {
            get
            {
                return _currentTimerType;
            }
            set
            {
                _currentTimerType = value;
                RaisePropertyChanged("CurrentTimerType");
            }
        }

        private double _timerProgress = 0;

        public double TimerProgress
        {
            get
            {
                return _timerProgress;
            }
            set
            {
                _timerProgress = value;
                RaisePropertyChanged("TimerProgress");
            }
        }

        private int _count = 0;

        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
                RaisePropertyChanged("Count");
            }
        }

        private bool _isPlay = false;

        public bool IsPlay
        {
            get
            {
                return _isPlay;
            }
            set
            {
                _isPlay = value;
                RaisePropertyChanged("IsPlay");
            }
        }

        #endregion

        #region ::Constructor::

        public PomodoroViewModel()
        {
            // Initialize settings.
            _settings = Settings.GetInstance();

            // Initialize timer.
            _timer = new Timer(1000);
            _timer.Enabled = true;
            _timer.Stop();

            _timer.Elapsed += OnTimerElapsed; // Add timer elapsed event subscriber.
        }

        #endregion

        #region ::Timer-Related::

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Residue time span processing part.
            if (ResidueTimeSpan.TotalSeconds <= 0)
            {
                if (IsPlay)
                {
                    Reset();

                    // Show balloon tip.
                    if (_settings.IsNotify)
                    {
                        PomodoroView.NotifyIcon.Visible = true;
                        PomodoroView.NotifyIcon.ShowBalloonTip(3000, "Pomodoro", "Now time's up!", System.Windows.Forms.ToolTipIcon.Info);
                        PomodoroView.NotifyIcon.Visible = false;
                    }

                    if (CurrentTimerType == TimerType.Pomodoro)
                    {
                        Count++;
                    }

                    // Check timer automatically and restart timer.
                    if (_settings.IsAutomatic)
                    {
                        if (CurrentTimerType == TimerType.Pomodoro && _settings.IsAutomaticUseLongBreak)
                        {
                            LongBreak();
                        }
                        else if (CurrentTimerType == TimerType.Pomodoro && !_settings.IsAutomaticUseLongBreak)
                        {
                            ShortBreak();
                        }
                        else if (CurrentTimerType == TimerType.ShortBreak || CurrentTimerType == TimerType.LongBreak)
                        {
                            Pomodoro();
                        }

                        Start();
                    }
                }
            }
            else
            {
                // Subtract a second from residue time span.
                ResidueTimeSpan = ResidueTimeSpan.Subtract(new TimeSpan(0, 0, 1));

                // Timer progress processing part.
                TimeSpan entireTimeSpan = new TimeSpan();

                if (CurrentTimerType == TimerType.Pomodoro)
                    entireTimeSpan = _settings.Pomodoro;
                else if (CurrentTimerType == TimerType.ShortBreak)
                    entireTimeSpan = _settings.ShortBreak;
                else if (CurrentTimerType == TimerType.LongBreak)
                    entireTimeSpan = _settings.LongBreak;

                TimerProgress = (ResidueTimeSpan.TotalSeconds / entireTimeSpan.TotalSeconds) * 100;
            }
        }

        #endregion

        #region ::Methods::

        private void Start()
        {
            IsPlay = true;
            _timer.Start();
        }

        private void Pause()
        {
            IsPlay = false;
            _timer.Stop();
        }

        private void Reset()
        {
            IsPlay = false;
            _timer.Stop();

            CurrentTimerType = TimerType.None;

            // Reset residue time span.
            ResidueTimeSpan = new TimeSpan(0, 0, 0);

            // Reset timer progress.
            TimerProgress = 0.0f;
        }

        private void Option()
        {
            Settings.SetInstance(_settings);

            Window window = new OptionView();
            if (window.ShowDialog() == true)
            {
                _settings = Settings.GetInstance();
            }
        }

        private void Pomodoro()
        {
            CurrentTimerType = TimerType.Pomodoro;
            ResidueTimeSpan = _settings.Pomodoro;

            // Reset timer progress to 100 percent.
            TimerProgress = 99.999f;
        }

        private void ShortBreak()
        {
            CurrentTimerType = TimerType.ShortBreak;
            ResidueTimeSpan = _settings.ShortBreak;

            // Reset timer progress to 100 percent.
            TimerProgress = 99.999f;
        }

        private void LongBreak()
        {
            CurrentTimerType = TimerType.LongBreak;
            ResidueTimeSpan = _settings.LongBreak;

            // Reset timer progress to 100 percent.
            TimerProgress = 99.999f;
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

        #region ::Commands::

        private ICommand _startCommand;

        public ICommand StartCommand
        {
            get
            {
                return (_startCommand) ?? (_startCommand = new DelegateCommand(Start));
            }
        }

        private ICommand _pauseCommand;

        public ICommand PauseCommand
        {
            get
            {
                return (_pauseCommand) ?? (_pauseCommand = new DelegateCommand(Pause));
            }
        }

        private ICommand _resetCommand;

        public ICommand ResetCommand
        {
            get
            {
                return (_resetCommand) ?? (_resetCommand = new DelegateCommand(Reset));
            }
        }

        private ICommand _optionCommand;

        public ICommand OptionCommand
        {
            get
            {
                return (_optionCommand) ?? (_optionCommand = new DelegateCommand(Option));
            }
        }

        private ICommand _pomodoroCommand;

        public ICommand PomodoroCommand
        {
            get
            {
                return (_pomodoroCommand) ?? (_pomodoroCommand = new DelegateCommand(Pomodoro));
            }
        }

        private ICommand _shortBreakCommand;

        public ICommand ShortBreakCommand
        {
            get
            {
                return (_shortBreakCommand) ?? (_shortBreakCommand = new DelegateCommand(ShortBreak));
            }
        }

        private ICommand _longBreakCommand;

        public ICommand LongBreakCommand
        {
            get
            {
                return (_longBreakCommand) ?? (_longBreakCommand = new DelegateCommand(LongBreak));
            }
        }

        #endregion
    }
}
