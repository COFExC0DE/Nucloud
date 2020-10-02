using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sala.Model {
    class Instructor {
        public int idPersona;
        public List<Servicio> Servicios { get; set; }

        public Instructor(int pIdPersona)
        {
            idPersona = pIdPersona;
        }
    }

    
}
