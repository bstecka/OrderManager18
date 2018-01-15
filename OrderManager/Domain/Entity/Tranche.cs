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

        public Tranche(int? id, CounterpartysStock stock, int numberOfItems)
        {
            Id = id;
            NumberOfItems = numberOfItems;
            Stock = stock;
        }

        public Tranche(int? id, CounterpartysStock stock, int numberOfItems, int? orderId) : this(id, stock, numberOfItems)
        {
            OrderId = orderId;
        }

        public Tranche(int? id, CounterpartysStock stock, int numberOfItems, int? orderId, List<PercentageDiscount> discounts) : this(id, stock, numberOfItems, orderId)
        {
            Discounts = discounts;
        }

        public Tranche(int? id, CounterpartysStock stock, int numberOfItems, int? orderId, double quotaDiscount, List<PercentageDiscount> discounts) : this(id, stock, numberOfItems, orderId, discounts)
        {
            QuotaDiscount = quotaDiscount;
        }


        public Tranche(int? id, int numberOfItems, CounterpartysStock stock, List<PercentageDiscount> discounts)
        {
            Id = id;
            NumberOfItems = numberOfItems;
            Discounts = discounts;
            Stock = stock;
        }

        public int? Id { get => id; set => id = value; }
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
        public int? OrderId { get => orderId; set => orderId = value; }
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

        public double PriceNetto
        {
            get
            {
                var price = stocksPriceWithDiscounts(discounts, quotaDiscount);
                return (price < 0.01 ? 0.01 : price) * numberOfItems;
            }
        }

        public double PriceBrutto
        {
            get
            {
                var price = stocksPriceWithDiscounts(discounts, quotaDiscount);
                return (price < 0.01 ? 0.01 : price)
                    * (1 + stock.Stock.VAT * 0.01) * numberOfItems;
            }
        }

        private double stocksPriceWithDiscounts(List<PercentageDiscount> discounts, double quotaDiscount)
        {
            return stock.PriceNetto * (discounts == null || discounts.Count == 0 ? 1
            : 1 - discounts.Sum(discount => discount.Amount)) - quotaDiscount;
        }

        public override string ToString()
        {
            return id + " " + numberOfItems + " " + stock.Stock.Name;
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            if (!(obj is Tranche)) return false;
            Tranche tr = (Tranche)obj;
            return this.id == tr.Id;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
    /*
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
        public double QuotaDiscount
        {
            get => quotaDiscount;
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
                1 : (1 - discounts.Sum(discount => discount.Amount))) * numberOfItems)
                * (1 + stock.Stock.VAT * 0.01) - quotaDiscount;
        }
        public CounterpartysStock Stock { get => stock; set => stock = value; }
        public List<PercentageDiscount> Discounts { get => discounts; set => discounts = value; }

        public override string ToString()
        {
            return id + " " + numberOfItems + " " + stock.Stock.Name;
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            if (!(obj is Tranche)) return false;
            Tranche tr = (Tranche)obj;
            return this.id == tr.Id;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }*/
}
