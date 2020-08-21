using Pomodoro.Entities;
using System;
using System.Collections.Generic;

namespace Pomodoro.Models
{
    public class Records
    {
        #region ::Singleton Pattern::

        // Singleton pattern for saving settings.
        [NonSerialized]
        private static Records _instance;

        public static Records Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Records();

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        #endregion

        #region ::Properties::

        public Dictionary<DateTime, List<RecordEvent>> RecordEvents { get; set; }

        #endregion
    }
}
