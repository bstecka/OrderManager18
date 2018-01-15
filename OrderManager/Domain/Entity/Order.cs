using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Entity
{
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

        public int? Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public DateTime DateOfCreation { get => dateOfCreation; set => dateOfCreation = value; }
        public DateTime? DateOfConclusion { get => dateOfConclusion; set => dateOfConclusion = value; }
        internal Counterparty Counterparty { get => counterparty; set => counterparty = value; }
        internal ORDERSTATE State { get => state; set => state = value; }
        internal User Creator { get => creator; set => creator = value; }
        internal List<Tranche> Tranches { get => tranches; set { if (value.Count() < 1) throw new ArgumentException(); tranches = value; } }

        internal Order ParentOrder
        {
            get => parentOrder;
            set => parentOrder = value;
        }

        public double PriceNetto
        {
            get { return tranches.Sum(t => t.PriceNetto); }
        }

        public double PriceBrutto
        {
            get { return tranches.Sum(t => t.PriceBrutto); }
        }

        public override bool Equals(object obj)
        {
            return obj is Order && ((Order)obj).Name.Equals(name);
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }
        
        public override string ToString() { return "Zamowienie " + id + " " + name + ", stan: " + state + ", user: " + creator.ToString() + ", data utworzenia: " + dateOfCreation + ", kontrahent: " + counterparty.ToString(); }
    }
}
