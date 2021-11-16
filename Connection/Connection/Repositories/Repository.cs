using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;


namespace Connection
{
    internal class Repository
    {
        internal ConnectionBase connection;

        internal Repository(ConnectionBase connection)
        {
            this.connection = connection;
        }

        

        internal User ValidateToken(string token)
        {
            try
            {
                string query = @"SELECT USER_SESSION.ID,USER_ID,EMAIL,NAME,HASH_TOKEN,IP_ADRESS,USER_SESSION.CREATED_AT,USER_SESSION.MODIFIED_AT FROM USER_SESSION
                            INNER JOIN USER ON USER_SESSION.USER_ID = USER.ID WHERE hash_token = @hash_token";

                using (SqliteCommand sqliteCommand = new SqliteCommand(query, connection.GetConnection()))
                {
                    sqliteCommand.Parameters.AddWithValue("@hash_token", token);

                    using (SqliteDataReader dataReader = sqliteCommand.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            return new User()
                            {
                                Id = dataReader["user_id"].ToString(),
                                Name = dataReader["name"].ToString(),
                                Email = dataReader["email"].ToString(),
                                Session = new Session()
                                {
                                    Id = dataReader["id"].ToString(),
                                    IpAdress = dataReader["ip_adress"].ToString(),
                                    Token = dataReader["hash_token"].ToString(),
                                },
                                CreatedAt = Convert.ToDateTime(dataReader["created_at"]),
                                ModifiedAt = Convert.ToDateTime(dataReader["modified_at"]),
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

        internal bool UpdateTokenLifeSpan(User user)
        {
            try
            {
                string query = @"UPDATE USER_SESSION SET MODIFIED_AT = @MODIFIED_AT WHERE ID = @ID";

                using (SqliteCommand sqliteCommand = new SqliteCommand(query, connection.GetConnection(),connection.GetTransaction()))
                {
                    sqliteCommand.Parameters.AddWithValue("@ID", user.Session.Id);
                    sqliteCommand.Parameters.AddWithValue("@MODIFIED_AT", DateTime.Now);

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
