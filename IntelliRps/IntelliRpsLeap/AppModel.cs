using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ReactiveProperty<bool> IsGameConsecutive { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> IsMatchActive { get; } = new ReactiveProperty<bool>();
        public ReactiveCollection<MatchInfo> Matches { get; } = new ReactiveCollection<MatchInfo>();

        public AppModel()
        {
            Game.Value = new Game();

            HandTracker.PlayerShape
                .Where(s => s.HasValue)
                .Do(s =>
                {
                    var computerShape = Game.Value.NextComputerShape();
                    Game.Value.AddMatchInfo(s.Value, computerShape);

                    ComputerShape.Value = computerShape;
                    IsMatchActive.Value = false;
                    Scoreline.Value = new Scoreline(Game.Value.MatchResultMap);
                    Matches.AddOnScheduler(Game.Value.MatchHistory.Last());
                })
                .Where(_ => IsGameConsecutive.Value)
                .Subscribe(_ => SetNextMatchTimer());
            HandTracker.PlayerShape
                .Where(s => !s.HasValue)
                .Do(s =>
                {
                    ComputerShape.Value = null;
                    IsMatchActive.Value = false;
                })
                .Where(_ => IsGameConsecutive.Value)
                .Subscribe(_ => SetNextMatchTimer());
        }

        void SetNextMatchTimer()
        {
            Observable.Timer(TimeSpan.FromSeconds(0.5))
                .Subscribe(_ => IsMatchActive.Value = true);
        }

        public void StartConsecutiveGame()
        {
            IsGameConsecutive.Value = true;
            IsMatchActive.Value = true;
        }

        public void StartJudgment()
        {
            HandTracker.TrackPlayerShape();
        }

        public void StopConsecutiveGame()
        {
            IsGameConsecutive.Value = false;
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
