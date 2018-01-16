using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Entity
{
    /// <summary>
    /// Enumeration representing the state of the order
    /// </summary>
    public enum ORDERSTATE
    {
        duringRealization = 1,
        duringReview = 2,
        cancelled = 3,
        realized = 4
    }

    [Serializable]
    public class Order
    {
        private int? id;
        private string name;
        private Counterparty counterparty;
        private User creator;
        private Order parentOrder;
        private DateTime dateOfCreation;
        private DateTime? dateOfConclusion;
        private ORDERSTATE state;
        private List<Tranche> tranches;

        //no dateOfConclusion, no parentOrder
        /// <summary>
        /// Initializes a new instance of the <see cref="Order"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="counterparty">The counterparty.</param>
        /// <param name="dateOfCreation">The date of creation.</param>
        /// <param name="state">The state.</param>
        /// <param name="creator">The creator.</param>
        /// <param name="tranches">The tranches.</param>
        public Order(int? id, string name, Counterparty counterparty, DateTime dateOfCreation, ORDERSTATE state, User creator, List<Tranche> tranches)
        {
            this.id = id;
            this.name = name;
            this.creator = creator;
            this.counterparty = counterparty;
            this.dateOfCreation = dateOfCreation;
            this.state = state;
            this.tranches = tranches;
        }

        //no parentOrder
        /// <summary>
        /// Initializes a new instance of the <see cref="Order"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="counterparty">The counterparty.</param>
        /// <param name="dateOfCreation">The date of creation.</param>
        /// <param name="dateOfConclusion">The date of conclusion.</param>
        /// <param name="state">The state.</param>
        /// <param name="creator">The creator.</param>
        /// <param name="tranches">The tranches.</param>
        public Order(int id, string name, Counterparty counterparty, DateTime dateOfCreation, DateTime dateOfConclusion, 
            ORDERSTATE state, User creator, List<Tranche> tranches)
        {
            this.id = id;
            this.name = name;
            this.creator = creator;
            this.counterparty = counterparty;
            this.dateOfCreation = dateOfCreation;
            this.dateOfConclusion = dateOfConclusion;
            this.state = state;
            this.tranches = tranches;
        }

        //no dateOfConclusion
        /// <summary>
        /// Initializes a new instance of the <see cref="Order"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="counterparty">The counterparty.</param>
        /// <param name="dateOfCreation">The date of creation.</param>
        /// <param name="state">The state.</param>
        /// <param name="creator">The creator.</param>
        /// <param name="tranches">The tranches.</param>
        /// <param name="parentOrder">The parent order.</param>
        public Order(int id, string name, Counterparty counterparty, DateTime dateOfCreation, ORDERSTATE state, User creator, List<Tranche> tranches, Order parentOrder)
        {
            this.id = id;
            this.name = name;
            this.creator = creator;
            this.counterparty = counterparty;
            this.dateOfCreation = dateOfCreation;
            this.state = state;
            this.parentOrder = parentOrder;
            this.tranches = tranches;
        }

        //all
        /// <summary>
        /// Initializes a new instance of the <see cref="Order"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="counterparty">The counterparty.</param>
        /// <param name="dateOfCreation">The date of creation.</param>
        /// <param name="dateOfConclusion">The date of conclusion.</param>
        /// <param name="state">The state.</param>
        /// <param name="creator">The creator.</param>
        /// <param name="tranches">The tranches.</param>
        /// <param name="parentOrder">The parent order.</param>
        public Order(int id, string name, Counterparty counterparty, DateTime dateOfCreation, DateTime dateOfConclusion, 
            ORDERSTATE state, User creator, List<Tranche> tranches, Order parentOrder)
        {
            this.id = id;
            this.name = name;
            this.creator = creator;
            this.counterparty = counterparty;
            this.dateOfCreation = dateOfCreation;
            this.dateOfConclusion = dateOfConclusion;
            this.state = state;
            this.parentOrder = parentOrder;
            this.tranches = tranches;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int? Id { get => id; set => id = value; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get => name; set => name = value; }
        /// <summary>
        /// Gets or sets the date of creation.
        /// </summary>
        /// <value>
        /// The date of creation.
        /// </value>
        public DateTime DateOfCreation { get => dateOfCreation; set => dateOfCreation = value; }
        /// <summary>
        /// Gets or sets the date of conclusion.
        /// </summary>
        /// <value>
        /// The date of conclusion.
        /// </value>
        public DateTime? DateOfConclusion { get => dateOfConclusion; set => dateOfConclusion = value; }
        /// <summary>
        /// Gets or sets the counterparty.
        /// </summary>
        /// <value>
        /// The counterparty.
        /// </value>
        internal Counterparty Counterparty { get => counterparty; set => counterparty = value; }
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        internal ORDERSTATE State { get => state; set => state = value; }
        /// <summary>
        /// Gets or sets the creator.
        /// </summary>
        /// <value>
        /// The creator.
        /// </value>
        internal User Creator { get => creator; set => creator = value; }
        /// <summary>
        /// Gets or sets the tranches.
        /// </summary>
        /// <value>
        /// The tranches.
        /// </value>
        /// <exception cref="ArgumentException"></exception>
        internal List<Tranche> Tranches { get => tranches; set { if (value.Count() < 1) throw new ArgumentException(); tranches = value; } }

        /// <summary>
        /// Gets or sets the parent order.
        /// </summary>
        /// <value>
        /// The parent order.
        /// </value>
        internal Order ParentOrder
        {
            get => parentOrder;
            set => parentOrder = value;
        }

        /// <summary>
        /// Gets the price netto.
        /// </summary>
        /// <value>
        /// The price netto.
        /// </value>
        public double PriceNetto
        {
            get { return tranches.Sum(t => t.PriceNetto); }
        }

        /// <summary>
        /// Gets the price brutto.
        /// </summary>
        /// <value>
        /// The price brutto.
        /// </value>
        public double PriceBrutto
        {
            get { return tranches.Sum(t => t.PriceBrutto); }
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
            return obj is Order && ((Order)obj).Name.Equals(name);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() { return "Zamowienie " + id + " " + name + ", stan: " + state + ", user: " + creator.ToString() + ", data utworzenia: " + dateOfCreation + ", kontrahent: " + counterparty.ToString(); }
    }
}
