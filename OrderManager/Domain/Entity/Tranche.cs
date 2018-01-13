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
        public int NumberOfItems { get => numberOfItems;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Nieprawidłowa liczba sztuk towaru w transzy.");
                numberOfItems = value;
            }
        }
        public int? OrderId { get => orderId; set => orderId = value; }
        public double QuotaDiscount { get => quotaDiscount;
            set
            {
                if (value <= 0 || stocksPriceWithDiscounts(discounts, value) < 0.01)
                    throw new ArgumentException("Naliczenie rabatu spowoduje obniżenie ceny transzy poniżej 0.01 zł.");
                quotaDiscount = value;
            }
        }
        public CounterpartysStock Stock { get => stock;
            set
            {
                if (value == null || value.PriceNetto < 0.01)
                    throw new ArgumentException("Nieprawidłowe dane towaru kontrahenta.");
                stock = value;
            }
        }
        public List<PercentageDiscount> Discounts { get => discounts;
            set
            {
                if (stocksPriceWithDiscounts(value, quotaDiscount) < 0.01)
                    throw new ArgumentException("Naliczenie rabatów spowoduje obniżenie ceny transzy poniżej 0.01 zł.");
                discounts = value;
            } 
        }

        public double PriceNetto
        {
            get
            {
                return stocksPriceWithDiscounts(discounts, quotaDiscount) * numberOfItems;
            }
        }

        public double PriceBrutto
        {
            get
            {
                return stocksPriceWithDiscounts(discounts, quotaDiscount) 
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
    }
}
