using System;
using System.Collections.Generic;
using System.Text;

namespace Connection
{
    public class UnauthorizedConnection:Exception
    {
        public UnauthorizedConnection()
        {

        }
        public UnauthorizedConnection(string exception) : base(exception)
        {

        }

    }
}
