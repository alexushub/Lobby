using System;
using LobbyMVC5.DAL;

namespace LobbyMVC5.Hubs
{
    public class LobbyDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AuthorName { get; set; }

        public DateTime CreationDate { get; set; }

        public int CurrentUsersAmount { get; set; }

        public int RequiredUsersAmount { get; set; }

        public LobbyState State { get; set; }

        public string CurrentUsersIds { get; set; }

    }
}