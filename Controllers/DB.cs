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

        public void SetDriver(string uri, string user, string password) {
            Driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
            Client = new BoltGraphClient(uri, user, password);
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
                                                RETURN a.Name, a.LastName, A.phone, a.Email, a.Ced");
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

        public async Task<List<Member>> GetMembersOfGroup(int id) {
            IResultCursor cursor;
            var member = new List<Member>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(String.Format(@"MATCH (a:Member)-[:MemberOf]-(gr:Group) where gr.cod = {0} 
                                                RETURN a.Name, a.LastName, A.phone, a.Email, a.Ced", id));
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

        public async void AddZone(int cod, Node node) {
            await Client.ConnectAsync();
            await Client.Cypher
                .Match("(c:Coordination)")
                .Where((Coordination c) => c.Cod == cod)
                .Create("(c)-[:Has]->(zo:Zona {new})")
                .WithParam("new", node)
                .ExecuteWithoutResultsAsync();
        }

        public async void AddBranch(int cod, Node node) {
            await Client.ConnectAsync();

            await Client.Cypher
                        .Match("(c:Zona)")
                        .Where((Node c) => c.Cod == cod)
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

        public async void AddMemberToNode(int cod, string ced, string node) {
            await Client.ConnectAsync();

            await Client.Cypher
                .Match(String.Format("(me:Miembro), (gr:{0})", node))
                .Where((Member me) => me.Ced == ced)
                .AndWhere((Node gr) => gr.Cod == cod)
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

        public async Task<List<Node>> ZoneBranches(int id) {
            IResultCursor cursor;
            var Nodes = new List<Node>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(String.Format(@"MATCH (zo:Zona)-[:Has]-(br:Rama) where zo.Cod = {0} return br", id));
                Nodes = await cursor.ToListAsync(record => new Node() {
                    Name = record["a.Name"].As<string>(),
                    Cod = Int32.Parse(record["a.Cod"].As<string>())
                });
            } finally {
                await session.CloseAsync();
            }
            return Nodes;
        }

        public async Task<List<Group>> BranchGroups(int id) {
            IResultCursor cursor;
            var Nodes = new List<Group>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(String.Format(@"MATCH (br:Rama)-[:Has]-(gr:Grupo) where br.Cod = {0} return gr", id));
                Nodes = await cursor.ToListAsync(record => new Group() {
                    Name = record["a.Name"].As<string>(),
                    Cod = Int32.Parse(record["a.Cod"].As<string>())
                });
            } finally {
                await session.CloseAsync();
            }
            return Nodes;
        }

        public async Task<List<Node>> ChiefZones(string ced) {
            IResultCursor cursor;
            var Nodes = new List<Node>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(String.Format(@"MATCH (z:Chief)-[:Has]-(br:Zona) where z.Ced = {0} return br", ced));
                Nodes = await cursor.ToListAsync(record => new Node() {
                    Name = record["a.Name"].As<string>(),
                    Cod = Int32.Parse(record["a.Cod"].As<string>())
                });
            } finally {
                await session.CloseAsync();
            }
            return Nodes;
        }
    }
}
