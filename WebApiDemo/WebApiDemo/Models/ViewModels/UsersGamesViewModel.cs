using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiabloDatabase;

namespace WebApiDemo.Models.ViewModels
{
    public class UsersGamesViewModel
    {
        public string GameName { get; set; }

        public UsersGamesViewModel(UsersGames game )
        {
            GameName = game.Games.Name;
        }
    }
}