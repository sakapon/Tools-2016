using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using IntelliRps;
using Reactive.Bindings;

namespace IntelliRpsLeap
{
    public class MainViewModel
    {
        public static readonly Func<RpsShape?, string> ToImagePath1 = s => s.HasValue ? $"Images/{s}-1.png" : null;
        public static readonly Func<RpsShape?, string> ToImagePath2 = s => s.HasValue ? $"Images/{s}-2.png" : null;
        public static readonly Func<bool, Visibility> ToVisible = b => b ? Visibility.Visible : Visibility.Hidden;
        public static readonly Func<bool, Visibility> ToHidden = b => b ? Visibility.Hidden : Visibility.Visible;
        public static readonly Func<MatchResult, string> ToMatchResultColor = r =>
            r == MatchResult.Win ? "#FF009900" :
            r == MatchResult.Tie ? "#FFFFBB00" :
            "#FFCC0000";

        public AppModel AppModel { get; } = new AppModel();

        public ReactiveProperty<double> MatchesListWidth { get; } = new ReactiveProperty<double>();
        public ReactiveProperty<double> MatchesListX { get; } = new ReactiveProperty<double>();

        public MainViewModel()
        {
            MatchesListWidth
                .Where(width => width + MatchesListX.Value > 800)
                .Subscribe(_ => ScrollMatchesList());
        }

        void ScrollMatchesList()
        {
            var renderingCount = NextRenderingCount();
            var d = NextXDelta() / renderingCount;

            Observable.Timer(NextScrollDelay(), RenderingPeriod)
                .Take(renderingCount)
                .Subscribe(_ => MatchesListX.Value -= d);
        }

        static readonly TimeSpan RenderingPeriod = TimeSpan.FromSeconds(0.015);
        static readonly Random Random = new Random();
        static double NextXDelta() => Random.Next(150, 250);
        static int NextRenderingCount() => Random.Next(20, 50);
        static TimeSpan NextScrollDelay() => TimeSpan.FromMilliseconds(Random.Next(100, 500));
    }
}
