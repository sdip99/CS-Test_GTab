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
        private string _url = "";
        private HashSet<string> catSet;

        private HttpClient client;
        public JsonFeed() { }
        public JsonFeed(string endpoint)
        {
            _url = endpoint;
            this.client = new HttpClient();
            client.BaseAddress = new Uri(endpoint);
        }

        public void setURL(string url)
        {
            this._url = url;
            this.client = new HttpClient();
            client.BaseAddress = new Uri(url);
        }

        public string[] GetRandomJokes(string firstname, string lastname, string category, int n)
        {
            String[] jokes = GetRandomJokes(category, n);
            for (int i = 0; i < jokes.Length; i++)
            {
                jokes[i] = jokeWithRandomName(jokes[i], firstname, lastname);
            }
            return jokes;
        }

        public string[] GetRandomJokes(string cat, int count)
        {
            String[] jokes = new String[count];
            // HttpClient client = new HttpClient();
            // client.BaseAddress = new Uri(_url);
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

        public string jokeWithRandomName(string joke, string fName, string lName)
        {
            int index = joke.IndexOf("Chuck Norris");
            string prefix = joke.Substring(0, index);
            string suffix = joke.Substring(index + "Chuck Norris".Length, joke.Length - (index + "Chuck Norris".Length));
            string jokeWithRandomName = prefix + "" + fName + " " + lName + suffix;
            return jokeWithRandomName;
        }

        public dynamic Getnames()
        {
            // HttpClient client = new HttpClient();
            // client.BaseAddress = new Uri(_url);
            // var result = client.GetStringAsync("").Result;
            var jsonResult = JsonConvert.DeserializeObject<dynamic>(client.GetStringAsync("").Result);
            return jsonResult.results[0];
        }

        public HashSet<string> GetCategories()
        {
            catSet = JsonConvert.DeserializeObject<HashSet<string>>(client.GetStringAsync("categories").Result);
            return catSet;
        }
    }
}
