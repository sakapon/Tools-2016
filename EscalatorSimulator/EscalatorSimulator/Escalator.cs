using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Media.Animation;
using Reactive.Bindings;

namespace EscalatorSimulator
{
    public class Escalator
    {
        public Lane Lane_L { get; }
        public Lane Lane_R { get; }

        public ReadOnlyReactiveProperty<int> People1_L { get; }
        public ReadOnlyReactiveProperty<int> People1_R { get; }
        public ReadOnlyReactiveProperty<int> People1 { get; }

        public ReadOnlyReactiveProperty<int> People2_L { get; }
        public ReadOnlyReactiveProperty<int> People2_R { get; }
        public ReadOnlyReactiveProperty<int> People2 { get; }

        public Escalator(Lane left, Lane right)
        {
            Lane_L = left;
            Lane_R = right;

            People1_L = Observable.Interval(Lane_L.Interval)
                .Select(i => (int)i + 1)
                .DelaySubscription(Lane_L.InitialDelay)
                .ToReadOnlyReactiveProperty(mode: ReactivePropertyMode.DistinctUntilChanged);
            People1_R = Observable.Interval(Lane_R.Interval)
                .Select(i => (int)i + 1)
                .DelaySubscription(Lane_R.InitialDelay)
                .ToReadOnlyReactiveProperty(mode: ReactivePropertyMode.DistinctUntilChanged);
            People1 = People1_L.Merge(People1_R)
                .Select(_ => People1_L.Value + People1_R.Value)
                .ToReadOnlyReactiveProperty();

            People2_L = Observable.Interval(Lane_L.Interval)
                .Select(i => (int)i + 1)
                .DelaySubscription(Lane_L.InitialDelay + Lane_L.KeyFrames[3].KeyTime.TimeSpan)
                .ToReadOnlyReactiveProperty(mode: ReactivePropertyMode.DistinctUntilChanged);
            People2_R = Observable.Interval(Lane_R.Interval)
                .Select(i => (int)i + 1)
                .DelaySubscription(Lane_R.InitialDelay + Lane_R.KeyFrames[3].KeyTime.TimeSpan)
                .ToReadOnlyReactiveProperty(mode: ReactivePropertyMode.DistinctUntilChanged);
            People2 = People2_L.Merge(People2_R)
                .Select(_ => People2_L.Value + People2_R.Value)
                .ToReadOnlyReactiveProperty();
        }
    }

    public class Lane
    {
        public double WalkingVelocity { get; }
        public double EscalatingVelocity { get; }
        public DoubleKeyFrameCollection KeyFrames { get; }

        public TimeSpan InitialDelay { get; set; }
        public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(1.0);

        public Lane(double walkingVelocity, double escalatingVelocity)
        {
            WalkingVelocity = walkingVelocity;
            EscalatingVelocity = escalatingVelocity;

            KeyFrames = InitializeKeyFrames();
        }

        DoubleKeyFrameCollection InitializeKeyFrames()
        {
            var length1 = AppSettings.WalkingLength;
            var length2 = length1 + AppSettings.EscalatingLength;
            var length3 = length2 + AppSettings.WalkingLength;
            var time1 = AppSettings.WalkingLength / WalkingVelocity;
            var time2 = time1 + AppSettings.EscalatingLength / EscalatingVelocity;
            var time3 = time2 + AppSettings.WalkingLength / WalkingVelocity;

            var kfs = new DoubleKeyFrameCollection();
            kfs.Add(new LinearDoubleKeyFrame(0, TimeSpan.Zero));
            kfs.Add(new LinearDoubleKeyFrame(AppSettings.ScaleToScreen * length1, TimeSpan.FromSeconds(time1)));
            kfs.Add(new LinearDoubleKeyFrame(AppSettings.ScaleToScreen * length2, TimeSpan.FromSeconds(time2)));
            kfs.Add(new LinearDoubleKeyFrame(AppSettings.ScaleToScreen * length3, TimeSpan.FromSeconds(time3)));
            return kfs;
        }

        public DoubleAnimationUsingKeyFrames CreateAnimation()
        {
            return new DoubleAnimationUsingKeyFrames { KeyFrames = KeyFrames.Clone() };
        }
    }
}
