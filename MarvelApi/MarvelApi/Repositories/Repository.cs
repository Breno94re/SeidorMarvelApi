using Connection;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarvelApi
{
    internal class Repository
    {
        private ConnectionBase connection;

        internal Repository(ConnectionBase connection)
        {
            this.connection = connection;
        }


        internal MarvelApiConfiguration GetApiConfiguration()
        {
            try
            {
                string query = @"SELECT ID,USER_ID,PRIVATE_KEY,PUBLIC_KEY,SALT,MODIFIED_AT,CREATED_AT FROM USER_API_MARVEL_CONFIG WHERE USER_ID = @USER_ID";

                using (SqliteCommand sqliteCommand = new SqliteCommand(query, connection.GetConnection()))
                {
                    sqliteCommand.Parameters.AddWithValue("@USER_ID", connection.userData.Id);

                    using (SqliteDataReader sqliteDataReader = sqliteCommand.ExecuteReader())
                    {
                        if (sqliteDataReader.Read())
                        {
                            return new MarvelApiConfiguration()
                            {
                                Id = sqliteDataReader["id"].ToString(),
                                UserId = sqliteDataReader["user_id"].ToString(),
                                PrivateKey = sqliteDataReader["private_key"].ToString(),
                                PublicKey = sqliteDataReader["public_key"].ToString(),
                                Salt = sqliteDataReader["salt"].ToString(),
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

        internal bool CreateNewMarvelApiConfig(MarvelApiConfiguration marvelApiConfiguration)
        {
            try
            {
                string query = @"INSERT INTO USER_API_MARVEL_CONFIG(ID,USER_ID,PRIVATE_KEY,PUBLIC_KEY,SALT,CREATED_AT,MODIFIED_AT) 
                                VALUES(@ID,@USER_ID,@PRIVATE_KEY,@PUBLIC_KEY,@SALT,@CREATED_AT,@MODIFIED_AT)";

                using (SqliteCommand sqliteCommand = new SqliteCommand(query, connection.GetConnection(), connection.GetTransaction()))
                {
                    sqliteCommand.Parameters.AddWithValue("@ID", marvelApiConfiguration.Id);
                    sqliteCommand.Parameters.AddWithValue("@USER_ID", connection.userData.Id);
                    sqliteCommand.Parameters.AddWithValue("@PRIVATE_KEY", marvelApiConfiguration.PrivateKey);
                    sqliteCommand.Parameters.AddWithValue("@PUBLIC_KEY", marvelApiConfiguration.PublicKey);
                    sqliteCommand.Parameters.AddWithValue("@SALT", marvelApiConfiguration.Salt);
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




    }
}
