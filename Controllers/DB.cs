using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
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

        public class Res{
            public long Num { get; set; }
        }

        public void SetDriver(string uri, string user, string password) {
            Driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
            Client = new BoltGraphClient(uri, user, password);
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

        public async Task<List<Node>> GetLeads(string ced) {
            IResultCursor cursor;
            var cloud = new List<Node>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(String.Format(@"MATCH (m:Miembro)-[:LeaderOf]->(a) WHERE m.Ced = '{0}'
                                                RETURN a.Name, a.Cod", ced));
                cloud = await cursor.ToListAsync(record => new Node() {
                    Name = record["a.Name"].As<string>(),
                    Cod = Int32.Parse(record["a.Cod"].As<string>())
                });
            } finally {
                await session.CloseAsync();
            }
            return cloud;
        }

        public async Task<List<Node>> MemberOf(string ced) {
            IResultCursor cursor;
            var cloud = new List<Node>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(String.Format(@"MATCH (m:Miembro)-[:MemberOf]->(a) WHERE m.Ced = '{0}'
                                                RETURN a.Name, a.Cod", ced));
                cloud = await cursor.ToListAsync(record => new Node() {
                    Name = record["a.Name"].As<string>(),
                    Cod = Int32.Parse(record["a.Cod"].As<string>())
                });
            } finally {
                await session.CloseAsync();
            }
            return cloud;
        }

        public async Task<List<Group>> GetCoaches(string ced) {
            IResultCursor cursor;
            var cloud = new List<Group>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(String.Format(@"MATCH (m:Miembro)-[:MonitorOf]->(a:Grupo) WHERE m.Ced = '{0}'
                                                RETURN a.Name, a.Cod", ced));
                cloud = await cursor.ToListAsync(record => new Group() {
                    Name = record["a.Name"].As<string>(),
                    Cod = Int32.Parse(record["a.Cod"].As<string>())
                });
            } finally {
                await session.CloseAsync();
            }
            return cloud;
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
        

        public async Task<Member> GetCoach(string node, int cod) {
            IResultCursor cursor;
            var member = new List<Member>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(String.Format(@"MATCH (a:Miembro)-[:MonitorOf]->(b:{0}) Where b.Cod = {1}
                                                RETURN a.Name, a.LastName, a.Phone, a.Email, a.Ced", node, cod));
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
            return member.Count > 0 ? member[0] : new Member() { Name = "None"};
        }

        public async Task<List<Member>> GetLeaders(string node, int cod) {
            IResultCursor cursor;
            var member = new List<Member>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(String.Format(@"MATCH (a:Miembro)-[:LeaderOf]->(b:{0}) Where b.Cod = {1}
                                                RETURN a.Name, a.LastName, a.Phone, a.Email, a.Ced", node, cod));
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

        public async void CreateChief(Chief chief, int code) {
            await Client.ConnectAsync();
            await Client.Cypher.Match("(ch:Chief)-[:ChiefOf]->(c:Cloud)").Where("c.Cod = {cod}").DetachDelete("ch").WithParam("cod", code).ExecuteWithoutResultsAsync();
            await Client.Cypher.Match("(c:Cloud)").Where("c.Cod = {cod}").Create("(cc:Chief {newCoord})-[:ChiefOf]->(c)").WithParams(new { cod = code, newCoord = chief }).ExecuteWithoutResultsAsync();
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
                .Create("(me)-[:MemberOf]->(gr)")
                .ExecuteWithoutResultsAsync();
        }

        public async void AddMemberToCoord(int cod, string ced) {
            await Client.ConnectAsync();

            await Client.Cypher
                .Match("(me:Miembro), (gr:Coordination)")
                .Where((Member me) => me.Ced == ced)
                .AndWhere((Coordination gr) => gr.Cod == cod)
                .Create("(me)-[:MemberOf]->(gr)")
                .ExecuteWithoutResultsAsync();
        }

        public async void AddMemberToBranch(int cod, string ced) {
            await Client.ConnectAsync();

            await Client.Cypher
                .Match("(me:Miembro), (gr:Rama)")
                .Where((Member me) => me.Ced == ced)
                .AndWhere((Branch gr) => gr.Cod == cod)
                .Create("(me)-[:MemberOf]->(gr)")
                .ExecuteWithoutResultsAsync();
        }

        public async void AddMemberToZone(int cod, string ced) {
            await Client.ConnectAsync();

            await Client.Cypher
                .Match("(me:Miembro), (gr:Zona)")
                .Where((Member me) => me.Ced == ced)
                .AndWhere((NuCloudWeb.Models.Zone gr) => gr.Cod == cod)
                .Create("(me)-[:MemberOf]->(gr)")
                .ExecuteWithoutResultsAsync();
        }


        public async void RemoveMember(string node, int cod, string ced) {
            await Client.ConnectAsync();

            await Client.Cypher
                .Match(String.Format("(m:Miembro)-[r:MemberOf]->(n:{0})", node))
                .Where("m.Ced = {ced}")
                .AndWhere("n.Cod = {cod}")
                .Delete("r")
                .WithParams(new { cod = cod, ced = ced })
                .ExecuteWithoutResultsAsync();
        }

        public async void MakeMemberNodeLeader(int cod, string ced, string n) {
            await Client.ConnectAsync();

            await Client.Cypher
                .Match(String.Format("(me:Miembro), (gr:{0})", n))
                .Where((Member me) => me.Ced == ced)
                .AndWhere("gr.Cod = {cod}")
                .Merge("(me) -[:LeaderOf]->(gr)")
                .WithParam("cod", cod)
                .ExecuteWithoutResultsAsync();

            await Client.Cypher.Match(String.Format("(m:Miembro)-[r:MemberOf]->(:{0})", n)).Where("m.Ced = {ced}").WithParam("ced", ced).Delete("r").ExecuteWithoutResultsAsync();

            if (n == "Grupo")
                await Client.Cypher.Match("(:Miembro)-[r:MonitorOf]->(:Grupo)").Delete("r").ExecuteWithoutResultsAsync();
        }

        public async void MakeMemberMonitor(int cod, string ced) {
            await Client.ConnectAsync();

            await Client.Cypher
                .Match("(me:Miembro), (gr:Grupo)")
                .Where((Member me) => me.Ced == ced)
                .AndWhere((Group gr) => gr.Cod == cod)
                .Merge("(me) -[:MonitorOf]->(gr)")
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

        public async Task<long> CantMemberForCed(String ced){
            
            await Client.ConnectAsync();

            var resultsAsync = Client.Cypher
                            .Match("(me:Miembro)")
                            .Where((Member me) => me.Ced == ced)
                            .Return((me) => new Res
                            {
                                Num = me.Count()
                            })
                            .ResultsAsync;
            return resultsAsync.Result.Single().Num;
        }

        public async Task<long> CantMemberForCedAndPass(String ced, String pass)
        {

            await Client.ConnectAsync();

            var resultsAsync = Client.Cypher
                            .Match("(me:Miembro)")
                            .Where((Member me) => me.Ced == ced)
                            .AndWhere((Member me) => me.Password == pass)
                            .Return((me) => new Res
                            {
                                Num = me.Count()
                            })
                            .ResultsAsync;
            return resultsAsync.Result.Single().Num;
        }

        public async Task<long> CantGroupForCod(int cod)
        {
            await Client.ConnectAsync();

            var resultsAsync = Client.Cypher
                            .Match("(gr:Grupo)")
                            .Where((Group gr) => gr.Cod == cod)
                            .Return((gr) => new Res
                            {
                                Num = gr.Count()
                            })
                            .ResultsAsync;
            return resultsAsync.Result.Single().Num;
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

        public async Task<Chief> GetChiefOfCloud(int cod) {
            IResultCursor cursor;
            var Chiefs = new List<Chief>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(String.Format(@"MATCH (a:Chief)-[:ChiefOf]->(c:Cloud) WHERE c.Cod = {0} return a.Name, a.LastName, a.Ced, a.Email, a.Phone, a.Start, a.End", cod));
                Chiefs = await cursor.ToListAsync(record => new Chief() {
                    Name = record["a.Name"].As<string>(),
                    LastName = record["a.LastName"].As<string>(),
                    Phone = Int32.Parse(record["a.Phone"].As<string>()),
                    Email = record["a.Email"].As<string>(),
                    Ced = record["a.Ced"].As<string>(),
                    Start = record["a.Start"].As<string>(),
                    End = record["a.End"].As<string>()
                });
            } finally {
                await session.CloseAsync();
            }
            return Chiefs.Count > 0 ? Chiefs[0] : new Chief() {  Name = "Node", LastName = ""};
        }

        public async Task<Chief> GetChief(string ced) {
            IResultCursor cursor;
            var Chiefs = new List<Chief>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(String.Format(@"MATCH (a:Chief)-[:ChiefOf]->(:Cloud) WHERE a.Ced = '{0}' return a.Name, a.LastName, a.Ced, a.Email, a.Phone, a.Start, a.End", ced));
                Chiefs = await cursor.ToListAsync(record => new Chief() {
                    Name = record["a.Name"].As<string>(),
                    LastName = record["a.LastName"].As<string>(),
                    Phone = Int32.Parse(record["a.Phone"].As<string>()),
                    Email = record["a.Email"].As<string>(),
                    Ced = record["a.Ced"].As<string>(),
                    Start = record["a.Start"].As<string>(),
                    End = record["a.End"].As<string>()
                });
            } finally {
                await session.CloseAsync();
            }
            return Chiefs[0];
        }

        public async Task<int> GetParentCode(string parent, string child, int cod) {
            IResultCursor cursor;
            var code = new List<int>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try {
                cursor = await session.RunAsync(String.Format(@"match (a:{0})-[:Has]->(b:{1}) where b.Cod = {2} return a.Cod", parent, child, cod));
                code = await cursor.ToListAsync(record => Int32.Parse(record["a.Cod"].As<string>()));
            } finally {
                await session.CloseAsync();
            }
            return code[0];
        }
        
        
        public async Task<List<Member>> GetObserver(int id, string node)
        {
            IResultCursor cursor;
            var member = new List<Member>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            try
            {
                //List<string> emailMonitor = await session.RunAsync(@"MATCH(me:Miembro)-[:MonitorOf]->(gr:Grupo) where me.Ced = {0} RETURN me.Email", id));
                cursor = await session.RunAsync(String.Format(@"MATCH (a:Miembro)-[:MemberOf]->(gr:{0}) where gr.Cod = {1} RETURN a.Name, a.LastName, a.Phone, a.Email, a.Ced", node, id));
                member = await cursor.ToListAsync(record => new Member()
                {
                    Name = record["a.Name"].As<string>(),
                    LastName = record["a.LastName"].As<string>(),
                    Phone = Int32.Parse(record["a.Phone"].As<string>()),
                    Email = record["a.Email"].As<string>(),
                    Ced = record["a.Ced"].As<string>()
                });
            }
            finally
            {
                await session.CloseAsync();
            }
            return member;
        }

        public async void SendObserver(int id, string node, string msj)
        {
            IResultCursor cursor;
            IResultCursor cursor2;
            var member = new List<Member>();
            var member2 = new List<Member>();
            IAsyncSession session = DB.Instance.Driver.AsyncSession();
            await Client.ConnectAsync();
            
            cursor2 = await session.RunAsync(String.Format(@"MATCH(me:Miembro)-[:MonitorOf]->(gr:Grupo) WHERE me.Ced = '2'  RETURN me.Email"));
            member2 = await cursor2.ToListAsync(record => new Member()
            {
                Email = record["me.Email"].As<string>()
            });

            cursor = await session.RunAsync(String.Format(@"MATCH (a:Miembro)-[:MemberOf]->(gr:{0}) WHERE gr.Cod = {1} RETURN a.Email", node, id));
            member = await cursor.ToListAsync(record => new Member()
            {
                Email = record["a.Email"].As<string>()
            });
            Debug.WriteLine("Mensaje:"+msj);
            if (0 < member2.Count) { Debug.WriteLine("Monitor: " + member2[0].Email); }
            if (0 < member.Count) {

                foreach (var student in member) {
                    Debug.WriteLine("Student's:" + student.Email);
                }
            }


            string to = "guzbol@jetmail.uno";
            string from = "kaztof@jetmail.uno";
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Nucloud";
            message.Body = @""+msj;
            SmtpClient client = new SmtpClient("smtp.jetmail.uno", 587);
            // Credentials are necessary if the server requires the client
            // to authenticate before it will send email on the client's behalf.
            client.UseDefaultCredentials = false;

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}", ex.ToString());
            }

        }
    }
}
