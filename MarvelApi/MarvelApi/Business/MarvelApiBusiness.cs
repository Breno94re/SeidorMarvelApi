using Connection;
using Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using Utility;

namespace MarvelApi
{
    public class MarvelApiBusiness
    {
        private ConnectionBase connection;
        private Repository repository;
        private Package package;
        private HttpWebRequest httpWebRequest;
        private LoggerBusiness logger;


         public MarvelApiBusiness(string token)
        {
            try
            {
                package = new Package();
                connection = new ConnectionBase(token);
                repository = new Repository(connection);
                logger = new LoggerBusiness(connection);
            }
            catch (ConnectionException)
            {
                throw;
            }
        }

        public Package CreateNewApiConfig(MarvelApiConfiguration marvelApiConfiguration)
        {
            try
            {
                package.Notifications = Validation.ValidateApiConfiguration(marvelApiConfiguration);

                if (package.HasNotifications())
                {
                    package.SetBadRequest();

                    return package;
                }

                marvelApiConfiguration.Id = Guid.NewGuid().ToString();
                marvelApiConfiguration.Salt = Guid.NewGuid().ToString().Replace("-", string.Empty);

                connection.OpenConnection();



                if (repository.GetApiConfiguration() == null)
                {
                    connection.OpenTransaction();

                    if (!repository.CreateNewMarvelApiConfig(marvelApiConfiguration))
                    {
                        package.Notifications.Add(new Notifications()
                        {
                            Title = "Unexpected Error",
                            Message = "unfortunately we got some unexpected error"
                        });

                        package.SetBadRequest();

                        return package;
                    }
                }
                else
                {

                    connection.OpenTransaction();

                    if (!repository.UpdateExistingApiConfig(marvelApiConfiguration))
                    {
                        package.Notifications.Add(new Notifications()
                        {
                            Title = "Unexpected Error",
                            Message = "unfortunately we got some unexpected error"
                        });

                        package.SetBadRequest();

                        return package;
                    }
                }

                connection.CommitTransaction();

                package.SetOk();

                return package;

            }
            catch (Exception)
            {
                if (connection.HasTransaction())
                {
                    connection.RollbackTransaction();
                }

                throw;
            }
            finally
            {
                if (package.HasNotifications() && connection.HasTransaction())
                {
                    connection.RollbackTransaction();
                }

                connection.CloseConnection();
            }
        }

        public Package GetApiConfig()
        {
            try
            {
                connection.OpenConnection();

                package.Data = repository.GetApiConfigurationSafe();

                if (package.Data == null)
                {
                    package.Notifications.Add(new Notifications()
                    {
                        Title = "Not Found",
                        Message = "No Marvel Api configuration for this user was found"
                    });

                    package.SetBadRequest();

                    return package;
                }

                package.SetOk();

                return package;

            }
            catch (Exception)
            {
                if (connection.HasTransaction())
                {
                    connection.RollbackTransaction();
                }

                throw;
            }
            finally
            {
                if (package.HasNotifications() && connection.HasTransaction())
                {
                    connection.RollbackTransaction();
                }

                connection.CloseConnection();
            }
        }

        public Package GetHeroByName(string name)
        {
            try
            {
                connection.OpenConnection();

                MarvelApiConfiguration marvelApiConfiguration = repository.GetApiConfiguration();

                if (marvelApiConfiguration == null)
                {
                    package.Notifications.Add(new Notifications()
                    {
                        Title = "NotFound",
                        Message = "We didn't find your Marvel Api Configuration,you need to register public and private key before searching"
                    });

                    package.SetBadRequest();

                    return package;
                }

                marvelApiConfiguration.Md5 = BuildMarvelMd5(marvelApiConfiguration);

                MarvelPayload marvelPayload = MarvelApiGetRequest($"characters?name={name}&ts={marvelApiConfiguration.Salt.ToLower()}&apikey={marvelApiConfiguration.PublicKey}&hash={marvelApiConfiguration.Md5.ToLower()}");

                if (marvelPayload == null)
                {
                    package.Notifications.Add(new Notifications()
                    {
                        Title = "Unexpected error",
                        Message = "We couldn't communicate to marvel api's"
                    });

                    package.SetBadRequest();

                    return package;
                }

                connection.OpenTransaction();

                if (!logger.LogMarvelQuery(new Log() { Character = name, Series = "", Id = Guid.NewGuid().ToString() }))
                {
                    package.Notifications.Add(new Notifications()
                    {
                        Title = "Unexpected error",
                        Message = "We couldn't log your action"
                    });

                    package.SetBadRequest();

                    return package;
                }

                package.Data = marvelPayload.data;

                connection.CommitTransaction();

                package.SetOk();

                return package;
            }
            catch (Exception)
            {
                if (connection.HasTransaction())
                {
                    connection.RollbackTransaction();
                }

                throw;
            }
            finally
            {
                if (package.HasNotifications() && connection.HasTransaction())
                {
                    connection.RollbackTransaction();
                }

                connection.CloseConnection();
            }
        }

        public Package GetSeriesByName(string name)
        {
            try
            {
                connection.OpenConnection();

                MarvelApiConfiguration marvelApiConfiguration = repository.GetApiConfiguration();

                if (marvelApiConfiguration == null)
                {
                    package.Notifications.Add(new Notifications()
                    {
                        Title = "NotFound",
                        Message = "We didn't find your Marvel Api Configuration,you need to register public and private key before searching"
                    });

                    package.SetBadRequest();

                    return package;
                }

                marvelApiConfiguration.Md5 = BuildMarvelMd5(marvelApiConfiguration);

                MarvelPayload marvelPayload = MarvelApiGetRequest($"series?title={name}&ts={marvelApiConfiguration.Salt.ToLower()}&apikey={marvelApiConfiguration.PublicKey}&hash={marvelApiConfiguration.Md5.ToLower()}");

                if (marvelPayload == null)
                {
                    package.Notifications.Add(new Notifications()
                    {
                        Title = "Unexpected error",
                        Message = "We couldn't communicate to marvel api's"
                    });

                    package.SetBadRequest();

                    return package;
                }

                connection.OpenTransaction();

                if (!logger.LogMarvelQuery(new Log() { Character = "", Series = name, Id = Guid.NewGuid().ToString() }))
                {
                    package.Notifications.Add(new Notifications()
                    {
                        Title = "Unexpected error",
                        Message = "We couldn't log your action"
                    });

                    package.SetBadRequest();

                    return package;
                }

                connection.CommitTransaction();

                package.Data = marvelPayload.data;

                package.SetOk();

                return package;
            }
            catch (Exception)
            {
                if (connection.HasTransaction())
                {
                    connection.RollbackTransaction();
                }

                throw;
            }
            finally
            {
                if (package.HasNotifications() && connection.HasTransaction())
                {
                    connection.RollbackTransaction();
                }

                connection.CloseConnection();
            }
        }



        private string BuildMarvelMd5(MarvelApiConfiguration marvelApiConfiguration)
        {
            try
            {
                return Utilities.CreateMD5($"{marvelApiConfiguration.Salt.ToLower()}{marvelApiConfiguration.PrivateKey}{marvelApiConfiguration.PublicKey}");
            }
            catch (Exception)
            {
                throw;
            }

        }

        private MarvelPayload MarvelApiGetRequest(string url)
        {
            try
            {
                MarvelPayload marvelPayload = new MarvelPayload();
                httpWebRequest = (HttpWebRequest)WebRequest.Create($"https://gateway.marvel.com:443/v1/public/{url}");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";

                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string json = streamReader.ReadToEnd();

                    marvelPayload = JsonSerializer.Deserialize<MarvelPayload>(json);
                }

                return marvelPayload;
            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}
