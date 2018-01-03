using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Entity
{
    class Tranche
    {
        private int? id;
        private int numberOfItems;
        private double quotaDiscount;
        private CounterpartysStock stock;
        private List<PercentageDiscount> discounts;
        private int orderId;

        public Tranche(int? id, CounterpartysStock stock, int numberOfItems)
        {
            this.id = id;
            this.numberOfItems = numberOfItems;
            this.stock = stock;
            this.orderId = 1;
        }

        public Tranche(int? id, CounterpartysStock stock, int numberOfItems, int orderId)
        {
            this.id = id;
            this.numberOfItems = numberOfItems;
            this.stock = stock;
            this.orderId = orderId;
        }

        public Tranche(int? id, CounterpartysStock stock, int numberOfItems, int orderId, List<PercentageDiscount> discounts)
        {
            this.id = id;
            this.numberOfItems = numberOfItems;
            this.discounts = discounts;
            this.stock = stock;
            this.orderId = orderId;
        }

        public Tranche(int? id, CounterpartysStock stock, int numberOfItems, int orderId, double quotaDiscount, List<PercentageDiscount> discounts)
        {
            this.id = id;
            this.numberOfItems = numberOfItems;
            this.orderId = orderId;
            this.quotaDiscount = quotaDiscount;
            this.discounts = discounts;
            this.stock = stock;
        }

        public int? Id { get => id; set => id = value; }
        public int NumberOfItems { get => numberOfItems; set => numberOfItems = value; }
        public int OrderId { get => orderId; set => orderId = value; }
        public double QuotaDiscount { get => quotaDiscount; set => quotaDiscount = value; }
        public double PriceNetto
        {
            get => (stock.PriceNetto * (1 - discounts.Sum(discount => discount.Amount))
                * numberOfItems) - quotaDiscount;
        }
        public double PriceBrutto
        {
            get => (stock.PriceNetto * (1 - discounts.Sum(discount => discount.Amount))
                * numberOfItems) * stock.Stock.VAT * 0.01 - quotaDiscount;
        }
        internal CounterpartysStock Stock { get => stock; set => stock = value; }
        internal List<PercentageDiscount> Discounts { get => discounts; set => discounts = value; }

        public override string ToString()
        {
            return id + " " + numberOfItems + " " + stock.Stock.Name;
        }
    }
}
