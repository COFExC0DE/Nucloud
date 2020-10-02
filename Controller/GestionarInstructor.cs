using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sala.Model;

namespace Sala.Controller {
    class GestionarInstructor {
        public List<Instructor> Instructores { get; set; }

        internal void agregarInstructor(int i, string n, int t, string e) {
            Instructores.Add(new Instructor() {
                ID = i,
                Nombre = n,
                Telefono = t,
                Email = e
            });
            throw new NotImplementedException();
        }

        public Instructor obtenerInstructor(int cod) {
            return Instructores.Find(x => x.ID == cod);
        }

        public void agregarServicio(int codInstructor, Servicio servicio) {
            obtenerInstructor(codInstructor).Servicios.Add(servicio);
        }

        public void eliminarServicio(int codInstructor, int codServicio) {
            obtenerInstructor(codInstructor).Servicios.RemoveAll(x => x.Codigo == codServicio);
        }
    }
}
