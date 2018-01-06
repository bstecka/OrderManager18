using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Entity
{
    public class User
    {
        private int id;
        private string name;
        private string surname;
        private string login;
        private string password;
        private string phoneNumber;
        private string type;

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

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }
        public string Login { get => login; set => login = value; }
        public string Password { get => password; set => password = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public string Type { get => type; set => type = value; }

        public override string ToString() { return id + " " + name + " " + surname; }
    }
}