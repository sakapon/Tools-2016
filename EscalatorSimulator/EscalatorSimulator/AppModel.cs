using System;
using System.Collections.Generic;
using System.Linq;

namespace EscalatorSimulator
{
    public class AppModel
    {
        public Escalator[] Escalators { get; }

        public AppModel()
        {
            Escalators = new[]
            {
                new Escalator(new Lane(2.0, 1.0), new Lane(2.0, 3.0) { Interval = TimeSpan.FromSeconds(1.5) }),
                new Escalator(new Lane(2.0, 1.0), new Lane(2.0, 3.0)),
                new Escalator(new Lane(2.0, 1.0), new Lane(2.0, 1.0) { InitialDelay = TimeSpan.FromSeconds(0.5) }),
                new Escalator(new Lane(2.0, 1.0) { Interval = TimeSpan.FromSeconds(0.5) }, new Lane(2.0, 1.0) { Interval = TimeSpan.FromSeconds(0.5) }),
            };
        }
    }

    public static class AppSettings
    {
        public const double WalkingLength = 2.0;
        public const double EscalatingLength = 12.0;

        public const double ScaleToScreen = -20.0;

        public const double CircleRadius = 10.0;
    }
}
