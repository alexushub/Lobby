using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data.Entity;

namespace LobbyMVC5.DAL
{
    public class LobbyRepository : Repository<Lobby>, ILobbyRepository
    {
        public LobbyDbContext LobbyContext
        {
            get
            {
                return Context as LobbyDbContext;
            }
        }

        public Lobby Get(int id)
        {
            return LobbyContext.Lobbies.Include("Author").FirstOrDefault(m => m.Id == id);
        }

        public IEnumerable<Lobby> GetAll()
        {
            return LobbyContext.Lobbies.Include("Author").ToList();
        }

        public IEnumerable<Lobby> GetWaitingLobbies(int reqUsersAmount)
        {
            return LobbyContext.Lobbies.Include("Author").Where(m => m.State == LobbyState.Waiting && m.RequiredUsersAmount == reqUsersAmount).ToList();
        }

        public LobbyRepository(LobbyDbContext context) : base(context)
        {
        }

        public void StartLobby(int lobbyId)
        {
            var lobby = LobbyContext.Lobbies.FirstOrDefault(m => m.Id == lobbyId);
            
            lobby.State = LobbyState.Active;
        }

        public void AbortLobby(int lobbyId)
        {
            var lobby = LobbyContext.Lobbies.FirstOrDefault(m => m.Id == lobbyId);

            lobby.State = LobbyState.Aborted;
        }

        public void FinishLobby(int lobbyId)
        {
            var lobby = LobbyContext.Lobbies.FirstOrDefault(m => m.Id == lobbyId);

            lobby.State = LobbyState.Finished;
        }

        public void JoinUserToLobby(int userId, int lobbyId)
        {
            var userIdStr = userId.ToString();

            var lobby = LobbyContext.Lobbies.FirstOrDefault(m => m.Id == lobbyId);

            var ids = lobby.CurrentUsersIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().Select(m => m.ToLower()).ToList();

            if (!ids.Contains(userIdStr))
            {
                ids.Add(userIdStr);
            }

            var newIds = String.Join(",", ids);

            lobby.CurrentUsersAmount = ids.Count;

            lobby.CurrentUsersIds = newIds;
        }

        public void DisjoinUserFromLobby(int userId, int lobbyId)
        {
            var userIdStr = userId.ToString();

            var lobby = LobbyContext.Lobbies.FirstOrDefault(m => m.Id == lobbyId);

            var ids = lobby.CurrentUsersIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().Select(m => m.ToLower()).ToList();

            if (ids.Contains(userIdStr))
            {
                ids.Remove(userIdStr);
            }

            var newIds = String.Join(",", ids);

            lobby.CurrentUsersAmount = ids.Count;

            lobby.CurrentUsersIds = newIds;
        }

        public IEnumerable<string> GetCurrentUserNamesInLobby(int lobbyId)
        {
            var lobby = LobbyContext.Lobbies.FirstOrDefault(m => m.Id == lobbyId);

            var ids = lobby.CurrentUsersIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList().Select(m => m.ToLower()).ToList();

            var userNames = ids.Select(m => int.Parse(m)).Select(s => LobbyContext.LobbyUsers.FirstOrDefault(m => m.Id == s).UserName).ToList();

            return userNames;
        }

    }
}
