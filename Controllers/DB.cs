using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;
using Neo4jClient;
using NuCloudWeb.Models;

namespace NuCloudWeb.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public sealed class DB : ControllerBase {
        private static DB instance = null;
        public IDriver Driver;
        public BoltGraphClient Client;

        public static DB Instance {
            get {
                if (instance == null)
                    instance = new DB();
                return instance;
            }
        }

        public async Task<Coordination> GetCoordination(int cod) {
            IResultCursor cursor;
            var Coordinations = new List<Coordination>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(String.Format(@"MATCH (a:Coordination) WHERE a.Cod = {0} return a.Name, a.Cod", cod));
                Coordinations = await cursor.ToListAsync(record => new Coordination() {
                    Name = record["a.Name"].As<string>(),
                    Cod = Int32.Parse(record["a.Cod"].As<string>())
                });
            } finally {
                await session.CloseAsync();
            }
            return Coordinations[0];
        }

        public void SetDriver(string uri, string user, string password) {
            Driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
            Client = new BoltGraphClient(uri, user, password);
        }

        internal async void AddCloud(Cloud g) {
            await Client.ConnectAsync();

            await Client.Cypher
                .Create("(c:Cloud {cloud})-[:Has]->(a:Coordination {coord})")
                .WithParams(new { 
                    cloud = g,
                    coord = new Coordination() { 
                        Name = g.Name,
                        Cod = g.Cod
                    }
                })
                .ExecuteWithoutResultsAsync();
        }

        public async Task<List<Cloud>> GetClouds() {
            IResultCursor cursor;
            var cloud = new List<Cloud>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(@"MATCH (a:Cloud)
                                                RETURN a.Name, a.PhoneNumber, a.Website, a.Cod");
                cloud = await cursor.ToListAsync(record => new Cloud() {
                    Name = record["a.Name"].As<string>(),
                    Website = record["a.Website"].As<string>(),
                    Cod = Int32.Parse(record["a.Cod"].As<string>()),
                    PhoneNumber = Int32.Parse(record["a.PhoneNumber"].As<string>()),
                });
            } finally {
                await session.CloseAsync();
            }
            return cloud;
        }

        public async Task<Cloud> GetCloud(int cod) {
            IResultCursor cursor;
            var cloud = new List<Cloud>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(String.Format(@"MATCH (a:Cloud) where a.Cod = {0}
                                                RETURN a.Name, a.PhoneNumber, a.Website, a.Cod", cod));
                cloud = await cursor.ToListAsync(record => new Cloud() {
                    Name = record["a.Name"].As<string>(),
                    Website = record["a.Website"].As<string>(),
                    Cod = Int32.Parse(record["a.Cod"].As<string>()),
                    PhoneNumber = Int32.Parse(record["a.PhoneNumber"].As<string>()),
                });
            } finally {
                await session.CloseAsync();
            }
            return cloud[0];
        }

        public void Dispose() {
            Driver?.Dispose();
        }

        public async Task<List<Member>> GetMonitors() {
            IResultCursor cursor;
            var member = new List<Member>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(@"MATCH (a:Miembro)-[:MemberOf]->(:Rama)
                                                RETURN a.Name, a.LastName, a.Phone, a.Email, a.Ced");
                member = await cursor.ToListAsync(record => new Member() {
                    Name = record["a.Name"].As<string>(),
                    LastName = record["a.LastName"].As<string>(),
                    Phone = Int32.Parse(record["a.Phone"].As<string>()),
                    Email = record["a.Email"].As<string>(),
                    Ced = record["a.Ced"].As<string>()
                });
            } finally {
                await session.CloseAsync();
            }
            return member;
        }

        public async Task<List<Member>> GetMembers() {
            IResultCursor cursor;
            var member = new List<Member>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(@"MATCH (a:Miembro)
                                                RETURN a.Name, a.LastName, a.Phone, a.Email, a.Ced");
                member = await cursor.ToListAsync(record => new Member() {
                    Name = record["a.Name"].As<string>(),
                    LastName = record["a.LastName"].As<string>(),
                    Phone = Int32.Parse(record["a.Phone"].As<string>()),
                    Email = record["a.Email"].As<string>(),
                    Ced = record["a.Ced"].As<string>()
                });
            } finally {
                await session.CloseAsync();
            }
            return member;
        }

        public async Task<Member> GetMember(string ced) {
            IResultCursor cursor;
            var member = new List<Member>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(String.Format(@"MATCH (a:Miembro) where a.Ced = '{0}'
                                                RETURN a.Name, a.LastName, a.Phone, a.Email, a.Ced", ced));

                member = await cursor.ToListAsync(record => new Member() {
                    Name = record["a.Name"].As<string>(),
                    LastName = record["a.LastName"].As<string>(),
                    Phone = Int32.Parse(record["a.Phone"].As<string>()),
                    Email = record["a.Email"].As<string>(),
                    Ced = record["a.Ced"].As<string>()
                });

            } finally {
                await session.CloseAsync();
            }
            return member[0];
        }

        public async Task<List<Member>> GetMembersOfNode(int id, string node) {
            IResultCursor cursor;
            var member = new List<Member>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(String.Format(@"MATCH (a:Miembro)-[:MemberOf]-(gr:{0}) where gr.Cod = {1} 
                                                RETURN a.Name, a.LastName, a.Phone, a.Email, a.Ced", node, id));
                member = await cursor.ToListAsync(record => new Member() {
                    Name = record["a.Name"].As<string>(),
                    LastName = record["a.LastName"].As<string>(),
                    Phone = Int32.Parse(record["a.Phone"].As<string>()),
                    Email = record["a.Email"].As<string>(),
                    Ced = record["a.Ced"].As<string>()
                });
            } finally {
                await session.CloseAsync();
            }
            return member;
        }

        public async void CreateCoordination(Coordination c) {
            await Client.ConnectAsync();
            await Client.Cypher.Create("(cc:Coordination {newCoord})").WithParam("newCoord", c).ExecuteWithoutResultsAsync();
        }

        public async void CreateChief(Chief chief) {
            await Client.ConnectAsync();
            await Client.Cypher.Create("(cc:Chief {newCoord})").WithParam("newCoord", chief).ExecuteWithoutResultsAsync();
        }

        public async void CreateMember(Member member) {
            await Client.ConnectAsync();
            await Client.Cypher.Create("(cc:Miembro {new})").WithParam("new", member).ExecuteWithoutResultsAsync();
        }

        public async void AddZone(int cod, NuCloudWeb.Models.Zone node) {
            await Client.ConnectAsync();
            await Client.Cypher
                .Match("(c:Coordination)")
                .Where((Coordination c) => c.Cod == cod)
                .Create("(c)-[:Has]->(zo:Zona {new})")
                .WithParam("new", node)
                .ExecuteWithoutResultsAsync();
        }

        public async void AddBranch(int cod, Branch node) {
            await Client.ConnectAsync();

            await Client.Cypher
                        .Match("(c:Zona)")
                        .Where((Branch c) => c.Cod == cod)
                        .Create("(c)-[:Has]->(br:Rama {new})")
                        .WithParam("new", node)
                        .ExecuteWithoutResultsAsync();
        }

        public async void AddGroup(int cod, Group node) {
            await Client.ConnectAsync();

            await Client.Cypher
                .Match("(c:Rama)")
                .Where((Node c) => c.Cod == cod)
                .Create("(c)-[:Has]->(br:Grupo {new})")
                .WithParam("new", node)
                .ExecuteWithoutResultsAsync();
        }

        public async void AddMemberToGroup(int cod, string ced) {
            await Client.ConnectAsync();

            await Client.Cypher
                .Match("(me:Miembro), (gr:Grupo)")
                .Where((Member me) => me.Ced == ced)
                .AndWhere((Group gr) => gr.Cod == cod)
                .Create("(me)-[:MemberOf {leader:0, monitor:0}]->(gr)")
                .ExecuteWithoutResultsAsync();
        }

        public async void AddChiefToCoord(int cod, string ced) {
            await Client.ConnectAsync();

            await Client.Cypher
                .Match("(me:Chief), (gr:Coordination)")
                .Where((Chief me) => me.Ced == ced)
                .AndWhere((Coordination gr) => gr.Cod == cod)
                .Create("(me)-[:HeadOf]->(gr)")
                .ExecuteWithoutResultsAsync();
        }

        public async void AddMemberToBranch(int cod, string ced) {
            await Client.ConnectAsync();

            await Client.Cypher
                .Match("(me:Miembro), (gr:Rama)")
                .Where((Member me) => me.Ced == ced)
                .AndWhere((Branch gr) => gr.Cod == cod)
                .Create("(me)-[:MemberOf {leader:0}]->(gr)")
                .ExecuteWithoutResultsAsync();
        }

        public async void AddMemberToZone(int cod, string ced) {
            await Client.ConnectAsync();

            await Client.Cypher
                .Match("(me:Miembro), (gr:Zona)")
                .Where((Member me) => me.Ced == ced)
                .AndWhere((NuCloudWeb.Models.Zone gr) => gr.Cod == cod)
                .Create("(me)-[:MemberOf {leader:0}]->(gr)")
                .ExecuteWithoutResultsAsync();
        }

        public async void MakeMemberGroupLeader(int cod, string ced) {
            await Client.ConnectAsync();

            await Client.Cypher
                .Match("(me:Miembro), (gr:Grupo)")
                .Where((Member me) => me.Ced == ced)
                .AndWhere((Group gr) => gr.Cod == cod)
                .Merge("(me) -[mo: MemberOf]->(gr)")
                .Set("mo.leader = 1")
                .ExecuteWithoutResultsAsync();
        }

        public async void MakeMemberMonitor(int cod, string ced) {
            await Client.ConnectAsync();

            await Client.Cypher
                .Match("(me:Miembro), (gr:Grupo)")
                .Where((Member me) => me.Ced == ced)
                .AndWhere((Group gr) => gr.Cod == cod)
                .Merge("(me) -[mo: MemberOf]->(gr)")
                .Set("mo.monitor = 1")
                .ExecuteWithoutResultsAsync();
        }

        public async void EditMember(Member member) {
            await Client.ConnectAsync();

            await Client.Cypher
                .Match("(me:Miembro)")
                .Where((Member me) => me.Ced == member.Ced)
                .Merge("(me)-[:MemberOf]->()")
                .Set("me = {mem}")
                .WithParam("mem", member)
                .ExecuteWithoutResultsAsync();
        }

        public async Task<List<Branch>> ZoneBranches(int id) {
            IResultCursor cursor;
            var Branches = new List<Branch>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(String.Format(@"MATCH (zo:Zona)-[:Has]-(a:Rama) where zo.Cod = {0} return a.Name, a.Cod", id));
                Branches = await cursor.ToListAsync(record => new Branch() {
                    Name = record["a.Name"].As<string>(),
                    Cod = Int32.Parse(record["a.Cod"].As<string>())
                });
            } finally {
                await session.CloseAsync();
            }
            return Branches;
        }

        public async Task<List<Group>> BranchGroups(int id) {
            IResultCursor cursor;
            var Nodes = new List<Group>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(String.Format(@"MATCH (br:Rama)-[:Has]-(a:Grupo) where br.Cod = {0} return a.Name, a.Cod", id));
                Nodes = await cursor.ToListAsync(record => new Group() {
                    Name = record["a.Name"].As<string>(),
                    Cod = Int32.Parse(record["a.Cod"].As<string>())
                });
            } finally {
                await session.CloseAsync();
            }
            return Nodes;
        }

        public async Task<List<NuCloudWeb.Models.Zone>> CoordinationZones(int cod) {
            IResultCursor cursor;
            var Zones = new List<NuCloudWeb.Models.Zone>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(String.Format(@"MATCH (z:Coordination)-[:Has]-(a:Zona) where z.Cod = {0} return a.Name, a.Cod", cod));
                Zones = await cursor.ToListAsync(record => new NuCloudWeb.Models.Zone() {
                    Name = record["a.Name"].As<string>(),
                    Cod = Int32.Parse(record["a.Cod"].As<string>())
                });
            } finally {
                await session.CloseAsync();
            }
            return Zones;
        }

        public async Task<List<Coordination>> GetCoordinations() {
            IResultCursor cursor;
            var Coordinations = new List<Coordination>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(@"MATCH (a:Coordination) return a.Name, a.Cod");
                Coordinations = await cursor.ToListAsync(record => new Coordination() {
                    Name = record["a.Name"].As<string>(),
                    Cod = Int32.Parse(record["a.Cod"].As<string>())
                });
            } finally {
                await session.CloseAsync();
            }
            return Coordinations;
        }

        public async Task<NuCloudWeb.Models.Zone> GetZone(int cod) {
            IResultCursor cursor;
            var Zones = new List<NuCloudWeb.Models.Zone>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(String.Format(@"MATCH (a:Zona) WHERE a.Cod = {0} return a.Name, a.Cod", cod));
                Zones = await cursor.ToListAsync(record => new NuCloudWeb.Models.Zone() {
                    Name = record["a.Name"].As<string>(),
                    Cod = Int32.Parse(record["a.Cod"].As<string>())
                });
            } finally {
                await session.CloseAsync();
            }
            return Zones[0];
        }

        public async Task<Branch> GetBranch(int cod) {
            IResultCursor cursor;
            var Branches = new List<Branch>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(String.Format(@"MATCH (a:Rama) WHERE a.Cod = {0} return a.Name, a.Cod", cod));
                Branches = await cursor.ToListAsync(record => new Branch() {
                    Name = record["a.Name"].As<string>(),
                    Cod = Int32.Parse(record["a.Cod"].As<string>())
                });
            } finally {
                await session.CloseAsync();
            }
            return Branches[0];
        }

        public async Task<Group> GetGroup(int cod) {
            IResultCursor cursor;
            var Groups = new List<Group>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(String.Format(@"MATCH (a:Grupo) WHERE a.Cod = {0} return a.Name, a.Cod", cod));
                Groups = await cursor.ToListAsync(record => new Group() {
                    Name = record["a.Name"].As<string>(),
                    Cod = Int32.Parse(record["a.Cod"].As<string>())
                });
            } finally {
                await session.CloseAsync();
            }
            return Groups[0];
        }
    }
}
