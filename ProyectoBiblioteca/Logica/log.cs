using System;
using System.Data.SqlClient;

class SessionLogger
{
   
    public static string connectionString = "Data Source=.;Initial Catalog=DB_BIBLIOTECA;Integrated Security=True";

    public static void Log(string message)
    {
        string sql = "INSERT INTO SessionLog (Timestamp, Message) VALUES (@Timestamp, @Message)";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Timestamp", DateTime.Now);
            command.Parameters.AddWithValue("@Message", message);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}



