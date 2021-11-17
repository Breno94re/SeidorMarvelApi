using Connection;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logger
{
    internal class Repository
    {
        private ConnectionBase connection;

        internal Repository(ConnectionBase connection)
        {
            this.connection = connection;
        }

        internal bool InsertLog(Log log)
        {
            try
            {
                string query = @"INSERT INTO LOG(ID,USER_ID,SERIES,CHARACTER,CREATED_AT) 
                                VALUES(@ID,@USER_ID,@SERIES,@CHARACTER,@CREATED_AT)";

                using (SqliteCommand sqliteCommand = new SqliteCommand(query, connection.GetConnection(), connection.GetTransaction()))
                {
                    sqliteCommand.Parameters.AddWithValue("@ID", log.Id);
                    sqliteCommand.Parameters.AddWithValue("@USER_ID", connection.userData.Id);
                    sqliteCommand.Parameters.AddWithValue("@SERIES", log.Series);
                    sqliteCommand.Parameters.AddWithValue("@CHARACTER", log.Character);
                    sqliteCommand.Parameters.AddWithValue("@CREATED_AT", DateTime.Now);

                    return sqliteCommand.ExecuteNonQuery() > 0;
                }
            }
            catch (SqliteException)
            {
                throw;
            }
        }

        internal List<Log> ListLogByUser()
        {
            try
            {
                string query = @"SELECT ID,USER_ID,SERIES,CHARACTER,CREATED_AT FROM LOG WHERE USER_ID = @USER_ID";

                using (SqliteCommand sqliteCommand = new SqliteCommand(query, connection.GetConnection()))
                {
                    sqliteCommand.Parameters.AddWithValue("@USER_ID", connection.userData.Id);

                    List<Log> logs = new List<Log>();

                    using (SqliteDataReader dataReader = sqliteCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            logs.Add( new Log()
                            {
                                Id = dataReader["id"].ToString(),
                                UserId = dataReader["user_id"].ToString(),
                                Series = dataReader["series"].ToString(),
                                Character = dataReader["character"].ToString(),
                                CreatedAt = Convert.ToDateTime(dataReader["created_at"]),
                            });
                        }

                        return logs.Count > 0 ? logs : null;
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
