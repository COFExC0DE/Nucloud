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

        public async Task<List<Member>> GetAll() {
            IResultCursor cursor;
            var people = new List<Member>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(@"MATCH (a:Member)
                        RETURN a.name, a.lastName, a.phone, a.eMail, a.ced");
                people = await cursor.ToListAsync(record => new Member() {
                    Name = record["a.name"].As<string>(),
                    LastName = record["a.lastName"].As<string>(),
                    Phone = Int32.Parse(record["a.phone"].As<string>()),
                    Email = record["a.eMail"].As<string>(),
                    Ced = record["a.ced"].As<string>()
                });
            } finally {
                await session.CloseAsync();
            }
            return people;
        }
    }
}
