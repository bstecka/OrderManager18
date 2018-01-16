using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Entity
{
    [Serializable]

    public class Tranche
    {
        private int? id;
        private int numberOfItems;
        private double quotaDiscount;
        private CounterpartysStock stock;
        private List<PercentageDiscount> discounts;
        private int? orderId;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tranche"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="stock">The stock.</param>
        /// <param name="numberOfItems">The number of items.</param>
        public Tranche(int? id, CounterpartysStock stock, int numberOfItems)
        {
            Id = id;
            NumberOfItems = numberOfItems;
            Stock = stock;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tranche"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="stock">The stock.</param>
        /// <param name="numberOfItems">The number of items.</param>
        /// <param name="orderId">The order identifier.</param>
        public Tranche(int? id, CounterpartysStock stock, int numberOfItems, int? orderId) : this(id, stock, numberOfItems)
        {
            OrderId = orderId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tranche"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="stock">The stock.</param>
        /// <param name="numberOfItems">The number of items.</param>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="discounts">The discounts.</param>
        public Tranche(int? id, CounterpartysStock stock, int numberOfItems, int? orderId, List<PercentageDiscount> discounts) : this(id, stock, numberOfItems, orderId)
        {
            Discounts = discounts;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tranche"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="stock">The stock.</param>
        /// <param name="numberOfItems">The number of items.</param>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="quotaDiscount">The quota discount.</param>
        /// <param name="discounts">The discounts.</param>
        public Tranche(int? id, CounterpartysStock stock, int numberOfItems, int? orderId, double quotaDiscount, List<PercentageDiscount> discounts) : this(id, stock, numberOfItems, orderId, discounts)
        {
            QuotaDiscount = quotaDiscount;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Tranche"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="numberOfItems">The number of items.</param>
        /// <param name="stock">The stock.</param>
        /// <param name="discounts">The discounts.</param>
        public Tranche(int? id, int numberOfItems, CounterpartysStock stock, List<PercentageDiscount> discounts)
        {
            Id = id;
            NumberOfItems = numberOfItems;
            Discounts = discounts;
            Stock = stock;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int? Id { get => id; set => id = value; }
        /// <summary>
        /// Gets or sets the number of items.
        /// </summary>
        /// <value>
        /// The number of items.
        /// </value>
        /// <exception cref="ArgumentException">Nieprawidłowa liczba sztuk towaru w transzy.</exception>
        public int NumberOfItems
        {
            get => numberOfItems;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Nieprawidłowa liczba sztuk towaru w transzy.");
                numberOfItems = value;
            }
        }
        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>
        /// The order identifier.
        /// </value>
        public int? OrderId { get => orderId; set => orderId = value; }
        /// <summary>
        /// Gets or sets the quota discount.
        /// </summary>
        /// <value>
        /// The quota discount.
        /// </value>
        /// <exception cref="ArgumentException">Naliczenie rabatu spowoduje obniżenie ceny transzy poniżej 0.01 zł.</exception>
        public double QuotaDiscount
        {
            get => quotaDiscount;
            set
            {
                if (value < 0) //|| stocksPriceWithDiscounts(discounts, value) < 0.01)
                    throw new ArgumentException("Naliczenie rabatu spowoduje obniżenie ceny transzy poniżej 0.01 zł.");
                quotaDiscount = value;
            }
        }
        /// <summary>
        /// Gets or sets the stock.
        /// </summary>
        /// <value>
        /// The stock.
        /// </value>
        /// <exception cref="ArgumentException">Nieprawidłowe dane towaru kontrahenta.</exception>
        public CounterpartysStock Stock
        {
            get => stock;
            set
            {
                if (value == null || value.PriceNetto < 0.01)
                    throw new ArgumentException("Nieprawidłowe dane towaru kontrahenta.");
                stock = value;
            }
        }
        public List<PercentageDiscount> Discounts
        {
            get => discounts;
            set
            {
                //if (stocksPriceWithDiscounts(value, quotaDiscount) < 0.01)
                    //throw new ArgumentException("Naliczenie rabatów spowoduje obniżenie ceny transzy poniżej 0.01 zł.");
                discounts = value;
            }
        }

        /// <summary>
        /// Gets the price netto.
        /// </summary>
        /// <value>
        /// The price netto.
        /// </value>
        public double PriceNetto
        {
            get
            {
                var price = stocksPriceWithDiscounts(discounts, quotaDiscount);
                return (price < 0.01 ? 0.01 : price) * numberOfItems;
            }
        }

        /// <summary>
        /// Gets the price brutto.
        /// </summary>
        /// <value>
        /// The price brutto.
        /// </value>
        public double PriceBrutto
        {
            get
            {
                var price = stocksPriceWithDiscounts(discounts, quotaDiscount);
                return (price < 0.01 ? 0.01 : price)
                    * (1 + stock.Stock.VAT * 0.01) * numberOfItems;
            }
        }

        /// <summary>
        /// Gets the stocks price with discounts assigned to the tranche.
        /// </summary>
        /// <param name="discounts">The discounts.</param>
        /// <param name="quotaDiscount">The quota discount.</param>
        /// <returns>Returns the price of the stock with discounts.</returns>
        private double stocksPriceWithDiscounts(List<PercentageDiscount> discounts, double quotaDiscount)
        {
            return stock.PriceNetto * (discounts == null || discounts.Count == 0 ? 1
            : 1 - discounts.Sum(discount => discount.Amount)) - quotaDiscount;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return id + " " + numberOfItems + " " + stock.Stock.Name;
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
            if (obj == this) return true;
            if (!(obj is Tranche)) return false;
            Tranche tr = (Tranche)obj;
            return this.id == tr.Id;
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
