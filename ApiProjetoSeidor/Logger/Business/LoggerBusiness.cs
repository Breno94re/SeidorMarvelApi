using Connection;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;
using Utility;

namespace Logger
{
    public class LoggerBusiness
    {
        private ConnectionBase connection;
        private Repository repository;
        private Package package;

        public LoggerBusiness(string token)
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

        public LoggerBusiness(ConnectionBase connection)
        {
            try
            {
                package = new Package();
                this.connection = connection;
                repository = new Repository(connection);
            }
            catch (ConnectionException)
            {
                throw;
            }
        }


        public bool LogMarvelQuery(Log log)
        {
            try
            {
                return repository.InsertLog(log);
            }
            catch (SqliteException)
            {
                throw;
            }
        }


        public Package ListLogByUser()
        {
            try
            {
                connection.OpenConnection();

                package.Data = repository.ListLogByUser();

                if(package.Data == null)
                {
                    package.Notifications.Add(new Notifications()
                    {
                        Title = "NotFound",
                        Message = "We didn't find any log in the system"
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


    }
}
