using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTesting
{
    public class MyConsoleWriter
    {
        public static void WriteInfo(string text)
        {
            DateTime now = DateTime.Now;
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.Write($"[{now:HH:mm:ss.fff} | INFO]");
            Console.ResetColor();
            Console.WriteLine(text);
        }
        public static void WriteError(string text)
        {
            DateTime now = DateTime.Now;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.Write($"[{now:HH:mm:ss.fff} | INFO]");
            Console.ResetColor();
            Console.WriteLine(text);
        }
    }
}
