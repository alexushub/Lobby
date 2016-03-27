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
    public class UsersOnlineHub : Hub
    {
        private static List<UserOnlineDTO> usersOnline = new List<UserOnlineDTO>();


        public void UserOnline(string userName)
        {
            if (!String.IsNullOrEmpty(userName))
            {
                using (var uow = new UnitOfWork(LobbyDbContext.Create()))
                {
                    var lobbyUser = uow.Users.Find(m => m.UserName == userName).FirstOrDefault();
                    var userUid = Context.ConnectionId;

                    if (!usersOnline.Select(m => m.UserName).Contains(userName))
                    {
                        usersOnline.Add(new UserOnlineDTO()
                        {
                            UserName = lobbyUser.UserName,
                            UserUid = userUid
                        });
                    }
                }
            }
            
            Clients.All.showUsersOnline(usersOnline);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var userUid = Context.ConnectionId;

            var userOnline = usersOnline.FirstOrDefault(m => m.UserUid == userUid);

            usersOnline.Remove(userOnline);

            Clients.All.showUsersOnline(usersOnline);

            return base.OnDisconnected(stopCalled);
        }
    }
}
