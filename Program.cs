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

                Console.WriteLine("1) Crear servicio para la sala");
                Console.WriteLine("2) Ingresar un nuevo cliente de la sala");
                Console.WriteLine("3) Ver los servicios de la sala");
                Console.WriteLine("4) Ver Clientes de la sala");
                Console.WriteLine("5) Ingresar un Nuevo Instructor");
                Console.WriteLine("6) Ver Instructores");
                Console.WriteLine("7) Agregar servicio a Instructor");
                Console.WriteLine("8) Eliminar servicio a Instructor");
                Console.WriteLine("9) Agregar clase");
                Console.WriteLine("10) Agregar estudiante a clase");
                Console.WriteLine("11) Cambiar suplente de clase");
                Console.WriteLine("12) Ver horario");
                Console.WriteLine("13) Salir");
                int valor_Tomado = Int32.Parse(Console.ReadLine());
                Console.Clear();
                if (valor_Tomado == 1) {
                    Console.WriteLine("Ingresar codigo y descripcion");

                    int Codigo = Int32.Parse(Console.ReadLine());
                    string Descripcion = Console.ReadLine();

                    gServicio.agregarServicio(Codigo, Descripcion);

                    Console.WriteLine("Se guardo el nuevo servicio del GYM con exito");
                }

                if (valor_Tomado == 2) {
                    Console.WriteLine("Ingresar ID, Nombre, Telefono y Email (En ese orden)");
                    int I = Int32.Parse(Console.ReadLine());
                    string N = Console.ReadLine();
                    int T = Int32.Parse(Console.ReadLine());
                    string E = Console.ReadLine();

                    gCliente.ingresarCliente(I, N, T, E);

                    Console.WriteLine("Se guardo el nuevo Cliente");
                }

                if (valor_Tomado == 3) {
                    foreach (Servicio s in gServicio.Servicios) {
                        Console.WriteLine("{0} {1}", s.Codigo, s.Descripcion);
                    }
                }

                if (valor_Tomado == 4) {
                    foreach (Cliente c in gCliente.Clientes) {
                        Console.WriteLine("{0} {1} {2} {3}", c.ID, c.Nombre, c.activo ? "Activo" : "", c.moroso ? "Moroso" : "");
                    }
                }

                if (valor_Tomado == 5) {

                    Console.WriteLine("Ingresar ID, Nombre, Telefono y Email (En ese orden) del Instructor");
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
                    Console.WriteLine("Ingresar id del instructor y codigo del servicio (En ese orden) que desea agregar");
                    int id = Int32.Parse(Console.ReadLine());
                    int cod = Int32.Parse(Console.ReadLine());
                    if (gInstructor.obtenerInstructor(id) != null && gServicio.obtenerServicio(cod) != null) {
                        gInstructor.agregarServicio(id, gServicio.obtenerServicio(cod));
                    } else {
                        Console.WriteLine("Servicio o instructor no existen");
                    }
                }

                if (valor_Tomado == 8) {
                    Console.WriteLine("Ingresar id del instructor y codigo del servicio (En ese orden) que desea eliminar");
                    int id = Int32.Parse(Console.ReadLine());
                    int cod = Int32.Parse(Console.ReadLine());
                    if (gInstructor.obtenerInstructor(id) != null && gServicio.obtenerServicio(cod) != null) {
                        gInstructor.eliminarServicio(id, cod);
                    } else {
                        Console.WriteLine("Servicio o instructor no existen");
                    }
                }

                if (valor_Tomado == 9) {
                    Console.WriteLine("Ingrese codigo de clase");
                    string cod = Console.ReadLine();

                    Console.WriteLine("Ingrese cupo de clase");
                    int cupo = Int32.Parse(Console.ReadLine());

                    Console.WriteLine("Ingrese id de Instructor");
                    int idIns = Int32.Parse(Console.ReadLine());

                    Console.WriteLine("Ingrese id de Suplente");
                    int idSup = Int32.Parse(Console.ReadLine());

                    Console.WriteLine("Ingrese cod de Servicio");
                    int codSer = Int32.Parse(Console.ReadLine());

                    Console.WriteLine("Ingrese duracion de la clase");
                    int duracion = Int32.Parse(Console.ReadLine());
                    Dia dia;
                    while (true) {
                        int input = Int32.Parse(Console.ReadLine());
                        Console.WriteLine("Elegir dia\n1.Lunes\n2.Martes\n3.Miercoles\n4.Jueves\n5.Viernes\n6.Sabado\n7.Domingo");

                        if (input > 0 && input < 8) {
                            dia = (Dia)input;
                            break;
                        }
                    }

                    Console.WriteLine("Ingrese hora de la clase (Numero del 0 al 23)");
                    int hora = Int32.Parse(Console.ReadLine());
                    if (gInstructor.obtenerInstructor(idIns) != null && gInstructor.obtenerInstructor(idSup) != null && gServicio.obtenerServicio(codSer) != null) { 
                        gClase.agregarClase(cod, cupo, gInstructor.obtenerInstructor(idIns), gInstructor.obtenerInstructor(idSup), gServicio.obtenerServicio(codSer), duracion, dia, hora);
                    }
                }

                if (valor_Tomado == 10) {
                    Console.WriteLine("Ingrese id de estudiante");
                    int id = Int32.Parse(Console.ReadLine());

                    Console.WriteLine("Ingrese codigo de clase");
                    string cod = Console.ReadLine();

                    if (gClase.obtenerClase(cod) != null && gCliente.obtenerCliente(id) != null) {
                        gClase.agregarEstudiante(gClase.obtenerClase(cod), gCliente.obtenerCliente(id));
                    } else {
                        Console.WriteLine("No existe el instructor o clase");
                    }
                }

                if (valor_Tomado == 11) {
                    Console.WriteLine("Ingrese id de suplente");
                    int id = Int32.Parse(Console.ReadLine());

                    Console.WriteLine("Ingrese codigo de clase");
                    string cod = Console.ReadLine();

                    if (gClase.obtenerClase(cod) != null && gInstructor.obtenerInstructor(id) != null) {
                        gClase.cambiarSuplente(gClase.obtenerClase(cod), gInstructor.obtenerInstructor(id));
                    } else {
                        Console.WriteLine("No existe el instructor o clase");
                    }
                }

                if (valor_Tomado == 12) {
                    foreach (Clase clase in gClase.Clases) {
                        gClase.printClase(clase);
                    }
                }

                if (valor_Tomado == 13) {
                    repetir = false;
                }

                Console.WriteLine("\nPresione cualquier tecla para continuar");
                Console.ReadLine();            
                Console.Clear();
            } while (repetir);
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
