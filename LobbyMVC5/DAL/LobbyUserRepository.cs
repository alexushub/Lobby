using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LobbyMVC5.DAL
{
    public class LobbyUserRepository : Repository<LobbyUser>, ILobbyUserRepository
    {
        public LobbyDbContext LobbyContext
        {
            get
            {
                return Context as LobbyDbContext;
            }
        }
        public LobbyUserRepository(LobbyDbContext context) : base(context)
        {
        }

        public void UpdateUserPaneltyState(int userId, bool isActive)
        {
            var user = LobbyContext.LobbyUsers.FirstOrDefault(m => m.Id == userId);

            user.IsPenaltyActive = isActive;
        }
    }
}
