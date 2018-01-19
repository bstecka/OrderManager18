using OrderManager.DAL.InternalSysDAO;
using OrderManager.Domain.Service;
using OrderManager.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Entity
{
    [Serializable]
    public class Counterparty
    {
        private int id;
        private int distance;
        private Int64 nip;
        private string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="Counterparty"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="nip">The nip.</param>
        /// <param name="name">The name.</param>
        /// <param name="distance">The distance.</param>
        public Counterparty(int id, long nip, string name, int distance)
        {
            this.id = id;
            this.distance = distance;
            this.nip = nip;
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
        /// Gets or sets the distance.
        /// </summary>
        /// <value>
        /// The distance.
        /// </value>
        public int Distance { get => distance; set => distance = value; }
        /// <summary>
        /// Gets or sets the nip.
        /// </summary>
        /// <value>
        /// The nip.
        /// </value>
        public long Nip { get => nip; set => nip = value; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get => name; set => name = value; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() { return id + " " + name; }
        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is Counterparty && ((Counterparty)obj).Id == id;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
    enum Priority { Price, Frequency, Distance };

}
