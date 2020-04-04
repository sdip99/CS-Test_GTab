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

        // Hash set for storing categories
        private HashSet<string> catSet;

        private HttpClient client;
        public JsonFeed() { }
        public JsonFeed(string endpoint)
        {
            _url = endpoint;
            this.client = new HttpClient();
            client.BaseAddress = new Uri(endpoint);
        }

        // Setter method for setting url and creating Http Object
        public void setURL(string url)
        {
            this._url = url;
            this.client = new HttpClient();
            client.BaseAddress = new Uri(url);
        }


        /* Method for getting random jokes and replacing Chuck Norris in the joke to firstName and lastname */
        public string[] GetRandomJokes(string firstname, string lastname, string category, int n)
        {
            String[] jokes = GetRandomJokes(category, n); // Get # of joke in the category
            for (int i = 0; i < jokes.Length; i++)
            {
                jokes[i] = jokeWithRandomName(jokes[i], firstname, lastname); // Replace 'Chuck Norris' from the joke
            }
            return jokes;
        }

        /*@Overloading
            Method for getting jokes from Chuck Norris API */
        public string[] GetRandomJokes(string cat, int count)
        {
            String[] jokes = new String[count];
            string url = _url + "jokes/random";
            if (!string.IsNullOrEmpty(cat)) // check for the category value
            {
                url += "?category=" + cat; // Setting url for getting joke of specific category
            }
            dynamic jokeObj;
            for (int i = 0; i < count; i++) // API call for getting # of joke 
            {
                jokeObj = JsonConvert.DeserializeObject<dynamic>(client.GetStringAsync(url).Result);
                jokes[i] = jokeObj.value.ToString(); // Adding it to string[]
            }
            return jokes;
        }


        /* Method for replacing Chuck norris string from joke to firstName and lastName specified*/
        public string jokeWithRandomName(string joke, string fName, string lName)
        {
            int index = joke.IndexOf("Chuck Norris");
            string prefix = joke.Substring(0, index);
            string suffix = joke.Substring(index + "Chuck Norris".Length, joke.Length - (index + "Chuck Norris".Length));
            string jokeWithRandomName = prefix + "" + fName + " " + lName + suffix;
            return jokeWithRandomName;
        }


        /* Method for getting Random first name and last name from the API */
        public dynamic Getnames()
        {
            var jsonResult = JsonConvert.DeserializeObject<dynamic>(client.GetStringAsync("").Result);
            return jsonResult.results[0];
        }


        /* Method for getting list of categories from the API*/
        public HashSet<string> GetCategories()
        {
            catSet = JsonConvert.DeserializeObject<HashSet<string>>(client.GetStringAsync("categories").Result);
            return catSet;
        }
    }
}
