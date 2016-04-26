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

        public ReactiveProperty<RpsShape?> ComputerShape { get; } = new ReactiveProperty<RpsShape?>();
        public ReactiveProperty<Scoreline> Scoreline { get; } = new ReactiveProperty<Scoreline>();

        public AppModel()
        {
            Game.Value = new Game();

            HandTracker.PlayerShape
                .Where(s => s.HasValue)
                .Subscribe(s =>
                {
                    var computerShape = Game.Value.NextComputerShape();
                    ComputerShape.Value = computerShape;
                    Game.Value.AddMatchInfo(s.Value, computerShape);
                    Scoreline.Value = new Scoreline(Game.Value.MatchResultMap);
                });
        }

        public void StartGame()
        {
            Game.Value = new Game();
        }

        public void StartMatch()
        {
        }
    }

    public struct Scoreline
    {
        public int Wins { get; }
        public int Ties { get; }
        public int Losses { get; }

        public Scoreline(EnumCountMap<MatchResult> matchResultMap)
        {
            Wins = matchResultMap[MatchResult.Win];
            Ties = matchResultMap[MatchResult.Tie];
            Losses = matchResultMap[MatchResult.Loss];
        }
    }
}
