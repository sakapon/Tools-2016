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
        public WavesPlayer WavesPlayer { get; }

        public ReactiveProperty<Game> Game { get; } = new ReactiveProperty<Game>();

        public ReactiveProperty<RpsShape?> ComputerShape { get; } = new ReactiveProperty<RpsShape?>();
        public ReactiveProperty<Scoreline> Scoreline { get; } = new ReactiveProperty<Scoreline>();
        public ReactiveProperty<bool> IsGameConsecutive { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> IsMatchActive { get; } = new ReactiveProperty<bool>();
        public ReactiveCollection<MatchInfo> Matches { get; } = new ReactiveCollection<MatchInfo>();

        static readonly string[] Priming = new[] { "Jan", "Ken", "Pon" };

        public AppModel()
        {
            var sounds = Priming.ToDictionary(n => n, n => $@"Sounds\{n}.wav");
            WavesPlayer = new WavesPlayer(sounds);
            WavesPlayer.LoadAsync();

            Game.Value = new Game();
            Game.Subscribe(g =>
            {
                Scoreline.Value = new Scoreline(g.MatchResultMap);
                Matches.ClearOnScheduler();
                Matches.AddRangeOnScheduler(g.MatchHistory);
            });

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

            IsMatchActive
                .Where(b => b)
                .Subscribe(_ => SetPrimingSounds());
        }

        void SetNextMatchTimer()
        {
            Observable.Timer(TimeSpan.FromSeconds(0.5))
                .Subscribe(_ => IsMatchActive.Value = true);
        }

        void SetPrimingSounds0()
        {
            Observable.Timer(TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.4))
                .Take(3)
                .Subscribe(i => WavesPlayer.Play(Priming[i]));
        }

        void SetPrimingSounds()
        {
            Observable.Timer(TimeSpan.FromSeconds(0.03))
                .Subscribe(i => WavesPlayer.Play(Priming[0]));
            Observable.Timer(TimeSpan.FromSeconds(0.55))
                .Subscribe(i => WavesPlayer.Play(Priming[1]));
            Observable.Timer(TimeSpan.FromSeconds(0.92))
                .Subscribe(i => WavesPlayer.Play(Priming[2]));
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

        public void ResetGame()
        {
            StopConsecutiveGame();

            Game.Value = new Game();
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
