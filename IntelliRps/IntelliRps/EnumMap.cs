using System;
using System.Collections.Generic;
using System.Linq;

namespace IntelliRps
{
    public class EnumCountMap<TEnum>
    {
        public static readonly TEnum[] EnumValues = (TEnum[])Enum.GetValues(typeof(TEnum));

        public int[] Counts { get; } = new int[EnumValues.Length];

        public int this[TEnum key]
        {
            get { return Counts[Array.IndexOf(EnumValues, key)]; }
            set { Counts[Array.IndexOf(EnumValues, key)] = value; }
        }

        public EnumCountMap()
        {
            if (!typeof(TEnum).IsEnum) throw new InvalidOperationException();
        }

        public EnumProbabilityMap<TEnum> GetProbability() => new EnumProbabilityMap<TEnum>(Counts);
    }

    public class EnumProbabilityMap<TEnum>
    {
        public static readonly TEnum[] EnumValues = (TEnum[])Enum.GetValues(typeof(TEnum));

        public double[] Probabilities { get; }

        public double this[TEnum key]
        {
            get { return Probabilities[Array.IndexOf(EnumValues, key)]; }
        }

        public EnumProbabilityMap(double[] ratio)
        {
            if (!typeof(TEnum).IsEnum) throw new InvalidOperationException();
            if (ratio.Length != EnumValues.Length) throw new ArgumentException();

            // Normalizes ratio.
            var sum = ratio.Sum();
            var probs =
                sum == 0 ? Enumerable.Repeat(1.0 / EnumValues.Length, EnumValues.Length) :
                ratio.Select(c => c / sum);
            Probabilities = probs.ToArray();
        }

        public EnumProbabilityMap(int[] ratio)
        {
            if (!typeof(TEnum).IsEnum) throw new InvalidOperationException();
            if (ratio.Length != EnumValues.Length) throw new ArgumentException();

            // Normalizes ratio.
            var sum = ratio.Sum();
            var probs =
                sum == 0 ? Enumerable.Repeat(1.0 / EnumValues.Length, EnumValues.Length) :
                ratio.Select(c => c / (double)sum);
            Probabilities = probs.ToArray();
        }

        public Dictionary<TEnum, double> ToDictionary() => Enumerable.Range(0, EnumValues.Length)
            .ToDictionary(i => EnumValues[i], i => Probabilities[i]);

        public TEnum NextRandomValue() => EnumValues[GameRules.NextIndex(Probabilities)];
    }
}
