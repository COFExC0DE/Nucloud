using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
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
            do
            {

                Console.WriteLine("\n 1) Crear servicio para la sala");
                Console.WriteLine("2) Ingresar un nuevo miembro de la sala");
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
                    Console.WriteLine("\n Ingresar ID, Nombre, Telefono y Email (En ese orden)");
                    Persona add_Persona = new Persona()
                    {   
                        ID = Int32.Parse(Console.ReadLine()),
                        Nombre = Console.ReadLine(),
                        Telefono = Int32.Parse(Console.ReadLine()),
                        Email = Console.ReadLine()
                    };
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
