using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Reactive.Bindings;

namespace IntelliRpsLeap
{
    public class AppModel
    {
        public LeapManager LeapManager { get; } = new LeapManager();

        public ReadOnlyReactiveProperty<int?> ExtendedFingersCount { get; }

        public AppModel()
        {
            ExtendedFingersCount = LeapManager.FrameArrived
                .Select(f => f.Hands.Frontmost)
                .Select(h => h.IsValid ? h.Fingers.Count(f => f.IsExtended) : default(int?))
                .ToReadOnlyReactiveProperty();
        }
    }
}
