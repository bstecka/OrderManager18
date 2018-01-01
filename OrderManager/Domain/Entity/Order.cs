using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Entity
{
    enum ORDERSTATE
    {
        duringRealization,
        duringReview,
        cancelled,
        realized
    }

    class Order
    {
        private int id;
        private string name;
        private Counterparty counterparty;
        private User creator;
        private Order parentOrder;
        private double nettoSum;
        private double bruttoSum;
        private DateTime dateOfCreation;
        private DateTime dateOfConclusion;
        private ORDERSTATE state;
        private List<Tranche> tranches;

        public Order(int id, string name, Counterparty counterparty, double nettoSum, double bruttoSum, DateTime dateOfCreation, DateTime dateOfConclusion, string state, User creator)
        {
            this.id = id;
            this.name = name;
            this.creator = creator;
            this.counterparty = counterparty;
            this.nettoSum = nettoSum;
            this.bruttoSum = bruttoSum;
            this.dateOfCreation = dateOfCreation;
            this.state = (ORDERSTATE) Enum.Parse(typeof(ORDERSTATE), state);
        }

        public Order(int id, string name, Counterparty counterparty, double nettoSum, double bruttoSum, DateTime dateOfCreation, DateTime dateOfConclusion, string state, User creator, Order parentOrder)
        {
            this.id = id;
            this.name = name;
            this.creator = creator;
            this.counterparty = counterparty;
            this.nettoSum = nettoSum;
            this.bruttoSum = bruttoSum;
            this.dateOfCreation = dateOfCreation;
            this.state = (ORDERSTATE) Enum.Parse(typeof(ORDERSTATE), state);
            this.ParentOrder = parentOrder;
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public double NettoSum { get => nettoSum; set => nettoSum = value; }
        public double BruttoSum { get => bruttoSum; set => bruttoSum = value; }
        public DateTime DateOfCreation { get => dateOfCreation; set => dateOfCreation = value; }
        public DateTime DateOfConclusion { get => dateOfConclusion; set => dateOfConclusion = value; }
        internal Counterparty Counterparty { get => counterparty; set => counterparty = value; }
        internal Order ParentOrder { get => parentOrder; set => parentOrder = value; }
        internal ORDERSTATE State { get => state; set => state = value; }
        internal User Creator { get => creator; set => creator = value; }
        internal List<Tranche> Tranches { get => tranches; set => tranches = value; }

        public override string ToString() { return id + " " + name; }
    }
}
