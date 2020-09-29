using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sala.Model;

namespace Sala.Controller {
    class GestionarServicio {
        List<Servicio> Servicios { get; set; }

        Servicio obtenerServicio(int cod) {
            return Servicios.Find(x => x.Codigo == cod);
        }
    }
}
