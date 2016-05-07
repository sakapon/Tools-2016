using System;
using System.Collections.Generic;
using System.Linq;
using IntelliRps;

namespace IntelliRpsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();

            Console.WriteLine("Press [1][2][3] keys to play.");
            Console.WriteLine("Press [Enter] key to exit.");
            Console.WriteLine();

            while (true)
            {
                var consoleKey = Console.ReadKey();
                if (consoleKey.Key == ConsoleKey.Enter) return;
                switch (consoleKey.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.D2:
                    case ConsoleKey.D3:
                        break;
                    default:
                        continue;
                }

                var playerShape = (RpsShape)(int.Parse(consoleKey.KeyChar.ToString()) - 1);
                var computerShape = game.NextComputerShape();
                game.AddMatchInfo(playerShape, computerShape);

                Console.Write(" ");
                Console.WriteLine($"YOU: {ToString(playerShape)} - {ToString(computerShape)} :COM");
                Console.WriteLine($"{game.MatchResultMap[MatchResult.Win]} wins - {game.MatchResultMap[MatchResult.Tie]} ties - {game.MatchResultMap[MatchResult.Loss]} losses");
            }
        }

        static string ToString(RpsShape shape)
        {
            switch (shape)
            {
                case RpsShape.Rock:
                    return "Ｏ";
                case RpsShape.Paper:
                    return "Ｗ";
                case RpsShape.Scissors:
                    return "Ｖ";
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
