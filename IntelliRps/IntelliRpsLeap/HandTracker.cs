using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using IntelliRps;
using Reactive.Bindings;

namespace IntelliRpsLeap
{
    public class HandTracker
    {
        public LeapManager LeapManager { get; } = new LeapManager();

        public ReadOnlyReactiveProperty<int?> ExtendedFingersCount { get; }
        public ReadOnlyReactiveProperty<RpsShape?> HandShape { get; }

        public ReactiveProperty<RpsShape?> PlayerShape { get; } = new ReactiveProperty<RpsShape?>();

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

        public void StartPlayerShapeTracking()
        {
            Observable.Repeat(HandShape.Value, 1)
                .Merge(HandShape)
                .Throttle(TimeSpan.FromSeconds(0.2))
                .Take(1)
                .Subscribe(s => PlayerShape.Value = s);
        }

        static RpsShape ToShape(int fingers) =>
            fingers < 2 ? RpsShape.Rock :
            fingers < 4 ? RpsShape.Scissors :
            RpsShape.Paper;
    }
}
