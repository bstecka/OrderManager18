using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Entity
{
    enum ORDERSTATE
    {
        duringRealization = 1,
        duringReview = 2,
        cancelled = 3,
        realized = 4
    }

    class Order
    {
        private int id;
        private string name;
        private Counterparty counterparty;
        private User creator;
        private Order parentOrder;
        private DateTime dateOfCreation;
        private DateTime dateOfConclusion;
        private ORDERSTATE state;
        private List<Tranche> tranches;

        //no dateOfConclusion, no parentOrder
        public Order(int id, string name, Counterparty counterparty, DateTime dateOfCreation, string state, User creator, List<Tranche> tranches)
        {
            this.id = id;
            this.name = name;
            this.creator = creator;
            this.counterparty = counterparty;
            this.dateOfCreation = dateOfCreation;
            this.state = (ORDERSTATE)Enum.Parse(typeof(ORDERSTATE), state);
            this.ParentOrder = parentOrder;
            this.tranches = tranches;
        }

        //no parentOrder
        public Order(int id, string name, Counterparty counterparty, DateTime dateOfCreation, DateTime dateOfConclusion, string state, User creator, List<Tranche> tranches)
        {
            this.id = id;
            this.name = name;
            this.creator = creator;
            this.counterparty = counterparty;
            this.dateOfCreation = dateOfCreation;
            this.dateOfConclusion = dateOfConclusion;
            this.state = (ORDERSTATE)Enum.Parse(typeof(ORDERSTATE), state);
            this.tranches = tranches;
        }

        //no dateOfConclusion
        public Order(int id, string name, Counterparty counterparty, DateTime dateOfCreation, string state, User creator, List<Tranche> tranches, Order parentOrder)
        {
            this.id = id;
            this.name = name;
            this.creator = creator;
            this.counterparty = counterparty;
            this.dateOfCreation = dateOfCreation;
            this.state = (ORDERSTATE)Enum.Parse(typeof(ORDERSTATE), state);
            this.ParentOrder = parentOrder;
            this.tranches = tranches;
        }

        //all
        public Order(int id, string name, Counterparty counterparty, DateTime dateOfCreation, DateTime dateOfConclusion, string state, User creator, List<Tranche> tranches, Order parentOrder)
        {
            this.id = id;
            this.name = name;
            this.creator = creator;
            this.counterparty = counterparty;
            this.dateOfCreation = dateOfCreation;
            this.dateOfConclusion = dateOfConclusion;
            this.state = (ORDERSTATE)Enum.Parse(typeof(ORDERSTATE), state);
            this.ParentOrder = parentOrder;
            this.tranches = tranches;
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public DateTime DateOfCreation { get => dateOfCreation; set => dateOfCreation = value; }
        public DateTime DateOfConclusion { get => dateOfConclusion; set => dateOfConclusion = value; }
        internal Counterparty Counterparty { get => counterparty; set => counterparty = value; }
        internal ORDERSTATE State { get => state; set => state = value; }
        internal User Creator { get => creator; set => creator = value; }
        internal List<Tranche> Tranches { get => tranches; set => tranches = value; }

        internal Order ParentOrder
        {
            get
            {
                if (parentOrder == null)
                {
                    DAL.InternalSysDAO.Order orderDAO = new DAL.InternalSysDAO.Order();
                    DTO.OrderMapper orderMapper = new DTO.OrderMapper(orderDAO);
                    DataTable parentTable = orderDAO.GetParentOrderById(id);
                    if (parentTable.Rows.Count < 1)
                        parentOrder = null;
                    else
                        parentOrder = orderMapper.MapFrom(parentTable);
                }
                return parentOrder;
            }
            set => parentOrder = value;
        }

        public double PriceNetto
        {
            get 
            {
                double price = 0.0;
                foreach (Tranche tranche in Tranches)
                    price += tranche.PriceNetto;
                return price;
            }
        }

        public double PriceBrutto
        {
            get
            {
                double price = 0.0;
                foreach (Tranche tranche in Tranches)
                    price += tranche.PriceBrutto;
                return price;
            }
        }

        public override string ToString() { return "Zamowienie " + id + " " + name + ", stan: " + state + ", user: " + creator.ToString() + ", data utworzenia: " + dateOfCreation + ", kontrahent: " + counterparty.ToString(); }
    }
}
