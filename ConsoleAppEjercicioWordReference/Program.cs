using System;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
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
            var palabraBuscada = "perro";//TODO: VOLVER A COLOCAR ESTO //Console.ReadLine();

            Console.Write("\nConsiguiendo datos, por favor espere... ");
            string definicion = ProcesarHTML(palabraBuscada);

            MostrarEnPantalla(palabraBuscada, definicion);

            Pausa();
        }

        private static string ProcesarHTML(string palabraBuscada)
        {
            var htmlPage = BajarPaginaAsync(palabraBuscada).Result;
            var pattern = GetPatternDefinicionPalabra();
            var match = GetDefinicionDePalabra(htmlPage, pattern);
            var definicion = ConvertirMatchAString(match);
            return definicion;
        }

        private static void Pausa()
        {
            Console.Write("Presione una tecla para finalizar... ");
            Console.ReadKey();
        }

        private static void MostrarEnPantalla(string palabraBuscada, string definicion)
        {
            Console.WriteLine($"\n\n{palabraBuscada.ToUpper()}:");
            Console.WriteLine(definicion);
        }

        static async Task<string> BajarPaginaAsync(string palabra)
        {
            var handler = new HttpClientHandler();

            if (AcceptGzipCompresion)
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpClient client = new HttpClient(handler))
            {
                if (UseUserAgent)
                    client.DefaultRequestHeaders.Add("User-Agent", "WordReference Scraping App");

                HttpResponseMessage response = await client.GetAsync($"{URL}{palabra.ToLower()}");
                return await response.Content.ReadAsStringAsync();
            }
        }

        static Match GetDefinicionDePalabra(string htmlPage, string pattern)
        {
            var match = Regex.Match(htmlPage, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
                Console.WriteLine("\n\nENCONTRADO:\n\n" + match.Value);
            Console.WriteLine("\n\nFINAL DEL MATCH\n\n");

            return match;
        }

        static string GetPatternDefinicionPalabra()
        {
            var pattern = @"<div id=""article"">\s*<br>\s*<span class=small1>(.|\n)*?</span>\s*<\s*div\s*onclick=\'redirectWR\(event,""ESdefinicion""\)\'\s*class=\'trans clickable\'>\s*<h3>(?<titulo>[\b\w+\b]+)\s*<span\s*class=supr1>\s*\d*\s*</span>(?<tituloSegundo>\s*[,*\s*[\b\w+\b]*]*\s*)</h3>\s*<ol\s*class=\'entry\'>\s*<li>(?<subtituloPrimario>\s*\w*\.*\s*)<span class=i>(?<subtituloSecundario>\s*\w*\.*\s*)</span>(?<subtituloTerciario>\s*[\b*\w*\b*(\.*|\,*)\s*]*\:*\s*)<br><span\s*class=i>(?<subtituloCuarto>\s*[\b*\w*\b*(\.*|\,*)\s*]*\s*)</span>\s*<br>\s*</ol>\s*</div>\s*<br>\s*<span class=small1>(.|\n)*?</span>\s*";
            return pattern;
        }

        static string ConvertirMatchAString(Match match)
        {
            var definicion = $@"{match.Groups["titulo"]} {match.Groups["tituloSegundo"]}";
            definicion += "\n";
            definicion += $"    {match.Groups["subtituloPrimario"]} {match.Groups["subtituloSecundario"]} {match.Groups["subtituloTerciario"]} {match.Groups["subtituloCuarto"]}";
            definicion += "\n";

            return definicion;
        }
    }
}