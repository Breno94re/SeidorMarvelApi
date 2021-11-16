using System;
using System.Collections.Generic;
using System.Text;

namespace Connection
{
    public class ConnectionException : Exception
    {
        public ConnectionException()
        {

        }
        public ConnectionException(string exception) : base(exception)
        {

        }


    }
}
