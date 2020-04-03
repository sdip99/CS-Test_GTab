using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    class JsonFeed
    {
        static string _url = "";

        public JsonFeed() { }
        public JsonFeed(string endpoint)
        {
            _url = endpoint;
        }

        public static string[] GetRandomJokes(string firstname, string lastname, string category, int n)
        {
            String[] jokes = GetRandomJokes(category, n);
            if (firstname != null && lastname != null)
            {
                for (int i = 0; i < jokes.Length; i++)
                {
                    jokes[i] = jokeWithRandomName(jokes[i], firstname, lastname);
                }
            }
            return jokes;
        }

        public static string[] GetRandomJokes(string cat, int count)
        {
            String[] jokes = new String[count];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_url);
            string url = _url + "jokes/random";
            if (!string.IsNullOrEmpty(cat))
            {
                url += "?category=" + cat;
            }
            dynamic jokeObj;
            for (int i = 0; i < count; i++)
            {
                jokeObj = JsonConvert.DeserializeObject<dynamic>(client.GetStringAsync(url).Result);
                jokes[i] = jokeObj.value.ToString();
            }
            return jokes;
        }

        // public static string getJoke(string cat)
        // {
        //     HttpClient client = new HttpClient();
        //     client.BaseAddress = new Uri(_url);
        //     if (cat != null || cat != "")
        //     {
        //         cat = "?category=" + cat;
        //     }
        //     dynamic jokeObj = JsonConvert.DeserializeObject<dynamic>(client.GetStringAsync("jokes/random/" + cat).Result);
        //     return jokeObj.value.ToString();
        // }

        public static string jokeWithRandomName(string joke, string fName, string lName)
        {
            int index = joke.IndexOf("Chuck Norris");
            string prefix = joke.Substring(0, index);
            string suffix = joke.Substring(index + "Chuck Norris".Length, joke.Length - (index + "Chuck Norris".Length));
            string jokeWithRandomName = prefix + "" + fName + " " + lName + suffix;
            return jokeWithRandomName;
        }

        public static dynamic Getnames()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_url);
            // var result = client.GetStringAsync("").Result;
            var jsonResult = JsonConvert.DeserializeObject<dynamic>(client.GetStringAsync("").Result);
            return jsonResult.results[0];
        }

        public static string[] GetCategories()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_url);
            return new string[] { Task.FromResult(client.GetStringAsync("categories").Result).Result };
        }
    }
}
