using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Entity
{
    public class LoggedUser
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="LoggedUser"/> class from being created.
        /// </summary>
        private LoggedUser() { }

        private static User user;

        /// <summary>
        /// Gets or sets the currently logged user.
        /// </summary>
        /// <value>
        /// The currently logged user.
        /// </value>
        public static User User {
            get
            {
                return new User(1, "Jan", "Kowalski", "kowalski", "haslo", "663097070", "administrator");
            }
            set => user = value; }
    }
}
