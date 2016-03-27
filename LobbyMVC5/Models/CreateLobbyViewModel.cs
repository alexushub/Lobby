using LobbyMVC5.DAL;
using LobbyMVC5.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LobbyMVC5.Models
{
    public class CreateLobbyViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "NameEmpty")]
        [StringLength(255, MinimumLength = 3, ErrorMessageResourceType = typeof (Resources.Resources), ErrorMessageResourceName = "LobbyNameShouldBe")]
        public string Name { get; set; }

        [Required]
        public string RequiredUsersAmountStr { get; set; }

    }
}
