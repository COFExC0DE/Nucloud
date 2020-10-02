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

        void agregarPersona() {

        }

        static void Main(string[] args) {
            bool repetir = true;

            GestionarClase gClase = new GestionarClase();
            GestionarCliente gCliente = new GestionarCliente();
            GestionarInstructor gInstructor = new GestionarInstructor();
            GestionarServicio gServicio = new GestionarServicio();

            //GestionarCliente g_Cliente = new GestionarCliente();// esto es de prueba
            do {

                Console.WriteLine("\n 1) Crear servicio para la sala");
                Console.WriteLine("2) Ingresar un nuevo cliente de la sala");
                Console.WriteLine("3) Ver los servicios de la sala");
                Console.WriteLine("4) Ver Clientes de la sala");
                Console.WriteLine("5) Ingresar un Nuevo Instructor");
                Console.WriteLine("6) Ver Instructores");
                Console.WriteLine("7) Agregar servicio a Instructor");
                Console.WriteLine("7) Eliminar servicio a Instructor");

                Console.WriteLine("Ingresar un valor (1, 2, 3...) que quiera realizar");
                int valor_Tomado = Int32.Parse(Console.ReadLine());

                if (valor_Tomado == 1) {
                    Console.WriteLine("\n Ingresar codigo y descripcion");

                    int Codigo = Int32.Parse(Console.ReadLine());
                    string Descripcion = Console.ReadLine();

                    gServicio.agregarServicio(Codigo, Descripcion);

                    Console.WriteLine("\n Se guardo el nuevo servicio del GYM con exito");
                }
                if (valor_Tomado == 2) {
                    Console.WriteLine("\n Ingresar ID, Nombre, Telefono y Email (En ese orden)");
                    int I = Int32.Parse(Console.ReadLine());
                    string N = Console.ReadLine();
                    int T = Int32.Parse(Console.ReadLine());
                    string E = Console.ReadLine();

                    gCliente.ingresarCliente(I, N, T, E);

                    Console.WriteLine("\n Se guardo el nuevo Cliente");
                }
                if (valor_Tomado == 3) {
                    foreach (Servicio s in gServicio.Servicios) {
                        Console.WriteLine("{0} {1}", s.Codigo, s.Descripcion);
                    }
                }

                if (valor_Tomado == 4) {
                    foreach (Cliente c in gCliente.Clientes) {
                        Console.WriteLine("ID de Persona = {0}, Moroso = {1}, Activo = {2}", c.ID, c.moroso, c.activo);
                    }
                }
                if (valor_Tomado == 5) {

                    Console.WriteLine("\n Ingresar ID, Nombre, Telefono y Email (En ese orden) del Instructor");
                    int I = Int32.Parse(Console.ReadLine());
                    string N = Console.ReadLine();
                    int T = Int32.Parse(Console.ReadLine());
                    string E = Console.ReadLine();

                    gInstructor.agregarInstructor(I, N, T, E);
                }
                if (valor_Tomado == 6) {
                    foreach (Instructor c in gInstructor.Instructores) {
                        Console.WriteLine("{0} {1}", c.ID, c.Nombre);
                    }
                }
                if (valor_Tomado == 7) {
                    Console.WriteLine("\n Ingresar id del instructor y codigo del servicio (En ese orden) que desea agregar");
                    int id = Int32.Parse(Console.ReadLine());
                    int cod = Int32.Parse(Console.ReadLine());
                    if (gInstructor.obtenerInstructor(id) != null && gServicio.obtenerServicio(cod) != null) {
                        gInstructor.agregarServicio(id, gServicio.obtenerServicio(cod));
                    } else {
                        Console.WriteLine("Servicio o instructor no existen");
                    }
                }
                if (valor_Tomado == 8) {
                    Console.WriteLine("\n Ingresar id del instructor y codigo del servicio (En ese orden) que desea eliminar");
                    int id = Int32.Parse(Console.ReadLine());
                    int cod = Int32.Parse(Console.ReadLine());
                    if (gInstructor.obtenerInstructor(id) != null && gServicio.obtenerServicio(cod) != null) {
                        gInstructor.eliminarServicio(id, cod);
                    } else {
                        Console.WriteLine("Servicio o instructor no existen");
                    }
                }
            } while (repetir == true);
        }
        static Persona generaPersona() {
            Console.WriteLine("\n Ingresar ID, Nombre, Telefono y Email (En ese orden)");
            int I = Int32.Parse(Console.ReadLine());
            string N = Console.ReadLine();
            int T = Int32.Parse(Console.ReadLine());
            string E = Console.ReadLine();

            Persona add_Persona = new Persona() {
                ID = I,
                Nombre = N,
                Telefono = T,
                Email = E
            };
            return add_Persona;
        }
    }
}
