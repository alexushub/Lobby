using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LobbyMVC5.DAL
{

    public class LobbyDbContext : IdentityDbContext<ApplicationUser>
    {
        public LobbyDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<LobbyDbContext>(new LobbyDbInitializer());
            //Database.Initialize(true);
        }

        public static LobbyDbContext Create()
        {
            return new LobbyDbContext();
        }

        public DbSet<LobbyUser> LobbyUsers { get; set; }

        public DbSet<Lobby> Lobbies { get; set; }
    }
}
