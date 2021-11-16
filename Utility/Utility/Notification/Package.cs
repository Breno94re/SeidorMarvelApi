using System;
using System.Collections.Generic;
using System.Text;

namespace Utility
{
    public class Package
    {
        public int HttpCode { get; set; }
        public string HttpStatus { get; set; }
        public string Status { get; set; }
        public object Data { get; set; }
        public List<Notifications> Notifications { get; set; } = new List<Notifications>();
        public bool HasNotifications()
        {
            return Notifications!=null && Notifications.Count>0;
        }
        public void SetBadRequest()
        {
           HttpCode = (int)HttpStatusCode.BadRequest;
           HttpStatus = Utilities.GetEnumDescription(HttpStatusCode.BadRequest);
        }
        public void SetUnauthorized()
        {
            HttpCode = (int)HttpStatusCode.Unauthorized;
            HttpStatus = Utilities.GetEnumDescription(HttpStatusCode.Unauthorized);
        }

        public void SetOk()
        {
            HttpCode = (int)HttpStatusCode.Ok;
            HttpStatus = Utilities.GetEnumDescription(HttpStatusCode.Ok);
            Status = "Successful Operation";
        }

    }
}
