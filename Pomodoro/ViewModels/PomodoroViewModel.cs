using NAudio.Wave;
using Newtonsoft.Json;
using Pomodoro.Commands;
using Pomodoro.Entities;
using Pomodoro.Models;
using Pomodoro.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace Pomodoro.ViewModels
{
    public class PomodoroViewModel : INotifyPropertyChanged
    {
        #region ::Fields::

        private Settings _settings;

        private Records _records;

        private Timer _timer;

        private readonly Stopwatch _stopwatch;

        private DateTime _startTime;

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

            // Initialize records.
            _records = Records.GetInstance();

            if (_records.RecordEvents == null)
                _records.RecordEvents = new Dictionary<DateTime, List<RecordEvent>>();

            // Initialize timer.
            _timer = new Timer(1000);
            _timer.Enabled = true;
            _timer.Stop();
            _timer.Elapsed += OnTimerElapsed; // Add timer elapsed event subscriber.

            // Initialize stopwatch.
            _stopwatch = new Stopwatch();
        }

        #endregion

        #region ::Recording-Related::

        private void AddRecord()
        {
            _stopwatch.Stop();

            List<RecordEvent> recordEventList = new List<RecordEvent>();
            RecordEvent record = new RecordEvent(CurrentTimerType, _startTime, DateTime.Now, _stopwatch.Elapsed);
            recordEventList.Add(record);

            if (_records.RecordEvents.ContainsKey(DateTime.Today))
            {
                _records.RecordEvents[DateTime.Today].Add(record);
            }
            else
            {
                _records.RecordEvents.Add(DateTime.Today, recordEventList);
            }
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
                    // If the remaining time is 0 seconds, and the timer type is None.
                    if (CurrentTimerType == TimerType.None)
                    {
                        Reset();
                    }
                    else
                    {
                        AddRecord();
                    }

                    // Show balloon tip.
                    if (_settings.IsNotify)
                    {
                        PomodoroView.NotifyIcon.Visible = true;
                        PomodoroView.NotifyIcon.ShowBalloonTip(3000, "Pomodoro", "Now time's up!", System.Windows.Forms.ToolTipIcon.Info);
                        PomodoroView.NotifyIcon.Visible = false;
                    }

                    // Add pomodoro count.
                    if (CurrentTimerType == TimerType.Pomodoro)
                    {
                        Count++;
                    }

                    // Check timer state automatically and restart timer.
                    if (_settings.IsAutomatic)
                    {
                        if (CurrentTimerType == TimerType.Pomodoro && _settings.IsAutomaticUseLongBreak)
                        {
                            Reset();
                            LongBreak();
                        }
                        else if (CurrentTimerType == TimerType.Pomodoro && !_settings.IsAutomaticUseLongBreak)
                        {
                            Reset();
                            ShortBreak();
                        }
                        else if (CurrentTimerType == TimerType.ShortBreak || CurrentTimerType == TimerType.LongBreak)
                        {
                            Reset();
                            Pomodoro();
                        }
                    }
                    else
                    {
                        Reset();
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
            
            // Run stopwatch and initialize diagnostics information.
            _stopwatch.Start();
            _startTime = DateTime.Now;
        }

        private void Pause()
        {
            IsPlay = false;
            _timer.Stop();

            _stopwatch.Stop();
        }

        private void Reset()
        {
            IsPlay = false;
            _timer.Stop();

            _stopwatch.Reset();

            CurrentTimerType = TimerType.None;

            // Reset residue time span.
            ResidueTimeSpan = new TimeSpan(0, 0, 0);

            // Reset timer progress.
            TimerProgress = 0.0f;
        }

        private void Record()
        {
            Records.SetInstance(_records);

            Window window = new RecordView();
            if (window.ShowDialog() == true)
            {
                _records = Records.GetInstance();
            }
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

            Start();
        }

        private void ShortBreak()
        {
            CurrentTimerType = TimerType.ShortBreak;
            ResidueTimeSpan = _settings.ShortBreak;

            // Reset timer progress to 100 percent.
            TimerProgress = 99.999f;

            Start();
        }

        private void LongBreak()
        {
            CurrentTimerType = TimerType.LongBreak;
            ResidueTimeSpan = _settings.LongBreak;

            // Reset timer progress to 100 percent.
            TimerProgress = 99.999f;

            Start();
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

        private ICommand _recordCommand;

        public ICommand RecordCommand
        {
            get
            {
                return (_recordCommand) ?? (_recordCommand = new DelegateCommand(Record));
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
