using Newtonsoft.Json;
using SecimKurullariBot.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SecimKurullariBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Start().Wait();
        }

        async static Task Start()
        {
            Console.WriteLine("Started Crawler Job");

            string json = File.ReadAllText("ysk.json");
            var cities = JsonConvert.DeserializeObject<List<YSKModel>>(json);

            SaadetCrawler saadet = new SaadetCrawler();
            foreach (var city in cities)
            {
                await saadet.GetPhoneNumber(city);
            }
            File.WriteAllText("data.json", JsonConvert.SerializeObject(cities));
            Console.WriteLine("Crawler Finished");
        }
    }
}
