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
    public class Stock
    {
        private int id;
        private int maxInStockRoom;
        private int minInStockRoom;
        private int numberOfImtems;
        private int weightOfItem;
        private int maxNumOfItemsOnEuropallet;
        private int vat;
        private string code;
        private string name;
        private Category category;
        private bool inGeneratedOrders;

        /// <summary>
        /// Initializes a new instance of the <see cref="Stock"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="maxInStockRoom">The maximum in stock room.</param>
        /// <param name="minInStockRoom">The minimum in stock room.</param>
        /// <param name="numberOfImtems">The number of imtems.</param>
        /// <param name="weightOfItem">The weight of item.</param>
        /// <param name="maxNumOfItemsOnEuropallet">The maximum number of items on europallet.</param>
        /// <param name="vat">The vat.</param>
        /// <param name="code">The code.</param>
        /// <param name="name">The name.</param>
        /// <param name="category">The category.</param>
        /// <param name="inGeneratedOrders">if set to <c>true</c> [in generated orders].</param>
        public Stock(int id, int maxInStockRoom, int minInStockRoom, int numberOfImtems, int weightOfItem, int maxNumOfItemsOnEuropallet, int vat, string code, string name, Category category, bool inGeneratedOrders)
        {
            this.Id = id;
            this.MaxInStockRoom = maxInStockRoom;
            this.MinInStockRoom = minInStockRoom;
            this.numberOfImtems = numberOfImtems;
            this.WeightOfItem = weightOfItem;
            this.MaxNumberOfItemsOnEuropallet = maxNumOfItemsOnEuropallet;
            this.VAT = vat;
            this.Code = code;
            this.Name = name;
            this.Category = category;
            this.InGeneratedOrders = inGeneratedOrders;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public virtual int Id { get => id; set => id = value; }
        /// <summary>
        /// Gets or sets the maximum in stock room.
        /// </summary>
        /// <value>
        /// The maximum in stock room.
        /// </value>
        /// <exception cref="ArgumentException">Thrown if value is smaller than minInStockRoom.</exception>
        public int MaxInStockRoom { get => maxInStockRoom;
        set { if (value < minInStockRoom) throw new ArgumentException(); maxInStockRoom = value; }
        }
        /// <summary>
        /// Gets or sets the minimum in stock room.
        /// </summary>
        /// <value>
        /// The minimum in stock room.
        /// </value>
        /// <exception cref="ArgumentException">Thrown if value is lesser than 0.</exception>
        public int MinInStockRoom { get => minInStockRoom;
            set { if (value < 0) throw new ArgumentException(); minInStockRoom = value; }
        }
        /// <summary>
        /// Gets the number of items in stock room.
        /// </summary>
        /// <value>
        /// The number of items in stock room.
        /// </value>
        public int NumberOfItemsInStockRoom { get => numberOfImtems;}
        /// <summary>
        /// Gets or sets the weight of item.
        /// </summary>
        /// <value>
        /// The weight of item.
        /// </value>
        /// <exception cref="ArgumentException">Thrown if value is lesser than 0.</exception>
        public int WeightOfItem { get => weightOfItem; set { if (value < 0) throw new ArgumentException(); weightOfItem = value; } }
        /// <summary>
        /// Gets or sets the maximum number of items on europallet.
        /// </summary>
        /// <value>
        /// The maximum number of items on europallet.
        /// </value>
        /// <exception cref="ArgumentException">Thrown if value is lesser than 0.</exception>
        public int MaxNumberOfItemsOnEuropallet { get => maxNumOfItemsOnEuropallet; set { if (value < 0) throw new ArgumentException(); maxNumOfItemsOnEuropallet = value; } }
        /// <summary>
        /// Gets or sets the vat.
        /// </summary>
        /// <value>
        /// The vat.
        /// </value>
        public int VAT { get => vat; set => vat = value; }
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get => code; set => code = value; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get => name; set => name = value; }
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public Category Category { get => category; set => category = value; }
        /// <summary>
        /// Gets or sets a value indicating whether [in generated orders].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [in generated orders]; otherwise, <c>false</c>.
        /// </value>
        public bool InGeneratedOrders { get => inGeneratedOrders; set => inGeneratedOrders = value; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is Stock && ((Stock)obj).Id == id;
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
