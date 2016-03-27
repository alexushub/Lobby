using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobbyMVC5.DAL
{
    class UnitOfWork : IUnitOfWork
    {
        private LobbyDbContext _context;

        public UnitOfWork(LobbyDbContext context)
        {
            _context = context;
            Lobbies = new LobbyRepository(_context);
            Users = new LobbyUserRepository(_context);
        }
        

        public ILobbyRepository Lobbies { get; private set; }

        public ILobbyUserRepository Users { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
