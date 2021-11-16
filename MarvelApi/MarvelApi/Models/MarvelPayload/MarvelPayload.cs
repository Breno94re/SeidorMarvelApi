using System;
using System.Collections.Generic;
using System.Text;

namespace MarvelApi
{
    public class MarvelPayload
    {
        public int code { get; set; }
        public string status { get; set; }
        public string copyright { get; set; }
        public string attributionText { get; set; }
        public string attributionHTML { get; set; }
        public string etag { get; set; }
        public MarvelData data { get; set; } = new MarvelData();


    }
}
