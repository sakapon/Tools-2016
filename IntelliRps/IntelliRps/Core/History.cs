using System;
using System.Diagnostics;

namespace IntelliRps.Core
{
    public class History : QueueCollection<DateTime>
    {
        public TimeSpan? MaxSpan { get; private set; }

        public History() { }
        public History(int maxCount) : base(maxCount) { }

        public History(TimeSpan maxSpan)
        {
            MaxSpan = maxSpan;
        }

        public History(int maxCount, TimeSpan maxSpan)
            : base(maxCount)
        {
            MaxSpan = maxSpan;
        }

        public void Record()
        {
            var now = DateTime.Now;

            if (MaxSpan.HasValue)
            {
                while (Count > 0 && now - this[0] >= MaxSpan)
                {
                    RemoveAt(0);
                }
            }

            Add(now);
        }
    }

    public class History<T> : QueueCollection<HistoryItem<T>>
    {
        public TimeSpan? MaxSpan { get; private set; }

        public History() { }
        public History(int maxCount) : base(maxCount) { }

        public History(TimeSpan maxSpan)
        {
            MaxSpan = maxSpan;
        }

        public History(int maxCount, TimeSpan maxSpan)
            : base(maxCount)
        {
            MaxSpan = maxSpan;
        }

        public void Record(T value)
        {
            var now = DateTime.Now;

            if (MaxSpan.HasValue)
            {
                while (Count > 0 && now - this[0].Timestamp >= MaxSpan)
                {
                    RemoveAt(0);
                }
            }

            Add(new HistoryItem<T>(value, now));
        }
    }

    [DebuggerDisplay(@"\{{Timestamp}: {Value}\}")]
    public struct HistoryItem<T>
    {
        public T Value { get; private set; }
        public DateTime Timestamp { get; private set; }

        public HistoryItem(T value, DateTime timestamp)
            : this()
        {
            Value = value;
            Timestamp = timestamp;
        }
    }
}
