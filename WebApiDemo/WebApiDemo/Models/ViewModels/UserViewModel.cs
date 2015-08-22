using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiabloDatabase;

namespace WebApiDemo.Models.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string lastName { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public ICollection<UsersGamesViewModel> UserGames { get; set; }

        public UserViewModel(Users user)
        {
            UserId = user.Id;
            Username = user.Username;
            FirstName = user.FirstName;
            lastName = user.LastName;
            Email = user.Email;
            UserGames = new List<UsersGamesViewModel>();
            foreach (var game in user.UsersGames)
            {
                UserGames.Add(new UsersGamesViewModel(game));
            }
            
        }
    }
}