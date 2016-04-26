using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using IntelliRps;
using Reactive.Bindings;

namespace IntelliRpsLeap
{
    public class AppModel
    {
        public LeapManager LeapManager { get; } = new LeapManager();

        public ReadOnlyReactiveProperty<int?> ExtendedFingersCount { get; }
        public ReadOnlyReactiveProperty<RpsShape?> HandShape { get; }

        public ReactiveProperty<Game> Game { get; } = new ReactiveProperty<Game>();

        public AppModel()
        {
            ExtendedFingersCount = LeapManager.FrameArrived
                .Select(f => f.Hands.Frontmost)
                .Select(h => h.IsValid ? h.Fingers.Count(f => f.IsExtended) : default(int?))
                .ToReadOnlyReactiveProperty();
            HandShape = ExtendedFingersCount
                .Select(f => f.HasValue ? ToShape(f.Value) : default(RpsShape?))
                .ToReadOnlyReactiveProperty();

            Game.Value = new Game();
        }

        public void StartGame()
        {
            Game.Value = new Game();
        }

        public void StartMatch()
        {
        }

        static RpsShape ToShape(int fingers) =>
            fingers < 2 ? RpsShape.Rock :
            fingers < 4 ? RpsShape.Scissors :
            RpsShape.Paper;
    }
}
