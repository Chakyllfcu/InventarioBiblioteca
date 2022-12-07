using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoBiblioteca.Models
{
    public class Persona
    {
        public int IdPersona { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public string Codigo { get; set; }
        public int Salario { get; set; }
        public int AFP { get; set; }
        public int ARS { get; set; }
        public int SalarioNeto { get; set; }
        public TipoPersona oTipoPersona { get; set; }
        public bool Estado { get; set; }


    }
}