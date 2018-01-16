using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Entity
{
    [Serializable]
    public class Category
    {
        int id;
        string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        public Category(int id, string name)
        {
            this.id = id;
            this.name = name;
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
    }
}
