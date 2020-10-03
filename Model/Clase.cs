using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sala.Model {

    enum Dia { Lunes, Martes, Miercoles, Jueves, Viernes, Sabado, Domingo }

    class Clase {
        public string Codigo { get; set; }
        public int Cupo { get; set; }
        public Instructor Instructor { get; set; }
        public Instructor Suplente { get; set; }
        public Servicio Servicio { get; set; }
        public List<Cliente> Estudiantes { get; set; }
        public Dia Dia { get; set; }
        public int Duracion { get; set; }
        public int Hora { get; set; }

        public Clase() {
            Estudiantes = new List<Cliente>();
        }
    }
}
