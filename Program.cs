using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Sala.Controller;
using Sala.Model;
using Sala.Controller;

namespace Sala {
    class Program {

        void agregarPersona(){

        }

        static void Main(string[] args) {
            bool repetir = true;
            List<Persona> person = new List<Persona>(1000);
            List<Servicio> service = new List<Servicio>(1000);
            List<Cliente> customr = new List<Cliente>(1000);
            List<Instructor> instruct = new List<Instructor>(1000);
            IGestionar gestor;

            //GestionarCliente g_Cliente = new GestionarCliente();// esto es de prueba
            do
            {

                Console.WriteLine("\n 1) Crear servicio para la sala");
                Console.WriteLine("2) Ingresar un nuevo cliente de la sala");
                Console.WriteLine("3) Ver los servicios de la sala");
                Console.WriteLine("4) Ver Clientes de la sala");
                Console.WriteLine("4) Ingresar un Nuevo Instructor");

                Console.WriteLine("Ingresar un valor (1, 2, 3...) que quiera realizar");
                int valor_Tomado = Int32.Parse(Console.ReadLine());

                if (valor_Tomado == 1)
                {
                    Console.WriteLine("\n Ingresar codigo y descripcion");
                    Servicio add_Servicio = new Servicio();

                    {
                        int Codigo = Int32.Parse(Console.ReadLine());
                        string Descripcion = Console.ReadLine();
                    };
                    service.Add(add_Servicio);

                    Console.WriteLine("\n Se guardo el nuevo servicio del GYM con exito");
                }
                if (valor_Tomado == 2)
                {
                    Persona add_Persona = generaPersona();
                    Cliente add_Cliente = new Cliente(add_Persona.ID);
                    

                    person.Add(add_Persona);
                    customr.Add(add_Cliente);

                    Console.WriteLine("\n Se guardo el nuevo Cliente");
                }
                
                if (valor_Tomado == 4)
                {
                    foreach (Cliente c in customr)
                    {
                        Console.WriteLine("ID de Persona = {0}, Moroso = {1}, Activo = {2}", c.vIdPersona, c.moroso, c.activo);
                    }
                }
                if (valor_Tomado == 5)
                {
                    Persona add_Persona = generaPersona();
                    Instructor add_Instructor = new Instructor(add_Persona.ID);

                }
            } while (repetir == true);
        }
        static Persona generaPersona() {
            Console.WriteLine("\n Ingresar ID, Nombre, Telefono y Email (En ese orden)");
            int I = Int32.Parse(Console.ReadLine());
            string N = Console.ReadLine();
            int T = Int32.Parse(Console.ReadLine());
            string E = Console.ReadLine();

            Persona add_Persona = new Persona()
            {
                ID = I,
                Nombre = N,
                Telefono = T,
                Email = E
            };
            return add_Persona;
        }
    }
}
