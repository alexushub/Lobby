using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace LobbyMVC5.DAL
{
    class LobbyDbInitializer : DropCreateDatabaseIfModelChanges<LobbyDbContext>
    {
        protected override void Seed(LobbyDbContext context)
        {
            UserManager<ApplicationUser> _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            if (_userManager.FindByEmail("user01@mail.com") == null)
            {
                var email = "user01@mail.com";
                //var userName = "user01";

                var user01 = new ApplicationUser()
                {
                    UserName = email,
                    Email = email
                };

                var us01 = new LobbyUser()
                {
                    UserName = email,
                    Email = email
                };

                _userManager.Create(user01, "password");
                context.LobbyUsers.Add(us01);

                email = "user02@mail.com";
                //var userName = "user01";

                var user02 = new ApplicationUser()
                {
                    UserName = email,
                    Email = email
                };

                var us02 = new LobbyUser()
                {
                    UserName = email,
                    Email = email
                };

                _userManager.Create(user02, "password");
                context.LobbyUsers.Add(us02);

                //var user02 = new LobbyUser()
                //{
                //    UserName = "user02@mail.com",
                //    Email = "user02@mail.com"
                //};

                //res = _userManager.Create(user02, "password");

                //    var user03 = new LobbyUser()
                //    {
                //        UserName = "user03@mail.com",
                //        Email = "user03@mail.com"
                //    };

                //    res = _userManager.Create(user03, "password");

                //    var user04 = new LobbyUser()
                //    {
                //        UserName = "user04@mail.com",
                //        Email = "user04@mail.com"
                //    };

                //    res = _userManager.Create(user04, "password");
                //}

                //var lob01 = new Lobby()
                //{
                //    Name = "lob01",
                //    Author = _userManager.FindByEmail("user01@mail.com"),
                //    RequiredUsersAmount = 4,
                //    CurrentUsersAmount = 1,
                //    CreationDate = DateTime.Now,
                //    State = LobbyState.Waiting,
                //    CurrentUsersIds = ""
            };

            //context.Lobbies.Add(lob01);
            context.SaveChanges();

            base.Seed(context);
        }
    }
}