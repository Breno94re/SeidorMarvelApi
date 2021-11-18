using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Connection
{
    public class ConnectionBase
    {
        private SqliteConnection sqliteConnection = null;
        private SqliteTransaction sqliteTransaction = null;
        private Repository repository;
        public User userData { get; set; }

        public ConnectionBase(string token)
        {
            try
            {
                SetConnection();

                ValidateSession(token);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ConnectionBase()
        {
            try
            {
                SetConnection();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ValidateSession(string token)
        {
            try
            {

                if(string.IsNullOrEmpty(token))
                {
                    throw new UnauthorizedConnection("");
                }

                repository = new Repository(this);

                sqliteConnection.Open();

                userData = repository.ValidateToken(token) ?? throw new UnauthorizedConnection();

                if (!ValidateTokenExpiration(userData))
                {
                    throw new UnauthorizedConnection("");
                }

                OpenTransaction();

                if(!repository.UpdateTokenLifeSpan(userData))
                {
                    throw new UnauthorizedConnection("");
                }

                CommitTransaction();


            }
            catch (UnauthorizedConnection)
            {
                if (HasTransaction())
                {
                    RollbackTransaction();
                }

                throw;
            }
            finally
            {

               CloseConnection();
            }
        }
        public bool ValidateTokenExpiration(User user)
        {
            if (user.ModifiedAt == null)
            {
                return false;
            }
            else if ((DateTime.Now - user.ModifiedAt).TotalHours > 48)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void SetConnection()
        {
            try
            {
                sqliteConnection = new SqliteConnection(@"Data Source=C:\Users\BrenoAlmeida\Desktop\ProjetoSeidor\DbLite\SeidorMarvelDb.db;");
                SQLitePCL.Batteries_V2.Init();
            }
            catch (SqliteException)
            {
                throw;
            }
        }

        public SqliteConnection GetConnection()
        {
            return sqliteConnection;
        }

        public void OpenConnection()
        {
            if (sqliteConnection.State == System.Data.ConnectionState.Closed)
            {
                sqliteConnection.Open();
            }
        }

        public void OpenTransaction()
        {
            if (sqliteConnection.State == System.Data.ConnectionState.Open)
            {
                sqliteTransaction = sqliteConnection.BeginTransaction();
            }
        }

        public bool HasTransaction()
        {
            return sqliteConnection.State == System.Data.ConnectionState.Open && sqliteTransaction != null;
        }

        public SqliteTransaction GetTransaction()
        {
            if (sqliteConnection.State == System.Data.ConnectionState.Open && HasTransaction())
            {
                return sqliteTransaction;
            }
            else
            {
                return null;
            }
        }

        public void CommitTransaction()
        {
            if (sqliteConnection.State == System.Data.ConnectionState.Open && HasTransaction())
            {
                sqliteTransaction.Commit();
                sqliteTransaction = null;

            }
        }

        public void RollbackTransaction()
        {
            if (sqliteConnection.State == System.Data.ConnectionState.Open && HasTransaction())
            {
                sqliteTransaction.Rollback();
                sqliteTransaction = null;

            }
        }


        public void CloseConnection()
        {
            if (sqliteConnection.State == System.Data.ConnectionState.Open)
            {
                sqliteConnection.Close();
            }
        }

    }
}
