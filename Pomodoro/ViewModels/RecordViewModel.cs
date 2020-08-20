using Pomodoro.Entities;
using Pomodoro.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Pomodoro.ViewModels
{
    public class RecordViewModel : INotifyPropertyChanged
    {
        #region ::Fields::

        private Records _records = Records.GetInstance();

        #endregion

        #region ::Properties::

        private DateTime _pickedDate = DateTime.Today;
        
        public DateTime PickedDate
        {
            get
            {
                return _pickedDate;
            }
            set
            {
                _pickedDate = value;
                UpdateData(value);
                RaisePropertyChanged("PickedDate");
            }
        }

        private ObservableCollection<RecordEvent> _eventDatas = null;
        public ObservableCollection<RecordEvent> EventDatas
        {
            get
            {
                if (_eventDatas == null)
                {
                    _eventDatas = new ObservableCollection<RecordEvent>();
                }
                return _eventDatas;
            }
            set
            {
                _eventDatas = value;
                RaisePropertyChanged("EventDatas");
            }
        }

        #endregion

        #region ::Constructor::

        public RecordViewModel()
        {
            UpdateData(DateTime.Today);
        }

        #endregion

        #region ::Data Updater::

        private void UpdateData(DateTime date)
        {
            EventDatas.Clear();

            if (_records.RecordEvents.ContainsKey(date))
            {
                foreach (RecordEvent record in _records.RecordEvents[date])
                {
                    record.TimeSpan.Subtract(new TimeSpan(0, 0, 1));
                    EventDatas.Add(record);
                }
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
