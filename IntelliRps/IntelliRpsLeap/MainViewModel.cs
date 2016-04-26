using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using IntelliRps;
using Reactive.Bindings;

namespace IntelliRpsLeap
{
    public class MainViewModel
    {
        public static readonly Func<RpsShape?, string> ToImagePath1 = s => s.HasValue ? $"Images/{s}-1.png" : null;
        public static readonly Func<RpsShape?, string> ToImagePath2 = s => s.HasValue ? $"Images/{s}-2.png" : null;

        public AppModel AppModel { get; } = new AppModel();

        public MainViewModel()
        {
        }
    }
}
