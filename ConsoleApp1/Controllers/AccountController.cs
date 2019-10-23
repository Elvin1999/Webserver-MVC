using ConsoleApp1.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Controllers
{
   public class AccountController
    {
        [Auth]
        public string About()
        {
            return "Account/About";
        }
        [Auth]
        public string LogOut()
        {
            return "<a href='/account/authpage'>auth page </a>";
        }
        public string AuthPage()
        {
            return "<form method='post' action='account/login'>" +
                    "<input type='text' name='login' required></br>" +
                    "input type='submit'</br>" +
                    "</form>";
        }
        [HttpAttribute("POST")]
        public string Login(string login)
        {
            return "<a href=''>home page</a></br>" +
                "<a href=''>about</a></br>" +
                "<a href=''>logout</a></br>";
        }
    }
}
