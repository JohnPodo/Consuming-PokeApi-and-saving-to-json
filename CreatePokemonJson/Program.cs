using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CreatePokemonJson.Models;
using Newtonsoft.Json;
using RestSharp;

namespace CreatePokemonJson
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Procedure Started");
            var pokemons = FetchData<Pokemon>("https://pokeapi.co/api/v2/pokemon");
            Console.WriteLine("Pokemons Fetched");
            CreateJson("Pokemon", JsonConvert.SerializeObject(pokemons, Formatting.Indented));
            Console.WriteLine("Pokemons was Succesfully Written on json");
            var items = FetchData<Item>("https://pokeapi.co/api/v2/item");
            Console.WriteLine("Items Fetched");
            CreateJson("Items", JsonConvert.SerializeObject(items, Formatting.Indented));
            Console.WriteLine("Items was Succesfully Written on json");
            Console.WriteLine("Finished");
            Console.ReadLine();
        }

        private static void CreateJson(string fileName,string content)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), $"{fileName}.json");
                File.WriteAllText(path, content);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static List<T> FetchData<T>(string firstUrl) where T:class
        {
            var next = string.Empty;
            var startingUrl = firstUrl;
            var response = ExecuteRequest(firstUrl);
            var count = response.count;
            var succesfulCount = 0;
            var data = new List<T>();
            if (response != null)
            {
                do
                {
                    try
                    {
                        foreach (var pokemon in response.results)
                        {
                            var newItem = FetchT<T>(pokemon.url);
                            if (newItem != null)
                            { 
                                data.Add(newItem);
                                succesfulCount = succesfulCount + 1;
                                float percentNumber = ((float)succesfulCount / (float)count) * 100;
                                Console.Write($"\r{percentNumber.ToString("0.00")}%");

                            }

                        }
                        next = response.next;
                        if (!string.IsNullOrEmpty(next))
                            response = ExecuteRequest(next);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        next = string.Empty;
                    }
                } while (!string.IsNullOrEmpty(next));
                return data;
            }
            return null;
        }

        private static T FetchT<T>(string url)
        {
            RestClient client = new RestClient(url);
            RestRequest request = new RestRequest();
            request.Method = Method.GET;
            request.RequestFormat = DataFormat.Json;
            var response = client.Execute(request);
            var deserialisedResponse = JsonConvert.DeserializeObject<T>(response.Content);
            return deserialisedResponse;
        }

        private static GeneralResponse ExecuteRequest(string url)
        {
            try
            {
                RestClient client = new RestClient(url);
                RestRequest request = new RestRequest();
                request.Method = Method.GET;
                request.RequestFormat = DataFormat.Json;
                var response = client.Execute(request);
                var deserialisedResponse = JsonConvert.DeserializeObject<GeneralResponse>(response.Content);
                return deserialisedResponse;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
           
        }
         
    }
}
