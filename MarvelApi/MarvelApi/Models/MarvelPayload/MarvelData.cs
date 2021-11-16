using System;
using System.Collections.Generic;
using System.Text;

namespace MarvelApi
{
    public class MarvelData
    {
        public int offset { get; set; }
        public int limit { get; set; }
        public int total { get; set; }
        public int count { get; set; }
        public List<MarvelResults> results { get; set; } = new List<MarvelResults>();
    }
}
