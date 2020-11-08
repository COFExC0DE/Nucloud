using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;

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

        public async Task<List<string>> GetAll() {
            IResultCursor cursor;
            var people = new List<string>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(@"MATCH (a:Person)
                        RETURN a.name as name
                        limit 10");
                people = await cursor.ToListAsync(record => record["name"].As<string>());
            } finally {
                await session.CloseAsync();
            }
            return people;
        }
    }
}
