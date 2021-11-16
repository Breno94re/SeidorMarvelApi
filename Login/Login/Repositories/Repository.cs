using Connection;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Login
{
    internal class Repository
    {
        private ConnectionBase connection;

        internal Repository(ConnectionBase connection)
        {
            this.connection = connection;
        }

        internal User GetUser(LoginAuth login)
        {
            try
            {
                string query = @"SELECT ID,EMAIL,NAME FROM USER WHERE LOWER(EMAIL) = LOWER(@EMAIL)";

                using (SqliteCommand sqliteCommand = new SqliteCommand(query, connection.GetConnection()))
                {
                    sqliteCommand.Parameters.AddWithValue("@EMAIL", login.Email);

                    using (SqliteDataReader dataReader = sqliteCommand.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            return new User()
                            {
                                Id = dataReader["id"].ToString(),
                                Email = dataReader["email"].ToString(),
                                Name = dataReader["name"].ToString(),

                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (SqliteException)
            {
                throw;
            }
        }
        internal bool VerifyEmail(string md5)
        {
            try
            {
                string query = @"SELECT ID FROM USER WHERE LOWER(EMAIL) = LOWER(@EMAIL)";

                using (SqliteCommand sqliteCommand = new SqliteCommand(query, connection.GetConnection()))
                {
                    sqliteCommand.Parameters.AddWithValue("@EMAIL", md5);

                    using (SqliteDataReader dataReader = sqliteCommand.ExecuteReader())
                    {
                        return dataReader.Read();  
                    }
                }
            }
            catch (SqliteException)
            {
                throw;
            }
        }

        internal bool CreateNewUser(User user)
        {
            try
            {
                string query = @"INSERT INTO USER(ID,NAME,EMAIL,SALT_MD5,MODIFIED_AT,CREATED_AT) 
                                VALUES(@ID,@NAME,@EMAIL,@SALT_MD5,@MODIFIED_AT,@CREATED_AT)";

                using (SqliteCommand sqliteCommand = new SqliteCommand(query, connection.GetConnection(),connection.GetTransaction()))
                {
                    sqliteCommand.Parameters.AddWithValue("@ID", user.Id);
                    sqliteCommand.Parameters.AddWithValue("@NAME", user.Name);
                    sqliteCommand.Parameters.AddWithValue("@EMAIL", user.Email);
                    sqliteCommand.Parameters.AddWithValue("@SALT_MD5", user.Password);
                    sqliteCommand.Parameters.AddWithValue("@MODIFIED_AT", DateTime.Now);
                    sqliteCommand.Parameters.AddWithValue("@CREATED_AT", DateTime.Now);

                    return sqliteCommand.ExecuteNonQuery() > 0;
                }
            }
            catch (SqliteException)
            {
                throw;
            }
        }

        internal bool CreateNewSession(User user)
        {
            try
            {
                string query = @"INSERT INTO USER_SESSION(ID,USER_ID,IP_ADRESS,HASH_TOKEN,MODIFIED_AT,CREATED_AT) 
                                VALUES(@ID,@USER_ID,@IP_ADRESS,@HASH_TOKEN,@MODIFIED_AT,@CREATED_AT)";

                using (SqliteCommand sqliteCommand = new SqliteCommand(query, connection.GetConnection(), connection.GetTransaction()))
                {
                    sqliteCommand.Parameters.AddWithValue("@ID", user.Session.Id);
                    sqliteCommand.Parameters.AddWithValue("@USER_ID", user.Id);
                    sqliteCommand.Parameters.AddWithValue("@IP_ADRESS", user.Session.IpAdress);
                    sqliteCommand.Parameters.AddWithValue("@HASH_TOKEN", user.Session.Token);
                    sqliteCommand.Parameters.AddWithValue("@MODIFIED_AT", DateTime.Now);
                    sqliteCommand.Parameters.AddWithValue("@CREATED_AT", DateTime.Now);

                    return sqliteCommand.ExecuteNonQuery() > 0;
                }
            }
            catch (SqliteException)
            {
                throw;
            }
        }

        internal User GetUserByMd5(User user)
        {
            try
            {
                string query = @"SELECT ID,EMAIL,NAME FROM USER WHERE SALT_MD5 = @SALT_MD5 AND EMAIL = @EMAIL";

                using (SqliteCommand sqliteCommand = new SqliteCommand(query, connection.GetConnection()))
                {
                    sqliteCommand.Parameters.AddWithValue("@EMAIL", user.Email);
                    sqliteCommand.Parameters.AddWithValue("@SALT_MD5", user.Password);


                    using (SqliteDataReader dataReader = sqliteCommand.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            return new User()
                            {
                                Id = dataReader["id"].ToString(),
                                Email = dataReader["email"].ToString(),
                                Name = dataReader["name"].ToString(),
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (SqliteException)
            {
                throw;
            }
        }


    }
}
