using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sala.Model {
    class Cliente{
        List<Cliente> listaCliente = new List<Cliente>(1000);
        public int vIdPersona;
        public DateTime fechaMatricula;
        public bool moroso { get; set; }
        public bool activo { get; set; }

        public Cliente(int pIdPersona)
        {
            vIdPersona = pIdPersona;
            fechaMatricula = DateTime.Now;
            moroso = false;
            activo = true;
        }

        public void agregarCliente(Cliente cliente){
            listaCliente.Add(cliente);
        }
    }
}
