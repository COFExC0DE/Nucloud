using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sala.Model;

namespace Sala.Controller {
    class GestionarClase {
        List<Clase> Clases { get; set; }

        public GestionarClase() {
            Clases = new List<Clase>();
        }

        public Clase obtenerClase(string cod) {
            return Clases.Find(x => x.Codigo == cod);
        }

        public void cambiarSuplente(Clase clase, Instructor instructor) {
            clase.Instructor = instructor;
        }

        public void agregarEstudiante(Clase clase, Cliente cliente) {
            clase.Estudiantes.Add(cliente);
        }

        internal void agregarClase(string cod, int cupo, Instructor instructor, Instructor suplente, Servicio servicio, int duracion, Dia dia, int hora) {
            Clases.Add(new Clase() {
                Codigo = cod,
                Cupo = cupo,
                Instructor = instructor,
                Suplente = suplente,
                Servicio = servicio,
                Duracion = duracion,
                Hora = hora,
                Dia = dia
            });
        }
    }
}