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

        public AppModel AppModel { get; } = new AppModel();

        public ReadOnlyReactiveProperty<Visibility> MatchShapeVisibility { get; }

        public MainViewModel()
        {
            MatchShapeVisibility = AppModel.IsMatchActive
                .Select(b => b ? Visibility.Hidden : Visibility.Visible)
                .ToReadOnlyReactiveProperty();
        }
    }
}
