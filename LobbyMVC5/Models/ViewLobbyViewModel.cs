using LobbyMVC5.DAL;
using LobbyMVC5.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LobbyMVC5.Models
{
    public class ViewLobbyViewModel
    {
        public ViewLobbyViewModel()
        {

        }

        public ViewLobbyViewModel(Lobby lobby)
        {
            Id = lobby.Id;
            Name = lobby.Name;
            CurrentUsersAmount = lobby.CurrentUsersAmount;
            RequiredUsersAmount = lobby.RequiredUsersAmount;
            CreationDate = lobby.CreationDate;
            Author = lobby.Author;
            State = lobby.State;
            CurrentUsersIds = lobby.CurrentUsersIds;
            
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public int CurrentUsersAmount { get; set; }

        public int RequiredUsersAmount { get; set; }

        public DateTime CreationDate { get; set; }

        public LobbyUser Author { get; set; }

        public LobbyState State { get; set; }

        public string CurrentUsersIds { get; set; }

        public int CurrentViewerUserId { get; set; }
    }
}
