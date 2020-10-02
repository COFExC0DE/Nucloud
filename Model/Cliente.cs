using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sala.Model {
    class Cliente : Persona {
        public int vIdPersona;
        public bool Moroso { get; set; }
        public bool Activo { get; set; }

        public Cliente(int pIdPersona)
        {
            vIdPersona = pIdPersona;
            Moroso = false;
            Activo = true;
        }
    }
}
