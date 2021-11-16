using Connection;
using System;
using System.Collections.Generic;
using System.Text;
using Utility;

namespace Login
{
    internal static class Validation
    {

        internal static List<Notifications> ValidateUser(User user)
        {
            List<Notifications> notifications = new List<Notifications>();

            if (user == null)
            {
                notifications.Add(new Notifications()
                {
                    Title = "Null object",
                    Message = "Object cannot be null or empty"

                });

                return notifications; // stopping validation and returning the not object because user input was null and we cannot procced the op;
            }

            if(string.IsNullOrEmpty(user.Email))
            {
                notifications.Add(new Notifications()
                {
                    Title = "Empty field",
                    Message = "Email cannot be null or empty"
                });
            }

            if (!string.IsNullOrEmpty(user.Email) && user.Email.Length>255)
            {
                notifications.Add(new Notifications()
                {
                    Title = "Field Max Size",
                    Message = "Email cannot be more than 255 chars in size"
                });
            }

            if (string.IsNullOrEmpty(user.Name))
            {
                notifications.Add(new Notifications()
                {
                    Title = "Empty field",
                    Message = "Name cannot be null or empty"
                });
            }

            if (!string.IsNullOrEmpty(user.Name) && user.Name.Length > 255)
            {
                notifications.Add(new Notifications()
                {
                    Title = "Field Max Size",
                    Message = "Name cannot be more than 255 chars in size"
                });
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                notifications.Add(new Notifications()
                {
                    Title = "Empty field",
                    Message = "Password cannot be null or empty"
                });
            }

            if (!string.IsNullOrEmpty(user.Password) &&  user.Password.Length > 255)
            {
                notifications.Add(new Notifications()
                {
                    Title = "Field Max Size",
                    Message = "Password cannot be more than 255 chars in size"
                });
            }

            return notifications;
        }


    }
}
