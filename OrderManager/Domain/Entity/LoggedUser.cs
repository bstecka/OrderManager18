using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Entity
{
    public class LoggedUser
    {
        private LoggedUser() { }

        private static User user;

        public static User User {
            get
            {
                return new User(1, "Jan", "Kowalski", "kowalski", "haslo", "663097070", "administrator");
            }
            set => user = value; }
    }
}
