﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobbyMVC5.DAL
{
    interface IUnitOfWork : IDisposable
    {
        ILobbyRepository Lobbies { get; }

        ILobbyUserRepository Users { get; }

        int Complete();
    }
}