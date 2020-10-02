using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sala.Model;

namespace Sala.Controller {
    class GestionarInstructor {
        List<Instructor> Instructores { get; set; }

        internal void agregarInstructor(int i, string n, int t, string e) {
            Instructores.Add(new Instructor() {
                ID = i,
                Nombre = n,
                Telefono = t,
                Email = e
            });
            throw new NotImplementedException();
        }

        Instructor obtenerServicio(int cod) {
            return Instructores.Find(x => x.ID == cod);
        }

        void agregarServicio(int codInstructor, Servicio servicio) {
            obtenerServicio(codInstructor).Servicios.Add(servicio);
        }

        void eliminarServicio(int codInstructor, int codServicio) {
            obtenerServicio(codInstructor).Servicios.RemoveAll(x => x.Codigo == codServicio);
        }
    }
}
