using ProyectoBiblioteca.Logica;
using ProyectoBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace ProyectoBiblioteca.Controllers
{
    public class LoginController : Controller
    {

        private Log log = new Log();
        // GET: Login
        public ActionResult Index()
        {
            return View();
             
    }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            string connectionString = "Data Source=DESKTOP-P0DH32H;Initial Catalog=DB_BIBLIOTECA;Integrated Security=True";

           

            Persona ousuario = PersonaLogica.Instancia.Listar().Where(u => u.Correo == correo && u.Clave == clave && u.oTipoPersona.IdTipoPersona != 5 && u.Estado == true).FirstOrDefault();

            if (ousuario == null)
            {
                log.AddEntry(new LogEntry { Timestamp = DateTime.Now, Username = correo, Level = "WARN", Message = "Inicio de sesión fallido" });
                log.InsertarDatosEnBD(DateTime.Now, correo,"WARN", "hola");

                

                // Crear una conexión a la base de datos
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Abrir la conexión
                    connection.Open();

                    SqlCommand command = new SqlCommand("UPDATE PERSONA SET FailedAttempts = FailedAttempts + 1 WHERE Correo = @correo", connection);
                    SqlCommand commandsql = new SqlCommand("UPDATE PERSONA SET Estado = 0, FailedAttempts = 0 WHERE Correo = @correo AND FailedAttempts > 1", connection);

                    command.Parameters.AddWithValue("@correo", correo);
                    commandsql.Parameters.AddWithValue("@correo", correo);
                    // Ejecutar el comando SQL
                    command.ExecuteNonQuery();
                    commandsql.ExecuteNonQuery();


                    
                    SqlCommand commands = new SqlCommand("SELECT Estado FROM PERSONA WHERE Correo = @correo", connection);
                    commands.Parameters.AddWithValue("@correo", correo);


                    bool estado = (bool)commands.ExecuteScalar();

                    if (!estado)
                    {
                        ViewBag.error = "Su contraseña fue bloqueada por intentos fallidos, contacte un administrador";
                    }
                    else
                    {
                        ViewBag.Error = "Usuario o contraseña no correcta,o esta desabilitada,consulte al Administrador o cree una nueva cuenta";
                    }

                    connection.Close();

                }
               





                return View();
               
            }
            else
            {
                Session["Usuario"] = ousuario;
                log.AddEntry(new LogEntry { Timestamp = DateTime.Now, Username = correo, Level = "SUSS", Message = "Inicio de sesión exitoso" });
                log.InsertarDatosEnBD(DateTime.Now, correo, "SUSS", "Inicio de sesión exitoso");
                return RedirectToAction("Index", "Admin");
            }
            

        }

       



        public ActionResult MostrarDatos()
        {
            string connectionString = "Data Source=DESKTOP-P0DH32H;Initial Catalog=DB_BIBLIOTECA;Integrated Security=True";
            List<LogEntry> logi = new List<LogEntry>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM logss";

                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DateTime Time = (DateTime)reader["time"];
                    string Usuario = (string)reader["usuario"];
                    string Nivel = (string)reader["nivel"];
                    string Messajes = (string)reader["messaje"];

                    logi.Add(new LogEntry {  Timestamp = Time, Username = Usuario, Level = Nivel, Message = Messajes });
                }

                reader.Close();
            }

            return View(logi);
        }
    }


    






}