using System;

namespace day01
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = InputParser.Parse("input.txt");

            Console.WriteLine("---- Part 01 ----");
            var part1 = FirstStar.Run(input);
            Console.WriteLine($"Result: {part1.Text}");

            Console.WriteLine("---- Part 02 ----");
            var part2 = SecondStar.Run(input);
            Console.WriteLine($"Result: {part2.Text}");

            Console.WriteLine("-----------------");
            Console.WriteLine($"1) Copy {part1} to Clipboard");
            Console.WriteLine($"2) Copy {part2} to Clipboard");

            Console.WriteLine("Any) Quit");

            var key = Console.ReadKey();

            if (key.Key == ConsoleKey.D1)
            {
                WindowsClipboard.SetText(part1.Text);
            }
            else if (key.Key == ConsoleKey.D2)
            {
                WindowsClipboard.SetText(part2.Text);
            }
        }
    }
}
