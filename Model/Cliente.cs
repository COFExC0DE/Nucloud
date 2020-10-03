using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sala.Model {
    class Cliente : Persona {
        public DateTime fechaMatricula;
        public bool moroso { get; set; }
        public bool activo { get; set; }

        public Cliente() {
            fechaMatricula = DateTime.Now;
            moroso = false;
            activo = true;
        }

    }
}
