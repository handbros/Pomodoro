using System;

namespace Pomodoro.Entities
{
    public class RecordEvent
    {
        public TimerType TimerType { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public TimeSpan TimeSpan { get; set; }

        public RecordEvent(TimerType timerType, DateTime startTime, DateTime endTime, TimeSpan timeSpan)
        {
            TimerType = timerType;
            StartTime = startTime;
            EndTime = endTime;
            TimeSpan = timeSpan;
        }
    }
}
