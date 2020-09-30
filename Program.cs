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
                Persona ing_Persona = new Persona()
                {
                    ID = Int32.Parse(Console.ReadLine()),
                    Nombre = Console.ReadLine(),
                    Telefono = Int32.Parse(Console.ReadLine()),
                    Email = Console.ReadLine()
                };
                
                customr.Add(ing_Persona);
                foreach (Persona c in customr)
                {
                    Console.WriteLine("ID = {0}, Nombre = {1}, Telefono = {2}, email = {3}", c.ID, c.Nombre, c.Telefono, c.Email);
                }
            } while (repetir == true);
        }
    }
}
