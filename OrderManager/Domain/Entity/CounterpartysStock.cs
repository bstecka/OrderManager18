using OrderManager.Domain.Service;
using OrderManager.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Entity
{
    class CounterpartysStock
    {
        private int id;
        private Stock stock;
        private Counterparty counterparty;
        private double priceNetto;
        private List<PercentageDiscount> validDiscounts;

        public CounterpartysStock(int id, Stock stock, Counterparty counterparty, double priceNetto)
        {
            this.id = id;
            this.stock = stock;
            this.counterparty = counterparty;
            this.priceNetto = priceNetto;
        }

        public int Id { get => id; set => id = value; }
        public double PriceNetto { get => priceNetto; set => priceNetto = value; }
        internal Stock Stock { get => stock; set => stock = value; }
        internal Counterparty Counterparty { get => counterparty; set => counterparty = value; }
        internal List<PercentageDiscount> ValidDiscounts {
            get
            {
                if(validDiscounts == null)
                {
                    DAL.ExternalSysDAO.CounterpartysStock counterpartysStockDAO = new DAL.ExternalSysDAO.CounterpartysStock();
                    DAL.InternalSysDAO.PercentageDiscount percentageDiscountDAO = new DAL.InternalSysDAO.PercentageDiscount();
                    validDiscounts = (new CounterpartysStockService(counterpartysStockDAO,
                        new CounterpartysStockMapper(counterpartysStockDAO))).
                        GetValidDiscounts(percentageDiscountDAO,
                            new PercentageDiscountMapper(percentageDiscountDAO, counterpartysStockDAO), 
                            this);
                }
                return validDiscounts;
            }
            set => validDiscounts = value; }

        public override string ToString()
        {
            return id + " " + stock.Name + " dostarczane przez " + counterparty.Name;
        }


        public override bool Equals(object obj)
        {
            return obj is CounterpartysStock && ((CounterpartysStock)obj).Id == id;
        }

        public override int GetHashCode()
        {
            return id;
        }
    }
}
