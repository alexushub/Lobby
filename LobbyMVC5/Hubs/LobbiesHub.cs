using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Xsl;
using LobbyMVC5.Models;
using LobbyMVC5.DAL;

namespace LobbyMVC5.Hubs
{
    public class LobbiesHub : Hub
    {
        public async Task ShowLobbies(int reqUsersAmount)
        {
            var userUid = Context.ConnectionId;
            using (var uow = new UnitOfWork(LobbyDbContext.Create()))
            {
                var lobbies = uow.Lobbies.GetWaitingLobbies(reqUsersAmount);

                var res = new
                {
                    lobbies = lobbies,
                    reqUsersAmount = reqUsersAmount
                };

                await Clients.All.showLobbies(res);
            }
        }
    }
}
