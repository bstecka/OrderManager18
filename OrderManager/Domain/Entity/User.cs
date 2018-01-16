using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Entity
{
    [Serializable]
    public class User
    {
        private int id;
        private string name;
        private string surname;
        private string login;
        private string password;
        private string phoneNumber;
        private string type;

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="surname">The surname.</param>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="type">The type.</param>
        public User(int id, string name, string surname, string login, string password, string phoneNumber, string type)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.login = login;
            this.password = password;
            this.phoneNumber = phoneNumber;
            this.type = type;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get => id; set => id = value; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get => name; set => name = value; }
        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The surname.
        /// </value>
        public string Surname { get => surname; set => surname = value; }
        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        /// <value>
        /// The login.
        /// </value>
        public string Login { get => login; set => login = value; }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get => password; set => password = value; }
        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get => type; set => type = value; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() { return id + " " + name + " " + surname; }
    }
}