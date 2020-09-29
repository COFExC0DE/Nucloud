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
    }
}