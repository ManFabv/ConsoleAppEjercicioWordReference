using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RestSharp;

namespace ConsoleAppEjercicioWordReference
{
    class Program
    {
        private static string URL = @"http://www.wordreference.com/definicion";

        static void Main(string[] args)
        {
            var htmlPage = BajarPaginaAsync("perro").Result;

            Console.WriteLine(htmlPage);

            Console.Write("Final...");
            Console.ReadKey();
        }

        static async Task<string> BajarPaginaAsync(string palabra)
        {
            var handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpClient client = new HttpClient(handler))
            { 
                client.DefaultRequestHeaders.Add("User-Agent", "C# App");
                HttpResponseMessage response = await client.GetAsync($"{URL}//{palabra.ToLower()}");
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
