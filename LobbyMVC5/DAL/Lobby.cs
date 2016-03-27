using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobbyMVC5.DAL
{
    public class Lobby
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public LobbyUser Author { get; set; }

        public DateTime CreationDate { get; set; }

        public int CurrentUsersAmount { get; set; }

        public int RequiredUsersAmount { get; set; }

        public LobbyState State { get; set; }

        public string CurrentUsersIds { get; set; }
    }
}
