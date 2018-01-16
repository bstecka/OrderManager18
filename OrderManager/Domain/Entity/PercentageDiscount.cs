using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Entity
{
    [Serializable]
    public class PercentageDiscount
    {
        int id;
        DateTime since;
        DateTime until;
        double sumNetto;
        double amount;
        bool summing;
        bool active;
        List<CounterpartysStock> counterpartysStock;

        /// <summary>
        /// Initializes a new instance of the <see cref="PercentageDiscount"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="since">The since.</param>
        /// <param name="until">The until.</param>
        /// <param name="sumNetto">The sum netto.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="summing">if set to <c>true</c> [summing].</param>
        /// <param name="active">if set to <c>true</c> [active].</param>
        /// <param name="counterpartysStock">The counterpartys stock.</param>
        /// <exception cref="ArgumentException"></exception>
        public PercentageDiscount(int id, DateTime since, DateTime until, double sumNetto, double amount, 
            bool summing, bool active, List<CounterpartysStock> counterpartysStock)
        {
            if (since > until || sumNetto < 0 || amount > 1)
                throw new ArgumentException();
            this.id = id;
            this.since = since;
            this.until = until;
            this.sumNetto = sumNetto;
            this.amount = amount;
            this.summing = summing;
            this.active = active;
            this.counterpartysStock = counterpartysStock;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PercentageDiscount"/> class.
        /// </summary>
        /// <param name="percentageDiscount">The percentage discount.</param>
        /// <param name="newCounterpartysStock">The new counterpartys stock.</param>
        public PercentageDiscount(PercentageDiscount percentageDiscount, List<CounterpartysStock> newCounterpartysStock)
        {
            id = percentageDiscount.Id;
            since = percentageDiscount.Since;
            until = percentageDiscount.Until;
            sumNetto = percentageDiscount.SumNetto;
            amount = percentageDiscount.Amount;
            summing = percentageDiscount.Summing;
            active = percentageDiscount.Active;
            counterpartysStock = newCounterpartysStock;
        }

        /// <summary>
        /// Gets or sets the since.
        /// </summary>
        /// <value>
        /// The since.
        /// </value>
        public DateTime Since { get => since; set => since = value; }
        /// <summary>
        /// Gets or sets the until.
        /// </summary>
        /// <value>
        /// The until.
        /// </value>
        public DateTime Until { get => until; set => until = value; }
        /// <summary>
        /// Gets or sets the sum netto.
        /// </summary>
        /// <value>
        /// The sum netto.
        /// </value>
        public double SumNetto { get => sumNetto; set => sumNetto = value; }
        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public virtual double Amount { get => amount; set => amount = value; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PercentageDiscount"/> is summing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if summing; otherwise, <c>false</c>.
        /// </value>
        public bool Summing { get => summing; set => summing = value; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PercentageDiscount"/> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        public bool Active { get => active; set => active = value; }
        /// <summary>
        /// Gets or sets the counterpartys stock.
        /// </summary>
        /// <value>
        /// The counterpartys stock.
        /// </value>
        public List<CounterpartysStock> CounterpartysStock { get => counterpartysStock; set => counterpartysStock = value; }
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get => id; set => id = value; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Rabat procentowy ID:" + id + " " + sumNetto;
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
            return obj is PercentageDiscount && ((PercentageDiscount)obj).Id == id;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return id;
        }
    }
}
