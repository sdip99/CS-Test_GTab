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
        static string[] results;

        static HashSet<string> categories;
        static char key;
        static Tuple<string, string> names;
        static ConsolePrinter printer = new ConsolePrinter();

        // Creating and utilizing the single request object
        static JsonFeed reqObj = new JsonFeed();
        static void Main(string[] args)
        {
            Console.Clear();
            printer.Value("Press '?' to get instructions or any key to exit").ToString();
            printer.Value("-------------------------------------------------").ToString();
            if (Console.ReadLine() == "?")
            {
                Console.Clear();
                while (true)
                {
                    // Exception handling, In case of any error
                    try
                    {
                        names = null; // setting names to null for avoiding any possible conflicts
                        printer.Value("----------------------------------------------------------------").ToString();
                        printer.Value("Press c to get categories").ToString();
                        printer.Value("Press r to get random jokes").ToString();
                        printer.Value("Tired? Press x to exit").ToString();
                        printer.Value("----------------------------------------------------------------").ToString();
                        GetEnteredKey(Console.ReadLine());
                        if (key == 'x') // Exit the application for 'x' input
                        {
                            GoodByeMessage();
                            Environment.Exit(0);
                        }
                        else if (key == 'c') // Prints the cateegory values for 'c' input
                        {
                            getCategories();
                            Console.Clear();
                            PrintCategories();
                        }
                        else if (key == 'r') // Random joke option ('r') selection
                        {
                            randomJokeOptionStart(); // ask radom user and category selection
                        }
                        else // Invalid selection - values other than 'c','r','x'
                        {
                            Console.Clear();
                            printer.Value("\n*Invalid option selected. Kindly select from the given option").ToString();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Clear();
                        printer.Value("\n* " + e.Message + "\nCotact support@whyThisIsHappening.ca or Try Again!").ToString();
                    }

                }
            }
            GoodByeMessage(); // Prints goodbye messgae
        }


        /* Starting point for random joke 'r' selection. 
            This will asks user for any random name user wants to use in the joke*/
        private static void randomJokeOptionStart()
        {
            printer.Value("\n\nWant to use a random name? y/n").ToString();
            printer.Value("----------------------------------------").ToString();
            GetEnteredKey(Console.ReadLine());
            if (key == 'y') // 'y' selection
            {
                //API call for getting random user name
                GetNames();
                // Ask next question
                askCategory();
            }
            else if (key == 'n') // 'n' selection
                                 // Ask next question
                askCategory();
            else // Error handling
            {
                Console.Clear();
                printer.Value("\n* Invalid option selected. Type 'y' or 'n'").ToString();
                randomJokeOptionStart(); // Ask agin for valid input

            }
        }


        /* Asking user for providing the category of the joke*/
        private static void askCategory()
        {
            printer.Value("\n\nWant to specify a category? y/n").ToString();
            printer.Value("----------------------------------------").ToString();
            GetEnteredKey(Console.ReadLine());

            string cat = "";
            if (key == 'y') // 'y' selection
            {
                typeCategory(); // Ask next question
            }
            else if (key == 'n')
            {
                askJokeCount(cat); // 'n' selection would ask next question
            }
            else // Error handling
            {
                Console.Clear();
                printer.Value("* Invalid option selected. Type 'y' or 'n'").ToString();
                askCategory(); // Ask agin for valid input
            }
        }


        /* Asking user to type the category of the joke*/
        private static void typeCategory()
        {
            getCategories();
            PrintCategories();// Print category for better user experience
            printer.Value("\nType any of the above category").ToString();
            printer.Value("----------------------------------------").ToString();
            string cat = Console.ReadLine();
            if (validateCategory(cat)) // Validating user input with available category values
                askJokeCount(cat); // Ask next question
            else // Error handling
            {
                Console.Clear();
                printer.Value("\n* Invalid value. Kindly entered any of the below category").ToString();
                typeCategory(); // Ask agian for valid input
            }
        }


        /* Method for checking valid categories */
        private static Boolean validateCategory(string val)
        {
            return categories.Contains(val);
        }


        /*  Ask for the number of joke user want to see*/
        private static void askJokeCount(string cat)
        {
            printer.Value("\n\nHow many jokes do you want? (1-9)").ToString();
            printer.Value("----------------------------------------").ToString();
            try
            {
                int n = Int32.Parse(Console.ReadLine());
                GetRandomJokes(cat, n); // Call the API and returns the number of joke
                PrintJokes();
            }
            catch (FormatException e) // Error handling
            {
                printer.Value("\n* " + e.Message + " Enter the value in range of 1 to 9").ToString();
                askJokeCount(cat); // Ask again for valid input
            }
        }


        /* Method for printing the categories value */
        private static void PrintCategories()
        {
            printer.Value("\nCategories:\n***********\n " + string.Join(",", categories.ToArray()) + "\n").ToString();
        }


        /* Method for printing the Goodbye message */
        private static void GoodByeMessage()
        {
            Console.Clear();
            printer.Value("\nAdios, amigos! \n").ToString();
        }


        /* Method for printing the jokes */
        private static void PrintJokes()
        {
            Console.Clear();
            printer.Value("\nJokes:\n**********\n-> " + string.Join("\n-> ", results) + "\n**********\n").ToString();
        }


        /* Method for setting the key value based on user in put */
        private static void GetEnteredKey(string k)
        {
            // converting to lower case, in case of any uppercase input
            string s = k.ToLower();
            switch (s)
            {
                case "c":
                    key = 'c';
                    break;
                case "1":
                    key = '1';
                    break;
                case "2":
                    key = '2';
                    break;
                case "3":
                    key = '3';
                    break;
                case "4":
                    key = '4';
                    break;
                case "5":
                    key = '5';
                    break;
                case "6":
                    key = '6';
                    break;
                case "7":
                    key = '7';
                    break;
                case "8":
                    key = '8';
                    break;
                case "9":
                    key = '9';
                    break;
                case "r":
                    key = 'r';
                    break;
                case "y":
                    key = 'y';
                    break;
                case "n":
                    key = 'n';
                    break;
                case "x":
                    key = 'x';
                    break;
                default:
                    key = 'u';
                    break;
            }
        }


        /* Method for fetching the radom jokes from the API and setting it to string[] object*/
        private static void GetRandomJokes(string category, int number)
        {
            // setting an object url
            reqObj.setURL("https://api.chucknorris.io/");
            if (names?.Item1 != null || names?.Item2 != null)
            {
                results = reqObj.GetRandomJokes(names?.Item1, names?.Item2, category, number);
            }
            else
            {
                results = reqObj.GetRandomJokes(category, number);
            }

        }


        /* Method for getting the category values from the API and setting it to HashSet<string> object*/
        private static void getCategories()
        {

            reqObj.setURL("https://api.chucknorris.io/jokes/");
            categories = reqObj.GetCategories();
        }


        /* Method for getting random names from the API and setting (firstName, lastName) to Tuple object */
        private static void GetNames()
        {
            reqObj.setURL("https://randomuser.me/api/");
            dynamic result = reqObj.Getnames();
            names = Tuple.Create(result.name.first.ToString(), result.name.last.ToString());
        }
    }
}
