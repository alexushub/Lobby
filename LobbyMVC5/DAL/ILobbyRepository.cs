using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LobbyMVC5.DAL
{
    public interface ILobbyRepository : IRepository<Lobby>
    {
        void StartLobby(int lobbyId);

        void AbortLobby(int lobbyId);

        void FinishLobby(int lobbyId);

        void JoinUserToLobby(int userId, int lobbyId);

        void DisjoinUserFromLobby(int userId, int lobbyId);

        IEnumerable<string> GetCurrentUserNamesInLobby(int lobbyId);

        IEnumerable<Lobby> GetWaitingLobbies(int reqUsersAmount);
    }
}
