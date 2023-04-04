using ProyectoBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

public class Log
{
    private List<LogEntry> entries = new List<LogEntry>();

    public void AddEntry(LogEntry entry)
    {
        entries.Add(entry);
    }

    public void ExportToFile(string filename)
    {
        using (StreamWriter writer = new StreamWriter(filename))
        {
            foreach (LogEntry entry in entries)
            {
                writer.WriteLine($"{entry.Timestamp}: [{entry.Level}] {entry.Username}: {entry.Message}");
            }
        }
    }


    public void InsertarDatosEnBD(DateTime time, string user, string level, string messaje)
    {
        string connectionString = "Data Source=DESKTOP-P0DH32H;Initial Catalog=DB_BIBLIOTECA;Integrated Security=True;";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            foreach (LogEntry entry in entries)
            {

                string query = "INSERT INTO logss (time, usuario,nivel, messaje) VALUES (@time, @users, @nivel, @messaje)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@time", entry.Timestamp);
                    command.Parameters.AddWithValue("@users", entry.Username);
                    command.Parameters.AddWithValue("@nivel", entry.Level);
                    command.Parameters.AddWithValue("@messaje", entry.Message);
                    connection.Open();
                    command.ExecuteNonQuery();
                }

        }
    }


    }
}






