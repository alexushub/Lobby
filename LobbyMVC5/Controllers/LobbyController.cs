using LobbyMVC5.DAL;
using LobbyMVC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LobbyMVC5.Hubs;
using Microsoft.AspNet.SignalR;

namespace LobbyMVC5.Controllers
{
    [System.Web.Mvc.Authorize]
    public class LobbyController : BaseController
    {
        public ActionResult Index()
        {
            using (var uow = new UnitOfWork(LobbyDbContext.Create()))
            {
                var lobbies = uow.Lobbies.GetAll().ToList();
                var result = lobbies.Select(m => new ViewLobbyViewModel(m));

                return View(result);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(CreateLobbyViewModel vm)
        {
            if (ModelState.IsValid)
            {
                using (var uow = new UnitOfWork(LobbyDbContext.Create()))
                {
                    var currUser = uow.Users.Find(m => m.UserName == User.Identity.Name).FirstOrDefault();

                    var newLobby = new Lobby()
                    {
                        CreationDate = DateTime.Now,
                        RequiredUsersAmount = int.Parse(vm.RequiredUsersAmountStr),
                        CurrentUsersAmount = 1,
                        Name = vm.Name,
                        State = LobbyState.Waiting,
                        Author = currUser,
                        CurrentUsersIds = currUser.Id.ToString()
                    };

                    uow.Lobbies.Add(newLobby);

                    uow.Complete();

                    //send to clients

                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<LobbiesHub>();

                    var lobbies = uow.Lobbies.GetWaitingLobbies(2);

                    var res = new
                    {
                        lobbies = lobbies,
                        reqUsersAmount = 2
                    };

                    await hubContext.Clients.All.showLobbies(res);

                    lobbies = uow.Lobbies.GetWaitingLobbies(4);

                    res = new
                    {
                        lobbies = lobbies,
                        reqUsersAmount = 4
                    };

                    await hubContext.Clients.All.showLobbies(res);

                    return RedirectToAction("ViewLobby", "Lobby", new { id = newLobby.Id });
                }



            }

            return View(vm);
            //return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ViewLobby(int id)
        {
            using (var uow = new UnitOfWork(LobbyDbContext.Create()))
            {
                var lobby = uow.Lobbies.Get(id);
                var currUser = uow.Users.Find(m => m.UserName == User.Identity.Name).FirstOrDefault();

                var viewLobby = new ViewLobbyViewModel(lobby);
                viewLobby.CurrentViewerUserId = currUser.Id;

                return View(viewLobby);
            }
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<int> StartLobby(int id)
        {
            using (var uow = new UnitOfWork(LobbyDbContext.Create()))
            {
                uow.Lobbies.StartLobby(id);

                uow.Complete();

                //send to clients

                var hubContext = GlobalHost.ConnectionManager.GetHubContext<LobbiesHub>();

                var lobbies = uow.Lobbies.GetWaitingLobbies(2);

                var res = new
                {
                    lobbies = lobbies,
                    reqUsersAmount = 2
                };

                await hubContext.Clients.All.showLobbies(res);

                lobbies = uow.Lobbies.GetWaitingLobbies(4);

                res = new
                {
                    lobbies = lobbies,
                    reqUsersAmount = 4
                };

                await hubContext.Clients.All.showLobbies(res);
            }

            return 1;
        }

        [HttpGet]
        public bool JoinLobby(string userName, int lobbyId)
        {
            using (var uow = new UnitOfWork(LobbyDbContext.Create()))
            {
                var user = uow.Users.Find(m => m.Email == userName).FirstOrDefault();
                //var lobby = uow.Lobbies.Get(lobbyId);

                uow.Lobbies.JoinUserToLobby(user.Id, lobbyId);

                uow.Complete();

                return true;
            }
        }

        [HttpGet]
        public bool DisjoinLobby(string userName, int lobbyId)
        {
            using (var uow = new UnitOfWork(LobbyDbContext.Create()))
            {
                var user = uow.Users.Find(m => m.Email == userName).FirstOrDefault();
                //var lobby = uow.Lobbies.Get(lobbyId);

                uow.Lobbies.DisjoinUserFromLobby(user.Id, lobbyId);

                uow.Complete();

                return true;
            }
        }

        [HttpGet]
        public bool GetPenaltyStateForUser(string userName)
        {
            using (var uow = new UnitOfWork(LobbyDbContext.Create()))
            {
                var user = uow.Users.Find(m => m.Email == userName).FirstOrDefault();

                return user.IsPenaltyActive;
            }
        }
    }
}