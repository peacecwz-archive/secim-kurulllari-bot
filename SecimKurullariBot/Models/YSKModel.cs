using System;
using System.Collections.Generic;
using System.Text;

namespace SecimKurullariBot.Models
{
    public class YSKModel
    {
        public int ilId { get; set; }
        public string ilAdi { get; set; }
        public List<Ilce> ilceler { get; set; }
    }

    public class Ilce
    {
        public string ilceAdi { get; set; }
        public int birimId { get; set; }
        public string birimAdi { get; set; }
        public string birimTel { get; set; }
        public string birimAdres { get; set; }
    }

}
