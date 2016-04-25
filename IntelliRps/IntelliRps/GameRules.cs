using System;
using System.Collections.Generic;
using System.Linq;

namespace IntelliRps
{
    public static class GameRules
    {
        public const int MaxHistoryDegree = 2;

        public static RpsShape GetDefeatingShape(RpsShape opponentShape) => (RpsShape)(((int)opponentShape + 1) % 3);

        public static MatchResult GetResult(RpsShape myShape, RpsShape opponentShape) => (MatchResult)((opponentShape - myShape + 4) % 3);

        public static IEnumerable<string> CreateHistoryKeys() => Enumerable.Range(0, MaxHistoryDegree + 1).SelectMany(CreateHistoryKeys);

        public static IEnumerable<string> CreateHistoryKeys(int degree) =>
            degree == 0 ? new[] { "" } :
            CreateHistoryKeys(degree - 1).SelectMany(k => Enumerable.Range(0, 3).Select(i => $"{k}{i}"));

        static readonly Random Random = new Random();

        public static int NextIndex(double[] probabilities)
        {
            var value = Random.NextDouble();
            var v = 0.0;

            for (var i = 0; i < probabilities.Length; i++)
            {
                v += probabilities[i];
                if (value < v) return i;
            }

            return probabilities.Length - 1;
        }
    }

    public enum RpsShape
    {
        Rock,
        Paper,
        Scissors,
    }

    public enum MatchResult
    {
        Win,
        Tie,
        Loss,
    }
}
