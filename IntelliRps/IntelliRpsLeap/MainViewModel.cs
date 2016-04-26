using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Reactive.Bindings;

namespace IntelliRpsLeap
{
    public class MainViewModel
    {
        public AppModel AppModel { get; } = new AppModel();

        public ReadOnlyReactiveProperty<string> HandShapeImagePath { get; }

        public MainViewModel()
        {
            HandShapeImagePath = AppModel.HandShape
                .Select(s => s.HasValue ? $"Images/{s}-1.png" : null)
                .ToReadOnlyReactiveProperty();
        }
    }
}
