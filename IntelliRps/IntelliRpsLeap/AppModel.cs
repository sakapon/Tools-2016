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
        public HandTracker HandTracker { get; } = new HandTracker();

        public ReactiveProperty<Game> Game { get; } = new ReactiveProperty<Game>();

        public ReactiveProperty<RpsShape?> PlayerHandShape { get; } = new ReactiveProperty<RpsShape?>();
        public ReactiveProperty<RpsShape?> ComputerHandShape { get; } = new ReactiveProperty<RpsShape?>();

        public AppModel()
        {
            Game.Value = new Game();
        }

        public void StartGame()
        {
            Game.Value = new Game();
        }

        public void StartMatch()
        {
        }
    }
}
