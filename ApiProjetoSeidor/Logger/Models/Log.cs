using System;
using System.Collections.Generic;
using System.Text;

namespace Logger
{
    public class Log
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Series { get; set; }
        public string Character { get; set; }
        public DateTime CreatedAt { get; set; } = new DateTime();

    }
}
