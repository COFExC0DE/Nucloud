using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sala.Model;

namespace Sala.Controller {
    class GestionarCliente {
        public List<Cliente> Clientes { get; set; }

        public GestionarCliente() {
            Clientes = new List<Cliente>();
        }

        public void ingresarCliente(int id, string n, int t, string e) {
            Clientes.Add(new Cliente() { 
                ID = id,
                Nombre = n,
                Telefono = t,
                Email = e
            });
        }

        public Cliente obtenerCliente(int id) {
            return Clientes.Find(x => x.ID == id);
        }

        void aplicarPago(int id) { 
        
        }
    }
}
