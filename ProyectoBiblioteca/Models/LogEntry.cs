using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoBiblioteca.Models
{
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Username { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
    }

   

}