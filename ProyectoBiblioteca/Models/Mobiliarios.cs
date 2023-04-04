using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoBiblioteca.Models
{
    public class Mobiliarios
    {
        public int IdMobiliario { get; set; }
        public string NombreObjeto { get; set; }
        public string Marca { get; set; }
        public string Color { get; set; }
        
        public Marca oMarca { get; set; }
        public string Ubicacion { get; set; }
        public int Cantidad { get; set; }
        public bool Estado { get; set; }

        public string base64 { get; set; }
        public string extension { get; set; }
    }
}