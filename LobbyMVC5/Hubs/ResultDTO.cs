using LobbyMVC5.DAL;
using System;
using System.Collections.Generic;

namespace LobbyMVC5.Hubs
{
    public class ResultDTO
    {
        public List<UserOnlineDTO> Users { get; set; }

        public Lobby Lobby { get; set; }

        public bool IsNeedToUpdateButtons { get; set; }

        public string Message { get; set; }

    }
}