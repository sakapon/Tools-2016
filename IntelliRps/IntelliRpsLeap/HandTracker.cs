using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using IntelliRps;
using IntelliRps.Core;
using Reactive.Bindings;

namespace IntelliRpsLeap
{
    public class HandTracker
    {
        public LeapManager LeapManager { get; } = new LeapManager();

        public ReadOnlyReactiveProperty<int?> ExtendedFingersCount { get; }
        public ReadOnlyReactiveProperty<RpsShape?> HandShape { get; }

        public HandTracker()
        {
            ExtendedFingersCount = LeapManager.FrameArrived
                .Select(f => f.Hands.Frontmost)
                .Select(h => h.IsValid ? h.Fingers.Count(f => f.IsExtended) : default(int?))
                .ToReadOnlyReactiveProperty();
            HandShape = ExtendedFingersCount
                .Select(f => f.HasValue ? ToShape(f.Value) : default(RpsShape?))
                .ToReadOnlyReactiveProperty();
        }

        static RpsShape ToShape(int fingers) =>
            fingers < 2 ? RpsShape.Rock :
            fingers < 4 ? RpsShape.Scissors :
            RpsShape.Paper;
    }
}
