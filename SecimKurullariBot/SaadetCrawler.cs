using HtmlAgilityPack;
using SecimKurullariBot.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SecimKurullariBot
{
    public class SaadetCrawler
    {
        private HtmlDocument _doc;
        private readonly HttpClient _client;
        public SaadetCrawler()
        {
            var handler = new HttpClientHandler();
            handler.AutomaticDecompression = System.Net.DecompressionMethods.None;
            this._client = new HttpClient(handler)
            {
                BaseAddress = new Uri("http://saadet.org.tr")
            };
            _client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            //_client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
            _client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "tr-TR,tr;q=0.9,en-US;q=0.8,en;q=0.7");
            _client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_13_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.139 Safari/537.36");
            _client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type:", "application/x-www-form-urlencoded");
            _doc = new HtmlDocument();

        }

        public async Task GetPhoneNumber(YSKModel city)
        {
            var response = await _client.GetAsync($"/kurum_il.php?ILID={city.ilId}");
            if (!response.IsSuccessStatusCode) return;
            var responseString = await response.Content.ReadAsStreamAsync();
            _doc.Load(responseString, Encoding.GetEncoding("iso-8859-9"));
            var div = _doc.DocumentNode.SelectSingleNode("//ul[@class='gallery']");
            var cities = div.SelectNodes(".//li");
            foreach (var cityItem in cities)
            {
                var item = cityItem.SelectSingleNode(".//div[@class='event-row-inner']");
                string birimAdi = item.SelectSingleNode(".//h4").InnerText.Trim();
                string birimAdres = item.SelectSingleNode(".//p").InnerText.Trim();
                var birim = city.ilceler.FirstOrDefault(x => x.birimAdi == birimAdi);
                birim.birimAdres = birimAdres;
                try
                {
                    birim.ilceAdi = birimAdi.Substring(0, birimAdi.IndexOf(" İLÇE")).Replace(" İLÇE", "");
                }
                catch
                {
                    if (birimAdi.IndexOf(".İLÇE") > -1)
                        birim.ilceAdi = birimAdi.Substring(0, birimAdi.IndexOf(".İLÇE") - 2).Replace(".İLÇE", "");
                    else
                        birim.ilceAdi = birimAdi.Split(' ')[0];

                }
                if (birim.ilceAdi.IndexOf('.') > -1)
                {
                    birim.ilceAdi = birim.ilceAdi.Substring(0, birim.ilceAdi.IndexOf('.') - 2);
                }
            }
        }
    }
}
