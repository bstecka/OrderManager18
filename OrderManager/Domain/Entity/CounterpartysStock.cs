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
    public class CounterpartysStock
    {
        private int id;
        private Stock stock;
        private Counterparty counterparty;
        private double priceNetto;

        /// <summary>
        /// Initializes a new instance of the <see cref="CounterpartysStock"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="stock">The stock.</param>
        /// <param name="counterparty">The counterparty.</param>
        /// <param name="priceNetto">The price netto.</param>
        public CounterpartysStock(int id, Stock stock, Counterparty counterparty, double priceNetto)
        {
            this.id = id;
            this.Stock = stock;
            this.counterparty = counterparty;
            this.priceNetto = priceNetto;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get => id; set => id = value; }
        /// <summary>
        /// Gets or sets the price netto.
        /// </summary>
        /// <value>
        /// The price netto.
        /// </value>
        /// <exception cref="ArgumentException"></exception>
        public virtual double PriceNetto { get => priceNetto; set { if (value < 0.01) throw new ArgumentException(); priceNetto = value; } }
        /// <summary>
        /// Gets or sets the stock.
        /// </summary>
        /// <value>
        /// The stock.
        /// </value>
        internal Stock Stock { get => stock; set => stock = value; }
        /// <summary>
        /// Gets or sets the counterparty.
        /// </summary>
        /// <value>
        /// The counterparty.
        /// </value>
        internal Counterparty Counterparty { get => counterparty; set => counterparty = value; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return id + " " + stock.Name + " dostarczane przez " + counterparty.Name;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is CounterpartysStock && ((CounterpartysStock)obj).Id == id;
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
}
