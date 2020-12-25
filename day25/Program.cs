using System;

namespace day25
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("---- Part 01 ----");
            var part1 = FirstStar.Run();
            Console.WriteLine($"Result: {part1}");

            Console.WriteLine("-----------------");
            Console.WriteLine($"1) Copy {part1} to Clipboard");

            Console.WriteLine("Any) Quit");

            var key = Console.ReadKey();

            if (key.Key == ConsoleKey.D1)
                WindowsClipboard.SetText(part1);
        }
    }
}
