using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sala.Model {
    class Instructor : Persona {
        public List<Servicio> Servicios { get; set; }
    }
}
