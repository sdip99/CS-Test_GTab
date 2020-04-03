using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    class Program
    {
        static string[] results = new string[50];
        static char key;
        static Tuple<string, string> names;
        static ConsolePrinter printer = new ConsolePrinter();

        static void Main(string[] args)
        {
            printer.Value("Press ? to get instructions.").ToString();
            if (Console.ReadLine() == "?")
            {
                while (true)
                {
                    printer.Value("Press c to get categories").ToString();
                    printer.Value("Press r to get random jokes").ToString();
                    GetEnteredKey(Console.ReadKey());
                    if (key == 'c')
                    {
                        getCategories();
                        PrintResults();
                    }
                    if (key == 'r')
                    {
                        printer.Value("\nWant to use a random name? y/n").ToString();
                        GetEnteredKey(Console.ReadKey());
                        if (key == 'y')
                            GetNames();
                        printer.Value("\nWant to specify a category? y/n").ToString();
                        GetEnteredKey(Console.ReadKey());
                        string cat = "";
                        if (key == 'y')
                        {
                            printer.Value("\nEnter a category;").ToString();
                            cat = Console.ReadLine();
                        }
                        printer.Value("\nHow many jokes do you want? (1-9)").ToString();
                        int n = Int32.Parse(Console.ReadLine());
                        GetRandomJokes(cat, n);
                        PrintResults();
                    }
                    names = null;
                }
            }

        }

        private static void PrintResults()
        {
            printer.Value("[\n" + string.Join(",\n", results) + "\n]").ToString();
        }

        private static void GetEnteredKey(ConsoleKeyInfo consoleKeyInfo)
        {
            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.C:
                    key = 'c';
                    break;
                case ConsoleKey.D0:
                    key = '0';
                    break;
                case ConsoleKey.D1:
                    key = '1';
                    break;
                case ConsoleKey.D3:
                    key = '3';
                    break;
                case ConsoleKey.D4:
                    key = '4';
                    break;
                case ConsoleKey.D5:
                    key = '5';
                    break;
                case ConsoleKey.D6:
                    key = '6';
                    break;
                case ConsoleKey.D7:
                    key = '7';
                    break;
                case ConsoleKey.D8:
                    key = '8';
                    break;
                case ConsoleKey.D9:
                    key = '9';
                    break;
                case ConsoleKey.R:
                    key = 'r';
                    break;
                case ConsoleKey.Y:
                    key = 'y';
                    break;
                default:
                    key = 'n';
                    break;
            }
        }

        private static void GetRandomJokes(string category, int number)
        {
            new JsonFeed("https://api.chucknorris.io/");
            results = JsonFeed.GetRandomJokes(names?.Item1, names?.Item2, category, number);
        }

        private static void getCategories()
        {
            new JsonFeed("https://api.chucknorris.io/jokes/");
            results = JsonFeed.GetCategories();
        }

        private static void GetNames()
        {
            new JsonFeed("https://randomuser.me/api/");
            dynamic result = JsonFeed.Getnames();
            names = Tuple.Create(result.name.first.ToString(), result.name.last.ToString());
        }
    }
}
