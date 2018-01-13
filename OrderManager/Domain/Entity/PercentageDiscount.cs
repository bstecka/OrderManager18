using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Entity
{
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

        public DateTime Since { get => since; set => since = value; }
        public DateTime Until { get => until; set => until = value; }
        public double SumNetto { get => sumNetto; set => sumNetto = value; }
        public virtual double Amount { get => amount; set => amount = value; }
        public bool Summing { get => summing; set => summing = value; }
        public bool Active { get => active; set => active = value; }
        public List<CounterpartysStock> CounterpartysStock { get => counterpartysStock; set => counterpartysStock = value; }
        public int Id { get => id; set => id = value; }

        public override string ToString()
        {
            return "Rabat procentowy ID:" + id + " " + sumNetto;
        }

        public override bool Equals(object obj)
        {
            return obj is PercentageDiscount && ((PercentageDiscount)obj).Id == id;
        }

        public override int GetHashCode()
        {
            return id;
        }
    }
}
