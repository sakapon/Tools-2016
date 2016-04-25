using System;
using System.Collections.Generic;
using System.Linq;

namespace IntelliRps
{
    public class Game
    {
        string ShapeHistory = "";
        Dictionary<string, EnumCountMap<RpsShape>> ShapeHistoryMap = GameRules.CreateHistoryKeys().ToDictionary(k => k, _ => new EnumCountMap<RpsShape>());

        public List<MatchInfo> MatchHistory { get; } = new List<MatchInfo>();
        public EnumCountMap<MatchResult> MatchResultMap { get; } = new EnumCountMap<MatchResult>();

        public RpsShape NextComputerShape()
        {
            var probs = GetProbabilities();

            // --- Makes it hard for the player to win.
            var d = probs.ToDictionary();
            var top2 = d
                .OrderByDescending(p => p.Value)
                .Take(2)
                .ToArray();
            if (0.65 * top2[0].Value < top2[1].Value && top2[0].Value + top2[1].Value > 0.8)
            {
                var result = GameRules.GetResult(top2[0].Key, top2[1].Key);
                return result == MatchResult.Win ? top2[0].Key : top2[1].Key;
            }
            // ---

            var playerShape = probs.NextRandomValue();
            return GameRules.GetDefeatingShape(playerShape);
        }

        public void AddMatchInfo(RpsShape playerShape, RpsShape computerShape)
        {
            var degrees = Enumerable.Range(0, GameRules.MaxHistoryDegree + 1)
                .TakeWhile(d => ShapeHistory.Length >= d);
            foreach (var d in degrees)
            {
                var key = ShapeHistory.Substring(ShapeHistory.Length - d, d);
                ShapeHistoryMap[key][playerShape] += 1;
            }

            ShapeHistory += ((int)playerShape).ToString();

            var match = new MatchInfo(playerShape, computerShape);
            MatchHistory.Add(match);

            MatchResultMap[match.PlayerResult] += 1;
        }

        EnumProbabilityMap<RpsShape> GetProbabilities0()
        {
            return ShapeHistoryMap[""].GetProbability();
        }

        EnumProbabilityMap<RpsShape> GetProbabilities()
        {
            if (ShapeHistory.Length == 0) return GetProbabilities0();

            var degreeProbs = Enumerable.Range(0, GameRules.MaxHistoryDegree + 1)
                .TakeWhile(d => ShapeHistory.Length >= d)
                .Select(d => ShapeHistory.Substring(ShapeHistory.Length - d, d))
                .Select(k => ShapeHistoryMap[k].GetProbability())
                .ToArray();

            var ratio = EnumProbabilityMap<RpsShape>.EnumValues
                .Select(s => degreeProbs.Sum(r => r[s] * r[s]))
                .ToArray();
            return new EnumProbabilityMap<RpsShape>(ratio);
        }
    }

    public class MatchInfo
    {
        public RpsShape PlayerShape { get; }
        public RpsShape ComputerShape { get; }

        public MatchResult PlayerResult => GameRules.GetResult(PlayerShape, ComputerShape);
        public MatchResult ComputerResult => GameRules.GetResult(ComputerShape, PlayerShape);

        public MatchInfo(RpsShape playerShape, RpsShape computerShape)
        {
            PlayerShape = playerShape;
            ComputerShape = computerShape;
        }
    }
}
