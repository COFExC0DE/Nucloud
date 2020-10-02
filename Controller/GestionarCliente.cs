using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sala.Model;

namespace Sala.Controller {
    class GestionarCliente {
        List<Cliente> Clientes { get; set; }

        Cliente obtenerCliente(int id) {//Intentar esto :D public String obtenerCliente(Cliente id)
            return Clientes.Find(x => x.ID == id);
        }

        void aplicarPago(int id) { 
        
        }
    }
}
