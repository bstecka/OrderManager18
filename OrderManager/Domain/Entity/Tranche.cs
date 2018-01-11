using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Entity
{
    public class Tranche
    {
        private int? id;
        private int numberOfItems;
        private double quotaDiscount;
        private CounterpartysStock stock;
        private List<PercentageDiscount> discounts;
        private int? orderId;
        
        public Tranche(int? id, CounterpartysStock stock, int numberOfItems)
        {
            if (stock == null || numberOfItems < 0)
                throw new ArgumentException();
            this.id = id;
            this.numberOfItems = numberOfItems;
            this.stock = stock;
        }

        public Tranche(int? id, CounterpartysStock stock, int numberOfItems, int? orderId) : this(id, stock, numberOfItems)
        {
            this.orderId = orderId;
        }

        public Tranche(int? id, CounterpartysStock stock, int numberOfItems, int? orderId, List<PercentageDiscount> discounts) : this(id, stock, numberOfItems, orderId)
        {
            this.discounts = discounts;
        }

        public Tranche(int? id, CounterpartysStock stock, int numberOfItems, int? orderId, double quotaDiscount, List<PercentageDiscount> discounts) : this(id, stock, numberOfItems, orderId, discounts)
        {
            if (quotaDiscount > PriceNetto)
                throw new ArgumentException();
            this.quotaDiscount = quotaDiscount;
        }


        public Tranche(int? id, int numberOfItems, CounterpartysStock stock, List<PercentageDiscount> discounts)
        {
            this.id = id;
            this.numberOfItems = numberOfItems;
            this.discounts = discounts;
            this.stock = stock;
        }

        public int? Id { get => id; set => id = value; }
        public int NumberOfItems { get => numberOfItems; set => numberOfItems = value; }
        public int? OrderId { get => orderId; set => orderId = value; }
        public double QuotaDiscount { get => quotaDiscount;
            set
            {
                if (quotaDiscount > PriceNetto) throw new ArgumentException();
                quotaDiscount = value;
            }
        }
        public double PriceNetto
        {
            get => (stock.PriceNetto *
                (discounts == null || discounts.Count == 0 ? 
                1 : (1 - discounts.Sum(discount => discount.Amount))) * numberOfItems) 
                - quotaDiscount;
        }
        public double PriceBrutto
        {
            get => (stock.PriceNetto *
                (discounts == null || discounts.Count == 0 ? 
                1 :(1 - discounts.Sum(discount => discount.Amount))) * numberOfItems)
                * (1 + stock.Stock.VAT * 0.01) - quotaDiscount;
        }
        internal CounterpartysStock Stock { get => stock; set => stock = value; }
        internal List<PercentageDiscount> Discounts { get => discounts; set => discounts = value; }

        public override string ToString()
        {
            return id + " " + numberOfItems + " " + stock.Stock.Name;
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            if (!(obj is Tranche)) return false;
            Tranche tr = (Tranche) obj;
            return this.id == tr.Id;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}
