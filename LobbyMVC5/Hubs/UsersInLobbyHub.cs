using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Xsl;
using LobbyMVC5.Models;
using LobbyMVC5.DAL;

namespace LobbyMVC5.Hubs
{

    public class UsersInLobbyHub : Hub
    {
        public async Task ShowMeUsersInLobby(int lobbyId)
        {
            var userUid = Context.ConnectionId;
            using (var uow = new UnitOfWork(LobbyDbContext.Create()))
            {
                var users = uow.Lobbies.GetCurrentUserNamesInLobby(lobbyId).Select(m => new UserOnlineDTO() { UserName = m }).ToList();

                await Groups.Add(userUid, lobbyId.ToString());

                var result = new ResultDTO()
                {
                    Users = users,
                    Lobby = null,
                    IsNeedToUpdateButtons = false,
                    Message = null
                };

                await Clients.Caller.showUsersInLobby(result);
            }
        }

        public async Task UserInLobby(string userName, int lobbyId)
        {
            using (var uow = new UnitOfWork(LobbyDbContext.Create()))
            {
                var lobbyUser = uow.Users.Find(m => m.UserName == userName).FirstOrDefault();
                var userUid = Context.ConnectionId;
                var lobby = uow.Lobbies.Get(lobbyId);

                var users = uow.Lobbies.GetCurrentUserNamesInLobby(lobbyId).Select(m => new UserOnlineDTO() { UserName = m }).ToList();

                await Groups.Add(userUid, lobbyId.ToString());

                var result = new ResultDTO()
                {
                    Users = users,
                    Lobby = lobby,
                    IsNeedToUpdateButtons = true,
                    Message = null
                };

                await Clients.Group(lobbyId.ToString()).showUsersInLobby(result);
                //await Clients.Group(lobbyId.ToString()).showLobbyState(users);
            }
        }

        public async Task UserLeaveLobby(string userName, int lobbyId)
        {
            using (var uow = new UnitOfWork(LobbyDbContext.Create()))
            {
                var userUid = Context.ConnectionId;
                var lobby = uow.Lobbies.Get(lobbyId);

                var users = uow.Lobbies.GetCurrentUserNamesInLobby(lobbyId).Select(m => new UserOnlineDTO() { UserName = m }).ToList();

                var result = new ResultDTO()
                {
                    Users = users,
                    Lobby = lobby,
                    IsNeedToUpdateButtons = true,
                    Message = null
                };

                await Clients.Group(lobbyId.ToString()).showUsersInLobby(result);

                if (lobby.State == LobbyState.Active)
                {
                    var user = uow.Users.Find(m => m.UserName == userName).FirstOrDefault();
                    uow.Users.UpdateUserPaneltyState(user.Id, true);

                    //user.IsPenaltyActive = true;
                    //uow.Users.u

                    uow.Complete();

                    await Clients.Client(userUid).setUserPenaltyState(true);

                    await Task.Run(() =>
                    {
                        Thread.Sleep(60000);

                        uow.Users.UpdateUserPaneltyState(user.Id, false);
                        uow.Complete();
                        //user.IsPenaltyActive = false;

                        Clients.Client(userUid).setUserPenaltyState(false);

                    });

                }
            }
        }

        public async Task UserStartLobby(int lobbyId)
        {
            using (var uow = new UnitOfWork(LobbyDbContext.Create()))
            {
                var lobby = uow.Lobbies.Get(lobbyId);
                var users =
                    uow.Lobbies.GetCurrentUserNamesInLobby(lobbyId)
                        .Select(m => new UserOnlineDTO() {UserName = m})
                        .ToList();

                var result = new ResultDTO()
                {
                    Users = users,
                    Lobby = lobby,
                    IsNeedToUpdateButtons = true,
                    Message = "Lobby started!"
                };

                await Clients.Group(lobbyId.ToString()).showUsersInLobby(result);

            }

            await Task.Run(() =>
                {
                    using (var uow = new UnitOfWork(LobbyDbContext.Create()))
                    {
                        Thread.Sleep(60000);

                        uow.Lobbies.FinishLobby(lobbyId);
                        uow.Complete();
                    }

                });
            using (var uow = new UnitOfWork(LobbyDbContext.Create()))
            {
                var lobby = uow.Lobbies.Get(lobbyId);
                var users =
                    uow.Lobbies.GetCurrentUserNamesInLobby(lobbyId)
                        .Select(m => new UserOnlineDTO() {UserName = m})
                        .ToList();

                var result = new ResultDTO()
                {
                    Users = users,
                    Lobby = lobby,
                    IsNeedToUpdateButtons = true,
                    Message = "Lobby finished!"
                };

                await Clients.Group(lobbyId.ToString()).showUsersInLobby(result);
            }
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override async Task OnDisconnected(bool stopCalled)
        {
            using (var uow = new UnitOfWork(LobbyDbContext.Create()))
            {
                var userUid = Context.ConnectionId;

                var keyList = uow.Lobbies.GetAll().Select(m => m.Id).ToList();

                foreach (var key in keyList)
                {
                    await Groups.Remove(userUid, key.ToString());
                }

                await base.OnDisconnected(stopCalled);
            }
        }
    }
}
