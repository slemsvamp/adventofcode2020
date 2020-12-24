using System;

namespace day24
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = InputParser.Parse("input.txt");

            Console.WriteLine("---- Part 01 ----");
            var part1 = FirstStar.Run(input);
            Console.WriteLine($"Result: {part1.Result}");

            Console.WriteLine("---- Part 02 ----");
            var part2 = SecondStar.Run(part1.Hexes);
            Console.WriteLine($"Result: {part2}");

            Console.WriteLine("-----------------");
            Console.WriteLine($"1) Copy {part1.Result} to Clipboard");
            Console.WriteLine($"2) Copy {part2} to Clipboard");

            Console.WriteLine("Any) Quit");

            var key = Console.ReadKey();

            if (key.Key == ConsoleKey.D1)
            {
                WindowsClipboard.SetText(part1.Result);
            }
            else if (key.Key == ConsoleKey.D2)
            {
                WindowsClipboard.SetText(part2);
            }
        }
    }
}
