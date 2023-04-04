using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ProyectoBiblioteca.Logica
{
    public class Utilidades
    {
        public static string convertirBase64(string ruta)
        {
            // Si no existe la ruta devuelve null.
            if (!File.Exists(ruta))
                return null;

            byte[] bytes = File.ReadAllBytes(ruta);
            string file = Convert.ToBase64String(bytes);
            return file;
        }
    }
}