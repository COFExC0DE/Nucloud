using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sala.Model;

namespace Sala.Controller {
    class EncargadoServicio {
        public List<Servicio> Servicios { get; set; }

        public EncargadoServicio() {
            Servicios = new List<Servicio>();
        }

        public void agregarServicio(int cod, string desc) {
            Servicios.Add(new Servicio() {
                Codigo = cod,
                Descripcion = desc
            });
        }

        public Servicio obtenerServicio(int cod) {
            return Servicios.Find(x => x.Codigo == cod);
        }
    }
}
