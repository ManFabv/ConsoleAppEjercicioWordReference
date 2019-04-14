using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleAppEjercicioWordReference
{
    static class Program
    {
        private static string URL = @"http://www.wordreference.com/definicion/";
        private static bool UseUserAgent = true;
        private static bool AcceptGzipCompresion = true;
        static void Main()
        {
            Console.Write("Palabra a buscar: ");
            var palabraBuscada = Console.ReadLine();

            Console.Write("\nConsiguiendo datos. Espere... ");
            var htmlPage = BajarPaginaAsync(palabraBuscada).Result;

            var pattern = GetPatternDefinicionPalabra();
            var definicion = GetDefinicionDePalabra(htmlPage, pattern);

            Console.WriteLine($"\n\nDefinicion de {palabraBuscada}:\n");
            Console.WriteLine(definicion);

            Console.Write("Presione una tecla para finalizar... ");
            Console.ReadKey();
        }

        static async Task<string> BajarPaginaAsync(string palabra)
        {
            var handler = new HttpClientHandler();

            if(AcceptGzipCompresion)
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpClient client = new HttpClient(handler))
            { 
                if(UseUserAgent)
                    client.DefaultRequestHeaders.Add("User-Agent", "WordReference Scraping App");

                HttpResponseMessage response = await client.GetAsync($"{URL}{palabra.ToLower()}");
                return await response.Content.ReadAsStringAsync();
            }
        }

        static string GetDefinicionDePalabra(string htmlPage, string pattern)
        {
            var definicion = string.Empty;

            //TODO: Procesar la pagina

            return definicion;
        }

        static string GetPatternDefinicionPalabra()
        {
            var pattern = string.Empty;

            //TODO: generar pattern

            return pattern;
        }
    }
}