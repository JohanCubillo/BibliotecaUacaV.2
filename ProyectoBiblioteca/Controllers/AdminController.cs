using ProyectoBiblioteca.Logica;
using ProyectoBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoBiblioteca.Controllers
{
    public class AdminController : Controller
    {
        private Log log = new Log();
        private static Persona oPesona;
        // GET: Admin
        public ActionResult Index()
        {
            if(Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");

            oPesona = (Persona)Session["Usuario"];

            return View();
        }

        public ActionResult CerrarSesion()
        {

            log.AddEntry(new LogEntry { Timestamp = DateTime.Now, Username = oPesona.Correo, Level = "BACK", Message = "Termino su session" });
            log.InsertarDatosEnBD(DateTime.Now, oPesona.Correo, "BACK", "Termino su session");
            Session["Usuario"] = null;
            return RedirectToAction("Index", "Login");
            

        }

    }
}