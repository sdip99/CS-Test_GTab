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

        static HashSet<string> categories;
        static char key;
        static Tuple<string, string> names;
        static ConsolePrinter printer = new ConsolePrinter();

        static JsonFeed reqObj = new JsonFeed();
        static void Main(string[] args)
        {
            Console.Clear();
            printer.Value("Press '?' to get instructions or any key to exit").ToString();
            printer.Value("----------------------------------------").ToString();
            if (Console.ReadLine() == "?")
            {
                Console.Clear();
                while (true)
                {
                    printer.Value("----------------------------------------------------------------").ToString();
                    printer.Value("Press c to get categories (Type and wait for the response)").ToString();
                    printer.Value("Press r to get random jokes").ToString();
                    printer.Value("Tired? Press x to exit").ToString();
                    printer.Value("----------------------------------------------------------------").ToString();
                    GetEnteredKey(Console.ReadKey());
                    if (key == 'x')
                    {
                        Environment.Exit(0);
                    }
                    else if (key == 'c')
                    {
                        getCategories();
                        PrintResults();
                    }
                    else if (key == 'r')
                    {
                        randomJokeOptionStart();
                        // printer.Value("\n\nWant to use a random name? y/n (Type and wait for the response)").ToString();
                        // printer.Value("----------------------------------------").ToString();
                        // GetEnteredKey(Console.ReadKey());
                        // if (key == 'y')
                        //     GetNames();
                        // else
                        // {
                        //     printer.Value("*Invalid option selected. Kindly select from the given option").ToString();
                        //     continue;

                        // }
                        // printer.Value("\n\nWant to specify a category? y/n (Type and wait for the response)").ToString();
                        // printer.Value("----------------------------------------").ToString();
                        // GetEnteredKey(Console.ReadKey());
                        // string cat = "";
                        // if (key == 'y')
                        // {
                        //     getCategories();
                        //     PrintResults();
                        //     printer.Value("\nType any of the above category and press 'Enter'").ToString();
                        //     printer.Value("----------------------------------------").ToString();
                        //     cat = Console.ReadLine();
                        // }
                        // printer.Value("\n\nHow many jokes do you want? (1-9)").ToString();
                        // printer.Value("----------------------------------------").ToString();
                        // int n = Int32.Parse(Console.ReadLine());
                        // GetRandomJokes(cat, n);
                        // PrintJokes();
                        // break;

                    }
                    else
                    {
                        Console.Clear();
                        printer.Value("\n*Invalid option selected. Kindly select from the given option").ToString();
                    }

                }
            }
        }

        private static void randomJokeOptionStart()
        {
            printer.Value("\n\nWant to use a random name? y/n (Type and wait for the response)").ToString();
            printer.Value("----------------------------------------").ToString();
            GetEnteredKey(Console.ReadKey());
            if (key == 'y')
            {
                GetNames();
                askCategory();
            }
            else if (key == 'n')
                askCategory();
            else
            {
                Console.Clear();
                printer.Value("\n*Invalid option selected. Type 'y' or 'n'").ToString();
                randomJokeOptionStart();

            }
        }

        private static void askCategory()
        {
            printer.Value("\n\nWant to specify a category? y/n (Type and wait for the response)").ToString();
            printer.Value("----------------------------------------").ToString();
            GetEnteredKey(Console.ReadKey());
            string cat = "";
            if (key == 'y')
            {
                getCategories();
                PrintResults();
                typeCategory();
            }
            else if (key == 'n')
            {
                askJokeCount(cat);
            }
            else
            {
                Console.Clear();
                printer.Value("*Invalid option selected. Type 'y' or 'n'").ToString();
                askCategory();
            }
        }

        private static void typeCategory()
        {
            printer.Value("\nType any of the above category and press 'Enter'").ToString();
            printer.Value("----------------------------------------").ToString();
            string cat = Console.ReadLine();
            if (validateCategory(cat))
                askJokeCount(cat);
            else
            {
                printer.Value("*Invalid value. Kindly entered any of the above category").ToString();
                typeCategory();
            }
        }

        private static Boolean validateCategory(string val)
        {
            return categories.Contains(val);
        }
        private static void askJokeCount(string cat)
        {
            printer.Value("\n\nHow many jokes do you want? (1-9)").ToString();
            printer.Value("----------------------------------------").ToString();
            try
            {
                int n = Int32.Parse(Console.ReadLine());
                GetRandomJokes(cat, n);
                PrintJokes();
            }
            catch (Exception e)
            {
                printer.Value("\n" + e.Message + "Enter the value in range of 1 to 9").ToString();
                askJokeCount(cat);
            }

        }
        private static void PrintResults()
        {
            printer.Value("\n\n Categories: " + string.Join(",\n", categories.ToArray())).ToString();
        }
        private static void PrintJokes()
        {
            printer.Value("\nJokes:\n**********\n-> " + string.Join("\n-> ", results) + "\n**********\n").ToString();
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
                case ConsoleKey.N:
                    key = 'n';
                    break;
                case ConsoleKey.X:
                    key = 'x';
                    break;
                default:
                    key = 'u';
                    break;
            }
        }

        private static void GetRandomJokes(string category, int number)
        {
            reqObj.setURL("https://api.chucknorris.io/");
            // JsonFeed jsonReq = new JsonFeed("https://api.chucknorris.io/");
            if (names?.Item1 != null || names?.Item2 != null)
            {
                results = reqObj.GetRandomJokes(names?.Item1, names?.Item2, category, number);
            }
            else
            {
                results = reqObj.GetRandomJokes(category, number);
            }

        }

        private static void getCategories()
        {
            reqObj.setURL("https://api.chucknorris.io/jokes/");
            // new JsonFeed("https://api.chucknorris.io/jokes/");
            categories = reqObj.GetCategories();
        }

        private static void GetNames()
        {
            reqObj.setURL("https://randomuser.me/api/");
            //new JsonFeed("https://randomuser.me/api/");
            dynamic result = reqObj.Getnames();
            names = Tuple.Create(result.name.first.ToString(), result.name.last.ToString());
        }
    }
}
