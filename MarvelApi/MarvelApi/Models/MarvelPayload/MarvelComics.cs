using System;
using System.Collections.Generic;
using System.Text;

namespace MarvelApi
{
    public class MarvelComics
    {
        public int available {get;set;}
        public string collectionURI { get; set; }
        public List<MarvelItems> items { get; set; } = new List<MarvelItems>();
        public int returned { get; set; }
    }
}
