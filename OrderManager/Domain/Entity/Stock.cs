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

        public virtual int Id { get => id; set => id = value; }
        public int MaxInStockRoom { get => maxInStockRoom;
        set { if (value < minInStockRoom) throw new ArgumentException(); maxInStockRoom = value; }
        }
        public int MinInStockRoom { get => minInStockRoom;
            set { if (value < 0) throw new ArgumentException(); minInStockRoom = value; }
        }
        public int NumberOfItemsInStockRoom { get => numberOfImtems;}
        public int WeightOfItem { get => weightOfItem; set { if (value < 0) throw new ArgumentException(); weightOfItem = value; } }
        public int MaxNumberOfItemsOnEuropallet { get => maxNumOfItemsOnEuropallet; set { if (value < 0) throw new ArgumentException(); maxNumOfItemsOnEuropallet = value; } }
        public int VAT { get => vat; set => vat = value; }
        public string Code { get => code; set => code = value; }
        public string Name { get => name; set => name = value; }
        public Category Category { get => category; set => category = value; }
        public bool InGeneratedOrders { get => inGeneratedOrders; set => inGeneratedOrders = value; }

        public override bool Equals(object obj)
        {
            return obj is Stock && ((Stock)obj).Id == id;
        }

        public override int GetHashCode()
        {
            return id;
        }
    }
}
