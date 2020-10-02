using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sala.Model;

namespace Sala.Controller {
    class GestionarClase {
        List<Clase> Clases { get; set; }

        Clase obtenerServicio(string cod) {
            return Clases.Find(x => x.Codigo == cod);
        }

        void cambiarSuplente(Clase clase, Instructor instructor) {
            clase.Instructor = instructor;
        }

        void agregarEstudiante(Clase clase, Cliente cliente) {
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
            throw new NotImplementedException();
        }
    }
}