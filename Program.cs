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

namespace Sala {
    class Program {

        static void Main(string[] args) {


            bool repetir = true;
            List<Persona> customr = new List<Persona>(1000);
            List<Servicio> service = new List<Servicio>(1000);

            GestionarCliente g_Cliente = new GestionarCliente();// esto es de prueba
            bool f = false;// esto es de prueba
            bool t = true;// esto es de prueba
            Cliente add_Cliente = new Cliente()// esto es de prueba
            {
                Moroso = t,// esto es de prueba
                Activo = f// esto es de prueba
            };// esto es de prueba

            do
            {

                Console.WriteLine("\n 1) Crear servicio para la sala");
                Console.WriteLine("2) Ingresar un nuevo cliente de la sala");
                Console.WriteLine("3) Ver los servicios de la sala");
                Console.WriteLine("4) Ver miembros de la sala");

                Console.WriteLine("Ingresar un valor (1, 2, 3...) que quiera realizar");
                int valor_Tomado = Int32.Parse(Console.ReadLine());

                if (valor_Tomado == 1)
                {
                    Console.WriteLine("\n Ingresar codigo y descripcion");
                    Servicio add_Servicio = new Servicio()
                    {
                        Codigo = Int32.Parse(Console.ReadLine()),
                        Descripcion = Console.ReadLine()
                    };
                    service.Add(add_Servicio);

                    Console.WriteLine("\n Se guardo el nuevo servicio del GYM con exito");
                }
                if (valor_Tomado == 2)
                {
                    int I = Int32.Parse(Console.ReadLine());
                    string N = Console.ReadLine();
                    int T = Int32.Parse(Console.ReadLine());
                    string E = Console.ReadLine();

                    Console.WriteLine("\n Ingresar ID, Nombre, Telefono y Email (En ese orden)");
                    Persona add_Persona = new Persona()
                    {   
                        ID = I,
                        Nombre = N,
                        Telefono = T,
                        Email = E
                    };
                    add_Cliente.ID = I;// esto es de prueba
                    g_Cliente.addClientes(add_Cliente);// esto es de prueba


                    customr.Add(add_Persona);
                    Console.WriteLine("\n Se guardo el nuevo Cliente");
                }
                
                if (valor_Tomado == 4)
                {
                    foreach (Persona c in customr)
                    {
                        Console.WriteLine("ID = {0}, Nombre = {1}, Telefono = {2}, email = {3}", c.ID, c.Nombre, c.Telefono, c.Email);
                    }
                }
                if (valor_Tomado == 3)
                {
                    foreach (Servicio s in service)
                    {
                        Console.WriteLine("Codigo = {0}, Descripcion = {1}", s.Codigo, s.Descripcion);
                    }
                }
            } while (repetir == true);
        }
    }
}
