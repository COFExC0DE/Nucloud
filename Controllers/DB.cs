using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;
using NuCloudWeb.Models;

namespace NuCloudWeb.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public sealed class DB : ControllerBase {
        private static DB instance = null;
        public IDriver Driver;

        public static DB Instance {
            get {
                if (instance == null)
                    instance = new DB();
                return instance;
            }
        }

        public void SetDriver(string uri, string user, string password) {
            Driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        }

        public void Dispose() {
            Driver?.Dispose();
        }

        public async Task<List<Member>> GetMembers() {
            IResultCursor cursor;
            var member = new List<Member>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(@"MATCH (a:Member)
                        RETURN a.name, a.lastName, a.phone, a.eMail, a.ced");
                member = await cursor.ToListAsync(record => new Member() {
                    Name = record["a.name"].As<string>(),
                    LastName = record["a.lastName"].As<string>(),
                    Phone = Int32.Parse(record["a.phone"].As<string>()),
                    Email = record["a.eMail"].As<string>(),
                    Ced = record["a.ced"].As<string>()
                });
            } finally {
                await session.CloseAsync();
            }
            return member;
        }

        public async Task<List<Member>> GetMembersOfGroup(int id) {
            IResultCursor cursor;
            var member = new List<Member>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(String.Format(@"MATCH (a:Member)-[:MemberOf]-(gr:Group) where gr.cod = {0} 
                                                RETURN a.name, a.lastName, a.phone, a.eMail, a.ced", id));
                member = await cursor.ToListAsync(record => new Member() {
                    Name = record["a.name"].As<string>(),
                    LastName = record["a.lastName"].As<string>(),
                    Phone = Int32.Parse(record["a.phone"].As<string>()),
                    Email = record["a.eMail"].As<string>(),
                    Ced = record["a.ced"].As<string>()
                });
            } finally {
                await session.CloseAsync();
            }
            return member;
        }

        public async void CreateChief(Chief chief) {
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                await session.RunAsync(String.Format("CREATE (ee:Coordinador { name: '{0}', lastName: '{1}', ced: '{2}', eMail: '{3}', phone: '{4}', start: '{5}', end: '{6}' })",
                                                        chief.Name, chief.LastName, chief.Ced, chief.Email, chief.Phone, chief.Start, chief.End));
            } finally {
                await session.CloseAsync();
            }
        }

        public async void CreateMember(Member member) {
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                await session.RunAsync(String.Format(@"CREATE (ee:Member { name: {0}, lastName: {1}, ced: {2}, eMail: {3}, phone: {4}})",
                                                        member.Name, member.LastName, member.Ced, member.Email, member.Phone));
            } finally {
                await session.CloseAsync();
            }
        }

        public async void AddZone(string ced, string name, int id) {
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                await session.RunAsync(String.Format(@"MATCH (e:Coordinador) where e.ced = {0}
                                                        CREATE(zo: Zone { name: {1}, cod: {2}})
                                                        CREATE(e) -[:Have]->(zo)
                                                        ",
                                                        ced, name, id));
            } finally {
                await session.CloseAsync();
            }
        }

        public async void AddBranch(int cod, string name, int id) {
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                await session.RunAsync(String.Format(@"MATCH (e:Zone) where e.cod = {0}
                                                        CREATE (br:Branch {name: {1}, cod: {2}})
                                                        CREATE(e) -[:Have]->(br)",
                                                        cod, name, id));
            } finally {
                await session.CloseAsync();
            }
        }

        public async void AddGroup(int cod, string name, int id) {
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                await session.RunAsync(String.Format(@"MATCH (e:Branch) where e.cod = {0}
                                                        CREATE (gr:Group {name: {1}, cod: {2}})
                                                        CREATE(e) -[:Have]->(gr)",
                                                        cod, name, id));
            } finally {
                await session.CloseAsync();
            }
        }
    }
}
