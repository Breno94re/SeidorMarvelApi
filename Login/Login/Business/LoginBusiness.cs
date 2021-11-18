using System;
using System.Collections.Generic;
using System.Text;
using Connection;
using Utility;

namespace Login
{
    public class LoginBusiness
    {
        private ConnectionBase connection;
        private Repository repository;
        private Package package;

        public LoginBusiness()
        {
            try
            {
                package = new Package();
                connection = new ConnectionBase();
                repository = new Repository(connection);
            }
            catch (ConnectionException)
            {
                throw;
            }
        }

        public LoginBusiness(string token)
        {
            try
            {
                package = new Package();
                connection = new ConnectionBase(token);
                repository = new Repository(connection);
            }
            catch (ConnectionException)
            {
                throw;
            }
        }

        public Package Login(LoginAuth login)
        {
            try
            {
                connection.OpenConnection();

                User user = repository.GetUser(login);

                if(user==null)
                {
                    package.Notifications.Add(new Notifications() {Title="Unauthorized",Message="no account was found"});
                    package.SetUnauthorized();
                    return package;
                }

                user.Password = Utilities.CreateMD5($"{user.Name.ToLower()}{user.Id}{user.Email.ToLower()}{login.Password}");

                user = repository.GetUserByMd5(user);

                if (user == null)
                {
                    package.Notifications.Add(new Notifications() { Title = "Unauthorized", Message = "wrong password" });
                    package.SetUnauthorized();
                    return package;
                }

                user.Session.Token = Guid.NewGuid().ToString();
                user.Session.Id = Guid.NewGuid().ToString();
                user.Session.IpAdress = login.IpAdress;
                connection.OpenTransaction();

                if(!repository.CreateNewSession(user))
                {
                    package.Notifications.Add(new Notifications()
                    {
                        Title = "Unexpected Error",
                        Message = "unfortunately we got some unexpected error"
                    });

                    package.SetBadRequest();

                    return package;
                }

                connection.CommitTransaction();
                
                package.SetOk();

                package.Data = user.Session;

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


        public Package CreateNewUser(User user)
        {
            try
            {
                connection.OpenConnection();

                package.Notifications = Validation.ValidateUser(user);

                if (package.HasNotifications())
                {
                    package.SetBadRequest();

                    return package;
                }

                if (repository.VerifyEmail(user.Email))
                {
                    package.SetBadRequest();

                    package.Notifications.Add(new Notifications()
                    {
                        Title = "Duplicated Field",
                        Message = "Email already exists on our base"
                    });

                    return package;
                }
                
                user.Id = Guid.NewGuid().ToString();

                user.Password = Utilities.CreateMD5($"{user.Name.ToLower()}{user.Id}{user.Email.ToLower()}{user.Password}");

                connection.OpenTransaction();

                if (!repository.CreateNewUser(user))
                {
                    package.Notifications.Add(new Notifications()
                    {
                        Title = "Unexpected Error",
                        Message = "unfortunately we got some unexpected error"
                    });

                    package.SetBadRequest();

                    return package;
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
                if(package.HasNotifications() && connection.HasTransaction())
                {
                    connection.RollbackTransaction();
                }

                connection.CloseConnection();
            }
        }


        public Package ValidateToken()
        {
            try
            {
                Package package = new Package();

                package.SetOk();

                return package;

            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}
