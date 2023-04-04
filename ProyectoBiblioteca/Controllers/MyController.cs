using Microsoft.AspNetCore.Mvc;
using ProyectoBiblioteca.Models;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Web.Mvc;

// ...
public class MyController : Controller
{
        public ActionResult MyAction()
        {
            string connectionString = "Data Source=.;Initial Catalog=DB_BIBLIOTECA;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Ejecutar consulta SQL
                SqlCommand command = new SqlCommand("SELECT time, usuario, nivel, messaje FROM logss", connection);
                SqlDataReader reader = command.ExecuteReader();

                // Guardar los resultados en un modelo o una lista
                List<LogEntry> myData = new List<LogEntry>();
                while (reader.Read())
                {
                    LogEntry model = new LogEntry();
                    model.Timestamp = reader.GetDateTime(0);
                    model.Username = reader.GetString(1);
                    model.Level = reader.GetString(2);
                    model.Message = reader.GetString(3);
                    myData.Add(model);
                }

                // Pasar los resultados a la vista
                return View(myData);
            }
        }
}

