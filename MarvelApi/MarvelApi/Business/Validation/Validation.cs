using System;
using System.Collections.Generic;
using System.Text;
using Utility;

namespace MarvelApi
{
    internal static class Validation
    {
        internal static List<Notifications> ValidateApiConfiguration(MarvelApiConfiguration marvelApiConfiguration)
        {
            List<Notifications> notifications = new List<Notifications>();

            if (marvelApiConfiguration == null)
            {
                notifications.Add(new Notifications()
                {
                    Title = "Null object",
                    Message = "Object cannot be null or empty"

                });

                return notifications; // stopping validation and returning the not object because user input was null and we cannot procced the op;
            }

            if (string.IsNullOrEmpty(marvelApiConfiguration.PrivateKey))
            {
                notifications.Add(new Notifications()
                {
                    Title = "Empty field",
                    Message = "PrivateKey cannot be null or empty"
                });
            }

            if (!string.IsNullOrEmpty(marvelApiConfiguration.PrivateKey) && marvelApiConfiguration.PrivateKey.Length > 255)
            {
                notifications.Add(new Notifications()
                {
                    Title = "Field Max Size",
                    Message = "PrivateKey cannot be more than 255 chars in size"
                });
            }

            if (string.IsNullOrEmpty(marvelApiConfiguration.PublicKey))
            {
                notifications.Add(new Notifications()
                {
                    Title = "Empty field",
                    Message = "PublicKey cannot be null or empty"
                });
            }

            if (!string.IsNullOrEmpty(marvelApiConfiguration.PublicKey) && marvelApiConfiguration.PublicKey.Length > 255)
            {
                notifications.Add(new Notifications()
                {
                    Title = "Field Max Size",
                    Message = "PublicKey cannot be more than 255 chars in size"
                });
            }

            return notifications;
        }


    }
}
