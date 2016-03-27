using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LobbyMVC5.DAL
{
    public interface ILobbyUserRepository : IRepository<LobbyUser>
    {
        //LobbyUser GetUserByEmail(string email);
        void UpdateUserPaneltyState(int userId, bool isActive);
    }
}
