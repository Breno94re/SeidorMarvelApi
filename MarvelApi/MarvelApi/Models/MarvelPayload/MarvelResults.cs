using System;
using System.Collections.Generic;
using System.Text;

namespace MarvelApi
{
    public class MarvelResults
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string modified { get; set; }
        public MarvelThumbnail thumbnail { get; set; } = new MarvelThumbnail();
        public string resourceURI { get; set; }
        public MarvelComics comics { get; set; } = new MarvelComics();
        public MarvelComics creators { get; set; } = new MarvelComics();
        public MarvelComics characters { get; set; } = new MarvelComics();
        public MarvelComics series { get; set; } = new MarvelComics();
        public MarvelComics stories { get; set; } = new MarvelComics();
        public MarvelComics events { get; set; } = new MarvelComics();
        public MarvelMisc next { get; set; } = new MarvelMisc();
        public MarvelMisc previous { get; set; } = new MarvelMisc();
        public List<MarvelUrls> urls { get; set; } = new List<MarvelUrls>();


    }
}
