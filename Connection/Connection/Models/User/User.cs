using System;
using System.Collections.Generic;
using System.Text;

namespace Connection
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Session Session { get; set; } = new Session();
        public DateTime CreatedAt { get; set; } = new DateTime();
        public DateTime ModifiedAt { get; set; } = new DateTime();


    }
}
