using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NuCloudWeb.Models;

namespace NuCloudWeb.Controllers {
    public class TestController : Controller {
        // 
        // GET: /Test/
        public IActionResult Index() {
            return View();
        }

        // 
        // GET: /Test/Crear/ 
        public void Crear() {
            /*DB.Instance.CreateCoordination(new Coordination() {
                Name = "Coordinacion",
                Cod = 1
            });
            
            DB.Instance.AddZone(1, new Node() {
                Name = "Zona 1",
                Cod = 1
            });

            DB.Instance.AddZone(1, new Node() {
                Name = "Zona 2",
                Cod = 2
            });
            /*
            DB.Instance.AddBranch(1, new Node() {
                Name = "Rama 1",
                Cod = 1
            });

            DB.Instance.AddBranch(1, new Node() {
                Name = "Rama 2",
                Cod = 2
            });

            DB.Instance.AddBranch(2, new Node() {
                Name = "Rama 3",
                Cod = 3
            });
            
            DB.Instance.AddGroup(1, new Group() {
                Name = "Grupo 1",
                Cod = 1
            });

            DB.Instance.AddGroup(2, new Group() {
                Name = "Grupo 2",
                Cod = 2
            });

            DB.Instance.AddGroup(3, new Group() {
                Name = "Grupo 3",
                Cod = 3
            });

            DB.Instance.AddGroup(1, new Group() {
                Name = "Grupo 4",
                Cod = 4
            });

            DB.Instance.AddGroup(2, new Group() {
                Name = "Grupo 5",
                Cod = 5
            }); 

            DB.Instance.CreateChief(new Chief() {
                Name = "Ericka",
                LastName = "Solano",
                Email = "esolano@mail.com",
                Ced = "1",
                Phone = 8754346
            });

            DB.Instance.CreateMember(new Member() {
                Name = "Andrés",
                LastName = "Castro",
                Email = "frodo@mail.com",
                Ced = "2",
                Phone = 78946416
            });

            DB.Instance.CreateMember(new Member() {
                Name = "Gerarld",
                LastName = "Sanchez",
                Email = "kixo@mail.com",
                Ced = "3",
                Phone = 46874156
            });

            DB.Instance.CreateMember(new Member() {
                Name = "Nakisha",
                LastName = "Dixon",
                Email = "naki@mail.com",
                Ced = "4",
                Phone = 78944188
            });

            DB.Instance.CreateMember(new Member() {
                Name = "Jose",
                LastName = "Barrientos",
                Email = "barr@mail.com",
                Ced = "5",
                Phone = 7894654
            });

            DB.Instance.CreateMember(new Member() {
                Name = "Maria",
                LastName = "Duran",
                Email = "duran@mail.com",
                Ced = "6",
                Phone = 56465464
            });

            DB.Instance.CreateMember(new Member() {
                Name = "Jose Pablo",
                LastName = "Cruz",
                Email = "art@mail.com",
                Ced = "7",
                Phone = 77824563
            });

            DB.Instance.CreateMember(new Member() {
                Name = "Tobias",
                LastName = "Bolaños",
                Email = "toby@mail.com",
                Ced = "8",
                Phone = 876645
            });

            DB.Instance.CreateMember(new Member() {
                Name = "Juan",
                LastName = "Santamaría",
                Email = "fuego@mail.com",
                Ced = "9",
                Phone = 7654654
            });

            DB.Instance.CreateMember(new Member() {
                Name = "Carlos",
                LastName = "Castro",
                Email = "cc@mail.com",
                Ced = "10",
                Phone = 3213123
            }); 

            DB.Instance.AddMemberToGroup(1,"2");
            DB.Instance.AddMemberToGroup(2, "3");
            DB.Instance.AddMemberToGroup(3, "4");
            DB.Instance.AddMemberToGroup(4, "5");
            DB.Instance.AddMemberToGroup(5, "6");
            DB.Instance.AddMemberToGroup(2, "7");
            DB.Instance.AddMemberToGroup(3, "8");
            DB.Instance.AddMemberToGroup(4, "9");
            DB.Instance.AddMemberToGroup(5, "10"); 

            DB.Instance.MakeMemberGroupLeader(1, "2");
            DB.Instance.MakeMemberGroupLeader(2, "3");
            DB.Instance.MakeMemberGroupLeader(3, "4");
            DB.Instance.MakeMemberGroupLeader(4, "5");
            DB.Instance.MakeMemberGroupLeader(5, "6");

            DB.Instance.AddMemberToNode(1, "2", "Rama");
            DB.Instance.AddMemberToNode(2, "3", "Rama");
            DB.Instance.AddMemberToNode(3, "4", "Rama");
            DB.Instance.AddMemberToNode(1, "5", "Rama");
            DB.Instance.AddMemberToNode(2, "6", "Rama"); 

            DB.Instance.AddChiefToCoord(1, "1"); */

        }

    }
}
